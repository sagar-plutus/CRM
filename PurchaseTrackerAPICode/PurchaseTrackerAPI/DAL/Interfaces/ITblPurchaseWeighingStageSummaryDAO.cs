using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseWeighingStageSummaryDAO
    {
        String SqlSelectQuery();

        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleIdForWeighingReport(Int32 purchaseScheduleId, bool fromWeighing);
        List<TblPurchaseWeighingStageSummaryTO> SelectAllTblPurchaseWeighingStageSummary();
        int updateIsValidFlagToInvalid(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        TblPurchaseWeighingStageSummaryTO SelectTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage);
        TblPurchaseWeighingStageSummaryTO SelectTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleId(Int32 purchaseScheduleId, bool fromWeighing);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, SqlConnection conn, SqlTransaction tran, Boolean isGetAllWeighingDtls);
        List<TblPurchaseWeighingStageSummaryTO> GetVehWtDetailsForWeighingMachine(Int32 purchaseScheduleId, string weightTypeId, string wtMachineIds, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, Boolean isGetAllWeighingDtls);
        List<TblPurchaseWeighingStageSummaryTO> SelectAllTblPurchaseWeighingStageSummary(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseWeighingStageSummaryTO> ConvertDTToList(SqlDataReader tblPurchaseWeighingStageSummaryTODT);
        int InsertTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
        int InsertTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlCommand cmdInsert);
        int UpdateRecoveryEngineerId(int loginUserId, int purchaseScheduleSummaryId);
        int PostUpdatePhotographerId(int loginUserId, int purchaseScheduleSummaryId);
        int UpdateGraderId(int loginUserId, int purchaseScheduleSummaryId);
        int UpdateSupervisorId(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);
        int UpdateTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
        int UpdateTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlCommand cmdUpdate);
        int UpdateTblPurchaseWeighingStageSummaryRecoveryDtls(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage);
        int DeleteTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchaseWeighingStage, SqlCommand cmdDelete);
        int DeleteAllWeighingStageAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseWeighingStageSummaryForIsValid(int rootScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseWeighingStageSummaryTO> GetVehWtDetailsForWeighingMachine(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        int UpdateUnlodingEndTime(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseWeighingStageSummaryTO> SelectTblPurchaseWeighingStageSummary(DateTime fromDate, DateTime toDate,String purchaseManagerIds);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetails(Int32 purchaseScheduleId, Int32 idPurchaseWeighingStage);
        int UpdateUnlodingStartTime(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);

    }
}