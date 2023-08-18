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
    public class DimGstCodeTypeBL
    {
        #region Selection
        public static List<DimGstCodeTypeTO> SelectAllDimGstCodeTypeList()
        {
            return DimGstCodeTypeDAO.SelectAllDimGstCodeType();
        }

        public static DimGstCodeTypeTO SelectDimGstCodeTypeTO(Int32 idCodeType)
        {
            return DimGstCodeTypeDAO.SelectDimGstCodeType(idCodeType);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO)
        {
            return DimGstCodeTypeDAO.InsertDimGstCodeType(dimGstCodeTypeTO);
        }

        public static int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimGstCodeTypeDAO.InsertDimGstCodeType(dimGstCodeTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO)
        {
            return DimGstCodeTypeDAO.UpdateDimGstCodeType(dimGstCodeTypeTO);
        }

        public static int UpdateDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimGstCodeTypeDAO.UpdateDimGstCodeType(dimGstCodeTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimGstCodeType(Int32 idCodeType)
        {
            return DimGstCodeTypeDAO.DeleteDimGstCodeType(idCodeType);
        }

        public static int DeleteDimGstCodeType(Int32 idCodeType, SqlConnection conn, SqlTransaction tran)
        {
            return DimGstCodeTypeDAO.DeleteDimGstCodeType(idCodeType, conn, tran);
        }

        #endregion
        
    }
}
