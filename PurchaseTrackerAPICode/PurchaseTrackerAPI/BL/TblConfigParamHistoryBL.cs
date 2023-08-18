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
    public class TblConfigParamHistoryBL: ITblConfigParamHistoryBL
    {
        private readonly ITblConfigParamHistoryDAO _itblConfigParamHistoryDAO;
        public TblConfigParamHistoryBL(ITblConfigParamHistoryDAO iTblConfigParamHistoryDAO)
        {
            _itblConfigParamHistoryDAO = iTblConfigParamHistoryDAO;
        }

        #region Selection

        public  List<TblConfigParamHistoryTO> SelectAllTblConfigParamHistoryList()
        {
            return _itblConfigParamHistoryDAO.SelectAllTblConfigParamHistory();
        }

        public  TblConfigParamHistoryTO SelectTblConfigParamHistoryTO(Int32 idParamHistory)
        {
            return  _itblConfigParamHistoryDAO.SelectTblConfigParamHistory(idParamHistory);
        }

        

        #endregion
        
        #region Insertion
        public  int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            return _itblConfigParamHistoryDAO.InsertTblConfigParamHistory(tblConfigParamHistoryTO);
        }

        public  int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblConfigParamHistoryDAO.InsertTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            return _itblConfigParamHistoryDAO.UpdateTblConfigParamHistory(tblConfigParamHistoryTO);
        }

        public  int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblConfigParamHistoryDAO.UpdateTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblConfigParamHistory(Int32 idParamHistory)
        {
            return _itblConfigParamHistoryDAO.DeleteTblConfigParamHistory(idParamHistory);
        }

        public  int DeleteTblConfigParamHistory(Int32 idParamHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _itblConfigParamHistoryDAO.DeleteTblConfigParamHistory(idParamHistory, conn, tran);
        }

        #endregion
        
    }
}
