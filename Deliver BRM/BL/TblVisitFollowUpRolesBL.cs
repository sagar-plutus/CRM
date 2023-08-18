using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblVisitFollowUpRolesBL
    {
        #region Selection


        public static List<TblVisitFollowUpRolesTO> SelectAllTblVisitFollowUpRolesList()
        {
            List<TblVisitFollowUpRolesTO> visitFollowUpRolesTOList = TblVisitFollowUpRolesDAO.SelectAllTblVisitFollowUpRoles();
            if (visitFollowUpRolesTOList != null)
                return visitFollowUpRolesTOList;
            else
                return null;
        }

        public static List<DropDownTO> SelectFollowUpUserRoleListForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> followUpUserList = TblVisitFollowUpRolesDAO.SelectFollowUpUserRoleListForDropDown();
                if (followUpUserList != null)
                    return followUpUserList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectFollowupRoleList");
                return null;
            }
        }

        public static List<DropDownTO> SelectFollowUpRoleListForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> followUpUserList = TblVisitFollowUpRolesDAO.SelectFollowUpRoleListForDropDown();
                if (followUpUserList != null)
                    return followUpUserList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectFollowupRoleList");
                return null;
            }
        }

        public static TblVisitFollowUpRolesTO SelectTblVisitFollowUpRolesTO(Int32 idVisitFollowUpRole)
        {
            DataTable tblVisitFollowUpRolesTODT = TblVisitFollowUpRolesDAO.SelectTblVisitFollowUpRoles(idVisitFollowUpRole);
            List<TblVisitFollowUpRolesTO> tblVisitFollowUpRolesTOList = ConvertDTToList(tblVisitFollowUpRolesTODT);
            if (tblVisitFollowUpRolesTOList != null && tblVisitFollowUpRolesTOList.Count == 1)
                return tblVisitFollowUpRolesTOList[0];
            else
                return null;
        }

        public static List<TblVisitFollowUpRolesTO> ConvertDTToList(DataTable tblVisitFollowUpRolesTODT)
        {
            List<TblVisitFollowUpRolesTO> tblVisitFollowUpRolesTOList = new List<TblVisitFollowUpRolesTO>();
            if (tblVisitFollowUpRolesTODT != null)
            {

            }
            return tblVisitFollowUpRolesTOList;
        }

        // Vaibhav [08-Nov-2017] added to select visit role list
        public static List<DropDownTO> SelectVisitRoleListForDropDown()
        {
            List<DropDownTO> visitRoleList = TblVisitFollowUpRolesDAO.SelectVisitRoleForDropDown();
            if (visitRoleList != null)
                return visitRoleList;
            else
                return null;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO)
        {
            return TblVisitFollowUpRolesDAO.InsertTblVisitFollowUpRoles(tblVisitFollowUpRolesTO);
        }

        public static int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFollowUpRolesDAO.InsertTblVisitFollowUpRoles(tblVisitFollowUpRolesTO, conn, tran);
        }

        //Sudhir[20-NOV-2017] For Inserting Record into [tblFollowUpRole]
        public static int InsertTblFollowUpRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFollowUpRolesDAO.InsertTblFollowUpRole(tblRoleTO, conn, tran);
        }
        #endregion

        #region Updation
        public static int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO)
        {
            return TblVisitFollowUpRolesDAO.UpdateTblVisitFollowUpRoles(tblVisitFollowUpRolesTO);
        }

        public static int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFollowUpRolesDAO.UpdateTblVisitFollowUpRoles(tblVisitFollowUpRolesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole)
        {
            return TblVisitFollowUpRolesDAO.DeleteTblVisitFollowUpRoles(idVisitFollowUpRole);
        }

        public static int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFollowUpRolesDAO.DeleteTblVisitFollowUpRoles(idVisitFollowUpRole, conn, tran);
        }

        #endregion

    }
}
