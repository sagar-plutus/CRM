using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

using System.Text;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PuchaseMeterialController : Controller
    {
        private readonly ITblPurchaseParityDetailsBL _iTblPurchaseParityDetailsBL;
        private readonly ITblPurchaseParitySummaryBL _iTblPurchaseParitySummaryBL;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblGradeQtyDtlsBL _iTblGradeQtyDtlsBL;

        public PuchaseMeterialController(Icommondao icommondao,ITblPurchaseParitySummaryBL iTblPurchaseParitySummaryBL, ITblPurchaseParityDetailsBL iTblPurchaseParityDetailsBL,
            ITblGradeQtyDtlsBL iTblGradeQtyDtlsBL)
        {
            _iCommonDAO = icommondao;
            _iTblPurchaseParitySummaryBL = iTblPurchaseParitySummaryBL;
            _iTblGradeQtyDtlsBL = iTblGradeQtyDtlsBL;
        }
        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
       
        [Route("GetPurchaseParityDetails")]
        [HttpGet]
        public TblPurchaseParitySummaryTO GetPurchaseParityDetails(Int32 stateId, Int32 prodSpecId, Int32 brandId = 0)
        {
            TblPurchaseParitySummaryTO latestParityTO = _iTblPurchaseParitySummaryBL.SelectStatesActiveParitySummaryTO(stateId, brandId);
            int parityId = 0;
            if (latestParityTO == null)
            {
                latestParityTO = new TblPurchaseParitySummaryTO();
            }
            else
            {
                parityId = latestParityTO.IdParity;
            }

            List<TblPurchaseParityDetailsTO> list = null;
            if (list == null || list.Count == 0)
            {
                list = _iTblPurchaseParityDetailsBL.SelectAllEmptyParityDetailsList(prodSpecId, stateId, brandId);
                if (list!= null)
                {
                    list = list.OrderBy(a => a.ProdCatId).ThenBy(a => a.ProdItemId).ToList();
                }
                latestParityTO.ParityDetailList = list;
            }
            else
                latestParityTO.ParityDetailList = list;
            return latestParityTO;
        }

        [Route("GetPurchaseCompetitorMaterialList")]
        [HttpGet]
        public List<TblPurchaseParitySummaryTO> GetPurchaseCompetitorMaterialList(Int32 competitorId, DateTime fromDate, DateTime toDate)
        {
            List <TblPurchaseParitySummaryTO> tblBookingsTOList = _iTblPurchaseParitySummaryBL.SelectAllPurchaseCompetitorMaterialList(competitorId, fromDate, toDate);
            return tblBookingsTOList;
        }
        
        [Route("GetPurchaseMaterialList")]
        [HttpGet]
        public List<DropDownTO> GetPurchaseMaterialList(Int32 stateId)
        {
            List<TblPurchaseParitySummaryTO> tblRateDeclareReasonsTOList = _iTblPurchaseParitySummaryBL.SelectAllMaterialReasonsList(stateId);
            if (tblRateDeclareReasonsTOList != null && tblRateDeclareReasonsTOList.Count > 0)
            {
                List<DropDownTO> reasonList = new List<DropDownTO>();
                for (int i = 0; i < tblRateDeclareReasonsTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblRateDeclareReasonsTOList[i].ProdClassDesc;
                    dropDownTO.Value = tblRateDeclareReasonsTOList[i].IdProdClass;
                    reasonList.Add(dropDownTO);
                }
                return reasonList;
            }
            else return null;
        }

        #endregion

        #region Post

        [Route("PostParityDetails")]
        [HttpPost]
        public ResultMessage PostParityDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseParitySummaryTO paritySummaryTO = JsonConvert.DeserializeObject<TblPurchaseParitySummaryTO>(data["parityTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : paritySummaryTO Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO.ParityDetailList == null || paritySummaryTO.ParityDetailList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : ParityDetailList Found NULL";
                    return resultMessage;
                }

                if (paritySummaryTO.StateId <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Selected State Not Found";
                    resultMessage.DisplayMessage = "Records could not be updated ";
                    return resultMessage;
                }

                DateTime createdDate =  _iCommonDAO.ServerDateTime;
                paritySummaryTO.CreatedOn = createdDate;
                paritySummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                paritySummaryTO.IsActive = 1;

                return _iTblPurchaseParitySummaryBL.SaveParitySettings(paritySummaryTO);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostParityDetails";
                return resultMessage;
            }
        }

        [Route("PostProductImgDetails")]
        [HttpPost]
        public ResultMessage PostProductImgDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                SaveProductImgTO saveProductImgTO = JsonConvert.DeserializeObject<SaveProductImgTO>(data["parityTO"].ToString());
                StringBuilder sb = new StringBuilder(saveProductImgTO.imgData);
                sb.Replace("data:image/png;base64,", "");
                byte[] contents = Convert.FromBase64String(sb.ToString());
                string fileName = "user" + saveProductImgTO.IdPerson + ".png";
                var uploadPath = AppContext.BaseDirectory + fileName;
                System.IO.File.WriteAllBytes(uploadPath, contents);
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                DateTime createdDate =  _iCommonDAO.ServerDateTime;
                //paritySummaryTO.CreatedOn = createdDate;
                //paritySummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                //paritySummaryTO.IsActive = 1;

                return _iTblPurchaseParitySummaryBL.SaveProductImgSettings(saveProductImgTO);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostParityDetails";
                return resultMessage;
            }
        }

        [Route("PostPurchaseGradeQtyDtls")]
        [HttpPost]
        public ResultMessage PostPurchaseGradeQtyDtls([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblGradeQtyDtlsTO> tblGradeQtyDtlsTOList = JsonConvert.DeserializeObject<List<TblGradeQtyDtlsTO>>(data["finalGradeQtyDtlsTOList"].ToString());
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

                if (tblGradeQtyDtlsTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblGradeQtyDtlsTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                resultMessage = _iTblGradeQtyDtlsBL.SavePurchaseGradeQtyDtls(tblGradeQtyDtlsTOList, loginUserId);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error In Method PostPurchaseGradeQtyDtls");
                return resultMessage;
            }
        }
        #endregion

    }
}
