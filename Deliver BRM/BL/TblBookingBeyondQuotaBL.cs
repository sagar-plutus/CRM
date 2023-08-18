using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using Microsoft.Extensions.Logging;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblBookingBeyondQuotaBL
    {
        #region Selection
       
        public static List<TblBookingBeyondQuotaTO> SelectAllTblBookingBeyondQuotaList()
        {
            return TblBookingBeyondQuotaDAO.SelectAllTblBookingBeyondQuota();
            
        }

        public static TblBookingBeyondQuotaTO SelectTblBookingBeyondQuotaTO(Int32 idBookingAuth)
        {
           return TblBookingBeyondQuotaDAO.SelectTblBookingBeyondQuota(idBookingAuth);
        }

        public static List<TblBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId)
        {
            return TblBookingBeyondQuotaDAO.SelectAllStatusHistoryOfBooking(bookingId);

        }

        #endregion

        #region Insertion
        public static int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            return TblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO);
        }

        public static int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            return TblBookingBeyondQuotaDAO.UpdateTblBookingBeyondQuota(tblBookingBeyondQuotaTO);
        }

        public static int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingBeyondQuotaDAO.UpdateTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblBookingBeyondQuota(Int32 idBookingAuth)
        {
            return TblBookingBeyondQuotaDAO.DeleteTblBookingBeyondQuota(idBookingAuth);
        }

        public static int DeleteTblBookingBeyondQuota(Int32 idBookingAuth, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingBeyondQuotaDAO.DeleteTblBookingBeyondQuota(idBookingAuth, conn, tran);
        }

        #endregion
        
    }
}
