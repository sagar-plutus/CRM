using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehicleDetailsDAO
    {

         List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsList(DateTime fromDate, String UserId);
        String SqlSelectQuery();
        String SqlSelectQueryNew();
        List<TblPurchaseVehicleDetailsTO> GetVehicleDetailsByEnquiryId(Int32 purchaseEnquiryId);
        List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDetailsList(Int32 schedulePurchaseId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDetailsList(Int32 schedulePurchaseId);
        List<TblPurchaseVehicleDetailsTO> SelectVehicleItemDetailsByScheduleSummaryIds(string schedulePurchaseIds);
        List<TblPurchaseVehicleDetailsTO> SelectVehicleItemDetailsByScheduleSummaryIds(string schedulePurchaseIds, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleDetailsTO> GetPurchaseScheduleDetailsBySpotEntryVehicleId(Int32 spoEntryVehicleId);
        List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsList(String userid, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleDetailsTO> ConvertDTToListNew(SqlDataReader tblPurchaseVehicleDetailsTODT);
        List<TblPurchaseVehicleDetailsTO> ConvertDTToList(SqlDataReader tblPurchaseVehicleDetailsTODT, Boolean isReadAll = true);
        List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsByStatus(string statusId, DateTime fromDate, String vehicleNo, SqlConnection conn, SqlTransaction tran);
          List<TblPurchaseVehicleDetailsTO> ConvertDTToListOld(SqlDataReader tblLoadingTODT);
        int InsertTblPurchaseVehicleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran);
        int SaveVehicleSpotEntry(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlCommand cmdInsert);
        int ExecuteInsertionCommandNew(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseScheduleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO);
        int UpdateTblPurchaseScheduleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseVehicleDetails(Int32 idSchedulePurchase, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehicleDetails2(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehicleDetails(Int32 idSchedulePurchase);
        int ExecuteDeletionCommand(Int32 idSchedulePurchase, SqlCommand cmdDelete);
        int ExecuteDeletionCommand2(Int32 purchaseScheduleSummaryId, SqlCommand cmdDelete);
        int DeleteAllVehicleItemDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

    }
}