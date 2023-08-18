using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryHistoryBL
    {
        List<TblPurchaseEnquiryHistoryTO> SelectAllTblPurchaseEnquiryHistory();
        List<TblPurchaseEnquiryHistoryTO> SelectAllTblPurchaseEnquiryHistoryList();
        TblPurchaseEnquiryHistoryTO SelectTblPurchaseEnquiryHistoryTO(Int32 idPurchaseEnquiryHistory);
        List<TblPurchaseEnquiryHistoryTO> SelectAllStatusHistoryOfBookingDetails(Int32 idPurchaseEnquiry);
        int InsertTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO);
        int InsertTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO);
        int UpdateTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory);
        int DeleteTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory, SqlConnection conn, SqlTransaction tran);

    }
}