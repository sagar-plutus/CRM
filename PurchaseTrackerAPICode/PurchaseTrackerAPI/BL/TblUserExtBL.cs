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
    public class TblUserExtBL : ITblUserExtBL
    {
        private readonly ITblUserExtDAO _iTblUserExtDAO;
        public TblUserExtBL(ITblUserExtDAO iTblUserExtDAO)
        {
            _iTblUserExtDAO = iTblUserExtDAO;
        }
        #region Selection
        
        public  List<TblUserExtTO> SelectAllTblUserExtList()
        {
            return _iTblUserExtDAO.SelectAllTblUserExt();
        }

        public  TblUserExtTO SelectTblUserExtTO(Int32 userId)
        {
            return _iTblUserExtDAO.SelectTblUserExt(userId);
        }

        

        #endregion
        
        #region Insertion
        public  int InsertTblUserExt(TblUserExtTO tblUserExtTO)
        {
            return _iTblUserExtDAO.InsertTblUserExt(tblUserExtTO);
        }

        public  int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserExtDAO.InsertTblUserExt(tblUserExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblUserExt(TblUserExtTO tblUserExtTO)
        {
            return _iTblUserExtDAO.UpdateTblUserExt(tblUserExtTO);
        }

        public  int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserExtDAO.UpdateTblUserExt(tblUserExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblUserExt()
        {
            return _iTblUserExtDAO.DeleteTblUserExt();
        }

        public  int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserExtDAO.DeleteTblUserExt(conn, tran);
        }

        #endregion
        
    }
}
