using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceItemDetailsDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails();
        List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails(Int64 purchaseInvoiceId);
        List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceItemDetailsTO> SelectPurchaseInvoiceItemDtlsForOtherTaxId(Int64 purchaseInvoiceId, Int32 otherTaxTypeId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceItemDetailsTO SelectTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem);
        List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceItemDetailsTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceItemDetailsTODT);
        int InsertTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO);
        int InsertTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO);
        int UpdateTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem);
        int DeleteTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int64 idPurchaseInvoiceItem, SqlCommand cmdDelete);

    }
}