using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;

namespace SalesTrackerAPI.BL
{
    public class TblUserVerBL
    {
        #region Selection
        public static List<TblUserVerTO> SelectAllTblUserVer()
        {
            return TblUserVerDAO.SelectAllTblUserVer();
        }

        public static List<TblUserVerTO> SelectAllTblUserVerList()
        {
            List<TblUserVerTO> tblUserVerTODT = TblUserVerDAO.SelectAllTblUserVer();
            //return ConvertDTToList(tblUserVerTODT);
            return tblUserVerTODT;
        }

        public static TblUserVerTO SelectTblUserVerTO(Int32 idUserVer)
        {
            List<TblUserVerTO> tblUserVerTOList = TblUserVerDAO.SelectTblUserVer(idUserVer);
           // List<TblUserVerTO> tblUserVerTOList = ConvertDTToList(tblUserVerTODT);
            if(tblUserVerTOList != null && tblUserVerTOList.Count == 1)
                return tblUserVerTOList[0];
            else
                return null;
        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblUserVer(TblUserVerTO tblUserVerTO)
        {
            return TblUserVerDAO.InsertTblUserVer(tblUserVerTO);
        }

        public static int InsertTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserVerDAO.InsertTblUserVer(tblUserVerTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblUserVer(TblUserVerTO tblUserVerTO)
        {
            return TblUserVerDAO.UpdateTblUserVer(tblUserVerTO);
        }

        public static int UpdateTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserVerDAO.UpdateTblUserVer(tblUserVerTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblUserVer(Int32 idUserVer)
        {
            return TblUserVerDAO.DeleteTblUserVer(idUserVer);
        }

        public static int DeleteTblUserVer(Int32 idUserVer, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserVerDAO.DeleteTblUserVer(idUserVer, conn, tran);
        }

        #endregion
        
    }
}
