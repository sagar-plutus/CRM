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
    public class DimPageElementTypesBL : IDimPageElementTypesBL
    {
        private readonly IDimPageElementTypesDAO _iDimPageElementTypesDAO;
        public DimPageElementTypesBL(IDimPageElementTypesDAO iDimPageElementTypesDAO)
        {
            _iDimPageElementTypesDAO = iDimPageElementTypesDAO;
        }
        #region Selection
       
        public  List<DimPageElementTypesTO> SelectAllDimPageElementTypesList()
        {
            return _iDimPageElementTypesDAO.SelectAllDimPageElementTypes();
        }

        public  DimPageElementTypesTO SelectDimPageElementTypesTO(Int32 idPageEleType)
        {
            return _iDimPageElementTypesDAO.SelectDimPageElementTypes(idPageEleType);
        }

        #endregion
        
        #region Insertion
        public  int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
            return _iDimPageElementTypesDAO.InsertDimPageElementTypes(dimPageElementTypesTO);
        }

        public  int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPageElementTypesDAO.InsertDimPageElementTypes(dimPageElementTypesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO)
        {
            return _iDimPageElementTypesDAO.UpdateDimPageElementTypes(dimPageElementTypesTO);
        }

        public  int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPageElementTypesDAO.UpdateDimPageElementTypes(dimPageElementTypesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimPageElementTypes(Int32 idPageEleType)
        {
            return _iDimPageElementTypesDAO.DeleteDimPageElementTypes(idPageEleType);
        }

        public  int DeleteDimPageElementTypes(Int32 idPageEleType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPageElementTypesDAO.DeleteDimPageElementTypes(idPageEleType, conn, tran);
        }

        #endregion
        
    }
}
