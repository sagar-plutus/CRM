using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseBookingBeyondQuotaBL
    {
        List<TblPurchaseBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId);
        List<TblPurchaseBookingBeyondQuotaTO> SelectAllPurchaseEnquiryHistory(Int32 bookingId);
     //   int InsertTblBookingBeyondQuota(TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO);
      //  int InsertTblBookingBeyondQuota(TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran);

    }
}