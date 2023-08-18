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
    public class TblLoadingStatusHistoryBL
    {
        #region Selection
       
        public static List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistoryList(Int32 loadingId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingStatusHistoryDAO.SelectAllTblLoadingStatusHistory(loadingId,conn,tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistoryList(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                return TblLoadingStatusHistoryDAO.SelectAllTblLoadingStatusHistory(loadingId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static TblLoadingStatusHistoryTO SelectTblLoadingStatusHistoryTO(Int32 idLoadingHistory)
        {
            return TblLoadingStatusHistoryDAO.SelectTblLoadingStatusHistory(idLoadingHistory);
        }

        

        #endregion
        
        #region Insertion
        public static int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO)
        {
            return TblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO);
        }

        public static int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO)
        {
            return TblLoadingStatusHistoryDAO.UpdateTblLoadingStatusHistory(tblLoadingStatusHistoryTO);
        }

        public static int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingStatusHistoryDAO.UpdateTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory)
        {
            return TblLoadingStatusHistoryDAO.DeleteTblLoadingStatusHistory(idLoadingHistory);
        }

        public static int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingStatusHistoryDAO.DeleteTblLoadingStatusHistory(idLoadingHistory, conn, tran);
        }

        #endregion
        
    }
}
