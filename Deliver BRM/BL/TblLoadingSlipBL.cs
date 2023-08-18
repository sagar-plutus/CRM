using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;
using SalesTrackerAPI.IoT;
using static SalesTrackerAPI.BL.FinalBookingData;

namespace SalesTrackerAPI.BL
{
    public class TblLoadingSlipBL
    {
        #region Selection
       
        public static List<TblLoadingSlipTO> SelectAllTblLoadingSlipList()
        {
           return  TblLoadingSlipDAO.SelectAllTblLoadingSlip();
           
        }

        public static List<TblLoadingSlipTO> SelectAllLoadingSlipListWithDetails(Int32 loadingId)
        {
            try
            {
                List<TblLoadingSlipTO> list = TblLoadingSlipDAO.SelectAllTblLoadingSlip(loadingId);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TblLoadingSlipDtlTO = BL.TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(list[i].IdLoadingSlip);
                    list[i].LoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(list[i].IdLoadingSlip);
                    list[i].DeliveryAddressTOList = BL.TblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(list[i].IdLoadingSlip);
                }

                if (list != null && list.Count > 0)
                    list = list.OrderBy(o => o.LoadingSlipExtTOList[0].LoadingLayerid).ToList();

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<TblLoadingSlipTO> SelectAllLoadingSlipListWithDetails(Int32 loadingId,SqlConnection conn,SqlTransaction tran)
        {
            try
            {
                List<TblLoadingSlipTO> list = TblLoadingSlipDAO.SelectAllTblLoadingSlip(loadingId,conn,tran);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TblLoadingSlipDtlTO = BL.TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(list[i].IdLoadingSlip,conn,tran);
                    list[i].LoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(list[i].IdLoadingSlip,conn,tran);
                    list[i].DeliveryAddressTOList = BL.TblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(list[i].IdLoadingSlip,conn,tran);
                }

                if (list != null && list.Count > 0)
                    list = list.OrderBy(o => o.LoadingSlipExtTOList[0].LoadingLayerid).ToList();

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //GJ@20171002 : Get the Loading Slip details By Loading Slip id
        public static TblLoadingSlipTO SelectAllLoadingSlipWithDetails(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                TblLoadingSlipTO tblLoadingSlipTO = TblLoadingSlipDAO.SelectTblLoadingSlip(loadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    return null;
                }
                tblLoadingSlipTO.TblLoadingSlipDtlTO = BL.TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(loadingSlipId, conn, tran);
                tblLoadingSlipTO.LoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(loadingSlipId, conn, tran);
                tblLoadingSlipTO.DeliveryAddressTOList = BL.TblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(loadingSlipId, conn, tran);

                int iotConfigId = Startup.WeighingSrcConfig;
                if(iotConfigId==(int)Constants.WeighingDataSourceE.IoT && tblLoadingSlipTO.TranStatusE!=Constants.TranStatusE.LOADING_DELIVERED)
                {
                    IoT.IotCommunication.GetItemDataFromIotForGivenLoadingSlip(tblLoadingSlipTO);
                }

                return tblLoadingSlipTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static TblLoadingSlipTO SelectAllLoadingSlipWithDetailsForExtract(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                TblLoadingSlipTO tblLoadingSlipTO = TblLoadingSlipDAO.SelectTblLoadingSlip(loadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    return null;
                }
                tblLoadingSlipTO.TblLoadingSlipDtlTO = BL.TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(loadingSlipId, conn, tran);
                tblLoadingSlipTO.LoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(loadingSlipId, conn, tran);
                tblLoadingSlipTO.DeliveryAddressTOList = BL.TblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(loadingSlipId, conn, tran);
                //IoT.IotCommunication.GetItemDataFromIotForGivenLoadingSlipForExtract(tblLoadingSlipTO);
                return tblLoadingSlipTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static List<TblLoadingSlipTO> SelectAllLoadingSlipList(List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, string selectedOrgStr,int isFromBasicMode = 0)
        {
            var checkIotFlag = loadingStatusId;
            int configId = Startup.WeighingSrcConfig;
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                checkIotFlag = 0;
            }
            List<TblLoadingSlipTO> list = new List<TblLoadingSlipTO>();
            List<TblLoadingSlipTO> finalList = new List<TblLoadingSlipTO>();

            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            List<TblLoadingSlipTO> tblLoadingTOSlipList = TblLoadingSlipDAO.SelectAllTblLoadingSlipList(tblUserRoleTO, cnfId, loadingStatusId, fromDate, toDate, loadingTypeId, dealerId, selectedOrgStr,isFromBasicMode);
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                if (tblLoadingTOSlipList != null && tblLoadingTOSlipList.Count > 0)
                {
                    var deliverList = tblLoadingTOSlipList.Where(s => s.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED || s.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || s.TranStatusE == Constants.TranStatusE.LOADING_NOT_CONFIRM).ToList();
                    // var deliverList = tblLoadingTOList.Where(s => s.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED || s.TranStatusE == Constants.TranStatusE.LOADING_CANCEL).ToList();
                    string finalStatusId = IotCommunication.GetIotEncodedStatusIdsForGivenStatus(loadingStatusId.ToString());
                    list = TblLoadingBL.SetLoadingStatusData(finalStatusId.ToString(), true, configId, tblLoadingTOSlipList);
                    if (deliverList != null)
                        finalList.AddRange(deliverList);
                    if (list != null)
                        finalList.AddRange(list);
                }

                if (finalList != null && finalList.Count > 0)
                {
                    if (loadingStatusId > 0)
                    {
                        finalList = finalList.Where(w => w.StatusId == loadingStatusId).ToList();
                    }
                }
                return finalList;
            }
            else
            {
                return tblLoadingTOSlipList;
            }
            //return tblLoadingTOSlipList;
        }


        //Priyanka [08-05-2018] : Added for showing ORC report in loading slip.
        public static List<TblORCReportTO> SelectORCReportDetailsList(DateTime fromDate, DateTime toDate, Int32 flag, string selectOrgIdStr)
        {
            fromDate = Convert.ToDateTime(fromDate);
            toDate = Convert.ToDateTime(toDate);
            if (string.IsNullOrEmpty(selectOrgIdStr))
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    selectOrgIdStr = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }
            }
            return TblLoadingSlipDAO.SelectORCReportDetailsList(fromDate, toDate, flag, selectOrgIdStr);
        }

        public static TblLoadingSlipTO SelectTblLoadingSlipTO(Int32 idLoadingSlip)
        {
            return  TblLoadingSlipDAO.SelectTblLoadingSlip(idLoadingSlip);
        }

        public static Dictionary<int, string> SelectRegMobileNoDCTForLoadingDealers(String loadingIds, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDAO.SelectRegMobileNoDCTForLoadingDealers(loadingIds, conn, tran);
        }

        public static TblLoadingSlipTO SelectAllLoadingSlipWithDetails(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
                tblLoadingSlipTO = SelectAllLoadingSlipWithDetails(loadingSlipId, conn, tran);
                if(tblLoadingSlipTO == null)
                {
                    tran.Rollback();
                    return null;
                }
                return tblLoadingSlipTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static TblLoadingSlipTO SelectAllLoadingSlipWithDetailsForExtract(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
                tblLoadingSlipTO = SelectAllLoadingSlipWithDetailsForExtract(loadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    tran.Rollback();
                    return null;
                }
                return tblLoadingSlipTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        
        //Sudhir-Added For Support
        public static List<TblLoadingSlipTO> SelectAllLoadingListByVehicleNo(string vehicleNo)
        {
            return TblLoadingSlipDAO.SelectAllLoadingSlipListByVehicleNo(vehicleNo);
        }
        //Sudhir-Added For Support
        public static List<TblLoadingSlipTO> SelectLoadingTOWithDetailsByLoadingSlipIdForSupport(String loadingSlipNo)
        {
            return TblLoadingSlipDAO.SelectTblLoadingTOByLoadingSlipIdForSupport(loadingSlipNo);
        }

        //Sudhir[09-03-2018] Added for Selecting Loading Cycle List
        public static List<TblLoadingSlipTO> SelectAllTblLoadingSlipByDate(DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 cnfId)
        {
            try
            {
                List<TblLoadingSlipTO> tblLoadingSlipTOList = TblLoadingSlipDAO.SelectAllTblLoadingSlipByDate(fromDate, toDate, tblUserRoleTO,cnfId);

                return tblLoadingSlipTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Sudhir[09-03-2018] Added for Selecting Loading Cycle List
        public static List<TblLoadingSlipTO> SelectAllLoadingCycleList(DateTime startDate, DateTime endDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 vehicleStatus)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
                {
                    tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
                }
                List<TblLoadingSlipTO> loadingSlipTOList = SelectAllTblLoadingSlipByDate(startDate, endDate, tblUserRoleTO,cnfId);
                List<DimStatusTO> dimStatusTOList = DimStatusBL.SelectAllDimStatusList();
                int confiqId = Constants.getweightSourceConfigTO();
                foreach (TblLoadingSlipTO tblLoadingSlipTO in loadingSlipTOList)
                {
                    LoadingStatusDateTO loadingStatusDateTO = new LoadingStatusDateTO();
                    if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT) && tblLoadingSlipTO.StatusId!= (Int32)Constants.TranStatusE.LOADING_DELIVERED)
                    {
                        TblLoadingTO tblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTO.LoadingId);
                        tblLoadingSlipTO.LoadingStatusHistoryTOList = GetVehicalHistoryDataFromIoT(tblLoadingTO,tblLoadingSlipTO, dimStatusTOList);
                    }
                    else
                    {
                        tblLoadingSlipTO.LoadingStatusHistoryTOList = BL.TblLoadingStatusHistoryBL.SelectAllTblLoadingStatusHistoryList(tblLoadingSlipTO.LoadingId);
                    }
                    //tblLoadingSlipTO.LoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(tblLoadingSlipTO.IdLoadingSlip);
                    if (tblLoadingSlipTO.LoadingStatusHistoryTOList != null && tblLoadingSlipTO.LoadingStatusHistoryTOList.Count > 0)
                    {
                        for (int i = 0; i < tblLoadingSlipTO.LoadingStatusHistoryTOList.Count; i++)
                        {
                            if (tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING))
                            {
                                loadingStatusDateTO.VehicleReported = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.VehicleReportedStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.VehicleReported - tblLoadingSlipTO.CreatedOn;
                                loadingStatusDateTO.VehicleReportedMin = diff.ToString();
                            }
                            if (tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN))
                            {
                                loadingStatusDateTO.InstructedForIn = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.InstructedForInStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.InstructedForIn - loadingStatusDateTO.VehicleReported;
                                loadingStatusDateTO.InstructedForInMin = diff.ToString();
                            }
                            if (tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_GATE_IN))
                            {
                                loadingStatusDateTO.GateIn = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.GateInStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.GateIn - loadingStatusDateTO.InstructedForIn;
                                loadingStatusDateTO.GateInMin = diff.ToString();
                            }
                            if (tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED))
                            {
                                loadingStatusDateTO.GateOut = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.GateOutStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.GateOut - loadingStatusDateTO.LoadingCompleted;
                                loadingStatusDateTO.GateOutMin = diff.ToString();
                            }
                            if (tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_COMPLETED))
                            {
                                loadingStatusDateTO.LoadingCompleted = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate;
                                loadingStatusDateTO.LoadingCompletedStr = tblLoadingSlipTO.LoadingStatusHistoryTOList[i].StatusDate.ToString("dd/MM/yyyy HH:mm");
                                TimeSpan diff = loadingStatusDateTO.LoadingCompleted - loadingStatusDateTO.GateIn;
                                loadingStatusDateTO.LoadingCompletedMin = diff.ToString();
                            }
                        }
                    }
                    tblLoadingSlipTO.LoadingStatusDateTO = loadingStatusDateTO;
                }
                List<TblLoadingSlipTO> list = new List<TblLoadingSlipTO>();
                List<TblLoadingSlipTO> finalList = new List<TblLoadingSlipTO>();
                if (vehicleStatus==0) //For All
                {
                    list= loadingSlipTOList.Where(x => x.StatusId != Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)).ToList();
                    finalList.AddRange(list);
                    finalList.AddRange(loadingSlipTOList.Where(x => x.StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)).ToList());
                    loadingSlipTOList = finalList;
                }
                else if(vehicleStatus==1) //For Pending
                {
                    loadingSlipTOList = loadingSlipTOList.Where(x => x.StatusId != Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)).ToList();

                }
                else if (vehicleStatus == 2)//For Completed
                {
                    loadingSlipTOList = loadingSlipTOList.Where(x => x.StatusId == Convert.ToInt32(Constants.TranStatusE.LOADING_DELIVERED)).ToList();
                }

                //Priyanka [05-04-2019] : Added to get the distinct records from loading slip list by loading id.
                loadingSlipTOList = loadingSlipTOList.GroupBy(w => w.LoadingId).Select(s=>s.FirstOrDefault()).ToList();
                return loadingSlipTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTempLoading");
                return null;
            }
        }

        /// <summary>
        /// Kiran [14-12-2018] get vehical status history from IoT
        /// </summary>
        /// <returns></returns>
        public static List<TblLoadingStatusHistoryTO> GetVehicalHistoryDataFromIoT(TblLoadingTO tblLoadingTO, TblLoadingSlipTO loadingSlipTo, List<DimStatusTO> statuslist)
        {
            List<TblLoadingStatusHistoryTO> statusHistoryList = new List<TblLoadingStatusHistoryTO>();
            if (tblLoadingTO != null)
            {
              //  GateIoTResult gateIoTResult = IotCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblLoadingTO.ModbusRefId);
                GateIoTResult gateIoTResult = IoT.GateCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblLoadingTO);
                if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count > 0)
                {
                    loadingSlipTo.VehicleNo = Convert.ToString(gateIoTResult.Data[0][(Int32)IoTConstants.GateIoTColE.VehicleNo]);
                    DimStatusTO latestStatusTo = statuslist.Where(w => w.IotStatusId == Convert.ToInt16(gateIoTResult.Data[0][(Int32)IoTConstants.GateIoTColE.StatusId])).FirstOrDefault();
                    if (latestStatusTo != null)
                    {
                        loadingSlipTo.StatusId = latestStatusTo.IdStatus;
                    }
                    loadingSlipTo.VehicleNo = Convert.ToString(gateIoTResult.Data[0][(Int32)IoTConstants.GateIoTColE.VehicleNo]);
                    for (int j = 0; j < gateIoTResult.Data.Count; j++)
                    {
                        TblLoadingStatusHistoryTO statusHistoryTO = new TblLoadingStatusHistoryTO();
                        statusHistoryTO.LoadingId = loadingSlipTo.LoadingId;
                        DimStatusTO dimStatusTO = statuslist.Where(w => w.IotStatusId == Convert.ToInt16(gateIoTResult.Data[j][(Int32)IoTConstants.GateIoTColE.StatusId])).FirstOrDefault();
                        if (dimStatusTO != null)
                        {
                            statusHistoryTO.StatusId = dimStatusTO.IdStatus;
                        }
                        statusHistoryTO.StatusDate = IotCommunication.IoTDateTimeStringToDate((String)gateIoTResult.Data[j][(int)IoTConstants.GateIoTColE.StatusDate]);
                        statusHistoryTO.StatusRemark = dimStatusTO.StatusName;
                        statusHistoryList.Add(statusHistoryTO);
                    }
                }
            }
            return statusHistoryList;
        }


        /// <summary>
        /// Vijaymala [08-05-2018] added to get notified loading list withiin period 
        /// </summary>
        /// <returns></returns>
        public static List<TblLoadingSlipTO> SelectAllNotifiedTblLoadingList(DateTime fromDate, DateTime toDate, Int32 callFlag,string selectOrgIdStr)
        {
            return TblLoadingSlipDAO.SelectAllNotifiedTblLoadingList(fromDate, toDate, callFlag, selectOrgIdStr);
        }

        #endregion

        #region Insertion
        public static int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            return TblLoadingSlipDAO.InsertTblLoadingSlip(tblLoadingSlipTO);
        }

        public static int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDAO.InsertTblLoadingSlip(tblLoadingSlipTO, conn, tran);
        }

        

        #endregion
        
        #region Updation
        public static int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            return TblLoadingSlipDAO.UpdateTblLoadingSlip(tblLoadingSlipTO);
        }

        public static int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDAO.UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
        }

        public static int UpdateTblLoadingSlip(TblLoadingTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDAO.UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
        }

        public static ResultMessage ChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO,Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMessage =ChangeLoadingSlipConfirmationStatus(tblLoadingSlipTO, loginUserId, conn, tran);
                if(resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return null;
                }
                
                return resultMessage;
            }
            catch (Exception ex)
            {
                 resultMessage.DefaultExceptionBehaviour(ex, "ChangeLoadingSlipConfirmationStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
         


        }

        public static ResultMessage ChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            DateTime serverDate = Constants.ServerDateTime;
            try
            {
                

                //Check First Invoice is created against the loading Slip. If created then do not allow to change the status
                //TblInvoiceTO invoiceTO = BL.TblInvoiceBL.SelectInvoiceTOFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                //if (invoiceTO != null)
                //{
                //    resultMessage.DefaultBehaviour("Invoice is already prepared against loadingSlip. Ref Inv Id:" + invoiceTO.IdInvoice);
                //    resultMessage.DisplayMessage = "Hey..Not allowed. Invoice is already prepared against selected loadingSlip";
                //    return resultMessage;
                //}

                Int32 lastConfirmationStatus = tblLoadingSlipTO.IsConfirmed;
                if (lastConfirmationStatus == 1)
                    tblLoadingSlipTO.IsConfirmed = 0;
                else
                    tblLoadingSlipTO.IsConfirmed = 1;

                List<TblLoadingSlipExtTO> loadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                var stringsArray = loadingSlipExtTOList.Select(i => i.ParityDtlId.ToString()).ToArray();
                var parityDtlIds = string.Join(",", stringsArray);
               // List<TblParityDetailsTO> parityDetailsTOList = BL.TblParityDetailsBL.SelectAllParityDetailsListByIds(parityDtlIds, conn, tran);
                List<TblBookingsTO> bookingList = BL.TblBookingsBL.SelectAllBookingsListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                TblLoadingTO loadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTO.LoadingId, conn, tran);
                Double freightPerMT = 0;
                if (loadingTO.IsFreightIncluded == 1)
                {
                    freightPerMT = loadingTO.FreightAmt;
                }

                for (int e = 0; e < tblLoadingSlipTO.LoadingSlipExtTOList.Count; e++)
                {

                    TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[e];
                    TblLoadingSlipExtHistoryTO loadingSlipExtHistoryTO = new TblLoadingSlipExtHistoryTO();

                    //Assign Last Calculated values to History Object
                    loadingSlipExtHistoryTO.CreatedBy = loginUserId;
                    loadingSlipExtHistoryTO.CreatedOn = serverDate;
                    loadingSlipExtHistoryTO.LastConfirmationStatus = lastConfirmationStatus;
                    loadingSlipExtHistoryTO.LastRateCalcDesc = tblLoadingSlipExtTO.RateCalcDesc;
                    loadingSlipExtHistoryTO.LastRatePerMT = tblLoadingSlipExtTO.RatePerMT;
                    loadingSlipExtHistoryTO.ParityDtlId = tblLoadingSlipExtTO.ParityDtlId;
                    loadingSlipExtHistoryTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
                    loadingSlipExtHistoryTO.LastCdAplAmt = tblLoadingSlipExtTO.CdApplicableAmt;

                    if (tblLoadingSlipExtTO.BookingId > 0)
                    {

                        #region ReCalculate Actual Price From Booking and Parity Settings

                        var tblBookingsTO = bookingList.Where(b => b.IdBooking == tblLoadingSlipExtTO.BookingId).FirstOrDefault();

                        //Priyanka [28-03-2019] Added
                        //if (tblBookingsTO == null)
                        //{
                        //    var BookingRefId = bookingList[0].BookingRefId;
                        //    List<TblBookingsTO> bookingListSplitedList = BL.TblBookingsBL.SelectAllBookingsListFromBookingDisplayNo(BookingRefId, conn, tran);
                        //    if(bookingListSplitedList != null && bookingListSplitedList.Count > 0)
                        //    {
                        //        tblBookingsTO = bookingListSplitedList[0];
                        //    }
                        //}
                            Double orcAmtPerTon = 0;
                        //if (tblBookingsTO.OrcMeasure == "Rs/MT")
                        //{t
                        //  b  orcAmtPerTon = tblBookingsTO.OrcAmt;
                        //}
                        //else
                        //{
                        //    if (tblBookingsTO.OrcAmt > 0)
                        //        orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
                        //}

                        if (tblLoadingSlipTO.OrcMeasure == "Rs/MT")
                        {
                            orcAmtPerTon = tblLoadingSlipTO.OrcAmt;
                        }
                        else
                        {
                            if (tblLoadingSlipTO.OrcAmt > 0)
                                orcAmtPerTon = tblLoadingSlipTO.OrcAmt / tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty;
                        }


                        String rateCalcDesc = string.Empty;
                        rateCalcDesc = "B.R : " + tblBookingsTO.BookingRate + "|";
                        Double bookingPrice = tblBookingsTO.BookingRate;
                        Double parityAmt = 0;
                        Double priceSetOff = 0;
                        Double paritySettingAmt = 0;
                        Double bvcAmt = 0;
                        //TblParitySummaryTO parityTO = null;
                        //Vijaymala[18-06-2018] added to get data from parity details
                        TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);

                        TblParityDetailsTO parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, addrTO.StateId, tblBookingsTO.BookingDatetime);


                        //Vijaymala Commented[18-06-2018]-to get data from paritydetails
                        //if (parityDetailsTOList != null)
                        //{
                          //  var parityDtlTO = parityDetailsTOList.Where(m => m.MaterialId == tblLoadingSlipExtTO.MaterialId
                            //                        && m.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                              //                      && m.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId).FirstOrDefault();
                            if (parityDtlTO != null)
                            {
                                parityAmt = parityDtlTO.ParityAmt;
                                if (tblLoadingSlipTO.IsConfirmed != 1)
                                    priceSetOff = parityDtlTO.NonConfParityAmt;
                                else
                                    priceSetOff = 0;

                                tblLoadingSlipExtTO.ParityDtlId = parityDtlTO.IdParityDtl;
                            }
                            else
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : parityDtlTO Not Found";
                            string mateDesc = tblLoadingSlipExtTO.DisplayName;
                                //tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                                resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                                return resultMessage;
                            }

                            //parityTO = BL.TblParitySummaryBL.SelectTblParitySummaryTO(parityDtlTO.ParityId, conn, tran);
                            //if (parityTO == null)
                            //{
                            //    resultMessage.DefaultBehaviour();
                            //    resultMessage.Text = "Error : ParityTO Not Found";
                            //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            //    return resultMessage;
                            //}

                            paritySettingAmt = parityDtlTO.BaseValCorAmt + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                            bvcAmt = parityDtlTO.BaseValCorAmt;
                            rateCalcDesc += "BVC Amt :" + parityDtlTO.BaseValCorAmt + "|" + "Exp Amt :" + parityDtlTO.ExpenseAmt + "|" + " Other :" + parityDtlTO.OtherAmt + "|";
                        //}
                        //else
                        //{
                        //    resultMessage.DefaultBehaviour();
                        //    resultMessage.Text = "Error : ParityTO Not Found";
                        //    resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                        //    return resultMessage;
                        //}

                        Double cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);
                        Double cdAmt = 0;
                        if (tblLoadingSlipTO.CdStructure > 0)
                            cdAmt = (cdApplicableAmt * tblLoadingSlipTO.CdStructure) / 100;

                        rateCalcDesc += "CD :" + Math.Round(cdAmt, 2) + "|";
                        Double rateAfterCD = cdApplicableAmt - cdAmt;

                        Double gstApplicableAmt = 0;
                        if (tblLoadingSlipTO.IsConfirmed == 1)
                            gstApplicableAmt = rateAfterCD + freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                        else
                            gstApplicableAmt = rateAfterCD;

                        Double gstAmt = (gstApplicableAmt * 18) / 100;
                        gstAmt = Math.Round(gstAmt, 2);

                        Double finalRate = 0;
                        if (tblLoadingSlipTO.IsConfirmed == 1)
                            finalRate = gstApplicableAmt + gstAmt;
                        else
                            finalRate = gstApplicableAmt + gstAmt + freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;

                        tblLoadingSlipExtTO.TaxableRateMT = gstApplicableAmt;
                        tblLoadingSlipExtTO.RatePerMT = finalRate;
                        tblLoadingSlipExtTO.CdApplicableAmt = cdApplicableAmt;
                        tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;

                        rateCalcDesc += " ORC :" + orcAmtPerTon + "|" + " Parity :" + parityAmt + "|" + " NC Amt :" + priceSetOff + "|" + " Freight :" + freightPerMT + "|" + " GST :" + gstAmt + "|";
                        tblLoadingSlipExtTO.RateCalcDesc = rateCalcDesc;
                        #endregion
                    }
                   
                    result = BL.TblLoadingSlipExtBL.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error : While UpdateTblLoadingSlipExt Against LoadingSlip");
                        return resultMessage;
                    }


                    //Assign New Values and Save the history Record
                    loadingSlipExtHistoryTO.CurrentRatePerMT = tblLoadingSlipExtTO.RatePerMT;
                    loadingSlipExtHistoryTO.CurrentConfirmationStatus = tblLoadingSlipTO.IsConfirmed;
                    loadingSlipExtHistoryTO.CurrentRateCalcDesc = tblLoadingSlipExtTO.RateCalcDesc;
                    loadingSlipExtHistoryTO.CurrentCdAplAmt = tblLoadingSlipExtTO.CdApplicableAmt;

                    result = BL.TblLoadingSlipExtHistoryBL.InsertTblLoadingSlipExtHistory(loadingSlipExtHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error : While InsertTblLoadingSlipExtHistory Against LoadingSlip");
                        return resultMessage;
                    }
                }

                result = UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error : While UpdateTblLoadingSlip");
                    return resultMessage;
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ChangeLoadingSlipConfirmationStatus");
                return resultMessage;
            }
            finally
            {
            }
        }

            #endregion

        #region Deletion
        public static int DeleteTblLoadingSlip(Int32 idLoadingSlip)
        {
            return TblLoadingSlipDAO.DeleteTblLoadingSlip(idLoadingSlip);
        }

        public static int DeleteTblLoadingSlip(Int32 idLoadingSlip, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDAO.DeleteTblLoadingSlip(idLoadingSlip, conn, tran);
        }

        public static ResultMessage DeleteLoadingSlipWithDetails(TblLoadingTO tblLoadingTO,Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                #region Delete Slip


                Int32 result = 0;
                if (tblLoadingTO.LoadingType != (int)Constants.LoadingTypeE.OTHER)
                {
                    TblLoadingSlipDtlTO tblLoadingSlipDtlTO = TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(loadingSlipId, conn, tran);
                    if (tblLoadingSlipDtlTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error : tblLoadingTo found null");
                        return resultMessage;
                    }

                    result = BL.TblLoadingSlipDtlBL.DeleteTblLoadingSlipDtl(tblLoadingSlipDtlTO.IdLoadSlipDtl, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Details.");
                        return resultMessage;
                    }
                }
                //Delete Address

                List<TblLoadingSlipAddressTO> tblLoadingSlipAddressTOList = TblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(loadingSlipId, conn, tran);
                if (tblLoadingSlipAddressTOList != null && tblLoadingSlipAddressTOList.Count > 0)
                {
                    for (int u = 0; u < tblLoadingSlipAddressTOList.Count; u++)
                    {
                        result = TblLoadingSlipAddressBL.DeleteTblLoadingSlipAddress(tblLoadingSlipAddressTOList[u].IdLoadSlipAddr, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Address Details for IdLoadSlipAddr = " + tblLoadingSlipAddressTOList[u].IdLoadSlipAddr);
                            return resultMessage;
                        }
                    }

                }

                List<TblLoadingSlipExtTO> tblLoadingSlipExtList = TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(loadingSlipId, conn, tran);
                if (tblLoadingSlipExtList != null && tblLoadingSlipExtList.Count > 0)
                {
                    for (int j = 0; j < tblLoadingSlipExtList.Count; j++)
                    {
                        result = TblLoadingSlipExtHistoryBL.DeleteTempLoadingSlipExtHistoryTOList(tblLoadingSlipExtList[j].IdLoadingSlipExt, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Extenstion History Details for IdLoadingSlipExt = " + tblLoadingSlipExtList[j].IdLoadingSlipExt);
                            return resultMessage;
                        }

                        result = TblLoadingSlipExtBL.DeleteTblLoadingSlipExt(tblLoadingSlipExtList[j].IdLoadingSlipExt, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Extenstion Details for IdLoadingSlipExt = " + tblLoadingSlipExtList[j].IdLoadingSlipExt);
                            return resultMessage;
                        }
                    }

                }

                result = BL.TblLoadingSlipBL.DeleteTblLoadingSlip(loadingSlipId, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Details");
                    return resultMessage;
                }

                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage = new ResultMessage();
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteLoadingSlipWithDetails");
                return resultMessage;
            }
        }

        #endregion

    }
}
