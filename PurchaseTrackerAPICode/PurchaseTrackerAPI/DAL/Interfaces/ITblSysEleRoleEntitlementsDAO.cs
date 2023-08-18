using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSysEleRoleEntitlementsDAO
    {
        String SqlSelectQuery();
        List<TblSysEleRoleEntitlementsTO> SelectAllTblSysEleRoleEntitlements(int roleId);
        Dictionary<int, String> SelectAllTblSysEleRoleEntitlementsDCT(Int32 roleId);
        TblSysEleRoleEntitlementsTO SelectTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission);
        TblSysEleRoleEntitlementsTO SelectRoleSysEleUserEntitlements(Int32 roleId, Int32 sysEleId, SqlConnection conn, SqlTransaction tran);
        List<TblSysEleRoleEntitlementsTO> ConvertDTToList(SqlDataReader tblSysEleRoleEntitlementsTODT);
          int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO);
        int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteInsertionCommand(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlCommand cmdInsert);
        int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO);
        int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlCommand cmdUpdate);
        int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission);
        int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission, SqlCommand cmdDelete);

    }
}