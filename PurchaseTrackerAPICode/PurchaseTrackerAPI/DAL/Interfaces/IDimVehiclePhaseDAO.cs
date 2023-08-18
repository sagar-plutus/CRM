using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimVehiclePhaseDAO
    {
        String SqlSelectQuery();
          List<DimVehiclePhaseTO> SelectAllDimVehiclePhase(Int32 isActive);
        List<DimVehiclePhaseTO> SelectDimVehiclePhase(Int32 idVehiclePhase);
        List<DimVehiclePhaseTO> SelectAllDimVehiclePhase(SqlConnection conn, SqlTransaction tran);
        List<DimVehiclePhaseTO> ConvertDTToList(SqlDataReader dimVehiclePhaseTODT);
        int InsertDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO);
        int InsertDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimVehiclePhaseTO dimVehiclePhaseTO, SqlCommand cmdInsert);
          int UpdateDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO);
        int UpdateDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimVehiclePhaseTO dimVehiclePhaseTO, SqlCommand cmdUpdate);
        int DeleteDimVehiclePhase(Int32 idVehiclePhase);
        int DeleteDimVehiclePhase(Int32 idVehiclePhase, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVehiclePhase, SqlCommand cmdDelete);

    }
}