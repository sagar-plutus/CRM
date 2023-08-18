using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryScheduleBL
    {
        List<TblPurchaseEnquiryScheduleTO> SelectAllTblPurchaseEnquiryScheduleList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseEnquirySchedule(TblPurchaseEnquiryScheduleTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseEnquirySchedule(Int32 idSchedulePurchase, SqlConnection conn, SqlTransaction tran);

    }
}