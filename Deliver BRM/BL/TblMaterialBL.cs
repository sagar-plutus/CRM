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
    public class TblMaterialBL
    {
        #region Selection
       
        public static List<TblMaterialTO> SelectAllTblMaterialList()
        {
            return  TblMaterialDAO.SelectAllTblMaterial();
           
        }

        public static List<DropDownTO> SelectAllMaterialListForDropDown()
        {
            List<DropDownTO> list = TblMaterialDAO.SelectAllMaterialForDropDown();
            if(list!=null )
            {
                //Dictionary<string, object> testDCT = new Dictionary<string, object>();
                //List<DimProdSpecTO> dimProdSpecTOList = BL.DimProdSpecBL.SelectAllDimProdSpecList();
                //if (dimProdSpecTOList != null && dimProdSpecTOList.Count > 0)
                //{
                //    for (int i = 0; i < dimProdSpecTOList.Count; i++)
                //    {
                //        testDCT.Add(dimProdSpecTOList[i].ProdSpecDesc, dimProdSpecTOList[i].IdProdSpec);
                //    }
                //}
                //else return null;

                //var testV = GetDynamicObject(testDCT);
                //for (int i = 0; i < list.Count; i++)
                //{
                //    list[i].Tag = testV;
                //}
            }

            return list;

        }

        public static dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new VDynObject(properties);
        }

        public static TblMaterialTO SelectTblMaterialTO(Int32 idMaterial)
        {
            return TblMaterialDAO.SelectTblMaterial(idMaterial);
          
        }

        /// <summary>
        /// Vijaymala[12-09-2017] Added To Get Material Type List
        /// </summary>
        /// <returns></returns>
        public static List<DropDownTO> SelectMaterialTypeDropDownList()
        {
            List<DropDownTO> list = TblMaterialDAO.SelectMaterialTypeDropDownList();
            return list;

        }


        #endregion

        #region Insertion
        public static int InsertTblMaterial(TblMaterialTO tblMaterialTO)
        {
            return TblMaterialDAO.InsertTblMaterial(tblMaterialTO);
        }

        public static int InsertTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblMaterialDAO.InsertTblMaterial(tblMaterialTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblMaterial(TblMaterialTO tblMaterialTO)
        {
            return TblMaterialDAO.UpdateTblMaterial(tblMaterialTO);
        }

        public static int UpdateTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblMaterialDAO.UpdateTblMaterial(tblMaterialTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblMaterial(Int32 idMaterial)
        {
            return TblMaterialDAO.DeleteTblMaterial(idMaterial);
        }

        public static int DeleteTblMaterial(Int32 idMaterial, SqlConnection conn, SqlTransaction tran)
        {
            return TblMaterialDAO.DeleteTblMaterial(idMaterial, conn, tran);
        }

        #endregion
        
    }
}
