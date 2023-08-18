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
    public class TblQuotaConsumHistoryBL
    {
        #region Selection
       
        public static List<TblQuotaConsumHistoryTO> SelectAllTblQuotaConsumHistoryList()
        {
            return TblQuotaConsumHistoryDAO.SelectAllTblQuotaConsumHistory();
        }

        public static TblQuotaConsumHistoryTO SelectTblQuotaConsumHistoryTO(Int32 idQuotaConsmHIstory)
        {
            return TblQuotaConsumHistoryDAO.SelectTblQuotaConsumHistory(idQuotaConsmHIstory);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO)
        {
            return TblQuotaConsumHistoryDAO.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO);
        }

        public static int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblQuotaConsumHistoryDAO.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO)
        {
            return TblQuotaConsumHistoryDAO.UpdateTblQuotaConsumHistory(tblQuotaConsumHistoryTO);
        }

        public static int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblQuotaConsumHistoryDAO.UpdateTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory)
        {
            return TblQuotaConsumHistoryDAO.DeleteTblQuotaConsumHistory(idQuotaConsmHIstory);
        }

        public static int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory, SqlConnection conn, SqlTransaction tran)
        {
            return TblQuotaConsumHistoryDAO.DeleteTblQuotaConsumHistory(idQuotaConsmHIstory, conn, tran);
        }

        #endregion
        
    }
}
