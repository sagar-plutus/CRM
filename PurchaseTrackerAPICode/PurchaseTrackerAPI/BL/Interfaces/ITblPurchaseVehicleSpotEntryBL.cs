using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehicleSpotEntryBL
    {
        List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnquiryVehicleEntryTO(Int32 purchaseEnquiryId);
        TblPurchaseVehicleSpotEntryTO SelectSpotVehicleAgainstScheduleId(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnqVehEntryTOList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnqVehEntryTOList(Int32 purchaseEnquiryId);
        List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntry();
        List<VehicleNumber> SelectAllVehicles();
        TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTO(Int32 idVehicleSpotEntry);
        TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTO(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran);
        TblPurchaseVehicleSpotEntryTO GetSpotEntryVehicleDetailsWithMaterials(Int32 idVehicleSpotEntry);
        // ResultMessage SaveVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, Int32 loginUserId);
        List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehicles(DateTime fromDate, DateTime toDate, String loginUserId, Int32 id, bool skipDatetime);
        List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehiclesPending(string vehicleNo);
        int InsertTblRecycleDocuments(TblRecycleDocumentTO tblIssueDocumentsTO);
        int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO);
        int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO);
        int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry);
        int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateBookingDtlsForSpotVehicle(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, Boolean isForRevertLink);
        DropDownTO SelectAllSpotEntryVehiclesCount(int pmId, int supplierId, int materialTypeId);
        TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTOByRootId(int idPurchaseScheduleSummary);
        List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotVehCountForDashboard(String pmId, Int32 supplierId, Int32 materialTypeId);
    }
}