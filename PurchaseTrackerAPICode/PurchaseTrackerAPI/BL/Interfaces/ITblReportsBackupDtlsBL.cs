using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblReportsBackupDtlsBL
    {
        List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtls();
        List<TblReportsBackupDtlsTO> SelectReportBackupDtls(string reportName, DateTime currentDate);
        List<TblReportsBackupDtlsTO> SelectReportBackupDateDtls(string reportName);
        List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtlsList();
        TblReportsBackupDtlsTO SelectTblReportsBackupDtlsTO(Int32 idReportBackup);
        int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO);
        int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO);
        int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblReportsBackupDtls(Int32 idReportBackup);
        int DeleteTblReportsBackupDtls(Int32 idReportBackup, SqlConnection conn, SqlTransaction tran);

        DateTime SelectReportMinBackUpdate();

    }
}
