using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceHistoryDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseInvoiceHistoryTO> SelectAllTblPurchaseInvoiceHistory();
        TblPurchaseInvoiceHistoryTO SelectTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory);
        List<TblPurchaseInvoiceHistoryTO> SelectAllTblPurchaseInvoiceHistory(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceHistoryTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceHistoryTODT);
        int InsertTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO);
        int InsertTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO);
        int UpdateTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory);
        int DeleteTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int64 idPurchaseInvHistory, SqlCommand cmdDelete);

    }
 }