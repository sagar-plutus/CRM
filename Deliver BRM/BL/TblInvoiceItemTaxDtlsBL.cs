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
    public class TblInvoiceItemTaxDtlsBL
    {
        #region Selection

        public static List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList()
        {
            return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls();
        }

        public static TblInvoiceItemTaxDtlsTO SelectTblInvoiceItemTaxDtlsTO(Int32 idInvItemTaxDtl)
        {
            return TblInvoiceItemTaxDtlsDAO.SelectTblInvoiceItemTaxDtls(idInvItemTaxDtl);
        }

        public static List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList(Int32 invItemId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls(invItemId, conn, tran);
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
        public static List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList(Int32 invItemId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls(invItemId, conn, tran);
        }

        public static List<TblInvoiceItemTaxDtlsTO> SelectInvoiceItemTaxDtlsListByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtlsByInvoiceId(invoiceId, conn, tran);
        }

        #endregion

        #region Insertion
        public static int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO)
        {
            return TblInvoiceItemTaxDtlsDAO.InsertTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO);
        }

        public static int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.InsertTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO)
        {
            return TblInvoiceItemTaxDtlsDAO.UpdateTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO);
        }

        public static int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.UpdateTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl)
        {
            return TblInvoiceItemTaxDtlsDAO.DeleteTblInvoiceItemTaxDtls(idInvItemTaxDtl);
        }

        public static int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.DeleteTblInvoiceItemTaxDtls(idInvItemTaxDtl, conn, tran);
        }

        public static int DeleteInvoiceItemTaxDtlsByInvId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.DeleteInvoiceItemTaxDtlsByInvId(invoiceId, conn, tran);

        }
        #endregion

    }
}
