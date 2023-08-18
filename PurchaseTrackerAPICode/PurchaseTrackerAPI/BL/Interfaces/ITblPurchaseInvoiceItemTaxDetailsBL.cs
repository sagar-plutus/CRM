using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceItemTaxDetailsBL
    {
        List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails();
        List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetailsList();
        List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetailsList(Int64 purchaseInvoiceItemId);
        List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetailsList(Int64 purchaseInvoiceItemId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceItemTaxDetailsTO SelectTblPurchaseInvoiceItemTaxDetailsTO(Int64 idPurchaseInvItemTaxDtl);
        int InsertTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO);
        int InsertTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO);
        int UpdateTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl);
        int DeleteTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoiceItemTaxDetailsByPurchaseInvoiceId(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);

    }
}