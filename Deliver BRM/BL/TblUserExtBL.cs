using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.BL
{
    public class TblUserExtBL
    {
        #region Selection
        
        public static List<TblUserExtTO> SelectAllTblUserExtList()
        {
            return TblUserExtDAO.SelectAllTblUserExt();
        }

        public static TblUserExtTO SelectTblUserExtTO(Int32 userId)
        {
            return TblUserExtDAO.SelectTblUserExt(userId);
        }

        

        #endregion
        
        #region Insertion
        public static int InsertTblUserExt(TblUserExtTO tblUserExtTO)
        {
            return TblUserExtDAO.InsertTblUserExt(tblUserExtTO);
        }

        public static int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserExtDAO.InsertTblUserExt(tblUserExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblUserExt(TblUserExtTO tblUserExtTO)
        {
            return TblUserExtDAO.UpdateTblUserExt(tblUserExtTO);
        }

        public static int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserExtDAO.UpdateTblUserExt(tblUserExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblUserExt()
        {
            return TblUserExtDAO.DeleteTblUserExt();
        }

        public static int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran)
        {
            return TblUserExtDAO.DeleteTblUserExt(conn, tran);
        }

        #endregion
        
    }
}
