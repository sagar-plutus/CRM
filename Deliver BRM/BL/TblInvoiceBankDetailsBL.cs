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
    public class TblInvoiceBankDetailsBL
    {
        #region Selection
        public static List<TblInvoiceBankDetailsTO> SelectAllTblInvoiceBankDetails()
        {
            return TblInvoiceBankDetailsDAO.SelectAllTblInvoiceBankDetails();
        }

        public static TblInvoiceBankDetailsTO SelectTblInvoiceBankDetailsTO(Int32 idBank)
        {
            return TblInvoiceBankDetailsDAO.SelectTblInvoiceBankDetails(idBank);
        }

        public static List<TblInvoiceBankDetailsTO> SelectInvoiceBankDetails(Int32 organizationId)
        {
            return TblInvoiceBankDetailsDAO.SelectInvoiceBankDetails(organizationId);
        }



        #endregion

        #region Insertion
        public static int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO)
        {
            return TblInvoiceBankDetailsDAO.InsertTblInvoiceBankDetails(tblInvoiceBankDetailsTO);
        }

        public static int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceBankDetailsDAO.InsertTblInvoiceBankDetails(tblInvoiceBankDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO)
        {
            return TblInvoiceBankDetailsDAO.UpdateTblInvoiceBankDetails(tblInvoiceBankDetailsTO);
        }

        public static int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceBankDetailsDAO.UpdateTblInvoiceBankDetails(tblInvoiceBankDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblInvoiceBankDetails(Int32 idBank)
        {
            return TblInvoiceBankDetailsDAO.DeleteTblInvoiceBankDetails(idBank);
        }

        public static int DeleteTblInvoiceBankDetails(Int32 idBank, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceBankDetailsDAO.DeleteTblInvoiceBankDetails(idBank, conn, tran);
        }

        #endregion
        
    }
}
