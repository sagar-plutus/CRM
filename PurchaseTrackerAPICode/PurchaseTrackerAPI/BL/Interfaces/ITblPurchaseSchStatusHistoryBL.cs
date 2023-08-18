using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblPurchaseSchStatusHistoryBL
    {
        List<TblPurchaseSchStatusHistoryTO> SelectAllTblPurchaseSchStatusHistory();
        List<TblPurchaseSchStatusHistoryTO> SelectAllTblPurchaseSchStatusHistoryList();
        TblPurchaseSchStatusHistoryTO SelectTblPurchaseSchStatusHistoryTO(Int32 idPurchaseSchStatusHistory);
        int InsertTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO);
        int InsertTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO);
        int UpdateTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory);
        int DeleteTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory, SqlConnection conn, SqlTransaction tran);
        ResultMessage SavePurVehStatusHistory(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran);

        int DeletePurchaseVehHistoryDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
    }
}
