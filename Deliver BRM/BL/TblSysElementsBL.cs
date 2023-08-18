using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblSysElementsBL
    {
        #region Selection
        //public static List<TblSysElementsTO> SelectAllTblSysElementsList(int menuPgId)
        //{
        //    return  TblSysElementsDAO.SelectAllTblSysElements(menuPgId);
        //}

        public static TblSysElementsTO SelectTblSysElementsTO(Int32 idSysElement)
        {
            return  TblSysElementsDAO.SelectTblSysElements(idSysElement);
        }


        /// <summary>
        /// Sanjay [2017-04-20] Following function will return the dictionary of element with its permissions details
        /// for given user and role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static Dictionary<int, String> SelectSysElementUserEntitlementDCT(int userId, int roleId)
        {
            Dictionary<int, String> roleEntitlementDict = DAL.TblSysEleRoleEntitlementsDAO.SelectAllTblSysEleRoleEntitlementsDCT(roleId);
            List<TblSysEleUserEntitlementsTO> userEntitlementList = BL.TblSysEleUserEntitlementsBL.SelectAllTblSysEleUserEntitlementsList(userId);
            return SinkUpDictionaryAndList(ref roleEntitlementDict, userEntitlementList);
        }

        public static Dictionary<int, String> SelectSysElementUserMultiRoleEntitlementDCT(int userId, String roleId, int? moduleId)
        {
            //passed null to get all permissions
            Dictionary<int, String> roleEntitlementDict = TblSysEleRoleEntitlementsDAO.SelectAllTblSysEleMultipleRoleEntitlementsDCT(roleId, null);
            List<TblSysEleUserEntitlementsTO> userEntitlementList = TblSysEleUserEntitlementsBL.SelectAllTblSysEleUserEntitlementsList(userId, null);
            return SinkUpDictionaryAndList(ref roleEntitlementDict, userEntitlementList);
        }

         public static List<PermissionTO>  getAllUsersWithModulePermission(int moduleId,int roled,int DeptId)
        {
            int cnt=0;
            List<PermissionTO> permissionTOList = new List<PermissionTO>();
             List<DropDownTO> userList=new List<DropDownTO>();
                     if(roled!=0)
                    {
                       userList= TblUserRoleDAO.SelectUsersFromRoleForDropDown(roled);                      
                    }
                   else if(DeptId!=0){
                     userList=DimensionDAO.GetUserListDepartmentWise(DeptId.ToString());
                    }
                    else{
                     userList = TblUserDAO.SelectAllActiveUsersForDropDown();
                         }
               
            
            List<TblSysElementsTO> list = TblSysElementsDAO.SelectAllTblSysElements(0, "M", 0);
            TblSysElementsTO tblSysElementsTO = list.Where(w => w.ModuleId == moduleId).FirstOrDefault();
            if (userList != null && userList.Count > 0 && tblSysElementsTO!=null)
            {
                foreach(var item in userList )
                {                     
                     var UserRoleList = TblUserRoleBL.SelectAllActiveUserRoleList(item.Value);
                     int[] roleList = UserRoleList.Where(a => a.IsActive == 1).Select(s => s.RoleId).ToArray();
                     String  roleId = string.Join(",", roleList.ToArray());
                    
                 
                      var SysEleAccessDCT = SelectSysElementUserMultiRoleEntitlementDCT(item.Value, roleId,tblSysElementsTO.IdSysElement);
                      
                      
                           PermissionTO permissionTO = new PermissionTO();
                       
                        permissionTO.IdSysElement = tblSysElementsTO.IdSysElement;
                        permissionTO.MenuId = tblSysElementsTO.MenuId;
                        permissionTO.PageElementId = tblSysElementsTO.PageElementId;
                        permissionTO.Type = tblSysElementsTO.Type;
                        permissionTO.UserId = item.Value;
                        permissionTO.UserName = item.Text;
                                        // Imp person
                       if( SysEleAccessDCT.Count==0)  // if Record is Not Exist
                     {
                        permissionTO.IsboolImpPerson=false;
                    } else{
                        permissionTO.IsImpPerson=TblSysElementsDAO.SelectIsImportantPerson(item.Value, permissionTO.IdSysElement);
                    if(permissionTO.IsImpPerson==1) {
                        permissionTO.IsboolImpPerson=true;
                    } else{
                         permissionTO.IsboolImpPerson=false;   
                          } 
                       }
                        //End
                       if( SysEleAccessDCT.Count==0)  // if Record is Not Exist
                     {  permissionTO.EffectivePermission = "NA";
                       permissionTO.IsPermission=false;
                     }
                       else
                       {
                       var isModulePermission=   SysEleAccessDCT.Where(m=>m.Key==tblSysElementsTO.IdSysElement).FirstOrDefault();
                       if(isModulePermission.Key>0)
                       {
                        permissionTO.EffectivePermission = SysEleAccessDCT[tblSysElementsTO.IdSysElement];
                       }                                           
                     
                        if(permissionTO.EffectivePermission=="RW")
                        {
                             permissionTO.IsPermission=true; 
                             cnt+=1;
                        }
                        else{
                            permissionTO.IsPermission=false;
                        }
                     
                          
                       }
                       permissionTOList.Add(permissionTO);
                     
                }
                           
               
               
            }
            if(permissionTOList!=null && permissionTOList.Count>0)
            {
                permissionTOList[0].ConfigLicenseCnt=cnt.ToString();
            }
            return permissionTOList;
        }

        public static List<TblSysElementsTO> SelectTblSysElementsByModulId(int idModule)
        {
            return TblSysElementsDAO.SelectTblSysElementsByModulId(idModule);

        }

        private static Dictionary<int, string> SinkUpDictionaryAndList(ref Dictionary<int, String> roleEntitlementDict, List<TblSysEleUserEntitlementsTO> userEntitlementList)
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


        public static List<PermissionTO> SelectAllPermissionList(int menuPgId, int roleId, int userId,int moduleId)
        {
            List<PermissionTO> permissionTOList = new List<PermissionTO>();
            var type = "MI";
            if((roleId != 0 || userId != 0) && moduleId == 0)
            {
                type = "M";
            }
            List<TblSysElementsTO> list = TblSysElementsDAO.SelectAllTblSysElements(menuPgId, type, moduleId);
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


        public static Dictionary<int, String> SelectSysElementUserMultiRoleEntitlementDCT(int userId, String roleId)
        {
            Dictionary<int, String> roleEntitlementDict = DAL.TblSysEleRoleEntitlementsDAO.SelectAllTblSysEleMultipleRoleEntitlementsDCT(roleId);
            List<TblSysEleUserEntitlementsTO> userEntitlementList = BL.TblSysEleUserEntitlementsBL.SelectAllTblSysEleUserEntitlementsList(userId);
            return SinkUpDictionaryAndList(ref roleEntitlementDict, userEntitlementList);
        }


        #endregion

        #region Insertion
        public static int InsertTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            return TblSysElementsDAO.InsertTblSysElements(tblSysElementsTO);
        }

        public static int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysElementsDAO.InsertTblSysElements(tblSysElementsTO, conn, tran);
        }

        public static ResultMessage SaveRoleOrUserPermission(PermissionTO permissionTO)
        {
            ResultMessage resultMsg = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (permissionTO.UserId > 0)
                {
                    TblSysEleUserEntitlementsTO userPermissionTO = DAL.TblSysEleUserEntitlementsDAO.SelectUserSysEleUserEntitlements(permissionTO.UserId, permissionTO.IdSysElement, conn, tran);
                    if (userPermissionTO == null)
                    {
                        // Insert New Entry
                        userPermissionTO = new TblSysEleUserEntitlementsTO();
                        userPermissionTO.UserId = permissionTO.UserId;
                        userPermissionTO.Permission = permissionTO.EffectivePermission;
                        userPermissionTO.SysEleId = permissionTO.IdSysElement;
                        userPermissionTO.CreatedBy = permissionTO.CreatedBy;
                        userPermissionTO.CreatedOn = permissionTO.CreatedOn;
                        result = BL.TblSysEleUserEntitlementsBL.InsertTblSysEleUserEntitlements(userPermissionTO, conn, tran);
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
                        result = BL.TblSysEleUserEntitlementsBL.UpdateTblSysEleUserEntitlements(userPermissionTO, conn, tran);
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
                    TblSysEleRoleEntitlementsTO rolePermissionTO = DAL.TblSysEleRoleEntitlementsDAO.SelectRoleSysEleUserEntitlements(permissionTO.RoleId, permissionTO.IdSysElement, conn, tran);
                    if (rolePermissionTO == null)
                    {
                        // Insert New Entry
                        rolePermissionTO = new TblSysEleRoleEntitlementsTO();
                        rolePermissionTO.RoleId = permissionTO.RoleId;
                        rolePermissionTO.Permission = permissionTO.EffectivePermission;
                        rolePermissionTO.SysEleId = permissionTO.IdSysElement;
                        rolePermissionTO.CreatedBy = permissionTO.CreatedBy;
                        rolePermissionTO.CreatedOn = permissionTO.CreatedOn;
                        result = BL.TblSysEleRoleEntitlementsBL.InsertTblSysEleRoleEntitlements(rolePermissionTO, conn, tran);
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
                        result = BL.TblSysEleRoleEntitlementsBL.UpdateTblSysEleRoleEntitlements(rolePermissionTO, conn, tran);
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
        public static int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            return TblSysElementsDAO.UpdateTblSysElements(tblSysElementsTO);
        }

        public static int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysElementsDAO.UpdateTblSysElements(tblSysElementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblSysElements(Int32 idSysElement)
        {
            return TblSysElementsDAO.DeleteTblSysElements(idSysElement);
        }

        public static int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran)
        {
            return TblSysElementsDAO.DeleteTblSysElements(idSysElement, conn, tran);
        }

        #endregion
        
    }
}
