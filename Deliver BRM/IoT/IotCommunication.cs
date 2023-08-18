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
using SalesTrackerAPI.BL;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace SalesTrackerAPI.IoT
{
    public class IotCommunication
    {

        public static TblLoadingTO GetItemDataFromIotAndMerge(TblLoadingTO tblLoadingTO, Boolean loadingWithDtls, Boolean getStatusHistory = false, Int32 isWeighing = 0)
        {
            if (tblLoadingTO != null)
            {

                if ((tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) && tblLoadingTO.ModbusRefId == 0)
                    return tblLoadingTO;

                //Call To Gate IoT For Vehicle & Transport Details
                    GateIoTResult gateIoTResult = IoT.GateCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblLoadingTO);
                    if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count != 0)
                    {
                        tblLoadingTO.VehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
                        tblLoadingTO.TransporterOrgId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.TransportorId]);
                        String statusDate = (String)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusDate];

                        Int32 statusId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusId]);

                        DimStatusTO dimStatusTO = BL.DimStatusBL.SelectDimStatusTOByIotStatusId(statusId);
                        tblLoadingTO.StatusDate = IoTDateTimeStringToDate(statusDate);

                        if (dimStatusTO != null)
                        {
                            tblLoadingTO.StatusId = dimStatusTO.IdStatus;
                            tblLoadingTO.StatusDesc = dimStatusTO.StatusName;
                        }
                        if (tblLoadingTO.LoadingSlipList != null)
                            tblLoadingTO.LoadingSlipList.ForEach(f => { f.StatusId = tblLoadingTO.StatusId; f.StatusName = tblLoadingTO.StatusDesc; f.StatusDate = tblLoadingTO.StatusDate; f.VehicleNo = tblLoadingTO.VehicleNo; });
                        String transporterName = TblOrganizationBL.GetFirmNameByOrgId(tblLoadingTO.TransporterOrgId);
                        tblLoadingTO.TransporterOrgName = transporterName;


                        //Saket [2019-04-16] Added to get status Histoy.
                        if (getStatusHistory)
                        {
                            tblLoadingTO.LoadingStatusHistoryTOList = new List<TblLoadingStatusHistoryTO>();

                            List<DimStatusTO> statuslist = DimStatusBL.SelectAllDimStatusList();

                            for (int j = 0; j < gateIoTResult.Data.Count; j++)
                            {
                                TblLoadingStatusHistoryTO statusHistoryTO = new TblLoadingStatusHistoryTO();
                                statusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                                DimStatusTO dimStatusTO1 = statuslist.Where(w => w.IotStatusId == Convert.ToInt16(gateIoTResult.Data[j][(Int32)IoTConstants.GateIoTColE.StatusId])).FirstOrDefault();
                                if (dimStatusTO1 != null)
                                {
                                    statusHistoryTO.StatusId = dimStatusTO1.IdStatus;
                                }
                                statusHistoryTO.StatusDate = IotCommunication.IoTDateTimeStringToDate((String)gateIoTResult.Data[j][(int)IoTConstants.GateIoTColE.StatusDate]);
                                statusHistoryTO.StatusRemark = dimStatusTO1.StatusName;
                                tblLoadingTO.LoadingStatusHistoryTOList.Add(statusHistoryTO);
                            }

                        }
                    }

                    else
                    {
                        throw new Exception("IoT details not found");
                    }
                
                if (loadingWithDtls)
                {
                    // List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();
                    List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineOfWeighingList(tblLoadingTO.IdLoading);
                    if (tblLoadingTO.LoadingSlipList != null)
                    {
                        List<TblLoadingSlipExtTO> totalLoadingSlipExtList = new List<TblLoadingSlipExtTO>();
                        foreach (var loadingSlip in tblLoadingTO.LoadingSlipList)
                        {
                            // var list = loadingSlip.LoadingSlipExtTOList.Where(w => w.LoadedWeight != 0).ToList();
                            totalLoadingSlipExtList.AddRange(loadingSlip.LoadingSlipExtTOList);
                        }

                        //var layerList = totalLoadingSlipExtList.GroupBy(x => x.LoadingLayerid).ToList();
                        List<int> totalLayerList = new List<int>();
                        if (!totalLayerList.Contains(0))
                            totalLayerList.Add(0);
                        var tLayerList = tblWeighingMachineList.GroupBy(test => test.LayerId)
                                           .Select(grp => grp.First()).ToList();

                        foreach (var item in tLayerList)
                        {
                            if (item.LayerId != 0)
                                totalLayerList.Add(item.LayerId);
                        }
                        var distinctWeighingMachineList = tblWeighingMachineList.GroupBy(test => test.IdWeighingMachine)
                                          .Select(grp => grp.First()).ToList();
                        //Addedd by kiran to avoid IoT calls 13/03/19
                        if (tblLoadingTO.StatusId == Convert.ToInt16(Constants.TranStatusE.LOADING_IN_PROGRESS) || tblLoadingTO.StatusId == Convert.ToInt16(Constants.TranStatusE.LOADING_COMPLETED) || tblLoadingTO.StatusId == Convert.ToInt16(Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH) || tblLoadingTO.ModbusRefId > 0)
                        {
                            //Sanjay [03-June-2019] Now Layerwise call will not be required as data will be received from TCP/ip communication
                            //Now pass layerid=0. IoT Code will internally give data for all layers.
                            //for (int i = 0; i < totalLayerList.Count; i++)
                            //{
                            //int layerid = totalLayerList[i];
                            int layerid = 0;
                            Int32 loadingId = tblLoadingTO.ModbusRefId;
                            object layerData = null;
                            //var distinctWeighingMachineList = tblWeighingMachineList.GroupBy(x => x.IdWeighingMachine).ToList();//tblWeighingMachineList.Distinct().ToList();

                            if (distinctWeighingMachineList != null && distinctWeighingMachineList.Any())
                            {
                                for (int mc = 0; mc < distinctWeighingMachineList.Count; mc++)
                                {
                                    //Call to Weight IoT
                                    NodeJsResult itemList = WeighingCommunication.GetLoadingLayerData(loadingId, layerid, distinctWeighingMachineList[mc]);
                                    if (itemList.Data != null)
                                    {
                                        layerData = itemList.Data;
                                        //if (layerid == 0)
                                        if(false)
                                        {
                                            List<int[]> defaultResultList = new List<int[]>();
                                            if (itemList.Data != null && itemList.Data.Count > 0)
                                            {
                                                defaultResultList = itemList.Data.Where(w => w[(int)IoTConstants.WeightIotColE.WeighTypeId] == (Int32)Constants.TransMeasureTypeE.TARE_WEIGHT || w[(int)IoTConstants.WeightIotColE.WeighTypeId] == (Int32)Constants.TransMeasureTypeE.GROSS_WEIGHT).ToList();

                                                if (tblLoadingTO.DynamicItemListDCT.ContainsKey(distinctWeighingMachineList[mc].IdWeighingMachine))
                                                    tblLoadingTO.DynamicItemListDCT[distinctWeighingMachineList[mc].IdWeighingMachine].AddRange(defaultResultList);
                                                else
                                                    tblLoadingTO.DynamicItemListDCT.Add(distinctWeighingMachineList[mc].IdWeighingMachine, defaultResultList);
                                                //tblLoadingTO.DynamicItemList.AddRange(defaultResultList);
                                            }
                                        }
                                        else
                                        {
                                            if (tblLoadingTO.DynamicItemListDCT.ContainsKey(distinctWeighingMachineList[mc].IdWeighingMachine))
                                                tblLoadingTO.DynamicItemListDCT[distinctWeighingMachineList[mc].IdWeighingMachine].AddRange(itemList.Data);
                                            else
                                                //tblLoadingTO.DynamicItemList.AddRange(itemList.Data);
                                                tblLoadingTO.DynamicItemListDCT.Add(distinctWeighingMachineList[mc].IdWeighingMachine, itemList.Data);


                                            if (itemList.Data != null && itemList.Data.Count > 0)
                                            {
                                                for (int f = 0; f < itemList.Data.Count; f++)
                                                {
                                                    var itemRefId = itemList.Data[f][(int)IoTConstants.WeightIotColE.ItemRefNo];
                                                    var itemTO = totalLoadingSlipExtList.Where(w => w.ModbusRefId == itemRefId).FirstOrDefault();
                                                    if (itemTO != null)
                                                    {
                                                        itemTO.LoadedWeight = itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedWt];
                                                        itemTO.CalcTareWeight = itemList.Data[f][(int)IoTConstants.WeightIotColE.CalcTareWt];
                                                        itemTO.LoadedBundles = itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedBundle];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //}
                        }
                    }
                }
            }

            return tblLoadingTO;
        }

        public static void GetItemDataFromIotForGivenLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            if (tblLoadingSlipTO != null)
            {

                if (tblLoadingSlipTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED)
                    return;

                if (true)
                {
                    //List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();
                    List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineOfWeighingList(tblLoadingSlipTO.LoadingId);

                    List<TblLoadingSlipExtTO> totalLoadingSlipExtList = tblLoadingSlipTO.LoadingSlipExtTOList;

                    TblLoadingTO loadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTO.LoadingId);

                    GateIoTResult gateIoTResult = IoT.GateCommunication.GetLoadingStatusHistoryDataFromGateIoT(loadingTO);
                    if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count != 0)
                    {
                        tblLoadingSlipTO.VehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
                    }

                    //var layerList = totalLoadingSlipExtList.GroupBy(x => x.LoadingLayerid).ToList();
                    //List<int> totalLayerList = new List<int>();
                    //if (!totalLayerList.Contains(0))
                    //    totalLayerList.Add(0);
                    List<int> totalLayerList = new List<int>();
                    //var tLayerList = tblWeighingMachineList.GroupBy(test => test.LayerId)
                    //                   .Select(grp => grp.First()).ToList();
                    var tLayerList = totalLoadingSlipExtList.GroupBy(x => x.LoadingLayerid).Select(grp => grp.First()).ToList();
                    foreach (var item in tLayerList)
                    {
                        if (item.LoadingLayerid != 0)
                            totalLayerList.Add(item.LoadingLayerid);
                    }
                    var distinctWeighingMachineList = tblWeighingMachineList.GroupBy(test => test.IdWeighingMachine)
                                          .Select(grp => grp.First()).ToList();
                    //foreach (var item in layerList)
                    //{
                    //    totalLayerList.Add(item.Key);
                    //}

                    //Sanjay [03-June-2019] Now Layerwise call will not be required as data will be received from TCP/ip communication
                    //Now pass layerid=0. IoT Code will internally give data for all layers.
                    //for (int i = 0; i < totalLayerList.Count; i++)
                    //{
                    //int layerid = totalLayerList[i];
                    int layerid = 0;
                    Int32 loadingId = loadingTO.ModbusRefId;
                    for (int mc = 0; mc < distinctWeighingMachineList.Count; mc++)
                    {
                        //Call to Weight IoT
                        NodeJsResult itemList = WeighingCommunication.GetLoadingLayerData(loadingId, layerid, distinctWeighingMachineList[mc]);
                        if (itemList.Data != null)
                        {

                            if (itemList.Data != null && itemList.Data.Count > 0)
                            {
                                for (int f = 0; f < itemList.Data.Count; f++)
                                {
                                    var itemRefId = itemList.Data[f][(int)IoTConstants.WeightIotColE.ItemRefNo];
                                    var itemTO = totalLoadingSlipExtList.Where(w => w.ModbusRefId == itemRefId).FirstOrDefault();
                                    if (itemTO != null)
                                    {
                                        itemTO.LoadedWeight = itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedWt];
                                        itemTO.CalcTareWeight = itemList.Data[f][(int)IoTConstants.WeightIotColE.CalcTareWt];
                                        itemTO.LoadedBundles = itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedBundle];
                                    }
                                }
                            }
                        }
                    }
                    //}
                }
            }
        }

        public static DateTime IoTDateTimeStringToDate(string statusDate)
        {
            var dateList = statusDate.Split(',').ToList();
            DateTime serverDate = Constants.ServerDateTime;
            DateTime dateTime = new DateTime();
            if (dateList != null && dateList.Count == 5)
            {
                Int32 date = Convert.ToInt32(dateList[0]);
                Int32 month = Convert.ToInt32(dateList[1]);
                Int32 year = Convert.ToInt32(serverDate.Year.ToString().Substring(0, 2) + dateList[2]);
                Int32 hr = Convert.ToInt32(dateList[3]);
                Int32 min = Convert.ToInt32(dateList[4]);

                dateTime = new DateTime(year, month, date, hr, min, 0);
            }
            return dateTime;
        }

        public static List<TblLoadingTO> GetLoadingData(List<TblLoadingTO> tblLoadingTOList)
        {

            if (tblLoadingTOList != null && tblLoadingTOList.Count > 0)
            {
                for (int i = 0; i < tblLoadingTOList.Count; i++)
                {
                    GetItemDataFromIotAndMerge(tblLoadingTOList[i], false);
                }
            }

            return tblLoadingTOList;
        }

        public static void GetWeighingMeasuresFromIoT(string loadingId, bool isUnloading, List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                string[] loadingIdsList = loadingId.Split(',');
                if (loadingIdsList != null && loadingIdsList.Length > 0)
                {
                    //List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();
                    for (int i = 0; i < loadingIdsList.Length; i++)
                    {
                        TblLoadingTO tblLoadingTO = TblLoadingBL.SelectLoadingTOWithDetails(Convert.ToInt32(loadingIdsList[i]));
                        //tblLoadingTO = TblLoadingBL.getDataFromIotAndMerge(tblLoadingTO);
                        //NodeJsResult itemList = GetLoadingLayerData(tblLoadingTO.ModbusRefId, 0);
                        // List<int[]> weighingDataList = new List<int[]>();
                        //if (itemList != null)
                        //{
                        //    List<int[]> defaultResultList = new List<int[]>();
                        //    if (itemList.Data != null && itemList.Data.Count > 0)
                        //    {
                        //        defaultResultList = itemList.Data.Where(w => w[(int)IoTConstants.WeightIotColE.WeighTypeId] == (Int32)Constants.TransMeasureTypeE.TARE_WEIGHT || w[(int)IoTConstants.WeightIotColE.WeighTypeId] == (Int32)Constants.TransMeasureTypeE.GROSS_WEIGHT).ToList();
                        //    }
                        //    weighingDataList.AddRange(defaultResultList);
                        //}
                        if (tblLoadingTO.DynamicItemListDCT != null && tblLoadingTO.DynamicItemListDCT.Count > 0)
                        {
                            foreach (KeyValuePair<int, List<int[]>> pair in tblLoadingTO.DynamicItemListDCT)
                            {
                                foreach (var item in pair.Value)
                                {
                                    TblWeighingMeasuresTO measuresTO = new TblWeighingMeasuresTO();
                                    measuresTO.LoadingId = tblLoadingTO.IdLoading;
                                    measuresTO.WeightMeasurTypeId = item[(int)IoTConstants.WeightIotColE.WeighTypeId];
                                    measuresTO.WeightMT = item[(int)IoTConstants.WeightIotColE.Weight];
                                    measuresTO.VehicleNo = tblLoadingTO.VehicleNo;
                                    measuresTO.UnLoadingId = Convert.ToInt32(isUnloading);
                                    measuresTO.WeighingMachineId = pair.Key;
                                    int dateTimeVal = item[(int)IoTConstants.WeightIotColE.TimeStamp];
                                    string dateTimeValTemp = dateTimeVal.ToString();
                                    if (dateTimeValTemp.Length <= 5)
                                    {
                                        dateTimeValTemp = "0" + dateTimeVal;
                                    }
                                    string hrsMin = dateTimeValTemp.Substring(2, 4);
                                    string hrs = hrsMin.Substring(0, 2).ToString();
                                    string min = hrsMin.Substring(2, 2).ToString();
                                    string date = dateTimeValTemp.Replace(hrsMin, "");

                                    DateTime dateTime = new DateTime(Constants.ServerDateTime.Year, Constants.ServerDateTime.Month, Convert.ToInt32(date), Convert.ToInt32(hrs), Convert.ToInt32(min), 0);
                                    measuresTO.CreatedOn = dateTime;
                                    tblWeighingMeasuresTOList.Add(measuresTO);
                                }
                                //Console.WriteLine("{0}, {1}", pair.Key, pair.Value);
                            }
                        }
                        //if (tblLoadingTO.DynamicItemList != null && tblLoadingTO.DynamicItemList.Count > 0)
                        //    weighingDataList.AddRange(tblLoadingTO.DynamicItemList);
                        //for (int wd = 0; wd < weighingDataList.Count; wd++)
                        //{
                        //    TblWeighingMeasuresTO measuresTO = new TblWeighingMeasuresTO();
                        //    measuresTO.LoadingId = tblLoadingTO.IdLoading;
                        //    measuresTO.WeightMeasurTypeId = weighingDataList[wd][(int)IoTConstants.WeightIotColE.WeighTypeId];
                        //    measuresTO.WeightMT = weighingDataList[wd][(int)IoTConstants.WeightIotColE.Weight];
                        //    measuresTO.VehicleNo = tblLoadingTO.VehicleNo;
                        //    measuresTO.UnLoadingId = Convert.ToInt32(isUnloading);
                        //    measuresTO.WeighingMachineId = tblLoadingTO.DynamicItemListDCT.Keys[];
                        //    // measuresTO.CreatedBy = 1;
                        //    tblWeighingMeasuresTOList.Add(measuresTO);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static int PostGateAPIDataToModbusTcpApi(TblLoadingTO tblLoadingTO, Object[] writeData)
        {

            try
            {
                if (writeData.Length != 5)
                {
                    return 0;
                }
                    var tRequest = WebRequest.Create(tblLoadingTO.IoTUrl + "WriteOnGateIoTCommand") as HttpWebRequest;
                return GateCommunication.PostGateApiCalls(tblLoadingTO, writeData, tRequest);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int UpdateLoadingStatusOnGateAPIToModbusTcpApi(TblLoadingTO tblLoadingTO, Object[] writeData)
        {
            try
            {
                if (writeData.Length != 2)
                {
                    return 0;
                }
                //var tRequest = WebRequest.Create(Startup.GateIotApiURL + "UpdateStatusCommand") as HttpWebRequest;
                var tRequest = WebRequest.Create(tblLoadingTO.IoTUrl + "UpdateStatusCommand") as HttpWebRequest;
                return GateCommunication.PostGateApiCalls(tblLoadingTO, writeData, tRequest);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static NodeJsResult DeleteSingleLoadingFromWeightIoTByModBusRefId(TblLoadingTO tblLoadingTO)
        {
            NodeJsResult nodeJsResult = new NodeJsResult();
            try
            {
                if (tblLoadingTO != null && tblLoadingTO.ModbusRefId != 0)
                {
                    Int32 modBusRefId = tblLoadingTO.ModbusRefId;

                    //List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();

                    List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineOfWeighingList(tblLoadingTO.IdLoading);

                    if (tblWeighingMachineList != null && tblWeighingMachineList.Count > 0)
                    {
                        tblWeighingMachineList = tblWeighingMachineList.GroupBy(g => g.IdWeighingMachine).Select(s => s.FirstOrDefault()).ToList();
                        for (int i = 0; i < tblWeighingMachineList.Count; i++)
                        {
                            TblWeighingMachineTO tblWeighingMachineTO = tblWeighingMachineList[i];
                            if (!String.IsNullOrEmpty(tblWeighingMachineTO.IoTUrl))
                            {
                                //Addes by kiran for retry 3 times to delete weighing data
                                int cnt2 = 0;
                                while (cnt2 < 3)
                                {
                                    nodeJsResult = IoT.WeighingCommunication.DeleteSingleLoadingFromWeightIoT(modBusRefId, 0, tblWeighingMachineTO);
                                    if (nodeJsResult.Code == 1)
                                    {
                                        break;
                                    }
                                    cnt2++;
                                }
                            }
                        }
                    }
                    else
                    {
                        nodeJsResult.Code = 1;
                    }
                    return nodeJsResult;
                }
                else
                {
                    nodeJsResult.DefaultErrorBehavior(0, "Loading id not found");
                    return nodeJsResult;
                }
            }
            catch (Exception ex)
            {
                nodeJsResult.DefaultErrorBehavior(0, "Error in DeleteSingleLoadingFromWeightIoT");
                return nodeJsResult;
            }
        }





        public static GateIoTResult GetLoadingSlipsByStatusFromIoTByStatusId(String statusIds, TblGateTO tblGateTO)
        {
            //Sanjay [03-June-2019] Now Iot communication is shifted to TCP/IP and hence data length is increased.
            //It will return max  87 per call
            //Int32 maxRecordPerCylce = 24;
            Int32 maxRecordPerCylce = 83;

            List<DimStatusTO> dimStatusTOList = DimStatusBL.SelectAllDimStatusList();

            GateIoTResult gateIoTResult = new GateIoTResult();
            try
            {

                if (!String.IsNullOrEmpty(statusIds))
                {
                    List<String> statusList = statusIds.Split(',').ToList();

                    Boolean callAllFunction = false;
                    if (Convert.ToInt32(statusIds) == 101 || Convert.ToInt32(statusIds) == 102 || Convert.ToInt32(statusIds) == 103
                        || Convert.ToInt32(statusIds) == 104 || Convert.ToInt32(statusIds) == 105) // Convered to int as it has been decided that this value always be single. To Pass multiple combination it been again encoded to some number
                        callAllFunction = true;

                    for (int i = 0; i < statusList.Count; i++)
                    {
                        Int32 statusId = Convert.ToInt32(statusList[i]);
                        Int32 startLoadingId = 1;  // this is default value from which records will be search from Iot if not passed

                        DimStatusTO dimStatusTO = dimStatusTOList.Where(w => w.IdStatus == statusId).FirstOrDefault();
                        if (dimStatusTO != null || callAllFunction)
                        {
                            Int32 breakLoop = 0;
                            while (breakLoop != 1)
                            {
                                GateIoTResult gateIoTResultTemp = null;
                                if (callAllFunction)
                                    gateIoTResultTemp = IoT.GateCommunication.GetAllLoadingSlipsByStatusFromIoT(Convert.ToInt32(statusIds), tblGateTO, startLoadingId);
                                else
                                    gateIoTResultTemp = IoT.GateCommunication.GetLoadingSlipsByStatusFromIoT(dimStatusTO.IotStatusId, tblGateTO, startLoadingId);

                                if (gateIoTResultTemp != null && gateIoTResultTemp.Data != null)
                                {
                                    if (gateIoTResultTemp.Data.Count >= maxRecordPerCylce)
                                    {
                                        startLoadingId = Convert.ToInt32(gateIoTResultTemp.Data[gateIoTResultTemp.Data.Count - 1][(Int32)IoTConstants.GateIoTColE.LoadingId]);
                                        startLoadingId += 1;
                                    }
                                    else
                                    {
                                        breakLoop = 1;
                                    }
                                    gateIoTResult.Data.AddRange(gateIoTResultTemp.Data);
                                }
                                else
                                {
                                    breakLoop = 1;
                                }
                            }
                        }
                    }
                }
                return gateIoTResult;
            }
            catch (Exception ex)
            {
                gateIoTResult.DefaultErrorBehavior(0, "Error in GetLoadingStatusHistoryDataFromGateIoT");
                return gateIoTResult;
            }
        }

        //Sanjay Gunjal [03-June-2019] Commented as Async and await will not work as Iot Works in queque or sequentially.
        //public static async Task<GateIoTResult> GetLoadingSlipsByStatusFromIoTByStatusIdV2(String statusIds, List<long> totalTime)
        //{
        //    Int32 maxRecordPerCylce = 15;

        //    List<DimStatusTO> dimStatusTOList = DimStatusBL.SelectAllDimStatusList();
        //    long totalTimeq = 0;
        //    GateIoTResult gateIoTResult = new GateIoTResult();
        //    try
        //    {

        //        if (!String.IsNullOrEmpty(statusIds))
        //        {
        //            List<String> statusList = statusIds.Split(',').ToList();

        //            for (int i = 0; i < statusList.Count; i++)
        //            {
        //                Int32 statusId = Convert.ToInt32(statusList[i]);

        //                if (statusId == 0)
        //                {

        //                }

        //                Int32 startLoadingId = 1;

        //                DimStatusTO dimStatusTO = dimStatusTOList.Where(w => w.IdStatus == statusId).FirstOrDefault();
        //                if (dimStatusTO != null)
        //                {
        //                    Int32 breakLoop = 0;
        //                    while (breakLoop != 1)
        //                    {
        //                        var watchiot = new System.Diagnostics.Stopwatch();
        //                        watchiot.Start();

        //                        GateIoTResult gateIoTResultTemp = await GetLoadingSlipsByStatusFromIoTV2(dimStatusTO.IotStatusId, startLoadingId);
        //                        if (gateIoTResultTemp != null && gateIoTResultTemp.Data != null)
        //                        {
        //                            if (gateIoTResultTemp.Data.Count >= maxRecordPerCylce)
        //                            {
        //                                startLoadingId = Convert.ToInt32(gateIoTResultTemp.Data[gateIoTResultTemp.Data.Count - 1][(Int32)IoTConstants.GateIoTColE.LoadingId]);

        //                                startLoadingId += 1;

        //                            }
        //                            else
        //                            {
        //                                breakLoop = 1;
        //                            }
        //                            gateIoTResult.Data.AddRange(gateIoTResultTemp.Data);
        //                        }
        //                        else
        //                        {
        //                            breakLoop = 1;
        //                        }

        //                        watchiot.Stop();
        //                        long resiot = watchiot.ElapsedMilliseconds;
        //                        totalTimeq += resiot;
        //                    }
        //                }
        //            }
        //        }
        //        totalTime.Add(totalTimeq);
        //        return gateIoTResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        gateIoTResult.DefaultErrorBehavior(0, "Error in GetLoadingStatusHistoryDataFromGateIoT");
        //        return gateIoTResult;
        //    }
        //}

        //Sanjay Gunjal [03-June-2019] Commented as Async and await will not work as Iot Works in queque or sequentially.
        //public static async Task<GateIoTResult> GetLoadingSlipsByStatusFromIoTV2(Int32 statusId, Int32 startLoadingId = 1)
        //{
        //    // ... Target page.

        //    GateIoTResult gateIoTResult = new GateIoTResult();
        //    try
        //    {
        //        string page = "";// Startup.GateIotApiURL + "GetLoadingStatusDataAll?loadingId=" + startLoadingId + "&statusId=" + statusId + "";

        //        // ... Use HttpClient.
        //        using (HttpClient client = new HttpClient())
        //        using (HttpResponseMessage response = await client.GetAsync(page))
        //        using (HttpContent content = response.Content)
        //        {
        //            // ... Read the string.
        //            string result = await content.ReadAsStringAsync();

        //            var resultdata = JsonConvert.DeserializeObject<GateIoTResult>(result);
        //            if (response.StatusCode == HttpStatusCode.OK)
        //            {
        //                if (resultdata != null && resultdata.Code == 1)
        //                {
        //                    gateIoTResult.DefaultSuccessBehavior(1, "Records Received Successfully.", resultdata.Data);
        //                }
        //            }
        //            else
        //            {
        //                gateIoTResult.DefaultErrorBehavior(0, resultdata.Msg);
        //            }

        //            client.Dispose();
        //        }

        //        return gateIoTResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        //public static NodeJsResult PingPongExecution()
        //{
        //    lock (balanceLock)
        //    {
        //        NodeJsResult nodeJsResult = new NodeJsResult();
        //        try
        //        {
        //                String url = Startup.GateIotApiURL + "GetLoadingLayerData";
        //                String result;
        //                WebRequest request = WebRequest.Create(url);
        //                request.Method = "GET";
        //                request.Timeout = 3000;
        //                var response = (HttpWebResponse)request.GetResponseAsync().Result;
        //                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        //                {
        //                    result = sr.ReadToEnd();
        //                    var resultdata = JsonConvert.DeserializeObject<NodeJsResult>(result);
        //                    if (response.StatusCode == HttpStatusCode.OK)
        //                    {
        //                        if (resultdata != null && resultdata.Code == 1)
        //                        {
        //                            nodeJsResult.DefaultSuccessBehavior(1, "data Found", resultdata.Data);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        nodeJsResult.DefaultErrorBehavior(0, resultdata.Msg);
        //                    }
        //                    request.Abort();
        //                    sr.Dispose();
        //                }
        //                return nodeJsResult;
        //            }
        //            else
        //            {
        //                nodeJsResult.DefaultErrorBehavior(0, "Loading id not found");
        //                return nodeJsResult;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            nodeJsResult.DefaultErrorBehavior(0, "Error in GetLoadingLayerData");
        //            return nodeJsResult;
        //        }
        //    }
        //}

        //public static void PingPongExecution(TblGateTO tblGateTO)
        //{
        //    lock (balanceLock)
        //    {
        //        try
        //        {
        //            var queryString = "&PortNumber=" + tblGateTO.PortNumber;
        //            queryString += "&MachineIP=" + tblGateTO.MachineIP;

        //            var request = WebRequest.Create(tblGateTO.IoTUrl + "pingPongMethod" + queryString) as HttpWebRequest;
        //            request.Method = "GET";
        //            request.Timeout = 4000;
        //            var response = (HttpWebResponse)request.GetResponseAsync().Result;
        //            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        //            {
        //                request.Abort();
        //                sr.Dispose();
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //}

        public static List<int[]> GenerateFrameData(TblLoadingTO loadingTO, TblWeighingMeasuresTO weighingMeasureTo, List<TblLoadingSlipExtTO> wtTakentLoadingExtToList)
        {
            List<int[]> frameList = new List<int[]>();
            if (wtTakentLoadingExtToList == null || wtTakentLoadingExtToList.Count == 0)
            {
                FormatIoTFrameToWrite(loadingTO, weighingMeasureTo, wtTakentLoadingExtToList, frameList, 0);
            }
            else
            {
                for (int i = 0; i < wtTakentLoadingExtToList.Count; i++)
                {
                    FormatIoTFrameToWrite(loadingTO, weighingMeasureTo, wtTakentLoadingExtToList, frameList, i);
                }
            }
            return frameList;
        }

        public static List<object[]> GenerateGateIoTFrameData(TblLoadingTO loadingTO, string vehicleNo, Int32 statusId, Int32 transportorId)
        {
            List<object[]> frameList = new List<object[]>();
            FormatStdGateIoTFrameToWrite(loadingTO, vehicleNo, statusId, transportorId, frameList);
            return frameList;
        }

        public static List<object[]> GenerateGateIoTStatusFrameData(TblLoadingTO loadingTO, Int32 statusId)
        {
            List<object[]> frameList = new List<object[]>();
            FormatStatusUpdateGateIoTFrameToWrite(loadingTO, statusId, frameList);
            return frameList;
        }

        private static void FormatIoTFrameToWrite(TblLoadingTO loadingTO, TblWeighingMeasuresTO weighingMeasureTo, List<TblLoadingSlipExtTO> wtTakentLoadingExtToList, List<int[]> frameList, int i)
        {
            try
            {
                var loadingId = loadingTO.ModbusRefId;
                var MeasurTypeId = weighingMeasureTo.WeightMeasurTypeId;
                var layerId = (MeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].LoadingLayerid;
                var itemId = (MeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].ModbusRefId;
                var VehicleWeight = weighingMeasureTo.WeightMT;
                var LoadedWeight = (MeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].LoadedWeight;
                var TareWt = (MeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].CalcTareWeight;
                DateTime serverDt = Constants.ServerDateTime;
                var Day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
                var Hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
                var Minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
                var timeStamp = Convert.ToInt32(Day + "" + Hour + "" + Minute);
                var loadedBundles = (MeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].LoadedBundles;
                frameList.Add(new int[9] { loadingId, layerId, itemId, MeasurTypeId, (int)VehicleWeight, (int)LoadedWeight, (int)TareWt, (int)timeStamp, (int)loadedBundles });

            }
            catch (Exception ex)
            {

            }

        }

        public static Int32 GetDateToTimestap()
        {
            Int32 timeStamp = 0;
            DateTime serverDt = Constants.ServerDateTime;
            var Day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
            var Hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
            var Minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
            return timeStamp = Convert.ToInt32(Day + "" + Hour + "" + Minute);
        }

        private static void FormatStdGateIoTFrameToWrite(TblLoadingTO loadingTO, string vehicleNo, Int32 statusId, Int32 transportorId, List<object[]> frameList)
        {
            try
            {
                var loadingId = loadingTO.ModbusRefId;
                DateTime serverDt = Constants.ServerDateTime;
                var day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
                var month = serverDt.Month.ToString().Length == 1 ? "0" + serverDt.Month.ToString() : serverDt.Month.ToString();
                var year = serverDt.ToString("yy");
                var hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
                var minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
                var timeStamp = (day + "" + month + "" + year + "" + hour + "" + minute).ToString();
                if (timeStamp.Length != 10 || transportorId == 0)
                {
                    frameList = new List<object[]>();
                }
                else
                {
                    frameList.Add(new object[5] { loadingId, vehicleNo, statusId, timeStamp, transportorId });
                }
            }
            catch (Exception ex)
            {
                frameList = new List<object[]>();
            }
        }

        private static void FormatStatusUpdateGateIoTFrameToWrite(TblLoadingTO loadingTO, Int32 statusId, List<object[]> frameList)
        {
            try
            {
                var loadingId = loadingTO.ModbusRefId;
                frameList.Add(new object[2] { loadingId, statusId });
            }
            catch (Exception ex)
            {

            }
        }

        public static string GetIotEncodedStatusIdsForGivenStatus(string statusIds)
        {
            if (statusIds.Equals("7,14,20"))
                return 101 + "";
            if (statusIds.Equals("15,16,25,24"))
                return 102 + "";
            if (statusIds.Equals("15,24"))
                return 103 + "";
            if (statusIds.Equals("0"))
                return 104 + "";
            if (statusIds.Equals("16,25"))
                return 105 + "";

            return statusIds;
        }

        public static string GetIotDecodedStatusIdsForGivenStatus(string statusIds)
        {
            if (statusIds.Equals("101"))
                return "7,14,20";
            if (statusIds.Equals("102"))
                return "15,16,24,25";
            if (statusIds.Equals("103"))
                return "15,24";
            if (statusIds.Equals("104"))
                return 0 + "";
            if (statusIds.Equals("105"))
                return "16,25";

            return statusIds;
        }

        public static GateIoTResult GetDecryptedLoadingId(string dataFrame, string methodName, string URL)
        {
            GateIoTResult gateIOTResult = new GateIoTResult();
            try
            {
                if (String.IsNullOrEmpty(dataFrame))
                {
                    gateIOTResult.DefaultErrorBehavior(0, "transaction ID not found");
                    return gateIOTResult;
                }
                String url = URL + methodName + "?data=" + dataFrame;
                String result;
                System.Net.WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                //request.Timeout = 10000;
                var response = (HttpWebResponse)request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    var resultdata = JsonConvert.DeserializeObject<GateIoTResult>(result);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (resultdata != null && resultdata.Code == 1)
                        {
                            gateIOTResult.DefaultSuccessBehavior(1, "data Found", resultdata.Data);
                        }
                    }
                    else
                    {
                        gateIOTResult.DefaultErrorBehavior(0, resultdata.Msg);
                    }
                    request.Abort();
                    sr.Dispose();
                }
                return gateIOTResult;
            }
            catch (Exception ex)
            {
                gateIOTResult.DefaultErrorBehavior(0, "Error in GetDecryptedLoadingId");
                return gateIOTResult;
            }
        }
    }
}

