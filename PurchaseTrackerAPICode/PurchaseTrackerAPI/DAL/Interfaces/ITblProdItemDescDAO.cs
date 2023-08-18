using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;


namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblProdItemDescDAO
    {
        #region Selection
        List<TblProdItemDescTO> SelectAllTblProdItemDesc();
        List<TblProdItemDescTO> SelectAllTblProdItemDesc(int itemId);
        TblProdItemDescTO SelectTblProdItemDesc(Int32 idProdItemDesc);

        List<TblProdItemDescTO> SelectAllTblProdItemDesc(SqlConnection conn, SqlTransaction tran);
        #endregion
        #region Insertion
        int InsertTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO);
        int InsertTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
        int UpdateTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO);
        int UpdateTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO, SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Deletion
        int DeleteTblProdItemDesc(Int32 idProdItemDesc);
        int DeleteTblProdItemDesc(Int32 idProdItemDesc, SqlConnection conn, SqlTransaction tran);

        #endregion
    }
}