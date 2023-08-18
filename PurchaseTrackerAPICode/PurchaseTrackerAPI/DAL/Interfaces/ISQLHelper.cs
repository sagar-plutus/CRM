using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ISQLHelper
    {
        int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParmeter);
        int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters);
        object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters);
        SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, CommandBehavior behavior, params SqlParameter[] commandParameters);

    }
}