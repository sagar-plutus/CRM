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
    public class TblConfigParamHistoryBL
    {
        #region Selection

        public static List<TblConfigParamHistoryTO> SelectAllTblConfigParamHistoryList()
        {
            return TblConfigParamHistoryDAO.SelectAllTblConfigParamHistory();
        }

        public static TblConfigParamHistoryTO SelectTblConfigParamHistoryTO(Int32 idParamHistory)
        {
            return  TblConfigParamHistoryDAO.SelectTblConfigParamHistory(idParamHistory);
        }

        

        #endregion
        
        #region Insertion
        public static int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            return TblConfigParamHistoryDAO.InsertTblConfigParamHistory(tblConfigParamHistoryTO);
        }

        public static int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamHistoryDAO.InsertTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            return TblConfigParamHistoryDAO.UpdateTblConfigParamHistory(tblConfigParamHistoryTO);
        }

        public static int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamHistoryDAO.UpdateTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblConfigParamHistory(Int32 idParamHistory)
        {
            return TblConfigParamHistoryDAO.DeleteTblConfigParamHistory(idParamHistory);
        }

        public static int DeleteTblConfigParamHistory(Int32 idParamHistory, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamHistoryDAO.DeleteTblConfigParamHistory(idParamHistory, conn, tran);
        }

        #endregion
        
    }
}
