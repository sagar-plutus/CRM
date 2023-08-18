using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblQualityPhaseDAO
    {
        String SqlSelectQuery();
         int DeleteAllQualityPhaseAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblQualityPhaseTO> SelectAllTblQualityPhase(int PurchaseScheduleSummaryId);
        List<TblQualityPhaseTO> SelectAllTblQualityPhase();
        List<TblQualityPhaseTO> SelectTblQualityPhase(Int32 idTblQualityPhase);
        List<TblQualityPhaseTO> SelectTblQualityPhaseList(Int32 purchaseScheduleSummaryId, Int32 isActive);
        List<TblQualityPhaseTO> SelectAllTblQualityPhase(SqlConnection conn, SqlTransaction tran);
        List<TblQualityPhaseTO> ConvertDTToList(SqlDataReader tblQualityPhaseTODT);
        int InsertTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO);
        int InsertTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblQualityPhaseTO tblQualityPhaseTO, SqlCommand cmdInsert);
        int UpdateTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO);

        int UpdateTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblQualityPhaseTO tblQualityPhaseTO, SqlCommand cmdUpdate);
        int UpdateTblQualityPhaseDeact(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommandDeAct(TblQualityPhaseTO tblQualityPhaseTO, SqlCommand cmdUpdate);
        int DeleteTblQualityPhase(Int32 idTblQualityPhase);
        int SelectFromDtlsAndQuality(int purchaseScheduleSummaryId, int vehiclePhaseId, int flagTypeId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblQualityPhase(Int32 idTblQualityPhase, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTblQualityPhase, SqlCommand cmdDelete);
         List<DropDownTO> GetAllIdsForSampleType(int purchaseScheduleSummaryId, int vehiclePhaseId, int flagTypeId, int QualitySampleTypeId);
        List<DropDownTO> GetFlagType();
    }
 }