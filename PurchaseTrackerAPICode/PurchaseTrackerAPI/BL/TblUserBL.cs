using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblUserBL : ITblUserBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblUserExtBL _iTblUserExtBL;
        private readonly ITblRoleBL _iTblRoleBL;
        private readonly ITblAddressBL _iTblAddressBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly IConnectionString _iConnectionString;
        public TblUserBL(ITblUserDAO iTblUserDAO, Icommondao icommondao, IConnectionString iConnectionString, ITblPersonBL iTblPersonBL, ITblAddressBL iTblAddressBL, ITblRoleBL iTblRoleBL, ITblUserRoleBL iTblUserRoleBL, ITblUserExtBL iTblUserExtBL)
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblUserExtBL = iTblUserExtBL;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblRoleBL = iTblRoleBL;
            _iTblAddressBL = iTblAddressBL;
            _iTblPersonBL = iTblPersonBL;
            _iTblUserDAO = iTblUserDAO;
        }
        #region Selection
        
        public  List<TblUserTO> SelectAllTblUserList(Boolean onlyActiveYn)
        {
            return _iTblUserDAO.SelectAllTblUser(onlyActiveYn);
        }

        public  TblUserTO SelectTblUserTO(Int32 idUser)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblUserDAO.SelectTblUser(idUser,conn,tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }            
        }

        public  int SelectUserByImeiNumber(string idDevice)
        {
            TblUserTO tblUserTo = new TblUserTO();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                tblUserTo =  _iTblUserDAO.SelectUserByImeiNumber(idDevice, conn, tran);
                if (tblUserTo != null)
                    return tblUserTo.IdUser;
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }

        public  TblUserTO SelectTblUserTO(Int32 idUser,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.SelectTblUser(idUser, conn, tran);

        }

        public  TblUserTO SelectTblUserTO(String userID,String password)
        {
            return _iTblUserDAO.SelectTblUser(userID,password);
        }

        public  Boolean IsThisUserExists(String userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Boolean result = IsThisUserExists(userId, conn, tran);
                tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public  Boolean IsThisUserExists(String userId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.IsThisUserExists(userId, conn,tran);
        }

        public  Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserMobileNoDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }

        public  Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserDeviceRegNoDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }

        public  List<TblUserTO> SelectAllTblUserList(Int32 orgId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.SelectAllTblUser(orgId,conn,tran);
        }

        public  List<DropDownTO> SelectAllActiveUsersForDropDown()
        {
            return _iTblUserDAO.SelectAllActiveUsersForDropDown();
        }
        public  List<DropDownTO> GetUnloadingPersonListForDropDown(string roleId)
        {
            return _iTblUserDAO.GetUnloadingPersonListForDropDown(roleId);
        }

         public  List<DropDownTO> GetUnloadingPersonListForDropDown(Int32 roleId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.GetUnloadingPersonListForDropDown(roleId,conn,tran);
        }
         
        #endregion

        #region Insertion
        public  int InsertTblUser(TblUserTO tblUserTO)
        {
            return _iTblUserDAO.InsertTblUser(tblUserTO);
        }

        public  int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.InsertTblUser(tblUserTO, conn, tran);
        }

        public  String CreateUserName(string firstName,string lastName, SqlConnection conn,SqlTransaction tran)
        {
            try
            {
                String userName = string.Empty;
                userName = firstName.TrimEnd(' ') + "." + lastName.TrimEnd(' ');
                Boolean isUserExist = true;
                for (int i = 0; i < 5; i++) //Max 5 Is Considered
                {
                    if(i==0)
                    {
                        isUserExist = IsThisUserExists(userName, conn, tran);
                        if (!isUserExist)
                            return userName;
                        else continue;
                    }
                    else
                    {
                        string newUser = userName + i;
                        isUserExist = IsThisUserExists(newUser, conn, tran);
                        if (!isUserExist)
                            return newUser;
                        else continue;
                    }
                }

                userName = userName +  _iCommonDAO.ServerDateTime.ToString("ddMMyyyy");
                return userName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public  ResultMessage SaveNewUser(TblUserTO tblUserTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime =  _iCommonDAO.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                String userId =CreateUserName(tblUserTO.UserPersonTO.FirstName, tblUserTO.UserPersonTO.LastName, conn, tran);
                userId = userId.ToLower();
                String pwd = Constants.DefaultPassword;

                if (tblUserTO.UserPersonTO.DobDay > 0 && tblUserTO.UserPersonTO.DobMonth > 0 && tblUserTO.UserPersonTO.DobYear > 0)
                {
                    tblUserTO.UserPersonTO.DateOfBirth = new DateTime(tblUserTO.UserPersonTO.DobYear, tblUserTO.UserPersonTO.DobMonth, tblUserTO.UserPersonTO.DobDay);
                }
                else
                {
                    tblUserTO.UserPersonTO.DateOfBirth = DateTime.MinValue;
                }

                tblUserTO.UserPersonTO.CreatedBy = loginUserId;
                tblUserTO.UserPersonTO.CreatedOn = serverDateTime;
                int result = _iTblPersonBL.InsertTblPerson(tblUserTO.UserPersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblPerson for Users in Method SaveNewUser ";
                    return rMessage;
                }

                tblUserTO.UserDisplayName = tblUserTO.UserPersonTO.FirstName + " " + tblUserTO.UserPersonTO.LastName;
                tblUserTO.IsActive = 1;
                tblUserTO.UserLogin = userId;
                tblUserTO.UserPasswd = pwd;
                result = InsertTblUser(tblUserTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblUser for Users in Method SaveNewUser ";
                    return rMessage;
                }

                tblUserTO.UserExtTO = new TblUserExtTO();
                tblUserTO.UserExtTO.CreatedBy = loginUserId;
                tblUserTO.UserExtTO.CreatedOn = serverDateTime;
                tblUserTO.UserExtTO.PersonId = tblUserTO.UserPersonTO.IdPerson;
                tblUserTO.UserExtTO.UserId = tblUserTO.IdUser;
                tblUserTO.UserExtTO.OrganizationId = tblUserTO.OrganizationId;
                //add if condition for purchase manager...organization for Purchase manager is not available so address id will save 0
                TblRoleTO tblRole = _iTblRoleBL.SelectTblRoleTO(tblUserTO.UserRoleList[0].RoleId);
                if (tblRole.RoleDesc.ToString().Contains("Purchase Manager"))
                {
                    tblUserTO.UserExtTO.AddressId = 0;
                }
                else
                {
                    TblAddressTO addressTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(tblUserTO.OrganizationId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                    if (addressTO == null)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error..Record could not be saved. Address Details for the organization + " + tblUserTO.OrganizationName + " is not set";
                        rMessage.DisplayMessage = "Error..Record could not be saved. Address Details for the organization + " + tblUserTO.OrganizationName + " is not set";
                        return rMessage;
                    }
                    tblUserTO.UserExtTO.AddressId = addressTO.IdAddr;
                }
                result = _iTblUserExtBL.InsertTblUserExt(tblUserTO.UserExtTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUserExt for Users in Method SaveNewUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                tblUserTO.UserRoleList[0].UserId = tblUserTO.IdUser;
                tblUserTO.UserRoleList[0].IsActive = 1;
                tblUserTO.UserRoleList[0].CreatedBy = loginUserId;
                tblUserTO.UserRoleList[0].CreatedOn = serverDateTime;

                result = _iTblUserRoleBL.InsertTblUserRole(tblUserTO.UserRoleList[0], conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewUser ";
                    return rMessage;
                }

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewUser ";
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        
        #region Updation
        public  int UpdateTblUser(TblUserTO tblUserTO)
        {
            return _iTblUserDAO.UpdateTblUser(tblUserTO);
        }

        public  int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.UpdateTblUser(tblUserTO, conn, tran);
        }

        public  ResultMessage UpdateUser(TblUserTO tblUserTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime =  _iCommonDAO.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                if (tblUserTO.UserPersonTO != null)
                {
                    if (tblUserTO.UserPersonTO.DobDay > 0 && tblUserTO.UserPersonTO.DobMonth > 0 && tblUserTO.UserPersonTO.DobYear > 0)
                    {
                        tblUserTO.UserPersonTO.DateOfBirth = new DateTime(tblUserTO.UserPersonTO.DobYear, tblUserTO.UserPersonTO.DobMonth, tblUserTO.UserPersonTO.DobDay);
                    }
                    else
                    {
                        tblUserTO.UserPersonTO.DateOfBirth = DateTime.MinValue;
                    }

                    result = _iTblPersonBL.UpdateTblPerson(tblUserTO.UserPersonTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While UpdateTblPerson for Users in Method UpdateUser ";
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return rMessage;
                    }

                    tblUserTO.UserDisplayName = tblUserTO.UserPersonTO.FirstName + " " + tblUserTO.UserPersonTO.LastName;

                }

                result = UpdateTblUser(tblUserTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUser for Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                tblUserTO.UserExtTO.PersonId = tblUserTO.UserPersonTO.IdPerson;
                tblUserTO.UserExtTO.UserId = tblUserTO.IdUser;
                tblUserTO.UserExtTO.OrganizationId = tblUserTO.OrganizationId;

                result =_iTblUserExtBL.UpdateTblUserExt(tblUserTO.UserExtTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUserExt for Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                result = _iTblUserRoleBL.UpdateTblUserRole(tblUserTO.UserRoleList[0], conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While UpdateTblUserRole for C&F Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;

                    return rMessage;
                }

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.Text = "Exception Error While UpdateUser Method UpdateUser ";
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Deletion
        public  int DeleteTblUser(Int32 idUser)
        {
            return _iTblUserDAO.DeleteTblUser(idUser);
        }

        public  int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.DeleteTblUser(idUser, conn, tran);
        }

        #endregion
        
    }
}
