using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;

namespace  PurchaseTrackerAPI.BL
{
    public class TblProdItemDescBL : ITblProdItemDescBL
    {
        private readonly ITblProdItemDescDAO _iTblProdItemDescDAO;

        public TblProdItemDescBL(ITblProdItemDescDAO iTblProdItemDescDAO)
        {
            _iTblProdItemDescDAO = iTblProdItemDescDAO;
        }

        #region Selection
        public List<TblProdItemDescTO> SelectAllTblProdItemDesc()
        {
            return _iTblProdItemDescDAO.SelectAllTblProdItemDesc();
        }

        public List<TblProdItemDescTO> SelectAllTblProdItemDescList()
        {
            return _iTblProdItemDescDAO.SelectAllTblProdItemDesc();

        }

        public TblProdItemDescTO SelectTblProdItemDescTO(Int32 idProdItemDesc)
        {
            return _iTblProdItemDescDAO.SelectTblProdItemDesc(idProdItemDesc);

        }

        #endregion

        #region Insertion
        public int InsertTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO)
        {
            return _iTblProdItemDescDAO.InsertTblProdItemDesc(tblProdItemDescTO);
        }

        public int InsertTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemDescDAO.InsertTblProdItemDesc(tblProdItemDescTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO)
        {
            return _iTblProdItemDescDAO.UpdateTblProdItemDesc(tblProdItemDescTO);
        }

        public int UpdateTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemDescDAO.UpdateTblProdItemDesc(tblProdItemDescTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblProdItemDesc(Int32 idProdItemDesc)
        {
            return _iTblProdItemDescDAO.DeleteTblProdItemDesc(idProdItemDesc);
        }

        public int DeleteTblProdItemDesc(Int32 idProdItemDesc, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemDescDAO.DeleteTblProdItemDesc(idProdItemDesc, conn, tran);
        }

        #endregion

    }
}
