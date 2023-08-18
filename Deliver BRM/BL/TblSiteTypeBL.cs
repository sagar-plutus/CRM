using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.BL
{
    public class TblSiteTypeBL
    {
        #region Selection

        public static List<TblSiteTypeTO> SelectAllTblSiteTypeList()
        {
            List<TblSiteTypeTO> siteTypeTOList = TblSiteTypeDAO.SelectAllTblSiteTypeList();
            if (siteTypeTOList != null)
                return siteTypeTOList;
            else
                return null;
        }

        public static TblSiteTypeTO SelectTblSiteTypeTO(Int32 idSiteType)
        {
            DataTable tblSiteTypeTODT = TblSiteTypeDAO.SelectTblSiteType(idSiteType);
            List<TblSiteTypeTO> tblSiteTypeTOList = ConvertDTToList(tblSiteTypeTODT);
            if (tblSiteTypeTOList != null && tblSiteTypeTOList.Count == 1)
                return tblSiteTypeTOList[0];
            else
                return null;
        }

        public static List<TblSiteTypeTO> ConvertDTToList(DataTable tblSiteTypeTODT)
        {
            List<TblSiteTypeTO> tblSiteTypeTOList = new List<TblSiteTypeTO>();
            if (tblSiteTypeTODT != null)
            {
              
            }
            return tblSiteTypeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblSiteType(TblSiteTypeTO tblSiteTypeTO)
        {
            return TblSiteTypeDAO.InsertTblSiteType(tblSiteTypeTO);
        }

        public static int InsertTblSiteType(ref TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSiteTypeDAO.InsertTblSiteType(ref tblSiteTypeTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO)
        {
            return TblSiteTypeDAO.UpdateTblSiteType(tblSiteTypeTO);
        }

        public static int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSiteTypeDAO.UpdateTblSiteType(tblSiteTypeTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblSiteType(Int32 idSiteType)
        {
            return TblSiteTypeDAO.DeleteTblSiteType(idSiteType);
        }

        public static int DeleteTblSiteType(Int32 idSiteType, SqlConnection conn, SqlTransaction tran)
        {
            return TblSiteTypeDAO.DeleteTblSiteType(idSiteType, conn, tran);
        }

        #endregion

    }
}
