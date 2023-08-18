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
    public class TblPagesBL
    {
        #region Selection

        public static List<TblPagesTO> SelectAllTblPagesList()
        {
            return  TblPagesDAO.SelectAllTblPages();
        }

        internal static List<TblPagesTO> SelectAllTblPagesList(int moduleId)
        {
            return TblPagesDAO.SelectAllTblPages(moduleId);
        }

        public static TblPagesTO SelectTblPagesTO(Int32 idPage)
        {
            return  TblPagesDAO.SelectTblPages(idPage);
        }

        #endregion
        
        #region Insertion
        public static int InsertTblPages(TblPagesTO tblPagesTO)
        {
            return TblPagesDAO.InsertTblPages(tblPagesTO);
        }

        public static int InsertTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPagesDAO.InsertTblPages(tblPagesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblPages(TblPagesTO tblPagesTO)
        {
            return TblPagesDAO.UpdateTblPages(tblPagesTO);
        }

        public static int UpdateTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPagesDAO.UpdateTblPages(tblPagesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblPages(Int32 idPage)
        {
            return TblPagesDAO.DeleteTblPages(idPage);
        }

        public static int DeleteTblPages(Int32 idPage, SqlConnection conn, SqlTransaction tran)
        {
            return TblPagesDAO.DeleteTblPages(idPage, conn, tran);
        }

       

        #endregion

    }
}
