using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblUserRoleBL : ITblUserRoleBL
    {
        private readonly ITblUserRoleDAO _iTblUserRoleDAO;
        public TblUserRoleBL(ITblUserRoleDAO iTblUserRoleDAO)
        {
            _iTblUserRoleDAO = iTblUserRoleDAO;
        }
        #region Selection
        
        public  List<TblUserRoleTO> SelectAllTblUserRoleList()
        {
            return _iTblUserRoleDAO.SelectAllTblUserRole();
        }

        public  List<TblUserRoleTO> SelectAllActiveUserRoleList(Int32 userId)
        {
            return _iTblUserRoleDAO.SelectAllActiveUserRole(userId);
        }

        public  TblUserRoleTO SelectTblUserRoleTO(Int32 idUserRole)
        {
            return _iTblUserRoleDAO.SelectTblUserRole(idUserRole);
            
        }
        public  Int32 IsAreaConfigurationEnabled(Int32 userId)
        {
            int isConfEn = _iTblUserRoleDAO.IsAreaAllocatedUser(userId);
            return isConfEn;

        }
        public  List<DropDownTO> SelectUsersFromRoleForDropDown(Int32 roleId)
        {
            return _iTblUserRoleDAO.SelectUsersFromRoleForDropDown(roleId);

        }

        #endregion

        #region Insertion
        public  int InsertTblUserRole(TblUserRoleTO tblUserRoleTO)
        {
            return _iTblUserRoleDAO.InsertTblUserRole(tblUserRoleTO);
        }

        public  int InsertTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserRoleDAO.InsertTblUserRole(tblUserRoleTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO)
        {
            return _iTblUserRoleDAO.UpdateTblUserRole(tblUserRoleTO);
        }

        public  int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserRoleDAO.UpdateTblUserRole(tblUserRoleTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblUserRole(Int32 idUserRole)
        {
            return _iTblUserRoleDAO.DeleteTblUserRole(idUserRole);
        }

        public  int DeleteTblUserRole(Int32 idUserRole, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserRoleDAO.DeleteTblUserRole(idUserRole, conn, tran);
        }

        #endregion
        
    }
}
