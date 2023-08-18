using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ILoadingBL
    {
        TblTRLoadingTO GetLoadingDetails(Int32 idLoading);
        List<TblTRLoadingTO> GetLoadingDetailsList(LoadingFilterTO loadingFilterTO);
        List<TblTRLoadingTO> GetVehicleWiseLoadingDetailsTOList(LoadingFilterTO loadingFilterTO);
        ResultMessage AddLoading(TblTRLoadingTO tblTRLoadingTO);
        ResultMessage UpdateLoadingStatus(TblTRLoadingTO tblTRLoadingTO);
        ResultMessage UpdateLoading(TblTRLoadingTO tblTRLoadingTO);
        List<TblTRLoadingWeighingTO> GetWeighingDetails(Int32 LoadingId, Int32 LoadingTypeId);
        ResultMessage PostWeighingDetails(TblTRLoadingWeighingTO tblTRLoadingWeighingTO);
        ResultMessage UpdateLoadingVehicle(TblTRLoadingTO tblTRLoadingTO); 
        ResultMessage CloseLoadingTrasaction(TblTRLoadingTO tblTRLoadingTO);
        ResultMessage UpdateWeighingRemark(TblTRLoadingTO tblTRLoadingTO);
        List<TblTRLoadingTO> GetVehicleLoadingHistory(Int32 IdVehicle);
        ResultMessage DownloadSLASummaryReport(UnloadingSLAFilterTO unloadingSLAFilterTO);
        ResultMessage DownloadSLAFurnaceSummaryReport(UnloadingSLAFilterTO unloadingSLAFilterTO);
        ResultMessage DownloadVehicleWiseLoadingReport(LoadingFilterTO loadingFilterTO);
        ResultMessage DownloadWBDeviationReport(LoadingFilterTO loadingFilterTO);
        ResultMessage DownloadPOVsActualLoadingReport(LoadingFilterTO loadingFilterTO);
        ResultMessage DownloadLoadingUnloadingMasterReport(LoadingFilterTO loadingFilterTO);
        ResultMessage DownloadTallyStockReport(LoadingFilterTO loadingFilterTO);
        ResultMessage AddSLA(TblUnloadingSLATO tblUnloadingSLATO);
        TblUnloadingSLATO GetUnloadingSLADetailsTO(Int32 idSLA);
        List<TblUnloadingSLATO> GetUnloadingSLADetailsList(UnloadingSLAFilterTO unloadingSLAFilterTO);
        ResultMessage MigrateTransferRequest();
        List<TblTRLoadingWeighingTO> GetWeighingDetails(String LoadingIdStr, String LoadingTypeIdStr);
        List<TblMaterialTypeReport> GetMaterialTypeReport();
    }
}
