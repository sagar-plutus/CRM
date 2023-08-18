using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseScheduleStatusHistoryBL : ITblPurchaseScheduleStatusHistoryBL
    {

        private readonly ITblPurchaseScheduleStatusHistoryDAO _iTblPurchaseScheduleStatusHistoryDAO;
        public TblPurchaseScheduleStatusHistoryBL(ITblPurchaseScheduleStatusHistoryDAO iTblPurchaseScheduleStatusHistoryDAO)
        {
            _iTblPurchaseScheduleStatusHistoryDAO = iTblPurchaseScheduleStatusHistoryDAO;
        }
        #region Selection
        public  List<TblPurchaseScheduleStatusHistoryTO> SelectAllTblPurchaseScheduleStatusHistory()
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.SelectAllTblPurchaseScheduleStatusHistory();
        }

        public  List<TblPurchaseScheduleStatusHistoryTO> SelectAllTblPurchaseScheduleStatusHistoryList()
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.SelectAllTblPurchaseScheduleStatusHistory();
        }

        public  TblPurchaseScheduleStatusHistoryTO SelectTblPurchaseScheduleStatusHistoryTO(Int32 purchaseScheduleSummaryId)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.SelectTblPurchaseScheduleStatusHistory(purchaseScheduleSummaryId);

        }

        public  List<TblPurchaseScheduleStatusHistoryTO> SelectTblPurchaseScheduleStatusHistoryTO(Int32 purchaseScheduleSummaryId, bool getApprovedTO, bool isForApproval, Int32 StatusId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.SelectTblPurchaseScheduleStatusHistory(purchaseScheduleSummaryId, getApprovedTO, isForApproval, StatusId, conn, tran);

        }



        #endregion

        #region Insertion
        public  int InsertTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.InsertTblPurchaseScheduleStatusHistory(tblPurchaseScheduleStatusHistoryTO);
        }

        public  int InsertTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.InsertTblPurchaseScheduleStatusHistory(tblPurchaseScheduleStatusHistoryTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.UpdateTblPurchaseScheduleStatusHistory(tblPurchaseScheduleStatusHistoryTO);
        }

        public  int UpdateTblPurchaseScheduleStatusHistory(TblPurchaseScheduleStatusHistoryTO tblPurchaseScheduleStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.UpdateTblPurchaseScheduleStatusHistory(tblPurchaseScheduleStatusHistoryTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.DeleteTblPurchaseScheduleStatusHistory(purchaseScheduleSummaryId);
        }

        public  int DeleteTblPurchaseScheduleStatusHistory(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.DeleteTblPurchaseScheduleStatusHistory(purchaseScheduleSummaryId, conn, tran);
        }
        public  int DeleteAllStatusHistoryAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleStatusHistoryDAO.DeleteAllStatusHistoryAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }


        #endregion

    }
}
