using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;
namespace SalesTrackerAPI.BL
{
    public class TblRoleBL
    {
        #region Selection
        public static TblRoleTO SelectAllTblRole()
        {
            return TblRoleDAO.SelectAllTblRole();
        }

        public static List<TblRoleTO> SelectAllTblRoleList()
        {
            TblRoleTO tblRoleTODT = TblRoleDAO.SelectAllTblRole();
            return ConvertDTToList(tblRoleTODT);
        }

        public static TblRoleTO SelectTblRoleTO(Int32 idRole)
        {
            TblRoleTO tblRoleTODT = TblRoleDAO.SelectTblRole(idRole);
            List<TblRoleTO> tblRoleTOList = ConvertDTToList(tblRoleTODT);
            if (tblRoleTOList != null && tblRoleTOList.Count == 1)
                return tblRoleTOList[0];
            else
                return null;
        }

        public static TblRoleTO getDepartmentIdFromUserId(Int32 userId)
        {
            TblRoleTO tblRoleTODT = TblRoleDAO.getDepartmentIdFromUserId(userId);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }

        public static List<TblRoleTO> ConvertDTToList(TblRoleTO tblRoleTODT)
        {
            List<TblRoleTO> tblRoleTOList = new List<TblRoleTO>();
            if (tblRoleTODT != null)
            {
            }
            return tblRoleTOList;
        }

        public static TblRoleTO SelectTblRoleOnOrgStructureId(Int32 orgStructutreId)
        {
            TblRoleTO tblRoleTODT = TblRoleDAO.SelectTblRoleOnOrgStructureId(orgStructutreId);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }

        /// <summary>
        /// Sudhir[22-AUG-2018] Added Connection ,Transaction
        /// </summary>
        /// <param name="orgStructutreId"></param>
        /// <returns></returns>
        public static TblRoleTO SelectTblRoleOnOrgStructureId(Int32 orgStructutreId,SqlConnection conn,SqlTransaction tran)
        {
            TblRoleTO tblRoleTODT = TblRoleDAO.SelectTblRoleOnOrgStructureId(orgStructutreId,conn,tran);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }

        #endregion

        #region Insertion
        public static int InsertTblRole(TblRoleTO tblRoleTO)
        {
            return TblRoleDAO.InsertTblRole(tblRoleTO);
        }

        public static int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblRoleDAO.InsertTblRole(tblRoleTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblRole(TblRoleTO tblRoleTO)
        {
            return TblRoleDAO.UpdateTblRole(tblRoleTO);
        }

        public static int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblRoleDAO.UpdateTblRole(tblRoleTO, conn, tran);
        }

        public static ResultMessage UpdateRoleType(TblOrgStructureTO tblOrgStructureTO)
        {
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();

            TblRoleTO tblRoleTO = TblRoleBL.SelectTblRoleOnOrgStructureId(tblOrgStructureTO.IdOrgStructure);
            if (tblRoleTO != null)
            {
                tblRoleTO.RoleTypeId = tblOrgStructureTO.RoleTypeId;
                result = UpdateTblRole(tblRoleTO);
                if (result < 1)
                {
                    //tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Updating Role Type");
                    // return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                // return resultMessage;
            }
            return resultMessage;
        }


        #endregion

        #region Deletion
        public static int DeleteTblRole(Int32 idRole)
        {
            return TblRoleDAO.DeleteTblRole(idRole);
        }

        public static int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran)
        {
            return TblRoleDAO.DeleteTblRole(idRole, conn, tran);
        }

        #endregion

    }
}
