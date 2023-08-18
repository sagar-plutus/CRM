using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseScheduleSummaryBL
    {
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsListPhasewise(DateTime fromDate, DateTime toDate, String userId, Int32 rootScheduleId, string showListE);

        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsListPhasewiseForComp(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTempTO);
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsList(String vehicleNo, String statusId, Int32 idPurchaseScheduleSummary);
        List<TblPurchaseScheduleSummaryTO> SelectVehicleListForAllCommonApprovals(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO);//, SqlConnection conn, SqlTransaction tran)
        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsList(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO); //, SqlConnection conn, SqlTransaction tran)

        List<TblPurchaseScheduleSummaryTO> GetVehSummaryForDashboard(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO); //, SqlConnection conn, SqlTransaction tran)
        List<JObject> GetListOfMasterReport(string fromDate, string toDate,Int32 masterReportTypeE, String flagDropbox, String purchaseManagerIds);
        List<dynamic> GetOldMasterReport(string fromDate, string toDate, Int32 masterReportTypeE,String purchaseManagerIds);
        List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummary();
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummary(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        ResultMessage PrintListOfMasterReport(string fromDate, string toDate,String purchaseManagerIds);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 idPurchaseScheduleSummary, Boolean isActive, SqlConnection conn, SqlTransaction tran);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByRootScheduleID(Int32 RootScheduleId, Boolean isActive, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootScheduleID(Int32 RootScheduleId, Boolean isActive);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 parentScheduleId);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByParentScheduleId(Int32 parentScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryDtlsByEnquiryId(Int32 purchaseEnquiryId);
        void GetScheduleLatestStatus(List<TblPurchaseScheduleSummaryTO> scheduleList);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootId(Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId, SqlConnection conn, SqlTransaction tran);
        Int32 SelectVehicleScheduleByRootAndStatusIdCount(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId, SqlConnection con, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId);
        List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, string statusIds, string vehiclePhaseIds);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummary(Int32 purchaseEnquiryId);
        List<TblPurchaseScheduleSummaryTO> SelectEnquiryScheduleSummary(Int32 purchaseEnquiryId);
        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForPurchaseEnquiry(Int32 purchaseEnquiryId);
        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForPurchaseEnquiry(Int32 purchaseEnquiryId, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> GetVehicleListForPendingQualityFlags(string pmUserId);
        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListByDate(DateTime scheduleDate);
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailListForRecovery(DateTime fromDate, DateTime toDate, string statusId, Int32 loggedInUserId, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummaryList();
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryDetails(Int32 idSchedulePurchaseSummary, Boolean isActive);
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId, string statusIds);
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId, string statusIds, SqlConnection conn, SqlTransaction tran);
        DropDownTO SelectSuperwiserFromTblPurchaseScheduleSummary(int statusId);
        List<TblPurchaseScheduleSummaryTO> GetScheduleDetailsByPurchaseEnquiryIdForDisplay(Int32 enquiryPurchaseId);
        List<TblPurchaseScheduleSummaryTO> SelectAllCorrectionCompleVehicles(DateTime toDate, Int32 cOrNcId);
        TblPurchaseScheduleSummaryTO SelectTblPurchaseScheduleSummaryDtlsTO(Int32 isPurchaseScheduleSummary, Int32 rootScheduleId, Int32 isGetQtyOfNewStatus = 0);
        TblPurchaseScheduleSummaryTO GetVehicleDetailsByScheduleIds(Int32 IdPurchaseScheduleSummary, Int32 statusId, Int32 vehiclePhaseId, Int32 rootScheduleId, Int32 isGetQtyOfNewStatus = 0);
        List<TblPurchaseScheduleSummaryTO> GetAllVehicleListForGrading(String statusId, DateTime fromDate, DateTime toDate, Int32 loggedInUserId, Int32 idPurchaseScheduleSummary, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> GetAllVehicleListForRecovery(String statusId, Int32 loggedInUserId, DateTime fromDate, DateTime toDate, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> GetAllVehicleListForUnloading(String statusId, Int32 loggedInUserId, DateTime fromDate, DateTime toDate,int showList, Int32 idPurchaseScheduleSummary, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsList(String statusId, DateTime date);
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsList(DateTime fromDate, DateTime toDate, string statusId, Int32 idPurchaseScheduleSummary, Int32 rootScheduleId);
        DimStatusTO GetNextStatusTO(Int32 currentStatusId, List<DimStatusTO> dimStatusTOList, Int32 isNext);
        List<TblPurchaseScheduleSummaryTO> GetScheduleDetailsByPurchaseEnquiryId(Int32 enquiryPurchaseId);
        void GetSameProdItemsCombinedList(List<TblPurchaseScheduleSummaryTO> filterList,Boolean isConsiderRec);
        void GetSameProdItemsCombinedListForReportByItemName(List<TblPurchaseScheduleSummaryTO> filterList);
        void GetSameProdItemsCombinedListForReport(List<TblPurchaseScheduleSummaryTO> filterList);
        List<TblPurchaseScheduleSummaryTO> GetAllScheduleDetailsByPhaseForAllVehicle(int purchaseEnquiryId);
        List<TblPurchaseScheduleSummaryTO> GetAllScheduleDetailsByPhase(int purchaseEnquiryId);
        void AsignSeqNoForScheduleItemDts(List<TblPurchaseScheduleSummaryTO> scheduleTOList);
        ResultMessage UpdateApprovedWeighingDetails(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, int loginUserId);
        List<TblPurchaseScheduleSummaryTO> GetAllScheduleDetailsByPhaseAndVehicleID(int IdPurchaseScheduleSummary);
        void AsignVehiclePhaseForBothVehicle(List<TblPurchaseScheduleSummaryTO> scheduleTOList);
        List<TblPurchaseScheduleSummaryTO> GetAllScheduleDetailsByPhaseAndVehicleIDForApproval(int IdPurchaseScheduleSummary);
        List<TblPurchaseScheduleSummaryTO> GetVehPhaseWiseItemsList(Int32 vehiclePhase, Int32 statusId, List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOTempList);
        Boolean CheckIsVehicleGradingConfirmed(Int32 purchaseScheduleId);
        List<TblPurchaseScheduleSummaryTO> GetAllPurchaseScheduleSummaryForCommerAppr(Int32 approvalType, Int32 idPurchaseScheduleSummary);
        List<TblPurchaseScheduleSummaryTO> GetPurchaseScheduleSummaryTOByVehicleNo(String vehicleNo, Int32 actualRootScheduleId);
        int InsertTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        int InsertTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveVehicleScheduleDetails(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        ResultMessage AddReportedScheduleForSpotVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, DateTime currentDate);
        ResultMessage UpdateScheduleVehicleNoOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Int32 loginUserId);
        ResultMessage UpdateSpotEntryVehicleSupplier(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, Int32 loginUserId);

        ResultMessage CompeleteRecoveryAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        int UpdateTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        int UpdateVehicleTypeOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        int UpdateRejectedQtyDtlsAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateWeighingCompletedAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, int loginUserId, SqlConnection conn, SqlTransaction tran);

        ResultMessage UpdateWeighingCompleted(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, int loginUserId);
        int UpdateScheduleVehicleNoOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseMaterialSample(TblPurchaseMaterialSampleTO tblPurchaseMaterialSampleTO, TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, bool isSendNotification);
        int UpdateTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseScheduleSummaryStatusOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdatePurchaseScheduleCalculationDtls(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseScheduleSummaryCommercialApproval(Int32 rootScheduleId, Int32 CommercialApproval, Int32 CommercialVerified, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseScheduleSummary(Int32 idPurchaseScheduleSummary);
        int DeleteTblPurchaseScheduleSummary(Int32 idPurchaseScheduleSummary, SqlConnection conn, SqlTransaction tran);
        ResultMessage CheckIfVehicleScheduleAlreadyExits(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran, Int32 count);
        ResultMessage InsertMaterailItemDetails(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Boolean isItemChange, Boolean isSendNotification, DateTime currentdate);
        ResultMessage SaveScheduleRecoveryDtls(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblPurchaseScheduleSummaryTO gradingScheduleTO, TblPurchaseWeighingStageSummaryTO weighingStageSummaryTO, Boolean isItemChange, Boolean isSendNotification, DateTime currentdate, SqlConnection conn, SqlTransaction tran);
        ResultMessage CheckGradingAndRecoveryCompleted(TblPurchaseScheduleSummaryTO scheduleSummaryTO);
        ResultMessage saveData(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Boolean isItemChange, Boolean isSendNotification, DateTime serverDateTime,ref string padtaApprovalMsg, SqlConnection conn, SqlTransaction tran);
        ResultMessage NotifyStatusChangeAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Boolean isSendNotification, DateTime serverDateTime);
        ResultMessage SetStatusCompleteAfterCorrection(int purchaseEnquiryId,Int32 loginUserId,Int32 isAuto, SqlConnection conn, SqlTransaction tran, Int32 statusId, string saudaCloseRemark = null,Int32 rootScheduleId = 0);
        string GetVehicleStatus(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        TblPurchaseScheduleSummaryTO CheckVehiclePreviousStatus(Int32 CurrentStatusId, Int32 IdPurchaseScheduleSummary);
        int UpdatePreviousActiveStatus(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateItemDetails(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Boolean isItemChange, DateTime currentdate, SqlConnection conn, SqlTransaction tran);
        ResultMessage CalculateRecoveryItemDtls(TblPurchaseScheduleSummaryTO recoveryScheduleTO, TblPurchaseWeighingStageSummaryTO weighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        Boolean CheckIsRecoveryAndGradingCompleted(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, ref TblPurchaseScheduleSummaryTO gradingScheduleTO,Boolean isTakeGradingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage CalculateExpressionAgaintGrading(TblPurchaseScheduleSummaryTO recoveryScheduleTO, TblPurchaseScheduleSummaryTO tempUnloadingCompleted, TblPurchaseWeighingStageSummaryTO weighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleDetailsTO> ConvertGradingListToScheduleItemList(List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList);
        ResultMessage CalculateGradingItemsDtls(TblPurchaseScheduleSummaryTO gradingScheduleTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateVehicleScheduleDetails(TblPurchaseScheduleSummaryTO previousPurchaseScheduleSummaryTO, TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, string statusId, string spotEntryVehicleStatusId, DateTime currentDate);
        ResultMessage InsertVehicleScheduleDetails(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, string spotEntryVehicleStatusId);
        ResultMessage MarkUnloadingCompleteWithTareWtDtls(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
        ResultMessage checkIfQtyGoesOutofBand(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew, TblPurchaseEnquiryTO enquiryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteVehicleScheduleDetails(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Boolean getScheduleDetails, Boolean isSetPreviousStatus);
        ResultMessage CalculateItemDetails(List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList, TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran);
        double GetTransportAmountDtls(double qty, double freight, Int32 isFixed);
        void AsignTransportAmtToItemDtlsList(List<TblPurchaseVehicleDetailsTO> scheduleItemDtlsList, double transportAmtPerMT);
        double GetGradeExpressionDtls(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsLocalTO, List<TblExpressionDtlsTO> ExpressionDtlsTOList, string keyStr, string keyValStr, out Double includeInMetalCostExpValue, Int32 cOrNcId,double maxRecVal,List<TblVariablesTO> tblVariablesTOList);
        void GetVaribleList(SqlConnection conn, SqlTransaction tran, ref string keyStr, ref string keyValStr,ref double maxRecVal, TblConfigParamsTO maxRecValConfigTO,ref List<TblVariablesTO> tblVariablesTOList);
        double CalculateGradeExpression(string expStr, string keyStr, string keyValStr);
        double CalculateItemsMetalCost(List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList, TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        void AsignSignleGradePadtaVal(double singleGradePadta, List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList);
        int UpdateScheduleDtls(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage CheckIfAllQualityFlagsAreCompleted(int RootScheduleId, int phaseId);
        List<TblPurchaseScheduleSummaryTO> getListofShcheduleSummary(string fromDate, string toDate, String purchaseManagerIds);
        List<TblPurchaseScheduleSummaryTO> getListofShcheduleSummaryForReport(DateTime fromDate, DateTime toDate,String purchaseManagerIds,Int32 isConsiderTm = 0);
        //List<TblPurchaseScheduleSummaryTO> getListofShcheduleSummaryForReportForDropbox(string vehicleIds, int cOrNcId);
        List<TblPurchaseScheduleSummaryTO> getListofShcheduleSummary(Int32 rootScheduleId);
        ResultMessage SaveScheduleDtlsAsPerCorNC(TblPurchaseScheduleSummaryTO scheduleTO, Boolean isProcess, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdatePurchaseInvoiceGradeDtls(List<TblPurchaseScheduleSummaryTO> scheduleUpdateList, List<TblPurchaseScheduleSummaryTO> scheduleInsertList, SqlConnection conn, SqlTransaction tran);
        ResultMessage CalculateGradeExpressionDtls(TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran);
        void CalculateRateAsPerCorNC(TblPurchaseScheduleSummaryTO scheduleTO, TblPurchaseEnquiryTO enquiryTO);

        void CalculateRateAsPerCorNCForEnquiry(TblPurchaseEnquiryTO enquiryTO);
        ResultMessage AdjustPurchaseInvoiceQty(TblPurchaseScheduleSummaryTO scheduleTO, double totalInvoiceQty, TblPurchaseEnquiryTO enquiryTO, TblBaseItemMetalCostTO tblBaseItemMetalCostTO,Boolean isForBRM, SqlConnection conn, SqlTransaction tran);
        double GetPurchaseInvoiceQty(List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDtlsList);

        void GetBaseItemCostAsPerCOrNC(List<TblBaseItemMetalCostTO> tblBaseItemMetalCostTOList,TblBaseItemMetalCostTO tblBaseItemMetalCostTO);
        ResultMessage UpdateScheduleDtls(List<TblPurchaseScheduleSummaryTO> scheduleUpdateList, SqlConnection conn, SqlTransaction tran);
        ResultMessage InsertScheduleDtls(List<TblPurchaseScheduleSummaryTO> scheduleUpdateList, SqlConnection conn, SqlTransaction tran);
        ResultMessage GetCombinedvehicleItemDtlsForCAndNC(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        List<VehicleStatusDateTO> GetAllVehicleStatusDateTOList(DateTime fromDate, DateTime toDate, string pmUserId, Int32 vehicleFilterId);

        List<VehicleStatusDateTO> GetAllVehicleTrackingDtlsList(DateTime fromDate, DateTime toDate, string pmUserId, Int32 vehicleFilterId,Int32 isPrintExcelReport);

        List<VehicleStatusDateTO> SelectAllVehicleTrackingDtls(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO);
        ResultMessage DataExtractionForCorrectionCompleVehicles();
        ResultMessage DeleteAllDataAgainstRootScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeleteVehiclePhotoDtls(Int32 rootScheduleId, List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeleteFileFromAzure(List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList, SqlConnection conn, SqlTransaction tran);
        Task<int> DeleteAzureFiles(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, CloudBlobContainer sourceContainer);
        ResultMessage InsertNCScheduleAfterDataExtraction(TblPurchaseScheduleSummaryTO insertScheduleTOForNC, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeleteAllVehicleDtlsAgainstRootScheduleId(TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeleteAllDataAgainstVehicleSchedule(TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran);

        ResultMessage MarkVehicleOut(TblPurchaseScheduleSummaryTO scheduleTO,Int32 loginUserId);

        ResultMessage SubmitProcessCharge(TblPurchaseScheduleSummaryTO scheduleTO);

        ResultMessage PostVehicleOutDtls(TblPurchaseScheduleSummaryTO scheduleSummaryTO,Int32 loginUserId);
        ResultMessage MarkVehicleRejected(TblPurchaseScheduleSummaryTO scheduleSummaryTO, Int32 loginUserId, bool fromApprovalScreen);

        ResultMessage InsertWeighingDetails(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryDetailsDB(Int32 idSchedulePurchaseSummary, Boolean isActive);
        List<DropDownTO> GetBaseMetalCostList(Int32 rootScheduleId);

        List<TblPurchaseScheduleSummaryTO> CompareVehItemDtlsWithCurrentOrUnloladingDate(List<TblPurchaseScheduleSummaryTO> scheduleToList, DropDownTO baseItemCostE);

        ResultMessage CalculateVehItemDtlsWithCurrentOrUnloladingDate(List<TblPurchaseScheduleSummaryTO> scheduleToList, DropDownTO dropDownTO,TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn,SqlTransaction tran);

        List<DropDownTO> SelectAllSystemUsersFromRoleTypeWithVehAllocation(Int32 roleTypeId, Int32 nameWithCount);

        ResultMessage MigrateBaseMetalCostDtls();

        List<TblPurchaseScheduleSummaryTO> GetSudharSaudaReportDtls(int idPurchaseScheduleSummary);

        ResultMessage CheckVehScheQtyMatchWithVehGradesQty(TblPurchaseScheduleSummaryTO scheduleTO);

        ResultMessage CompleteSaudaStatusAndAddConsumptionEntry(TblPurchaseEnquiryTO enquiryTO, double diff, Int32 isAuto, Int32 loginUserId, SqlConnection conn, SqlTransaction tran);

        //Dictionary<Int32, Int32> GetTodaysAllocatedVehiclesCnt(Int32 roleTypeId, DateTime serverDate);

        ResultMessage SendNotificationsForSaudaCloseApproval(TblPurchaseEnquiryTO enquiryTO, Boolean isForPendForApproval, Int32 isApprove, Int32 loginUserId);

        ResultMessage MarkVehicleRequested(List<TblPurchaseScheduleSummaryTO> scheduleList,Int32 loginUserId);

        ResultMessage SaveVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, Int32 loginUserId);
        int UpdateIsActiveOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
       


        ResultMessage WriteIotDataTODB();
        ResultMessage UpdateVehicleOutFlagForIOT();


        int PostTallyReportListForExcel(List<TallyReportTO> tallyReportList);


        ResultMessage UpdateMaterialTypeOfSauda(TblPurchaseScheduleSummaryTO scheduleSummaryTO, Int32 loginUserId,Boolean isUpdateMaterialType);
        int UpdateCorNcIdOfVehicle(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        TblPurchaseScheduleSummaryTO GetVehTotalQtyDashboardInfo(string loginUserId = "");
        ResultMessage PostVehicleFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, Int32 loginUserId);

        ResultMessage LinkVehicleToExistingSauda(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO);

        ResultMessage LinkVehicleToExistingSaudaList(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        ResultMessage PostScheduleTcDtls(List<TblPurchaseSchTcDtlsTO> tblPurchaseSchTcDtlsTOList,Int32 loginUserId);
        ResultMessage UpdateSupervisorId(int loginUserId, TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);

        List<DropDownTO> SelectAllVehAddOnFunDtls(Int32 rootscheduleId);
        List<TblPurchaseScheduleSummaryTO> UnloadingRateScrapQuery(DateTime fromDate, DateTime toDate);
        ResultMessage UpdateSupplier(TblPurchaseScheduleSummaryTO scheduleSummaryTO, Int32 supplierId);

        ResultMessage UpdateWholeScheduleAsCorNC(TblPurchaseScheduleSummaryTO schduleSummaryTO, TblPurchaseEnquiryTO enquiryTO, Boolean isUpdateSaudaDtlsAsPerCorNC, Boolean isUpdateAllSchOfEnq,Boolean isUpdateOnlyGradingDtls, SqlConnection conn, SqlTransaction tran);

        List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummaryTOListFromStatusIds(String statusId);

        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryTOByModBusRefId(Int32 modbusRefId);

         double GetWeighingStageWiseSumOfImpurities(Int32 rootSheduleId, string impuritiesStr);

        //Saket [2020-05-23] Added for optimization
        List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleDBBackUp(Int32 statusId, Int32 isDBBackUp);
        ResultMessage UpdateDensityAndVehicleType(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, int loginUserId);

        void CalculateRateAsPerCorNCForSaudaConversion(TblPurchaseScheduleSummaryTO scheduleTO, List<TblPurchaseEnquiryTO> enquiryTO);
        ResultMessage UpdateIsFreightDetailsAdded(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran);
       // void CalculateRateAsPerCorNCForSaudaConversion(TblPurchaseScheduleSummaryTO scheduleTO, TblPurchaseEnquiryTO enquiryTO);
        int SavePartyWeightDtls(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, int vehicleSpotEntryId, int loginUserId, SqlConnection conn, SqlTransaction tran);

        List<TblPurchaseScheduleSummaryTO> getListofShcheduleSummaryForDropbox(string vehicleIds, int cOrNcId);

        List<TallyReportTO> GetListOfTallyReport(string fromDate, string toDate, int ConfirmTypeId, int supplierId, String purchaseManagerIds, int materialTypeId, Boolean isForNewTallyReport, string vehicleIds, Int32 isConsiderTm = 0);

        ResultMessage PrintTallyReport(string fromDate, string toDate, int ConfirmTypeId, int supplierId, String purchaseManagerIds, int materialTypeId, Boolean isForNewTallyReport, string vehicleIds);

        //Added by minal 02 May 2021
        ResultMessage DataFromExportToExcel();
        List<TblSpotEntryContainerDtlsTO> SelectContainerDetailsBySpotEntryId(int spotEntryId);
    }
}