using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DashboardModels;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class RateController : Controller
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblRateDeclareReasonsBL _iTblRateDeclareReasonsBL;
        private readonly ITblRateBandDeclarationPurchaseBL _iTblRateBandDeclarationPurchaseBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblGlobalRatePurchaseBL _iTblGlobalRatePurchaseBL;
        private readonly ITblBaseItemMetalCostBL _iTblBaseItemMetalCostBL;
        private readonly ICircularDependancyBL _iICircularDependancyBL;

        private readonly ITblGradeExpressionDtlsBL _iTblGradeExpressionDtlsBL;
        private readonly ICircularDependancyBL _ICircularDependancyBL;

        public RateController (ITblRateBandDeclarationPurchaseBL iTblRateBandDeclarationPurchaseBL, Icommondao icommondao , ITblRateDeclareReasonsBL iTblRateDeclareReasonsBL, ITblConfigParamsBL iTblConfigParamsBL, ITblGlobalRatePurchaseBL iTblGlobalRatePurchaseBL, ITblBaseItemMetalCostBL iTblBaseItemMetalCostBL,
        ITblGradeExpressionDtlsBL iTblGradeExpressionDtlsBL,
        ICircularDependancyBL iCircularDependancyBL) {
            _iTblRateBandDeclarationPurchaseBL = iTblRateBandDeclarationPurchaseBL;
            _iTblBaseItemMetalCostBL = iTblBaseItemMetalCostBL;
            _iTblGlobalRatePurchaseBL = iTblGlobalRatePurchaseBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblRateDeclareReasonsBL = iTblRateDeclareReasonsBL;
            _iCommonDAO = icommondao;
            _iTblGradeExpressionDtlsBL = iTblGradeExpressionDtlsBL;
            _ICircularDependancyBL = iCircularDependancyBL;

        }

        #region Get Methods...
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

        [Route("GetRateReasonsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetRateReasonsForDropDown()
        {
            List<TblRateDeclareReasonsTO> tblRateDeclareReasonsTOList = _iTblRateDeclareReasonsBL.SelectAllTblRateDeclareReasonsList();
            if (tblRateDeclareReasonsTOList != null && tblRateDeclareReasonsTOList.Count > 0)
            {
                List<DropDownTO> reasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblRateDeclareReasonsTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblRateDeclareReasonsTOList[i].ReasonDesc;
                    dropDownTO.Value = tblRateDeclareReasonsTOList[i].IdRateReason;
                    reasonList.Add(dropDownTO);
                }
                return reasonList;
            }
            else return null;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

        #region code added/modified by swati

        [Route("GetLatestRateInfo")]
        [HttpGet]
        public List<TblRateBandDeclarationPurchaseTO> GetLatestRateInfo(Int32 cnfId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate =  _iCommonDAO.ServerDateTime;

            List<TblRateBandDeclarationPurchaseTO> list = _iTblRateBandDeclarationPurchaseBL.SelectLatestRateBandDeclarationPurchaseTOList(cnfId, sysDate);
            if (list != null)
            {
                //commented by swati pisal
                //need to confirm bcz using this code today's rateband declaration entry is inActive
                //for (int i = 0; i < list.Count; i++)
                //{
                //    if (list[i].ValidUpto > 0)
                //    {
                //        if (!BL.TblRateBandDeclarationPurchaseBL.CheckForValidityAndReset(list[i]))
                //        {
                //            list.RemoveAt(i);
                //            i--;
                //        }
                //    }
                //}

                return list;
            }
            return null;
        }

        [Route("GetRateDeclartionDtlsWhileBooking")]
        [HttpGet]
        public List<TblRateBandDeclarationPurchaseTO> GetRateDeclartionDtlsWhileBooking(Int32 cnfId)
        {
            return _ICircularDependancyBL.GetRateDeclartionDtlsWhileBooking(cnfId);
        }

        [Route("GetRateInfo")]
        [HttpGet]
        public List<TblRateBandDeclarationPurchaseTO> GetRateInfo(Int32 cnfId)
        {
            DateTime sysDate = new DateTime();
            if (sysDate == DateTime.MinValue)
                sysDate =  _iCommonDAO.ServerDateTime;

            TblRateBandDeclarationPurchaseTO listOldRate = _iTblRateBandDeclarationPurchaseBL.SelectOldRateTOList(cnfId, sysDate);
            TblRateBandDeclarationPurchaseTO listNewRate = _iTblRateBandDeclarationPurchaseBL.SelectLatestRateTOList(cnfId, sysDate);
            List<TblRateBandDeclarationPurchaseTO> listRate = new List<TblRateBandDeclarationPurchaseTO>();

            if (listOldRate != null)
            {
                listRate.Add(listOldRate);
                listRate.Add(listNewRate);
            }
            else
                listRate.Add(listNewRate);

            return listRate;
        }

        [Route("GetRateBandDeclarationInfo")]
        [HttpGet]
        public TblRateBandDeclarationPurchaseTO GetRateBandDeclarationInfo(Int32 rateBandDeclarationId)
        {
            return _iTblRateBandDeclarationPurchaseBL.SelectTblRateBandDeclaration(rateBandDeclarationId);
        }

        [Route("GetPurchaseManagerWithBand")]
        [HttpGet]
        public List<TblRateBandDeclarationPurchaseTO> GetPurchaseManagerWithBand(DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate =  _iCommonDAO.ServerDateTime;

            List<TblGlobalRatePurchaseTO> globalRatePurchase = GetLatestPurchaseRate(0);
            Int32 globalPurchaseId = 0;
            if (globalRatePurchase != null && globalRatePurchase.Count > 0)
            {
                globalPurchaseId = globalRatePurchase.Max(e => e.IdGlobalRatePurchase);
            }
            List<TblRateBandDeclarationPurchaseTO> tblRateBandDeclarationPurchaseTO = _iTblRateBandDeclarationPurchaseBL.GetPurchaseManagerWithBand(globalPurchaseId);
            return tblRateBandDeclarationPurchaseTO;
        }

        // [HttpGet]
        [Route("GetLatestPurchaseRate")]
        [HttpGet]
        public List<TblGlobalRatePurchaseTO> GetLatestPurchaseRate(Int32 forQuota)
        {
            DateTime sysDate =  _iCommonDAO.ServerDateTime;
            int Count = 4;
            return _iTblGlobalRatePurchaseBL.SelectLatestRateOfPurchaseDCT(forQuota, sysDate, ref Count);
        }

        [Route("GetGlobalPurchaseRateList")]
        [HttpGet]
        public List<TblGlobalRatePurchaseTO> GetGlobalPurchaseRateList(String fromDate, String toDate)
        {
            DateTime frmDate = Convert.ToDateTime(fromDate);
            DateTime tDate = Convert.ToDateTime(toDate);
            if (frmDate == DateTime.MinValue)
                frmDate =  _iCommonDAO.ServerDateTime.AddDays(-7);
            if (tDate == DateTime.MinValue)
                tDate =  _iCommonDAO.ServerDateTime;
            List<TblGlobalRatePurchaseTO> globalPurchaseRateList = _iTblGlobalRatePurchaseBL.GetGlobalPurchaseRateList(frmDate, tDate);
            return globalPurchaseRateList;
        }

        [Route("GetMinAndMaxValueConfigForRate")]
        [HttpGet]
        public String GetMinAndMaxValueConfigForRate()
        {
            string configValue = string.Empty;
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_MIN_AND_MAX_RATE_DEFAULT_VALUES);
            if (tblConfigParamsTO != null)
                configValue = Convert.ToString(tblConfigParamsTO.ConfigParamVal);
            return configValue;
        }
        [Route("GetLatestBaseItemMetalCost")]
        [HttpGet]
        public List<TblBaseItemMetalCostTO> GetLatestBaseItemMetalCost(Int32 globalRatePurchaseId)
        {
            List<TblBaseItemMetalCostTO> tblBaseItemMetalCostTO = _iTblBaseItemMetalCostBL.SelectLatestBaseItemMetalCost(globalRatePurchaseId);
            return tblBaseItemMetalCostTO;
        }

        [Route("GetBaseItemGradeExpreDtls")]
        [HttpGet]
        public TblBaseItemMetalCostTO GetBaseItemGradeExpreDtls(Int32 globalRatePurchaseId,Int32 cOrNcId)
        {
            TblBaseItemMetalCostTO tblBaseItemMetalCostTO = _iTblGradeExpressionDtlsBL.GetBaseItemGradeExpreDtls(globalRatePurchaseId,cOrNcId);
            return tblBaseItemMetalCostTO;
        }

      

        // POST api/values
        [Route("AnnounceRateAndQuota")]
        [HttpPost]
        public ResultMessage AnnounceRateAndQuota([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblRateBandDeclarationPurchaseTO> rateBandDeclarationList = JsonConvert.DeserializeObject<List<TblRateBandDeclarationPurchaseTO>>(data["cnfList"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                var comments = data["comments"].ToString();
                var rateReasonId = data["rateReasonId"].ToString();
                var rateReasonDesc = data["rateReasonDesc"].ToString();
                var rate = data["rate"].ToString();
                //TblBaseItemMetalCostTO tblBaseItemMetalCostTO = JsonConvert.DeserializeObject<TblBaseItemMetalCostTO>(data["baseItemMetalCostTO"].ToString());

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                // 1. Prepare TblGlobalRateTO
                List<TblGlobalRatePurchaseTO> tblGlobalRateTOList = new List<TblGlobalRatePurchaseTO>();
                DateTime serverDate = _iCommonDAO.ServerDateTime;
                TblGlobalRatePurchaseTO tblGlobalRateTO = new TblGlobalRatePurchaseTO();
                tblGlobalRateTO.CreatedOn = serverDate;
                tblGlobalRateTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblGlobalRateTO.Comments = Convert.ToString(comments);
                tblGlobalRateTO.RateReasonId = Convert.ToInt32(rateReasonId);
                tblGlobalRateTO.RateReasonDesc = Convert.ToString(rateReasonDesc);
                tblGlobalRateTO.Rate = Convert.ToDouble(rate);

                // tblBaseItemMetalCostTO.CreatedBy = Convert.ToInt32(loginUserId);
                // tblBaseItemMetalCostTO.UpdatedBy = Convert.ToInt32(loginUserId);
                // tblBaseItemMetalCostTO.CreatedOn = serverDate;
                // tblBaseItemMetalCostTO.UpdatedOn = serverDate;

                if (rateBandDeclarationList != null && rateBandDeclarationList.Count > 0)
                {
                    // 1. Prepare TblGlobalRatePurchaseTO

                    //  List<TblRateBandDeclarationPurchaseTO> tblRateBandDeclarationPurchaseList = new List<TblRateBandDeclarationPurchaseTO>();
                    // TblGlobalRatePurchaseTO tblGlobalRatePurchaseTOList = new TblGlobalRatePurchaseTO();
                    tblGlobalRateTO.RateBandDeclarationPurchaseTOList = new List<TblRateBandDeclarationPurchaseTO>();
                    for (int q = 0; q < rateBandDeclarationList.Count; q++)
                    {
                        TblRateBandDeclarationPurchaseTO rateBandTO = rateBandDeclarationList[q];
                        TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO = new TblRateBandDeclarationPurchaseTO();
                        tblRateBandDeclarationPurchaseTO.UserId = rateBandTO.UserId;
                        tblRateBandDeclarationPurchaseTO.RateBandCosting = rateBandTO.RateBandCosting;
                        tblRateBandDeclarationPurchaseTO.RateBandCorrection = rateBandTO.RateBandCorrection;
                        tblRateBandDeclarationPurchaseTO.ValidUpto = rateBandTO.ValidUpto;
                        tblRateBandDeclarationPurchaseTO.CalculatedRateCosting = rateBandTO.CalculatedRateCosting;
                        tblRateBandDeclarationPurchaseTO.CalculatedRateCorrection = rateBandTO.CalculatedRateCorrection;
                        tblRateBandDeclarationPurchaseTO.CreatedOn = serverDate;
                        tblRateBandDeclarationPurchaseTO.CreatedBy = Convert.ToInt32(loginUserId);
                        tblRateBandDeclarationPurchaseTO.IsActive = 1;
                        tblRateBandDeclarationPurchaseTO.GradeWiseTargetQtyTOList = rateBandTO.GradeWiseTargetQtyTOList;
                        //  tblRateBandDeclarationPurchaseTO.Tag = rateBandTO;
                        tblGlobalRateTO.RateBandDeclarationPurchaseTOList.Add(tblRateBandDeclarationPurchaseTO);
                    }
                    tblGlobalRateTOList.Add(tblGlobalRateTO);
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblOrganizationTOList Found NULL");
                    return resultMessage;
                }

                return _iTblRateBandDeclarationPurchaseBL.SaveDeclaredRate(tblGlobalRateTOList,Convert.ToInt32(loginUserId),serverDate);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AnnounceRateAndQuota");
                return resultMessage;
            }
        }

        #endregion

    }
}
