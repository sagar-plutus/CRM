using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Data;
using PurchaseTrackerAPI.DashboardModels;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryDAO
    {

        List<TblRecycleDocumentTO> SelectAllDocumentIdFromSpotEntryId(int transId, int transTypeId);
        List<TblPurchaseEnquiryTO> GetAllEnquiryList(String userId, Int32 organizationId, Int32 statusId, DateTime fromDate, DateTime toDate, Int32 isConvertToSauda, Int32 isPending, Int32 materialTypeId,string cOrNcId,Int32 isSkipDateFilter);//, TblUserRoleTO tblUserRoleTO)
        String SqlSelectQuery();
          TblPurchaseEnquiryTO SelectTblPurchaseEnquiry(Int32 idPurchaseEnquiry);
        TblPurchaseEnquiryTO SelectTblBookingsForPurchase(Int32 idPurchaseEnquiry, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryTO> SelectAllPurchaseEnquiryForPM(Int32 userId, Int32 rateBandPurchaseId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryTO> GetSupplierWithMaterialHistList(Int32 supplierId, Int32 lastNRecords);
        List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId);
        TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId);
        TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        List<TblRecycleDocumentTO> ConvertDTRecycleDocumentToList(SqlDataReader tblRecycleDocumentsTODT);
        TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 userId, Int32 supplierId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryTO> SelectTblPurchaseEnquirySpotEntryTO(Int32 spotEntryId);
        List<TblOrganizationTO> SelectAllTblOrganization(Constants.OrgTypeE orgTypeE);
        List<TblPurchaseEnquiryTO> SelectAllBookingsListForAcceptance(String cnfId, TblUserRoleTO tblUserRoleTO,Int32 isGetPendSaudaToClose, Int32 IsOrderOREnq);
        List<TblPurchaseEnquiryTO> SelectSaudaListBySaudaIds(string saudaIds);
        Int32 SelectMaxEnquiryNo(Int32 finYear, SqlConnection conn, SqlTransaction tran);
        List<TblOrganizationTO> ConvertDTToListNew(SqlDataReader tblOrganizationTODT);
        List<TblPurchaseEnquiryTO> ConvertDTToList(SqlDataReader tblPurchaseEnquiryTODT, Boolean isPMDisplay = true);
        TblPurchaseEnquiryTO SelectTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran);
        TblPurchaseEnquiryTO SelectTblBookings(Int32 idBooking);
        BookingInfo SelectBookingDashboardInfo(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date);

        List<BookingInfo> SelectMaterialWiseEnqOrSaudaInfoForDashboard(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date, Int32 isConvertToSauda);
        BookingInfo SelectBookingSaudaDashboardInfo(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date);
        BookingInfo SelectTodayRtaeDashboardInfo(TblUserRoleTO tblUserRoleTO, int orgId, DateTime date);
        int InsertTblPurchaseEnquiry(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdInsert);
        int ExecuteInsertionCommandForHistory(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdInsert);
        int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingActionsTO tblBookingActionsTO, SqlCommand cmdInsert);
        int UpdateTblBookingsForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBookingsForConverToSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO);
        int ExecuteUpdationCommand(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdUpdate);
        int ExecuteUpdationCommandForSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdUpdate);
        int DeactivateAllDeclaredQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBookings(TblPurchaseEnquiryTO tblBookingsTO);
        int UpdateTblBookings(TblPurchaseEnquiryTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommandForHistory(TblPurchaseEnquiryTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        double SelectParityForCAndNC(Int32 StateId,bool CorNC, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryTO> SelectAllTodaysBookingsWithOpeningBalance(int cnfOrgId, int dealerOrgId, DateTime serverDate);
        List<TblPurchaseEnquiryTO> SelectAllPendingBookingsList(int cnfOrgId, int dealerOrgId, DateTime date, string v1, bool v2);

        List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId,SqlConnection conn,SqlTransaction tran);

        int UpdateMaterialTypeOfSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);

        List<TblPurchaseEnquiryTO> SelectPurchaseEnquiryRateWise(String globalRatePurchaseIds);

        int UpdateEnquiryPendingBookingQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);

        int UpdatePendingNoOfVehicles(TblPurchaseEnquiryTO enquiryTO, int pendNoOfVeh, SqlConnection conn, SqlTransaction tran);
    }
}