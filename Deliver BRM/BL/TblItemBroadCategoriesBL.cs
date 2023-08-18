using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblItemBroadCategoriesBL
    {
        #region Selection
        public static List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategories()
        {
            return TblItemBroadCategoriesDAO.SelectAllTblItemBroadCategories();
        }

        public static List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategoriesList()
        {
            List<TblItemBroadCategoriesTO> tblItemBroadCategoriesToList = TblItemBroadCategoriesDAO.SelectAllTblItemBroadCategories();
            if (tblItemBroadCategoriesToList != null && tblItemBroadCategoriesToList.Count > 0)
                return tblItemBroadCategoriesToList;
            else
                return null;
        }

        public static TblItemBroadCategoriesTO SelectTblItemBroadCategoriesTO(Int32 iditemBroadCategories)
        {
            TblItemBroadCategoriesTO tblItemBroadCategoriesTODT = TblItemBroadCategoriesDAO.SelectTblItemBroadCategories(iditemBroadCategories);
            if (tblItemBroadCategoriesTODT != null)
                return tblItemBroadCategoriesTODT;
            else
                return null;
        }

        #endregion

        #region Insertion
        public static int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO)
        {
            return TblItemBroadCategoriesDAO.InsertTblItemBroadCategories(tblItemBroadCategoriesTO);
        }

        public static int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemBroadCategoriesDAO.InsertTblItemBroadCategories(tblItemBroadCategoriesTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO)
        {
            return TblItemBroadCategoriesDAO.UpdateTblItemBroadCategories(tblItemBroadCategoriesTO);
        }

        public static int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemBroadCategoriesDAO.UpdateTblItemBroadCategories(tblItemBroadCategoriesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblItemBroadCategories(Int32 iditemBroadCategories)
        {
            return TblItemBroadCategoriesDAO.DeleteTblItemBroadCategories(iditemBroadCategories);
        }

        public static int DeleteTblItemBroadCategories(Int32 iditemBroadCategories, SqlConnection conn, SqlTransaction tran)
        {
            return TblItemBroadCategoriesDAO.DeleteTblItemBroadCategories(iditemBroadCategories, conn, tran);
        }

        #endregion
    }
}
