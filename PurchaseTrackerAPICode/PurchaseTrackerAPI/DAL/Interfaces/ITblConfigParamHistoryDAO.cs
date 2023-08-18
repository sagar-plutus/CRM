using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblConfigParamHistoryDAO
    {
        String SqlSelectQuery();
          List<TblConfigParamHistoryTO> SelectAllTblConfigParamHistory();
        TblConfigParamHistoryTO SelectTblConfigParamHistory(Int32 idParamHistory);
        List<TblConfigParamHistoryTO> ConvertDTToList(SqlDataReader tblConfigParamHistoryTODT);
        int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO);
        int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlCommand cmdInsert);
        int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO);
        int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblConfigParamHistory(Int32 idParamHistory);
        int DeleteTblConfigParamHistory(Int32 idParamHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idParamHistory, SqlCommand cmdDelete);

    }
}