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
    public class TblInvoiceOtherDetailsBL
    {
        #region Selection
        public static List<TblInvoiceOtherDetailsTO> SelectAllTblInvoiceOtherDetails()
        {
            return TblInvoiceOtherDetailsDAO.SelectAllTblInvoiceOtherDetails();
        }


        public static TblInvoiceOtherDetailsTO SelectTblInvoiceOtherDetailsTO(Int32 idInvoiceOtherDetails)
        {
            return TblInvoiceOtherDetailsDAO.SelectTblInvoiceOtherDetails(idInvoiceOtherDetails);
        }
        public static List<TblInvoiceOtherDetailsTO> SelectInvoiceOtherDetails(Int32 organizationId)
        {
            return TblInvoiceOtherDetailsDAO.SelectInvoiceOtherDetails(organizationId);
        }


        #endregion

        #region Insertion
        public static int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO)
        {
            return TblInvoiceOtherDetailsDAO.InsertTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO);
        }

        public static int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceOtherDetailsDAO.InsertTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO)
        {
            return TblInvoiceOtherDetailsDAO.UpdateTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO);
        }

        public static int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceOtherDetailsDAO.UpdateTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails)
        {
            return TblInvoiceOtherDetailsDAO.DeleteTblInvoiceOtherDetails(idInvoiceOtherDetails);
        }

        public static int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceOtherDetailsDAO.DeleteTblInvoiceOtherDetails(idInvoiceOtherDetails, conn, tran);
        }

        #endregion
        
    }
}
