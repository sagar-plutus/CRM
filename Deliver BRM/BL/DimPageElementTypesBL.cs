using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SalesTrackerAPI.BL
{
    public class DimPageElementTypesBL
    {
        #region Selection
       
        public static List<DimPageElementTypesTO> SelectAllDimPageElementTypesList()
        {
            return  DimPageElementTypesDAO.SelectAllDimPageElementTypes();
        }

        public static DimPageElementTypesTO SelectDimPageElementTypesTO(Int32 idPageEleType)
        {
            return  DimPageElementTypesDAO.SelectDimPageElementTypes(idPageEleType);
        }

        #endregion
        
        #region Insertion
        public static int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
            return DimPageElementTypesDAO.InsertDimPageElementTypes(dimPageElementTypesTO);
        }

        public static int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimPageElementTypesDAO.InsertDimPageElementTypes(dimPageElementTypesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
            return DimPageElementTypesDAO.UpdateDimPageElementTypes(dimPageElementTypesTO);
        }

        public static int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimPageElementTypesDAO.UpdateDimPageElementTypes(dimPageElementTypesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimPageElementTypes(Int32 idPageEleType)
        {
            return DimPageElementTypesDAO.DeleteDimPageElementTypes(idPageEleType);
        }

        public static int DeleteDimPageElementTypes(Int32 idPageEleType, SqlConnection conn, SqlTransaction tran)
        {
            return DimPageElementTypesDAO.DeleteDimPageElementTypes(idPageEleType, conn, tran);
        }

        #endregion
        
    }
}
