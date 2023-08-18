using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblVehicleBL
    {
        DataTable SelectAllTblVehicle();
        List<TblVehicleTO> SelectAllTblVehicleList();
        TblVehicleTO SelectTblVehicleTO(Int32 idVehicle);
        TblVehicleTO SelectTblVehicleTO(Int32 idVehicle, SqlConnection conn, SqlTransaction tran);

        List<TblVehicleTO> GetAllInternalTranferVehical(InternalTransferFilterTO InternalTransferFilterTO);
        ResultMessage  InsertTblVehicle(TblVehicleTO tblVehicleTO);
        int InsertTblVehicle(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVehicle(TblVehicleTO tblVehicleTO);
        int UpdateTblVehicle(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblVehicleDtl(TblVehicleTO tblVehicleTO);
        ResultMessage UpdateVehicalApprovalStatus(TblVehicleTO tblVehicleTO);
        ResultMessage UpdateVehicalStatus(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateVehicalStatus(TblVehicleTO tblVehicleTO);
    }

}
