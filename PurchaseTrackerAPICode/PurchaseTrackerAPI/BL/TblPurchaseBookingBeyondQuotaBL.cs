using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseBookingBeyondQuotaBL : ITblPurchaseBookingBeyondQuotaBL
    {
        private readonly ITblPurchaseBookingBeyondQuotaDAO _iTblPurchaseBookingBeyondQuotaDAO;
        public TblPurchaseBookingBeyondQuotaBL(ITblPurchaseBookingBeyondQuotaDAO iTblPurchaseBookingBeyondQuotaDAO)
        {
            _iTblPurchaseBookingBeyondQuotaDAO = iTblPurchaseBookingBeyondQuotaDAO;
        }

        #region Selection

        public  List<TblPurchaseBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId)
        {
            return _iTblPurchaseBookingBeyondQuotaDAO.SelectAllStatusHistoryOfBooking(bookingId);
        }

        public  List<TblPurchaseBookingBeyondQuotaTO> SelectAllPurchaseEnquiryHistory(Int32 bookingId)
        {
            return _iTblPurchaseBookingBeyondQuotaDAO.SelectAllPurchaseEnquiryHistory(bookingId);
        }

        #endregion


        #region Insertion
        /*
        public  int InsertTblBookingBeyondQuota(TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            return _iTblRateBandDeclarationPurchaseDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO);
        }

        public  int InsertTblBookingBeyondQuota(TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRateBandDeclarationPurchaseDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
        }
        */
        #endregion
    }
}
