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
    public class TblPurchaseInvoiceHistoryBL : ITblPurchaseInvoiceHistoryBL
    {
        private readonly ITblPurchaseInvoiceHistoryDAO _iTblPurchaseInvoiceHistoryDAO;
        public TblPurchaseInvoiceHistoryBL(ITblPurchaseInvoiceHistoryDAO iTblPurchaseInvoiceHistoryDAO)
        {
            _iTblPurchaseInvoiceHistoryDAO = iTblPurchaseInvoiceHistoryDAO;
        }
        #region Selection
        public  List<TblPurchaseInvoiceHistoryTO> SelectAllTblPurchaseInvoiceHistory()
        {
            return _iTblPurchaseInvoiceHistoryDAO.SelectAllTblPurchaseInvoiceHistory();
        }

        public  List<TblPurchaseInvoiceHistoryTO> SelectAllTblPurchaseInvoiceHistoryList()
        {
            return _iTblPurchaseInvoiceHistoryDAO.SelectAllTblPurchaseInvoiceHistory();
        }

        public  TblPurchaseInvoiceHistoryTO SelectTblPurchaseInvoiceHistoryTO(Int64 idPurchaseInvHistory)
        {
            return _iTblPurchaseInvoiceHistoryDAO.SelectTblPurchaseInvoiceHistory(idPurchaseInvHistory);
        }

      
        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO)
        {
            return _iTblPurchaseInvoiceHistoryDAO.InsertTblPurchaseInvoiceHistory(tblPurchaseInvoiceHistoryTO);
        }

        public  int InsertTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceHistoryDAO.InsertTblPurchaseInvoiceHistory(tblPurchaseInvoiceHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO)
        {
            return _iTblPurchaseInvoiceHistoryDAO.UpdateTblPurchaseInvoiceHistory(tblPurchaseInvoiceHistoryTO);
        }

        public  int UpdateTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceHistoryDAO.UpdateTblPurchaseInvoiceHistory(tblPurchaseInvoiceHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory)
        {
            return _iTblPurchaseInvoiceHistoryDAO.DeleteTblPurchaseInvoiceHistory(idPurchaseInvHistory);
        }

        public  int DeleteTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceHistoryDAO.DeleteTblPurchaseInvoiceHistory(idPurchaseInvHistory, conn, tran);
        }

        #endregion
        
    }
}
