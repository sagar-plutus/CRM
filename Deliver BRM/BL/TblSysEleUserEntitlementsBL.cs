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
    public class TblSysEleUserEntitlementsBL
    {
        #region Selection
       
        public static List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlementsList(int userId)
        {
            return  TblSysEleUserEntitlementsDAO.SelectAllTblSysEleUserEntitlements(userId);
        }

        public static List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlementsList(int userId, int? moduleId)
        {
            return TblSysEleUserEntitlementsDAO.SelectAllTblSysEleUserEntitlements(userId, moduleId);
        }


        public static TblSysEleUserEntitlementsTO SelectTblSysEleUserEntitlementsTO(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission)
        {
           return  TblSysEleUserEntitlementsDAO.SelectTblSysEleUserEntitlements(idUserEntitlement, userId, sysEleId, permission);
        }

        

        #endregion
        
        #region Insertion
        public static int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO)
        {
            return TblSysEleUserEntitlementsDAO.InsertTblSysEleUserEntitlements(tblSysEleUserEntitlementsTO);
        }

        public static int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysEleUserEntitlementsDAO.InsertTblSysEleUserEntitlements(tblSysEleUserEntitlementsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO)
        {
            return TblSysEleUserEntitlementsDAO.UpdateTblSysEleUserEntitlements(tblSysEleUserEntitlementsTO);
        }

        public static int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysEleUserEntitlementsDAO.UpdateTblSysEleUserEntitlements(tblSysEleUserEntitlementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission)
        {
            return TblSysEleUserEntitlementsDAO.DeleteTblSysEleUserEntitlements(idUserEntitlement, userId, sysEleId, permission);
        }

        public static int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysEleUserEntitlementsDAO.DeleteTblSysEleUserEntitlements(idUserEntitlement, userId, sysEleId, permission, conn, tran);
        }

        #endregion
        
    }
}
