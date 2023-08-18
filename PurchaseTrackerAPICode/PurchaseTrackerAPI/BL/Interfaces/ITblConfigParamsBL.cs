using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblConfigParamsBL
    {
        List<TblConfigParamsTO> SelectAllTblConfigParamsList();
        TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName);
        TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName, SqlConnection conn, SqlTransaction tran);
          TblConfigParamsTO SelectTblConfigParamsTO(Int32 idConfigParam);
        TblConfigParamsTO SelectTblConfigParamsTO(String configParamName);
        List<Int32> SelectDefaultPmRoleIds();

        String SelectTblConfigParamsValByNameString(string configParamName);

        TblConfigParamsTO SelectTblConfigParamsTO(string configParamName, SqlConnection conn, SqlTransaction tran);
        Int32 GetStockConfigIsConsolidate();
          int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO);
        int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO);
          int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblConfigParams(Int32 idConfigParam);
        int DeleteTblConfigParams(Int32 idConfigParam, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveBirimMachineQty(double birimMachineQty, Int32 loginUserId);
        double GetCurrentValueOfV8RefVar(String variableName);
    }
}