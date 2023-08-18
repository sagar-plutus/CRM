using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;

namespace SalesTrackerAPI.BL
{
    public class TblUserPwdHistoryBL
    {
        #region Selection
        public static List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistory()
        {
            return TblUserPwdHistoryDAO.SelectAllTblUserPwdHistory();
        }

        public static List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistoryList()
        {
            return TblUserPwdHistoryDAO.SelectAllTblUserPwdHistory();
        }

        public static TblUserPwdHistoryTO SelectTblUserPwdHistoryTO(Int32 idUserPwdHistory)
        {
            return TblUserPwdHistoryDAO.SelectTblUserPwdHistory(idUserPwdHistory);
        }

        

        #endregion
        
        #region Insertion
        public static int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO)
        {
            return TblUserPwdHistoryDAO.InsertTblUserPwdHistory(tblUserPwdHistoryTO);
        }

        public static int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserPwdHistoryDAO.InsertTblUserPwdHistory(tblUserPwdHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO)
        {
            return TblUserPwdHistoryDAO.UpdateTblUserPwdHistory(tblUserPwdHistoryTO);
        }

        public static int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserPwdHistoryDAO.UpdateTblUserPwdHistory(tblUserPwdHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblUserPwdHistory(Int32 idUserPwdHistory)
        {
            return TblUserPwdHistoryDAO.DeleteTblUserPwdHistory(idUserPwdHistory);
        }

        public static int DeleteTblUserPwdHistory(Int32 idUserPwdHistory, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserPwdHistoryDAO.DeleteTblUserPwdHistory(idUserPwdHistory, conn, tran);
        }

        #endregion
        
    }
}
