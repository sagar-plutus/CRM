using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGradeExpressionDtlsDAO
    {
        String SqlSelectQuery();
        List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpressionDtls();
        List<TblGradeExpressionDtlsTO> SelectTblGradeExpressionDtls(Int32 idGradeExpressionDtls);

        List<TblGradeExpressionDtlsTO> SelectGradeExpreDtlsByBaseMetalId(Int32 baseItemMetalCostId);
        List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtls(string enquiryDetailsId, SqlConnection conn, SqlTransaction tran);
        List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtls(string enquiryDetailsId);
        List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtlsByScheduleId(string scheduleDtlsId);
        List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtlsByScheduleId(string scheduleDtlsId, SqlConnection conn, SqlTransaction tran);
        List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpressionDtls(SqlConnection conn, SqlTransaction tran);
        List<TblGradeExpressionDtlsTO> ConvertDTToList(SqlDataReader tblGradeExpressionDtlsTODT);
        int InsertTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO);
        int InsertTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlCommand cmdInsert);
        int UpdateTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO);
        int UpdateTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblGradeExpressionDtls(Int32 idGradeExpressionDtls);
        int DeleteTblGradeExpressionDtls(Int32 idGradeExpressionDtls, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idGradeExpressionDtls, SqlCommand cmdDelete);
        int DeleteAllGradeExpDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

        List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpDtlsByGlobalRateId(string globleRatePurchaseIds,SqlConnection conn, SqlTransaction tran);

    }
}