using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblVisitPersonTypeBL
    {
        #region Selection
        public static DataTable SelectAllTblVisitPersonType()
        {
            return TblVisitPersonTypeDAO.SelectAllTblVisitPersonType();
        }

        public static List<TblVisitPersonTypeTO> SelectAllTblVisitPersonTypeList()
        {
            DataTable tblVisitPersonTypeTODT = TblVisitPersonTypeDAO.SelectAllTblVisitPersonType();
            return ConvertDTToList(tblVisitPersonTypeTODT);
        }

        public static TblVisitPersonTypeTO SelectTblVisitPersonTypeTO(Int32 idPersonType)
        {
            DataTable tblVisitPersonTypeTODT = TblVisitPersonTypeDAO.SelectTblVisitPersonType(idPersonType);
            List<TblVisitPersonTypeTO> tblVisitPersonTypeTOList = ConvertDTToList(tblVisitPersonTypeTODT);
            if (tblVisitPersonTypeTOList != null && tblVisitPersonTypeTOList.Count == 1)
                return tblVisitPersonTypeTOList[0];
            else
                return null;
        }

        public static List<TblVisitPersonTypeTO> ConvertDTToList(DataTable tblVisitPersonTypeTODT)
        {
            List<TblVisitPersonTypeTO> tblVisitPersonTypeTOList = new List<TblVisitPersonTypeTO>();
            if (tblVisitPersonTypeTODT != null)
            {
            }
            return tblVisitPersonTypeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO)
        {
            return TblVisitPersonTypeDAO.InsertTblVisitPersonType(tblVisitPersonTypeTO);
        }

        public static int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitPersonTypeDAO.InsertTblVisitPersonType(tblVisitPersonTypeTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO)
        {
            return TblVisitPersonTypeDAO.UpdateTblVisitPersonType(tblVisitPersonTypeTO);
        }

        public static int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitPersonTypeDAO.UpdateTblVisitPersonType(tblVisitPersonTypeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblVisitPersonType(Int32 idPersonType)
        {
            return TblVisitPersonTypeDAO.DeleteTblVisitPersonType(idPersonType);
        }

        public static int DeleteTblVisitPersonType(Int32 idPersonType, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitPersonTypeDAO.DeleteTblVisitPersonType(idPersonType, conn, tran);
        }

        #endregion

    }
}
