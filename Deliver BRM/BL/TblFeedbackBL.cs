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
    public class TblFeedbackBL
    {
        #region Selection

        public static List<TblFeedbackTO> SelectAllTblFeedbackList()
        {
            return TblFeedbackDAO.SelectAllTblFeedback();
        }

        public static TblFeedbackTO SelectTblFeedbackTO(Int32 idFeedback)
        {
            return  TblFeedbackDAO.SelectTblFeedback(idFeedback);
        }

        internal static List<TblFeedbackTO> SelectAllTblFeedbackList(int userId, DateTime frmDt, DateTime toDt)
        {
            return TblFeedbackDAO.SelectAllTblFeedback(userId,frmDt,toDt);

        }

        #endregion

        #region Insertion
        public static int InsertTblFeedback(TblFeedbackTO tblFeedbackTO)
        {
            return TblFeedbackDAO.InsertTblFeedback(tblFeedbackTO);
        }

        public static int InsertTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFeedbackDAO.InsertTblFeedback(tblFeedbackTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO)
        {
            return TblFeedbackDAO.UpdateTblFeedback(tblFeedbackTO);
        }

        public static int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFeedbackDAO.UpdateTblFeedback(tblFeedbackTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblFeedback(Int32 idFeedback)
        {
            return TblFeedbackDAO.DeleteTblFeedback(idFeedback);
        }

        public static int DeleteTblFeedback(Int32 idFeedback, SqlConnection conn, SqlTransaction tran)
        {
            return TblFeedbackDAO.DeleteTblFeedback(idFeedback, conn, tran);
        }

       

        #endregion

    }
}
