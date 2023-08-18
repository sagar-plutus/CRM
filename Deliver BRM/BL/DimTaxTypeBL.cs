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
    public class DimTaxTypeBL
    {
        #region Selection

        public static List<DimTaxTypeTO> SelectAllDimTaxTypeList()
        {
            return DimTaxTypeDAO.SelectAllDimTaxType();
        }

        public static DimTaxTypeTO SelectDimTaxTypeTO(Int32 idTaxType)
        {
            return  DimTaxTypeDAO.SelectDimTaxType(idTaxType);
        }

        #endregion
        
        #region Insertion
        public static int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO)
        {
            return DimTaxTypeDAO.InsertDimTaxType(dimTaxTypeTO);
        }

        public static int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimTaxTypeDAO.InsertDimTaxType(dimTaxTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO)
        {
            return DimTaxTypeDAO.UpdateDimTaxType(dimTaxTypeTO);
        }

        public static int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimTaxTypeDAO.UpdateDimTaxType(dimTaxTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimTaxType(Int32 idTaxType)
        {
            return DimTaxTypeDAO.DeleteDimTaxType(idTaxType);
        }

        public static int DeleteDimTaxType(Int32 idTaxType, SqlConnection conn, SqlTransaction tran)
        {
            return DimTaxTypeDAO.DeleteDimTaxType(idTaxType, conn, tran);
        }

        #endregion
        
    }
}
