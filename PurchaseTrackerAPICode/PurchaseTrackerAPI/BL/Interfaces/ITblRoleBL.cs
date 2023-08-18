using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblRoleBL
    {
        TblRoleTO SelectAllTblRole();
        List<TblRoleTO> SelectAllTblRoleList();
        TblRoleTO SelectTblRoleTO(Int32 idRole);
        List<TblRoleTO> ConvertDTToList(TblRoleTO tblRoleTODT);
        int InsertTblRole(TblRoleTO tblRoleTO);
        int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblRole(TblRoleTO tblRoleTO);
        int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblRole(Int32 idRole);
        int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran);

    }
}