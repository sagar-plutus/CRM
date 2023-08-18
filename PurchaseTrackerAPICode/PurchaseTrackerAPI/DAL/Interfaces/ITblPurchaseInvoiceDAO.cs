using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoice();
        List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId);
        List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceTO SelectTblPurchaseInvoice(Int64 idInvoicePurchase);
        TblPurchaseInvoiceTO SelectTblPurchaseInvoice(Int64 idInvoicePurchase, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoice(SqlConnection conn, SqlTransaction tran);
        int SelectPurchaseInvoiceByInvoiceIdandFinYear(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, Boolean isFromEdit, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceTODT);
        int InsertTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO);
        int InsertTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO);
        int UpdateTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseInvoice(Int64 idInvoicePurchase);
        int DeleteTblPurchaseInvoice(Int64 idInvoicePurchase, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int64 idInvoicePurchase, SqlCommand cmdDelete);
        int UpdatePOAndGrrNoForInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
    }
}