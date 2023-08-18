using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.DashboardModels;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PurchasebookingController : Controller
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblPurchaseBookingActionsBL _iTblPurchaseBookingActionsBL;
        private readonly ITblPurchaseEnquiryBL _iTblPurchaseEnquiryBL;
        private readonly ITblGradeWiseTargetQtyBL _iTblGradeWiseTargetQtyBL;
        private readonly ITblPurchaseBookingBeyondQuotaBL _iTblPurchaseBookingBeyondQuotaBL;
        private readonly ITblPurchaseEnquiryHistoryBL _iTblPurchaseEnquiryHistoryBL;
        public PurchasebookingController(
            ITblPurchaseEnquiryBL iTblPurchaseEnquiryBL,
            Icommondao icommondao,
            ITblGradeWiseTargetQtyBL iTblGradeWiseTargetQtyBL, ITblPurchaseBookingBeyondQuotaBL iTblPurchaseBookingBeyondQuotaBL, ITblPurchaseEnquiryHistoryBL iTblPurchaseEnquiryHistoryBL, ITblPurchaseBookingActionsBL iTblPurchaseBookingActionsBL)
        {
            _iTblPurchaseBookingActionsBL = iTblPurchaseBookingActionsBL;
            _iTblPurchaseEnquiryHistoryBL = iTblPurchaseEnquiryHistoryBL;
            _iTblPurchaseBookingBeyondQuotaBL = iTblPurchaseBookingBeyondQuotaBL;
            _iTblGradeWiseTargetQtyBL = iTblGradeWiseTargetQtyBL;
            _iTblPurchaseEnquiryBL = iTblPurchaseEnquiryBL;
            _iCommonDAO = icommondao;
        }

        #region Get

        [Route("GetPendingBookingsForAcceptanceByRole")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetPendingBookingsForAcceptanceByRole(String cnfId, string userRoleTO,Int32 isGetPendSaudaToClose, Int32 IsOrderOREnq = -1)
        {
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
            List<TblPurchaseEnquiryTO> tblBookingsTOList = _iTblPurchaseEnquiryBL.SelectAllBookingsListForAcceptance(cnfId, tblUserRoleTO, isGetPendSaudaToClose,IsOrderOREnq);
            return tblBookingsTOList;
        }

        [Route("GradeWiseTargetQtyDetailsByPurchaseManager")]
        [HttpGet]
        public List<TblGradeWiseTargetQtyTO> GradeWiseTargetQtyDetailsByPurchaseManager(Int32 purchaseManagerId,Int32 rateBandPurchaseId,Int32 prodClassId)
        {
            List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTOList = _iTblGradeWiseTargetQtyBL.SelectGradeWiseTargetQtyDtls(rateBandPurchaseId,purchaseManagerId);
            if(tblGradeWiseTargetQtyTOList!=null && tblGradeWiseTargetQtyTOList.Count>0)
            {
                if(prodClassId>0)
                {
                   tblGradeWiseTargetQtyTOList=tblGradeWiseTargetQtyTOList.Where(a=>a.ProdClassId==prodClassId).ToList();
                }
                
                return tblGradeWiseTargetQtyTOList;
            }
            else
                return null;
            
        }

        // [Route("GradeWiseTargetQtyDtls")]
        // [HttpGet]
        // public List<TblGradeWiseTargetQtyTO> GradeWiseTargetQtyDtls(Int32 purchaseManagerId, string prodItemIds)
        // {
        //     List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTOList = BL.TblGradeWiseTargetQtyBL.GradeWiseTargetQtyDtls(purchaseManagerId, prodItemIds);
        //     return tblGradeWiseTargetQtyTOList;
        // }


        [Route("GetBookingStatusHistory")]
        [HttpGet]
        public List<TblPurchaseBookingBeyondQuotaTO> GetBookingStatusHistory(int bookingId)
        {
            List<TblPurchaseBookingBeyondQuotaTO> list = _iTblPurchaseBookingBeyondQuotaBL.SelectAllStatusHistoryOfBooking(bookingId);
            if (list != null)
            {
                List<TblPurchaseBookingBeyondQuotaTO> finalList = new List<TblPurchaseBookingBeyondQuotaTO>();
                var statusIds = list.GroupBy(g => g.StatusId).ToList();
                for (int i = 0; i < statusIds.Count; i++)
                {
                    var latestObj = list.Where(l => l.StatusId == statusIds[i].Key).OrderBy(s => s.StatusDate).FirstOrDefault();
                    finalList.Add(latestObj);
                }

                finalList = finalList.OrderByDescending(s => s.StatusDate).ToList();
                return list;
            }

            return null;
        }
        /// <summary>
        /// Priyanka [10-01-2019]
        /// </summary>
        /// <param name="idPurchaseEnquiry"></param>
        /// <returns></returns>
        [Route("GetBookingStatusHistoryDetails")]
        [HttpGet]
        public List<TblPurchaseEnquiryHistoryTO> GetBookingStatusHistoryDetails(int idPurchaseEnquiry)
        {
            List<TblPurchaseEnquiryHistoryTO> list = _iTblPurchaseEnquiryHistoryBL.SelectAllStatusHistoryOfBookingDetails(idPurchaseEnquiry);
            if (list != null)
            {
                //List<TblPurchaseBookingBeyondQuotaTO> finalList = new List<TblPurchaseBookingBeyondQuotaTO>();
                //var statusIds = list.GroupBy(g => g.StatusId).ToList();
                //for (int i = 0; i < statusIds.Count; i++)
                //{
                //    var latestObj = list.Where(l => l.StatusId == statusIds[i].Key).OrderBy(s => s.StatusDate).FirstOrDefault();
                //    finalList.Add(latestObj);
                //}

                //finalList = finalList.OrderByDescending(s => s.StatusDate).ToList();
                return list;
            }

            return null;
        }


        [Route("GetBookingHistory")]
        [HttpGet]
        public List<TblPurchaseBookingBeyondQuotaTO> GetBookingHistory(int bookingId, int logingid)
        {
            List<TblPurchaseBookingBeyondQuotaTO> purchasehistorylist = _iTblPurchaseBookingBeyondQuotaBL.SelectAllPurchaseEnquiryHistory(bookingId);
            List<TblPurchaseBookingBeyondQuotaTO> purchaselist = _iTblPurchaseBookingBeyondQuotaBL.SelectAllStatusHistoryOfBooking(bookingId);
            List<TblPurchaseBookingBeyondQuotaTO> finalList = new List<TblPurchaseBookingBeyondQuotaTO>();
            finalList.AddRange(purchaselist);
            return finalList;
        }

        [Route("GetBookingDashboardInfo")]
        [HttpGet]
        public BookingInfo GetBookingDashboardInfo(String roleId, string orgId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate =  _iCommonDAO.ServerDateTime;
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(roleId);
            return _iTblPurchaseEnquiryBL.SelectBookingDashboardInfo(tblUserRoleTO, orgId, sysDate);
        }

        //Prajakta[2020-09-20] Added to get material wise sauda qty and avg price
        [Route("GetMaterialWiseEnqOrSaudaInfoForDashboard")]
        [HttpGet]
        public List<BookingInfo> GetMaterialWiseEnqOrSaudaInfoForDashboard(String roleId, string orgId, DateTime sysDate,Int32 isConvertToSauda)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommonDAO.ServerDateTime;
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(roleId);
            return _iTblPurchaseEnquiryBL.SelectMaterialWiseEnqOrSaudaInfoForDashboard(tblUserRoleTO, orgId, sysDate, isConvertToSauda);
        }

        [Route("GetSaudaDashboardInfo")]
        [HttpGet]
        public BookingInfo GetSaudaDashboardInfo(String roleId, string orgId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate =  _iCommonDAO.ServerDateTime;
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(roleId);
            return _iTblPurchaseEnquiryBL.SelectBookingSaudaDashboardInfo(tblUserRoleTO, orgId, sysDate);
        }

        [Route("GetTodayRateDashboardInfo")]
        [HttpGet]
        public BookingInfo GetTodayRateDashboardInfo(String roleId, Int32 orgId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate =  _iCommonDAO.ServerDateTime;
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(roleId);
            return _iTblPurchaseEnquiryBL.SelectTodayRtaeDashboardInfo(tblUserRoleTO, orgId, sysDate);
        }

        [Route("GetPurchaseBookingOpenCloseDashboardInfo")]
        [HttpGet]
        public TblPurchaseBookingActionsTO GetPurchaseBookingOpenCloseDashboardInfo()
        {
            return _iTblPurchaseBookingActionsBL.SelectLatestBookingActionTO();
        }

        #endregion

        #region Post

        [Route("PostBookingAcceptance")]
        [HttpPost]
        public ResultMessage PostBookingAcceptance([FromBody] JObject data)
        {

            try
            {
                TblPurchaseEnquiryTO tblBookingsTO = JsonConvert.DeserializeObject<TblPurchaseEnquiryTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                DateTime currentDate= _iCommonDAO.ServerDateTime;
                if (tblBookingsTO == null)
                {
                    return null;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    return null;
                }

                tblBookingsTO.StatusDate = currentDate;
                //tblBookingsTO.CreatedOn =  _iCommonDAO.ServerDateTime; // to update the Convert to Sauda date
                tblBookingsTO.UpdatedOn = currentDate;
                tblBookingsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = _iTblPurchaseEnquiryBL.UpdateBookingConfirmations(tblBookingsTO);
                // if (resMsg.MessageType != ResultMessageE.Information)
                // {
                //     return 0;
                // }
                // else
                // {
                //     return 1;
                // }
                return resMsg;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("PostBookingClosure")]
        [HttpPost]
        public ResultMessage PostBookingClosure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseBookingActionsTO bookingActionsTO = JsonConvert.DeserializeObject<TblPurchaseBookingActionsTO>(data["bookingActionsTO"].ToString());
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

                bookingActionsTO.StatusDate =  _iCommonDAO.ServerDateTime;
                bookingActionsTO.StatusBy = Convert.ToInt32(loginUserId);
                return _iTblPurchaseBookingActionsBL.SaveBookingActions(bookingActionsTO);

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

        #endregion

    }
}
