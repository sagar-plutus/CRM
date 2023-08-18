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
   public class TblModuleBL : ITblModuleBL
    {
        private readonly ITblModuleDAO _iTblModuleDAO;
        public TblModuleBL(ITblModuleDAO iTblModuleDAO)
        {
            _iTblModuleDAO = iTblModuleDAO;
        }
        #region Selection
        public  TblModuleTO SelectTblModuleTO(Int32 idModule)
        {
            return  _iTblModuleDAO.SelectTblModule(idModule);
        }
        public  List<DropDownTO> SelectAllTblModuleList()
        {
            return _iTblModuleDAO.SelectAllTblModule();
        }
        public  List<TblModuleTO> SelectTblModuleList()
        {
            return _iTblModuleDAO.SelectTblModuleList();
        }

        public  TblModuleTO SelectTblModuleTO(Int32 idModule, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModuleDAO.SelectTblModule(idModule, conn, tran);
        }

        #endregion

        #region Insertion
        public  int InsertTblModule(TblModuleTO tblModuleTO)
        {
            return _iTblModuleDAO.InsertTblModule(tblModuleTO);
        }

        public  int InsertTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModuleDAO.InsertTblModule(tblModuleTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblModule(TblModuleTO tblModuleTO)
        {
            return _iTblModuleDAO.UpdateTblModule(tblModuleTO);
        }

        public  int UpdateTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModuleDAO.UpdateTblModule(tblModuleTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblModule(Int32 idModule)
        {
            return _iTblModuleDAO.DeleteTblModule(idModule);
        }

        public  int DeleteTblModule(Int32 idModule, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModuleDAO.DeleteTblModule(idModule, conn, tran);
        }

        #endregion
        
    }
}
