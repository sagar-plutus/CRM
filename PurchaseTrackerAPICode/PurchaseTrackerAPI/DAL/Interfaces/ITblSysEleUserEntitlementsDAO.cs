using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSysEleUserEntitlementsDAO
    {
        String SqlSelectQuery();
        List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlements(int userId);
        TblSysEleUserEntitlementsTO SelectTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission);
        TblSysEleUserEntitlementsTO SelectUserSysEleUserEntitlements(Int32 userId, Int32 sysEleId, SqlConnection conn, SqlTransaction tran);
        List<TblSysEleUserEntitlementsTO> ConvertDTToList(SqlDataReader tblSysEleUserEntitlementsTODT);
        int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO);
        int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlCommand cmdInsert);
        int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO);
        int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlCommand cmdUpdate);
        int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission);
        int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission, SqlCommand cmdDelete);

    }
}