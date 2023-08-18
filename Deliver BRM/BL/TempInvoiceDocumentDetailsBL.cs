using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TempInvoiceDocumentDetailsBL
    {
        #region Selection
        public static List<TempInvoiceDocumentDetailsTO> SelectAllTempInvoiceDocumentDetails()
        {
            return TempInvoiceDocumentDetailsDAO.SelectAllTempInvoiceDocumentDetails();
        }

        public static TempInvoiceDocumentDetailsTO SelectTempInvoiceDocumentDetailsTO(Int32 idInvoiceDocument)
        {
            return TempInvoiceDocumentDetailsDAO.SelectTempInvoiceDocumentDetails(idInvoiceDocument);
        }

        public static List<TempInvoiceDocumentDetailsTO> SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(Int32 invoiceId)
        {
            return TempInvoiceDocumentDetailsDAO.SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(invoiceId);
        }


        /// <summary>
        /// Vijaymala[28-05-2018] :Added To get invoice document list by using invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static List<TempInvoiceDocumentDetailsTO> SelectTempInvoiceDocumentDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TempInvoiceDocumentDetailsDAO.SelectTempInvoiceDocumentDetailsByInvoiceId(invoiceId, conn, tran);
        }

        #endregion

        #region Insertion
        public static int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO)
        {
            return TempInvoiceDocumentDetailsDAO.InsertTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO);
        }

        public static int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TempInvoiceDocumentDetailsDAO.InsertTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO)
        {
            return TempInvoiceDocumentDetailsDAO.UpdateTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO);
        }

        public static int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TempInvoiceDocumentDetailsDAO.UpdateTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument)
        {
            return TempInvoiceDocumentDetailsDAO.DeleteTempInvoiceDocumentDetails(idInvoiceDocument);
        }

        public static int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument, SqlConnection conn, SqlTransaction tran)
        {
            return TempInvoiceDocumentDetailsDAO.DeleteTempInvoiceDocumentDetails(idInvoiceDocument, conn, tran);
        }
        public static int DeleteTblInvoiceDocumentByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TempInvoiceDocumentDetailsDAO.DeleteTblInvoiceDocumentByInvoiceId(invoiceId, conn, tran);
        }

        #endregion

    }
}
