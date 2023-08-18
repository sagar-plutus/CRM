using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.DashboardModels;
using Microsoft.Extensions.Logging;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class QuotaAndRateController : Controller
    {
        private readonly ILogger loggerObj;
        public QuotaAndRateController(ILogger<QuotaAndRateController> logger)
        {
            loggerObj = logger;
            Constants.LoggerObj = logger;
        }

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
            List<TblRateDeclareReasonsTO> tblRateDeclareReasonsTOList = BL.TblRateDeclareReasonsBL.SelectAllTblRateDeclareReasonsList();
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

        [Route("GetRateDeclarationHistory")]
        [HttpGet]
        public List<TblGlobalRateTO> GetRateDeclarationHistory(String fromDate, String toDate)
        {
            DateTime frmDate = Convert.ToDateTime(fromDate);
            DateTime tDate = Convert.ToDateTime(toDate);
            if (frmDate == DateTime.MinValue)
                frmDate = Constants.ServerDateTime.AddDays(-7);
            if (tDate == DateTime.MinValue)
                tDate = Constants.ServerDateTime;

            return BL.TblGlobalRateBL.SelectTblGlobalRateTOList(frmDate, tDate);
        }
        //Aniket
        [HttpGet]
        [Route("GetRateDetialsForGraph")]
        public List<GlobalRateTOFroGraph> GetRateDetialsForGraph(String fromDate,String toDate)
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

            return BL.TblGlobalRateBL.SelectTblGlobalRateListForGraph(frmDt,toDt);
        }
        /// <summary>
        /// Sanjay [2017-02-10] To Get Information About Specific Quota
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        [Route("GetQuotaDeclarationInfo")]
        [HttpGet]
        public TblQuotaDeclarationTO GetQuotaDeclarationInfo(Int32 quotaDeclarationId)
        {
            return BL.TblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(quotaDeclarationId);
        }

        /// <summary>
        /// Sanjay [2017-05-08] To Get Information About Specific Quota
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        [Route("CheckForValidityAndReset")]
        [HttpGet]
        public Boolean CheckForValidityAndReset(int quotaDeclarationId)
        {
            TblQuotaDeclarationTO tblQuotaDeclarationTO = BL.TblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(quotaDeclarationId);
            return BL.TblQuotaDeclarationBL.CheckForValidityAndReset(tblQuotaDeclarationTO);
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Show Latest Quota remaining and Rate Band for C&F agent on booking screen
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        [Route("GetLatestQuotaAndRateInfo")]
        [HttpGet]
        public List<TblQuotaDeclarationTO> GetLatestQuotaAndRateInfo(Int32 cnfId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = Constants.ServerDateTime;

            List<TblQuotaDeclarationTO> list = BL.TblQuotaDeclarationBL.SelectLatestQuotaDeclarationTOList(cnfId, sysDate);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ValidUpto > 0)
                    {
                        if (!BL.TblQuotaDeclarationBL.CheckForValidityAndReset(list[i]))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }

                return list;
            }
            return null;
        }

        [Route("GetQuotaAndRateDashboardInfo")]
        [HttpGet]
        public QuotaAndRateInfo GetQuotaAndRateDashboardInfo(int roleTypeId, Int32 orgId, DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = Constants.ServerDateTime;

            return BL.TblQuotaDeclarationBL.SelectQuotaAndRateDashboardInfo(roleTypeId, orgId, sysDate);
        }


        [Route("GetMinAndMaxValueConfigForRate")]
        [HttpGet]
        public String GetMinAndMaxValueConfigForRate()
        {
            string configValue = string.Empty;
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_MIN_AND_MAX_RATE_DEFAULT_VALUES);
            if (tblConfigParamsTO != null)
                configValue = Convert.ToString(tblConfigParamsTO.ConfigParamVal);
            return configValue;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // POST api/values
        [Route("AnnounceRateAndQuota")]
        [HttpPost]
        public ResultMessage AnnounceRateAndQuota([FromBody] JObject data)
        {
            loggerObj.LogError("1");

            
            try
            {
                List<TblOrganizationTO> tblOrganizationTOList = JsonConvert.DeserializeObject<List<TblOrganizationTO>>(data["cnfList"].ToString());
                var declaredRate = data["declaredRate"].ToString();
                var loginUserId = data["loginUserId"].ToString();
                var comments = data["comments"].ToString();
                var rateReasonId = data["rateReasonId"].ToString();
                var rateReasonDesc = data["rateReasonDesc"].ToString();
                ResultMessage resultMessage = new StaticStuff.ResultMessage();
                if (Convert.ToDouble(declaredRate) == 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "declaredRate Found 0";
                    return resultMessage;
                }

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (Convert.ToInt32(rateReasonId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "rateReasonId Found 0";
                    return resultMessage;
                }

                if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
                {
                    // 1. Prepare TblGlobalRateTO

                    DateTime serverDate = StaticStuff.Constants.ServerDateTime;
                    TblGlobalRateTO tblGlobalRateTO = new TblGlobalRateTO();
                    tblGlobalRateTO.CreatedOn = serverDate;
                    tblGlobalRateTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblGlobalRateTO.Rate = Convert.ToDouble(declaredRate);
                    tblGlobalRateTO.Comments = Convert.ToString(comments);
                    tblGlobalRateTO.RateReasonId = Convert.ToInt32(rateReasonId);
                    tblGlobalRateTO.RateReasonDesc = Convert.ToString(rateReasonDesc);

                    //2. Prepare Quota Declaration List
                    List<TblQuotaDeclarationTO> tblQuotaDeclarationTOList = new List<TblQuotaDeclarationTO>();
                    List<TblQuotaDeclarationTO> tblQuotaExtensionTOList = new List<TblQuotaDeclarationTO>();

                    var quotaExtList = tblOrganizationTOList.Where(q => q.ValidUpto > 0).ToList();

                    if (quotaExtList != null && quotaExtList.Count > 0)
                    {
                        for (int i = 0; i < quotaExtList.Count; i++)
                        {
                            TblOrganizationTO orgTO = quotaExtList[i];

                            TblQuotaDeclarationTO tblQuotaDeclarationTO = new TblQuotaDeclarationTO();
                            tblQuotaDeclarationTO.OrgId = orgTO.IdOrganization;
                            tblQuotaDeclarationTO.IdQuotaDeclaration = orgTO.QuotaDeclarationId;
                            tblQuotaDeclarationTO.ValidUpto = orgTO.ValidUpto;
                            tblQuotaDeclarationTO.UpdatedOn = serverDate;
                            tblQuotaDeclarationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                            tblQuotaDeclarationTO.IsActive = 1;
                            tblQuotaExtensionTOList.Add(tblQuotaDeclarationTO);
                        }
                    }

                    for (int i = 0; i < tblOrganizationTOList.Count; i++)
                    {
                        TblOrganizationTO orgTO = tblOrganizationTOList[i];

                        TblQuotaDeclarationTO tblQuotaDeclarationTO = new TblQuotaDeclarationTO();
                        tblQuotaDeclarationTO.OrgId = orgTO.IdOrganization;
                        tblQuotaDeclarationTO.QuotaAllocDate = serverDate;
                        tblQuotaDeclarationTO.RateBand = orgTO.LastRateBand;
                        tblQuotaDeclarationTO.AllocQty = orgTO.LastAllocQty;
                        tblQuotaDeclarationTO.CreatedOn = serverDate;
                        tblQuotaDeclarationTO.CreatedBy = Convert.ToInt32(loginUserId);
                        tblQuotaDeclarationTO.IsActive = 1;

                        tblQuotaDeclarationTO.Tag = orgTO;
                        tblQuotaDeclarationTOList.Add(tblQuotaDeclarationTO);

                    }

                    int result= BL.TblQuotaDeclarationBL.SaveDeclaredRateAndAllocatedQuota(tblQuotaExtensionTOList,tblQuotaDeclarationTOList, tblGlobalRateTO);
                    if(result!=1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error In SaveDeclaredRateAndAllocatedQuota Method";
                        return resultMessage;
                    }

                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Booking Quota Announced Sucessfully";
                    return resultMessage;

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "tblOrganizationTOList Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                ResultMessage resultMessage = new StaticStuff.ResultMessage();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call AnnounceRateAndQuota";
                return resultMessage;
            }
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
    }
}
