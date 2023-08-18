using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using PurchaseTrackerAPI.BL;
using PurchaseTrackerAPI.DAL.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblSysElementsBL _iTblSysElementsBL;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblUserExtBL _iTblUserExtBL;
        private readonly Idimensionbl _idimensionbl;
        private readonly ITblFeedbackBL _iTblFeedbackBL;
        private readonly ITblPurchaseManagerSupplierBL _iTblPurchaseManagerSupplierBL;
        public UserController(Idimensionbl idimensionbl, ITblUserBL iTblUserBL, ITblUserExtBL iTblUserExtBL, ITblUserRoleBL iTblUserRoleBL, ITblPurchaseManagerSupplierBL iTblPurchaseManagerSupplierBL, ITblFeedbackBL iTblFeedbackBL, ITblSysElementsBL iTblSysElementsBL, ITblPersonBL iTblPersonBL, Icommondao icommondao)
        {
            _iCommonDAO = icommondao;
            _iTblPersonBL = iTblPersonBL;
            _iTblSysElementsBL = iTblSysElementsBL;
            _iTblFeedbackBL = iTblFeedbackBL;
            _iTblPurchaseManagerSupplierBL = iTblPurchaseManagerSupplierBL;
            _iTblUserRoleBL = iTblUserRoleBL;
             _iTblUserExtBL =  iTblUserExtBL;
            _iTblUserBL = iTblUserBL;
            _idimensionbl = idimensionbl;

        }

        #region GET

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value10", "value20" };
        }

        //[Route("GetUser")]
        //[HttpGet]
        //public TblUserTO GetUser(String userLogin, String userPwd)
        //{
        //    String encPwd = Encrypt("123");
        //    String decPwd = Decrypt(encPwd);

        //    TblUserTO tblUserTO = _iTblUserBL.SelectTblUserTO(userLogin, userPwd);
        //    if (tblUserTO != null)
        //    {
        //        tblUserTO.UserRoleList = _iTblUserRoleBL.SelectAllActiveUserRoleList(tblUserTO.IdUser);
        //    }
        //    return tblUserTO;
        //}

        [Route("GetUsersFromRoleForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUsersFromRoleForDropDown(Int32 roleId)
        {
            List<DropDownTO> userList = _iTblUserRoleBL.SelectUsersFromRoleForDropDown(roleId);
            return userList;
        }

        [Route("GetPurchaseManagerListOfSupplierForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(Int32 supplierId)
        {
            List<DropDownTO> purchaseManagerList = _iTblPurchaseManagerSupplierBL.GetPurchaseManagerListOfSupplierForDropDown(supplierId);
            return purchaseManagerList;
        }


        [Route("GetActiveUserDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetActiveUserDropDownList()
        {
            List<DropDownTO> userList = _iTblUserBL.SelectAllActiveUsersForDropDown();
            return userList;
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
                frmDt =  _iCommonDAO.ServerDateTime.AddDays(-7);
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt =  _iCommonDAO.ServerDateTime;

            return _iTblFeedbackBL.SelectAllTblFeedbackList(userId, frmDt, toDt);
        }

        // [Route("GetUserAllocatedAreaList")]
        // [HttpGet]
        // public List<TblUserAreaAllocationTO> GetUserAllocatedAreaList(int userId)
        // {
        //     return BL.TblUserAreaAllocationBL.SelectAllTblUserAreaAllocationList(userId);
        // }


        [Route("GetRoleOrUserPermissionList")]
        [HttpGet]
        public List<PermissionTO> GetRoleOrUserPermissionList(int menuPageId, int roleId, int userId)
        {
            return _iTblSysElementsBL.SelectAllPermissionList(menuPageId, roleId, userId);
        }

        [Route("GetAllSystemUserList")]
        [HttpGet]
        public List<TblUserTO> GetAllSystemUserList()
        {
            List<TblUserTO> list = _iTblUserBL.SelectAllTblUserList(true);
            if (list != null)
            {
                List<TblUserRoleTO> userRoleList = _iTblUserRoleBL.SelectAllTblUserRoleList();
                for (int i = 0; i < list.Count; i++)
                {
                    var roleList = userRoleList.Where(r => r.UserId == list[i].IdUser && r.IsActive == 1).ToList();
                    list[i].UserRoleList = roleList;
                }

                list = list.OrderBy(o => o.UserRoleList[0].RoleDesc).ThenBy(o => o.UserDisplayName).ToList();
            }
            return list;
        }

        [Route("GetUserDetails")]
        [HttpGet]
        public TblUserTO GetUserDetails(Int32 userId)
        {
            TblUserTO userTO = _iTblUserBL.SelectTblUserTO(userId);
            if (userTO != null)
            {
                userTO.UserExtTO = _iTblUserExtBL.SelectTblUserExtTO(userId);
                if (userTO.UserExtTO != null)
                    userTO.UserPersonTO = _iTblPersonBL.SelectTblPersonTO(userTO.UserExtTO.PersonId);

                userTO.UserRoleList = _iTblUserRoleBL.SelectAllActiveUserRoleList(userId);
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
        /// swati pisal
        /// Get List of all supplier list
        /// </summary>
        /// <param name="userId"></param> Here userId means PMId
        /// <returns></returns>
        [Route("GetSupplierAndPurchaseManagerList")]
        [HttpGet]
        public List<TblPurchaseManagerSupplierTO> GetSupplierAndPurchaseManagerList(int userId)
        {
            return _iTblPurchaseManagerSupplierBL.SelectAllActivePurchaseManagerSupplierList(userId);
        }

        [Route("GetSupplierByPMDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetSupplierByPMDropDownList(string userId)
        {
            List<DropDownTO> userList = _iTblPurchaseManagerSupplierBL.GetSupplierByPMDropDownList(userId);
            return userList;
        }

        [Route("GetSupplierStateId")]
        [HttpGet]
        public Int32 GetSupplierStateId(int supplierID)
        {
            Int32 stateId = _iTblPurchaseManagerSupplierBL.GetSupplierStateId(supplierID);
            return stateId;
        }

        /// <summary>
        /// Swati Pisal
        /// Get Purchase List
        /// </summary>
        /// <returns></returns>
        [Route("GetActivePurchaseDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetActivePurchaseDropDownList()
        {
            List<DropDownTO> userList = _iTblPurchaseManagerSupplierBL.SelectPurchaseFromRoleForDropDown();
            return userList;
        }

        /// <summary>
        /// Deepali
        /// Get Graders List
        /// </summary>
        /// <returns></returns>
        [Route("GetActiveUsersDropDownListByRoleTypeId")]
        [HttpGet]
        public List<DropDownTO> GetActiveUsersDropDownListByRoleTypeId(Int32 RoleTypeId)
        {
            List<DropDownTO> userList = _idimensionbl.SelectAllSystemUsersFromRoleType(RoleTypeId);
            return userList;
        }

        #endregion


        #region POST

        //Prajakta[2019-04-11] Commented as per disscussion with Saket Dhoble
        // [Route("PostLogin")]
        // [HttpPost]
        // public TblUserTO PostLogin([FromBody] TblUserTO userTO)
        // {
        //     try
        //     {

        //         if (userTO == null)
        //         {
        //             return null;
        //         }
        //         //String[] devices = { "dr5RvjV8_hk:APA91bFrDgE0NFAI8u5-eTVGrG4BGGJywIbHYywxrrLmmTLrC2-pjQLhhA48Tc7WF32hJTkd_Ik60MkfzZJXhcuJupu1hIshP-3ri-FrSQAQQHimCj4CWBfVsmIZB8K8qom3mzLS3x5S" };
        //         //String body = "ddd"; String title = "dddddd";
        //         //string ss = VitplNotify.NotifyToRegisteredDevices(devices,body,title);
        //         //BL.TblLoadingBL.CancelAllNotConfirmedLoadingSlips();
        //         ResultMessage rMessage = new ResultMessage();
        //         rMessage = BL.TblLoginBL.LogIn(userTO);
        //         if (rMessage.MessageType != ResultMessageE.Information)
        //         {
        //             return null;
        //         }
        //         else
        //         {
        //             userTO = (TblUserTO)rMessage.Tag;
        //             return userTO;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return null;
        //     }
        // }

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

                TblUserTO userTo =_iTblUserBL.SelectTblUserTO(TblUserTO.IdUser);
                if (userTo == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "userTO Found NULL";
                    return resultMessage;
                }

                userTo.UserPasswd = TblUserTO.UserPasswd;

                int result = _iTblUserBL.UpdateTblUser(userTo);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error In Updating Password";
                    return resultMessage;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Password Changed Succesfully";
                    return resultMessage;
                }
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

                feedbackTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                feedbackTO.CreatedBy = Convert.ToInt32(loginUserId);

                int result = _iTblFeedbackBL.InsertTblFeedback(feedbackTO);
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

        //Prajakta[2019-04-11] Commented as per disscussion with Saket Dhoble
        // [Route("PostLogOut")]
        // [HttpPost]
        // public int PostLogOut([FromBody] TblUserTO userTO)
        // {
        //     try
        //     {

        //         if (userTO == null)
        //         {
        //             return 0;
        //         }

        //         ResultMessage rMessage = new ResultMessage();
        //         rMessage = BL.TblLoginBL.LogOut(userTO);
        //         if (rMessage.MessageType != ResultMessageE.Information)
        //         {
        //             return 0;
        //         }
        //         else
        //         {
        //             return 1;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return -1;
        //     }
        // }


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

                DateTime confirmedDate =  _iCommonDAO.ServerDateTime;
                for (int i = 0; i < userAreaAllocationTOList.Count; i++)
                {
                    userAreaAllocationTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    userAreaAllocationTOList[i].CreatedOn = confirmedDate;
                    userAreaAllocationTOList[i].IsActive = 1;
                }
                ResultMessage resMsg = null;
                //ResultMessage resMsg = BL.TblUserAreaAllocationBL.SaveUserAreaAllocation(userAreaAllocationTOList);
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

                DateTime confirmedDate =  _iCommonDAO.ServerDateTime;
                permissionTO.CreatedBy = Convert.ToInt32(loginUserId);
                permissionTO.CreatedOn = confirmedDate;

                ResultMessage resMsg = _iTblSysElementsBL.SaveRoleOrUserPermission(permissionTO);
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
                ResultMessage resMsg = _iTblUserBL.SaveNewUser(userTO, userId);
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
                    userTO.DeactivatedOn =  _iCommonDAO.ServerDateTime;
                    int result = _iTblUserBL.UpdateTblUser(userTO);
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
                    ResultMessage resMsg =_iTblUserBL.UpdateUser(userTO, userId);
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


        /// <summary>
        /// code added by swati
        /// Assign supplier to purchase manager
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostSupplierToPurchaseManager")]
        [HttpPost]
        public ResultMessage PostSupplierToPurchaseManager([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseManagerSupplierTO> assignSupplierList = JsonConvert.DeserializeObject<List<TblPurchaseManagerSupplierTO>>(data["permissionTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }
                int result = 0;
                DateTime serverDate =  _iCommonDAO.ServerDateTime;
                var userId = data["userId"].ToString();
                if (assignSupplierList != null && assignSupplierList.Count > 0)
                {
                    for (int q = 0; q < assignSupplierList.Count; q++)
                    {
                        TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO = new TblPurchaseManagerSupplierTO();
                        tblPurchaseManagerSupplierTO.UserId = Convert.ToInt32(userId);
                        tblPurchaseManagerSupplierTO.OrganizationId = assignSupplierList[q].OrganizationId;
                        tblPurchaseManagerSupplierTO.CreatedOn = serverDate;
                        tblPurchaseManagerSupplierTO.CreatedBy = Convert.ToInt32(loginUserId);
                        tblPurchaseManagerSupplierTO.IsActive = (assignSupplierList[q].IsChecked ? 1 : 0);
                        result = _iTblPurchaseManagerSupplierBL.InsertUpdateTblPurchaseManagerSupplier(tblPurchaseManagerSupplierTO);
                    }

                }

                if (result == 1)
                {
                    returnMsg.MessageType = ResultMessageE.Information;
                    returnMsg.Result = 1;
                    returnMsg.Text = "Suppliers Updated Successfully";
                    returnMsg.DisplayMessage = "Suppliers Updated Successfully";

                    return returnMsg;
                }
                else
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API :Record Not Saved";
                    return returnMsg;
                }

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

        #endregion


        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
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

        //public  string EncryptPassword(string password, string salt)
        //{
        //    using (var sha256 = SHA256.Create())
        //    {
        //        //var saltedPassword = string.Format("{0}{1}", salt, password);
        //        var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(password);
        //        return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
        //    }
        //}

        //private string Encrypt(string clearText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}

        //private string Decrypt(string cipherText)
        //{
        //    string EncryptionKey = "MAKV2SPBNI99212";
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}

        #endregion

    }
}
