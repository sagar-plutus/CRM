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
    public class TblUserRoleBL
    {
        #region Selection
        
        public static List<TblUserRoleTO> SelectAllTblUserRoleList()
        {
            return TblUserRoleDAO.SelectAllTblUserRole();
        }

        public static List<TblUserRoleTO> SelectAllActiveUserRoleList(Int32 userId)
        {
            return TblUserRoleDAO.SelectAllActiveUserRole(userId);
        }

        /// <summary>
        /// Sudhir[22-AUG-2018] Added Connection , Transaction
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<TblUserRoleTO> SelectAllActiveUserRoleList(Int32 userId, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserRoleDAO.SelectAllActiveUserRole(userId,conn,tran);
        }

        public static TblUserRoleTO SelectTblUserRoleTO(Int32 idUserRole)
        {
            return TblUserRoleDAO.SelectTblUserRole(idUserRole);
            
        }
        public static Int32 IsAreaConfigurationEnabled(Int32 userId)
        {
            int isConfEn = TblUserRoleDAO.IsAreaAllocatedUser(userId);
            return isConfEn;

        }
        public static List<DropDownTO> SelectUsersFromRoleForDropDown(Int32 roleId)
        {
            return TblUserRoleDAO.SelectUsersFromRoleForDropDown(roleId);

        }

        public static List<DropDownTO> SelectUsersFromRoleIdsForDropDown(string roleId)
        {
            return TblUserRoleDAO.SelectUsersFromRoleIdsForDropDown(roleId);

        }

        /// <summary>
        /// Vijaymala[11-01-2019]added to get user role object acc to priority
        /// </summary>
        /// <param name="userRoleTOList"></param>
        /// <returns></returns>
        public static TblUserRoleTO SelectUserRoleTOAccToPriority(List<TblUserRoleTO> userRoleTOList)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            List<TblUserRoleTO> temptblUserRoleTOList = new List<TblUserRoleTO>();
            if (userRoleTOList != null && userRoleTOList.Count > 0)
            {
                temptblUserRoleTOList = userRoleTOList.Where(ele => (ele.EnableAreaAlloc != 1) && (ele.RoleTypeId != (Convert.ToInt32(Constants.SystemRoleTypeE.C_AND_F_AGENT))) && (ele.RoleTypeId != (Convert.ToInt32(Constants.SystemRoleTypeE.Dealer)))).ToList();
                if (temptblUserRoleTOList != null && temptblUserRoleTOList.Count > 0)
                {
                    tblUserRoleTO = temptblUserRoleTOList[0];
                }
                else
                {
                    temptblUserRoleTOList = userRoleTOList.Where(ele => ele.EnableAreaAlloc == 1).ToList();
                    if (temptblUserRoleTOList != null && temptblUserRoleTOList.Count > 0)
                    {
                        tblUserRoleTO = temptblUserRoleTOList[0];
                    }
                    else
                    {
                        temptblUserRoleTOList = userRoleTOList.Where(ele => ele.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.C_AND_F_AGENT)).ToList();
                        if (temptblUserRoleTOList != null && temptblUserRoleTOList.Count > 0)
                        {
                            tblUserRoleTO = temptblUserRoleTOList[0];
                        }
                        else
                        {
                            temptblUserRoleTOList = userRoleTOList.Where(ele => ele.RoleTypeId == Convert.ToInt32(Constants.SystemRoleTypeE.Dealer)).ToList();
                            if (temptblUserRoleTOList != null && temptblUserRoleTOList.Count > 0)
                            {
                                tblUserRoleTO = temptblUserRoleTOList[0];

                            }

                        }
                    }
                }
            }
            return tblUserRoleTO;

        }

        #endregion

        #region Insertion
        public static int InsertTblUserRole(TblUserRoleTO tblUserRoleTO)
        {
            return TblUserRoleDAO.InsertTblUserRole(tblUserRoleTO);
        }

        public static int InsertTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserRoleDAO.InsertTblUserRole(tblUserRoleTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO)
        {
            return TblUserRoleDAO.UpdateTblUserRole(tblUserRoleTO);
        }

        public static int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserRoleDAO.UpdateTblUserRole(tblUserRoleTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblUserRole(Int32 idUserRole)
        {
            return TblUserRoleDAO.DeleteTblUserRole(idUserRole);
        }

        public static int DeleteTblUserRole(Int32 idUserRole, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserRoleDAO.DeleteTblUserRole(idUserRole, conn, tran);
        }

        #endregion
        
    }
}
