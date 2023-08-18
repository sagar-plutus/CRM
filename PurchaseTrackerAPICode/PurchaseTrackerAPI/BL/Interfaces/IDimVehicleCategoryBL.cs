using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimVehicleCategoryBL
    {
        List<DimVehicleCategoryTO> SelectAllDimVehicleCategory();
        List<DimVehicleCategoryTO> SelectAllDimVehicleCategoryList();
        DimVehicleCategoryTO SelectDimVehicleCategoryTO(Int32 idVehicleCategory);
        int InsertDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO);
        int InsertDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO, SqlConnection conn, SqlTransaction tran);
          int UpdateDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO);
        int UpdateDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimVehicleCategory(Int32 idVehicleCategory);
        int DeleteDimVehicleCategory(Int32 idVehicleCategory, SqlConnection conn, SqlTransaction tran);

    }
}