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
    public class TblOtherTaxesBL
    {
        #region Selection
       
        public static List<TblOtherTaxesTO> SelectAllTblOtherTaxesList()
        {
           return  TblOtherTaxesDAO.SelectAllTblOtherTaxes();
        }

        public static TblOtherTaxesTO SelectTblOtherTaxesTO(Int32 idOtherTax)
        {
            return  TblOtherTaxesDAO.SelectTblOtherTaxes(idOtherTax);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO)
        {
            return TblOtherTaxesDAO.InsertTblOtherTaxes(tblOtherTaxesTO);
        }

        public static int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherTaxesDAO.InsertTblOtherTaxes(tblOtherTaxesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO)
        {
            return TblOtherTaxesDAO.UpdateTblOtherTaxes(tblOtherTaxesTO);
        }

        public static int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherTaxesDAO.UpdateTblOtherTaxes(tblOtherTaxesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblOtherTaxes(Int32 idOtherTax)
        {
            return TblOtherTaxesDAO.DeleteTblOtherTaxes(idOtherTax);
        }

        public static int DeleteTblOtherTaxes(Int32 idOtherTax, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherTaxesDAO.DeleteTblOtherTaxes(idOtherTax, conn, tran);
        }

        #endregion
        
    }
}
