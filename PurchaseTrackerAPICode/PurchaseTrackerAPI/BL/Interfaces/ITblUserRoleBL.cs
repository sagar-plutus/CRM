using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblUserRoleBL
    {
        List<TblUserRoleTO> SelectAllTblUserRoleList();
        List<TblUserRoleTO> SelectAllActiveUserRoleList(Int32 userId);
        TblUserRoleTO SelectTblUserRoleTO(Int32 idUserRole);
        Int32 IsAreaConfigurationEnabled(Int32 userId);
        List<DropDownTO> SelectUsersFromRoleForDropDown(Int32 roleId);
        int InsertTblUserRole(TblUserRoleTO tblUserRoleTO);
        int InsertTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO);
        int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserRole(Int32 idUserRole);
        int DeleteTblUserRole(Int32 idUserRole, SqlConnection conn, SqlTransaction tran);

    }
}