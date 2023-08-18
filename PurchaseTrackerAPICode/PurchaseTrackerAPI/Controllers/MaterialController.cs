using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.StaticStuff;
using Newtonsoft.Json;
using PurchaseTrackerAPI.DAL.Interfaces;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {

        private readonly ITblProductItemBL _iTblProductItemBL;
        private readonly ITblProductInfoBL _iTblProductInfoBL;
        private readonly ITblMaterialBL _iTblMaterialBL;
        private readonly ITblProdClassificationBL _iTblProdClassificationBL;
        private readonly Icommondao _iCommonDAO;
         private readonly ITblPurchaseParityDetailsBL _iTblPurchaseParityDetailsBL;

        public MaterialController(Icommondao icommondao, ITblProductInfoBL iTblProductInfoBL , ITblMaterialBL iTblMaterialBL, ITblProdClassificationBL iTblProdClassificationBL
                                  , ITblProductItemBL iTblProductItemBL , ITblPurchaseParityDetailsBL iTblPurchaseParityDetailsBL)
        {
            _iCommonDAO = icommondao;
            _iTblProductItemBL = iTblProductItemBL;
            _iTblProdClassificationBL = iTblProdClassificationBL;
            _iTblMaterialBL = iTblMaterialBL;
            _iTblProductInfoBL = iTblProductInfoBL;
            _iTblPurchaseParityDetailsBL=iTblPurchaseParityDetailsBL;

        }
        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetMaterialDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetMaterialDropDownList()
        {
            return _iTblMaterialBL.SelectAllMaterialListForDropDown();

        }

        [Route("GetProductAndSpecsList")]
        [HttpGet]
        public List<TblProductInfoTO> GetProductAndSpecsList(Int32 prodCatId)
        {
            return _iTblProductInfoBL.SelectAllEmptyProductInfoList(prodCatId);

        }

        [Route("GetMaterialList")]
        [HttpGet]
        public List<TblMaterialTO> GetMaterialList()
        {
            return _iTblMaterialBL.SelectAllTblMaterialList();
        }

        /// <summary>
        /// Sanjay [2017-04-25] If parityId=0 then will return latest parity details if exist 
        /// and if parityId <>0 then parity details of given parityId
        /// </summary>
        /// <param name="parityId"></param>
        /// <param name="prodSpecId"> Added On 24/07/2017. After discussion with Nitin K [Meeting Ref. 21/07/2017 Pune] parity will be against prod Spec also.</param>
        /// <returns></returns>
        [Route("GetParityDetails")]
        [HttpGet]
        public TblParitySummaryTO GetParityDetails(Int32 stateId, Int32 prodSpecId = 0, Int32 brandId = 0)
        {

            return null;
        }

        [Route("GetProductClassificationList")]
        [HttpGet]
        public List<TblProdClassificationTO> GetProductClassificationList(string prodClassType = "")
        {
            return _iTblProdClassificationBL.SelectAllTblProdClassificationList(prodClassType);
        }

        [Route("GetProdClassesForDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetProdClassesForDropDownList(Int32 parentClassId = 0)
        {
            return _iTblProdClassificationBL.SelectAllProdClassificationForDropDown(parentClassId);
        }

        [Route("GetProductClassificationDetails")]
        [HttpGet]
        public TblProdClassificationTO GetProductClassificationDetails(Int32 idProdClass)
        {
            return _iTblProdClassificationBL.SelectTblProdClassificationTO(idProdClass);
        }

        [Route("GetProductItemList")]
        [HttpGet]
        public List<TblProductItemTO> GetProductItemList(Int32 specificationId = 0)
        {
            return _iTblProductItemBL.SelectAllTblProductItemList(specificationId);
        }

        /// <summary>
        /// Swati Pisal
        /// Get statewise parity details 
        /// </summary>
        /// <param name="specificationId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [Route("GetProductItemListWithParityAndRecovery")]
        [HttpGet]
        public List<TblProductItemTO> GetProductItemListWithParityAndRecovery(Int32 specificationId = 0, Int32 stateId = 0)
        {
            return _iTblProductItemBL.SelectAllTblProductItemListWithParityAndRecovery(specificationId, stateId);
        }

        //Prajakta[2019-02-21] Get Latest parity while booking as per disscussion with vivek sir and saket
        [Route("gnGetProductItemListWithParityAndRecoveryNew")]
        [HttpGet]
        public List<TblProductItemTO> gnGetProductItemListWithParityAndRecoveryNew(Int32 specificationId = 0, Int32 stateId = 0)
        {
            return _iTblProductItemBL.SelectAllTblProductItemListWithParityAndRecoveryNew(specificationId, stateId);
        }
         [Route("GetGradeBookingParityDtls")]
        [HttpGet]
        public List<TblProductItemTO> GetGradeBookingParityDtls(DateTime saudaCreatedOn,Int32 specificationId = 0, Int32 stateId = 0)
        {
            return _iTblProductItemBL.GetGradeBookingParityDtls(saudaCreatedOn,specificationId, stateId);
        }
        [Route("gnGetProductGraidList")]
        [HttpGet]
        public List<TblProductItemTO> gnGetProductGraidList(Int32 specificationId = 0)
        {
            return _iTblProductItemBL.SelectAllTblProductGraidList(specificationId);
        }
        
        //Prajakta[2019-04-20] Added
        [Route("GetBaseItemRecovery")]
        [HttpGet]
        public TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem = 0, Int32 stateId = 0)
        {
            return _iTblProductItemBL.GetBaseItemRecovery(isBaseItem, stateId);
        }

        [Route("GetProductItemDetails")]
        [HttpGet]
        public TblProductItemTO GetProductItemDetails(Int32 idProdItem)
        {
            return _iTblProductItemBL.SelectTblProductItemTO(idProdItem);
        }

        /// <summary>
        /// GJ@20170818 : Get the Prouct Master Info List by LoadingSlipExt Ids for Bundles calculation
        /// </summary>
        /// <param name="strLoadingSlipExtIds"></param>
        /// <param name="strLoadingSlipExtIds">Added to know the Loading Slip Ext Ids</param>
        /// <returns></returns>
        [Route("GetProductSpecificationListByLoadingSlipExtIds")]
        [HttpGet]
        public List<TblProductInfoTO> GetProductSpecificationListByLoadingSlipExtIds(string strLoadingSlipExtIds)
        {
            return _iTblProductInfoBL.SelectProductInfoListByLoadingSlipExtIds(strLoadingSlipExtIds);
        }

        /// <summary>
        /// Saket [2017-01-17] Added.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllProductSpecificationList")]
        [HttpGet]
        public List<TblProductInfoTO> GetAllProductSpecificationList()
        {
            return _iTblProductInfoBL.SelectAllTblProductInfoListLatest();
        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To Get Material Type List
        /// </summary>
        /// <returns></returns>
        [Route("GetMaterialTypeDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetMaterialTypeDropDownList()
        {
            return _iTblMaterialBL.SelectMaterialTypeDropDownList();
        }
        //Prajakta[2018-08-13] Added to get Scrap Material Details 
        [Route("GetProductClassListByItemCatg")]
        [HttpGet]
        public List<TblProdClassificationTO> GetProductClassListByItemCatg(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            List<TblProdClassificationTO> TblProdClassificationTOCatlist = _iTblProdClassificationBL.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);
            List<TblProdClassificationTO> TblProdClassificationTOSpecificationlist=new  List<TblProdClassificationTO>();
            if (TblProdClassificationTOCatlist != null && TblProdClassificationTOCatlist.Count > 0)
            {
                 string catStr = (string.Join(",", TblProdClassificationTOCatlist.Select(x => x.IdProdClass.ToString()).ToArray()));

                List<TblProdClassificationTO> TblProdClassificationTOSubCatlist = _iTblProdClassificationBL.SelectAllTblProdClassification(catStr,"SC");
                 if (TblProdClassificationTOSubCatlist != null && TblProdClassificationTOSubCatlist.Count > 0)
                 {
                    string subCatStr = (string.Join(",", TblProdClassificationTOSubCatlist.Select(x => x.IdProdClass.ToString()).ToArray()));

                   TblProdClassificationTOSpecificationlist = _iTblProdClassificationBL.SelectAllTblProdClassification(subCatStr,"S");
                 }
               
            }
            return TblProdClassificationTOSpecificationlist;
             
        }

        /// <summary>
        /// Harshala [2019-09-19] Added.
        /// </summary>
        [Route("GetParityHistoryListProductwise")]
        [HttpGet]
        public List<TblPurchaseParityDetailsTO> GetParityHistoryListProductwise(string ProdItemId, Int32 stateId)
        {
            DateTime confirmedDate=_iCommonDAO.ServerDateTime; 
            return _iTblPurchaseParityDetailsBL.GetBookingItemsParityDtls(ProdItemId,confirmedDate,stateId);
          
        }
        #endregion

        #region Post

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("PostProductInformation")]
        [HttpPost]
        public ResultMessage PostProductInformation([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblProductInfoTO> productInfoTOList = JsonConvert.DeserializeObject<List<TblProductInfoTO>>(data["productInfoTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (productInfoTOList == null || productInfoTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : productInfoTOList Found NULL";
                    return resultMessage;
                }

                DateTime createdDate =  _iCommonDAO.ServerDateTime;
                for (int i = 0; i < productInfoTOList.Count; i++)
                {
                    productInfoTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    productInfoTOList[i].CreatedOn = createdDate;
                }
                ResultMessage rMessage = new ResultMessage();
                rMessage = _iTblProductInfoBL.SaveProductInformation(productInfoTOList);
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostProductInformation";
                return resultMessage;
            }
        }


        /// <summary>
        /// Sanjay [2017-04-21] To Save Material Sizewise Parity Details
        /// Will Deactivate all Prev Parity Details and Inserts New Parity Details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostParityDetails")]
        [HttpPost]
        public ResultMessage PostParityDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblParitySummaryTO paritySummaryTO = JsonConvert.DeserializeObject<TblParitySummaryTO>(data["parityTO"].ToString());
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

                return null; //BL.TblParitySummaryBL.SaveParitySettings(paritySummaryTO);
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

        [Route("PostNewProductClassification")]
        [HttpPost]
        public ResultMessage PostNewProductClassification([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProdClassificationTO prodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (prodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("API : prodClassificationTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate =  _iCommonDAO.ServerDateTime;
                prodClassificationTO.CreatedOn = createdDate;
                prodClassificationTO.CreatedBy = Convert.ToInt32(loginUserId);
                prodClassificationTO.IsActive = 1;
                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProdClassificationBL.InsertProdClassification(prodClassificationTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While InsertTblProdClassification");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductClassification");
                return resultMessage;
            }
        }

        [Route("PostUpdateProductClassification")]
        [HttpPost]
        public ResultMessage PostUpdateProductClassification([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProdClassificationTO prodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (prodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("API : prodClassificationTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate =  _iCommonDAO.ServerDateTime;
                prodClassificationTO.UpdatedOn = createdDate;
                prodClassificationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProdClassificationBL.UpdateProdClassification(prodClassificationTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While UpdateTblProdClassification");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductClassification");
                return resultMessage;
            }
        }

        [Route("PostNewProductItem")]
        [HttpPost]
        public ResultMessage PostNewProductItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProductItemTO productItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (productItemTO == null)
                {
                    resultMessage.DefaultBehaviour("API : productItemTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate =  _iCommonDAO.ServerDateTime;
                productItemTO.CreatedOn = createdDate;
                productItemTO.CreatedBy = Convert.ToInt32(loginUserId);
                productItemTO.IsActive = 1;
                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProductItemBL.InsertTblProductItem(productItemTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While InsertTblProductItem");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewProductItem");
                return resultMessage;
            }
        }

        [Route("PostUpdateProductItem")]
        [HttpPost]
        public ResultMessage PostUpdateProductItem([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblProductItemTO productItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("API : Login User ID Found NULL");
                    return resultMessage;
                }

                if (productItemTO == null)
                {
                    resultMessage.DefaultBehaviour("API : productItemTO Found NULL");
                    return resultMessage;
                }

                DateTime createdDate =  _iCommonDAO.ServerDateTime;
                productItemTO.UpdatedOn = createdDate;
                productItemTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                int result = _iTblProductItemBL.UpdateTblProductItem(productItemTO);
                if (result == 1)
                {
                    rMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    rMessage.DefaultBehaviour("Error While UpdateTblProductItem");
                }
                return rMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateProductItem");
                return resultMessage;
            }
        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To save Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewMaterial")]
        [HttpPost]
        public ResultMessage PostNewMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }
                //tblMaterialTO.MateCompOrgId = 19;
                // tblMaterialTO.MateSubCompOrgId = 20;
                tblMaterialTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblMaterialTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                tblMaterialTO.IsActive = 1;

                int result = _iTblMaterialBL.InsertTblMaterial(tblMaterialTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewMaterial");
                return resultMessage;
            }

        }

        /// <summary>
        /// Vijaymala[12-09-2017] Added To Update Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateMaterial")]
        [HttpPost]
        public ResultMessage PostUpdateMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO != null)
                {
                    tblMaterialTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.UpdatedOn =  _iCommonDAO.ServerDateTime;

                    result = _iTblMaterialBL.UpdateTblMaterial(tblMaterialTO);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be updated");
                        return resultMessage;
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateMaterial");
                return resultMessage;
            }

        }


        /// <summary>
        /// Vijaymala[12-09-2017] Added To Update Material Size
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeactivateMaterial")]
        [HttpPost]
        public ResultMessage PostDeactivateMaterial([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblMaterialTO tblMaterialTO = JsonConvert.DeserializeObject<TblMaterialTO>(data["materialSizeTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO == null)
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }

                if (tblMaterialTO != null)
                {
                    tblMaterialTO.DeactivatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.DeactivatedOn =  _iCommonDAO.ServerDateTime;
                    tblMaterialTO.IsActive = 0;
                    result = _iTblMaterialBL.UpdateTblMaterial(tblMaterialTO);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                        return resultMessage;
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateMaterial");
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
