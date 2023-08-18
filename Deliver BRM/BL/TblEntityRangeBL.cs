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
    public class TblEntityRangeBL
    {
        #region Selection

        public static List<TblEntityRangeTO> SelectAllTblEntityRangeList()
        {
            return  TblEntityRangeDAO.SelectAllTblEntityRange();
        }

        public static TblEntityRangeTO SelectTblEntityRangeTO(Int32 idEntityRange)
        {
            return  TblEntityRangeDAO.SelectTblEntityRange(idEntityRange);
        }
        public static TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName)
        {
            return TblEntityRangeDAO.SelectTblEntityRangeByEntityName(entityName);
        }

        public static TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName, int finYearId)
        {
            return TblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityName,finYearId);
        }

        public static TblEntityRangeTO SelectEntityRangeTOFromInvoiceType(String entityName, int finYearId, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityName, finYearId, conn, tran);
        }

        public static TblEntityRangeTO SelectEntityRangeTOFromInvoiceType(Int32 invoiceTypeId,int finYearId, SqlConnection conn,SqlTransaction tran)
        {
            return TblEntityRangeDAO.SelectEntityRangeFromInvoiceType(invoiceTypeId,finYearId, conn, tran);
        }

        // Vaibhav [07-Jan-2018] Added t select entity data
        public static TblEntityRangeTO SelectTblEntityRangeTOByEntityName(string entityName,int finYearId, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.SelectTblEntityRangeByEntityName(entityName,finYearId,conn,tran);
        }

        #endregion

        #region Insertion
        public static int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            return TblEntityRangeDAO.InsertTblEntityRange(tblEntityRangeTO);
        }

        public static int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.InsertTblEntityRange(tblEntityRangeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO)
        {
            return TblEntityRangeDAO.UpdateTblEntityRange(tblEntityRangeTO);
        }

        public static int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.UpdateTblEntityRange(tblEntityRangeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblEntityRange(Int32 idEntityRange)
        {
            return TblEntityRangeDAO.DeleteTblEntityRange(idEntityRange);
        }

        public static int DeleteTblEntityRange(Int32 idEntityRange, SqlConnection conn, SqlTransaction tran)
        {
            return TblEntityRangeDAO.DeleteTblEntityRange(idEntityRange, conn, tran);
        }

        #endregion
        
    }
}
