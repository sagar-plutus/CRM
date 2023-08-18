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
    public class TblInvoiceAddressBL
    {
        #region Selection
        public static List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList()
        {
            return  TblInvoiceAddressDAO.SelectAllTblInvoiceAddress();
        }

        public static TblInvoiceAddressTO SelectTblInvoiceAddressTO(Int32 idInvoiceAddr)
        {
            return  TblInvoiceAddressDAO.SelectTblInvoiceAddress(idInvoiceAddr);
        }

        public static List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList(Int32 invoiceId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceAddressDAO.SelectAllTblInvoiceAddress(invoiceId,conn,tran);
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

        public static List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.SelectAllTblInvoiceAddress(invoiceId, conn, tran);
        }

        // Vaibhav [14-Nov-2017] added to select invoice address details
        public static List<TblInvoiceAddressTO> SelectTempInvoiceAddressList(Int32 invoiceId)
        {
            return TblInvoiceAddressDAO.SelectAllTempInvoiceAddress(invoiceId);
        }

        #endregion

        #region Insertion
        public static int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            return TblInvoiceAddressDAO.InsertTblInvoiceAddress(tblInvoiceAddressTO);
        }

        public static int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.InsertTblInvoiceAddress(tblInvoiceAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            return TblInvoiceAddressDAO.UpdateTblInvoiceAddress(tblInvoiceAddressTO);
        }

        public static int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.UpdateTblInvoiceAddress(tblInvoiceAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblInvoiceAddress(Int32 idInvoiceAddr)
        {
            return TblInvoiceAddressDAO.DeleteTblInvoiceAddress(idInvoiceAddr);
        }

        public static int DeleteTblInvoiceAddress(Int32 idInvoiceAddr, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.DeleteTblInvoiceAddress(idInvoiceAddr, conn, tran);
        }

        public static int DeleteTblInvoiceAddressByinvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.DeleteTblInvoiceAddressByinvoiceId(invoiceId, conn, tran);
        }

        #endregion

    }
}
