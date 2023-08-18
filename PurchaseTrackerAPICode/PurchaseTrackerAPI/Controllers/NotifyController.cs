using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL;

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class NotifyController : Controller
    {

        private readonly ITblAlertActionDtlBL _iTblAlertActionDtlBL;
        private readonly ITblAlertUsersBL _iTblAlertUsersBL;
        private readonly Icommondao _iCommonDAO;
        private readonly  Itblalertinstancebl _iTblAlertInstanceBL;
        public NotifyController(ITblAlertUsersBL iTblAlertUsersBL, Icommondao icommondao, ITblAlertActionDtlBL iTblAlertActionDtlBL, Itblalertinstancebl iTblAlertInstanceBL
            )
        {
            _iCommonDAO = icommondao;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iTblAlertActionDtlBL = iTblAlertActionDtlBL;
            _iTblAlertUsersBL = iTblAlertUsersBL;
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

        [Route("GetAllActiveAlertList")]
        [HttpGet]
        public List<TblAlertUsersTO> GetAllActiveAlertList(Int32 userId,Int32 roleId)
        {
            return _iTblAlertUsersBL.SelectAllActiveAlertList(userId, roleId);
        }

        [Route("PostAlertAcknowledgement")]
        [HttpPost]
        public ResultMessage PostAlertAcknowledgement([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblAlertUsersTO alertUsersTO = JsonConvert.DeserializeObject<TblAlertUsersTO>(data["alertUsersTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (alertUsersTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                int result = 0;
                TblAlertActionDtlTO tblAlertActionDtlTO = new TblAlertActionDtlTO();

                if (alertUsersTO.IsReseted==1)
                {
                    //Check For Existence
                    TblAlertActionDtlTO existingAlertActionDtlTO = _iTblAlertActionDtlBL.SelectTblAlertActionDtlTO(alertUsersTO.AlertInstanceId, Convert.ToInt32(loginUserId));
                    if(existingAlertActionDtlTO!=null)
                    {
                        existingAlertActionDtlTO.ResetDate =  _iCommonDAO.ServerDateTime;
                        result = _iTblAlertActionDtlBL.UpdateTblAlertActionDtl(existingAlertActionDtlTO);
                        if (result == 1)
                        {
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Text = "Alert Resetted Sucessfully";
                            resultMessage.Result = 1;
                            return resultMessage;
                        }
                        else
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Alert Acknowledgement/Reset";
                            resultMessage.Result = 0;
                            return resultMessage;
                        }

                    }
                    else
                    {
                        tblAlertActionDtlTO.ResetDate=  _iCommonDAO.ServerDateTime;
                        goto xxx;
                    }

                }

                xxx:
                tblAlertActionDtlTO.UserId = Convert.ToInt32( loginUserId);
                tblAlertActionDtlTO.AcknowledgedOn =  _iCommonDAO.ServerDateTime;
                tblAlertActionDtlTO.AlertInstanceId = alertUsersTO.AlertInstanceId;
                result = _iTblAlertActionDtlBL.InsertTblAlertActionDtl(tblAlertActionDtlTO);
                if (result == 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Alert Acknowledged Sucessfully";
                    resultMessage.Result = 1;
                    return resultMessage;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Alert Acknowledgement";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostAlertAcknowledgement";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        [Route("PostResetAllAlerts")]
        [HttpPost]
        public ResultMessage PostResetAllAlerts([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                Int32 roleId = Convert.ToInt32(data["roleId"].ToString());

                List<TblAlertUsersTO> list = _iTblAlertUsersBL.SelectAllActiveAlertList(loginUserId, roleId);

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (list == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "list Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                int result = 0;
                TblAlertActionDtlTO tblAlertActionDtlTO = new TblAlertActionDtlTO();
                return _iTblAlertActionDtlBL.ResetAllAlerts(loginUserId, list, result);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostResetAllAlerts";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("PostAutoResetAndDeleteAlerts")]
        [HttpGet]
        public ResultMessage PostAutoResetAndDeleteAlerts()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                return _iTblAlertInstanceBL.AutoResetAndDeleteAlerts();

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception In Method PostAutoResetAndDeleteAlerts";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
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
    }
}
