using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblDocumentDetailsDAO
    {
        String SqlSelectQuery();
          List<TblDocumentDetailsTO> SelectAllTblDocumentDetails();
        TblDocumentDetailsTO SelectTblDocumentDetails(Int32 idDocument);
        List<TblDocumentDetailsTO> SelectAllTblDocumentDetails(SqlConnection conn, SqlTransaction tran);
        List<TblDocumentDetailsTO> SelectAllTblDocumentDetails(string documentIds, SqlConnection conn, SqlTransaction tran);
        List<TblDocumentDetailsTO> SelectDocumentDetailsBasedOnFileType(Int32 FileTypeId, Int32 createdBy);
        int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO);
        int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblDocumentDetailsTO tblDocumentDetailsTO, SqlCommand cmdInsert);
        int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO);
        int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblDocumentDetailsTO tblDocumentDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblDocumentDetails(Int32 idDocument);
        int DeleteTblDocumentDetails(Int32 idDocument, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idDocument, SqlCommand cmdDelete);
        List<TblDocumentDetailsTO> ConvertDTToList(SqlDataReader tblDocumentDetailsTODT);

    }
}