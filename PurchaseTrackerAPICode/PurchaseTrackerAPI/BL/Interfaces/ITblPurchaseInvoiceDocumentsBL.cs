using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceDocumentsBL
    {
        List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments();
        List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments(Int64 purchaseInvoiceId);
        List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseDocToVerifyWithDocDtls();
        List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocumentsList();
        TblPurchaseInvoiceDocumentsTO SelectTblPurchaseInvoiceDocumentsTO(Int64 idPurchaseInvDocument);
        List<TblPurchaseInvoiceDocumentsTO> SelecTblPurDocToVerifyWithDocDtlsAgainstPurInvId(Int64 purchaseInvoiceId);
        int InsertTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO);
        int InsertTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO);
        int UpdateTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument);
        int DeleteTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument, SqlConnection conn, SqlTransaction tran);

    }
}