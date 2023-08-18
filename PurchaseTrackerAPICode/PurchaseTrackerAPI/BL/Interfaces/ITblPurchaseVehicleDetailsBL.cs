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
    public interface ITblPurchaseVehicleDetailsBL
    {
        List<TblPurchaseVehicleDetailsTO> GetVehicleDetailsByEnquiryId(Int32 purchaseEnquiryId);
        List<TblPurchaseVehicleDetailsTO> GetGradeExpressionDetails(List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList);
        List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDetailsList(Int32 schedulePurchaseId, Boolean isGetGradeExpDtls, SqlConnection conn = null, SqlTransaction tran = null);
          List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDtlsList(Int32 schedulePurchaseId);
        List<TblPurchaseVehicleDetailsTO> GetPurchaseScheduleDetailsBySpotEntryVehicleId(Int32 spotEntryVehicleId);
        void SelectVehItemDtlsWithOrWithoutGradeExpDtls(List<TblPurchaseScheduleSummaryTO> scheduleTOList, Boolean isGetGradeExpDtls, SqlConnection conn = null, SqlTransaction tran = null);
        List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsByStatus(String statusId, DateTime fromDate, String vehicleNo);
        int InsertTblPurchaseVehicleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveVehicleSpotEntry(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO);
        int InsertVehicleSpotEntry(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseScheduleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO);
        int UpdateTblPurchaseScheduleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehicleDetails(Int32 idSchedulePurchase, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehicleDetails(Int32 idSchedulePurchase);
        int DeleteTblPurchaseVehicleDetailsByScheduleId(Int32 scheduleId, SqlConnection conn, SqlTransaction tran);
        int DeleteAllVehicleItemDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

    }
}