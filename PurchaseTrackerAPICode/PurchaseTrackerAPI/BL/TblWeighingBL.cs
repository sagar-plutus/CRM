using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.IoT.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.IoT;
using System.Linq;
using Newtonsoft.Json.Linq;
namespace PurchaseTrackerAPI.BL
{
    public class TblWeighingBL : ITblWeighingBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblWeighingDAO _iTblWeighingDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IGateCommunication _iGateCommunication;
        private readonly ITblWeighingMachineDAO _iTblWeighingMachineDAO;
        private readonly ITblGateBL _iTblGateBL;
        private readonly IIotCommunication _iIotCommunication;
        private readonly ITblPurchaseScheduleSummaryBL _tblPurchaseScheduleSummaryBL;
        private readonly IDimStatusDAO _idimStatusDAO;
        private readonly ITblPurchaseWeighingStageSummaryDAO _iTblPurchaseWeighingStageSummaryDAO;
        private readonly IWeighingCommunication _iWeighingCommunication;
        public TblWeighingBL(ITblWeighingDAO iTblWeighingDAO, Icommondao icommondao, 
            ITblConfigParamsDAO iTblConfigParamsDAO, IGateCommunication iGateCommunication, 
            ITblWeighingMachineDAO iTblWeighingMachineDAO,
                              ITblGateBL iTblGateBL, IIotCommunication iIotCommunication
                             , ITblPurchaseScheduleSummaryBL tblPurchaseScheduleSummaryBL,
                             IDimStatusDAO idimStatusDAO, ITblPurchaseWeighingStageSummaryDAO iTblPurchaseWeighingStageSummaryDAO,
                             IWeighingCommunication iWeighingCommunication)
        {
            _iCommonDAO = icommondao;
            _iTblWeighingDAO = iTblWeighingDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iGateCommunication = iGateCommunication;
            _iTblWeighingMachineDAO = iTblWeighingMachineDAO;
            _iTblGateBL = iTblGateBL;
            _iIotCommunication = iIotCommunication;
            _tblPurchaseScheduleSummaryBL = tblPurchaseScheduleSummaryBL;
            _idimStatusDAO = idimStatusDAO;
            _iTblPurchaseWeighingStageSummaryDAO = iTblPurchaseWeighingStageSummaryDAO;
            _iWeighingCommunication = iWeighingCommunication;
        }
        #region Selection
        public  List<TblWeighingTO> SelectAllTblWeighing()
        {
            return _iTblWeighingDAO.SelectAllTblWeighing();
        }

        

        public  TblWeighingTO SelectTblWeighingTO(Int32 idWeighing)
        {
            return _iTblWeighingDAO.SelectTblWeighing(idWeighing);
        }

        public  TblWeighingTO SelectTblWeighingByMachineIp(string ipAddr,int machineId)
        {
            TblWeighingTO tblWeighingTO = new TblWeighingTO();
            int confiqId = _iTblConfigParamsDAO.IoTSetting();
            if (machineId > 0 && confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                TblWeighingMachineTO tblWeighingMachineTO = _iTblWeighingMachineDAO.SelectTblWeighingMachine(machineId);
                if (tblWeighingMachineTO == null)
                {
                    return null;
                }
                string weight = _iGateCommunication.ReadWeightFromWeightIoT(tblWeighingMachineTO);
                tblWeighingTO.Measurement = weight;
                tblWeighingTO.MachineIp = tblWeighingMachineTO.MachineIP;
                tblWeighingTO.TimeStamp = _iCommonDAO.ServerDateTime;
                return tblWeighingTO;
            }
            DateTime serverDateTime =  _iCommonDAO.ServerDateTime;
            DateTime defaultTime1 = serverDateTime.AddHours(15);
            tblWeighingTO = _iTblWeighingDAO.SelectTblWeighingByMachineIp(ipAddr, defaultTime1);
            if (tblWeighingTO == null)
            {
                return null;
            }
            //DateTime dt = DateTime.Now.AddMinutes(-10);
            TimeSpan CurrentdateTime = serverDateTime.TimeOfDay;
            TimeSpan weighingTime = tblWeighingTO.TimeStamp.TimeOfDay;
            //TimeSpan diffTime = CurrentdateTime - toDateTime;
            TimeSpan defaultTime = CurrentdateTime.Add(new TimeSpan(-2, -30, -30));
            if (weighingTime == TimeSpan.Zero || weighingTime < defaultTime)
            {
                return null;
            }
            else
            {
                DeleteTblWeighingByByMachineIp(ipAddr);
            }
           
            return tblWeighingTO;

        }


        #endregion

        #region Insertion
        public  int InsertTblWeighing(TblWeighingTO tblWeighingTO)
        {
            return _iTblWeighingDAO.InsertTblWeighing(tblWeighingTO);
        }

        public  int InsertTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingDAO.InsertTblWeighing(tblWeighingTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblWeighing(TblWeighingTO tblWeighingTO)
        {
            return _iTblWeighingDAO.UpdateTblWeighing(tblWeighingTO);
        }

        public  int UpdateTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingDAO.UpdateTblWeighing(tblWeighingTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblWeighing(Int32 idWeighing)
        {
            return _iTblWeighingDAO.DeleteTblWeighing(idWeighing);
        }
        /// <summary>
        /// GJ@20170830 : Remove the all previous weighing measured records from tables againest IpAddr
        /// </summary>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        public  int DeleteTblWeighingByByMachineIp(string ipAddr)
        {
            try
            {              
                return _iTblWeighingDAO.DeleteTblWeighingByByMachineIp(ipAddr);                
            }
            catch (Exception)
            {
                return 0;
            }
            
        }

        public  int DeleteTblWeighing(Int32 idWeighing, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWeighingDAO.DeleteTblWeighing(idWeighing, conn, tran);
        }

        #endregion


        //chetan[20-jan-2020] added for data write on db
        public int FetchFromIotAndWriteDB()
        {

            List<TblMachineBackupTO> list = TblWeighingDAO.GetALLMachineData(1);
            if (list != null)
            {
                int cnt = 0;
                foreach (var item in list)
                {
                    //if(cnt == 0)
                    //{
                    int a = FetchFromIotAndWrite(item);
                    cnt++;
                    //}
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int FetchFromIotAndWrite(TblMachineBackupTO tblMachineBackupTO)
        {
            TblGateTO tblGateTO = _iTblGateBL.GetDefaultTblGateTO();
            if (tblGateTO != null)
            {
                List<TblInvoiceTO> tblInvoiceTOListAll = new List<TblInvoiceTO>();
                string IOTframe = "";
                int result = 0;
                SqlConnection recycleConnection = new SqlConnection(Startup.ConnectionString);
                SqlTransaction recycleTran = null;
                try
                {
                    #region All Activity Related To Recycle And Weighing Data
                    recycleConnection.Open();
                    recycleTran = recycleConnection.BeginTransaction();
                    if (!String.IsNullOrEmpty(tblMachineBackupTO.Machinedata))
                    {
                        var frames = new ArraySegment<string>(tblMachineBackupTO.Machinedata.Split(","), 0, 8);
                        IOTframe = string.Join(",", frames.ToArray());
                        GateIoTResult gateIoTResult = _iIotCommunication.GetDecryptedLoadingId(IOTframe, "GetLoadingDecriptionData", tblGateTO.IoTUrl);
                        if (gateIoTResult != null && gateIoTResult.Code != 0 && gateIoTResult.Data.Count > 0)
                        {
                            #region Update All Loading Slip details From IoT
                            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _tblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryTOByModBusRefId(Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.LoadingId]));
                            if (tblPurchaseScheduleSummaryTOList != null)
                            {
                                for (int i = 0; i < tblPurchaseScheduleSummaryTOList.Count; i++)
                                {
                                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = tblPurchaseScheduleSummaryTOList[i];
                                    tblPurchaseScheduleSummaryTO.VehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
                                    // tblPurchaseScheduleSummaryTO.SupplierId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.TransportorId]);//transporterName
                                    String statusDate = (String)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusDate];

                                    Int32 statusId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusId]);

                                    DimStatusTO dimStatusTO = _idimStatusDAO.SelectDimStatusTOByIotStatusId(statusId, recycleConnection, recycleTran);
                                    tblPurchaseScheduleSummaryTO.ScheduleDate = _iIotCommunication.IoTDateTimeStringToDate(statusDate);
                                    if (dimStatusTO != null)
                                    {
                                        tblPurchaseScheduleSummaryTO.StatusId = dimStatusTO.IdStatus;
                                        //tblPurchaseScheduleSummaryTO.StatusReason = dimStatusTO.StatusName;
                                    }
                                    result = _tblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO, recycleConnection, recycleTran);
                                    if (result != 1)
                                    {
                                        recycleTran.Rollback();
                                        return 0;
                                    }
                                }


                                recycleTran.Commit();

                                #endregion
                            }

                        }
                        return result;
                    }
                    else
                    {
                        return 0;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    recycleTran.Rollback();
                    return 0;
                }
                finally
                {
                    recycleConnection.Close();
                }
            }
            return 0;
        }

        public int GetCalculatedLayerData(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran)
        {
            int resultTemp = 0;
            List<int> portList = new List<int>();
            List<String> layerWiseData = TblWeighingDAO.SelectlayerWiseData(Math.Floor(Convert.ToDecimal(tblPurchaseScheduleSummaryTO.ModbusRefId * 256)), portList);
            if (layerWiseData != null && layerWiseData.Count > 0)
            {
                for (int u = 0; u < layerWiseData.Count; u++)
                {
                    GateIoTResult itemList = _iIotCommunication.GetDecryptedLoadingId(layerWiseData[u], "GetLoadingDecriptedLayerData", tblGateTO.IoTUrl);
                    List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = _iTblPurchaseWeighingStageSummaryDAO.GetVehWtDetailsForWeighingMachine(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary, conn, tran);

                    if (itemList.Data != null && itemList.Data.Count > 0)
                    {
                        for (int f = 0; f < itemList.Data.Count; f++)
                        {
                            #region Insert In Temp Weighing Messure

                            TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = new TblPurchaseWeighingStageSummaryTO();
                            int layerId = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.LayerId]);
                            int weighTypeId = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.WeighTypeId]);
                            int purchaseScheduleId = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadingId]);
                            double grossWeight = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.GrossWeight]);
                            double unLoadedWeight = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.UnLoadedWeight]);
                            double netWeight = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.NetWeight]);

                            tblPurchaseWeighingStageSummaryTO = tblPurchaseWeighingStageSummaryTOList.Where(w => w.PurchaseScheduleSummaryId == purchaseScheduleId && w.WeightStageId == layerId).FirstOrDefault();
                            tblPurchaseWeighingStageSummaryTO.GrossWeightMT = grossWeight;
                            tblPurchaseWeighingStageSummaryTO.ActualWeightMT = unLoadedWeight;
                            tblPurchaseWeighingStageSummaryTO.NetWeightMT = netWeight;
                            tblPurchaseWeighingStageSummaryTO.VehicleNo = tblPurchaseScheduleSummaryTO.VehicleNo;

                            resultTemp = _iTblPurchaseWeighingStageSummaryDAO.UpdateTblPurchaseWeighingStageSummary(tblPurchaseWeighingStageSummaryTO, conn, tran);

                            if (resultTemp != 1)
                            {
                                return 0;
                            }
                            #endregion
                        }
                    }
                }
            }
            return 1;
        }
        //RestoreDBToGATEIOT
        public ResultMessage RestoreToIOT()
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            TblGateTO tblGateTO = _iTblGateBL.GetDefaultTblGateTO();
            Int32 result = 0;
            try
            {
                String statusids = string.Empty;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_DB_TO_IOT_STATUS_IDS);
                if (tblConfigParamsTO != null && (!string.IsNullOrEmpty(tblConfigParamsTO.ConfigParamVal.ToString())))
                {
                    statusids = Convert.ToString(tblConfigParamsTO.ConfigParamVal);
                }
                List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _tblPurchaseScheduleSummaryBL.SelectAllTblPurchaseScheduleSummaryTOListFromStatusIds(statusids);
                for (int i = 0; i < tblPurchaseScheduleSummaryTOList.Count; i++)
                {
                    try
                    {
                        conn.Open();
                        tran = conn.BeginTransaction();
                        TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = tblPurchaseScheduleSummaryTOList[i];
                        tblPurchaseScheduleSummaryTO.IoTUrl = tblGateTO.IoTUrl;
                        tblPurchaseScheduleSummaryTO.PortNumber = tblGateTO.PortNumber;
                        tblPurchaseScheduleSummaryTO.MachineIP = tblGateTO.MachineIP;
                        tblPurchaseScheduleSummaryTO.GateId = tblGateTO.IdGate;
                        if (tblPurchaseScheduleSummaryTO.ModbusRefId > 0)
                            continue;
                        if (string.IsNullOrEmpty(tblPurchaseScheduleSummaryTO.VehicleNo))
                            continue;
                        tblPurchaseScheduleSummaryTO.ModbusRefId = _iCommonDAO.GetNextAvailableModRefIdNew();
                        int purchaseScheduleId = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;
                        List<TblPurchaseScheduleSummaryTO> allScheduleList = _tblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootId(purchaseScheduleId, conn, tran);
                        List<object[]> frameList = new List<object[]>();
                        List<object[]> updateStatusframeList = new List<object[]>();
                        if (allScheduleList != null && allScheduleList.Count > 0)
                        {
                            int k = 0;
                            allScheduleList = allScheduleList.OrderBy(s => s.StatusId).ToList();
                            TblPurchaseScheduleSummaryTO tblSummaryTO = allScheduleList[k];
                            DimStatusTO statusTO = _idimStatusDAO.SelectDimStatus(tblSummaryTO.StatusId, conn, tran);
                            tblSummaryTO.IoTUrl = tblGateTO.IoTUrl;
                            tblSummaryTO.PortNumber = tblGateTO.PortNumber;
                            tblSummaryTO.MachineIP = tblGateTO.MachineIP;
                            tblSummaryTO.GateId = tblGateTO.IdGate;
                            tblSummaryTO.ModbusRefId = tblPurchaseScheduleSummaryTO.ModbusRefId;
                            frameList.AddRange(_iIotCommunication.GenerateGateIoTFrameData(tblSummaryTO, tblPurchaseScheduleSummaryTO.VehicleNo, statusTO.IotStatusId));

                            // tblSummaryTO.VehicleNo = string.Empty;
                            tblSummaryTO.StatusId = (Int32)Constants.TranStatusE.New;
                            result = _tblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummary(tblSummaryTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error while UpdateTblPurchaseScheduleSummary.");
                                return resultMessage;
                            }
                            for (; k < allScheduleList.Count; k++)
                            {
                                tblSummaryTO = allScheduleList[k];
                                tblSummaryTO.IoTUrl = tblGateTO.IoTUrl;
                                tblSummaryTO.PortNumber = tblGateTO.PortNumber;
                                tblSummaryTO.MachineIP = tblGateTO.MachineIP;
                                tblSummaryTO.GateId = tblGateTO.IdGate;
                                tblSummaryTO.ModbusRefId = tblPurchaseScheduleSummaryTO.ModbusRefId;
                                statusTO = _idimStatusDAO.SelectDimStatus(tblSummaryTO.StatusId, conn, tran);
                                //tblSummaryTO.IoTUrl = tblGateTO.IoTUrl;
                                //tblSummaryTO.PortNumber = tblGateTO.PortNumber;
                                //tblSummaryTO.MachineIP = tblGateTO.MachineIP;
                                //tblSummaryTO.GateId = tblGateTO.IdGate;
                                //tblSummaryTO.ModbusRefId = tblPurchaseScheduleSummaryTO.ModbusRefId;
                                object[] statusframeTO = new object[2] { tblPurchaseScheduleSummaryTO.ModbusRefId, statusTO.IotStatusId };
                                updateStatusframeList.Add(statusframeTO);
                                // tblSummaryTO.VehicleNo = string.Empty;
                                tblSummaryTO.StatusId = (Int32)Constants.TranStatusE.New;
                                result = _tblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummary(tblSummaryTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error while UpdateTblPurchaseScheduleSummary.");
                                    return resultMessage;
                                }
                            }
                        }
                        if (frameList != null && frameList.Count > 0)
                        {
                            for (int f = 0; f < frameList.Count; f++)
                            {
                                if (f == 0)
                                    result = _iIotCommunication.PostGateAPIDataToModbusTcpApi(tblPurchaseScheduleSummaryTO, frameList[f]);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error while PostGateAPIDataToModbusTcpApi");
                                    resultMessage.DisplayMessage = "Failed due to network error, Please try one more time";
                                    return resultMessage;
                                }
                            }
                            if (updateStatusframeList != null && updateStatusframeList.Count > 0)
                            {
                                for (int j = 0; j < updateStatusframeList.Count; j++)
                                {
                                    result = _iIotCommunication.UpdateLoadingStatusOnGateAPIToModbusTcpApi(tblPurchaseScheduleSummaryTO, updateStatusframeList[j]);
                                    if (result != 1)
                                    {
                                        resultMessage.DefaultBehaviour();
                                        return resultMessage;
                                    }
                                }
                            }
                            List<TblPurchaseWeighingStageSummaryTO> weighingList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId, false);
                            if (weighingList != null && weighingList.Count > 0)
                            {
                                for (int j = 0; j < weighingList.Count; j++)
                                {
                                    TblPurchaseWeighingStageSummaryTO tblWeighingMeasuresTO = weighingList[j];
                                    List<int[]> frameList1 = _iIotCommunication.GenerateFrameData(tblPurchaseScheduleSummaryTO, tblWeighingMeasuresTO);
                                    if (frameList1 != null && frameList1.Count > 0)
                                    {
                                        for (int f = 0; f < frameList1.Count; f++)
                                        {
                                            TblWeighingMachineTO machineTO = _iTblWeighingMachineDAO.SelectTblWeighingMachine(tblWeighingMeasuresTO.WeighingMachineId, conn, tran);
                                            if (machineTO == null)
                                            {
                                                tran.Rollback();
                                                resultMessage.DefaultBehaviour("MachineTo or IoT not found ");
                                                return resultMessage;
                                            }
                                            // result = _iIotCommunication.PostGateAPIDataToModbusTcpApi(tblPurchaseScheduleSummaryTO, frameList1[f]);
                                            result = _iWeighingCommunication.PostDataFrommodbusTcpApi(tblPurchaseScheduleSummaryTO, frameList1[f], machineTO);
                                            if (result != 1)
                                            {
                                                _iGateCommunication.DeleteSingleLoadingFromGateIoT(tblPurchaseScheduleSummaryTO);
                                                tran.Rollback();
                                                resultMessage.Text = "Error in PostDataFrommodbusTcpApi";
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                resultMessage.DisplayMessage = "Failed due to network error, Please try one more time";
                                                resultMessage.Result = 0;
                                                return resultMessage;
                                            }
                                        }
                                    }
                                    // tblWeighingMeasuresTO.VehicleNo = string.Empty;
                                    //tblWeighingMeasuresTO.GrossWeightMT = 0;
                                    //tblWeighingMeasuresTO.ActualWeightMT = 0;
                                    // tblWeighingMeasuresTO.NetWeightMT = 0;
                                    //tblWeighingMeasuresTO.WeightMeasurTypeId = 0;
                                    result = _iTblPurchaseWeighingStageSummaryDAO.UpdateTblPurchaseWeighingStageSummary(tblWeighingMeasuresTO, conn, tran);
                                    if (result != 1)
                                    {
                                        _iGateCommunication.DeleteSingleLoadingFromGateIoT(tblPurchaseScheduleSummaryTO);
                                        tran.Rollback();
                                        resultMessage.Text = "Error in PostDataFrommodbusTcpApi";
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.DisplayMessage = "Failed due to network error, Please try one more time";
                                        resultMessage.Result = 0;
                                        return resultMessage;
                                    }
                                }
                            }
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        return resultMessage;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return resultMessage;
            }
            finally
            {
                //conn.Close();
            }
            return resultMessage;
        }


    }
}
