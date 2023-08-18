using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class LoadingBL : ILoadingBL
    {
        #region Declaration & Constructor
        private readonly ILoadingDAO _iLoadingDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommon;
        private readonly ITblTransferRequestDAO _iTblTransferRequestDAO;
        private readonly Idimensionbl _iDimensionBL;
        private readonly ITblEntityRangeBL _iTblEntityRangeBL;
        private readonly ITblVehicleDAO _iTblVehicleDAO;
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IRunReport _iRunReport;
        private readonly ITblAddonsFunDtlsDAO _iTblAddonsFunDtlsDAO;
        private readonly ITblModuleDAO _iTblModuleDAO;
        private static readonly object loadingLock = new object();
        private static readonly object vehicleLock = new object();
        private static readonly object updateVehicleLock = new object();
        private static readonly object weighingLock = new object();
        private static readonly object slaLock = new object(); 
        public LoadingBL(ITblModuleDAO iTblModuleDAO, ITblAddonsFunDtlsDAO iTblAddonsFunDtlsDAO, IRunReport iRunReport, ITblConfigParamsDAO iTblConfigParamsDAO, IDimReportTemplateBL iDimReportTemplateBL, ITblEntityRangeBL iTblEntityRangeBL, Idimensionbl iDimensionBL, ITblTransferRequestDAO iTblTransferRequestDAO, ITblVehicleDAO iTblVehicleDAO, IConnectionString iConnectionString, ILoadingDAO iLoadingDAO, Icommondao iCommon)
        {
            _iLoadingDAO = iLoadingDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
            _iTblVehicleDAO = iTblVehicleDAO;
            _iTblTransferRequestDAO = iTblTransferRequestDAO;
            _iDimensionBL = iDimensionBL;
            _iTblEntityRangeBL = iTblEntityRangeBL;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iRunReport = iRunReport;
            _iTblAddonsFunDtlsDAO = iTblAddonsFunDtlsDAO;
            _iTblModuleDAO = iTblModuleDAO;
        }
        #endregion
        #region GET
        public TblTRLoadingTO GetLoadingDetails(Int32 idLoading)
        {
            return _iLoadingDAO.GetLoadingDetailsTO(idLoading);
        }
        public TblUnloadingSLATO GetUnloadingSLADetailsTO(Int32 idSLA)
        {
            return _iLoadingDAO.GetUnloadingSLADetailsTO(idSLA);
        }

        public List<TblMaterialTypeReport> GetMaterialTypeReport()
        {
            return _iLoadingDAO.GetMaterialTypeReport();
        }

        public List<TblTRLoadingTO> GetLoadingDetailsList(LoadingFilterTO loadingFilterTO)
        {
            List<TblTRLoadingTO> TblTRLoadingTOList = _iLoadingDAO.GetLoadingDetailsTOList(loadingFilterTO);
            #region Get Addon Details List
            if (TblTRLoadingTOList != null && TblTRLoadingTOList.Count > 0 && (loadingFilterTO.ScreenName == Constants.UNLOADING_SUMMARY_SCREEN || loadingFilterTO.ScreenName == Constants.LOADING_SLIP_SUMMARY))
            {
                String IdLoadings = string.Join(",", TblTRLoadingTOList.Select(d => d.IdLoading.ToString()).ToArray());
                List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList = _iTblAddonsFunDtlsDAO.SelectAddonDetailsList(IdLoadings, (Int32)Constants.DefaultModuleID, Constants.CommonNoteTrasactionType, Constants.CommonNoteLoadingPageElementId);
                if (tblAddonsFunDtlsTOList != null && tblAddonsFunDtlsTOList.Count > 0)
                {
                    TblTRLoadingTOList.ForEach(element =>
                    {
                        var matchTO = tblAddonsFunDtlsTOList.Where(w => w.TransId == element.IdLoading).FirstOrDefault();
                        if (matchTO != null)
                        {
                            element.AddOnCnt = matchTO.Count;
                        }
                    });
                }
                if (loadingFilterTO.ScreenName == Constants.UNLOADING_SUMMARY_SCREEN)
                {
                    IdLoadings = "";
                    tblAddonsFunDtlsTOList = null;
                    IdLoadings = string.Join(",", TblTRLoadingTOList.Select(d => d.IdLoading.ToString()).ToArray());
                    tblAddonsFunDtlsTOList = _iTblAddonsFunDtlsDAO.SelectAddonDetailsList(IdLoadings, (Int32)Constants.DefaultModuleID, Constants.CommonNoteTrasactionType, Constants.CommonNotePageElementId);
                    if (tblAddonsFunDtlsTOList != null && tblAddonsFunDtlsTOList.Count > 0)
                    {
                        TblTRLoadingTOList.ForEach(element =>
                        {
                            var matchTO = tblAddonsFunDtlsTOList.Where(w => w.TransId == element.IdLoading).FirstOrDefault();
                            if (matchTO != null)
                            {
                                element.AddOnUnCnt = matchTO.Count;
                            }
                        });
                    }
                }
            }
            #endregion
            return TblTRLoadingTOList;
        }
        public List<TblUnloadingSLATO> GetUnloadingSLADetailsList(UnloadingSLAFilterTO unloadingSLAFilterTO)
        {
            return _iLoadingDAO.GetUnloadingSLADetailsList(unloadingSLAFilterTO);
        }
        public List<TblTRLoadingTO> GetVehicleWiseLoadingDetailsTOList(LoadingFilterTO loadingFilterTO)
        {
            return _iLoadingDAO.GetVehicleWiseLoadingDetailsTOList(loadingFilterTO);
        }
        public List<TblTRLoadingWeighingTO> GetWeighingDetails(Int32 LoadingId, Int32 LoadingTypeId)
        {
            return _iLoadingDAO.GetWeighingDetails(LoadingId, LoadingTypeId);
        }
        public List<TblTRLoadingWeighingTO> GetWeighingDetails(String LoadingIdStr, String LoadingTypeIdStr)
        {
            return _iLoadingDAO.GetWeighingDetails(LoadingIdStr, LoadingTypeIdStr);
        }
        public List<TblTRLoadingTO> GetVehicleLoadingHistory(Int32 IdVehicle)
        {
            return _iLoadingDAO.GetVehicleLoadingHistory(IdVehicle);
        }
        #endregion
        #region POST
        public ResultMessage AddLoading(TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Loading Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();

                lock (loadingLock)
                {
                    #region Set Basic Values
                    int result = 0;
                    DateTime ServerDateTime = _iCommon.ServerDateTime;
                    tblTRLoadingTO.CreatedOn = ServerDateTime;
                    tblTRLoadingTO.StatusBy = tblTRLoadingTO.CreatedBy;
                    tblTRLoadingTO.StatusOn = ServerDateTime;
                    tblTRLoadingTO.IsActive = 1;
                    TblTransferRequestTO tblTransferRequestTO = new TblTransferRequestTO();
                    #endregion

                    #region Validate Vehicle Available or Not & Update
                    TblVehicleTO tblVehicleTO = _iTblVehicleDAO.SelectTblVehicle(tblTRLoadingTO.VehicleId, conn, tran);
                    if (tblVehicleTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Vehicle Details Not Found");
                        return resultMessage;
                    }
                    if (tblVehicleTO.VehicleStatusId != (Int32)Constants.InternalTransferRequestVehicalStatusE.NEW)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Vehicle Not Available For Transaction. " + tblVehicleTO.VehicleNo + "(" + tblVehicleTO.VehicalStatusName + ")");
                        return resultMessage;
                    }
                    if(tblVehicleTO.VehicleStatusId == (Int32)Constants.InternalTransferRequestVehicalStatusE.NEW)
                    {
                        tblVehicleTO.VehicleStatusId = (Int32)Constants.InternalTransferRequestVehicalStatusE.IN_PROCESS;
                        tblVehicleTO.UpdatedBy = tblTRLoadingTO.CreatedBy;
                        tblVehicleTO.UpdatedOn = ServerDateTime;
                        result = _iTblVehicleDAO.UpdateVehicalStatus(tblVehicleTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Update Vehicle Status");
                            return resultMessage;
                        }
                    }
                    tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.PENDING_FOR_GATE_IN;
                    if (tblVehicleTO.VehicleStatusId == (Int32)Constants.InternalTransferRequestVehicalStatusE.PENDING_FOR_APPROVAL)
                    {
                        tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.PENDING_FOR_HOD_APPROVAL;
                    }
                    #endregion

                    #region Validate Transfer Request Details and Prepare Data
                    if (tblTRLoadingTO.TransferRequestId > 0)
                    {
                        tblTransferRequestTO = _iTblTransferRequestDAO.SelectTblTransferRequest(tblTRLoadingTO.TransferRequestId, conn, tran);
                        if(tblTransferRequestTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Transfer Request Details Not Found.");
                            return resultMessage;
                        }
                        tblTransferRequestTO.Scheduleqty = tblTransferRequestTO.Scheduleqty + tblTRLoadingTO.ScheduleQty;
                        if (tblTransferRequestTO.Scheduleqty > tblTransferRequestTO.Qty)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Loading Qty Should Not Be Greater that Pending Request Qty. Please Refresh and try Again.");
                            return resultMessage;
                        }
                        tblTransferRequestTO.StatusId = (Int32)Constants.InternalTransferRequestStatusE.IN_PROCESS;
                        if (tblTransferRequestTO.Scheduleqty == tblTransferRequestTO.Qty)
                        {
                            tblTransferRequestTO.StatusId = (Int32)Constants.InternalTransferRequestStatusE.CLOSE;
                        }
                        tblTransferRequestTO.StatusChangedBy = tblTRLoadingTO.CreatedBy;
                        tblTransferRequestTO.StatusChangedOn = ServerDateTime;
                        tblTRLoadingTO.TransferRequestId = tblTransferRequestTO.IdTransferRequest;
                        result = _iTblTransferRequestDAO.UpdateTblTransferRequestScheduleQtyNStatus(tblTransferRequestTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Update Transfer Request");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        #region Prepare Transfer Request TO (Use to create auto generated request)
                        tblTransferRequestTO.CreatedBy = tblTRLoadingTO.CreatedBy;
                        tblTransferRequestTO.CreatedOn = ServerDateTime;
                        tblTransferRequestTO.FromLocationId = tblTRLoadingTO.FromLocationId;
                        tblTransferRequestTO.ToLocationId = tblTRLoadingTO.ToLocationId;
                        tblTransferRequestTO.Scheduleqty = tblTRLoadingTO.ScheduleQty;
                        tblTransferRequestTO.UnloadingPointId = tblTRLoadingTO.UnloadingPointId;
                        tblTransferRequestTO.StatusChangedBy = tblTRLoadingTO.CreatedBy;
                        tblTransferRequestTO.StatusChangedOn = ServerDateTime;
                        tblTransferRequestTO.Qty = tblTRLoadingTO.ScheduleQty;
                        tblTransferRequestTO.Narration = tblTRLoadingTO.Narration;
                        tblTransferRequestTO.MaterialTypeId = tblTRLoadingTO.MaterialTypeId;
                        tblTransferRequestTO.MaterialSubTypeId = tblTRLoadingTO.MaterialSubTypeId;
                        tblTransferRequestTO.IsAutoCreated = 1;
                        tblTransferRequestTO.IsActive = 1;
                        tblTransferRequestTO.StatusId = (Int32)Constants.InternalTransferRequestStatusE.CLOSE;
          
                        #region Set Entity Rage For Transfer Request
                        DimFinYearTO FinYearTO = _iDimensionBL.GetCurrentFinancialYear(ServerDateTime, conn, tran);
                        if (FinYearTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Current Fin Year Object Not Found");
                            return resultMessage;
                        }
                        TblEntityRangeTO entityRangeTODtls = _iTblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(Constants.INTERNAL_TRANSFER_REQUEST, FinYearTO.IdFinYear, conn, tran);
                        if (entityRangeTODtls == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("entity range not found in Function AddVoucher");
                            return resultMessage;
                        }
                        entityRangeTODtls.EntityPrevValue = entityRangeTODtls.EntityPrevValue + 1;
                        result = _iTblEntityRangeBL.UpdateTblEntityRange(entityRangeTODtls, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to update record - UpdateTblEntityRange");
                            return resultMessage;
                        }
                        tblTransferRequestTO.RequestDisplayNo = entityRangeTODtls.Prefix + entityRangeTODtls.EntityPrevValue + entityRangeTODtls.Suffix;
                        #endregion
                        result = _iTblTransferRequestDAO.InsertTblTransferRequest(tblTransferRequestTO, conn, tran);
                        if(result == 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Add Request - InsertTblTransferRequest");
                            return resultMessage;
                        }
                        tblTRLoadingTO.TransferRequestId = tblTransferRequestTO.IdTransferRequest;
                        #endregion
                    }
                    #endregion

                    #region Set Entity Rage
                    DimFinYearTO curFinYearTO = _iDimensionBL.GetCurrentFinancialYear(tblTRLoadingTO.CreatedOn, conn, tran);
                    if (curFinYearTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Current Fin Year Object Not Found");
                        return resultMessage;
                    }
                    TblEntityRangeTO entityRangeTO = _iTblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(Constants.LOADING_DISPLAY_NO, curFinYearTO.IdFinYear, conn, tran);
                    if (entityRangeTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("entity range not found in Function AddLoading");
                        return resultMessage;
                    }
                    entityRangeTO.EntityPrevValue = entityRangeTO.EntityPrevValue + 1;
                    result = _iTblEntityRangeBL.UpdateTblEntityRange(entityRangeTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to update record - UpdateTblEntityRange");
                        return resultMessage;
                    }
                    tblTRLoadingTO.LoadingSlipNo = entityRangeTO.Prefix + entityRangeTO.EntityPrevValue + entityRangeTO.Suffix;
                    #endregion

                    #region Add Loading
                    result = _iLoadingDAO.InsertLoading(tblTRLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertLoading");
                        return resultMessage;
                    }
                    #endregion

                    #region Add Loading History
                    result = _iLoadingDAO.InsertLoadingHistory(tblTRLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertLoadingHistory");
                        return resultMessage;
                    }
                    #endregion
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tblTRLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "AddLoading");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public ResultMessage AddSLA(TblUnloadingSLATO tblUnloadingSLATO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblUnloadingSLATO == null)
                {
                    resultMessage.DefaultBehaviour("SLA Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();
                lock (slaLock)
                {
                    TblUnloadingSLATO tempTblUnloadingSLATO = _iLoadingDAO.GetUnloadingSLADetailsTOByLoadingId(tblUnloadingSLATO.LoadingId);
                    if (tempTblUnloadingSLATO != null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("SLA Document Already Added");
                        resultMessage.Result = 2;
                        return resultMessage;
                    }

                    #region Set Basic Values
                    int result = 0;
                    DateTime ServerDateTime = _iCommon.ServerDateTime;
                    tblUnloadingSLATO.CreatedOn = ServerDateTime;
                    tblUnloadingSLATO.IsActive = 1;
                    #endregion

                    #region Set Entity Rage
                    DimFinYearTO curFinYearTO = _iDimensionBL.GetCurrentFinancialYear(tblUnloadingSLATO.CreatedOn, conn, tran);
                    if (curFinYearTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Current Fin Year Object Not Found");
                        return resultMessage;
                    }
                    TblEntityRangeTO entityRangeTO = _iTblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(Constants.SLA_DISPLAY_NO, curFinYearTO.IdFinYear, conn, tran);
                    if (entityRangeTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("entity range not found in Function AddSLA");
                        return resultMessage;
                    }
                    entityRangeTO.EntityPrevValue = entityRangeTO.EntityPrevValue + 1;
                    result = _iTblEntityRangeBL.UpdateTblEntityRange(entityRangeTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to update record - UpdateTblEntityRange");
                        return resultMessage;
                    }
                    tblUnloadingSLATO.DisplayNo = entityRangeTO.Prefix + entityRangeTO.EntityPrevValue + entityRangeTO.Suffix;
                    #endregion

                    #region Add SLA
                    result = _iLoadingDAO.InsertSLA(tblUnloadingSLATO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertSLA");
                        return resultMessage;
                    }
                    #endregion
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tblUnloadingSLATO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "AddSLA");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public ResultMessage UpdateLoadingStatus(TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Loading Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();

                lock (vehicleLock)
                {
                    #region Set Basic Values
                    int result = 0;
                    DateTime ServerDateTime = _iCommon.ServerDateTime;
                    tblTRLoadingTO.StatusOn = ServerDateTime;
                    #endregion

                    #region Validate Loading Vehicle Status
                    TblTRLoadingTO tempTblTRLoadingTO = _iLoadingDAO.GetLoadingDetailsTO(tblTRLoadingTO.IdLoading);
                    if (tempTblTRLoadingTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Loading Vehicle Details Not Found");
                        return resultMessage;
                    }
                    if(tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.LOADING_1ST_WEIGHING && tempTblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.PENDING_FOR_GATE_IN)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Vehicle Phase Already Changed. Please Refresh and check Again.");
                        return resultMessage;
                    }
                    else if (tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.UNLOADING_GATE_IN && tempTblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.LOADING_GATE_OUT)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Vehicle Phase Already Changed. Please Refresh and check Again.");
                        return resultMessage;
                    }
                    else if (tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.UNLOADING_1ST_WEIGHING && tempTblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.UNLOADING_GATE_IN)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Vehicle Phase Already Changed. Please Refresh and check Again.");
                        return resultMessage;
                    }
                    else if (tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.COMPLETED && tempTblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.UNLOAD_GATE_OUT)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Vehicle Phase Already Changed. Please Refresh and check Again.");
                        return resultMessage;
                    }
                    //else if (tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.PENDING_FOR_GATE_IN || tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.REJECTED)
                    //{
                    //    if(tempTblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.PENDING_FOR_HOD_APPROVAL)
                    //    {
                    //        tran.Rollback();
                    //        resultMessage.DefaultBehaviour("Vehicle Phase Already Changed. Please Refresh and check Again.");
                    //        return resultMessage;
                    //    }
                    //    #region If Loading Status Rejected then substract loading qty from request schedule quantity
                    //    if(tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.REJECTED)
                    //    {
                    //        TblTransferRequestTO tblTransferRequestTO = _iTblTransferRequestDAO.SelectTblTransferRequest(tblTRLoadingTO.TransferRequestId, conn, tran);
                    //        if (tblTransferRequestTO == null)
                    //        {
                    //            tran.Rollback();
                    //            resultMessage.DefaultBehaviour("Transfer Request Details Not Found.");
                    //            return resultMessage;
                    //        }
                    //        tblTransferRequestTO.Scheduleqty = tblTransferRequestTO.Scheduleqty - tempTblTRLoadingTO.ScheduleQty;
                    //        tblTransferRequestTO.UpdatedBy = tblTRLoadingTO.StatusBy;
                    //        tblTransferRequestTO.UpdatedOn = ServerDateTime;
                    //        tblTransferRequestTO.StatusChangedBy = tblTRLoadingTO.StatusBy;
                    //        tblTransferRequestTO.StatusChangedOn = ServerDateTime;
                    //        tblTransferRequestTO.StatusId = (Int32)Constants.InternalTransferRequestStatusE.IN_PROCESS;
                    //        if (tblTransferRequestTO.Scheduleqty == 0)
                    //        {
                    //            tblTransferRequestTO.StatusId = (Int32)Constants.InternalTransferRequestStatusE.NEW;
                    //        }
                    //        result = _iTblTransferRequestDAO.UpdateTblTransferRequest(tblTransferRequestTO, conn, tran);
                    //        if (result != 1)
                    //        {
                    //            tran.Rollback();
                    //            resultMessage.DefaultBehaviour("Failed to Update - UpdateLoadingStatus");
                    //            return resultMessage;
                    //        }
                    //    }
                    //    #endregion
                    //}
                    #endregion
                    #region Update Loading
                    result = _iLoadingDAO.UpdateLoadingStatus(tblTRLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Update - UpdateLoadingStatus");
                        return resultMessage;
                    }
                    #endregion
                    #region Update Vehicle
                    if(tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.COMPLETED)
                    {
                        TblVehicleTO tblVehicleTO = _iTblVehicleDAO.SelectTblVehicle(tblTRLoadingTO.VehicleId);
                        if(tblVehicleTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Vehicle Details Not Found - SelectTblVehicle");
                            return resultMessage;
                        }
                        tblVehicleTO.VehicleStatusId = (Int32)Constants.InternalTransferRequestVehicalStatusE.NEW;
                        tblVehicleTO.UpdatedBy = tblTRLoadingTO.StatusBy;
                        tblVehicleTO.UpdatedOn = ServerDateTime;
                        if (tblVehicleTO.VehicleTypeId == (Int32)Constants.VehicalTypeE.ONE_TIME)
                        {
                            tblVehicleTO.VehicleStatusId = (Int32)Constants.InternalTransferRequestVehicalStatusE.CLOSE;
                        }
                        result = _iTblVehicleDAO.UpdateVehicalStatus(tblVehicleTO, conn, tran);
                        if(result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Update Vehicle Status - UpdateVehicalStatus");
                            return resultMessage;
                        }
                    }
                    #endregion
                    #region Add Loading History
                    TblTRLoadingTO UpdatedTblTRLoadingTO = _iLoadingDAO.GetLoadingDetailsTO(tblTRLoadingTO.IdLoading, conn, tran);
                    if(UpdatedTblTRLoadingTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Get Loading Details - GetLoadingDetailsTO");
                        return resultMessage;
                    }
                    result = _iLoadingDAO.InsertLoadingHistory(UpdatedTblTRLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertLoadingHistory");
                        return resultMessage;
                    }
                    #endregion
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tblTRLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateLoadingStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public ResultMessage UpdateLoadingVehicle(TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage(); 
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Loading Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();

                lock (updateVehicleLock)
                {
                    #region Set Basic Values
                    int result = 0;
                    DateTime ServerDateTime = _iCommon.ServerDateTime;
                    tblTRLoadingTO.UpdatedOn = ServerDateTime;
                    #endregion

                    #region Validate Loading Details
                    TblTRLoadingTO tempTblTRLoadingTO = _iLoadingDAO.GetLoadingDetailsTO(tblTRLoadingTO.IdLoading);
                    if (tempTblTRLoadingTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Loading Vehicle Details Not Found");
                        return resultMessage;
                    }
                    if (tempTblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.COMPLETED)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Transaction Already Completed. Please Refresh and check Again.");
                        return resultMessage;
                    }
                    else if(tempTblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.REJECTED)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Transaction Already Rejected. Please Refresh and check Again.");
                        return resultMessage;
                    }
                    #endregion
                    #region Validate Vehicle Available or Not & Update
                    TblVehicleTO tblVehicleTO = _iTblVehicleDAO.SelectTblVehicle(tblTRLoadingTO.VehicleId, conn, tran);
                    if (tblVehicleTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Vehicle Details Not Found");
                        return resultMessage;
                    }
                    if (tblVehicleTO.VehicleStatusId != (Int32)Constants.InternalTransferRequestVehicalStatusE.NEW)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Vehicle Not Available For Transaction. " + tblVehicleTO.VehicleNo + "(" + tblVehicleTO.VehicalStatusName + ")");
                        return resultMessage;
                    }
                    if (tblVehicleTO.VehicleStatusId == (Int32)Constants.InternalTransferRequestVehicalStatusE.NEW)
                    {
                        tblVehicleTO.VehicleStatusId = (Int32)Constants.InternalTransferRequestVehicalStatusE.IN_PROCESS;
                        tblVehicleTO.UpdatedBy = tblTRLoadingTO.UpdatedBy;
                        tblVehicleTO.UpdatedOn = ServerDateTime;
                        result = _iTblVehicleDAO.UpdateVehicalStatus(tblVehicleTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Update Vehicle Status");
                            return resultMessage;
                        }
                    }
                    #region Update Old Vehicle
                    TblVehicleTO BreakdownVehicleTO = _iTblVehicleDAO.SelectTblVehicle(tempTblTRLoadingTO.IdVehicle, conn, tran);
                    if (BreakdownVehicleTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Breakdown Vehicle Details Not Found");
                        return resultMessage;
                    }
                    if(BreakdownVehicleTO.VehicleTypeId == (Int32)Constants.VehicalTypeE.ONE_TIME)
                    {
                        BreakdownVehicleTO.VehicleStatusId = (Int32)Constants.InternalTransferRequestVehicalStatusE.CLOSE;
                    }
                    else
                    {
                        BreakdownVehicleTO.VehicleStatusId = (Int32)Constants.InternalTransferRequestVehicalStatusE.BREAKDOWN;
                    }
                    BreakdownVehicleTO.UpdatedBy = tblTRLoadingTO.UpdatedBy;
                    BreakdownVehicleTO.UpdatedOn = ServerDateTime;
                    result = _iTblVehicleDAO.UpdateVehicalStatus(BreakdownVehicleTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Update Vehicle Status");
                        return resultMessage;
                    }
                    #endregion
                    #endregion
                    #region Update Loading
                    result = _iLoadingDAO.UpdateLoadingVehicle(tblTRLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Update - UpdateLoadingVehicle");
                        return resultMessage;
                    }
                    #endregion
                    #region Add Loading History
                    TblTRLoadingTO UpdatedTblTRLoadingTO = _iLoadingDAO.GetLoadingDetailsTO(tblTRLoadingTO.IdLoading, conn, tran);
                    if (UpdatedTblTRLoadingTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Get Loading Details - GetLoadingDetailsTO");
                        return resultMessage;
                    }
                    result = _iLoadingDAO.InsertLoadingHistory(UpdatedTblTRLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertLoadingHistory");
                        return resultMessage;
                    }
                    #endregion
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tblTRLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateLoadingStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public ResultMessage UpdateWeighingRemark(TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Loading Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();
                #region Update Loading
                int result = 0;
                result = _iLoadingDAO.UpdateWeighingRemark(tblTRLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Failed to Update - UpdateWeighingRemark");
                    return resultMessage;
                }
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tblTRLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateLoadingStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public ResultMessage CloseLoadingTrasaction(TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Loading Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();
                #region Set Basic Values
                int result = 0;
                DateTime ServerDateTime = _iCommon.ServerDateTime;
                #endregion

                #region Validate Loading Details
                TblTRLoadingTO tempTblTRLoadingTO = _iLoadingDAO.GetLoadingDetailsTO(tblTRLoadingTO.IdLoading);
                if (tempTblTRLoadingTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Loading Vehicle Details Not Found");
                    return resultMessage;
                }
                if (tempTblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.COMPLETED)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Transaction Already Completed. Please Refresh and check Again.");
                    return resultMessage;
                }
                else if (tempTblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.REJECTED)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Transaction Already Rejected. Please Refresh and check Again.");
                    return resultMessage;
                }
                tempTblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.REJECTED;
                tempTblTRLoadingTO.StatusBy = tblTRLoadingTO.UpdatedBy;
                tempTblTRLoadingTO.StatusOn = ServerDateTime;
                tempTblTRLoadingTO.TransactionCloseRemark = tblTRLoadingTO.TransactionCloseRemark;
                #endregion
                #region Update Request
                TblTransferRequestTO tblTransferRequestTO = _iTblTransferRequestDAO.SelectTblTransferRequest(tblTRLoadingTO.TransferRequestId, conn, tran);
                if (tblTransferRequestTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Transfer Request Details Not Found.");
                    return resultMessage;
                }
                tblTransferRequestTO.Scheduleqty = tblTransferRequestTO.Scheduleqty - tempTblTRLoadingTO.ScheduleQty;
                if (tblTransferRequestTO.Scheduleqty < 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Something Went Wrong Schedule Qty Should Not Be Negative - "+ tblTransferRequestTO.Scheduleqty);
                    return resultMessage;
                }
                if(tblTransferRequestTO.IsAutoCreated != 1)
                {
                    if(tblTransferRequestTO.Scheduleqty > 0)
                    {
                        tblTransferRequestTO.StatusId = (Int32)Constants.InternalTransferRequestStatusE.IN_PROCESS;
                    }
                    else
                    {
                        tblTransferRequestTO.StatusId = (Int32)Constants.InternalTransferRequestStatusE.NEW;
                    }
                }
                tblTransferRequestTO.StatusChangedBy = tblTRLoadingTO.UpdatedBy;
                tblTransferRequestTO.StatusChangedOn = ServerDateTime;
                result = _iTblTransferRequestDAO.UpdateTblTransferRequestScheduleQtyNStatus(tblTransferRequestTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Failed to Update Transfer Request");
                    return resultMessage;
                }
                #endregion
                #region Update Vehicle
                TblVehicleTO tblVehicleTO = _iTblVehicleDAO.SelectTblVehicle(tempTblTRLoadingTO.VehicleId, conn, tran);
                if (tblVehicleTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Vehicle Details Not Found");
                    return resultMessage;
                }
                tblVehicleTO.VehicleStatusId = (Int32)Constants.InternalTransferRequestVehicalStatusE.NEW;
                if (tblVehicleTO.VehicleTypeId == (Int32)Constants.VehicalTypeE.ONE_TIME)
                {
                    tblVehicleTO.VehicleStatusId = (Int32)Constants.InternalTransferRequestVehicalStatusE.CLOSE;
                }
                tblVehicleTO.UpdatedBy = tblTRLoadingTO.UpdatedBy;
                tblVehicleTO.UpdatedOn = ServerDateTime;
                result = _iTblVehicleDAO.UpdateVehicalStatus(tblVehicleTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Failed to Update Vehicle Status");
                    return resultMessage;
                }
                #endregion
                #region Update Loading
                result = _iLoadingDAO.UpdateLoadingStatus(tempTblTRLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Failed to Update - UpdateLoadingVehicle");
                    return resultMessage;
                }
                #endregion
                #region Add Loading History
                result = _iLoadingDAO.InsertLoadingHistory(tempTblTRLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Record Saved Failed - InsertLoadingHistory");
                    return resultMessage;
                }
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tempTblTRLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "CloseLoadingTrasaction");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public ResultMessage UpdateLoading(TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Loading Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();

                #region Set Basic Values
                int result = 0;
                DateTime ServerDateTime = _iCommon.ServerDateTime;
                #endregion

                #region Get Loading Details and Prepare Data
                TblTRLoadingTO tempTblTRLoadingTO = _iLoadingDAO.GetLoadingDetailsTO(tblTRLoadingTO.IdLoading);
                if (tempTblTRLoadingTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Loading Vehicle Details Not Found");
                    return resultMessage;
                }
                if (tblTRLoadingTO.ReviewUnloadingParam != 1 && tblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.PENDING_FOR_GATE_IN && tblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.LOADING_1ST_WEIGHING && tblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.UNDER_LOADING && tblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.LOADING_FINAL_WEIGHING && tblTRLoadingTO.StatusId != (Int32)Constants.LoadingStatusE.PENDING_FOR_HOD_APPROVAL)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Record Not Available to Update");
                    return resultMessage;
                }
                tempTblTRLoadingTO.UpdatedOn = ServerDateTime;
                tempTblTRLoadingTO.UpdatedBy = tblTRLoadingTO.UpdatedBy;
                if(tblTRLoadingTO.ReviewUnloadingParam == 1)
                {
                    if(tempTblTRLoadingTO.IsReviewUnloading == 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Document Already Reviewed, Please Refresh and Check.");
                        return resultMessage;
                    }
                    tempTblTRLoadingTO.UnMaterialTypeId = tblTRLoadingTO.UnMaterialTypeId;
                    tempTblTRLoadingTO.UnMaterialSubTypeId = tblTRLoadingTO.UnMaterialSubTypeId;
                    tempTblTRLoadingTO.UnloadingNarration = tblTRLoadingTO.UnloadingNarration;
                    tempTblTRLoadingTO.IsReviewUnloading = 1;
                }
                else
                {
                    tempTblTRLoadingTO.ToLocationId = tblTRLoadingTO.ToLocationId;
                    tempTblTRLoadingTO.UnloadingPointId = tblTRLoadingTO.UnloadingPointId;
                    tempTblTRLoadingTO.Narration = tblTRLoadingTO.Narration;
                }
                #endregion

                #region Update Loading
                result = _iLoadingDAO.UpdateLoading(tempTblTRLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Failed to Update - UpdateLoading");
                    return resultMessage;
                }
                #endregion
                #region Add Loading History
                result = _iLoadingDAO.InsertLoadingHistory(tempTblTRLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Record Saved Failed - InsertLoadingHistory");
                    return resultMessage;
                }
                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tblTRLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateLoading");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public TblTRLoadingWeighingTO CalculateWeighingValues(TblTRLoadingWeighingTO tblTRLoadingWeighingTO, List<TblTRLoadingWeighingTO> tblTRLoadingWeighingTOList)
        {
            if(tblTRLoadingWeighingTO.LoadingTypeId == (Int32)Constants.DocumentTypeE.LOADING)
            {
                if(tblTRLoadingWeighingTO.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.TARE_WEIGHT)
                {
                    tblTRLoadingWeighingTO.GrossWeight = tblTRLoadingWeighingTO.ActualWeight;
                    tblTRLoadingWeighingTO.NetWeight = 0;
                    tblTRLoadingWeighingTO.WeighingStageId = 1;
                }
                else if(tblTRLoadingWeighingTO.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.INTERMEDIATE_WEIGHT)
                {
                    var macthTO = tblTRLoadingWeighingTOList.OrderByDescending(o => o.WeighingStageId).FirstOrDefault();
                    if(macthTO != null){
                        tblTRLoadingWeighingTO.WeighingStageId = macthTO.WeighingStageId + 1;
                        tblTRLoadingWeighingTO.GrossWeight = macthTO.ActualWeight;
                        tblTRLoadingWeighingTO.NetWeight = tblTRLoadingWeighingTO.ActualWeight- macthTO.ActualWeight;
                    }
                }
                else if(tblTRLoadingWeighingTO.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.GROSS_WEIGHT)
                {
                    var macthTO = tblTRLoadingWeighingTOList.OrderByDescending(o => o.WeighingStageId).FirstOrDefault();
                    if (macthTO != null)
                    {
                        tblTRLoadingWeighingTO.WeighingStageId = macthTO.WeighingStageId + 1;
                        tblTRLoadingWeighingTO.GrossWeight = macthTO.ActualWeight;
                        tblTRLoadingWeighingTO.ActualWeight = macthTO.ActualWeight;
                        tblTRLoadingWeighingTO.NetWeight = 0;
                    }
                }
            }
            else if (tblTRLoadingWeighingTO.LoadingTypeId == (Int32)Constants.DocumentTypeE.UNLOADING)
            {
                if (tblTRLoadingWeighingTO.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.GROSS_WEIGHT)
                {
                    tblTRLoadingWeighingTO.GrossWeight = tblTRLoadingWeighingTO.ActualWeight;
                    tblTRLoadingWeighingTO.NetWeight = 0;
                    tblTRLoadingWeighingTO.WeighingStageId = 1;
                }
                else if (tblTRLoadingWeighingTO.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.INTERMEDIATE_WEIGHT)
                {
                    var macthTO = tblTRLoadingWeighingTOList.OrderByDescending(o => o.WeighingStageId).FirstOrDefault();
                    if (macthTO != null)
                    {
                        tblTRLoadingWeighingTO.WeighingStageId = macthTO.WeighingStageId + 1;
                        tblTRLoadingWeighingTO.GrossWeight = macthTO.ActualWeight;
                        tblTRLoadingWeighingTO.NetWeight = macthTO.ActualWeight - tblTRLoadingWeighingTO.ActualWeight;
                    }
                }
                else if (tblTRLoadingWeighingTO.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.TARE_WEIGHT)
                {
                    var macthTO = tblTRLoadingWeighingTOList.OrderByDescending(o => o.WeighingStageId).FirstOrDefault();
                    if (macthTO != null)
                    {
                        tblTRLoadingWeighingTO.WeighingStageId = macthTO.WeighingStageId + 1;
                        tblTRLoadingWeighingTO.GrossWeight = macthTO.ActualWeight;
                        tblTRLoadingWeighingTO.ActualWeight = macthTO.ActualWeight;
                        tblTRLoadingWeighingTO.NetWeight = 0;
                    }
                }
            }
            return tblTRLoadingWeighingTO;
        }
        public ResultMessage PostWeighingDetails(TblTRLoadingWeighingTO tblTRLoadingWeighingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblTRLoadingWeighingTO == null)
                {
                    resultMessage.DefaultBehaviour("Loading Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblTRLoadingWeighingTO> tblTRLoadingWeighingTOList = new List<TblTRLoadingWeighingTO>();
                lock (weighingLock)
                {
                    #region validate Loading details before update
                    TblTRLoadingTO tempTblTRLoadingTO = _iLoadingDAO.GetLoadingDetailsTO(tblTRLoadingWeighingTO.LoadingId, conn, tran);
                    if (tempTblTRLoadingTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Get Loading Details - GetLoadingDetailsTO");
                        return resultMessage;
                    }
                    if(tempTblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.UNDER_UNLOADING && tempTblTRLoadingTO.IsReviewUnloading == 0)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Unloading Review Pending.");
                        return resultMessage;
                    }
                    if (tempTblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.REJECTED)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Transaction Already Closed.");
                        return resultMessage;
                    }
                    #endregion
                    #region Set Basic Values
                    int result = 0;
                    DateTime ServerDateTime = _iCommon.ServerDateTime;
                    TblTRLoadingTO tblTRLoadingTO = new TblTRLoadingTO();
                    tblTRLoadingWeighingTO.CreatedOn = ServerDateTime;
                    tblTRLoadingTO.StatusBy = tblTRLoadingWeighingTO.CreatedBy;
                    tblTRLoadingTO.StatusOn = ServerDateTime;
                    tblTRLoadingTO.IdLoading = tblTRLoadingWeighingTO.LoadingId;
                    #endregion

                    #region Get Weighing Details and Prepare Data
                    tblTRLoadingWeighingTOList = _iLoadingDAO.GetWeighingDetails(tblTRLoadingWeighingTO.LoadingId, tblTRLoadingWeighingTO.LoadingTypeId);
                    if (tblTRLoadingWeighingTOList != null && tblTRLoadingWeighingTOList.Count > 0)
                    {
                        var tareWeightTO = tblTRLoadingWeighingTOList.Where(w => w.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.TARE_WEIGHT).FirstOrDefault();
                        var interMediateWeightTO = tblTRLoadingWeighingTOList.Where(w => w.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.INTERMEDIATE_WEIGHT).FirstOrDefault();
                        var grossWeightTO = tblTRLoadingWeighingTOList.Where(w => w.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.GROSS_WEIGHT).FirstOrDefault();
                        if (tblTRLoadingWeighingTO.LoadingTypeId == (Int32)Constants.DocumentTypeE.LOADING)
                        {
                            if (grossWeightTO != null)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Weighing Alredy Completed, Kindly Refresh.");
                                return resultMessage;
                            }
                            if (tblTRLoadingWeighingTO.IsWeighingCompleted == 1)
                            {
                                if (tareWeightTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Tare Weight Not Found, Can Not Complete Weighing");
                                    return resultMessage;
                                }
                                if (interMediateWeightTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Intermediate Weight Not Found, Can Not Complete Weighing");
                                    return resultMessage;
                                }
                                tblTRLoadingWeighingTO.WeighingMeasureTypeId = (Int32)Constants.WeighingMeasureTypeIdE.GROSS_WEIGHT;
                                tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.LOADING_GATE_OUT;
                            }
                            else
                            {
                                if (tareWeightTO == null)
                                {
                                    tblTRLoadingWeighingTO.WeighingMeasureTypeId = (Int32)Constants.WeighingMeasureTypeIdE.TARE_WEIGHT;
                                    tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.UNDER_LOADING;
                                }
                                tblTRLoadingWeighingTO.WeighingMeasureTypeId = (Int32)Constants.WeighingMeasureTypeIdE.INTERMEDIATE_WEIGHT;
                                tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.LOADING_FINAL_WEIGHING;
                            }
                            var macthTO = tblTRLoadingWeighingTOList.OrderByDescending(o => o.WeighingStageId).FirstOrDefault();
                            if (macthTO != null && tblTRLoadingWeighingTO.IsWeighingCompleted != 1)
                            {
                                if (tblTRLoadingWeighingTO.ActualWeight <= macthTO.ActualWeight)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Measurement Should be greater than last Measurement - " + macthTO.ActualWeight + " KG.");
                                    return resultMessage;
                                }
                            }
                        }
                        else if (tblTRLoadingWeighingTO.LoadingTypeId == (Int32)Constants.DocumentTypeE.UNLOADING)
                        {
                            if (tareWeightTO != null)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Weighing Alredy Completed, Kindly Refresh.");
                                return resultMessage;
                            }
                            if (tblTRLoadingWeighingTO.IsWeighingCompleted == 1)
                            {
                                if (grossWeightTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Gross Weight Not Found, Can Not Complete Weighing");
                                    return resultMessage;
                                }
                                if (interMediateWeightTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Intermediate Weight Not Found, Can Not Complete Weighing");
                                    return resultMessage;
                                }
                                tblTRLoadingWeighingTO.WeighingMeasureTypeId = (Int32)Constants.WeighingMeasureTypeIdE.TARE_WEIGHT;
                                tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.UNLOAD_GATE_OUT;
                            }
                            else
                            {
                                if (grossWeightTO == null)
                                {
                                    tblTRLoadingWeighingTO.WeighingMeasureTypeId = (Int32)Constants.WeighingMeasureTypeIdE.GROSS_WEIGHT;
                                    tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.UNDER_UNLOADING;
                                }
                                tblTRLoadingWeighingTO.WeighingMeasureTypeId = (Int32)Constants.WeighingMeasureTypeIdE.INTERMEDIATE_WEIGHT;
                                tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.UNDER_FINAL_WEIGHING;
                            }
                            var macthTO = tblTRLoadingWeighingTOList.OrderByDescending(o => o.WeighingStageId).FirstOrDefault();
                            if (macthTO != null && tblTRLoadingWeighingTO.IsWeighingCompleted != 1)
                            {
                                if (tblTRLoadingWeighingTO.ActualWeight >= macthTO.ActualWeight)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Measurement Should be less than last Measurement - " + macthTO.ActualWeight + " KG.");
                                    return resultMessage;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (tblTRLoadingWeighingTO.LoadingTypeId == (Int32)Constants.DocumentTypeE.LOADING)
                        {
                            tblTRLoadingWeighingTO.WeighingMeasureTypeId = (Int32)Constants.WeighingMeasureTypeIdE.TARE_WEIGHT;
                            tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.UNDER_LOADING;
                            if (tblTRLoadingWeighingTO.IsWeighingCompleted == 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Tare Weight Not Found, Can Not Complete Weighing");
                                return resultMessage;
                            }
                        }
                        else if (tblTRLoadingWeighingTO.LoadingTypeId == (Int32)Constants.DocumentTypeE.UNLOADING)
                        {
                            if (tblTRLoadingWeighingTO.IsWeighingCompleted == 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Gross Weight Not Found, Can Not Complete Weighing");
                                return resultMessage;
                            }
                            tblTRLoadingWeighingTO.WeighingMeasureTypeId = (Int32)Constants.WeighingMeasureTypeIdE.GROSS_WEIGHT;
                            tblTRLoadingTO.StatusId = (Int32)Constants.LoadingStatusE.UNDER_UNLOADING;
                        }
                    }
                    #endregion

                    #region Add Weighing Details
                    tblTRLoadingWeighingTO = CalculateWeighingValues(tblTRLoadingWeighingTO, tblTRLoadingWeighingTOList);
                    result = _iLoadingDAO.InsertWeighing(tblTRLoadingWeighingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertWeighing");
                        return resultMessage;
                    }
                    #endregion
                    #region Update Loading
                    result = _iLoadingDAO.UpdateLoadingStatus(tblTRLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Update - UpdateLoadingStatus");
                        return resultMessage;
                    }
                    #endregion
                    #region Add Loading History
                    TblTRLoadingTO UpdatedTblTRLoadingTO = _iLoadingDAO.GetLoadingDetailsTO(tblTRLoadingTO.IdLoading, conn, tran);
                    if (UpdatedTblTRLoadingTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Get Loading Details - GetLoadingDetailsTO");
                        return resultMessage;
                    }
                    result = _iLoadingDAO.InsertLoadingHistory(UpdatedTblTRLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertLoadingHistory");
                        return resultMessage;
                    }
                    #endregion
                    if (tblTRLoadingWeighingTO.IsWeighingCompleted == 1 && tblTRLoadingWeighingTO.LoadingTypeId == (Int32)Constants.DocumentTypeE.UNLOADING)
                    {
                        tblTRLoadingWeighingTOList = _iLoadingDAO.GetWeighingDetails(tblTRLoadingWeighingTO.LoadingId, (Int32)Constants.LoadingTypeIdE.LOADING+","+(Int32)Constants.LoadingTypeIdE.UNLOADING, conn, tran);
                        if (tblTRLoadingWeighingTOList == null || tblTRLoadingWeighingTOList.Count == 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Get Weighing Details - GetWeighingDetails");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        if (tblTRLoadingWeighingTOList == null)
                            tblTRLoadingWeighingTOList = new List<TblTRLoadingWeighingTO>();
                        tblTRLoadingWeighingTOList.Add(tblTRLoadingWeighingTO);
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tblTRLoadingWeighingTOList;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "PostWeighingDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public ResultMessage DownloadSLASummaryReport(UnloadingSLAFilterTO unloadingSLAFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable LoadingDT = new DataTable();
                DataTable HeaderDT = new DataTable();
                #region Get Vehicle Wise Loading Details List
                List<TblUnloadingSLATO> TblUnloadingSLATOList = _iLoadingDAO.GetUnloadingSLAReportDetails(unloadingSLAFilterTO);
                if (TblUnloadingSLATOList == null || TblUnloadingSLATOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("SLA Summary Report Details Not Found");
                    resultMessage.Result = 2;
                    return resultMessage;
                } 
                #endregion
                #region Set DataTable Values
                LoadingDT = ToDataTable(TblUnloadingSLATOList);
                if (LoadingDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get Vehicle Wise Loading Report Details - ToDataTable");
                    return resultMessage;
                }
                LoadingDT.TableName = "LoadingDT";
                #endregion
                #region Set HeaderDT
                HeaderDT.Columns.Add("FromDate");
                HeaderDT.Columns.Add("ToDate");
                HeaderDT.Rows.Add();
                if (Convert.ToString(unloadingSLAFilterTO.FromDate) != "01-01-0001 00:00:00" && Convert.ToString(unloadingSLAFilterTO.FromDate) != "01/01/0001 12:00:00 AM" && Convert.ToString(unloadingSLAFilterTO.ToDate) != "01-01-0001 00:00:00" && Convert.ToString(unloadingSLAFilterTO.ToDate) != "01/01/0001 12:00:00 AM")
                {
                    HeaderDT.Rows[0]["FromDate"] = unloadingSLAFilterTO.FromDate.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = unloadingSLAFilterTO.ToDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    HeaderDT.Rows[0]["FromDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                }
                HeaderDT.TableName = "HeaderDT";
                #endregion
                printDataSet.Tables.Add(LoadingDT);
                printDataSet.Tables.Add(HeaderDT);

                String templateName = Constants.SLASummaryReport;
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = Constants.SLASummaryReport + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Something wents wrong please try again");
                    return resultMessage;
                }
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
                    string[] fileEntries = Directory.GetFiles(directoryName, "*" + Constants.SLASummaryReport + "*");
                    string[] filesList = Directory.GetFiles(directoryName, "*" + Constants.SLASummaryReport + "*");
                    foreach (string file in filesList)
                    {
                        File.Delete(file);
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }
        public ResultMessage DownloadSLAFurnaceSummaryReport(UnloadingSLAFilterTO unloadingSLAFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable LoadingDT = new DataTable();
                DataTable LoadingDtlDT = new DataTable();

                DataTable HeaderDT = new DataTable();
                #region Get Vehicle Wise Loading Details List
                List<TblUnloadingSLATO> TblUnloadingSLATOList = _iLoadingDAO.GetUnloadingSLAFurnaceReportDetails(unloadingSLAFilterTO);
                if (TblUnloadingSLATOList == null || TblUnloadingSLATOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("SLA Furnace Summary Report Details Not Found");
                    resultMessage.Result = 2;
                    return resultMessage;
                }
                List<TblUnloadingSLATO> TblUnloadingSLATODetailList = new List<TblUnloadingSLATO>();
                if (TblUnloadingSLATOList != null && TblUnloadingSLATOList.Count > 0)
                {
                    List<TblUnloadingSLATO> TblUnloadingSLATOListTemp = TblUnloadingSLATOList.GroupBy(x => x.IdGenericMaster).Select(s => s.FirstOrDefault()).ToList(); 
                    if (TblUnloadingSLATOListTemp != null && TblUnloadingSLATOListTemp.Count > 0)
                    {
                        for (int i = 0; i < TblUnloadingSLATOListTemp.Count; i++)
                        {
                            TblUnloadingSLATO TblUnloadingSLATO = TblUnloadingSLATOListTemp[i] ;
                            List<TblUnloadingSLATO> TblUnloadingSLATOListLocal = TblUnloadingSLATOList.Where(w => w.IdGenericMaster == TblUnloadingSLATO.IdGenericMaster).ToList();
                            if (TblUnloadingSLATOListLocal != null && TblUnloadingSLATOListLocal.Count > 0)
                            {
                                TblUnloadingSLATODetailList.AddRange(TblUnloadingSLATOListLocal);
                            }
                        }
                    }
                }
                #endregion
                #region Set DataTable Values
                List<DimGenericMasterTO > DimGenericMasterTOList = _iDimensionBL.GetGenericMasterData((int)Constants.MasterDimenstion.Unloading_Point,0,0);
                if (DimGenericMasterTOList != null && DimGenericMasterTOList.Count > 0)
                {
                    LoadingDT.Columns.Add("SR NO");
                    LoadingDT.Columns.Add("Material");
                    for (int i = 0; i < DimGenericMasterTOList.Count; i++)
                    {
                        LoadingDT.Columns.Add("Required "+ DimGenericMasterTOList[i].Value);
                        LoadingDT.Columns.Add(DimGenericMasterTOList[i].Value);
                        LoadingDT.Columns.Add("SLA% " + DimGenericMasterTOList[i].Value);

                    }
                    if (LoadingDT.Columns.Count > 0 && LoadingDT != null)
                    {
                        List<TblUnloadingSLATO> TblUnloadingSLATOListTemp = TblUnloadingSLATOList.GroupBy(x => x.ItemName ).Select(s => s.FirstOrDefault()).ToList();
                        if (TblUnloadingSLATOListTemp != null && TblUnloadingSLATOListTemp.Count > 0)
                        {
                            for (int a = 0; a < TblUnloadingSLATOListTemp.Count; a++)
                            {
                                LoadingDT.Rows.Add();
                                int count = LoadingDT.Rows.Count;
                                LoadingDT.Rows[count-1]["SR NO"] = count;
                                LoadingDT.Rows[count - 1]["Material"] = TblUnloadingSLATOListTemp[a].ItemName.ToString();
                                for (int k = 0; k < DimGenericMasterTOList.Count; k++)
                                {
                                    List<TblUnloadingSLATO> tblUnloadingSLATOList = TblUnloadingSLATOList.Where(w => w.IdGenericMaster == DimGenericMasterTOList[k].IdGenericMaster).ToList();
                                    if (tblUnloadingSLATOList != null && tblUnloadingSLATOList.Count >0)
                                    {
                                        tblUnloadingSLATOList = tblUnloadingSLATOList.Where(w => TblUnloadingSLATOListTemp[a].ItemName.Contains(w.ItemName)).ToList(); 
                                        if (tblUnloadingSLATOList != null && tblUnloadingSLATOList.Count > 0)
                                        {
                                            LoadingDT.Rows[count - 1]["Required " + DimGenericMasterTOList[k].Value] = tblUnloadingSLATOList[0].Required;
                                            LoadingDT.Rows[count - 1][DimGenericMasterTOList[k].Value] = tblUnloadingSLATOList[0].Supply;
                                            LoadingDT.Rows[count - 1]["SLA% " + DimGenericMasterTOList[k].Value] = tblUnloadingSLATOList[0].SLAPer;
                                        }
                                    }
                                    //&& (TblUnloadingSLATOListTemp[a].ItemName.Contains(w.ItemName ).ToString ())).


                                    //listB.Where(b => listA.Any(a => b.Contains(a))
                                    //&& w. == TblUnloadingSLATOListTemp[a].IdGenericMaster).ToList();
                                    
                                }
                            }
                        } 
                    }
                }

                //LoadingDT = ToDataTable(TblUnloadingSLATOList);
                //LoadingDtlDT = ToDataTable(TblUnloadingSLATODetailList);

                if (LoadingDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get SLA Furnace -LoadingDT ToDataTable");
                    return resultMessage;
                }
                if (LoadingDtlDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get SLA Furnace Summary Report -LoadingDtlDT ToDataTable");
                    return resultMessage;
                }
                LoadingDT.TableName = "POFollowUpDT";
                LoadingDtlDT.TableName = "LoadingDtlDT";
                #endregion
                #region Set HeaderDT
                HeaderDT.Columns.Add("FromDate");
                HeaderDT.Columns.Add("ToDate");
                HeaderDT.Rows.Add();
                if (Convert.ToString(unloadingSLAFilterTO.FromDate) != "01-01-0001 00:00:00" && Convert.ToString(unloadingSLAFilterTO.FromDate) != "01/01/0001 12:00:00 AM" && Convert.ToString(unloadingSLAFilterTO.ToDate) != "01-01-0001 00:00:00" && Convert.ToString(unloadingSLAFilterTO.ToDate) != "01/01/0001 12:00:00 AM")
                {
                    HeaderDT.Rows[0]["FromDate"] = unloadingSLAFilterTO.FromDate.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = unloadingSLAFilterTO.ToDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    HeaderDT.Rows[0]["FromDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                }
                HeaderDT.TableName = "HeaderDT";
                #endregion
                printDataSet.Tables.Add(LoadingDT);
                printDataSet.Tables.Add(LoadingDtlDT);

                printDataSet.Tables.Add(HeaderDT);

                String templateName = Constants.SLAFurnaceSummaryReport;
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = Constants.SLAFurnaceSummaryReport + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Something wents wrong please try again");
                    return resultMessage;
                }
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
                    string[] fileEntries = Directory.GetFiles(directoryName, "*" + Constants.SLAFurnaceSummaryReport + "*");
                    string[] filesList = Directory.GetFiles(directoryName, "*" + Constants.SLAFurnaceSummaryReport + "*");
                    foreach (string file in filesList)
                    {
                        File.Delete(file);
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }
        public ResultMessage DownloadVehicleWiseLoadingReport(LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable LoadingDT = new DataTable();
                DataTable HeaderDT = new DataTable();
                #region Get Vehicle Wise Loading Details List
                List<TblTRLoadingTO> tblTRLoadingTOList = _iLoadingDAO.GetVehicleWiseLoadingReportDetails(loadingFilterTO);
                if (tblTRLoadingTOList == null || tblTRLoadingTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Vehicle Wise Loading Report Details Not Found");
                    resultMessage.Result = 2;
                    return resultMessage;
                }
                #endregion
                #region Set DataTable Values
                LoadingDT = ToDataTable(tblTRLoadingTOList);
                if (LoadingDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get Vehicle Wise Loading Report Details - ToDataTable");
                    return resultMessage;
                }
                LoadingDT.TableName = "LoadingDT";
                #endregion
                #region Set HeaderDT
                HeaderDT.Columns.Add("FromDate");
                HeaderDT.Columns.Add("ToDate");
                HeaderDT.Rows.Add();
                if (Convert.ToString(loadingFilterTO.FromDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.FromDate) != "01/01/0001 12:00:00 AM" && Convert.ToString(loadingFilterTO.ToDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.ToDate) != "01/01/0001 12:00:00 AM")
                {
                    HeaderDT.Rows[0]["FromDate"] = loadingFilterTO.FromDate.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = loadingFilterTO.ToDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    HeaderDT.Rows[0]["FromDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                }
                HeaderDT.TableName = "HeaderDT";
                #endregion
                printDataSet.Tables.Add(LoadingDT);
                printDataSet.Tables.Add(HeaderDT);

                String templateName = Constants.VehicleWiseLoadingReport;
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = Constants.VehicleWiseLoadingReport + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Something wents wrong please try again");
                    return resultMessage;
                }
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
                    string[] fileEntries = Directory.GetFiles(directoryName, "*" + Constants.VehicleWiseLoadingReport + "*");
                    string[] filesList = Directory.GetFiles(directoryName, "*" + Constants.VehicleWiseLoadingReport + "*");
                    foreach (string file in filesList)
                    {
                        File.Delete(file);
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }
        public ResultMessage DownloadLoadingUnloadingMasterReport(LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable LoadingDT = new DataTable();
                DataTable HeaderDT = new DataTable();
                #region Get Vehicle Wise Loading Details List
                List<TblTRLoadingTO> tblTRLoadingTOList = _iLoadingDAO.GetLoadingDetailsTOList(loadingFilterTO);
                if (tblTRLoadingTOList == null || tblTRLoadingTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Loading-Unloading Master Report Details Not Found");
                    resultMessage.Result = 2;
                    return resultMessage;
                }
                tblTRLoadingTOList.ForEach(element =>
                {
                    element.VehicleStatus = "Pending";
                    if (element.StatusId == (Int32)Constants.LoadingStatusE.COMPLETED || element.StatusId == (Int32)Constants.LoadingStatusE.UNLOAD_GATE_OUT)
                    {
                        element.VehicleStatus = "Completed";
                    }
                    if(element.StatusId == (Int32)Constants.LoadingStatusE.REJECTED)
                    {
                        element.VehicleStatus = "Closed";
                    }
                });
                String IdLoadings = string.Join(",", tblTRLoadingTOList.Select(d => d.IdLoading.ToString()).ToArray());
                List<TblTRLoadingWeighingTO> tblTRLoadingWeighingTOList = _iLoadingDAO.GetWeighingDetails(IdLoadings, (Int32)Constants.LoadingTypeIdE.LOADING + "," + (Int32)Constants.LoadingTypeIdE.UNLOADING);
                if (tblTRLoadingWeighingTOList != null && tblTRLoadingWeighingTOList.Count > 0)
                {
                    tblTRLoadingTOList.ForEach(element =>
                    {
                        var matchTO = tblTRLoadingWeighingTOList.Where(w => w.LoadingId == element.IdLoading).ToList();
                        if (matchTO != null && matchTO.Count > 0)
                        {
                            var loadingWeighingTOList = matchTO.Where(w => w.LoadingTypeId == (Int32)Constants.LoadingTypeIdE.LOADING).ToList();
                            if(loadingWeighingTOList != null && loadingWeighingTOList.Count > 0)
                            {
                                element.LoadingGrossWeight = loadingWeighingTOList.Max(m => m.ActualWeight);
                                element.LoadingNetWeight = loadingWeighingTOList.Sum(m => m.NetWeight);
                                var tareWeightTO = loadingWeighingTOList.Where(w => w.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.TARE_WEIGHT).FirstOrDefault();
                                if(tareWeightTO != null)
                                {
                                    element.LoadingTareWeight = tareWeightTO.ActualWeight;
                                }
                            }
                            var UnLoadingWeighingTOList = matchTO.Where(w => w.LoadingTypeId == (Int32)Constants.LoadingTypeIdE.UNLOADING).ToList();
                            if (UnLoadingWeighingTOList != null && UnLoadingWeighingTOList.Count > 0)
                            {
                                var grossWeightTO = UnLoadingWeighingTOList.Where(w => w.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.GROSS_WEIGHT).FirstOrDefault();
                                if (grossWeightTO != null)
                                {
                                    element.UnloadingGrossWeight = grossWeightTO.ActualWeight;
                                }
                                element.UnloadingNetWeight = UnLoadingWeighingTOList.Sum(m => m.NetWeight);
                                element.UnloadingTareWeight = UnLoadingWeighingTOList.Min(m => m.ActualWeight);
                            }
                        }
                    });
                }

                string statusIdStr = (Int32)Constants.LoadingStatusE.LOADING_GATE_OUT + "," + (Int32)Constants.LoadingStatusE.UNLOAD_GATE_OUT;
                List<TblTRLoadingTO> TblTRLoadingHistoryTOList = _iLoadingDAO.GetTRLoadingHistoryDetails(statusIdStr, IdLoadings);
                if (TblTRLoadingHistoryTOList != null && TblTRLoadingHistoryTOList.Count > 0)
                {
                    tblTRLoadingTOList.ForEach(element =>
                    {
                        TblTRLoadingTO loadingCompletedTO = TblTRLoadingHistoryTOList.Where(w => w.IdLoading == element.IdLoading && w.StatusId == (Int32)Constants.LoadingStatusE.LOADING_GATE_OUT).FirstOrDefault();
                        TblTRLoadingTO unLoadingCompletedTO = TblTRLoadingHistoryTOList.Where(w => w.IdLoading == element.IdLoading && w.StatusId == (Int32)Constants.LoadingStatusE.UNLOAD_GATE_OUT).FirstOrDefault();
                        if(loadingCompletedTO != null)
                        {
                            element.LoadingCompletedOnDateStr = loadingCompletedTO.StatusOn.ToString("dd/MM/yyyy");
                            element.LoadingCompletedOnTimeStr = loadingCompletedTO.StatusOn.ToString("hh:mm tt");
                        }
                        if (unLoadingCompletedTO != null)
                        {
                            element.UnLoadingCompletedOnDateStr = unLoadingCompletedTO.StatusOn.ToString("dd/MM/yyyy");
                            element.UnLoadingCompletedOnTimeStr = unLoadingCompletedTO.StatusOn.ToString("hh:mm tt");
                        }
                        if(loadingCompletedTO != null && unLoadingCompletedTO != null)
                        {
                            TimeSpan value = unLoadingCompletedTO.StatusOn.Subtract(loadingCompletedTO.StatusOn);
                            element.UnloadingTimeDiff = value.ToString();
                        }
                    });
                }
                #endregion
                #region Set DataTable Values
                LoadingDT = ToDataTable(tblTRLoadingTOList);
                if (LoadingDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get Loading-Unloading Master Report Details - ToDataTable");
                    return resultMessage;
                }
                LoadingDT.TableName = "LoadingDT";
                #endregion
                #region Set HeaderDT
                HeaderDT.Columns.Add("FromDate");
                HeaderDT.Columns.Add("ToDate");
                HeaderDT.Rows.Add();
                if (Convert.ToString(loadingFilterTO.FromDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.FromDate) != "01/01/0001 12:00:00 AM" && Convert.ToString(loadingFilterTO.ToDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.ToDate) != "01/01/0001 12:00:00 AM")
                {
                    HeaderDT.Rows[0]["FromDate"] = loadingFilterTO.FromDate.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = loadingFilterTO.ToDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    HeaderDT.Rows[0]["FromDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                }
                HeaderDT.TableName = "HeaderDT";
                #endregion
                printDataSet.Tables.Add(LoadingDT);
                printDataSet.Tables.Add(HeaderDT);

                String templateName = Constants.LoadingUnloadingMasterReport;
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = Constants.LoadingUnloadingMasterReport + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Something wents wrong please try again");
                    return resultMessage;
                }
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
                    string[] fileEntries = Directory.GetFiles(directoryName, "*" + Constants.LoadingUnloadingMasterReport + "*");
                    string[] filesList = Directory.GetFiles(directoryName, "*" + Constants.LoadingUnloadingMasterReport + "*");
                    foreach (string file in filesList)
                    {
                        File.Delete(file);
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }
        public ResultMessage DownloadWBDeviationReport(LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable LoadingDT = new DataTable();
                DataTable HeaderDT = new DataTable();
                #region Get Vehicle Wise Loading Details List
                List<TblTRLoadingTO> tblTRLoadingTOList = _iLoadingDAO.GetVehicleWiseLoadingReportDetails(loadingFilterTO);
                if (tblTRLoadingTOList == null || tblTRLoadingTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("WB Deviation Report Details Not Found");
                    resultMessage.Result = 2;
                    return resultMessage;
                }
                List<List<TblTRLoadingTO>> enumerationsList = new List<List<TblTRLoadingTO>>();
                enumerationsList = ListExtensions.ChunkBy(tblTRLoadingTOList, 75);
                for (int i = 0; i < enumerationsList.Count; i++)
                {
                    string IdLoadingStr = string.Join(",", enumerationsList[i].Select(n => n.IdLoading.ToString()).ToArray());
                    List<TblTRLoadingWeighingTO> TblTRLoadingWeighingTOList = _iLoadingDAO.GetWeighingNetWeightDetails(IdLoadingStr, (Int32)Constants.LoadingTypeIdE.UNLOADING);
                    if (TblTRLoadingWeighingTOList != null && TblTRLoadingWeighingTOList.Count > 0)
                    {
                        tblTRLoadingTOList.ForEach(element =>
                        {
                            var matchTOList = TblTRLoadingWeighingTOList.Where(w => w.LoadingId == element.IdLoading).ToList();
                            if (matchTOList.Count > 0)
                            {
                                element.UnloadingNetWeight = matchTOList[0].TotalNetWeight * Convert.ToDecimal(0.001);
                            }
                        });
                    }
                }
                #endregion
                #region Set DataTable Values
                LoadingDT = ToDataTable(tblTRLoadingTOList);
                if (LoadingDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get Vehicle Wise Loading Report Details - ToDataTable");
                    return resultMessage;
                }
                LoadingDT.TableName = "LoadingDT";
                #endregion
                #region Set HeaderDT
                HeaderDT.Columns.Add("FromDate");
                HeaderDT.Columns.Add("ToDate");
                HeaderDT.Rows.Add();
                if (Convert.ToString(loadingFilterTO.FromDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.FromDate) != "01/01/0001 12:00:00 AM" && Convert.ToString(loadingFilterTO.ToDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.ToDate) != "01/01/0001 12:00:00 AM")
                {
                    HeaderDT.Rows[0]["FromDate"] = loadingFilterTO.FromDate.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = loadingFilterTO.ToDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    HeaderDT.Rows[0]["FromDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                }
                HeaderDT.TableName = "HeaderDT";
                #endregion
                printDataSet.Tables.Add(LoadingDT);
                printDataSet.Tables.Add(HeaderDT);

                String templateName = Constants.WBDeviationReport;
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = Constants.WBDeviationReport + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Something wents wrong please try again");
                    return resultMessage;
                }
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
                    string[] fileEntries = Directory.GetFiles(directoryName, "*" + Constants.WBDeviationReport + "*");
                    string[] filesList = Directory.GetFiles(directoryName, "*" + Constants.WBDeviationReport + "*");
                    foreach (string file in filesList)
                    {
                        File.Delete(file);
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }
        public ResultMessage DownloadPOVsActualLoadingReport(LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try 
            {
                DataSet printDataSet = new DataSet();
                DataTable LoadingDT = new DataTable();
                DataTable RequestDT = new DataTable();
                DataTable HeaderDT = new DataTable();
                #region Get Vehicle Wise Loading Details List
                List<TblTRLoadingTO> tblTRLoadingTOList = _iLoadingDAO.GetVehicleWiseLoadingReportDetails(loadingFilterTO);
                if (tblTRLoadingTOList == null || tblTRLoadingTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("PO Vs Actual Loading Report Details Not Found");
                    resultMessage.Result = 2;
                    return resultMessage;
                }
                List<List<TblTRLoadingTO>> enumerationsList = new List<List<TblTRLoadingTO>>();
                enumerationsList = ListExtensions.ChunkBy(tblTRLoadingTOList, 75);
                for (int i = 0; i < enumerationsList.Count; i++)
                {
                    string IdLoadingStr = string.Join(",", enumerationsList[i].Select(n => n.IdLoading.ToString()).ToArray());
                    List<TblTRLoadingWeighingTO> TblTRLoadingWeighingTOList = _iLoadingDAO.GetWeighingNetWeightDetails(IdLoadingStr, (Int32)Constants.LoadingTypeIdE.UNLOADING);
                    if (TblTRLoadingWeighingTOList != null && TblTRLoadingWeighingTOList.Count > 0)
                    {
                        tblTRLoadingTOList.ForEach(element =>
                        {
                            var matchTOList = TblTRLoadingWeighingTOList.Where(w => w.LoadingId == element.IdLoading).ToList();
                            if (matchTOList.Count > 0)
                            {
                                element.UnloadingNetWeight = matchTOList[0].TotalNetWeight * Convert.ToDecimal(0.001);
                            }
                        });
                    }
                }
                #endregion
                #region Get Loading Request Details
                List<List<TblTRLoadingTO>> loadingEnumerationsList = new List<List<TblTRLoadingTO>>();
                List<TblTransferRequestTO> tblTransferRequestTOList = new List<TblTransferRequestTO>();
                loadingEnumerationsList = ListExtensions.ChunkBy(tblTRLoadingTOList, 75);
                for (int i = 0; i < loadingEnumerationsList.Count; i++)
                {
                    string IdRequestStr = string.Join(",", loadingEnumerationsList[i].Select(n => n.TransferRequestId.ToString()).ToArray());
                    List<TblTransferRequestTO> tempTblTransferRequestTOList = _iTblTransferRequestDAO.SelectTblTransferRequest(IdRequestStr);
                    if (tempTblTransferRequestTOList != null && tempTblTransferRequestTOList.Count > 0)
                    {
                        tempTblTransferRequestTOList.ForEach(element =>
                        {
                            tblTransferRequestTOList.Add(element);
                        });
                    }
                }
                #endregion
                #region Set DataTable Values
                LoadingDT = ToDataTable(tblTRLoadingTOList);
                if (LoadingDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get Vehicle Wise Loading Report Details - ToDataTable");
                    return resultMessage;
                }
                LoadingDT.TableName = "LoadingDT";
                RequestDT = ToDataTable(tblTransferRequestTOList);
                if (RequestDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get Request Details Againts Loading Details - ToDataTable");
                    return resultMessage;
                }
                RequestDT.TableName = "RequestDT";
                #endregion
                #region Set HeaderDT
                HeaderDT.Columns.Add("FromDate");
                HeaderDT.Columns.Add("ToDate");
                HeaderDT.Rows.Add();
                if (Convert.ToString(loadingFilterTO.FromDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.FromDate) != "01/01/0001 12:00:00 AM" && Convert.ToString(loadingFilterTO.ToDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.ToDate) != "01/01/0001 12:00:00 AM")
                {
                    HeaderDT.Rows[0]["FromDate"] = loadingFilterTO.FromDate.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = loadingFilterTO.ToDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    HeaderDT.Rows[0]["FromDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                }
                HeaderDT.TableName = "HeaderDT";
                #endregion
                printDataSet.Tables.Add(LoadingDT);
                printDataSet.Tables.Add(HeaderDT);
                printDataSet.Tables.Add(RequestDT);

                String templateName = Constants.POVsActualLoadingReport;
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = Constants.POVsActualLoadingReport + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Something wents wrong please try again");
                    return resultMessage;
                }
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
                    string[] fileEntries = Directory.GetFiles(directoryName, "*" + Constants.POVsActualLoadingReport + "*");
                    string[] filesList = Directory.GetFiles(directoryName, "*" + Constants.POVsActualLoadingReport + "*");
                    foreach (string file in filesList)
                    {
                        File.Delete(file);
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }
        public ResultMessage DownloadTallyStockReport(LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable LoadingDT = new DataTable();
                DataTable LoadingDetailsDT = new DataTable();
                DataTable HeaderDT = new DataTable();
                #region Get Vehicle Wise Loading Details List
                List<TblTRLoadingTO> tblTRLoadingTOList = _iLoadingDAO.GetMaterialAndLocationWiseLoadingDtls(loadingFilterTO);
                if (tblTRLoadingTOList == null || tblTRLoadingTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Tally Stock Report Details Not Found");
                    resultMessage.Result = 2;
                    return resultMessage;
                }
                List<TblTRLoadingTO> LoadingAndMaterialWiseLoadingDetailsList = new List<TblTRLoadingTO>();
                for (int i = 0; i < tblTRLoadingTOList.Count; i++)
                {
                    String WhereClause = " AND tblTRLoading.fromLocationId = " + tblTRLoadingTOList[i].FromLocationId + " AND tblTRLoading.toLocationId = " + tblTRLoadingTOList[i].ToLocationId + " AND tblTRLoading.materialTypeId = " + tblTRLoadingTOList[i].MaterialTypeId + " AND tblTRLoading.materialSubTypeId = " + tblTRLoadingTOList[i].MaterialSubTypeId + " ";
                    List<TblTRLoadingTO> tempTblTRLoadingTOList = _iLoadingDAO.GetVehicleWiseLoadingReportDetails(loadingFilterTO, WhereClause);
                    if (tempTblTRLoadingTOList == null || tempTblTRLoadingTOList.Count == 0)
                    {
                        resultMessage.DefaultBehaviour("Tally Stock Report Details Not Found - Index : " + i);
                        return resultMessage;
                    }
                    if(tempTblTRLoadingTOList.Count != tblTRLoadingTOList[i].TotalTransactionCnt)
                    {
                        resultMessage.DefaultBehaviour("Tally Stock Report Details Not Found - Count Mismatched - Index : " + i);
                        return resultMessage;
                    }
                    String IdLoadings = string.Join(",", tempTblTRLoadingTOList.Select(d => d.IdLoading.ToString()).ToArray());
                    List<TblTRLoadingWeighingTO> tblTRLoadingWeighingTOList = _iLoadingDAO.GetWeighingDetails(IdLoadings, (Int32)Constants.LoadingTypeIdE.LOADING + "," + (Int32)Constants.LoadingTypeIdE.UNLOADING);
                    if (tblTRLoadingWeighingTOList != null && tblTRLoadingWeighingTOList.Count > 0)
                    {
                        tempTblTRLoadingTOList.ForEach(element =>
                        {
                            var matchTO = tblTRLoadingWeighingTOList.Where(w => w.LoadingId == element.IdLoading).ToList();
                            if (matchTO != null && matchTO.Count > 0)
                            {
                                var loadingWeighingTOList = matchTO.Where(w => w.LoadingTypeId == (Int32)Constants.LoadingTypeIdE.LOADING).ToList();
                                if (loadingWeighingTOList != null && loadingWeighingTOList.Count > 0)
                                {
                                    element.LoadingGrossWeight = loadingWeighingTOList.Max(m => m.ActualWeight) / 1000;
                                    element.LoadingNetWeight = loadingWeighingTOList.Sum(m => m.NetWeight) / 1000;
                                    var tareWeightTO = loadingWeighingTOList.Where(w => w.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.TARE_WEIGHT).FirstOrDefault();
                                    if (tareWeightTO != null)
                                    {
                                        element.LoadingTareWeight = tareWeightTO.ActualWeight / 1000;
                                    }
                                }
                                var UnLoadingWeighingTOList = matchTO.Where(w => w.LoadingTypeId == (Int32)Constants.LoadingTypeIdE.UNLOADING).ToList();
                                if (UnLoadingWeighingTOList != null && UnLoadingWeighingTOList.Count > 0)
                                {
                                    var grossWeightTO = UnLoadingWeighingTOList.Where(w => w.WeighingMeasureTypeId == (Int32)Constants.WeighingMeasureTypeIdE.GROSS_WEIGHT).FirstOrDefault();
                                    if (grossWeightTO != null)
                                    {
                                        element.UnloadingGrossWeight = grossWeightTO.ActualWeight / 1000;
                                    }
                                    element.UnloadingNetWeight = UnLoadingWeighingTOList.Sum(m => m.NetWeight) / 1000;
                                    element.UnloadingTareWeight = UnLoadingWeighingTOList.Min(m => m.ActualWeight) / 1000;
                                }
                            }
                        });
                    }
                    tblTRLoadingTOList[i].LoadingNetWeight = tempTblTRLoadingTOList.Sum(s => s.LoadingNetWeight);
                    tblTRLoadingTOList[i].UnloadingNetWeight = tempTblTRLoadingTOList.Sum(s => s.UnloadingNetWeight);
                    tempTblTRLoadingTOList.ForEach(element =>
                    {
                        LoadingAndMaterialWiseLoadingDetailsList.Add(element);
                    });
                }
                #endregion
                #region Set DataTable Values
                LoadingDT = ToDataTable(tblTRLoadingTOList);
                if (LoadingDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get Vehicle Wise Loading Report Details - ToDataTable");
                    return resultMessage;
                }
                LoadingDT.TableName = "LoadingDT";
                LoadingDetailsDT = ToDataTable(LoadingAndMaterialWiseLoadingDetailsList);
                if (LoadingDT == null)
                {
                    resultMessage.DefaultBehaviour("Failed to Get Vehicle Wise Loading Report Details - ToDataTable");
                    return resultMessage;
                }
                LoadingDetailsDT.TableName = "LoadingDetailsDT";
                #endregion
                #region Set HeaderDT
                HeaderDT.Columns.Add("FromDate");
                HeaderDT.Columns.Add("ToDate");
                HeaderDT.Rows.Add();
                if (Convert.ToString(loadingFilterTO.FromDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.FromDate) != "01/01/0001 12:00:00 AM" && Convert.ToString(loadingFilterTO.ToDate) != "01-01-0001 00:00:00" && Convert.ToString(loadingFilterTO.ToDate) != "01/01/0001 12:00:00 AM")
                {
                    HeaderDT.Rows[0]["FromDate"] = loadingFilterTO.FromDate.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = loadingFilterTO.ToDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    HeaderDT.Rows[0]["FromDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                    HeaderDT.Rows[0]["ToDate"] = _iCommon.ServerDateTime.ToString("dd/MM/yyyy");
                }
                HeaderDT.TableName = "HeaderDT";
                #endregion
                printDataSet.Tables.Add(LoadingDT);
                printDataSet.Tables.Add(HeaderDT);
                printDataSet.Tables.Add(LoadingDetailsDT);

                String templateName = Constants.TallyStockReport;
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = Constants.TallyStockReport + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Something wents wrong please try again");
                    return resultMessage;
                }
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
                    string[] fileEntries = Directory.GetFiles(directoryName, "*" + Constants.TallyStockReport + "*");
                    string[] filesList = Directory.GetFiles(directoryName, "*" + Constants.TallyStockReport + "*");
                    foreach (string file in filesList)
                    {
                        File.Delete(file);
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }
        public ResultMessage MigrateTransferRequest()
        {
            ResultMessage resultMessage = new ResultMessage();
            List<ResultMessage> resultMessageList = new List<ResultMessage>();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            SqlConnection requestConn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction requestTran = null;
            SqlConnection migconn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.MIGRATION_CONNECTION_STRING));
            SqlTransaction migtran = null;
            try
            {
                #region Set Basic Values
                DateTime ServerDateTime = _iCommon.ServerDateTime;
                LoadingFilterTO loadingFilterTO = new LoadingFilterTO();
                LoadingFilterTO reportFilterTO = new LoadingFilterTO();
                reportFilterTO.FromDate = ServerDateTime;
                reportFilterTO.ToDate = ServerDateTime;
                reportFilterTO.SkipDateFilter = false;
                UnloadingSLAFilterTO unloadingSLAFilterTO = new UnloadingSLAFilterTO();
                unloadingSLAFilterTO.FromDate = ServerDateTime;
                unloadingSLAFilterTO.ToDate = ServerDateTime;
                unloadingSLAFilterTO.SkipDateFilter = false;
                reportFilterTO.StatusIdStr = Convert.ToString((Int32)Constants.LoadingStatusE.COMPLETED);
                Int32 MigrateVoucherBeforeDays = 0;
                Int32 result = 0;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_MIGRATE_TRANSFER_REQUEST_BEFORE_DAYS);
                if (tblConfigParamsTO == null)
                {
                    resultMessage.DefaultBehaviour("tblConfigParamsTO Not Found");
                    return resultMessage;
                }
                if (!String.IsNullOrEmpty(tblConfigParamsTO.ConfigParamVal))
                {
                    MigrateVoucherBeforeDays = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                }
                loadingFilterTO.SkipDateFilter = true;
                loadingFilterTO.StatusIdStr = (Int32)Constants.LoadingStatusE.COMPLETED +","+ (Int32)Constants.LoadingStatusE.REJECTED;
                loadingFilterTO.MigrationDate = _iCommon.ServerDateTime.AddDays(-MigrateVoucherBeforeDays);
                #endregion
                #region Get Voucher Details List
                List<TblTRLoadingTO> tempTblTRLoadingTOList = _iLoadingDAO.GetLoadingDetailsTOList(loadingFilterTO);
                if (tempTblTRLoadingTOList == null || tempTblTRLoadingTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Record Not Available to Migrate");
                    return resultMessage;
                }
                String toLocationStr = "";
                TblConfigParamsTO toLocationConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SLA_DOCUMENT_MANDATORY_FOR_FOLLOWING_TO_LOCATION);
                if(toLocationConfigParamsTO != null)
                {
                    if (!String.IsNullOrEmpty(toLocationConfigParamsTO.ConfigParamVal))
                    {
                        toLocationStr = toLocationConfigParamsTO.ConfigParamVal;
                    }
                }
                List<TblTRLoadingTO> CompletedTblTRLoadingTOList = new List<TblTRLoadingTO>();
                tempTblTRLoadingTOList.ForEach(element =>
                {
                    if(element.StatusId == (Int32)Constants.LoadingStatusE.COMPLETED)
                    {
                        if(element.IdSLA > 0)
                        {
                            CompletedTblTRLoadingTOList.Add(element);
                        }
                        else
                        {
                            if (!toLocationStr.Contains(Convert.ToString(element.ToLocationId)))
                            {
                                CompletedTblTRLoadingTOList.Add(element);
                            }
                        }
                    }
                });
                //List<TblTRLoadingTO> CompletedTblTRLoadingTOList = tempTblTRLoadingTOList.Where(w => w.IdSLA > 0 && w.StatusId == (Int32)Constants.LoadingStatusE.COMPLETED).ToList();
                List<TblTRLoadingTO> RejectedTblTRLoadingTOList = tempTblTRLoadingTOList.Where(w => w.StatusId == (Int32)Constants.LoadingStatusE.REJECTED).ToList();
                #endregion
                conn.Open();
                tran = conn.BeginTransaction();
                migconn.Open();
                migtran = migconn.BeginTransaction();
                requestConn.Open();
                requestTran = requestConn.BeginTransaction();
                String file_Name = "";
                List<TblAddonsFunDtlsTO> AllTblAddonsFunDtlsTOList = new List<TblAddonsFunDtlsTO>();
                #region Completed Vehicle BackUp & Upload Report
                if (CompletedTblTRLoadingTOList != null && CompletedTblTRLoadingTOList.Count > 0)
                {
                    #region BackUp Data
                    for (int i = 0; i < CompletedTblTRLoadingTOList.Count; i++)
                    {
                        TblTRLoadingTO tblTRLoadingTO = CompletedTblTRLoadingTOList[i];

                        result = _iLoadingDAO.InsertFinalLoading(tblTRLoadingTO, migconn, migtran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Record Saved Failed - InsertFinalLoading - " + tblTRLoadingTO.LoadingSlipNo);
                            return resultMessage;
                        }

                        #region Get Addon Details List
                        List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList = _iTblAddonsFunDtlsDAO.SelectAddonDetailsList(tblTRLoadingTO.IdLoading, (Int32)Constants.DefaultModuleID, Constants.CommonNoteTrasactionType, Constants.CommonNotePageElementId+","+Constants.CommonNoteLoadingPageElementId, null);
                        if (tblAddonsFunDtlsTOList != null && tblAddonsFunDtlsTOList.Count > 0)
                        {
                            tblAddonsFunDtlsTOList.ForEach(element =>
                            {
                                AllTblAddonsFunDtlsTOList.Add(element);
                            });
                        }
                        #endregion
                    }
                    List<List<TblTRLoadingTO>> enumerationsList = new List<List<TblTRLoadingTO>>();
                    enumerationsList = ListExtensions.ChunkBy(CompletedTblTRLoadingTOList, 75);
                    for (int i = 0; i < enumerationsList.Count; i++)
                    {
                        string IdLoadingStr = string.Join(",", enumerationsList[i].Select(n => n.IdLoading.ToString()).ToArray());
                        if (!String.IsNullOrEmpty(IdLoadingStr))
                        {
                            result = _iLoadingDAO.InsertFinalWeighing(IdLoadingStr, migconn, migtran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Record Saved Failed - InsertFinalWeighing - " + IdLoadingStr);
                                return resultMessage;
                            }

                            result = _iLoadingDAO.DeleteTRLoadingWeighing(IdLoadingStr, conn, tran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteTRLoadingWeighing - " + IdLoadingStr);
                                return resultMessage;
                            }

                            result = _iLoadingDAO.InsertFinalUnloadingSLA(IdLoadingStr, migconn, migtran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Record Saved Failed - InsertFinalUnloadingSLA - " + IdLoadingStr);
                                return resultMessage;
                            }

                            result = _iLoadingDAO.DeleteUnloadingSLA(IdLoadingStr, conn, tran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteUnloadingSLA - " + IdLoadingStr);
                                return resultMessage;
                            }

                            result = _iLoadingDAO.InsertFinalLoadingHistory(IdLoadingStr, migconn, migtran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Record Saved Failed - InsertFinalLoadingHistory - " + IdLoadingStr);
                                return resultMessage;
                            }

                            result = _iLoadingDAO.DeleteTRLoadingHistory(IdLoadingStr, conn, tran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteTRLoadingHistory - " + IdLoadingStr);
                                return resultMessage;
                            }

                            result = _iLoadingDAO.DeleteTRLoading(IdLoadingStr, conn, tran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteTRLoading - " + IdLoadingStr);
                                return resultMessage;
                            }
                        }
                        #region Delete Completed Request
                        string TransferRequestIdStr = string.Join(",", enumerationsList[i].Select(n => n.TransferRequestId.ToString()).ToArray());
                        if (!String.IsNullOrEmpty(TransferRequestIdStr))
                        {
                            result = _iLoadingDAO.DeleteTransferRequest(TransferRequestIdStr, requestConn, requestTran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteTransferRequest - " + TransferRequestIdStr);
                                return resultMessage;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    #region Create Report & Upload
                    #region Set Basic Configuration
                    // Create azure storage  account connection.
                    CloudStorageAccount storage_Account = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));
                    // Create the blob client.
                    CloudBlobClient blob_Client = storage_Account.CreateCloudBlobClient();
                    // Retrieve reference to a target container.
                    CloudBlobContainer container_ = blob_Client.GetContainerReference(Constants.AzureSourceContainerName);
                    #endregion
                    #region Create All Reports
                    #region SLA SUMMARY REPORT
                    resultMessage = DownloadSLASummaryReport(unloadingSLAFilterTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        if(resultMessage.Result != 2)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Download SLA Summary Report - DownloadSLASummaryReport" + resultMessage.Text);
                            return resultMessage;
                        }
                    }
                    if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.Text = Constants.SLASummaryReport;
                        resultMessageList.Add(resultMessage.DeepCopy());
                    }
                    #endregion
                    #region vehicle wise loading report
                    resultMessage = DownloadVehicleWiseLoadingReport(reportFilterTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        if (resultMessage.Result != 2)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Download Vehicle Wise Loading Report - DownloadVehicleWiseLoadingReport" + resultMessage.Text);
                            return resultMessage;
                        } 
                    }
                    if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.Text = Constants.VehicleWiseLoadingReport;
                        resultMessageList.Add(resultMessage.DeepCopy());
                    }
                    #endregion
                    #region Tally Stock Report
                    resultMessage = DownloadTallyStockReport(reportFilterTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        if (resultMessage.Result != 2)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Download Tally Stock Report - DownloadTallyStockReport" + resultMessage.Text);
                            return resultMessage;
                        }
                    }
                    if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.Text = Constants.TallyStockReport;
                        resultMessageList.Add(resultMessage.DeepCopy());
                    }
                    #endregion
                    #region WB Deviation Report
                    resultMessage = DownloadWBDeviationReport(reportFilterTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        if (resultMessage.Result != 2)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Download WB Deviation Report - DownloadWBDeviationReport" + resultMessage.Text);
                            return resultMessage;
                        }
                    }
                    if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.Text = Constants.WBDeviationReport;
                        resultMessageList.Add(resultMessage.DeepCopy());
                    }
                    #endregion
                    #region PO Vs Actual Loading Report
                    resultMessage = DownloadPOVsActualLoadingReport(reportFilterTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        if (resultMessage.Result != 2)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Download PO Vs Actual Loading Report - DownloadPOVsActualLoadingReport" + resultMessage.Text);
                            return resultMessage;
                        }
                    }
                    if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.Text = Constants.POVsActualLoadingReport;
                        resultMessageList.Add(resultMessage.DeepCopy());
                    }
                    #endregion
                    #region SLA Furnace Report
                    resultMessage = DownloadSLAFurnaceSummaryReport(unloadingSLAFilterTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        if (resultMessage.Result != 2)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Download SLA Furnace Report - DownloadSLAFurnaceSummaryReport" + resultMessage.Text);
                            return resultMessage;
                        }
                    }
                    if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.Text = Constants.SLAFurnaceSummaryReport;
                        resultMessageList.Add(resultMessage.DeepCopy());
                    }
                    #endregion
                    #region Loading - Unloading Master Report
                    reportFilterTO.StatusIdStr = "";
                    resultMessage = DownloadLoadingUnloadingMasterReport(reportFilterTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        if (resultMessage.Result != 2)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Download Loading - Unloading Master Report - DownloadLoadingUnloadingMasterReport" + resultMessage.Text);
                            return resultMessage;
                        }
                    }
                    if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.Text = Constants.LoadingUnloadingMasterReport;
                        resultMessageList.Add(resultMessage.DeepCopy());
                    }
                    #endregion
                    #endregion
                    #region Upload Report to Azure
                    if(resultMessageList != null && resultMessageList.Count > 0)
                    {
                        for (int j = 0; j < resultMessageList.Count; j++)
                        {
                            String Report_file_Name = resultMessageList[j].Text + _iCommon.ServerDateTime.ToString("ddMMyyyyHHmmss") + ".xls";
                            CloudBlockBlob block_Blob1 = container_.GetBlockBlobReference(Report_file_Name);
                            byte[] ReportByteStream = (byte[])resultMessageList[j].Tag;
                            Task t3 = block_Blob1.UploadFromByteArrayAsync(ReportByteStream, 0, ReportByteStream.Length);
                        }
                    }
                    #endregion
                    #endregion
                }
                #endregion
                #region Delete Addon Files 
                if (AllTblAddonsFunDtlsTOList != null && AllTblAddonsFunDtlsTOList.Count > 0)
                {
                    String idAddonsfunDtlsStr = string.Join(",", AllTblAddonsFunDtlsTOList.Select(d => d.IdAddonsfunDtls.ToString()).ToArray());
                    result = _iTblAddonsFunDtlsDAO.DeleteTblAddonsFunDtls(idAddonsfunDtlsStr, conn, tran);
                    if (result == 0)
                    {
                        tran.Rollback();
                        migtran.Rollback();
                        requestTran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to Delete Addons Details - DeleteTblAddonsFunDtls");
                        return resultMessage;
                    }
                    AllTblAddonsFunDtlsTOList = AllTblAddonsFunDtlsTOList.Where(w => w.FunRefId != (Int32)Constants.dimAddonsFunE.NOTES).ToList();
                    if (AllTblAddonsFunDtlsTOList != null && AllTblAddonsFunDtlsTOList.Count > 0)
                    {
                        resultMessage = DeleteFileFromAzure(AllTblAddonsFunDtlsTOList, conn, tran);
                        if (resultMessage == null && resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            migtran.Rollback();
                            requestTran.Rollback();
                            resultMessage.DefaultBehaviour("Failed to Delete File From Azure");
                            return resultMessage;
                        }
                    }
                }
                #endregion
                #region Delete Data
                if(RejectedTblTRLoadingTOList != null && RejectedTblTRLoadingTOList.Count > 0)
                {
                    List<List<TblTRLoadingTO>> enumerationsList = new List<List<TblTRLoadingTO>>();
                    enumerationsList = ListExtensions.ChunkBy(RejectedTblTRLoadingTOList, 75);
                    for (int i = 0; i < enumerationsList.Count; i++)
                    {
                        string IdLoadingStr = string.Join(",", enumerationsList[i].Select(n => n.IdLoading.ToString()).ToArray());
                        if (!String.IsNullOrEmpty(IdLoadingStr))
                        { 
                            result = _iLoadingDAO.DeleteTRLoadingWeighing(IdLoadingStr, conn, tran);
                            if (result == -1)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteTRLoadingWeighing - " + IdLoadingStr);
                                return resultMessage;
                            }

                            result = _iLoadingDAO.DeleteTRLoadingHistory(IdLoadingStr, conn, tran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteTRLoadingHistory - " + IdLoadingStr);
                                return resultMessage;
                            }

                            result = _iLoadingDAO.DeleteTRLoading(IdLoadingStr, conn, tran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteTRLoading - " + IdLoadingStr);
                                return resultMessage;
                            }
                        }
                        #region delete completed request
                        string TransferRequestIdStr = string.Join(",", enumerationsList[i].Select(n => n.TransferRequestId.ToString()).ToArray());
                        if (!String.IsNullOrEmpty(TransferRequestIdStr))
                        {
                            result = _iLoadingDAO.DeleteTransferRequest(TransferRequestIdStr, requestConn, requestTran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                migtran.Rollback();
                                requestTran.Rollback();
                                resultMessage.DefaultBehaviour("Delete Record Failed - DeleteTransferRequest - " + TransferRequestIdStr);
                                return resultMessage;
                            }
                        }
                        #endregion
                    }
                }
                #endregion
                tran.Commit();
                migtran.Commit();
                requestTran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = file_Name;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                migtran.Rollback();
                requestTran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "MigrateTransferRequest");
                return resultMessage;
            }
            finally
            {
                conn.Close();
                requestConn.Close();
                migconn.Close();
            }
        }
        public ResultMessage DeleteFileFromAzure(List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (tblAddonsFunDtlsTOList == null || tblAddonsFunDtlsTOList.Count == 0)
                {
                    throw new Exception("tblAddonsFunDtlsTOList == null");
                }
                string AzureConnectionStr = _iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING);
                if (string.IsNullOrEmpty(AzureConnectionStr))
                {
                    throw new Exception("AzureConnectionStr == null");
                }
                string AzureSourceContainerName = "";
                Int32 moduleId = Constants.DefaultModuleID;
                TblModuleTO tblModuleTO = _iTblModuleDAO.SelectTblModule(moduleId);
                if (tblModuleTO == null)
                {
                    throw new Exception("tblModuleTO == null");
                }
                AzureSourceContainerName = tblModuleTO.ContainerName;
                if (string.IsNullOrEmpty(AzureSourceContainerName))
                {
                    throw new Exception("AzureSourceContainerName == null");
                }
                // Create azure storage  account connection.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureConnectionStr);
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve reference to a source container.
                CloudBlobContainer sourceContainer = blobClient.GetContainerReference(AzureSourceContainerName);
                for (int i = 0; i < tblAddonsFunDtlsTOList.Count; i++)
                {
                    Task<int> result = DeleteAzureFiles(tblAddonsFunDtlsTOList[i], sourceContainer);
                    // if(result < 0)
                    // {
                    //     throw new Exception("Error in DeleteAzureFiles(tblAddonsFunDtlsTOList[i],sourceContainer);");
                    // }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (System.Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in DeleteFileFromAzure(List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList, SqlConnection conn,SqlTransaction tran)");
                return resultMessage;
            }
        }
        public async Task<int> DeleteAzureFiles(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, CloudBlobContainer sourceContainer)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string path = tblAddonsFunDtlsTO.FunRefVal;

                string fileName = Path.GetFileName(path);

                CloudBlockBlob sourceBlob = sourceContainer.GetBlockBlobReference(fileName);

                await sourceBlob.DeleteIfExistsAsync();

                return 1;
            }
            catch (System.Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteAzureFiles(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)");
                return -1;
            }
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion
    }
}
 