using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace BL
{
    public class DimPurchaseGradeQtyTypeBL: IDimPurchaseGradeQtyTypeBL
    {
        private IDimPurchaseGradeQtyTypeDAO _iDimPurchaseGradeQtyTypeDAO;

        public DimPurchaseGradeQtyTypeBL(IDimPurchaseGradeQtyTypeDAO iDimPurchaseGradeQtyTypeDAO)
        {
            _iDimPurchaseGradeQtyTypeDAO = iDimPurchaseGradeQtyTypeDAO;
        }

        #region Selection
        public List<DimPurchaseGradeQtyTypeTO> SelectAllDimPurchaseGradeQtyType()
        {
            return _iDimPurchaseGradeQtyTypeDAO.SelectAllDimPurchaseGradeQtyType();
        }

        public List<DimPurchaseGradeQtyTypeTO> SelectAllDimPurchaseGradeQtyTypeList()
        {
            return _iDimPurchaseGradeQtyTypeDAO.SelectAllDimPurchaseGradeQtyType();            
        }

        public List<DimPurchaseGradeQtyTypeTO> SelectDimPurchaseGradeQtyTypeTO(Int32 idPurchaseGradeQtyType)
        {
            return _iDimPurchaseGradeQtyTypeDAO.SelectDimPurchaseGradeQtyType(idPurchaseGradeQtyType);            
        }

        #endregion
        
        #region Insertion
        public int InsertDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO)
        {
            return _iDimPurchaseGradeQtyTypeDAO.InsertDimPurchaseGradeQtyType(dimPurchaseGradeQtyTypeTO);
        }

        public int InsertDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPurchaseGradeQtyTypeDAO.InsertDimPurchaseGradeQtyType(dimPurchaseGradeQtyTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO)
        {
            return _iDimPurchaseGradeQtyTypeDAO.UpdateDimPurchaseGradeQtyType(dimPurchaseGradeQtyTypeTO);
        }

        public int UpdateDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPurchaseGradeQtyTypeDAO.UpdateDimPurchaseGradeQtyType(dimPurchaseGradeQtyTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType)
        {
            return _iDimPurchaseGradeQtyTypeDAO.DeleteDimPurchaseGradeQtyType(idPurchaseGradeQtyType);
        }

        public int DeleteDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPurchaseGradeQtyTypeDAO.DeleteDimPurchaseGradeQtyType(idPurchaseGradeQtyType, conn, tran);
        }

        #endregion
        
    }
}
