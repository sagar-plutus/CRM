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
    public class TblInvoiceHistoryBL
    {
        #region Selection

        public static List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryList()
        {
            return TblInvoiceHistoryDAO.SelectAllTblInvoiceHistory();
        }


        public static TblInvoiceHistoryTO SelectTblInvoiceHistoryTO(Int32 idInvHistory)
        {
            return TblInvoiceHistoryDAO.SelectTblInvoiceHistory(idInvHistory);
        }

        public static TblInvoiceHistoryTO SelectTblInvoiceHistoryTORateByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.SelectTblInvoiceHistoryTORateByInvoiceItemId(invoiceItemId, conn, tran);
        }

        public static List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryById(Int32 byId, Boolean isByInvoiceId = false)
        {
            return TblInvoiceHistoryDAO.SelectAllTblInvoiceHistoryById(byId, isByInvoiceId);
        }

        // Vaibhav [15-Nov-2017] added to select invoice history data
        public static List<TblInvoiceHistoryTO> SelectTempInvoiceHistory(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.SelectTempInvoiceHistory(invoiceId,conn,tran);
        }

        //Vijaymala [24/01/2018] : Added To Get Invoice History When Cd Change
        public static TblInvoiceHistoryTO SelectTblInvoiceHistoryTOCdByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.SelectTblInvoiceHistoryTOCdByInvoiceItemId(invoiceItemId, conn, tran);
        }
        #endregion

        #region Insertion
        public static int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO)
        {
            return TblInvoiceHistoryDAO.InsertTblInvoiceHistory(tblInvoiceHistoryTO);
        }

        public static int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.InsertTblInvoiceHistory(tblInvoiceHistoryTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO)
        {
            return TblInvoiceHistoryDAO.UpdateTblInvoiceHistory(tblInvoiceHistoryTO);
        }

        public static int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.UpdateTblInvoiceHistory(tblInvoiceHistoryTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblInvoiceHistory(Int32 idInvHistory)
        {
            return TblInvoiceHistoryDAO.DeleteTblInvoiceHistory(idInvHistory);
        }

        public static int DeleteTblInvoiceHistory(Int32 idInvHistory, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.DeleteTblInvoiceHistory(idInvHistory, conn, tran);
        }

        public static int DeleteTblInvoiceHistoryByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.DeleteTblInvoiceHistoryByInvoiceId(invoiceId, conn, tran);
        }

        #endregion

    }
}
