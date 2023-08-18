using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimVehicleTypeBL
    {
        List<DimVehicleTypeTO> SelectAllDimVehicleTypeList();
        DimVehicleTypeTO SelectDimVehicleTypeTO(Int32 idVehicleType);
        List<TblPurchaseMaterialSampleCategoryTo> SelectDimSampleCategoryTO();
        List<TblPurchaseMaterialSampleTypeTo> SelectDimSampleTypeTO();
        int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO);
        int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO);
        int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimVehicleType(Int32 idVehicleType);
        int DeleteDimVehicleType(Int32 idVehicleType, SqlConnection conn, SqlTransaction tran);

    }
}