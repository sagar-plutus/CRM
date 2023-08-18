using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.BL
{
    public class TblTaxRatesBL
    {
        #region Selection

        public static List<TblTaxRatesTO> SelectAllTblTaxRatesList()
        {
            return TblTaxRatesDAO.SelectAllTblTaxRates();
        }

        public static TblTaxRatesTO SelectTblTaxRatesTO()
        {
            return TblTaxRatesDAO.SelectTblTaxRates();
        }

        public static List<TblTaxRatesTO> SelectAllTblTaxRatesList(Int32 idGstCode)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblTaxRatesDAO.SelectAllTblTaxRates(idGstCode, conn, tran);
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

        public static List<TblTaxRatesTO> SelectAllTblTaxRatesList(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return TblTaxRatesDAO.SelectAllTblTaxRates(idGstCode, conn, tran);

        }
        #endregion

        #region Insertion
        public static int InsertTblTaxRates(TblTaxRatesTO tblTaxRatesTO)
        {
            return TblTaxRatesDAO.InsertTblTaxRates(tblTaxRatesTO);
        }

        public static int InsertTblTaxRates(TblTaxRatesTO tblTaxRatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblTaxRatesDAO.InsertTblTaxRates(tblTaxRatesTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblTaxRates(TblTaxRatesTO tblTaxRatesTO)
        {
            return TblTaxRatesDAO.UpdateTblTaxRates(tblTaxRatesTO);
        }

        public static int UpdateTblTaxRates(TblTaxRatesTO tblTaxRatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblTaxRatesDAO.UpdateTblTaxRates(tblTaxRatesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblTaxRates(Int32 idTaxRate)
        {
            return TblTaxRatesDAO.DeleteTblTaxRates(idTaxRate);
        }

        public static int DeleteTblTaxRates(Int32 idTaxRate, SqlConnection conn, SqlTransaction tran)
        {
            return TblTaxRatesDAO.DeleteTblTaxRates(idTaxRate, conn, tran);
        }

        #endregion

    }
}
