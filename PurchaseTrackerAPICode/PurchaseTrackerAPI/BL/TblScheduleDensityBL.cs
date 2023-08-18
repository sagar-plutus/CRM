using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblScheduleDensityBL : ITblScheduleDensityBL
    {

        private readonly ITblScheduleDensityDAO _iTblScheduleDensityDAO;

        public TblScheduleDensityBL(ITblScheduleDensityDAO iTblScheduleDensityDAO)
        {
            _iTblScheduleDensityDAO = iTblScheduleDensityDAO;
        }
        #region Selection
        public List<TblScheduleDensityTO> SelectAllTblScheduleDensity()
        {
            return _iTblScheduleDensityDAO.SelectAllTblScheduleDensity();
        }

        public  List<TblScheduleDensityTO> SelectAllTblScheduleDensityList()
        {
            return _iTblScheduleDensityDAO.SelectAllTblScheduleDensity();

        }

        public TblScheduleDensityTO SelectTblScheduleDensityTO(Int32 idScheduleDensity)
        {
           return _iTblScheduleDensityDAO.SelectTblScheduleDensity(idScheduleDensity);
           
        }
              
        #endregion
        
        #region Insertion
        public int InsertTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO)
        {
            return _iTblScheduleDensityDAO.InsertTblScheduleDensity(tblScheduleDensityTO);
        }

        public int InsertTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblScheduleDensityDAO.InsertTblScheduleDensity(tblScheduleDensityTO, conn, tran);
        }

        #endregion
         
        #region Updation
        public  int UpdateTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO)
        {
            return _iTblScheduleDensityDAO.UpdateTblScheduleDensity(tblScheduleDensityTO);
        }

        public  int UpdateTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblScheduleDensityDAO.UpdateTblScheduleDensity(tblScheduleDensityTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblScheduleDensity(Int32 idScheduleDensity)
        {
            return _iTblScheduleDensityDAO.DeleteTblScheduleDensity(idScheduleDensity);
        }

        public int DeleteTblScheduleDensity(Int32 idScheduleDensity, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblScheduleDensityDAO.DeleteTblScheduleDensity(idScheduleDensity, conn, tran);
        }
        public int DeletePurchaseVehDensityDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblScheduleDensityDAO.DeletePurchaseVehDensityDtls(purchaseScheduleId, conn, tran);
        }

        #endregion

    }
}
