using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceDocumentsDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments();
        List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments(Int64 purchaseInvoiceId);
        TblPurchaseInvoiceDocumentsTO SelectTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument);
        List<TblPurchaseInvoiceDocumentsTO> SelecTblPurDocToVerifyWithDocDtlsAgainstPurInvId(Int64 purchaseInvoiceId);
        List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceDocumentsTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceDocumentsTODT);
        int InsertTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO);
        int InsertTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO);
        int UpdateTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument);
        int DeleteTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int64 idPurchaseInvDocument, SqlCommand cmdDelete);

    }
}