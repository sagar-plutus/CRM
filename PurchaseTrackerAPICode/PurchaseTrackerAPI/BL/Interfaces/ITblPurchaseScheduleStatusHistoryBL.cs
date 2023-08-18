using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseScheduleStatusHistoryBL
    {
        List<TblPurchaseScheduleStatusHistoryTO> SelectAllTblPurchaseScheduleStatusHistory();
        List<TblPurchaseScheduleStatusHistoryTO> SelectAllTblPurchaseScheduleStatusHistoryList();
        TblPurchaseScheduleStatusHistoryTO SelectTblPurchaseScheduleStatusHistoryTO(Int32 purchaseScheduleSummaryId);
        List<TblPurchaseScheduleStatusHistoryTO> SelectTblPurchaseScheduleStatusHistoryTO(Int32 purchaseScheduleSummaryId, bool getApprovedTO, bool isForApproval, Int32 StatusId, SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO);
        int InsertTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO);
        int UpdateTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId);
        int DeleteTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran);
        int DeleteAllStatusHistoryAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

    }
}