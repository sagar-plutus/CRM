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
    public class TblPmUserBL
    {
        #region Selection
        public static List<TblPmUserTO> SelectAllTblPmUser()
        {
            return TblPmUserDAO.SelectAllTblPmUser();
        }

        public static List<TblPmUserTO> SelectAllTblPmUserList()
        {
             return TblPmUserDAO.SelectAllTblPmUser();
        }
        public static List<TblPmUserTO> SelectAllPMForUser(Int32 loginUserId)
        {
             return TblPmUserDAO.SelectAllPMForUser(loginUserId);
        }

        public static TblPmUserTO SelectTblPmUserTO(Int32 idPmUser)
        {
            List<TblPmUserTO> tblPmUserTOList = TblPmUserDAO.SelectTblPmUser(idPmUser);
            if(tblPmUserTOList != null && tblPmUserTOList.Count == 1)
                return tblPmUserTOList[0];
            else
                return null;
        }

      
        #endregion
        
        #region Insertion
        public static int InsertTblPmUser(TblPmUserTO tblPmUserTO)
        {
            return TblPmUserDAO.InsertTblPmUser(tblPmUserTO);
        }

        public static int InsertTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPmUserDAO.InsertTblPmUser(tblPmUserTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblPmUser(TblPmUserTO tblPmUserTO)
        {
            return TblPmUserDAO.UpdateTblPmUser(tblPmUserTO);
        }

        public static int UpdateTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPmUserDAO.UpdateTblPmUser(tblPmUserTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblPmUser(Int32 idPmUser)
        {
            return TblPmUserDAO.DeleteTblPmUser(idPmUser);
        }

        public static int DeleteTblPmUser(Int32 idPmUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblPmUserDAO.DeleteTblPmUser(idPmUser, conn, tran);
        }

        #endregion
        
    }
}
