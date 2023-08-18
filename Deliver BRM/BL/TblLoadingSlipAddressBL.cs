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
    public class TblLoadingSlipAddressBL
    {
        #region Selection
        public static List<TblLoadingSlipAddressTO> SelectAllTblLoadingSlipAddressList(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingSlipAddressDAO.SelectAllTblLoadingSlipAddress(loadingSlipId,conn,tran);
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

        public static List<TblLoadingSlipAddressTO> SelectAllTblLoadingSlipAddressList(Int32 loadingSlipId,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingSlipAddressDAO.SelectAllTblLoadingSlipAddress(loadingSlipId,conn,tran);
        }

        public static TblLoadingSlipAddressTO SelectTblLoadingSlipAddressTO(Int32 idLoadSlipAddr)
        {
            return TblLoadingSlipAddressDAO.SelectTblLoadingSlipAddress(idLoadSlipAddr);
        }

        #endregion
        
        #region Insertion
        public static int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO)
        {
            return TblLoadingSlipAddressDAO.InsertTblLoadingSlipAddress(tblLoadingSlipAddressTO);
        }

        public static int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipAddressDAO.InsertTblLoadingSlipAddress(tblLoadingSlipAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO)
        {
            return TblLoadingSlipAddressDAO.UpdateTblLoadingSlipAddress(tblLoadingSlipAddressTO);
        }

        public static int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipAddressDAO.UpdateTblLoadingSlipAddress(tblLoadingSlipAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr)
        {
            return TblLoadingSlipAddressDAO.DeleteTblLoadingSlipAddress(idLoadSlipAddr);
        }

        public static int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingSlipAddressDAO.DeleteTblLoadingSlipAddress(idLoadSlipAddr, conn, tran);
        }

        #endregion
        
    }
}
