using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.BL;

namespace SalesTrackerAPI.BL
{
    public class DimProdSpecDescBL
    {

        #region Selection

           public static List<DimProdSpecDescTO> SelectAllDimProdSpecDescList()
            {                
                return DimProdSpecDescDAO.SelectAllDimProdSpecDesc();
            }

            public static DimProdSpecDescTO SelectDimPRodSpecDescTO(Int32 idCodeType)
            {
            return DimProdSpecDescDAO.SelectDimProdSpecDesc(idCodeType);              
            }

        /// <summary>
        /// Added by vinod Dated:12/12/2017 for the select of max record from the product Specification 
        /// </summary>
        /// <returns></returns>
        /// 

        public static int SelectAllDimProdSpecDescriptionList()
        {           
            return DimProdSpecDescDAO.SelectDimProdSpecDescription();           
        }

        #endregion

        #region Insertion
        public static int InsertDimProdSpecDesc(DimProdSpecDescTO ProSpecDesc)
            {
                return DimProdSpecDescDAO.InsertDimProdSpecDesc(ProSpecDesc);              
            }

            public static int InsertDimProdSpecDesc(DimProdSpecDescTO dimProSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
                return DimProdSpecDescDAO.InsertDimProdSpecDesc(dimProSpecDescTO, conn, tran);               
            }

            #endregion

            #region Updation
            public static int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO)
            {
                return DimProdSpecDescDAO.UpdateDimProdSpecDesc(dimProdSpecDescTO);
            }
            public static int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
               return DimProdSpecDescDAO.UpdateDimProdSpecDesc(dimProdSpecDescTO, conn,tran);            
            }

            #endregion

            #region Deletion
            public static int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO)
            {
            return DimProdSpecDescDAO.UpdateDimProdSpecDescription(DimProdSpecDescTO);            
            }

            public static int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
            {
            return DimProdSpecDescDAO.UpdateDimProdSpecDescription(DimProdSpecDescTO, conn, tran);
            }

            #endregion

    }
}