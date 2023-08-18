using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceHistoryBL
    {
        List<TblPurchaseInvoiceHistoryTO> SelectAllTblPurchaseInvoiceHistory();
        List<TblPurchaseInvoiceHistoryTO> SelectAllTblPurchaseInvoiceHistoryList();
        TblPurchaseInvoiceHistoryTO SelectTblPurchaseInvoiceHistoryTO(Int64 idPurchaseInvHistory);
        int InsertTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO);
        int InsertTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO);
        int UpdateTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory);
        int DeleteTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory, SqlConnection conn, SqlTransaction tran);

    }
}