using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseCompetitorController : Controller
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblPurchaseCompetitorExtBL _iTblPurchaseCompetitorExtBL;
        private readonly ITblPurchaseCompetitorUpdatesBL _iTblPurchaseCompetitorUpdatesBL;
        public PurchaseCompetitorController(ITblPurchaseCompetitorExtBL iTblPurchaseCompetitorExtBL, ITblPurchaseCompetitorUpdatesBL iTblPurchaseCompetitorUpdatesBL, Icommondao icommondao)
        {
            _iCommonDAO = icommondao;
            _iTblPurchaseCompetitorUpdatesBL = iTblPurchaseCompetitorUpdatesBL;
            _iTblPurchaseCompetitorExtBL = iTblPurchaseCompetitorExtBL;
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

        [Route("GetCompetitorBrandNamesDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetCompetitorBrandNamesDropDownList(Int32 competitorOrgId)
        {
            List<DropDownTO> list = _iTblPurchaseCompetitorUpdatesBL.SelectCompetitorBrandNamesDropDownList(competitorOrgId);
            return list;
        }

        [Route("GetCompetitorMaterilGradeDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetCompetitorMaterilGradeDropDownList(Int32 MaterialId, Int32 competitorOrgId)
        {
            List<DropDownTO> list = _iTblPurchaseCompetitorUpdatesBL.SelectCompetitorMaterialGradeDropDownList(MaterialId,competitorOrgId);
            return list;
        }

        [Route("GetLastPriceForCompetitorAndBrand")]
        [HttpGet]
        public TblPurchaseCompetitorUpdatesTO GetLastPriceForCompetitorAndBrand(Int32 brandId)
        {
            return _iTblPurchaseCompetitorUpdatesBL.SelectLastPriceForCompetitorAndBrand(brandId);
        }

        [Route("GetMaterialListByCompetitorId")]
        [HttpGet]
        public List<TblPurchaseCompetitorExtTO> GetMaterialListByCompetitorId(Int32 competitorId)
        {
            return _iTblPurchaseCompetitorExtBL.GetMaterialListByCompetitorId(competitorId);
        }


        #endregion

        #region Post

        [Route("PostMarketUpdate")]
        [HttpPost]
        public ResultMessage PostMarketUpdate([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseCompetitorUpdatesTO> competitorUpdatesTOList = JsonConvert.DeserializeObject<List<TblPurchaseCompetitorUpdatesTO>>(data["competitorUpdatesTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (competitorUpdatesTOList == null || competitorUpdatesTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : sizeSpecWiseStockTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate =  _iCommonDAO.ServerDateTime;
                DateTime stockDate = confirmedDate.Date;
                for (int i = 0; i < competitorUpdatesTOList.Count; i++)
                {
                    competitorUpdatesTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    competitorUpdatesTOList[i].CreatedOn = confirmedDate;
                    competitorUpdatesTOList[i].UpdateDatetime = confirmedDate;
                    if (competitorUpdatesTOList[i].DealerId > 0)
                       competitorUpdatesTOList[i].InformerName = competitorUpdatesTOList[i].DealerName;
                    else if (competitorUpdatesTOList[i].OtherSourceId > 0)
                       competitorUpdatesTOList[i].InformerName = competitorUpdatesTOList[i].OtherSourceDesc;
                    else
                       competitorUpdatesTOList[i].InformerName = competitorUpdatesTOList[i].OtherSourceOtherDesc; 
                }

                ResultMessage resMsg = _iTblPurchaseCompetitorUpdatesBL.SaveMarketUpdate(competitorUpdatesTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostMarketUpdate";
                return returnMsg;
            }
        }

        /*
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        */

        #endregion

    }
}
