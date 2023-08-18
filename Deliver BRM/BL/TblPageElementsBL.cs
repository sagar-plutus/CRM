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
    public class TblPageElementsBL
    {
        #region Selection

        public static List<TblPageElementsTO> SelectAllTblPageElementsList()
        {
           return  TblPageElementsDAO.SelectAllTblPageElements();
        }

        public static TblPageElementsTO SelectTblPageElementsTO(Int32 idPageElement)
        {
            return  TblPageElementsDAO.SelectTblPageElements(idPageElement);
        }

        internal static List<TblPageElementsTO> SelectAllTblPageElementsList(int pageId)
        {
            return TblPageElementsDAO.SelectAllTblPageElements(pageId);
        }

        #endregion

        #region Insertion
        public static int InsertTblPageElements(TblPageElementsTO tblPageElementsTO)
        {
            return TblPageElementsDAO.InsertTblPageElements(tblPageElementsTO);
        }

        public static int InsertTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPageElementsDAO.InsertTblPageElements(tblPageElementsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO)
        {
            return TblPageElementsDAO.UpdateTblPageElements(tblPageElementsTO);
        }

        public static int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPageElementsDAO.UpdateTblPageElements(tblPageElementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblPageElements(Int32 idPageElement)
        {
            return TblPageElementsDAO.DeleteTblPageElements(idPageElement);
        }

        public static int DeleteTblPageElements(Int32 idPageElement, SqlConnection conn, SqlTransaction tran)
        {
            return TblPageElementsDAO.DeleteTblPageElements(idPageElement, conn, tran);
        }

       

        #endregion

    }
}
