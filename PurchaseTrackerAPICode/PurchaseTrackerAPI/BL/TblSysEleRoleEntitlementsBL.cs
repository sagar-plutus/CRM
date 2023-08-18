using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{

    public class TblSysEleRoleEntitlementsBL : ITblSysEleRoleEntitlementsBL
    {

        private readonly ITblSysEleRoleEntitlementsDAO _iTblSysEleRoleEntitlementsDAO;
        public TblSysEleRoleEntitlementsBL(ITblSysEleRoleEntitlementsDAO iTblSysEleRoleEntitlementsDAO)
        {
            _iTblSysEleRoleEntitlementsDAO = iTblSysEleRoleEntitlementsDAO;
        }
        #region Selection
        public  List<TblSysEleRoleEntitlementsTO> SelectAllTblSysEleRoleEntitlementsList(int roleId)
        {
            return  _iTblSysEleRoleEntitlementsDAO.SelectAllTblSysEleRoleEntitlements(roleId);
        }

        public  TblSysEleRoleEntitlementsTO SelectTblSysEleRoleEntitlementsTO(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission)
        {
            return _iTblSysEleRoleEntitlementsDAO.SelectTblSysEleRoleEntitlements(idRoleEntitlement, roleId, sysEleId, permission);
        }

       

        #endregion
        
        #region Insertion
        public  int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO)
        {
            return _iTblSysEleRoleEntitlementsDAO.InsertTblSysEleRoleEntitlements(tblSysEleRoleEntitlementsTO);
        }

        public  int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysEleRoleEntitlementsDAO.InsertTblSysEleRoleEntitlements(tblSysEleRoleEntitlementsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO)
        {
            return _iTblSysEleRoleEntitlementsDAO.UpdateTblSysEleRoleEntitlements(tblSysEleRoleEntitlementsTO);
        }

        public  int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysEleRoleEntitlementsDAO.UpdateTblSysEleRoleEntitlements(tblSysEleRoleEntitlementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission)
        {
            return _iTblSysEleRoleEntitlementsDAO.DeleteTblSysEleRoleEntitlements(idRoleEntitlement, roleId, sysEleId, permission);
        }

        public  int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysEleRoleEntitlementsDAO.DeleteTblSysEleRoleEntitlements(idRoleEntitlement, roleId, sysEleId, permission, conn, tran);
        }

        #endregion
        
    }
}
