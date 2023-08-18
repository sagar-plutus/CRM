using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseScheduleStatusHistoryDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseScheduleStatusHistoryTO> SelectAllTblPurchaseScheduleStatusHistory();
        TblPurchaseScheduleStatusHistoryTO SelectTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId);
        List<TblPurchaseScheduleStatusHistoryTO> SelectTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId, bool getApprovedTO, bool isForApproval, Int32 StatusId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleStatusHistoryTO> SelectAllTblPurchaseScheduleStatusHistory(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseScheduleStatusHistoryTO> ConvertDTToList(SqlDataReader tblPurchaseScheduleStatusHistoryTODT);
        int InsertTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO);
        int InsertTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO);
        int UpdateTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId);
        int DeleteTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 purchaseScheduleSummaryId, SqlCommand cmdDelete);
        int DeleteAllStatusHistoryAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

    }
}