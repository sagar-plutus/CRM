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
    public class TblSysElementsBL : ITblSysElementsBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblSysEleUserEntitlementsBL _iTblSysEleUserEntitlementsBL;
        private readonly ITblSysEleUserEntitlementsDAO _iTblSysEleUserEntitlementsDAO;
        private readonly ITblSysEleRoleEntitlementsDAO _iTblSysEleRoleEntitlementsDAO;
        private readonly ITblSysElementsDAO _iTblSysElementsDAO;
        private readonly ITblSysEleRoleEntitlementsBL _iTblSysEleRoleEntitlementsBL;
        public TblSysElementsBL(ITblSysElementsDAO iTblSysElementsDAO, IConnectionString iConnectionString, ITblSysEleRoleEntitlementsDAO iTblSysEleRoleEntitlementsDAO, ITblSysEleUserEntitlementsDAO iTblSysEleUserEntitlementsDAO, ITblSysEleUserEntitlementsBL iTblSysEleUserEntitlementsBL, ITblSysEleRoleEntitlementsBL iTblSysEleRoleEntitlementsBL)
        {
            _iConnectionString = iConnectionString;
            _iTblSysEleRoleEntitlementsBL = iTblSysEleRoleEntitlementsBL;
            _iTblSysEleUserEntitlementsBL = iTblSysEleUserEntitlementsBL;
            _iTblSysEleUserEntitlementsDAO = iTblSysEleUserEntitlementsDAO;
            _iTblSysEleRoleEntitlementsDAO = iTblSysEleRoleEntitlementsDAO;
            _iTblSysElementsDAO = iTblSysElementsDAO;
        }
        #region Selection
        public  List<TblSysElementsTO> SelectAllTblSysElementsList(int menuPgId)
        {
            return  _iTblSysElementsDAO.SelectAllTblSysElements(menuPgId);
        }

        public  TblSysElementsTO SelectTblSysElementsTO(Int32 idSysElement)
        {
            return  _iTblSysElementsDAO.SelectTblSysElements(idSysElement);
        }


        /// <summary>
        /// Sanjay [2017-04-20] Following function will return the dictionary of element with its permissions details
        /// for given user and role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public  Dictionary<int, String> SelectSysElementUserEntitlementDCT(int userId, int roleId)
        {
            Dictionary<int, String> roleEntitlementDict = _iTblSysEleRoleEntitlementsDAO.SelectAllTblSysEleRoleEntitlementsDCT(roleId);
            List<TblSysEleUserEntitlementsTO> userEntitlementList = _iTblSysEleUserEntitlementsBL.SelectAllTblSysEleUserEntitlementsList(userId);
            return SinkUpDictionaryAndList(ref roleEntitlementDict, userEntitlementList);
        }


        private  Dictionary<int, string> SinkUpDictionaryAndList(ref Dictionary<int, String> roleEntitlementDict, List<TblSysEleUserEntitlementsTO> userEntitlementList)
        {
            if (userEntitlementList != null && userEntitlementList.Count > 0)
            {
                if (roleEntitlementDict != null && roleEntitlementDict.Count > 0)
                {
                    for (int i = 0; i < userEntitlementList.Count; i++)
                    {
                        //if key present then override else insert
                        if (roleEntitlementDict.ContainsKey(userEntitlementList[i].SysEleId))
                        {
                            roleEntitlementDict[userEntitlementList[i].SysEleId] = userEntitlementList[i].Permission;
                        }
                        else
                        {
                            roleEntitlementDict.Add(userEntitlementList[i].SysEleId, userEntitlementList[i].Permission);
                        }
                    }
                }
                else // create new dictionary and add all user entitlement
                {
                    roleEntitlementDict = new Dictionary<int, string>();
                    for (int i = 0; i < userEntitlementList.Count; i++)
                    {
                        //if key not present then insert else override
                        if (!roleEntitlementDict.ContainsKey(userEntitlementList[i].SysEleId))
                        {
                            roleEntitlementDict.Add(userEntitlementList[i].SysEleId, userEntitlementList[i].Permission);
                        }
                        else
                        {
                            roleEntitlementDict[userEntitlementList[i].SysEleId] = userEntitlementList[i].Permission;
                        }
                    }
                }
            }
            return roleEntitlementDict;
        }


        public  List<PermissionTO> SelectAllPermissionList(int menuPgId, int roleId, int userId)
        {
            List<PermissionTO> permissionTOList = new List<PermissionTO>();
            List<TblSysElementsTO> list = _iTblSysElementsDAO.SelectAllTblSysElements(menuPgId);
            if (list != null)
            {
                Dictionary<int, String> permissionDCT = SelectSysElementUserEntitlementDCT(userId, roleId);

                for (int i = 0; i < list.Count; i++)
                {
                    PermissionTO permissionTO = new PermissionTO();
                    permissionTO.IdSysElement = list[i].IdSysElement;
                    permissionTO.MenuId = list[i].MenuId;
                    permissionTO.PageElementId = list[i].PageElementId;
                    permissionTO.Type = list[i].Type;
                    permissionTO.RoleId = roleId;
                    permissionTO.UserId = userId;
                    permissionTO.ElementName = list[i].ElementName;
                    permissionTO.ElementDesc = list[i].ElementDesc;

                    if (permissionDCT != null && permissionDCT.ContainsKey(list[i].IdSysElement))
                    {
                        permissionTO.EffectivePermission = permissionDCT[list[i].IdSysElement];
                    }
                    else
                        permissionTO.EffectivePermission = "NA";

                    permissionTOList.Add(permissionTO);

                }
            }

            return permissionTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            return _iTblSysElementsDAO.InsertTblSysElements(tblSysElementsTO);
        }

        public  int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysElementsDAO.InsertTblSysElements(tblSysElementsTO, conn, tran);
        }

        public  ResultMessage SaveRoleOrUserPermission(PermissionTO permissionTO)
        {
            ResultMessage resultMsg = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (permissionTO.UserId > 0)
                {
                    TblSysEleUserEntitlementsTO userPermissionTO = _iTblSysEleUserEntitlementsDAO.SelectUserSysEleUserEntitlements(permissionTO.UserId, permissionTO.IdSysElement, conn, tran);
                    if (userPermissionTO == null)
                    {
                        // Insert New Entry
                        userPermissionTO = new TblSysEleUserEntitlementsTO();
                        userPermissionTO.UserId = permissionTO.UserId;
                        userPermissionTO.Permission = permissionTO.EffectivePermission;
                        userPermissionTO.SysEleId = permissionTO.IdSysElement;
                        userPermissionTO.CreatedBy = permissionTO.CreatedBy;
                        userPermissionTO.CreatedOn = permissionTO.CreatedOn;
                        result = _iTblSysEleUserEntitlementsBL.InsertTblSysEleUserEntitlements(userPermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Inserting User Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                    }
                    else
                    {
                        userPermissionTO.Permission = permissionTO.EffectivePermission;
                        result = _iTblSysEleUserEntitlementsBL.UpdateTblSysEleUserEntitlements(userPermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Updating User Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                    }
                }
                else
                {
                    TblSysEleRoleEntitlementsTO rolePermissionTO = _iTblSysEleRoleEntitlementsDAO.SelectRoleSysEleUserEntitlements(permissionTO.RoleId, permissionTO.IdSysElement, conn, tran);
                    if (rolePermissionTO == null)
                    {
                        // Insert New Entry
                        rolePermissionTO = new TblSysEleRoleEntitlementsTO();
                        rolePermissionTO.RoleId = permissionTO.RoleId;
                        rolePermissionTO.Permission = permissionTO.EffectivePermission;
                        rolePermissionTO.SysEleId = permissionTO.IdSysElement;
                        rolePermissionTO.CreatedBy = permissionTO.CreatedBy;
                        rolePermissionTO.CreatedOn = permissionTO.CreatedOn;
                        result =_iTblSysEleRoleEntitlementsBL.InsertTblSysEleRoleEntitlements(rolePermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Inserting role Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                    }
                    else
                    {
                        rolePermissionTO.Permission = permissionTO.EffectivePermission;
                        result = _iTblSysEleRoleEntitlementsBL.UpdateTblSysEleRoleEntitlements(rolePermissionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.MessageType = ResultMessageE.Error;
                            resultMsg.Result = 0;
                            resultMsg.Text = "Error while Updating role Permission";
                            resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                            return resultMsg;
                        }
                    }
                }


                tran.Commit();
                resultMsg.MessageType = ResultMessageE.Information;
                resultMsg.Result = 1;
                resultMsg.Text = "Permission Updated Successfully";
                resultMsg.DisplayMessage = "Permission Updated Successfully";
                return resultMsg;
            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Exception = ex;
                resultMsg.Result = -1;
                resultMsg.Text = "Exception Error While SaveRoleOrUserPermission ";
                resultMsg.DisplayMessage = "Error. Permissions could not be updated";
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        
        #region Updation
        public  int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            return _iTblSysElementsDAO.UpdateTblSysElements(tblSysElementsTO);
        }

        public  int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysElementsDAO.UpdateTblSysElements(tblSysElementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblSysElements(Int32 idSysElement)
        {
            return _iTblSysElementsDAO.DeleteTblSysElements(idSysElement);
        }

        public  int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysElementsDAO.DeleteTblSysElements(idSysElement, conn, tran);
        }

        #endregion
        
    }
}
