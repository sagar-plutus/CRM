using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using PurchaseTrackerAPI.IoT.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.IoT;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.IoT
{
    public class IotCommunication : IIotCommunication
    {
        private readonly IGateCommunication _iGateCommunication;
        private readonly IDimStatusBL _iDimStatusBL;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        private readonly ITblWeighingMachineDAO _iTblWeighingMachineDAO;
        private readonly IWeighingCommunication _iWeighingCommunication;
        private readonly ITblPurchaseScheduleSummaryDAO _iTblPurchaseScheduleSummaryDAO;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;


        public IotCommunication(IWeighingCommunication iWeighingCommunication, Icommondao icommondao, ITblConfigParamsDAO iTblConfigParamsDAO, IGateCommunication iGateCommunication, ITblPurchaseScheduleSummaryDAO iTblPurchaseScheduleSummaryDAO, IDimStatusBL iDimStatusBL, ITblOrganizationBL iTblOrganizationBL, ITblWeighingMachineDAO iTblWeighingMachineDAO)
        {
            _iCommonDAO = icommondao;
            _iGateCommunication = iGateCommunication;
            _iDimStatusBL = iDimStatusBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblWeighingMachineDAO = iTblWeighingMachineDAO;
            _iWeighingCommunication = iWeighingCommunication;
            _iTblPurchaseScheduleSummaryDAO = iTblPurchaseScheduleSummaryDAO;

        }
        public TblPurchaseScheduleSummaryTO GetItemDataFromIotAndMerge(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)
        {
            int confiqId = _iTblConfigParamsDAO.IoTSetting();
            if (confiqId == Convert.ToInt32(StaticStuff.Constants.WeighingDataSourceE.IoT))
            {
                if (tblPurchaseScheduleSummaryTO != null)
                {
                    if (tblPurchaseScheduleSummaryTO.StatusId == (Int32)StaticStuff.Constants.TranStatusE.VEHICLE_OUT
                       || tblPurchaseScheduleSummaryTO.StatusId == (Int32)StaticStuff.Constants.TranStatusE.VEHICLE_REJECTED_BEFORE_WEIGHING)
                    {
                        return tblPurchaseScheduleSummaryTO;
                    }
                    //Call To Gate IoT For Vehicle & Transport Details
                    GateIoTResult gateIoTResult = _iGateCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblPurchaseScheduleSummaryTO);
                    if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count != 0)
                    {
                        //tblPurchaseScheduleSummaryTO.VehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
                        tblPurchaseScheduleSummaryTO.VehicleNo = GetVehicleNumbers((string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo], true);//chetan[10-feb-2020] add for write old vehicle on IOT
                        //tblLoadingTO.TransporterOrgId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.TransportorId]);
                        String statusDate = (String)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusDate];
                        //Int32 statusId = 1;
                        //if (tblPurchaseScheduleSummaryTO.RootScheduleId != 0)
                        Int32 statusId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusId]);

                        DimStatusTO dimStatusTO = _iDimStatusBL.SelectDimStatusTOByIotStatusId(statusId);
                        //tblPurchaseScheduleSummaryTO.StatusDate = IoTDateTimeStringToDate(statusDate);

                        if (dimStatusTO != null)
                        {
                            tblPurchaseScheduleSummaryTO.StatusId = dimStatusTO.IdStatus;
                            tblPurchaseScheduleSummaryTO.StatusDesc = dimStatusTO.StatusName;
                        }

                    }
                }
            }

            return tblPurchaseScheduleSummaryTO;
        }

        public List<TblPurchaseScheduleSummaryTO> GetItemDataFromIotAndMergeMulti(List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryList)
        {
            List<TblPurchaseScheduleSummaryTO> scheduleTOList = new List<TblPurchaseScheduleSummaryTO>();
            List<TblPurchaseScheduleSummaryTO> scheduleTOList1 = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryList != null && tblPurchaseScheduleSummaryList.Count > 0)
            {
                //Call To Gate IoT For Vehicle & Transport Details
               
                GateIoTResult gateIoTResult = _iGateCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblPurchaseScheduleSummaryList[0]);
                if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count != 0)
                {
                    tblPurchaseScheduleSummaryList.ForEach(c => {
                        c.VehicleNo = GetVehicleNumbers((string)(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo]), true);
                    });
                  
                    scheduleTOList1 = tblPurchaseScheduleSummaryList.Where(a => a.VehiclePhaseId > 0).ToList();
                    var data = tblPurchaseScheduleSummaryList.Where(w => w.RootScheduleId == w.IdPurchaseScheduleSummary).ToList().FirstOrDefault();
                    //var data = tblPurchaseScheduleSummaryList.OrderBy(w => w.CreatedOn).ToList();
                    if (data != null)
                        scheduleTOList.Add(data);
                    if (scheduleTOList1.Count > 1)
                    {
                        List<DropDownTO> phasesList = _iTblPurchaseScheduleSummaryDAO.getListofPhasesUsedForUnloadingQty();
                        if (phasesList.Count > 0)
                            for (int i = 0; i < phasesList.Count; i++)
                            {
                                List<TblPurchaseScheduleSummaryTO> filterList = phaseWiseFilterList(scheduleTOList1, (Int32)StaticStuff.Constants.TranStatusE.UNLOADING_COMPLETED, Convert.ToInt32(phasesList[i].Value));
                                if (filterList.Count > 0)
                                    scheduleTOList.AddRange(filterList);
                            }
                    }else
                    {
                        TblPurchaseScheduleSummaryTO IsLast = tblPurchaseScheduleSummaryList.Where(s=>s.IsActive == 1).ToList().FirstOrDefault();
                        if (IsLast != null && IsLast.VehiclePhaseId > 0 && IsLast.VehiclePhaseId != (Int32)StaticStuff.Constants.PurchaseVehiclePhasesE.OUTSIDE_INSPECTION)
                        {
                            List<TblPurchaseScheduleSummaryTO> filterList = phaseWiseFilterList(scheduleTOList1, (Int32)StaticStuff.Constants.TranStatusE.UNLOADING_COMPLETED, IsLast.VehiclePhaseId);
                            if (filterList.Count > 0)
                                scheduleTOList.AddRange(filterList);
                        }
                            else
                        {
                            int statusId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusId]);
                            DimStatusTO dimStatusTO = _iDimStatusBL.SelectDimStatusTOByIotStatusId(statusId);
                            if (dimStatusTO != null && IsLast != null)
                            {
                                IsLast.StatusId = dimStatusTO.IdStatus;
                                IsLast.StatusDesc = dimStatusTO.StatusName;
                                scheduleTOList.Add(IsLast);
                            }
                        }
                    }
                }
              
            }
            List < TblPurchaseScheduleSummaryTO > remainingItems = tblPurchaseScheduleSummaryList.Where(n => scheduleTOList.Any(o => o.IdPurchaseScheduleSummary == n.IdPurchaseScheduleSummary)).ToList();
            scheduleTOList.AddRange(remainingItems);
            return scheduleTOList;
        }

        public  List<TblPurchaseScheduleSummaryTO> phaseWiseFilterList(List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryList, int statusId,int phaseId)
        {
            List<TblPurchaseScheduleSummaryTO> filterList = tblPurchaseScheduleSummaryList.Where(a => a.VehiclePhaseId == phaseId).ToList();
            if (filterList.Count > 0)
            {
                if (filterList.Count > 1)
                {
                    //Prajakta[2021-05-20]Commented and added to get two records for correction for isBoth vehcile
                    //TblPurchaseScheduleSummaryTO tempUnloadingSummaryTo = filterList.Where(c => c.IsActive != 1).ToList().FirstOrDefault();
                    //DimStatusTO dimStatusTO = _iDimStatusBL.SelectDimStatusTO(statusId);
                    //if (dimStatusTO != null)
                    //{
                    //    tempUnloadingSummaryTo.StatusId = dimStatusTO.IdStatus;
                    //    tempUnloadingSummaryTo.StatusDesc = dimStatusTO.StatusName;
                    //}

                    //List<TblPurchaseScheduleSummaryTO> scheduleList = filterList.Where(c => c.IsActive != 1).ToList();
                    List<TblPurchaseScheduleSummaryTO> scheduleList = filterList;
                    for (int i = 0; i < scheduleList.Count; i++)
                    {
                        TblPurchaseScheduleSummaryTO tempUnloadingSummaryTo = scheduleList[i];
                        DimStatusTO dimStatusTO = _iDimStatusBL.SelectDimStatusTO(statusId);
                        if (dimStatusTO != null)
                        {
                            tempUnloadingSummaryTo.StatusId = dimStatusTO.IdStatus;
                            tempUnloadingSummaryTo.StatusDesc = dimStatusTO.StatusName;
                        }
                    }
                   
                    TblPurchaseScheduleSummaryTO tempOutSummaryTo = filterList.Where(c => c.IsActive == 1 && c.IsVehicleOut == 1).ToList().FirstOrDefault();
                    DimStatusTO dimStatusTOOut = _iDimStatusBL.SelectDimStatusTO((Int32)StaticStuff.Constants.TranStatusE.VEHICLE_OUT);
                    if (dimStatusTOOut != null)
                    {
                        if(tempOutSummaryTo != null)
                        {
                            tempOutSummaryTo.StatusId = dimStatusTOOut.IdStatus;
                            tempOutSummaryTo.StatusDesc = dimStatusTOOut.StatusName;
                        }
                       
                    }
                }
                else
                {
                    TblPurchaseScheduleSummaryTO tempUnloadingSummaryTo = filterList[0];
                    DimStatusTO dimStatusTO = _iDimStatusBL.SelectDimStatusTO(statusId);
                    if (dimStatusTO != null)
                    {
                        tempUnloadingSummaryTo.StatusId = dimStatusTO.IdStatus;
                        tempUnloadingSummaryTo.StatusDesc = dimStatusTO.StatusName;
                    }
                }
            }

            return filterList;
        }

        public List<TblPurchaseWeighingStageSummaryTO> GetWeightDataFromIotAndMerge(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryList)
        {
            if(tblPurchaseWeighingStageSummaryList.Count == 0)
            {
                return tblPurchaseWeighingStageSummaryList;
            }
            TblWeighingMachineTO machineTO = _iTblWeighingMachineDAO.SelectTblWeighingMachine(tblPurchaseWeighingStageSummaryList[0].WeighingMachineId);
            if (machineTO == null)
            {
                return tblPurchaseWeighingStageSummaryList;
            }
            NodeJsResult itemList = _iWeighingCommunication.GetLoadingLayerData(tblPurchaseScheduleSummaryTO.ModbusRefId, 0, machineTO);
            if (itemList.Data != null)
            {
                if (itemList.Data != null && itemList.Data.Count > 0)
                {
                    for (int f = 0; f < itemList.Data.Count; f++)
                    {
                        var itemRefId = itemList.Data[f][(int)IoTConstants.WeightIotColE.ItemRefNo];
                        TblPurchaseWeighingStageSummaryTO weighingStageSummaryTO = tblPurchaseWeighingStageSummaryList.Where(w => w.WeightStageId == itemRefId).FirstOrDefault();
                        if (weighingStageSummaryTO != null)
                        {
                            weighingStageSummaryTO.GrossWeightMT = itemList.Data[f][(int)IoTConstants.WeightIotColE.GrossWeight];
                            weighingStageSummaryTO.NetWeightMT = itemList.Data[f][(int)IoTConstants.WeightIotColE.NetWeight];
                            weighingStageSummaryTO.ActualWeightMT = itemList.Data[f][(int)IoTConstants.WeightIotColE.UnLoadedWeight];
                            weighingStageSummaryTO.WeightMeasurTypeId = itemList.Data[f][(int)IoTConstants.WeightIotColE.WeighTypeId];
                        }
                    }
                }
            }
            return tblPurchaseWeighingStageSummaryList;
        }


            public DateTime IoTDateTimeStringToDate(string statusDate)
        {
            var dateList = statusDate.Split(',').ToList();
            DateTime serverDate = _iCommonDAO.ServerDateTime;
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

        public List<TblPurchaseScheduleSummaryTO> GetLoadingData(List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryList)
        {

            if (tblPurchaseScheduleSummaryList != null && tblPurchaseScheduleSummaryList.Count > 0)
            {
                for (int i = 0; i < tblPurchaseScheduleSummaryList.Count; i++)
                {
                    GetItemDataFromIotAndMerge(tblPurchaseScheduleSummaryList[i]);
                }
            }

            return tblPurchaseScheduleSummaryList;
        }

       

        public int PostGateAPIDataToModbusTcpApi(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTo, Object[] writeData)
        {

            try
            {
                if (writeData.Length != 5)
                {
                    return 0;
                }
                var tRequest = WebRequest.Create(tblPurchaseScheduleSummaryTo.IoTUrl + "WriteOnGateIoTCommand") as HttpWebRequest;
                return _iGateCommunication.PostGateApiCalls(tblPurchaseScheduleSummaryTo, writeData, tRequest);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int UpdateLoadingStatusOnGateAPIToModbusTcpApi(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTo, Object[] writeData)
        {
            try
            {
                if (writeData.Length != 2)
                {
                    return 0;
                }
                //var tRequest = WebRequest.Create(Startup.GateIotApiURL + "UpdateStatusCommand") as HttpWebRequest;
                var tRequest = WebRequest.Create(tblPurchaseScheduleSummaryTo.IoTUrl + "UpdateStatusCommand") as HttpWebRequest;
                return _iGateCommunication.PostGateApiCalls(tblPurchaseScheduleSummaryTo, writeData, tRequest);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public NodeJsResult DeleteSingleLoadingFromWeightIoTByModBusRefId(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTo, List<TblPurchaseWeighingStageSummaryTO> WeighingStageList)
        {
            NodeJsResult nodeJsResult = new NodeJsResult();
            try
            {
                if (tblPurchaseScheduleSummaryTo != null && tblPurchaseScheduleSummaryTo.ModbusRefId != 0)
                {
                    Int32 modBusRefId = tblPurchaseScheduleSummaryTo.ModbusRefId;

                    if (WeighingStageList != null && WeighingStageList.Count > 0)
                    {
                        WeighingStageList = WeighingStageList.GroupBy(g => g.WeighingMachineId).Select(s => s.FirstOrDefault()).ToList();
                        for (int i = 0; i < WeighingStageList.Count; i++)
                        {
                            TblWeighingMachineTO machineTO = _iTblWeighingMachineDAO.SelectTblWeighingMachine(WeighingStageList[i].WeighingMachineId);
                            if (machineTO == null)
                            {
                                return nodeJsResult;
                            }
                            if (!String.IsNullOrEmpty(machineTO.IoTUrl))
                            {
                                //Addes by kiran for retry 3 times to delete weighing data
                                int cnt2 = 0;
                                while (cnt2 < 3)
                                {
                                    nodeJsResult = _iWeighingCommunication.DeleteSingleLoadingFromWeightIoT(modBusRefId, 0, machineTO);
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

        public GateIoTResult GetLoadingSlipsByStatusFromIoTByStatusId(String statusIds, TblGateTO tblGateTO)
        {
            //Sanjay [03-June-2019] Now Iot communication is shifted to TCP/IP and hence data length is increased.
            //It will return max  87 per call
            //Int32 maxRecordPerCylce = 24;
            Int32 maxRecordPerCylce = 83;
            // List<DimStatusTO> dimStatusTOList = _iDimStatusBL.SelectAllDimStatusList();
            //chetan[24-feb-2020] added for get status transcation type wise.
            int transactionTypeId = (int)PurchaseTrackerAPI.StaticStuff.Constants.TransactionTypeE.SAUDA;
            List<DimStatusTO> dimStatusTOList = _iDimStatusBL.SelectAllDimStatusList(transactionTypeId);

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
                                    gateIoTResultTemp = _iGateCommunication.GetAllLoadingSlipsByStatusFromIoT(Convert.ToInt32(statusIds), tblGateTO, startLoadingId);
                                else
                                    gateIoTResultTemp = _iGateCommunication.GetLoadingSlipsByStatusFromIoT(dimStatusTO.IotStatusId, tblGateTO, startLoadingId);

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

        public List<int[]> GenerateFrameData(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblPurchaseWeighingStageSummaryTO weighingMeasureTo)
        {
            return FormatIoTFrameToWrite(tblPurchaseScheduleSummaryTO, weighingMeasureTo);   
        }

        public List<object[]> GenerateGateIoTFrameData(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, string vehicleNo, Int32 statusId)
        {
            List<object[]> frameList = new List<object[]>();
            vehicleNo = GetVehicleNumbers(vehicleNo, false);//chetan[10-feb-2020] add for write old vehicle on IOT
            FormatStdGateIoTFrameToWrite(tblPurchaseScheduleSummaryTO, vehicleNo, statusId, 0, frameList);
            return frameList;
        }

        public List<object[]> GenerateGateIoTStatusFrameData(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Int32 statusId)
        {
            List<object[]> frameList = new List<object[]>();
            FormatStatusUpdateGateIoTFrameToWrite(tblPurchaseScheduleSummaryTO, statusId, frameList);
            return frameList;
        }

        public List<int[]> FormatIoTFrameToWrite(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblPurchaseWeighingStageSummaryTO weighingMeasureTo)
        {
            try
            {
                List<int[]> frameList = new List<int[]>();
                var PurchaseScheduleSummaryId = tblPurchaseScheduleSummaryTO.ModbusRefId;
                var MeasurTypeId = weighingMeasureTo.WeightMeasurTypeId;
                var layerId = (MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : 0;//wtTakentLoadingExtToList[i].LoadingLayerid;
                var itemId = weighingMeasureTo.WeightStageId;//(MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : wtTakentLoadingExtToList[i].ModbusRefId;
                var GrossWeight = weighingMeasureTo.GrossWeightMT;
                var UnLoadedWeight =  weighingMeasureTo.ActualWeightMT;
                var NetWeight =  weighingMeasureTo.NetWeightMT;
                DateTime serverDt = _iCommonDAO.ServerDateTime;
                var Day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
                var Hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
                var Minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
                var timeStamp = Convert.ToInt32(Day + "" + Hour + "" + Minute);
                var loadedBundles = (MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)StaticStuff.Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : 0;//wtTakentLoadingExtToList[i].LoadedBundles;
                frameList.Add(new int[9] { PurchaseScheduleSummaryId, layerId, itemId, MeasurTypeId, (int)GrossWeight,(int)UnLoadedWeight, (int)NetWeight, (int)timeStamp, (int)loadedBundles });
                return frameList;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Int32 GetDateToTimestap()
        {
            Int32 timeStamp = 0;
            DateTime serverDt = _iCommonDAO.ServerDateTime;
            var Day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
            var Hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
            var Minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
            return timeStamp = Convert.ToInt32(Day + "" + Hour + "" + Minute);
        }

        public void FormatStdGateIoTFrameToWrite(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, string vehicleNo, Int32 statusId, Int32 transportorId, List<object[]> frameList)
        {
            try
            {
                var loadingId = tblPurchaseScheduleSummaryTO.ModbusRefId;
                DateTime serverDt = _iCommonDAO.ServerDateTime;
                var day = serverDt.Day.ToString().Length == 1 ? "0" + serverDt.Day.ToString() : serverDt.Day.ToString();
                var month = serverDt.Month.ToString().Length == 1 ? "0" + serverDt.Month.ToString() : serverDt.Month.ToString();
                var year = serverDt.ToString("yy");
                var hour = serverDt.Hour.ToString().Length == 1 ? "0" + serverDt.Hour.ToString() : serverDt.Hour.ToString();
                var minute = serverDt.Minute.ToString().Length == 1 ? "0" + serverDt.Minute.ToString() : serverDt.Minute.ToString();
                var timeStamp = (day + "" + month + "" + year + "" + hour + "" + minute).ToString();
                if (timeStamp.Length != 10 )// || transportorId == 0)
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

        public void FormatStatusUpdateGateIoTFrameToWrite(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Int32 statusId, List<object[]> frameList)
        {
            try
            {
                var loadingId = tblPurchaseScheduleSummaryTO.ModbusRefId;
                frameList.Add(new object[2] { loadingId, statusId });
            }
            catch (Exception ex)
            {

            }
        }

        public string GetIotEncodedStatusIdsForGivenStatus(string statusIds)
        {
            //Commented for get all data one time
            //if (statusIds.Equals("501,502,503,504,505,508,524,530"))
            //    return 104 + "";
            //if (statusIds.Equals("533,509,519,521,531"))
            //    return 104 + "";
            //if (statusIds.Equals("502"))
            //    return 104 + "";
            //if (statusIds.Equals("505,508,509"))
            //    return 104 + "";
            //if (statusIds.Equals("0"))
            //    return 104 + "";
            //if (statusIds.Equals("16,25"))
            //    return 104 + "";

            return "104";
        }

        public string GetIotDecodedStatusIdsForGivenStatus(string statusIds)
        {
            if (statusIds.Equals("101"))
                return "7,14,20";
            if (statusIds.Equals("102"))
                return "15,16,24,25,26";
            if (statusIds.Equals("103"))
                return "15,24";
            if (statusIds.Equals("104"))
                return 0 + "";
            if (statusIds.Equals("105"))
                return "16,25";

            return statusIds;
        }

        public GateIoTResult GetDecryptedLoadingId(string dataFrame, string methodName, string URL)
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

        public void GetItemDataFromIotForGivenLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            //if (tblLoadingSlipTO != null)
            //{

            //    if (tblLoadingSlipTO.TranStatusE == StaticStuff.Constants.TranStatusE.LOADING_DELIVERED)
            //        return;

            //    if (true)
            //    {
            //        //List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();
            //        List<TblWeighingMachineTO> tblWeighingMachineList = _iTblWeighingMachineDAO.SelectAllTblWeighingMachineOfWeighingList(tblLoadingSlipTO.LoadingId);

            //        List<TblLoadingSlipExtTO> totalLoadingSlipExtList = tblLoadingSlipTO.LoadingSlipExtTOList;

            //        TblLoadingTO loadingTO = _iTblLoadingDAO.SelectTblLoading(tblLoadingSlipTO.LoadingId);

            //        GateIoTResult gateIoTResult = _iGateCommunication.GetLoadingStatusHistoryDataFromGateIoT(loadingTO);
            //        if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count != 0)
            //        {
            //            tblLoadingSlipTO.VehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
            //            tblLoadingSlipTO.TransporterOrgId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.TransportorId]);
            //         }

            //        //var layerList = totalLoadingSlipExtList.GroupBy(x => x.LoadingLayerid).ToList();
            //        //List<int> totalLayerList = new List<int>();
            //        //if (!totalLayerList.Contains(0))
            //        //    totalLayerList.Add(0);
            //        List<int> totalLayerList = new List<int>();
            //        //var tLayerList = tblWeighingMachineList.GroupBy(test => test.LayerId)
            //        //                   .Select(grp => grp.First()).ToList();
            //        var tLayerList = totalLoadingSlipExtList.GroupBy(x => x.LoadingLayerid).Select(grp => grp.First()).ToList();
            //        foreach (var item in tLayerList)
            //        {
            //            if (item.LoadingLayerid != 0)
            //                totalLayerList.Add(item.LoadingLayerid);
            //        }
            //        var distinctWeighingMachineList = tblWeighingMachineList.GroupBy(test => test.IdWeighingMachine)
            //                              .Select(grp => grp.First()).ToList();
            //        //foreach (var item in layerList)
            //        //{
            //        //    totalLayerList.Add(item.Key);
            //        //}

            //        //Sanjay [03-June-2019] Now Layerwise call will not be required as data will be received from TCP/ip communication
            //        //Now pass layerid=0. IoT Code will internally give data for all layers.
            //        //for (int i = 0; i < totalLayerList.Count; i++)
            //        //{
            //        //int layerid = totalLayerList[i];
            //        int layerid = 0;
            //        Int32 loadingId = loadingTO.ModbusRefId;
            //        for (int mc = 0; mc < distinctWeighingMachineList.Count; mc++)
            //        {
            //            //Call to Weight IoT
            //            NodeJsResult itemList = _iWeighingCommunication.GetLoadingLayerData(loadingId, layerid, distinctWeighingMachineList[mc]);
            //            if (itemList.Data != null)
            //            {

            //                if (itemList.Data != null && itemList.Data.Count > 0)
            //                {
            //                    for (int f = 0; f < itemList.Data.Count; f++)
            //                    {
            //                        var itemRefId = itemList.Data[f][(int)IoTConstants.WeightIotColE.ItemRefNo];
            //                        var itemTO = totalLoadingSlipExtList.Where(w => w.ModbusRefId == itemRefId).FirstOrDefault();
            //                        if (itemTO != null)
            //                        {
            //                            itemTO.LoadedWeight = itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedWt];
            //                            itemTO.CalcTareWeight = itemList.Data[f][(int)IoTConstants.WeightIotColE.CalcTareWt];
            //                            itemTO.LoadedBundles = itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedBundle];
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        //}
            //    }
            //}
        }
        public List<TblPurchaseScheduleSummaryTO> setGateDetailsFormIoT(string statusId, List<DimStatusTO> statusList, List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList)
        {
            if(tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
            {
                return null;
            }

            List<TblPurchaseScheduleSummaryTO> distGate = tblPurchaseScheduleSummaryTOList.GroupBy(g => g.GateId).Select(s => s.FirstOrDefault()).ToList();

            GateIoTResult gateIoTResult = new GateIoTResult();
            //GetIotEncodedStatusIdsForGivenStatus for DB status To IoT status
            string finalStatusId = GetIotEncodedStatusIdsForGivenStatus(statusId);
            for (int g = 0; g < distGate.Count; g++)
            {
                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTOTemp = distGate[g];
                TblGateTO tblGateTO = new TblGateTO(tblPurchaseScheduleSummaryTOTemp.GateId, tblPurchaseScheduleSummaryTOTemp.IoTUrl, tblPurchaseScheduleSummaryTOTemp.MachineIP, tblPurchaseScheduleSummaryTOTemp.PortNumber);
                GateIoTResult temp = GetLoadingSlipsByStatusFromIoTByStatusId(finalStatusId, tblGateTO);

                if (temp != null && temp.Data != null)
                {
                    gateIoTResult.Data.AddRange(temp.Data);
                }
            }
            if (gateIoTResult != null && gateIoTResult.Data != null)
            {
                for (int d = 0; d < tblPurchaseScheduleSummaryTOList.Count; d++)
                {
                    if(tblPurchaseScheduleSummaryTOList[d].StatusId == (Int32)StaticStuff.Constants.TranStatusE.VEHICLE_OUT
                    || tblPurchaseScheduleSummaryTOList[d].StatusId == (Int32)StaticStuff.Constants.TranStatusE.VEHICLE_REJECTED_BEFORE_WEIGHING)
                    {
                        continue;
                    }
                    
                    var data = gateIoTResult.Data.Where(w => Convert.ToInt32(w[0]) == tblPurchaseScheduleSummaryTOList[d].ModbusRefId).FirstOrDefault();
                    if (data != null)
                    {
                        //tblPurchaseScheduleSummaryTOList[d].VehicleNo = Convert.ToString(data[(int)IoTConstants.GateIoTColE.VehicleNo]);
                        tblPurchaseScheduleSummaryTOList[d].VehicleNo =GetVehicleNumbers(Convert.ToString(data[(int)IoTConstants.GateIoTColE.VehicleNo]),true);
                        DimStatusTO dimStatusTO = statusList.Where(w => w.IotStatusId == Convert.ToInt32(data[(int)IoTConstants.GateIoTColE.StatusId]) && w.TransactionTypeId == (int)StaticStuff.Constants.txnTypeEnum.SCRAP_VEHICLE_SCHEDULE).FirstOrDefault();
                        if (dimStatusTO != null)
                        {
                            tblPurchaseScheduleSummaryTOList[d].StatusId = dimStatusTO.IdStatus;
                            tblPurchaseScheduleSummaryTOList[d].StatusDesc = dimStatusTO.StatusDesc;
                        }
                    }
                    //}else
                    //{
                    //    tblPurchaseScheduleSummaryTOList.RemoveAt(d);
                    //    d--;
                    //}
                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }

        //chetan[10-feb-2020] added for allow old vehicle on IOT
        public string GetVehicleNumbers(string vehicleNo, Boolean isForSelect)
        {
            if (String.IsNullOrEmpty(vehicleNo))
            {
                return vehicleNo;
            }

            string oldAndNewVehicleNo = vehicleNo;
            string[] splitVehicleNoArr = vehicleNo.Split(" ");
            if (splitVehicleNoArr.Length == 4)
            {
                string stateCodeStr = Convert.ToString(splitVehicleNoArr[0]);
                string stateCodeInt = Convert.ToString(splitVehicleNoArr[1]);
                string numberStr = Convert.ToString(splitVehicleNoArr[2]);
                string numberInt = Convert.ToString(splitVehicleNoArr[3]);

                if (isForSelect)
                {
                    int stateCodeConvertInInt = 0;

                    if (!String.IsNullOrEmpty(stateCodeInt) && stateCodeInt != "-")
                    {
                        stateCodeConvertInInt = Convert.ToInt32(stateCodeInt);
                    }

                    if (stateCodeConvertInInt == 0)
                    {
                        oldAndNewVehicleNo = string.Empty;
                        oldAndNewVehicleNo = stateCodeStr + numberStr.Substring(0, 1) + " - - " + numberInt;
                    }
                    else
                    {
                        if (stateCodeInt.Length == 1)
                        {
                            stateCodeInt = "0" + stateCodeInt;
                        }
                        if (numberInt.Length != 4)
                        {
                            for (int n = numberInt.Length; n < 4; n++)
                            {
                                numberInt = "0" + numberInt;
                            }
                        }

                        if (numberStr.Length == 1)
                        {
                            numberStr = "-" + numberStr;
                        }

                        oldAndNewVehicleNo = stateCodeStr + " " + stateCodeInt + " " + numberStr + " " + numberInt;
                    }
                }
                else
                {
                    if (stateCodeInt.Contains("-") || stateCodeStr.Length == 3 || numberStr.Length == 3)
                    {
                        oldAndNewVehicleNo = string.Empty;
                        if (stateCodeStr.Length == 3)
                        {
                            oldAndNewVehicleNo = stateCodeStr.Substring(0, 2) + " 00 " + stateCodeStr.Substring(2) + "- " + numberInt;
                        }
                        else if (numberStr.Length == 3)
                        {
                            oldAndNewVehicleNo = numberStr.Substring(0, 2) + " 00 " + numberStr.Substring(2, 1) + "- " + numberInt;
                        }
                    }
                }
            }
            return oldAndNewVehicleNo;
        }
    }
    
}

