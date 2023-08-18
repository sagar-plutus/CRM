using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.BL
{
    public class TblVersionBL
    {
        #region Selection
       

        public static List<TblVersionTO> SelectAllTblVersionList()
        {
            return TblVersionDAO.SelectAllTblVersion();
            //return ConvertDTToList(tblVersionTODT);
        }

        public static TblVersionTO SelectTblVersionTO(Int32 idVersion)
        {
            return TblVersionDAO.SelectTblVersion(idVersion);

        }
        public static TblVersionTO SelectLatestVersionTO()
        {
            return TblVersionDAO.SelectLatestVersionTO();

        }




        #endregion

        #region Insertion
        public static int InsertTblVersion(TblVersionTO tblVersionTO)
        {
            return TblVersionDAO.InsertTblVersion(tblVersionTO);
        }

        public static int InsertTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVersionDAO.InsertTblVersion(tblVersionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblVersion(TblVersionTO tblVersionTO)
        {
            return TblVersionDAO.UpdateTblVersion(tblVersionTO);
        }

        public static int UpdateTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVersionDAO.UpdateTblVersion(tblVersionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblVersion(Int32 idVersion)
        {
            return TblVersionDAO.DeleteTblVersion(idVersion);
        }

        public static int DeleteTblVersion(Int32 idVersion, SqlConnection conn, SqlTransaction tran)
        {
            return TblVersionDAO.DeleteTblVersion(idVersion, conn, tran);
        }

        #endregion
        
    }
}
