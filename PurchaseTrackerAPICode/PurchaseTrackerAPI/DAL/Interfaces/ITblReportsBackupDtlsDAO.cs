using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblReportsBackupDtlsDAO
    {
        String SqlSelectQuery();
        List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtls();
        List<TblReportsBackupDtlsTO> SelectReportBackupDtls(string reportName, DateTime currentDate);
        List<TblReportsBackupDtlsTO> SelectReportBackupDateDtls(string reportName);
        List<TblReportsBackupDtlsTO> SelectTblReportsBackupDtls(Int32 idReportBackup);
        List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtls(SqlConnection conn, SqlTransaction tran);
        int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO);
        int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlCommand cmdInsert);
        int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO);
        int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblReportsBackupDtls(Int32 idReportBackup);
        int DeleteTblReportsBackupDtls(Int32 idReportBackup, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idReportBackup, SqlCommand cmdDelete);
        DateTime SelectReportMinBackupDate();

    }
}
