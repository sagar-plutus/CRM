using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryDetailsBL
    {

        int UpdateEnquiryItemPendingQty(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateEnquiryItemsPendingQty(TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsListByEnquiryId(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryDetailsTO> SelectTblEnquiryDetailsList(Int32 purchaseEnquiryId,SqlConnection conn = null,SqlTransaction tran = null);
        int InsertTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO);
        int UpdateTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseEnquiryDetails(Int32 idPurchaseEnquiryDetails, SqlConnection conn, SqlTransaction tran);
        int DeleteAllGradeDetailsForEnquiry(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);


    }
}