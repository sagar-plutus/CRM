using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblMachineCalibrationDAO
    {
        String SqlSelectQuery();
        List<TblMachineCalibrationTO> SelectAllTblMachineCalibration();
        TblMachineCalibrationTO SelectTblMachineCalibration(Int32 idMachineCalibration);
        List<TblMachineCalibrationTO> SelectAllTblMachineCalibration(SqlConnection conn, SqlTransaction tran);
        TblMachineCalibrationTO SelectTblMachineCalibrationByWeighingMachineId(Int32 weighingMachineId);
        List<TblMachineCalibrationTO> ConvertDTToList(SqlDataReader tblMachineCalibrationTODT);
        int InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO);
        int InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblMachineCalibrationTO tblMachineCalibrationTO, SqlCommand cmdInsert);
        int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO);
        int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblMachineCalibrationTO tblMachineCalibrationTO, SqlCommand cmdUpdate);
        int DeleteTblMachineCalibration(Int32 idMachineCalibration);
        int DeleteTblMachineCalibration(Int32 idMachineCalibration, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idMachineCalibration, SqlCommand cmdDelete);

    }
}