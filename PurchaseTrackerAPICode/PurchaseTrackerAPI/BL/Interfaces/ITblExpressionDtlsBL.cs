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
    public interface ITblExpressionDtlsBL
    {
        List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(Int32 isActive, Int32 prodClassId);
        List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(Int32 isActive, Int32 prodClassId, SqlConnection conn, SqlTransaction tran);
        List<TblExpressionDtlsTO> SelectAllTblExpressionDtlsList();
        TblExpressionDtlsTO SelectTblExpressionDtlsTO(Int32 idExpDtls);
        int InsertTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO);
        int InsertTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO);
        int UpdateTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblExpressionDtls(Int32 idExpDtls);
        int DeleteTblExpressionDtls(Int32 idExpDtls, SqlConnection conn, SqlTransaction tran);
        ResultMessage EditExpressionDetails(TblExpressionDtlsTO expTO, int loginUserId);
        List<TblExpressionDtlsTO> GetHistoryOfExpressionsbyUniqueNo(int uniqueTrackId);
    }
}