using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceItemDetailsBL
    {
        List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails();
        List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetailsList();
        List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetailsList(Int64 purchaseInvoiceId);
        List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetailsList(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceItemDetailsTO> SelectPurchaseInvoiceItemDtlsForOtherTaxId(Int64 purchaseInvoiceId, Int32 otherTaxTypeId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceItemDetailsTO SelectTblPurchaseInvoiceItemDetailsTO(Int64 idPurchaseInvoiceItem);
        int InsertTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO);
        int InsertTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO);
        int UpdateTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem);
        int DeleteTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem, SqlConnection conn, SqlTransaction tran);

    }
}