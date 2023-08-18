using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblIssueTypeBL
    {
        #region Selection
        public static DataTable SelectAllTblIssueType()
        {
            return TblIssueTypeDAO.SelectAllTblIssueType();
        }

        public static List<TblIssueTypeTO> SelectAllTblIssueTypeList()
        {
            DataTable tblIssueTypeTODT = TblIssueTypeDAO.SelectAllTblIssueType();
            return ConvertDTToList(tblIssueTypeTODT);
        }

        public static TblIssueTypeTO SelectTblIssueTypeTO(Int32 idIssueType)
        {
            DataTable tblIssueTypeTODT = TblIssueTypeDAO.SelectTblIssueType(idIssueType);
            List<TblIssueTypeTO> tblIssueTypeTOList = ConvertDTToList(tblIssueTypeTODT);
            if (tblIssueTypeTOList != null && tblIssueTypeTOList.Count == 1)
                return tblIssueTypeTOList[0];
            else
                return null;
        }

        public static List<TblIssueTypeTO> ConvertDTToList(DataTable tblIssueTypeTODT)
        {
            List<TblIssueTypeTO> tblIssueTypeTOList = new List<TblIssueTypeTO>();
            if (tblIssueTypeTODT != null)
            {
            }
            return tblIssueTypeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO)
        {
            return TblIssueTypeDAO.InsertTblIssueType(tblIssueTypeTO);
        }

        public static int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblIssueTypeDAO.InsertTblIssueType(tblIssueTypeTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO)
        {
            return TblIssueTypeDAO.UpdateTblIssueType(tblIssueTypeTO);
        }

        public static int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblIssueTypeDAO.UpdateTblIssueType(tblIssueTypeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblIssueType(Int32 idIssueType)
        {
            return TblIssueTypeDAO.DeleteTblIssueType(idIssueType);
        }

        public static int DeleteTblIssueType(Int32 idIssueType, SqlConnection conn, SqlTransaction tran)
        {
            return TblIssueTypeDAO.DeleteTblIssueType(idIssueType, conn, tran);
        }

        #endregion

    }
}
