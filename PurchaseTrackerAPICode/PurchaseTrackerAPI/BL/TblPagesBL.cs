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
    public class TblPagesBL : ITblPagesBL
    {
        private readonly ITblPagesDAO _itblPagesDAO;
        public TblPagesBL(ITblPagesDAO iTblPagesDAO)
        {
            _itblPagesDAO = iTblPagesDAO;
        }
        #region Selection

        public  List<TblPagesTO> SelectAllTblPagesList()
        {
            return  _itblPagesDAO.SelectAllTblPages();
        }

        public  List<TblPagesTO> SelectAllTblPagesList(int moduleId)
        {
            return _itblPagesDAO.SelectAllTblPages(moduleId);
        }

        public  TblPagesTO SelectTblPagesTO(Int32 idPage)
        {
            return  _itblPagesDAO.SelectTblPages(idPage);
        }

        #endregion
        
        #region Insertion
        public  int InsertTblPages(TblPagesTO tblPagesTO)
        {
            return _itblPagesDAO.InsertTblPages(tblPagesTO);
        }

        public  int InsertTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblPagesDAO.InsertTblPages(tblPagesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPages(TblPagesTO tblPagesTO)
        {
            return _itblPagesDAO.UpdateTblPages(tblPagesTO);
        }

        public  int UpdateTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblPagesDAO.UpdateTblPages(tblPagesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPages(Int32 idPage)
        {
            return _itblPagesDAO.DeleteTblPages(idPage);
        }

        public  int DeleteTblPages(Int32 idPage, SqlConnection conn, SqlTransaction tran)
        {
            return _itblPagesDAO.DeleteTblPages(idPage, conn, tran);
        }

       

        #endregion

    }
}
