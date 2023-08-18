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
    public class TblVisitIssueReasonsBL
    {
        #region Selection
        public static DataTable SelectAllTblVisitIssueReasons()
        {
            return TblVisitIssueReasonsDAO.SelectAllTblVisitIssueReasons();
        }

        public static List<TblVisitIssueReasonsTO> SelectAllTblVisitIssueReasonsList()
        {
            DataTable tblVisitIssueReasonsTODT = TblVisitIssueReasonsDAO.SelectAllTblVisitIssueReasons();
            return ConvertDTToList(tblVisitIssueReasonsTODT);
        }

        public static List<TblVisitIssueReasonsTO> SelectAllVisitIssueReasonsListForDropDown()
        {
            List<TblVisitIssueReasonsTO> visitIssueReasonTOList = TblVisitIssueReasonsDAO.SelectAllTblVisitIssueReasonsForDropDOwn();
            if (visitIssueReasonTOList != null)
                return visitIssueReasonTOList;
            else
                return null;
        }

        public static TblVisitIssueReasonsTO SelectTblVisitIssueReasonsTO(Int32 idVisitIssueReasons)
        {
            DataTable tblVisitIssueReasonsTODT = TblVisitIssueReasonsDAO.SelectTblVisitIssueReasons(idVisitIssueReasons);
            List<TblVisitIssueReasonsTO> tblVisitIssueReasonsTOList = ConvertDTToList(tblVisitIssueReasonsTODT);
            if (tblVisitIssueReasonsTOList != null && tblVisitIssueReasonsTOList.Count == 1)
                return tblVisitIssueReasonsTOList[0];
            else
                return null;
        }

        public static List<TblVisitIssueReasonsTO> ConvertDTToList(DataTable tblVisitIssueReasonsTODT)
        {
            List<TblVisitIssueReasonsTO> tblVisitIssueReasonsTOList = new List<TblVisitIssueReasonsTO>();
            if (tblVisitIssueReasonsTODT != null)
            {
                //for (int rowCount = 0; rowCount < tblVisitIssueReasonsTODT.Rows.Count; rowCount++)
                //{
                //    TblVisitIssueReasonsTO tblVisitIssueReasonsTONew = new TblVisitIssueReasonsTO();
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["idVisitIssueReasons"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.IdVisitIssueReasons = Convert.ToInt32(tblVisitIssueReasonsTODT.Rows[rowCount]["idVisitIssueReasons"].ToString());
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["issueTypeId"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.IssueTypeId = Convert.ToInt32(tblVisitIssueReasonsTODT.Rows[rowCount]["issueTypeId"].ToString());
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.IsActive = Convert.ToInt32(tblVisitIssueReasonsTODT.Rows[rowCount]["isActive"].ToString());
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["visitIssueReasonName"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.VisitIssueReasonName = Convert.ToString(tblVisitIssueReasonsTODT.Rows[rowCount]["visitIssueReasonName"].ToString());
                //    if (tblVisitIssueReasonsTODT.Rows[rowCount]["visitIssueReasonDesc"] != DBNull.Value)
                //        tblVisitIssueReasonsTONew.VisitIssueReasonDesc = Convert.ToString(tblVisitIssueReasonsTODT.Rows[rowCount]["visitIssueReasonDesc"].ToString());
                //    tblVisitIssueReasonsTOList.Add(tblVisitIssueReasonsTONew);
                //}
            }
            return tblVisitIssueReasonsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitIssueReasons( TblVisitIssueReasonsTO tblVisitIssueReasonsTO)
        {
            return TblVisitIssueReasonsDAO.InsertTblVisitIssueReasons(tblVisitIssueReasonsTO);
        }

        public static int InsertTblVisitIssueReasons(ref TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitIssueReasonsDAO.InsertTblVisitIssueReasons(ref tblVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO)
        {
            return TblVisitIssueReasonsDAO.UpdateTblVisitIssueReasons(tblVisitIssueReasonsTO);
        }

        public static int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitIssueReasonsDAO.UpdateTblVisitIssueReasons(tblVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            return TblVisitIssueReasonsDAO.DeleteTblVisitIssueReasons(idVisitIssueReasons);
        }

        public static int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitIssueReasonsDAO.DeleteTblVisitIssueReasons(idVisitIssueReasons, conn, tran);
        }

        #endregion

    }
}
