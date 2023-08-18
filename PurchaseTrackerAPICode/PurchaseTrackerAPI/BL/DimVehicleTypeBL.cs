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
    public class DimVehicleTypeBL : IDimVehicleTypeBL
    {
        private readonly IDimVehicleTypeDAO _iDimVehicleCategoryDAO;
        private readonly ITblPurchaseVehicleMaterialSampleDAO _iTblPurchaseVehicleMaterialSampleDAO;
        public DimVehicleTypeBL(IDimVehicleTypeDAO iDimVehicleCategoryDAO
                                , ITblPurchaseVehicleMaterialSampleDAO iTblPurchaseVehicleMaterialSampleDAO)
            {
            _iTblPurchaseVehicleMaterialSampleDAO = iTblPurchaseVehicleMaterialSampleDAO;
            _iDimVehicleCategoryDAO = iDimVehicleCategoryDAO;
            }

        #region Selection

        public  List<DimVehicleTypeTO> SelectAllDimVehicleTypeList()
        {
            return _iDimVehicleCategoryDAO.SelectAllDimVehicleType();
        }

        public  DimVehicleTypeTO SelectDimVehicleTypeTO(Int32 idVehicleType)
        {
            return _iDimVehicleCategoryDAO.SelectDimVehicleType(idVehicleType);
        }

        public  List<TblPurchaseMaterialSampleCategoryTo> SelectDimSampleCategoryTO( )
        {
            return _iTblPurchaseVehicleMaterialSampleDAO.SqlSelectSampleCategoryQuery( );
        }
        public  List<TblPurchaseMaterialSampleTypeTo> SelectDimSampleTypeTO( )
        {
            return _iTblPurchaseVehicleMaterialSampleDAO.SqlSelectSampleTypeQuery( );
        }


        #endregion

        #region Insertion
        public  int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
            return _iDimVehicleCategoryDAO.InsertDimVehicleType(dimVehicleTypeTO);
        }

        public  int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleCategoryDAO.InsertDimVehicleType(dimVehicleTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
            return _iDimVehicleCategoryDAO.UpdateDimVehicleType(dimVehicleTypeTO);
        }

        public  int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleCategoryDAO.UpdateDimVehicleType(dimVehicleTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimVehicleType(Int32 idVehicleType)
        {
            return _iDimVehicleCategoryDAO.DeleteDimVehicleType(idVehicleType);
        }

        public  int DeleteDimVehicleType(Int32 idVehicleType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleCategoryDAO.DeleteDimVehicleType(idVehicleType, conn, tran);
        }

        #endregion
        
    }
}
