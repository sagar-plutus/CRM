using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryHistoryDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseEnquiryHistoryTO> SelectAllTblPurchaseEnquiryHistory();
        List<TblPurchaseEnquiryHistoryTO> SelectAllStatusHistoryOfBookingDetails(Int32 idPurchaseEnquiry);
        List<TblPurchaseEnquiryHistoryTO> SelectTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory);
        List<TblPurchaseEnquiryHistoryTO> SelectAllTblPurchaseEnquiryHistory(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryHistoryTO> SelectAllStatusHistoryOfBooking(Int32 bookingId);
        List<TblPurchaseEnquiryHistoryTO> ConvertDTToList(SqlDataReader tblPurchaseEnquiryHistoryTODT);
        int InsertTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO);
        int InsertTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO);
        int UpdateTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory);
        int DeleteTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchaseEnquiryHistory, SqlCommand cmdDelete);

    }
}