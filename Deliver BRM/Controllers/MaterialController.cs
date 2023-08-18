using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.StaticStuff;
using Newtonsoft.Json;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
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
            return BL.TblMaterialBL.SelectAllMaterialListForDropDown();
        }

        [Route("GetProductAndSpecsList")]
        [HttpGet]
        public List<TblProductInfoTO> GetProductAndSpecsList(Int32 prodCatId)
        {
            return BL.TblProductInfoBL.SelectAllEmptyProductInfoList(prodCatId);

        }

        [Route("GetMaterialList")]
        [HttpGet]
        public List<TblMaterialTO> GetMaterialList()
        {
            return BL.TblMaterialBL.SelectAllTblMaterialList();
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
        public TblParitySummaryTO GetParityDetails(Int32 stateId, Int32 prodSpecId = 0)
        {
            TblParitySummaryTO latestParityTO = BL.TblParitySummaryBL.SelectStatesActiveParitySummaryTO(stateId);
            int parityId = 0;
            if (latestParityTO == null)
            {
                latestParityTO = new TblParitySummaryTO();

            }
            else
            {
                parityId = latestParityTO.IdParity;
            }

            //Sanjay [2017-06-25] Changes as Statewsie latest all spec wise needs to show
            //List<TblParityDetailsTO> list = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(parityId, prodSpecId, stateId);
            List<TblParityDetailsTO> list = null;
            if (list == null || list.Count == 0)
            {
                list= BL.TblParityDetailsBL.SelectAllEmptyParityDetailsList(prodSpecId, stateId);
                list = list.OrderBy(a => a.ProdCatId).ThenBy(a=>a.MaterialId).ToList();
                latestParityTO.ParityDetailList = list;
            }
            else
                latestParityTO.ParityDetailList = list;
            return latestParityTO;
        }

        [Route("GetProductClassificationList")]
        [HttpGet]
        public List<TblProdClassificationTO> GetProductClassificationList(string prodClassType = "")
        {
            return BL.TblProdClassificationBL.SelectAllTblProdClassificationList(prodClassType);
        }

        [Route("GetProdClassesForDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetProdClassesForDropDownList(Int32 parentClassId = 0)
        {
            return BL.TblProdClassificationBL.SelectAllProdClassificationForDropDown(parentClassId);
        }

        [Route("GetProductClassificationDetails")]
        [HttpGet]
        public TblProdClassificationTO GetProductClassificationDetails(Int32 idProdClass)
        {
            return BL.TblProdClassificationBL.SelectTblProdClassificationTO(idProdClass);
        }

        [Route("GetProductItemList")]
        [HttpGet]
        public List<TblProductItemTO> GetProductItemList(Int32 specificationId = 0)
        {
            return BL.TblProductItemBL.SelectAllTblProductItemList(specificationId);
        }

        [Route("GetProductItemDetails")]
        [HttpGet]
        public TblProductItemTO GetProductItemDetails(Int32 idProdItem)
        {
            return BL.TblProductItemBL.SelectTblProductItemTO(idProdItem);
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
            return BL.TblProductInfoBL.SelectProductInfoListByLoadingSlipExtIds(strLoadingSlipExtIds);
        }
        /// <summary>
        /// Vijaymala[12-09-2017] Added To Get Material Type List
        /// </summary>
        /// <returns></returns>
        [Route("GetMaterialTypeDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetMaterialTypeDropDownList()
        {
            return BL.TblMaterialBL.SelectMaterialTypeDropDownList();
        }


        /// <summary>
        ///Sudhir[15-Mar-2018] Added for Get ProductItem List Based Product Classification Id.  
        /// </summary>
        /// <param name="idProdClass"></param>
        /// <returns></returns>
        [Route("GetProductItemByProdClass")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblProductItemTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetProductItemByProdClass(Int32 idProdClass)
        {
            try
            {
                List<TblProductItemTO> productItemList = BL.TblProductItemBL.SelectProductItemList(idProdClass);
                if (productItemList != null)
                {
                    if (productItemList.Count > 0)
                        return Ok(productItemList);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(productItemList);
                }

            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }



        /// <summary>
        /// Sanjay [2018-02-19] To Show all Item Prod Catg List
        /// </summary>
        /// <returns></returns>
        /// <remarks>Retrives All Item Product Categories for e.g. FG,Scrap,Service Items etc</remarks>
        [Route("GetItemProductCategoryList")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetItemProductCategoryList()
        {
            try
            {
                List<DropDownTO> list = BL.DimensionBL.GetItemProductCategoryListForDropDown();
                if (list != null)
                {
                    if (list.Count == 0)
                        return NoContent();
                    return Ok(list);
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (System.Exception exc)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Sanjay [2018-02-19] To Retrive All the Product Classfication List By Item Product Category Enum
        /// </summary>
        /// <param name="itemProdCategoryE"></param>
        /// <returns></returns>
        [Route("GetProductClassListByItemCatg")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblProdClassificationTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetProductClassListByItemCatg(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            try
            {
                List<TblProdClassificationTO> list = BL.TblProdClassificationBL.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);

                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (System.Exception exc)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        ///  Priyanka [23-02-2018] : Added to Deactivate the Category from product classification list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostDeactivateCategory")]
        [HttpPost]
        public ResultMessage PostDeactivateCategory([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblProdClassificationTO tblprodClassificationTO = JsonConvert.DeserializeObject<TblProdClassificationTO>(data["prodClassificationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblprodClassificationTO == null)
                {
                    resultMessage.DefaultBehaviour("tblprodClassificationTO Found NULL");
                    return resultMessage;
                }

                if (tblprodClassificationTO != null)
                {
                    tblprodClassificationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblprodClassificationTO.UpdatedOn = Constants.ServerDateTime;

                    tblprodClassificationTO.IsActive = 0;

                    result = BL.TblProdClassificationBL.UpdateProdClassification(tblprodClassificationTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateCategory");
                return resultMessage;
            }

        }

        /// <summary>
        ///  Priyanka [23-02-2018] : Added to Deactivate Item/Product in Product Classification.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostDeactivateItemOrProduct")]
        [HttpPost]
        public ResultMessage PostDeactivateItemOrProduct([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result;
                TblProductItemTO tblprodItemTO = JsonConvert.DeserializeObject<TblProductItemTO>(data["prodItemTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblprodItemTO == null)
                {
                    resultMessage.DefaultBehaviour("tblprodItemTO Found NULL");
                    return resultMessage;
                }

                if (tblprodItemTO != null)
                {
                    tblprodItemTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblprodItemTO.UpdatedOn = Constants.ServerDateTime;
                    tblprodItemTO.IsActive = 0;

                    result = BL.TblProductItemBL.UpdateTblProductItem(tblprodItemTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateItemOrProduct");
                return resultMessage;
            }

        }

        ///// <summary>
        /////Sudhir[20-Mar-2018] Added for Get ParityDetails List
        ///// </summary>
        ///// <param name="idProdClass"></param>
        ///// <returns></returns>
        //[Route("GetParityDetailsList")]
        //[HttpGet]
        //public List<TblParityDetailsTO> GetParityDetailsList(Int32 productItemId, Int32 prodCatId, Int32 prodSpecId, Int32 materialId)
        //{
        //    try
        //    {
        //        List<TblParityDetailsTO> list = BL.TblParityDetailsBL.SelectAllParityDetailsOnProductItemId(productItemId, prodCatId, prodSpecId, materialId);
        //        if (list != null)
        //            return list;
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        ///Priyanka [14-09-2018] Added for Get ParityDetails List
        /// </summary>
        /// <returns></returns>
        [Route("GetParityDetailsList")]
        [HttpGet]
        public List<TblParityDetailsTO> GetParityDetailsList(Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo = 0)
        {
            try
            {
                List<TblParityDetailsTO> list = BL.TblParityDetailsBL.SelectAllParityDetailsOnProductItemId(productItemId, prodCatId, stateId, currencyId, productSpecInfoListTo);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
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

                DateTime createdDate = Constants.ServerDateTime;
                for (int i = 0; i < productInfoTOList.Count; i++)
                {
                    productInfoTOList[i].CreatedBy = Convert.ToInt32( loginUserId);
                    productInfoTOList[i].CreatedOn = createdDate;
                }
                ResultMessage rMessage = new ResultMessage();
                rMessage = BL.TblProductInfoBL.SaveProductInformation(productInfoTOList);
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

                DateTime createdDate = Constants.ServerDateTime;
                paritySummaryTO.CreatedOn = createdDate;
                paritySummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                paritySummaryTO.IsActive = 1;

                return BL.TblParitySummaryBL.SaveParitySettings(paritySummaryTO);
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

                DateTime createdDate = Constants.ServerDateTime;
                prodClassificationTO.CreatedOn = createdDate;
                prodClassificationTO.CreatedBy = Convert.ToInt32(loginUserId);
                prodClassificationTO.IsActive = 1;
                ResultMessage rMessage = new ResultMessage();
                int result = BL.TblProdClassificationBL.InsertProdClassification(prodClassificationTO);
                if(result==1)
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

                DateTime createdDate = Constants.ServerDateTime;
                prodClassificationTO.UpdatedOn = createdDate;
                prodClassificationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                //int result = BL.TblProdClassificationBL.UpdateTblProdClassification(prodClassificationTO);
                int result = BL.TblProdClassificationBL.UpdateProdClassification(prodClassificationTO);
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

                DateTime createdDate = Constants.ServerDateTime;
                productItemTO.CreatedOn = createdDate;
                productItemTO.CreatedBy = Convert.ToInt32(loginUserId);
                productItemTO.IsActive = 1;
                ResultMessage rMessage = new ResultMessage();
                int result = BL.TblProductItemBL.InsertTblProductItem(productItemTO);
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

                DateTime createdDate = Constants.ServerDateTime;
                productItemTO.UpdatedOn = createdDate;
                productItemTO.UpdatedBy = Convert.ToInt32(loginUserId);
                ResultMessage rMessage = new ResultMessage();
                int result = BL.TblProductItemBL.UpdateTblProductItem(productItemTO);
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

                if (tblMaterialTO == null )
                {
                    resultMessage.DefaultBehaviour("tblMaterialTO Found NULL");
                    return resultMessage;
                }
                    //tblMaterialTO.MateCompOrgId = 19;
                   // tblMaterialTO.MateSubCompOrgId = 20;
                    tblMaterialTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.CreatedOn = Constants.ServerDateTime;
                    tblMaterialTO.IsActive = 1;

                    int result = BL.TblMaterialBL.InsertTblMaterial(tblMaterialTO);
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

                if (tblMaterialTO != null )
                {
                    tblMaterialTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblMaterialTO.UpdatedOn = Constants.ServerDateTime;

                    result = BL.TblMaterialBL.UpdateTblMaterial(tblMaterialTO);
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
                    tblMaterialTO.DeactivatedOn= Constants.ServerDateTime;
                    tblMaterialTO.IsActive = 0;
                    result = BL.TblMaterialBL.UpdateTblMaterial(tblMaterialTO);
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

        /// <summary>
        /// Sudhir[21-MARCH-2018] Added  for Insert Parity Details New Req. Data insert only in tblParityDetails.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("SaveParityDetailsOtherItem")]
        [HttpPost]
        public ResultMessage SaveParityDetailsOtherItem([FromBody] JObject data)
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

                Int32 isForUpdate = (Convert.ToInt32(data["isForUpdate"].ToString()));

                //if (paritySummaryTO.StateId <= 0)
                //{
                //    resultMessage.DefaultBehaviour();
                //    resultMessage.Text = "API : Selected State Not Found";
                //    resultMessage.DisplayMessage = "Records could not be updated ";
                //    return resultMessage;
                //}

                DateTime createdDate = Constants.ServerDateTime;
                paritySummaryTO.CreatedOn = createdDate;
                paritySummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                paritySummaryTO.IsActive = 1;

                return BL.TblParityDetailsBL.SaveParityDetailsOtherItem(paritySummaryTO, isForUpdate);
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
