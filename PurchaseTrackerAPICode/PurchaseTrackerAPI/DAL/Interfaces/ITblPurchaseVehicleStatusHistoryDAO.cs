using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehicleStatusHistoryDAO
    {

        String SqlSelectQuery();


        List<TblPurchaseVehicleStatusHistoryTO> SelectAllTblPurchaseVehicleStatusHistory();

        List<TblPurchaseVehicleStatusHistoryTO> SelectTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory);


        List<TblPurchaseVehicleStatusHistoryTO> SelectAllTblPurchaseVehicleStatusHistory(SqlConnection conn, SqlTransaction tran);

        int InsertTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO);

        int InsertTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteInsertionCommand(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlCommand cmdInsert);

        int UpdateTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO);


        int UpdateTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlConnection conn, SqlTransaction tran);


        int ExecuteUpdationCommand(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlCommand cmdUpdate);



        int DeleteTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory);

        int DeleteTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory, SqlConnection conn, SqlTransaction tran);


        int ExecuteDeletionCommand(Int32 idPurVehStatusHistory, SqlCommand cmdDelete);

    }
}