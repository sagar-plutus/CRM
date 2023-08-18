using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGradeExpressionDtlsBL
    {
        List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpressionDtls();
        List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpressionDtlsList();
        List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtls(string enquiryDetailsId, SqlConnection conn, SqlTransaction tran);
        List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtls(string enquiryDetailsId);

        List<TblGradeExpressionDtlsTO> SelectGradeExpreDtlsByBaseMetalId(Int32 baseItemMetalCostId);

        TblBaseItemMetalCostTO GetBaseItemGradeExpreDtls(Int32 globalRatePurchaseId,Int32 cOrNcId);
        List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtlsByScheduleId(string scheduleDtlsId);
        void SelectGradeExpDtlsList(List<TblPurchaseVehicleDetailsTO> scheduleItemDtlsList, SqlConnection conn = null, SqlTransaction tran = null);
        void SelectGradeExpDtlsList(List<TblPurchaseEnquiryDetailsTO> enquiryItemDtlsList, SqlConnection conn = null, SqlTransaction tran = null);
        List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtlsByScheduleId(string scheduleDtlsId, SqlConnection conn, SqlTransaction tran);
        TblGradeExpressionDtlsTO SelectTblGradeExpressionDtlsTO(Int32 idGradeExpressionDtls);
        int InsertTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO);
        int InsertTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO);
        int UpdateTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGradeExpressionDtls(Int32 idGradeExpressionDtls);
        int DeleteTblGradeExpressionDtls(Int32 idGradeExpressionDtls, SqlConnection conn, SqlTransaction tran);
        int DeleteAllGradeExpDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpDtlsByGlobalRateId(string globleRatePurchaseIds,SqlConnection conn, SqlTransaction tran);

    }
}