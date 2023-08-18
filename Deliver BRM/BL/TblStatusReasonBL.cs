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
    public class TblStatusReasonBL
    {
        #region Selection      

        public static List<TblStatusReasonTO> SelectAllTblStatusReasonList()
        {
            return TblStatusReasonDAO.SelectAllTblStatusReason();
        }

        public static TblStatusReasonTO SelectTblStatusReasonTO(Int32 idStatusReason)
        {
            return  TblStatusReasonDAO.SelectTblStatusReason(idStatusReason);
        }

        /// <summary>
        /// Sanjay [2017-03-06] To Get All the list of reason for given status
        /// if statusid=0 then returns all reasons
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public static List<TblStatusReasonTO> SelectAllTblStatusReasonList(Int32 statusId)
        {
            return TblStatusReasonDAO.SelectAllTblStatusReason(statusId);
        }

        #endregion

        #region Insertion
        public static int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO)
        {
            return TblStatusReasonDAO.InsertTblStatusReason(tblStatusReasonTO);
        }

        public static int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStatusReasonDAO.InsertTblStatusReason(tblStatusReasonTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO)
        {
            return TblStatusReasonDAO.UpdateTblStatusReason(tblStatusReasonTO);
        }

        public static int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStatusReasonDAO.UpdateTblStatusReason(tblStatusReasonTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblStatusReason(Int32 idStatusReason)
        {
            return TblStatusReasonDAO.DeleteTblStatusReason(idStatusReason);
        }

        public static int DeleteTblStatusReason(Int32 idStatusReason, SqlConnection conn, SqlTransaction tran)
        {
            return TblStatusReasonDAO.DeleteTblStatusReason(idStatusReason, conn, tran);
        }

        #endregion
        
    }
}
