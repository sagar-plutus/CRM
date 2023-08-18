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
    public interface ITblPurchaseInvoiceBL
    {
        List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoice();
        List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceList();
        List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId);
        List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceTO GetPurchaseInvoiceAgainstScheduleWithDtls(Int32 rootPurchaseSchId);
        TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTO(Int64 idInvoicePurchase);
        TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTO(Int64 idInvoicePurchase, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTOWithDetails(Int64 purchaseInvoiceId);
        TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTOWithDetails(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        int SelectPurchaseInvoiceByInvoiceIdandFinYear(TblPurchaseInvoiceTO tblPurchaseInvoiceTO,Boolean isFromEdit, SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO);
        int InsertTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage InsertPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO);
        ResultMessage SubmitInvoiceAgainstVehicle(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO);
        int UpdateTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveUpdatedPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO);
        ResultMessage CreateHistoryTO(TblPurchaseInvoiceTO existingInvoiceTO, TblPurchaseInvoiceTO currentInvoiceTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdatePurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoice(Int64 idInvoicePurchase);
        int DeleteTblPurchaseInvoice(Int64 idInvoicePurchase, SqlConnection conn, SqlTransaction tran);
        //ResultMessage CreatePurchaseInvoicePOWithGRR(Int32 rootScheduleId);
        int UpdatePOAndGrrNoForInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran);
    }
}