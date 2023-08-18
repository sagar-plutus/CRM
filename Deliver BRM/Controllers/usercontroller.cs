using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using SalesTrackerAPI.BL;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        #region GET

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetUser")]
        [HttpGet]
        public TblUserTO GetUser(String userLogin, String userPwd)
        {
            String encPwd = Encrypt("123");
            String decPwd = Decrypt(encPwd);

            TblUserTO tblUserTO = BL.TblUserBL.SelectTblUserTO(userLogin, userPwd);
            if (tblUserTO != null)
            {
                tblUserTO.UserRoleList = BL.TblUserRoleBL.SelectAllActiveUserRoleList(tblUserTO.IdUser);
            }
            return tblUserTO;
        }

        //Hrushikesh
        [Route("GetAllActiveRolesOnUserId")]
        [HttpGet]
        public List<TblUserRoleTO> getAllActiveRoles(int userId)
        {
            return BL.TblUserRoleBL.SelectAllActiveUserRoleList(userId);

        }


        [Route("GetPermissionsOnModuleId")]
        [HttpGet]
        public TblUserTO getPermissionsOnModule(int userId, int moduleId)
        {

            return BL.TblLoginBL.getPermissionsOnModule(userId, moduleId);


        }

        [Route("GetUsersFromRoleForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUsersFromRoleForDropDown(Int32 roleId)
        {
            List<DropDownTO> userList = BL.TblUserRoleBL.SelectUsersFromRoleForDropDown(roleId);
            return userList;
        }

        [Route("GetUsersFromRoleIds")]
        [HttpGet]
        public List<DropDownTO> GetUsersFromRoleIds(String roleId)
        {
            List<DropDownTO> userList = BL.TblUserRoleBL.SelectUsersFromRoleIdsForDropDown(roleId);
            return userList;
        }

        /// <summary>
        /// Saket [2018-10-30] Added as these call was in STAPI code.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [Route("GetPersonOnPersonId")]
        [HttpGet]
        public TblPersonTO GetPersonOnPersonId(Int32 personId)
        {
            return BL.TblPersonBL.SelectTblPersonTO(personId);
        }


        [Route("GetPersonOnUserId")]
        [HttpGet]
        public TblPersonTO GetPersonOnUserId(Int32 UserId)
        {
            return BL.TblPersonBL.GetPersonOnUserId(UserId);
        }





        [Route("GetActiveUserDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetActiveUserDropDownList()
        {
            List<DropDownTO> userList = BL.TblUserBL.SelectAllActiveUsersForDropDown();
            return userList;
        }

        [Route("GetCurrentActiveUsers")]
        [HttpGet]
        public List<TblLoginTO> GetCurrentActiveUsers()
        {
            List<TblLoginTO> activeUserList = BL.TblLoginBL.GetCurrentActiveUsers();
            return activeUserList;
        }


        [Route("GetUsersConfigration")]
        [HttpGet]
        public dimUserConfigrationTO GetUsersConfigration(int ConfigDesc)
        {
            dimUserConfigrationTO UserConfigrationTO = BL.TblLoginBL.GetUsersConfigration(ConfigDesc);
            return UserConfigrationTO;
        }

        [Route("GetFeedbackList")]
        [HttpGet]
        public List<TblFeedbackTO> GetFeedbackList(int userId, string fromDate, string toDate)
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
                frmDt = Constants.ServerDateTime.AddDays(-7);
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = Constants.ServerDateTime;

            return BL.TblFeedbackBL.SelectAllTblFeedbackList(userId, frmDt, toDt);
        }

        [Route("GetUserAllocatedAreaList")]
        [HttpGet]
        public List<TblUserAreaAllocationTO> GetUserAllocatedAreaList(int userId)
        {
            return BL.TblUserAreaAllocationBL.SelectAllTblUserAreaAllocationList(userId);
        }


        [Route("GetRoleOrUserPermissionList")]
        [HttpGet]
        public List<PermissionTO> GetRoleOrUserPermissionList(int menuPageId, int roleId, int userId, int moduleId)
        {
            return BL.TblSysElementsBL.SelectAllPermissionList(menuPageId, roleId, userId, moduleId);
        }

        [Route("GetAllSystemUserList")]
        [HttpGet]

        public List<TblUserTO> GetAllSystemUserList()
        {
            List<TblUserTO> list = BL.TblUserBL.SelectAllTblUserList(true);
            if (list != null)
            {
                List<TblUserRoleTO> userRoleList = BL.TblUserRoleBL.SelectAllTblUserRoleList();
                for (int i = 0; i < list.Count; i++)
                {
                    var roleList = userRoleList.Where(r => r.UserId == list[i].IdUser && r.IsActive == 1).ToList();
                    if (roleList != null && roleList.Count > 0)
                    {
                        list[i].UserRoleList = roleList;
                    }
                }
                // list = list.OrderBy(o => o.UserRoleList[0].RoleDesc).ThenBy(o => o.UserDisplayName).ToList();

                var tempList = list.Where(o => o.UserRoleList == null);

                list = list.Where(o => o.UserRoleList != null && o.UserRoleList.Count > 0).OrderBy(o => o.UserRoleList[0].RoleDesc).ThenBy(o => o.UserDisplayName).ToList();

                list.AddRange(tempList);
            }
            return list;
        }


        /// Get birthday or anniversary notifications - Tejaswini
        [Route("GetBirthdays/{Date}/{UpcomingDays}/{IsBirthday}")]
        [HttpGet]
        public List<BirthdayAlertTO> GetTodaysBirthdays(string Date, Int32 UpcomingDays, Int32 IsBirthday)
        {
            DateTime date = DateTime.MinValue;
            if (Date != null)
            {
                //fromDate = DateTimeOffset.Parse(FromDate).UtcDateTime;
                date = Convert.ToDateTime(Date);
            }
            return TblPersonBL.SelectAllPersonBirthday(date, UpcomingDays, IsBirthday);
        }


        [Route("GetUserDetails")]
        [HttpGet]
        public TblUserTO GetUserDetails(Int32 userId)
        {
            TblUserTO userTO = BL.TblUserBL.SelectTblUserTO(userId);
            if (userTO != null)
            {
                userTO.UserExtTO = BL.TblUserExtBL.SelectTblUserExtTO(userId);
                if (userTO.UserExtTO != null)
                    userTO.UserPersonTO = TblPersonBL.SelectTblPersonTO(userTO.UserExtTO.PersonId);

                userTO.UserRoleList = BL.TblUserRoleBL.SelectAllActiveUserRoleList(userId);
            }

            return userTO;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Vijaymala [04-12-2018] added to select all role type list
        /// </summary>
        /// <param ></param>
        [Route("GetAllDimRoleTypeList")]
        [HttpGet]
        public List<DimRoleTypeTO> GetAllDimRoleTypeList()
        {
            return BL.DimRoleTypeBL.SelectAllDimRoleTypeList();
        }

        #endregion

        #region POST

        //[Route("PostLogin")]
        //[HttpPost]
        //public TblUserTO PostLogin([FromBody] TblUserTO userTO)
        //{
        //    try
        //    {

        //        if (userTO == null)
        //        {
        //            return null;
        //        }
        //        //String[] devices = { "dr5RvjV8_hk:APA91bFrDgE0NFAI8u5-eTVGrG4BGGJywIbHYywxrrLmmTLrC2-pjQLhhA48Tc7WF32hJTkd_Ik60MkfzZJXhcuJupu1hIshP-3ri-FrSQAQQHimCj4CWBfVsmIZB8K8qom3mzLS3x5S" };
        //        //String body = "ddd"; String title = "dddddd";
        //        //string ss = VitplNotify.NotifyToRegisteredDevices(devices,body,title);
        //        //BL.TblLoadingBL.CancelAllNotConfirmedLoadingSlips();
        //        ResultMessage rMessage = new ResultMessage();
        //        rMessage = BL.TblLoginBL.LogIn(userTO);
        //        if (rMessage.MessageType != ResultMessageE.Information)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            userTO = (TblUserTO)rMessage.Tag;
        //            return userTO;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        [Route("PostLogin")]
        [HttpPost]
        public ResultMessage PostLogin([FromBody] TblUserTO userTO)
        {
            try
            {

                if (userTO == null)
                {
                    return null;
                }
                //String[] devices = { "dr5RvjV8_hk:APA91bFrDgE0NFAI8u5-eTVGrG4BGGJywIbHYywxrrLmmTLrC2-pjQLhhA48Tc7WF32hJTkd_Ik60MkfzZJXhcuJupu1hIshP-3ri-FrSQAQQHimCj4CWBfVsmIZB8K8qom3mzLS3x5S" };
                //String body = "ddd"; String title = "dddddd";
                //string ss = VitplNotify.NotifyToRegisteredDevices(devices,body,title);
                //BL.TblLoadingBL.CancelAllNotConfirmedLoadingSlips();
                ResultMessage rMessage = new ResultMessage();
                rMessage = BL.TblLoginBL.LogIn(userTO);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return rMessage;
                }
                else
                {
                    return rMessage;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("PostForgotPsw")]
        [HttpPost]
        public async Task<ResultMessage> PostForgotPsw([FromBody] TblForgotPswTO forgotPswTO)
        {
            try
            {

                if (forgotPswTO == null)
                {
                    return null;
                }
                ResultMessage rMessage = new ResultMessage();
                Task<ResultMessage> resultMessag = BL.TblLoginBL.ForgotPswAsync(forgotPswTO);
                rMessage = await resultMessag;
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return rMessage;
                }
                else
                {
                    return rMessage;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [Route("PostChangeCrdentials")]
        [HttpPost]
        public ResultMessage PostChangeCrdentials([FromBody] TblUserTO TblUserTO)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (TblUserTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "userTO Found NULL";
                    return resultMessage;
                }

                ResultMessage rMessage = new ResultMessage();

                TblUserTO userTo = BL.TblUserBL.SelectTblUserTO(TblUserTO.IdUser);
                if (userTo == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "userTO Found NULL";
                    return resultMessage;
                }


                TblUserPwdHistoryTO tblUserPwdHistoryTO = new TblUserPwdHistoryTO();
                tblUserPwdHistoryTO.OldPwd = userTo.UserPasswd;
                tblUserPwdHistoryTO.NewPwd = TblUserTO.UserPasswd;
                tblUserPwdHistoryTO.CreatedOn = Constants.ServerDateTime;
                tblUserPwdHistoryTO.CreatedBy = TblUserTO.UserExtTO.CreatedBy;  //Pass from GUI
                tblUserPwdHistoryTO.UserId = userTo.IdUser;

                userTo.UserPasswd = TblUserTO.UserPasswd;

                return BL.TblUserBL.UpdateTblUser(userTo, tblUserPwdHistoryTO);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error At API Level";
                return resultMessage;
            }
        }

        [Route("PostFeedback")]
        [HttpPost]
        public ResultMessage PostFeedback([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblFeedbackTO feedbackTO = JsonConvert.DeserializeObject<TblFeedbackTO>(data["feedbackTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (feedbackTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Feedback Object Found Null";
                    return returnMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                feedbackTO.CreatedOn = Constants.ServerDateTime;
                feedbackTO.CreatedBy = Convert.ToInt32(loginUserId);

                int result = BL.TblFeedbackBL.InsertTblFeedback(feedbackTO);
                if (result == 1)
                {
                    returnMsg.MessageType = ResultMessageE.Information;
                    returnMsg.Result = 1;
                    returnMsg.Text = "Feedback Saved Successfully";
                    return returnMsg;
                }
                else
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "Error While InsertTblFeedback ";
                    return returnMsg;
                }

            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostFeedback";
                return returnMsg;
            }
        }

        [Route("PostLogOut")]
        [HttpPost]
        public int PostLogOut([FromBody] TblUserTO userTO)
        {
            try
            {

                if (userTO == null)
                {
                    return 0;
                }

                ResultMessage rMessage = new ResultMessage();
                rMessage = BL.TblLoginBL.LogOut(userTO);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        [Route("GetModuleDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetModuleDropDownList()
        {
            List<DropDownTO> userList = BL.TblModuleBL.SelectAllTblModuleList();
            return userList;
        }

        [Route("PostUserAreaAllocation")]
        [HttpPost]
        public ResultMessage PostUserAreaAllocation([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<TblUserAreaAllocationTO> userAreaAllocationTOList = JsonConvert.DeserializeObject<List<TblUserAreaAllocationTO>>(data["userAreaAllocationTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (userAreaAllocationTOList == null || userAreaAllocationTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : userAreaAllocationTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = Constants.ServerDateTime;
                for (int i = 0; i < userAreaAllocationTOList.Count; i++)
                {
                    userAreaAllocationTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    userAreaAllocationTOList[i].CreatedOn = confirmedDate;
                    userAreaAllocationTOList[i].IsActive = 1;
                }

                ResultMessage resMsg = BL.TblUserAreaAllocationBL.SaveUserAreaAllocation(userAreaAllocationTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserAreaAllocation";
                return returnMsg;
            }
        }


        [Route("PostUserOrRolePermission")]
        [HttpPost]
        public ResultMessage PostUserOrRolePermission([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                PermissionTO permissionTO = JsonConvert.DeserializeObject<PermissionTO>(data["permissionTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (permissionTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : permissionTO Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = Constants.ServerDateTime;
                permissionTO.CreatedBy = Convert.ToInt32(loginUserId);
                permissionTO.CreatedOn = confirmedDate;

                ResultMessage resMsg = BL.TblSysElementsBL.SaveRoleOrUserPermission(permissionTO);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUserOrRolePermission";
                return returnMsg;
            }
        }

        [Route("PostNewUser")]
        [HttpPost]
        public ResultMessage PostNewUser([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblUserTO userTO = JsonConvert.DeserializeObject<TblUserTO>(data["userTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (userTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : userTO Found Null";
                    return returnMsg;
                }

                int userId = Convert.ToInt32(loginUserId);
                ResultMessage resMsg = BL.TblUserBL.SaveNewUser(userTO, userId);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostNewUser";
                return returnMsg;
            }
        }

        [Route("PostUpdateUserDtl")]
        [HttpPost]
        public ResultMessage PostUpdateUserDtl([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblUserTO userTO = JsonConvert.DeserializeObject<TblUserTO>(data["userTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (userTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : userTO Found Null";
                    return returnMsg;
                }

                int userId = Convert.ToInt32(loginUserId);
                if (userTO.IsActive == 0)
                {
                    userTO.DeactivatedBy = userId;
                    userTO.DeactivatedOn = Constants.ServerDateTime;
                    int result = BL.TblUserBL.UpdateTblUser(userTO);
                    if (result == 1)
                    {
                        returnMsg.MessageType = ResultMessageE.Information;
                        returnMsg.Result = 1;
                        returnMsg.Text = "Record Updated Successfully";
                        returnMsg.DisplayMessage = "Record Updated Successfully";
                        return returnMsg;
                    }
                    else
                    {
                        returnMsg.MessageType = ResultMessageE.Error;
                        returnMsg.Result = 0;
                        returnMsg.Text = "API : Error In Method UpdateTblUser";
                        returnMsg.DisplayMessage = Constants.DefaultErrorMsg;
                        return returnMsg;
                    }
                }
                else
                {
                    ResultMessage resMsg = BL.TblUserBL.UpdateUser(userTO, userId);
                    return resMsg;
                }
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostUpdateUserDtl";
                return returnMsg;
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        #endregion


        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }


        /// <summary>
        ///Sudhir[24-APR-2018] Added for Uploading Image  
        /// </summary>
        /// <param name="tblDocumentDetailsTOTblDocumentDetailsTO"></param>
        /// <returns></returns>
        [Route("UploadUserProfilePicture")]
        [HttpPost]
        public ResultMessage UploadUserProfilePicture([FromBody] JObject data)
        {
            TblDocumentDetailsTO tblDocumentDetailsTO = JsonConvert.DeserializeObject<TblDocumentDetailsTO>(data["data"].ToString());

            //TblDocumentDetailsTO tblDocumentDetailsTO = data;
            return BL.TblDocumentDetailsBL.UploadUserProfilePicture(tblDocumentDetailsTO);
        }

        /// <summary>
        ///Sudhir[24-APR-2018] Added for Uploading Image  
        /// </summary>
        /// <param name="tblDocumentDetailsTOTblDocumentDetailsTO"></param>
        /// <returns></returns>
        [Route("UploadUserProfilePictureAsync")]
        [HttpPost]
        public async Task<ResultMessage> UploadUserProfilePictureAsync([FromBody] JObject data)
        {
            
            TblDocumentDetailsTO tblDocumentDetailsTO = JsonConvert.DeserializeObject<TblDocumentDetailsTO>(data["data"].ToString());
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                Task<ResultMessage> resultMessag = BL.TblDocumentDetailsBL.UploadFileAsync(tblDocumentDetailsTO);
                resultMessage = await resultMessag;
                return resultMessage; 
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion

        #region DELETE

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

        #region OTHER FUNCTION

        public static string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                //var saltedPassword = string.Format("{0}{1}", salt, password);
                var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion

    }
}
