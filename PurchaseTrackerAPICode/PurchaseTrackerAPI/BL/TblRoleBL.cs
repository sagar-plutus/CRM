using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblRoleBL : ITblRoleBL
    {
        private readonly ITblRoleDAO _iTblRoleDAO;
        public TblRoleBL(ITblRoleDAO iTblRoleDAO)
        {
            _iTblRoleDAO = iTblRoleDAO;
        }
        #region Selection
        public  TblRoleTO SelectAllTblRole()
        {
            return _iTblRoleDAO.SelectAllTblRole();
        }

        public  List<TblRoleTO> SelectAllTblRoleList()
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectAllTblRole();
            return ConvertDTToList(tblRoleTODT);
        }

        public  TblRoleTO SelectTblRoleTO(Int32 idRole)
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectTblRole(idRole);
            //List<TblRoleTO> tblRoleTOList = ConvertDTToList(tblRoleTODT);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }

        public  List<TblRoleTO> ConvertDTToList(TblRoleTO tblRoleTODT)
        {
            List<TblRoleTO> tblRoleTOList = new List<TblRoleTO>();
            if (tblRoleTODT != null)
            {
            }
            return tblRoleTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblRole(TblRoleTO tblRoleTO)
        {
            return _iTblRoleDAO.InsertTblRole(tblRoleTO);
        }

        public  int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.InsertTblRole(tblRoleTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblRole(TblRoleTO tblRoleTO)
        {
            return _iTblRoleDAO.UpdateTblRole(tblRoleTO);
        }

        public  int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.UpdateTblRole(tblRoleTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblRole(Int32 idRole)
        {
            return _iTblRoleDAO.DeleteTblRole(idRole);
        }

        public  int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.DeleteTblRole(idRole, conn, tran);
        }

        #endregion

    }
}
