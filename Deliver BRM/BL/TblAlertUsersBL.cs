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

namespace SalesTrackerAPI.BL
{
    public class TblAlertUsersBL
    {
        #region Selection
        public static List<TblAlertUsersTO> SelectAllTblAlertUsersList()
        {
            return  TblAlertUsersDAO.SelectAllTblAlertUsers();
        }

        public static TblAlertUsersTO SelectTblAlertUsersTO(Int32 idAlertUser)
        {
           return  TblAlertUsersDAO.SelectTblAlertUsers(idAlertUser);
        }

        public static List<TblAlertUsersTO> SelectAllActiveNotAKAlertList(Int32 userId,Int32 roleId)
        {
            return TblAlertUsersDAO.SelectAllActiveNotAKAlertList(userId,roleId);
        }

        public static List<TblAlertUsersTO> SelectAllActiveAlertList(Int32 userId, List<TblUserRoleTO> tblUserRoleToList, int loginId, int ModuleId)
        {
            
            //UserTracking
            int result = CheckUserLogin(loginId, ModuleId, userId);
            String roleIds = String.Empty;
            if (tblUserRoleToList != null && tblUserRoleToList.Count > 0)
            {
                var stringsArray = tblUserRoleToList.Select(i => i.RoleId.ToString()).ToArray();
                roleIds = string.Join(",", stringsArray);
            }
            List<TblAlertUsersTO> list = TblAlertUsersDAO.SelectAllActiveAlertList(userId, roleIds);
            List<TblAlertActionDtlTO> alertActionDtlTOList = BL.TblAlertActionDtlBL.SelectAllTblAlertActionDtlList(userId);
            if (alertActionDtlTOList != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var isAck = alertActionDtlTOList.Where(a => a.AlertInstanceId == list[i].AlertInstanceId).LastOrDefault();
                    if (isAck != null)
                    {
                        if (isAck.ResetDate != DateTime.MinValue)
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                      else
                        {
                            //Hrushikesh added to not to filter snoozed notification

                            list[i].IsAcknowledged = 1;
                            // while time is greater than snooze time dont show notification
                            if (isAck.SnoozeOn > Constants.ServerDateTime)
                            {

                                //Removed from list
                                list.RemoveAt(i);
                                i--;
                            }
                            else if (isAck.SnoozeOn < Constants.ServerDateTime && isAck.SnoozeOn > DateTime.MinValue)
                            {
                                list[i].IsAcknowledged = 0;
                            }

                        }
                    }
                }


                //list = list.OrderByDescending(a => a.IsAcknowledged==0).ThenBy(a=>a.AlertInstanceId).ToList();
                list = list.OrderByDescending(a => a.RaisedOn).ThenBy(a => a.AlertInstanceId).ToList();
                if (list != null && list.Count > 0)
                {
                    List<TblAlertUsersTO> accList = list.Where(w => w.IsAcknowledged == 1).ToList();

                    List<TblAlertUsersTO> notAccList = list.Where(w => w.IsAcknowledged == 0).ToList();

                    list = new List<TblAlertUsersTO>();

                    list.AddRange(notAccList);
                    list.AddRange(accList);
                }

            }

            if (list != null && list.Count > 0)
            {

                list = list.GroupBy(ele => ele.AlertInstanceId).Select(s => s.FirstOrDefault()).ToList();
            }
                        //UserTracking/
            // if(result==1)
            if (result == 1 || result == 2)
            {
                //logout user by loginID
                var InvalidSessionHis = new TblModuleCommHisTO();
                InvalidSessionHis.LoginId = loginId;
                TblModuleBL.UpdateTblModuleCommHisBeforeLogin(InvalidSessionHis, null, null);
            }
            if (list != null && list.Count > 0)
            {
                list[0].IsLogOut = result;
            }
            else
            {
                TblAlertUsersTO a = new TblAlertUsersTO();
                //Hrushikesh added to not to show empty notification
                a.IsAcknowledged = 1;
                a.IsLogOut = result;
                list.Add(a);
            }
            //end
            return list;
        }


 // check Logout Entry user Tracking
        public static int CheckUserLogin(int loginId, int ModuleId, int userId)
        {

            int result = CommonDAO.CheckLogOutEntry(loginId);
            if (result == 0 && ModuleId != 0 && userId != 0)
            {
                TblModuleTO tblModule = TblModuleBL.GetAllActiveAllowedCnt(ModuleId, userId, loginId);
                if ((tblModule.NoOfActiveLicenseCnt > tblModule.NoOfAllowedLicenseCnt) && tblModule.IsImpPerson != 1)
                {
                    return 1;
                }
                if ((tblModule.ImpPersonCount > tblModule.NoOfAllowedLicenseCnt) && tblModule.IsImpPerson == 1)
                {
                    return 1;
                }
                int userActive = CommonDAO.IsUserDeactivate(userId);
                if (userActive == 0)
                {
                    // return 1;
                    return 2; //user Deactivate
                }
            }
            return result;
        }
        //End
        #endregion

        #region Insertion
        public static int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            return TblAlertUsersDAO.InsertTblAlertUsers(tblAlertUsersTO);
        }

        public static int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertUsersDAO.InsertTblAlertUsers(tblAlertUsersTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            return TblAlertUsersDAO.UpdateTblAlertUsers(tblAlertUsersTO);
        }

        public static int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertUsersDAO.UpdateTblAlertUsers(tblAlertUsersTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblAlertUsers(Int32 idAlertUser)
        {
            return TblAlertUsersDAO.DeleteTblAlertUsers(idAlertUser);
        }

        public static int DeleteTblAlertUsers(Int32 idAlertUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertUsersDAO.DeleteTblAlertUsers(idAlertUser, conn, tran);
        }

        #endregion
        
    }
}
