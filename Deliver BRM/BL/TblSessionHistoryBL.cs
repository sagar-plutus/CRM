using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SalesTrackerAPI.BL
{
    public class TblSessionHistoryBL
    {
        #region Selection
        public static TblSessionHistoryTO SelectAllTblSessionHistory()
        {
            return TblSessionHistoryDAO.SelectAllTblSessionHistory();
        }

        public static List<TblSessionHistoryTO> SelectAllTblSessionHistoryList()
        {
            return TblSessionHistoryDAO.SelectAllTblSessionHistoryData();
        }

        public static List<TblSessionHistoryTO> SelectTblSessionHistoryTO(Int32 idSessionHistory)
        {
           return TblSessionHistoryDAO.SelectTblSessionHistory(idSessionHistory);
        }

        #endregion
        
        #region Insertion
        public static int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO)
        {
            return TblSessionHistoryDAO.InsertTblSessionHistory(tblSessionHistoryTO);
        }

        public static int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSessionHistoryDAO.InsertTblSessionHistory(tblSessionHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO)
        {
            return TblSessionHistoryDAO.UpdateTblSessionHistory(tblSessionHistoryTO);
        }

        public static int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSessionHistoryDAO.UpdateTblSessionHistory(tblSessionHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblSessionHistory(Int32 idSessionHistory)
        {
            return TblSessionHistoryDAO.DeleteTblSessionHistory(idSessionHistory);
        }
        public static int DeleteTblSessionHistory()
        {
            return TblSessionHistoryDAO.DeleteTblSessionHistory();
        }

        public static int DeleteTblSessionHistory(Int32 idSessionHistory, SqlConnection conn, SqlTransaction tran)
        {
            return TblSessionHistoryDAO.DeleteTblSessionHistory(idSessionHistory, conn, tran);
        }

        #endregion
        
    }
}
