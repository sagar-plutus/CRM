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
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblLoginBL : ITblLoginBL
    {
        private readonly ITblLoginDAO _iTblLoginDAO;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblMenuStructureBL _iTblMenuStructureBL;
        private readonly ITblSysElementsBL _iTblSysElementsBL;
        private readonly Icommondao _iCommonDAO;

        public TblLoginBL(ITblLoginDAO iTblLoginDAO, ITblUserRoleBL iTblUserRoleBL, ITblUserBL iTblUserBL, ITblMenuStructureBL iTblMenuStructureBL
                          , ITblSysElementsBL iTblSysElementsBL,
            Icommondao icommondao)
        {
                            _iCommonDAO = icommondao;
            _iTblSysElementsBL = iTblSysElementsBL;
            _iTblMenuStructureBL = iTblMenuStructureBL;
            _iTblUserBL= iTblUserBL;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblLoginDAO = iTblLoginDAO;
        }

        #region Selection
      
        public  List<TblLoginTO> SelectAllTblLoginList()
        {
           return  _iTblLoginDAO.SelectAllTblLogin();
        }

        public  TblLoginTO SelectTblLoginTO(Int32 idLogin)
        {
            return _iTblLoginDAO.SelectTblLogin(idLogin);
        }

       

        #endregion
        
        #region Insertion
        public  int InsertTblLogin(TblLoginTO tblLoginTO)
        {
            return _iTblLoginDAO.InsertTblLogin(tblLoginTO);
        }

        public  int InsertTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoginDAO.InsertTblLogin(tblLoginTO, conn, tran);
        }

        public  ResultMessage LogIn(TblUserTO tblUserTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                #region 1. Check Is This user Exists First

                TblUserTO userExistUserTO = _iTblUserBL.SelectTblUserTO(tblUserTO.UserLogin, tblUserTO.UserPasswd);
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
                    userExistUserTO.UserRoleList = _iTblUserRoleBL.SelectAllActiveUserRoleList(userExistUserTO.IdUser);
                    if (userExistUserTO.UserRoleList == null || userExistUserTO.UserRoleList.Count == 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Your Role Is Not Defined In The System , Please contact your system admin";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    int roleId = userExistUserTO.UserRoleList.Where(a => a.IsActive == 1).FirstOrDefault().RoleId;

                    userExistUserTO.SysEleAccessDCT = _iTblSysElementsBL.SelectSysElementUserEntitlementDCT(userExistUserTO.IdUser, roleId);
                    if (userExistUserTO.SysEleAccessDCT == null || userExistUserTO.SysEleAccessDCT.Count == 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "SysEleAccessDCT Not Found";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    List<TblMenuStructureTO> allMenuList = _iTblMenuStructureBL.SelectAllTblMenuStructureList().OrderBy(s => s.SerNo).ToList();
                    userExistUserTO.MenuStructureTOList = new List<TblMenuStructureTO>();

                    for (int m = 0; m < allMenuList.Count; m++)
                    {
                        if (userExistUserTO.SysEleAccessDCT.ContainsKey(allMenuList[m].SysElementId))
                        {
                            if (userExistUserTO.SysEleAccessDCT[allMenuList[m].SysElementId] == "RW")
                                userExistUserTO.MenuStructureTOList.Add(allMenuList[m]);
                        }
                    }

                }

                #region 2. Mark Login Entry
                userExistUserTO.LoginTO = tblUserTO.LoginTO;
                userExistUserTO.LoginTO.LoginDate =  _iCommonDAO.ServerDateTime;
                userExistUserTO.LoginTO.UserId = userExistUserTO.IdUser;
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
                    && String.IsNullOrEmpty(userExistUserTO.ImeiNumber))
                {
                    if (!string.IsNullOrEmpty(tblUserTO.RegisteredDeviceId)
                        && !string.IsNullOrEmpty(tblUserTO.ImeiNumber))
                    {
                        userExistUserTO.RegisteredDeviceId = tblUserTO.RegisteredDeviceId;
                        userExistUserTO.ImeiNumber = tblUserTO.ImeiNumber;
                        _iTblUserBL.UpdateTblUser(userExistUserTO);
                    }
                }

                #endregion

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

        public  ResultMessage LogOut(TblUserTO tblUserTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                #region 1. Check Is This user Exists First

                TblUserTO userExistUserTO = _iTblUserBL.SelectTblUserTO(tblUserTO.UserLogin, tblUserTO.UserPasswd);
                if (userExistUserTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "User Not Found";
                    return resultMessage;
                }

                #endregion

                #region 2. Update Login Entry
                TblLoginTO loginTO = tblUserTO.LoginTO;
                loginTO.LogoutDate =  _iCommonDAO.ServerDateTime;
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

        #endregion

        #region Updation
        public  int UpdateTblLogin(TblLoginTO tblLoginTO)
        {
            return _iTblLoginDAO.UpdateTblLogin(tblLoginTO);
        }

        public  int UpdateTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoginDAO.UpdateTblLogin(tblLoginTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblLogin(Int32 idLogin)
        {
            return _iTblLoginDAO.DeleteTblLogin(idLogin);
        }

        public  int DeleteTblLogin(Int32 idLogin, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoginDAO.DeleteTblLogin(idLogin, conn, tran);
        }

        #endregion
        
    }
}
