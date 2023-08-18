using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblQualityPhaseDtlsDAO
    {
        String SqlSelectQuery();
         int DeleteAllQualityPhaseDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtls();
        List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtls(Int32 tblQualityPhaseId);
        List<TblQualityPhaseDtlsTO> SelectTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls);
        int SelectAlertDefinationID(int qualitySampleTypeId, int flag, SqlConnection conn, SqlTransaction tran);
        List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtls(SqlConnection conn, SqlTransaction tran);
        List<TblQualityPhaseDtlsTO> ConvertDTToList(SqlDataReader tblQualityPhaseDtlsTODT);
        int InsertTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO);
        int InsertTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteInsertionCommand(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlCommand cmdInsert);
        int UpdateTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO);
        string GetVehicleNameForNotification(int purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls);
        int DeleteTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTblQualityPhaseDtls, SqlCommand cmdDelete);

    }
}