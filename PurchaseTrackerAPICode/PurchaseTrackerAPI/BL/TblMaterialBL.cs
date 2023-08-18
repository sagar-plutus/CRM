using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblMaterialBL : ITblMaterialBL
    {
        private readonly ITblMaterialDAO _iTblMaterialDAO;
        public TblMaterialBL(ITblMaterialDAO iTblMaterialDAO)
        {
            _iTblMaterialDAO = iTblMaterialDAO;
        }

        #region Selection
       
        public  List<TblMaterialTO> SelectAllTblMaterialList()
        {
            return  _iTblMaterialDAO.SelectAllTblMaterial();
           
        }

        public  List<DropDownTO> SelectAllMaterialListForDropDown()
        {
            List<DropDownTO> list = _iTblMaterialDAO.SelectAllMaterialForDropDown();
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

        public  dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new VDynObject(properties);
        }

        public  TblMaterialTO SelectTblMaterialTO(Int32 idMaterial)
        {
            return _iTblMaterialDAO.SelectTblMaterial(idMaterial);
          
        }

        /// <summary>
        /// Vijaymala[12-09-2017] Added To Get Material Type List
        /// </summary>
        /// <returns></returns>
        public  List<DropDownTO> SelectMaterialTypeDropDownList()
        {
            List<DropDownTO> list = _iTblMaterialDAO.SelectMaterialTypeDropDownList();
            return list;

        }


        #endregion

        #region Insertion
        public  int InsertTblMaterial(TblMaterialTO tblMaterialTO)
        {
            return _iTblMaterialDAO.InsertTblMaterial(tblMaterialTO);
        }

        public  int InsertTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMaterialDAO.InsertTblMaterial(tblMaterialTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblMaterial(TblMaterialTO tblMaterialTO)
        {
            return _iTblMaterialDAO.UpdateTblMaterial(tblMaterialTO);
        }

        public  int UpdateTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMaterialDAO.UpdateTblMaterial(tblMaterialTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblMaterial(Int32 idMaterial)
        {
            return _iTblMaterialDAO.DeleteTblMaterial(idMaterial);
        }

        public  int DeleteTblMaterial(Int32 idMaterial, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMaterialDAO.DeleteTblMaterial(idMaterial, conn, tran);
        }

        #endregion
        
    }
}
