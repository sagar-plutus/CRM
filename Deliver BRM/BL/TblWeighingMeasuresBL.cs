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
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace SalesTrackerAPI.BL
{
    public class TblWeighingMeasuresBL
    {
        #region Selection

        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresList()
        {
            return TblWeighingMeasuresDAO.SelectAllTblWeighingMeasures();
        }

        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId)
        {
            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            tblWeighingMeasuresTOList = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(loadingId);
            if (tblWeighingMeasuresTOList.Count > 0)
            {
                tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
            }
            return tblWeighingMeasuresTOList;
        }


        /// <summary>
        /// Sanjay [2017-12-18] For Listing record count against each loading. Required for Loading % calc
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> SelectLoadingWiseExtRecordCountDCT()
        {
            return TblWeighingMeasuresDAO.SelectLoadingWiseRecordCountDCT();
        }

        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran)
        {

            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            //int confiqId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

            int confiqId = Constants.getweightSourceConfigTO();

            if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.DB) ||
                confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.BOTH))

            {
                tblWeighingMeasuresTOList = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(loadingId, conn, tran);
                if (tblWeighingMeasuresTOList.Count > 0)
                {
                    tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
                }
            }
            else
            {
                IoT.IotCommunication.GetWeighingMeasuresFromIoT(Convert.ToString(loadingId), false, tblWeighingMeasuresTOList, conn, tran);

            }
            return tblWeighingMeasuresTOList;
        }
        /*GJ@20170829 : Get the All weighing Measurement by Loading Ids : */
        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(string loadingId, Boolean isUnloading)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Kiran [06-Dec-2018] Config for reading weighing data from Iot Or DB
                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
                int confiqId = Constants.getweightSourceConfigTO();
                if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.DB) ||
                    confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.BOTH) || isUnloading == true)
                {
                    tblWeighingMeasuresTOList = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingIds(loadingId, isUnloading, conn, tran);
                    if (tblWeighingMeasuresTOList != null && tblWeighingMeasuresTOList.Count > 0)
                    {
                        //tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
                        tblWeighingMeasuresTOList.OrderByDescending(p => p.IdWeightMeasure);
                    }
                }
                else
                {
                    IoT.IotCommunication.GetWeighingMeasuresFromIoT(loadingId, isUnloading, tblWeighingMeasuresTOList, conn, tran);
                    tblWeighingMeasuresTOList.OrderBy(p => p.CreatedOn);
                }



                return tblWeighingMeasuresTOList;
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



        /*GJ@20170828 : Get the All weighing Measurement by Vehicle No : */
        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByVehicleNo(string vehicleNo)
        {
            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            tblWeighingMeasuresTOList = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByVehicleNo(vehicleNo);
            if (tblWeighingMeasuresTOList.Count > 0)
            {
                tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
            }
            return tblWeighingMeasuresTOList;
        }
        public static TblWeighingMeasuresTO SelectTblWeighingMeasuresTO(Int32 idWeightMeasure)
        {
            return TblWeighingMeasuresDAO.SelectTblWeighingMeasures(idWeightMeasure);
        }


        /// <summary>
        /// Vijaymala added to get distinct weighing count
        /// </summary>
        /// <param name="loadingId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static Int32 SelectDistinctWeighingMeasuresListByLoadingId(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            Int32 weighingCount = 0;

            Int32 cofigId = Constants.getweightSourceConfigTO();
            if (cofigId == (Int32)Constants.WeighingDataSourceE.IoT)
            {
                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(tblLoadingTO.IdLoading, conn, tran);
                var grossWtMeasuresTOList = tblWeighingMeasuresTOList.Where(t => t.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT);
                var distinctWeighingMeasureList = grossWtMeasuresTOList.GroupBy(b => b.WeighingMachineId).ToList();
                if (distinctWeighingMeasureList != null && distinctWeighingMeasureList.Count > 0)
                {
                    weighingCount = distinctWeighingMeasureList.Count;
                }
            }
            else
            {

                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
                List<TblLoadingTO> tblLoadingToList = BL.TblLoadingBL.SelectAllLoadingListByVehicleNo(tblLoadingTO.VehicleNo, Convert.ToBoolean(tblLoadingTO.IsAllowNxtLoading), conn, tran);


                if (tblLoadingToList != null && tblLoadingToList.Count > 0)
                {
                    for (int i = 0; i < tblLoadingToList.Count; i++)
                    {
                        TblLoadingTO tempLoadingTO = tblLoadingToList[i];
                        List<TblWeighingMeasuresTO> tempWeighingMeasureList = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(tempLoadingTO.IdLoading, conn, tran);
                        tblWeighingMeasuresTOList.AddRange(tempWeighingMeasureList);
                    }
                }

                //  tblWeighingMeasuresTOList = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(tblLoadingTO.IdLoading, conn,tran);

                if (tblWeighingMeasuresTOList != null && tblWeighingMeasuresTOList.Count > 0)
                {
                    var grossWtMeasuresTOList = tblWeighingMeasuresTOList.Where(t => t.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT);
                    var distinctWeighingMeasureList = grossWtMeasuresTOList.GroupBy(b => b.WeighingMachineId).ToList();
                    if (distinctWeighingMeasureList != null && distinctWeighingMeasureList.Count > 0)
                    {
                        weighingCount = distinctWeighingMeasureList.Count;
                    }
                }
            }
            return weighingCount;
        }

        #endregion

        #region Insertion
        public static int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO)
        {
            return TblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO);
        }

        public static int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
        }

        public static ResultMessage MarkAllLoadingSlipExtIsAllowTareWeightAgainstLoading(Int32 loadingId, Int32 userId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (loadingId > 0)
                {
                    List<TblLoadingSlipTO> tblLoadingSlipTOList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(loadingId, conn, tran);

                    if (tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 0)
                    {
                        for (int i = 0; i < tblLoadingSlipTOList.Count; i++)
                        {
                            TblLoadingSlipTO tblLoadingSlipTO = tblLoadingSlipTOList[i];
                            if (tblLoadingSlipTO != null && tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                            {
                                for (int p = 0; p < tblLoadingSlipTO.LoadingSlipExtTOList.Count; p++)
                                {
                                    TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[p];

                                    if (tblLoadingSlipExtTO.LoadedWeight == 0 && tblLoadingSlipExtTO.IsAllowWeighingMachine == 0
                                        && tblLoadingSlipExtTO.WeightMeasureId == 0)
                                    {
                                        tblLoadingSlipExtTO.IsAllowWeighingMachine = 1;
                                        tblLoadingSlipExtTO.UpdatedBy = userId;
                                        tblLoadingSlipExtTO.UpdatedOn = Constants.ServerDateTime;
                                        result = TblLoadingSlipExtBL.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
                                        if (result != 1)
                                        {
                                            throw new Exception("Error While Updating loading slip ext");
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {

                resultMessage.Text = "Exception Error While Record Save : SaveNewWeighinMachineMeasurement";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        /*GJ@20170816 : Insert Weighing Machine Measurement : START*/
        public static ResultMessage SaveNewWeighinMachineMeasurement(TblWeighingMeasuresTO tblWeighingMeasuresTO, List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {

                Boolean isTareWeight = false;

                conn.Open();
                tran = conn.BeginTransaction();
                if (tblWeighingMeasuresTO == null)
                {
                    tran.Rollback();
                    resultMessage.Text = "WeighinMachine Mesurement Found Null : SaveNewWeighinMachineMeasurement";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                //Priyanka [06-12-2018]

                int configId = Constants.getweightSourceConfigTO();

                #region 0. Check Tare taken againest machine Id
                if (tblWeighingMeasuresTO.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT)
                {
                    List<TblWeighingMeasuresTO> weighingMeasuresToList = new List<TblWeighingMeasuresTO>();
                    List<TblWeighingMachineTO> weighingMeasuresToListIot = new List<TblWeighingMachineTO>();
                    // TblWeighingMeasuresTO tblWeighingMeasureTo = new TblWeighingMeasuresTO();
                    if (configId == 1)
                    {
                        weighingMeasuresToList = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(tblWeighingMeasuresTO.LoadingId, conn, tran);
                        if (weighingMeasuresToList.Count > 0)
                        {
                            var vRes = weighingMeasuresToList.Where(p => p.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT
                            && p.WeighingMachineId == tblWeighingMeasuresTO.WeighingMachineId).FirstOrDefault();
                            if (vRes != null)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Tare already occured this machine");
                                resultMessage.DisplayMessage = "Tare weight already taken againest this Machine.";
                                return resultMessage;
                            }
                        }

                        if (weighingMeasuresToList == null || weighingMeasuresToList.Count == 0)
                        {
                            isTareWeight = true;
                        }
                    }
                    else
                    {
                        weighingMeasuresToListIot = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineOfWeighingList(tblWeighingMeasuresTO.LoadingId, conn, tran);
                        if (weighingMeasuresToListIot.Count > 0)
                        {
                            var vRes = weighingMeasuresToListIot.Where(p => p.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT
                            && p.IdWeighingMachine == tblWeighingMeasuresTO.WeighingMachineId).FirstOrDefault();
                            if (vRes != null)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Tare already occured this machine");
                                resultMessage.DisplayMessage = "Tare weight already taken againest this Machine.";
                                return resultMessage;
                            }
                        }

                        if (weighingMeasuresToListIot == null || weighingMeasuresToListIot.Count == 0)
                        {
                            isTareWeight = true;
                        }
                    }




                }
                #endregion

                #region 1. Save the Weighing Machine Mesurement 

                if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0 && tblLoadingSlipExtTOList[0].NewTareWeight == 1)
                {
                    for (int p = 0; p < tblLoadingSlipExtTOList.Count; p++)
                    {
                        tblLoadingSlipExtTOList[p].LoadedWeight = 0;
                    }
                }
                else
                {
                    //@ModBusTCP : Priyanka[06-12-2018]
                    if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.DB) ||
                        configId == Convert.ToInt32(Constants.WeighingDataSourceE.BOTH) || tblWeighingMeasuresTO.UnLoadingId != 0)
                    {
                        result = DAL.TblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.Text = "";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                     
                    }
                    if ((configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT) ||
                    configId == Convert.ToInt32(Constants.WeighingDataSourceE.BOTH)) && (tblWeighingMeasuresTO.UnLoadingId == 0))
                    {
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
                    }

                }

                #endregion

                #region 2. Update the Loading Slip Ext Weighing Machine Measurement

                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOdata = new List<TblLoadingSlipExtTO>();
                if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0)
                {
                    //@ModBusTCP : Priyanka[06-12-2018]
                    if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.DB) ||
                           configId == Convert.ToInt32(Constants.WeighingDataSourceE.BOTH))
                    {
                        foreach (var tblLoadingSlipExtTo in tblLoadingSlipExtTOList)
                        {

                            tblLoadingSlipExtTo.WeightMeasureId = tblWeighingMeasuresTO.IdWeightMeasure;

                            tblLoadingSlipExtTo.UpdatedBy = tblWeighingMeasuresTO.CreatedBy;
                            tblLoadingSlipExtTOdata.Add(tblLoadingSlipExtTo);
                            result = DAL.TblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTo, conn, tran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                resultMessage.Text = "";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }
                    }
                    else if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT)) // Update only sequence number
                    {
                        foreach (var tblLoadingSlipExtTo in tblLoadingSlipExtTOList)
                        {
                            result = DAL.TblLoadingSlipExtDAO.UpdateLoadingSlipExtSeqNumber(tblLoadingSlipExtTo, conn, tran);
                            if (result <= 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While updating items weighing sequence in loadingslipext");
                                return resultMessage;
                            }
                        }
                    }
                }

                #endregion
                if (tblWeighingMeasuresTO.UnLoadingId == 0)
                {
                    TblLoadingTO loadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblWeighingMeasuresTO.LoadingId, conn, tran);

                    #region Auto Invoice & Invoice Validation

                    //Sanjay [2017-09-17] Call For Auto Invoice
                    if (tblWeighingMeasuresTO.IsLoadingCompleted == 1)
                    {
                        //Kiran [06 Dec 2018] Commented as shifted before condition as it is required further in function
                        //TblLoadingTO loadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblWeighingMeasuresTO.LoadingId, conn, tran);
                        if (loadingTO != null)
                        {
                            resultMessage = BL.TblInvoiceBL.PrepareAndSaveNewTaxInvoice(loadingTO, tblLoadingSlipExtTOList, conn, tran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                tran.Rollback();
                                return resultMessage;
                            }
                        }
                        String wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE;
                        if (loadingTO.IsInternalCnf == 1)
                             wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE_INTERNAL_TRANSFER;

                        TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(wtScaleConfigStr, conn, tran);
                        //TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);
                        if (tblConfigParamsTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Default Weighing scale setting is not found.");
                            return resultMessage;
                        }
                        //Sanjay [2017-10-08] On every final item weight auto gross for any count of weighin measurement
                        //Discusse With Nitin K sir
                        //if(Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) > 1)
                        //{
                        //TblWeighingMeasuresTO cloneTo = tblWeighingMeasuresTO.Clone()
                        //tblWeighingMeasuresTO.WeightMeasurTypeId = (int)Constants.TransMeasureTypeE.GROSS_WEIGHT;
                        //result = DAL.TblWeighingMeasuresDAO.InsertTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
                        //if (result < 0)
                        //{
                        //    tran.Rollback();
                        //    resultMessage.Text = "";
                        //    resultMessage.MessageType = ResultMessageE.Error;
                        //    resultMessage.Result = 0;
                        //    return resultMessage;
                        //}
                        //}

                    }

                    //GJ [2017-09-30] Call for To check Invoice Number Generated Againest Vehicle No


                    if (tblWeighingMeasuresTO.IsCheckInvoiceGenerated == 1)
                    {
                        resultMessage = CheckInvoiceNoGeneratedByVehicleNo(tblWeighingMeasuresTO.VehicleNo, conn, tran, false, tblWeighingMeasuresTO.LoadingId);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    #endregion

                    #region Write Data To IoT Devices Based On Configuration

                    //@ModBusTCP : Priyanka[06-12-2018]
                    if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT) ||
                        configId == Convert.ToInt32(Constants.WeighingDataSourceE.BOTH))
                    {
                        List<int[]> frameList = IoT.IotCommunication.GenerateFrameData(loadingTO, tblWeighingMeasuresTO, tblLoadingSlipExtTOList);
                        if (frameList != null && frameList.Count > 0)
                        {
                            for (int f = 0; f < frameList.Count; f++)
                            {
                                TblWeighingMachineTO machineTO = BL.TblWeighingMachineBL.SelectTblWeighingMachineTO(tblWeighingMeasuresTO.WeighingMachineId, conn, tran);
                                if (machineTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("MachineTo or IoT not found ");
                                    return resultMessage;
                                }
                                result = IoT.WeighingCommunication.PostDataFrommodbusTcpApi(loadingTO, frameList[f], machineTO);
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
                        if (tblWeighingMeasuresTO.IsLoadingCompleted == 1)
                        {

                            //@Added By Kiran For Final gross weight(IoT) 16/06/2019
                            if ((configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT) ||
                            configId == Convert.ToInt32(Constants.WeighingDataSourceE.BOTH)) && (tblWeighingMeasuresTO.UnLoadingId == 0))
                            {
                                TblWeghingMessureDtlsTO tblWeghingMessureDtlsTO = new TblWeghingMessureDtlsTO();
                                var MeasurTypeId = tblWeighingMeasuresTO.WeightMeasurTypeId;
                                var layerId = (int)Constants.TransMeasureTypeE.GROSS_WEIGHT;
                                tblWeghingMessureDtlsTO.LoadingId = tblWeighingMeasuresTO.LoadingId;
                                tblWeghingMessureDtlsTO.WeighingMachineId = tblWeighingMeasuresTO.WeighingMachineId;
                                tblWeghingMessureDtlsTO.WeightMeasurTypeId = (int)Constants.TransMeasureTypeE.GROSS_WEIGHT;
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
                            }
                            List<TblLoadingSlipExtTO> tblLoadingSlipExtTOListForGross = new List<TblLoadingSlipExtTO>();
                            TblWeighingMeasuresTO tblWeighingMeasuresTOForGross = new TblWeighingMeasuresTO();
                            tblWeighingMeasuresTOForGross.WeightMeasurTypeId = (int)Constants.TransMeasureTypeE.GROSS_WEIGHT;
                            tblWeighingMeasuresTOForGross.WeightMT = tblWeighingMeasuresTO.WeightMT;
                            List<int[]> frameListForGross = IoT.IotCommunication.GenerateFrameData(loadingTO, tblWeighingMeasuresTOForGross, tblLoadingSlipExtTOListForGross);
                            if (frameListForGross != null && frameListForGross.Count > 0)
                            {
                                TblWeighingMachineTO machineTOForGross = BL.TblWeighingMachineBL.SelectTblWeighingMachineTO(tblWeighingMeasuresTO.WeighingMachineId, conn, tran);
                                if (machineTOForGross == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("MachineTo or IoT not found ");
                                    return resultMessage;
                                }
                                result = IoT.WeighingCommunication.PostDataFrommodbusTcpApi(loadingTO, frameListForGross[0], machineTOForGross);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Error in PostDataFrommodbusTcpApi When Write Gross Weight";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.DisplayMessage = "Failed due to network error, Please try one more time";
                                    resultMessage.Result = 0;
                                    return resultMessage;
                                }
                            }
                        }
                    }

                        #endregion

                        if (isTareWeight)
                        {
                            resultMessage = UpdateLoadingStatusOnWeighing(loadingTO, tblWeighingMeasuresTO, configId, conn, tran);
                            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                            {
                                throw new Exception("Error while updating loading status");
                            }
                        }
                        loadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH);
                        loadingTO.StatusReason = "Invoice Generated and ready for dispach";
                        loadingTO.StatusDate = Constants.ServerDateTime;
                        // loadingTO.IdLoading = tblLoadingSlipTOselect.LoadingId;


                        if (configId == (Int32)Constants.WeighingDataSourceE.IoT || configId == (Int32)Constants.WeighingDataSourceE.BOTH)
                        {
                        String wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE;
                        if (loadingTO.IsInternalCnf == 1)
                            wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE_INTERNAL_TRANSFER;

                        TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(wtScaleConfigStr, conn, tran);
                        //TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);
                        if (configParamsTO != null)
                            {
                                if (Convert.ToInt32(configParamsTO.ConfigParamVal) == 2)
                                {
                                //@Kiran Added for Update status
                                    //List<TblWeighingMeasuresTO> weighingMeasuresToList = new List<TblWeighingMeasuresTO>();
                                    //weighingMeasuresToList = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(tblWeighingMeasuresTO.LoadingId, conn, tran);
                                    List<TblWeighingMachineTO> weighingMeasuresToList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineOfWeighingList(tblWeighingMeasuresTO.LoadingId, conn, tran);
                                    if (weighingMeasuresToList.Count > 0)
                                    {
                                        var vRes = weighingMeasuresToList.Where(p => p.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT).ToList();
                                        if (vRes != null)
                                        {
                                            vRes = vRes.GroupBy(g => g.IdWeighingMachine).Select(s => s.FirstOrDefault()).ToList();
                                            if (vRes != null && vRes.Count >= 2)
                                            {
                                                resultMessage = TblLoadingBL.UpdateLoadingStatusToGateIoT(loadingTO, conn, tran);
                                                if (resultMessage.MessageType != ResultMessageE.Information)
                                                {
                                                    return resultMessage;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Saved Sucessfully";
                    resultMessage.Result = 1;
                    return resultMessage;

                }
            catch (Exception ex)
            {

                resultMessage.Text = "Exception Error While Record Save : SaveNewWeighinMachineMeasurement";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }




        /*GJ@20170930 : Prepare method to check Invoice No is generated or Not*/
        public static ResultMessage CheckInvoiceNoGeneratedByVehicleNo(string vehicleNo, SqlConnection conn, SqlTransaction tran, Boolean isForOutOnly = false, int loadingId = 0)
        {
            ResultMessage resMessage = new StaticStuff.ResultMessage();
            try
            {
                int weightSourceConfigId = Constants.getweightSourceConfigTO();
                List<TblLoadingTO> loadingList = new List<TblLoadingTO>();
                if (isForOutOnly)
                {
                    if (weightSourceConfigId != (Int32)Constants.WeighingDataSourceE.IoT)
                    {
                        loadingList = BL.TblLoadingBL.SelectAllLoadingListByVehicleNoForDelOut(vehicleNo, conn, tran);
                    }
                    else
                    {
                        loadingList = BL.TblLoadingBL.SelectAllLoadingListByVehicleNoForDelOut(loadingId, conn, tran);
                    }
                }
                else
                {
                    if (weightSourceConfigId != (Int32)Constants.WeighingDataSourceE.IoT)
                    {
                        loadingList = BL.TblLoadingBL.SelectAllLoadingListByVehicleNo(vehicleNo, false, conn, tran);
                    }
                    else
                    {
                        loadingList = BL.TblLoadingBL.SelectAllLoadingListByVehicleNo(vehicleNo, false, conn, tran, loadingId);
                    }
                }

                if (loadingList == null || loadingList.Count == 0)
                {
                    resMessage.DefaultBehaviour("Loading To Found Null againest Vehicle No");
                    return resMessage;
                }
                List<TblLoadingSlipTO> loadingSlipTOList = new List<TblLoadingSlipTO>();
                for (int i = 0; i < loadingList.Count; i++)
                {
                    TblLoadingTO loadingEle = loadingList[i];
                    List<TblLoadingSlipTO> loadingSlipTOListById = new List<TblLoadingSlipTO>();
                    loadingSlipTOListById = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(loadingEle.IdLoading, conn, tran);
                    if (loadingSlipTOListById == null || loadingSlipTOListById.Count == 0)
                    {
                        resMessage.DefaultBehaviour("Loading Slip List Found Null againest Loading Id");
                        return resMessage;
                    }
                    loadingSlipTOList.AddRange(loadingSlipTOListById);
                }

                if (loadingSlipTOList == null || loadingSlipTOList.Count == 0)
                {
                    resMessage.DefaultBehaviour("Loading Slip List Found Null againest Vehicle No");
                    return resMessage;
                }
                string resultStr = "Invoices are not authorized for selected Vehicle " + vehicleNo + " Pending Loading slips are :  ";
                string LoadingSlipNos = string.Empty;
                for (int i = 0; i < loadingSlipTOList.Count; i++)
                {
                    List<TblInvoiceTO> invoiceToList = new List<TblInvoiceTO>();
                    invoiceToList = BL.TblInvoiceBL.SelectInvoiceListFromLoadingSlipId(loadingSlipTOList[i].IdLoadingSlip, conn, tran);
                    if (invoiceToList == null || invoiceToList.Count == 0)
                    {
                        LoadingSlipNos = string.IsNullOrEmpty(LoadingSlipNos) ? loadingSlipTOList[i].LoadingSlipNo : LoadingSlipNos + "," + loadingSlipTOList[i].LoadingSlipNo;
                    }

                    var unauthInvs = invoiceToList.Where(inv => inv.InvoiceStatusE != Constants.InvoiceStatusE.AUTHORIZED).ToList();
                    if (unauthInvs != null && unauthInvs.Count > 0)
                    {
                        LoadingSlipNos = string.IsNullOrEmpty(LoadingSlipNos) ? loadingSlipTOList[i].LoadingSlipNo : LoadingSlipNos + "," + loadingSlipTOList[i].LoadingSlipNo;
                    }
                }
                if (!string.IsNullOrEmpty(LoadingSlipNos))
                {
                    resMessage.MessageType = ResultMessageE.Error;
                    resMessage.DisplayMessage = resultStr + LoadingSlipNos;
                    resMessage.Text = resMessage.DisplayMessage;
                    resMessage.Result = 0;
                    return resMessage;
                }

                resMessage.DefaultSuccessBehaviour();
                return resMessage;
            }
            catch (Exception ex)
            {
                resMessage.DefaultExceptionBehaviour(ex, "CheckInvoiceNoGeneratedByVehicleNo");
                return resMessage;
            }
        }

        /*GJ@20170816 : Insert Weighing Machine Measurement : END*/

        public static ResultMessage UpdateLoadingSlipExtTo(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (tblLoadingSlipExtTO == null)
                {
                    tran.Rollback();
                    resultMessage.Text = "tblLoadingSlipExtTO Found Null : UpdateLoadingSlipExtTo";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                result = DAL.TblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO);
                if (result < 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {

                resultMessage.Text = "Exception Error While Record Save : UpdateLoadingSlipExtTo";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {

            }
        }

        #endregion

        #region Updation

        public static ResultMessage UpdateLoadingStatusOnWeighing(TblLoadingTO tblLoadingTO, TblWeighingMeasuresTO tblWeighingMeasuresTO, int weightMeasureSourceId, SqlConnection conn, SqlTransaction tran)
        {

            DateTime serverDatetime = Constants.ServerDateTime;
            // Update loading status 
            tblLoadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.LOADING_IN_PROGRESS);
            tblLoadingTO.StatusReason = "Loading In Progress";
            tblLoadingTO.StatusDate = serverDatetime;
            tblLoadingTO.IdLoading = tblLoadingTO.IdLoading;

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            TblLoadingTO tblLoadingTOnewStatus = new TblLoadingTO();
            if (weightMeasureSourceId == (int)Constants.WeighingDataSourceE.DB ||
                weightMeasureSourceId == (int)Constants.WeighingDataSourceE.BOTH)
            {
                if (tblLoadingTO.CallFlag != 0)
                {
                    tblLoadingTO.CallFlagBy = tblWeighingMeasuresTO.CreatedBy;
                }
                //Loading 
                result = TblLoadingSlipDAO.UpdateTblLoadingById(tblLoadingTO, conn, tran);
                if (result <= 0)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblLoadingById at Loading Progress Status Update");
                    return resultMessage;
                }

                // loadibg slip status 
                tblLoadingTOnewStatus.StatusId = Convert.ToInt16(Constants.TranStatusE.LOADING_IN_PROGRESS);
                tblLoadingTOnewStatus.StatusReason = "Loading In Progress";
                tblLoadingTOnewStatus.StatusDate = serverDatetime;
                tblLoadingTOnewStatus.LoadingDatetime = serverDatetime;
                tblLoadingTOnewStatus.IdLoading = tblLoadingTO.IdLoading;
                result = TblLoadingSlipDAO.UpdateTblLoadingSlip(tblLoadingTOnewStatus, conn, tran);
                if (result <= 0)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblLoadingSlip at Loading Progress Status Update");
                    return resultMessage;
                }
            }
            else
            {
                return TblLoadingBL.UpdateLoadingStatusToGateIoT(tblLoadingTO, conn, tran);
            }

            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public static int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO)
        {
            return TblWeighingMeasuresDAO.UpdateTblWeighingMeasures(tblWeighingMeasuresTO);
        }

        public static int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingMeasuresDAO.UpdateTblWeighingMeasures(tblWeighingMeasuresTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblWeighingMeasures(Int32 idWeightMeasure)
        {
            return TblWeighingMeasuresDAO.DeleteTblWeighingMeasures(idWeightMeasure);
        }

        public static int DeleteTblWeighingMeasures(Int32 idWeightMeasure, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingMeasuresDAO.DeleteTblWeighingMeasures(idWeightMeasure, conn, tran);
        }

        #endregion

    }
}
