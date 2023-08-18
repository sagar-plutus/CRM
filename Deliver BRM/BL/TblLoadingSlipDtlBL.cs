using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.BL
{
    public class TblLoadingSlipDtlBL
    {
        #region Selection
      
        public static List<TblLoadingSlipDtlTO> SelectAllTblLoadingSlipDtlList()
        {
            return TblLoadingSlipDtlDAO.SelectAllTblLoadingSlipDtl();           
        }

        public static TblLoadingSlipDtlTO SelectTblLoadingSlipDtlTO(Int32 idLoadSlipDtl)
        {
            return TblLoadingSlipDtlDAO.SelectTblLoadingSlipDtl(idLoadSlipDtl);
        }

        public static TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId)
        {
            return TblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(loadingSlipId);
        }

        public static TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(loadingSlipId,conn,tran);
        }

        public static List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromLoadingId(Int32 loadingId,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingSlipDtlDAO.SelectAllLoadingSlipDtlListFromLoadingId(loadingId,conn,tran);
        }

        /// <summary>
        /// Vijaymala added [24-04-2018]:added to get loading slip details from bookingId
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromBookingId(Int32 bookingId, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDtlDAO.SelectAllLoadingSlipDtlListFromBookingId(bookingId, conn, tran);
        }
        #endregion

        #region Insertion
        public static int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO)
        {
            return TblLoadingSlipDtlDAO.InsertTblLoadingSlipDtl(tblLoadingSlipDtlTO);
        }

        public static int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDtlDAO.InsertTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO)
        {
            return TblLoadingSlipDtlDAO.UpdateTblLoadingSlipDtl(tblLoadingSlipDtlTO);
        }

        public static int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDtlDAO.UpdateTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl)
        {
            return TblLoadingSlipDtlDAO.DeleteTblLoadingSlipDtl(idLoadSlipDtl);
        }

        public static int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipDtlDAO.DeleteTblLoadingSlipDtl(idLoadSlipDtl, conn, tran);
        }

        #endregion
        
    }
}
