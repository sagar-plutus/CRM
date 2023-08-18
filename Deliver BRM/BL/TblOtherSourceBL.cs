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
    public class TblOtherSourceBL
    {
        #region Selection

        public static List<TblOtherSourceTO> SelectAllTblOtherSourceList()
        {
            return TblOtherSourceDAO.SelectAllTblOtherSource();
        }

        public static TblOtherSourceTO SelectTblOtherSourceTO(Int32 idOtherSource)
        {
            return TblOtherSourceDAO.SelectTblOtherSource(idOtherSource);
        }

        public static List<TblOtherSourceTO> SelectTblOtherSourceListFromDesc(string OtherSourceDesc)
        {
            return TblOtherSourceDAO.SelectTblOtherSourceListFromDesc(OtherSourceDesc);
        }

        public static List<DropDownTO> SelectOtherSourceOfMarketTrendForDropDown()
        {
            return TblOtherSourceDAO.SelectOtherSourceOfMarketTrendForDropDown();
        }

        #endregion

        #region Insertion
        public static int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO)
        {
            return TblOtherSourceDAO.InsertTblOtherSource(tblOtherSourceTO);
        }

        public static int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherSourceDAO.InsertTblOtherSource(tblOtherSourceTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO)
        {
            return TblOtherSourceDAO.UpdateTblOtherSource(tblOtherSourceTO);
        }

        public static int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherSourceDAO.UpdateTblOtherSource(tblOtherSourceTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblOtherSource(Int32 idOtherSource)
        {
            return TblOtherSourceDAO.DeleteTblOtherSource(idOtherSource);
        }

        public static int DeleteTblOtherSource(Int32 idOtherSource, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherSourceDAO.DeleteTblOtherSource(idOtherSource, conn, tran);
        }

        #endregion

    }
}
