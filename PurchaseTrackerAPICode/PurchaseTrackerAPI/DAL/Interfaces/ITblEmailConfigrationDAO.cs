using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblEmailConfigrationDAO
    {
        String SqlSelectQuery();
        List<TblEmailConfigrationTO> SelectAllDimEmailConfigration();
        TblEmailConfigrationTO SelectDimEmailConfigrationIsActive();
        List<TblEmailConfigrationTO> SelectAllDimEmailConfigration(SqlConnection conn, SqlTransaction tran);
        int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO);
        int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblEmailConfigrationTO dimEmailConfigrationTO, SqlCommand cmdInsert);
        int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO);
        int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblEmailConfigrationTO dimEmailConfigrationTO, SqlCommand cmdUpdate);
        List<TblEmailConfigrationTO> ConvertDTToList(SqlDataReader dimEmailConfigrationTODT);
        int DeleteDimEmailConfigration(Int32 idEmailConfig);
        int DeleteDimEmailConfigration(Int32 idEmailConfig, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idEmailConfig, SqlCommand cmdDelete);

    }
}