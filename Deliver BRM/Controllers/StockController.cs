using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using SalesTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.StaticStuff;
using System.Dynamic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class StockController : Controller
    {
        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetStockLocationsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetStockLocationsForDropDown()
        {
            List<TblLocationTO> tblLocationTOList = BL.TblLocationBL.SelectAllParentLocation();
            if (tblLocationTOList != null && tblLocationTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblLocationTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblLocationTOList[i].LocationDesc;
                    dropDownTO.Value = tblLocationTOList[i].IdLocation;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetCompartmentsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCompartmentsForDropDown(Int32 locationId)
        {
            List<TblLocationTO> tblLocationTOList = BL.TblLocationBL.SelectAllCompartmentLocationList(locationId);
            if (tblLocationTOList != null && tblLocationTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblLocationTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblLocationTOList[i].LocationDesc;
                    dropDownTO.Value = tblLocationTOList[i].IdLocation;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetProdCategoryForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetProdCategoryForDropDown()
        {
            List<DimProdCatTO> dimProdCatTOList = BL.DimProdCatBL.SelectAllDimProdCatList();
            if (dimProdCatTOList != null && dimProdCatTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < dimProdCatTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = dimProdCatTOList[i].ProdCateDesc;
                    dropDownTO.Value = dimProdCatTOList[i].IdProdCat;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetProdSepcificationsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetProdSepcificationsForDropDown()
        {
            List<DimProdSpecTO> dimProdSpecTOList = BL.DimProdSpecBL.SelectAllDimProdSpecList();
            if (dimProdSpecTOList != null && dimProdSpecTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < dimProdSpecTOList.Count; i++)
                {
                    if (dimProdSpecTOList[i].IsActive == 1)  //Saket [2018-01-30] Added
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = dimProdSpecTOList[i].ProdSpecDesc;
                        dropDownTO.Value = dimProdSpecTOList[i].IdProdSpec;
                        statusReasonList.Add(dropDownTO);
                    }
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetMateAndSpecsList")]
        [HttpGet]
        public List<TblStockDetailsTO> GetMateAndSpecsList(Int32 locationId, Int32 prodCatId, DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = Constants.ServerDateTime.Date;

            return BL.TblStockDetailsBL.SelectAllTblStockDetailsList(locationId, prodCatId, stockDate);

        }

        [Route("GetStockDtlsByCategAndSpecs")]
        [HttpGet]
        public List<TblStockDetailsTO> GetStockDtlsByCategAndSpecs(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = Constants.ServerDateTime.Date;

            return BL.TblStockDetailsBL.SelectStockDetailsListByProdCatgAndSpec(prodCatId, prodSpecId, stockDate);

        }

        [Route("GetStockSummaryDetails")]
        [HttpGet]
        public List<SizeSpecWiseStockTO> GetStockSummaryDetails(DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = Constants.ServerDateTime.Date;

            List<SizeSpecWiseStockTO> list = BL.TblStockDetailsBL.SelectSizeAndSpecWiseStockSummary(stockDate);
            //List<SizeSpecWiseStockTO>  xx=list.OrderBy(sp => sp.ProdSpecId).ToList();
            return list;

        }

        [Route("GetDatewiseRunningSizeDtls")]
        [HttpGet]
        public List<TblRunningSizesTO> GetDatewiseRunningSizeDtls(DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = Constants.ServerDateTime.Date;

            return BL.TblRunningSizesBL.SelectAllTblRunningSizesList(stockDate);

        }

        [Route("IsStockUpdateConfirmed")]
        [HttpGet]
        public Boolean IsStockUpdateConfirmed(DateTime stockDate)
        {
            if (stockDate.Date == DateTime.MinValue)
                stockDate = Constants.ServerDateTime;

            TblStockSummaryTO tblStockSummaryTO = BL.TblStockSummaryBL.SelectTblStockSummaryTO(stockDate);
            if (tblStockSummaryTO != null && tblStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                return true;
            else return false;
        }

        [Route("GetLastUpdatedStockDate")]
        [HttpGet]
        public String GetLastUpdatedStockDate(Int32 compartmentId, Int32 prodCatId)
        {
            return BL.TblStockSummaryBL.SelectLastStockUpdatedDateTime(compartmentId, prodCatId);
        }


        [Route("GetStockAsPerBooks")]
        [HttpGet]
        public TblStockAsPerBooksTO GetStockAsPerBooks(DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = Constants.ServerDateTime.Date;

            return BL.TblStockAsPerBooksBL.SelectTblStockAsPerBooksTO(stockDate);
        }

        /// <summary>
        /// Sanjay [2017-05-03] To Get All the compartment whose stock for the given date is not taken
        /// </summary>
        /// <param name="stockDate"></param>
        /// <returns></returns>
        [Route("GetStockNotTakenCompartmentList")]
        [HttpGet]
        public List<TblLocationTO> GetStockNotTakenCompartmentList(DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = Constants.ServerDateTime.Date;

            return BL.TblLocationBL.SelectStkNotTakenCompartmentList(stockDate);
        }

        [Route("GetDashboardStockUpdateInfo")]
        [HttpGet]
        public SalesTrackerAPI.DashboardModels.StockUpdateInfo GetDashboardStockUpdateInfo(DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = Constants.ServerDateTime.Date;

            return BL.TblStockSummaryBL.SelectDashboardStockUpdateInfo(sysDate);
        }


        #endregion

        #region Post

        // POST api/values
        [Route("PostDailyStockUpdate")]
        [HttpPost]
        public ResultMessage PostDailyStockUpdate([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblStockSummaryTO stockSummaryTO = JsonConvert.DeserializeObject<TblStockSummaryTO>(data["stockSummaryTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (stockSummaryTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Object Found Null";
                    return returnMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (stockSummaryTO.StockDetailsTOList == null || stockSummaryTO.StockDetailsTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : StockDetailsTOList Found Null";
                    return returnMsg;
                }


                for (int i = 0; i < stockSummaryTO.StockDetailsTOList.Count; i++)
                {
                    stockSummaryTO.StockDetailsTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    stockSummaryTO.StockDetailsTOList[i].UpdatedBy = Convert.ToInt32(loginUserId);
                    stockSummaryTO.StockDetailsTOList[i].CreatedOn = Constants.ServerDateTime;
                    stockSummaryTO.StockDetailsTOList[i].UpdatedOn = Constants.ServerDateTime;
                }

                stockSummaryTO.CreatedOn = Constants.ServerDateTime;
                stockSummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = BL.TblStockSummaryBL.UpdateDailyStock(stockSummaryTO);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While DailyStockUpdate";
                return returnMsg;
            }
        }

        // POST api/values
        [Route("PostStockSummaryConfirmation")]
        [HttpPost]
        public ResultMessage PostStockSummaryConfirmation([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<SizeSpecWiseStockTO> sizeSpecWiseStockTOList = JsonConvert.DeserializeObject<List<SizeSpecWiseStockTO>>(data["sizeSpecWiseStockTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (sizeSpecWiseStockTOList == null || sizeSpecWiseStockTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : sizeSpecWiseStockTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = Constants.ServerDateTime;
                for (int i = 0; i < sizeSpecWiseStockTOList.Count; i++)
                {
                    sizeSpecWiseStockTOList[i].ConfirmedBy = Convert.ToInt32(loginUserId);
                    sizeSpecWiseStockTOList[i].ConfirmedOn = confirmedDate;
                }

                ResultMessage resMsg = BL.TblStockSummaryBL.ConfirmStockSummary(sizeSpecWiseStockTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostStockSummaryConfirmation";
                return returnMsg;
            }
        }

        [Route("PostDailyRunningSize")]
        [HttpPost]
        public ResultMessage PostDailyRunningSize([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<TblRunningSizesTO> runningSizesTOList = JsonConvert.DeserializeObject<List<TblRunningSizesTO>>(data["runningSizesTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (runningSizesTOList == null || runningSizesTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : sizeSpecWiseStockTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = Constants.ServerDateTime;
                DateTime stockDate = confirmedDate.Date;

                TblStockSummaryTO tblStockSummaryTO = BL.TblStockSummaryBL.SelectTblStockSummaryTO(stockDate);
                if (tblStockSummaryTO != null && tblStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Is Already confirmed for the Date " + stockDate.ToString(Constants.DefaultDateFormat);
                    return returnMsg;
                }

                for (int i = 0; i < runningSizesTOList.Count; i++)
                {
                    runningSizesTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    runningSizesTOList[i].CreatedOn = confirmedDate;
                }

                ResultMessage resMsg = BL.TblRunningSizesBL.SaveDailyRunningSizeInfo(runningSizesTOList, stockDate);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostDailyRunningSize";
                return returnMsg;
            }
        }

        [Route("PostStockUpList")]
        [HttpPost]
        public ResultMessage PostStockUpList([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblStockDetailsTO StockDetailsTO = JsonConvert.DeserializeObject<TblStockDetailsTO>(data["stockDetailTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (StockDetailsTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : sizeSpecWiseStockTOList Found Null";
                    return returnMsg;
                }


                int Result = 1;

                Result = BL.TblStockDetailsBL.SaveStockUp(StockDetailsTO,loginUserId);

                if (Result != 1)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Text = "Error While UpdateTblStockDetails";
                    returnMsg.Tag = StockDetailsTO;
                }
                else
                {
                    returnMsg.MessageType = ResultMessageE.Information;
                     returnMsg.Result = 1;
                    returnMsg.Text = "Stock Updated Successfully";
                    returnMsg.Tag = StockDetailsTO;
               
                }


                return returnMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostStockUpList";
                return returnMsg;
            }
        }

        [Route("PostStockAsPerBooks")]
        [HttpPost]
        public ResultMessage PostStockAsPerBooks([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblStockAsPerBooksTO stockAsPerBooksTO = JsonConvert.DeserializeObject<TblStockAsPerBooksTO>(data["stockAsPerBooksTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (stockAsPerBooksTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Object Found Null";
                    return returnMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                DateTime stockDate = Constants.ServerDateTime;

                TblStockSummaryTO tblStockSummaryTO = BL.TblStockSummaryBL.SelectTblStockSummaryTO(stockDate);
                if (tblStockSummaryTO == null || tblStockSummaryTO.ConfirmedOn == DateTime.MinValue)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Todays Stock Is Not Confirmed Yet.";
                    return returnMsg;
                }

                stockAsPerBooksTO.CreatedOn = stockDate;
                stockAsPerBooksTO.CreatedBy = Convert.ToInt32(loginUserId);
                stockAsPerBooksTO.IsConfirmed = 1;
                stockAsPerBooksTO.StockFactor = stockAsPerBooksTO.StockInMT / tblStockSummaryTO.TotalStock;
                return BL.TblStockAsPerBooksBL.SaveStockAsPerBooks(stockAsPerBooksTO);

            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostStockAsPerBooks";
                return returnMsg;
            }
        }

        [Route("RemoveRunningSizeDetails")]
        [HttpPost]
        public ResultMessage RemoveRunningSizeDetails([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblRunningSizesTO runningSizesTO = JsonConvert.DeserializeObject<TblRunningSizesTO>(data["runningSizesTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (runningSizesTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : runningSizesTO Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = Constants.ServerDateTime;
                DateTime stockDate = confirmedDate.Date;

                TblStockSummaryTO tblStockSummaryTO = BL.TblStockSummaryBL.SelectTblStockSummaryTO(stockDate);
                if (tblStockSummaryTO != null && tblStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Is Already confirmed for the Date " + stockDate.ToString(Constants.DefaultDateFormat);
                    return returnMsg;
                }

                ResultMessage resMsg = BL.TblRunningSizesBL.RemoveRunningSizeDtls(runningSizesTO, tblStockSummaryTO, Convert.ToInt32(loginUserId));
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostDailyRunningSize";
                return returnMsg;
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
