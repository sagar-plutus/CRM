using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimVehicleCategoryDAO
    {
        String SqlSelectQuery();
        List<DimVehicleCategoryTO> SelectAllDimVehicleCategory();
        DimVehicleCategoryTO SelectDimVehicleCategory(Int32 idVehicleCategory);
        List<DimVehicleCategoryTO> SelectAllDimVehicleCategory(SqlConnection conn, SqlTransaction tran);
        List<DimVehicleCategoryTO> ConvertDTToList(SqlDataReader dimVehicleCategoryTODT);
        int InsertDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO);
        int InsertDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimVehicleCategoryTO dimVehicleCategoryTO, SqlCommand cmdInsert);
        int UpdateDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO);
        int UpdateDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimVehicleCategoryTO dimVehicleCategoryTO, SqlCommand cmdUpdate);
        int DeleteDimVehicleCategory(Int32 idVehicleCategory);
        int DeleteDimVehicleCategory(Int32 idVehicleCategory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVehicleCategory, SqlCommand cmdDelete);

    }
}