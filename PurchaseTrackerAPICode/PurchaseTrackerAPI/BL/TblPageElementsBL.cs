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
    
    public class TblPageElementsBL : ITblPageElementsBL
    {
        private readonly ITblPageElementsDAO _iTblPageElementsDAO;

        #region Selection
        public TblPageElementsBL(ITblPageElementsDAO iTblPageElementsDAO)
        {
            _iTblPageElementsDAO = iTblPageElementsDAO;
        }
        public  List<TblPageElementsTO> SelectAllTblPageElementsList()
        {
           return  _iTblPageElementsDAO.SelectAllTblPageElements();
        }

        public  TblPageElementsTO SelectTblPageElementsTO(Int32 idPageElement)
        {
            return  _iTblPageElementsDAO.SelectTblPageElements(idPageElement);
        }

        public  List<TblPageElementsTO> SelectAllTblPageElementsList(int pageId)
        {
            return _iTblPageElementsDAO.SelectAllTblPageElements(pageId);
        }

        #endregion

        #region Insertion
        public  int InsertTblPageElements(TblPageElementsTO tblPageElementsTO)
        {
            return _iTblPageElementsDAO.InsertTblPageElements(tblPageElementsTO);
        }

        public  int InsertTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPageElementsDAO.InsertTblPageElements(tblPageElementsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO)
        {
            return _iTblPageElementsDAO.UpdateTblPageElements(tblPageElementsTO);
        }

        public  int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPageElementsDAO.UpdateTblPageElements(tblPageElementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPageElements(Int32 idPageElement)
        {
            return _iTblPageElementsDAO.DeleteTblPageElements(idPageElement);
        }

        public  int DeleteTblPageElements(Int32 idPageElement, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPageElementsDAO.DeleteTblPageElements(idPageElement, conn, tran);
        }

       

        #endregion

    }
}
