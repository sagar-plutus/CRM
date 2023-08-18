using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblVehicleDAO
    {
        DataTable SelectAllTblVehicle();
        TblVehicleTO SelectTblVehicle(Int32 idVehicle);
        TblVehicleTO SelectTblVehicle(Int32 idVehicle, SqlConnection conn, SqlTransaction tran);
        DataTable SelectAllTblVehicle(SqlConnection conn, SqlTransaction tran);
        int InsertTblVehicle(TblVehicleTO tblVehicleTO);
        int InsertTblVehicle(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVehicleTO tblVehicleTO, SqlCommand cmdInsert);
        int UpdateTblVehicle(TblVehicleTO tblVehicleTO);
        int UpdateTblVehicle(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVehicleTO tblVehicleTO, SqlCommand cmdUpdate);
            
        List<TblVehicleTO> GetAllInternalTranferVehical(InternalTransferFilterTO voucherFilterTO);
        Boolean checkAlreadyVehicalByName(TblVehicleTO tblVehicleTO);
        Boolean checkAlreadyVehicalById(TblVehicleTO tblVehicleTO);
        int UpdateVehicalApprovalStatus(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran);

        int UpdateVehicalStatus(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran);
    }
}
