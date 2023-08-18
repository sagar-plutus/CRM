using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblRoleDAO
    {
        String SqlSelectQuery();
        TblRoleTO SelectAllTblRole();
        TblRoleTO SelectTblRole(Int32 idRole);
        List<TblRoleTO> ConvertDTToList(SqlDataReader tblRoleTODT);
        TblRoleTO SelectAllTblRole(SqlConnection conn, SqlTransaction tran);
        int InsertTblRole(TblRoleTO tblRoleTO);
        int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblRoleTO tblRoleTO, SqlCommand cmdInsert);
        int UpdateTblRole(TblRoleTO tblRoleTO);
        int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblRoleTO tblRoleTO, SqlCommand cmdUpdate);
        int DeleteTblRole(Int32 idRole);
        int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idRole, SqlCommand cmdDelete);

    }
}