using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseInvoiceController : Controller
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblPurchaseInvoiceBL _iTblPurchaseInvoiceBL;
        public PurchaseInvoiceController(ITblPurchaseInvoiceBL itblPurchaseInvoiceBL, Icommondao icommondao) {
            _iTblPurchaseInvoiceBL = itblPurchaseInvoiceBL;
            _iCommonDAO = icommondao;
        }
        [Route("GetPurchaseInvoiceAgainstSchedule")]
        [HttpGet]
        public List<TblPurchaseInvoiceTO> GetPurchaseInvoiceAgainstSchedule(Int32 rootPurchaseSchId)
        {
            return _iTblPurchaseInvoiceBL.SelectAllTblPurchaseInvoiceListAgainstSchedule(rootPurchaseSchId);
        }

        [Route("GetPurchaseInvoiceAgainstScheduleWithDtls")]
        [HttpGet]
        public TblPurchaseInvoiceTO GetPurchaseInvoiceAgainstScheduleWithDtls(Int32 rootPurchaseSchId)
        {
            return _iTblPurchaseInvoiceBL.GetPurchaseInvoiceAgainstScheduleWithDtls(rootPurchaseSchId);
        }



        [Route("GetPurchaseInvoiceDetails")]
        [HttpGet]
        public TblPurchaseInvoiceTO GetPurchaseInvoiceDetails(Int32 purchaseInvoiceId)
        {
            return _iTblPurchaseInvoiceBL.SelectTblPurchaseInvoiceTOWithDetails(purchaseInvoiceId);
        }


        [Route("PostPurchaseInvoice")]
        [HttpPost]
        public ResultMessage PostPurchaseInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseInvoiceTO tblPurchaseInvoiceTO = JsonConvert.DeserializeObject<TblPurchaseInvoiceTO>(data["purchaseInvoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    tblPurchaseInvoiceTO.CreatedBy = Convert.ToInt32(loginUserId);
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }
                if (tblPurchaseInvoiceTO != null)
                {
                    DateTime serverDate =  _iCommonDAO.ServerDateTime;
                    tblPurchaseInvoiceTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblPurchaseInvoiceTO.CreatedOn = serverDate;
                    //tblPurchaseInvoiceTO.InvoiceDate = serverDate;
                    tblPurchaseInvoiceTO.StatusDate = serverDate;
                    if (tblPurchaseInvoiceTO.StatusId == 0)
                    {
                        tblPurchaseInvoiceTO.StatusId = (Int32)Constants.InvoiceStatusE.NEW;
                    }
                    //tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.MANUAL_INVOICE;

                    // tblInvoiceTO.IsActive = 1;

                    //tblPurchaseInvoiceTO.DeliveredOn = tblInvoiceTO.StatusDate;//viaymala added
                    return _iTblPurchaseInvoiceBL.InsertPurchaseInvoice(tblPurchaseInvoiceTO);
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


        [Route("PostEditPurchaseInvoice")]
        [HttpPost]
        public ResultMessage PostEditPurchaseInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseInvoiceTO tblPurchaseInvoiceTO = JsonConvert.DeserializeObject<TblPurchaseInvoiceTO>(data["purchaseInvoiceTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <=  0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
               
                if (tblPurchaseInvoiceTO != null)
                {
                    DateTime serverDate =  _iCommonDAO.ServerDateTime;
                    tblPurchaseInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblPurchaseInvoiceTO.UpdatedOn = serverDate;

                    return _iTblPurchaseInvoiceBL.SaveUpdatedPurchaseInvoice(tblPurchaseInvoiceTO);
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

        //[Route("CreatePurchaseInvoicePOWithGRR")]
        //[HttpPost]
        //public ResultMessage CreatePurchaseInvoicePOWithGRR()
        //{
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    try
        //    {
        //        Int32 rootScheduleId = 78130;
        //        return _iTblPurchaseInvoiceBL.CreatePurchaseInvoicePOWithGRR(rootScheduleId);

        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoice");
        //        return resultMessage;
        //    }

        //}



    }
}