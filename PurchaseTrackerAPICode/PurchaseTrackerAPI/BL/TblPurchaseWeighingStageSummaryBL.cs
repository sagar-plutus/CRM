using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.IoT.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseWeighingStageSummaryBL : ITblPurchaseWeighingStageSummaryBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly IPurchaseScheduleSummerycircularBL _iTblPurchaseScheduleSummarycircularBL;
        private readonly ICircularDependancyBL _iTblPurchaseScheduleSummaryBL;
        private readonly ITblPurchaseScheduleSummaryDAO _iTblPurchaseScheduleSummaryDAO;
        private readonly INotification notification;
        private readonly ICircularDependancyBL _iTblPurchaseEnquiryBL;
        private readonly ITblQualityPhaseBL _iTblQualityPhaseBL;
        private readonly ITblPurchaseWeighingStageSummaryDAO _iTblPurchaseWeighingStageSummaryDAO;
        private readonly ICircularDependancyBL _iTblPurchaseUnloadingDtlBL;
        private readonly ITblPurchaseGradingDtlsBL _iTblPurchaseGradingDtlsBL;
        private readonly ITblPurchaseVehicleStageCntBL _iTblPurchaseVehicleStageCntBL;
        private readonly ITblPurchaseScheduleStatusHistoryBL _iTblPurchaseScheduleStatusHistoryBL;
        private readonly IConnectionString _iConnectionString;
        private readonly IIotCommunication _iIotCommunication;
        private readonly IGateCommunication _iGateCommunication;
        private readonly ITblWeighingMachineDAO _iTblWeighingMachineDAO;
        private readonly IWeighingCommunication _iWeighingCommunication;
        private readonly IDimStatusDAO _iDimStatusDAO;
        private readonly ITblGateBL _iTblGateBL;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblPurchaseVehicleDetailsBL _iTblPurchaseVehicleDetailsBL;
        private readonly ITblPurchaseUnloadingDtlDAO _iTblPurchaseUnloadingDtlDAO;
        private readonly ITblUserBL _ITblUserBL;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        private readonly ITblPartyWeighingMeasuresDAO _iTblPartyWeighingMeasuresDAO;
        public TblPurchaseWeighingStageSummaryBL(
                         ICircularDependancyBL iTblPurchaseScheduleSummaryBL,
                         ITblQualityPhaseBL iTblQualityPhaseBL
                         , ITblPurchaseWeighingStageSummaryDAO iTblPurchaseWeighingStageSummaryDAO
                         , ICircularDependancyBL circularBL
                         , ITblPurchaseGradingDtlsBL iTblPurchaseGradingDtlsBL
                        , ITblPurchaseVehicleStageCntBL iTblPurchaseVehicleStageCntBL
                         , ITblPurchaseScheduleStatusHistoryBL iTblPurchaseScheduleStatusHistoryBL
                         , IPurchaseScheduleSummerycircularBL iTblPurchaseScheduleSummarycircularBL
                         , INotification inotification
                         , IConnectionString iConnectionString
                          , Icommondao icommondao, ITblGateBL iTblGateBL,
             ITblWeighingMachineDAO iTblWeighingMachineDAO, IGateCommunication iGateCommunication, IIotCommunication iIotCommunication
            , IWeighingCommunication iWeighingCommunication, IDimStatusDAO iDimStatusDAO, ITblConfigParamsDAO iTblConfigParamsDAO
           , ITblPurchaseScheduleSummaryDAO iTblPurchaseScheduleSummaryDAO, ITblPurchaseVehicleDetailsBL iTblPurchaseVehicleDetailsBL,
             ITblPurchaseUnloadingDtlDAO iTblPurchaseUnloadingDtlDAO, ITblUserBL ITblUserBL, ITblOrganizationBL iTblOrganizationBL, ITblPartyWeighingMeasuresDAO iTblPartyWeighingMeasuresDAO
             )
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            notification = inotification;
            _iTblPurchaseScheduleSummarycircularBL = iTblPurchaseScheduleSummarycircularBL;
            _iTblPurchaseUnloadingDtlBL = circularBL;
            _iTblPurchaseEnquiryBL = circularBL;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _iTblQualityPhaseBL = iTblQualityPhaseBL;
            _iTblPurchaseWeighingStageSummaryDAO = iTblPurchaseWeighingStageSummaryDAO;
            _iTblPurchaseGradingDtlsBL = iTblPurchaseGradingDtlsBL;
            _iTblPurchaseVehicleStageCntBL = iTblPurchaseVehicleStageCntBL;
            _iTblPurchaseScheduleStatusHistoryBL = iTblPurchaseScheduleStatusHistoryBL;
            _iIotCommunication = iIotCommunication;
            _iGateCommunication = iGateCommunication;
            _iTblWeighingMachineDAO = iTblWeighingMachineDAO;
            _iWeighingCommunication = iWeighingCommunication;
            _iDimStatusDAO = iDimStatusDAO;
            _iTblGateBL = iTblGateBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblPurchaseScheduleSummaryDAO = iTblPurchaseScheduleSummaryDAO;
            _iTblPurchaseVehicleDetailsBL = iTblPurchaseVehicleDetailsBL;
            _iTblPurchaseUnloadingDtlDAO = iTblPurchaseUnloadingDtlDAO;
            _ITblUserBL = ITblUserBL;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblPartyWeighingMeasuresDAO = iTblPartyWeighingMeasuresDAO;
        }



        #region Selection
        public List<TblPurchaseWeighingStageSummaryTO> SelectAllTblPurchaseWeighingStageSummary()
        {
            return _iTblPurchaseWeighingStageSummaryDAO.SelectAllTblPurchaseWeighingStageSummary();
        }

        public List<TblPurchaseWeighingStageSummaryTO> SelectAllTblPurchaseWeighingStageSummaryList()
        {
            return _iTblPurchaseWeighingStageSummaryDAO.SelectAllTblPurchaseWeighingStageSummary();
            //return ConvertDTToList(tblPurchaseWeighingStageSummaryTODT);
        }

        public TblPurchaseWeighingStageSummaryTO SelectTblPurchaseWeighingStageSummaryTO(Int32 idPurchaseWeighingStage)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.SelectTblPurchaseWeighingStageSummary(idPurchaseWeighingStage);
        }
        public TblPurchaseWeighingStageSummaryTO SelectTblPurchaseWeighingStageSummaryTO(Int32 idPurchaseWeighingStage, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.SelectTblPurchaseWeighingStageSummary(idPurchaseWeighingStage, conn, tran);
        }

        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleId(Int32 purchaseScheduleId, bool fromWeighing)
        {
            if (!fromWeighing)
            {
                List<TblPurchaseWeighingStageSummaryTO> weighingList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId, fromWeighing);
                if (weighingList != null && weighingList.Count > 0)
                {
                    //Added  by @KKM For fetch Gate Data From IoT
                    int confiqId = _iTblConfigParamsDAO.IoTSetting();
                    if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT) && weighingList.Count > 0)
                    {
                        TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDtlsTO(purchaseScheduleId, 0);
                        weighingList = _iIotCommunication.GetWeightDataFromIotAndMerge(tblPurchaseScheduleSummaryTO, weighingList);
                    }
                }
                return weighingList;
            }
            else
            {
                List<TblPurchaseWeighingStageSummaryTO> weighingList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId, fromWeighing);
                if (weighingList != null && weighingList.Count > 0)
                {
                    TblPartyWeighingMeasuresTO partyWeighingTO = _iTblPartyWeighingMeasuresDAO.SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(purchaseScheduleId);

                    Int32 isGradingBeforeUnld = 1;
                    for (int i = 0; i < weighingList.Count; i++)
                    {
                        List<TblPurchaseUnloadingDtlTO> unloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(weighingList[i].IdPurchaseWeighingStage, isGradingBeforeUnld);
                        if (unloadingDtlTOList != null && unloadingDtlTOList.Count > 0)
                        {
                            unloadingDtlTOList = unloadingDtlTOList.Where(a => a.IsNextUnldGrade == 1).ToList();

                            if (unloadingDtlTOList != null && unloadingDtlTOList.Count > 0)
                                weighingList[i].PurchaseUnloadingDtlTOList = unloadingDtlTOList;
                        }
                        if (partyWeighingTO != null)
                        {
                            weighingList[i].PartyGrossWt = partyWeighingTO.GrossWt;
                            weighingList[i].PartyNetWt = partyWeighingTO.NetWt;
                            weighingList[i].PartyTareWt = partyWeighingTO.TareWt;
                        }

                        weighingList[i].CreatedOnStr = Constants.GetDateWithFormate(weighingList[i].CreatedOn);

                    }

                }
                int confiqId = _iTblConfigParamsDAO.IoTSetting();
                if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT) && weighingList.Count > 0)
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDtlsTO(purchaseScheduleId, 0);
                    weighingList = _iIotCommunication.GetWeightDataFromIotAndMerge(tblPurchaseScheduleSummaryTO, weighingList);
                }
                return weighingList;
            }
        }

        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleIdForWeighingReport(Int32 purchaseScheduleId, bool fromWeighing)
        {
            if (!fromWeighing)
            {
                List<TblPurchaseWeighingStageSummaryTO> weighingList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetailsBySchduleIdForWeighingReport(purchaseScheduleId, fromWeighing);
                if (weighingList != null && weighingList.Count > 0)
                {
                    //Added  by @KKM For fetch Gate Data From IoT
                    int confiqId = _iTblConfigParamsDAO.IoTSetting();
                    if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT) && weighingList.Count > 0)
                    {
                        TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDtlsTO(purchaseScheduleId, 0);
                        weighingList = _iIotCommunication.GetWeightDataFromIotAndMerge(tblPurchaseScheduleSummaryTO, weighingList);
                    }
                }
                return weighingList;
            }
            else
            {
                List<TblPurchaseWeighingStageSummaryTO> weighingList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetailsBySchduleIdForWeighingReport(purchaseScheduleId, fromWeighing);
                if (weighingList != null && weighingList.Count > 0)
                {
                    TblPartyWeighingMeasuresTO partyWeighingTO = _iTblPartyWeighingMeasuresDAO.SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(purchaseScheduleId);

                    Int32 isGradingBeforeUnld = 1;
                    for (int i = 0; i < weighingList.Count; i++)
                    {
                        List<TblPurchaseUnloadingDtlTO> unloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(weighingList[i].IdPurchaseWeighingStage, isGradingBeforeUnld);
                        if (unloadingDtlTOList != null && unloadingDtlTOList.Count > 0)
                        {
                            unloadingDtlTOList = unloadingDtlTOList.Where(a => a.IsNextUnldGrade == 1).ToList();

                            if (unloadingDtlTOList != null && unloadingDtlTOList.Count > 0)
                                weighingList[i].PurchaseUnloadingDtlTOList = unloadingDtlTOList;
                        }
                        if (partyWeighingTO != null)
                        {
                            weighingList[i].PartyGrossWt = partyWeighingTO.GrossWt;
                            weighingList[i].PartyNetWt = partyWeighingTO.NetWt;
                            weighingList[i].PartyTareWt = partyWeighingTO.TareWt;
                        }

                        weighingList[i].CreatedOnStr = Constants.GetDateWithFormate(weighingList[i].CreatedOn);

                    }

                }
                int confiqId = _iTblConfigParamsDAO.IoTSetting();
                if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT) && weighingList.Count > 0)
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDtlsTO(purchaseScheduleId, 0);
                    weighingList = _iIotCommunication.GetWeightDataFromIotAndMerge(tblPurchaseScheduleSummaryTO, weighingList);
                }
                return weighingList;
            }
        }


        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, Boolean isGetAllWeighingDtls = false)
        {
            //Added  by @KKM For fetch Gate Data From IoT
            int confiqId = _iTblConfigParamsDAO.IoTSetting();
            if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDtlsTO(purchaseScheduleId, 0);
                var rootIdOrPurchaseId = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;
                List<TblPurchaseWeighingStageSummaryTO> TblPurchaseWeighingStageSummaryList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetailsBySchduleId(rootIdOrPurchaseId, false);
                TblPurchaseWeighingStageSummaryList = _iIotCommunication.GetWeightDataFromIotAndMerge(tblPurchaseScheduleSummaryTO, TblPurchaseWeighingStageSummaryList);
                if (!String.IsNullOrEmpty(weightTypeId))
                    TblPurchaseWeighingStageSummaryList = TblPurchaseWeighingStageSummaryList.Where(w => w.WeightMeasurTypeId == Convert.ToInt32(weightTypeId)).ToList();
                return TblPurchaseWeighingStageSummaryList;
            }
            else
            {
                return _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeightDetails(purchaseScheduleId, weightTypeId, isGetAllWeighingDtls);
            }
        }
        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, SqlConnection conn, SqlTransaction tran, Boolean isGetAllWeighingDtls = false)
        {
            //Added  by @KKM For fetch Gate Data From IoT
            int confiqId = _iTblConfigParamsDAO.IoTSetting();
            if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDtlsTO(purchaseScheduleId, 0);
                var rootIdOrPurchaseId = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;
                List<TblPurchaseWeighingStageSummaryTO> TblPurchaseWeighingStageSummaryList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeightDetails(rootIdOrPurchaseId, null, conn, tran, false);
                TblPurchaseWeighingStageSummaryList = _iIotCommunication.GetWeightDataFromIotAndMerge(tblPurchaseScheduleSummaryTO, TblPurchaseWeighingStageSummaryList);
                if (!String.IsNullOrEmpty(weightTypeId) && TblPurchaseWeighingStageSummaryList.Count > 0)
                    TblPurchaseWeighingStageSummaryList = TblPurchaseWeighingStageSummaryList.Where(w => w.WeightMeasurTypeId == Convert.ToInt32(weightTypeId)).ToList();
                return TblPurchaseWeighingStageSummaryList;
            } else
                return _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeightDetails(purchaseScheduleId, weightTypeId, conn, tran, isGetAllWeighingDtls);
        }
        public List<TblPurchaseWeighingStageSummaryTO> GetVehWtDetailsForWeighingMachine(Int32 purchaseScheduleId, string weightTypeId, string wtMachineIds, SqlConnection conn, SqlTransaction tran)
        {
            //Added  by @KKM For fetch Gate Data From IoT
            int confiqId = _iTblConfigParamsDAO.IoTSetting();
            if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDtlsTO(purchaseScheduleId, 0);
                var rootIdOrPurchaseId = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;
                List<TblPurchaseWeighingStageSummaryTO> TblPurchaseWeighingStageSummaryList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetailsBySchduleId(rootIdOrPurchaseId, false);
                TblPurchaseWeighingStageSummaryList = _iIotCommunication.GetWeightDataFromIotAndMerge(tblPurchaseScheduleSummaryTO, TblPurchaseWeighingStageSummaryList);
                TblPurchaseWeighingStageSummaryList = TblPurchaseWeighingStageSummaryList.Where(w => w.WeightMeasurTypeId == Convert.ToInt32(weightTypeId) && w.WeighingMachineId == Convert.ToInt32(wtMachineIds)).ToList();
                return TblPurchaseWeighingStageSummaryList;
            }
            else
                return _iTblPurchaseWeighingStageSummaryDAO.GetVehWtDetailsForWeighingMachine(purchaseScheduleId, weightTypeId, wtMachineIds, conn, tran);
        }

        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightAndUnloadingMaterialDetails(Int32 purchaseScheduleId, Int32 formTypeE)
        {
            Boolean isUnloadingDtlsPresent = false;
            Boolean isConfirmRecordPresent = false;
            Int32 isGradingBeforeUnld = 0;
            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryReturnTOList = new List<TblPurchaseWeighingStageSummaryTO>();

            if (formTypeE == (Int32)Constants.ItemGradingFormTypeE.RECOVERY)
            {

                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.GetVehicleDetailsByScheduleIds(purchaseScheduleId, (Int32)Constants.TranStatusE.UNLOADING_IS_IN_PROCESS, 0, 0);
                if (tblPurchaseScheduleSummaryTO == null)
                {
                    tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.GetVehicleDetailsByScheduleIds(purchaseScheduleId, (Int32)Constants.TranStatusE.UNLOADING_COMPLETED, (Int32)Constants.PurchaseVehiclePhasesE.UNLOADING_COMPLETED, 0);
                }

                if (tblPurchaseScheduleSummaryTO != null)
                {
                    purchaseScheduleId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
                }
            }

            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId, false);
            if (tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
            {
                // tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.Where(a => a.NetWeightMT > 0).ToList();
                if (tblPurchaseWeighingStageSummaryTOList.Count > 0)
                {

                    if (formTypeE == (Int32)Constants.ItemGradingFormTypeE.RECOVERY)
                    {

                        tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.Where(a => a.NetWeightMT > 0).ToList();
                        if (tblPurchaseWeighingStageSummaryTOList.Count == 0)
                            return null;

                        tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.OrderBy(a => a.IdPurchaseWeighingStage).ToList();


                        var temp = tblPurchaseWeighingStageSummaryTOList.Where(w => w.IsRecConfirm == 1).ToList();
                        if (temp != null && temp.Count > 0)
                        {
                            tblPurchaseWeighingStageSummaryReturnTOList.AddRange(temp);
                        }
                        var temp1 = tblPurchaseWeighingStageSummaryTOList.Where(w => w.IsRecConfirm == 0).FirstOrDefault();
                        if (temp1 != null)
                        {
                            tblPurchaseWeighingStageSummaryReturnTOList.Add(temp1);
                        }

                        return tblPurchaseWeighingStageSummaryReturnTOList;
                    }

                    if (formTypeE == (Int32)Constants.ItemGradingFormTypeE.UNLOADING)
                    {
                        tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.Where(a => a.NetWeightMT > 0).ToList();
                        if (tblPurchaseWeighingStageSummaryTOList.Count == 0)
                            return null;

                        tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.OrderBy(a => a.IdPurchaseWeighingStage).ToList();


                        for (int i = 0; i < tblPurchaseWeighingStageSummaryTOList.Count; i++)
                        {

                            tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
                            List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage, isGradingBeforeUnld);

                            if (i == 0)
                            {
                                if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                                {
                                    isUnloadingDtlsPresent = true;
                                    tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = tblPurchaseUnloadingDtlTOList;
                                    tblPurchaseWeighingStageSummaryReturnTOList.Add(tblPurchaseWeighingStageSummaryTOList[i]);
                                }
                                else
                                {
                                    isUnloadingDtlsPresent = true;
                                    tblPurchaseWeighingStageSummaryReturnTOList.Add(tblPurchaseWeighingStageSummaryTOList[i]);
                                }
                            }
                            else
                            {
                                //List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOTempList = tblPurchaseWeighingStageSummaryTOList.Where(a => a.IdPurchaseWeighingStage == tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage - 1).ToList();
                                List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOTempList = new List<TblPurchaseWeighingStageSummaryTO>();
                                tblPurchaseWeighingStageSummaryTOTempList.Add(tblPurchaseWeighingStageSummaryTOList[i - 1]);

                                if (tblPurchaseWeighingStageSummaryTOTempList != null && tblPurchaseWeighingStageSummaryTOTempList.Count == 1)
                                {
                                    if (tblPurchaseWeighingStageSummaryTOTempList[0].PurchaseUnloadingDtlTOList != null && tblPurchaseWeighingStageSummaryTOTempList[0].PurchaseUnloadingDtlTOList.Count > 0)
                                    {
                                        if (Convert.ToBoolean(tblPurchaseWeighingStageSummaryTOTempList[0].PurchaseUnloadingDtlTOList[0].IsConfirmUnloading))
                                        {
                                            if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                                            {
                                                tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = tblPurchaseUnloadingDtlTOList;
                                            }
                                            tblPurchaseWeighingStageSummaryReturnTOList.Add(tblPurchaseWeighingStageSummaryTOList[i]);
                                        }

                                    }

                                }
                            }
                        }
                        if (!isUnloadingDtlsPresent)
                        {
                            tblPurchaseWeighingStageSummaryReturnTOList = new List<TblPurchaseWeighingStageSummaryTO>();
                        }
                    }
                    else if (formTypeE == (Int32)Constants.ItemGradingFormTypeE.GRADING)
                    {
                        tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.Where(a => a.NetWeightMT > 0).ToList();
                        if (tblPurchaseWeighingStageSummaryTOList.Count == 0)
                            return null;

                        tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.OrderBy(a => a.IdPurchaseWeighingStage).ToList();

                        Double totalNetWtOfVeh = tblPurchaseWeighingStageSummaryTOList.Sum(a => a.NetWeightMT);

                        for (int i = 0; i < tblPurchaseWeighingStageSummaryTOList.Count; i++)
                        {
                            tblPurchaseWeighingStageSummaryTOList[i].TotalNetWtOfVeh = totalNetWtOfVeh;
                            if (i == 0)
                            {
                                List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsBL.SelectTblPurchaseGradingDtlsTOListByWeighingId(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
                                if (tblPurchaseGradingDtlsTOList != null && tblPurchaseGradingDtlsTOList.Count > 0)
                                {
                                    tblPurchaseWeighingStageSummaryTOList[i].PurchaseGradingDtlsTOList = new List<TblPurchaseGradingDtlsTO>();
                                    tblPurchaseWeighingStageSummaryTOList[i].PurchaseGradingDtlsTOList = tblPurchaseGradingDtlsTOList;
                                    tblPurchaseWeighingStageSummaryReturnTOList.Add(tblPurchaseWeighingStageSummaryTOList[i]);
                                }
                                else
                                {
                                    tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
                                    List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
                                    if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                                    {
                                        if (Convert.ToBoolean(tblPurchaseUnloadingDtlTOList[0].IsConfirmUnloading))
                                        {
                                            tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = tblPurchaseUnloadingDtlTOList;
                                            tblPurchaseWeighingStageSummaryReturnTOList.Add(tblPurchaseWeighingStageSummaryTOList[i]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Check previous Id Grading dtls
                                List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsBL.SelectTblPurchaseGradingDtlsTOListByWeighingId(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);

                                //List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOTempList = tblPurchaseWeighingStageSummaryTOList.Where(a => a.IdPurchaseWeighingStage == tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage - 1).ToList();
                                List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOTempList = new List<TblPurchaseWeighingStageSummaryTO>();
                                tblPurchaseWeighingStageSummaryTOTempList.Add(tblPurchaseWeighingStageSummaryTOList[i - 1]);

                                if (tblPurchaseWeighingStageSummaryTOTempList != null && tblPurchaseWeighingStageSummaryTOTempList.Count == 1)
                                {
                                    if (tblPurchaseWeighingStageSummaryTOTempList[0].PurchaseGradingDtlsTOList != null && tblPurchaseWeighingStageSummaryTOTempList[0].PurchaseGradingDtlsTOList.Count > 0)
                                    {
                                        if (Convert.ToBoolean(tblPurchaseWeighingStageSummaryTOTempList[0].PurchaseGradingDtlsTOList[0].IsConfirmGrading))
                                        {
                                            if (tblPurchaseGradingDtlsTOList != null && tblPurchaseGradingDtlsTOList.Count > 0)
                                            {
                                                tblPurchaseWeighingStageSummaryTOList[i].PurchaseGradingDtlsTOList = tblPurchaseGradingDtlsTOList;
                                                tblPurchaseWeighingStageSummaryReturnTOList.Add(tblPurchaseWeighingStageSummaryTOList[i]);
                                            }
                                            else
                                            {
                                                tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
                                                List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
                                                if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                                                {
                                                    if (Convert.ToBoolean(tblPurchaseUnloadingDtlTOList[0].IsConfirmUnloading))
                                                    {
                                                        tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = tblPurchaseUnloadingDtlTOList;
                                                        tblPurchaseWeighingStageSummaryReturnTOList.Add(tblPurchaseWeighingStageSummaryTOList[i]);
                                                    }
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                    else if (formTypeE == (Int32)Constants.ItemGradingFormTypeE.GRADING_BEFORE_UNLOADING)
                    {
                        isGradingBeforeUnld = 1;
                        for (int i = 0; i < tblPurchaseWeighingStageSummaryTOList.Count; i++)
                        {
                            tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
                            List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage, isGradingBeforeUnld);
                            if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                            {
                                tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = tblPurchaseUnloadingDtlTOList;
                                // tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = tblPurchaseUnloadingDtlTOList.Where(a => a.IsGradingBeforeUnld == 1).ToList();
                                // tblPurchaseWeighingStageSummaryTOList[i].AfterGradingUnloadingDtlTOList = tblPurchaseUnloadingDtlTOList.Where(a => a.IsGradingBeforeUnld == 0 && a.IsNextUnldGrade == 0).ToList();
                            }
                        }
                        tblPurchaseWeighingStageSummaryReturnTOList = tblPurchaseWeighingStageSummaryTOList;

                    }

                }

            }
            return tblPurchaseWeighingStageSummaryReturnTOList;
        }

        //chetan[28-feb-2020] added for get  tblPurchase weighing to and loding detail

        //public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightAndUnloadingMaterialDetailsTOList(Int32 purchaseScheduleId)
        //{
        //    try
        //    {

        //        TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = new TblPurchaseWeighingStageSummaryTO();
        //        List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryReturnTOList = new List<TblPurchaseWeighingStageSummaryTO>();

        //        List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId, false);
        //        if (tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
        //        {
        //            tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.Where(a => a.NetWeightMT > 0).ToList();
        //            if (tblPurchaseWeighingStageSummaryTOList.Count == 0)
        //                return null;
        //            tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.OrderBy(a => a.IdPurchaseWeighingStage).ToList();
        //            tblPurchaseWeighingStageSummaryTO = tblPurchaseWeighingStageSummaryTOList[0];
        //            tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
        //            Double totalNetWtOfVeh = tblPurchaseWeighingStageSummaryTOList.Sum(a => a.NetWeightMT);

        //            Dictionary<int, int> productIdDCT = new Dictionary<int, int>();


        //            for (int i = 0; i < tblPurchaseWeighingStageSummaryTOList.Count; i++)
        //            {
        //                List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsBL.SelectTblPurchaseGradingDtlsTOListByWeighingId(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
        //                if (tblPurchaseGradingDtlsTOList != null && tblPurchaseGradingDtlsTOList.Count > 0)
        //                {

        //                    for (int j = 0; j < tblPurchaseGradingDtlsTOList.Count; j++)
        //                    {
        //                        TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO = new TblPurchaseGradingDtlsTO();
        //                        if (productIdDCT.ContainsKey(tblPurchaseGradingDtlsTOList[j].ProdItemId))
        //                        {
        //                            tblPurchaseGradingDtlsTO = tblPurchaseWeighingStageSummaryTO.PurchaseGradingDtlsTOList.Where(w => w.ProdItemId == tblPurchaseGradingDtlsTOList[j].ProdItemId).FirstOrDefault();
        //                            if (tblPurchaseGradingDtlsTO != null)
        //                            {
        //                                tblPurchaseGradingDtlsTO.QtyMT += tblPurchaseGradingDtlsTOList[j].QtyMT;
        //                                //tblPurchaseGradingDtlsTO.ProductAmount += tblPurchaseGradingDtlsTOList[j].ProductAmount;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            productIdDCT.Add(tblPurchaseGradingDtlsTOList[j].ProdItemId, tblPurchaseGradingDtlsTOList[j].ProdItemId);
        //                            tblPurchaseGradingDtlsTO = tblPurchaseGradingDtlsTOList[j];
        //                            tblPurchaseWeighingStageSummaryTO.PurchaseGradingDtlsTOList.Add(tblPurchaseGradingDtlsTO);
        //                        }


        //                    }
        //                    //tblPurchaseWeighingStageSummaryTO.PurchaseGradingDtlsTOList.AddRange(tblPurchaseGradingDtlsTOList);
        //                }
        //                else
        //                {
        //                    List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
        //                    if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
        //                    {
        //                        if (Convert.ToBoolean(tblPurchaseUnloadingDtlTOList[0].IsConfirmUnloading))
        //                        {
        //                            for (int c = 0; c < tblPurchaseUnloadingDtlTOList.Count; c++)
        //                            {
        //                                TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO = new TblPurchaseUnloadingDtlTO();
        //                                if (productIdDCT.ContainsKey(tblPurchaseUnloadingDtlTOList[c].ProdItemId))
        //                                {
        //                                    if (tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList != null
        //                                        && tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList.Count > 0)
        //                                    {
        //                                        tblPurchaseUnloadingDtlTO = tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList.Where(w => w.ProdItemId == tblPurchaseUnloadingDtlTOList[c].ProdItemId).FirstOrDefault();
        //                                        if (tblPurchaseUnloadingDtlTO == null)
        //                                        {
        //                                            TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO = tblPurchaseWeighingStageSummaryTO.PurchaseGradingDtlsTOList.Where(w => w.ProdItemId == tblPurchaseUnloadingDtlTOList[c].ProdItemId).FirstOrDefault();
        //                                            if (tblPurchaseGradingDtlsTO != null)
        //                                            {
        //                                                tblPurchaseGradingDtlsTO.QtyMT += tblPurchaseUnloadingDtlTOList[c].QtyMT;
        //                                                //tblPurchaseGradingDtlsTO.ProductAmount += tblPurchaseUnloadingDtlTOList[c].ra;
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            tblPurchaseUnloadingDtlTO.QtyMT += tblPurchaseUnloadingDtlTOList[c].QtyMT;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        tblPurchaseUnloadingDtlTO = tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList.Where(w => w.ProdItemId == tblPurchaseUnloadingDtlTOList[c].ProdItemId).FirstOrDefault();
        //                                        if (tblPurchaseUnloadingDtlTO == null)
        //                                        {
        //                                            TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO = tblPurchaseWeighingStageSummaryTO.PurchaseGradingDtlsTOList.Where(w => w.ProdItemId == tblPurchaseUnloadingDtlTOList[c].ProdItemId).FirstOrDefault();
        //                                            if (tblPurchaseGradingDtlsTO != null)
        //                                            {
        //                                                tblPurchaseGradingDtlsTO.QtyMT += tblPurchaseUnloadingDtlTOList[c].QtyMT;
        //                                                //tblPurchaseGradingDtlsTO.ProductAmount += tblPurchaseUnloadingDtlTOList[c].ra;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    productIdDCT.Add(tblPurchaseUnloadingDtlTOList[c].ProdItemId, tblPurchaseUnloadingDtlTOList[c].ProdItemId);
        //                                    tblPurchaseUnloadingDtlTO = tblPurchaseUnloadingDtlTOList[c];
        //                                    if (tblPurchaseUnloadingDtlTO != null)
        //                                        tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList.Add(tblPurchaseUnloadingDtlTO);
        //                                }

        //                            }
        //                            //tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList.AddRange(tblPurchaseUnloadingDtlTOList);
        //                        }
        //                    }
        //                }
        //            }
        //            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOFinalList = new List<TblPurchaseWeighingStageSummaryTO>();
        //            tblPurchaseWeighingStageSummaryTOFinalList.Add(tblPurchaseWeighingStageSummaryTO);
        //            return tblPurchaseWeighingStageSummaryTOFinalList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return null;
        //}

        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightAndUnloadingMaterialDetailsTOList(Int32 purchaseScheduleId)
        {
            try
            {

                TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = new TblPurchaseWeighingStageSummaryTO();
                List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryReturnTOList = new List<TblPurchaseWeighingStageSummaryTO>();

                Dictionary<int, int> productIdDCT = new Dictionary<int, int>();

                List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId, false);
                if (tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
                {
                    tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.Where(a => a.NetWeightMT > 0).ToList();
                    if (tblPurchaseWeighingStageSummaryTOList.Count == 0)
                        return null;
                    tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.OrderBy(a => a.IdPurchaseWeighingStage).ToList();
                    tblPurchaseWeighingStageSummaryTO = tblPurchaseWeighingStageSummaryTOList[0];
                    tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
                    Double totalNetWtOfVeh = tblPurchaseWeighingStageSummaryTOList.Sum(a => a.NetWeightMT);
                    tblPurchaseWeighingStageSummaryTO.TotalNetWtOfVeh = totalNetWtOfVeh;
                    //List<TblPurchaseGradingDtlsTO> gradingDtlsTempList = new List<TblPurchaseGradingDtlsTO>();
                    List<TblPurchaseUnloadingDtlTO> unloadingDtlsTempList = new List<TblPurchaseUnloadingDtlTO>();

                    for (int i = 0; i < tblPurchaseWeighingStageSummaryTOList.Count; i++)
                    {
                        List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsBL.SelectTblPurchaseGradingDtlsTOListByWeighingId(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
                        if (tblPurchaseGradingDtlsTOList != null && tblPurchaseGradingDtlsTOList.Count > 0)
                        {
                            tblPurchaseWeighingStageSummaryTO.PurchaseGradingDtlsTOList.AddRange(tblPurchaseGradingDtlsTOList);
                            break;
                        }
                        else
                        {
                            List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
                            if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                            {
                                if (Convert.ToBoolean(tblPurchaseUnloadingDtlTOList[0].IsConfirmUnloading))
                                {
                                    for (int k = 0; k < tblPurchaseUnloadingDtlTOList.Count; k++)
                                    {

                                        if (productIdDCT.ContainsKey(tblPurchaseUnloadingDtlTOList[k].ProdItemId))
                                        {
                                            TblPurchaseUnloadingDtlTO tempTO = unloadingDtlsTempList.Where(a => a.ProdItemId == tblPurchaseUnloadingDtlTOList[k].ProdItemId).FirstOrDefault();
                                            if (tempTO != null)
                                            {
                                                tempTO.QtyMT = Math.Round((tempTO.QtyMT + tblPurchaseUnloadingDtlTOList[k].QtyMT), 3);
                                                //tempTO.ProductAmount += tblPurchaseUnloadingDtlTOList[k].ProductAmount;
                                            }
                                        }
                                        else
                                        {
                                            TblPurchaseUnloadingDtlTO tempGradingTO = new TblPurchaseUnloadingDtlTO();
                                            tempGradingTO.ProdItemId = tblPurchaseUnloadingDtlTOList[k].ProdItemId;
                                            tempGradingTO.ItemName = tblPurchaseUnloadingDtlTOList[k].ItemName;
                                            tempGradingTO.QtyMT = tblPurchaseUnloadingDtlTOList[k].QtyMT;
                                            tempGradingTO.IsNonCommercialItem = tblPurchaseUnloadingDtlTOList[k].IsNonCommercialItem;
                                            tempGradingTO.IsGradeSelected = tblPurchaseUnloadingDtlTOList[k].IsGradeSelected;

                                            unloadingDtlsTempList.Add(tempGradingTO);
                                            productIdDCT.Add(tempGradingTO.ProdItemId, tempGradingTO.ProdItemId);
                                        }

                                    }

                                }
                            }
                        }
                    }
                    if (unloadingDtlsTempList != null && unloadingDtlsTempList.Count > 0)
                        tblPurchaseWeighingStageSummaryTO.PurchaseUnloadingDtlTOList.AddRange(unloadingDtlsTempList);

                    List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOFinalList = new List<TblPurchaseWeighingStageSummaryTO>();
                    tblPurchaseWeighingStageSummaryTOFinalList.Add(tblPurchaseWeighingStageSummaryTO);
                    return tblPurchaseWeighingStageSummaryTOFinalList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public List<GradeInfoTO> GradeInfoTOListForSudharReport(int purchaseScheduleId)
        {
            List<GradeInfoTO> finalGradeInfoTOList = new List<GradeInfoTO>();
            try
            {
                Dictionary<int, int> productIdDCT = new Dictionary<int, int>();
                GradeInfoTO GradeInfoTODust = new GradeInfoTO();
                GradeInfoTODust.GradeName = "Dust";
                GradeInfoTODust.GradeQtyUnloaded = 0;
                // List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId, false);
                List<TblPurchaseScheduleSummaryTO> TblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryDAO.SelectVehicleScheduleByRootAndStatusId(purchaseScheduleId, 0, 0);

                if (TblPurchaseScheduleSummaryTOList != null && TblPurchaseScheduleSummaryTOList.Count > 0)
                {
                    //for (int i = 0; i < TblPurchaseScheduleSummaryTOList.Count; i++)
                    //{
                    // List<TblPurchaseScheduleSummaryTO> TblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryDAO.SelectVehicleScheduleByRootAndStatusId(tblPurchaseWeighingStageSummaryTOList[i].PurchaseScheduleSummaryId, (int)(Int32)Constants.TranStatusE.UNLOADING_COMPLETED, (int)StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS);
                    //if (TblPurchaseScheduleSummaryTOList != null && TblPurchaseScheduleSummaryTOList.Count > 0)
                    //{
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = TblPurchaseScheduleSummaryTOList.Where(w => w.VehiclePhaseId == (int)StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS && w.StatusId == (Int32)Constants.TranStatusE.UNLOADING_COMPLETED).FirstOrDefault();
                    //TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = TblPurchaseScheduleSummaryTOList[i];
                    //if (tblPurchaseScheduleSummaryTO.VehiclePhaseId != (int)StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS)
                    //    continue;
                    if (tblPurchaseScheduleSummaryTO != null)
                    {
                        List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDtlsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary);
                        if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                        {
                            for (int j = 0; j < tblPurchaseVehicleDetailsTOList.Count; j++)
                            {
                                TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO = tblPurchaseVehicleDetailsTOList[j];
                                if (tblPurchaseVehicleDetailsTO.IsNonCommercialItem == 1)
                                    continue;
                                if (productIdDCT.ContainsKey(tblPurchaseVehicleDetailsTO.ProdItemId))
                                {
                                    GradeInfoTO gradeInfoTOExist = finalGradeInfoTOList.Where(a => a.ProdItemId == tblPurchaseVehicleDetailsTO.ProdItemId).FirstOrDefault();
                                    if (gradeInfoTOExist != null)
                                    {
                                        gradeInfoTOExist.GradeQtyUnloaded = Math.Round((gradeInfoTOExist.GradeQtyUnloaded + tblPurchaseVehicleDetailsTO.Qty), 3);
                                    }

                                }
                                else
                                {
                                    GradeInfoTO GradeInfoTO = new GradeInfoTO();
                                    GradeInfoTO.GradeQtyUnloaded = tblPurchaseVehicleDetailsTO.Qty;
                                    GradeInfoTO.GradeName = tblPurchaseVehicleDetailsTO.ItemName;
                                    GradeInfoTO.ProdItemId = tblPurchaseVehicleDetailsTO.ProdItemId;
                                    finalGradeInfoTOList.Add(GradeInfoTO);
                                    productIdDCT.Add(GradeInfoTO.ProdItemId, GradeInfoTO.ProdItemId);
                                }
                            }

                            tblPurchaseVehicleDetailsTOList = tblPurchaseVehicleDetailsTOList.Where(w => w.IsNonCommercialItem == 1).ToList();
                            if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                            {
                                double dustQty = tblPurchaseVehicleDetailsTOList.Sum(a => a.Qty);
                                dustQty = Math.Round(dustQty, 3);
                                GradeInfoTODust.GradeQtyUnloaded += dustQty;
                            }
                        }
                    }

                    //List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
                    //if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                    //{
                    //    if (Convert.ToBoolean(tblPurchaseUnloadingDtlTOList[0].IsConfirmUnloading))
                    //    {
                    //        for (int k = 0; k < tblPurchaseUnloadingDtlTOList.Count; k++)
                    //        {

                    //            if (productIdDCT.ContainsKey(tblPurchaseUnloadingDtlTOList[k].ProdItemId))
                    //            {
                    //                GradeInfoTO gradeInfoTOExist = finalGradeInfoTOList.Where(a => a.ProdItemId == tblPurchaseUnloadingDtlTOList[k].ProdItemId).FirstOrDefault();
                    //                if (gradeInfoTOExist != null)
                    //                {
                    //                    gradeInfoTOExist.GradeQtyUnloaded = Math.Round((gradeInfoTOExist.GradeQtyUnloaded + tblPurchaseUnloadingDtlTOList[k].QtyMT), 3);
                    //                    //tempTO.ProductAmount += tblPurchaseUnloadingDtlTOList[k].ProductAmount;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (tblPurchaseUnloadingDtlTOList[k].IsNonCommercialItem == 1)
                    //                {
                    //                    GradeInfoTODust.GradeQtyUnloaded += tblPurchaseUnloadingDtlTOList[k].QtyMT;
                    //                }
                    //                else
                    //                {
                    //                    GradeInfoTO GradeInfoTO = new GradeInfoTO();
                    //                    GradeInfoTO.GradeQtyUnloaded = tblPurchaseUnloadingDtlTOList[k].QtyMT;
                    //                    GradeInfoTO.GradeName = tblPurchaseUnloadingDtlTOList[k].ItemName;
                    //                    GradeInfoTO.ProdItemId = tblPurchaseUnloadingDtlTOList[k].ProdItemId;
                    //                    finalGradeInfoTOList.Add(GradeInfoTO);
                    //                    productIdDCT.Add(GradeInfoTO.ProdItemId, GradeInfoTO.ProdItemId);
                    //                }
                    //            }

                    //        }
                    //    }
                    //}
                    //}
                    if (finalGradeInfoTOList != null && finalGradeInfoTOList.Count > 0)
                    {
                        GradeInfoTO gradeInfoTOGT = new GradeInfoTO();
                        gradeInfoTOGT.GradeName = "Total";
                        gradeInfoTOGT.IsGrandTotalRecord = true;
                        gradeInfoTOGT.GradeQtyUnloaded = Math.Round(finalGradeInfoTOList.Sum(s => s.GradeQtyUnloaded), 3);
                        GradeInfoTODust.IsDustRecord = true;
                        finalGradeInfoTOList.Add(GradeInfoTODust);

                        GradeInfoTO gradeInfoTO = new GradeInfoTO();
                        gradeInfoTO.GradeQtyUnloaded = Math.Round(finalGradeInfoTOList.Sum(s => s.GradeQtyUnloaded), 3);
                        gradeInfoTO.IsTotalRecord = true;
                        finalGradeInfoTOList.Add(gradeInfoTOGT);
                        finalGradeInfoTOList.Add(gradeInfoTO);
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return finalGradeInfoTOList;
        }

        public List<UnloadedTimeReportTO> UnlodingTimeRport(string fromDate, string toDate, Int32 isForWeighingPointWise,Boolean isForReport,String purchaseManagerIds)
        {
            List<UnloadedTimeReportTO> UnloadedTimeReportTOList = new List<UnloadedTimeReportTO>();
            //List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = _iTblPurchaseWeighingStageSummaryDAO.SelectAllTblPurchaseWeighingStageSummary();
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            string seprator = ",";
            if (isForReport)
                seprator = "\n";
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = _iTblPurchaseWeighingStageSummaryDAO.SelectTblPurchaseWeighingStageSummary(from_Date, to_Date, purchaseManagerIds);
            if (tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
            {
                // string idWeighingStageSummerIdStr = string.Join(',', tblPurchaseWeighingStageSummaryTOList.Select(s => s.IdPurchaseWeighingStage.ToString()).ToArray());
                // List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(idWeighingStageSummerIdStr);
                List<TblPurchaseWeighingStageSummaryTO> supervisorIdList = tblPurchaseWeighingStageSummaryTOList.GroupBy(g => g.SupervisorId).Select(s => s.FirstOrDefault()).ToList();
                if (supervisorIdList != null && supervisorIdList.Count > 0)
                {
                    for (int i = 0; i < supervisorIdList.Count; i++)
                    {
                        int supervisorId = supervisorIdList[i].SupervisorId;
                        if (supervisorId == 0)
                            continue;
                        TblUserTO tblUserTO = _ITblUserBL.SelectTblUserTO(supervisorId);
                        List<TblPurchaseWeighingStageSummaryTO> superviserWeighingStageSummaryList = tblPurchaseWeighingStageSummaryTOList.Where(w => w.SupervisorId == supervisorId).ToList();
                        if (superviserWeighingStageSummaryList != null && superviserWeighingStageSummaryList.Count > 0)
                        {
                            //if (isForWeighingPointWise==1)
                            //{
                            //    //TblOrganizationTO tblOrganizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(VehicleIdList[j].SupplierId);
                            //    UnloadedTimeReportTO unloadedTimeReportTO = new UnloadedTimeReportTO();
                            //    unloadedTimeReportTO.UnloadingPointName = tblUserTO.UserDisplayName;
                            //    unloadedTimeReportTO.GraderName = tblUserTO.UserDisplayName;
                            //    // unloadedTimeReportTO.PartyName = tblOrganizationTO.FirmName;
                            //    //unloadedTimeReportTO.VehicleNo = vehicleNo;
                            //    Dictionary<string, double> gradeUnloadedDCT = new Dictionary<string, double>();
                            //    Dictionary<string, double> quantityUnloadedDCT = new Dictionary<string, double>();
                            //    Int64 avgUnloadingTime = 0;
                            //    // List<TblPurchaseWeighingStageSummaryTO> VehicleWiseStageSummaryList = superviserWeighingStageSummaryList.Where(w => w.VehicleNo == vehicleNo).ToList();
                            //    for (int k = 0; k < superviserWeighingStageSummaryList.Count; k++)
                            //    {
                            //        TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = superviserWeighingStageSummaryList[k];
                            //        TblPurchaseWeighingStageSummaryTO parentWeighingStageSummaryTO = new TblPurchaseWeighingStageSummaryTO();
                            //        List<TblPurchaseWeighingStageSummaryTO> parentWeighingStageSummaryTOList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetails(tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId, tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage);
                            //        if (parentWeighingStageSummaryTOList != null && parentWeighingStageSummaryTOList.Count > 0)
                            //        {
                            //            parentWeighingStageSummaryTO = parentWeighingStageSummaryTOList[0];
                            //            avgUnloadingTime += parentWeighingStageSummaryTO.IntervalTime;
                            //        }
                            //        List<TblPurchaseUnloadingDtlTO> gradeUnloadedUnloadingDtlTOList = _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(parentWeighingStageSummaryTO.IdPurchaseWeighingStage, 1);
                            //        if (gradeUnloadedUnloadingDtlTOList != null && gradeUnloadedUnloadingDtlTOList.Count > 0)
                            //        {
                            //            for (int c = 0; c < gradeUnloadedUnloadingDtlTOList.Count; c++)
                            //            {
                            //                TblPurchaseUnloadingDtlTO gradeUnloadedUnloadingDtlTO = gradeUnloadedUnloadingDtlTOList[c];

                            //                if (gradeUnloadedDCT.ContainsKey(gradeUnloadedUnloadingDtlTO.ItemName))
                            //                {
                            //                    gradeUnloadedDCT[gradeUnloadedUnloadingDtlTO.ItemName] += gradeUnloadedUnloadingDtlTO.QtyMT;
                            //                }
                            //                else
                            //                {
                            //                    gradeUnloadedDCT.Add(gradeUnloadedUnloadingDtlTO.ItemName, gradeUnloadedUnloadingDtlTO.QtyMT);
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            continue;
                            //        }
                            //        List<TblPurchaseUnloadingDtlTO> quantityUnloadedDtlTOList = _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage, 0);
                            //        if (quantityUnloadedDtlTOList != null && quantityUnloadedDtlTOList.Count > 0)
                            //        {
                            //            for (int c = 0; c < quantityUnloadedDtlTOList.Count; c++)
                            //            {
                            //                TblPurchaseUnloadingDtlTO quantityUnloadedDtlTO = quantityUnloadedDtlTOList[c];

                            //                if (quantityUnloadedDCT.ContainsKey(quantityUnloadedDtlTO.ItemName))
                            //                {
                            //                    quantityUnloadedDCT[quantityUnloadedDtlTO.ItemName] += quantityUnloadedDtlTO.QtyMT;
                            //                }
                            //                else
                            //                {
                            //                    quantityUnloadedDCT.Add(quantityUnloadedDtlTO.ItemName, quantityUnloadedDtlTO.QtyMT);
                            //                }
                            //            }
                            //        }
                            //    }
                            //    string gradeUnloadeStr = string.Empty;
                            //    foreach (var item in gradeUnloadedDCT)
                            //    {
                            //        gradeUnloadeStr += item.Key.ToString() + "-" + item.Value.ToString() + seprator;
                            //    }
                            //    string quantityUnloadedStr = string.Empty;
                            //    foreach (var item in quantityUnloadedDCT)
                            //    {
                            //        quantityUnloadedStr += item.Key.ToString() + "-" + item.Value.ToString() + seprator;
                            //    }
                            //    unloadedTimeReportTO.AvgUnloadingTime = Math.Round((Convert.ToDouble(avgUnloadingTime) / superviserWeighingStageSummaryList.Count), 2);
                            //    unloadedTimeReportTO.GradeUnloaded = gradeUnloadeStr;
                            //    unloadedTimeReportTO.QuantityUnloaded = quantityUnloadedStr;
                            //    UnloadedTimeReportTOList.Add(unloadedTimeReportTO);

                            //}
                            //else
                            //{
                                List<TblPurchaseWeighingStageSummaryTO> VehicleIdList = superviserWeighingStageSummaryList.GroupBy(g => g.VehicleNo).Select(s => s.FirstOrDefault()).ToList();
                                if (VehicleIdList != null && VehicleIdList.Count > 0)
                                {
                                    for (int j = 0; j < VehicleIdList.Count; j++)
                                    {
                                        string vehicleNo = VehicleIdList[j].VehicleNo;
                                        TblOrganizationTO tblOrganizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(VehicleIdList[j].SupplierId);
                                        UnloadedTimeReportTO unloadedTimeReportTO = new UnloadedTimeReportTO();
                                        unloadedTimeReportTO.UnloadingPointName = tblUserTO.UserDisplayName;
                                        unloadedTimeReportTO.GraderName = tblUserTO.UserDisplayName;
                                        unloadedTimeReportTO.PartyName = tblOrganizationTO.FirmName;
                                        unloadedTimeReportTO.VehicleNo = vehicleNo;
                                        Dictionary<string, double> gradeUnloadedDCT = new Dictionary<string, double>();
                                        Dictionary<string, double> quantityUnloadedDCT = new Dictionary<string, double>();
                                        Int64 avgUnloadingTime = 0;
                                        List<TblPurchaseWeighingStageSummaryTO> VehicleWiseStageSummaryList = superviserWeighingStageSummaryList.Where(w => w.VehicleNo == vehicleNo).ToList();
                                        for (int k = 0; k < VehicleWiseStageSummaryList.Count; k++)
                                        {
                                            TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = VehicleWiseStageSummaryList[k];
                                            TblPurchaseWeighingStageSummaryTO parentWeighingStageSummaryTO = new TblPurchaseWeighingStageSummaryTO();
                                            List<TblPurchaseWeighingStageSummaryTO> parentWeighingStageSummaryTOList = _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeighingDetails(tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId, tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage);
                                            if (parentWeighingStageSummaryTOList != null && parentWeighingStageSummaryTOList.Count > 0)
                                            {
                                                parentWeighingStageSummaryTO = parentWeighingStageSummaryTOList[0];
                                            TimeSpan tsIntervalTime = parentWeighingStageSummaryTO.GradingEndTime - parentWeighingStageSummaryTO.GradingStartTime;

                                            avgUnloadingTime += Convert.ToInt64(Math.Round(tsIntervalTime.TotalMinutes));
                                            }
                                            List<TblPurchaseUnloadingDtlTO> gradeUnloadedUnloadingDtlTOList = _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(parentWeighingStageSummaryTO.IdPurchaseWeighingStage, 1);
                                            if (gradeUnloadedUnloadingDtlTOList != null && gradeUnloadedUnloadingDtlTOList.Count > 0)
                                            {
                                                for (int c = 0; c < gradeUnloadedUnloadingDtlTOList.Count; c++)
                                                {

                                                    TblPurchaseUnloadingDtlTO gradeUnloadedUnloadingDtlTO = gradeUnloadedUnloadingDtlTOList[c];
                                                    if (gradeUnloadedUnloadingDtlTO.IsNextUnldGrade==1)
                                                        continue;
                                                    if (gradeUnloadedDCT.ContainsKey(gradeUnloadedUnloadingDtlTO.ItemName))
                                                    {
                                                        gradeUnloadedDCT[gradeUnloadedUnloadingDtlTO.ItemName] += gradeUnloadedUnloadingDtlTO.QtyMT;
                                                    }
                                                    else
                                                    {
                                                        gradeUnloadedDCT.Add(gradeUnloadedUnloadingDtlTO.ItemName, gradeUnloadedUnloadingDtlTO.QtyMT);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                            List<TblPurchaseUnloadingDtlTO> quantityUnloadedDtlTOList = _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage, 0);
                                            if (quantityUnloadedDtlTOList != null && quantityUnloadedDtlTOList.Count > 0)
                                            {
                                                for (int c = 0; c < quantityUnloadedDtlTOList.Count; c++)
                                                {
                                                    TblPurchaseUnloadingDtlTO quantityUnloadedDtlTO = quantityUnloadedDtlTOList[c];

                                                    if (quantityUnloadedDCT.ContainsKey(quantityUnloadedDtlTO.ItemName))
                                                    {
                                                        quantityUnloadedDCT[quantityUnloadedDtlTO.ItemName] += quantityUnloadedDtlTO.QtyMT;
                                                    }
                                                    else
                                                    {
                                                        quantityUnloadedDCT.Add(quantityUnloadedDtlTO.ItemName, quantityUnloadedDtlTO.QtyMT);
                                                    }
                                                }
                                            }
                                        }
                                        string gradeUnloadeStr = string.Empty;
                                        foreach (var item in gradeUnloadedDCT)
                                        {
                                            gradeUnloadeStr += item.Key.ToString() + "-" + item.Value.ToString() + seprator;
                                        }
                                        string quantityUnloadedStr = string.Empty;
                                        foreach (var item in quantityUnloadedDCT)
                                        {
                                            quantityUnloadedStr += item.Key.ToString() + "-" + item.Value.ToString() + seprator;
                                        }
                                        unloadedTimeReportTO.AvgUnloadingTime = Math.Round((Convert.ToDouble(avgUnloadingTime) / VehicleWiseStageSummaryList.Count), 2);
                                        unloadedTimeReportTO.GradeUnloaded = gradeUnloadeStr;
                                        unloadedTimeReportTO.QuantityUnloaded = quantityUnloadedStr;
                                        UnloadedTimeReportTOList.Add(unloadedTimeReportTO);
                                    }
                                }
                           // }
                        }
                    }
                }
            }

            if(UnloadedTimeReportTOList!=null && UnloadedTimeReportTOList.Count>0)
            {
                if (isForWeighingPointWise == 1)
                {
                    UnloadedTimeReportTOList = UnloadedTimeReportTOList.OrderByDescending(s => s.AvgUnloadingTime).ToList();
                    List<UnloadedTimeReportTO> sortunloadedTimeReportTOList = new List<UnloadedTimeReportTO>();
                    List<UnloadedTimeReportTO> unloadedPointList = UnloadedTimeReportTOList.GroupBy(g => g.UnloadingPointName).Select(s => s.FirstOrDefault()).ToList();
                    for (int i = 0; i < unloadedPointList.Count; i++)
                    {
                        UnloadedTimeReportTO unloadedTimeReportTO = unloadedPointList[i];
                        unloadedTimeReportTO.PartyName += "(" + unloadedTimeReportTO.AvgUnloadingTime + ")";
                        List<UnloadedTimeReportTO> unloadedTimeReportTOListPointWise = UnloadedTimeReportTOList.Where(w => w.UnloadingPointName == unloadedTimeReportTO.UnloadingPointName).ToList();
                        unloadedTimeReportTO.AvgUnloadingTime =Math.Round(((unloadedTimeReportTOListPointWise.Sum(s => s.AvgUnloadingTime)) / unloadedTimeReportTOListPointWise.Count),2);
                        sortunloadedTimeReportTOList.Add(unloadedTimeReportTO);
                    }
                    UnloadedTimeReportTOList = sortunloadedTimeReportTOList;
                }
                else
                {
                    double matAvgUnloadingTime = UnloadedTimeReportTOList.Max(m => m.AvgUnloadingTime);
                    for (int i = 0; i < UnloadedTimeReportTOList.Count; i++)
                    {
                        if (UnloadedTimeReportTOList[i].AvgUnloadingTime == matAvgUnloadingTime)
                        {
                            UnloadedTimeReportTOList[i].IsMax = true;
                        }
                        else
                        {
                            UnloadedTimeReportTOList[i].IsMax = false;
                        }
                    }
                }
            }
            return UnloadedTimeReportTOList;
        }

            #endregion

            #region Insertion
            public int InsertTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.InsertTblPurchaseWeighingStageSummary(tblPurchaseWeighingStageSummaryTO);
        }

        public int InsertTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.InsertTblPurchaseWeighingStageSummary(tblPurchaseWeighingStageSummaryTO, conn, tran);
        }


        // public ResultMessage InsertWeighingDetails(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        // {
        //     SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //     SqlTransaction tran = null;
        //     Int32 result = 0;
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     resultMessage.MessageType = ResultMessageE.None;
        //     DateTime createdDate = _iCommonDAO.ServerDateTime;
        //     try
        //     {
        //         conn.Open();
        //         tran = conn.BeginTransaction();

        //         //check if tare weight is taken 
        //         if (tblPurchaseWeighingStageSummaryTO.WeightMeasurTypeId == Convert.ToInt32(Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT))
        //         {
        //             List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = GetVehicleWeightDetails(tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId, Convert.ToInt32(Constants.TransMeasureTypeE.TARE_WEIGHT).ToString(), conn, tran);
        //             if (tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
        //             {
        //                 tran.Rollback();
        //                 resultMessage.Result = 0;
        //                 resultMessage.MessageType = ResultMessageE.Error;
        //                 resultMessage.Text = "Already Tare Weight Is Taken";
        //                 return resultMessage;
        //             }

        //         }
        //         //If gross weight is taken then update vehicle status as Unloading Is In Process

        //         TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTO(tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId, true, conn, tran);
        //         if (tblPurchaseScheduleSummaryTO != null)
        //         {
        //             tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId = tblPurchaseScheduleSummaryTO.RootScheduleId;
        //         }
        //         if (tblPurchaseWeighingStageSummaryTO.WeightMeasurTypeId == Convert.ToInt32(Constants.TransMeasureTypeE.GROSS_WEIGHT))
        //         {
        //             if (tblPurchaseScheduleSummaryTO != null)
        //             {
        //                 tblPurchaseScheduleSummaryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.UNLOADING_IS_IN_PROCESS);
        //                 tblPurchaseScheduleSummaryTO.UpdatedBy = tblPurchaseWeighingStageSummaryTO.CreatedBy;
        //                 tblPurchaseScheduleSummaryTO.UpdatedOn = createdDate;
        //                 result = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO, conn, tran);
        //                 if (result <= 0)
        //                 {
        //                     tran.Rollback();
        //                     resultMessage.MessageType = ResultMessageE.Error;
        //                     resultMessage.Text = "Error While Updating Schedule Status";
        //                     return resultMessage;
        //                 }
        //             }
        //         }

        //         result = InsertTblPurchaseWeighingStageSummary(tblPurchaseWeighingStageSummaryTO, conn, tran);

        //         if (result > 0 && tblPurchaseWeighingStageSummaryTO.WeightMeasurTypeId == Convert.ToInt32(Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT))
        //         {
        //             resultMessage = _iTblPurchaseVehicleStageCntBL.InsertOrUpdateVehicleWtStageCount(tblPurchaseScheduleSummaryTO, tblPurchaseWeighingStageSummaryTO, null, null, conn, tran);
        //             result = resultMessage.Result;
        //         }
        //         if (result <= 0)
        //         {
        //             tran.Rollback();
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "Error While Saving Weighing Details";
        //             return resultMessage;
        //         }

        //         if (tblPurchaseWeighingStageSummaryTO.IsUpdateIsWeigingFlag)
        //         {
        //             tblPurchaseScheduleSummaryTO.IsWeighing = 1;
        //             result = _iTblPurchaseScheduleSummaryBL.UpdateWeighingCompletedAgainstVehicle(tblPurchaseScheduleSummaryTO, tblPurchaseWeighingStageSummaryTO.CreatedBy, conn, tran);
        //             if (result <= 0)
        //             {
        //                 tran.Rollback();
        //                 resultMessage.MessageType = ResultMessageE.Error;
        //                 resultMessage.Text = "Error While Saving Weighing Details";
        //                 return resultMessage;
        //             }
        //         }

        //         TblPurchaseEnquiryTO enquiryTO = _iTblPurchaseEnquiryBL.SelectTblPurchaseEnquiryTO(tblPurchaseScheduleSummaryTO.PurchaseEnquiryId);
        //         if (enquiryTO == null)
        //         {
        //             tran.Rollback();
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "Error While Saving Weighing Details";
        //             return resultMessage;
        //         }
        //         int result1 = 0;
        //         bool skipCheck = false;
        //         List<TblPurchaseScheduleStatusHistoryTO> HistoryTOListOld = new List<TblPurchaseScheduleStatusHistoryTO>();

        //         HistoryTOListOld = _iTblPurchaseScheduleStatusHistoryBL.SelectTblPurchaseScheduleStatusHistoryTO(tblPurchaseScheduleSummaryTO.RootScheduleId > 0 ? tblPurchaseScheduleSummaryTO.RootScheduleId : tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary, true, true, (int)Constants.TranStatusE.UNLOADING_IS_IN_PROCESS, conn, tran);
        //         if (HistoryTOListOld != null && HistoryTOListOld.Count > 0)
        //         {
        //             foreach (var item in HistoryTOListOld)
        //             {
        //                 if (item.IsApproved == 1)
        //                 {
        //                     skipCheck = true;
        //                 }
        //             }
        //         }
        //         if (skipCheck == false)
        //         {
        //             resultMessage = _iTblPurchaseScheduleSummarycircularBL.checkIfQtyGoesOutofBand(tblPurchaseScheduleSummaryTO, enquiryTO, conn, tran);
        //             if (resultMessage.Result == 0)
        //             {
        //                 tblPurchaseWeighingStageSummaryTO.IsValid = 1;
        //                 result1 = _iTblPurchaseWeighingStageSummaryDAO.updateIsValidFlagToInvalid(tblPurchaseWeighingStageSummaryTO, conn, tran);
        //                 if (result1 == 0)
        //                 {
        //                     tran.Rollback();
        //                     resultMessage.MessageType = ResultMessageE.Error;
        //                     resultMessage.Text = "Error While Saving Weighing Details";
        //                     return resultMessage;
        //                 }
        //             }
        //         }

        //         if (resultMessage.Result == 0 && tblPurchaseWeighingStageSummaryTO.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT)
        //         {
        //             TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
        //             List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
        //             Int32 conversionFact = 1000;
        //             //get purchase manager of supplier
        //             tblAlertUsersTOList = new List<TblAlertUsersTO>();
        //             string sourceEntityId = null;

        //             if (sourceEntityId == null)
        //             {
        //                 if (tblPurchaseScheduleSummaryTO.RootScheduleId > 0)
        //                 {
        //                     sourceEntityId = tblPurchaseScheduleSummaryTO.RootScheduleId.ToString();
        //                 }
        //                 else
        //                 {
        //                     sourceEntityId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary.ToString();
        //                 }
        //             }
        //             List<TblAlertUsersTO> AlertUsersTOList = new List<TblAlertUsersTO>();
        //             _iTblQualityPhaseBL.ResetAllPreviousNotification((int)NotificationConstants.NotificationsE.WEIGHING_STAGE_COMPLETED, sourceEntityId);

        //             if (enquiryTO != null)
        //             {
        //                 TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
        //                 tblAlertUsersTO.UserId = enquiryTO.UserId;
        //                 tblAlertUsersTO.RaisedOn = createdDate;
        //                 tblAlertUsersTOList.Add(tblAlertUsersTO);
        //             }

        //             tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_SCHEDULE_PENDING_FOR_APPROVAL;
        //             tblAlertInstanceTO.AlertAction = "Vehicle Schedule Pending For Approval";
        //             tblAlertInstanceTO.AlertComment = "Vehicle no: " + tblPurchaseScheduleSummaryTO.VehicleNo + " of booking No :" + enquiryTO.EnqDisplayNo + " is pending for  approval On weighing as weighing qty is greater than total sauda qty";
        //             tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
        //             tblAlertInstanceTO.EffectiveFromDate = createdDate;
        //             tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
        //             tblAlertInstanceTO.IsActive = 1;
        //             tblAlertInstanceTO.SourceDisplayId = "VEHICLE_SCHEDULE_PENDING_FOR_APPROVAL";
        //             if (tblPurchaseScheduleSummaryTO.RootScheduleId > 0)
        //             {
        //                 tblAlertInstanceTO.SourceEntityId = tblPurchaseScheduleSummaryTO.RootScheduleId;
        //             }
        //             else
        //             {
        //                 tblAlertInstanceTO.SourceEntityId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
        //             }
        //             tblAlertInstanceTO.RaisedBy = tblPurchaseWeighingStageSummaryTO.CreatedBy;
        //             tblAlertInstanceTO.RaisedOn = createdDate;
        //             tblAlertInstanceTO.IsAutoReset = 1;

        //             //Reset Prev alert of Vehicle Send In
        //             AlertsToReset alertsToReset = new AlertsToReset();
        //             alertsToReset.ResetAlertInstanceTOList = new List<ResetAlertInstanceTO>();
        //             ResetAlertInstanceTO resetAlertInstanceTO = new ResetAlertInstanceTO();
        //             resetAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.WEIGHING_STAGE_COMPLETED;
        //             resetAlertInstanceTO.SourceEntityTxnId = tblPurchaseScheduleSummaryTO.RootScheduleId;
        //             alertsToReset.ResetAlertInstanceTOList.Add(resetAlertInstanceTO);
        //             tblAlertInstanceTO.AlertsToReset = alertsToReset;


        //             notification.SendNotificationToUsers(tblAlertInstanceTO);

        //         }

        //         if (resultMessage.Result == 1 && tblPurchaseWeighingStageSummaryTO.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT)
        //         {
        //             TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
        //             List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
        //             Int32 conversionFact = 1000;
        //             //get purchase manager of supplier
        //             tblAlertUsersTOList = new List<TblAlertUsersTO>();
        //             TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO1 = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId, true, conn, tran);
        //             string sourceEntityId = null;

        //             if (sourceEntityId == null)
        //             {
        //                 if (tblPurchaseScheduleSummaryTO.RootScheduleId > 0)
        //                 {
        //                     sourceEntityId = tblPurchaseScheduleSummaryTO.RootScheduleId.ToString();
        //                 }
        //                 else
        //                 {
        //                     sourceEntityId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary.ToString();
        //                 }
        //             }
        //             List<TblAlertUsersTO> AlertUsersTOList = new List<TblAlertUsersTO>();
        //             _iTblQualityPhaseBL.ResetAllPreviousNotification((int)NotificationConstants.NotificationsE.WEIGHING_STAGE_COMPLETED, sourceEntityId);

        //             if (tblPurchaseScheduleSummaryTO1 != null)
        //             {
        //                 TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
        //                 tblAlertUsersTO.UserId = tblPurchaseScheduleSummaryTO1.SupervisorId;
        //                 tblAlertUsersTO.RaisedOn = createdDate;
        //                 tblAlertUsersTOList.Add(tblAlertUsersTO);
        //             }

        //             tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.WEIGHING_STAGE_COMPLETED;
        //             tblAlertInstanceTO.AlertAction = "WEIGHING_STAGE_COMPLETED";
        //             // tblAlertInstanceTO.AlertComment = "Weighing Stage " + tblPurchaseWeighingStageSummaryTO.WeightStageId  + " Completed For Scrap Vehicle No: " + tblPurchaseScheduleSummaryTO.VehicleNo + " With Net Weight- " + tblPurchaseWeighingStageSummaryTO.NetWeightMT;
        //             tblAlertInstanceTO.AlertComment = "Weighing Stage " + tblPurchaseWeighingStageSummaryTO.WeightStageId + " completed for Vehicle No:" + tblPurchaseWeighingStageSummaryTO.VehicleNo + " with nt. wt-" + tblPurchaseWeighingStageSummaryTO.NetWeightMT / conversionFact + "(MT)";
        //             tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
        //             tblAlertInstanceTO.EffectiveFromDate = createdDate;
        //             tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
        //             tblAlertInstanceTO.IsActive = 1;
        //             tblAlertInstanceTO.SourceDisplayId = "WEIGHING_STAGE_COMPLETED";
        //             if (tblPurchaseScheduleSummaryTO.RootScheduleId > 0)
        //             {
        //                 tblAlertInstanceTO.SourceEntityId = tblPurchaseScheduleSummaryTO1.RootScheduleId;
        //             }
        //             else
        //             {
        //                 tblAlertInstanceTO.SourceEntityId = tblPurchaseScheduleSummaryTO1.IdPurchaseScheduleSummary;
        //             }
        //             tblAlertInstanceTO.RaisedBy = tblPurchaseWeighingStageSummaryTO.CreatedBy;
        //             tblAlertInstanceTO.RaisedOn = createdDate;
        //             tblAlertInstanceTO.IsAutoReset = 1;

        //             //Reset Prev alert of Vehicle Send In
        //             AlertsToReset alertsToReset = new AlertsToReset();
        //             alertsToReset.ResetAlertInstanceTOList = new List<ResetAlertInstanceTO>();
        //             ResetAlertInstanceTO resetAlertInstanceTO = new ResetAlertInstanceTO();
        //             resetAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.SEND_IN;
        //             resetAlertInstanceTO.SourceEntityTxnId = tblPurchaseScheduleSummaryTO.ParentPurchaseScheduleSummaryId;
        //             alertsToReset.ResetAlertInstanceTOList.Add(resetAlertInstanceTO);
        //             tblAlertInstanceTO.AlertsToReset = alertsToReset;


        //             notification.SendNotificationToUsers(tblAlertInstanceTO);

        //         }

        //         if (tblPurchaseWeighingStageSummaryTO.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT)
        //         {
        //             TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
        //             List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
        //             Int32 conversionFact = 1000;
        //             //get supervisor
        //             tblAlertUsersTOList = new List<TblAlertUsersTO>();
        //             TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO1 = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId, true, conn, tran);
        //             string sourceEntityId = null;
        //             if (sourceEntityId == null)
        //             {
        //                 if (tblPurchaseScheduleSummaryTO.RootScheduleId > 0)
        //                 {
        //                     sourceEntityId = tblPurchaseScheduleSummaryTO1.RootScheduleId.ToString();
        //                 }
        //                 else
        //                 {
        //                     sourceEntityId = tblPurchaseScheduleSummaryTO1.IdPurchaseScheduleSummary.ToString();
        //                 }
        //             }
        //             List<TblAlertUsersTO> AlertUsersTOList = new List<TblAlertUsersTO>();
        //             _iTblQualityPhaseBL.ResetAllPreviousNotification((int)NotificationConstants.NotificationsE.SEND_IN, sourceEntityId);

        //             if (tblPurchaseScheduleSummaryTO1 != null)
        //             {
        //                 TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
        //                 tblAlertUsersTO.UserId = tblPurchaseScheduleSummaryTO1.SupervisorId;
        //                 tblAlertUsersTO.RaisedOn = createdDate;
        //                 tblAlertUsersTOList.Add(tblAlertUsersTO);
        //             }

        //             tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.WEIGHING_STAGE_COMPLETED;
        //             tblAlertInstanceTO.AlertAction = "WEIGHING_STAGE_COMPLETED";
        //             // tblAlertInstanceTO.AlertComment = "Weighing Stage " + tblPurchaseWeighingStageSummaryTO.WeightStageId  + " Completed For Scrap Vehicle No: " + tblPurchaseScheduleSummaryTO.VehicleNo + " With Net Weight- " + tblPurchaseWeighingStageSummaryTO.NetWeightMT;
        //             tblAlertInstanceTO.AlertComment = "Gross weight is taken for vehicle no. " + tblPurchaseWeighingStageSummaryTO.VehicleNo;
        //             tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
        //             tblAlertInstanceTO.EffectiveFromDate = createdDate;
        //             tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
        //             tblAlertInstanceTO.IsActive = 1;
        //             tblAlertInstanceTO.SourceDisplayId = "WEIGHING_STAGE_COMPLETED";
        //             if (tblPurchaseScheduleSummaryTO.RootScheduleId > 0)
        //             {
        //                 tblAlertInstanceTO.SourceEntityId = tblPurchaseScheduleSummaryTO1.RootScheduleId;
        //             }
        //             else
        //             {
        //                 tblAlertInstanceTO.SourceEntityId = tblPurchaseScheduleSummaryTO1.IdPurchaseScheduleSummary;
        //             }
        //             tblAlertInstanceTO.RaisedBy = tblPurchaseWeighingStageSummaryTO.CreatedBy;
        //             tblAlertInstanceTO.RaisedOn = createdDate;
        //             tblAlertInstanceTO.IsAutoReset = 1;


        //             notification.SendNotificationToUsers(tblAlertInstanceTO);

        //         }


        //         tran.Commit();
        //         if (resultMessage.Result == 1)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Information;
        //             resultMessage.Text = "Record Saved Successfully.";
        //             resultMessage.DisplayMessage = "Record Saved Successfully.";

        //         }
        //         resultMessage.Result = 1;
        //         // resultMessage.DefaultSuccessBehaviour();
        //         return resultMessage;

        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "InsertWeighingDetails At BL");
        //         return resultMessage;
        //     }
        //     finally
        //     {
        //         conn.Close();
        //     }


        // }

        public ResultMessage UpdateRecoveryEngineerId(int loginUserId, int purchaseScheduleSummaryId)
        {
            ResultMessage resmsg = new ResultMessage();
            int res = _iTblPurchaseWeighingStageSummaryDAO.UpdateRecoveryEngineerId(loginUserId, purchaseScheduleSummaryId);
            if (res > 0)
            {
                resmsg.Result = 1;
                resmsg.Text = "Saved Successfully";
                resmsg.DisplayMessage = "Saved Successfully";
                resmsg.MessageType = ResultMessageE.Information;
            }
            return resmsg;
        }
        public ResultMessage UpdateGraderId(int loginUserId, int purchaseScheduleSummaryId)
        {
            ResultMessage resmsg = new ResultMessage();
            int res = _iTblPurchaseWeighingStageSummaryDAO.UpdateGraderId(loginUserId, purchaseScheduleSummaryId);
            if (res > 0)
            {
                resmsg.Result = 1;
                resmsg.Text = "Saved Successfully";
                resmsg.DisplayMessage = "Saved Successfully";
                resmsg.MessageType = ResultMessageE.Information;
            }
            return resmsg;
        }

        public ResultMessage PostUpdatePhotographerId(int loginUserId, int purchaseScheduleSummaryId)
        {
            ResultMessage resmsg = new ResultMessage();
            int res = _iTblPurchaseWeighingStageSummaryDAO.PostUpdatePhotographerId(loginUserId, purchaseScheduleSummaryId);
            if (res > 0)
            {
                resmsg.Result = 1;
                resmsg.Text = "Saved Successfully";
                resmsg.DisplayMessage = "Saved Successfully";
                resmsg.MessageType = ResultMessageE.Information;
            }
            return resmsg;
        }

    

        #endregion

        #region Updation
        public int UpdateTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.UpdateTblPurchaseWeighingStageSummary(tblPurchaseWeighingStageSummaryTO);
        }

        public int UpdateTblPurchaseWeighingStageSummaryForIsValid(int rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.UpdateTblPurchaseWeighingStageSummaryForIsValid(rootScheduleId, conn, tran);
        }

        public int UpdateTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.UpdateTblPurchaseWeighingStageSummary(tblPurchaseWeighingStageSummaryTO, conn, tran);
        }

        public int UpdateUnlodingEndTime(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.UpdateUnlodingEndTime(tblPurchaseWeighingStageSummaryTO, conn, tran);
        }
        public ResultMessage UpdateTblPurchaseWeighingStageSummaryRecoveryDtls(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime createdDate = _iCommonDAO.ServerDateTime;


            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Prajakta[2019-03-29] Added to check if recovery is already confimred
                TblPurchaseWeighingStageSummaryTO weighingTO = SelectTblPurchaseWeighingStageSummaryTO(tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage, conn, tran);
                if (weighingTO == null)
                {
                    throw new Exception("weighingTO == NULL");
                }

                if (weighingTO.IsRecConfirm == 1)
                {
                    resultMessage.DefaultBehaviour();
                    // resultMessage.MessageType=ResultMessageE.Information;
                    resultMessage.DisplayMessage = "Recovery " + weighingTO.RecoveryPer + " % is already confirmed for weighing stage- " + weighingTO.WeightStageId + ".Please refresh and try again.";
                    return resultMessage;
                }

                tblPurchaseWeighingStageSummaryTO.RecoveryOn = createdDate;
                result = _iTblPurchaseWeighingStageSummaryDAO.UpdateTblPurchaseWeighingStageSummaryRecoveryDtls(tblPurchaseWeighingStageSummaryTO, conn, tran);
                if (result <= 0)
                {
                    throw new Exception("Error while updating weighing recovery details");
                }
                if (result > 0)
                {
                    TblPurchaseScheduleSummaryTO ScheduleSummaryTO = new TblPurchaseScheduleSummaryTO();
                    ScheduleSummaryTO.RootScheduleId = tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId;
                    resultMessage = _iTblPurchaseVehicleStageCntBL.InsertOrUpdateVehicleWtStageCount(ScheduleSummaryTO, null, tblPurchaseWeighingStageSummaryTO, null, null, conn, tran);
                    result = resultMessage.Result;
                }

                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTO(tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId, false, conn, tran);
                if (tblPurchaseScheduleSummaryTO == null)
                {
                    throw new Exception("tblPurchaseScheduleSummaryTO = NULL");

                }


                //Prajakta[2019-02-13] Commented and can be used for auto inserting the Recovery record
                // TblPurchaseScheduleSummaryTO gradingScheduleTO = null;
                // Boolean tempIsItemChange = true;
                // Boolean tempIsSendNotification = false;
                // resultMessage = _iTblPurchaseScheduleSummaryBL.SaveScheduleRecoveryDtls(tblPurchaseScheduleSummaryTO, gradingScheduleTO, tblPurchaseWeighingStageSummaryTO, tempIsItemChange, tempIsSendNotification, createdDate, conn, tran);
                // if (resultMessage.MessageType != ResultMessageE.Information)
                // {
                //     throw new Exception("Error in SaveScheduleRecoveryDtls()");
                // }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (System.Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblPurchaseWeighingStageSummaryRecoveryDtls(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)");
                return resultMessage;

            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.DeleteTblPurchaseWeighingStageSummary(idPurchaseWeighingStage);
        }

        public int DeleteTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.DeleteTblPurchaseWeighingStageSummary(idPurchaseWeighingStage, conn, tran);
        }

        public int DeleteAllWeighingStageAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.DeleteAllWeighingStageAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }

        public int UpdateUnlodingStartTime(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.UpdateUnlodingStartTime(tblPurchaseWeighingStageSummaryTO);
        }

        #endregion

    }
}
