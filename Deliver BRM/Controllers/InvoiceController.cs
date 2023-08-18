using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static SalesTrackerAPI.StaticStuff.Constants;
using SalesTrackerAPI.BL;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {

        private readonly ILogger loggerObj;
        public InvoiceController(ILogger<InvoiceController> logger)
        {
            loggerObj = logger;
            Constants.LoggerObj = logger;
        }
        //Aditee02042020
        [HttpGet]
        [Route("SelectAllInvoiceAddrById")]
        public List<TblInvoiceAddressTO> SelectAllInvoiceAddrById(Int32 dealerId, String addrSrcTypeString)
        {
            return TblInvoiceBL.SelectTblInvoiceAddressByDealerId(dealerId, addrSrcTypeString);
        }
        /// <summary>
        /// Ramdas.w @24102017 this method is  Get Generated Invoice List
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isConfirm"></param>
        /// <param name="cnfId"></param>
        /// <param name="dealerID"></param>
        /// <param name="userRoleTO"></param>
        /// <returns></returns>
        [Route("GetInvoiceList")]
        [HttpGet]
        public List<TblInvoiceTO> GetInvoiceList(string fromDate, string toDate, int isConfirm, Int32 cnfId, Int32 dealerID, String userRoleTOList, string selectedOrg)
        {
            try
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
                return BL.TblInvoiceBL.SelectAllTblInvoiceList(frmDt, toDt, isConfirm, cnfId, dealerID, tblUserRoleTOList, selectedOrg);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }


        [Route("GetInvoiceDetails")]
        [HttpGet]
        public TblInvoiceTO GetInvoiceDetails(Int32 invoiceId,Int32  IsExtractionAllowed=0)
        {
            Constants.writeLog("GetInvoiceDetails : For Id - " + invoiceId + " Main START ");
            TblInvoiceTO tblInvoiceTO= BL.TblInvoiceBL.GetInvoiceDetails(invoiceId,IsExtractionAllowed);
            Constants.writeLog("GetInvoiceDetails : For Id - " + invoiceId + " Main END " );
            return tblInvoiceTO;
        }


        /// <summary>
        /// Ramdas.W:@22092017:API This method is used to Get List of Invoice By Status
        /// </summary>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        [Route("GetInvoiceListByStatus")]
        [HttpGet]
        public List<TblInvoiceTO> GetInvoiceListByStatus(int statusId, int distributorOrgId)
        {
            try
            {
                return BL.TblInvoiceBL.SelectTblInvoiceByStatus(statusId, distributorOrgId);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }

        }

        /// <summary>
        /// Vijaymala[15-09-2017] Added To Get Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>

        [Route("GetRptInvoiceList")]
        [HttpGet]
        public List<TblInvoiceRptTO> GetRptInvoiceList(string fromDate, string toDate, int isConfirm,string strOrgTempId)
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
            if (string.IsNullOrEmpty(strOrgTempId))
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    strOrgTempId = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }
            }
            return BL.TblInvoiceBL.SelectAllRptInvoiceList(frmDt, toDt, isConfirm, strOrgTempId);
        }

        

        /// <summary>
        /// Vijaymala[06-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>

        [Route("GetInvoiceExportList")]
        [HttpGet]
        public List<TblInvoiceRptTO> GetInvoiceExportList(string fromDate, string toDate, int isConfirm,string strOrgTempId)
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

            if (string.IsNullOrEmpty(strOrgTempId))
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    strOrgTempId = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }
            }
            return BL.TblInvoiceBL.SelectInvoiceExportList(frmDt, toDt, isConfirm,strOrgTempId);
        }

        /// <summary>
        /// Vijaymala[07-10-2017] Added To Get Invoice List To Generate HSN Excel
        /// </summary>
        /// <returns></returns>

        [Route("GetHsnExportList")]
        [HttpGet]
        public List<TblInvoiceRptTO> GetHsnExportList(string fromDate, string toDate, int isConfirm, string strOrgTempId)
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
            if (string.IsNullOrEmpty(strOrgTempId))
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    strOrgTempId = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }
            }
            return BL.TblInvoiceBL.SelectHsnExportList(frmDt, toDt, isConfirm, strOrgTempId);
        }

        /// <summary>
        /// Vijaymala[11-01-2018] Added To Get Sales Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>

        [Route("GetSalesInvoiceListForReport")]
        [HttpGet]
        public List<TblInvoiceRptTO> GetSalesInvoiceListForReport(string fromDate, string toDate, int isConfirm,string selectedOrg, int isManual)
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
            if (string.IsNullOrEmpty(selectedOrg))
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    selectedOrg = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }
            }
            return BL.TblInvoiceBL.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm, selectedOrg, isManual);
        }

        //Deepali Added [07-06-2021] for task no 1145
        [Route("PrintSaleReport")]
        [HttpGet]
        public ResultMessage PrintSaleReport(string fromDate, string toDate, int isConfirm, string selectedOrg,int isFromPurchase)
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
            if (string.IsNullOrEmpty(selectedOrg))
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    selectedOrg = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }
            }
            return BL.TblInvoiceBL.PrintSaleReport(frmDt, toDt, isConfirm, selectedOrg, isFromPurchase);
        }
        /// <summary>
        /// Priyanka [26-03-2018] : Added to get the other item list of Invoice Report.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isConfirm"></param>
        /// <param name="otherTaxId"></param>
        /// <returns></returns>

        [Route("GetOtherItemListForReport")]
        [HttpGet]
        public List<TblOtherTaxRpt> GetOtherItemListForReport(string fromDate, string toDate, int isConfirm, int otherTaxId,string strOrgTempId)
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
            if (string.IsNullOrEmpty(strOrgTempId))
            {
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    strOrgTempId = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }
            }
            return BL.TblInvoiceBL.SelectOtherTaxDetailsReport(frmDt, toDt, isConfirm, otherTaxId,strOrgTempId);
        }



        /// <summary>
        /// Sudhir Added For get InvoiceTO based on loadingSlipId
        /// </summary>
        /// <param name="loadingSlipId"></param>
        /// <returns></returns>
        [Route("GetInvoiceDetailsByLoadingSlipId")]
        [HttpGet]
        public List<TblInvoiceTO> GetInvoiceDetailsByLoadingSlipId(Int32 loadingSlipId)
        {
            return BL.TblInvoiceBL.SelectInvoiceTOListFromLoadingSlipId(loadingSlipId);
        }

        /// <summary>
        /// Vijaymala added[09-05-2018] :To get notified invoice list
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isConfirm"></param>
        /// <returns></returns>

        [Route("GetAllTNotifiedblInvoiceList")]
        [HttpGet]
        public List<TblInvoiceTO> GetAllTNotifiedblInvoiceList(string fromDate, string toDate, int isConfirm,string strOrgTempId)
        {
            try
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
                if (string.IsNullOrEmpty(strOrgTempId))
                {
                    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (tblConfigParamsTO != null)
                    {
                        strOrgTempId = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                    }
                }
                return BL.TblInvoiceBL.SelectAllTNotifiedblInvoiceList(frmDt, toDt, isConfirm, strOrgTempId);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Vijaymala added[23-05-2018] :To get  invoice document list
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isConfirm"></param>
        /// <returns></returns>

        [Route("GetInvoiceDocumentDetails")]
        [HttpGet]
        public List<TempInvoiceDocumentDetailsTO> GetInvoiceDocumentDetails(Int32 invoiceId)
        {
            try
            {
                    return BL.TempInvoiceDocumentDetailsBL.SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(invoiceId);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        [Route("GetInvoiceAddressList")]
        [HttpGet]
        public List<TblInvoiceAddressTO> GetInvoiceAddressList(Int32 invoiceId)
        {
            try
            {
                List<TblInvoiceAddressTO> invoiceAddressTOList = new List<TblInvoiceAddressTO>();

                if (invoiceId != null)
                {
                    invoiceAddressTOList = TblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId);
                }
                return invoiceAddressTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Ramdas.W:14092017:API This method is used to Added new Invoice 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostInvoice")]
        [HttpPost]
        public ResultMessage PostInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    tblInvoiceTO.CreatedBy = Convert.ToInt32(loginUserId);
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }
                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    tblInvoiceTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.CreatedOn = serverDate;
                    tblInvoiceTO.InvoiceDate = serverDate;
                    tblInvoiceTO.StatusDate = serverDate;
                    tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.NEW;
                    //tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.MANUAL_INVOICE;

                    // tblInvoiceTO.IsActive = 1;

                    tblInvoiceTO.DeliveredOn = tblInvoiceTO.StatusDate;//viaymala added
                    return BL.TblInvoiceBL.InsertTblInvoice(tblInvoiceTO);
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Invoice ";
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostNewInvoice";
                return resultMessage;
            }


        }

       


        [Route("PostEditInvoice")]
        [HttpPost]
        public ResultMessage PostEditInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.UpdatedOn = serverDate;

                    return BL.TblInvoiceBL.SaveUpdatedInvoice(tblInvoiceTO);
                }

                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoice");
                return resultMessage;
            }

        }


        [Route("PostexchangeInvoice")]
        [HttpPost]
        public ResultMessage exchangeInvoice(int invoiceId, int invGenerateModeId, int fromOrgId, int toOrgId = 0, int isCalculateWithBaseRate = 0)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return TblInvoiceBL.exchangeInvoice(invoiceId, invGenerateModeId, fromOrgId, toOrgId, isCalculateWithBaseRate);
            }
            catch (Exception e)
            {
                resultMessage.DefaultExceptionBehaviour(e, "exchangeIvoice");
                return resultMessage;
            }
        }

        [Route("PostUpdateInvoiceStatus")]
        [HttpPost]
        public ResultMessage PostUpdateInvoiceStatus([FromBody] TblInvoiceTO tblInvoiceTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return TblInvoiceBL.PostUpdateInvoiceStatus(tblInvoiceTO);
            }
            catch (Exception e)
            {
                resultMessage.DefaultExceptionBehaviour(e, "PostUpdateInvoiceStatus");
                return resultMessage;
            }
        }

        [Route("PostGenerateInvoiceNumber")]
        [HttpPost]
        public ResultMessage PostGenerateInvoiceNumber([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var invoiceId = data["invoiceId"].ToString();
                var invGenerateModeId = data["invGenerateModeId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (Convert.ToInt32(invoiceId) <= 0)
                {
                    resultMessage.DefaultBehaviour("invoiceId Not Found");
                    return resultMessage;
                }
               Int32 isConfirm = 1;
                return BL.TblInvoiceBL.GenerateInvoiceNumber(Convert.ToInt32(invoiceId), Convert.ToInt32(loginUserId), isConfirm, Convert.ToInt32(invGenerateModeId));
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostGenerateInvoiceNumber");
                return resultMessage;
            }
        }


        [Route("PostEditInvoiceForNonCommercialDtls")]
        [HttpPost]
        public ResultMessage PostEditInvoiceForNonCommercialDtls([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.UpdatedOn = serverDate;

                    return BL.TblInvoiceBL.UpdateInvoiceNonCommercialDetails(tblInvoiceTO);
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoiceForNonCommercialDtls");
                return resultMessage;
            }

        }

        /// <summary>
        /// GJ@20171001 : Post Edit ConfirmNonConfirm status with calculation
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("PostUpdateInvoiceConfirmNonConfirmDetails")]
        [HttpPost]
        public ResultMessage PostUpdateInvoiceConfirmNonConfirmDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.UpdatedOn = serverDate;
                    
                    return BL.TblInvoiceBL.UpdateInvoiceConfrimNonConfirmDetails(tblInvoiceTO, tblInvoiceTO.UpdatedBy);
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoiceForStatusConversion");
                return resultMessage;
            }
        }


        /// <summary>
        /// Vijaymala[22-05-2018] : Added To save invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("PostInvoiceDocumentDetails")]
        [HttpPost]
        public ResultMessage PostInvoiceDocumentDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                List<TblDocumentDetailsTO> tblDocumentDetailsTOList = JsonConvert.DeserializeObject <List<TblDocumentDetailsTO>>(data["invoiceDocumentDetailsTOList"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblDocumentDetailsTOList == null && tblDocumentDetailsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Error : Invoice Document Details List Found Empty Or Null");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    return BL.TblInvoiceBL.SaveInvoiceDocumentDetails(tblInvoiceTO, tblDocumentDetailsTOList, Convert.ToInt32(loginUserId));
                }

                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostInvoiceDocumentDetails");
                return resultMessage;
            }

        }

        /// <summary>
        /// Vijaymala[22-05-2018] : Added To deacivate invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("PostDeactivateInvoiceDocumentDetails")]
        [HttpPost]
        public ResultMessage PostDeactivateInvoiceDocumentDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO = JsonConvert.DeserializeObject<TempInvoiceDocumentDetailsTO>(data["invoiceDocumentDetailsTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tempInvoiceDocumentDetailsTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    return BL.TblInvoiceBL.DeactivateInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO,  Convert.ToInt32(loginUserId));
                }

                else
                {
                    resultMessage.DefaultBehaviour("tempInvoiceDocumentDetailsTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateInvoiceDocumentDetails");
                return resultMessage;
            }

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


        #region Post

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {


        }

        /// <summary>
        /// Vaibhav [29-Nov-2017] Added to separate transactional data 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostExtractEnquiryData")]
        [HttpPost]
        public ResultMessage PostExtractEnquiryData()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
              return BL.TblInvoiceBL.ExtractEnquiryData();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostExtractEnquiryData");
                return resultMessage;
            }
        }

        //Added by minal 04 June 2021
        [Route("CreateAndBackupExcelFile")]
        [HttpGet]
        public ResultMessage CreateAndBackupExcelFile()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                return BL.TblInvoiceBL.CreateAndBackupExcelFile();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostExtractEnquiryData");
                return resultMessage;
            }
        }
        //Added by minal
        #endregion



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
        
        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        [Route("GenerateEInvoice")]
        [HttpPost]
        public ResultMessage GenerateEInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                Int32 eInvoiceCreationType = Convert.ToInt32(data["generateEInvoiceTypeE"].ToString());
                //Int32 idInvoice = 29194;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                if (eInvoiceCreationType == 0)
                {
                    resultMessage.DefaultBehaviour("E-Invoice Creation type not found");
                    return resultMessage;
                }
                List<TblInvoiceAddressTO> tblInvoiceAddressTOList = JsonConvert.DeserializeObject<List<TblInvoiceAddressTO>>(data["invoiceAddressTOList"].ToString());
                resultMessage = TblInvoiceBL.UpdateInvoiceAddress(tblInvoiceAddressTOList);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                if (eInvoiceCreationType == (Int32)Constants.EGenerateEInvoiceCreationType.UPDATE_ONLY_ADDRESS)
                {
                    return resultMessage;
                }

                return TblInvoiceBL.GenerateEInvoice(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice), Convert.ToInt32(eInvoiceCreationType));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[19-11-2020] : Added To Cancel eInvvoice.
        /// </summary>
        [Route("CancelEInvoice")]
        [HttpPost]
        public ResultMessage CancelEInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                //Int32 idInvoice = 29194;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                return TblInvoiceBL.CancelEInvoice(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[01-03-2021] : Added To Get And Update eInvvoice.
        /// </summary>
        [Route("GetAndUpdateEInvoice")]
        [HttpPost]
        public ResultMessage GetAndUpdateEInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                return TblInvoiceBL.GetAndUpdateEInvoice(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate EWayBill.
        /// </summary>
        [Route("GenerateEWayBill")]
        [HttpPost]
        public ResultMessage GenerateEWayBill([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                var distanceInKM = data["distanceInKM"].ToString();
                //Int32 idInvoice = 29194;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                return TblInvoiceBL.GenerateEWayBill(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice), Convert.ToDecimal(distanceInKM));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[19-11-2020] : Added To Cancel EWayBill.
        /// </summary>
        [Route("CancelEWayBill")]
        [HttpPost]
        public ResultMessage CancelEWayBill([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                //Int32 idInvoice = 29194;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                return TblInvoiceBL.CancelEWayBill(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

    }
}
