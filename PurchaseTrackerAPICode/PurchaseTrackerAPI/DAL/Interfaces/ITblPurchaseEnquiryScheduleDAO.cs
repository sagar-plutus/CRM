using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryScheduleDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseEnquiryScheduleTO> SelectAllTblPurchaseEnquiryScheduleList(Int32 purchaseEnquiryId);
        List<TblPurchaseEnquiryScheduleTO> SelectAllTblPurchaseEnquiryScheduleList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryScheduleTO> ConvertDTToList(SqlDataReader tblPurchaseEnquiryScheduleTODT);
        int InsertTblPurchaseEnquirySchedule(TblPurchaseEnquiryScheduleTO tblPurchaseEnquiryScheduleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseEnquiryScheduleTO tblPurchaseEnquiryScheduleTO, SqlCommand cmdInsert);
        int DeleteTblPurchaseEnquirySchedule(Int32 idSchedulePurchase, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSchedulePurchase, SqlCommand cmdDelete);

    } }