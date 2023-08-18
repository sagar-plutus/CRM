using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseSchStatusHistoryDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseSchStatusHistoryTO> SelectAllTblPurchaseSchStatusHistory();
        List<TblPurchaseSchStatusHistoryTO> SelectTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory);
        List<TblPurchaseSchStatusHistoryTO> SelectAllTblPurchaseSchStatusHistory(SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO);
        int InsertTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO);
        int UpdateTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory);
        int DeleteTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchaseSchStatusHistory, SqlCommand cmdDelete);

        int DeletePurchaseVehHistoryDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

    }
}
