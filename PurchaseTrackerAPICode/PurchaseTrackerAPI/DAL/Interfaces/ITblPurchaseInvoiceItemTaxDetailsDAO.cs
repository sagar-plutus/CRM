using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceItemTaxDetailsDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails();
        List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails(Int64 purchaseInvoiceItemId);
        List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails(Int64 purchaseInvoiceItemId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceItemTaxDetailsTO SelectTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl);
        List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceItemTaxDetailsTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceItemTaxDetailsTODT);
        int InsertTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO);
        int InsertTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO);
        int UpdateTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl);
        int DeleteTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoiceItemTaxDetailsByPurchaseInvoiceId(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int64 idPurchaseInvItemTaxDtl, SqlCommand cmdDelete);

    }
}