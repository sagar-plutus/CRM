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
    public class TblFeedbackBL : ITblFeedbackBL
    {
        private readonly ITblFeedbackDAO _iTblFeedbackDAO;
        public TblFeedbackBL(ITblFeedbackDAO itblFeedbackDAO)
        {
            _iTblFeedbackDAO = itblFeedbackDAO;
        }
        #region Selection

        public  List<TblFeedbackTO> SelectAllTblFeedbackList()
        {
            return _iTblFeedbackDAO.SelectAllTblFeedback();
        }

        public  TblFeedbackTO SelectTblFeedbackTO(Int32 idFeedback)
        {
            return  _iTblFeedbackDAO.SelectTblFeedback(idFeedback);
        }

        public  List<TblFeedbackTO> SelectAllTblFeedbackList(int userId, DateTime frmDt, DateTime toDt)
        {
            return _iTblFeedbackDAO.SelectAllTblFeedback(userId,frmDt,toDt);

        }

        #endregion

        #region Insertion
        public  int InsertTblFeedback(TblFeedbackTO tblFeedbackTO)
        {
            return _iTblFeedbackDAO.InsertTblFeedback(tblFeedbackTO);
        }

        public  int InsertTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFeedbackDAO.InsertTblFeedback(tblFeedbackTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO)
        {
            return _iTblFeedbackDAO.UpdateTblFeedback(tblFeedbackTO);
        }

        public  int UpdateTblFeedback(TblFeedbackTO tblFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFeedbackDAO.UpdateTblFeedback(tblFeedbackTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblFeedback(Int32 idFeedback)
        {
            return _iTblFeedbackDAO.DeleteTblFeedback(idFeedback);
        }

        public  int DeleteTblFeedback(Int32 idFeedback, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblFeedbackDAO.DeleteTblFeedback(idFeedback, conn, tran);
        }

       

        #endregion

    }
}
