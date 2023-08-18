using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{


    public class TblPurchaseEnquiryHistoryBL : ITblPurchaseEnquiryHistoryBL
    {

        private readonly ITblPurchaseEnquiryHistoryDAO _iTblPurchaseEnquiryHistoryDAO;
        public TblPurchaseEnquiryHistoryBL(ITblPurchaseEnquiryHistoryDAO iTblPurchaseEnquiryHistoryDAO)
        {
            _iTblPurchaseEnquiryHistoryDAO = iTblPurchaseEnquiryHistoryDAO;
        }

        #region Selection
        public   List<TblPurchaseEnquiryHistoryTO> SelectAllTblPurchaseEnquiryHistory()
        {
            return _iTblPurchaseEnquiryHistoryDAO.SelectAllTblPurchaseEnquiryHistory();
        }

        public  List<TblPurchaseEnquiryHistoryTO> SelectAllTblPurchaseEnquiryHistoryList()
        {
             return _iTblPurchaseEnquiryHistoryDAO.SelectAllTblPurchaseEnquiryHistory();
            //return ConvertDTToList(tblPurchaseEnquiryHistoryTODT);
        }

        public  TblPurchaseEnquiryHistoryTO SelectTblPurchaseEnquiryHistoryTO(Int32 idPurchaseEnquiryHistory)
        {
            List<TblPurchaseEnquiryHistoryTO> tblPurchaseEnquiryHistoryTOList = _iTblPurchaseEnquiryHistoryDAO.SelectTblPurchaseEnquiryHistory(idPurchaseEnquiryHistory);
            //List<TblPurchaseEnquiryHistoryTO> tblPurchaseEnquiryHistoryTOList = ConvertDTToList(tblPurchaseEnquiryHistoryTODT);
            if(tblPurchaseEnquiryHistoryTOList != null && tblPurchaseEnquiryHistoryTOList.Count == 1)
                return tblPurchaseEnquiryHistoryTOList[0];
            else
                return null;
        }
        //Priyanka [10-01-2019] : Added to show the status history of enquiry.
        public  List<TblPurchaseEnquiryHistoryTO> SelectAllStatusHistoryOfBookingDetails(Int32 idPurchaseEnquiry)
        {
            return _iTblPurchaseEnquiryHistoryDAO.SelectAllStatusHistoryOfBookingDetails(idPurchaseEnquiry);
            //return ConvertDTToList(tblPurchaseEnquiryHistoryTODT);
        }

        

        #endregion

        #region Insertion
        public  int InsertTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO)
        {
            return _iTblPurchaseEnquiryHistoryDAO.InsertTblPurchaseEnquiryHistory(tblPurchaseEnquiryHistoryTO);
        }

        public  int InsertTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryHistoryDAO.InsertTblPurchaseEnquiryHistory(tblPurchaseEnquiryHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO)
        {
            return _iTblPurchaseEnquiryHistoryDAO.UpdateTblPurchaseEnquiryHistory(tblPurchaseEnquiryHistoryTO);
        }

        public  int UpdateTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryHistoryDAO.UpdateTblPurchaseEnquiryHistory(tblPurchaseEnquiryHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory)
        {
            return _iTblPurchaseEnquiryHistoryDAO.DeleteTblPurchaseEnquiryHistory(idPurchaseEnquiryHistory);
        }

        public  int DeleteTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryHistoryDAO.DeleteTblPurchaseEnquiryHistory(idPurchaseEnquiryHistory, conn, tran);
        }

        #endregion
        
    }
}
