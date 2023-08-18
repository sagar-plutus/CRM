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
    public interface ITblPurchaseGradingDtlsBL
    {
        List<TblPurchaseGradingDtlsTO> SelectAllTblPurchaseGradingDtls();
        List<TblPurchaseGradingDtlsTO> SelectAllTblPurchaseGradingDtlsList();
        TblPurchaseGradingDtlsTO SelectTblPurchaseGradingDtlsTO(Int32 idGradingDtls);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByWeighingId(Int32 weighingStageId);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByWeighingId(Int32 weighingStageId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByGradingDtlsId(Int32 gradingDtlsId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByScheduleId(string purchaseScheduleId);
        List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByScheduleId(string purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO);
        int InsertTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveGradingMaterialDetails(List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList, Int32 loginUserId);
        int UpdateTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO);
        int UpdateTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseGradingDtls(Int32 idGradingDtls);
        int DeleteTblPurchaseGradingDtls(Int32 idGradingDtls, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeleteGradingDetails(List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList);
        int DeleteAllGradingDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        int DeleteAllGradingDtls(Int32 scheduleSummaryId, SqlConnection conn, SqlTransaction tran);

    }
}