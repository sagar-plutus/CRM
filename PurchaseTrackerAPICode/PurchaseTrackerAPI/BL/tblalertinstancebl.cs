using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblAlertInstanceBL : Itblalertinstancebl
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblAlertDefinitionBL _itblAlertDefinitionBL;
        private readonly ITblAlertUsersBL _iTblAlertUsersBL;
        private readonly ITblAlertInstanceDAO _itblAlertInstanceDAO;
        private readonly ITblAlertEscalationSettingsBL _iTblAlertEscalationSettingsBL;
        private readonly ITblUserBL _iTblUserBL;
        private readonly Ivitplnotify _iVitplNotify;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly Ivitplsms _iVitplSMS;
        readonly private ITblSmsBL _iTblSmsBL;
        public TblAlertInstanceBL(ITblAlertInstanceDAO itblAlertInstanceDAO
                                   , IConnectionString iConnectionString
                                   , ITblAlertDefinitionBL itblAlertDefinitionBL
                                  , ITblAlertEscalationSettingsBL iTblAlertEscalationSettingsBL
                                  , ITblAlertUsersBL iTblAlertUsersBL
                                  , ITblUserBL iTblUserBL
                                  , Ivitplnotify iVitplNotify
                                  , ITblConfigParamsBL iTblConfigParamsBL
                                  , Ivitplsms iVitplSMS
                                  , ITblSmsBL iTblSmsBL
                                  ) {
            _iConnectionString = iConnectionString;
            _iTblAlertUsersBL = iTblAlertUsersBL;
            _iTblUserBL = iTblUserBL;
            _itblAlertInstanceDAO = itblAlertInstanceDAO;
            _itblAlertDefinitionBL = itblAlertDefinitionBL;
            _iTblAlertEscalationSettingsBL = iTblAlertEscalationSettingsBL;
            _iVitplNotify = iVitplNotify;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            //  _iVitplSMS = iVitplSMS;
            _iTblSmsBL = iTblSmsBL;
        }
        #region Selection

        public  List<TblAlertInstanceTO> SelectAllTblAlertInstanceList()
        {
            return  _itblAlertInstanceDAO.SelectAllTblAlertInstance();
        }

        public  TblAlertInstanceTO SelectTblAlertInstanceTO(Int32 idAlertInstance)
        {
            return  _itblAlertInstanceDAO.SelectTblAlertInstance(idAlertInstance);
        }

        public  List<TblAlertInstanceTO> SelectAllTblAlertInstanceList(Int32 userId,Int32 roleId)
        {
            return _itblAlertInstanceDAO.SelectAllTblAlertInstance();
        }

        #endregion

        #region Insertion
        public  ResultMessage SaveNewAlertInstance(TblAlertInstanceTO alertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {
            String commSprtedConcernedUsers = string.Empty;
            string hodUserIdList = string.Empty;
            Dictionary<int, Dictionary<int, string>> levelUserIdNameDCT = new Dictionary<int, Dictionary<int, string>>();
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                // 1. Get Alert Definition
                TblAlertDefinitionTO mstAlertDefinitionTO = _itblAlertDefinitionBL.SelectTblAlertDefinitionTO(alertInstanceTO.AlertDefinitionId, conn, tran);
                if (mstAlertDefinitionTO == null)
                {
                    resultMessage.Text = "TblAlertDefinitionTO Found NULL. Alert Definition is not given for this alert";
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    return resultMessage;
                }

                List<TblAlertSubscriptSettingsTO> channelTOList = new List<TblAlertSubscriptSettingsTO>();
                List<TblAlertUsersTO> alertUsersTOList = new List<TblAlertUsersTO>();

                // 2. Check If subscription and subscribed users & Channels
                if (mstAlertDefinitionTO.AlertSubscribersTOList == null || mstAlertDefinitionTO.AlertSubscribersTOList.Count == 0)
                {
                    resultMessage.Text = "Subscribers Not Found";
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    return resultMessage;
                }

                for (int i = 0; i < mstAlertDefinitionTO.AlertSubscribersTOList.Count; i++)
                {
                    TblAlertSubscribersTO mstAlertDefinitionSubscribersTO = mstAlertDefinitionTO.AlertSubscribersTOList[i];
                    TblAlertUsersTO alertUsersTO = new TblAlertUsersTO();
                    alertUsersTO.AlertSubscriptSettingsTOList = mstAlertDefinitionSubscribersTO.AlertSubscriptSettingsTOList;
                    alertUsersTO.UserId = mstAlertDefinitionSubscribersTO.UserId;
                    alertUsersTO.RoleId = mstAlertDefinitionSubscribersTO.RoleId;

                    channelTOList.AddRange(alertUsersTO.AlertSubscriptSettingsTOList);
                    alertUsersTOList.Add(alertUsersTO);
                }

                if (alertInstanceTO.AlertUsersTOList == null)
                    alertInstanceTO.AlertUsersTOList = new List<TblAlertUsersTO>();

                alertInstanceTO.AlertUsersTOList.AddRange(alertUsersTOList);




                // 3. Insert Alert Instance
                int result = InsertTblAlertInstance(alertInstanceTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblAlertInstance";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                var channelList = channelTOList.GroupBy(c => c.NotificationTypeId).ToList();

                Dictionary<Int32, string> regDeviceDCT = new Dictionary<int, string>();

                // 4. Insert alert Instance Users according to communication channels
                for (int c = 0; c < channelList.Count; c++)
                {

                    #region Dashboard alert
                    if (channelList[c].Key == (int)NotificationConstants.NotificationTypeE.ALERT)
                    {

                        var userList = (from x in alertUsersTOList
                                        where x.AlertSubscriptSettingsTOList.Any(b => b.NotificationTypeId == (int)NotificationConstants.NotificationTypeE.ALERT)
                                        select x).ToList();

                        for (int auCnt = 0; auCnt < userList.Count; auCnt++)
                        {
                            userList[auCnt].AlertInstanceId = alertInstanceTO.IdAlertInstance;

                            result = _iTblAlertUsersBL.InsertTblAlertUsers(userList[auCnt], conn, tran);
                            if (result != 1)
                            {
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While InsertTblAlertUsers";
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }

                        var userIds = string.Join(",", userList.Where(p => p.UserId > 0)
                                 .Select(p => p.UserId.ToString()));

                        if (!string.IsNullOrEmpty(userIds))
                        {
                            Dictionary<Int32, string> userDeviceDCT = new Dictionary<int, string>();
                            userDeviceDCT = _iTblUserBL.SelectUserDeviceRegNoDCTByUserIdOrRole(userIds, true, conn, tran);
                            if (userDeviceDCT != null && userDeviceDCT.Count > 0)
                                regDeviceDCT = userDeviceDCT;
                        }

                        // As per discussion with Nitin Kabra Sir 31-03-2017 ,Do Not Consider C&F Agent as for C&F Agent SMS will be sent on registered mobile number of the firm.

                        var roleIds = string.Join(",", userList.Where(p => p.RoleId > 0 && p.RoleId != (int)Constants.SystemRolesE.C_AND_F_AGENT)
                              .Select(p => p.RoleId.ToString()));

                        if (!string.IsNullOrEmpty(roleIds))
                        {
                            Dictionary<Int32, string> roleDeviceDCT = new Dictionary<int, string>();
                            roleDeviceDCT = _iTblUserBL.SelectUserDeviceRegNoDCTByUserIdOrRole(roleIds, false, conn, tran);
                            if (roleDeviceDCT != null && roleDeviceDCT.Count > 0)
                            {
                                foreach (var item in roleDeviceDCT.Keys)
                                {
                                    if (!regDeviceDCT.ContainsKey(item))
                                    {
                                        regDeviceDCT.Add(item, roleDeviceDCT[item]);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Send Alert Email
                    else if (channelList[c].Key == (int)NotificationConstants.NotificationTypeE.EMAIL)
                    {

                    }
                    #endregion

                    #region SMS

                    else if (channelList[c].Key == (int)NotificationConstants.NotificationTypeE.SMS)
                    {
                        var userList = (from x in alertUsersTOList
                                        where x.AlertSubscriptSettingsTOList.Any(b => b.NotificationTypeId == (int)NotificationConstants.NotificationTypeE.SMS)
                                        select x).ToList();

                        //Get Mobile No Dtls

                        if (userList != null)
                        {
                            List<TblSmsTO> smsTOList = new List<TblSmsTO>();

                            var userIds = string.Join(",", userList.Where(p => p.UserId > 0)
                                  .Select(p => p.UserId.ToString()));

                            if (!string.IsNullOrEmpty(userIds))
                            {
                                Dictionary<Int32, string> userDCT = new Dictionary<int, string>();
                                userDCT = _iTblUserBL.SelectUserMobileNoDCTByUserIdOrRole(userIds, true, conn, tran);

                                if (userDCT != null)
                                {
                                    foreach (var item in userDCT.Keys)
                                    {
                                        TblSmsTO smsTO = new TblSmsTO();
                                        smsTO.MobileNo = userDCT[item];
                                        smsTO.SourceTxnDesc = alertInstanceTO.SourceDisplayId;
                                        smsTO.SmsTxt = alertInstanceTO.AlertComment;
                                        smsTOList.Add(smsTO);
                                    }
                                }
                            }

                            // As per discussion with Nitin Kabra Sir 31-03-2017 ,Do Not Consider C&F Agent as for C&F Agent SMS will be sent on registered mobile number of the firm.

                            var roleIds = string.Join(",", userList.Where(p => p.RoleId > 0 && p.RoleId != (int)Constants.SystemRolesE.C_AND_F_AGENT)
                                  .Select(p => p.RoleId.ToString()));

                            if (!string.IsNullOrEmpty(roleIds))
                            {
                                Dictionary<Int32, string> roleDCT = new Dictionary<int, string>();
                                roleDCT = _iTblUserBL.SelectUserMobileNoDCTByUserIdOrRole(roleIds, false, conn, tran);

                                if (roleDCT != null)
                                {
                                    foreach (var item in roleDCT.Keys)
                                    {
                                        TblSmsTO smsTO = new TblSmsTO();
                                        smsTO.MobileNo = roleDCT[item];
                                        smsTO.SourceTxnDesc = alertInstanceTO.SourceDisplayId;
                                        smsTO.SmsTxt = alertInstanceTO.AlertComment;
                                        smsTOList.Add(smsTO);
                                    }
                                }
                            }

                            if (smsTOList != null && smsTOList.Count > 0)
                            {
                                if (alertInstanceTO.SmsTOList == null)
                                    alertInstanceTO.SmsTOList = smsTOList;
                                else
                                {
                                    alertInstanceTO.SmsTOList.AddRange(smsTOList);
                                }
                            }
                        }

                    }

                    #endregion

                }

                #region Dashboard Alert For Organizations

                if (alertInstanceTO.AlertUsersTOList != null)
                {
                    for (int auCnt = 0; auCnt < alertInstanceTO.AlertUsersTOList.Count; auCnt++)
                    {
                        alertInstanceTO.AlertUsersTOList[auCnt].AlertInstanceId = alertInstanceTO.IdAlertInstance;

                        result = _iTblAlertUsersBL.InsertTblAlertUsers(alertInstanceTO.AlertUsersTOList[auCnt], conn, tran);
                        if (result != 1)
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblAlertUsers";
                            resultMessage.Result = 0;
                            return resultMessage;
                        }

                        if (!regDeviceDCT.ContainsKey(alertInstanceTO.AlertUsersTOList[auCnt].UserId))
                        {
                            if (!string.IsNullOrEmpty(alertInstanceTO.AlertUsersTOList[auCnt].DeviceId))
                                regDeviceDCT.Add(alertInstanceTO.AlertUsersTOList[auCnt].UserId, alertInstanceTO.AlertUsersTOList[auCnt].DeviceId);
                        }
                    }
                }

                // Call to FCM Notification Webrequest. This is currently synchronous webrequest call as its async call is not working
                // If we observed slower performance we may need o change the call

                if (regDeviceDCT != null && regDeviceDCT.Count > 0)
                {
                    string[] devices = new string[regDeviceDCT.Count];
                    String notifyBody = alertInstanceTO.AlertComment;
                    String notifyTitle = mstAlertDefinitionTO.AlertDefDesc;
                    int array = 0;
                    foreach (var item in regDeviceDCT.Keys)
                    {
                        devices[array] = regDeviceDCT[item];
                        array++;
                    }

                    _iVitplNotify.NotifyToRegisteredDevices(devices, notifyBody, notifyTitle);
                }

                #endregion

                #region Send SMS

                TblConfigParamsTO smsActivationConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.SMS_SUBSCRIPTION_ACTIVATION, conn, tran);
                Int32 smsActive = 0;
                if (smsActivationConfTO != null)
                    smsActive = Convert.ToInt32(smsActivationConfTO.ConfigParamVal);

                if (smsActive == 1)
                {
                    if (alertInstanceTO.SmsTOList != null && alertInstanceTO.SmsTOList.Count > 0)
                    {
                        for (int sms = 0; sms < alertInstanceTO.SmsTOList.Count; sms++)
                        {
                            String smsResponse = _iVitplSMS.SendSMSAsync(alertInstanceTO.SmsTOList[sms]);
                            alertInstanceTO.SmsTOList[sms].ReplyTxt = smsResponse;
                            alertInstanceTO.SmsTOList[sms].AlertInstanceId = alertInstanceTO.IdAlertInstance;
                            alertInstanceTO.SmsTOList[sms].SentOn = alertInstanceTO.RaisedOn;

                            result = _iTblSmsBL.InsertTblSms(alertInstanceTO.SmsTOList[sms], conn, tran);
                        }
                    }
                }

                #endregion

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Alert Sent Successfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception In Method SaveNewAlertInstance(AlertInstanceTO alertInstanceTO, SqlConnection conn, SqlTransaction tran)";
                return resultMessage;
            }
        }

        public  ResultMessage AutoResetAndDeleteAlerts()
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //To Delete The alerts , we need to delete alertUser, alertUserActions and dependentSMSs
                // We will incorporate this logic later
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELETE_ALERT_BEFORE_DAYS, conn, tran);
                Int32 delBforeDays = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                DateTime cancellationDateTime = DateTime.MinValue;


                //Reset All alert Instances which are having isAutoReset = 1
                int result = _itblAlertInstanceDAO.ResetAutoResetAlertInstances(conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In ResetAutoResetAlertInstances";
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Alerts Resetted Sucessfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error in Method AutoResetAndDeleteAlerts at BL Level";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public  int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO)
        {
            return _itblAlertInstanceDAO.InsertTblAlertInstance(tblAlertInstanceTO);
        }

        public  int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertInstanceDAO.InsertTblAlertInstance(tblAlertInstanceTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO)
        {
            return _itblAlertInstanceDAO.UpdateTblAlertInstance(tblAlertInstanceTO);
        }

        public  int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertInstanceDAO.UpdateTblAlertInstance(tblAlertInstanceTO, conn, tran);
        }

        public  int ResetAlertInstance(int alertDefId, int sourceEntityId, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertInstanceDAO.ResetAlertInstance(alertDefId,sourceEntityId, conn, tran);
        }

        public  int ResetAlertInstanceByDef(string alertDefIds, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertInstanceDAO.ResetAlertInstanceByDef(alertDefIds,  conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblAlertInstance(Int32 idAlertInstance)
        {
            return _itblAlertInstanceDAO.DeleteTblAlertInstance(idAlertInstance);
        }

        public  int DeleteTblAlertInstance(Int32 idAlertInstance, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertInstanceDAO.DeleteTblAlertInstance(idAlertInstance, conn, tran);
        }

        #endregion
        
    }
}
