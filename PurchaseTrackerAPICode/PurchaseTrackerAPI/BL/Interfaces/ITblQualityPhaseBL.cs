using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblQualityPhaseBL
    {
        int DeleteAllQualityPhaseAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveCompletedPhaseSampleListsagainstPurrchaseScheduleSummaryID(List<TblQualityPhaseDtlsTO> tblQualityPhaseDtlsTOList, int loginUserId);
        List<DropDownTO> GetFlagType();
        List<TblQualityPhaseTO> GetQualityFlagCheckLists(int idPurchaseScheduleSummary, int userId, int qualityFormTypeE, int flagTypeId);
        List<DropDownTO> GetAllIdsForSampleType(int purchaseScheduleSummaryId, int vehiclePhaseId, int flagTypeId, int QualitySampleTypeId);
         ResultMessage SavePhaseSampleListsagainstPurrchaseScheduleSummaryID(List<TblQualityPhaseTO> tblQualityPhaseTOList, int loginUserId);
        List<TblQualityPhaseTO> SelectAllTblQualityPhase();
        List<TblQualityPhaseTO> SelectAllTblQualityPhaseList(int PurchaseScheduleSummaryId);
        List<TblQualityPhaseTO> SelectAllTblQualityPhaseList(Int32 purchaseScheduleSummaryId, Int32 isActive);
        TblQualityPhaseTO SelectTblQualityPhaseTO(Int32 idTblQualityPhase);
        int InsertTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO);
        int InsertTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO);
        int UpdateTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblQualityPhaseDeAct(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblQualityPhase(Int32 idTblQualityPhase);
        int DeleteTblQualityPhase(Int32 idTblQualityPhase, SqlConnection conn, SqlTransaction tran);
        void ResetAllPreviousNotification(int defId, string SourceEntityId);
        void ResetAllAlerts(TblAlertInstanceTO tblAlertInstanceTO);

    }
}