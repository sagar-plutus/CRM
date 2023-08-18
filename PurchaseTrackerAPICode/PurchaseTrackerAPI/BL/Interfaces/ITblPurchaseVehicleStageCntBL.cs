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
    public interface ITblPurchaseVehicleStageCntBL
    {
        List<TblPurchaseVehicleStageCntTO> SelectAllTblPurchaseVehicleStageCnt();
        List<TblPurchaseVehicleStageCntTO> SelectAllTblPurchaseVehicleStageCntList();
        TblPurchaseVehicleStageCntTO SelectTblPurchaseVehicleStageCntTO(Int32 idPurchaseVehicleStageCnt);
        TblPurchaseVehicleStageCntTO SelectPurchaseVehicleStageCntByRootScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO);
        int InsertTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO);
        int UpdateTblPurchaseVehicleStageCntForWeighing(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt);
        int DeleteTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt, SqlConnection conn, SqlTransaction tran);
        ResultMessage InsertOrUpdateVehicleWtStageCount(TblPurchaseScheduleSummaryTO scheduleSummaryTO, TblPurchaseWeighingStageSummaryTO weighingStageSummaryTO,TblPurchaseWeighingStageSummaryTO weighingStageSummaryTOForRec, TblPurchaseUnloadingDtlTO unloadingDtlTO, TblPurchaseGradingDtlsTO gradingDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteAllStageCntAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

    }
}