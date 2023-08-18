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
    public class TblFreightUpdateBL
    {
        #region Selection
        
        public static List<TblFreightUpdateTO> SelectAllTblFreightUpdateList()
        {
            return TblFreightUpdateDAO.SelectAllTblFreightUpdate();
        }

        public static TblFreightUpdateTO SelectTblFreightUpdateTO(Int32 idFreightUpdate)
        {
            return TblFreightUpdateDAO.SelectTblFreightUpdate(idFreightUpdate);
        }

        internal static List<TblFreightUpdateTO> SelectAllTblFreightUpdateList(DateTime frmDt, DateTime toDt, int districtId, int talukaId)
        {
            return TblFreightUpdateDAO.SelectAllTblFreightUpdate(frmDt,toDt, districtId,talukaId);

        }


        #endregion

        #region Insertion
        public static int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO)
        {
            return TblFreightUpdateDAO.InsertTblFreightUpdate(tblFreightUpdateTO);
        }

        public static int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFreightUpdateDAO.InsertTblFreightUpdate(tblFreightUpdateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO)
        {
            return TblFreightUpdateDAO.UpdateTblFreightUpdate(tblFreightUpdateTO);
        }

        public static int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblFreightUpdateDAO.UpdateTblFreightUpdate(tblFreightUpdateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblFreightUpdate(Int32 idFreightUpdate)
        {
            return TblFreightUpdateDAO.DeleteTblFreightUpdate(idFreightUpdate);
        }

        public static int DeleteTblFreightUpdate(Int32 idFreightUpdate, SqlConnection conn, SqlTransaction tran)
        {
            return TblFreightUpdateDAO.DeleteTblFreightUpdate(idFreightUpdate, conn, tran);
        }

       
        #endregion

    }
}
