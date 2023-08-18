using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DashboardModels;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryBL
    {
         TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
         TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId);
        List<TblRecycleDocumentTO> SelectAllDocumentIdFromSpotEntryId(int transId, int transTypeId);
         List<TblPurchaseEnquiryTO> SelectTblPurchaseEnquirySpotEntryTO(Int32 spotEntryId);
        TblPurchaseEnquiryTO SelectTblPurchaseEnquiry(Int32 idPurchaseEnquiry);
        List<TblPurchaseEnquiryTO> GetEnquiryItemDtlsFromBookingIds(string saudaIds);
        List<TblPurchaseEnquiryTO> SelectAllBookingsListForAcceptance(String cnfId, TblUserRoleTO tblUserRoleTO,Int32 isGetPendSaudaToClose, Int32 IsOrderOREnq);
        TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 userId, Int32 supplierId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran);
         List<TblPurchaseEnquiryTO> GetAllEnquiryList(Int32 SpotedId, String userId, Int32 organizationId, Int32 statusId, DateTime fromDate, DateTime toDate, Int32 isConvertToSauda, Int32 isPending,Int32 materialTypeId,string cOrNcId, Int32 isSkipDateFilter);
        List<TblPurchaseEnquiryTO> GetSupplierWithMaterialHistList(Int32 supplierId, Int32 lastNRecords);
        List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId);
        TblPurchaseEnquiryTO SelectTblBookingsForPurchaseTO(Int32 idPurchaseEnquiry, SqlConnection conn, SqlTransaction tran);
        List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE);
        Int32 SelectMaxEnquiryNo(Int32 finYear, SqlConnection conn, SqlTransaction tran);
        TblPurchaseEnquiryTO SelectTblBookingsTO(Int32 idBooking, SqlConnection conn, SqlTransaction tran);
        BookingInfo SelectBookingDashboardInfo(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date);
        List<BookingInfo> SelectMaterialWiseEnqOrSaudaInfoForDashboard(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date, Int32 isConvertToSauda);
        BookingInfo SelectBookingSaudaDashboardInfo(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date);
        BookingInfo SelectTodayRtaeDashboardInfo(TblUserRoleTO tblUserRoleTO, Int32 orgId, DateTime date);
        ResultMessage SaveNewPurchaseEnquiry(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO);
        ResultMessage DisplayEnquiryResultMsg(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, Dictionary<string, object> enquiryDtlsDict, TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO);
        Boolean CheckIfGradeWithinTargetQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, List<TblPurchaseEnquiryDetailsTO> tblPurchaseEnquiryDetailsTOList, List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyUpdateTOList, List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyInsertTOList, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryTO> SelectAllPurchaseEnquiryForPM(Int32 userId, Int32 rateBandPurchaseId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseEnquiry(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveVehicleDetailsBookingForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO);
        int SavePurchaseScheduleDetails(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        ResultMessage UpdateBookingForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO);
        ResultMessage SavePurchaseEnquiryDtls(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateBookingForSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO);
        int UpdateTblBookingsForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage CloseOpenQtySauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO);
        Int32 UpdatePendingBookingQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, Boolean isCheckForExistingQty, TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBookings(TblPurchaseEnquiryTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblBookingHistory(TblPurchaseEnquiryTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateBookingConfirmations(TblPurchaseEnquiryTO tblBookingsTO);
        ResultMessage CloseSaudaManually(TblPurchaseEnquiryTO tblPurchaseEnquiryTO);
        List<TblPurchaseEnquiryTO> SelectAllTodaysBookingsWithOpeningBalance(int cnfOrgId, int dealerOrgId, DateTime serverDate);
        List<TblPurchaseEnquiryTO> SelectAllPendingBookingsList(int cnfOrgId, int dealerOrgId, DateTime date, string v1, bool v2);
        ResultMessage ApproveRejectCloseSauda(TblPurchaseEnquiryTO enquiryTO, Int32 isApproveOrReject,Int32 loginUserId);
        ResultMessage ApproveRejectCloseSaudaDtls(TblPurchaseEnquiryTO enquiryTO, Int32 isApproveOrReject,Int32 loginUserId, SqlConnection conn, SqlTransaction tran);
        ResultMessage AddUpdateShipmentDetails(List<TblpurchaseEnqShipmemtDtlsTO> tblpurchaseEnqShipmemtDtlsTO, Int32 loginUserId);
        List<TblpurchaseEnqShipmemtDtlsTO> GetShipmentDetailsByPurchaseEnquiryId(int purchaseEnquiryId);
        ResultMessage PrintShipmentReport(int purchaseEnquiryId);
        List<TblPurchaseEnquiryTO> GetMaterialTypeWiseTotalPendingQty(Int32 SpotedId, String userId, Int32 organizationId, Int32 statusId, Int32 isConvertToSauda, Int32 isPending, Int32 materialTypeId, string cOrNcId, DateTime fromDate, DateTime toDate, Int32 isSkipDateFilter = 0);
    }
}