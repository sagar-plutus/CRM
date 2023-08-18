using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.DashboardModels;
using System.Globalization;
using SalesTrackerAPI.BL;
using Microsoft.Extensions.Logging;
using System.Threading;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoadSlipController : Controller
    {

        private readonly ILogger loggerObj;

        public LoadSlipController(ILogger<LoadSlipController> logger)
        {
            loggerObj = logger;
            Constants.LoggerObj = logger;
        }

        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("CorrectTareWeightAgainstVehicle")]
        [HttpGet]
        public ResultMessage CorrectTareWeightAgainstVehicle()
        {
            return TblLoadingBL.CorrectTareWeightAgainstVehicle();
        }



        [Route("GetLoadStsReasonsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetLoadStsReasonsForDropDown(Int32 statusId)
        {
            List<TblStatusReasonTO> tblStatusReasonTOList = BL.TblStatusReasonBL.SelectAllTblStatusReasonList(statusId);
            if (tblStatusReasonTOList != null && tblStatusReasonTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblStatusReasonTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblStatusReasonTOList[i].ReasonDesc;
                    dropDownTO.Value = tblStatusReasonTOList[i].IdStatusReason;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }


        [Route("RemoveVehOutDatFromIotDevice")]
        [HttpGet]
        public ResultMessage RemoveVehOutDatFromIotDevice()
        {
            return TblLoadingBL.RemoveDatFromIotDevice();
        }

        [Route("PingPongExecution")]
        [HttpGet]
        public int PingPongExecution()
        {
            //TblLoadingBL.PingPongExecution();
            return 1;
        }

        [Route("GetSuperwisorListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetSuperwisorListForDropDown()
        {
            //For testing only
            //SalesTrackerAPI.IoT.IotCommunication.DeleteSingleLoadingFromWeightIoT(1,0, "http://192.168.20.13:3000/api/");

            List<TblSupervisorTO> tblSupervisorTOList = BL.TblSupervisorBL.SelectAllTblSupervisorList();
            if (tblSupervisorTOList != null && tblSupervisorTOList.Count > 0)
            {
                tblSupervisorTOList = tblSupervisorTOList.OrderBy(a => a.SupervisorName).ToList();
                Dictionary<Int32, Int32> DCT = BL.TblLoadingBL.SelectCountOfLoadingsOfSuperwisorDCT(Constants.ServerDateTime);

                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblSupervisorTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Value = tblSupervisorTOList[i].IdSupervisor;

                    String countToAppend = string.Empty;
                    if (DCT != null && DCT.ContainsKey(tblSupervisorTOList[i].IdSupervisor))
                        countToAppend = " (" + DCT[tblSupervisorTOList[i].IdSupervisor] + ")";
                    dropDownTO.Text = tblSupervisorTOList[i].SupervisorName + countToAppend;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetVehicleNumberList")]
        [HttpGet]
        public List<VehicleNumber> GetVehicleNumberList()
        {
            return BL.TblLoadingBL.SelectAllVehicles();
        }

        [Route("GetLoadingslipDetails")]
        [HttpGet]
        public TblLoadingTO GetLoadingslipDetails(Int32 loadingId)
        {
            TblLoadingTO tblLoadingTO = BL.TblLoadingBL.SelectLoadingTOWithDetails(loadingId);
            return tblLoadingTO;
        }

        [Route("GetUrlToPrintLoadingSlip")]
        [HttpPost ]
        public ResultMessage GetUrlToPrintLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            int LoadingSlipNo = Convert.ToInt32(data["LoadingSlipNo"]);
            return resultMessage = BL.TblLoadingBL.GetUrlToPrintLoadingSlip(LoadingSlipNo);
        }

        /// <summary>
        /// GJ@20171012 : Get the LoadingTo details by LoadingSlipId
        /// </summary>
        /// <param name="loadingId"></param>
        /// <returns></returns>
        [Route("GetLoadingTODetailsByLoadingSlipId")]
        [HttpGet]
        public TblLoadingTO GetLoadingTODetailsByLoadingSlipId(Int32 loadingSlipId)
        {
            return BL.TblLoadingBL.SelectLoadingTOWithDetailsByLoadingSlipId(loadingSlipId);
        }


        [Route("GetAllLoadingSlipList")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetAllLoadingSlipList(string userRoleTOList, Int32 cnfId, Int32 loadingStatusId, string fromDate, String toDate, Int32 loadingTypeId, Int32 dealerId, string selectedOrgStr, Int32 isConfirm = -1, Int32 brandId = 0, Int32 superwisorId = 0,int isFromBasicMode = 0)
        {
            try
            {
                DateTime frmDate = DateTime.MinValue;
                DateTime tDate = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                DateTime serverDate = Constants.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate.Date;
                if (tDate == DateTime.MinValue)
                    tDate = serverDate.Date;

                if (string.IsNullOrEmpty(selectedOrgStr) && isFromBasicMode != 1)
                {
                    TblConfigParamsTO tblConfigParamsTO = TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (tblConfigParamsTO != null)
                    {
                        selectedOrgStr = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                    }
                }
                List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

                return TblLoadingSlipBL.SelectAllLoadingSlipList(tblUserRoleTOList, cnfId, loadingStatusId, frmDate, tDate, loadingTypeId, dealerId, selectedOrgStr,isFromBasicMode);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [Route("GetLoadingslipListWithDetails")]
        [HttpGet]
        public List<TblLoadingTO> GetLoadingslipDetailsList(string loadingIds)
        {
            return BL.TblLoadingBL.SelectLoadingTOListWithDetails(loadingIds);
        }

        [Route("GetAllPendingLoadingList")]
        [HttpGet]
        public List<TblLoadingTO> GetAllPendingLoadingList(string userRoleTOList, Int32 cnfId, Int32 loadingStatusId, string fromDate, String toDate, Int32 loadingTypeId, Int32 dealerId, string selectedOrgStr, Int32 isConfirm = -1, Int32 brandId = 0, Int32 loadingNavigateId = 0, Int32 deliveryInformation = 0)
        {


            try
            {
                DateTime frmDate = DateTime.MinValue;
                DateTime tDate = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                DateTime serverDate = Constants.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate.Date;
                if (tDate == DateTime.MinValue)
                    tDate = serverDate.Date;

                //TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(roleId);
                List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

                var list = BL.TblLoadingBL.SelectAllTblLoadingList(tblUserRoleTOList, cnfId, loadingStatusId, frmDate, tDate, loadingTypeId, deliveryInformation, dealerId, selectedOrgStr, isConfirm, brandId, loadingNavigateId);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [Route("GetAllBookingScheduleList")]
        [HttpGet]
        public List<TblBookingScheduleTO> GetAllBookingScheduleList(string fromDate, string toDate,Int32 cnfOrgId,Int32 dealerOrgId)
        {

            try
            {
                DateTime frmDate = DateTime.MinValue;
                DateTime tDate = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                DateTime serverDate = Constants.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate.Date;
                if (tDate == DateTime.MinValue)
                    tDate = serverDate.Date;

                var list = BL.TblBookingScheduleBL.GetAllBookingScheduleList(frmDate, tDate, cnfOrgId, dealerOrgId);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("GetAllLoadingListByVehicleNo")]
        [HttpGet]
        public List<TblLoadingTO> GetAllLoadingListByVehicleNo(string vehicleNo, String loadingDate)
        {
            try
            {

                DateTime frmDate = Convert.ToDateTime(Convert.ToDateTime(loadingDate).ToString(Constants.AzureDateFormat));
                DateTime serverDate = Constants.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate;

                return BL.TblLoadingBL.SelectAllLoadingListByVehicleNo(vehicleNo, frmDate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GJ@20170829 : Get the Loading slip list those are in but not completed losding slip
        /// </summary>
        /// <param name="prodCatId"></param>
        /// <param name="prodSpecId"></param>
        /// <returns></returns>
        [Route("GetAllInLoadingListByVehicleNo")]
        [HttpGet]
        public List<TblLoadingTO> GetAllInLoadingListByVehicleNo(string vehicleNo, int loadingId = 0)
        {
            try
            {

                return BL.TblLoadingBL.SelectAllLoadingListByVehicleNo(vehicleNo, false, loadingId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("GetCnfLoadingCofiguration")]
        [HttpGet]
        public List<TblLoadingQuotaConfigTO> GetCnfLoadingCofiguration(Int32 prodCatId, Int32 prodSpecId)
        {
            List<TblLoadingQuotaConfigTO> list = BL.TblLoadingQuotaConfigBL.SelectLatestLoadingQuotaConfigList(prodCatId, prodSpecId);
            return list;
        }

        [Route("GetCnfLoadingCofigurationForOther")]
        [HttpGet]
        public List<TblLoadingQuotaConfigTO> GetCnfLoadingCofigurationForOther()
        {
            List<TblLoadingQuotaConfigTO> list = BL.TblLoadingQuotaConfigBL.SelectLatestLoadingQuotaConfigForOther();
            return list;
        }

        [Route("GetCnfLoadingQuotaDeclaration")]
        [HttpGet]
        public List<TblLoadingQuotaDeclarationTO> GetCnfLoadingQuotaDeclaration(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = Constants.ServerDateTime;

            List<TblLoadingQuotaDeclarationTO> list = BL.TblLoadingQuotaDeclarationBL.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate, prodCatId, prodSpecId);
            return list;
        }

        [Route("IsLoadingQuotaDeclaredToday")]
        [HttpGet]
        public Boolean IsLoadingQuotaDeclaredToday(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = Constants.ServerDateTime;

            return BL.TblLoadingQuotaDeclarationBL.IsLoadingQuotaDeclaredForTheDate(stockDate, prodCatId, prodSpecId);
        }

        [Route("IsLoadingQuotaDeclaredForTheDate")]
        [HttpGet]
        public Boolean IsLoadingQuotaDeclaredForTheDate(DateTime stockDate)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = Constants.ServerDateTime;
            return BL.TblLoadingQuotaDeclarationBL.IsLoadingQuotaDeclaredForTheDate(stockDate);
        }

        [Route("GetAvailableLoadingQuotaForCnf")]
        [HttpGet]
        public List<TblLoadingQuotaDeclarationTO> GetAvailableLoadingQuotaForCnf(Int32 cnfId, DateTime stockDate)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = Constants.ServerDateTime;

            List<TblLoadingQuotaDeclarationTO> list = BL.TblLoadingQuotaDeclarationBL.SelectAvailableLoadingQuotaForCnf(cnfId, stockDate);
            return list;
        }

        [Route("GetEmptySizeAndProductListForLoading")]
        [HttpGet]
        public List<TblLoadingSlipExtTO> GetEmptySizeAndProductListForLoading(Int32 prodCatId, Int32 boookingId)
        {
            List<TblLoadingSlipExtTO> list = BL.TblLoadingSlipExtBL.SelectEmptyLoadingSlipExtList(prodCatId, boookingId);
            return list;
        }

        [Route("GetEmptySizeAndProdLstForNewLoading")]
        [HttpGet]
        public List<TblLoadingSlipExtTO> GetEmptySizeAndProductListForLoading(Int32 prodCatId, Int32 prodSpecId, Int32 boookingId)
        {
            List<TblLoadingSlipExtTO> list = BL.TblLoadingSlipExtBL.SelectEmptyLoadingSlipExtList(prodCatId, prodSpecId, boookingId);
            return list;
        }


        /// <summary>
        /// Sanjay [2017-05-25] It will return material details alongwith its layer details
        /// Can be used to show popup for showing why Loading Approval is needed based on quotaafterloading property
        /// </summary>
        /// <param name="loadingId"></param>
        /// <returns></returns>
        [Route("GetLoadingMaterialDetails")]
        [HttpGet]
        public List<TblLoadingSlipExtTO> GetLoadingMaterialDetails(Int32 loadingId)
        {
            List<TblLoadingSlipExtTO> list = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(loadingId);
            return list;
        }


        [Route("GetLoadingSlipsByStatus")]
        [HttpGet]
        public List<TblLoadingTO> GetLoadingSlipsByStatus(String statusId)
        {
            //var watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            int weightSourceConfigId = Constants.getweightSourceConfigTO();
            List<TblLoadingTO> list = new List<TblLoadingTO>();
            var tempStatusIds = statusId;
            if (weightSourceConfigId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                tempStatusIds = Convert.ToString((int)Constants.TranStatusE.LOADING_CONFIRM);
            }
            list = BL.TblLoadingBL.SelectAllLoadingListByStatus(tempStatusIds);
            //int configId = Constants.getweightSourceConfigTO();

            if (list != null && list.Count > 0)
            {
                //var watchiot = new System.Diagnostics.Stopwatch();
                //watchiot.Start();
                //list = TblLoadingBL.SetLoadingStatusData(statusId, configId, list);
                string finalStatusId = IoT.IotCommunication.GetIotEncodedStatusIdsForGivenStatus(statusId);
                list = TblLoadingBL.SetLoadingStatusData(finalStatusId, true, weightSourceConfigId, list);
                //watchiot.Stop();
                //long resiot = watchiot.ElapsedMilliseconds;

                var statusIds = statusId.Split(',').ToList();
                list = list.Where(w => statusIds.Contains(Convert.ToString(w.StatusId))).ToList();

                //watch.Stop();
                //long res = watch.ElapsedMilliseconds;

                List<TblLoadingTO> finalList = new List<TblLoadingTO>();
                for (int i = 0; i < statusIds.Count; i++)
                {
                    if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_CONFIRM
                        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
                        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)
                    {
                        var sendInList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (sendInList != null)
                            finalList.AddRange(sendInList);

                        var reportedList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (reportedList != null)
                            finalList.AddRange(reportedList);

                        var confirmList = list.Where(r => r.StatusId != (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
                                                     && r.StatusId != (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (confirmList != null)
                            finalList.AddRange(confirmList);

                        return finalList;
                    }
                    else if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_GATE_IN
                        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_COMPLETED
                        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_IN_PROGRESS
                        || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH
                        )

                    // Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH
                    {

                        var reportedList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_COMPLETED).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (reportedList != null)
                            finalList.AddRange(reportedList);

                        var confirmList = list.Where(r => r.StatusId != (int)Constants.TranStatusE.LOADING_COMPLETED).ToList().OrderBy(d => d.StatusDate).ToList();
                        if (confirmList != null)
                            finalList.AddRange(confirmList);

                        return finalList;
                    }
                }
            }

            return list;
        }

        //[Route("GetLoadingSlipsByStatus")]
        //[HttpGet]
        //public async Task<List<TblLoadingTO>> GetLoadingSlipsByStatus(String statusId)
        //{
        //    var watch = new System.Diagnostics.Stopwatch();
        //    watch.Start();

        //    int weightSourceConfigId = Constants.getweightSourceConfigTO();
        //    List<TblLoadingTO> list = new List<TblLoadingTO>();
        //    var tempStatusIds = statusId;
        //    if (weightSourceConfigId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
        //    {
        //        tempStatusIds = Convert.ToString((int)Constants.TranStatusE.LOADING_CONFIRM);
        //    }
        //    list = BL.TblLoadingBL.SelectAllLoadingListByStatus(tempStatusIds);
        //    int configId = Constants.getweightSourceConfigTO();

        //    //var watchiot = new System.Diagnostics.Stopwatch();
        //    //watchiot.Start();

        //    string finalStatusId = IoT.IotCommunication.GetIotEncodedStatusIdsForGivenStatus(statusId);
        //    list = await TblLoadingBL.SetLoadingStatusDataV2(finalStatusId,true, configId, list);

        //    //watchiot.Stop();
        //    //long resiot = watchiot.ElapsedMilliseconds;

        //    var statusIds = statusId.Split(',').ToList();
        //    list = list.Where(w => statusIds.Contains(Convert.ToString(w.StatusId))).ToList();



        //    //this.loggerObj.LogInformation("GetLoadingSlipsByStatus Req Time ms : " + res);

        //    if (list != null)
        //    {
        //        List<TblLoadingTO> finalList = new List<TblLoadingTO>();
        //        for (int i = 0; i < statusIds.Count; i++)
        //        {
        //            if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_CONFIRM
        //                || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
        //                || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)
        //            {
        //                var sendInList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN).ToList().OrderBy(d => d.StatusDate).ToList();
        //                if (sendInList != null)
        //                    finalList.AddRange(sendInList);

        //                var reportedList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING).ToList().OrderBy(d => d.StatusDate).ToList();
        //                if (reportedList != null)
        //                    finalList.AddRange(reportedList);

        //                var confirmList = list.Where(r => r.StatusId != (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
        //                                             && r.StatusId != (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN).ToList().OrderBy(d => d.StatusDate).ToList();
        //                if (confirmList != null)
        //                    finalList.AddRange(confirmList);

        //                watch.Stop();
        //                long res = watch.ElapsedMilliseconds;

        //                return finalList;
        //            }
        //            else if (Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_GATE_IN
        //                || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_COMPLETED
        //                || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.LOADING_IN_PROGRESS
        //                || Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH
        //                )

        //            // Convert.ToInt32(statusIds[i]) == (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH
        //            {

        //                var reportedList = list.Where(r => r.StatusId == (int)Constants.TranStatusE.LOADING_COMPLETED).ToList().OrderBy(d => d.StatusDate).ToList();
        //                if (reportedList != null)
        //                    finalList.AddRange(reportedList);

        //                var confirmList = list.Where(r => r.StatusId != (int)Constants.TranStatusE.LOADING_COMPLETED).ToList().OrderBy(d => d.StatusDate).ToList();
        //                if (confirmList != null)
        //                    finalList.AddRange(confirmList);

        //                watch.Stop();
        //                long res = watch.ElapsedMilliseconds;

        //                return finalList;
        //            }
        //        }
        //    }

        //    return list;
        //}

        [Route("GetLoadingDashboardInfo")]
        [HttpGet]
        public LoadingInfo GetLoadingDashboardInfo(String userRoleList, Int32 orgId, DateTime sysDate, Int32 loadingType = 1)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = Constants.ServerDateTime;

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleList);
            return BL.TblLoadingBL.SelectDashboardLoadingInfo(tblUserRoleTOList, orgId, sysDate, loadingType);
        }

        [Route("IsLoadingAllowed")]
        [HttpGet]
        public ResultMessage IsLoadingAllowed(DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = Constants.ServerDateTime;

            ResultMessage resultMessage = new ResultMessage();
            TblLoadingAllowedTimeTO latestAllowedLoadingTimeTO = BL.TblLoadingAllowedTimeBL.SelectAllowedLoadingTimeTO(sysDate);
            if (latestAllowedLoadingTimeTO == null)
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_DEFAULT_ALLOWED_UPTO_TIME);
                Double maxAllowedDelPeriod = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);

                DateTime serverDate = Constants.ServerDateTime;
                DateTime curSysDate = serverDate.Date;
                DateTime dateToCheck = curSysDate.AddHours(maxAllowedDelPeriod);
                if (serverDate < dateToCheck)
                {
                    resultMessage.Result = 1;
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Allowed Upto " + dateToCheck.ToString(Constants.DefaultDateFormat);
                }
                else
                {
                    resultMessage.Result = 0;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Loading is allowed upto " + dateToCheck.ToString(Constants.DefaultDateFormat) + " only";
                }
            }
            else
            {
                DateTime serverDate = Constants.ServerDateTime;
                if (serverDate < latestAllowedLoadingTimeTO.AllowedLoadingTime)
                {
                    resultMessage.Result = 1;
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Allowed Upto " + latestAllowedLoadingTimeTO.AllowedLoadingTime.ToString(Constants.DefaultDateFormat);
                }
                else
                {
                    resultMessage.Result = 0;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Loading is allowed upto " + latestAllowedLoadingTimeTO.AllowedLoadingTime.ToString(Constants.DefaultDateFormat) + " only";
                }
            }

            return resultMessage;
        }


        [Route("IsLoadingSlipCanBePrepared")]
        [HttpGet]
        public ResultMessage IsLoadingSlipCanBePrepared(int parentLoadingId)
        {

            ResultMessage resultMessage = new ResultMessage();
            List<TblLoadingTO> list = BL.TblLoadingBL.SelectAllLoadingsFromParentLoadingId(parentLoadingId);
            if (list == null || list.Count == 0)
            {
                resultMessage.Result = 1;
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Allowed, No Loading Slips Prepared.";
            }
            else
            {
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Not Allowed. Loading is already Prepared against this . Ref No : " + list[0].IdLoading + " " + list[0].LoadingSlipNo + " Vehicle No :" + list[0].VehicleNo;
            }

            return resultMessage;
        }

        [Route("GetAllowedLoadingTimeDetails")]
        [HttpGet]
        public TblLoadingAllowedTimeTO GetAllowedLoadingTimeDetails(DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = Constants.ServerDateTime;

            ResultMessage resultMessage = new ResultMessage();
            TblLoadingAllowedTimeTO latestAllowedLoadingTimeTO = BL.TblLoadingAllowedTimeBL.SelectAllowedLoadingTimeTO(sysDate);
            if (latestAllowedLoadingTimeTO == null)
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_DEFAULT_ALLOWED_UPTO_TIME);
                Double maxAllowedDelPeriod = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);

                DateTime serverDate = Constants.ServerDateTime;
                DateTime curSysDate = serverDate.Date;
                DateTime dateToCheck = curSysDate.AddHours(maxAllowedDelPeriod);
                latestAllowedLoadingTimeTO = new TblLoadingAllowedTimeTO();
                latestAllowedLoadingTimeTO.AllowedLoadingTime = dateToCheck;
                latestAllowedLoadingTimeTO.CreatedOn = serverDate;
            }

            return latestAllowedLoadingTimeTO;
        }

        [Route("IsThisVehicleDelivered")]
        [HttpGet]
        public ResultMessage IsThisVehicleDelivered(String vehicleNo)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<TblLoadingTO> list = BL.TblLoadingBL.SelectAllLoadingListByVehicleNo(vehicleNo, true, 0);
            if (list == null || list.Count == 0)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Allowed , All Loadings Are Delivered";
                resultMessage.Result = 1;
            }
            else
            {
                var lastObj = list.OrderByDescending(s => s.StatusDate).FirstOrDefault();
                if (lastObj != null && lastObj.IsAllowNxtLoading == 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Allowed ,  next Loadings is Allowed";
                    resultMessage.Result = 1;
                    return resultMessage;

                }
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Not Allowed , Selected Vehicle :" + vehicleNo + " is not delivered . Last status is " + lastObj.StatusDesc;
                resultMessage.Result = 0;
            }

            return resultMessage;
        }


        [Route("GetAddressesForNewLoadingSlip")]
        [HttpGet]
        public List<TblBookingDelAddrTO> GetAddressesForNewLoadingSlip(Int32 addrSourceTypeId, Int32 entityId)
        {
            return BL.TblBookingDelAddrBL.SelectDeliveryAddrListFromDealer(addrSourceTypeId, entityId);
        }

        /// <summary>
        ///  YogeshAchrya@26052020 : get All address details list againts organisation with dealer name
        /// </summary>
        /// <param name="OrganisationId"></param>
        /// <returns></returns>
        [Route("GetAllAddressesDetailsList")]
        [HttpGet]
        public List<TblBookingDelAddrTO> GetAllAddressesDetailsList(Int32 OrganisationId)
        {
            return BL.TblBookingDelAddrBL.GetAllAddressesDetailsList(OrganisationId);
        }

        /// <summary>
        /// GJ@20170810 : get All vehicle number List , those are in 'In' premises
        /// </summary>
        /// <param name="statusId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("GetVehicleNumberListByStauts")]
        [HttpGet]
        public List<DropDownTO> GetVehicleNumberListByStauts(int statusId, int status, int userId = 0)
        {
            //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Main START ");
            List<DropDownTO> list = BL.TblLoadingBL.SelectAllVehiclesByStatus(statusId, status, userId);
            //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Main END ");
            return list;

        }

        /// <summary>
        /// Vaibhav [14-Sep-2017] Added to select all unloading slip details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllUnLoadingDetails")]
        [HttpGet]
        public List<TblUnLoadingTO> GetAllUnLoadingDetails(DateTime startDate, DateTime endDate)
        {
            return BL.TblUnLoadingBL.SelectAllTblUnLoadingList(startDate, endDate);
        }

        /// <summary>
        /// Vaibhav [14-Sep-2017] Added to select particular unloading slip details 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetUnLoadingDetails")]
        [HttpGet]
        public TblUnLoadingTO GetUnLoadingDetails(Int32 unLoadingId)
        {
            return BL.TblUnLoadingBL.SelectTblUnLoadingTO(unLoadingId);
        }

        /// <summary>
        /// Vaibhav [20-Sep-2017] Added to get unloading standard desc list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetUnloadingStandDescList")]
        [HttpGet]
        public List<TblUnloadingStandDescTO> GetUnloadingStandDescList()
        {
            return BL.TblUnloadingStandDescBL.SelectAllTblUnloadingStandDescList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [Route("IsInvoiceGeneratedByLoadingSlipId")]
        [HttpGet]
        public TblInvoiceTO IsInvoiceGeneratedByLoadingSlipId(Int32 idLoadingSlip)
        {
            List<TblInvoiceTO> invoiceTOList = BL.TblInvoiceBL.SelectInvoiceTOFromLoadingSlipId(idLoadingSlip);
            //if (string.IsNullOrEmpty(invoiceTO.Select(ele =>ele.InvoiceNo).SingleOrDefault()))
            //{
            //    return null;
            //}
            //return invoiceTO;

            // Vaibhav [05-Jan-2018] As per dicussion with saket make changes.
            if (invoiceTOList != null && invoiceTOList.Count > 0)
            {
                foreach (var invoiceTO in invoiceTOList)
                {
                    if (!string.IsNullOrEmpty(invoiceTO.InvoiceNo))
                    {
                        return invoiceTO;
                    }
                }

                return invoiceTOList[0];
            }
            else
                return null;
        }

        /// <summary>
        /// GJ@20171012 : get the Loading Slip details By LoadingSlipId
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetLoadingSlipDetailsByLoadingSlipId")]
        [HttpGet]
        public TblLoadingSlipTO GetLoadingSlipDetailsByLoadingSlipId(Int32 loadingSlipId)
        {
            return BL.TblLoadingSlipBL.SelectAllLoadingSlipWithDetails(loadingSlipId);
        }

        //Priyanka [08-05-2018] : Added for Displaying ORC Report for booking and loading.
        [Route("GetORCReportDetailsList")]
        [HttpGet]
        public List<TblORCReportTO> GetORCReportDetailsList(DateTime fromDate, DateTime toDate, Int32 flag,string selectOrgIdStr)
        {
            return BL.TblLoadingSlipBL.SelectORCReportDetailsList(fromDate, toDate, flag, selectOrgIdStr);
        }

        //Sudhir
        [Route("GetAllLoadingListByVehicleNoForSupport")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetAllLoadingListByVehicleNoForSupport(string vehicleNo)
        {
            try
            {

                //DateTime frmDate = Convert.ToDateTime(Convert.ToDateTime(loadingDate).ToString(Constants.AzureDateFormat));
                //DateTime serverDate = Constants.ServerDateTime;
                //if (frmDate == DateTime.MinValue)
                //    frmDate = serverDate;

                return BL.TblLoadingSlipBL.SelectAllLoadingListByVehicleNo(vehicleNo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Sudhir
        [Route("GetLoadingTODetailsByLoadingSlipNoForSupport")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetLoadingTODetailsByLoadingSlipNoForSupport(String loadingSlipNo)
        {
            return BL.TblLoadingSlipBL.SelectLoadingTOWithDetailsByLoadingSlipIdForSupport(loadingSlipNo);
        }


        /// <summary>
        /// [27-02-2018] Sudhir : Added To Get Loading Cycle List By Date. 
        /// </summary>
        /// <returns></returns>

        [Route("GetAllLoadingCycleListByDate")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetAllLoadingCycleListByDate(string fromDate, string toDate, String userRoleTOList, Int32 cnfId, Int32 vehicleStatus)
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
                frmDt = Constants.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = Constants.ServerDateTime.Date;

            //vijaymala[04-04-2018]modify the code to display records acc to role
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return BL.TblLoadingSlipBL.SelectAllLoadingCycleList(frmDt, toDt, tblUserRoleTOList, cnfId, vehicleStatus);
            // return BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtByDate(frmDt, toDt);
        }


        /// <summary>
        /// Vijaymala[24-04-2018] added : to get loading details by using booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [Route("GetLoadingTODetailsByBookingId")]
        [HttpGet]
        public List<TblLoadingTO> GetLoadingTODetailsByBookingId(Int32 bookingId)
        {
            return BL.TblLoadingBL.SelectAllTblLoadingByBookingId(bookingId);
        }

        /// <summary>
        /// Vijaymala [08-05-2018] added to get notified loading list withiin period 
        /// </summary>
        /// <returns></returns>
        [Route("GetAllNotifiedLoadingList")]
        [HttpGet]
        public List<TblLoadingSlipTO> GetAllNotifiedLoadingList(string fromDate, String toDate, Int32 callFlag,string selectOrgIdStr)
        {
            try
            {
                DateTime frmDate = DateTime.MinValue;
                DateTime tDate = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                DateTime serverDate = Constants.ServerDateTime;
                if (frmDate == DateTime.MinValue)
                    frmDate = serverDate.Date;
                if (tDate == DateTime.MinValue)
                    tDate = serverDate.Date;
                if (string.IsNullOrEmpty(selectOrgIdStr))
                {
                    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (tblConfigParamsTO != null)
                    {
                        selectOrgIdStr = Convert.ToString(tblConfigParamsTO.ConfigParamVal)+",0";
                    }
                }

                return BL.TblLoadingSlipBL.SelectAllNotifiedTblLoadingList(frmDate, tDate, callFlag, selectOrgIdStr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Sudhir[18-APR-2018] Added for Showig Graphical Data of Loading Status CNF Wise.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="orgId"></param>
        /// <param name="sysDate"></param>
        /// <param name="loadingType"></param>
        /// <returns></returns>
        [Route("GetLoadingStatusInfo")]
        [HttpGet]
        public List<LoadingInfo> GetLoadingStatusInfo(String userRoleTOList, Int32 orgId, DateTime sysDate, string fromDate, String toDate, Int32 loadingType = 1, Int32 dealerOrgId = 0)
        {

            DateTime frmDate = DateTime.MinValue;
            DateTime tDate = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            DateTime serverDate = Constants.ServerDateTime;
            if (frmDate == DateTime.MinValue)
                frmDate = serverDate.Date;
            if (tDate == DateTime.MinValue)
                tDate = serverDate.Date;

            if (sysDate == DateTime.MinValue)
                sysDate = Constants.ServerDateTime;

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            //TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(roleId);
            return BL.TblLoadingBL.SelectLoadingStatusGraph(tblUserRoleTOList, orgId, frmDate, tDate, loadingType, dealerOrgId);
        }


        /// <summary>
        /// Sanjay [2018-07-30] To Generate consolidated report for unloading with all data
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("GetConsolidatedUnloadingView")]
        [HttpGet]
        public List<UnloadingRptTO> GetConsolidatedUnloadingView(string fromDate, String toDate)
        {

            DateTime frmDate = DateTime.MinValue;
            DateTime tDate = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                frmDate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                tDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            return BL.TblUnLoadingBL.SelectAllUnLoadingListForReport(frmDate, tDate);
        }

        [Route("GetLoadingslipDetailsByBooking")]
        [HttpGet]
        public TblLoadingTO GetLoadingslipDetailsByBooking(String bookingIdsList, String scheduleIdsList)
        {
            return TblLoadingBL.SelectLoadingTOWithDetailsByBooking(bookingIdsList, scheduleIdsList);
        }

        [Route("GetLoadingslipDetailsByBookingVal")]
        [HttpPost]
        public TblLoadingTO GetLoadingslipDetailsByBooking([FromBody] JObject data)
        {
            TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
            String[] tempBookingIds = JsonConvert.DeserializeObject<string[]>(data["bookingIdsList"].ToString());
            string bookingIdsList = string.Join(",", tempBookingIds);

            String[] tempScheduleIds = JsonConvert.DeserializeObject<string[]>(data["scheduleIdsList"].ToString());
            string scheduleIdsList = string.Join(",", tempScheduleIds);

            return TblLoadingBL.SelectLoadingTOWithDetailsByBooking(bookingIdsList, scheduleIdsList, tblLoadingTO);

        }
        #endregion

        #region Post

        [Route("PostNewLoadingSlip")]
        [HttpPost]
        public ResultMessage PostNewLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                loggerObj.LogError("start PostNewLoadingSlip" + Constants.ServerDateTime);
                //TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingSlipTO"].ToString());
                //for (int i = 1; i < 150; i++)
                //{
                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingSlipTO"].ToString());
                //tblLoadingTO.VehicleNo = "SD 22 TT " + (4500 + i);
                //tblLoadingTO.LoadingSlipList.ForEach(w => w.VehicleNo = tblLoadingTO.VehicleNo);
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "LoadingSlipList Found NULL";
                    return resultMessage;
                }


                tblLoadingTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;
                tblLoadingTO.StatusDate = Constants.ServerDateTime;
                tblLoadingTO.CreatedOn = Constants.ServerDateTime;
                tblLoadingTO.StatusReason = "New - Considered For Loading";
                tblLoadingTO.ParentLoadingId = tblLoadingTO.IdLoading;
                TblEntityRangeTO loadingEntityRangeTO = new TblEntityRangeTO();
                if (tblLoadingTO.LoadingType != (int)Constants.LoadingTypeE.DELIVERY_INFO)
                {
                    loadingEntityRangeTO = BL.TblLoadingBL.SelectEntityRangeForLoadingCount(Constants.ENTITY_RANGE_LOADING_COUNT); //Connection and tran for selection and updation removed as due to processing time concurrent request takes existing values from enti
                }else
                {
                    loadingEntityRangeTO = BL.TblLoadingBL.SelectEntityRangeForLoadingCount(Constants.ENTITY_RANGE_DELIVERYINFO_COUNT); //Connection and tran for selection and updation removed as due to processing time concurrent request takes existing values from enti
                }
                if (loadingEntityRangeTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : loadingEntityRangeTO is null";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                //tblLoadingTO.ModbusRefId = GetNextAvailableModRefId(conn, tran);
                //Harshala added[03/11/2020]
                String Day = tblLoadingTO.CreatedOn.Day.ToString();
                String Month = tblLoadingTO.CreatedOn.Month.ToString();
                if (Day.Length == 1)
                    Day = Day.Insert(0, "0");

                if (Month.Length == 1)
                    Month = Month.Insert(0, "0");
                //

                String loadingSlipNo = tblLoadingTO.CreatedOn.Year + "" + Month + "" + Day + "/" + loadingEntityRangeTO.EntityPrevValue;

                tblLoadingTO.LoadingSlipNo = loadingSlipNo;
                if (Constants.getweightSourceConfigTO() == (int)Constants.WeighingDataSourceE.IoT)
                {
                    tblLoadingTO.ModbusRefId = BL.TblLoadingBL.GetNextAvailableModRefIdNew();
                    if (tblLoadingTO.ModbusRefId == 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : ModbusRef List gretter than 255 or Number not found Or Dublicate number found";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                }

                loadingEntityRangeTO.EntityPrevValue++;
                int result = BL.TblEntityRangeBL.UpdateTblEntityRange(loadingEntityRangeTO);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : While UpdateTblEntityRange";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                return BL.TblLoadingBL.SaveNewLoadingSlip(tblLoadingTO);
                // }
                // return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }

        }


        [Route("PostDeliverySlipConfirmations")]
        [HttpPost]
        public ResultMessage PostDeliverySlipConfirmations([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }


                tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.StatusDate = Constants.ServerDateTime;
                if (tblLoadingTO.CallFlag != 0)
                {
                    tblLoadingTO.CallFlagBy = Convert.ToInt32(loginUserId);
                }
                tblLoadingTO.UpdatedOn = tblLoadingTO.StatusDate;

                if (tblLoadingTO.IsRestorePrevStatus == 0)
                    return BL.TblLoadingBL.UpdateDeliverySlipConfirmations(tblLoadingTO);
                else
                    return BL.TblLoadingBL.RestorePreviousStatusForLoading(tblLoadingTO);


            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        /// <summary>
        /// GJ@20170829 : Delivery slip Loading completion confirmation for multiple Loading Slip
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeliverySlipListConfirmations")]
        [HttpPost]
        public ResultMessage PostDeliverySlipListConfirmations([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblLoadingTO> tblLoadingTOList = JsonConvert.DeserializeObject<List<TblLoadingTO>>(data["loadingTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTOList == null || tblLoadingTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("tblLoadingTOList Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                foreach (var tblLoadingTO in tblLoadingTOList)
                {
                    tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblLoadingTO.StatusDate = Constants.ServerDateTime;
                    tblLoadingTO.UpdatedOn = tblLoadingTO.StatusDate;

                    if (tblLoadingTO.IsRestorePrevStatus == 0)
                        BL.TblLoadingBL.UpdateDeliverySlipConfirmations(tblLoadingTO);
                    else
                        BL.TblLoadingBL.RestorePreviousStatusForLoading(tblLoadingTO);
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeliverySlipListConfirmations");
                return resultMessage;
            }
        }


        [Route("PostLoadingSuperwisorDtl")]
        [HttpPost]
        public ResultMessage PostLoadingSuperwisorDtl([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                TblLoadingTO existingTblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading);
                if (existingTblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "existingTblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                existingTblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                existingTblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                existingTblLoadingTO.SuperwisorId = tblLoadingTO.SuperwisorId;
                if (existingTblLoadingTO.CallFlag != 0)
                {
                    existingTblLoadingTO.CallFlagBy = Convert.ToInt32(loginUserId);

                }
                int result = BL.TblLoadingBL.UpdateTblLoading(existingTblLoadingTO);
                if (result == 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Updated Sucessfully";
                    resultMessage.Result = 1;
                    return resultMessage;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("PostLoadingConfiguration")]
        [HttpPost]
        public ResultMessage PostLoadingConfiguration([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                List<TblLoadingQuotaConfigTO> loadingQuotaConfigTOList = JsonConvert.DeserializeObject<List<TblLoadingQuotaConfigTO>>(data["loadingQuotaConfigList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingQuotaConfigTOList == null || loadingQuotaConfigTOList.Count == 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingQuotaConfigTOList Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = Constants.ServerDateTime;

                for (int i = 0; i < loadingQuotaConfigTOList.Count; i++)
                {
                    loadingQuotaConfigTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    loadingQuotaConfigTOList[i].CreatedOn = createdDate;
                }

                return BL.TblLoadingQuotaConfigBL.SaveNewLoadingQuotaConfiguration(loadingQuotaConfigTOList);
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostLoadingConfiguration";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }

        [Route("PostLoadingQuotaDeclaration")]
        [HttpPost]
        public ResultMessage PostLoadingQuotaDeclaration([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                List<TblLoadingQuotaDeclarationTO> loadingQuotaDeclarationTOList = JsonConvert.DeserializeObject<List<TblLoadingQuotaDeclarationTO>>(data["loadingQuotaDeclarationTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingQuotaDeclarationTOList == null || loadingQuotaDeclarationTOList.Count == 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingQuotaDeclarationTOList Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = Constants.ServerDateTime;
                Boolean isQuotaDeclared = BL.TblLoadingQuotaDeclarationBL.IsLoadingQuotaDeclaredForTheDate(createdDate.Date, loadingQuotaDeclarationTOList[0].ProdCatId, loadingQuotaDeclarationTOList[0].ProdSpecId);
                if (isQuotaDeclared)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Loading Quota Is Already Declared For The Day :" + createdDate.Date.ToString(Constants.DefaultDateFormat);
                    rMessage.Result = 0;
                    return rMessage;
                }


                for (int i = 0; i < loadingQuotaDeclarationTOList.Count; i++)
                {
                    loadingQuotaDeclarationTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    loadingQuotaDeclarationTOList[i].CreatedOn = createdDate;
                    loadingQuotaDeclarationTOList[i].IsActive = 1;
                }

                rMessage = BL.TblLoadingQuotaDeclarationBL.SaveLoadingQuotaDeclaration(loadingQuotaDeclarationTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostLoadingQuotaDeclaration";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }


        [Route("PostLoadingQuotaTransferNotes")]
        [HttpPost]
        public ResultMessage PostLoadingQuotaTransferNotes([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                List<TblLoadingQuotaTransferTO> loadingQuotaTransferTOList = JsonConvert.DeserializeObject<List<TblLoadingQuotaTransferTO>>(data["loadingQuotaTransferTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingQuotaTransferTOList == null || loadingQuotaTransferTOList.Count == 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingQuotaTransferTOList Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = Constants.ServerDateTime;

                for (int i = 0; i < loadingQuotaTransferTOList.Count; i++)
                {
                    loadingQuotaTransferTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    loadingQuotaTransferTOList[i].CreatedOn = createdDate;
                }

                rMessage = BL.TblLoadingQuotaTransferBL.SaveLoadingQuotaTransferNotes(loadingQuotaTransferTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostLoadingQuotaTransferNotes";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }

        [Route("PostMaterailTransferNotes")]
        [HttpPost]
        public ResultMessage PostMaterailTransferNotes([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                List<TblLoadingQuotaTransferTO> loadingQuotaTransferTOList = JsonConvert.DeserializeObject<List<TblLoadingQuotaTransferTO>>(data["loadingQuotaTransferTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingQuotaTransferTOList == null || loadingQuotaTransferTOList.Count == 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingQuotaTransferTOList Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = Constants.ServerDateTime;

                for (int i = 0; i < loadingQuotaTransferTOList.Count; i++)
                {
                    loadingQuotaTransferTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    loadingQuotaTransferTOList[i].CreatedOn = createdDate;
                }

                rMessage = BL.TblLoadingQuotaTransferBL.SaveMaterialTransferNotes(loadingQuotaTransferTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostLoadingQuotaTransferNotes";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }

        [Route("PostCancelNotConfirmLoadings")]
        [HttpGet]
        public ResultMessage PostCancelNotConfirmLoadings()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return BL.TblLoadingBL.CancelAllNotConfirmedLoadingSlips();
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        [Route("PostAllowedLoadingTime")]
        [HttpPost]
        public ResultMessage PostAllowedLoadingTime([FromBody] JObject data)
        {
            ResultMessage rMessage = new ResultMessage();

            try
            {

                TblLoadingAllowedTimeTO loadingAllowedTimeTO = JsonConvert.DeserializeObject<TblLoadingAllowedTimeTO>(data["loadingAllowedTimeTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "UserID Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                if (loadingAllowedTimeTO == null)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "loadingAllowedTimeTO Found NULL";
                    rMessage.Result = 0;
                    return rMessage;
                }

                DateTime createdDate = Constants.ServerDateTime;
                loadingAllowedTimeTO.CreatedBy = Convert.ToInt32(loginUserId);
                loadingAllowedTimeTO.CreatedOn = createdDate;

                loadingAllowedTimeTO.AllowedLoadingTime = Convert.ToDateTime(loadingAllowedTimeTO.ExtendedHrs);

                //int result= BL.TblLoadingAllowedTimeBL.InsertTblLoadingAllowedTime(loadingAllowedTimeTO);
                //if(result==1)
                //{
                //    rMessage.MessageType = ResultMessageE.Information;
                //    rMessage.Text = "Loading Time Saved Successfully";
                //    rMessage.Result = 1;
                //    return rMessage;
                //}

                //rMessage.MessageType = ResultMessageE.Error;
                //rMessage.Text = "Error while  InsertTblLoadingAllowedTime: PostAllowedLoadingTime";
                //rMessage.Result = 0;
                //return rMessage;
                return BL.TblLoadingAllowedTimeBL.SaveAllowedLoadingTime(loadingAllowedTimeTO);
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In API Call : PostAllowedLoadingTime";
                rMessage.Exception = ex;
                rMessage.Result = -1;
                return rMessage;
            }
        }

        /// <summary>
        /// GJ@20170824 : API to populate the Loading Slip status changes cycle automatically
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeliverySlipConfirmationsCycleAuto")]
        [HttpPost]
        public ResultMessage PostDeliverySlipConfirmationsCycleAuto([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_CYCLE_STATUS_IDS);
                string autoCancelCycleStatusIds = tblConfigParamsTO.ConfigParamVal;
                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM)
                {
                    TblLoadingTO existingTblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading);
                    if (existingTblLoadingTO == null)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "existingTblLoadingTO Found NULL";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                    existingTblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    existingTblLoadingTO.StatusDate = Constants.ServerDateTime;
                    existingTblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                    if (autoCancelCycleStatusIds != null && autoCancelCycleStatusIds != "")
                    {
                        string[] statusIds = autoCancelCycleStatusIds.Split(',');
                        foreach (var statusId in statusIds)
                        {
                            existingTblLoadingTO.StatusId = Convert.ToInt32(statusId);
                            existingTblLoadingTO.SuperwisorId = tblLoadingTO.SuperwisorId;
                            DimStatusTO dimStatusTO = BL.DimStatusBL.SelectDimStatusTO(existingTblLoadingTO.StatusId);
                            if (dimStatusTO != null)
                            {
                                existingTblLoadingTO.StatusReason = dimStatusTO.StatusDesc;
                            }
                            // tblLoadingTO.TranStatusE = Convert.ToInt32(statusId);
                            resultMessage = BL.TblLoadingBL.UpdateDeliverySlipConfirmations(existingTblLoadingTO);
                        }

                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Text = "Record Updated Sucessfully";
                        resultMessage.Result = 1;
                        return resultMessage;
                    }
                }
                //return BL.TblLoadingBL.UpdateDeliverySlipConfirmations(tblLoadingTO);              


            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            return resultMessage;
        }

        [Route("PostLoadingSlipUpdate")]
        [HttpPost]
        public ResultMessage PostLoadingSlipUpdate([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                TblLoadingTO existingTblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading);
                if (existingTblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "existingTblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                //Added By Kiran For avoid to change old value 06/09/19
                int weightSourceConfigId = Constants.getweightSourceConfigTO();
                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                {
                    tblLoadingTO.VehicleNo = existingTblLoadingTO.VehicleNo;
                    tblLoadingTO.StatusId = existingTblLoadingTO.StatusId;
                    tblLoadingTO.TransporterOrgId = existingTblLoadingTO.TransporterOrgId;
                }
                //existingTblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //existingTblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                //existingTblLoadingTO.CallFlag = tblLoadingTO.CallFlag;
                //existingTblLoadingTO.FlagUpdatedOn = Constants.ServerDateTime;
                tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                tblLoadingTO.CallFlagBy = Convert.ToInt32(loginUserId);
                int result = BL.TblLoadingBL.UpdateTblLoading(tblLoadingTO);
                if (result == 1)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblLoading");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostLoadingSlipUpdate");
                return resultMessage;
            }
        }


        [Route("PostChangeLoadSlipConfirmationStatus")]
        [HttpPost]
        public ResultMessage PostChangeLoadSlipConfirmationStatus([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loadingSlipId = data["loadingSlipId"].ToString();
                var loginUserId = data["loginUserId"].ToString();

                TblLoadingSlipTO loadingSlipTO = BL.TblLoadingSlipBL.SelectTblLoadingSlipTO(Convert.ToInt32(loadingSlipId));
                if (loadingSlipTO == null)
                {
                    resultMessage.DefaultBehaviour("loadingSlipTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                return BL.TblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(loadingSlipTO, Convert.ToInt32(loginUserId));
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostChangeLoadSlipConfirmationStatus");
                return resultMessage;
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        ///  Vaibhav [13-Sep-2017] API to insert Unloading Slip Details
        /// </summary>
        /// <param name="value"></param>
        [Route("PostNewUnLoadingSlip")]
        [HttpPost]
        public ResultMessage PostNewUnLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblUnLoadingTO tblUnLoadingTO = JsonConvert.DeserializeObject<TblUnLoadingTO>(data["UnLoadingTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblUnLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUnLoadingTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                tblUnLoadingTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblUnLoadingTO.CreatedOn = Constants.ServerDateTime;
                tblUnLoadingTO.StatusId = Convert.ToInt32(Constants.TranStatusE.UNLOADING_NEW);
                tblUnLoadingTO.StatusDate = StaticStuff.Constants.ServerDateTime;
                tblUnLoadingTO.Status = Constants.TranStatusE.UNLOADING_NEW.ToString();

                return BL.TblUnLoadingBL.SaveNewUnLoadingSlipDetails(tblUnLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewUnLoadingSlip");
                return resultMessage;
            }
        }

        /// <summary>
        /// GJ@20170917 : API to Update the UnLoading Slip Confirmations
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateUnloadingById")]
        [HttpPost]
        public ResultMessage PostUpdateUnloadingById([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            try
            {
                TblUnLoadingTO tblUnLoadingTO = JsonConvert.DeserializeObject<TblUnLoadingTO>(data["UnLoadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblUnLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUnLoadingTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                tblUnLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblUnLoadingTO.StatusDate = Constants.ServerDateTime;
                tblUnLoadingTO.UpdatedOn = tblUnLoadingTO.StatusDate;

                result = BL.TblUnLoadingBL.UpdateTblUnLoading(tblUnLoadingTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Server error : Record not updated");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateUnloadingById");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [20-Sep-2017] Added to get unloading standard desc list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewUnloadingDescription")]
        [HttpPost]
        public ResultMessage PostNewUnloadingDescription([FromBody] JObject data)
        {

            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblUnloadingStandDescTO> UnloadingStandDescTOList = JsonConvert.DeserializeObject<List<TblUnloadingStandDescTO>>(data["UnloadingStandDescTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                int result = 0;
                if (UnloadingStandDescTOList == null)
                {
                    resultMessage.DefaultBehaviour("UnloadingStandDescTOList Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                foreach (var UnloadingStandDescTO in UnloadingStandDescTOList)
                {
                    UnloadingStandDescTO.IsActive = 1;
                    result = BL.TblUnloadingStandDescBL.InsertTblUnloadingStandDesc(UnloadingStandDescTO);
                }
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewUnloadingDescription");
                return resultMessage;
            }

        }

        /// <summary>
        /// Vaibhav [20-Sep-2017] Added to Update unloading standard desc
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateUnloadingDescription")]
        [HttpPost]
        public ResultMessage PostUpdateUnloadingDescription([FromBody] JObject data)
        {

            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblUnloadingStandDescTO UnloadingStandDescTO = JsonConvert.DeserializeObject<TblUnloadingStandDescTO>(data["UnloadingStandDescTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (UnloadingStandDescTO == null)
                {
                    resultMessage.DefaultBehaviour("UnloadingStandDescTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }


                UnloadingStandDescTO.IsActive = 1;
                int result = BL.TblUnloadingStandDescBL.UpdateTblUnloadingStandDesc(UnloadingStandDescTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateUnloadingDescription");
                return resultMessage;
            }

        }

        /// <summary>
        /// Vaibhav [20-Sep-2017] Deactivate Unloading Description
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeactivateUnloadingDescription")]
        [HttpPost]
        public ResultMessage PostDeactivateUnloadingDescription([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblUnloadingStandDescTO UnloadingStandDescTO = JsonConvert.DeserializeObject<TblUnloadingStandDescTO>(data["UnloadingStandDescTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (UnloadingStandDescTO == null)
                {
                    resultMessage.DefaultBehaviour("UnloadingStandDescTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }


                UnloadingStandDescTO.IsActive = 0;

                int result = BL.TblUnloadingStandDescBL.UpdateTblUnloadingStandDesc(UnloadingStandDescTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be updated");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateUnloadingDescription");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [12-Oct-2017] added to deactivate unloading slip
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostDeactivateUnLoadingSlip")]
        [HttpPost]
        public ResultMessage PostDeactivateUnLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblUnLoadingTO tblUnLoadingTO = JsonConvert.DeserializeObject<TblUnLoadingTO>(data["UnLoadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (tblUnLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUnLoadingTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                return BL.TblUnLoadingBL.DeactivateUnLoadingSlip(tblUnLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateUnLoadingSlip");
                return resultMessage;
            }
        }

        /// <summary>
        /// GJ@20171107 : Remove the IsAllow condition on Weighment screen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// 
        [Route("PostRemoveIsAllowOneMoreLoading")]
        [HttpPost]
        public ResultMessage PostRemoveIsAllowOneMoreLoading([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }
                return BL.TblLoadingBL.removeIsAllowOneMoreLoading(tblLoadingTO, Convert.ToInt32(loginUserId));

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostLoadingSlipUpdate");
                return resultMessage;
            }
        }


        [Route("PostChangeGateIOTAgainstLoading")]
        [HttpPost]
        public ResultMessage PostChangeGateIOTAgainstLoading([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblLoadingTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.UpdatedOn = Constants.ServerDateTime;

                return BL.TblLoadingBL.PostChangeGateIOTAgainstLoading(tblLoadingTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostDeliverySlipConfirmations";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        #endregion

        #region Put

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Vaibhav [14-Sep-2017] API to update unloading details
        /// </summary>
        /// <param name="id"></param>
        [Route("PutUnLoadingSlip")]
        [HttpPut]
        public static ResultMessage PutUnLoadingSlip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblUnLoadingTO tblUnLoadingTO = JsonConvert.DeserializeObject<TblUnLoadingTO>(data[""].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblUnLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblUnLoadingTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                tblUnLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblUnLoadingTO.UpdatedOn = Constants.ServerDateTime;
                tblUnLoadingTO.StatusId = Convert.ToInt32(Constants.TranStatusE.LOADING_NEW);
                tblUnLoadingTO.StatusDate = StaticStuff.Constants.ServerDateTime;

                return BL.TblUnLoadingBL.UpdateUnLoadingSlipDetails(tblUnLoadingTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PutUnLoadingSlip");
                return resultMessage;
            }
        }

        #endregion

        #region Delete

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion


        //Sudhir[30-APR-2018]
        [Route("MigrateParityDetailsData")]
        [HttpPost]
        public ResultMessage MigrateParityDetailsData()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                return BL.TblParityDetailsBL.MigrateParityRelatedData();
                //resultMessage.DefaultBehaviour("Migrate Parity Details Function is Commmented");
                //return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "MigrateParityDetailsData()");
                return resultMessage;
            }
        }

        //[Route("TestGetStatusHistoryByLoadingId")]
        //[HttpGet]
        //public ResultMessage TestGetStatusHistoryByLoadingId(TblLoadingTO loading)
        //{
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();


        //        return BL.TblLoadingBL.GetStatusHistoryByLoadingIdFromIOT(loading);

        //    }


        [Route("PostInvoiceReportListForExcel")]
        [HttpPost]
        public ResultMessage PostInvoiceReportListForExcel([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblInvoiceRptTO> tblInvoiceList = JsonConvert.DeserializeObject<List<TblInvoiceRptTO>>(data["data"].ToString());
                var result = BL.FinalBookingData.CreateTempInvoiceExcel(tblInvoiceList, null, null);
                if (result == 1)
                {
                    resultMessage.DefaultSuccessBehaviour();
                }
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }
        }



        //[Deepali -12-03-2021]
        [Route("PostConvertBasicModeLodingToNormalModde")]
        [HttpPost]
        public ResultMessage PostConvertBasicModeLodingToNormalModde([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblLoadingSlipTO tblLoadingTO = JsonConvert.DeserializeObject<TblLoadingSlipTO>(data["tblLoadingTO"].ToString());

                if (tblLoadingTO != null)
                {
                    var result = BL.TblLoadingBL.PostConvertBasicModeLodingToNormalModde(tblLoadingTO);
                    if (result == 1)
                    {
                        resultMessage.DefaultSuccessBehaviour();
                    }
                }
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }
        }

        [Route("PostLinkingOfLoadingSlipToBooking")]
        [HttpPost]
        public ResultMessage PostLinkingOfLoadingSlipToBooking([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblBookingsTO tblBookingsTO = JsonConvert.DeserializeObject<TblBookingsTO>(data["tblBookingsTO"].ToString());

                if (tblBookingsTO != null)
                {
                    var result = BL.TblLoadingBL.PostLinkingOfLoadingSlipToBooking(tblBookingsTO);
                    if (result == 1)
                    {
                        resultMessage.DefaultSuccessBehaviour();
                    }                  

                }
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception in API Call";
                return resultMessage;
            }
        }
    }
}
