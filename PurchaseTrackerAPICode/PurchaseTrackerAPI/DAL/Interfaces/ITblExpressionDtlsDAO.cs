using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblExpressionDtlsDAO
    {
        String SqlSelectQuery();
          List<TblExpressionDtlsTO> SelectAllTblExpressionDtls();
        List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(Int32 isActive, Int32 prodClassId);
        List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(Int32 isActive, Int32 prodClassId, SqlConnection conn, SqlTransaction tran);
        List<TblExpressionDtlsTO> SelectTblExpressionDtls(Int32 idExpDtls);
        List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(SqlConnection conn, SqlTransaction tran);
        List<TblExpressionDtlsTO> ConvertDTToList(SqlDataReader tblExpressionDtlsTODT);
        int InsertTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO);
        int InsertTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblExpressionDtlsTO tblExpressionDtlsTO, SqlCommand cmdInsert);
        int UpdateTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO);
        int UpdateTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblExpressionDtlsTO tblExpressionDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblExpressionDtls(Int32 idExpDtls);
        int DeleteTblExpressionDtls(Int32 idExpDtls, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idExpDtls, SqlCommand cmdDelete);
        int UpdateTblExpressionDtlsEdit(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        List<TblExpressionDtlsTO> GetHistoryOfExpressionsbyUniqueNo(int uniqueTrackId);
    }
}