using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoadingController : Controller
    {
        #region Declaration & Constructor
        private readonly ILoadingBL _iLoadingBL;
        public LoadingController(ILoadingBL iLoadingBL)
        {
            _iLoadingBL = iLoadingBL;
        }
        #endregion
        #region GET
        [Route("GetLoadingDetails")]
        [HttpGet]
        public TblTRLoadingTO GetLoadingDetails(Int32 idLoading)
        {
            return _iLoadingBL.GetLoadingDetails(idLoading);
        }
        [Route("GetUnloadingSLADetailsTO")]
        [HttpGet]
        public TblUnloadingSLATO GetUnloadingSLADetailsTO(Int32 idSLA)
        {
            return _iLoadingBL.GetUnloadingSLADetailsTO(idSLA);
        }
        [Route("GetUnloadingSLADetailsList")]
        [HttpPost]
        public List<TblUnloadingSLATO> GetUnloadingSLADetailsList([FromBody] UnloadingSLAFilterTO unloadingSLAFilterTO)
        {
            return _iLoadingBL.GetUnloadingSLADetailsList(unloadingSLAFilterTO);
        }
        [Route("GetLoadingDetailsList")]
        [HttpPost]
        public List<TblTRLoadingTO> GetLoadingDetailsList([FromBody] LoadingFilterTO loadingFilterTO)
        {
            return _iLoadingBL.GetLoadingDetailsList(loadingFilterTO);
        }
        [Route("GetVehicleWiseLoadingDetailsTOList")]
        [HttpPost]
        public List<TblTRLoadingTO> GetVehicleWiseLoadingDetailsTOList([FromBody] LoadingFilterTO loadingFilterTO)
        {
            return _iLoadingBL.GetVehicleWiseLoadingDetailsTOList(loadingFilterTO);
        }
        [Route("GetWeighingDetails")]
        [HttpGet]
        public List<TblTRLoadingWeighingTO> GetWeighingDetails(Int32 LoadingId, Int32 LoadingTypeId)
        {
            return _iLoadingBL.GetWeighingDetails(LoadingId, LoadingTypeId);
        }
        [Route("GetWeighingDetailsByLoadingTypeIdStr")]
        [HttpGet]
        public List<TblTRLoadingWeighingTO> GetWeighingDetailsByLoadingTypeIdStr(String LoadingIdStr, String LoadingTypeIdStr)
        {
            return _iLoadingBL.GetWeighingDetails(LoadingIdStr, LoadingTypeIdStr);
        }
        [Route("GetVehicleLoadingHistory")]
        [HttpGet]
        public List<TblTRLoadingTO> GetVehicleLoadingHistory(Int32 IdVehicle)
        {
            return _iLoadingBL.GetVehicleLoadingHistory(IdVehicle);
        }

        [Route("GetMaterialTypeReport")]
        [HttpGet]
        public List<TblMaterialTypeReport> GetMaterialTypeReport()
        {
            return _iLoadingBL.GetMaterialTypeReport();
        }

        #endregion
        #region POST
        [Route("AddLoading")]
        [HttpPost]
        public ResultMessage AddLoading([FromBody] TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.AddLoading(tblTRLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to AddLoading");
                return resultMessage;
            }
        }
        [Route("UpdateLoadingStatus")]
        [HttpPost]
        public ResultMessage UpdateLoadingStatus([FromBody] TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.UpdateLoadingStatus(tblTRLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to UpdateLoadingStatus");
                return resultMessage;
            }
        }
        [Route("UpdateLoadingVehicle")]
        [HttpPost]
        public ResultMessage UpdateLoadingVehicle([FromBody] TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.UpdateLoadingVehicle(tblTRLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to UpdateLoadingVehicle");
                return resultMessage;
            }
        }
        [Route("UpdateWeighingRemark")]
        [HttpPost]
        public ResultMessage UpdateWeighingRemark([FromBody] TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.UpdateWeighingRemark(tblTRLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to UpdateWeighingRemark");
                return resultMessage;
            }
        }
        [Route("CloseLoadingTrasaction")]
        [HttpPost]
        public ResultMessage CloseLoadingTrasaction([FromBody] TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.CloseLoadingTrasaction(tblTRLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to CloseLoadingTrasaction");
                return resultMessage;
            }
        }
        [Route("UpdateLoading")]
        [HttpPost]
        public ResultMessage UpdateLoading([FromBody] TblTRLoadingTO tblTRLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.UpdateLoading(tblTRLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to UpdateLoadingStatus");
                return resultMessage;
            }
        }
        [Route("PostWeighingDetails")]
        [HttpPost]
        public ResultMessage PostWeighingDetails([FromBody] TblTRLoadingWeighingTO tblTRLoadingWeighingTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.PostWeighingDetails(tblTRLoadingWeighingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to PostWeighingDetails");
                return resultMessage;
            }
        }
        [Route("DownloadSLASummaryReport")]
        [HttpPost]
        public ResultMessage DownloadSLASummaryReport([FromBody] UnloadingSLAFilterTO unloadingSLAFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.DownloadSLASummaryReport(unloadingSLAFilterTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to DownloadSLASummaryReport");
                return resultMessage;
            }
        }

        [Route("DownloadSLAFurnaceSummaryReport")]
        [HttpPost]
        public ResultMessage DownloadSLAFurnaceSummaryReport([FromBody] UnloadingSLAFilterTO unloadingSLAFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.DownloadSLAFurnaceSummaryReport(unloadingSLAFilterTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to DownloadSLASummaryReport");
                return resultMessage;
            }
        }
        [Route("DownloadVehicleWiseLoadingReport")]
        [HttpPost]
        public ResultMessage DownloadVehicleWiseLoadingReport([FromBody] LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.DownloadVehicleWiseLoadingReport(loadingFilterTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to DownloadVehicleWiseLoadingReport");
                return resultMessage;
            }
        }
        [Route("DownloadWBDeviationReport")]
        [HttpPost]
        public ResultMessage DownloadWBDeviationReport([FromBody] LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.DownloadWBDeviationReport(loadingFilterTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to DownloadWBDeviationReport");
                return resultMessage;
            }
        }
        [Route("DownloadPOVsActualLoadingReport")]
        [HttpPost]
        public ResultMessage DownloadPOVsActualLoadingReport([FromBody] LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.DownloadPOVsActualLoadingReport(loadingFilterTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to DownloadPOVsActualLoadingReport");
                return resultMessage;
            }
        }
        [Route("DownloadLoadingUnloadingMasterReport")]
        [HttpPost]
        public ResultMessage DownloadLoadingUnloadingMasterReport([FromBody] LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.DownloadLoadingUnloadingMasterReport(loadingFilterTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to DownloadLoadingUnloadingMasterReport");
                return resultMessage;
            }
        }
        [Route("DownloadTallyStockReport")]
        [HttpPost]
        public ResultMessage DownloadTallyStockReport([FromBody] LoadingFilterTO loadingFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.DownloadTallyStockReport(loadingFilterTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to DownloadTallyStockReport");
                return resultMessage;
            }
        }
        [Route("AddSLA")]
        [HttpPost]
        public ResultMessage AddSLA([FromBody] TblUnloadingSLATO tblUnloadingSLATO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (!ModelState.IsValid)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.data = BadRequest(ModelState);
                    return resultMessage;
                }
                return _iLoadingBL.AddSLA(tblUnloadingSLATO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to AddSLA");
                return resultMessage;
            }
        }
        [Route("MigrateTransferRequest")]
        [HttpPost]
        public ResultMessage MigrateTransferRequest()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                return _iLoadingBL.MigrateTransferRequest();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to MigrateTransferRequest");
                return resultMessage;
            }
        }
        #endregion
    }
}
