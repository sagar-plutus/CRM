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
    public class TblPurchaseEnquiryScheduleBL : ITblPurchaseEnquiryScheduleBL
    {
        private readonly ITblPurchaseEnquiryScheduleDAO _iTblPurchaseEnquiryScheduleDAO;

        public TblPurchaseEnquiryScheduleBL(ITblPurchaseEnquiryScheduleDAO iTblPurchaseEnquiryScheduleDAO)
        {
            _iTblPurchaseEnquiryScheduleDAO = iTblPurchaseEnquiryScheduleDAO;
        }
        
        #region Selection
        public  List<TblPurchaseEnquiryScheduleTO> SelectAllTblPurchaseEnquiryScheduleList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryScheduleDAO.SelectAllTblPurchaseEnquiryScheduleList(purchaseEnquiryId, conn, tran);
        }
        #endregion

        #region Insertion
        public  int InsertTblPurchaseEnquirySchedule(TblPurchaseEnquiryScheduleTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryScheduleDAO.InsertTblPurchaseEnquirySchedule(tblPurchaseEnquiryTO, conn, tran);
        }
        #endregion

        #region Deletion
        public  int DeleteTblPurchaseEnquirySchedule(Int32 idSchedulePurchase, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryScheduleDAO.DeleteTblPurchaseEnquirySchedule(idSchedulePurchase, conn, tran);
        }
        #endregion
    }
}
