using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseGradingDtlsDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseGradingDtlsTO> SelectAllTblPurchaseGradingDtls();
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtls(Int32 idGradingDtls);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByWeighingId(Int32 weighingStageId);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByScheduleId(string purchaseScheduleIds);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByScheduleId(string purchaseScheduleIds, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByWeighingId(Int32 weighingStageId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByGradingDtlsId(Int32 gradingDtlsId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseGradingDtlsTO> SelectAllTblPurchaseGradingDtls(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseGradingDtlsTO> ConvertDTToList(SqlDataReader tblPurchaseGradingDtlsTODT);
        int InsertTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO);
        int InsertTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO);
        int UpdateTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlCommand cmdUpdate);
          int DeleteTblPurchaseGradingDtls(Int32 idGradingDtls);
        int DeleteTblPurchaseGradingDtls(Int32 idGradingDtls, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idGradingDtls, SqlCommand cmdDelete);
        int DeleteAllGradingDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

        int DeleteAllGradingDtls(Int32 scheduleSummaryId, SqlConnection conn, SqlTransaction tran);

    }
}