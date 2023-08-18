using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimVehiclePhaseBL
    {
        //List<DimVehiclePhaseTO> SelectAllDimVehiclePhase();
        List<DimVehiclePhaseTO> SelectAllDimVehiclePhaseList(Int32 isActive);
        DimVehiclePhaseTO SelectDimVehiclePhaseTO(Int32 idVehiclePhase);
        int InsertDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO);
        int InsertDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO);
        int UpdateDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimVehiclePhase(Int32 idVehiclePhase);
        int DeleteDimVehiclePhase(Int32 idVehiclePhase, SqlConnection conn, SqlTransaction tran);

    }
}