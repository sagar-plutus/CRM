using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblConfigParamHistoryBL
    {
        List<TblConfigParamHistoryTO> SelectAllTblConfigParamHistoryList();
        TblConfigParamHistoryTO SelectTblConfigParamHistoryTO(Int32 idParamHistory);
        int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO);
        int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO);
        int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblConfigParamHistory(Int32 idParamHistory);
        int DeleteTblConfigParamHistory(Int32 idParamHistory, SqlConnection conn, SqlTransaction tran);

    }
}