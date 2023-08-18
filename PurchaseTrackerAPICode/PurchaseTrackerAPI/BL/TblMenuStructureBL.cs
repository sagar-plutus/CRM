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
    public class TblMenuStructureBL : ITblMenuStructureBL
    {
        private readonly ITblMenuStructureDAO _iTblMenuStructureDAO;
        public TblMenuStructureBL(ITblMenuStructureDAO iTblMenuStructureDAO)
        {
            _iTblMenuStructureDAO = iTblMenuStructureDAO;
        }
        #region Selection
        public  List<TblMenuStructureTO> SelectAllTblMenuStructureList()
        {
            return  _iTblMenuStructureDAO.SelectAllTblMenuStructure();
        }

        public  TblMenuStructureTO SelectTblMenuStructureTO(Int32 idMenu)
        {
            return  _iTblMenuStructureDAO.SelectTblMenuStructure(idMenu);
        }

       

        #endregion
        
        #region Insertion
        public  int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO)
        {
            return _iTblMenuStructureDAO.InsertTblMenuStructure(tblMenuStructureTO);
        }

        public  int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMenuStructureDAO.InsertTblMenuStructure(tblMenuStructureTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO)
        {
            return _iTblMenuStructureDAO.UpdateTblMenuStructure(tblMenuStructureTO);
        }

        public  int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMenuStructureDAO.UpdateTblMenuStructure(tblMenuStructureTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblMenuStructure(Int32 idMenu)
        {
            return _iTblMenuStructureDAO.DeleteTblMenuStructure(idMenu);
        }

        public  int DeleteTblMenuStructure(Int32 idMenu, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMenuStructureDAO.DeleteTblMenuStructure(idMenu, conn, tran);
        }

        #endregion
        
    }
}
