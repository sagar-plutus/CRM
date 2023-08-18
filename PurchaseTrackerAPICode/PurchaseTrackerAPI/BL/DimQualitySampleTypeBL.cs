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
    public class DimQualitySampleTypeBL : IDimQualitySampleTypeBL
    {
        private readonly IDimQualitySampleTypeDAO _iDimQualitySampleTypeDAO;
        

        public DimQualitySampleTypeBL(IDimQualitySampleTypeDAO iDimQualitySampleTypeDAO )
        {
            _iDimQualitySampleTypeDAO = iDimQualitySampleTypeDAO;

        }

        #region Selection
        public  List<DimQualitySampleTypeTO> SelectAllDimQualitySampleType()
        {
            return _iDimQualitySampleTypeDAO.SelectAllDimQualitySampleType();
        }

        public  List<DimQualitySampleTypeTO> SelectAllDimQualitySampleTypeList()
        {
            return  _iDimQualitySampleTypeDAO.SelectAllDimQualitySampleType();
            //return ConvertDTToList(dimQualitySampleTypeTODT);
        }

        public  DimQualitySampleTypeTO SelectDimQualitySampleTypeTO(Int32 idQualitySampleType)
        {
             List<DimQualitySampleTypeTO> dimQualitySampleTypeTOList=_iDimQualitySampleTypeDAO.SelectDimQualitySampleType(idQualitySampleType);
            if(dimQualitySampleTypeTOList != null && dimQualitySampleTypeTOList.Count == 1)
                return dimQualitySampleTypeTOList[0];
            else
                return null;
        }

       
        #endregion
        
        #region Insertion
        public  int InsertDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO)
        {
            return _iDimQualitySampleTypeDAO.InsertDimQualitySampleType(dimQualitySampleTypeTO);
        }

        public  int InsertDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimQualitySampleTypeDAO.InsertDimQualitySampleType(dimQualitySampleTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO)
        {
            return _iDimQualitySampleTypeDAO.UpdateDimQualitySampleType(dimQualitySampleTypeTO);
        }

        public  int UpdateDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimQualitySampleTypeDAO.UpdateDimQualitySampleType(dimQualitySampleTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimQualitySampleType(Int32 idQualitySampleType)
        {
            return _iDimQualitySampleTypeDAO.DeleteDimQualitySampleType(idQualitySampleType);
        }

        public  int DeleteDimQualitySampleType(Int32 idQualitySampleType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimQualitySampleTypeDAO.DeleteDimQualitySampleType(idQualitySampleType, conn, tran);
        }

        #endregion
        
    }
}
