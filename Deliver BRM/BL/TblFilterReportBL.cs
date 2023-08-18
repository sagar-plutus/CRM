using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblFilterReportBL
    {
        #region Selection
        public static List<TblFilterReportTO> SelectAllTblFilterReport()
        {
            return TblFilterReportDAO.SelectAllTblFilterReport();
        }

        public static List<TblFilterReportTO> SelectAllTblFilterReportList()
        {
            List<TblFilterReportTO> tblFilterReportTODT = TblFilterReportDAO.SelectAllTblFilterReport();
            return tblFilterReportTODT;
        }

        public static TblFilterReportTO SelectTblFilterReportTO(Int32 idFilterReport)
        {
            TblFilterReportTO tblFilterReportTODT = TblFilterReportDAO.SelectTblFilterReport(idFilterReport);
            if (tblFilterReportTODT != null)
                return tblFilterReportTODT;
            else
                return null;
        }

        public static List<TblFilterReportTO> SelectTblFilterReportList(Int32 reportId)
        {
            List<TblFilterReportTO>  tblFilterReportTODTList = TblFilterReportDAO.SelectTblFilterReportList(reportId);
            if (tblFilterReportTODTList != null)
                return tblFilterReportTODTList;
            else
                return null;
        }




        #endregion

        #region Insertion
        public static int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO)
        {
            return TblFilterReportDAO.InsertTblFilterReport(tblFilterReportTO);
        }

        public static int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFilterReportDAO.InsertTblFilterReport(tblFilterReportTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO)
        {
            return TblFilterReportDAO.UpdateTblFilterReport(tblFilterReportTO);
        }

        public static int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFilterReportDAO.UpdateTblFilterReport(tblFilterReportTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblFilterReport(Int32 idFilterReport)
        {
            return TblFilterReportDAO.DeleteTblFilterReport(idFilterReport);
        }

        public static int DeleteTblFilterReport(Int32 idFilterReport, SqlConnection conn, SqlTransaction tran)
        {
            return TblFilterReportDAO.DeleteTblFilterReport(idFilterReport, conn, tran);
        }

        #endregion
    }
}
