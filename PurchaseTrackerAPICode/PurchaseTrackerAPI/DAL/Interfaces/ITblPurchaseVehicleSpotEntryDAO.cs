using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehicleSpotEntryDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehicles(DateTime fromDate, DateTime toDate, String loginUserId, Int32 id, bool skipDatetime); //, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntry();
        List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnquiryVehicleEntryTO(Int32 purchaseEnquiryId);
        List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnqVehEntryTOList(Int32 purchaseEnquiryId);
        List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnqVehEntryTOList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry);
        List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran);
        TblPurchaseVehicleSpotEntryTO SelectSpotVehicleAgainstScheduleId(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntry(SqlConnection conn, SqlTransaction tran);
        List<VehicleNumber> SelectAllVehicles();
        List<TblPurchaseVehicleSpotEntryTO> ConvertDTToList(SqlDataReader tblPurchaseVehicleSpotEntryTODT);
        List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehiclesPending(string vehicleNo);
        int InsertTblRecyleDocuments(TblRecycleDocumentTO tblRecycleDocumentTO);
        int ExecuteInsertionCommandDocs(TblRecycleDocumentTO tblRecycleDocumentsTO, SqlCommand cmdInsert);
        int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO);
        int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO);
        int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry);
        int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVehicleSpotEntry, SqlCommand cmdDelete);
        DropDownTO SelectAllSpotEntryVehiclesCount(int pmId, int supplierId, int statusId);
        TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTOByRootId(Int32 idVehicleSpotEntry);
        List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotVehCountForDashboard(String pmId, Int32 supplierId, Int32 materialTypeId);
    }
}