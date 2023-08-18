using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.IoT.Interfaces;
using Microsoft.Extensions.Logging;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseVehicleReportController : Controller
    {
        private readonly ILogger loggerObj;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblQualityPhaseDtlsBL _iTblQualityPhaseDtlsBL;
        private readonly IDimVehiclePhaseBL _iDimVehiclePhaseBL;
        private readonly ITblPurchaseEnquiryBL _iTblPurchaseEnquiryBL;
        private readonly ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;
        private readonly ITblPurchaseVehicleSpotEntryBL _iTblPurchaseVehicleSpotEntryBL;
        private readonly ITblQualityPhaseBL _iTblQualityPhaseBL;
        private readonly IDimQualitySampleTypeBL _iDimQualitySampleTypeBL;
        private readonly ITblPurchaseVehicleDetailsBL _iTblPurchaseVehicleDetailsBL;
        private readonly ITblPurchaseInvoiceDocumentsBL _iTblPurchaseInvoiceDocumentsBL;
        private readonly ITblPartyWeighingMeasuresBL _iTblPartyWeighingMeasuresBL;
        private readonly ITblPurchaseVehicleOtherEntryBL _iTblPurchaseVehicleOtherEntryBL;
        private readonly ITblPurchaseItemDescBL _iTblPurchaseItemDescBL;
        private readonly ITblPurchaseBookingOpngBalBL _iTblPurchaseBookingOpngBalBL;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IGateCommunication _iGateCommunication;
        private readonly IIotCommunication _iIotCommunication;
        private readonly ITblPurchaseVehFreightDtlsBL _iTblPurchaseVehFreightDtlsBL;
        private readonly ITblPurchaseSchTcDtlsBL _iTblPurchaseSchTcDtlsBL;
        private readonly ITblPurchaseVehLinkSaudaBL _iTblPurchaseVehLinkSaudaBL;
        private static readonly object vehicleLock = new object();
        public PurchaseVehicleReportController(
            Icommondao icommondao,
            ITblPurchaseScheduleSummaryBL iTblPurchaseScheduleSummaryBL
                        , ITblPurchaseEnquiryBL itblPurchaseEnquiryBL,
                       ITblPurchaseVehicleSpotEntryBL iTblPurchaseVehicleSpotEntryBL
                        , ITblQualityPhaseBL iTblQualityPhaseBL
                        , IDimQualitySampleTypeBL iDimQualitySampleTypeBL
                       , IDimVehiclePhaseBL iDimVehiclePhaseBL
                        , ITblPurchaseVehicleDetailsBL iTblPurchaseVehicleDetailsBL
                        , ITblQualityPhaseDtlsBL iTblQualityPhaseDtlsBL
                       , ITblPartyWeighingMeasuresBL iTblPartyWeighingMeasuresBL
                        , ITblPurchaseInvoiceDocumentsBL iTblPurchaseInvoiceDocumentsBL
                        , ITblPurchaseVehicleOtherEntryBL iTblPurchaseVehicleOtherEntryBL
                        , ITblPurchaseItemDescBL iTblPurchaseItemDescBL
                        , ITblPurchaseBookingOpngBalBL iTblPurchaseBookingOpngBalBL
           , ITblConfigParamsDAO iTblConfigParamsDAO,
                       IGateCommunication iGateCommunication
            , IIotCommunication iIotCommunication
                        , ITblPurchaseVehFreightDtlsBL iTblPurchaseVehFreightDtlsBL
            , ILogger<PurchaseVehicleReportController> logger
            , ITblPurchaseSchTcDtlsBL iTblPurchaseSchTcDtlsBL
            , ITblPurchaseVehLinkSaudaBL iTblPurchaseVehLinkSaudaBL

                       )
        {
            _iTblPurchaseVehicleOtherEntryBL = iTblPurchaseVehicleOtherEntryBL;
            _iTblPurchaseInvoiceDocumentsBL = iTblPurchaseInvoiceDocumentsBL;
            _iTblPartyWeighingMeasuresBL = iTblPartyWeighingMeasuresBL;
            _iTblQualityPhaseDtlsBL = iTblQualityPhaseDtlsBL;
            _iTblPurchaseVehicleDetailsBL = iTblPurchaseVehicleDetailsBL;
            _iDimQualitySampleTypeBL = iDimQualitySampleTypeBL;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _iTblPurchaseEnquiryBL = itblPurchaseEnquiryBL;
            _iTblPurchaseVehicleSpotEntryBL = iTblPurchaseVehicleSpotEntryBL;
            _iTblQualityPhaseBL = iTblQualityPhaseBL;
            _iDimVehiclePhaseBL = iDimVehiclePhaseBL;
            _iCommonDAO = icommondao;
            _iTblPurchaseItemDescBL = iTblPurchaseItemDescBL;
            _iTblPurchaseBookingOpngBalBL = iTblPurchaseBookingOpngBalBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iGateCommunication = iGateCommunication;
            _iIotCommunication = iIotCommunication;
            _iTblPurchaseVehFreightDtlsBL = iTblPurchaseVehFreightDtlsBL;
            loggerObj = logger;
            _iTblPurchaseSchTcDtlsBL = iTblPurchaseSchTcDtlsBL;
            _iTblPurchaseVehLinkSaudaBL = iTblPurchaseVehLinkSaudaBL;
        }


        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        //}

        [Route("GetAllSchTcDtlsList")]
        [HttpGet]
        public List<TblPurchaseSchTcDtlsTO> GetAllSchTcDtlsList(String rootScheduleId)
        {
            return _iTblPurchaseSchTcDtlsBL.SelectAllScheTcDtls(rootScheduleId);
        }

        [Route("GetVehicleList")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetVehicleList(String tblPurSchSummaryFilterTO)
        {
            TblPurSchSummaryFilterTO tblPurSchSummaryFilterTempTO = JsonConvert.DeserializeObject<TblPurSchSummaryFilterTO>(tblPurSchSummaryFilterTO);

            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(tblPurSchSummaryFilterTempTO.FromDateStr))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(tblPurSchSummaryFilterTempTO.FromDateStr).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(tblPurSchSummaryFilterTempTO.ToDateStr))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(tblPurSchSummaryFilterTempTO.ToDateStr).ToString(Constants.AzureDateFormat));


            tblPurSchSummaryFilterTempTO.FromDate = from_Date;
            tblPurSchSummaryFilterTempTO.ToDate = to_Date;
            if (tblPurSchSummaryFilterTempTO.ToDate == DateTime.MinValue)
            {
                tblPurSchSummaryFilterTempTO.ToDate = _iCommonDAO.ServerDateTime;
            }
            if (tblPurSchSummaryFilterTempTO.ForScheduleActualOrUnloading == 3)
            {
                tblPurSchSummaryFilterTempTO.FromDate = _iCommonDAO.ServerDateTime;
                tblPurSchSummaryFilterTempTO.ToDate = _iCommonDAO.ServerDateTime;
            }

            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllVehicleDetailsList(tblPurSchSummaryFilterTempTO);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                return tblPurchaseScheduleSummaryTOList;
            }
            else return null;
        }

        [Route("GetVehSummaryForDashboard")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetVehSummaryForDashboard(String tblPurSchSummaryFilterTO)
        {
            TblPurSchSummaryFilterTO tblPurSchSummaryFilterTempTO = JsonConvert.DeserializeObject<TblPurSchSummaryFilterTO>(tblPurSchSummaryFilterTO);

            tblPurSchSummaryFilterTempTO.ForScheduleActualOrUnloading = 3;
            tblPurSchSummaryFilterTempTO.SkipDateTime = true;
            tblPurSchSummaryFilterTempTO.IsInStatusIds = true;
            tblPurSchSummaryFilterTempTO.FromDate = _iCommonDAO.ServerDateTime;
            tblPurSchSummaryFilterTempTO.ToDate = tblPurSchSummaryFilterTempTO.FromDate;

            //if (tblPurSchSummaryFilterTempTO.ForScheduleActualOrUnloading == 3)
            //{
            //    tblPurSchSummaryFilterTempTO.FromDate = _iCommonDAO.ServerDateTime;
            //    tblPurSchSummaryFilterTempTO.ToDate = _iCommonDAO.ServerDateTime;
            //}

            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.GetVehSummaryForDashboard(tblPurSchSummaryFilterTempTO);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                return tblPurchaseScheduleSummaryTOList;
            }
            else return null;
        }

        [Route("GetVehicleListForAllCommonApprovals")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetVehicleListForAllCommonApprovals(String tblPurSchSummaryFilterTO)
        {

            TblPurSchSummaryFilterTO tblPurSchSummaryFilterTempTO = JsonConvert.DeserializeObject<TblPurSchSummaryFilterTO>(tblPurSchSummaryFilterTO);

            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(tblPurSchSummaryFilterTempTO.FromDateStr))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(tblPurSchSummaryFilterTempTO.FromDateStr).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(tblPurSchSummaryFilterTempTO.ToDateStr))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(tblPurSchSummaryFilterTempTO.ToDateStr).ToString(Constants.AzureDateFormat));


            tblPurSchSummaryFilterTempTO.FromDate = from_Date;
            tblPurSchSummaryFilterTempTO.ToDate = to_Date;


            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectVehicleListForAllCommonApprovals(tblPurSchSummaryFilterTempTO);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                return tblPurchaseScheduleSummaryTOList;
            }
            else return null;
        }

        [Route("GetAllIdsForSampleType")]
        [HttpGet]
        public List<DropDownTO> GetAllIdsForSampleType(int PurchaseScheduleSummaryId, int VehiclePhaseId, int FlagTypeId, int QualitySampleTypeId)
        {

            return _iTblQualityPhaseBL.GetAllIdsForSampleType(PurchaseScheduleSummaryId, VehiclePhaseId, FlagTypeId, QualitySampleTypeId);

        }

        [Route("GetVehicleListForPendingQualityFlags")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetVehicleListForPendingQualityFlags(string pmUserId)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.GetVehicleListForPendingQualityFlags(pmUserId);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                return tblPurchaseScheduleSummaryTOList;
            }
            else return null;
        }

        // [Route("GetPurchaseMaterialSample")]
        // [HttpGet]
        // public TblPurchaseMaterialSampleTO GetPurchaseMaterialSample(int id)
        // {

        //     TblPurchaseMaterialSampleTO tblPurchaseMaterialSampleTypeTo = _iTblPurchaseScheduleSummaryBL.GetTblPurchaseMaterialSample(id);
        //     if (tblPurchaseMaterialSampleTypeTo != null )
        //     {
        //         return tblPurchaseMaterialSampleTypeTo;
        //     }
        //     else return new TblPurchaseMaterialSampleTO();
        // }

        [Route("GetAllReportedVehicleList")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsList(String statusId, DateTime date)
        {
            return _iTblPurchaseScheduleSummaryBL.SelectAllReportedVehicleDetailsList(statusId, date);

        }
        [Route("GetAllVehicleInOutList")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailsList(string fromDate, string toDate, string statusId, Int32 idPurchaseScheduleSummary = 0, Int32 rootScheduleId = 0)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            return _iTblPurchaseScheduleSummaryBL.SelectAllReportedVehicleDetailsList(from_Date, to_Date, statusId, idPurchaseScheduleSummary, rootScheduleId);

        }
        [Route("GetScheduleDetailsOfVehicle")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> SelectAllReportedVehicleDetailList(String vehicleNo, String statusId, Int32 idPurchaseScheduleSummary)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllReportedVehicleDetailsList(vehicleNo, statusId, idPurchaseScheduleSummary);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                return tblPurchaseScheduleSummaryTOList;
            }
            else return null;
        }

        [Route("GetVehicleSportEntrydetails")]
        [HttpGet]
        public List<TblPurchaseVehicleSpotEntryTO> GetVehicleSportEntrydetails(string fromDate, string toDate, String loginUserId, Int32 id = 2, bool skipDatetime = false)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = _iTblPurchaseVehicleSpotEntryBL.SelectAllSpotEntryVehicles(from_Date, to_Date, loginUserId, id, skipDatetime);
            return tblPurchaseVehicleSpotEntryTOList;
        }

        [Route("GetVehicleSportEntrydetailsCount")]
        [HttpGet]
        public DropDownTO GetVehicleSportEntrydetailsCount(int pmId = 0, int supplierId = 0, int materialTypeId = 0)
        {
            DropDownTO DropDownTOCount = _iTblPurchaseVehicleSpotEntryBL.SelectAllSpotEntryVehiclesCount(pmId, supplierId, materialTypeId);
            return DropDownTOCount;
        }
        [Route("GetAllVehicleListForUnloading")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllVehicleListForUnloading(String statusId, Int32 loggedInUserId, string fromDate, string toDate, int showList, Int32 idPurchaseScheduleSummary, Int32 rootScheduleId = 0)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            return _iTblPurchaseScheduleSummaryBL.GetAllVehicleListForUnloading(statusId, loggedInUserId, from_Date, to_Date, showList, idPurchaseScheduleSummary, rootScheduleId);
        }

        [Route("GetAllVehicleListForRecovery")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllVehicleListForRecovery(String statusId, Int32 loggedInUserId, string fromDate, string toDate, Int32 rootScheduleId = 0)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            return _iTblPurchaseScheduleSummaryBL.GetAllVehicleListForRecovery(statusId, loggedInUserId, from_Date, to_Date, rootScheduleId);
        }

        [Route("GetAllVehicleListForGrading")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllVehicleListForGrading(String statusId, Int32 loggedInUserId, string fromDate, string toDate, Int32 idPurchaseScheduleSummary = 0, Int32 rootScheduleId = 0)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            return _iTblPurchaseScheduleSummaryBL.GetAllVehicleListForGrading(statusId, from_Date, to_Date, loggedInUserId, idPurchaseScheduleSummary, rootScheduleId);
        }



        [Route("GetActiveUsersDropDownListByRoleTypeIdWithVehAllocation")]
        [HttpGet]
        public List<DropDownTO> GetActiveUsersDropDownListByRoleTypeIdWithVehAllocation(Int32 RoleTypeId, Int32 nameWithCount = 1)
        {
            List<DropDownTO> userList = _iTblPurchaseScheduleSummaryBL.SelectAllSystemUsersFromRoleTypeWithVehAllocation(RoleTypeId, nameWithCount);
            return userList;
        }


        [Route("GetAllVehicleStatusDate")]
        [HttpGet]
        public List<VehicleStatusDateTO> GetAllVehicleStatusDate(string fromDate, string toDate, string pmUserId, Int32 vehicleFilterId,Int32 isPrintExcelReport)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommonDAO.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommonDAO.ServerDateTime.Date;


            return _iTblPurchaseScheduleSummaryBL.GetAllVehicleTrackingDtlsList(frmDt, toDt, pmUserId, vehicleFilterId, isPrintExcelReport);
            // return BL.TblLoadingSlipBL.SelectAllLoadingCycleList(frmDt, toDt, tblUserRoleTO, cnfId, vehicleStatus);
        }






        // [Route("b")]
        // [HttpGet]
        // public  ResultMessage Test()
        // {
        //         ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTOTemp = _iTblPurchaseScheduleSummaryBL.CheckVehiclePreviousStatus(CurrentStatusId,IdPurchaseScheduleSummary);
        //     if (tblPurchaseScheduleSummaryTOTemp == null)
        //     {
        //         resultMessage.MessageType = ResultMessageE.Information;
        //         resultMessage.Result = 1;
        //         resultMessage.Text = "";
        //         return resultMessage;
        //     }
        //     else
        //     {
        //         string statusDesc = _iTblPurchaseScheduleSummaryBL.GetVehicleStatus(tblPurchaseScheduleSummaryTOTemp);
        //         resultMessage.MessageType = ResultMessageE.Error;
        //         resultMessage.Result = 0;
        //         resultMessage.Text = statusDesc;
        //         return resultMessage;
        //     }
        // }
        [Route("GetVehicleDetails")]
        [HttpGet]
        public TblPurchaseScheduleSummaryTO GetVehicleDetails(Int32 purchaseSummaryId)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDetails(purchaseSummaryId, true);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count == 1)
            {

                Boolean isGetGradeExpDtls = false;
                _iTblPurchaseVehicleDetailsBL.SelectVehItemDtlsWithOrWithoutGradeExpDtls(tblPurchaseScheduleSummaryTOList, isGetGradeExpDtls);

                // List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                // tblPurchaseVehicleDetailsTOList = BL.TblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTOList[0].IdPurchaseScheduleSummary);
                // if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                // {
                //     tblPurchaseScheduleSummaryTOList[0].PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;
                //     //BL.TblPurchaseVehicleDetailsBL.GetGradeExpressionDetails(tblPurchaseScheduleSummaryTOList[0].PurchaseScheduleSummaryDetailsTOList);
                // }
                return tblPurchaseScheduleSummaryTOList[0];
            }
            else
                return null;


            // return tblVehicleDetailsTOList;
        }

        [Route("GetVehicleDetailsLight")]
        [HttpGet]
        public TblPurchaseScheduleSummaryTO GetVehicleDetailsLight(Int32 purchaseSummaryId)
        {

            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDetails(purchaseSummaryId, true);
            
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count == 1)
            {
                return _iIotCommunication.GetItemDataFromIotAndMerge(tblPurchaseScheduleSummaryTOList[0]);
            }
            else
                return null;


            // return tblVehicleDetailsTOList;
        }

        [Route("GetVehicleDtlsByPurchaseScheduleId")]
        [HttpGet]
        public TblPurchaseScheduleSummaryTO GetVehicleDetailsByScheduleId(Int32 purchaseScheduleId)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDetails(purchaseScheduleId, false);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count == 1)
            {

                Boolean isGetGradeExpDtls = true;
                _iTblPurchaseVehicleDetailsBL.SelectVehItemDtlsWithOrWithoutGradeExpDtls(tblPurchaseScheduleSummaryTOList, isGetGradeExpDtls);

                // List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                // tblPurchaseVehicleDetailsTOList = BL.TblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTOList[0].IdPurchaseScheduleSummary);
                // if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                // {
                //     tblPurchaseScheduleSummaryTOList[0].PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;
                //     //BL.TblGradeExpressionDtlsBL.SelectGradeExpDtlsList(tblPurchaseScheduleSummaryTOList[0].PurchaseScheduleSummaryDetailsTOList);
                //     //BL.TblPurchaseVehicleDetailsBL.GetGradeExpressionDetails(tblPurchaseScheduleSummaryTOList[0].PurchaseScheduleSummaryDetailsTOList);
                // }
                return tblPurchaseScheduleSummaryTOList[0];
            }
            else
                return null;


            // return tblVehicleDetailsTOList;
        }

        [Route("GetVehicleCorrectionPhaseDtls")]
        [HttpGet]
        public TblPurchaseScheduleSummaryTO GetVehicleCorrectionPhaseDtls(Int32 rootScheduleId, Int32 statusId, Int32 vehiclePhaseId)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectVehicleScheduleByRootAndStatusId(rootScheduleId, statusId, vehiclePhaseId);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count == 1)
            {

                Boolean isGetGradeExpDtls = true;
                _iTblPurchaseVehicleDetailsBL.SelectVehItemDtlsWithOrWithoutGradeExpDtls(tblPurchaseScheduleSummaryTOList, isGetGradeExpDtls);

                // List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                // tblPurchaseVehicleDetailsTOList = BL.TblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTOList[0].IdPurchaseScheduleSummary);
                // if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                // {
                //     tblPurchaseScheduleSummaryTOList[0].PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;
                //     //BL.TblGradeExpressionDtlsBL.SelectGradeExpDtlsList(tblPurchaseScheduleSummaryTOList[0].PurchaseScheduleSummaryDetailsTOList);
                //     //BL.TblPurchaseVehicleDetailsBL.GetGradeExpressionDetails(tblPurchaseScheduleSummaryTOList[0].PurchaseScheduleSummaryDetailsTOList);
                // }
                return tblPurchaseScheduleSummaryTOList[0];
            }
            else
                return null;


            // return tblVehicleDetailsTOList;
        }

        [Route("GetPurchaseVehicleDetailsByParentScheduleId")]
        [HttpGet]
        public TblPurchaseScheduleSummaryTO GetPurchaseVehicleDetailsByParentScheduleId(Int32 parentPurchaseSummaryId)
        {
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTO(parentPurchaseSummaryId);
            if (tblPurchaseScheduleSummaryTO != null)
            {
                List<TblPurchaseScheduleSummaryTO> tempList = new List<TblPurchaseScheduleSummaryTO>();
                tempList.Add(tblPurchaseScheduleSummaryTO);
                Boolean isGetGradeExpDtls = false;

                _iTblPurchaseVehicleDetailsBL.SelectVehItemDtlsWithOrWithoutGradeExpDtls(tempList, isGetGradeExpDtls);

                // List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                // tblPurchaseVehicleDetailsTOList = BL.TblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary);
                // if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                // {
                //     tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;
                // }
            }

            return tblPurchaseScheduleSummaryTO;

        }

        [Route("GetSpotEntryVehicleDetails")]
        [HttpGet]
        public TblPurchaseVehicleSpotEntryTO GetSpotEntryVehicleDetails(Int32 spotEntryVehicleId)
        {
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = _iTblPurchaseVehicleSpotEntryBL.SelectTblPurchaseVehicleSpotEntryTO(spotEntryVehicleId);
            return tblPurchaseVehicleSpotEntryTO;
            // return tblVehicleDetailsTOList;
        }

        [Route("GetSpotEntryVehicleDetailsWithMaterials")]
        [HttpGet]
        public TblPurchaseVehicleSpotEntryTO GetSpotEntryVehicleDetailsWithMaterials(Int32 spotEntryVehicleId)
        {
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = _iTblPurchaseVehicleSpotEntryBL.GetSpotEntryVehicleDetailsWithMaterials(spotEntryVehicleId);
            return tblPurchaseVehicleSpotEntryTO;
            // return tblVehicleDetailsTOList;
        }

        [Route("GetVehicleNumberList")]
        [HttpGet]
        public List<VehicleNumber> GetVehicleNumberList()
        {
            return _iTblPurchaseVehicleSpotEntryBL.SelectAllVehicles();
        }

        [Route("GetScheduleDetailsByPurchaseEnquiryIdForDisplay")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetScheduleDetailsByPurchaseEnquiryIdForDisplay(Int32 enquiryPurchaseId, Boolean isGetItemDtls)
        {
            //List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllVehicleDetailsListForPurchaseEnquiry(enquiryPurchaseId);

            // List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllVehicleDetailsListForPurchaseEnquiry(enquiryPurchaseId, 0);

            // //To get all records
            // if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            // {
            //     if (isGetItemDtls)
            //     {
            //         Boolean isGetGradeExpDtls = false;
            //         _iTblPurchaseVehicleDetailsBL.SelectVehItemDtlsWithOrWithoutGradeExpDtls(tblPurchaseScheduleSummaryTOList, isGetGradeExpDtls);
            //     }

            //     for (int i = 0; i < tblPurchaseScheduleSummaryTOList.Count; i++)
            //     {
            //         TblPurchaseScheduleSummaryTO scheduleTO = tblPurchaseScheduleSummaryTOList[i];

            //         if (scheduleTO.StatusId != (Int32)Constants.TranStatusE.VEHICLE_CANCELED)
            //         {
            //             List<TblPurchaseScheduleSummaryTO> tempTOList = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(scheduleTO.ActualRootScheduleId, true);
            //             if (tempTOList != null && tempTOList.Count == 1)
            //             {
            //                 scheduleTO.StatusId = tempTOList[0].StatusId;
            //                 scheduleTO.StatusDesc = tempTOList[0].StatusDesc;
            //                 scheduleTO.StatusName = tempTOList[0].StatusName;
            //                 scheduleTO.OrgScheduleQty = tempTOList[0].OrgScheduleQty;
            //                 scheduleTO.OrgUnloadedQty = tempTOList[0].OrgUnloadedQty;
            //                 scheduleTO.VehiclePhaseId = tempTOList[0].VehiclePhaseId;
            //             }
            //         }

            //     }

            // }
            // return tblPurchaseScheduleSummaryTOList;


            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectEnquiryScheduleSummary(enquiryPurchaseId);
            int confiqId = _iTblConfigParamsDAO.IoTSetting();
            if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
                {
                    foreach (var PurchaseScheduleSummaryTO in tblPurchaseScheduleSummaryTOList)
                    {
                        _iIotCommunication.GetItemDataFromIotAndMerge(PurchaseScheduleSummaryTO);
                    }
                }
                
            }
            //To get all records
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                if (isGetItemDtls)
                {
                    Boolean isGetGradeExpDtls = false;
                    _iTblPurchaseVehicleDetailsBL.SelectVehItemDtlsWithOrWithoutGradeExpDtls(tblPurchaseScheduleSummaryTOList, isGetGradeExpDtls);
                }

            }
            return tblPurchaseScheduleSummaryTOList;
        }

        [Route("GetScheduleDetailsByRootId")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetScheduleDetailsByRootId(Int32 rootScheduleId, Boolean isActive)
        {

            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(rootScheduleId, isActive);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {

                Boolean isGetGradeExpDtls = false;
                _iTblPurchaseVehicleDetailsBL.SelectVehItemDtlsWithOrWithoutGradeExpDtls(tblPurchaseScheduleSummaryTOList, isGetGradeExpDtls);


                // for (int i = 0; i < tblPurchaseScheduleSummaryTOList.Count; i++)
                // {
                //     List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                //     TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = new TblPurchaseScheduleSummaryTO();
                //     tblPurchaseScheduleSummaryTO = tblPurchaseScheduleSummaryTOList[i];
                //     tblPurchaseVehicleDetailsTOList = BL.TblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary);
                //     if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                //     {
                //         tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;
                //     }
                // }


            }

            return tblPurchaseScheduleSummaryTOList;
        }

        [Route("GetScheduleDetailsByDate")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetScheduleDetailsByDate(DateTime scheduledate)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllVehicleDetailsListByDate(scheduledate);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {

                Boolean isGetGradeExpDtls = false;
                _iTblPurchaseVehicleDetailsBL.SelectVehItemDtlsWithOrWithoutGradeExpDtls(tblPurchaseScheduleSummaryTOList, isGetGradeExpDtls);


                // for (int i = 0; i < tblPurchaseScheduleSummaryTOList.Count; i++)
                // {
                //     TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = tblPurchaseScheduleSummaryTOList[i];
                //     List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();

                //     tblPurchaseVehicleDetailsTOList = BL.TblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary);
                //     if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                //     {
                //         tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList = tblPurchaseVehicleDetailsTOList;
                //     }
                // }
            }


            return tblPurchaseScheduleSummaryTOList;
        }

        [Route("GetVehicleDetailsByStatus")]
        [HttpGet]
        public List<TblPurchaseVehicleDetailsTO> GetVehicleDetailsByStatus(String statusId, String fromDate, String vehicleNo)
        {
            DateTime frmDate = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDate = Convert.ToDateTime(fromDate);
            }
            List<TblPurchaseVehicleDetailsTO> list = _iTblPurchaseVehicleDetailsBL.SelectAllVehicleDetailsByStatus(statusId, frmDate, vehicleNo);
            if (list != null)
            {
                List<TblPurchaseVehicleDetailsTO> finalList = new List<TblPurchaseVehicleDetailsTO>();
                finalList.AddRange(list);
                return finalList;

                #region
                //string[] statusIds = statusId.Split(',');

                //for (int i = 0; i < statusIds.Length; i++)
                //{
                //    //if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.BOOKING_NEW)
                //    if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_CONFIRM
                //     || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
                //     || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)
                //    {
                //        var sendInList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN).ToList().OrderBy(d => d.StatusDate).ToList();
                //        //var sendInList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.BOOKING_NEW).ToList().OrderBy(d => d.StatusDate).ToList();

                //        if (sendInList != null)
                //            finalList.AddRange(sendInList);

                //        var reportedList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING).ToList().OrderBy(d => d.StatusDate).ToList();
                //        if (reportedList != null)
                //            finalList.AddRange(reportedList);

                //        var confirmList = list.Where(r => r.StatusId != (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
                //                                     && r.StatusId != (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN).ToList().OrderBy(d => d.StatusDate).ToList();
                //        if (confirmList != null)
                //            finalList.AddRange(confirmList);

                //        return finalList;
                //    }
                //    else if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_GATE_IN
                //        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_COMPLETED)
                //    {

                //        var reportedList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_COMPLETED).ToList().OrderBy(d => d.StatusDate).ToList();
                //        if (reportedList != null)
                //            finalList.AddRange(reportedList);

                //        var confirmList = list.Where(r => r.StatusId != (int)Constants.TranStatusE.LOADING_COMPLETED).ToList().OrderBy(d => d.StatusDate).ToList();
                //        if (confirmList != null)
                //            finalList.AddRange(confirmList);

                //        return finalList;
                //    }
                //}
                #endregion
            }

            return list;
        }

        [Route("GetVehicleDetailsByEnquiryId")]
        [HttpGet]
        public List<TblPurchaseVehicleDetailsTO> GetVehicleDetailsByEnquiryId(Int32 purchaseEnquiryId)
        {
            List<TblPurchaseVehicleDetailsTO> tblVehicleDetailsTOList = _iTblPurchaseVehicleDetailsBL.GetVehicleDetailsByEnquiryId(purchaseEnquiryId);
            return tblVehicleDetailsTOList;
        }

        [Route("GetVehicleDetailsByPurchaseScheduleId")]
        [HttpGet]
        public List<TblPurchaseVehicleDetailsTO> GetVehicleDetailsByPurchaseScheduleId(Int32 purchaseScheduleId)
        {
            Boolean isGetGradeExpDtls = true;
            List<TblPurchaseVehicleDetailsTO> tblVehicleDetailsTOList = _iTblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(purchaseScheduleId, isGetGradeExpDtls);
            return tblVehicleDetailsTOList;
        }

        [Route("GetPurchaseScheduleDetailsBySpotEntryVehicleId")]
        [HttpGet]
        public List<TblPurchaseVehicleDetailsTO> GetPurchaseScheduleDetailsBySpotEntryVehicleId(Int32 spotEntryVehicleId)
        {
            List<TblPurchaseVehicleDetailsTO> tblVehicleDetailsTOList = _iTblPurchaseVehicleDetailsBL.GetPurchaseScheduleDetailsBySpotEntryVehicleId(spotEntryVehicleId);
            return tblVehicleDetailsTOList;
        }
        [Route("GetScheduleDetailsByPurchaseEnquiryId")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetScheduleDetailsByPurchaseEnquiryId(Int32 enquiryPurchaseId)
        {
            return _iTblPurchaseScheduleSummaryBL.GetScheduleDetailsByPurchaseEnquiryId(enquiryPurchaseId);
        }

        [Route("GetVehicleDetailsByScheduleId")]
        [HttpGet]
        public TblPurchaseScheduleSummaryTO GetVehicleDetailsByScheduleId(Int32 IdPurchaseScheduleSummary, Int32 statusId, Int32 vehiclePhaseId, Int32 rootScheduleId = 0, Int32 isGetQtyOfNewStatus = 0)
        {

            return _iTblPurchaseScheduleSummaryBL.GetVehicleDetailsByScheduleIds(IdPurchaseScheduleSummary, statusId, vehiclePhaseId, rootScheduleId, isGetQtyOfNewStatus);
        }

        [Route("CheckVehiclePreviousStatus")]
        [HttpGet]
        public ResultMessage CheckVehiclePreviousStatus(Int32 CurrentStatusId, Int32 IdPurchaseScheduleSummary)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTOTemp = _iTblPurchaseScheduleSummaryBL.CheckVehiclePreviousStatus(CurrentStatusId, IdPurchaseScheduleSummary);
            if (tblPurchaseScheduleSummaryTOTemp == null)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "";
                return resultMessage;
            }
            else
            {
                string statusDesc = _iTblPurchaseScheduleSummaryBL.GetVehicleStatus(tblPurchaseScheduleSummaryTOTemp);
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                resultMessage.Text = statusDesc;
                return resultMessage;
            }
        }

        [Route("GetAllParentScheduleDetails")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllParentScheduleDetails(string fromDate, string toDate, String userId, Int32 rootScheduleId, string showListE)
        {

            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOListReturn = new List<TblPurchaseScheduleSummaryTO>();
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllReportedVehicleDetailsListPhasewise(from_Date, to_Date, userId, rootScheduleId, showListE);

            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOListRec = tblPurchaseScheduleSummaryTOList.Where(w => w.VehiclePhaseId == (int)Constants.PurchaseVehiclePhasesE.RECOVERY).ToList();
                if (tblPurchaseScheduleSummaryTOListRec != null && tblPurchaseScheduleSummaryTOListRec.Count > 0)
                {
                    tblPurchaseScheduleSummaryTOListReturn = tblPurchaseScheduleSummaryTOListRec;
                    List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOListOther = tblPurchaseScheduleSummaryTOList.Where(w => w.VehiclePhaseId != (int)Constants.PurchaseVehiclePhasesE.RECOVERY).ToList();
                    if (tblPurchaseScheduleSummaryTOListOther != null && tblPurchaseScheduleSummaryTOListOther.Count > 0)
                    {
                        foreach (var item in tblPurchaseScheduleSummaryTOListOther)
                        {
                            tblPurchaseScheduleSummaryTOListReturn.Add(item);
                        }
                    }
                }
                else
                {
                    tblPurchaseScheduleSummaryTOListReturn = tblPurchaseScheduleSummaryTOList;
                }
            }
            return tblPurchaseScheduleSummaryTOListReturn;
        }

        [Route("GetAllParentScheduleDetailsForComparison")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllParentScheduleDetailsForComparison(String tblPurSchSummaryFilterTO)
        {
            TblPurSchSummaryFilterTO tblPurSchSummaryFilterTempTO = JsonConvert.DeserializeObject<TblPurSchSummaryFilterTO>(tblPurSchSummaryFilterTO);

            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(tblPurSchSummaryFilterTempTO.FromDateStr))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(tblPurSchSummaryFilterTempTO.FromDateStr).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(tblPurSchSummaryFilterTempTO.ToDateStr))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(tblPurSchSummaryFilterTempTO.ToDateStr).ToString(Constants.AzureDateFormat));


            tblPurSchSummaryFilterTempTO.FromDate = from_Date;
            tblPurSchSummaryFilterTempTO.ToDate = to_Date;

            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOListReturn = new List<TblPurchaseScheduleSummaryTO>();
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllReportedVehicleDetailsListPhasewiseForComp(tblPurSchSummaryFilterTempTO);

            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOListRec = tblPurchaseScheduleSummaryTOList.Where(w => w.VehiclePhaseId == (int)Constants.PurchaseVehiclePhasesE.RECOVERY).ToList();
                if (tblPurchaseScheduleSummaryTOListRec != null && tblPurchaseScheduleSummaryTOListRec.Count > 0)
                {
                    tblPurchaseScheduleSummaryTOListReturn = tblPurchaseScheduleSummaryTOListRec;
                    List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOListOther = tblPurchaseScheduleSummaryTOList.Where(w => w.VehiclePhaseId != (int)Constants.PurchaseVehiclePhasesE.RECOVERY).ToList();
                    if (tblPurchaseScheduleSummaryTOListOther != null && tblPurchaseScheduleSummaryTOListOther.Count > 0)
                    {
                        foreach (var item in tblPurchaseScheduleSummaryTOListOther)
                        {
                            tblPurchaseScheduleSummaryTOListReturn.Add(item);
                        }
                    }
                }
                else
                {
                    tblPurchaseScheduleSummaryTOListReturn = tblPurchaseScheduleSummaryTOList;
                }

            }
            return tblPurchaseScheduleSummaryTOListReturn;
        }

        [Route("GetAllScheduleDetailsByPhase")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllScheduleDetailsByPhase(int purchaseEnquiryId)
        {
            return _iTblPurchaseScheduleSummaryBL.GetAllScheduleDetailsByPhase(purchaseEnquiryId);
            //return _iTblPurchaseScheduleSummaryBL.GetAllScheduleDetailsByPhaseAndVehicleID(purchaseEnquiryId);
        }

        [Route("GetAllScheduleDetailsByPhaseForAllVehicle")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllScheduleDetailsByPhaseForAllVehicle(int purchaseEnquiryId)
        {
            return _iTblPurchaseScheduleSummaryBL.GetAllScheduleDetailsByPhaseForAllVehicle(purchaseEnquiryId);
            //return _iTblPurchaseScheduleSummaryBL.GetAllScheduleDetailsByPhaseAndVehicleID(purchaseEnquiryId);
        }

        [Route("GetAllScheduleDetailsByPhaseAndVehicleID")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllScheduleDetailsByPhaseAndVehicleID(int idPurchaseScheduleSummary)
        {
            return _iTblPurchaseScheduleSummaryBL.GetAllScheduleDetailsByPhaseAndVehicleID(idPurchaseScheduleSummary);
            // return _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDetails(purchaseEnquiryId);
        }

        [Route("GetSudharSaudaReportDtls")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetSudharSaudaReportDtls(int idPurchaseScheduleSummary)
        {
            return _iTblPurchaseScheduleSummaryBL.GetSudharSaudaReportDtls(idPurchaseScheduleSummary);
        }

        [Route("GetAllScheduleDetailsByPhaseAndVehicleIDForApproval")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllScheduleDetailsByPhaseAndVehicleIDForApproval(int idPurchaseScheduleSummary)
        {
            return _iTblPurchaseScheduleSummaryBL.GetAllScheduleDetailsByPhaseAndVehicleIDForApproval(idPurchaseScheduleSummary);
            // return _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDetails(purchaseEnquiryId);
        }

        [Route("GetQualityFlagDtlsByPurchaseScheduleId")]
        [HttpGet]
        public List<TblQualityPhaseTO> GetQualityFlagDtlsByPurchaseScheduleId(Int32 purchaseScheduleSummaryId, Int32 isActive)
        {
            List<TblQualityPhaseTO> TblQualityPhaseTOList = _iTblQualityPhaseBL.SelectAllTblQualityPhaseList(purchaseScheduleSummaryId, isActive);
            if (TblQualityPhaseTOList != null && TblQualityPhaseTOList.Count > 0)
            {
                for (int i = 0; i < TblQualityPhaseTOList.Count; i++)
                {
                    TblQualityPhaseTO tblQualityPhaseTO = TblQualityPhaseTOList[i];
                    List<TblQualityPhaseDtlsTO> tblQualityPhaseDtlsTOList = new List<TblQualityPhaseDtlsTO>();

                    tblQualityPhaseDtlsTOList = _iTblQualityPhaseDtlsBL.SelectAllTblQualityPhaseDtlsList(tblQualityPhaseTO.IdTblQualityPhase);
                    if (tblQualityPhaseDtlsTOList != null && tblQualityPhaseDtlsTOList.Count > 0)
                    {
                        tblQualityPhaseTO.QualityPhaseDtlsTOList = tblQualityPhaseDtlsTOList;
                    }
                }
            }

            return TblQualityPhaseTOList;

        }
        [Route("GetAllVehiclePhasesList")]
        [HttpGet]
        public List<DropDownTO> GetAllVehiclePhasesList()
        {
            List<DimVehiclePhaseTO> dimVehiclePhaseTOList = _iDimVehiclePhaseBL.SelectAllDimVehiclePhaseList(1);
            List<DropDownTO> list = new List<DropDownTO>();
            if (dimVehiclePhaseTOList != null && dimVehiclePhaseTOList.Count > 0)
            {
                for (int i = 0; i < dimVehiclePhaseTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = dimVehiclePhaseTOList[i].PhaseName;
                    dropDownTO.Value = dimVehiclePhaseTOList[i].IdVehiclePhase;
                    dropDownTO.Tag = dimVehiclePhaseTOList[i].SequanceNo;
                    list.Add(dropDownTO);
                }
            }

            return list;



            // return dimVehiclePhaseTOList;



        }
        [Route("GetAllQualitySampleList")]
        [HttpGet]
        public List<DimQualitySampleTypeTO> GetAllQualitySampleList()
        {
            List<DimQualitySampleTypeTO> dimQualitySampleTypeTOList = _iDimQualitySampleTypeBL.SelectAllDimQualitySampleTypeList();
            return dimQualitySampleTypeTOList;
        }

        //Prajakta[26 Sept 2018]Added to check grading is completed for all weighing stages
        [Route("CheckIsVehicleGradingConfirmed")]
        [HttpGet]
        public Boolean CheckIsVehicleGradingConfirmed(Int32 IdPurchaseScheduleSummary)
        {
            return _iTblPurchaseScheduleSummaryBL.CheckIsVehicleGradingConfirmed(IdPurchaseScheduleSummary);
        }

        //Deepali[24 Dec 2018]Added to get Quality check List
        [Route("GetQualityFlagCheckLists")]
        [HttpGet]
        public List<TblQualityPhaseTO> GetQualityFlagCheckLists(Int32 IdPurchaseScheduleSummary, Int32 userId, Int32 qualityFormTypeE, Int32 flagTypeId)
      {
            return _iTblQualityPhaseBL.GetQualityFlagCheckLists(IdPurchaseScheduleSummary, userId, qualityFormTypeE, flagTypeId);
        }


        [Route("GetAllNewPurchaseScheduleDtls")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> SelectAllVehicleDetailsListForPurchaseEnquiry(Int32 purchaseEnquiryId)
        {
            return _iTblPurchaseScheduleSummaryBL.SelectAllVehicleDetailsListForPurchaseEnquiry(purchaseEnquiryId, 0);
        }

        //Deepali[9 jan 2018]Added to get Quality check List
        [Route("GetFlagType")]
        [HttpGet]
        public List<DropDownTO> GetFlagType()
        {
            return _iTblQualityPhaseBL.GetFlagType();
        }

        /// <summary>
        /// Priyanka [28-01-2019] : Added to get the purchase schedule
        /// </summary>
        /// <param name="purchaseEnquiryId"></param>
        /// <returns></returns>
        [Route("GetAllPurchaseScheduleSummary")]
        [HttpGet]
        public List<TblPurchaseScheduleSummaryTO> GetAllPurchaseScheduleSummary(Int32 approvalType = 0, Int32 idPurchaseScheduleSummary = 0)
        {
            return _iTblPurchaseScheduleSummaryBL.GetAllPurchaseScheduleSummaryForCommerAppr(approvalType, idPurchaseScheduleSummary);
        }

        /// <summary>
        /// Priyanka [28-01-2019]
        /// </summary>
        /// <returns></returns>
        [Route("GetAllTblPurchaseDocToVerify")]
        [HttpGet]
        public List<TblPurchaseInvoiceDocumentsTO> GetAllTblPurchaseDocToVerify()
        {
            return _iTblPurchaseInvoiceDocumentsBL.SelectAllTblPurchaseDocToVerifyWithDocDtls();
        }

        /// <summary>
        /// Priyanka [12-02-2019] Added to get the party weighing measure details.
        /// </summary>
        /// <param name="idPartyWeighingMeasures"></param>
        /// <returns></returns>
        [Route("GetPartyWeighingMeasure")]
        [HttpGet]
        public TblPartyWeighingMeasuresTO GetPartyWeighingMeasure(int purchaseScheduleSummaryId)
        {
            return _iTblPartyWeighingMeasuresBL.SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(purchaseScheduleSummaryId);
        }
        #endregion

        #region Post

        [Route("PostQualityFlagCheckLists")]
        [HttpPost]
        public ResultMessage PostQualityFlagCheckLists([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblQualityPhaseTO> tblQualityPhaseTOList = JsonConvert.DeserializeObject<List<TblQualityPhaseTO>>(data["TblQualityPhaseTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblQualityPhaseTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : PostQualityFlagCheckLists Found NULL";
                    return resultMessage;
                }

                resultMessage = _iTblQualityPhaseBL.SavePhaseSampleListsagainstPurrchaseScheduleSummaryID(tblQualityPhaseTOList, Convert.ToInt32(loginUserId));

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " PostVehicleSpotEnteryDetails");
                return resultMessage;
            }
        }


        [Route("PostCompletetedQualityFlagCheckLists")]
        [HttpPost]
        public ResultMessage PostCompletetedQualityFlagCheckLists([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblQualityPhaseDtlsTO> tblQualityPhaseDtlsTOList = JsonConvert.DeserializeObject<List<TblQualityPhaseDtlsTO>>(data["QualityPhaseDtlsTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblQualityPhaseDtlsTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : PostQualityFlagCheckLists Found NULL";
                    return resultMessage;
                }

                resultMessage = _iTblQualityPhaseBL.SaveCompletedPhaseSampleListsagainstPurrchaseScheduleSummaryID(tblQualityPhaseDtlsTOList, Convert.ToInt32(loginUserId));

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " PostVehicleSpotEnteryDetails");
                return resultMessage;
            }
        }

        /// <summary>
        /// Priyanka [12-02-2019] : Added to save the party weighing measure details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostPartyWeighingMeasure")]
        [HttpPost]
        public ResultMessage PostPartyWeighingMeasure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO = JsonConvert.DeserializeObject<TblPartyWeighingMeasuresTO>(data["partyWeighingMeasureTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)  
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblPartyWeighingMeasuresTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPartyWeighingMeasuresTO Found NULL";
                    return resultMessage;
                }

                DateTime createdDate = _iCommonDAO.ServerDateTime;
                tblPartyWeighingMeasuresTO.CreatedOn = createdDate;
                tblPartyWeighingMeasuresTO.CreatedBy = Convert.ToInt32(loginUserId);

                resultMessage.Result = _iTblPartyWeighingMeasuresBL.InsertTblPartyWeighingMeasures(tblPartyWeighingMeasuresTO);
                if (resultMessage.Result == 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Party Weighing Measures Added Successfully !";

                }
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostVehicleSpotEnteryDetails";
                return resultMessage;
            }
        }

        /// <summary>
        /// Priyanka [12-02-2019]
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdatePartyWeighingMeasure")]
        [HttpPost]
        public ResultMessage PostUpdatePartyWeighingMeasure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO = JsonConvert.DeserializeObject<TblPartyWeighingMeasuresTO>(data["partyWeighingMeasureTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblPartyWeighingMeasuresTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPartyWeighingMeasuresTO Found NULL";
                    return resultMessage;
                }

                DateTime createdDate = _iCommonDAO.ServerDateTime;
                tblPartyWeighingMeasuresTO.UpdatedOn = createdDate;
                tblPartyWeighingMeasuresTO.UpdatedBy = Convert.ToInt32(loginUserId);

                resultMessage.Result = _iTblPartyWeighingMeasuresBL.UpdateTblPartyWeighingMeasures(tblPartyWeighingMeasuresTO);
                if (resultMessage.Result == 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Party Weighing Measures Updated Successfully !";

                }
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostVehicleSpotEnteryDetails";
                return resultMessage;
            }
        }

        [Route("PostVehicleOtherEnteryDetails")]
        [HttpPost]
        public ResultMessage PostVehicleOtherEnteryDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleOthertEntryTO>(data["purchaseVehicleOtherEntryTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblPurchaseVehicleOtherEntryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : PostVehicleSpotEnteryDetails Found NULL";
                    return resultMessage;
                }

                DateTime createdDate = _iCommonDAO.ServerDateTime;
                tblPurchaseVehicleOtherEntryTO.CreatedOn = createdDate;
                // tblPurchaseVehicleOtherEntryTO.StatusDate = createdDate;//TEMP
                tblPurchaseVehicleOtherEntryTO.CreatedBy = Convert.ToInt32(loginUserId);

                return _iTblPurchaseVehicleOtherEntryBL.SaveVehicleOtherEntry(tblPurchaseVehicleOtherEntryTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostVehicleSpotEnteryDetails";
                return resultMessage;
            }
        }

        [Route("GetVehicleDetailByPurchaseEntryId")]
        [HttpGet]
        public List<TblPurchaseVehicleSpotEntryTO> GetVehicleDetailByPurchaseEntryId(Int32 purchaseEnquiryId)
        {
            // List<TblPurchaseVehicleSpotEntryTO> tblPurchaseEnquiryTOList = BL.TblPurchaseEnquiryBL.SelectTblPurchaseEnquiryVehicleEntryTO(purchaseEnquiryId);
            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseEnquiryTOList = _iTblPurchaseVehicleSpotEntryBL.SelectTblPurchaseEnquiryVehicleEntryTO(purchaseEnquiryId);


            if (tblPurchaseEnquiryTOList != null && tblPurchaseEnquiryTOList.Count > 0)
            {
                return tblPurchaseEnquiryTOList;
            }
            else
                return null;
        }



        /// <summary>
        /// Prajakta [21 Sept 2018] To Save Spot Entry Vehicles. TblPurchaseVehicleSpotEntryTO Need to pass via JObject
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostVehicleSpotEnteryDetails")]
        [HttpPost]
        public ResultMessage PostVehicleSpotEnteryDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblPurchaseVehicleSpotEntryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : PostVehicleSpotEnteryDetails Found NULL";
                    return resultMessage;
                }

                return _iTblPurchaseScheduleSummaryBL.SaveVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO, Convert.ToInt32(loginUserId));

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " PostVehicleSpotEnteryDetails");
                return resultMessage;
            }


        }


        [HttpPost("PostInsertDocumentsMappedToSpotEntry")]
        public IActionResult PostInsertDocumentsMappedToSpotEntry([FromBody] TblRecycleDocumentTO TblRecycleDocumentsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                // int TxnId = Convert.ToInt32(data["txnId"]);
                // int TxnTypeId = Convert.ToInt32(data["txnTypeId"]);
                // int userId = Convert.ToInt32(data["IdUser"]);
                string documentIds = TblRecycleDocumentsTO.DocumentIds;
                List<int> documentId = documentIds.Split(',').Select(int.Parse).ToList();

                if (documentId != null && documentId.Count > 0)
                {
                    TblRecycleDocumentTO tblRecycleDocumentsTO = new TblRecycleDocumentTO();
                    tblRecycleDocumentsTO.TxnId = TblRecycleDocumentsTO.TxnId;
                    tblRecycleDocumentsTO.TxnTypeId = TblRecycleDocumentsTO.TxnTypeId;
                    tblRecycleDocumentsTO.CreatedBy = TblRecycleDocumentsTO.CreatedBy;
                    tblRecycleDocumentsTO.CreatedOn = _iCommonDAO.ServerDateTime;

                    // tblRecycleDocumentsTO.UpdatedBy = userId;
                    tblRecycleDocumentsTO.UpdatedOn = _iCommonDAO.ServerDateTime;
                    tblRecycleDocumentsTO.IsActive = 1;
                    for (int i = 0; i < documentId.Count; i++)
                    {
                        tblRecycleDocumentsTO.DocumentId = documentId[i];
                        resultMessage.Result = _iTblPurchaseVehicleSpotEntryBL.InsertTblRecycleDocuments(tblRecycleDocumentsTO);
                    }
                }
                return Ok(resultMessage);
            }
            catch (Exception exc)
            {
                resultMessage.DefaultBehaviour(exc.Message);
                return Ok(resultMessage);
            }

        }



        [Route("UpdateVehicleStatusReported")]
        [HttpPost]
        public ResultMessage UpdateVehicleStatusReported([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Int32 result = _iTblPurchaseScheduleSummaryBL.InsertTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO);
            if (result >= 1)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Vehicle Reported Successfully";

            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API: Failed To Update Record";
            }
            return resultMessage;
        }
        [Route("SavePurchaseScheduleDetails")]
        [HttpPost]
        public ResultMessage SavePurchaseScheduleDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            var result = _iTblPurchaseEnquiryBL.SavePurchaseScheduleDetails(tblPurchaseScheduleSummaryTO);
            if (result >= 1)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Vehicle Reported Successfully";

            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API: Failed To Update Record";
            }
            return resultMessage;
        }
        [Route("InsertUpdatedMaterailItemDetails")]
        [HttpPost]
        public ResultMessage InsertUpdatedMaterailItemDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Boolean isItemChange = Convert.ToBoolean(data["isItemCalChange"].ToString());
            Boolean isSendNotification = Convert.ToBoolean(data["isSendNotification"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
            DateTime currentdate = _iCommonDAO.ServerDateTime;

            tblPurchaseScheduleSummaryTO.CreatedBy = loginUserId;
            tblPurchaseScheduleSummaryTO.UpdatedBy = loginUserId;

            tblPurchaseScheduleSummaryTO.CreatedOn = currentdate;
            tblPurchaseScheduleSummaryTO.UpdatedOn = tblPurchaseScheduleSummaryTO.CreatedOn;

            // #region Check the Vehicle is already in premises.
            // List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();

            // List<DimStatusTO> dimStatusList = BL.DimStatusBL.SelectAllDimStatusList();
            // DimStatusTO dimStatusTO = new DimStatusTO();
            // TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_SCRAP_IS_FOR_BHAGYALAXMI);
            // if (tblConfigParamsTO != null && Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 1)
            // {
            //     if (tblPurchaseScheduleSummaryTO.COrNcId == 1)
            //         dimStatusTO = _iTblPurchaseScheduleSummaryBL.GetNextStatusTO(tblPurchaseScheduleSummaryTO.CurrentStatusId, dimStatusList, 1);
            //     else
            //         dimStatusTO = _iTblPurchaseScheduleSummaryBL.GetNextStatusTO(tblPurchaseScheduleSummaryTO.CurrentStatusId, dimStatusList, 2);
            // }
            // else
            // {
            //     dimStatusTO = _iTblPurchaseScheduleSummaryBL.GetNextStatusTO(Convert.ToInt32(Constants.TranStatusE.New), dimStatusList, 1);
            // }
            // if (dimStatusTO != null)
            // {
            //     if (tblPurchaseScheduleSummaryTO.StatusId == dimStatusTO.IdStatus)
            //     {
            //         tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.GetPurchaseScheduleSummaryTOByVehicleNo(tblPurchaseScheduleSummaryTO.VehicleNo, tblPurchaseScheduleSummaryTO.ActualRootScheduleId);
            //         if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            //         {
            //             resultMessage.MessageType = ResultMessageE.Error;
            //             resultMessage.DisplayMessage = "Vehicle is already entered in premises.";
            //             resultMessage.Text = "Vehicle is already entered in premises.";
            //             return resultMessage;
            //         }
            //     }
            // }
            // #endregion

            // #region Check if all flags are completed
            // if (tblPurchaseScheduleSummaryTO.ForSaveOrSubmit > 0 || tblPurchaseScheduleSummaryTO.VehiclePhaseId != (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS)
            // {
            //     int res = 0;
            //     int idSummary = tblPurchaseScheduleSummaryTO.RootScheduleId > 0 ? tblPurchaseScheduleSummaryTO.RootScheduleId : tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            //     resultMessage = _iTblPurchaseScheduleSummaryBL.CheckIfAllQualityFlagsAreCompleted(idSummary, tblPurchaseScheduleSummaryTO.VehiclePhaseId);
            //     if (resultMessage.Result > 0)
            //     {
            //         return resultMessage;
            //     }
            // }
            // #endregion

            resultMessage = _iTblPurchaseScheduleSummaryBL.InsertMaterailItemDetails(tblPurchaseScheduleSummaryTO, isItemChange, isSendNotification, currentdate);

            if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
            {
                resultMessage.Tag = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            }
            // if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
            // {
            //     if (resultMessage.MessageType == ResultMessageE.Information)
            //     {
            //         resultMessage.MessageType = ResultMessageE.Information;
            //         resultMessage.Tag = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            //         resultMessage.Text = "Saved Successfully";
            //     }
            //     else
            //     {
            //         resultMessage.MessageType = ResultMessageE.Error;
            //         resultMessage.Text = "API: Failed To Update Record";
            //     }
            // }
            // else
            // {
            //     resultMessage = new StaticStuff.ResultMessage();
            //     resultMessage.MessageType = ResultMessageE.Error;
            //     resultMessage.Text = "API: Failed To Update Record";
            // }

            return resultMessage;
        }

        [Route("FIFOGetSuperwiser")]
        [HttpGet]
        public ResultMessage FIFOGetSuperwiser(int statusId)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            DropDownTO rslt = _iTblPurchaseScheduleSummaryBL.SelectSuperwiserFromTblPurchaseScheduleSummary(statusId);
            if (rslt != null)
            {
                resultMessage.Text = rslt.Value.ToString();
                resultMessage.DisplayMessage = rslt.Text;
            }
            return resultMessage;
        }

        [Route("UpdatedPurchaseScheduleSummary")]
        [HttpPost]
        public ResultMessage UpdatedPurchaseScheduleSummary([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            TblPurchaseMaterialSampleTO tblPurchaseMaterialSampleTO = JsonConvert.DeserializeObject<TblPurchaseMaterialSampleTO>(data["purchaseMaterialSampleTO"].ToString());


            tblPurchaseScheduleSummaryTO.UpdatedBy = 1;

            tblPurchaseScheduleSummaryTO.UpdatedOn = _iCommonDAO.ServerDateTime;
            tblPurchaseMaterialSampleTO.CreatedOn = _iCommonDAO.ServerDateTime;
            tblPurchaseMaterialSampleTO.UserId = 1;
            tblPurchaseMaterialSampleTO.PurchaseScheduleSummaryId = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            tblPurchaseMaterialSampleTO.PurchaseMaterialSampleTypeId = 0;
            tblPurchaseMaterialSampleTO.PurchaseMaterialSampleCategoryId = 0;
            tblPurchaseMaterialSampleTO.PhaseId = 1;
            tblPurchaseMaterialSampleTO.CreatedBy = 1;
            var rslt = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO);
            var rslt2 = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseMaterialSample(tblPurchaseMaterialSampleTO, tblPurchaseScheduleSummaryTO, true);
            if (rslt == 1 && rslt2 == 1)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = " Reported Updated Successfully";
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API: Failed To Update Record";
            }
            return resultMessage;
        }

        [Route("PostUpdatedVehicleScheduleDetails")]
        [HttpPost]
        public ResultMessage PostUpdatedVehicleScheduleDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO previousPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["previousPurchaseScheduleSummaryTO"].ToString());
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
            // tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList= new List<TblPurchaseVehicleDetailsTO>();

            string statusId = (data["statusId"].ToString());
            string spotEntryVehicleStatusId = (data["spotEntryVehicleStatusId"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
            DateTime currentDate = _iCommonDAO.ServerDateTime;

            if (tblPurchaseScheduleSummaryTO != null && tblPurchaseVehicleSpotEntryTO != null)
            {
                previousPurchaseScheduleSummaryTO.SpotEntryVehicleId = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                previousPurchaseScheduleSummaryTO.UpdatedBy = loginUserId;
                previousPurchaseScheduleSummaryTO.UpdatedOn = currentDate;
                previousPurchaseScheduleSummaryTO.VehicleNo = tblPurchaseVehicleSpotEntryTO.VehicleNo;

                tblPurchaseScheduleSummaryTO.SpotEntryVehicleId = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                tblPurchaseScheduleSummaryTO.UpdatedBy = loginUserId;
                tblPurchaseScheduleSummaryTO.UpdatedOn = currentDate;

                tblPurchaseScheduleSummaryTO.VehicleNo = tblPurchaseVehicleSpotEntryTO.VehicleNo;
                // previousPurchaseScheduleSummaryTO.Qty=tblPurchaseVehicleSpotEntryTO.VehicleQtyMT;


                //Change later
                if (tblPurchaseVehicleSpotEntryTO.DriverName != null && tblPurchaseVehicleSpotEntryTO.DriverName != "")
                    previousPurchaseScheduleSummaryTO.DriverName = tblPurchaseVehicleSpotEntryTO.DriverName;

                //change location id also
                if (tblPurchaseVehicleSpotEntryTO.Location != null && tblPurchaseVehicleSpotEntryTO.Location != "")
                    previousPurchaseScheduleSummaryTO.Location = tblPurchaseVehicleSpotEntryTO.Location;
            }

            resultMessage = _iTblPurchaseScheduleSummaryBL.UpdateVehicleScheduleDetails(previousPurchaseScheduleSummaryTO, tblPurchaseScheduleSummaryTO, tblPurchaseVehicleSpotEntryTO, statusId, spotEntryVehicleStatusId, currentDate);
            if (resultMessage.MessageType == ResultMessageE.Information)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Record Updated Successfully";
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                resultMessage.Text = "API: Failed To Update Record";
            }
            return resultMessage;
        }

        [Route("PostUpdatedSpotEntryDetails")]
        [HttpPost]
        public ResultMessage PostUpdatedSpotEntryDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
            Int32 result = 0;

            if (tblPurchaseVehicleSpotEntryTO != null && tblPurchaseVehicleSpotEntryTO != null)
            {
                result = _iTblPurchaseVehicleSpotEntryBL.UpdateTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO);
                if (result >= 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Record Updated Successfully";
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "API: Failed To Update Record";
                }
            }
            return resultMessage;
        }


        [Route("PostVehicleScheduleDetails")]
        [HttpPost]
        public ResultMessage PostVehicleScheduleDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
            // tblPurchaseScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList= new List<TblPurchaseVehicleDetailsTO>();

            //string statusId = (data["statusId"].ToString());
            string spotEntryVehicleStatusId = (data["spotEntryVehicleStatusId"].ToString());
            tblPurchaseScheduleSummaryTO.CreatedBy = 1;
            tblPurchaseScheduleSummaryTO.CreatedOn = _iCommonDAO.ServerDateTime;
            tblPurchaseScheduleSummaryTO.UpdatedBy = 1;
            tblPurchaseScheduleSummaryTO.UpdatedOn = _iCommonDAO.ServerDateTime;

            resultMessage = _iTblPurchaseScheduleSummaryBL.InsertVehicleScheduleDetails(tblPurchaseScheduleSummaryTO, tblPurchaseVehicleSpotEntryTO, spotEntryVehicleStatusId);
            if (resultMessage.MessageType == ResultMessageE.Information)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Record Updated Successfully";
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                resultMessage.Text = "API: Failed To Update Record";
            }
            return resultMessage;
        }

        [Route("DeleteVehicleSchedule")]
        [HttpPost]
        public ResultMessage DeleteVehicleSchedule([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Boolean getScheduleDetails = Convert.ToBoolean(data["getScheduleDetails"].ToString());
            Boolean isSetPreviousStatus = Convert.ToBoolean(data["isSetPreviousStatus"].ToString());
            //Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
            //tblPurchaseScheduleSummaryTO.UpdatedBy = loginUserId;
            tblPurchaseScheduleSummaryTO.UpdatedOn = _iCommonDAO.ServerDateTime;
            //Prajakta
            int result = 0;
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDetails(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary, true);
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count == 1)
            {
                result = _iTblPurchaseScheduleSummaryBL.DeleteVehicleScheduleDetails(tblPurchaseScheduleSummaryTO, getScheduleDetails, isSetPreviousStatus);
                if (result > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Record Deleted Successfully";
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "API: Failed To Delete Record";
                }
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                resultMessage.Text = "Vehicle Status Is Already Changed.Please Check.";
            }

            return resultMessage;
        }

        [Route("UpdateVehicleDetails")]
        [HttpPost]
        public ResultMessage UpdateVehicleDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

            int result = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummary(tblPurchaseScheduleSummaryTO);
            if (result > 0)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Record Updated Successfully";
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                resultMessage.Text = "API: Failed To Update Record";
            }
            return resultMessage;
        }

        [Route("UpdateVehicleTypeOnly")]
        [HttpPost]
        public ResultMessage UpdateVehicleTypeOnly([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

            int result = _iTblPurchaseScheduleSummaryBL.UpdateVehicleTypeOnly(tblPurchaseScheduleSummaryTO);
            if (result > 0)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Record Updated Successfully";
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                resultMessage.Text = "API: Failed To Update Record";
            }
            return resultMessage;
        }


        [Route("UpdateWeighingCompletedAgainstVehicle")]
        [HttpPost]
        public ResultMessage UpdateWeighingCompletedAgainstVehicle([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

             return _iTblPurchaseScheduleSummaryBL.UpdateWeighingCompleted(tblPurchaseScheduleSummaryTO, loginUserId);
           
        }

        [Route("UpdateScheduleVehicleNoOnly")]
        [HttpPost]
        public ResultMessage UpdateScheduleVehicleNoOnly([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Int32 loginUserId = (Int32)(data["loginUserId"]);

            resultMessage = _iTblPurchaseScheduleSummaryBL.UpdateScheduleVehicleNoOnly(tblPurchaseScheduleSummaryTO, loginUserId);
            return resultMessage;
        }

        [Route("UpdateDensityAndVehicleType")]
        [HttpPost]
        public ResultMessage UpdateDensityAndVehicleType([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Int32 loginUserId = (Int32)(data["loginUserId"]);

            resultMessage = _iTblPurchaseScheduleSummaryBL.UpdateDensityAndVehicleType(tblPurchaseScheduleSummaryTO, loginUserId);
            return resultMessage;
        }


        [Route("UpdateSpotEntryVehicleSupplier")]
        [HttpPost]
        public ResultMessage UpdateSpotEntryVehicleSupplier([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
            Int32 loginUserId = (Int32)(data["loginUserId"]);

            resultMessage = _iTblPurchaseScheduleSummaryBL.UpdateSpotEntryVehicleSupplier(tblPurchaseVehicleSpotEntryTO, loginUserId);
            return resultMessage;
        }


        [Route("CompeleteRecoveryAgainstVehicle")]
        [HttpPost]
        public ResultMessage CompeleteRecoveryAgainstVehicle([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            int res = 0;
            int idSummary = tblPurchaseScheduleSummaryTO.RootScheduleId > 0 ? tblPurchaseScheduleSummaryTO.RootScheduleId : tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            int phaseID = Convert.ToInt32(Constants.PurchaseVehiclePhasesE.RECOVERY);
            resultMessage = _iTblPurchaseScheduleSummaryBL.CheckIfAllQualityFlagsAreCompleted(idSummary, phaseID);
            if (resultMessage.Result > 0)
            {
                return resultMessage;
            }
            resultMessage = _iTblPurchaseScheduleSummaryBL.CompeleteRecoveryAgainstVehicle(tblPurchaseScheduleSummaryTO);
            return resultMessage;
        }

        [Route("MarkUnloadingCompleteWithTareWtDtls")]
        [HttpPost]
        public ResultMessage MarkUnloadingCompleteWithTareWtDtls([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = JsonConvert.DeserializeObject<TblPurchaseWeighingStageSummaryTO>(data["PurchaseWeighingStageSummaryTO"].ToString());
            int res = 0;
            int idSummary = tblPurchaseScheduleSummaryTO.RootScheduleId > 0 ? tblPurchaseScheduleSummaryTO.RootScheduleId : tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            resultMessage = _iTblPurchaseScheduleSummaryBL.CheckIfAllQualityFlagsAreCompleted(idSummary, tblPurchaseScheduleSummaryTO.VehiclePhaseId);
            if (resultMessage.Result > 0)
            {
                return resultMessage;
            }

            return _iTblPurchaseScheduleSummaryBL.MarkUnloadingCompleteWithTareWtDtls(tblPurchaseScheduleSummaryTO, tblPurchaseWeighingStageSummaryTO);

        }

        [Route("UpdateBookingDtlsForSpotVehicle")]
        [HttpPost]
        public ResultMessage UpdateBookingDtlsForSpotVehicle([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["PurchaseVehicleSpotEntryTO"].ToString());
            Boolean isRevertLink = Convert.ToBoolean(data["isRevertLink"].ToString());

            return _iTblPurchaseVehicleSpotEntryBL.UpdateBookingDtlsForSpotVehicle(tblPurchaseVehicleSpotEntryTO, isRevertLink);

        }

        
        [Route("CheckGradingAndRecoveryCompleted")]
        [HttpPost]
        public ResultMessage CheckGradingAndRecoveryCompleted([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

            return _iTblPurchaseScheduleSummaryBL.CheckGradingAndRecoveryCompleted(tblPurchaseScheduleSummaryTO);

        }

        [Route("GetCombinedvehicleItemDtlsForCAndNC")]
        [HttpPost]
        public List<TblPurchaseScheduleSummaryTO> GetCombinedvehicleItemDtlsForCAndNC([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            List<TblPurchaseScheduleSummaryTO> scheduleTOList = null;

            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

            resultMessage = _iTblPurchaseScheduleSummaryBL.GetCombinedvehicleItemDtlsForCAndNC(tblPurchaseScheduleSummaryTO);
            if (resultMessage.MessageType == ResultMessageE.Information)
            {
                if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(List<TblPurchaseScheduleSummaryTO>))
                {
                    scheduleTOList = new List<TblPurchaseScheduleSummaryTO>();
                    scheduleTOList = (List<TblPurchaseScheduleSummaryTO>)resultMessage.Tag;
                }
            }
            else
            {
                scheduleTOList = null;
            }

            return scheduleTOList;

        }

        [Route("DataExtractionForCorrectionCompleVehicles")]
        [HttpGet]
        public ResultMessage DataExtractionForCorrectionCompleVehicles()
        {
            return _iTblPurchaseScheduleSummaryBL.DataExtractionForCorrectionCompleVehicles();
        }

        //Added by minal For Dropbox
        [Route("CreateAndBackupExcelFile")]
        [HttpGet]
        public ResultMessage CreateAndBackupExcelFile()
        {
            return _iTblPurchaseScheduleSummaryBL.DataFromExportToExcel();
        }

        [Route("WriteIotDataTODB")]
        [HttpGet]
        public ResultMessage WriteIotDataTODB()
        {
            return _iTblPurchaseScheduleSummaryBL.WriteIotDataTODB();
        }

        [Route("UpdateVehicleOutFlagForIOT")]
        [HttpGet]
        public ResultMessage UpdateVehicleOutFlagForIOT()
        {
            return _iTblPurchaseScheduleSummaryBL.UpdateVehicleOutFlagForIOT();
        }

        [Route("PostTallyReportListForExcel")]
        [HttpPost]
        public ResultMessage PostTallyReportListForExcel([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                loggerObj.LogError("In PostTallyReportListForExcel Method");
                List<TallyReportTO> tblInvoiceList = JsonConvert.DeserializeObject<List<TallyReportTO>>(data["data"].ToString());
                loggerObj.LogError("tblInvoiceList List" + tblInvoiceList);
                var result = _iTblPurchaseScheduleSummaryBL.PostTallyReportListForExcel(tblInvoiceList);
                if (result == 1)
                {
                    resultMessage.DefaultSuccessBehaviour();
                }
                return resultMessage;

            }
            catch (Exception ex)
            {
                loggerObj.LogError("Exception *******"+ ex.Message);
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }
          
        }
        [Route("PostVehicle")]
        [HttpPost]
        public ResultMessage PostVehicle([FromBody] JObject data,Int32 TransactionId = 0)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                lock (vehicleLock)
                {
                    switch (TransactionId)
                    {
                        case 1:
                            return InsertUpdatedMaterailItemDetails(data);
                            break;
                        case 2:
                            return MarkUnloadingCompleteWithTareWtDtls(data);
                            break;
                        case 3:
                            return MarkVehicleOut(data);
                            break;
                        default:
                            return resultMessage;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "failed to PostVehicle");
                return resultMessage;
            }
        }
        [Route("MarkVehicleOut")]
        [HttpPost]
        public ResultMessage MarkVehicleOut([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

            return _iTblPurchaseScheduleSummaryBL.MarkVehicleOut(tblPurchaseScheduleSummaryTO, loginUserId);

        }

        [Route("SubmitProcessCharge")]
        [HttpPost]
        public ResultMessage SubmitProcessCharge([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

            return _iTblPurchaseScheduleSummaryBL.SubmitProcessCharge(tblPurchaseScheduleSummaryTO);

        }

        [Route("UpdateApprovedWeighingDetails")]
        [HttpPost]
        public ResultMessage UpdateApprovedWeighingDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
            DateTime currentdate = _iCommonDAO.ServerDateTime;
            if (tblPurchaseScheduleSummaryTO.IsApproved == 1)
            {
                resultMessage = _iTblPurchaseScheduleSummaryBL.UpdateApprovedWeighingDetails(tblPurchaseScheduleSummaryTO, loginUserId);
            }
            else
            {
                bool fromApprovalScreen = true;
                resultMessage = _iTblPurchaseScheduleSummaryBL.MarkVehicleRejected(tblPurchaseScheduleSummaryTO, loginUserId, fromApprovalScreen);
            }
            return resultMessage;
        }

        [Route("MarkVehicleRejected")]
        [HttpPost]
        public ResultMessage MarkVehicleRejected([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

            return _iTblPurchaseScheduleSummaryBL.MarkVehicleRejected(tblPurchaseScheduleSummaryTO, loginUserId, false);


        }

        [Route("GetBaseMetalCostList")]
        [HttpGet]
        public List<DropDownTO> GetBaseMetalCostList(Int32 rootScheduleId)
        {
            return _iTblPurchaseScheduleSummaryBL.GetBaseMetalCostList(rootScheduleId);

        }


        [Route("CompareVehItemDtlsWithCurrentOrUnloladingDate")]
        [HttpPost]
        public List<TblPurchaseScheduleSummaryTO> CompareVehItemDtlsWithCurrentOrUnloladingDate([FromBody] JObject data)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = JsonConvert.DeserializeObject<List<TblPurchaseScheduleSummaryTO>>(data["purchaseScheduleSummaryTOList"].ToString());
            DropDownTO dropDownTO = JsonConvert.DeserializeObject<DropDownTO>(data["commonInfoTO"].ToString());

            return _iTblPurchaseScheduleSummaryBL.CompareVehItemDtlsWithCurrentOrUnloladingDate(tblPurchaseScheduleSummaryTOList, dropDownTO);


        }

        [Route("MigrateBaseMetalCostDtls")]
        [HttpPost]
        public ResultMessage MigrateBaseMetalCostDtls()
        {
            return _iTblPurchaseScheduleSummaryBL.MigrateBaseMetalCostDtls();

        }

        [Route("GetAllDescriptionList")]
        [HttpGet]
        public List<TblPurchaseItemDescTO> GetAllDescriptionList(Int32 rootScheduleId, Int32 itemId, Int32 stageId, Int32 phaseId)
        {
            return _iTblPurchaseItemDescBL.SelectAllTblPurchaseItemDescList(rootScheduleId, itemId, stageId, phaseId);

        }

        [Route("GetAllDescriptionListForCorrection")]
        [HttpGet]
        public List<TblPurchaseItemDescTO> GetAllDescriptionListForCorrection(Int32 rootScheduleId, Int32 itemId, Int32 phaseId)
        {
            return _iTblPurchaseItemDescBL.GetAllDescriptionListForCorrection(rootScheduleId, itemId, phaseId);

        }


        [Route("PostItemWiseDescription")]
        [HttpPost]
        public ResultMessage PostItemWiseDescription([FromBody] JObject data)
        {
            try
            {
                List<TblPurchaseItemDescTO> purchaseProdItemDescTOList = JsonConvert.DeserializeObject<List<TblPurchaseItemDescTO>>(data["purchaseProdItemDescTOList"].ToString());
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

                if (purchaseProdItemDescTOList != null && purchaseProdItemDescTOList.Count > 0)
                {
                    return _iTblPurchaseItemDescBL.InsertTblPurchaseItemDesc(purchaseProdItemDescTOList, loginUserId);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("CalculateBookingsOpeningBalance")]
        [HttpGet]
        public ResultMessage CalculateBookingsOpeningBalance()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result = _iTblPurchaseBookingOpngBalBL.calculateOpeningClosingBal();
                if (result > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Saved Successfully";
                    resultMessage.Result = 1;
                    // resultMessage.Exception = ex;
                    return resultMessage;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Exception In Method CalculateBookingsOpeningBalance";
                    resultMessage.Result = -1;
                    // resultMessage.Exception = ex;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method CalculateBookingsOpeningBalance";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }



        [Route("GetAllPendingSaudaForReport")]
        [HttpGet]
        public List<SaudaReportTo> GetAllPendingSaudaForReport(Int32 cnfOrgId = 0, Int32 dealerOrgId = 0, Int32 materialTypeId = 0, Int32 statusId = 0)
        {
            return _iTblPurchaseBookingOpngBalBL.GetAllPendingSaudaForReport(cnfOrgId, dealerOrgId, materialTypeId, statusId);
        }


        [Route("MarkVehicleRequested")]
        [HttpPost]
        public ResultMessage MarkVehicleRequested([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = JsonConvert.DeserializeObject<List<TblPurchaseScheduleSummaryTO>>(data["purchaseScheduleSummaryTOList"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

            resultMessage = _iTblPurchaseScheduleSummaryBL.MarkVehicleRequested(tblPurchaseScheduleSummaryTOList,loginUserId);
            return resultMessage;
        }

        [Route("UpdateMaterialTypeOfSauda")]
        [HttpPost]
        public ResultMessage UpdateMaterialTypeOfSauda([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
            Boolean isUpdateMaterialType = Convert.ToBoolean(data["isUpdateMaterialType"].ToString());

            return resultMessage = _iTblPurchaseScheduleSummaryBL.UpdateMaterialTypeOfSauda(tblPurchaseScheduleSummaryTO,loginUserId,isUpdateMaterialType);
            
        }

        [Route("GetVehTotalQtyDashboardInfo")]
        [HttpGet]
        public TblPurchaseScheduleSummaryTO GetVehTotalQtyDashboardInfo(string loginUserId = "")
        {
            return _iTblPurchaseScheduleSummaryBL.GetVehTotalQtyDashboardInfo(loginUserId);
        }

        [Route("PostVehicleFreightDtls")]
        [HttpPost]
        public ResultMessage PostVehicleFreightDtls([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO = JsonConvert.DeserializeObject<TblPurchaseVehFreightDtlsTO>(data["purchaseVehFreightDtlsTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblPurchaseVehFreightDtlsTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseVehFreightDtlsTO Found NULL";
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseScheduleSummaryBL.PostVehicleFreightDtls(tblPurchaseVehFreightDtlsTO, Convert.ToInt32(loginUserId));
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in PostVehicleFreightDtls([FromBody] JObject data)");
                return resultMessage;
            }
        }

        [Route("GetVehicleFreightDtls")]
        [HttpGet]
        public List<TblPurchaseVehFreightDtlsTO> GetVehicleFreightDtls(Int32 purchaseScheduleSummaryId)
        {
            return _iTblPurchaseVehFreightDtlsBL.SelectFreightDtlsByPurchaseScheduleId(purchaseScheduleSummaryId);
        }

        [Route("GetVehicleLinkSaudaDtls")]
        [HttpGet]
        public List<TblPurchaseVehLinkSaudaTO> GetVehicleLinkSaudaDtls(Int32 rootScheduleId)
        {
            return _iTblPurchaseVehLinkSaudaBL.SelectTblPurchaseVehLinkSauda(rootScheduleId);
        }


        [Route("UpdateSupplier")]
        [HttpPost]
        public ResultMessage UpdateSupplier([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseScheduleSummaryTO scheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());
                Int32 supplierId = Convert.ToInt32(data["supplierId"].ToString());

                if (supplierId <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : supplierId = 0";
                    return resultMessage;
                }

                if (scheduleSummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : scheduleSummaryTO = NULL";
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseScheduleSummaryBL.UpdateSupplier(scheduleSummaryTO, supplierId);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in PostVehicleFreightDtls([FromBody] JObject data)");
                return resultMessage;
            }
        }

        [Route("LinkVehicleToExistingSauda")]
        [HttpPost]
        public ResultMessage LinkVehicleToExistingSauda([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["PurchaseVehicleSpotEntryTO"].ToString());

                if (tblPurchaseVehicleSpotEntryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseVehicleSpotEntryTO Found NULL";
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseScheduleSummaryBL.LinkVehicleToExistingSauda(tblPurchaseVehicleSpotEntryTO);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in LinkVehicleToExistingSauda([FromBody] JObject data)");
                return resultMessage;
            }
        }

        [Route("LinkVehicleToExistingSaudaList")]
        [HttpPost]
        public ResultMessage LinkVehicleToExistingSaudaList([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

                if (tblPurchaseScheduleSummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseScheduleSummaryTO Found NULL";
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseScheduleSummaryBL.LinkVehicleToExistingSaudaList(tblPurchaseScheduleSummaryTO);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in LinkVehicleToExistingSauda([FromBody] JObject data)");
                return resultMessage;
            }
        }

        //[Route("LinkVehicleToExistingSaudaList")]
        //[HttpPost]
        //public ResultMessage LinkVehicleToExistingSaudaList([FromBody] JObject data)
        //{
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    try
        //    {
        //        TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

        //        if (tblPurchaseScheduleSummaryTO == null)
        //        {
        //            resultMessage.DefaultBehaviour();
        //            resultMessage.Text = "API : tblPurchaseScheduleSummaryTO Found NULL";
        //            return resultMessage;
        //        }

        //        resultMessage = _iTblPurchaseScheduleSummaryBL.LinkVehicleToExistingSaudaList(tblPurchaseScheduleSummaryTO);
        //        return resultMessage;

        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "Error in LinkVehicleToExistingSauda([FromBody] JObject data)");
        //        return resultMessage;
        //    }
        //}

        [Route("PostScheduleTcDtls")]
        [HttpPost]
        public ResultMessage PostScheduleTcDtls([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseSchTcDtlsTO> tblPurchaseSchTcDtlsTOList = JsonConvert.DeserializeObject<List<TblPurchaseSchTcDtlsTO>>(data["purchaseSchTcDtlsTOList"].ToString());
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

                if (tblPurchaseSchTcDtlsTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseSchTcDtlsTOList Found NULL";
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseScheduleSummaryBL.PostScheduleTcDtls(tblPurchaseSchTcDtlsTOList, loginUserId);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in PostScheduleTcDtls([FromBody] JObject data)");
                return resultMessage;
            }
        }


        #endregion

        [Route("GetAddOnsDetails")]
        [HttpGet]
        public List<DropDownTO> GetAddOnsDetails(Int32 transId, Int32 ModuleId, string TransactionType, Int32 PageElementId)
        {
            try
            {
                return _iTblPurchaseScheduleSummaryBL.SelectAllVehAddOnFunDtls(transId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        // Deepali Added for task no 1151
        [Route("GetContainerDetailsBySpotEntryId")]
        [HttpGet]
        public List<TblSpotEntryContainerDtlsTO> GetContainerDetailsBySpotEntryId(int spotEntryId)
        {           
            
            List<TblSpotEntryContainerDtlsTO> tblSpotEntryContainerDtlsTOList = _iTblPurchaseScheduleSummaryBL.SelectContainerDetailsBySpotEntryId(spotEntryId);
            return tblSpotEntryContainerDtlsTOList;
        }


    }

}
