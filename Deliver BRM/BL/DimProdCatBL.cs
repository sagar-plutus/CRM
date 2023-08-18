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
    public class DimProdCatBL
    {
        #region Selection
        public static List<DimProdCatTO> SelectAllDimProdCatList()
        {
            return  DimProdCatDAO.SelectAllDimProdCat();
        }

        public static DimProdCatTO SelectDimProdCatTO(Int32 idProdCat)
        {
            return  DimProdCatDAO.SelectDimProdCat(idProdCat);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertDimProdCat(DimProdCatTO dimProdCatTO)
        {
            return DimProdCatDAO.InsertDimProdCat(dimProdCatTO);
        }

        public static int InsertDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdCatDAO.InsertDimProdCat(dimProdCatTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimProdCat(DimProdCatTO dimProdCatTO)
        {
            return DimProdCatDAO.UpdateDimProdCat(dimProdCatTO);
        }

        public static int UpdateDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdCatDAO.UpdateDimProdCat(dimProdCatTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimProdCat(Int32 idProdCat)
        {
            return DimProdCatDAO.DeleteDimProdCat(idProdCat);
        }

        public static int DeleteDimProdCat(Int32 idProdCat, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdCatDAO.DeleteDimProdCat(idProdCat, conn, tran);
        }

        #endregion
        
    }
}
