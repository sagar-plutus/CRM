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
    public class DimVisitIssueReasonsBL
    {
        #region Selection
        public static DataTable SelectAllDimVisitIssueReasons()
        {
            return DimVisitIssueReasonsDAO.SelectAllDimVisitIssueReasons();
        }

        public static List<DimVisitIssueReasonsTO> SelectAllDimVisitIssueReasonsList()
        {
            DataTable dimVisitIssueReasonsTODT = DimVisitIssueReasonsDAO.SelectAllDimVisitIssueReasons();
            return ConvertDTToList(dimVisitIssueReasonsTODT);
        }

        public static DimVisitIssueReasonsTO SelectDimVisitIssueReasonsTO(Int32 idVisitIssueReasons)
        {
            DataTable dimVisitIssueReasonsTODT = DimVisitIssueReasonsDAO.SelectDimVisitIssueReasons(idVisitIssueReasons);
            List<DimVisitIssueReasonsTO> dimVisitIssueReasonsTOList = ConvertDTToList(dimVisitIssueReasonsTODT);
            if (dimVisitIssueReasonsTOList != null && dimVisitIssueReasonsTOList.Count == 1)
                return dimVisitIssueReasonsTOList[0];
            else
                return null;
        }

        public static List<DimVisitIssueReasonsTO> ConvertDTToList(DataTable dimVisitIssueReasonsTODT)
        {
            List<DimVisitIssueReasonsTO> dimVisitIssueReasonsTOList = new List<DimVisitIssueReasonsTO>();
            if (dimVisitIssueReasonsTODT != null)
            {
            }
            return dimVisitIssueReasonsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO)
        {
            return DimVisitIssueReasonsDAO.InsertDimVisitIssueReasons(dimVisitIssueReasonsTO);
        }

        public static int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVisitIssueReasonsDAO.InsertDimVisitIssueReasons(dimVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO)
        {
            return DimVisitIssueReasonsDAO.UpdateDimVisitIssueReasons(dimVisitIssueReasonsTO);
        }

        public static int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimVisitIssueReasonsDAO.UpdateDimVisitIssueReasons(dimVisitIssueReasonsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            return DimVisitIssueReasonsDAO.DeleteDimVisitIssueReasons(idVisitIssueReasons);
        }

        public static int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran)
        {
            return DimVisitIssueReasonsDAO.DeleteDimVisitIssueReasons(idVisitIssueReasons, conn, tran);
        }

        #endregion

    }
}
