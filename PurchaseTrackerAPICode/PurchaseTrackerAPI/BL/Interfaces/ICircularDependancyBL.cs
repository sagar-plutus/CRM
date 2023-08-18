using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ICircularDependancyBL
    {


        //purchaseEnquiry
        TblPurchaseEnquiryTO SelectTblBookingsTO(Int32 idBooking, SqlConnection conn, SqlTransaction tran);
        TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId);
        int UpdateTblBookingsForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);

         TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        Int32 UpdatePendingBookingQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, Boolean isCheckForExistingQty, TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran);

        TblPurchaseEnquiryTO SelectTblBookingsForPurchaseTO(Int32 idPurchaseEnquiry, SqlConnection conn, SqlTransaction tran);
        //end
        List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummaryTOByRootScheduleID(Int32 RootScheduleId, Boolean isActive);

        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId,Int32 isGradingBeforeUnld=0);


        List<TblPurchaseScheduleSummaryTO> GetPurchaseScheduleSummaryTOByVehicleNo(String vehicleNo, Int32 actualRootScheduleId);

         List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId);
         List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId, SqlConnection conn, SqlTransaction tran);


         TblPurchaseScheduleSummaryTO GetVehicleDetailsByScheduleIds(Int32 IdPurchaseScheduleSummary, Int32 statusId, Int32 vehiclePhaseId, Int32 rootScheduleId);
         TblPurchaseScheduleSummaryTO SelectTblPurchaseScheduleSummaryDtlsTO(Int32 isPurchaseScheduleSummary, Int32 rootScheduleId);
         int UpdateTblPurchaseScheduleSummary(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);

        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByRootScheduleID(Int32 RootScheduleId, Boolean isActive, SqlConnection conn, SqlTransaction tran);

        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 parentScheduleId);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTOByParentScheduleId(Int32 parentScheduleId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseScheduleSummaryTO SelectAllEnquiryScheduleSummaryTO(Int32 idPurchaseScheduleSummary, Boolean isActive, SqlConnection conn, SqlTransaction tran);

        int InsertTblPurchaseEnquiry(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);

        Int32 SelectMaxEnquiryNo(Int32 finYear, SqlConnection conn, SqlTransaction tran);

        List<TblRateBandDeclarationPurchaseTO> SelectAllTblRateBandDeclarationPurchase(Int32 globalRatePurchaseId);
        //List<TblRateBandDeclarationPurchaseTO> SelectLatestRateBandDeclarationPurchaseTOList(Int32 cnfId, DateTime date);

        List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId,SqlConnection conn = null,SqlTransaction tran = null);

        ResultMessage CreatePurchaseInvoicePOWithGRR(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);

        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId, SqlConnection conn, SqlTransaction tran);
        int UpdateMaterialTypeOfSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);

        List<TblRateBandDeclarationPurchaseTO> GetRateDeclartionDtlsWhileBooking(Int32 cnfId);

        int UpdateIsGradingWhileUnloadingFlag(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, SqlConnection conn, SqlTransaction tran);

        ResultMessage PrintNCVehicalReport(List<TallyReportTO> tallyReportList);

        int UpdateEnquiryPendingBookingQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);

        List<TblPurchaseEnquiryTO> SelectSaudaListBySaudaIds(String saudaIds);
    }
}
