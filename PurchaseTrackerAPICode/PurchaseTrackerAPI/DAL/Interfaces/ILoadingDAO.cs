using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ILoadingDAO
    {
        TblTRLoadingTO GetLoadingDetailsTO(Int32 idLoading);
        List<TblTRLoadingTO> GetLoadingDetailsTOList(LoadingFilterTO loadingFilterTO);
        List<TblTRLoadingTO> GetVehicleWiseLoadingDetailsTOList(LoadingFilterTO loadingFilterTO);
        int InsertLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);
        int UpdateLoadingStatus(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);
        int UpdateLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);
        List<TblTRLoadingWeighingTO> GetWeighingDetails(Int32 LoadingId, Int32 LoadingTypeId);
        int InsertWeighing(TblTRLoadingWeighingTO tblTRLoadingWeighingTO, SqlConnection conn, SqlTransaction tran);
        List<TblTRLoadingWeighingTO> GetWeighingDetails(Int32 LoadingId, String LoadingTypeIdStr, SqlConnection conn, SqlTransaction tran);
        int InsertLoadingHistory(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);
        TblTRLoadingTO GetLoadingDetailsTO(Int32 idLoading, SqlConnection conn, SqlTransaction tran);
        int UpdateLoadingVehicle(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);
        int UpdateWeighingRemark(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);
        List<TblTRLoadingTO> GetVehicleLoadingHistory(Int32 IdVehicle);
        List<TblTRLoadingTO> GetVehicleWiseLoadingReportDetails(LoadingFilterTO loadingFilterTO, String WhereClause = "");
        List<TblTRLoadingWeighingTO> GetWeighingNetWeightDetails(String LoadingIdStr, Int32 LoadingTypeId);
        TblUnloadingSLATO GetUnloadingSLADetailsTO(Int32 idSLA);
        List<TblMaterialTypeReport> GetMaterialTypeReport();
        TblUnloadingSLATO GetUnloadingSLADetailsTOByLoadingId(Int32 loadingId);
        int InsertSLA(TblUnloadingSLATO tblUnloadingSLATO, SqlConnection conn, SqlTransaction tran);
        List<TblUnloadingSLATO> GetUnloadingSLADetailsList(UnloadingSLAFilterTO unloadingSLAFilterTO);
        List<TblTRLoadingTO> GetMaterialAndLocationWiseLoadingDtls(LoadingFilterTO loadingFilterTO);
        List<TblTRLoadingWeighingTO> GetWeighingDetails(String LoadingIdStr, String LoadingTypeIdStr);
        List<TblTRLoadingTO> GetTRLoadingHistoryDetails(String statusIdStr, string loadingIdStr);
        List<TblUnloadingSLATO> GetUnloadingSLAReportDetails(UnloadingSLAFilterTO unloadingSLAFilterTO, String WhereClause = "");
        List<TblUnloadingSLATO> GetUnloadingSLAFurnaceReportDetails(UnloadingSLAFilterTO unloadingSLAFilterTO, String WhereClause = "");
        int InsertFinalLoadingHistory(String loadingIdStr, SqlConnection conn, SqlTransaction tran);
        int InsertFinalLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);
        int InsertFinalWeighing(String loadingIdStr, SqlConnection conn, SqlTransaction tran);
        int DeleteTRLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran);
        int DeleteTRLoadingWeighing(String loadingIdStr, SqlConnection conn, SqlTransaction tran);
        int DeleteTRLoadingHistory(String loadingIdStr, SqlConnection conn, SqlTransaction tran);
        int DeleteTRLoading(String idLoadingStr, SqlConnection conn, SqlTransaction tran);
        int DeleteTransferRequest(String idTransferRequestStr, SqlConnection conn, SqlTransaction tran);
        int InsertFinalUnloadingSLA(String loadingIdStr, SqlConnection conn, SqlTransaction tran);
        int DeleteUnloadingSLA(String loadingIdStr, SqlConnection conn, SqlTransaction tran);
    }
}
