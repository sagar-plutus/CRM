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
    public class DimVehicleCategoryBL : IDimVehicleCategoryBL
    {
        private readonly IDimVehicleCategoryDAO _iDimVehicleCategoryDAO;
        public DimVehicleCategoryBL(IDimVehicleCategoryDAO  iDimVehicleCategoryDAO) {
            _iDimVehicleCategoryDAO = iDimVehicleCategoryDAO;
        }


        #region Selection
        public   List<DimVehicleCategoryTO> SelectAllDimVehicleCategory()
        {
            return _iDimVehicleCategoryDAO.SelectAllDimVehicleCategory();
        }

        public  List<DimVehicleCategoryTO> SelectAllDimVehicleCategoryList()
        {
            return _iDimVehicleCategoryDAO.SelectAllDimVehicleCategory();
            //return ConvertDTToList(dimVehicleCategoryTODT);
        }

        public  DimVehicleCategoryTO SelectDimVehicleCategoryTO(Int32 idVehicleCategory)
        {
             return _iDimVehicleCategoryDAO.SelectDimVehicleCategory(idVehicleCategory);
            // List<DimVehicleCategoryTO> dimVehicleCategoryTOList = ConvertDTToList(dimVehicleCategoryTODT);
            // if(dimVehicleCategoryTOList != null && dimVehicleCategoryTOList.Count == 1)
            //     return dimVehicleCategoryTOList[0];
            // else
            //     return null;
        }

      

        #endregion
        
        #region Insertion
        public  int InsertDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO)
        {
            return _iDimVehicleCategoryDAO.InsertDimVehicleCategory(dimVehicleCategoryTO);
        }

        public  int InsertDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleCategoryDAO.InsertDimVehicleCategory(dimVehicleCategoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO)
        {
            return _iDimVehicleCategoryDAO.UpdateDimVehicleCategory(dimVehicleCategoryTO);
        }

        public  int UpdateDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleCategoryDAO.UpdateDimVehicleCategory(dimVehicleCategoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimVehicleCategory(Int32 idVehicleCategory)
        {
            return _iDimVehicleCategoryDAO.DeleteDimVehicleCategory(idVehicleCategory);
        }

        public  int DeleteDimVehicleCategory(Int32 idVehicleCategory, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehicleCategoryDAO.DeleteDimVehicleCategory(idVehicleCategory, conn, tran);
        }

        #endregion
        
    }
}
