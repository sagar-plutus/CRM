using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSpotVehicleMaterialDtlsDAO
    {
        String SqlSelectQuery();
        List<TblSpotVehMatDtlsTO> SelectAllTblSpotVehicleMaterialDtls();
        List<TblSpotVehMatDtlsTO> SelectTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls);
        List<TblSpotVehMatDtlsTO> SelectTblSpotVehMatDtls(Int32 idVehicleSpotEntry);
        List<TblSpotVehMatDtlsTO> SelectAllTblSpotVehicleMaterialDtls(SqlConnection conn, SqlTransaction tran);
        List<TblSpotVehMatDtlsTO> ConvertDTToList(SqlDataReader tblSpotVehicleMaterialDtlsTODT);
        int InsertTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO);
        int InsertTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlCommand cmdInsert);
        int UpdateTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO);
        int UpdateTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls);
        int DeleteTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTblSpotVehicleMaterialDtls, SqlCommand cmdDelete);
        int DeleteSpotVehMaterialDtls(Int32 spotVehicleId, SqlConnection conn, SqlTransaction tran);

    }
}