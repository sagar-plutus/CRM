using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblLoginBL
    {
        #region Selection

        public static List<TblLoginTO> SelectAllTblLoginList()
        {
            return TblLoginDAO.SelectAllTblLogin();
        }

        public static TblLoginTO SelectTblLoginTO(Int32 idLogin)
        {
            return TblLoginDAO.SelectTblLogin(idLogin);
        }


        public static TblUserTO getPermissionsOnModule(int userId, int moduleId)
        {
            TblUserTO userExistUserTO = new TblUserTO();
            userExistUserTO.IdUser = userId;
            try
            {
                userExistUserTO.UserRoleList = TblUserRoleDAO.SelectAllActiveUserRole(userExistUserTO.IdUser);
                List<TblModuleTO> allModuleList = TblModuleDAO.SelectTblModuleList().ToList();

                if (userExistUserTO.UserRoleList != null || userExistUserTO.UserRoleList.Count > 0)
                {
                    int[] list = userExistUserTO.UserRoleList.Where(a => a.IsActive == 1).Select(s => s.RoleId).ToArray();
                    String roleId = string.Join(",", list.ToArray());
                    userExistUserTO.SysEleAccessDCT = TblSysElementsBL.SelectSysElementUserMultiRoleEntitlementDCT(userExistUserTO.IdUser, roleId, moduleId);
                    userExistUserTO.ModuleTOList = new List<TblModuleTO>();

                    for (int m = 0; m < allModuleList.Count; m++)
                    {
                        if (allModuleList[m].IsSubscribe == 1) //Sudhir[30-08-2018] Added for checking IsSubscribe or Not. 
                        {
                            if (userExistUserTO.SysEleAccessDCT.ContainsKey(allModuleList[m].SysElementId))
                            {
                                if (userExistUserTO.SysEleAccessDCT[allModuleList[m].SysElementId] != "RW")
                                    allModuleList[m].NavigateUrl = null; //Added Sudhir For Set NavigateURL NULL
                            }
                        }
                        else
                        {
                            allModuleList[m].NavigateUrl = null; //Added Sudhir For Set NavigateURL NULL
                        }
                        userExistUserTO.ModuleTOList.Add(allModuleList[m]);
                    }
                }

                for (int i = 0; i < allModuleList.Count; i++)
                {
                    if (allModuleList[i].ModeId == (int)Constants.ApplicationModeTypeE.BASIC_MODE)
                    {
                        List<TblSysElementsTO> tblSysElementsTOList = TblSysElementsBL.SelectTblSysElementsByModulId(allModuleList[i].IdModule);

                        if (tblSysElementsTOList != null && tblSysElementsTOList.Count > 0)
                        {
                            for (int syse = 0; syse < tblSysElementsTOList.Count; syse++)
                            {
                                if (userExistUserTO.SysEleAccessDCT.ContainsKey(tblSysElementsTOList[syse].IdSysElement) && tblSysElementsTOList[syse].BasicModeApplicable != 1)
                                {
                                    userExistUserTO.SysEleAccessDCT.Remove(tblSysElementsTOList[syse].IdSysElement);
                                }

                                TblModuleTO moduleTO = userExistUserTO.ModuleTOList.Where(w => w.SysElementId == tblSysElementsTOList[syse].IdSysElement).FirstOrDefault();

                                if (moduleTO != null && tblSysElementsTOList[syse].BasicModeApplicable != 1)
                                {
                                    userExistUserTO.ModuleTOList.Remove(moduleTO);
                                }
                            }
                        }
                    }

                }
                return userExistUserTO;
            }
            catch (Exception ex)
            {
                return userExistUserTO;
            }
        }


        #endregion

        #region Insertion
        public static int InsertTblLogin(TblLoginTO tblLoginTO)
        {
            return TblLoginDAO.InsertTblLogin(tblLoginTO);
        }

        public static int InsertTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoginDAO.InsertTblLogin(tblLoginTO, conn, tran);
        }

        public static ResultMessage LogIn(TblUserTO tblUserTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                #region Check Current Active Users Count and Configration Value @KM 13/02/2019
                Boolean isProceedToCreate = BL.TblLoginBL.GetUsersAvailability();
                if (!isProceedToCreate)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Login Limits Exceeded, Please Contact Your Administrative";
                    return resultMessage;
                }
                #endregion

                #region 1. Check Is This user Exists First

                TblUserTO userExistUserTO = TblUserBL.SelectTblUserTO(tblUserTO.UserLogin, tblUserTO.UserPasswd);
                if (userExistUserTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Invalid Credentials";
                    return resultMessage;
                }

                if (!string.IsNullOrEmpty(userExistUserTO.RegisteredDeviceId) && !string.IsNullOrEmpty(tblUserTO.RegisteredDeviceId))
                {
                    if (tblUserTO.RegisteredDeviceId != userExistUserTO.RegisteredDeviceId)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Hey , Not Allowed. Current Log In Device and Registered Device Not Matching";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                #endregion

                if (userExistUserTO != null)
                {
                    userExistUserTO.UserRoleList = BL.TblUserRoleBL.SelectAllActiveUserRoleList(userExistUserTO.IdUser);
                    if (userExistUserTO.UserRoleList == null || userExistUserTO.UserRoleList.Count == 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Your Role Is Not Defined In The System , Please contact your system admin";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    //int roleId = userExistUserTO.UserRoleList.Where(a => a.IsActive == 1).FirstOrDefault().RoleId;

                    //[Hrushikesh]--For bringing all permision against multiple role for same user

                    int[] list = userExistUserTO.UserRoleList.Where(a => a.IsActive == 1).Select(s => s.RoleId).ToArray();
                    String roleId = string.Join(",", list.ToArray());
                    userExistUserTO.SysEleAccessDCT = TblSysElementsBL.SelectSysElementUserMultiRoleEntitlementDCT(userExistUserTO.IdUser, roleId);

                    //userExistUserTO.SysEleAccessDCT = TblSysElementsBL.SelectSysElementUserEntitlementDCT(userExistUserTO.IdUser, roleId);
                    if (userExistUserTO.SysEleAccessDCT == null || userExistUserTO.SysEleAccessDCT.Count == 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "User has No Permissions.";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    //Prajakta[2019-04-30] Added to get PM - User List
                    userExistUserTO.PmUserList = BL.TblPmUserBL.SelectAllPMForUser(userExistUserTO.IdUser);

                    List<TblMenuStructureTO> allMenuList = TblMenuStructureBL.SelectAllTblMenuStructureList().OrderBy(s => s.SerNo).ToList();
                    userExistUserTO.MenuStructureTOList = new List<TblMenuStructureTO>();

                    for (int m = 0; m < allMenuList.Count; m++)
                    {
                        if (userExistUserTO.SysEleAccessDCT.ContainsKey(allMenuList[m].SysElementId))
                        {
                            if (userExistUserTO.SysEleAccessDCT[allMenuList[m].SysElementId] == "RW")
                                userExistUserTO.MenuStructureTOList.Add(allMenuList[m]);
                        }
                    }
                    List<TblModuleTO> allModuleList = BL.TblModuleBL.SelectTblModuleList().ToList();
                    userExistUserTO.ModuleTOList = new List<TblModuleTO>();

                    for (int m = 0; m < allModuleList.Count; m++)
                    {
                        if (allModuleList[m].IsSubscribe == 1) //Sudhir[30-08-2018] Added for checking IsSubscribe or Not. 
                         {
                            if (userExistUserTO.SysEleAccessDCT.ContainsKey(allModuleList[m].SysElementId))
                            {
                                if (userExistUserTO.SysEleAccessDCT[allModuleList[m].SysElementId] != "RW")
                                    allModuleList[m].NavigateUrl = null;
                            }
                        }
                        else
                        {
                            allModuleList[m].NavigateUrl = null;
                        }
                        userExistUserTO.ModuleTOList.Add(allModuleList[m]);
                    }

                    //// Vaibhav [05-Mar-2018] Added to get access token
                    // userExistUserTO.AuthorizationTO = Authentication.Authentication.getAccessToken(tblUserTO.UserLogin, tblUserTO.UserPasswd);

                    //// Vaibhav [05-Mar-2018] Added to get all product module list.
                    //userExistUserTO.ModuleTOList = BL.TblModuleBL.SelectAllTblModuleList();
                }

                #region 2. Mark Login Entry
                userExistUserTO.LoginTO = tblUserTO.LoginTO;
                userExistUserTO.LoginTO.LoginDate = Constants.ServerDateTime;
                userExistUserTO.LoginTO.UserId = userExistUserTO.IdUser;
                userExistUserTO.LoginTO.DeviceId = tblUserTO.DeviceId;
                int result = InsertTblLogin(userExistUserTO.LoginTO);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Could not login. Some error occured while login";
                    resultMessage.Tag = "Error While InsertTblLogin In Method Login";
                    resultMessage.Result = 0;
                    return resultMessage;
                }


                #endregion

                #region 3. Update Device Id for New Registration

                if (String.IsNullOrEmpty(userExistUserTO.RegisteredDeviceId)
                    && String.IsNullOrEmpty(userExistUserTO.DeviceId))
                {
                    if (!string.IsNullOrEmpty(tblUserTO.RegisteredDeviceId)
                        && !string.IsNullOrEmpty(tblUserTO.DeviceId))
                    {
                        userExistUserTO.RegisteredDeviceId = tblUserTO.RegisteredDeviceId;
                        userExistUserTO.ImeiNumber = tblUserTO.DeviceId;
                        TblUserBL.UpdateTblUser(userExistUserTO);
                    }
                }

                #endregion

                //Vijaymala Added[14-02-2017]:To set current company Id

                //TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_CURRENT_COMPANY);
                //if (configParamsTO != null)
                //{
                //    userExistUserTO.FirmNameE = Convert.ToInt16(configParamsTO.ConfigParamVal);
                //}

                tblUserTO = userExistUserTO;

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "User Logged In Sucessfully";
                resultMessage.Tag = userExistUserTO;
                resultMessage.Result = 1;

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Could not login. Some error occured while login";
                resultMessage.Tag = "Exception Error While LogIn at BL Level";
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }
        public static List<TblLoginTO> GetCurrentActiveUsers()
        {
            return TblLoginDAO.GetCurrentActiveUsers();
        }

        public static dimUserConfigrationTO GetUsersConfigration(int ConfigDesc)
        {
            return TblLoginDAO.GetUsersConfigration(ConfigDesc);
        }
        public static dimUserConfigrationTO GetUsersConfigration(int ConfigDesc, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoginDAO.GetUsersConfigration(ConfigDesc, conn, tran);
        }
        public static Boolean GetUsersQuata(SqlConnection conn, SqlTransaction tran)
        {
            Boolean isValid = true;
            dimUserConfigrationTO dimUserConfigrationTO = BL.TblLoginBL.GetUsersConfigration((int)Constants.UsersConfigration.USER_CONFIG, conn, tran);
            if (dimUserConfigrationTO != null)
            {
                List<TblUserTO> list = TblUserBL.SelectAllTblUserList(true);
                if (list != null && list.Count > 0)
                {
                    if (Convert.ToInt32(dimUserConfigrationTO.ConfigValue) <= list.Count)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        public static Boolean GetUsersAvailability()
        {
            Boolean isValid = true;
            dimUserConfigrationTO dimUserConfigrationTO = BL.TblLoginBL.GetUsersConfigration((int)Constants.UsersConfigration.USER_CONFIG);
            if (dimUserConfigrationTO != null)
            {
                List<TblLoginTO> list = GetCurrentActiveUsers();
                if (list != null && list.Count > 0)
                {
                    if (Convert.ToInt32(dimUserConfigrationTO.ConfigValue) <= list.Count)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }

        public static ResultMessage LogOut(TblUserTO tblUserTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                #region 1. Check Is This user Exists First

                TblUserTO userExistUserTO = TblUserBL.SelectTblUserTO(tblUserTO.UserLogin, tblUserTO.UserPasswd);
                if (userExistUserTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "User Not Found";
                    return resultMessage;
                }

                #endregion

                #region 2. Update Login Entry
                TblLoginTO loginTO = tblUserTO.LoginTO;
                loginTO.LogoutDate = Constants.ServerDateTime;
                int result = UpdateTblLogin(loginTO);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLogin In Method LogOut";
                    return resultMessage;
                }

                #endregion

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "User Logged Out Sucessfully";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method LogOut";
                resultMessage.Tag = ex;
                return resultMessage;
            }
        }

        internal static async Task<ResultMessage> ForgotPswAsync(TblForgotPswTO forgotPswTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            SqlTransaction tran = null;
            string results = null;
            int result = 0;
            try
            {

                #region 1. Check Is This user Exists First

                TblUserTO userExistUserTO = TblUserBL.SelectTblUserTOIfExists(forgotPswTO.MobileNo, forgotPswTO.PrimaryEmail);
                if (userExistUserTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Invalid Credentials";
                    return resultMessage;
                }
                #endregion

                #region Send Mail
                if (userExistUserTO.UserPersonTO.PrimaryEmail != null)
                {

                    SendMail sendMail = new SendMail();
                    sendMail.To = userExistUserTO.UserPersonTO.PrimaryEmail;

                    sendMail.BodyContent = "<h4>Dear " + userExistUserTO.UserDisplayName + ", </h4><p> Your Login ID is '" + userExistUserTO.UserLogin + "'</p><p>And Your Password is '" + userExistUserTO.UserPasswd + "'</p><p>Please login and Change your password.</p>";
                    sendMail.Message = "";
                    string mailSubject = "Password";
                    if (mailSubject.Length > 0)
                    {
                        sendMail.Subject = mailSubject;
                    }
                    else
                    {
                        sendMail.Subject = "Regards forgot password";
                    }
                    string FileName = null;
                    Task<ResultMessage> resultMessag = SendMailBL.SendEmailAsync(sendMail, FileName);
                    resultMessage =await resultMessag;
                }
                #endregion

                #region Send SMS               
                TblSmsTO tblSmsTO = new TblSmsTO();
                tblSmsTO.MobileNo = userExistUserTO.UserPersonTO.MobileNo;
                tblSmsTO.SmsTxt = "<h4>Dear " + userExistUserTO.UserDisplayName + ", </h4><p> Your Login ID is '" + userExistUserTO.UserLogin + "'</p><p> And Your Password is '" + userExistUserTO.UserPasswd + "'</p><p> Please login and Change your password.</p>";
                tblSmsTO.SmsTxt = Regex.Replace(tblSmsTO.SmsTxt, "<.*?>", String.Empty);
                results = VitplSMS.SendSMSAsync(tblSmsTO);


                #endregion
                
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Could not login. Some error occured while login";
                resultMessage.Tag = "Exception Error While LogIn at BL Level";
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblLogin(TblLoginTO tblLoginTO)
        {
            return TblLoginDAO.UpdateTblLogin(tblLoginTO);
        }

        public static int UpdateTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoginDAO.UpdateTblLogin(tblLoginTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblLogin(Int32 idLogin)
        {
            return TblLoginDAO.DeleteTblLogin(idLogin);
        }

        public static int DeleteTblLogin(Int32 idLogin, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoginDAO.DeleteTblLogin(idLogin, conn, tran);
        }

        #endregion

    }
}
