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
    public class TblLoadingSlipExtHistoryBL
    {
        #region Selection
        public static List<TblLoadingSlipExtHistoryTO> SelectAllTblLoadingSlipExtHistoryList()
        {
            return  TblLoadingSlipExtHistoryDAO.SelectAllTblLoadingSlipExtHistory();
        }
        
        public static TblLoadingSlipExtHistoryTO SelectTblLoadingSlipExtHistoryTO(Int32 idConfirmHistory)
        {
           return  TblLoadingSlipExtHistoryDAO.SelectTblLoadingSlipExtHistory(idConfirmHistory);
        }

        // Vaibhav [13-Nov-2017] added to select temp data
        public static List<TblLoadingSlipExtHistoryTO> SelectTempLoadingSlipExtHistoryTOList(Int32 loadingSlipExtId,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingSlipExtHistoryDAO.SelectTempLoadingSlipExtHistoryList(loadingSlipExtId,conn,tran);
        }

        
        #endregion

        #region Insertion
        public static int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO)
        {
            return TblLoadingSlipExtHistoryDAO.InsertTblLoadingSlipExtHistory(tblLoadingSlipExtHistoryTO);
        }

        public static int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipExtHistoryDAO.InsertTblLoadingSlipExtHistory(tblLoadingSlipExtHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO)
        {
            return TblLoadingSlipExtHistoryDAO.UpdateTblLoadingSlipExtHistory(tblLoadingSlipExtHistoryTO);
        }

        public static int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipExtHistoryDAO.UpdateTblLoadingSlipExtHistory(tblLoadingSlipExtHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory)
        {
            return TblLoadingSlipExtHistoryDAO.DeleteTblLoadingSlipExtHistory(idConfirmHistory);
        }

        public static int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipExtHistoryDAO.DeleteTblLoadingSlipExtHistory(idConfirmHistory, conn, tran);
        }

        public static int DeleteTempLoadingSlipExtHistoryTOList(Int32 IdLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipExtHistoryDAO.DeleteTempLoadingSlipExtHistoryTOList(IdLoadingSlipExt, conn, tran);
        }
        
        #endregion

    }
}
