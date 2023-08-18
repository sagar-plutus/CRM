using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.DashboardModels;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/PurchaseEnquiry")]
    public class PurchaseEnquiryController : Controller
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblPurchaseParityDetailsBL _iTblPurchaseParityDetailsBL;
        private readonly ITblPurchaseEnquiryDetailsBL _iTblPurchaseEnquiryDetailsBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblPurchaseEnquiryBL _iTblPurchaseEnquiryBL;
        private readonly ITblQualityPhaseBL _iTblQualityPhaseBL;
        private readonly ITblPurchaseBookingActionsBL _iTblPurchaseBookingActionsBL;
        private readonly ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;
        public PurchaseEnquiryController(ITblPurchaseEnquiryBL iTblPurchaseEnquiryBL, ITblPurchaseScheduleSummaryBL iTblPurchaseScheduleSummaryBL
                , ITblQualityPhaseBL iTblQualityPhaseBL, ITblConfigParamsBL iTblConfigParamsBL, ITblPurchaseBookingActionsBL iTblPurchaseBookingActionsBL
                , ITblPurchaseEnquiryDetailsBL iTblPurchaseEnquiryDetailsBL, Icommondao icommondao, ITblPurchaseParityDetailsBL iTblPurchaseParityDetailsBL)
        {
            _iCommonDAO = icommondao;
            _iTblPurchaseParityDetailsBL = iTblPurchaseParityDetailsBL;
            _iTblPurchaseEnquiryDetailsBL = iTblPurchaseEnquiryDetailsBL;
            _iTblPurchaseBookingActionsBL = iTblPurchaseBookingActionsBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _iTblQualityPhaseBL = iTblQualityPhaseBL;
            _iTblPurchaseEnquiryBL = iTblPurchaseEnquiryBL;
        }

        #region Get
        // GET: api/valuesa
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value10", "value2" };
        }

        [Route("GetEnquiryDetails")]
        [HttpGet]
        public TblPurchaseEnquiryTO GetEnquiryDetails(int purchaseEnquiryId)
        {
            return _iTblPurchaseEnquiryBL.SelectTblPurchaseEnquiry(purchaseEnquiryId);
        }

        [Route("GetEnquiryItemDtlsFromBookingIds")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetEnquiryItemDtlsFromBookingIds(string saudaIds)
        {
            return _iTblPurchaseEnquiryBL.GetEnquiryItemDtlsFromBookingIds(saudaIds);
        }

        // [Route("GradeWiseTargetQtyDetails")]
        // [HttpGet]
        // public List<TblGradeWiseTargetQtyTO> GradeWiseTargetQtyDetails(int rateBandPurchaseDeclareId, int goodRatePurchaseId)
        // {
        //     return BL.TblGradeWiseTargetQtyBL.GradeWiseTargetQtyDetails(rateBandPurchaseDeclareId, goodRatePurchaseId);
        // }

        [Route("GetAllEnquiryList")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetAllEnquiryList(Int32 SpotedId, String userId, Int32 organizationId, Int32 statusId, Int32 isConvertToSauda, Int32 isPending,Int32 materialTypeId,string cOrNcId, string fromDate, string toDate, Int32 isSkipDateFilter = 0) //, String userRoleTO)
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

            // TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);

            return _iTblPurchaseEnquiryBL.GetAllEnquiryList(SpotedId, userId, organizationId, statusId, frmDt, toDt, isConvertToSauda, isPending,materialTypeId, cOrNcId, isSkipDateFilter); //, tblUserRoleTO);
        }

        [Route("GetMaterialTypeWiseTotalPendingQty")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetMaterialTypeWiseTotalPendingQty(Int32 SpotedId, String userId, Int32 organizationId, Int32 statusId, Int32 isConvertToSauda, Int32 isPending, Int32 materialTypeId, string cOrNcId, string fromDate, string toDate, Int32 isSkipDateFilter = 0) //, String userRoleTO)
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

            return _iTblPurchaseEnquiryBL.GetMaterialTypeWiseTotalPendingQty(SpotedId, userId, organizationId, statusId, isConvertToSauda, isPending, materialTypeId, cOrNcId, frmDt, toDt, isSkipDateFilter);
        }

        [Route("GetSupplierWithMaterialHistory")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetSupplierWithMaterialHistory(Int32 supplierId, Int32 lastNRecords = 4)
        {
            return _iTblPurchaseEnquiryBL.GetSupplierWithMaterialHistList(supplierId, lastNRecords);
        }

        [Route("GetCompetitorListWithHistory")]
        [HttpGet]
        public List<TblOrganizationTO> GetCompetitorListWithHistory()
        {
            Constants.OrgTypeE orgTypeE = Constants.OrgTypeE.PURCHASE_COMPETITOR;
            List<TblOrganizationTO> list = _iTblPurchaseEnquiryBL.SelectAllTblOrganizationList(orgTypeE);

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_PURCHASE_COMPETITOR_TO_SHOW_IN_HISTORY);
            if (tblConfigParamsTO == null)
                return list;
            else
            {
                String csParamVal = tblConfigParamsTO.ConfigParamVal;
                if (csParamVal == "0")
                    return list;
                else
                {
                    List<TblOrganizationTO> finalList = new List<TblOrganizationTO>();
                    string[] idsToShow = csParamVal.Split(',');

                    for (int i = 0; i < idsToShow.Length; i++)
                    {
                        var orgTO = list.Where(a => a.IdOrganization == Convert.ToInt32(idsToShow[i])).LastOrDefault();
                        if (orgTO != null)
                            finalList.Add(orgTO);
                    }

                    finalList = finalList.OrderByDescending(o => o.CompetitorUpdatesTO.UpdateDatetime).ToList();
                    return finalList;
                }
            }
        }

        [Route("GetBookinOpenCloseInfo")]
        [HttpGet]
        public TblPurchaseBookingActionsTO GetBookinOpenCloseInfo()
        {
            return _iTblPurchaseBookingActionsBL.SelectLatestBookingActionTO();
        }
        [Route("GetSupplierWiseSaudaDetails")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId)
        {
            return _iTblPurchaseEnquiryBL.GetSupplierWiseSaudaDetails(supplierId, statusId);
        }


        [Route("GetSaudaDetailsByEnquiryId")]
        [HttpGet]
        public TblPurchaseEnquiryTO GetSaudaDetailsByEnquiryId(Int32 purchaseEnquiryId)
        {
            TblPurchaseEnquiryTO tblPurchaseEnquiryTO = _iTblPurchaseEnquiryBL.SelectTblPurchaseEnquiryTO(purchaseEnquiryId);
            if (tblPurchaseEnquiryTO != null)
            {
                List<TblPurchaseEnquiryDetailsTO> tblPurchaseEnquiryDetailsTOList = _iTblPurchaseEnquiryDetailsBL.SelectTblEnquiryDetailsList(purchaseEnquiryId);
                if (tblPurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryDetailsTOList.Count > 0)
                {
                    tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList = tblPurchaseEnquiryDetailsTOList;
                    //Prajakta[2019-03-18] Added to get booking item parity details
                    if (tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Count > 0)
                    {
                        // for (int i = 0; i < tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Count; i++)
                        // {

                        //     List<TblPurchaseParityDetailsTO> parityList = _iTblPurchaseParityDetailsBL.GetBookingItemsParityDtls(tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[i].ProdItemId.ToString(), tblPurchaseEnquiryTO.SaudaCreatedOn,Convert.ToInt32(tblPurchaseEnquiryTO.StateId));
                        //     if (parityList != null && parityList.Count > 0)
                        //     {
                        //         TblPurchaseParityDetailsTO parityDetailsTO = parityList[0];
                        //         tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[i].ParityAmt = parityDetailsTO.ParityAmt;
                        //         tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[i].NonConfParityAmt = parityDetailsTO.NonConfParityAmt;
                        //     }
                        // }


                        string prodItemIds = string.Join(", ", tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Select(i => i.ProdItemId));

                        if (!string.IsNullOrEmpty(prodItemIds))
                        {
                            List<TblPurchaseParityDetailsTO> parityList = _iTblPurchaseParityDetailsBL.GetBookingItemsParityDtls(prodItemIds, tblPurchaseEnquiryTO.SaudaCreatedOn, tblPurchaseEnquiryTO.StateId);
                            if (parityList != null && parityList.Count > 0)
                            {
                                for (int k = 0; k < tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList.Count; k++)
                                {
                                    List<TblPurchaseParityDetailsTO> resList = parityList.Where(s => s.ProdItemId == tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[k].ProdItemId).ToList();
                                    if (resList != null && resList.Count > 0)
                                    {
                                        TblPurchaseParityDetailsTO parityDetailsTO = resList[0];
                                        tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[k].ParityAmt = parityDetailsTO.ParityAmt;
                                        tblPurchaseEnquiryTO.PurchaseEnquiryDetailsTOList[k].NonConfParityAmt = parityDetailsTO.NonConfParityAmt;
                                    }

                                }
                            }
                        }
                    }
                }
            }

            if (tblPurchaseEnquiryTO != null)
                return tblPurchaseEnquiryTO;
            else
                return null;

        }

        [Route("GetSaudaDetailsBySpotEntryId")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetSaudaDetailsBySpotEntryId(Int32 spotEntryVehicleId)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = _iTblPurchaseEnquiryBL.SelectTblPurchaseEnquirySpotEntryTO(spotEntryVehicleId);
            if (tblPurchaseEnquiryTOList != null && tblPurchaseEnquiryTOList.Count > 0)
            {
                return tblPurchaseEnquiryTOList;
            }
            else
                return null;
        }

        [Route("GetPendingBookingsForAcceptanceByRole")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetPendingBookingsForAcceptanceByRole(String cnfId, string userRoleTO,Int32 isGetPendSaudaToClose, Int32 IsOrderOREnq = -1)
        {
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = _iTblPurchaseEnquiryBL.SelectAllBookingsListForAcceptance(cnfId, tblUserRoleTO,isGetPendSaudaToClose, IsOrderOREnq);
            return tblPurchaseEnquiryTOList;
        }


        /// <summary>
        /// Filter Issue based on Filter Type [⋯❰ Author:Deepali Sagale]
        /// </summary>
        /// <returns>Issue List Records</returns>
        [HttpGet("GetAllDocumentIdFromSpotEntryId")]
        [ProducesResponseType(typeof(List<TblRecycleDocumentTO>), 200)]
        public IActionResult GetAllDocumentIdFromSpotEntryId(Int32 TransId, Int32 TransTypeId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblRecycleDocumentTO> data = _iTblPurchaseEnquiryBL.SelectAllDocumentIdFromSpotEntryId(TransId, TransTypeId);
                if (data.Count > 0 && data == null)
                {
                    resultMessage.DefaultBehaviour("Sorry !!! No records found");
                    return Ok(resultMessage);
                }
                resultMessage.DefaultSuccessBehaviour();
                return Ok(data);
            }
            catch (Exception exc)
            {
                resultMessage.DefaultBehaviour(exc.Message);
                return Ok(resultMessage);
            }

        }

        [Route("GetShipmentDetailsByPurchaseEnquiryId")]
        [HttpGet]
        public List<TblpurchaseEnqShipmemtDtlsTO> GetShipmentDetailsByPurchaseEnquiryId(Int32 purchaseEnquiryId)
        {
            List<TblpurchaseEnqShipmemtDtlsTO> tblpurchaseEnqShipmemtDtlsTOList = _iTblPurchaseEnquiryBL.GetShipmentDetailsByPurchaseEnquiryId(purchaseEnquiryId);
            if (tblpurchaseEnqShipmemtDtlsTOList != null && tblpurchaseEnqShipmemtDtlsTOList.Count > 0)
            {
                return tblpurchaseEnqShipmemtDtlsTOList;
            }
            else
                return null;
        }
        [Route("PrintShipmentReport")]
        [HttpGet]
        public ResultMessage PrintShipmentReport(Int32 purchaseEnquiryId)
        {
           return _iTblPurchaseEnquiryBL.PrintShipmentReport(purchaseEnquiryId);
        }


        #endregion

        #region POST


        // POST api/values
        [Route("PostNewPurchaseEnquiry")]
        [HttpPost]
        public ResultMessage PostNewPurchaseEnquiry([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                //loggerObj.LogInformation(1, "In PostNewBooking", data);
                TblPurchaseEnquiryTO tblPurchaseEnquiryTO = JsonConvert.DeserializeObject<TblPurchaseEnquiryTO>(data["bookingTO"].ToString());
                TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
                List<TblPurchaseEnquiryDetailsTO> tblPurchaseEnquiryDetailsTO = new List<TblPurchaseEnquiryDetailsTO>();
                // var spotEntryVehicleId = Convert.ToString(data["spotEntryVehicleId"]);
                DateTime currentDate = _iCommonDAO.ServerDateTime;

                var loginUserId = data["loginUserId"].ToString();
                if (tblPurchaseEnquiryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "purchaseEnquiryTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }
                if (tblPurchaseVehicleSpotEntryTO != null)
                {
                    if (Convert.ToInt32(tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry) > 0)
                    {
                        tblPurchaseEnquiryTO.VehicleSpotEntryId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry);
                    }
                }

                if (tblPurchaseEnquiryTO.IsConvertToSauda == 1)
                    tblPurchaseEnquiryTO.SaudaCreatedOn = currentDate;

                tblPurchaseEnquiryTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblPurchaseEnquiryTO.CreatedOn = currentDate;
                tblPurchaseEnquiryTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //tblPurchaseEnquiryTO.UserId = Convert.ToInt32(loginUserId);
                tblPurchaseEnquiryTO.UpdatedOn = currentDate;
                // tblPurchaseEnquiryTO.Comments = "";
                resultMessage = _iTblPurchaseEnquiryBL.SaveNewPurchaseEnquiry(tblPurchaseEnquiryTO, tblPurchaseVehicleSpotEntryTO);

                if (resultMessage.Result > 0 && tblPurchaseEnquiryTO.BookingScheduleTOList != null)
                {
                    foreach (var item in tblPurchaseEnquiryTO.BookingScheduleTOList)
                    {
                        //Save Quality Flags
                        if (item.QualityPhaseTOList != null && item.QualityPhaseTOList.Count > 0)
                        {
                            Int32 txnId = 0;

                            txnId = item.RootScheduleId > 0 ? item.RootScheduleId : item.IdPurchaseScheduleSummary;
                            foreach (var item1 in item.QualityPhaseTOList)
                            {
                                item1.PurchaseScheduleSummaryId = txnId;
                                item1.IsActive = 1;
                                item1.CreatedOn = currentDate;
                                item1.CreatedBy = Convert.ToInt32(loginUserId);

                            }
                            resultMessage = _iTblQualityPhaseBL.SavePhaseSampleListsagainstPurrchaseScheduleSummaryID(item.QualityPhaseTOList, Convert.ToInt32(loginUserId));
                        }
                    }
                }

                return resultMessage;
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

        // [Route("SaveGradeWiseTargetDetails")]
        // [HttpPost]
        // public ResultMessage SaveGradeWiseTargetDetails([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {
        //         List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTOList = JsonConvert.DeserializeObject<List<TblGradeWiseTargetQtyTO>>(data["GradeWiseTargetQtyTOList"].ToString());
        //         int result = BL.TblGradeWiseTargetQtyBL.SaveGradeWiseTargetDetails(tblGradeWiseTargetQtyTOList);
        //         if (result == 1)
        //         {
        //             resultMessage.DefaultBehaviour();
        //             resultMessage.Text = "Record Saved Successfully";
        //             // resultMessage.Exception = ex;
        //             resultMessage.Result = 1;
        //             return resultMessage;
        //         }
        //         else
        //         {
        //             resultMessage.DefaultBehaviour();
        //             resultMessage.Text = "Failed To Save Record";
        //             // resultMessage.Exception = ex;
        //             resultMessage.Result = 1;
        //             return resultMessage;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultBehaviour();
        //         resultMessage.Text = "Exception Error in API Call";
        //         // resultMessage.Exception = ex;
        //         resultMessage.Result = -1;
        //         return resultMessage;
        //     }
        // }

        [Route("PostBookingUpdateForPurchase")]
        [HttpPost]
        public ResultMessage PostBookingUpdateForPurchase([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseEnquiryTO tblPurchaseEnquiryTO = JsonConvert.DeserializeObject<TblPurchaseEnquiryTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                DateTime currentDate = _iCommonDAO.ServerDateTime;
                if (tblPurchaseEnquiryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblPurchaseEnquiryTO Found NULL";
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


                tblPurchaseEnquiryTO.UpdatedOn = currentDate;
                tblPurchaseEnquiryTO.UpdatedBy = Convert.ToInt32(loginUserId);
                return _iTblPurchaseEnquiryBL.UpdateBookingForPurchase(tblPurchaseEnquiryTO);
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

        [Route("ApproveRejectCloseSauda")]
        [HttpPost]
        public ResultMessage ApproveRejectCloseSauda([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblPurchaseEnquiryTO enquiryTO = JsonConvert.DeserializeObject<TblPurchaseEnquiryTO>(data["bookingTO"].ToString());
            Int32 isApproveOrReject = Convert.ToInt32(data["isApproveOrReject"].ToString());
            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

            try
            {
                return _iTblPurchaseEnquiryBL.ApproveRejectCloseSauda(enquiryTO,isApproveOrReject,loginUserId);
            }
            catch (System.Exception ex)
            {
                resultMessage.DefaultBehaviour();
                return resultMessage;
            }

        }

        [Route("PostConvertToSaudaOrder")]
        [HttpPost]
        public ResultMessage PostConvertToSaudaOrder([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseEnquiryTO tblPurchaseEnquiryTO = new TblPurchaseEnquiryTO();
                var idPurchaseEnquiry = data["idPurchaseEnquiry"].ToString();
                var statusId = data["statusId"].ToString();
                var isConvertToSauda = data["isConvertToSauda"].ToString();
                var loginUserId = data["loginUserId"].ToString();
                var spotEntryVehicleId = Convert.ToString(data["spotEntryVehicleId"]);

                if (tblPurchaseEnquiryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblPurchaseEnquiryTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(spotEntryVehicleId) > 0)
                {
                    tblPurchaseEnquiryTO.VehicleSpotEntryId = Convert.ToInt32(spotEntryVehicleId);
                }
                tblPurchaseEnquiryTO.IdPurchaseEnquiry = Convert.ToInt32(idPurchaseEnquiry);
                tblPurchaseEnquiryTO.StatusId = Convert.ToInt32(statusId);
                tblPurchaseEnquiryTO.IsConvertToSauda = Convert.ToInt32(isConvertToSauda);

                tblPurchaseEnquiryTO.CreatedOn = _iCommonDAO.ServerDateTime; // changed so that Sauda is created on this date
                tblPurchaseEnquiryTO.UpdatedOn = _iCommonDAO.ServerDateTime;
                tblPurchaseEnquiryTO.UpdatedBy = Convert.ToInt32(loginUserId);
                return _iTblPurchaseEnquiryBL.UpdateBookingForSauda(tblPurchaseEnquiryTO);

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

        [Route("PostVehicleDetailsBookingForPurchase")]
        [HttpPost]
        public ResultMessage PostVehicleDetailsBookingForPurchase([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseEnquiryTO tblPurchaseEnquiryTO = JsonConvert.DeserializeObject<TblPurchaseEnquiryTO>(data["bookingTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblPurchaseEnquiryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblPurchaseEnquiryTO Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }
                tblPurchaseEnquiryTO.CreatedOn = _iCommonDAO.ServerDateTime;
                tblPurchaseEnquiryTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblPurchaseEnquiryTO.UpdatedOn = _iCommonDAO.ServerDateTime;
                tblPurchaseEnquiryTO.UpdatedBy = Convert.ToInt32(loginUserId);
                return _iTblPurchaseEnquiryBL.SaveVehicleDetailsBookingForPurchase(tblPurchaseEnquiryTO);

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



        [Route("SaveVehicleScheduleDetails")]
        [HttpPost]
        public ResultMessage SaveVehicleScheduleDetails([FromBody] JObject data)
        {
            TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = new TblPurchaseScheduleSummaryTO();
            tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            //int result = _iTblPurchaseScheduleSummaryBL.SaveVehicleScheduleDetails(tblPurchaseScheduleSummaryTO);
            resultMessage = _iTblPurchaseScheduleSummaryBL.SaveVehicleScheduleDetails(tblPurchaseScheduleSummaryTO);
            if (resultMessage.MessageType != ResultMessageE.Information)
            {
                return resultMessage;
            }

            // if (result == 2)
            // {
            //     string errText = "Vehicle No. - " + tblPurchaseScheduleSummaryTO.VehicleNo + " is already scheduled";
            //     resultMessage.DefaultBehaviour(errText);
            //     return resultMessage;
            // }

            if (tblPurchaseScheduleSummaryTO.QualityPhaseTOList != null && tblPurchaseScheduleSummaryTO.QualityPhaseTOList.Count > 0)
            {

                int loginUserId = tblPurchaseScheduleSummaryTO.CreatedBy;
                resultMessage = _iTblQualityPhaseBL.SavePhaseSampleListsagainstPurrchaseScheduleSummaryID(tblPurchaseScheduleSummaryTO.QualityPhaseTOList, Convert.ToInt32(loginUserId));
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }
            }

            //resultMessage.DefaultSuccessBehaviour();
            resultMessage.Tag = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;
            return resultMessage;


            // if (result >= 1)
            // {
            //     resultMessage.DefaultSuccessBehaviour();
            //     resultMessage.Tag = tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary;
            //     resultMessage.MessageType = ResultMessageE.Information;
            //     return resultMessage;
            // }
            // else
            // {
            //     resultMessage.DefaultBehaviour();
            //     resultMessage.Text = "Error While Saving Record";
            //     resultMessage.Result = 0;
            //     resultMessage.MessageType = ResultMessageE.Error;
            //     return resultMessage;
            // }
        }
        [Route("CloseOpenQtySauda")]
        [HttpPost]
        public ResultMessage CloseOpenQtySauda([FromBody] JObject data)
        {
            TblPurchaseEnquiryTO tblPurchaseEnquiryTO = new TblPurchaseEnquiryTO();
            tblPurchaseEnquiryTO = JsonConvert.DeserializeObject<TblPurchaseEnquiryTO>(data["bookingTO"].ToString());

            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            tblPurchaseEnquiryTO.UpdatedBy = loginUserId;
            tblPurchaseEnquiryTO.UpdatedOn = _iCommonDAO.ServerDateTime;

            return _iTblPurchaseEnquiryBL.CloseOpenQtySauda(tblPurchaseEnquiryTO);

        }

        [Route("CloseSaudaManually")]
        [HttpPost]
        public ResultMessage CloseSaudaManually([FromBody] JObject data)
        {
            TblPurchaseEnquiryTO tblPurchaseEnquiryTO = new TblPurchaseEnquiryTO();
            tblPurchaseEnquiryTO = JsonConvert.DeserializeObject<TblPurchaseEnquiryTO>(data["bookingTO"].ToString());

            Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            tblPurchaseEnquiryTO.UpdatedBy = loginUserId;
            tblPurchaseEnquiryTO.UpdatedOn = _iCommonDAO.ServerDateTime;

            return _iTblPurchaseEnquiryBL.CloseSaudaManually(tblPurchaseEnquiryTO);

        }


        [Route("AddUpdateShipmentDetails")]
        [HttpPost]
        public ResultMessage AddUpdateShipmentDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblpurchaseEnqShipmemtDtlsTO> tblpurchaseEnqShipmemtDtlsTOList = JsonConvert.DeserializeObject<List<TblpurchaseEnqShipmemtDtlsTO>>(data["tblpurchaseEnqShipmemtDtlsTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                DateTime currentDate = _iCommonDAO.ServerDateTime;
                if (tblpurchaseEnqShipmemtDtlsTOList == null && tblpurchaseEnqShipmemtDtlsTOList.Count>0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblpurchaseEnqShipmemtDtlsTO Found NULL";
                    return resultMessage;

                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                //tblpurchaseEnqShipmemtDtlsTO.UpdatedOn = currentDate;
                //tblpurchaseEnqShipmemtDtlsTO.UpdatedBy = Convert.ToInt32(loginUserId);
                return _iTblPurchaseEnquiryBL.AddUpdateShipmentDetails(tblpurchaseEnqShipmemtDtlsTOList, Convert.ToInt32(loginUserId));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method AddUpdateShipmentDetails";
                return resultMessage;
            }
        }


        #endregion

    }
}