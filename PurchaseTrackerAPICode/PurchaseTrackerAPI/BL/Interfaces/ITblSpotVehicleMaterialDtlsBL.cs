using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSpotVehicleMaterialDtlsBL
    {
        List<TblSpotVehMatDtlsTO> SelectAllTblSpotVehicleMaterialDtls();
        List<TblSpotVehMatDtlsTO> SelectAllTblSpotVehicleMaterialDtlsList();
        TblSpotVehMatDtlsTO SelectTblSpotVehicleMaterialDtlsTO(Int32 idTblSpotVehicleMaterialDtls);
        List<TblSpotVehMatDtlsTO> SelectAllSpotVehMatDtlsBySpotVehId(Int32 spotEntryVehicleId);
        int InsertTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO);
        int InsertTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO);
        int UpdateTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls);
        int DeleteTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls, SqlConnection conn, SqlTransaction tran);
        int DeleteSpotVehMaterialDtls(Int32 spotVehicleId, SqlConnection conn, SqlTransaction tran);

    }
}