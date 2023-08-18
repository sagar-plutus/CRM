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

namespace PurchaseTrackerAPI.BL
{
    public class DimPurchaseTcTypeElementBL : IDimPurchaseTcTypeElementBL
    {
        private readonly IDimPurchaseTcTypeElementDAO _iDimPurchaseTcTypeElementDAO;
        public DimPurchaseTcTypeElementBL(IDimPurchaseTcTypeElementDAO iDimPurchaseTcTypeElementDAO)
        {
            _iDimPurchaseTcTypeElementDAO = iDimPurchaseTcTypeElementDAO;

        }

        #region Selection
        public List<DimPurchaseTcTypeElementTO> SelectAllDimPurchaseTcTypeElement()
        {
            return _iDimPurchaseTcTypeElementDAO.SelectAllDimPurchaseTcTypeElement();
        }

        public List<DimPurchaseTcTypeElementTO> SelectAllDimPurchaseTcTypeElementList()
        {
            return _iDimPurchaseTcTypeElementDAO.SelectAllDimPurchaseTcTypeElement();
          
        }

        public DimPurchaseTcTypeElementTO SelectDimPurchaseTcTypeElementTO(Int32 idPurchasseTcTypeElement)
        {
            List<DimPurchaseTcTypeElementTO> dimPurchaseTcTypeElementTOList = _iDimPurchaseTcTypeElementDAO.SelectDimPurchaseTcTypeElement(idPurchasseTcTypeElement);
            if(dimPurchaseTcTypeElementTOList != null && dimPurchaseTcTypeElementTOList.Count == 1)
                return dimPurchaseTcTypeElementTOList[0];
            else
                return null;
        }

      
        #endregion
        
        #region Insertion
        public int InsertDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO)
        {
            return _iDimPurchaseTcTypeElementDAO.InsertDimPurchaseTcTypeElement(dimPurchaseTcTypeElementTO);
        }

        public int InsertDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPurchaseTcTypeElementDAO.InsertDimPurchaseTcTypeElement(dimPurchaseTcTypeElementTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO)
        {
            return _iDimPurchaseTcTypeElementDAO.UpdateDimPurchaseTcTypeElement(dimPurchaseTcTypeElementTO);
        }

        public int UpdateDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPurchaseTcTypeElementDAO.UpdateDimPurchaseTcTypeElement(dimPurchaseTcTypeElementTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement)
        {
            return _iDimPurchaseTcTypeElementDAO.DeleteDimPurchaseTcTypeElement(idPurchasseTcTypeElement);
        }

        public int DeleteDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPurchaseTcTypeElementDAO.DeleteDimPurchaseTcTypeElement(idPurchasseTcTypeElement, conn, tran);
        }
        #endregion
        
    }
}
