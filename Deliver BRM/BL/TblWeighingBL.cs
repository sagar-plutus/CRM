using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;
using System.Linq;
using SalesTrackerAPI.IoT;

namespace SalesTrackerAPI.BL
{
    public class TblWeighingBL
    {
        #region Selection
        public static List<TblWeighingTO> SelectAllTblWeighing()
        {
            return TblWeighingDAO.SelectAllTblWeighing();
        }



        public static TblWeighingTO SelectTblWeighingTO(Int32 idWeighing)
        {
            return TblWeighingDAO.SelectTblWeighing(idWeighing);
        }

        public static TblWeighingTO SelectTblWeighingByMachineIp(string ipAddr)
        {
            TblWeighingTO tblWeighingTO = new TblWeighingTO();
            DateTime serverDateTime = Constants.ServerDateTime;
            DateTime defaultTime1 = serverDateTime.AddHours(15);
            tblWeighingTO = TblWeighingDAO.SelectTblWeighingByMachineIp(ipAddr, defaultTime1);
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

        public static ResultMessage RestoreToIOT()
        {
            ResultMessage resultMessage = new ResultMessage();

            String statusids = (Int32)Constants.TranStatusE.LOADING_CONFIRM + "," + (Int32)Constants.TranStatusE.LOADING_GATE_IN + "," + (Int32)Constants.TranStatusE.LOADING_COMPLETED + "," + (Int32)Constants.TranStatusE.LOADING_IN_PROGRESS
                + "," + (Int32)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN + "," + (Int32)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING;

            List<TblLoadingTO> tblLoadingTOList = TblLoadingBL.SelectAllLoadingListByStatus(statusids);

            for (int i = 0; i < tblLoadingTOList.Count; i++)
            {
                TblLoadingTO tblLoadingTO = tblLoadingTOList[i];

                resultMessage = RestoreToIOTLoadingTO(tblLoadingTO);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }
            }




            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;

        }


        public static ResultMessage RestoreToIOTLoadingTO(TblLoadingTO tblLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<TblInvoiceTO> tblInvoiceTOListAll = new List<TblInvoiceTO>();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            Int32 result = 0;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();

                if (tblLoadingTO.ModbusRefId > 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

                tblLoadingTO.ModbusRefId = BL.TblLoadingBL.GetNextAvailableModRefIdNew();

                String vehicleNo = tblLoadingTO.VehicleNo;
                Int32 transporterId = tblLoadingTO.TransporterOrgId;
                Int32 statusId = tblLoadingTO.StatusId;

                //tblLoadingTO.TransporterOrgId = 0;
                //tblLoadingTO.VehicleNo = string.Empty;
                tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_CONFIRM;

                if (tblLoadingTO.GateId == 0)
                {
                    TblGateTO tblGateTO = TblGateBL.GetDefaultTblGateTO();
                    if (tblGateTO != null)
                    {
                        tblLoadingTO.GateId = tblGateTO.IdGate;
                        tblLoadingTO.PortNumber = tblGateTO.PortNumber;
                        tblLoadingTO.IoTUrl = tblGateTO.IoTUrl;
                        tblLoadingTO.MachineIP = tblGateTO.MachineIP;
                    }
                }

                //DimStatusTO dimStatusTO = BL.DimStatusBL.SelectDimStatusTOByIotStatusId(tblLoadingTO.StatusId);
                //tblLoadingTO.StatusDate = IoT.IotCommunication.IoTDateTimeStringToDate(statusDate);

                result = TblLoadingDAO.UpdateTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while updating updating loadingTO");
                }
                tblLoadingTO.LoadingSlipList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);


                if (tblLoadingTO.LoadingSlipList != null && tblLoadingTO.LoadingSlipList.Count > 0)
                {
                    for (int k = 0; k < tblLoadingTO.LoadingSlipList.Count; k++)
                    {

                        TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[k];
                        //if (tblLoadingSlipTO.IsConfirmed == 0)
                        if (false)
                        {
                            tblLoadingSlipTO.StatusId = tblLoadingTO.StatusId;
                            //tblLoadingSlipTO.StatusReason = tblLoadingTO.StatusDesc;
                            //tblLoadingSlipTO.StatusDate = tblLoadingTO.StatusDate;
                            tblLoadingSlipTO.VehicleNo = tblLoadingTO.VehicleNo;

                            result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);

                            if (result != 1)
                            {
                                throw new Exception("Error while updating loadingSlip Id - " + tblLoadingSlipTO.IdLoadingSlip);
                            }

                            //Get Invoice Agasint Loading Slip.

                            List<TblInvoiceTO> tblInvoiceTOList = TblInvoiceBL.SelectInvoiceListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                            if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                            {
                                for (int p = 0; p < tblInvoiceTOList.Count; p++)
                                {
                                    TblInvoiceTO tblInvoiceTO = tblInvoiceTOList[p];

                                    tblInvoiceTO.VehicleNo = tblLoadingSlipTO.VehicleNo;
                                    tblInvoiceTO.TransportOrgId = tblLoadingTO.TransporterOrgId;
                                    
                                    tblInvoiceTO.UpdatedOn = Constants.ServerDateTime;
                                    tblInvoiceTO.UpdatedBy = 1;

                                    result = TblInvoiceBL.UpdateTblInvoice(tblInvoiceTO, conn, tran);
                                    if (result != 1)
                                    {
                                        throw new Exception("Error while updating Invoice");
                                    }

                                    tblInvoiceTO.InvoiceItemDetailsTOList = BL.TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(tblInvoiceTO.IdInvoice, conn, tran);

                                    tblInvoiceTOListAll.Add(tblInvoiceTO);


                                }
                            }
                        }

                    }
                }



                List<TblLoadingSlipExtTO> totalLoadingSlipExtList = new List<TblLoadingSlipExtTO>();

                List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();

                if (tblLoadingTO.LoadingSlipList != null)
                {
                    foreach (var loadingSlip in tblLoadingTO.LoadingSlipList)
                    {
                        totalLoadingSlipExtList.AddRange(loadingSlip.LoadingSlipExtTOList);
                    }

                    foreach (var invoiceTO in tblInvoiceTOListAll)
                    {
                        tblInvoiceItemDetailsTOList.AddRange(invoiceTO.InvoiceItemDetailsTOList);
                    }
                }

                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOlist = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(tblLoadingTO.IdLoading, conn, tran);

                if (tblWeighingMeasuresTOlist != null && tblWeighingMeasuresTOlist.Count > 0)
                {
                    for (int w = 0; w < tblWeighingMeasuresTOlist.Count; w++)
                    {
                        TblWeighingMeasuresTO tblWeighingMeasuresTO = tblWeighingMeasuresTOlist[w];

                        List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = totalLoadingSlipExtList.Where(w1 => w1.WeightMeasureId == tblWeighingMeasuresTO.IdWeightMeasure).ToList();

                        List<int[]> frameList1 = IoT.IotCommunication.GenerateFrameData(tblLoadingTO, tblWeighingMeasuresTO, tblLoadingSlipExtTOList);
                        if (frameList1 != null && frameList1.Count > 0)
                        {
                            for (int f = 0; f < frameList1.Count; f++)
                            {
                                TblWeighingMachineTO machineTO = BL.TblWeighingMachineBL.SelectTblWeighingMachineTO(tblWeighingMeasuresTO.WeighingMachineId, conn, tran);
                                if (machineTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("MachineTo or IoT not found ");
                                    return resultMessage;
                                }
                                result = IoT.WeighingCommunication.PostDataFrommodbusTcpApi(tblLoadingTO, frameList1[f], machineTO);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Error in PostDataFrommodbusTcpApi";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.DisplayMessage = "Failed due to network error, Please try one more time";
                                    resultMessage.Result = 0;
                                    return resultMessage;
                                }
                            }
                        }

                        TblWeghingMessureDtlsTO tblWeghingMessureDtlsTO = new TblWeghingMessureDtlsTO();
                        var MeasurTypeId = tblWeighingMeasuresTO.WeightMeasurTypeId;
                        var layerId = (MeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT || MeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT) ? 0 : tblLoadingSlipExtTOList[0].LoadingLayerid;
                        tblWeghingMessureDtlsTO.LoadingId = tblWeighingMeasuresTO.LoadingId;
                        tblWeghingMessureDtlsTO.WeighingMachineId = tblWeighingMeasuresTO.WeighingMachineId;
                        tblWeghingMessureDtlsTO.WeightMeasurTypeId = tblWeighingMeasuresTO.WeightMeasurTypeId;
                        tblWeghingMessureDtlsTO.LayerId = layerId;
                        result = DAL.TblWeighingMeasuresDAO.InsertTblWeghingMessureDtls(tblWeghingMessureDtlsTO, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.Text = "";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }

                        result = TblWeighingMeasuresBL.DeleteTblWeighingMeasures(tblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                        if (result != 1)
                        {
                            throw new Exception("Error while deleting weighing measure -" + tblWeighingMeasuresTO.IdWeightMeasure);
                        }

                    }
                }


                DimStatusTO statusTO = DAL.DimStatusDAO.SelectDimStatus(statusId, conn, tran);
                if (statusTO == null || statusTO.IotStatusId == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("iot status id not found for loading to pass at gate iot");
                    return resultMessage;
                }

                // Call to post data to Gate IoT API
                List<object[]> frameList = IoT.IotCommunication.GenerateGateIoTFrameData(tblLoadingTO, vehicleNo, statusTO.IotStatusId, transporterId);
                if (frameList != null && frameList.Count > 0)
                {
                    for (int f = 0; f < frameList.Count; f++)
                    {
                        //Saket [2019-04-11] Keep common call for write data too IOT i.e from approval
                        //result = IoT.IotCommunication.PostGateAPIDataToModbusTcpApiForLoadingSlip(tblLoadingTO, frameList[f]);
                        result = IoT.IotCommunication.PostGateAPIDataToModbusTcpApi(tblLoadingTO, frameList[f]);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error while PostGateAPIDataToModbusTcpApi");
                            resultMessage.DisplayMessage = "Failed due to network error, Please try one more time";
                            return resultMessage;
                        }
                    }
                }

                tran.Commit();

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "RestoreToIOT");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public static int FetchFromIotAndWrite()
        {

            List<TblMachineBackupTO> list = TblWeighingDAO.GetALLMachineData();
            if (list != null)
            {
                int cnt = 0;
                foreach (var item in list)
                {
                    //if(cnt == 0)
                    //{
                    int a = FetchLoadingSlipFromIotAndWrite(item);
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

        public static int FetchLoadingSlipFromIotAndWrite(TblMachineBackupTO tblMachineBackupTO)
        {
            TblGateTO tblGateTO = TblGateBL.GetDefaultTblGateTO();
            if (tblGateTO != null)
            {
                List<TblInvoiceTO> tblInvoiceTOListAll = new List<TblInvoiceTO>();


                string IOTframe = "";
                int result = 0;
                SqlConnection DeliverConnection = new SqlConnection(Startup.ConnectionString);
                SqlTransaction DeliverTran = null;
                try
                {
                    #region All Activity Related To Loading Slip And Weighing Data

                    DeliverConnection.Open();
                    DeliverTran = DeliverConnection.BeginTransaction();
                    //String[] IOTframes = item.Machinedata.Split(",");
                    if (!String.IsNullOrEmpty(tblMachineBackupTO.Machinedata))
                    {
                        // var frames = new ArraySegment<string>(tblMachineBackupTO.Machinedata.Split(","), 0, 8);
                        var frames = new ArraySegment<string>(tblMachineBackupTO.Machinedata.Split(","), 0, 8);
                        IOTframe = string.Join(",", frames.ToList().ToArray());
                        //IOTframe = string.Join(",", tblMachineBackupTO.Machinedata.ToList().ToArray());
                        //IOTframe = "[" + tblMachineBackupTO.Machinedata + "]"; //string.Join(",", tblMachineBackupTO.Machinedata.ToList().ToArray());
                        //StaticStuff.IoTConstants.writeLog("method: GetLoadingIdsOfDeliverStatus," + IOTframe);
                        GateIoTResult gateIoTResult = IoT.IotCommunication.GetDecryptedLoadingId(IOTframe, "GetLoadingDecriptionData", tblGateTO.IoTUrl);
                        if (gateIoTResult != null && gateIoTResult.Code != 0 && gateIoTResult.Data.Count > 0)
                        {
                            #region Update All Loading Slip details From IoT
                            TblLoadingTO tblLoadingTO = TblLoadingBL.SelectTblLoadingTOByModBusRefId(Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.LoadingId]));
                    
                            if (tblLoadingTO != null)
                            {
                                //if (!String.IsNullOrEmpty(tblLoadingTO.VehicleNo))
                                //{
                                //    return 1;
                                //}

                                tblLoadingTO.VehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
                                tblLoadingTO.TransporterOrgId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.TransportorId]);
                                String statusDate = (String)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusDate];

                                Int32 statusId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.StatusId]);

                                DimStatusTO dimStatusTO = BL.DimStatusBL.SelectDimStatusTOByIotStatusId(statusId);
                                tblLoadingTO.StatusDate = IoT.IotCommunication.IoTDateTimeStringToDate(statusDate);

                                if (dimStatusTO != null)
                                {
                                    tblLoadingTO.StatusId = dimStatusTO.IdStatus;
                                    tblLoadingTO.StatusReason = dimStatusTO.StatusName;
                                }

                                result = TblLoadingDAO.UpdateTblLoading(tblLoadingTO, DeliverConnection, DeliverTran);
                                if (result != 1)
                                {
                                    DeliverTran.Rollback();
                                    return 0;
                                }
                                tblLoadingTO.LoadingSlipList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, DeliverConnection, DeliverTran);
                                //if (tblLoadingTO.LoadingSlipList != null)
                                //    tblLoadingTO.LoadingSlipList.ForEach(f =>
                                //    {
                                //        f.StatusId = tblLoadingTO.StatusId; f.StatusName = tblLoadingTO.StatusDesc; f.StatusDate = tblLoadingTO.StatusDate; f.VehicleNo = tblLoadingTO.VehicleNo;
                                //        TblLoadingSlipBL.UpdateTblLoadingSlip(f, DeliverConnection, DeliverTran);
                                //    });

                                if (tblLoadingTO.LoadingSlipList != null && tblLoadingTO.LoadingSlipList.Count > 0)
                                {
                                    for (int k = 0; k < tblLoadingTO.LoadingSlipList.Count; k++)
                                    {

                                        TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[k];
                                        tblLoadingSlipTO.StatusId = tblLoadingTO.StatusId;
                                        tblLoadingSlipTO.StatusReason = tblLoadingTO.StatusDesc;
                                        tblLoadingSlipTO.StatusDate = tblLoadingTO.StatusDate;
                                        tblLoadingSlipTO.VehicleNo = tblLoadingTO.VehicleNo;

                                        result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingSlipTO, DeliverConnection, DeliverTran);

                                        if (result != 1)
                                        {
                                            DeliverTran.Rollback();
                                            return 0;
                                        }


                                        //Get Invoice Agasint Loading Slip.

                                        List<TblInvoiceTO> tblInvoiceTOList = TblInvoiceBL.SelectInvoiceListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, DeliverConnection, DeliverTran);
                                        if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                                        {
                                            for (int p = 0; p < tblInvoiceTOList.Count; p++)
                                            {
                                                TblInvoiceTO tblInvoiceTO = tblInvoiceTOList[p];

                                                tblInvoiceTO.VehicleNo = tblLoadingSlipTO.VehicleNo;
                                                if (tblInvoiceTO.TransportOrgId == 0)
                                                {
                                                    tblInvoiceTO.TransportOrgId = tblLoadingTO.TransporterOrgId;
                                                }
                                                tblInvoiceTO.UpdatedOn = Constants.ServerDateTime;
                                                tblInvoiceTO.UpdatedBy = 1;

                                                result = TblInvoiceBL.UpdateTblInvoice(tblInvoiceTO, DeliverConnection, DeliverTran);
                                                if (result != 1)
                                                {
                                                    DeliverTran.Rollback();
                                                    return 0;
                                                }

                                                tblInvoiceTO.InvoiceItemDetailsTOList = BL.TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(tblInvoiceTO.IdInvoice, DeliverConnection, DeliverTran);

                                                tblInvoiceTOListAll.Add(tblInvoiceTO);


                                            }
                                        }


                                    }
                                }

                                #endregion

                                #region Write Data In Status History Table 

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
                                    statusHistoryTO.CreatedBy = 1;
                                    statusHistoryTO.CreatedOn = Constants.ServerDateTime;
                                    tblLoadingTO.LoadingStatusHistoryTOList.Add(statusHistoryTO);
                                    result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory(statusHistoryTO, DeliverConnection, DeliverTran);
                                    if (result != 1)
                                    {
                                        DeliverTran.Rollback();
                                        return 0;
                                    }
                                }


                                #endregion

                                #region Get All Weighing Data From IoT Backup

                                result = GetCalculatedLayerData(tblLoadingTO, tblGateTO, tblInvoiceTOListAll, DeliverConnection, DeliverTran);
                                if(result != 1)
                                {
                                    DeliverTran.Rollback();
                                }
                                DeliverTran.Commit();
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
                    DeliverTran.Rollback();
                    return 0;
                }
                finally
                {
                    DeliverConnection.Close();
                }
            }
            return 0;
        }

        public static int  GetCalculatedLayerData(TblLoadingTO tblLoadingTO,TblGateTO tblGateTO, List<TblInvoiceTO> invoiceTOListAll, SqlConnection sqlConnection,SqlTransaction sqlTransaction)
        {
            int resultTemp = 0;
            List<TblLoadingSlipExtTO> totalLoadingSlipExtList = new List<TblLoadingSlipExtTO>();

            List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();

            if (tblLoadingTO.LoadingSlipList != null)
            {
                foreach (var loadingSlip in tblLoadingTO.LoadingSlipList)
                {
                    totalLoadingSlipExtList.AddRange(loadingSlip.LoadingSlipExtTOList);
                }

                foreach (var invoiceTO in invoiceTOListAll)
                {
                    tblInvoiceItemDetailsTOList.AddRange(invoiceTO.InvoiceItemDetailsTOList);
                }

                List<int> portList = new List<int>();
                List<String> layerWiseData = TblWeighingDAO.SelectlayerWiseData(Math.Floor(Convert.ToDecimal(tblLoadingTO.ModbusRefId * 256)), portList);
                if (layerWiseData != null && layerWiseData.Count > 0)
                {
                    //string mergeAllLayerData = string.Join(",", layerWiseData.ToArray());
                    for (int u = 0; u < layerWiseData.Count; u++)
                    {
                        GateIoTResult itemList = IoT.IotCommunication.GetDecryptedLoadingId(layerWiseData[u], "GetLoadingDecriptedLayerData", tblGateTO.IoTUrl);
                        if (itemList.Data != null && itemList.Data.Count > 0)
                        {
                            for (int f = 0; f < itemList.Data.Count; f++)
                            {
                                #region Insert In Temp Weighing Messure
                                TblWeighingMeasuresTO tblWeighingMeasuresTO = new TblWeighingMeasuresTO();
                                tblWeighingMeasuresTO.CreatedBy = 1;
                                tblWeighingMeasuresTO.CreatedOn = Constants.ServerDateTime;
                                tblWeighingMeasuresTO.VehicleNo = tblLoadingTO.VehicleNo;
                                tblWeighingMeasuresTO.LoadingId = tblLoadingTO.IdLoading;
                                tblWeighingMeasuresTO.WeighingMachineId = TblWeighingDAO.GetMachineId(portList[u]);
                                tblWeighingMeasuresTO.WeightMeasurTypeId = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.WeighTypeId]);

                                if (tblWeighingMeasuresTO.WeightMeasurTypeId != 2)
                                    tblWeighingMeasuresTO.WeightMT = Convert.ToDouble(itemList.Data[f][(int)IoTConstants.WeightIotColE.Weight]);
                                else
                                {
                                    tblWeighingMeasuresTO.WeightMT = Convert.ToDouble(itemList.Data[f][(int)IoTConstants.WeightIotColE.CalcTareWt]) + Convert.ToDouble(itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedWt]);
                                }

                                tblWeighingMeasuresTO.MachineCalibrationId = 1;
                                resultTemp = TblWeighingMeasuresBL.InsertTblWeighingMeasures(tblWeighingMeasuresTO, sqlConnection,sqlTransaction);
                                if(resultTemp != 1)
                                {
                                    return 0;
                                }
                                #endregion

                                int itemRefId = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.ItemRefNo]);
                                if (itemRefId > 0)
                                {
                                    var itemTO = totalLoadingSlipExtList.Where(w => w.ModbusRefId == itemRefId).FirstOrDefault();
                                    if (itemTO != null)
                                    {
                                        itemTO.LoadedWeight = Convert.ToDouble(itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedWt]);
                                        itemTO.CalcTareWeight = Convert.ToDouble(itemList.Data[f][(int)IoTConstants.WeightIotColE.CalcTareWt]);
                                        itemTO.LoadedBundles = Convert.ToInt32(itemList.Data[f][(int)IoTConstants.WeightIotColE.LoadedBundle]);
                                        itemTO.WeightMeasureId = tblWeighingMeasuresTO.IdWeightMeasure;
                                        resultTemp = TblLoadingSlipExtBL.UpdateTblLoadingSlipExt(itemTO, sqlConnection, sqlTransaction);
                                        if (resultTemp != 1)
                                        {
                                            return 0;
                                        }


                                        List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOListAgainstExt = tblInvoiceItemDetailsTOList.Where(w => w.LoadingSlipExtId == itemTO.IdLoadingSlipExt).ToList();
                                        if (tblInvoiceItemDetailsTOListAgainstExt != null && tblInvoiceItemDetailsTOListAgainstExt.Count > 0)
                                        {
                                            for (int m = 0; m < tblInvoiceItemDetailsTOListAgainstExt.Count; m++)
                                            {
                                                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoiceItemDetailsTOListAgainstExt[m];
                                                tblInvoiceItemDetailsTO.Bundles = itemTO.LoadedBundles;
                                                tblInvoiceItemDetailsTO.InvoiceQty = itemTO.LoadedWeight / 1000;

                                                resultTemp = TblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, sqlConnection, sqlTransaction);
                                                if (resultTemp != 1)
                                                {
                                                    return 0;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return 1;
        }

        #endregion

        #region Insertion
        public static int InsertTblWeighing(TblWeighingTO tblWeighingTO)
        {
            return TblWeighingDAO.InsertTblWeighing(tblWeighingTO);
        }

        public static int InsertTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingDAO.InsertTblWeighing(tblWeighingTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblWeighing(TblWeighingTO tblWeighingTO)
        {
            return TblWeighingDAO.UpdateTblWeighing(tblWeighingTO);
        }

        public static int UpdateTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingDAO.UpdateTblWeighing(tblWeighingTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblWeighing(Int32 idWeighing)
        {
            return TblWeighingDAO.DeleteTblWeighing(idWeighing);
        }
        /// <summary>
        /// GJ@20170830 : Remove the all previous weighing measured records from tables againest IpAddr
        /// </summary>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        public static int DeleteTblWeighingByByMachineIp(string ipAddr)
        {
            try
            {
                return TblWeighingDAO.DeleteTblWeighingByByMachineIp(ipAddr);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static int DeleteTblWeighing(Int32 idWeighing, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingDAO.DeleteTblWeighing(idWeighing, conn, tran);
        }

        #endregion

    }
}
