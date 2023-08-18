using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.BL;

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class SupportController : Controller
    {
        #region GET
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

        [Route("GetAllInvoiceModes")]
        [HttpGet]
        public List<DropDownTO> GetAllInvoiceModes()
        {
            List<DropDownTO> invoiceModeslist = BL.DimensionBL.SelectInvoiceModeForDropDown();
            if (invoiceModeslist != null && invoiceModeslist.Count > 0)
            {
                return invoiceModeslist;
            }
            else
                return null;
        }

        [Route("GetAllInvoiceStatus")]
        [HttpGet]
        public List<DropDownTO> GetAllInvoiceStatus()
        {
            List<DropDownTO> invoiceStatuslist = BL.DimensionBL.GetInvoiceStatusDropDown();
            if (invoiceStatuslist != null && invoiceStatuslist.Count > 0)
            {
                return invoiceStatuslist;
            }
            else
                return null;
        }

        #endregion

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// This method is for Update Invoice Mode.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateInvoiceForSupport")]
        [HttpPost]
        public ResultMessage PostUpdateInvoiceForSupport([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                Int32 FromUser = 0;
                var loginUserId = data["loginUserId"].ToString();
                var fromUser = data["fromUser"].ToString();
                var Comments = data["comments"].ToString();
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
                    FromUser = Convert.ToInt32(fromUser);
                    return TblSupportDetailsBL.UpdateInvoiceForSupport(tblInvoiceTO, FromUser, Comments.ToString());
                    //return BL.TblInvoiceBL.UpdateInvoiceConfrimNonConfirmDetails(tblInvoiceTO, tblInvoiceTO.UpdatedBy);
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
        /// This Method is For Deleting WeighingMeasures 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeleteWeighingMeasureForSupport")]
        [HttpPost]
        public ResultMessage PostDeleteWeighingMeasureForSupport([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblWeighingMeasuresTO weighingMeasuresTO = JsonConvert.DeserializeObject<TblWeighingMeasuresTO>(data["weighingMeasureTo"].ToString());
                Int32 FromUser = 0;
                var loginUserId = data["loginUserId"].ToString();
                var fromUser = data["fromUser"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (weighingMeasuresTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    weighingMeasuresTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    weighingMeasuresTO.UpdatedOn = serverDate;
                    FromUser = Convert.ToInt32(fromUser);
                    return TblSupportDetailsBL.PostDeleteWeighingMeasureForSupport(weighingMeasuresTO, FromUser);
                    //return new ResultMessage();
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
        /// For posting weight using api call
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostTblWeighingTO")]
        [HttpPost]
        public ResultMessage PostTblWeighingTO(String data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                if (!String.IsNullOrEmpty(data))
                {

                    List<String> strlist = data.Split("*#*#*").ToList();

                    if (strlist != null && strlist.Count == 2)
                    {
                        TblWeighingTO tblWeighingTO = new TblWeighingTO();
                        tblWeighingTO.Measurement = strlist[0];
                        tblWeighingTO.MachineIp = strlist[1];
                        tblWeighingTO.TimeStamp = Constants.ServerDateTime;

                        Int32 result = TblWeighingBL.DeleteTblWeighingByByMachineIp(tblWeighingTO.MachineIp);


                        result = TblWeighingBL.InsertTblWeighing(tblWeighingTO);
                        if (result == 1)
                        {
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Text = "Success";
                            resultMessage.DisplayMessage = "Success";
                            resultMessage.Result = 1;
                        }
                        else
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error";
                            resultMessage.DisplayMessage = "Error";
                            resultMessage.Result = -1;
                        }

                    }

                }

                //TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                //Int32 FromUser = 0;
                //var loginUserId = data["loginUserId"].ToString();
                //var fromUser = data["fromUser"].ToString();
                //var Comments = data["comments"].ToString();
                //if (Convert.ToInt32(loginUserId) <= 0)
                //{
                //    resultMessage.DefaultBehaviour("loginUserId Not Found");
                //    return resultMessage;
                //}

                //if (tblInvoiceTO != null)
                //{
                //    DateTime serverDate = Constants.ServerDateTime;
                //    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //    tblInvoiceTO.UpdatedOn = serverDate;
                //    FromUser = Convert.ToInt32(fromUser);
                //    return TblSupportDetailsBL.UpdateInvoiceForSupport(tblInvoiceTO, FromUser, Comments.ToString());
                //    //return BL.TblInvoiceBL.UpdateInvoiceConfrimNonConfirmDetails(tblInvoiceTO, tblInvoiceTO.UpdatedBy);
                //}
                //else
                //{
                //    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                //    return resultMessage;
                //}
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoiceForStatusConversion");
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


        #region  IOT support
        [Route("GetModbusRefId")]
        [HttpGet]
        public IoTDetialsTo GetModbusRefId(ModbusTO modbusTo)
        {
            IoTDetialsTo ioTDetialsTo = new IoTDetialsTo();
            if (modbusTo == null)
            {
                return null;
            }
            int modbusRefId =  DimensionBL.GetModbusRefId(modbusTo);
           
            if (modbusRefId != 0)
            {
                DropDownTO dropDownTO = DimensionBL.GetPortNumberUsingModRef(modbusRefId);
                if (dropDownTO != null)
                {
                    ioTDetialsTo = BL.TblSupportDetailsBL.SelectInformationFormIoT(modbusRefId, dropDownTO.Value);
                }
                ioTDetialsTo.DropDownTO = dropDownTO;
            }
            ioTDetialsTo.ApiUrl = Startup.GateIotApiURL;
            return ioTDetialsTo;
        }
        #endregion
    }
}