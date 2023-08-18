using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DashboardModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using SalesTrackerAPI.BL;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class BookingController : Controller
    {

        #region Declaration

        private readonly ILogger loggerObj;

        #endregion

        #region Constructor

        public BookingController(ILogger<BookingController> logger)
        {
            loggerObj = logger;
            Constants.LoggerObj = logger;
        }

        #endregion

        #region Get
        //Aditee[04022020]
        [Route("GetExistingBookingAddrListByDealerId")]
        [HttpGet]
        public List<TblBookingDelAddrTO> GetExistingBookingAddrListByDealerId(Int32 dealerOrgId, String addrSrcTypeString)
        {
            List<TblBookingDelAddrTO> list = TblBookingDelAddrBL.SelectExistingBookingAddrListByDealerId(dealerOrgId, addrSrcTypeString);
            return list;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetBookinOpenCloseInfo")]
        [HttpGet]
        public TblBookingActionsTO GetBookinOpenCloseInfo()
        {
            return BL.TblBookingActionsBL.SelectLatestBookingActionTO();
        }

        [Route("GetBookingDetails")]
        [HttpGet]
        public TblBookingsTO GetBookingDetails(int bookingId)
        {
            return BL.TblBookingsBL.SelectBookingsTOWithDetails(bookingId);
        }

        [Route("GetBookingStatusHistory")]
        [HttpGet]
        public List<TblBookingBeyondQuotaTO> GetBookingStatusHistory(int bookingId)
        {
            List<TblBookingBeyondQuotaTO> list = BL.TblBookingBeyondQuotaBL.SelectAllStatusHistoryOfBooking(bookingId);
            if (list != null)
            {
                List<TblBookingBeyondQuotaTO> finalList = new List<TblBookingBeyondQuotaTO>();
                var statusIds = list.GroupBy(g => g.StatusId).ToList();
                //Prajakta[2021-05-05] Added to show latest history if booking is edited.
                if(statusIds.Count == 1)
                {
                    for (int i = 0; i < statusIds.Count; i++)
                    {
                        var latestObj = list.Where(l => l.StatusId == statusIds[i].Key).OrderByDescending(s => s.StatusDate).FirstOrDefault();
                        finalList.Add(latestObj);
                    }
                }
                else
                {
                    for (int i = 0; i < statusIds.Count; i++)
                    {
                        var latestObj = list.Where(l => l.StatusId == statusIds[i].Key).OrderBy(s => s.StatusDate).FirstOrDefault();
                        finalList.Add(latestObj);
                    }
                }

                finalList = finalList.OrderByDescending(s => s.StatusDate).ToList();
                return finalList;
            }

            return null;
        }

        [Route("GetBookingAddressDetails")]
        [HttpGet]
        public List<TblBookingDelAddrTO> GetBookingAddressDetails(Int32 bookingId)
        {
            List<TblBookingDelAddrTO> list = null;
            list = BL.TblBookingDelAddrBL.SelectAllTblBookingDelAddrList(bookingId);
            //Sanjay [2017-07-06] Commented as functionality changed. Now address will be from dealer,Cnf or New 
            //Chages shifted to LoadSlip controller
            //if (list == null || list.Count==0)
            //{
            //    list = BL.TblBookingDelAddrBL.SelectDeliveryAddrListFromDealer(bookingId);
            //}
            return list;
        }


        [Route("GetDealerBookingHistory")]
        [HttpGet]
        public List<TblBookingsTO> GetDealerBookingHistory(Int32 dealerId , Int32 lastNRecords=4)
        {
            return BL.TblBookingsBL.SelectAllLatestBookingOfDealer(dealerId, lastNRecords);
        }


        [Route("GetDealerBookingDtlList")]
        [HttpGet]
        public List<TblBookingExtTO> GetDealerBookingDtlList(Int32 dealerId)
        {
            return BL.TblBookingExtBL.SelectAllBookingDetailsWrtDealer(dealerId);
        }

        [Route("GetAllBookingList")]
        [HttpGet]
        public List<TblBookingsTO> GetAllBookingList(Int32 cnfId, Int32 dealerId,string statusId,string fromDate,string toDate, String userRoleTOList, Int32 BookingId = 0,int bookingTypeId=0,int skipDateFilter=0)
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

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

            return BL.TblBookingsBL.SelectBookingList(cnfId, dealerId, statusId,frmDt,toDt, tblUserRoleTOList, BookingId,bookingTypeId, skipDateFilter);
        }


        /// <summary>
        /// Priyanka [21-03-2018] : Added to get the view bookings in Booking Summary Report
        /// </summary> 
        /// <param name="typeId"></param>
        /// <param name="masterId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("GetAllBookingSummaryList")]
        [HttpGet]
        public List<TblBookingSummaryTO> GetAllBookingSummaryList(Int32 typeId, Int32 masterId, string fromDate, string toDate, String userRoleTOList, Int32 cnfId)
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

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);

            return BL.TblBookingsBL.SelectBookingSummaryList(typeId, masterId, frmDt, toDt, tblUserRoleTOList, cnfId);
        }
       
        [Route("GetPendingBookingList")]
        [HttpGet]
        public List<TblBookingsTO> GetPendingBookingList(Int32 cnfId, Int32 dealerId, string userRoleTOList)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return BL.TblBookingsBL.SelectAllBookingList(cnfId, dealerId, tblUserRoleTOList);
        }


        [Route("GetPendingBookingsForApproval")]
        [HttpGet]
        public List<TblBookingsTO> GetPendingBookingsForApproval()
        {
            return BL.TblBookingsBL.SelectAllBookingsListForApproval();
        }


        [Route("GetPendingBookingsForAcceptance")]
        [HttpGet]
        /// <summary>
        /// Sanjay [2017-02-27] Will return list of all bookings of given cnf which are beyond quota and Rate Band
        /// and Approved By Directors. This will be for Acceptance By Cnf
        /// </summary>
        /// <param name="cnfId"></param>
        public List<TblBookingsTO> GetPendingBookingsForAcceptance(Int32 cnfId)
        {
            return BL.TblBookingsBL.SelectAllBookingsListForAcceptance(cnfId,null);
        }

        [Route("GetPendingBookingsForAcceptanceByRole")]
        [HttpGet]
        /// <summary>
        /// Sanjay [2017-02-27] Will return list of all bookings of given cnf which are beyond quota and Rate Band
        /// and Approved By Directors. This will be for Acceptance By Cnf
        /// </summary>
        /// <param name="cnfId"></param>
        public List<TblBookingsTO> GetPendingBookingsForAcceptance(Int32 cnfId, string userRoleTOList)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return BL.TblBookingsBL.SelectAllBookingsListForAcceptance(cnfId, tblUserRoleTOList);
        }

        [Route("GetEmptySizeAndProductListForBooking")]
        [HttpGet]
        public List<TblBookingExtTO> GetEmptySizeAndProductListForBooking(Int32 prodCatId,Int32 prodSpecId)
        {
            List<TblBookingExtTO> list = BL.TblBookingExtBL.SelectEmptyBookingExtList(prodCatId, prodSpecId);
            return list;
        }


        [Route("GetBookingDashboardInfo")]
        [HttpGet]
        public BookingInfo GetBookingDashboardInfo(String userRoleList, Int32 orgId, Int32 dealerId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = Constants.ServerDateTime;

            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleList);

            return BL.TblBookingsBL.SelectBookingDashboardInfo(tblUserRoleTOList, orgId, dealerId, sysDate);
        }

        [Route("GetMinAndMaxValueConfigForBookingRate")]
        [HttpGet]
        public String GetMinAndMaxValueConfigForBookingRate()
        {
            string configValue = string.Empty;
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_BOOKING_RATE_MIN_AND_MAX_BAND);
            if (tblConfigParamsTO != null)
                configValue = Convert.ToString(tblConfigParamsTO.ConfigParamVal);
            return configValue;
        }

        [Route("GetAllPendingBookingsForReport")]
        [HttpGet]
        public List<PendingBookingRptTO> GetAllPendingBookingsForReport(Int32 cnfOrgId, Int32 dealerOrgId, string userRoleTOList, Int32 StateId, Int32 DistId)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return BL.TblBookingsBL.SelectAllPendingBookingsForReport(cnfOrgId, dealerOrgId, tblUserRoleTOList, StateId, DistId);
        }

        [Route("CalculateBookingsOpeningBalance")]
        [HttpGet]
        public ResultMessage CalculateBookingsOpeningBalance()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return BL.TblBookingOpngBalBL.CalculateBookingOpeningBalance();
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

        /// <summary>
        /// Vijaymala[2017-09-11]Added to get booking list to plot graph
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <param name="userRoleTO"></param>
        /// <returns></returns>
        [Route("GetBookingsListForGraph")]
        [HttpGet]
        public List<BookingGraphRptTO> GetBookingsListForGraph(Int32 OrganizationId, string userRoleTOList, Int32 dealerId)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return BL.TblBookingsBL.SelectBookingListForGraph(OrganizationId, tblUserRoleTOList, dealerId);
        }

        [Route("GetBookingScheduleListByBookingId")]
        [HttpGet]
        public List<TblBookingScheduleTO> GetBookingScheduleListByBookingId(Int32 bookingId)
        {
            List<TblBookingScheduleTO> list = TblBookingScheduleBL.SelectBookingScheduleByBookingId(bookingId);
            return list;
        }

        [Route("GetBulkBookingHistoryBookingId")]
        [HttpGet]
        public List<TblBookingsTO> GetBulkBookingHistoryBookingId(Int32 bookingId)
        {
            List<TblBookingsTO> list = TblBookingsBL.GetBulkBookingHistoryBookingId(bookingId);
            return list;
        }
        #endregion

        #region Post


        /// <summary>
        /// Sanjay [2017-03-06] Use this call to Update Bookings for Various Statuses
        /// i.e. Booking Confirmation beyond quota and Rate By Directors,Booking Acceptance By C&F
        /// Booking Delete,Booking Reject et.c
        /// When Booking is Deleted then additional Attribute IsDeleted should be marked 1 to check for further calculations
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST api/values
        [Route("PostBookingAcceptance")]
        [HttpPost]
        public Int32 PostBookingAcceptance([FromBody] JObject data)
        {
            try
            {
                TblBookingsTO tblBookingsTO = JsonConvert.DeserializeObject<TblBookingsTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblBookingsTO == null)
                {
                    return 0;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    return 0;
                }

                tblBookingsTO.StatusDate = Constants.ServerDateTime;
                tblBookingsTO.UpdatedOn = Constants.ServerDateTime;
                tblBookingsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = BL.TblBookingsBL.UpdateBookingConfirmations(tblBookingsTO);
                if (resMsg.MessageType != ResultMessageE.Information)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        // POST api/values
        [Route("PostNewBooking")]
        [HttpPost]
        public ResultMessage PostNewBooking([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                //loggerObj.LogInformation(1, "In PostNewBooking", data);
                TblBookingsTO tblBookingsTO = JsonConvert.DeserializeObject<TblBookingsTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblBookingsTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "bookingTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                tblBookingsTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblBookingsTO.CreatedOn = StaticStuff.Constants.ServerDateTime;
                tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;
                tblBookingsTO.StatusDate = tblBookingsTO.CreatedOn;
                tblBookingsTO.BookingDatetime = tblBookingsTO.CreatedOn;
                //#region entity range 
                //DimFinYearTO curFinYearTO = DimensionBL.GetCurrentFinancialYear(tblBookingsTO.CreatedOn);
                //if (curFinYearTO == null)
                //{
                //    resultMessage.Text = "Current Fin Year Object Not Found";
                //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    return resultMessage;
                //}
                //TblEntityRangeTO entityRangeTO = BL.TblEntityRangeBL.SelectTblEntityRangeTOByEntityName(Constants.REGULAR_BOOKING, curFinYearTO.IdFinYear);
                //if (entityRangeTO == null)
                //{
                //    resultMessage.Text = "entity range not found in Function SaveNewBooking";
                //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    return resultMessage;
                //}
                //entityRangeTO.EntityPrevValue = entityRangeTO.EntityPrevValue + 1;
                //var result = BL.TblEntityRangeBL.UpdateTblEntityRange(entityRangeTO);
                //if (result != 1)
                //{
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    resultMessage.Text = "Error : While UpdateTblEntityRange";
                //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                //    return resultMessage;
                //}
                //tblBookingsTO.BookingDisplayNo = entityRangeTO.EntityPrevValue.ToString();
                //#endregion
                return BL.TblBookingsBL.SaveNewBooking(tblBookingsTO,null,null);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Exception Error in API Call";
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
        }

        // POST api/values
        [Route("PostBookingUpdate")]
        [HttpPost]
        public ResultMessage PostBookingUpdate([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblBookingsTO tblBookingsTO = JsonConvert.DeserializeObject<TblBookingsTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblBookingsTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblBookingsTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                //TblBookingsTO existingBookingTO = BL.TblBookingsBL.SelectTblBookingsTO(tblBookingsTO.IdBooking);
                //tblBookingsTO.TranStatusE = existingBookingTO.TranStatusE;
                //tblBookingsTO.StatusRemark = existingBookingTO.StatusRemark;

                tblBookingsTO.UpdatedOn = Constants.ServerDateTime;
                tblBookingsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                return BL.TblBookingsBL.UpdateBooking(tblBookingsTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method PostBookingUpdate";
                return resultMessage;
            }
        }


        // POST api/values
        [Route("PostBookingClosure")]
        [HttpPost]
        public ResultMessage PostBookingClosure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblBookingActionsTO bookingActionsTO = JsonConvert.DeserializeObject<TblBookingActionsTO>(data["bookingActionsTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (bookingActionsTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "bookingActionsTO found null";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId found null";
                    return resultMessage;
                }

                bookingActionsTO.StatusDate = Constants.ServerDateTime;
                bookingActionsTO.StatusBy = Convert.ToInt32(loginUserId);
                return BL.TblBookingActionsBL.SaveBookingActions(bookingActionsTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method PostBookingClosure";
                return resultMessage;
            }
        }


        [Route("PostDeleteBookingById")]
        [HttpPost]
        public ResultMessage PostDeleteBookingById([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                var bookingId = data["bookingId"].ToString();
                var statusRemark = data["statusRemark"].ToString();
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(bookingId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "bookingId not found";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId not found";
                    return resultMessage;
                }

                TblBookingsTO tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(Convert.ToInt32(bookingId));
                tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_DELETE;
                tblBookingsTO.IsDeleted = 1;
                tblBookingsTO.StatusRemark = statusRemark;
                tblBookingsTO.StatusDate = Constants.ServerDateTime;
                tblBookingsTO.UpdatedOn = Constants.ServerDateTime;
                tblBookingsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                return BL.TblBookingsBL.UpdateBookingConfirmations(tblBookingsTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception In API Call : PostDeleteBooking";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        #endregion

        #region Put
        
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion

        #region Delete

        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

        
    }
}
