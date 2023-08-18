using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertUsersDAO
    {
        String SqlSelectQuery();
        List<TblAlertUsersTO> SelectAllTblAlertUsers();
        List<TblAlertUsersTO> SelectAllActiveNotAKAlertList(Int32 userId, Int32 roleId);
        List<TblAlertUsersTO> SelectAllActiveAlertList(Int32 userId, Int32 roleId);
        TblAlertUsersTO SelectTblAlertUsers(Int32 idAlertUser);
        List<TblAlertUsersTO> ConvertDTToList(SqlDataReader tblAlertUsersTODT);
        int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO);
        int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAlertUsersTO tblAlertUsersTO, SqlCommand cmdInsert);
        int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO);
        int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAlertUsersTO tblAlertUsersTO, SqlCommand cmdUpdate);
        int DeleteTblAlertUsers(Int32 idAlertUser);
        int DeleteTblAlertUsers(Int32 idAlertUser, SqlConnection conn, SqlTransaction tran);
          int ExecuteDeletionCommand(Int32 idAlertUser, SqlCommand cmdDelete);

    }
}