using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertUsersBL
    {
        List<TblAlertUsersTO> SelectAllTblAlertUsersList();
        TblAlertUsersTO SelectTblAlertUsersTO(Int32 idAlertUser);
        List<TblAlertUsersTO> SelectAllActiveNotAKAlertList(Int32 userId, Int32 roleId);
        List<TblAlertUsersTO> SelectAllActiveAlertList(Int32 userId, Int32 roleId);
        int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO);
        int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO);
        int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertUsers(Int32 idAlertUser);
        int DeleteTblAlertUsers(Int32 idAlertUser, SqlConnection conn, SqlTransaction tran);

    }
}