using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.IoT;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.IO;

namespace SalesTrackerAPI.BL {
    public class TblLoadingBL {
        #region Selection

        private readonly ILogger loggerObj;

        public TblLoadingBL (ILogger<TblLoadingBL> logger) {
            loggerObj = logger;
            Constants.LoggerObj = logger;
        }

        public static List<TblLoadingTO> SelectAllTblLoadingList () {
            return TblLoadingDAO.SelectAllTblLoading ();
        }

        public static List<TblLoadingTO> SelectAllLoadingsFromParentLoadingId (int parentLoadingId) {
            return TblLoadingDAO.SelectAllLoadingsFromParentLoadingId (parentLoadingId);
        }

        public static List<TblLoadingTO> SelectAllTblLoadingList (List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 deliveryInformation, Int32 dealerId, string selectedOrgIdStr, Int32 isConfirm, Int32 brandId, Int32 loadingNavigateId) {
            //Kiran Added For gate IoT data [06-12-2018]
            var checkIotFlag = loadingStatusId;
            int configId = Constants.getweightSourceConfigTO ();
            int modeId = Constants.getModeIdConfigTO();
            if (configId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT)) {
                checkIotFlag = 0;
            }
            List<TblLoadingTO> list = new List<TblLoadingTO> ();
            List<TblLoadingTO> finalList = new List<TblLoadingTO> ();

            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO ();

            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0) {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority (tblUserRoleTOList);

            }
            if (string.IsNullOrEmpty(selectedOrgIdStr))
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    selectedOrgIdStr = Convert.ToString(tblConfigParamsTO.ConfigParamVal)+",0";
                }
            }
            List<TblLoadingTO> tblLoadingTOList = TblLoadingDAO.SelectAllTblLoading(tblUserRoleTO, cnfId, checkIotFlag, fromDate, toDate, loadingTypeId, dealerId,selectedOrgIdStr, isConfirm, brandId, loadingNavigateId);

            if (configId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) && deliveryInformation == 0) {
                if (tblLoadingTOList != null && tblLoadingTOList.Count > 0) {
                    var deliverList = tblLoadingTOList.Where (s => s.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED || s.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || s.TranStatusE == Constants.TranStatusE.LOADING_NOT_CONFIRM).ToList ();
                    // var deliverList = tblLoadingTOList.Where(s => s.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED || s.TranStatusE == Constants.TranStatusE.LOADING_CANCEL).ToList();
                    string finalStatusId = IoT.IotCommunication.GetIotEncodedStatusIdsForGivenStatus (loadingStatusId.ToString ());
                    list = SetLoadingStatusData (finalStatusId.ToString (), true, configId, tblLoadingTOList);
                    if (deliverList != null)
                        finalList.AddRange (deliverList);
                    if (list != null)
                        finalList.AddRange (list);
                }

                if (finalList != null && finalList.Count > 0) {
                    if (loadingStatusId > 0) {
                        finalList = finalList.Where (w => w.StatusId == loadingStatusId).ToList ();
                    }
                }
                return finalList;
            } else {
                return tblLoadingTOList;
            }
        }

        public static List<TblLoadingSlipTO> SetLoadingStatusData(String loadingStatusId, bool isEncoded, int configId, List<TblLoadingSlipTO> tblLoadingTOList)
        {
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                List<DimStatusTO> statusList = DimStatusDAO.SelectAllDimStatus((Int32)Constants.TransactionTypeE.LOADING);
                //GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId(loadingStatusId.ToString());

                List<TblLoadingSlipTO> distGate = tblLoadingTOList.GroupBy(g => g.GateId).Select(s => s.FirstOrDefault()).ToList();

                GateIoTResult gateIoTResult = new GateIoTResult();

                for (int g = 0; g < distGate.Count; g++)
                {
                    TblLoadingSlipTO tblLoadingTOTemp = distGate[g];
                    TblGateTO tblGateTO = new TblGateTO(tblLoadingTOTemp.GateId, tblLoadingTOTemp.IotUrl, tblLoadingTOTemp.MachineIP, tblLoadingTOTemp.PortNumber);
                    GateIoTResult temp = IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId(loadingStatusId.ToString(), tblGateTO);

                    if (temp != null && temp.Data != null)
                    {
                        gateIoTResult.Data.AddRange(temp.Data);
                    }
                }

                if (gateIoTResult != null && gateIoTResult.Data != null)
                {
                    for (int d = 0; d < tblLoadingTOList.Count; d++)
                    {
                        var data = gateIoTResult.Data.Where(w => Convert.ToInt32(w[0]) == tblLoadingTOList[d].ModbusRefId).FirstOrDefault();
                        if (data != null)
                        {
                            tblLoadingTOList[d].VehicleNo = Convert.ToString(data[(int)IoTConstants.GateIoTColE.VehicleNo]);
                            //tblLoadingTOList[d].VehicleNo = IotCommunication.GetVehicleNumbers(Convert.ToString(data[(int)IoTConstants.GateIoTColE.VehicleNo]), true);
                            if (data.Length > 3)
                                tblLoadingTOList[d].TransporterOrgId = Convert.ToInt32(data[(int)IoTConstants.GateIoTColE.TransportorId]);
                            DimStatusTO dimStatusTO = statusList.Where(w => w.IotStatusId == Convert.ToInt32(data[(int)IoTConstants.GateIoTColE.StatusId])).FirstOrDefault();
                            if (dimStatusTO != null)
                            {
                                tblLoadingTOList[d].StatusId = dimStatusTO.IdStatus;
                                tblLoadingTOList[d].StatusDesc = dimStatusTO.StatusName;
                                tblLoadingTOList[d].StatusName = dimStatusTO.StatusName;
                            }

                        }
                        else
                        {
                            tblLoadingTOList.RemoveAt(d);
                            d--;
                        }
                    }
                    if (!String.IsNullOrEmpty(loadingStatusId))
                    {
                        string statusIdList = string.Empty;
                        if (isEncoded)
                            statusIdList = IotCommunication.GetIotDecodedStatusIdsForGivenStatus(loadingStatusId);

                        var statusIds = statusIdList.Split(',').ToList();

                        if (statusIds.Count == 1 && statusIds[0] == "0")
                            return tblLoadingTOList;

                        tblLoadingTOList = tblLoadingTOList.Where(w => statusIds.Contains(Convert.ToString(w.StatusId))).ToList();

                        //tblLoadingTOList = tblLoadingTOList.Where(w => w.StatusId == loadingStatusId).ToList();
                    }
                }
                else
                {
                    tblLoadingTOList = new List<TblLoadingSlipTO>();
                }
            }

            return tblLoadingTOList;
        }


        public static List<TblLoadingTO> SetLoadingStatusData (String loadingStatusId, bool isEncoded, int configId, List<TblLoadingTO> tblLoadingTOList) {
            if (configId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT)) {
                List<DimStatusTO> statusList = DimStatusBL.SelectAllDimStatusList ();
                //GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId(loadingStatusId.ToString());

                List<TblLoadingTO> distGate = tblLoadingTOList.GroupBy (g => g.GateId).Select (s => s.FirstOrDefault ()).ToList ();

                GateIoTResult gateIoTResult = new GateIoTResult ();

                for (int g = 0; g < distGate.Count; g++) {
                    TblLoadingTO tblLoadingTOTemp = distGate[g];
                    TblGateTO tblGateTO = new TblGateTO (tblLoadingTOTemp);
                    GateIoTResult temp = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId (loadingStatusId.ToString (), tblGateTO);

                    if (temp != null && temp.Data != null) {
                        gateIoTResult.Data.AddRange (temp.Data);
                    }
                }

                if (gateIoTResult != null && gateIoTResult.Data != null) {
                    for (int d = 0; d < tblLoadingTOList.Count; d++) {
                        var data = gateIoTResult.Data.Where (w => Convert.ToInt32 (w[0]) == tblLoadingTOList[d].ModbusRefId).FirstOrDefault ();
                        if (data != null) {
                            tblLoadingTOList[d].VehicleNo = Convert.ToString (data[(int) IoTConstants.GateIoTColE.VehicleNo]);
                            if (data.Length > 3)
                                tblLoadingTOList[d].TransporterOrgId = Convert.ToInt32 (data[(int) IoTConstants.GateIoTColE.TransportorId]);
                            DimStatusTO dimStatusTO = statusList.Where (w => w.IotStatusId == Convert.ToInt32 (data[(int) IoTConstants.GateIoTColE.StatusId])).FirstOrDefault ();
                            if (dimStatusTO != null) {
                                tblLoadingTOList[d].StatusId = dimStatusTO.IdStatus;
                                tblLoadingTOList[d].StatusDesc = dimStatusTO.StatusName;
                            }

                        } else {
                            tblLoadingTOList.RemoveAt (d);
                            d--;
                        }
                    }
                    if (!String.IsNullOrEmpty (loadingStatusId)) {
                        string statusIdList = string.Empty;
                        if (isEncoded)
                            statusIdList = IotCommunication.GetIotDecodedStatusIdsForGivenStatus (loadingStatusId);

                        var statusIds = statusIdList.Split (',').ToList ();

                        if (statusIds.Count == 1 && statusIds[0] == "0")
                            return tblLoadingTOList;

                        tblLoadingTOList = tblLoadingTOList.Where (w => statusIds.Contains (Convert.ToString (w.StatusId))).ToList ();

                        //tblLoadingTOList = tblLoadingTOList.Where(w => w.StatusId == loadingStatusId).ToList();
                    }
                } else {
                    tblLoadingTOList = new List<TblLoadingTO> ();
                }
            }

            return tblLoadingTOList;
        }

        //Sanjay Gunjal [03-June-2019] Commented as Async and await will not work as Iot Works in queque or sequentially.
        //public static async Task<List<TblLoadingTO>> SetLoadingStatusDataV2(String loadingStatusId, bool isEncoded, int configId, List<TblLoadingTO> tblLoadingTOList)
        //{
        //    if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
        //    {
        //        List<long> totaltime = new List<long>();
        //        List<DimStatusTO> statusList = DimStatusBL.SelectAllDimStatusList();
        //        GateIoTResult gateIoTResult = await IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusIdV2(loadingStatusId.ToString(), totaltime);
        //        if (gateIoTResult != null && gateIoTResult.Data != null)
        //        {
        //            for (int d = 0; d < tblLoadingTOList.Count; d++)
        //            {
        //                var data = gateIoTResult.Data.Where(w => Convert.ToInt32(w[0]) == tblLoadingTOList[d].ModbusRefId).FirstOrDefault();
        //                if (data != null)
        //                {
        //                    tblLoadingTOList[d].VehicleNo = Convert.ToString(data[(int)IoTConstants.GateIoTColE.VehicleNo]);
        //                    DimStatusTO dimStatusTO = statusList.Where(w => w.IotStatusId == Convert.ToInt32(data[(int)IoTConstants.GateIoTColE.StatusId])).FirstOrDefault();
        //                    if (dimStatusTO != null)
        //                    {
        //                        tblLoadingTOList[d].StatusId = dimStatusTO.IdStatus;
        //                        tblLoadingTOList[d].StatusDesc = dimStatusTO.StatusName;
        //                    }

        //                }
        //                else
        //                {
        //                    tblLoadingTOList.RemoveAt(d);
        //                    d--;
        //                }
        //            }
        //            if (!String.IsNullOrEmpty(loadingStatusId))
        //            {
        //                string statusIdList = string.Empty;
        //                if (isEncoded)
        //                    statusIdList = IotCommunication.GetIotDecodedStatusIdsForGivenStatus(loadingStatusId);

        //                var statusIds = statusIdList.Split(',').ToList();
        //                tblLoadingTOList = tblLoadingTOList.Where(w => statusIds.Contains(Convert.ToString(w.StatusId))).ToList();

        //                //tblLoadingTOList = tblLoadingTOList.Where(w => w.StatusId == loadingStatusId).ToList();
        //            }
        //        }
        //        else
        //        {
        //            tblLoadingTOList = new List<TblLoadingTO>();
        //        }
        //    }

        //    return tblLoadingTOList;
        //}

        public static Dictionary<Int32, Double> SelectLoadingWisePctCompletionDCT (Int32 isByWeight) {
            Dictionary<Int32, Double> loadingPctDCT = new Dictionary<int, double> ();
            Dictionary<Int32, String> loadingExtRecDCT = BL.TblLoadingSlipExtBL.SelectLoadingWiseExtRecordCountDCT ();
            Dictionary<Int32, Int32> weightRecDCT = BL.TblWeighingMeasuresBL.SelectLoadingWiseExtRecordCountDCT ();
            if (loadingExtRecDCT != null) {
                TblConfigParamsTO loadingConfigTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_DEFAULT_WEIGHING_SCALE);
                Int32 weighScale = 1;
                if (loadingConfigTO != null)
                    weighScale = Convert.ToInt32 (loadingConfigTO.ConfigParamVal);
                Int32 tareAndGrossWeighingFactor = 2;
                Int32 expTotalWeighing = weighScale * tareAndGrossWeighingFactor;

                foreach (var item in loadingExtRecDCT.Keys) {
                    int loadingId = item;
                    String weights = loadingExtRecDCT[item];
                    Int32 recordCount = Convert.ToInt32 (weights.Split ('|') [0]);
                    Int32 loadedCount = Convert.ToInt32 (weights.Split ('|') [1]);

                    Int32 totalExpRecs = expTotalWeighing + recordCount;
                    Int32 actRecs = loadedCount;
                    if (weightRecDCT.ContainsKey (item))
                        actRecs += weightRecDCT[item];

                    Double completionPct = (actRecs * 100) / totalExpRecs;

                    loadingPctDCT.Add (loadingId, completionPct);
                }

            }
            return loadingPctDCT;
        }

        public static List<TblLoadingTO> SelectAllLoadingListByStatus (string statusId, int gateId = 0) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return TblLoadingDAO.SelectAllLoadingListByStatus (statusId, conn, tran, gateId);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public static tblUserMachineMappingTo SelectUserMachineTo (int userId) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return TblLoadingDAO.SelectUserMachineTo (userId, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public static TblLoadingTO SelectTblLoadingTO (Int32 idLoading, SqlConnection conn, SqlTransaction tran) {
            return TblLoadingDAO.SelectTblLoading (idLoading, conn, tran);
        }

        public static string GetLoadinglayerDesc(Int32 loadingLayerId)
        {
            if (loadingLayerId == (Int32)Constants.LoadingLayerE.BOTTOM)
                return "Bottom";
            else if (loadingLayerId == (Int32)Constants.LoadingLayerE.MIDDLE1)
                return "Middle1";
            else if (loadingLayerId == (Int32)Constants.LoadingLayerE.MIDDLE2)
                return "Middle2";
            else if (loadingLayerId == (Int32)Constants.LoadingLayerE.MIDDLE3)
                return "Middle3";
            else return "Top";
        }

        public static TblLoadingTO SelectLoadingTOWithDetailsByBooking(String tempBookingsIdsList, String tempScheduleIdsList, TblLoadingTO scheduleTblLoadingTO = null)
        {
            try
            {


                TblLoadingTO tblLoadingTO = new TblLoadingTO();
                if (scheduleTblLoadingTO != null)
                {
                    tblLoadingTO = scheduleTblLoadingTO;
                }
                List<Int32> bookingsIdsList = new List<Int32>();
                List<Int32> scheduleIdsList = new List<Int32>();
                //List<TblBookingExtTO> tblAllBookingExtTOList = new List<TblBookingExtTO>();

                List<ScheduleLoadingLayerIdDCT> scheduleLoadingLayerIdDCT = new List<Models.ScheduleLoadingLayerIdDCT>();
                if (scheduleTblLoadingTO != null && scheduleTblLoadingTO.ScheduleLoadingLayerIdDCT != null)
                {
                    scheduleLoadingLayerIdDCT = scheduleTblLoadingTO.ScheduleLoadingLayerIdDCT;

                    for (int o = 0; o < scheduleLoadingLayerIdDCT.Count; o++)
                    {
                        ScheduleLoadingLayerIdDCT tempObj = scheduleLoadingLayerIdDCT[o];

                        if (!bookingsIdsList.Contains(tempObj.bookingId))
                        {
                            bookingsIdsList.Add(tempObj.bookingId);
                        }

                        if (!scheduleIdsList.Contains(tempObj.scheduleId))
                        {
                            scheduleIdsList.Add(tempObj.scheduleId);
                        }

                    }



                }
                else
                {
                    //get bookingIds List
                    if (!String.IsNullOrEmpty(tempBookingsIdsList))
                    {
                        bookingsIdsList = tempBookingsIdsList.Split(',').Select(int.Parse).ToList();
                    }
                    //get ScheduleIds List
                    if (!String.IsNullOrEmpty(tempScheduleIdsList))
                    {
                        scheduleIdsList = tempScheduleIdsList.Split(',').Select(int.Parse).ToList();
                    }
                }
                //= new TblBookingsTO();
                tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO>();

                if (bookingsIdsList != null && bookingsIdsList.Count > 0)
                {

                    for (int s = 0; s < bookingsIdsList.Count; s++)
                    {
                        List<TblBookingExtTO> tblAllBookingExtTOList = new List<TblBookingExtTO>();

                        Int32 bookingId = bookingsIdsList[s];
                        TblBookingsTO tblBookingTO = TblBookingsBL.SelectBookingsTOWithDetails(bookingId);
                        if (tblBookingTO != null)
                        {

                            //tblBookingTO.PaymentTermOptionRelationTOLst = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByBookingId(bookingId);

                            //tblLoadingTO.VehicleNo = tblBookingTO.VehicleNo;
                            //tblLoadingTO.FreightAmt = tblBookingTO.FreightAmt;
                            tblLoadingTO.CnfOrgId = tblBookingTO.CnFOrgId;
                            tblLoadingTO.CnfOrgName = tblBookingTO.CnfName;
                            tblLoadingTO.NoOfDeliveries = tblBookingTO.NoOfDeliveries;


                            if (tblBookingTO.BookingScheduleTOLst != null && tblBookingTO.BookingScheduleTOLst.Count > 0)
                            {
                                List<TblBookingScheduleTO> temptblBookingScheduleTOList = tblBookingTO.BookingScheduleTOLst;
                                List<TblBookingScheduleTO> tblBookingScheduleTOList = new List<TblBookingScheduleTO>();

                                //get schedule list
                                if (scheduleIdsList != null && scheduleIdsList.Count > 0)
                                {
                                    for (int p = 0; p < scheduleIdsList.Count; p++)
                                    {
                                        Int32 scheduleId = scheduleIdsList[p];
                                        TblBookingScheduleTO tempTblBookingScheduleTO = new TblBookingScheduleTO();
                                        tempTblBookingScheduleTO = temptblBookingScheduleTOList.Where(c => c.IdSchedule == scheduleId).FirstOrDefault();
                                        if (tempTblBookingScheduleTO != null)
                                        {

                                            if (scheduleLoadingLayerIdDCT != null && scheduleLoadingLayerIdDCT.Count > 0)
                                            {
                                                ScheduleLoadingLayerIdDCT temp = scheduleLoadingLayerIdDCT.Where(w => w.scheduleId == tempTblBookingScheduleTO.IdSchedule).FirstOrDefault();
                                                if (temp != null)
                                                {
                                                    tempTblBookingScheduleTO.LoadingLayerId = temp.scheduleLoadingLayerId;
                                                    tempTblBookingScheduleTO.LoadingLayerDesc = GetLoadinglayerDesc(tempTblBookingScheduleTO.LoadingLayerId);
                                                    tempTblBookingScheduleTO.OrderDetailsLst.ForEach(f => f.LoadingLayerId = temp.scheduleLoadingLayerId);
                                                }
                                            }

                                            tblBookingScheduleTOList.Add(tempTblBookingScheduleTO);
                                        }



                                    }
                                }
                                //get all extensionlist
                                if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
                                {
                                    for (int i = 0; i < tblBookingScheduleTOList.Count; i++)
                                    {
                                        TblBookingScheduleTO tblBookingScheduleTO = tblBookingScheduleTOList[i];
                                        if (tblBookingScheduleTO.OrderDetailsLst != null)
                                            tblAllBookingExtTOList.AddRange(tblBookingScheduleTO.OrderDetailsLst);
                                    }
                                }
                                //get distinct schedule list
                                List<TblBookingScheduleTO> distinctBookingScheduleList = tblBookingScheduleTOList.GroupBy(w => w.LoadingLayerId).Select(x => x.FirstOrDefault()).ToList();
                                if (distinctBookingScheduleList != null && distinctBookingScheduleList.Count > 0)
                                {
                                    for (int m = 0; m < distinctBookingScheduleList.Count; m++)
                                    {
                                        TblBookingScheduleTO tempBookingScheduleTO = distinctBookingScheduleList[m];
                                        tempBookingScheduleTO.OrderDetailsLst = new List<TblBookingExtTO>();
                                        List<TblBookingExtTO> distinctBookingExtList = tblAllBookingExtTOList.GroupBy(w => w.LoadingLayerId).Select(x => x.FirstOrDefault()).ToList();
                                        if (distinctBookingExtList != null && distinctBookingExtList.Count > 0)
                                        {
                                            for (int n = 0; n < distinctBookingExtList.Count; n++)
                                            {
                                                List<TblBookingExtTO> tempOrderList = tblAllBookingExtTOList.Where(oi => oi.LoadingLayerId == distinctBookingExtList[n].LoadingLayerId).ToList();
                                                for (int k = 0; k < tempOrderList.Count; k++)
                                                {
                                                    TblBookingExtTO tempBookingExtTO = tempOrderList[k];
                                                    if (tempBookingScheduleTO.LoadingLayerId == tempBookingExtTO.LoadingLayerId)
                                                    {
                                                        tempBookingScheduleTO.OrderDetailsLst.Add(tempBookingExtTO);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    Double bookQty = tblBookingTO.PendingQty;


                                    for (int p = 0; p < distinctBookingScheduleList.Count; p++)
                                    {
                                        TblBookingScheduleTO distBookingScheduleTO = distinctBookingScheduleList[p];
                                        var listToCheck = distBookingScheduleTO.OrderDetailsLst.GroupBy(a => new { a.ProdSpecId, a.ProdCatId, a.MaterialId, a.ProdItemId, a.ProdCatDesc, a.ProdSpecDesc, a.MaterialSubType, a.DisplayName }).
                                            Select(a => new {
                                                ProdCatId = a.Key.ProdCatId,
                                                ProdItemId = a.Key.ProdItemId,
                                                ProdSpecId = a.Key.ProdSpecId,
                                                //BrandId = a.Key.BrandId,
                                                MaterialId = a.Key.MaterialId,
                                                ProdCatDesc = a.Key.ProdCatDesc,
                                                ProdSpecDesc = a.Key.ProdSpecDesc,
                                                //BrandDesc = a.Key.BrandDesc,
                                                MaterialSubType = a.Key.MaterialSubType,
                                                DisplayName = a.Key.DisplayName,
                                                BalanceQty = a.Sum(acs => acs.BalanceQty)
                                            }).ToList();

                                        distBookingScheduleTO.OrderDetailsLst = new List<TblBookingExtTO>();
                                        for (int l = 0; l < listToCheck.Count; l++)
                                        {
                                            var listTo = listToCheck[l];
                                            TblBookingExtTO tblBookingExtTO = new TblBookingExtTO();
                                            tblBookingExtTO.MaterialId = listTo.MaterialId;
                                            tblBookingExtTO.ProdCatId = listTo.ProdCatId;
                                            tblBookingExtTO.ProdSpecId = listTo.ProdSpecId;
                                            tblBookingExtTO.ProdItemId = listTo.ProdItemId;
                                            //tblBookingExtTO.BrandId = listTo.BrandId;
                                            tblBookingExtTO.BookedQty = listTo.BalanceQty;
                                            tblBookingExtTO.BalanceQty = listTo.BalanceQty;
                                            tblBookingExtTO.MaterialSubType = listTo.MaterialSubType;
                                            tblBookingExtTO.ProdCatDesc = listTo.ProdCatDesc;
                                            tblBookingExtTO.ProdSpecDesc = listTo.ProdSpecDesc;
                                            //tblBookingExtTO.BrandDesc = listTo.BrandDesc;
                                            tblBookingExtTO.DisplayName = listTo.DisplayName;
                                            distBookingScheduleTO.OrderDetailsLst.Add(tblBookingExtTO);
                                        }

                                        TblLoadingSlipTO tblLoadingSlipTO = selectLoadingSlipTO(tblBookingTO);

                                        tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = tblBookingTO.IdBooking;
                                        tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingRate = tblBookingTO.BookingRate;
                                        tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;
                                        if (distBookingScheduleTO.DeliveryAddressLst != null && distBookingScheduleTO.DeliveryAddressLst.Count > 0)
                                        {
                                            List<TblBookingDelAddrTO> tblBookingDelAddrTOList = distBookingScheduleTO.DeliveryAddressLst;
                                            //.Where(ele => ele.ScheduleId == distBookingScheduleTO.IdSchedule).ToList();
                                            TblBookingDelAddrTO tblBookingDelAddrTO = new TblBookingDelAddrTO();
                                            for (int j = 0; j < tblBookingDelAddrTOList.Count; j++)
                                            {
                                                TblLoadingSlipAddressTO tblLoadingSlipAddressTO = new TblLoadingSlipAddressTO();
                                                tblBookingDelAddrTO = tblBookingDelAddrTOList[j];
                                                tblLoadingSlipAddressTO.BillingName = tblBookingDelAddrTO.BillingName;
                                                tblLoadingSlipAddressTO.GstNo = tblBookingDelAddrTO.GstNo;
                                                tblLoadingSlipAddressTO.PanNo = tblBookingDelAddrTO.PanNo;
                                                tblLoadingSlipAddressTO.AadharNo = tblBookingDelAddrTO.AadharNo;
                                                tblLoadingSlipAddressTO.ContactNo = tblBookingDelAddrTO.ContactNo;
                                                tblLoadingSlipAddressTO.Address = tblBookingDelAddrTO.Address;
                                                tblLoadingSlipAddressTO.VillageName = tblBookingDelAddrTO.VillageName;
                                                tblLoadingSlipAddressTO.TalukaName = tblBookingDelAddrTO.TalukaName;
                                                tblLoadingSlipAddressTO.DistrictName = tblBookingDelAddrTO.DistrictName;
                                                tblLoadingSlipAddressTO.StateId = tblBookingDelAddrTO.StateId;
                                                tblLoadingSlipAddressTO.State = tblBookingDelAddrTO.State;
                                                tblLoadingSlipAddressTO.Country = tblBookingDelAddrTO.Country;
                                                tblLoadingSlipAddressTO.Pincode = tblBookingDelAddrTO.Pincode.ToString();
                                                tblLoadingSlipAddressTO.TxnAddrTypeId = tblBookingDelAddrTO.TxnAddrTypeId;

                                                //Saket [2019-09-27] From pending booking auto loading slip addres src should be booking.
                                                //tblLoadingSlipAddressTO.AddrSourceTypeId = tblBookingDelAddrTO.AddrSourceTypeId;
                                                tblLoadingSlipAddressTO.AddrSourceTypeId = (int)Constants.AddressSourceTypeE.FROM_BOOKINGS;

                                                tblLoadingSlipAddressTO.LoadingLayerId = tblBookingDelAddrTO.LoadingLayerId;
                                                tblLoadingSlipAddressTO.BillingOrgId = tblBookingDelAddrTO.BillingOrgId;

                                                //tblLoadingSlipAddressTO.Country = tblBookingDelAddrTO.Country;
                                                tblLoadingSlipTO.DeliveryAddressTOList.Add(tblLoadingSlipAddressTO);
                                            }

                                        }

                                        if (distBookingScheduleTO.OrderDetailsLst != null && distBookingScheduleTO.OrderDetailsLst.Count > 0)
                                        {
                                            List<TblBookingExtTO> tblBookingExtTOList = distBookingScheduleTO.OrderDetailsLst;
                                            //.Where(ele => ele.ScheduleId == distBookingScheduleTO.IdSchedule).ToList();
                                            TblBookingExtTO tblBookingExtTO = new TblBookingExtTO();
                                            Double totLayerQty = 0;
                                            for (int k = 0; k < tblBookingExtTOList.Count; k++)
                                            {
                                                TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                                                tblBookingExtTO = tblBookingExtTOList[k];
                                                tblLoadingSlipExtTO.BalanceQty = tblBookingExtTO.BalanceQty;
                                                tblLoadingSlipExtTO.LoadingQty = tblBookingExtTO.BalanceQty;
                                                tblLoadingSlipExtTO.MaterialId = tblBookingExtTO.MaterialId;
                                                tblLoadingSlipExtTO.ProdCatId = tblBookingExtTO.ProdCatId;
                                                tblLoadingSlipExtTO.ProdSpecId = tblBookingExtTO.ProdSpecId;
                                                //tblLoadingSlipExtTO.BrandId = tblBookingExtTO.BrandId;
                                                tblLoadingSlipExtTO.ProdItemId = tblBookingExtTO.ProdItemId;
                                                tblLoadingSlipExtTO.DisplayName = tblBookingExtTO.DisplayName;
                                                totLayerQty += tblBookingExtTO.BalanceQty;
                                                // tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty += tblBookingExtTO.BalanceQty;
                                                tblLoadingSlipExtTO.LoadingLayerid = distBookingScheduleTO.LoadingLayerId;
                                                tblLoadingSlipExtTO.MaterialDesc = tblBookingExtTO.MaterialSubType;
                                                tblLoadingSlipExtTO.ProdCatDesc = tblBookingExtTO.ProdCatDesc;
                                                tblLoadingSlipExtTO.ProdSpecDesc = tblBookingExtTO.ProdSpecDesc;
                                                //tblLoadingSlipExtTO.BrandDesc = tblBookingExtTO.BrandDesc;
                                                tblLoadingSlipExtTO.BookingId = tblBookingTO.IdBooking;
                                                tblLoadingSlipExtTO.LoadingLayerDesc = distBookingScheduleTO.LoadingLayerDesc;
                                                tblLoadingSlipExtTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;
                                                tblLoadingSlipTO.LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                                            }
                                            if (bookQty >= totLayerQty)
                                            {
                                                bookQty = bookQty - totLayerQty;

                                            }
                                            else
                                            {
                                                totLayerQty = bookQty;
                                                bookQty = 0;
                                            }
                                            tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = totLayerQty;


                                        }
                                        else
                                        {
                                            //tblLoadingSlipTO.NoOfDeliveries = 1;
                                            TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                                            tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = tblBookingTO.PendingQty;
                                            tblLoadingSlipExtTO.LoadingLayerid = (int)Constants.LoadingLayerE.BOTTOM;
                                            tblLoadingSlipTO.LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);

                                        }
                                        tblLoadingTO.NoOfDeliveries = distinctBookingScheduleList.Count;
                                        tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTO);
                                    }

                                }

                            }
                            else
                            {
                                TblLoadingSlipTO tblLoadingSlipTO = selectLoadingSlipTO(tblBookingTO);
                                tblLoadingTO.NoOfDeliveries = 1;
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                                tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = tblBookingTO.IdBooking;
                                tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = tblBookingTO.PendingQty;
                                tblLoadingSlipExtTO.LoadingLayerid = (int)Constants.LoadingLayerE.BOTTOM;
                                tblLoadingSlipTO.LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                                tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTO);

                            }
                        }
                    }
                }
                return tblLoadingTO;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        private static TblLoadingSlipTO selectLoadingSlipTO(TblBookingsTO tblBookingTO)
        {
            TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
            //tblLoadingSlipTO.VehicleNo = tblBookingTO.VehicleNo;
            tblLoadingSlipTO.CnfOrgId = tblBookingTO.CnFOrgId;
            tblLoadingSlipTO.CnfOrgName = tblBookingTO.CnfName;
            tblLoadingSlipTO.DealerOrgId = tblBookingTO.DealerOrgId;
            tblLoadingSlipTO.DealerOrgName = tblBookingTO.DealerName;
            tblLoadingSlipTO.BookingId = tblBookingTO.IdBooking;
            tblLoadingSlipTO.CdStructureId = tblBookingTO.CdStructureId;
            tblLoadingSlipTO.CdStructure = tblBookingTO.CdStructure;
            tblLoadingSlipTO.IsConfirmed = tblBookingTO.IsConfirmed;
            tblLoadingSlipTO.DealerOrgId = tblBookingTO.DealerOrgId;
            tblLoadingSlipTO.DealerOrgName = tblBookingTO.DealerName;
            //tblLoadingSlipTO.FreightAmt = tblBookingTO.FreightAmt;
            tblLoadingSlipTO.OrcAmt = tblBookingTO.OrcAmt;
            tblLoadingSlipTO.OrcMeasure = tblBookingTO.OrcMeasure;
            //tblLoadingSlipTO.ORCPersonName = tblBookingTO.ORCPersonName;
            tblLoadingSlipTO.Comment = tblBookingTO.Comments;
            tblLoadingSlipTO.TblLoadingSlipDtlTO = new TblLoadingSlipDtlTO();
            tblLoadingSlipTO.DeliveryAddressTOList = new List<TblLoadingSlipAddressTO>();
            tblLoadingSlipTO.LoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
            //tblLoadingSlipTO.PaymentTermOptionRelationTOLst = tblBookingTO.PaymentTermOptionRelationTOLst;
            tblLoadingSlipTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;
            return tblLoadingSlipTO;
        }

        public static TblLoadingTO SelectTblLoadingTO (Int32 idLoading) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return TblLoadingDAO.SelectTblLoading (idLoading, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public static TblLoadingTO SelectTblLoadingTOByLoadingSlipId (Int32 loadingSlipId) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return TblLoadingDAO.SelectTblLoadingByLoadingSlipId (loadingSlipId, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public static TblLoadingTO SelectTblLoadingTOByModBusRefId (Int32 modBusRefId) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return TblLoadingBL.SelectTblLoadingTOByModBusRefId (modBusRefId, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }
        public static TblLoadingTO SelectTblLoadingTOByModBusRefId (Int32 modBusRefId, SqlConnection conn, SqlTransaction tran) {
            return TblLoadingDAO.SelectTblLoadingTOByModBusRefId (modBusRefId, conn, tran);
        }

        public static List<TblLoadingTO> SelectLoadingTOListWithDetails (string idLoadings) {
            try {
                string[] arrLoadingIds = null;
                List<TblLoadingTO> tblLoadingToList = new List<TblLoadingTO> ();
                int confiqId = Constants.getweightSourceConfigTO ();
                if (idLoadings.Contains (',')) {
                    arrLoadingIds = idLoadings.Split (',');
                } else {
                    arrLoadingIds = new string[] { idLoadings };
                }
                foreach (string loadingId in arrLoadingIds) {
                    TblLoadingTO tblLoadingTO = SelectTblLoadingTO (Convert.ToInt32 (loadingId));
                    tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (Convert.ToInt32 (loadingId));

                    if (confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) ||
                        confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.BOTH)) {
                        IoT.IotCommunication.GetItemDataFromIotAndMerge (tblLoadingTO, true);
                    }

                    tblLoadingToList.Add (tblLoadingTO);
                }

                return tblLoadingToList;
            } catch (Exception ex) {
                return null;
            }

        }
        public static TblLoadingTO SelectLoadingTOWithDetails (Int32 idLoading) {
            try {

                TblLoadingTO tblLoadingTO = SelectTblLoadingTO (idLoading);
                tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (idLoading);
                if (tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_DELIVERED && tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_NOT_CONFIRM) {
                    int confiqId = Constants.getweightSourceConfigTO ();
                    if (confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) ||
                        confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.BOTH)) {
                        IoT.IotCommunication.GetItemDataFromIotAndMerge (tblLoadingTO, true);
                    }
                }

                return tblLoadingTO;
            } catch (Exception ex) {
                return null;
            }

        }

        public static ResultMessage GetUrlToPrintLoadingSlip(Int32 idLoading)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblLoadingTO LoadingTO = SelectTblLoadingTO(idLoading);
                LoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(idLoading);
                double totalBundleQty = 0;
                double totalLoadingQty = 0;
                double loadindSlipId = 0;
                if (LoadingTO != null)
                {
                    Int32 IS_REQUIRE_DIFFERENT_DT_FOR_6MM_MATERIAL = 0;
                    int SHOW_LOADING_SLIP_TOTAL_VALUE = 0;
                    DataSet printDataSet = new DataSet();

                    //headerDT
                    DataTable headerDT = new DataTable();
                    DataTable addressDT = new DataTable();
                    DataTable loadingDT = new DataTable();
                    DataTable loadingItemDT = new DataTable();
                    DataTable loadingItemTotalDT = new DataTable();

                    DataTable specificItemLoadingDT = new DataTable();
                    DataTable itemFooterDetailsDT = new DataTable();

                    DataTable multipleInvoiceCopyDT = new DataTable();
                    headerDT.TableName = "headerDT";
                    loadingDT.TableName = "loadingDT";
                    addressDT.TableName = "addressDT";
                    loadingItemDT.TableName = "loadingItemDT";
                    loadingItemTotalDT.TableName = "loadingItemTotalDT";

                    specificItemLoadingDT.TableName = "specificItemLoadingDT";
                    itemFooterDetailsDT.TableName = "itemFooterDetailsDT";

                    multipleInvoiceCopyDT.Columns.Add("invoiceCopyName");
                    multipleInvoiceCopyDT.Columns.Add("layerId");

                    headerDT.Columns.Add("CreatedOnStr");
                    headerDT.Columns.Add("CnfOrgName");
                    headerDT.Columns.Add("TransporterOrgName");
                    headerDT.Columns.Add("DriverName");
                    headerDT.Columns.Add("ContactNo");
                    headerDT.Columns.Add("LoadingSlipNo");
                    headerDT.Columns.Add("DealerOrgName");
                    headerDT.Columns.Add("VehicleNo");
                    headerDT.Columns.Add("LoadingId");
                    headerDT.Columns.Add("BookingRate");
                    headerDT.Columns.Add("CdStructure");
                    headerDT.Columns.Add("IsConfirmed");
                    headerDT.Columns.Add("FreightAmt");
                    headerDT.Columns.Add("BillingName");
                    headerDT.Columns.Add("ConsigneeName");
                    headerDT.Columns.Add("BillingAddress");
                    headerDT.Columns.Add("ConsigneeAddress");
                    headerDT.Columns.Add("BillingGstNo");
                    headerDT.Columns.Add("ConsigneeGstNo");
                    headerDT.Columns.Add("ShippingTo");
                    headerDT.Columns.Add("ShippingAddress");
                    headerDT.Columns.Add("LoadingSlipId");
                    headerDT.Columns.Add("LoadingLayerDesc");
                    headerDT.Columns.Add("DriverContactNo");
                    headerDT.Columns.Add("PreparedBy");
                    headerDT.Columns.Add("Comment");
                    headerDT.Columns.Add("LayerNo");
                    headerDT.Columns.Add("BrandDesc");
                    headerDT.Columns.Add("SuperwiseName");
                    headerDT.Columns.Add("layerId");
                    headerDT.Columns.Add("OrcAmt");

                    loadingItemDT.Columns.Add("DisplayName");
                    loadingItemDT.Columns.Add("MaterialDesc");
                    loadingItemDT.Columns.Add("ProdItemDesc");
                    loadingItemDT.Columns.Add("LoadingQty");
                    loadingItemDT.Columns.Add("Bundles");
                    loadingItemDT.Columns.Add("LoadedWeight");
                    loadingItemDT.Columns.Add("MstLoadedBundles");
                    loadingItemDT.Columns.Add("LoadedBundles");
                    loadingItemDT.Columns.Add("RatePerMT");
                    loadingItemDT.Columns.Add("LoadingSlipId");
                    loadingItemDT.Columns.Add("BrandDesc");
                    loadingItemDT.Columns.Add("ProdSpecDesc");
                    loadingItemDT.Columns.Add("ProdcatDesc");
                    loadingItemDT.Columns.Add("ItemName");
                    loadingItemDT.Columns.Add("DisplayField");

                    loadingItemDT.Columns.Add("layerId");

                    specificItemLoadingDT.Columns.Add("DisplayName");
                    specificItemLoadingDT.Columns.Add("MaterialDesc");
                    specificItemLoadingDT.Columns.Add("ProdItemDesc");
                    specificItemLoadingDT.Columns.Add("LoadingQty");
                    specificItemLoadingDT.Columns.Add("Bundles");
                    specificItemLoadingDT.Columns.Add("LoadedWeight");
                    specificItemLoadingDT.Columns.Add("MstLoadedBundles");
                    specificItemLoadingDT.Columns.Add("LoadedBundles");
                    specificItemLoadingDT.Columns.Add("RatePerMT");
                    specificItemLoadingDT.Columns.Add("LoadingSlipId");
                    specificItemLoadingDT.Columns.Add("BrandDesc");
                    specificItemLoadingDT.Columns.Add("ProdSpecDesc");
                    specificItemLoadingDT.Columns.Add("ProdcatDesc");
                    specificItemLoadingDT.Columns.Add("ItemName");
                    specificItemLoadingDT.Columns.Add("DisplayField");

                    if (LoadingTO.LoadingSlipList != null && LoadingTO.LoadingSlipList.Count > 0)
                    {
                        for (int i = 0; i < LoadingTO.LoadingSlipList.Count; i++)
                        {
                            TblLoadingSlipTO loadingSlipTo = LoadingTO.LoadingSlipList[i];
                            headerDT.Rows.Add();
                            Int32 loadHeaderDTCount = headerDT.Rows.Count - 1;
                            totalLoadingQty = 0;
                            totalBundleQty = 0;
                            headerDT.Rows[loadHeaderDTCount]["LayerNo"] = i + 1;
                            headerDT.Rows[loadHeaderDTCount]["CreatedOnStr"] = LoadingTO.CreatedOnStr;
                            headerDT.Rows[loadHeaderDTCount]["CnfOrgName"] = LoadingTO.CnfOrgName;
                            headerDT.Rows[loadHeaderDTCount]["TransporterOrgName"] = LoadingTO.TransporterOrgName;
                            headerDT.Rows[loadHeaderDTCount]["DriverName"] = LoadingTO.DriverName;
                            headerDT.Rows[loadHeaderDTCount]["LoadingSlipNo"] = loadingSlipTo.LoadingSlipNo;
                            headerDT.Rows[loadHeaderDTCount]["DealerOrgName"] = loadingSlipTo.DealerOrgName;
                            headerDT.Rows[loadHeaderDTCount]["VehicleNo"] = LoadingTO.VehicleNo;
                            headerDT.Rows[loadHeaderDTCount]["LoadingId"] = loadingSlipTo.LoadingId;
                            if (loadingSlipTo.TblLoadingSlipDtlTO != null)
                            {
                                if (loadingSlipTo.TblLoadingSlipDtlTO.BookingRate > 0)
                                    headerDT.Rows[loadHeaderDTCount]["BookingRate"] = loadingSlipTo.TblLoadingSlipDtlTO.BookingRate;
                            }
                            else
                                headerDT.Rows[loadHeaderDTCount]["BookingRate"] = 0;

                            headerDT.Rows[loadHeaderDTCount]["CdStructure"] = loadingSlipTo.CdStructure;
                            headerDT.Rows[loadHeaderDTCount]["Comment"] = loadingSlipTo.Comment;
                            headerDT.Rows[loadHeaderDTCount]["PreparedBy"] = LoadingTO.CreatedByUserName;
                            headerDT.Rows[loadHeaderDTCount]["LoadingSlipId"] = loadingSlipTo.IdLoadingSlip;
                            headerDT.Rows[loadHeaderDTCount]["DriverContactNo"] = LoadingTO.ContactNo;
                            headerDT.Rows[loadHeaderDTCount]["SuperwiseName"]= LoadingTO.SuperwisorName;
                            headerDT.Rows[loadHeaderDTCount]["OrcAmt"] = loadingSlipTo.OrcAmt;
                            if (loadingSlipTo.IsConfirmed == 1)
                            {
                                headerDT.Rows[loadHeaderDTCount]["IsConfirmed"] = "Confirmed";

                            }
                            else
                            {
                                headerDT.Rows[loadHeaderDTCount]["IsConfirmed"] = "-";

                            }
                            //if (loadingSlipTo.IsFreightIncluded == 1)
                            //{
                            //    headerDT.Rows[loadHeaderDTCount]["FreightAmt"] = loadingSlipTo.FreightAmt + " (Included)";
                            //}
                            //else
                            //{
                            //    headerDT.Rows[loadHeaderDTCount]["FreightAmt"] = loadingSlipTo.FreightAmt + " (NotIncluded)";
                            //}

                            if (loadingSlipTo.DeliveryAddressTOList.Count > 0)
                            {
                                TblLoadingSlipAddressTO addressTO = loadingSlipTo.DeliveryAddressTOList.Where(w => w.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                                if (addressTO != null)
                                {
                                    headerDT.Rows[loadHeaderDTCount]["BillingName"] = addressTO.BillingName;
                                    headerDT.Rows[loadHeaderDTCount]["BillingAddress"] = addressTO.Address;
                                    headerDT.Rows[loadHeaderDTCount]["BillingGstNo"] = addressTO.GstNo;
                                    headerDT.Rows[loadHeaderDTCount]["ContactNo"] = addressTO.ContactNo;

                                }

                                TblLoadingSlipAddressTO addressTOC = loadingSlipTo.DeliveryAddressTOList.Where(w => w.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS).FirstOrDefault();
                                if (addressTOC != null)
                                {
                                    headerDT.Rows[loadHeaderDTCount]["ConsigneeName"] = addressTOC.BillingName;
                                    headerDT.Rows[loadHeaderDTCount]["ConsigneeAddress"] = addressTOC.Address;
                                    headerDT.Rows[loadHeaderDTCount]["ConsigneeGstNo"] = addressTOC.GstNo;
                                }
                                
                            }
                            List<TblLoadingSlipExtTO> LoadingSlipExtTOListTemp = loadingSlipTo.LoadingSlipExtTOList.GroupBy(g => g.LoadingLayerid).Select(s => s.FirstOrDefault()).ToList();
                            if (LoadingSlipExtTOListTemp != null && LoadingSlipExtTOListTemp.Count > 0)
                            {
                                headerDT.Rows[loadHeaderDTCount]["LoadingLayerDesc"] = LoadingSlipExtTOListTemp[0].LoadingLayerDesc;
                                headerDT.Rows[loadHeaderDTCount]["layerId"] = LoadingSlipExtTOListTemp[0].LoadingSlipId;

                                multipleInvoiceCopyDT.Rows.Add();
                                multipleInvoiceCopyDT.Rows[loadHeaderDTCount]["invoiceCopyName"] = LoadingSlipExtTOListTemp[0].LoadingLayerDesc+"_" + LoadingSlipExtTOListTemp[0].LoadingSlipId;
                                multipleInvoiceCopyDT.Rows[loadHeaderDTCount]["layerId"] = LoadingSlipExtTOListTemp[0].LoadingSlipId;
                            }

                            for (int j = 0; j < loadingSlipTo.LoadingSlipExtTOList.Count; j++)
                            {
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = loadingSlipTo.LoadingSlipExtTOList[j];
                                {
                                    loadingItemDT.Rows.Add();
                                    Int32 loadItemDTCount = loadingItemDT.Rows.Count - 1;

                                    loadingItemDT.Rows[loadItemDTCount]["layerId"] = LoadingSlipExtTOListTemp[0].LoadingSlipId;
                                    loadingItemDT.Rows[loadItemDTCount]["DisplayName"] = tblLoadingSlipExtTO.DisplayName;

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.MaterialDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["DisplayField"] = tblLoadingSlipExtTO.MaterialDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["DisplayField"] = tblLoadingSlipExtTO.ItemName;
                                    }
                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.MaterialDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["MaterialDesc"] = tblLoadingSlipExtTO.MaterialDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["MaterialDesc"] = "";
                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ItemName))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ItemName"] = tblLoadingSlipExtTO.ItemName;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ItemName"] = "";
                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ProdCatDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ProdCatDesc"] = tblLoadingSlipExtTO.ProdCatDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ProdCatDesc"] = "";

                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ProdSpecDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ProdSpecDesc"] = tblLoadingSlipExtTO.ProdSpecDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ProdSpecDesc"] = "";

                                    }
                                    //if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.BrandDesc))
                                    //{
                                    //    loadingItemDT.Rows[loadItemDTCount]["BrandDesc"] = tblLoadingSlipExtTO.BrandDesc;
                                    //}
                                    //else
                                    //{
                                    //    loadingItemDT.Rows[loadItemDTCount]["BrandDesc"] = "";

                                    //}

                                    loadingItemDT.Rows[loadItemDTCount]["ProdItemDesc"] = tblLoadingSlipExtTO.ProdItemDesc;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadingQty"] = tblLoadingSlipExtTO.LoadingQty;
                                    loadingItemDT.Rows[loadItemDTCount]["Bundles"] = tblLoadingSlipExtTO.Bundles;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadedWeight"] = tblLoadingSlipExtTO.LoadedWeight;
                                   // loadingItemDT.Rows[loadItemDTCount]["MstLoadedBundles"] = tblLoadingSlipExtTO.MstLoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadedBundles"] = tblLoadingSlipExtTO.LoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["RatePerMT"] = tblLoadingSlipExtTO.RatePerMT;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadingSlipId"] = tblLoadingSlipExtTO.LoadingSlipId;
                                    totalBundleQty += tblLoadingSlipExtTO.Bundles;
                                    totalLoadingQty += tblLoadingSlipExtTO.LoadingQty;
                                    loadindSlipId = tblLoadingSlipExtTO.LoadingSlipId;
                                }

                               
                            }
                            if (SHOW_LOADING_SLIP_TOTAL_VALUE == 1)
                            {
                                loadingItemDT.Rows.Add();
                                loadingItemDT.Rows[loadingItemDT.Rows.Count - 1]["LoadingQty"] = totalLoadingQty;
                                loadingItemDT.Rows[loadingItemDT.Rows.Count - 1]["Bundles"] = totalBundleQty;
                                loadingItemDT.Rows[loadingItemDT.Rows.Count - 1]["ProdSpecDesc"] = "Total";
                                loadingItemDT.Rows[loadingItemDT.Rows.Count - 1]["LoadingSlipId"] = loadindSlipId;
                            }
                        }
                    }

                    loadingItemTotalDT.Columns.Add("LoadingQty");
                    loadingItemTotalDT.Columns.Add("Bundles");
                    loadingItemTotalDT.Columns.Add("ProdSpecDesc");
                    
                    headerDT.TableName = "headerDT";

                    printDataSet.Tables.Add(headerDT);
                    multipleInvoiceCopyDT.TableName = "multipleInvoiceCopyDT";
                    printDataSet.Tables.Add(multipleInvoiceCopyDT);

                    loadingItemDT.TableName = "loadingItemDT";
                    specificItemLoadingDT.TableName = "specificItemLoadingDT";
                    printDataSet.Tables.Add(loadingItemDT);
                    printDataSet.Tables.Add(specificItemLoadingDT);
                    printDataSet.Tables.Add(loadingItemTotalDT);

                    //creating template'''''''''''''''''
                    string templateName = "LoadingSlipRpt";
                    String templateFilePath = SelectReportFullName(templateName);
                    //templateFilePath = "C:\\Templates\\BRMUAT\\LoadingSlipRpt.template.xls";
                    String fileName = "Bill-" + DateTime.Now.Ticks;

                    //download location for rewrite  template file
                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                    // RunReport runReport = new RunReport();
                    Boolean IsProduction = true;

                    TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                    if (tblConfigParamsTO != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                        {
                            IsProduction = false;
                        }
                    }
                    resultMessage = CommonDAO.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.PDF_DONT_OPEN, IsProduction);
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {
                            filePath = resultMessage.Tag.ToString();
                        }
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;
                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*Bill*");
                            string[] filesList = Directory.GetFiles(directoryName, "*Bill*");

                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                        }
                    }
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }

        public  static String SelectReportFullName(String reportName)
        {
            String reportFullName = null;

            //MstReportTemplateTO mstReportTemplateTO = MstReportTemplateDAO.SelectMstReportTemplateTO(reportName);
            DimReportTemplateTO dimReportTemplateTO = SelectDimReportTemplateTO(reportName);
            if (dimReportTemplateTO != null)
            {
                TblConfigParamsTO templatePath = TblConfigParamsDAO.SelectTblConfigParamsValByName("REPORT_TEMPLATE_FOLDER_PATH");

                if (templatePath != null)
                    return templatePath.ConfigParamVal + dimReportTemplateTO.ReportFileName + "." + dimReportTemplateTO.ReportFileExtension;
            }
            return reportFullName;
        }

        public  static DimReportTemplateTO SelectDimReportTemplateTO(String reportName)
        {
            return DAL.TblLoadingDAO.SelectDimReportTemplate(reportName);
        }
        public static TblLoadingTO SelectLoadingTOWithDetails (Int32 idLoading, SqlConnection conn, SqlTransaction tran, Boolean getStatusHistory = false) {
            try {
                TblLoadingTO tblLoadingTO = SelectTblLoadingTO (idLoading, conn, tran);
                tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (idLoading, conn, tran);
                if (tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_DELIVERED) {
                    int confiqId = Constants.getweightSourceConfigTO ();
                    if (confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) ||
                        confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.BOTH)) {
                        IoT.IotCommunication.GetItemDataFromIotAndMerge (tblLoadingTO, true, getStatusHistory);
                    }
                }
                return tblLoadingTO;
            } catch (Exception ex) {
                return null;
            }

        }

        public static TblLoadingTO SelectLoadingTOWithDetailsByLoadingSlipId (Int32 loadingSlipId) {
            try {
                TblLoadingTO tblLoadingTO = SelectTblLoadingTOByLoadingSlipId (loadingSlipId);
                tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (tblLoadingTO.IdLoading);

                if (tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_DELIVERED) {
                    int confiqId = Constants.getweightSourceConfigTO ();
                    if (confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) ||
                        confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.BOTH)) {
                        IoT.IotCommunication.GetItemDataFromIotAndMerge (tblLoadingTO, false);
                        foreach (var item in tblLoadingTO.LoadingSlipList) {
                            if (item.IdLoadingSlip == loadingSlipId)
                                IoT.IotCommunication.GetItemDataFromIotForGivenLoadingSlip (item);
                        }
                    }
                }
                return tblLoadingTO;
            } catch (Exception ex) {
                return null;
            }

        }

        public static List<VehicleNumber> SelectAllVehicles () {
            return TblLoadingDAO.SelectAllVehicles ();
        }
        public static List<DropDownTO> SelectAllVehiclesByStatus (int statusId, int status, int userId) {
            #region Get Vhical details from IoT 
            //Added By Kiran 12-12-18
            int weightSourceConfigId = Constants.getweightSourceConfigTO ();
            if (weightSourceConfigId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) && statusId != (int) Constants.TranStatusE.UNLOADING_NEW) {

                String statusIds = statusId + "," + status;
                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Loading List From DB Start ");
                tblUserMachineMappingTo tblUserMachineMappingTo = TblLoadingBL.SelectUserMachineTo (userId);
                int gateId = 0;
                if (tblUserMachineMappingTo != null && tblUserMachineMappingTo.UserId != 0 && tblUserMachineMappingTo.GateId != 0) {
                    gateId = tblUserMachineMappingTo.GateId;
                }
                List<TblLoadingTO> list = TblLoadingBL.SelectAllLoadingListByStatus (Convert.ToString ((int) Constants.TranStatusE.LOADING_CONFIRM) + "," + Convert.ToString ((int) Constants.TranStatusE.LOADING_IN_PROGRESS), gateId);
                string finalStatusId = IoT.IotCommunication.GetIotEncodedStatusIdsForGivenStatus (statusIds);

                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Loading List From DB END ");

                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + finalStatusId + " From Gate IoT Start");

                List<TblLoadingTO> distGate = list.GroupBy (g => g.GateId).Select (s => s.FirstOrDefault ()).ToList ();

                GateIoTResult gateIoTResult = new GateIoTResult ();

                for (int g = 0; g < distGate.Count; g++) {
                    TblLoadingTO tblLoadingTOTemp = distGate[g];
                    TblGateTO tblGateTO = new TblGateTO (tblLoadingTOTemp);
                    GateIoTResult temp = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId (finalStatusId, tblGateTO);

                    if (temp != null && temp.Data != null) {
                        gateIoTResult.Data.AddRange (temp.Data);
                    }
                }
                //GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId(finalStatusId);

                // Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + finalStatusId + " From Gate IoT END ");

                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Mapping & Processing Start ");

                if (gateIoTResult != null && gateIoTResult.Data != null) {
                    List<DropDownTO> dropDownList = new List<DropDownTO> ();
                    for (int j = 0; j < gateIoTResult.Data.Count; j++) {
                        DropDownTO dropDownTo = new DropDownTO ();
                        var data = list.Where (w => w.ModbusRefId == Convert.ToInt32 (gateIoTResult.Data[j][0])).FirstOrDefault ();
                        if (data != null) {
                            dropDownTo.Value = data.IdLoading;
                            dropDownTo.Text = Convert.ToString (gateIoTResult.Data[j][1]);
                            dropDownList.Add (dropDownTo);
                        }
                    }
                    return dropDownList;
                }
                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Mapping & Processing END ");

            }
            #endregion
            return TblLoadingDAO.SelectAllVehiclesListByStatus (statusId, status);
        }

        public static SalesTrackerAPI.DashboardModels.LoadingInfo SelectDashboardLoadingInfo (List<TblUserRoleTO> tblUserRoleTOList, Int32 orgId, DateTime sysDate, Int32 loadingType) {
            try {
                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO ();
                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0) {
                    tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority (tblUserRoleTOList);
                }
                return TblLoadingDAO.SelectDashboardLoadingInfo (tblUserRoleTO, orgId, sysDate, loadingType);
            } catch (Exception ex) {
                return null;
            }
        }

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNo (string vehicleNo, DateTime loadingDate) {
            return TblLoadingDAO.SelectAllLoadingListByVehicleNo (vehicleNo, loadingDate);
        }

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNo (string vehicleNo, bool isAllowNxtLoading, int loadingId) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return TblLoadingDAO.SelectAllLoadingListByVehicleNo (vehicleNo, isAllowNxtLoading, loadingId, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNo (string vehicleNo, bool isAllowNxtLoading, SqlConnection conn, SqlTransaction tran) {
            return TblLoadingDAO.SelectAllLoadingListByVehicleNo (vehicleNo, isAllowNxtLoading, 0, conn, tran);
        }

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNo (string vehicleNo, bool isAllowNxtLoading, SqlConnection conn, SqlTransaction tran, int loadingId = 0) {
            return TblLoadingDAO.SelectAllLoadingListByVehicleNo (vehicleNo, isAllowNxtLoading, loadingId, conn, tran);
        }

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut (string vehicleNo, SqlConnection conn, SqlTransaction tran) {
            return TblLoadingDAO.SelectAllLoadingListByVehicleNoForDelOut (vehicleNo, conn, tran);
        }

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut (int loadingId, SqlConnection conn, SqlTransaction tran) {
            return TblLoadingDAO.SelectAllLoadingListByVehicleNoForDelOut (loadingId, conn, tran);
        }

        public static List<TblLoadingTO> SelectAllInLoadingListByVehicleNo (string vehicleNo) {
            return TblLoadingDAO.SelectAllInLoadingListByVehicleNo (vehicleNo);
        }

        public static Dictionary<Int32, Int32> SelectCountOfLoadingsOfSuperwisorDCT (DateTime date) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return TblLoadingDAO.SelectCountOfLoadingsOfSuperwisor (date, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public static List<TblLoadingTO> SelectAllTblLoading (int cnfId, String loadingStatusIdIn, DateTime loadingDate) {
            return TblLoadingDAO.SelectAllTblLoading (cnfId, loadingStatusIdIn, loadingDate);
        }

        // Vaibhav [29-Nov-2017] Added to select all temp loading details.
        public static List<TblLoadingTO> SelectAllTempLoading (SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_MIGRATE_BEFORE_DAYS, conn, tran);
            if (tblConfigParamsTO == null) {
                resultMessage.DefaultBehaviour ("Error Booking loadingTO is null");
                return null;
            }

            DateTime statusDate = Constants.ServerDateTime.AddDays (-Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal));

            statusDate = GetLastTransactionDate(statusDate);

            try {
                return TblLoadingDAO.SelectAllTempLoading (conn, tran, statusDate);
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "SelectAllTempLoading");
                return null;
            }
        }

        public static DateTime GetLastTransactionDate(DateTime toDate)
        {
            DateTime transactionDate = toDate;

            DateTime reportMinDate = TblReportsBackupDtlsBL.SelectReportMinBackUpdate();
            if (reportMinDate != DateTime.MinValue && reportMinDate < toDate)
            {
                transactionDate = reportMinDate;
            }

            return transactionDate;
        }


        /// <summary>
        /// Vijaymala[24-04-2018] added to get loading details by using booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public static List<TblLoadingTO> SelectAllTblLoadingByBookingId (Int32 bookingId) {

            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO> ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                List<TblLoadingSlipDtlTO> tblLoadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromBookingId (bookingId, conn, tran);
                if (tblLoadingSlipDtlTOList != null && tblLoadingSlipDtlTOList.Count > 0) {

                    for (int i = 0; i < tblLoadingSlipDtlTOList.Count; i++) {

                        TblLoadingSlipDtlTO tblLoadingSlipDtlTO = tblLoadingSlipDtlTOList[i];

                        TblLoadingSlipTO tblLoadingSlipTO = TblLoadingSlipBL.SelectAllLoadingSlipWithDetails (tblLoadingSlipDtlTO.LoadingSlipId, conn, tran);

                        if (tblLoadingSlipTO != null) {

                            TblLoadingTO tblLoadingTO = TblLoadingBL.SelectTblLoadingTO (tblLoadingSlipTO.LoadingId, conn, tran);

                            TblLoadingTO tblLoadingTOAlready = tblLoadingTOList.Where (w => w.IdLoading == tblLoadingTO.IdLoading).FirstOrDefault ();

                            if (tblLoadingTOAlready != null) {
                                if (tblLoadingTOAlready.LoadingSlipList == null) {
                                    tblLoadingTOAlready.LoadingSlipList = new List<TblLoadingSlipTO> ();
                                }
                                tblLoadingTOAlready.LoadingSlipList.Add (tblLoadingSlipTO);
                            } else {

                                if (tblLoadingTO.LoadingSlipList == null) {
                                    tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO> ();
                                }
                                tblLoadingTO.LoadingSlipList.Add (tblLoadingSlipTO);
                                tblLoadingTOList.Add (tblLoadingTO);
                            }

                        }

                    }
                }
                return tblLoadingTOList;
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }

        }
        /// <summary>
        /// Sudhir[18-APR-2018] Added for Loading Status Showing GraphWise on CNF
        /// </summary>
        /// <param name="tblUserRoleTO"></param>
        /// <param name="orgId"></param>
        /// <param name="sysDate"></param>
        /// <param name="loadingType"></param>
        /// <param name="dealerOrgId"></param>
        /// <returns></returns>
        public static List<SalesTrackerAPI.DashboardModels.LoadingInfo> SelectLoadingStatusGraph (List<TblUserRoleTO> tblUserRoleTOList, Int32 orgId, DateTime fromDate, DateTime toDate, Int32 loadingType, Int32 dealerOrgId = 0) {
            //  BL.TblBookingOpngBalBL.CalculateBookingOpeningBalance();
            try {
                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO ();

                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0) {
                    tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority (tblUserRoleTOList);

                }
                return TblLoadingDAO.SelectLoadingStatusGraph (tblUserRoleTO, orgId, fromDate, toDate, loadingType, dealerOrgId);
            } catch (Exception ex) {
                return null;
            }
        }

        #endregion

        #region Insertion
        public static int InsertTblLoading (TblLoadingTO tblLoadingTO) {
            return TblLoadingDAO.InsertTblLoading (tblLoadingTO);
        }

        public static int InsertTblLoading (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            return TblLoadingDAO.InsertTblLoading (tblLoadingTO, conn, tran);
        }

        public static ResultMessage SaveNewLoadingSlip (TblLoadingTO tblLoadingTO) {
            // Constants.writeLog("SaveNewLoadingSlip start connection ");
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                #region 1. Save New Main Loading Slip
                if (tblLoadingTO.FromOrgId == 0)
                {

                    TblConfigParamsTO TblConfigParamsTODefultOrg = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (TblConfigParamsTODefultOrg != null)
                    {
                        tblLoadingTO.FromOrgId = Convert.ToInt32(TblConfigParamsTODefultOrg.ConfigParamVal);
                    }
                }

                //Sanjay [10-Dec-2018] For IoT Implementations
                int weightSourceConfigId = Constants.getweightSourceConfigTO ();
                //Int64 earlierCount = TblLoadingDAO.SelectCountOfLoadingSlips(tblLoadingTO.CreatedOn, conn, tran);
                //earlierCount++;
                //String loadingSlipNo = tblLoadingTO.CreatedOn.Year + "" + tblLoadingTO.CreatedOn.Month + "" + tblLoadingTO.CreatedOn.Day + "/" + earlierCount;

                // Vaibhav [30-Jan-2018] Commented and added to generate loading count.
                //TblEntityRangeTO loadingEntityRangeTO = SelectEntityRangeForLoadingCount(Constants.ENTITY_RANGE_LOADING_COUNT); //Connection and tran for selection and updation removed as due to processing time concurrent request takes existing values from entity range
                //if (loadingEntityRangeTO == null)
                //{
                //    tran.Rollback();
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    resultMessage.Text = "Error : loadingEntityRangeTO is null";
                //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                //    return resultMessage;
                //}
                ////tblLoadingTO.ModbusRefId = GetNextAvailableModRefId(conn, tran);
                //String loadingSlipNo = tblLoadingTO.CreatedOn.Year + "" + tblLoadingTO.CreatedOn.Month + "" + tblLoadingTO.CreatedOn.Day + "/" + tblLoadingTO.ModbusRefId; //loadingEntityRangeTO.EntityPrevValue;
                String loadingSlipNo = tblLoadingTO.LoadingSlipNo; //tblLoadingTO.CreatedOn.Year + "" + tblLoadingTO.CreatedOn.Month + "" + tblLoadingTO.CreatedOn.Day + "/" + tblLoadingTO.ModbusRefId; //loadingEntityRangeTO.EntityPrevValue;
                ////tblLoadingTO.ModbusRefId = loadingEntityRangeTO.EntityPrevValue;
                //loadingEntityRangeTO.EntityPrevValue++;
                //result = BL.TblEntityRangeBL.UpdateTblEntityRange(loadingEntityRangeTO);
                //if (result != 1)
                //{
                //    tran.Rollback();
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    resultMessage.Text = "Error : While UpdateTblEntityRange";
                //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                //    return resultMessage;
                //}

                Boolean isBoyondLoadingQuota = false;
                Double finalLoadQty = 0;
                //tblLoadingTO.LoadingSlipNo = loadingSlipNo;
                tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;
                tblLoadingTO.StatusDate = StaticStuff.Constants.ServerDateTime;
                tblLoadingTO.StatusReason = "Loading Scheduled";
                tblLoadingTO.CreatedOn = Constants.ServerDateTime;

                if (tblLoadingTO.LoadingType == (int) Constants.LoadingTypeE.DELIVERY_INFO) {
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.PENDING_DELIVERY_INFO_FOR_CNF_APPROVAL;
                    tblLoadingTO.StatusReason = "Pending Deliver Information";
                }
                int transporterId = tblLoadingTO.TransporterOrgId;
                string vehicleNumber = tblLoadingTO.VehicleNo;
                if (weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT) {
                    tblLoadingTO.TransporterOrgId = 0;
                    tblLoadingTO.VehicleNo = string.Empty;
                }

                #region Assign default Gate

                if (tblLoadingTO.GateId == 0) {
                    TblGateTO tblGateTO = TblGateBL.GetDefaultTblGateTO ();
                    if (tblGateTO != null) {
                        tblLoadingTO.GateId = tblGateTO.IdGate;
                        tblLoadingTO.PortNumber = tblGateTO.PortNumber;
                        tblLoadingTO.IoTUrl = tblGateTO.IoTUrl;
                        tblLoadingTO.MachineIP = tblGateTO.MachineIP;
                    }
                }

                #endregion

                result = InsertTblLoading (tblLoadingTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error in InsertTblLoading";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                #endregion

                #region 2. Save Individual Loading Slips and Its Qty Details

                if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : LoadingSlipList Found Empty Or Null";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                Double freightPerMT = 0;
                if (tblLoadingTO.IsFreightIncluded == 1) {
                    freightPerMT = tblLoadingTO.FreightAmt; // CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
                    //freightPerMT = CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
                    if (freightPerMT < 0) {
                        tran.Rollback ();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : Freight Calculations is less than 0. Please check the calculations immediatly";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                }
                int modbusRefIdInc = 0;
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_WEIGHT_TOLERANCE_IN_KGS, conn, tran);
                List<TblProductInfoTO> productConfgList = TblProductInfoBL.SelectAllTblProductInfoList (conn, tran);
                for (int i = 0; i < tblLoadingTO.LoadingSlipList.Count; i++) {
                    TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[i];
                    tblLoadingSlipTO.LoadingId = tblLoadingTO.IdLoading;
                    if (weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT) {
                        tblLoadingSlipTO.VehicleNo = string.Empty;
                    } else
                        tblLoadingSlipTO.VehicleNo = tblLoadingTO.VehicleNo;

                    tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;
                    tblLoadingSlipTO.CreatedBy = tblLoadingTO.CreatedBy;
                    tblLoadingSlipTO.CreatedOn = tblLoadingTO.CreatedOn;
                    tblLoadingSlipTO.NoOfDeliveries = tblLoadingTO.NoOfDeliveries;
                    tblLoadingSlipTO.StatusDate = tblLoadingTO.StatusDate;
                    tblLoadingSlipTO.StatusReason = tblLoadingTO.StatusReason;
                    tblLoadingSlipTO.ContactNo = tblLoadingTO.ContactNo;
                    tblLoadingSlipTO.DriverName = tblLoadingTO.DriverName;
                    tblLoadingSlipTO.FromOrgId = tblLoadingTO.FromOrgId;
                    if (tblLoadingTO.LoadingType == (int) Constants.LoadingTypeE.DELIVERY_INFO) {
                        tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.PENDING_DELIVERY_INFO_FOR_CNF_APPROVAL;
                    }
                    // Vaibhav [30-Jan-2018] Commented.
                    //Int64 slipCnt = TblLoadingSlipDAO.SelectCountOfLoadingSlips(tblLoadingTO.CreatedOn, tblLoadingSlipTO.IsConfirmed, conn, tran);
                    //slipCnt++;
                    String slipNo = string.Empty;
                    if (tblLoadingSlipTO.IsConfirmed == 1) {
                        //slipNo = tblLoadingTO.CreatedOn.Year.ToString() + "" + tblLoadingTO.CreatedOn.Month.ToString() + "" + tblLoadingTO.CreatedOn.Day.ToString() + "/" + slipCnt;

                        // Vaibhav [30-Jan-2018] Commented and added to generate confirm loading slip count.
                        TblEntityRangeTO entityRangeTO = SelectEntityRangeForLoadingCount (Constants.ENTITY_RANGE_C_LOADINGSLIP_COUNT, conn, tran);
                        if (entityRangeTO == null) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : entityRangeTO is null";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        slipNo = tblLoadingTO.CreatedOn.Year.ToString () + "" + tblLoadingTO.CreatedOn.Month.ToString () + "" + tblLoadingTO.CreatedOn.Day.ToString () + "/" + entityRangeTO.EntityPrevValue;

                        entityRangeTO.EntityPrevValue++;
                        result = BL.TblEntityRangeBL.UpdateTblEntityRange (entityRangeTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While UpdateTblEntityRange";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                    } else {
                        //slipNo = tblLoadingTO.CreatedOn.Year.ToString() + "" + tblLoadingTO.CreatedOn.Month.ToString() + "" + tblLoadingTO.CreatedOn.Day.ToString() + "" + "NC/" + slipCnt;

                        // Vaibhav [10-Jan-2018] Commented and added to generate nc loading slip count.
                        TblEntityRangeTO entityRangeTO = SelectEntityRangeForLoadingCount (Constants.ENTITY_RANGE_NC_LOADINGSLIP_COUNT, conn, tran);
                        if (entityRangeTO == null) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : entityRangeTO is null";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        slipNo = tblLoadingTO.CreatedOn.Year.ToString () + "" + tblLoadingTO.CreatedOn.Month.ToString () + "" + tblLoadingTO.CreatedOn.Day.ToString () + "" + "NC/" + entityRangeTO.EntityPrevValue;

                        entityRangeTO.EntityPrevValue++;
                        result = BL.TblEntityRangeBL.UpdateTblEntityRange (entityRangeTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While UpdateTblEntityRange";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }
                    }

                    tblLoadingSlipTO.LoadingSlipNo = slipNo;
                    result = BL.TblLoadingSlipBL.InsertTblLoadingSlip (tblLoadingSlipTO, conn, tran);
                    if (result != 1) {
                        tran.Rollback ();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : While inserting into InsertTblLoadingSlip";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }

                    #region Loading Slip Order And Qty Details

                    TblBookingsTO tblBookingsTO = new Models.TblBookingsTO ();
                    if (tblLoadingTO.LoadingType != (int) Constants.LoadingTypeE.OTHER) {
                        if (tblLoadingSlipTO.TblLoadingSlipDtlTO == null) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : LoadingSlipDtlTOList Found Empty Or Null";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        TblLoadingSlipDtlTO tblLoadingSlipDtlTO = tblLoadingSlipTO.TblLoadingSlipDtlTO;
                        tblLoadingSlipDtlTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;
                        tblLoadingSlipDtlTO.BookingId = tblLoadingSlipDtlTO.IdBooking;
                        result = TblLoadingSlipDtlBL.InsertTblLoadingSlipDtl (tblLoadingSlipDtlTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While inserting into InsertTblLoadingSlipDtl";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        finalLoadQty += tblLoadingSlipDtlTO.LoadingQty;

                        //Call to update pending booking qty for loading

                        tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO (tblLoadingSlipDtlTO.BookingId, conn, tran);
                        if (tblBookingsTO == null) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        if (tblBookingsTO.DealerOrgId != tblLoadingSlipTO.DealerOrgId) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : Loading Slip Dealer and Respective Booking Dealer Not Matches";
                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Dealer Info from booking and Loading not matches";
                            return resultMessage;
                        }

                        tblBookingsTO.IdBooking = tblLoadingSlipDtlTO.BookingId;
                        tblBookingsTO.PendingQty = tblBookingsTO.PendingQty - tblLoadingSlipDtlTO.LoadingQty;
                        tblBookingsTO.UpdatedBy = tblLoadingSlipTO.CreatedBy;
                        tblBookingsTO.UpdatedOn = Constants.ServerDateTime;

                        if (tblBookingsTO.PendingQty < 0) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Pending Qty Of Selected Booking #" + tblBookingsTO.IdBooking + " is less then loading Qty" + Environment.NewLine + " Please recreate the loading slip";
                            return resultMessage;
                        }

                        //Check for Weight Tolerance . If pending Qty is within weight tolerance then mark the booking status as closed.

                        if (tblConfigParamsTO != null) {
                            Double wtToleranceKgs = Convert.ToDouble (tblConfigParamsTO.ConfigParamVal);
                            Double pendingQtyInKgs = tblBookingsTO.PendingQty * 1000;
                            if (pendingQtyInKgs > 0 && pendingQtyInKgs <= wtToleranceKgs) {
                                TblBookingQtyConsumptionTO bookingQtyConsumptionTO = new TblBookingQtyConsumptionTO ();
                                bookingQtyConsumptionTO.BookingId = tblBookingsTO.IdBooking;
                                bookingQtyConsumptionTO.ConsumptionQty = tblBookingsTO.PendingQty;
                                bookingQtyConsumptionTO.CreatedBy = tblBookingsTO.UpdatedBy;
                                bookingQtyConsumptionTO.CreatedOn = tblBookingsTO.UpdatedOn;
                                bookingQtyConsumptionTO.StatusId = (int) tblBookingsTO.TranStatusE;
                                bookingQtyConsumptionTO.WeightTolerance = tblConfigParamsTO.ConfigParamVal + " KGs";
                                bookingQtyConsumptionTO.Remark = "Booking Pending Qty is Within Weight Tolerance Limit and Auto Closed";

                                result = BL.TblBookingQtyConsumptionBL.InsertTblBookingQtyConsumption (bookingQtyConsumptionTO, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error While InsertTblBookingQtyConsumption";
                                    resultMessage.Tag = bookingQtyConsumptionTO;
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }

                                tblBookingsTO.PendingQty = 0;
                            }
                        }

                        result = BL.TblBookingsBL.UpdateBookingPendingQty (tblBookingsTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }
                    }

                    #endregion

                    #region LoadingSlip Layer Material Details.

                    if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0) {
                        if (tblLoadingTO.LoadingType == (int) Constants.LoadingTypeE.OTHER) {
                            for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++) {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];
                                finalLoadQty += tblLoadingSlipExtTO.LoadingQty;
                            } //Vijaymala[23-Aug-2018] Commented for New Changes.
                        }
                        //if (tblLoadingTO.LoadingType == (int)Constants.LoadingTypeE.OTHER) Sudhir[10-APR-2018] Commented for New Changes.
                        if (false) {
                            for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++) {
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];
                                tblLoadingSlipExtTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;
                                result = BL.TblLoadingSlipExtBL.InsertTblLoadingSlipExt (tblLoadingSlipExtTO, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error : While InsertTblLoadingSlipExt Against other LoadingSlip";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                                finalLoadQty += tblLoadingSlipExtTO.LoadingQty;
                                isBoyondLoadingQuota = false;
                            }
                        } else {

                            List<TblLoadingQuotaDeclarationTO> quotaList = TblLoadingQuotaDeclarationBL.SelectLoadingQuotaListForCnfAndDate (tblLoadingTO.CnfOrgId, tblLoadingTO.CreatedOn, conn, tran);
                            #region Sudhir [26-MARCH-2018] Commented 
                            //List<TblParityDetailsTO> parityDetailsTOList = null;
                            //if (tblBookingsTO.ParityId > 0)
                            //    parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(tblBookingsTO.ParityId, 0, conn, tran);
                            #endregion
                            if (productConfgList == null) {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ();
                                resultMessage.Text = "Error : productConfgList Found NULL ";
                                resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Product Master Configuration is not completed.";
                                return resultMessage;
                            }

                            #region Check Stock,Loading Quota,Validate and Not Save 

                            List<TblProductItemTO> stockRequireProductItemList = TblProductItemBL.SelectProductItemListStockUpdateRequire (1);
                            Boolean isStockRequie = false;

                            for (int e = 0; e < tblLoadingSlipTO.LoadingSlipExtTOList.Count; e++) {
                                TblLoadingQuotaDeclarationTO loadingQuotaTOLive = null;
                                tblLoadingSlipTO.LoadingSlipExtTOList[e].ModbusRefId = modbusRefIdInc + 1;
                                modbusRefIdInc++;
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[e];
                                if (tblLoadingSlipExtTO.LoadingQty > 0) {
                                    tblLoadingSlipExtTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;

                                    #region Stock Availability Calculations and Validations
                                    List<TblStockDetailsTO> stockList = null;
                                    //Sudhir[02-APR-2018] Added Condition For ProductItemId.
                                    //if (tblLoadingSlipExtTO.ProdItemId == 0)
                                    //{
                                    //Check If Stock exist Or Not
                                    //stockList = TblStockDetailsDAO.SelectAllTblStockDetails(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingTO.CreatedOn, conn, tran); //Sudhir [10-APR-2018] Commented  
                                    stockList = TblStockDetailsDAO.SelectAllTblStockDetailsOther (tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingTO.CreatedOn, conn, tran);

                                    if (stockList == null) {
                                        tran.Rollback ();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error : stockList Found NULL ";
                                        resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Stock For the Size " + tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + " not found";
                                        resultMessage.Result = 0;
                                        return resultMessage;
                                    }

                                    if (tblLoadingTO.LoadingType != (int) Constants.LoadingTypeE.DELIVERY_INFO) {
                                        // To Use in Stock consumption , Wrt Loading Quota Availability Update Master Stock
                                        tblLoadingSlipExtTO.Tag = stockList;

                                        if (tblLoadingSlipExtTO.ProdItemId > 0) {
                                            isStockRequie = stockRequireProductItemList.Where (ele => ele.IdProdItem == tblLoadingSlipExtTO.ProdItemId).
                                            Select (x => x.IsStockRequire == 1).FirstOrDefault ();
                                            //Sudhir[05-APR-2018] Added for Checking IsStock Require for Each ProductItemId.
                                            if (isStockRequie) {
                                                var totalAvailStock = stockList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                    l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                                    l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                    l.ProdItemId == tblLoadingSlipExtTO.ProdItemId).Sum (s => s.TotalStock);
                                                if (totalAvailStock > tblLoadingSlipExtTO.LoadingQty) {

                                                } else {
                                                    String errorMsg = tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                                                    tran.Rollback ();
                                                    resultMessage.MessageType = ResultMessageE.Error;
                                                    resultMessage.Text = "Error - Stock Is Not Available for item :" + errorMsg;
                                                    resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved.Stock Is Not Available for item :" + errorMsg;
                                                    resultMessage.Result = 0;
                                                    resultMessage.Tag = tblLoadingSlipExtTO;
                                                    return resultMessage;
                                                }
                                            }
                                        }
                                        //Vijaymala [04-06-2018] changes the code to save loading quota for regular as well as isstockrequired item 
                                        else {
                                            isStockRequie = true;
                                        }
                                        if (isStockRequie) {
                                            TblLoadingQuotaDeclarationTO loadingQuotaTO = quotaList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                                l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                l.ProdItemId == tblLoadingSlipExtTO.ProdItemId).FirstOrDefault ();

                                            Double quotaBforeLoading = 0;
                                            Double quotaAfterLoading = 0;

                                            if (loadingQuotaTO != null) {
                                                loadingQuotaTOLive = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO (loadingQuotaTO.IdLoadingQuota, conn, tran);
                                                if (loadingQuotaTOLive == null) {
                                                    tran.Rollback ();
                                                    resultMessage.MessageType = ResultMessageE.Error;
                                                    resultMessage.Text = "Error : loadingQuotaTOLive Found NULL ";
                                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                    resultMessage.Result = 0;
                                                    return resultMessage;
                                                }

                                                quotaBforeLoading = loadingQuotaTOLive.BalanceQuota;
                                                quotaAfterLoading = quotaBforeLoading - tblLoadingSlipExtTO.LoadingQty;

                                                tblLoadingSlipExtTO.QuotaBforeLoading = quotaBforeLoading;
                                                tblLoadingSlipExtTO.QuotaAfterLoading = quotaAfterLoading;
                                                tblLoadingSlipExtTO.LoadingQuotaId = loadingQuotaTOLive.IdLoadingQuota;
                                            } else {
                                                isBoyondLoadingQuota = true;
                                            }
                                            if (!isBoyondLoadingQuota) {
                                                if (tblLoadingSlipExtTO.QuotaAfterLoading < 0)
                                                    isBoyondLoadingQuota = true;
                                            }
                                        }
                                        #endregion

                                        #region Calculate Bundles from Loading Qty and Product Configuration
                                        if (tblLoadingSlipExtTO.ProdItemId == 0) {
                                            var prodConfgTO = productConfgList.Where (p => p.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                p.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                p.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                            ).FirstOrDefault ();

                                            if (prodConfgTO == null) {
                                                tran.Rollback ();
                                                resultMessage.DefaultBehaviour ();
                                                resultMessage.Text = "Error : Product Configuration Not Found For " + tblLoadingSlipExtTO.DisplayName;
                                                //"MaterialId:" + tblLoadingSlipExtTO.MaterialDesc + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatDesc + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecDesc;
                                                resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
                                                return resultMessage;
                                            }

                                            //Product Configuration is per bundles and has avg Bundle Wtin Kg
                                            //Hence convert loading qty(MT) to KG
                                            if (prodConfgTO.AvgBundleWt == 0) {
                                                tran.Rollback ();
                                                resultMessage.DefaultBehaviour ();
                                                resultMessage.Text = "Error : Product Configuration Not Found For " + tblLoadingSlipExtTO.DisplayName;
                                                //"MaterialId:" + tblLoadingSlipExtTO.MaterialDesc + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatDesc + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecDesc;
                                                resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
                                                return resultMessage;
                                            }
                                            Double noOfBundles = (tblLoadingSlipExtTO.LoadingQty * 1000) / prodConfgTO.AvgBundleWt;
                                            tblLoadingSlipExtTO.Bundles = Math.Round (noOfBundles, 0);

                                        } else {
                                            tblLoadingSlipExtTO.Bundles = tblLoadingSlipExtTO.LoadingQty;
                                        }
                                        //}
                                        //else
                                        //{
                                        //    tblLoadingSlipExtTO.Bundles = tblLoadingSlipExtTO.LoadingQty;
                                        //}
                                    }
                                    #endregion

                                    #region Calculate Actual Price From Booking and Parity Settings

                                    Double orcAmtPerTon = 0;
                                    if (tblLoadingSlipTO.OrcMeasure == "Rs/MT") //Need to change
                                    {
                                        orcAmtPerTon = tblLoadingSlipTO.OrcAmt;
                                    } else {
                                        if (tblLoadingSlipTO.OrcAmt > 0)
                                            orcAmtPerTon = tblLoadingSlipTO.OrcAmt / tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty;
                                    }

                                    //Double orcAmtPerTon = 0;
                                    //if (tblBookingsTO.OrcMeasure == "Rs/MT")
                                    //{
                                    //    orcAmtPerTon = tblBookingsTO.OrcAmt;
                                    //}
                                    //else
                                    //{
                                    //    if (tblBookingsTO.OrcAmt > 0)
                                    //        orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
                                    //}

                                    String rateCalcDesc = string.Empty;
                                    rateCalcDesc = "B.R : " + tblBookingsTO.BookingRate + "|";
                                    Double bookingPrice = tblBookingsTO.BookingRate;
                                    Double parityAmt = 0;
                                    Double priceSetOff = 0;
                                    Double paritySettingAmt = 0;
                                    Double bvcAmt = 0;
                                    //TblParitySummaryTO parityTO = null; Sudhir[26-MARCH-2018] Commented As per new Parity Setting
                                    TblParityDetailsTO parityDtlTO = null;
                                    if (tblLoadingTO.LoadingType != (int) Constants.LoadingTypeE.OTHER) {
                                        if (true) {

                                            //var parityDtlTO = parityDetailsTOList.Where(m => m.MaterialId == tblLoadingSlipExtTO.MaterialId
                                            //                        && m.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                            //                        && m.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId).FirstOrDefault();

                                            //Get Latest To Based On -materialId, Date And Time Check Condition Actual TIme < = First Object.
                                            TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType (tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                                            if (tblBookingsTO.BookingRefId > 0)//Reshma Added For Bulk booking
                                            {
                                                TblBookingsTO tblBookingsTOParent = BL.TblBookingsBL.SelectTblBookingsTO(tblBookingsTO.BookingRefId);
                                                if (tblBookingsTOParent != null)
                                                {
                                                    parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, addrTO.StateId, tblBookingsTOParent.BookingDatetime);
                                                }
                                            }
                                            else
                                                parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking (tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, addrTO.StateId, tblBookingsTO.BookingDatetime);

                                            if (parityDtlTO != null) {
                                                parityAmt = parityDtlTO.ParityAmt;
                                                if (tblLoadingSlipTO.IsConfirmed != 1)
                                                    priceSetOff = parityDtlTO.NonConfParityAmt;
                                                else
                                                    priceSetOff = 0;

                                                tblLoadingSlipExtTO.ParityDtlId = parityDtlTO.IdParityDtl;
                                            } else {
                                                tran.Rollback ();
                                                resultMessage.DefaultBehaviour ();
                                                resultMessage.Text = "Error : parityDtlTO Not Found";
                                                string mateDesc = tblLoadingSlipExtTO.DisplayName;
                                                //tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                                                resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                                                return resultMessage;
                                            }

                                            #region
                                            //parityTO = BL.TblParitySummaryBL.SelectTblParitySummaryTO(parityDtlTO.ParityId, conn, tran);
                                            //if (parityTO == null)
                                            //{
                                            //    tran.Rollback();
                                            //    resultMessage.DefaultBehaviour();
                                            //    resultMessage.Text = "Error : ParityTO Not Found";
                                            //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            //    return resultMessage;
                                            //}
                                            #endregion
                                            //paritySettingAmt = parityTO.BaseValCorAmt + parityTO.ExpenseAmt + parityTO.OtherAmt;
                                            //bvcAmt = parityTO.BaseValCorAmt;
                                            //rateCalcDesc += "BVC Amt :" + parityTO.BaseValCorAmt + "|" + "Exp Amt :" + parityTO.ExpenseAmt + "|" + " Other :" + parityTO.OtherAmt + "|";
                                            paritySettingAmt = parityDtlTO.BaseValCorAmt + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                                            bvcAmt = parityDtlTO.BaseValCorAmt;
                                            rateCalcDesc += "BVC Amt :" + parityDtlTO.BaseValCorAmt + "|" + "Exp Amt :" + parityDtlTO.ExpenseAmt + "|" + " Other :" + parityDtlTO.OtherAmt + "|";
                                        } else {
                                            tran.Rollback ();
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "Error : parityDtlTO Not Found";
                                            resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                                            return resultMessage;
                                        }

                                        Double cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);

                                        //Saket [2018-03-22] As per new formula expenese and other amount is applicable for CD.
                                        //if (tblLoadingSlipTO.IsConfirmed == 1)
                                        //    cdApplicableAmt += parityTO.ExpenseAmt + parityTO.OtherAmt; 

                                        //cdApplicableAmt += parityTO.ExpenseAmt + parityTO.OtherAmt; [Sudhir 26-MARCH-2018] Commented.
                                        cdApplicableAmt += parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;

                                        Double cdAmt = 0;
                                        if (tblLoadingSlipTO.CdStructure > 0)
                                            cdAmt = (cdApplicableAmt * tblLoadingSlipTO.CdStructure) / 100;

                                        rateCalcDesc += "CD :" + Math.Round (cdAmt, 2) + "|";
                                        Double rateAfterCD = cdApplicableAmt - cdAmt;

                                        Double gstApplicableAmt = 0;
                                        if (tblLoadingSlipTO.IsConfirmed == 1)
                                            //gstApplicableAmt = rateAfterCD + freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;
                                            gstApplicableAmt = rateAfterCD + freightPerMT;
                                        else
                                            gstApplicableAmt = rateAfterCD;

                                        Double gstAmt = (gstApplicableAmt * 18) / 100;
                                        gstAmt = Math.Round (gstAmt, 2);

                                        Double finalRate = 0;
                                        if (tblLoadingSlipTO.IsConfirmed == 1)
                                            finalRate = gstApplicableAmt + gstAmt;
                                        else {
                                            //Saket [2018-03-22] As per new formula expenese and other amount is applicable for CD (Include in gstApplicableAmt).
                                            //finalRate = gstApplicableAmt + gstAmt + freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;
                                            finalRate = gstApplicableAmt + gstAmt + freightPerMT;
                                        }

                                        tblLoadingSlipExtTO.TaxableRateMT = gstApplicableAmt;
                                        tblLoadingSlipExtTO.RatePerMT = finalRate;
                                        tblLoadingSlipExtTO.CdApplicableAmt = cdApplicableAmt;

                                        //tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;[Sudhir[26-MARCH-2018] Commented
                                        tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;

                                        rateCalcDesc += " ORC :" + orcAmtPerTon + "|" + " Parity :" + parityAmt + "|" + " NC Amt :" + priceSetOff + "|" + " Freight :" + freightPerMT + "|" + " GST :" + gstAmt + "|";
                                        tblLoadingSlipExtTO.RateCalcDesc = rateCalcDesc;
                                        #endregion
                                    }

                                    Int32 bookingExtId = tblLoadingSlipExtTO.BookingExtId;
                                    tblLoadingSlipExtTO.BookingExtId = 0;
                                    result = BL.TblLoadingSlipExtBL.InsertTblLoadingSlipExt (tblLoadingSlipExtTO, conn, tran);
                                    tblLoadingSlipExtTO.BookingExtId = bookingExtId;

                                    if (result != 1) {
                                        tran.Rollback ();
                                        resultMessage.DefaultBehaviour ();
                                        resultMessage.Text = "Error : While InsertTblLoadingSlipExt Against LoadingSlip";
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        return resultMessage;
                                    }

                                    if (tblLoadingSlipExtTO.LoadingQuotaId > 0) // It means loading quota is not allocated. This request is beyond quota
                                    {
                                        //Create Loading Quota Consumption History Record
                                        Models.TblLoadingQuotaConsumptionTO quotaConsumptionTO = new TblLoadingQuotaConsumptionTO ();
                                        quotaConsumptionTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
                                        quotaConsumptionTO.QuotaQty = -tblLoadingSlipExtTO.LoadingQty;
                                        quotaConsumptionTO.AvailableQuota = tblLoadingSlipExtTO.QuotaBforeLoading;
                                        quotaConsumptionTO.BalanceQuota = tblLoadingSlipExtTO.QuotaAfterLoading;
                                        quotaConsumptionTO.LoadingQuotaId = tblLoadingSlipExtTO.LoadingQuotaId;
                                        quotaConsumptionTO.Remark = "Quota Consumed Against Loading Slip No :" + loadingSlipNo;
                                        quotaConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.OUT;
                                        quotaConsumptionTO.CreatedBy = tblLoadingTO.CreatedBy;
                                        quotaConsumptionTO.CreatedOn = tblLoadingTO.CreatedOn;

                                        result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption (quotaConsumptionTO, conn, tran);
                                        if (result != 1) {
                                            tran.Rollback ();
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip";
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            return resultMessage;
                                        }

                                        //Update Balance Quota for Declared Loading Quota
                                        loadingQuotaTOLive.BalanceQuota = loadingQuotaTOLive.BalanceQuota - tblLoadingSlipExtTO.LoadingQty;
                                        loadingQuotaTOLive.UpdatedBy = tblLoadingTO.CreatedBy;
                                        loadingQuotaTOLive.UpdatedOn = tblLoadingTO.CreatedOn;
                                        loadingQuotaTOLive.Remark = tblLoadingSlipExtTO.LoadingQty + " Qty is consumed against Loading Slip : " + loadingSlipNo;
                                        result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration (loadingQuotaTOLive, conn, tran);
                                        if (result != 1) {
                                            tran.Rollback ();
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "Error : While UpdateTblLoadingQuotaDeclaration Against LoadingSlip";
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            return resultMessage;
                                        }
                                    }
                                }


                                #region Adjust Balance Qty

                                List<TblBookingExtTO> tblBookingExtTOListAdj = new List<TblBookingExtTO>();
                                List<TblBookingExtTO> tblBookingExtTOList = new List<TblBookingExtTO>();


                                tblBookingExtTOList = TblBookingExtDAO.SelectAllTblBookingExt(tblBookingsTO.IdBooking, conn, tran);
                                if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
                                {
                                    tblBookingExtTOList = tblBookingExtTOList.OrderBy(o => o.ScheduleDate).ToList();
                                }

                                if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
                                {
                                    if (tblLoadingSlipExtTO.BookingExtId > 0)
                                    {
                                        TblBookingExtTO tblBookingExtTO = tblBookingExtTOList.Where(w => w.IdBookingExt == tblLoadingSlipExtTO.BookingExtId).FirstOrDefault();

                                        tblBookingExtTOListAdj.Add(tblBookingExtTO);

                                    }

                                    List<TblBookingExtTO> temp = tblBookingExtTOList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                       l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                       l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                       l.ProdItemId == tblLoadingSlipExtTO.ProdItemId).ToList();

                                    if (tblLoadingSlipExtTO.BookingExtId > 0)
                                    {
                                        temp = temp.Where(w => w.IdBookingExt != tblLoadingSlipExtTO.BookingExtId).ToList();
                                    }

                                    if (temp != null && temp.Count > 0)
                                    {
                                        tblBookingExtTOListAdj.AddRange(temp);
                                    }

                                    Double qtyToAdjust = tblLoadingSlipExtTO.LoadingQty;
                                    double uomQtytoAdjust = tblLoadingSlipExtTO.Bundles;
                                    for (int a = 0; a < tblBookingExtTOListAdj.Count; a++)
                                    {
                                        if (qtyToAdjust > 0)
                                        {
                                            TblBookingExtTO tblBookingExtTO = tblBookingExtTOListAdj[a];
                                            if (tblBookingExtTO.BalanceQty > 0)
                                            {
                                                if (qtyToAdjust <= tblBookingExtTO.BalanceQty)
                                                {
                                                    tblBookingExtTO.BalanceQty = tblBookingExtTO.BalanceQty - qtyToAdjust;
                                                    //tblBookingExtTO.PendingUomQty = tblBookingExtTO.PendingUomQty - uomQtytoAdjust;
                                                    qtyToAdjust = 0;
                                                    uomQtytoAdjust = 0;
                                                }
                                                else
                                                {

                                                    qtyToAdjust -= tblBookingExtTO.BalanceQty;
                                                    tblBookingExtTO.BalanceQty = 0;
                                                }

                                                tblBookingExtTO.BalanceQty = Math.Round(tblBookingExtTO.BalanceQty, 3);

                                                result = TblBookingExtDAO.UpdateTblBookingExt(tblBookingExtTO, conn, tran);
                                                if (result != 1)
                                                {
                                                    tran.Rollback();
                                                    resultMessage.DefaultBehaviour();
                                                    resultMessage.Text = "Error : While UpdateTblLoadingQuotaDeclaration Against LoadingSlip ";
                                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                    return resultMessage;
                                                }

                                            }
                                        }

                                    }

                                }
                                #endregion

                            }

                            #endregion

                            

                            #region Wrt Loading Quota Availability Update Master Stock

                            //If Loading Quota is expired then do not give master stock effects. Discussed with Nitin K
                            if (tblLoadingTO.LoadingType != (int) Constants.LoadingTypeE.DELIVERY_INFO) {
                                if (!isBoyondLoadingQuota) {

                                    for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++) {
                                        //Vijaymala [04-06-2018] changes the code to save stock consumption for regular as well as isstockrequired item 

                                        isStockRequie = false;
                                        //Vijaymala commented[01-06-2018]
                                        //if (tblLoadingSlipTO.LoadingSlipExtTOList[stk].ProdItemId == 0)
                                        //{

                                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];
                                        if (tblLoadingSlipExtTO.ProdItemId > 0) {
                                            isStockRequie = stockRequireProductItemList.Where (ele => ele.IdProdItem == tblLoadingSlipExtTO.ProdItemId).
                                            Select (x => x.IsStockRequire == 1).FirstOrDefault ();
                                        } else {
                                            isStockRequie = true;
                                        }

                                        if (isStockRequie) {
                                            //Check If Stock exist Or Not
                                            if (tblLoadingSlipExtTO.Tag == null) {
                                                tran.Rollback ();
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                resultMessage.Text = "Error : stockList Found NULL ";
                                                resultMessage.Result = 0;
                                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                return resultMessage;
                                            }

                                            List<TblStockDetailsTO> stockList = (List<TblStockDetailsTO>) tblLoadingSlipExtTO.Tag;

                                            // Create Stock Consumption History Record
                                            var stkConsList = stockList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                                l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                l.ProdItemId == tblLoadingSlipExtTO.ProdItemId).ToList (); //Vijaymala(05-06-2018) added for other item 

                                            Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;
                                            for (int s = 0; s < stkConsList.Count; s++) {

                                                if (totalLoadingQty > 0) {
                                                    resultMessage = UpdateStockAndConsumptionHistory (tblLoadingSlipExtTO, tblLoadingTO, stkConsList[s].IdStockDtl, ref totalLoadingQty, conn, tran);
                                                    if (resultMessage.MessageType != ResultMessageE.Information) {
                                                        tran.Rollback ();
                                                        resultMessage.DefaultBehaviour ();
                                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                        resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip";
                                                        return resultMessage;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                        }
                    } else {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ();
                        resultMessage.Text = "Error : LoadingSlipExtTOList(Loading Layer Details) Found Null Or Empty";
                        resultMessage.DisplayMessage = "Error 01 : No Loading Layer Found To Save";
                        return resultMessage;
                    }

                    #endregion

                    #region Save Loading Slip Layerwise Adress Details

                    if (tblLoadingSlipTO.DeliveryAddressTOList != null && tblLoadingSlipTO.DeliveryAddressTOList.Count > 0) {
                        for (int a = 0; a < tblLoadingSlipTO.DeliveryAddressTOList.Count; a++) {
                            TblLoadingSlipAddressTO deliveryAddressTO = tblLoadingSlipTO.DeliveryAddressTOList[a];
                            if (deliveryAddressTO.LoadingLayerId > 0) {
                                deliveryAddressTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;

                                if (string.IsNullOrEmpty (deliveryAddressTO.Country))
                                    deliveryAddressTO.Country = Constants.DefaultCountry;

                                result = BL.TblLoadingSlipAddressBL.InsertTblLoadingSlipAddress (deliveryAddressTO, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While InsertTblLoadingSlipAddress Against LoadingSlip";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    //Delivery Address will not be compulsory while loading
                    //    //tran.Rollback();
                    //    //resultMessage.MessageType = ResultMessageE.Error;
                    //    //resultMessage.Text = "Error : LoadingSlipAddressTOList(Loading Address Details) Found Null Or Empty";
                    //    //return resultMessage;
                    //}

                    #endregion
                }

                #endregion

                #region 3. Prepare A History Record

                TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = tblLoadingTO.GetLoadingStatusHistoryTO ();
                //Sanjay [2017-07-28] Condition Added As Proper history were not getting maintain
                if (isBoyondLoadingQuota) {
                    tblLoadingStatusHistoryTO.TranStatusE = Constants.TranStatusE.LOADING_NOT_CONFIRM;
                    tblLoadingStatusHistoryTO.StatusRemark = "Apporval Needed : Loading Beyond Loading Quota";
                } else {
                    tblLoadingStatusHistoryTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                    tblLoadingStatusHistoryTO.StatusRemark = "Loading Scheduled & Confirmed";
                    if (tblLoadingTO.LoadingType == (int) Constants.LoadingTypeE.DELIVERY_INFO) {
                        tblLoadingStatusHistoryTO.TranStatusE = Constants.TranStatusE.PENDING_DELIVERY_INFO_FOR_CNF_APPROVAL;
                        tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                    }
                }

                result = BL.TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory (tblLoadingStatusHistoryTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error in InsertTblLoadingStatusHistory";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                #endregion

                #region 4. Finally Update the Total Loading Qty And Its Status based on loading quota consumption

                tblLoadingTO.TotalLoadingQty = finalLoadQty;
                tblLoadingTO.UpdatedBy = tblLoadingTO.CreatedBy;
                tblLoadingTO.UpdatedOn = tblLoadingTO.CreatedOn;
                if (isBoyondLoadingQuota) {
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NOT_CONFIRM;
                    tblLoadingTO.StatusReason = "Apporval Needed : Loading Beyond Loading Quota";
                    tblLoadingTO.VehicleNo = vehicleNumber;
                    tblLoadingTO.TransporterOrgId = transporterId;

                } else {
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                    tblLoadingTO.StatusReason = "Loading Scheduled & Confirmed";
                }
                if (tblLoadingTO.LoadingType == (int) Constants.LoadingTypeE.DELIVERY_INFO) {
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.PENDING_DELIVERY_INFO_FOR_CNF_APPROVAL;
                    tblLoadingTO.StatusReason = "Pending Deliver Information";
                }
                result = UpdateTblLoading (tblLoadingTO, conn, tran);
                if (result != 1) {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                //Update Individual Loading Slip statuses
                result = TblLoadingSlipBL.UpdateTblLoadingSlip (tblLoadingTO, conn, tran);
                if (result <= 0) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoadingSlip In Method SaveNewLoadingSlip";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                #endregion

                #region 5. Notifications For Approval Or Information
                if (isBoyondLoadingQuota) {
                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO ();
                    tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED;
                    tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CONFIRMATION_REQUIRED";
                    tblAlertInstanceTO.AlertComment = "Not confirmed loading slip  " + tblLoadingTO.LoadingSlipNo + "  is awaiting for confirmation";
                    tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours (12);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CONFIRMATION_REQUIRED";
                    tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                    tblAlertInstanceTO.RaisedBy = tblLoadingTO.CreatedBy;
                    tblAlertInstanceTO.RaisedOn = tblLoadingTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 0;

                    //added code by @kiran for Save New Alert Instance in seprate thread
                    Thread thread = new Thread (delegate () {
                        BL.TblAlertInstanceBL.SaveNewAlertInstance (tblAlertInstanceTO);
                    });
                    thread.Start ();
                    // ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    //if (rMessage.MessageType != ResultMessageE.Information)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    resultMessage.Text = "Error While SaveNewAlertInstance";
                    //    resultMessage.Tag = tblAlertInstanceTO;
                    //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    //    return resultMessage;
                    //}

                }
                #endregion
                Constants.writeLog ("SaveNewLoadingSlip start write on IoT ");
                // Constants.LoggerObj.LogDebug("SaveNewLoadingSlip start write on IoT " + Constants.ServerDateTime);
                #region Sanjay [10-Dec-2018] Call To IoT To write the vehicle details

                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int)Constants.WeighingDataSourceE.BOTH)
                {
                    if (!isBoyondLoadingQuota && tblLoadingTO.LoadingType != (int)Constants.LoadingTypeE.DELIVERY_INFO)
                    {
                        DimStatusTO statusTO = DAL.DimStatusDAO.SelectDimStatus(tblLoadingTO.StatusId, conn, tran);
                        if (statusTO == null || statusTO.IotStatusId == 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("iot status id not found for loading to pass at gate iot");
                            return resultMessage;
                        }

                        // Call to post data to Gate IoT API
                        List<object[]> frameList = IoT.IotCommunication.GenerateGateIoTFrameData (tblLoadingTO, vehicleNumber, statusTO.IotStatusId, transporterId);
                        if (frameList != null && frameList.Count > 0) {
                            for (int f = 0; f < frameList.Count; f++) {
                                //Saket [2019-04-11] Keep common call for write data too IOT i.e from approval
                                //result = IoT.IotCommunication.PostGateAPIDataToModbusTcpApiForLoadingSlip(tblLoadingTO, frameList[f]);
                                result = IoT.IotCommunication.PostGateAPIDataToModbusTcpApi (tblLoadingTO, frameList[f]);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ("Error while PostGateAPIDataToModbusTcpApi");
                                    resultMessage.DisplayMessage = "Failed due to network error, Please try one more time";
                                    return resultMessage;
                                }
                            }
                        }
                    }
                }

                #endregion
                Constants.writeLog ("SaveNewLoadingSlip End Write on IoT ");
                // Constants.LoggerObj.LogDebug("SaveNewLoadingSlip End Write on IoT " + Constants.ServerDateTime);
                tran.Commit ();
                resultMessage.MessageType = ResultMessageE.Information;
                if (isBoyondLoadingQuota) {
                    resultMessage.Text = "Success, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated but approval needed. Loading Quota Exceeded";
                    resultMessage.DisplayMessage = "Success, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated but approval needed. Loading Quota Exceeded";
                } else {
                    resultMessage.Text = "Success, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated and approved.";
                    resultMessage.DisplayMessage = "Success, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated and approved.";
                    if (tblLoadingTO.LoadingType == (int) Constants.LoadingTypeE.DELIVERY_INFO) {
                        resultMessage.Text = "Success, New Delivery Information # - " + tblLoadingTO.LoadingSlipNo + " is generated and approved.";
                        resultMessage.DisplayMessage = "Success, Delivery Information # - " + tblLoadingTO.LoadingSlipNo + " is generated and approved.";
                    }
                }
                resultMessage.Tag = tblLoadingTO.IdLoading;
                resultMessage.Result = 1;
                return resultMessage;
            } catch (Exception ex) {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "SaveNewLoadingSlip");
                return resultMessage;
            } finally {
                conn.Close ();
                Startup.AvailableModbusRefList = DAL.TblLoadingDAO.GeModRefMaxData ();
            }
        }

       

        //common method added by vipul
        #region GetStatusHistory by  loading Id

        //public static ResultMessage GetStatusHistoryByLoadingIdFromIOT(TblLoadingTO tblLoading)
        //   {
        //    //Call To Gate IoT For Vehicle & Transport Details
        //    GateIoTResult gateIoTResult = IotCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblLoading);
        //    if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count != 0)
        //    {
        //        for (int j = 0; j < gateIoTResult.Data.Count; j++)
        //        {
        //            TblLoadingStatusHistoryTO statusHistoryTO = new TblLoadingStatusHistoryTO();
        //            statusHistoryTO.LoadingId = tblLoading.IdLoading;
        //            DimStatusTO dimStatusTO = statuslist.Where(w => w.IotStatusId == Convert.ToInt16(gateIoTResult.Data[j][(Int32)IoTConstants.GateIoTColE.StatusId])).FirstOrDefault();
        //            if (dimStatusTO != null)
        //            {
        //                statusHistoryTO.StatusId = dimStatusTO.IdStatus;
        //            }
        //            statusHistoryTO.StatusDate = IotCommunication.IoTDateTimeStringToDate((String)gateIoTResult.Data[j][(int)IoTConstants.GateIoTColE.StatusDate]);
        //            statusHistoryTO.StatusRemark = dimStatusTO.StatusName;
        //            statusHistoryList.Add(statusHistoryTO);
        //        }

        //    }
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    return resultMessage;
        //    }

        #endregion
        //public static int GetNextAvailableModRefId()
        //{
        //    //SqlConnection conn = new SqlConnection(Startup.ConnectionString);
        //    //SqlTransaction tran = null;
        //    //try
        //    //{
        //    //    conn.Open();
        //    //    tran = conn.BeginTransaction();
        //    //    return GetNextAvailableModRefId(conn, tran);
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    throw;
        //    //}
        //    //finally
        //    //{
        //    //    conn.Close();
        //    //}
        //}

        public static int GetNextAvailableModRefIdNew () {
            int modRefNumber = 0;
            List<int> list = Startup.AvailableModbusRefList;
            if (list != null && list.Count > 0) {
                int maxNumber = 1;
                modRefNumber = GetAvailNumber (list, maxNumber);
            } else {
                modRefNumber = 1;
            }
            bool isInList = list.Contains (modRefNumber);
            if (isInList)
                return 0;
            else
                Startup.AvailableModbusRefList.Add (modRefNumber);
            return modRefNumber;
        }

        public static int GetAvailNumber (List<int> list, int maxNumber) {
            if (list.Contains (maxNumber)) {
                if (maxNumber > 255) {
                    return 0;
                }
                maxNumber++;
                return GetAvailNumber (list, maxNumber);
            } else {
                return maxNumber;
            }
        }

        public static ResultMessage UpdateLoadingStatusToGateIoT (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();
            int result = 0;
            DimStatusTO statusTO = DAL.DimStatusDAO.SelectDimStatus (tblLoadingTO.StatusId, conn, tran);
            if (statusTO == null || statusTO.IotStatusId == 0) {
                resultMessage.DefaultBehaviour ("iot status id not found for loading to pass at gate iot");
                return resultMessage;
            }

            // Call to post data to Gate IoT API
            List<object[]> frameList = IoT.IotCommunication.GenerateGateIoTStatusFrameData (tblLoadingTO, statusTO.IotStatusId);
            if (frameList != null && frameList.Count > 0) {
                for (int f = 0; f < frameList.Count; f++) {
                    result = IoT.IotCommunication.UpdateLoadingStatusOnGateAPIToModbusTcpApi (tblLoadingTO, frameList[f]);
                    if (result != 1) {
                        resultMessage.DefaultBehaviour ("Error while PostGateAPIDataToModbusTcpApi");
                        return resultMessage;
                    }
                }
            } else {
                resultMessage.DefaultBehaviour ("frameList Found Null Or Empty while PostGateAPIDataToModbusTcpApi");
                return resultMessage;
            }

            resultMessage.DefaultSuccessBehaviour ();
            return resultMessage;
        }

        private static Double CalculateFreightAmtPerTon (List<TblLoadingSlipTO> list, Double totalFreightAmt) {
            try {
                Double freightAmt = 0;
                Double totalQtyInMT = 0;

                for (int i = 0; i < list.Count; i++) {
                    totalQtyInMT += list[i].TblLoadingSlipDtlTO.LoadingQty;
                }

                freightAmt = totalFreightAmt / totalQtyInMT;
                freightAmt = Math.Round (freightAmt, 2);
                return freightAmt;
            } catch (Exception ex) {
                return -1;
            } finally {

            }
        }

        public static ResultMessage UpdateStockAndConsumptionHistory (TblLoadingSlipExtTO tblLoadingSlipExtTO, TblLoadingTO tblLoadingTO, int stockDtlId, ref Double totalLoadingQty, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            int result = 0;
            Double stockQty = 0;

            //Checked from DB To Get Latest Stock Details
            TblStockDetailsTO stockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO (stockDtlId, conn, tran);
            if (stockDetailsTO.BalanceStock >= totalLoadingQty) {
                stockQty = totalLoadingQty;
            } else {
                stockQty = stockDetailsTO.BalanceStock;
            }

            TblStockConsumptionTO stockConsumptionTO = new TblStockConsumptionTO ();
            stockConsumptionTO.BeforeStockQty = stockDetailsTO.BalanceStock;
            stockConsumptionTO.AfterStockQty = stockDetailsTO.BalanceStock - stockQty;
            stockConsumptionTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
            stockConsumptionTO.CreatedBy = tblLoadingTO.CreatedBy;
            stockConsumptionTO.CreatedOn = tblLoadingTO.CreatedOn;
            stockConsumptionTO.Remark = stockQty + " Qty is consumed against Loading Slip : " + tblLoadingTO.LoadingSlipNo;
            stockConsumptionTO.StockDtlId = stockDetailsTO.IdStockDtl;
            stockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.OUT;
            stockConsumptionTO.TxnQty = -stockQty;

            result = BL.TblStockConsumptionBL.InsertTblStockConsumption (stockConsumptionTO, conn, tran);
            if (result != 1) {
                resultMessage.DefaultBehaviour ();
                resultMessage.Text = "Error : While InsertTblStockConsumption Against LoadingSlip";
                return resultMessage;
            }

            //Update Stock Balance Qty
            stockDetailsTO.BalanceStock = stockConsumptionTO.AfterStockQty;
            stockDetailsTO.LoadedStock += stockQty;
            stockDetailsTO.UpdatedBy = tblLoadingTO.CreatedBy;
            stockDetailsTO.UpdatedOn = tblLoadingTO.CreatedOn;
            result = BL.TblStockDetailsBL.UpdateTblStockDetails (stockDetailsTO, conn, tran);
            if (result != 1) {
                resultMessage.DefaultBehaviour ();
                resultMessage.Text = "Error : While UpdateTblStockDetails Against LoadingSlip";
                return resultMessage;
            }

            totalLoadingQty = totalLoadingQty - stockQty;
            resultMessage.MessageType = ResultMessageE.Information;
            resultMessage.Text = "Stock consumption marked Sucessfully";
            return resultMessage;
        }

        public static int PostConvertBasicModeLodingToNormalModde(TblLoadingSlipTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                result = PostConvertBasicModeLodingToNormalMode(tblLoadingTO, conn,tran);

                if(result <=0 )
                {
                    tran.Rollback();
                    return result;
                }
                tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        public static int PostConvertBasicModeLodingToNormalMode(TblLoadingSlipTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            try
            {
                #region update mode id to normal in tempLoading
                tblLoadingTO.ModeId = (int)Constants.ApplicationModeTypeE.NORMAL_MODE;
                result = DAL.TblLoadingDAO.UpdateModeToRegular(tblLoadingTO, conn,tran);

                return result;
                #endregion
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public static int PostLinkingOfLoadingSlipToBooking(TblBookingsTO tblBookingsTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO();

                tblLoadingSlipDtlTO.BookingId = tblBookingsTO.IdBooking;
                tblLoadingSlipDtlTO.LoadingSlipId = tblBookingsTO.LoadingSlipId;
                tblLoadingSlipDtlTO.LoadingQty = tblBookingsTO.LoadingSlipQty;

                result = DAL.TblLoadingSlipDtlDAO.InsertTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn,tran);
                if (result <= 0)
                {
                    tran.Rollback();
                    return result;
                }
                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty - tblBookingsTO.LoadingSlipQty;
                tblBookingsTO.UpdatedBy = 1;
                tblBookingsTO.UpdatedOn = Constants.ServerDateTime;
                result = DAL.TblBookingsDAO.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
                if (result <= 0)
                {
                    tran.Rollback();
                    return result;
                }


                TblLoadingSlipTO tblLoadingTO = new TblLoadingSlipTO();
                tblLoadingTO.LoadingId = tblBookingsTO.LoadingId;

                result = PostConvertBasicModeLodingToNormalMode(tblLoadingTO, conn, tran);
                if (result <= 0)
                {
                    tran.Rollback();
                    return result;
                }
                tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblLoading (TblLoadingTO tblLoadingTO) {
            return TblLoadingDAO.UpdateTblLoading (tblLoadingTO);
        }

        public static ResultMessage PostChangeGateIOTAgainstLoading (TblLoadingTO tblLoadingTO) {

            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                if (tblLoadingTO == null) {
                    throw new Exception ("LoadingTO==null");
                }
                if (tblLoadingTO.GateId == 0) {
                    throw new Exception ("tblLoadingTO.GateId == 0");
                }

                TblLoadingTO existingTblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO (tblLoadingTO.IdLoading);
                if (existingTblLoadingTO == null) {
                    throw new Exception ("existingTblLoadingTO == null" + tblLoadingTO.IdLoading);
                }

                if (tblLoadingTO.GateId == existingTblLoadingTO.GateId) {
                    throw new Exception ("GateId != existingGateId");
                }
                //previous gate Id
                int previousGateId = existingTblLoadingTO.GateId;
                existingTblLoadingTO.GateId = tblLoadingTO.GateId;
                existingTblLoadingTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                existingTblLoadingTO.UpdatedOn = tblLoadingTO.UpdatedOn;
                Int32 result = 0;
                //Get Data from IOT
                //start vipul[16/04/2019] read existing data from gate
                List<DimStatusTO> dimStatusTOList = DimStatusBL.SelectAllDimStatusList ();
                TblLoadingSlipTO loadingslip = new TblLoadingSlipTO ();
                loadingslip.LoadingId = existingTblLoadingTO.IdLoading;
                existingTblLoadingTO = IoT.IotCommunication.GetItemDataFromIotAndMerge (existingTblLoadingTO, false, true); //TblLoadingSlipBL.GetVehicalHistoryDataFromIoT(existingTblLoadingTO, loadingslip, dimStatusTOList);
                //end
                //Write Data to IOT
                int weightSourceConfigId = Constants.getweightSourceConfigTO ();
                // if ((weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int)Constants.WeighingDataSourceE.BOTH) && tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM)
                if ((weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int) Constants.WeighingDataSourceE.BOTH)) {
                    TblGateTO tblGateTO = TblGateBL.SelectTblGateTO (tblLoadingTO.GateId);
                    if (tblGateTO == null) {
                        throw new Exception ("tblGateTO == null for gateId - " + tblLoadingTO.GateId);
                    }

                    tblLoadingTO.PortNumber = tblGateTO.PortNumber;
                    tblLoadingTO.MachineIP = tblGateTO.MachineIP;
                    tblLoadingTO.IoTUrl = tblGateTO.IoTUrl;

                    //start
                    for (int i = 0; i < existingTblLoadingTO.LoadingStatusHistoryTOList.Count; i++) {

                        DimStatusTO statusTO = DAL.DimStatusDAO.SelectDimStatus (existingTblLoadingTO.LoadingStatusHistoryTOList[i].StatusId, conn, tran);
                        if (statusTO == null || statusTO.IotStatusId == 0) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("iot status id not found for loading to pass at gate iot");
                            return resultMessage;
                        }
                        // int result = 0;
                        // Call to post data to Gate IoT API
                        if (i == 0) {
                            List<object[]> frameList = IoT.IotCommunication.GenerateGateIoTFrameData (existingTblLoadingTO, existingTblLoadingTO.VehicleNo, statusTO.IotStatusId, existingTblLoadingTO.TransporterOrgId);
                            if (frameList != null && frameList.Count > 0) {
                                result = IoT.IotCommunication.PostGateAPIDataToModbusTcpApi (tblLoadingTO, frameList[i]);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ("Error while PostGateAPIDataToModbusTcpApi");
                                    return resultMessage;
                                }
                            } else {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ("Error while Generate Gate IoT Frame Data ");
                                return resultMessage;
                            }
                        } else {
                            List<object[]> frameList = IoT.IotCommunication.GenerateGateIoTStatusFrameData (existingTblLoadingTO, statusTO.IotStatusId);
                            if (frameList != null && frameList.Count > 0) {
                                for (int f = 0; f < frameList.Count; f++) {
                                    result = IoT.IotCommunication.UpdateLoadingStatusOnGateAPIToModbusTcpApi (tblLoadingTO, frameList[f]);
                                    if (result != 1) {
                                        resultMessage.DefaultBehaviour ("Error while PostGateAPIDataToModbusTcpApi");
                                        return resultMessage;
                                    }
                                }
                            } else {
                                resultMessage.DefaultBehaviour ("frameList Found Null Or Empty while PostGateAPIDataToModbusTcpApi");
                                return resultMessage;
                            }
                        }
                        Thread.Sleep (500);
                    }
                }

                if (result != 1) {
                    resultMessage.DefaultBehaviour ("Error while writing data on gate iot");
                    return resultMessage;
                }
                var result1 = new GateIoTResult ();
                result1.Code = 0;
                int cnt = 0;
                while (cnt < 3) {
                    result1 = IoT.GateCommunication.DeleteSingleLoadingFromGateIoT (existingTblLoadingTO);
                    if (result1.Code == 1) {
                        break;
                    }
                    cnt++;
                }
                // result1 = IotCommunication.DeleteSingleLoadingFromGateIoT(existingTblLoadingTO);
                if (result1 == null || result1.Code == 0) {
                    //Remove write data from another Gate IOT

                    TblGateTO tblGateTO = TblGateBL.SelectTblGateTO (tblLoadingTO.GateId);
                    if (tblGateTO == null) {
                        throw new Exception ("tblGateTO == null for gateId - " + tblLoadingTO.GateId);
                    }

                    existingTblLoadingTO.PortNumber = tblGateTO.PortNumber;
                    existingTblLoadingTO.MachineIP = tblGateTO.MachineIP;
                    existingTblLoadingTO.IoTUrl = tblGateTO.IoTUrl;
                    var result2 = new GateIoTResult ();
                    result2.Code = 0;
                    cnt = 0;
                    while (cnt < 3) {
                        result2 = IoT.GateCommunication.DeleteSingleLoadingFromGateIoT (existingTblLoadingTO);
                        if (result2.Code == 1) {
                            break;
                        }
                    }

                    throw new Exception ("Error while deleting gate IOT data");

                }

                existingTblLoadingTO.VehicleNo = "";
                existingTblLoadingTO.TransporterOrgId = 0;
                existingTblLoadingTO.StatusId = Convert.ToInt16 (Constants.TranStatusE.LOADING_CONFIRM);
                existingTblLoadingTO.StatusReason = "Loading Scheduled & Confirmed";
                result = TblLoadingBL.UpdateTblLoading (existingTblLoadingTO, conn, tran);
                if (result != 1) {
                    throw new Exception ("Error while updating gateId for loading Id - " + tblLoadingTO.IdLoading);
                }

                tran.Commit ();
                resultMessage.DefaultSuccessBehaviour ();

                return resultMessage;
            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "UpdateDeliverySlipConfirmations");
                return resultMessage;
            } finally {
                conn.Close ();
            }

        }

        public static int UpdateTblLoading (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            return TblLoadingDAO.UpdateTblLoading (tblLoadingTO, conn, tran);
        }
        public static ResultMessage UpdateDeliverySlipConfirmations (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                resultMessage = UpdateDeliverySlipConfirmations (tblLoadingTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information) {
                    tran.Rollback ();
                    return resultMessage;

                }

                // Vaibhav [15-Jan-2017] Check tran is null.
                if (tran.Connection != null && tran.Connection.State == ConnectionState.Open) {

                    tran.Commit ();
                }

                return resultMessage;
            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "UpdateDeliverySlipConfirmations");
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public static ResultMessage UpdateDeliverySlipConfirmations (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            int result = 0;

            //For Cancel it is used.
            Int32 modBusRefId = tblLoadingTO.ModbusRefId;

            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try {

                //Sanjay [10-Dec-2018] For IoT Implementations
                int weightSourceConfigId = Constants.getweightSourceConfigTO ();

                TblLoadingTO existingLoadingTO = SelectTblLoadingTO (tblLoadingTO.IdLoading, conn, tran);
                if (existingLoadingTO == null) {
                    //tran.Rollback(); 
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : existingLoadingTO Found NULL ";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED) {
                    if (existingLoadingTO.TranStatusE == Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH) {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Text = "Record Updated Sucessfully";
                        resultMessage.DisplayMessage = "Record Updated Sucessfully";
                        resultMessage.Result = 1;
                        resultMessage.Tag = tblLoadingTO;
                        return resultMessage;
                    }
                }

                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED) {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Record could not be updated as selected Loading is already " + existingLoadingTO.StatusDesc;
                    resultMessage.DisplayMessage = "Record could not be updated as selected loading is already " + existingLoadingTO.StatusDesc;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region While Delivery OUT check for invoices generated or not.

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED) {

                    if (tblLoadingTO.IsAllowNxtLoading == 0) {
                        Int32 weighingCount = BL.TblWeighingMeasuresBL.SelectDistinctWeighingMeasuresListByLoadingId (tblLoadingTO, conn, tran);

                        String wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE;
                        if (existingLoadingTO.IsInternalCnf == 1)
                            wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE_INTERNAL_TRANSFER;
                        TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(wtScaleConfigStr, conn, tran);
                        //TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);
                        if (configParamsTO != null) {
                            Int32 defWeighingScale = 0;
                            defWeighingScale = Convert.ToInt32 (configParamsTO.ConfigParamVal);
                            if (weighingCount < defWeighingScale) {

                                resultMessage.DisplayMessage = "Take final gross weight";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Take final gross weight ";
                                return resultMessage;

                            }
                        }
                    }

                    if (weightSourceConfigId != (Int32) Constants.WeighingDataSourceE.IoT) {
                        resultMessage = BL.TblWeighingMeasuresBL.CheckInvoiceNoGeneratedByVehicleNo (tblLoadingTO.VehicleNo, conn, tran, true);
                        if (resultMessage.MessageType != ResultMessageE.Information) {
                            //tran.Rollback();
                            return resultMessage;
                        }
                    }
                    // Vijaymala [30-03-2018] added:to update invoice deliveredOn date after loading slip out
                    resultMessage = BL.TblInvoiceBL.UpdateInvoiceAfterloadingSlipOut (tblLoadingTO.IdLoading, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information) {
                        //tran.Rollback();
                        return resultMessage;
                    }

                    #region Update Loading From IOT

                    //if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                    //{
                    //    List<TblLoadingSlipTO> tblLoadingSlipTOList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);
                    //    if (tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 0)
                    //    {
                    //        for (int i = 0; i < tblLoadingSlipTOList.Count; i++)
                    //        {
                    //            //result = TblLoadingSlipBL.UpdateTblLoadingSlip()
                    //            TblLoadingSlipTO tblLoadingSlipTO = tblLoadingSlipTOList[i];
                    //            if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                    //            {
                    //                for (int j = 0; j < tblLoadingSlipTO.LoadingSlipExtTOList.Count; j++)
                    //                {
                    //                    Int32 tempResult = TblLoadingSlipExtBL.UpdateTblLoadingSlipExt(tblLoadingSlipTO.LoadingSlipExtTOList[j], conn, tran);
                    //                    if (tempResult != 1)
                    //                    {
                    //                        resultMessage.MessageType = ResultMessageE.Error;
                    //                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    //                        resultMessage.Text = "Error While UpdateTblLoadingSlipExt In Method UpdateDeliverySlipConfirmations";
                    //                        return resultMessage;
                    //                    }
                    //                }
                    //            }

                    //        }
                    //    }
                    //}

                    #endregion

                }

                #endregion

                #region 0. If User Is Confirming Then Check it can be approve or Not
                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM) {
                    resultMessage = CanGivenLoadingSlipBeApproved (tblLoadingTO, conn, tran);
                    if (resultMessage.MessageType == ResultMessageE.Information &&
                        resultMessage.Result == 1) {
                        // Give Stock Effects
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType () == typeof (List<TblLoadingSlipExtTO>)) {
                            List<TblLoadingSlipExtTO> loadingSlipExtTOList = (List<TblLoadingSlipExtTO>) resultMessage.Tag;

                            for (int stk = 0; stk < loadingSlipExtTOList.Count; stk++) {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = loadingSlipExtTOList[stk];

                                //Vijaymala(05-06-2018) added to get stock for other item as well as regular item

                                //Check If Stock exist Or Not
                                List<TblStockDetailsTO> stockList = TblStockDetailsDAO.SelectAllTblStockDetailsOther (tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingTO.CreatedOn, conn, tran);

                                if (stockList == null) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : stockList Found NULL ";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.Result = 0;
                                    return resultMessage;
                                }

                                // Create Stock Consumption History Record
                                var stkConsList = stockList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                    l.ProdItemId == tblLoadingSlipExtTO.ProdItemId).ToList (); //Vijaymala(05-06-2018) added for other item

                                Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;
                                for (int s = 0; s < stkConsList.Count; s++) {

                                    if (totalLoadingQty > 0) {
                                        resultMessage = UpdateStockAndConsumptionHistory (tblLoadingSlipExtTO, tblLoadingTO, stkConsList[s].IdStockDtl, ref totalLoadingQty, conn, tran);
                                        if (resultMessage.MessageType != ResultMessageE.Information) {
                                            tran.Rollback ();
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip Confirmation";
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            return resultMessage;
                                        }
                                    }
                                }
                            }
                        }
                    } else {
                        tran.Rollback ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                }

                #endregion

                #region 1. Stock Calculations If Cancelling Loading
                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                    List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO> ();

                    if (weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT) {
                        tblWeighingMeasuresTOList = TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId (tblLoadingTO.IdLoading, conn, tran);
                    } else {
                        tblWeighingMeasuresTOList = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId (tblLoadingTO.IdLoading);
                    }
                    if (tblWeighingMeasuresTOList.Count == 0) {
                        // tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);

                        if (tblLoadingTO.LoadingTypeE != Constants.LoadingTypeE.OTHER) {
                            #region 2.1 Reverse Booking Pending Qty
                            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromLoadingId (tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipDtlTOList == null || loadingSlipDtlTOList.Count == 0) {
                                //tran.Rollback();
                                resultMessage.DefaultBehaviour ();
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                resultMessage.Text = "loadingSlipDtlTOList found null";
                                return resultMessage;
                            }
                            //Prajakta[2020-02-05] Added
                            List<TblLoadingSlipExtTO> loadingSlipExtTOTempList = TblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(tblLoadingTO.IdLoading.ToString(), conn, tran);
                            if (loadingSlipExtTOTempList == null || loadingSlipExtTOTempList.Count == 0)
                            {
                                //tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                resultMessage.Text = "loadingSlipExtTOList found null";
                                return resultMessage;
                            }

                            var distinctBookings = loadingSlipDtlTOList.GroupBy (b => b.BookingId).ToList ();
                            for (int i = 0; i < distinctBookings.Count; i++) {
                                Int32 bookingId = distinctBookings[i].Key;
                                Double bookingQty = loadingSlipDtlTOList.Where (b => b.BookingId == bookingId).Sum (l => l.LoadingQty);

                                //Call to update pending booking qty for loading
                                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO ();
                                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO (bookingId, conn, tran);
                                if (tblBookingsTO == null) {
                                    //tran.Rollback();
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                                    return resultMessage;
                                }

                                tblBookingsTO.IdBooking = bookingId;
                                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                                tblBookingsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                tblBookingsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                if (tblBookingsTO.PendingQty < 0) {
                                    //tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                                    return resultMessage;
                                }

                                result = BL.TblBookingsBL.UpdateBookingPendingQty (tblBookingsTO, conn, tran);
                                if (result != 1) {
                                    //tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                                //Prajakta[2020-02-05] Update Balance qty of bookingExt
                                resultMessage = UpdateBookingQty(loadingSlipExtTOTempList, bookingId, conn, tran);
                                if (resultMessage.MessageType == ResultMessageE.Error)
                                {
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While UpdateBookingExtPendingQty";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                            }
                            #endregion

                            //Hrushikesh aded to check Whether it is DI slip or Loading Slip
                            //No Effect on Stock for DI
                            if (tblLoadingTO.LoadingType != (int) Constants.LoadingTypeE.DELIVERY_INFO) {
                                #region 2.2 Reverse Loading Quota Consumed , Stock and Mark a history Record

                                List<TblLoadingSlipExtTO> loadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId (tblLoadingTO.IdLoading, conn, tran);
                                if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0) {
                                    //tran.Rollback();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.Text = "loadingSlipExtTOList found null";
                                    return resultMessage;
                                }

                                //TblLoadingTO existingLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);

                                for (int i = 0; i < loadingSlipExtTOList.Count; i++) {
                                    Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                    Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                                    Double quotaQty = loadingSlipExtTOList[i].LoadingQty;

                                    TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO (loadingQuotaId, conn, tran);
                                    if (tblLoadingQuotaDeclarationTO != null) {
                                        //tran.Rollback();
                                        //resultMessage.DefaultBehaviour();
                                        //resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        //resultMessage.Text = "tblLoadingQuotaDeclarationTO found null";
                                        //return resultMessage;

                                        // Update Loading Quota For Balance Qty
                                        Double balanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                        tblLoadingQuotaDeclarationTO.BalanceQuota += quotaQty;
                                        tblLoadingQuotaDeclarationTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                        tblLoadingQuotaDeclarationTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                        result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration (tblLoadingQuotaDeclarationTO, conn, tran);
                                        if (result != 1) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration While Cancelling Loading Slip";
                                            return resultMessage;
                                        }

                                        //History Record For Loading Quota consumptions
                                        TblLoadingQuotaConsumptionTO consumptionTO = new Models.TblLoadingQuotaConsumptionTO ();
                                        consumptionTO.AvailableQuota = balanceQuota;
                                        consumptionTO.BalanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                        consumptionTO.CreatedBy = tblLoadingQuotaDeclarationTO.UpdatedBy;
                                        consumptionTO.CreatedOn = tblLoadingQuotaDeclarationTO.UpdatedOn;
                                        consumptionTO.LoadingQuotaId = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
                                        consumptionTO.LoadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                        consumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;
                                        consumptionTO.QuotaQty = quotaQty;
                                        consumptionTO.Remark = "Quota reversed after loading slip is cancelled : - " + tblLoadingTO.LoadingSlipNo;
                                        result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption (consumptionTO, conn, tran);
                                        if (result != 1) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                    }
                                    // Update Stock i.e reverse stock. If It is confirmed loading slips

                                    if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM ||
                                        existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED ||
                                        existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED ||
                                        existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN ||
                                        existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING ||
                                        existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)

                                    {

                                        List<TblStockConsumptionTO> tblStockConsumptionTOList = BL.TblStockConsumptionBL.SelectAllStockConsumptionList (loadingSlipExtId, (int) Constants.TxnOperationTypeE.OUT, conn, tran);
                                        if (tblStockConsumptionTOList == null) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        for (int s = 0; s < tblStockConsumptionTOList.Count; s++) {
                                            Double qtyToReverse = Math.Abs (tblStockConsumptionTOList[s].TxnQty);
                                            TblStockDetailsTO tblStockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO (tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                                            if (tblStockDetailsTO == null) {
                                                resultMessage.DefaultBehaviour ();
                                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                resultMessage.Text = "tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                                                return resultMessage;
                                            }

                                            double prevStockQty = tblStockDetailsTO.BalanceStock;
                                            tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                                            tblStockDetailsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                            tblStockDetailsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                            result = BL.TblStockDetailsBL.UpdateTblStockDetails (tblStockDetailsTO, conn, tran);
                                            if (result != 1) {
                                                resultMessage.DefaultBehaviour ();
                                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                                                return resultMessage;
                                            }

                                            // Insert Stock Consumption History Record
                                            TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO ();
                                            reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                                            reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                                            reversedStockConsumptionTO.CreatedBy = tblLoadingTO.UpdatedBy;
                                            reversedStockConsumptionTO.CreatedOn = tblLoadingTO.UpdatedOn;
                                            reversedStockConsumptionTO.LoadingSlipExtId = loadingSlipExtId;
                                            reversedStockConsumptionTO.Remark = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled and Stock is reversed";
                                            reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                                            reversedStockConsumptionTO.TxnQty = qtyToReverse;
                                            reversedStockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;

                                            result = BL.TblStockConsumptionBL.InsertTblStockConsumption (reversedStockConsumptionTO, conn, tran);
                                            if (result != 1) {
                                                resultMessage.DefaultBehaviour ();
                                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                resultMessage.Text = "Error While InsertTblStockConsumption Against LoadingSlip Cancellation";
                                                return resultMessage;
                                            }
                                        }
                                    }
                                }

                                #endregion
                            }
                        }
                    } else if (tblWeighingMeasuresTOList.Count > 0) {
                        resultMessage.DefaultBehaviour ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        if (tblWeighingMeasuresTOList.Count == 1) {
                            resultMessage.Text = "Vehicle Tare weight already done can not Cancel";
                        } else if (tblWeighingMeasuresTOList.Count > 1) {
                            resultMessage.Text = "Vehicle Weighing already done can not Cancel";
                        }
                        return resultMessage;

                    }

                }

                #endregion

                #region 2. Update Loading Slip Status

                if (weightSourceConfigId != (int) Constants.WeighingDataSourceE.IoT || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM) {
                    //if (tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_CONFIRM
                    //    && tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN
                    //    && tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_IN_PROGRESS
                    //    && tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_COMPLETED
                    //    && tblLoadingTO.TranStatusE != Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH
                    //    )
                    //{

                    //Update LoadingTO Status First
                    if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                        tblLoadingTO.ModbusRefId = 0;
                        tblLoadingTO.IsDBup = 0;
                    }
                    
                    //hrushikesh
                    //change cancellation status for Delivery Information
                    if(tblLoadingTO.LoadingType==(int)Constants.LoadingTypeE.DELIVERY_INFO)
                    {
                        tblLoadingTO.StatusId= (int)Constants.TranStatusE.CANCEL_DELIVERY_INFO;

                    }
                    result = UpdateTblLoading (tblLoadingTO, conn, tran);
                    if (result != 1) {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblLoading In Method UpdateDeliverySlipConfirmations";
                        return resultMessage;
                    }

                    //Update Individual Loading Slip statuses
                    result = TblLoadingSlipBL.UpdateTblLoadingSlip (tblLoadingTO, conn, tran);
                    if (result <= 0) {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                        return resultMessage;
                    }
                    //}
                }
                #endregion

                #region 3. Create History Record
                if (weightSourceConfigId != (int) Constants.WeighingDataSourceE.IoT || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                    TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO ();
                    tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                    tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                    tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                    tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                    tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                    tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                    result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory (tblLoadingStatusHistoryTO, conn, tran);
                    if (result != 1) {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                        return resultMessage;
                    }
                }
                #endregion

                #region 4. Notifications For Approval Or Information
                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED ||

                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN) {
                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO ();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO> ();

                    List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList (tblLoadingTO.CnfOrgId, conn, tran);
                    if (cnfUserList != null && cnfUserList.Count > 0) {
                        for (int a = 0; a < cnfUserList.Count; a++) {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO ();
                            tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                            tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                            tblAlertUsersTOList.Add (tblAlertUsersTO);
                        }
                    }

                    if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM) {
                        tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMED;
                        tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CONFIRMED";
                        tblAlertInstanceTO.AlertComment = "Not confirmed loading slip  " + tblLoadingTO.LoadingSlipNo + "  For Vehicle No :" + tblLoadingTO.VehicleNo + "  is approved";
                        tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CONFIRMED";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO> ();
                        Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT (tblLoadingTO.CnfOrgId.ToString (), conn, tran);
                        if (cnfDCT != null) {
                            foreach (var item in cnfDCT.Keys) {
                                TblSmsTO smsTO = new TblSmsTO ();
                                smsTO.MobileNo = cnfDCT[item];
                                smsTO.SourceTxnDesc = "LOADING_SLIP_CONFIRMED";
                                smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                                tblAlertInstanceTO.SmsTOList.Add (smsTO);
                            }
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = BL.TblAlertInstanceBL.ResetAlertInstance ((int) NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                        if (result < 0) {
                            //tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }

                    } else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                        tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED;
                        tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CANCELLED";
                        tblAlertInstanceTO.AlertComment = "Your Generated Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  is cancelled due to " + tblLoadingTO.StatusReason;
                        tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CANCELLED";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO> ();

                        //SMS is not required for loading slip cancellation. Notification is already sent
                        //Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
                        //if (cnfDCT != null)
                        //{
                        //    foreach (var item in cnfDCT.Keys)
                        //    {
                        //        TblSmsTO smsTO = new TblSmsTO();
                        //        smsTO.MobileNo = cnfDCT[item];
                        //        smsTO.SourceTxnDesc = "LOADING_SLIP_CANCELLED";
                        //        smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                        //        tblAlertInstanceTO.SmsTOList.Add(smsTO);
                        //    }
                        //}

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = BL.TblAlertInstanceBL.ResetAlertInstance ((int) NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                        if (result < 0) {
                            //tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }
                    } else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED) {
                        tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.VEHICLE_OUT_FOR_DELIVERY;
                        tblAlertInstanceTO.AlertAction = "VEHICLE_OUT_FOR_DELIVERY";
                        tblAlertInstanceTO.AlertComment = "Your Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  of Vehicle No " + tblLoadingTO.VehicleNo + " is out for delivery";
                        tblAlertInstanceTO.SourceDisplayId = "VEHICLE_OUT_FOR_DELIVERY";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO> ();

                        // No need to send SMS to C&F as we are sending notification. Suggested By Kabra Sir
                        //SMS to C&F
                        //Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
                        //if (cnfDCT != null)
                        //{
                        //    foreach (var item in cnfDCT.Keys)
                        //    {
                        //        TblSmsTO smsTO = new TblSmsTO();
                        //        smsTO.MobileNo = cnfDCT[item];
                        //        smsTO.SourceTxnDesc = "VEHICLE_OUT_FOR_DELIVERY";
                        //        smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                        //        tblAlertInstanceTO.SmsTOList.Add(smsTO);
                        //    }
                        //}

                        //SMS to Dealer
                        Dictionary<int, string> dealerDCT = BL.TblLoadingSlipBL.SelectRegMobileNoDCTForLoadingDealers (tblLoadingTO.IdLoading.ToString (), conn, tran);
                        if (dealerDCT != null) {
                            foreach (var item in dealerDCT.Keys) {
                                TblSmsTO smsTO = new TblSmsTO ();
                                smsTO.MobileNo = dealerDCT[item];
                                smsTO.SourceTxnDesc = "VEHICLE_OUT_FOR_DELIVERY";
                                smsTO.SmsTxt = "Your Loading Slip Ref. " + tblLoadingTO.LoadingSlipNo + " is out for delivery";
                                tblAlertInstanceTO.SmsTOList.Add (smsTO);
                            }
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                    }
                    //Priyanka [09-10-2018] - Added to send notifications to persons about vehicle status like vehicle
                    //                        gate in, reported, clearance to send in for loading. 
                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN) {
                        tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.LOADING_GATE_IN;
                        tblAlertInstanceTO.AlertAction = "LOADING_GATE_IN";
                        tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is gate in for loading.";

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_GATE_IN";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO> ();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    } else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING) {
                        tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.VEHICLE_REPORTED_FOR_LOADING;
                        tblAlertInstanceTO.AlertAction = "VEHICLE_REPORTED_FOR_LOADING";
                        tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is reported for loading.";

                        tblAlertInstanceTO.SourceDisplayId = "VEHICLE_REPORTED_FOR_LOADING";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO> ();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    } else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN) {
                        tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.LOADING_VEHICLE_CLEARANCE_TO_SEND_IN;
                        tblAlertInstanceTO.AlertAction = "LOADING_VEHICLE_CLEARANCE_TO_SEND_IN";
                        tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is clear to send in for loading.";

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_VEHICLE_CLEARANCE_TO_SEND_IN";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO> ();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    }

                    tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours (10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                    tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;
                    ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance (tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information) {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While SaveNewAlertInstance In Method UpdateDeliverySlipConfirmations";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Tag = tblAlertInstanceTO;
                        return resultMessage;
                    }

                }

                #endregion

                #region Kiran [10-Dec-2018] Call To IoT To write the vehicle details 
                if ((weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int) Constants.WeighingDataSourceE.BOTH) && tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM) {

                    DimStatusTO statusTO = DAL.DimStatusDAO.SelectDimStatus (tblLoadingTO.StatusId, conn, tran);
                    if (statusTO == null || statusTO.IotStatusId == 0) {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ("iot status id not found for loading to pass at gate iot");
                        return resultMessage;
                    }
                    // int result = 0;
                    // Call to post data to Gate IoT API
                    List<object[]> frameList = IoT.IotCommunication.GenerateGateIoTFrameData (tblLoadingTO, tblLoadingTO.VehicleNo, statusTO.IotStatusId, tblLoadingTO.TransporterOrgId);
                    if (frameList != null && frameList.Count > 0) {
                        for (int f = 0; f < frameList.Count; f++) {
                            result = IoT.IotCommunication.PostGateAPIDataToModbusTcpApi (tblLoadingTO, frameList[f]);
                            if (result != 1) {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ("Error while PostGateAPIDataToModbusTcpApi");
                                return resultMessage;
                            }
                        }
                    }
                    tblLoadingTO.TransporterOrgId = 0;
                    tblLoadingTO.VehicleNo = string.Empty;
                    result = UpdateTblLoading (tblLoadingTO, conn, tran);
                    if (result != 1) {
                        tran.Rollback ();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While UpdateTblLoading";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                }
                #endregion

                #region 5. Status Update and history to Gate IoT

                if (weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int) Constants.WeighingDataSourceE.BOTH) {
                    //if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM
                    //    || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN
                    //    || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_IN_PROGRESS
                    //    || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED
                    //    || tblLoadingTO.TranStatusE == Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH
                    //    )
                    //{

                    if (tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_CANCEL) {
                        resultMessage = UpdateLoadingStatusToGateIoT (tblLoadingTO, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information) {
                            return resultMessage;
                        }
                    }
                    //}

                    if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {

                        if (weightSourceConfigId == (Int32) Constants.WeighingDataSourceE.IoT) {

                            if (modBusRefId > 0)
                            {

                                tblLoadingTO.ModbusRefId = modBusRefId;

                                int deleteResult = RemoveDateFromGateAndWeightIOT(tblLoadingTO);
                                if (deleteResult != 1)
                                {
                                    throw new Exception("Error While RemoveDateFromGateAndWeightIOT ");
                                }
                                Startup.AvailableModbusRefList = DAL.TblLoadingDAO.GeModRefMaxData();

                                tblLoadingTO.ModbusRefId = 0;
                            }
                        }
                    }
                }

                #endregion

                resultMessage.DefaultSuccessBehaviour ();
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "UpdateDeliverySlipConfirmations");
                return resultMessage;
            }

        }

        /// <summary>
        /// Prajakta[2020-02-05] Update Booking pending qty
        /// </summary>
        /// <param name="loadingSlipExtTOList"></param>
        /// <param name="bookingId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static ResultMessage UpdateBookingQty(List<TblLoadingSlipExtTO> loadingSlipExtTOList, int bookingId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            try
            {
                resultMessage.MessageType = ResultMessageE.Error;
                int result = 0;
                var localList = from lst in loadingSlipExtTOList
                                where lst.BookingId == bookingId
                                select lst;

                if (localList != null && localList.Any())
                {
                    List<TblLoadingSlipExtTO> loadingSlipExtTOLocalList = localList.ToList();

                    List<TblBookingExtTO> tblBookingExtTOList = TblBookingExtDAO.SelectAllTblBookingExt(bookingId, conn, tran);

                    for (int i = 0; i < loadingSlipExtTOLocalList.Count; i++)
                    {
                        var exist = from lst in tblBookingExtTOList
                                    where
                                    lst.ProdCatId == loadingSlipExtTOLocalList[i].ProdCatId
                                    && lst.ProdItemId == loadingSlipExtTOLocalList[i].ProdItemId
                                    && lst.ProdSpecId == loadingSlipExtTOLocalList[i].ProdSpecId
                                    && lst.MaterialId == loadingSlipExtTOLocalList[i].MaterialId
                                    select lst;

                        if (exist != null && exist.Any())
                        {
                            List<TblBookingExtTO> tblBookingExtTOLocalList = null;
                            double toBeAdjQty = loadingSlipExtTOLocalList[i].LoadingQty;
                            if (toBeAdjQty > 0)
                            {
                                tblBookingExtTOLocalList = exist.OrderByDescending(x => x.IdBookingExt).ToList();
                            }
                            else
                                tblBookingExtTOLocalList = exist.OrderBy(x => x.IdBookingExt).ToList();

                            for (int n = 0; n < tblBookingExtTOLocalList.Count; n++)
                            {
                                TblBookingExtTO tblBookingExtTO = tblBookingExtTOLocalList[n];

                                if (toBeAdjQty > 0)
                                {
                                    if (tblBookingExtTO.BalanceQty < tblBookingExtTO.BookedQty)
                                    {
                                        double balQty = tblBookingExtTO.BookedQty - tblBookingExtTO.BalanceQty;

                                        if (balQty <= toBeAdjQty)
                                        {
                                            tblBookingExtTO.BalanceQty = tblBookingExtTO.BookedQty;
                                            toBeAdjQty -= balQty;
                                        }
                                        else
                                        {
                                            tblBookingExtTO.BalanceQty += toBeAdjQty;
                                            toBeAdjQty = 0;
                                        }

                                        result = TblBookingExtDAO.UpdateTblBookingExtBalanceQty(tblBookingExtTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error While Update Booking Balance Qty In Method UpdateTblBookingExtBalanceQty";
                                            return resultMessage;
                                        }

                                        if (toBeAdjQty == 0)
                                            break;
                                    }
                                }
                                else
                                {
                                    if (tblBookingExtTO.BalanceQty > 0)
                                    {
                                        double absQty = Math.Abs(toBeAdjQty);
                                        if (tblBookingExtTO.BalanceQty > absQty)
                                        {
                                            tblBookingExtTO.BalanceQty += toBeAdjQty;
                                            toBeAdjQty = 0;
                                        }
                                        else
                                        {
                                            toBeAdjQty += tblBookingExtTO.BalanceQty;
                                            tblBookingExtTO.BalanceQty = 0;
                                        }

                                        result = TblBookingExtDAO.UpdateTblBookingExtBalanceQty(tblBookingExtTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error While Update Booking Balance Qty In Method UpdateTblBookingExtBalanceQty";
                                            return resultMessage;
                                        }

                                        if (toBeAdjQty == 0)
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        public static ResultMessage RestorePreviousStatusForLoading (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                TblLoadingTO existingLoadingTO = SelectTblLoadingTO (tblLoadingTO.IdLoading, conn, tran);
                if (existingLoadingTO == null) {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : existingLoadingTO Found NULL ";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED) {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Record could not be updated as selected Loading is already " + existingLoadingTO.StatusDesc;
                    resultMessage.DisplayMessage = "Record could not be updated as selected loading is already " + existingLoadingTO.StatusDesc;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                DimStatusTO statusTO = DAL.DimStatusDAO.SelectDimStatus (tblLoadingTO.StatusId, conn, tran);
                if (statusTO == null || statusTO.PrevStatusId == 0) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error...statusTO Found NULL In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }

                tblLoadingTO.StatusId = statusTO.PrevStatusId;
                #region 2. Update Loading Slip Status
                //Update LoadingTO Status First
                result = UpdateTblLoading (tblLoadingTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }

                //Update Individual Loading Slip statuses
                result = TblLoadingSlipBL.UpdateTblLoadingSlip (tblLoadingTO, conn, tran);
                if (result <= 0) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoadingSlip In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }
                #endregion

                #region 3. Create History Record

                TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO ();
                tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason + " Reversed";
                result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory (tblLoadingStatusHistoryTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                    return resultMessage;
                }

                #endregion

                tran.Commit ();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod RestorePreviousStatusForLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public static ResultMessage CancelAllNotConfirmedLoadingSlips () {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_SYTEM_ADMIN_USER_ID, conn, tran);
                Int32 sysAdminUserId = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                DateTime cancellationDateTime = DateTime.MinValue;

                #region 1. Loading Slip Cancellation

                TblConfigParamsTO cancelConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS, conn, tran);
                List<TblLoadingTO> loadingTOListToCancel = TblLoadingDAO.SelectAllLoadingListByStatus (cancelConfigParamsTO.ConfigParamVal, conn, tran);

                if (loadingTOListToCancel != null) {

                    for (int ic = 0; ic < loadingTOListToCancel.Count; ic++) {
                        TblLoadingTO tblLoadingTO = loadingTOListToCancel[ic];
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CANCEL;
                        tblLoadingTO.UpdatedBy = sysAdminUserId;
                        tblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                        tblLoadingTO.StatusDate = tblLoadingTO.UpdatedOn;
                        tblLoadingTO.StatusReason = "No Actions - Auto Cancelled";

                        #region 1. Stock Calculations If Cancelling Loading
                        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                            #region 2.1 Reverse Booking Pending Qty

                            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromLoadingId (tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipDtlTOList == null || loadingSlipDtlTOList.Count == 0) {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ();
                                resultMessage.Text = "loadingSlipDtlTOList found null";
                                return resultMessage;
                            }

                            var distinctBookings = loadingSlipDtlTOList.GroupBy (b => b.BookingId).ToList ();
                            for (int i = 0; i < distinctBookings.Count; i++) {
                                Int32 bookingId = distinctBookings[i].Key;
                                Double bookingQty = loadingSlipDtlTOList.Where (b => b.BookingId == bookingId).Sum (l => l.LoadingQty);

                                //Call to update pending booking qty for loading
                                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO ();
                                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO (bookingId, conn, tran);
                                if (tblBookingsTO == null) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                                    return resultMessage;
                                }

                                tblBookingsTO.IdBooking = bookingId;
                                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                                tblBookingsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                tblBookingsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                if (tblBookingsTO.PendingQty < 0) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                                    return resultMessage;
                                }

                                result = BL.TblBookingsBL.UpdateBookingPendingQty (tblBookingsTO, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                                    return resultMessage;
                                }
                            }

                            #endregion

                            #region 2.2 Reverse Loading Quota Consumed , Stock and Mark a history Record

                            List<TblLoadingSlipExtTO> loadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId (tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0) {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ();
                                resultMessage.Text = "loadingSlipExtTOList found null";
                                return resultMessage;
                            }

                            TblLoadingTO existingLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO (tblLoadingTO.IdLoading, conn, tran);

                            for (int i = 0; i < loadingSlipExtTOList.Count; i++) {
                                Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                                Double quotaQty = loadingSlipExtTOList[i].LoadingQty;

                                TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO (loadingQuotaId, conn, tran);
                                if (tblLoadingQuotaDeclarationTO == null) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "tblLoadingQuotaDeclarationTO found null";
                                    return resultMessage;
                                }

                                // Update Loading Quota For Balance Qty
                                Double balanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                tblLoadingQuotaDeclarationTO.BalanceQuota += quotaQty;
                                tblLoadingQuotaDeclarationTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                tblLoadingQuotaDeclarationTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration (tblLoadingQuotaDeclarationTO, conn, tran);
                                if (result != 1) {
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration While Cancelling Loading Slip";
                                    return resultMessage;
                                }

                                //History Record For Loading Quota consumptions
                                TblLoadingQuotaConsumptionTO consumptionTO = new Models.TblLoadingQuotaConsumptionTO ();
                                consumptionTO.AvailableQuota = balanceQuota;
                                consumptionTO.BalanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                consumptionTO.CreatedBy = tblLoadingQuotaDeclarationTO.UpdatedBy;
                                consumptionTO.CreatedOn = tblLoadingQuotaDeclarationTO.UpdatedOn;
                                consumptionTO.LoadingQuotaId = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
                                consumptionTO.LoadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                consumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;
                                consumptionTO.QuotaQty = quotaQty;
                                consumptionTO.Remark = "Quota reversed after loading slip is cancelled : - " + tblLoadingTO.LoadingSlipNo;
                                result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption (consumptionTO, conn, tran);
                                if (result != 1) {
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip Cancellation";
                                    return resultMessage;
                                }

                                // Update Stock i.e reverse stock. If It is confirmed loading slips

                                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN
                                ) {

                                    List<TblStockConsumptionTO> tblStockConsumptionTOList = BL.TblStockConsumptionBL.SelectAllStockConsumptionList (loadingSlipExtId, (int) Constants.TxnOperationTypeE.OUT, conn, tran);
                                    if (tblStockConsumptionTOList == null) {
                                        resultMessage.DefaultBehaviour ();
                                        resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                                        return resultMessage;
                                    }

                                    for (int s = 0; s < tblStockConsumptionTOList.Count; s++) {
                                        Double qtyToReverse = Math.Abs (tblStockConsumptionTOList[s].TxnQty);
                                        TblStockDetailsTO tblStockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO (tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                                        if (tblStockDetailsTO == null) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        double prevStockQty = tblStockDetailsTO.BalanceStock;
                                        tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                                        tblStockDetailsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                        tblStockDetailsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                        result = BL.TblStockDetailsBL.UpdateTblStockDetails (tblStockDetailsTO, conn, tran);
                                        if (result != 1) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        // Insert Stock Consumption History Record
                                        TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO ();
                                        reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                                        reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                                        reversedStockConsumptionTO.CreatedBy = tblLoadingTO.UpdatedBy;
                                        reversedStockConsumptionTO.CreatedOn = tblLoadingTO.UpdatedOn;
                                        reversedStockConsumptionTO.LoadingSlipExtId = loadingSlipExtId;
                                        reversedStockConsumptionTO.Remark = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled and Stock is reversed";
                                        reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                                        reversedStockConsumptionTO.TxnQty = qtyToReverse;
                                        reversedStockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;

                                        result = BL.TblStockConsumptionBL.InsertTblStockConsumption (reversedStockConsumptionTO, conn, tran);
                                        if (result != 1) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "Error While InsertTblStockConsumption Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }

                        #endregion

                        #region 2. Update Loading Slip Status
                        //Update LoadingTO Status First
                        result = UpdateTblLoading (tblLoadingTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While UpdateTblLoading In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }

                        //Update Individual Loading Slip statuses
                        result = TblLoadingSlipBL.UpdateTblLoadingSlip (tblLoadingTO, conn, tran);
                        if (result <= 0) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }
                        #endregion

                        #region 3. Create History Record

                        TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO ();
                        tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                        tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                        tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                        tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                        tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                        tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                        result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory (tblLoadingStatusHistoryTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }

                        #endregion

                        #region 4. Notifications For Approval Or Information
                        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                            TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO ();
                            List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO> ();

                            List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList (tblLoadingTO.CnfOrgId, conn, tran);
                            if (cnfUserList != null && cnfUserList.Count > 0) {
                                for (int a = 0; a < cnfUserList.Count; a++) {
                                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO ();
                                    tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                                    tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                                    tblAlertUsersTOList.Add (tblAlertUsersTO);
                                }
                            }

                            tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED;
                            tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CANCELLED";
                            tblAlertInstanceTO.AlertComment = "Your Generated Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  is auto cancelled ";
                            tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CANCELLED";
                            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO> ();

                            //SMS Not required in auto cancellation. Discussed in meeting 
                            //Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
                            //if (cnfDCT != null)
                            //{
                            //    foreach (var item in cnfDCT.Keys)
                            //    {
                            //        TblSmsTO smsTO = new TblSmsTO();
                            //        smsTO.MobileNo = cnfDCT[item];
                            //        smsTO.SourceTxnDesc = "LOADING_SLIP_CANCELLED";
                            //        smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                            //        tblAlertInstanceTO.SmsTOList.Add(smsTO);
                            //    }
                            //}

                            tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                            result = BL.TblAlertInstanceBL.ResetAlertInstance ((int) NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                            if (result < 0) {
                                tran.Rollback ();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While Reseting Prev Alert";
                                return resultMessage;
                            }

                            tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                            tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours (10);
                            tblAlertInstanceTO.IsActive = 1;
                            tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                            tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                            tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                            tblAlertInstanceTO.IsAutoReset = 1;
                            ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance (tblAlertInstanceTO, conn, tran);
                            if (rMessage.MessageType != ResultMessageE.Information) {
                                tran.Rollback ();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While SaveNewAlertInstance In Method UpdateDeliverySlipConfirmations";
                                resultMessage.Tag = tblAlertInstanceTO;
                                return resultMessage;
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                //#region 2. Loading Slip Auto Postpone

                //TblConfigParamsTO postponeConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID, conn, tran);
                //List<TblLoadingTO> loadingTOListToPostpone = TblLoadingDAO.SelectAllLoadingListByStatus(postponeConfigParamsTO.ConfigParamVal, conn, tran);

                //if (loadingTOListToPostpone == null)
                //{

                //    for (int ic = 0; ic < loadingTOListToPostpone.Count; ic++)
                //    {
                //        TblLoadingTO tblLoadingTO = loadingTOListToPostpone[ic];
                //        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_POSTPONED;
                //        tblLoadingTO.UpdatedBy = sysAdminUserId;
                //        tblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                //        tblLoadingTO.StatusDate = tblLoadingTO.UpdatedOn;
                //        tblLoadingTO.StatusReason = "No Actions - Auto Postponed For Tommorow";

                //        #region 1. Stock Calculations If Cancelling Loading
                //        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                //        {
                //            #region 2.1 Reverse Booking Pending Qty

                //            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                //            if (loadingSlipDtlTOList == null || loadingSlipDtlTOList.Count == 0)
                //            {
                //                tran.Rollback();
                //                resultMessage.DefaultBehaviour();
                //                resultMessage.Text = "loadingSlipDtlTOList found null";
                //                return resultMessage;
                //            }

                //            var distinctBookings = loadingSlipDtlTOList.GroupBy(b => b.BookingId).ToList();
                //            for (int i = 0; i < distinctBookings.Count; i++)
                //            {
                //                Int32 bookingId = distinctBookings[i].Key;
                //                Double bookingQty = loadingSlipDtlTOList.Where(b => b.BookingId == bookingId).Sum(l => l.LoadingQty);

                //                //Call to update pending booking qty for loading
                //                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();
                //                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(bookingId, conn, tran);
                //                if (tblBookingsTO == null)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.MessageType = ResultMessageE.Error;
                //                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                //                    return resultMessage;
                //                }

                //                tblBookingsTO.IdBooking = bookingId;
                //                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                //                tblBookingsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                //                tblBookingsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                //                if (tblBookingsTO.PendingQty < 0)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.MessageType = ResultMessageE.Error;
                //                    resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                //                    return resultMessage;
                //                }

                //                result = BL.TblBookingsBL.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
                //                if (result != 1)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.MessageType = ResultMessageE.Error;
                //                    resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                //                    return resultMessage;
                //                }
                //            }

                //            #endregion

                //            #region 2.2 Reverse Loading Quota Consumed , Stock and Mark a history Record

                //            List<TblLoadingSlipExtTO> loadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                //            if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0)
                //            {
                //                tran.Rollback();
                //                resultMessage.DefaultBehaviour();
                //                resultMessage.Text = "loadingSlipExtTOList found null";
                //                return resultMessage;
                //            }

                //            TblLoadingTO existingLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);

                //            for (int i = 0; i < loadingSlipExtTOList.Count; i++)
                //            {
                //                Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                //                Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                //                Double quotaQty = loadingSlipExtTOList[i].LoadingQty;

                //                TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaId, conn, tran);
                //                if (tblLoadingQuotaDeclarationTO == null)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.DefaultBehaviour();
                //                    resultMessage.Text = "tblLoadingQuotaDeclarationTO found null";
                //                    return resultMessage;
                //                }

                //                // Update Loading Quota For Balance Qty
                //                Double balanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                //                tblLoadingQuotaDeclarationTO.BalanceQuota += quotaQty;
                //                tblLoadingQuotaDeclarationTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                //                tblLoadingQuotaDeclarationTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                //                result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
                //                if (result != 1)
                //                {
                //                    resultMessage.DefaultBehaviour();
                //                    resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration While Cancelling Loading Slip";
                //                    return resultMessage;
                //                }

                //                //History Record For Loading Quota consumptions
                //                TblLoadingQuotaConsumptionTO consumptionTO = new Models.TblLoadingQuotaConsumptionTO();
                //                consumptionTO.AvailableQuota = balanceQuota;
                //                consumptionTO.BalanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                //                consumptionTO.CreatedBy = tblLoadingQuotaDeclarationTO.UpdatedBy;
                //                consumptionTO.CreatedOn = tblLoadingQuotaDeclarationTO.UpdatedOn;
                //                consumptionTO.LoadingQuotaId = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
                //                consumptionTO.LoadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                //                consumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
                //                consumptionTO.QuotaQty = quotaQty;
                //                consumptionTO.Remark = "Quota reversed after loading slip is cancelled : - " + tblLoadingTO.LoadingSlipNo;
                //                result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(consumptionTO, conn, tran);
                //                if (result != 1)
                //                {
                //                    resultMessage.DefaultBehaviour();
                //                    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip Cancellation";
                //                    return resultMessage;
                //                }

                //                // Update Stock i.e reverse stock. If It is confirmed loading slips

                //                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM
                //                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED
                //                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED
                //                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN
                //                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING)
                //                {

                //                    List<TblStockConsumptionTO> tblStockConsumptionTOList = BL.TblStockConsumptionBL.SelectAllStockConsumptionList(loadingSlipExtId, (int)Constants.TxnOperationTypeE.OUT, conn, tran);
                //                    if (tblStockConsumptionTOList == null)
                //                    {
                //                        resultMessage.DefaultBehaviour();
                //                        resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                //                        return resultMessage;
                //                    }

                //                    for (int s = 0; s < tblStockConsumptionTOList.Count; s++)
                //                    {
                //                        Double qtyToReverse = Math.Abs(tblStockConsumptionTOList[s].TxnQty);
                //                        TblStockDetailsTO tblStockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO(tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                //                        if (tblStockDetailsTO == null)
                //                        {
                //                            resultMessage.DefaultBehaviour();
                //                            resultMessage.Text = "tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                //                            return resultMessage;
                //                        }

                //                        double prevStockQty = tblStockDetailsTO.BalanceStock;
                //                        tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                //                        tblStockDetailsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                //                        tblStockDetailsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                //                        result = BL.TblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
                //                        if (result != 1)
                //                        {
                //                            resultMessage.DefaultBehaviour();
                //                            resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                //                            return resultMessage;
                //                        }

                //                        // Insert Stock Consumption History Record
                //                        TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO();
                //                        reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                //                        reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                //                        reversedStockConsumptionTO.CreatedBy = tblLoadingTO.UpdatedBy;
                //                        reversedStockConsumptionTO.CreatedOn = tblLoadingTO.UpdatedOn;
                //                        reversedStockConsumptionTO.LoadingSlipExtId = loadingSlipExtId;
                //                        reversedStockConsumptionTO.Remark = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled and Stock is reversed";
                //                        reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                //                        reversedStockConsumptionTO.TxnQty = qtyToReverse;
                //                        reversedStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;

                //                        result = BL.TblStockConsumptionBL.InsertTblStockConsumption(reversedStockConsumptionTO, conn, tran);
                //                        if (result != 1)
                //                        {
                //                            resultMessage.DefaultBehaviour();
                //                            resultMessage.Text = "Error While InsertTblStockConsumption Against LoadingSlip Cancellation";
                //                            return resultMessage;
                //                        }
                //                    }
                //                }
                //            }

                //            #endregion
                //        }

                //        #endregion

                //        #region 2. Update Loading Slip Status
                //        //Update LoadingTO Status First
                //        result = UpdateTblLoading(tblLoadingTO, conn, tran);
                //        if (result != 1)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While UpdateTblLoading In Method UpdateDeliverySlipConfirmations";
                //            return resultMessage;
                //        }

                //        //Update Individual Loading Slip statuses
                //        result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
                //        if (result <= 0)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                //            return resultMessage;
                //        }
                //        #endregion

                //        #region 3. Create History Record

                //        TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO();
                //        tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                //        tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                //        tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                //        tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                //        tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                //        tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                //        result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                //        if (result != 1)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                //            return resultMessage;
                //        }

                //        #endregion

                //        #region 4. Notifications For Approval Or Information
                //        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                //        {
                //            TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                //            List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                //            List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblLoadingTO.CnfOrgId, conn, tran);
                //            if (cnfUserList != null && cnfUserList.Count > 0)
                //            {
                //                for (int a = 0; a < cnfUserList.Count; a++)
                //                {
                //                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                //                    tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                //                    tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                //                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                //                }
                //            }

                //            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED;
                //            tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CANCELLED";
                //            tblAlertInstanceTO.AlertComment = "Your Generated Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  is auto cancelled ";
                //            tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CANCELLED";
                //            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                //            tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                //            result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, conn, tran);
                //            if (result < 0)
                //            {
                //                tran.Rollback();
                //                resultMessage.MessageType = ResultMessageE.Error;
                //                resultMessage.Text = "Error While Reseting Prev Alert";
                //                return resultMessage;
                //            }

                //            tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                //            tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                //            tblAlertInstanceTO.IsActive = 1;
                //            tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                //            tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                //            tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                //            tblAlertInstanceTO.IsAutoReset = 1;
                //            ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                //            if (rMessage.MessageType != ResultMessageE.Information)
                //            {
                //                tran.Rollback();
                //                resultMessage.MessageType = ResultMessageE.Error;
                //                resultMessage.Text = "Error While SaveNewAlertInstance In Method UpdateDeliverySlipConfirmations";
                //                resultMessage.Tag = tblAlertInstanceTO;
                //                return resultMessage;
                //            }
                //        }

                //        #endregion
                //    }
                //}

                //#endregion

                tran.Commit ();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod CancelAllNotConfirmedLoadingSlips";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public static ResultMessage CanGivenLoadingSlipBeApproved (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage ();

            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return CanGivenLoadingSlipBeApproved (tblLoadingTO, conn, tran);
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Text = "Loading Slip Can Not Be Approve";
                resultMessage.Exception = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public static ResultMessage CanGivenLoadingSlipBeApproved (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();

            try {

                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId (tblLoadingTO.IdLoading, conn, tran);
                if (tblLoadingSlipExtTOList == null) {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error. Loading Material Not Found";
                    return resultMessage;
                }

                var loadingSlipExtIds = string.Join (",", tblLoadingSlipExtTOList.Select (p => p.IdLoadingSlipExt.ToString ()));
                loadingSlipExtIds = loadingSlipExtIds.TrimEnd (',');
                List<TblLoadingQuotaDeclarationTO> loadingQuotaDeclarationTOList = BL.TblLoadingQuotaDeclarationBL.SelectAllLoadingQuotaDeclListFromLoadingExt (loadingSlipExtIds, conn, tran);
                if (loadingQuotaDeclarationTOList == null) {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error. Loading Quota Not Found";
                    return resultMessage;
                }

                var loadingQuotaIds = string.Join (",", loadingQuotaDeclarationTOList.Select (p => p.IdLoadingQuota.ToString ()));
                loadingQuotaIds = loadingQuotaIds.TrimEnd (',');

                var listToCheck = tblLoadingSlipExtTOList.Where (q => q.QuotaAfterLoading < 0).ToList ().GroupBy (a => new { a.ProdCatId, a.ProdCatDesc, a.ProdSpecId, a.ProdSpecDesc, a.MaterialId, a.MaterialDesc, a.ProdItemId, a.DisplayName }).Select (a => new { ProdCatId = a.Key.ProdCatId, ProdCatDesc = a.Key.ProdCatDesc, ProdSpecId = a.Key.ProdSpecId, ProdSpecDesc = a.Key.ProdSpecDesc, MaterialId = a.Key.MaterialId, MaterialDesc = a.Key.MaterialDesc, ProdItemId = a.Key.ProdItemId, DisplayName = a.Key.DisplayName, LoadingQty = a.Sum (acs => acs.LoadingQty) }).ToList ();

                //vijaymala added[]01-06-2018

                //Vijaymala [01-06-2018] changes the code to get isstockrequired item 

                List<TblProductItemTO> stockRequireProductItemList = TblProductItemBL.SelectProductItemListStockUpdateRequire (1);
                Boolean isStockRequie = false;

                if (listToCheck != null || listToCheck.Count > 0) {
                    Dictionary<Int32, Double> loadingQtyDCT = new Dictionary<int, double> ();

                    loadingQtyDCT = BL.TblLoadingSlipExtBL.SelectLoadingQuotaWiseApprovedLoadingQtyDCT (loadingQuotaIds, conn, tran);
                    Boolean isAllowed = true;
                    String reason = "Not Enough Quota For Following Items" + Environment.NewLine;
                    for (int i = 0; i < listToCheck.Count; i++) {
                        //TblLoadingSlipExtTO tblLoadingSlipExtTO = listToCheck[i];

                        //Vijaymala [04-06-2018] changes the code to get loading quota and display name for regular as well as isstockrequired item 

                        if (listToCheck[i].ProdItemId > 0) {
                            isStockRequie = stockRequireProductItemList.Where (ele => ele.IdProdItem == listToCheck[i].ProdItemId).
                            Select (x => x.IsStockRequire == 1).FirstOrDefault ();
                        } else {
                            isStockRequie = true;
                        }

                        if (isStockRequie) {
                            var loadingQuotaDeclarationTO = loadingQuotaDeclarationTOList.Where (l => l.ProdCatId == listToCheck[i].ProdCatId &&
                                l.ProdSpecId == listToCheck[i].ProdSpecId &&
                                l.MaterialId == listToCheck[i].MaterialId &&
                                l.ProdItemId == listToCheck[i].ProdItemId).FirstOrDefault ();

                            if (loadingQuotaDeclarationTO.IsActive == 0) {
                                if (isAllowed) {
                                    isAllowed = false;
                                }
                                if (listToCheck[i].ProdItemId > 0) {
                                    reason += listToCheck[i].DisplayName + " " + " R.Q. :" + listToCheck[i].LoadingQty + " has inactive loading quota" + Environment.NewLine;
                                } else {
                                    reason += listToCheck[i].MaterialDesc + " " + listToCheck[i].ProdCatDesc + "-" + listToCheck[i].ProdSpecDesc + " R.Q. :" + listToCheck[i].LoadingQty + " has inactive loading quota" + Environment.NewLine;
                                }

                            }

                            Double approvedLoadingQty = 0;
                            Double transferedQty = loadingQuotaDeclarationTO.TransferedQuota;
                            Double totalAvailableQty = loadingQuotaDeclarationTO.AllocQuota + loadingQuotaDeclarationTO.ReceivedQuota;
                            if (loadingQtyDCT != null && loadingQtyDCT.ContainsKey (loadingQuotaDeclarationTO.IdLoadingQuota))
                                approvedLoadingQty = loadingQtyDCT[loadingQuotaDeclarationTO.IdLoadingQuota];

                            Double pendingQty = totalAvailableQty - transferedQty - approvedLoadingQty;

                            if (listToCheck[i].LoadingQty > pendingQty) {
                                if (isAllowed) {
                                    isAllowed = false;
                                }
                                //Vijaymala [04-06-2018] changes the code to set reason for regular as well as isstockrequired item 

                                if (listToCheck[i].ProdItemId > 0) {
                                    reason += listToCheck[i].DisplayName + " " + " R.Q. :" + listToCheck[i].LoadingQty + " AND A.Q. :" + pendingQty + Environment.NewLine;
                                } else {
                                    reason += listToCheck[i].MaterialDesc + " " + listToCheck[i].ProdCatDesc + "-" + listToCheck[i].ProdSpecDesc + " R.Q. :" + listToCheck[i].LoadingQty + " AND A.Q. :" + pendingQty + Environment.NewLine;
                                }
                            }
                        }
                    }

                    if (!isAllowed) {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Loading Slip Can Not Be Approve";
                        resultMessage.Tag = reason;
                        return resultMessage;
                    }
                }

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Loading Slip Can Be Approve";
                resultMessage.Tag = tblLoadingSlipExtTOList;
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Text = "Loading Slip Can Not Be Approve";
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        public static ResultMessage updateLaodingToCallFlag (TblLoadingTO tblLoadingTO) {

            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_SYTEM_ADMIN_USER_ID, conn, tran);
                Int32 sysAdminUserId = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                DateTime cancellationDateTime = DateTime.MinValue;
                tran.Commit ();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod RestorePreviousStatusForLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        /// <summary>
        /// GJ@20171107 : check the Vehicle is complete the all material weight
        /// </summary>
        /// <param name="tblLoadingTO"></param>
        /// <returns></returns>
        public static ResultMessage IsVehicleWaitingForGross (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {

            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            List<TblLoadingTO> loadingToList = new List<TblLoadingTO> ();

            try {

                loadingToList = TblLoadingDAO.SelectAllLoadingListByVehicleNo (tblLoadingTO.VehicleNo, false, 0, conn, tran);
                if (loadingToList != null && loadingToList.Count > 0) {
                    loadingToList.OrderByDescending (p => p.IdLoading);
                    if (loadingToList[0].IdLoading != tblLoadingTO.IdLoading) {
                        resultMessage.DefaultBehaviour ("Not able to Remove the Allow one more Loading.");
                        return resultMessage;
                    }
                    TblLoadingTO eleLoadingTo = SelectLoadingTOWithDetails (loadingToList[0].IdLoading);
                    for (int j = 0; j < eleLoadingTo.LoadingSlipList.Count; j++) {
                        TblLoadingSlipTO eleLoadingslipTo = eleLoadingTo.LoadingSlipList[j];
                        for (int k = 0; k < eleLoadingslipTo.LoadingSlipExtTOList.Count; k++) {
                            if (eleLoadingslipTo.LoadingSlipExtTOList[k].WeightMeasureId == 0) {
                                resultMessage.DefaultBehaviour ("Weight not loaded for all material");
                                return resultMessage;
                            }
                        }
                    }

                } else {
                    resultMessage.DefaultBehaviour ("Loading Slip List found Null");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod IsVehicleWaitingForGross";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }

        }

        /// <summary>
        /// GJ@20171107 : Get the Last Weighing weight measurement and submit as gross weight
        /// </summary>
        /// <param name="idLoading"></param>
        /// <returns></returns>
        /// 
        public static TblWeighingMeasuresTO getWeighingGrossTo (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {

            try {
                List<TblWeighingMeasuresTO> weighingMeasuresToList = new List<TblWeighingMeasuresTO> ();
                // TblWeighingMeasuresTO tblWeighingMeasureTo = new TblWeighingMeasuresTO();
                weighingMeasuresToList = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId (tblLoadingTO.IdLoading, conn, tran);
                if (weighingMeasuresToList.Count > 0) {
                    weighingMeasuresToList = weighingMeasuresToList.OrderByDescending (p => p.CreatedOn).ToList ();
                    return weighingMeasuresToList[0];
                } else {
                    return null;
                }

            } catch (Exception ex) {
                return null;
            }
        }

        /// <summary>
        /// GJ@20171107 : Remove Allow one more Loading generation flag if required
        /// </summary>
        /// <param name="idLoading"></param>
        /// <returns></returns>
        /// 
        public static ResultMessage removeIsAllowOneMoreLoading (TblLoadingTO tblLoadingTO, int loginUserId) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                resultMessage = BL.TblLoadingBL.IsVehicleWaitingForGross (tblLoadingTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information) {
                    tran.Rollback ();
                    return resultMessage;
                }
                //Insert the Weighing measure Gross To
                TblWeighingMeasuresTO weighingMeasureTo = new TblWeighingMeasuresTO ();
                weighingMeasureTo = getWeighingGrossTo (tblLoadingTO, conn, tran);
                if (weighingMeasureTo == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Last weighing weight not found againest selected Loading");
                    return resultMessage;
                }
                weighingMeasureTo.IdWeightMeasure = 0;
                weighingMeasureTo.CreatedOn = Constants.ServerDateTime;
                weighingMeasureTo.UpdatedOn = Constants.ServerDateTime;
                weighingMeasureTo.WeightMeasurTypeId = (int) Constants.TransMeasureTypeE.GROSS_WEIGHT;

                #region 1. Save the Weighing Machine Mesurement 
                result = DAL.TblWeighingMeasuresDAO.InsertTblWeighingMeasures (weighingMeasureTo, conn, tran);
                if (result < 0) {
                    tran.Rollback ();
                    resultMessage.Text = "";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                #endregion

                //Updating Loading Slip flag status
                tblLoadingTO.UpdatedBy = Convert.ToInt32 (loginUserId);
                tblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                //result = BL.TblLoadingBL.UpdateTblLoading(tblLoadingTO);

                String wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE;
                if (tblLoadingTO.IsInternalCnf == 1)
                    wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE_INTERNAL_TRANSFER;
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(wtScaleConfigStr, conn, tran);
                //TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);
                if (tblConfigParamsTO != null) {
                    if (tblConfigParamsTO.ConfigParamVal == "1") {
                        tblLoadingTO.StatusId = (int) Constants.TranStatusE.LOADING_COMPLETED;
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_COMPLETED;
                        tblLoadingTO.StatusReason = "Loading Completed";
                    }
                }

                resultMessage = BL.TblLoadingBL.UpdateDeliverySlipConfirmations (tblLoadingTO, conn, tran);

                if (resultMessage.MessageType == ResultMessageE.Information) {
                    tran.Commit ();
                    resultMessage.DefaultSuccessBehaviour ();
                    return resultMessage;
                } else {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While UpdateTblLoading");
                    return resultMessage;
                }

            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod removeIsAllowOneMoreLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }
        #endregion

        #region Deletion
        public static int DeleteTblLoading (Int32 idLoading) {
            return TblLoadingDAO.DeleteTblLoading (idLoading);
        }

        public static int DeleteTblLoading (Int32 idLoading, SqlConnection conn, SqlTransaction tran) {
            return TblLoadingDAO.DeleteTblLoading (idLoading, conn, tran);
        }

        #endregion

        #region Methods

        // Vaibhav [30-Jan-2018] Added to update entity range for loading and loadingslip count.
        private static TblEntityRangeTO SelectEntityRangeForLoadingCount (string entityName, SqlConnection conn, SqlTransaction tran) {
            try {
                DimFinYearTO curFinYearTO = DimensionBL.GetCurrentFinancialYear (Constants.ServerDateTime, conn, tran);
                if (curFinYearTO == null) {
                    return null;
                }

                TblEntityRangeTO entityRangeTO = BL.TblEntityRangeBL.SelectTblEntityRangeTOByEntityName (entityName, curFinYearTO.IdFinYear, conn, tran);
                if (entityRangeTO == null) {
                    return null;
                }

                if (Constants.ServerDateTime.Date != entityRangeTO.CreatedOn.Date) {
                    entityRangeTO.CreatedOn = Constants.ServerDateTime;
                    entityRangeTO.EntityPrevValue = 1;

                    int result = BL.TblEntityRangeBL.UpdateTblEntityRange (entityRangeTO, conn, tran);
                    if (result != 1) {
                        return null;
                    }
                }

                return entityRangeTO;
            } catch (Exception ex) {
                ex.Message.ToString ();
                return null;
            }
        }

        public static TblEntityRangeTO SelectEntityRangeForLoadingCount (string entityName) {
            try {
                DimFinYearTO curFinYearTO = DimensionBL.GetCurrentFinancialYear (Constants.ServerDateTime);
                if (curFinYearTO == null) {
                    return null;
                }
                TblEntityRangeTO entityRangeTO = BL.TblEntityRangeBL.SelectTblEntityRangeTOByEntityName (entityName, curFinYearTO.IdFinYear);
                if (entityRangeTO == null) {
                    return null;
                }

                if (Constants.ServerDateTime.Date != entityRangeTO.CreatedOn.Date) {
                    entityRangeTO.CreatedOn = Constants.ServerDateTime;
                    entityRangeTO.EntityPrevValue = 1;

                    int result = BL.TblEntityRangeBL.UpdateTblEntityRange (entityRangeTO);
                    if (result != 1) {
                        return null;
                    }
                }

                return entityRangeTO;
            } catch (Exception ex) {
                ex.Message.ToString ();
                return null;
            }
        }

        public static ResultMessage RightDataFromIotToDB (Int32 loadingId, TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                Int32 cofigId = Constants.getweightSourceConfigTO ();
                if (cofigId == (Int32) Constants.WeighingDataSourceE.IoT || cofigId == (Int32) Constants.WeighingDataSourceE.BOTH) {
                    if (loadingId == 0) {
                        throw new Exception ("loadingId == 0");
                    }

                    //TblLoadingTO tblLoadingTO = SelectLoadingTOWithDetails(loadingId, conn, tran);
                    if (tblLoadingTO == null) {
                        throw new Exception ("tblLoadingTO == null");
                    }
                    if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0) {
                        throw new Exception ("tblLoadingTO.LoadingSlipList == 0");
                    }

                    //Move Confirm Data.
                    //tblLoadingTO.LoadingSlipList = tblLoadingTO.LoadingSlipList.Where(w => w.IsConfirmed == 1).ToList();
                    List<TblLoadingSlipTO> loadingSlipListConfirm = tblLoadingTO.LoadingSlipList.Where (w => w.IsConfirmed == 1).ToList ();

                    if (loadingSlipListConfirm.Count > 0) {

                        if (String.IsNullOrEmpty (tblLoadingTO.VehicleNo) || tblLoadingTO.TransporterOrgId == 0) {
                            throw new Exception ("tblLoadingTO Found NULL");
                        }

                        //Write DATA

                        Int32 result = UpdateTblLoading (tblLoadingTO, conn, tran);
                        if (result != 1) {
                            throw new Exception ("Error While updating Loading status for loadingId - " + tblLoadingTO.IdLoading);
                        }

                        if (tblLoadingTO.LoadingStatusHistoryTOList != null && tblLoadingTO.LoadingStatusHistoryTOList.Count > 0) {
                            for (int t = 0; t < tblLoadingTO.LoadingStatusHistoryTOList.Count; t++) {
                                TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = tblLoadingTO.LoadingStatusHistoryTOList[t];
                                tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                                //tblLoadingStatusHistoryTO.StatusDate = CommonDAO.SelectServerDateTime();
                                tblLoadingStatusHistoryTO.CreatedOn = tblLoadingStatusHistoryTO.StatusDate;
                                result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory (tblLoadingStatusHistoryTO, conn, tran);
                                if (result != 1) {
                                    throw new Exception ("Error While inserting status history record  for index " + t + " against loadingId - " + tblLoadingTO.IdLoading);

                                }

                            }
                        }

                        for (int j = 0; j < loadingSlipListConfirm.Count; j++) {
                            TblLoadingSlipTO tblLoadingSlipTO = loadingSlipListConfirm[j];

                            result = TblLoadingSlipBL.UpdateTblLoadingSlip (tblLoadingSlipTO, conn, tran);
                            if (result != 1) {
                                throw new Exception ("Error While updating LoadingSlip status For loadingslipId - " + tblLoadingSlipTO.IdLoadingSlip);
                            }

                            for (int k = 0; k < tblLoadingSlipTO.LoadingSlipExtTOList.Count; k++) {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[k];

                                if (tblLoadingSlipExtTO.LoadedWeight <= 0) {
                                    throw new Exception ("Loading Wt found zero for ext id IdLoadingSlipExt - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                                }

                                Int32 tempResult = TblLoadingSlipExtBL.UpdateTblLoadingSlipExt (tblLoadingSlipExtTO, conn, tran);
                                if (tempResult != 1) {
                                    throw new Exception ("Error While updating LoadingSlip Ext status for Ext Id - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                                }
                            }

                        }
                    }

                }

                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "");
                return resultMessage;
            }
        }

        public static ResultMessage MarkDeliverAndRemoveModBusRefs (Int32 loadingId) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                resultMessage = MarkDeliverAndRemoveModBusRefs (loadingId, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                    return resultMessage;
                }

                tran.Commit ();
                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "");
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }
        public static ResultMessage MarkDeliverAndRemoveModBusRefs (Int32 loadingId, SqlConnection conn, SqlTransaction tran) {

            DateTime serverDate = Constants.ServerDateTime;

            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                Int32 cofigId = Constants.getweightSourceConfigTO ();
                if (cofigId == (Int32) Constants.WeighingDataSourceE.IoT || cofigId == (Int32) Constants.WeighingDataSourceE.BOTH) {
                    if (loadingId == 0) {
                        throw new Exception ("loadingId == 0");
                    }
                    TblLoadingTO tblLoadingTO = SelectTblLoadingTO (loadingId, conn, tran);
                    tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (tblLoadingTO.IdLoading);
                    //TblLoadingTO tblLoadingTO = SelectLoadingTOWithDetails(loadingId, conn, tran);
                    if (tblLoadingTO == null) {
                        throw new Exception ("tblLoadingTO == null");
                    }
                    if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0) {
                        throw new Exception ("tblLoadingTO.LoadingSlipList == 0");
                    }
                    string loadingids = String.Join (",", tblLoadingTO.LoadingSlipList.Select (w => w.IdLoadingSlip).ToArray ());
                    Double TareWeight = DAL.TblInvoiceDAO.GetTareWeightFromInvoice (loadingids, conn, tran); //tempExtList.Min(m => m.CalcTareWeight);
                    Double invoiceTareWt = 0;

                    for (int p = 0; p < tblLoadingTO.LoadingSlipList.Count; p++) {
                        TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[p];

                        if (tblLoadingSlipTO.IsConfirmed == 1) {
                            for (int s = 0; s < tblLoadingSlipTO.LoadingSlipExtTOList.Count; s++) {
                                tblLoadingSlipTO.LoadingSlipExtTOList[s].CalcTareWeight = TareWeight;
                                TareWeight += tblLoadingSlipTO.LoadingSlipExtTOList[s].LoadedWeight;
                            }

                            invoiceTareWt = tblLoadingSlipTO.LoadingSlipExtTOList.Min (m => m.CalcTareWeight);

                        }

                        List<TblInvoiceTO> tblInvoiceTOList = TblInvoiceBL.SelectInvoiceListFromLoadingSlipId (tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                        if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0) {
                            for (int k = 0; k < tblInvoiceTOList.Count; k++) {
                                TblInvoiceTO tblInvoiceTO = tblInvoiceTOList[k];

                                //RemoveCommercialDataFromInvoice(conn, tran, tblInvoiceTO);
                                if (tblInvoiceTO.IsConfirmed == 0) {
                                    resultMessage = TblInvoiceBL.DeleteTblInvoiceDetails (tblInvoiceTO, conn, tran);
                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                                        throw new Exception ("Error While Deleting Invoice for idInvoice - " + tblInvoiceTO.IdInvoice);
                                    }
                                } else {
                                    tblInvoiceTO.DeliveredOn = serverDate;

                                    tblInvoiceTO.TareWeight = invoiceTareWt;
                                    tblInvoiceTO.GrossWeight = tblInvoiceTO.TareWeight + tblInvoiceTO.NetWeight;

                                    Int32 result = TblInvoiceDAO.UpdateTblInvoice (tblInvoiceTO, conn, tran);
                                    if (result == -1) {
                                        throw new Exception ("Error While Deleting Invoice for idInvoice - " + tblInvoiceTO.IdInvoice);
                                    }
                                }
                            }
                        }

                        if (tblLoadingSlipTO.IsConfirmed == 0) {
                            //Delete loadingSlip
                            Double sum = tblInvoiceTOList.Where (w => w.IsConfirmed == 0).ToList ().Count;

                            if (sum == tblInvoiceTOList.Count) {
                                resultMessage = TblLoadingSlipBL.DeleteLoadingSlipWithDetails (tblLoadingTO, tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                                    throw new Exception ("Error While Deleting Invoice for idInvoice - " + tblLoadingSlipTO.IdLoadingSlip);
                                }
                                tblLoadingTO.TotalLoadingQty = tblLoadingTO.TotalLoadingQty - tblLoadingSlipTO.LoadingSlipExtTOList.Sum (s => s.LoadingQty);
                                tblLoadingTO.NoOfDeliveries = tblLoadingTO.NoOfDeliveries - 1;
                                tblLoadingTO.LoadingSlipList.Remove (tblLoadingSlipTO);
                                p--;
                            }
                        }
                    }

                    //tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);
                    if (tblLoadingTO.LoadingSlipList == null) {
                        throw new Exception ("Error While fetching Loading Slip's against loadingId - " + loadingId);
                    }
                    if (tblLoadingTO.LoadingSlipList.Count == 0) {
                        //Int32
                        resultMessage = TblLoadingBL.DeleteLoadingData (loadingId, conn, tran);
                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                            throw new Exception ("Error While deleting Loading data for loadingId - " + loadingId);
                        }
                    }
                    int tempmodbusId = tblLoadingTO.ModbusRefId;
                    if (tblLoadingTO.LoadingSlipList.Count > 0) {
                        //Write DATA
                        tblLoadingTO.StatusId = (int) Constants.TranStatusE.LOADING_DELIVERED;
                        tblLoadingTO.StatusReason = "Delivered";
                        tblLoadingTO.ModbusRefId = 0;
                        TblLoadingStatusHistoryTO tblLodingStatusHistory = new TblLoadingStatusHistoryTO ();
                        tblLodingStatusHistory.LoadingId = tblLoadingTO.IdLoading;
                        tblLodingStatusHistory.StatusId = tblLoadingTO.StatusId;
                        tblLodingStatusHistory.CreatedBy = 1;
                        tblLodingStatusHistory.CreatedOn = Constants.ServerDateTime;
                        tblLodingStatusHistory.StatusDate = tblLodingStatusHistory.CreatedOn;
                        tblLodingStatusHistory.StatusRemark = tblLodingStatusHistory.StatusRemark;
                        int res = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory (tblLodingStatusHistory, conn, tran);

                        if (res != 1) {
                            throw new Exception ("Error While inserting InsertTblLoadingStatusHistory for loadingId - " + tblLoadingTO.IdLoading);
                        }
                        Int32 result = UpdateTblLoading (tblLoadingTO, conn, tran);
                        if (result != 1) {
                            throw new Exception ("Error While updating Loading status for loadingId - " + tblLoadingTO.IdLoading);
                        }

                        for (int j = 0; j < tblLoadingTO.LoadingSlipList.Count; j++) {
                            TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[j];

                            tblLoadingSlipTO.StatusId = tblLoadingTO.StatusId;
                            tblLoadingSlipTO.StatusReason = "Delivered";

                            result = TblLoadingSlipBL.UpdateTblLoadingSlip (tblLoadingSlipTO, conn, tran);
                            if (result != 1) {
                                throw new Exception ("Error While updating LoadingSlip status For loadingslipId - " + tblLoadingSlipTO.IdLoadingSlip);
                            }

                            for (int k = 0; k < tblLoadingSlipTO.LoadingSlipExtTOList.Count; k++) {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[k];

                                tblLoadingSlipExtTO.ModbusRefId = 0;

                                Int32 tempResult = TblLoadingSlipExtBL.UpdateTblLoadingSlipExt (tblLoadingSlipExtTO, conn, tran);
                                if (tempResult != 1) {
                                    throw new Exception ("Error While updating LoadingSlip Ext status for Ext Id - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                                }
                            }

                        }
                    }
                    tblLoadingTO.ModbusRefId = tempmodbusId;
                    int result1 = RemoveDateFromGateAndWeightIOT (tblLoadingTO);
                    if (result1 != 1) {
                        throw new Exception ("Error While RemoveDateFromGateAndWeightIOT ");
                    }
                }

                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "");
                return resultMessage;
            }
        }

        public static ResultMessage DeleteLoadingData (Int32 loadingId, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();

            try {
                #region Delete Slip

                Int32 result = 0;
                List<TblLoadingStatusHistoryTO> tblLoadingStatusHistoryTOList = new List<TblLoadingStatusHistoryTO> ();
                tblLoadingStatusHistoryTOList = BL.TblLoadingStatusHistoryBL.SelectAllTblLoadingStatusHistoryList (loadingId, conn, tran);
                if (tblLoadingStatusHistoryTOList == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("tblLoadingStatusHistoryTOList found null");
                    return resultMessage;
                }

                foreach (var tblLoadingStatusHistoryTO in tblLoadingStatusHistoryTOList) {

                    result = BL.TblLoadingStatusHistoryBL.DeleteTblLoadingStatusHistory (tblLoadingStatusHistoryTO.IdLoadingHistory, conn, tran);
                    if (result != 1) {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ("Error while delete loadindingSlip status history");
                        return resultMessage;
                    }

                }

                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO> ();
                tblWeighingMeasuresTOList = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId (loadingId, conn, tran);
                if (tblWeighingMeasuresTOList == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("tblWeighingMeasuresTOList found null");
                    return resultMessage;
                }

                foreach (var tblWeighingMeasuresTO in tblWeighingMeasuresTOList) {
                    if (tblWeighingMeasuresTO.IdWeightMeasure > 0) {
                        result = BL.TblWeighingMeasuresBL.DeleteTblWeighingMeasures (tblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("Error While Deleting tblWeighingMeasuresTOList ");
                            return resultMessage;
                        }
                    }
                }

                result = BL.TblLoadingBL.DeleteTblLoading (loadingId, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While Deleting Loading Details");
                    return resultMessage;
                }

                #endregion

                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;

            } catch (Exception ex) {
                resultMessage = new ResultMessage ();
                resultMessage.DefaultExceptionBehaviour (ex, "DeleteLoadingSlipWithDetails");
                return resultMessage;
            }
        }

        private static void RemoveCommercialDataFromInvoice (SqlConnection conn, SqlTransaction tran, TblInvoiceTO tblInvoiceTO) {
            if (tblInvoiceTO.IsConfirmed == 0) {

                tblInvoiceTO.InvoiceNo = "";
                tblInvoiceTO.ElectronicRefNo = "";
                tblInvoiceTO.VehicleNo = "";
                tblInvoiceTO.LrNumber = "";
                tblInvoiceTO.BasicAmt = 0;
                tblInvoiceTO.DiscountAmt = 0;
                tblInvoiceTO.TaxableAmt = 0;
                tblInvoiceTO.CgstAmt = 0;
                tblInvoiceTO.SgstAmt = 0;
                tblInvoiceTO.IgstAmt = 0;
                tblInvoiceTO.FreightAmt = 0;

                tblInvoiceTO.FreightPct = 0;

                tblInvoiceTO.RoundOffAmt = 0;

                tblInvoiceTO.GrandTotal = 0;

                tblInvoiceTO.TareWeight = 0;

                tblInvoiceTO.NetWeight = 0;

                tblInvoiceTO.GrossWeight = 0;

                tblInvoiceTO.ExpenseAmt = 0;
                tblInvoiceTO.OtherAmt = 0;
                //tblInvoiceTO.DeliveryLocation= "";

                Int32 result = TblInvoiceBL.UpdateTblInvoice (tblInvoiceTO, conn, tran);
                if (result != 1) {
                    throw new Exception ("Error While updating tblInvoiceTO for invoiceId - " + tblInvoiceTO.IdInvoice);
                }

                List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList (tblInvoiceTO.IdInvoice, conn, tran);

                if (tblInvoiceItemDetailsTOList != null && tblInvoiceItemDetailsTOList.Count > 0) {
                    for (int t = 0; t < tblInvoiceItemDetailsTOList.Count; t++) {
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoiceItemDetailsTOList[t];

                        tblInvoiceItemDetailsTO.Bundles = 0;
                        tblInvoiceItemDetailsTO.InvoiceQty = 0;
                        tblInvoiceItemDetailsTO.Rate = 0;
                        tblInvoiceItemDetailsTO.BasicTotal = 0;
                        tblInvoiceItemDetailsTO.TaxableAmt = 0;
                        tblInvoiceItemDetailsTO.GrandTotal = 0;
                        tblInvoiceItemDetailsTO.CdStructure = 0;
                        tblInvoiceItemDetailsTO.CdAmt = 0;
                        tblInvoiceItemDetailsTO.TaxPct = 0;
                        tblInvoiceItemDetailsTO.CdAmt = 0;

                        result = TblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails (tblInvoiceItemDetailsTO, conn, tran);
                        if (result != 1) {
                            throw new Exception ("Error While updating tblInvoiceItemDetailsTO for IdInvoiceItem - " + tblInvoiceItemDetailsTO.IdInvoiceItem);
                        }

                        tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList = BL.TblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList (tblInvoiceItemDetailsTO.IdInvoiceItem, conn, tran);

                        if (tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList != null && tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count > 0) {
                            for (int u = 0; u < tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count; u++) {
                                tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[u].TaxAmt = 0;
                                tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[u].TaxableAmt = 0;
                                result = TblInvoiceItemTaxDtlsBL.UpdateTblInvoiceItemTaxDtls (tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[u], conn, tran);
                                if (result != 1) {
                                    throw new Exception ("Error While updating InvoiceItemTaxDtlsTO for IdInvItemTaxDtl - " + tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[u].IdInvItemTaxDtl);
                                }
                            }
                        }

                    }
                }
            }
        }

        public static ResultMessage RemoveDatFromIotDevice () {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            String statusStr = Convert.ToString ((Int32) Constants.TranStatusE.LOADING_DELIVERED);

            List<TblGateTO> tblGateTOList = TblGateBL.SelectAllTblGateList (Constants.ActiveSelectionTypeE.Active);

            for (int g = 0; g < tblGateTOList.Count; g++) {

                TblGateTO tblGateTO = tblGateTOList[g];

                GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId (statusStr, tblGateTO);
                if (gateIoTResult != null && gateIoTResult.Data != null) {
                    for (int i = 0; i < gateIoTResult.Data.Count; i++) {
                        if (gateIoTResult.Data[i] != null) {
                            Int32 modBusLoadingRefId = Convert.ToInt32 (gateIoTResult.Data[i][(int) IoTConstants.GateIoTColE.LoadingId]);

                            TblLoadingTO tblLoadingTO = SelectTblLoadingTOByModBusRefId(modBusLoadingRefId);
                            //vipul[18/4/19] check allowed to remove or not
                            if (tblLoadingTO == null || tblLoadingTO.IsDBup == 0) {
                                continue;
                            }
                            //end
                            if (tblLoadingTO != null) {
                                tblLoadingTO.ModbusRefId = modBusLoadingRefId;
                                resultMessage = TblLoadingBL.MarkDeliverAndRemoveModBusRefs (tblLoadingTO.IdLoading);

                            }
                        }
                    }
                }
            }
            Startup.AvailableModbusRefList = DAL.TblLoadingDAO.GeModRefMaxData ();
            resultMessage.DefaultSuccessBehaviour ();
            return resultMessage;
        }

        //public static void PingPongExecution()
        //{
        //    List<TblGateTO> tblGateTOList = TblGateBL.SelectAllTblGateList(Constants.ActiveSelectionTypeE.Active);
        //    for (int g = 0; g < tblGateTOList.Count; g++)
        //    {
        //        TblGateTO tblGateTO = tblGateTOList[g];
        //        Thread thread = new Thread(delegate ()
        //        {
        //            IoT.IotCommunication.PingPongExecution(tblGateTO);
        //        });
        //        thread.Start();
        //    }
        //}

        private static int RemoveDateFromGateAndWeightIOT (TblLoadingTO tblLoadingTO) {
            //Addes by kiran for retry 3 times to delete All Data
            int cnt = 0;
            GateIoTResult result = new GateIoTResult ();
            while (cnt < 3) {
                result = IoT.GateCommunication.DeleteSingleLoadingFromGateIoT (tblLoadingTO);
                if (result.Code == 1) {
                    break;
                }
                Thread.Sleep (200);
                cnt++;
            }
            if (result.Code != 1) {
                return 0;
            }
            int cnt2 = 0;
            NodeJsResult nodeJsResult = new NodeJsResult ();
            while (cnt2 < 3) {
                nodeJsResult = IotCommunication.DeleteSingleLoadingFromWeightIoTByModBusRefId (tblLoadingTO);
                if (nodeJsResult.Code == 1) {
                    break;
                }
                Thread.Sleep (200);
                cnt2++;
            }
            if (nodeJsResult.Code != 1) {
                return 0;
            }
            return 1;
        }
        //public static ResultMessage RightDataFromIotToDB()
        //{ 
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    String statusStr = Convert.ToString((Int32)Constants.TranStatusE.LOADING_DELIVERED);
        //    GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId(statusStr);

        //    if (gateIoTResult != null && gateIoTResult.Data != null)
        //    {
        //        for (int i = 0; i < gateIoTResult.Data.Count; i++)
        //        {
        //            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
        //            SqlTransaction tran = null;
        //            resultMessage.MessageType = ResultMessageE.None;
        //            resultMessage.Text = "Not Entered In The Loop";
        //            try
        //            {
        //                conn.Open();
        //                tran = conn.BeginTransaction();

        //                Int32 loadingId = 0;

        //                //Get DATA
        //                TblLoadingTO tblLoadingTO = SelectLoadingTOWithDetails(loadingId);

        //                //Write DATA

        //                Int32 result = UpdateTblLoading(tblLoadingTO, conn, tran);
        //                if (result != 1)
        //                {
        //                    throw new Exception("Error While updating Loading status for loadingId - " + tblLoadingTO.IdLoading);
        //                }

        //                if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
        //                {
        //                    throw new Exception("tblLoadingTO.LoadingSlipList == 0");
        //                }

        //                for (int j = 0; j < tblLoadingTO.LoadingSlipList.Count; j++)
        //                {
        //                    TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[j];

        //                    result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
        //                    if (result != 1)
        //                    {
        //                        throw new Exception("Error While updating LoadingSlip status For loadingslipId - " + tblLoadingSlipTO.IdLoadingSlip);
        //                    }

        //                    for (int k = 0; k < tblLoadingSlipTO.LoadingSlipExtTOList.Count; k++)
        //                    {

        //                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[k];
        //                        Int32 tempResult = TblLoadingSlipExtBL.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
        //                        if (tempResult != 1)
        //                        {
        //                            throw new Exception("Error While updating LoadingSlip Ext status for Ext Id - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
        //                        }
        //                    }

        //                }

        //                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
        //                {
        //                    return resultMessage;
        //                }

        //                //Clear IOT data

        //                tran.Commit();
        //                resultMessage.DefaultSuccessBehaviour();
        //                return resultMessage;
        //            }
        //            catch (Exception ex)
        //            {
        //                resultMessage.DefaultExceptionBehaviour(ex, "");
        //                return resultMessage;
        //            }
        //            finally
        //            {
        //                conn.Close();
        //            }

        //        }

        //    }

        //    return resultMessage;
        //}


        public static ResultMessage CorrectTareWeightAgainstVehicle()
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                List<TblLoadingTO> tblLoadingToList = SelectAllTblLoadingList();
                DateTime fromDate = new DateTime(2021, 04, 22);

                tblLoadingToList = tblLoadingToList.Where(w => w.CreatedOn >= fromDate).ToList();

                //tblLoadingToList = tblLoadingToList.Where(w => w.IdLoading == 18465).ToList();

                tblLoadingToList = tblLoadingToList.Where(w => w.StatusId == (Int32)Constants.TranStatusE.LOADING_DELIVERED).ToList();

                tblLoadingToList = tblLoadingToList.OrderBy(o => o.IdLoading).ToList();

                for (int i = 0; i < tblLoadingToList.Count; i++)
                {
                    Double vehicleTareWt = 0;
                    Double vehicleTareWt2 = 0;


                    TblLoadingTO tblLoadingTO = tblLoadingToList[i];
                    List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(tblLoadingTO.IdLoading);

                    if (tblWeighingMeasuresTOList != null && tblWeighingMeasuresTOList.Count > 0)
                    {

                        tblWeighingMeasuresTOList = tblWeighingMeasuresTOList.Where(w => w.WeightMeasurTypeId == 1).ToList();

                        tblWeighingMeasuresTOList = tblWeighingMeasuresTOList.OrderByDescending(o => o.IdWeightMeasure).ToList();

                        TblWeighingMeasuresTO tareWtTO = tblWeighingMeasuresTOList.FirstOrDefault();
                        if (tareWtTO != null)
                        {
                            vehicleTareWt = tareWtTO.WeightMT;
                            vehicleTareWt2 = tareWtTO.WeightMT;
                        }
                    }

                    if (vehicleTareWt > 0)
                    {

                        List<TblLoadingSlipExtTO> AllTblLoadingSlipExtTO = new List<TblLoadingSlipExtTO>();

                        List<TblLoadingSlipTO> tblLoadingSlipTOList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);

                        tblLoadingSlipTOList.ForEach(f => AllTblLoadingSlipExtTO.AddRange(f.LoadingSlipExtTOList));

                        AllTblLoadingSlipExtTO = AllTblLoadingSlipExtTO.OrderBy(o => o.LoadingLayerid).ThenBy(p => p.WeighingSequenceNumber).ToList();

                        String loadingSlipIds = String.Join(',', tblLoadingSlipTOList.Select(s => s.IdLoadingSlip).ToList());

                        List<TblInvoiceTO> tblInvoiceTOList = TblInvoiceBL.SelectInvoiceListFromLoadingSlipIds(loadingSlipIds, conn, tran);


                        tblInvoiceTOList = tblInvoiceTOList.GroupBy(g => g.LoadingSlipId).Select(s => s.FirstOrDefault()).ToList();

                        tblInvoiceTOList = tblInvoiceTOList.OrderBy(o => o.TareWeight).ToList();

                        for (int j = 0; j < tblInvoiceTOList.Count; j++)
                        {
                            TblInvoiceTO tblInvoiceTO = tblInvoiceTOList[j];

                            tblInvoiceTO.TareWeight = vehicleTareWt;
                            tblInvoiceTO.GrossWeight = tblInvoiceTO.TareWeight + tblInvoiceTO.NetWeight;


                            int result = TblInvoiceBL.UpdateTblInvoiceFinalTareWt(tblInvoiceTO, conn, tran);
                            if (result != 1)
                            {

                            }

                            vehicleTareWt = tblInvoiceTO.GrossWeight;
                        }

                        for (int p = 0; p < AllTblLoadingSlipExtTO.Count; p++)
                        {
                            TblLoadingSlipExtTO tblLoadingSlipExtTO = AllTblLoadingSlipExtTO[p];
                            tblLoadingSlipExtTO.CalcTareWeight = vehicleTareWt2;

                            int result = TblLoadingSlipExtBL.UpdateFinalLoadingSlipExtCalTareWt(tblLoadingSlipExtTO, conn, tran);

                            if (result != 1)
                            {
                            }

                            vehicleTareWt2 = vehicleTareWt2 + tblLoadingSlipExtTO.LoadedWeight;
                        }


                    }

                }

                tran.Commit();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CorrectTareWeightAgainstVehicle");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region gettblgate

        public static List<TblGateTO> GetAllTblGate () {
            return TblLoadingDAO.SelectAllTblGateList ();
        }

        public static List<TblGateTO> GetAllTblGate (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (Startup.ConnectionString);
            // SqlTransaction tran = null;
            try {
                conn.Open ();
                //tran = conn.BeginTransaction();
                return TblLoadingDAO.SelectTblGateList (tblLoadingTO, conn);
            } catch (Exception ex) {

                throw;
            } finally {
                conn.Close ();
            }
        }
        #endregion
    }
}