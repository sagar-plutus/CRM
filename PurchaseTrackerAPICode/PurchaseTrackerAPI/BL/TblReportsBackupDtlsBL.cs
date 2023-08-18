using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblReportsBackupDtlsBL : ITblReportsBackupDtlsBL
    {
        private readonly ITblReportsBackupDtlsDAO _iTblReportsBackupDtlsDAO;
        public TblReportsBackupDtlsBL(ITblReportsBackupDtlsDAO iTblReportsBackupDtlsDAO)
        {
            _iTblReportsBackupDtlsDAO = iTblReportsBackupDtlsDAO;
        }

        #region Selection
        public List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtls()
        {
            return _iTblReportsBackupDtlsDAO.SelectAllTblReportsBackupDtls();
        }
        public DateTime SelectReportMinBackUpdate()
        {
            return _iTblReportsBackupDtlsDAO.SelectReportMinBackupDate();
        }

        public List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtlsList()
        {
            return _iTblReportsBackupDtlsDAO.SelectAllTblReportsBackupDtls();
        }
        public List<TblReportsBackupDtlsTO> SelectReportBackupDtls(string reportName, DateTime currentDate)
        {
            return _iTblReportsBackupDtlsDAO.SelectReportBackupDtls(reportName, currentDate);
        }
        public List<TblReportsBackupDtlsTO> SelectReportBackupDateDtls(string reportName)
        {
            return _iTblReportsBackupDtlsDAO.SelectReportBackupDateDtls(reportName);
        }

        public TblReportsBackupDtlsTO SelectTblReportsBackupDtlsTO(Int32 idReportBackup)
        {
            List<TblReportsBackupDtlsTO> tblReportsBackupDtlsTOList = _iTblReportsBackupDtlsDAO.SelectTblReportsBackupDtls(idReportBackup);
            if(tblReportsBackupDtlsTOList != null && tblReportsBackupDtlsTOList.Count == 1)
                return tblReportsBackupDtlsTOList[0];
            else
                return null;
        }

       
        #endregion
        
        #region Insertion
        public int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO)
        {
            return _iTblReportsBackupDtlsDAO.InsertTblReportsBackupDtls(tblReportsBackupDtlsTO);
        }

        public  int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsBackupDtlsDAO.InsertTblReportsBackupDtls(tblReportsBackupDtlsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO)
        {
            return _iTblReportsBackupDtlsDAO.UpdateTblReportsBackupDtls(tblReportsBackupDtlsTO);
        }

        public  int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsBackupDtlsDAO.UpdateTblReportsBackupDtls(tblReportsBackupDtlsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblReportsBackupDtls(Int32 idReportBackup)
        {
            return _iTblReportsBackupDtlsDAO.DeleteTblReportsBackupDtls(idReportBackup);
        }

        public  int DeleteTblReportsBackupDtls(Int32 idReportBackup, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblReportsBackupDtlsDAO.DeleteTblReportsBackupDtls(idReportBackup, conn, tran);
        }

        #endregion
        
    }
}
