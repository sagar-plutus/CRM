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
    public class TblInvoiceItemDetailsBL
    {
        #region Selection
        public static List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList()
        {
            return  TblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails();
        }

        public static TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTO(Int32 idInvoiceItem)
        {
            return  TblInvoiceItemDetailsDAO.SelectTblInvoiceItemDetails(idInvoiceItem);
        }
        /// <summary>
        /// Ramdas.w @14-12-2017 : get other Tax by Invoice 
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        public static TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTOByInvoice(Int32 idInvoice)
        {
            return TblInvoiceItemDetailsDAO.SelectTblInvoiceItemDetailsTOByInvoice(idInvoice);
        }

        public static List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList(Int32 invoiceId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails(invoiceId,conn,tran);
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

        public static List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            return TblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails(invoiceId, conn, tran);
        }

        // Vaibhav [14-nov-2017] added to select invoice item details 
        public static List<TblInvoiceItemDetailsTO> SelectTempInvoiceItemDetailsList(Int32 invoiceId)
        {
            return TblInvoiceItemDetailsDAO.SelectAllTempInvoiceItemDetails(invoiceId);
        }

        #endregion

        #region Insertion
        public static int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            return TblInvoiceItemDetailsDAO.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO);
        }

        public static int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemDetailsDAO.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            return TblInvoiceItemDetailsDAO.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO);
        }

        public static int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemDetailsDAO.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem)
        {
            return TblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetails(idInvoiceItem);
        }

        public static int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetails(idInvoiceItem, conn, tran);
        }

        public static int DeleteTblInvoiceItemDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetailsByInvoiceId(invoiceId, conn, tran);
        }

        #endregion

    }
}
