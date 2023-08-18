using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseScheduleSummaryDAO
    {
        String SqlSelectQuery();
        String SqlSelectQueryNew();
        String SelectQuery();
        String SelectQueryForPendingQualityFlag();
        String SelectQueryForApprovals();
        List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummary();
        List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummaryForCommercialApp(string ignoreStatusIds, Int32 approvalType, Int32 idPurchaseScheduleSummary,Int32 MaterialTypeId);
        List<TblPurchaseScheduleSummaryTO> SelectPurchaseScheduleSummaryTOByVehicleNo(String vehicleNo, Int32 actualRootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummary(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 idPurchaseScheduleSummary, Boolean isActive, SqlConnection conn, SqlTransaction tran);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByRootID(Int32 RootScheduleId, Boolean isActive, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootID(Int32 RootScheduleId, Boolean isActive);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 parentScheduleId);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByParentScheduleId(Int32 parentScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootId(Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootId(Int32 rootScheduleId, SqlConnection con, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId, SqlConnection con, SqlTransaction tran);
        Int32 SelectVehicleScheduleByRootAndStatusIdCount(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId, SqlConnection con, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, Int32 statusId, Int32 VehiclePhaseId);

        List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleByRootAndStatusId(Int32 rootScheduleId, string statusIds, string vehiclePhaseIds);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryDtlsByEnquiryId(Int32 purchaseEnquiryId);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryDtlsByEnquiryId(Int32 purchaseEnquiryId, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryDtlsByEnquiryId(Int32 purchaseEnquiryId, Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummary(Int32 purchaseEnquiryId);
        List<TblPurchaseScheduleSummaryTO> SelectEnquiryScheduleSummary(Int32 purchaseEnquiryId);
        List<TblPurchaseScheduleSummaryTO> SelectSuperwiserFromTblPurchaseScheduleSummary(int statusId, string roleIds);
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryDetails(Int32 idSchedulePurchaseSummary, Boolean isActive);
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId);


        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForPurchaseEnquiry(Int32 purchaseEnquiryId);
        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForGradeNote(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO,String purchaseManagerIds);

        //Added by minal 26 May 2021 For Dropbox
        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForGradeNoteForDropbox(string vehicleIds, int cOrNcId);
        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForMasterReportForDropbox(string vehicleIds, int cOrNcId);
        //Added by minal

        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForMasterReport(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO,String purchaseManagerIds);

        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsList(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO);
        List<VehicleStatusDateTO> SelectAllVehicleTrackingDtls(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO);
        List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListByDate(DateTime scheduleDate);
        List<TblPurchaseScheduleSummaryTO> GetVehicleListForPendingQualityFlags(string pmUserId);
        List<TblPurchaseScheduleSummaryTO> SelectVehicleListForAllCommonApprovals(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO);
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailList(String statusId, Int32 loggedInUserId, DateTime date);

        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsListPhasewise(DateTime fromDate, DateTime toDate, String userId, Int32 rootScheduleId, string showListE, string ignoreStatusIds);

        List<TblPurchaseScheduleSummaryTO> GetAllCorrectionCompletedVeh(DateTime fromDate, DateTime toDate);
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsListPhasewiseForComp(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO);
        string GetDisplayNameFromUserID(int createdBy);
        int SelectNextStatusOfCurrentStatus(int statusId);


        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsList(String vehicleNo, String statusId, Int32 idPurchaseScheduleSummary);

        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId, string statusIds);
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleDtlsByRootScheduleId(Int32 rootScheduleId, string statusIds, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleSummaryTO> SelectAllCorrectionCompleVehicles(DateTime toDate, Int32 cOrNcId);

        List<TblPurchaseScheduleSummaryTO> UnloadingRateScrapQuery(DateTime fromDate, DateTime toDate);
        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryDetailsList(Int32 idSchedulePurchaseSummary, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailListForRecovery(DateTime fromDate, DateTime toDate, string statusId, Int32 loggedInUserId, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailList(DateTime fromDate, DateTime toDate, string statusId, Int32 loggedInUserId, Int32 ShowList, Int32 idPurchaseScheduleSummary, Int32 rootScheduleId);
        List<TblPurchaseScheduleSummaryTO> ConvertDTToList2(SqlDataReader tblPurchaseScheduleSummaryTODT);
        List<TblPurchaseScheduleSummaryTO> ConvertDTToList(SqlDataReader tblPurchaseScheduleSummaryTODT);
        List<TblPurchaseScheduleSummaryTO> ConvertDTToListForPendingFlags(SqlDataReader tblPurchaseScheduleSummaryTODT);
        List<TblPurchaseScheduleSummaryTO> ConvertDTToListForApprovals(SqlDataReader tblPurchaseScheduleSummaryTODT);
        int InsertTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        int InsertTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        int UpdateVehicleTypeOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        int UpdateWeighingCompletedAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateRejectedQtyDtlsAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateScheduleVehicleNoOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateSpotEntryVehicleSupplier(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateScheduleVehicleNoOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate);
        int UpdateTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseScheduleSummaryStatusOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateCorrectionCompletedFlag(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateGradingCompletedOn(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateIsVehicleOut(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseScheduleSummaryCommercialApproval(Int32 rootScheduleId, Int32 CommercialApproval, Int32 CommercialVerified, SqlConnection conn, SqlTransaction tran);

        int SelectScheduledVehiclesAgainstEnquiry(int purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        int UpdateModbusRefPurchaseSchedule(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        int UpdatePurchaseScheduleCalculationDtls(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate);
        int ExecuteUpdationCommandStatusOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate);
        int UpdateCorrectionCompletedFlag(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlCommand cmdUpdate);
        int UpdateScheduleSupplier(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);

        int UpdateVehProcessCharge(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        
        int DeleteTblPurchaseScheduleSummary(Int32 idPurchaseScheduleSummary);
        int DeleteTblPurchaseScheduleSummary(Int32 idPurchaseScheduleSummary, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchaseScheduleSummary, SqlCommand cmdDelete);
        int UpdateStatusWeighingCompletedAgainstVehicle(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);

        Dictionary<Int32, Int32> GetAllocatedVehiclesAgainstRole(Int32 roleTypeId, Boolean getAllAlocatedVehicles);

        Dictionary<Int32, Int32> GetTodaysAllocatedVehiclesCnt(Int32 roleTypeId, DateTime serverDate);

        int UpdateCorNcIdOfVehicle(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateParentScheduleIdToNUll(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateCorNcIdForBothVehicle(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateScheduleVehicleQtyOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO,SqlConnection conn,SqlTransaction tran);
        List<DropDownTO> getListofPhasesUsedForUnloadingQty();

        int UpdateIsActiveOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateIsGradingWhileUnloadingFlag(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);

        List<TblPurchaseScheduleSummaryTO> SelectTblPurchaseScheduleSummaryTOByModBusRefId(Int32 modbusRefId);

        List<TblPurchaseScheduleSummaryTO> SelectAllTblPurchaseScheduleSummaryTOListFromStatusIds(String statusId);

        //Saket [2020-05-23] Added for optimization
        List<TblPurchaseScheduleSummaryTO> SelectVehicleScheduleDBBackUp(Int32 statusId, Int32 isDBBackUp);
        int updateDensityAndVehicleTypeForCurrentPhase(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);

        int UpdateScheduleEnquiryIdOnly(Int32 rootScheduleId,Int32 purchaseEnqId, SqlConnection conn, SqlTransaction tran);

        //Error : 'TblPurchaseScheduleSummaryDAO' does not implement interface member 'ITblPurchaseScheduleSummaryDAO.UpdateScheduleEnquiryIdOnly(TblPurchaseVehicleSpotEntryTO, SqlConnection, SqlTransaction)'
        //Comment added by minal 28 April 2021
        // int UpdateScheduleEnquiryIdOnly(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran);

        int UpdateIsFreightDetailsAdded(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateScheduleEnqQtyOnly(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
    }

}