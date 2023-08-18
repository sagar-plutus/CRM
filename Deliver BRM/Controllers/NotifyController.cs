using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using SalesTrackerAPI.BL;
using SalesTrackerAPI.DAL;

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class NotifyController : Controller
    {
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
        public List<TblAlertUsersTO> GetAllActiveAlertList(Int32 userId, String userRoleTOList,Int32 loginId = 0, Int32 ModuleId = 0)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            return BL.TblAlertUsersBL.SelectAllActiveAlertList(userId, tblUserRoleTOList,loginId, ModuleId);
        }


        [Route("snoozeResetNotification")]
        [HttpGet]
        public ResultMessage PostSnooze(int alertInstanceId, int snoozeTime, int userId = 0, string deviceId = null)
        {


            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                if (userId == 0 && deviceId == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "specify deviceId or userId";
                    resultMessage.Result = -1;
                    return resultMessage;
                }
                int result = 0;
                List<TblAlertActionDtlTO> list = TblAlertActionDtlBL.SelectAllTblAlertActionDtlList();
                if (deviceId != null)
                {
                    DropDownTO userTO = TblUserDAO.SelectTblUserOnDeviceId(deviceId);
                    if (userTO == null)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "couldnt find user for DeviceId";
                        resultMessage.Result = -1;
                        return resultMessage;
                    }

                    userId = userTO.Value;
                }


                var chkAlert = list.Where(e => e.UserId == userId && e.AlertInstanceId == alertInstanceId).LastOrDefault();
                if (chkAlert == null)
                {
                    TblAlertActionDtlTO tblAlertActionDtlTO = new TblAlertActionDtlTO();
                    tblAlertActionDtlTO.AcknowledgedOn = Constants.ServerDateTime;
                    tblAlertActionDtlTO.AlertInstanceId = alertInstanceId;
                    tblAlertActionDtlTO.UserId = userId;
                    tblAlertActionDtlTO.SnoozeOn = Constants.ServerDateTime.AddMinutes(snoozeTime);
                    tblAlertActionDtlTO.SnoozeCount++;
                    result = TblAlertActionDtlBL.InsertTblAlertActionDtl(tblAlertActionDtlTO);
                }
                else
                {
                    chkAlert.SnoozeCount++;
                    chkAlert.SnoozeOn = Constants.ServerDateTime.AddMinutes(snoozeTime);
                    result = TblAlertActionDtlBL.UpdateTblAlertActionDtl(chkAlert);
                }
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;

                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
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
                    TblAlertActionDtlTO existingAlertActionDtlTO = BL.TblAlertActionDtlBL.SelectTblAlertActionDtlTO(alertUsersTO.AlertInstanceId, Convert.ToInt32(loginUserId));
                    if(existingAlertActionDtlTO!=null)
                    {
                        existingAlertActionDtlTO.ResetDate = Constants.ServerDateTime;
                        result = BL.TblAlertActionDtlBL.UpdateTblAlertActionDtl(existingAlertActionDtlTO);
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
                        tblAlertActionDtlTO.ResetDate= Constants.ServerDateTime;
                        goto xxx;
                    }

                }

                xxx:
                tblAlertActionDtlTO.UserId = Convert.ToInt32( loginUserId);
                tblAlertActionDtlTO.AcknowledgedOn = Constants.ServerDateTime;
                tblAlertActionDtlTO.AlertInstanceId = alertUsersTO.AlertInstanceId;
                result = BL.TblAlertActionDtlBL.InsertTblAlertActionDtl(tblAlertActionDtlTO);
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


        /// <summary>
        /// Kiran Mehetre [24-10-2018] : Send Notification Perticular UserId
        /// </summary>
        /// <returns></returns>
        [Route("SendNotificationByUserId")]
        [HttpGet]
        public ResultMessage GetAlertDefinationList(Int32 UserId, String Msg)
        {
            TblUserTO userTo = new TblUserTO();
            ResultMessage resultMessage = new ResultMessage();
            userTo = TblUserBL.SelectTblUserTO(UserId);
            string[] devices = new string[1];
            if (userTo.RegisteredDeviceId != null)
                {
                devices[0] = userTo.RegisteredDeviceId;
                String notifyBody = userTo.UserDisplayName + Environment.NewLine + Msg;
                String notifyTitle = "SimpliChat";
                VitplNotify.NotifyToRegisteredDevices(devices, notifyBody, notifyTitle,0);
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Acknowledged Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }else
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "User Registered Device Id not found";
                resultMessage.Result = -1;
                return resultMessage;
            }
        }



        /// <summary>
        /// Priyanka [27-09-2018] : Added to get the alert defination list
        /// </summary>
        /// <returns></returns>
        [Route("GetAlertDefinationList")]
        [HttpGet]
        public List<TblAlertDefinitionTO> GetAlertDefinationList()
        {
            return BL.TblAlertDefinitionBL.SelectAllTblAlertDefinitionList();
        }

        /// <summary>
        /// Priyanka [27-09-2018] : Added to get the alert Subscribers list
        /// </summary>
        /// <returns></returns>
        [Route("GetAlertSubscribersList")]
        [HttpGet]
        public List<TblAlertSubscribersTO> GetAlertSubscribersList(Int32 alertDefId)
        {
            return BL.TblAlertSubscribersBL.SelectTblAlertSubscribersByAlertDefId(alertDefId);
        }

        /// <summary>
        /// Priyanka [27-09-2018] : Added to post the new alert Subscription setting
        /// </summary>
        /// <returns></returns>
        [Route("PostAlertSubcriptionSettings")]
        [HttpPost]
        public ResultMessage PostAlertSubcriptionSettings([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO = JsonConvert.DeserializeObject<TblAlertSubscriptSettingsTO>(data["alertSubscriptSettingTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblAlertSubscriptSettingsTO == null)
                {
                    resultMessage.DefaultBehaviour("tblAlertSubscriptSettingsTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                //tblAlertSubscriptSettingsTO.UpdatedOn = Constants.ServerDateTime;
                //tblAlertSubscriptSettingsTO.UpdatedBy = Convert.ToInt32(loginUserId);
               

                TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTONew = BL.TblAlertSubscriptSettingsBL.SelectTblAlertSubscriptSettingsFromNotifyId(tblAlertSubscriptSettingsTO.NotificationTypeId, tblAlertSubscriptSettingsTO.SubscriptionId, tblAlertSubscriptSettingsTO.AlertDefId);
                if (tblAlertSubscriptSettingsTONew != null)
                {
                    tblAlertSubscriptSettingsTONew.IsActive = 0;
                    tblAlertSubscriptSettingsTONew.IdSubscriSettings = tblAlertSubscriptSettingsTONew.IdSubscriSettings;
                    tblAlertSubscriptSettingsTONew.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblAlertSubscriptSettingsTONew.UpdatedOn = Constants.ServerDateTime;
                    int result1 = BL.TblAlertSubscriptSettingsBL.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTONew);
                    if (result1 != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be saved");
                        return resultMessage;
                    }
                }
              
                tblAlertSubscriptSettingsTO.CreatedOn = Constants.ServerDateTime;
                int result = BL.TblAlertSubscriptSettingsBL.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostEmailConfigurationDetails");
                return resultMessage;
            }
        }

        /// <summary>
        /// Priyanka [27-09-2018] : Added to post new role or user in subscibers list.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostNewRoleOrUserForSubscribers")]
        [HttpPost]
        public ResultMessage PostNewRoleOrUserForSubscribers([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblAlertSubscribersTO tblAlertSubscribersTO = JsonConvert.DeserializeObject<TblAlertSubscribersTO>(data["alertSubscribersToNew"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblAlertSubscribersTO == null)
                {
                    resultMessage.DefaultBehaviour("tblAlertSubscribersTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                tblAlertSubscribersTO.SubscribedOn = Constants.ServerDateTime;

                int result = BL.TblAlertSubscribersBL.InsertTblAlertSubscribers(tblAlertSubscribersTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostEmailConfigurationDetails");
                return resultMessage;
            }
        }

        /// <summary>
        /// Priyanka [27-09-2018] : Added to update the alert subscribers.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostUpdateAlertSubcribers")]
        [HttpPost]
        public ResultMessage PostUpdateAlertSubcribers([FromBody] JObject data)
        {

            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblAlertSubscribersTO tblAlertSubscribersTO = JsonConvert.DeserializeObject<TblAlertSubscribersTO>(data["alertSubscribersTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblAlertSubscribersTO == null)
                {
                    resultMessage.DefaultBehaviour("tblAlertSubscribersTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }


                tblAlertSubscribersTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //tblAlertSubscribersTO.SubscribedOn = Constants.ServerDateTime;
                tblAlertSubscribersTO.UpdatedOn = Constants.ServerDateTime;

                return BL.TblAlertSubscribersBL.UpdateAlertSubscribers(tblAlertSubscribersTO);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEmailConfigurationDetails");
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
                //Int32 roleId = Convert.ToInt32(data["roleId"].ToString());
                List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(data["userRoleList"].ToString());

                List<TblAlertUsersTO> list = BL.TblAlertUsersBL.SelectAllActiveAlertList(loginUserId, tblUserRoleTOList,0,0);

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
                return BL.TblAlertActionDtlBL.ResetAllAlerts(loginUserId, list, result);

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

         //TO send android Snooze notifications
        [Route("postSnoozeAndroidNotification")]
        [HttpGet]
        public void PostSnoozeAndroidNotification()
        {
            TblAlertInstanceBL.postSnoozeForAndroid();
        }

        [Route("PostAutoResetAndDeleteAlerts")]
        [HttpGet]
        public ResultMessage PostAutoResetAndDeleteAlerts()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                return BL.TblAlertInstanceBL.AutoResetAndDeleteAlerts();

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

        /// <summary>
        /// Sudhir --Added for Save New Alert.
        /// </summary>
        /// <param name="tblAlertInstanceTO"></param>
        /// <returns></returns>
        [Route("PostNewAlert")]
        [HttpPost]
        public ResultMessage PostNewAlert([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblAlertInstanceTO tblAlertInstanceTO = JsonConvert.DeserializeObject<TblAlertInstanceTO>(data["tblAlertInstanceTO"].ToString());

                if (tblAlertInstanceTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblAlertInstanceTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tblAlertInstanceTO.RaisedOn = Constants.ServerDateTime;
                //tblAlertInstanceTO.RaisedBy= Convert.ToInt32(loginUserId);

                return BL.TblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);

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

        /// <summary>
        /// Deepali --Added for Reset Old Alerts.
        /// </summary>
        /// <param name="tblAlertInstanceTO"></param>
        /// <returns></returns>
        [Route("PostResetOldAlerts")]
        [HttpPost]
        public ResultMessage PostResetOldAlerts([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblAlertInstanceTO tblAlertInstanceTO = JsonConvert.DeserializeObject<TblAlertInstanceTO>(data["tblAlertInstanceTO"].ToString());

                if (tblAlertInstanceTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblAlertInstanceTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tblAlertInstanceTO.RaisedOn = Constants.ServerDateTime;
                BL.TblAlertInstanceBL.ResetOldAlerts(tblAlertInstanceTO);
                return resultMessage;

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
