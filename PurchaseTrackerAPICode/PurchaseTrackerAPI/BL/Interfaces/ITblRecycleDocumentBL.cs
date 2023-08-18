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
    public interface ITblRecycleDocumentBL
    {
        List<TblRecycleDocumentTO> SelectAllTblRecycleDocument();
        List<TblRecycleDocumentTO> SelectAllTblRecycleDocumentList();
        TblRecycleDocumentTO SelectTblRecycleDocumentTO(Int32 idRecycleDocument);
        List<TblRecycleDocumentTO> SelectRecycleDocumentList(string txnId, Int32 txnTypeId, Int32 isActive, SqlConnection conn, SqlTransaction tran);
        List<TblDocumentDetailsTO> SelectAllRecycleDocumentList(string txnId, Int32 txnTypeId, Int32 isActive);
        int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO);
        int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO);
        int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblRecycleDocument(Int32 idRecycleDocument);
        int DeleteTblRecycleDocument(Int32 idRecycleDocument, SqlConnection conn, SqlTransaction tran);
        int SaveUploadedImages(List<TblRecycleDocumentTO> tblRecycleDocumentTOList, Int32 txnId, DateTime currentdate, SqlConnection conn, SqlTransaction tran);
        ResultMessage PostUploadedImages(List<TblRecycleDocumentTO> tblRecycleDocumentTOList, Int32 loginUserId);

    }
}