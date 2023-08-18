using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimVehicleTypeDAO
    {
        String SqlSelectQuery();
        List<DimVehicleTypeTO> SelectAllDimVehicleType();
        DimVehicleTypeTO SelectDimVehicleType(Int32 idVehicleType);
        List<DimVehicleTypeTO> ConvertDTToList(SqlDataReader dimVehicleTypeTODT);
          int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO);
        int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteInsertionCommand(DimVehicleTypeTO dimVehicleTypeTO, SqlCommand cmdInsert);
        int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO);
        int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimVehicleTypeTO dimVehicleTypeTO, SqlCommand cmdUpdate);
        int DeleteDimVehicleType(Int32 idVehicleType);
        int DeleteDimVehicleType(Int32 idVehicleType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVehicleType, SqlCommand cmdDelete);

    }
}