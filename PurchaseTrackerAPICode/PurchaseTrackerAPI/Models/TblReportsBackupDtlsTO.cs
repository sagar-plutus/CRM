using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblReportsBackupDtlsTO
    {
        #region Declarations
        Int32 idReportBackup;
        Int32 isBackUp;
        DateTime backupDate;
        String reportName;
        #endregion

        #region Constructor
        public TblReportsBackupDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdReportBackup
        {
            get { return idReportBackup; }
            set { idReportBackup = value; }
        }
        public Int32 IsBackUp
        {
            get { return isBackUp; }
            set { isBackUp = value; }
        }
        public DateTime BackupDate
        {
            get { return backupDate; }
            set { backupDate = value; }
        }
        public String ReportName
        {
            get { return reportName; }
            set { reportName = value; }
        }
        #endregion
    }
}
