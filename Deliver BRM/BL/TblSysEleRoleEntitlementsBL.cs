using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.BL
{
    public class TblSysEleRoleEntitlementsBL
    {
        #region Selection
        public static List<TblSysEleRoleEntitlementsTO> SelectAllTblSysEleRoleEntitlementsList(int roleId)
        {
            return  TblSysEleRoleEntitlementsDAO.SelectAllTblSysEleRoleEntitlements(roleId);
        }

        public static TblSysEleRoleEntitlementsTO SelectTblSysEleRoleEntitlementsTO(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission)
        {
            return TblSysEleRoleEntitlementsDAO.SelectTblSysEleRoleEntitlements(idRoleEntitlement, roleId, sysEleId, permission);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO)
        {
            return TblSysEleRoleEntitlementsDAO.InsertTblSysEleRoleEntitlements(tblSysEleRoleEntitlementsTO);
        }

        public static int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysEleRoleEntitlementsDAO.InsertTblSysEleRoleEntitlements(tblSysEleRoleEntitlementsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO)
        {
            return TblSysEleRoleEntitlementsDAO.UpdateTblSysEleRoleEntitlements(tblSysEleRoleEntitlementsTO);
        }

        public static int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysEleRoleEntitlementsDAO.UpdateTblSysEleRoleEntitlements(tblSysEleRoleEntitlementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission)
        {
            return TblSysEleRoleEntitlementsDAO.DeleteTblSysEleRoleEntitlements(idRoleEntitlement, roleId, sysEleId, permission);
        }

        public static int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysEleRoleEntitlementsDAO.DeleteTblSysEleRoleEntitlements(idRoleEntitlement, roleId, sysEleId, permission, conn, tran);
        }

        #endregion
        
    }
}
