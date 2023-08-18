using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblTaxRatesBL : ITblTaxRatesBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblTaxRatesDAO _iTblTaxRatesDAO;
        public TblTaxRatesBL(ITblTaxRatesDAO iTblTaxRatesDAO, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iTblTaxRatesDAO = iTblTaxRatesDAO;
        }
        #region Selection

        public  List<TblTaxRatesTO> SelectAllTblTaxRatesList()
        {
            return _iTblTaxRatesDAO.SelectAllTblTaxRates();
        }

        public  TblTaxRatesTO SelectTblTaxRatesTO()
        {
            return _iTblTaxRatesDAO.SelectTblTaxRates();
        }

        public  List<TblTaxRatesTO> SelectAllTblTaxRatesList(Int32 idGstCode)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblTaxRatesDAO.SelectAllTblTaxRates(idGstCode, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public  List<TblTaxRatesTO> SelectAllTblTaxRatesList(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTaxRatesDAO.SelectAllTblTaxRates(idGstCode, conn, tran);

        }
        #endregion

        #region Insertion
        public  int InsertTblTaxRates(TblTaxRatesTO tblTaxRatesTO)
        {
            return _iTblTaxRatesDAO.InsertTblTaxRates(tblTaxRatesTO);
        }

        public  int InsertTblTaxRates(TblTaxRatesTO tblTaxRatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTaxRatesDAO.InsertTblTaxRates(tblTaxRatesTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblTaxRates(TblTaxRatesTO tblTaxRatesTO)
        {
            return _iTblTaxRatesDAO.UpdateTblTaxRates(tblTaxRatesTO);
        }

        public  int UpdateTblTaxRates(TblTaxRatesTO tblTaxRatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTaxRatesDAO.UpdateTblTaxRates(tblTaxRatesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblTaxRates(Int32 idTaxRate)
        {
            return _iTblTaxRatesDAO.DeleteTblTaxRates(idTaxRate);
        }

        public  int DeleteTblTaxRates(Int32 idTaxRate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTaxRatesDAO.DeleteTblTaxRates(idTaxRate, conn, tran);
        }

        #endregion

    }
}
