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
    public class TblReportsBackupDtlsBL
    {
        #region Selection
        public static List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtls()
        {
            return TblReportsBackupDtlsDAO.SelectAllTblReportsBackupDtls();
        }

        public static List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtlsList()
        {
            return TblReportsBackupDtlsDAO.SelectAllTblReportsBackupDtls();
        }
        public static List<TblReportsBackupDtlsTO> SelectReportBackupDtls(string reportName, DateTime currentDate)
        {
            return TblReportsBackupDtlsDAO.SelectReportBackupDtls(reportName, currentDate);
        }
        public static List<TblReportsBackupDtlsTO> SelectReportBackupDateDtls(string reportName)
        {
            return TblReportsBackupDtlsDAO.SelectReportBackupDateDtls(reportName);
        }

        public static TblReportsBackupDtlsTO SelectTblReportsBackupDtlsTO(Int32 idReportBackup)
        {
            List<TblReportsBackupDtlsTO> tblReportsBackupDtlsTOList = TblReportsBackupDtlsDAO.SelectTblReportsBackupDtls(idReportBackup);
            if (tblReportsBackupDtlsTOList != null && tblReportsBackupDtlsTOList.Count == 1)
                return tblReportsBackupDtlsTOList[0];
            else
                return null;
        }

        public static DateTime SelectReportMinBackUpdate()
        {
            return TblReportsBackupDtlsDAO.SelectReportMinBackupDate();
        }

        #endregion

        #region Insertion
        public static int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO)
        {
            return TblReportsBackupDtlsDAO.InsertTblReportsBackupDtls(tblReportsBackupDtlsTO);
        }

        public static int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblReportsBackupDtlsDAO.InsertTblReportsBackupDtls(tblReportsBackupDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO)
        {
            return TblReportsBackupDtlsDAO.UpdateTblReportsBackupDtls(tblReportsBackupDtlsTO);
        }

        public static int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblReportsBackupDtlsDAO.UpdateTblReportsBackupDtls(tblReportsBackupDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblReportsBackupDtls(Int32 idReportBackup)
        {
            return TblReportsBackupDtlsDAO.DeleteTblReportsBackupDtls(idReportBackup);
        }

        public static int DeleteTblReportsBackupDtls(Int32 idReportBackup, SqlConnection conn, SqlTransaction tran)
        {
            return TblReportsBackupDtlsDAO.DeleteTblReportsBackupDtls(idReportBackup, conn, tran);
        }

        #endregion
    }
}
