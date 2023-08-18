using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using Microsoft.AspNetCore.Http;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblDocumentDetailsBL
    {
        List<TblDocumentDetailsTO> SelectAllTblDocumentDetails();
        List<TblDocumentDetailsTO> SelectAllTblDocumentDetailsList();
          TblDocumentDetailsTO SelectTblDocumentDetailsTO(Int32 idDocument);
        List<TblDocumentDetailsTO> SelectAllTblDocumentDetails(string documentIds, SqlConnection conn, SqlTransaction tran);
        List<TblDocumentDetailsTO> GetUploadedFileBasedOnFileType(Int32 fileTypeId, Int32 createdBy);
        int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO);
        int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO);
        int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblDocumentDetails(Int32 idDocument);
        int DeleteTblDocumentDetails(Int32 idDocument, SqlConnection conn, SqlTransaction tran);
        ResultMessage UploadDocument(List<TblDocumentDetailsTO> tblDocumentDetailsTOList);
        ResultMessage UploadDocumentWithConnTran(List<TblDocumentDetailsTO> tblDocumentDetailsTOList, SqlConnection conn, SqlTransaction tran);
         Task UploadMultipleTypesFile(List<IFormFile> files, Int32 createdBy, Int32 FileTypeId);

    }
}