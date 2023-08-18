using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSysEleUserEntitlementsBL
    {
        List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlementsList(int userId);
        TblSysEleUserEntitlementsTO SelectTblSysEleUserEntitlementsTO(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission);
        int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO);
        int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO);
        int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission);
        int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran);

    }
}