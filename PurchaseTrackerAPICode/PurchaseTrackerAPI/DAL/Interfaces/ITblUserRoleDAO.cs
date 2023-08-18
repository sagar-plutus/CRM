using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblUserRoleDAO
    {
        String SqlSelectQuery();
        List<TblUserRoleTO> SelectAllTblUserRole();
        List<TblUserRoleTO> SelectAllActiveUserRole(Int32 userId);
        TblUserRoleTO SelectTblUserRole(Int32 idUserRole);
        Int32 IsAreaAllocatedUser(Int32 userId);
        List<DropDownTO> SelectUsersFromRoleForDropDown(Int32 roleId);
        List<TblUserRoleTO> ConvertDTToList(SqlDataReader tblUserRoleTODT);
        int InsertTblUserRole(TblUserRoleTO tblUserRoleTO);
        int InsertTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserRoleTO tblUserRoleTO, SqlCommand cmdInsert);
        int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO);
        int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUserRoleTO tblUserRoleTO, SqlCommand cmdUpdate);
        int DeleteTblUserRole(Int32 idUserRole);
        int DeleteTblUserRole(Int32 idUserRole, SqlConnection conn, SqlTransaction tran);
          int ExecuteDeletionCommand(Int32 idUserRole, SqlCommand cmdDelete);
   }
 }