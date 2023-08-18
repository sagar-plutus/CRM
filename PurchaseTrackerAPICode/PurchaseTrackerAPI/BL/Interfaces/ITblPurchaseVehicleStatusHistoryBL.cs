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
    public interface ITblPurchaseVehicleStatusHistoryBL
    {
        List<TblPurchaseVehicleStatusHistoryTO> SelectAllTblPurchaseVehicleStatusHistory();
        List<TblPurchaseVehicleStatusHistoryTO> SelectAllTblPurchaseVehicleStatusHistoryList();
        TblPurchaseVehicleStatusHistoryTO SelectTblPurchaseVehicleStatusHistoryTO(Int32 idPurVehStatusHistory);
        int InsertTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO);
        int InsertTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO);
        int UpdateTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory);
        int DeleteTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory, SqlConnection conn, SqlTransaction tran);


    }
}