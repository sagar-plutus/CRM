using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblRecycleDocumentDAO
    {
        String SqlSelectQuery();
        List<TblRecycleDocumentTO> SelectAllTblRecycleDocument();
        List<TblRecycleDocumentTO> SelectTblRecycleDocument(Int32 idRecycleDocument);
        List<TblRecycleDocumentTO> SelectRecycleDocumentList(string txnId, Int32 txnTypeId, Int32 isActive, SqlConnection conn, SqlTransaction tran);
        List<TblRecycleDocumentTO> SelectAllTblRecycleDocument(SqlConnection conn, SqlTransaction tran);
        List<TblRecycleDocumentTO> ConvertDTToList(SqlDataReader tblRecycleDocumentTODT);
        int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO);
        int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblRecycleDocumentTO tblRecycleDocumentTO, SqlCommand cmdInsert);
        int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO);
        int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblRecycleDocumentTO tblRecycleDocumentTO, SqlCommand cmdUpdate);
        int DeleteTblRecycleDocument(Int32 idRecycleDocument);
        int DeleteTblRecycleDocument(Int32 idRecycleDocument, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idRecycleDocument, SqlCommand cmdDelete);

    }
}