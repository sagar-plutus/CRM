using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblVisitFeedbackBL
    {
        #region Selection
        public static DataTable SelectAllTblVisitFeedback()
        {
            return TblVisitFeedbackDAO.SelectAllTblVisitFeedback();
        }

        public static List<TblVisitFeedbackTO> SelectAllTblVisitFeedbackList()
        {
            DataTable tblVisitFeedbackTODT = TblVisitFeedbackDAO.SelectAllTblVisitFeedback();
            return ConvertDTToList(tblVisitFeedbackTODT);
        }

        public static TblVisitFeedbackTO SelectTblVisitFeedbackTO()
        {
            DataTable tblVisitFeedbackTODT = TblVisitFeedbackDAO.SelectTblVisitFeedback();
            List<TblVisitFeedbackTO> tblVisitFeedbackTOList = ConvertDTToList(tblVisitFeedbackTODT);
            if (tblVisitFeedbackTOList != null && tblVisitFeedbackTOList.Count == 1)
                return tblVisitFeedbackTOList[0];
            else
                return null;
        }

        public static List<TblVisitFeedbackTO> ConvertDTToList(DataTable tblVisitFeedbackTODT)
        {
            List<TblVisitFeedbackTO> tblVisitFeedbackTOList = new List<TblVisitFeedbackTO>();
            if (tblVisitFeedbackTODT != null)
            {

            }
            return tblVisitFeedbackTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO)
        {
            return TblVisitFeedbackDAO.InsertTblVisitFeedback(tblVisitFeedbackTO);
        }

        public static int InsertTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFeedbackDAO.InsertTblVisitFeedback(tblVisitFeedbackTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO)
        {
            return TblVisitFeedbackDAO.UpdateTblVisitFeedback(tblVisitFeedbackTO);
        }

        public static int UpdateTblVisitFeedback(TblVisitFeedbackTO tblVisitFeedbackTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFeedbackDAO.UpdateTblVisitFeedback(tblVisitFeedbackTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblVisitFeedback()
        {
            return TblVisitFeedbackDAO.DeleteTblVisitFeedback();
        }

        public static int DeleteTblVisitFeedback( SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFeedbackDAO.DeleteTblVisitFeedback( conn, tran);
        }

        #endregion

    }
}
