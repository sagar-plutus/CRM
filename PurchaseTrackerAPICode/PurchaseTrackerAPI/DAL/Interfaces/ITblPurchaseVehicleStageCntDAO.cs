using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehicleStageCntDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseVehicleStageCntTO> SelectAllTblPurchaseVehicleStageCnt();
        List<TblPurchaseVehicleStageCntTO> SelectTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt);
        List<TblPurchaseVehicleStageCntTO> SelectPurchaseVehicleStageCntByRootScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehicleStageCntTO> ConvertDTToList(SqlDataReader tblPurchaseVehicleStageCntTODT);
        List<TblPurchaseVehicleStageCntTO> SelectAllTblPurchaseVehicleStageCnt(SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO);
        int InsertTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO);
        int UpdateTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseVehicleStageCntForWeighing(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommandForWeighing(Int32 rootScheduleId, SqlCommand cmdUpdate);
        int ExecuteUpdationCommand(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt);
        int DeleteTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchaseVehicleStageCnt, SqlCommand cmdDelete);
        int DeleteAllStageCntAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection  conn, SqlTransaction tran);

    }
 }