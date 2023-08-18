using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblConfigParamsDAO
    {
        String SqlSelectQuery();
        List<TblConfigParamsTO> SelectAllTblConfigParams();
        TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName);
        TblConfigParamsTO SelectTblConfigParams(Int32 idConfigParam);
        TblConfigParamsTO SelectTblConfigParams(String configParamName, SqlConnection conn, SqlTransaction tran);
        List<TblConfigParamsTO> ConvertDTToList(SqlDataReader tblConfigParamsTODT);
        int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO);
        int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblConfigParamsTO tblConfigParamsTO, SqlCommand cmdInsert);
        int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO);
        int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblConfigParamsTO tblConfigParamsTO, SqlCommand cmdUpdate);
        int DeleteTblConfigParams(Int32 idConfigParam);
        int DeleteTblConfigParams(Int32 idConfigParam, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idConfigParam, SqlCommand cmdDelete);

        int IoTSetting();
        int IsSAPEnabled();

    }
}