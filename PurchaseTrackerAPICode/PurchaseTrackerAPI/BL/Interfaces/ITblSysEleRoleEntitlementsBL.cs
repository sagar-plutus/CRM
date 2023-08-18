using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSysEleRoleEntitlementsBL
    {
        List<TblSysEleRoleEntitlementsTO> SelectAllTblSysEleRoleEntitlementsList(int roleId);
        TblSysEleRoleEntitlementsTO SelectTblSysEleRoleEntitlementsTO(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission);
        int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO);
        int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran);
          int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO);
        int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission);
        int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran);

    }
}