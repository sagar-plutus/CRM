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
    public class DimProdSpecBL
    {
        #region Selection
        public static List<DimProdSpecTO> SelectAllDimProdSpecList()
        {
            return  DimProdSpecDAO.SelectAllDimProdSpec();
        }

        public static DimProdSpecTO SelectDimProdSpecTO(Int32 idProdSpec)
        {
            return  DimProdSpecDAO.SelectDimProdSpec(idProdSpec);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO)
        {
            return DimProdSpecDAO.InsertDimProdSpec(dimProdSpecTO);
        }

        public static int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdSpecDAO.InsertDimProdSpec(dimProdSpecTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO)
        {
            return DimProdSpecDAO.UpdateDimProdSpec(dimProdSpecTO);
        }

        public static int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdSpecDAO.UpdateDimProdSpec(dimProdSpecTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimProdSpec(Int32 idProdSpec)
        {
            return DimProdSpecDAO.DeleteDimProdSpec(idProdSpec);
        }

        public static int DeleteDimProdSpec(Int32 idProdSpec, SqlConnection conn, SqlTransaction tran)
        {
            return DimProdSpecDAO.DeleteDimProdSpec(idProdSpec, conn, tran);
        }

        #endregion
        
    }
}
