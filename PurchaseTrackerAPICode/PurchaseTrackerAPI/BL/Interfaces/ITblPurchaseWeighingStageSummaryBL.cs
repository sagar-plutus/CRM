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
    public interface ITblPurchaseWeighingStageSummaryBL
    {

        
        ResultMessage UpdateGraderId(int loginUserId, int purchaseScheduleSummaryId);
        ResultMessage UpdateRecoveryEngineerId(int loginUserId, int purchaseScheduleSummaryId);
        ResultMessage PostUpdatePhotographerId(int loginUserId, int purchaseScheduleSummaryId);
        List<TblPurchaseWeighingStageSummaryTO> SelectAllTblPurchaseWeighingStageSummary();
        List<TblPurchaseWeighingStageSummaryTO> SelectAllTblPurchaseWeighingStageSummaryList();
        TblPurchaseWeighingStageSummaryTO SelectTblPurchaseWeighingStageSummaryTO(Int32 idPurchaseWeighingStage);
        TblPurchaseWeighingStageSummaryTO SelectTblPurchaseWeighingStageSummaryTO(Int32 idPurchaseWeighingStage, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleId(Int32 purchaseScheduleId, bool fromWeighing);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleIdForWeighingReport(Int32 purchaseScheduleId, bool fromWeighing);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, Boolean isGetAllWeighingDtls = false);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, SqlConnection conn, SqlTransaction tran, Boolean isGetAllWeighingDtls = false);
        List<TblPurchaseWeighingStageSummaryTO> GetVehWtDetailsForWeighingMachine(Int32 purchaseScheduleId, string weightTypeId, string wtMachineIds, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightAndUnloadingMaterialDetails(Int32 purchaseScheduleId, Int32 formTypeE);
        List<GradeInfoTO> GradeInfoTOListForSudharReport(int purchaseScheduleId);
        List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightAndUnloadingMaterialDetailsTOList(Int32 purchaseScheduleId);
        int InsertTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
        int InsertTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        //ResultMessage InsertWeighingDetails(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
        int UpdateTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
        int UpdateTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblPurchaseWeighingStageSummaryRecoveryDtls(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
        int DeleteTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage);
        int DeleteTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage, SqlConnection conn, SqlTransaction tran);
        int DeleteAllWeighingStageAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseWeighingStageSummaryForIsValid(int rootScheduleId, SqlConnection conn, SqlTransaction tran);
        int UpdateUnlodingEndTime(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran);

        List<UnloadedTimeReportTO> UnlodingTimeRport(string fromDate, string toDate, Int32 isForWeighingPointWise,Boolean isForReport,String purchaseManagerIds);
        int UpdateUnlodingStartTime(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO);
    }
}