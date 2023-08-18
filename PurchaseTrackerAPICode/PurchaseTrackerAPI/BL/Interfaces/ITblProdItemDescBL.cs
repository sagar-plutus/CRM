using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;


namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblProdItemDescBL
    {

        #region Selection
        List<TblProdItemDescTO> SelectAllTblProdItemDesc();
        List<TblProdItemDescTO> SelectAllTblProdItemDescList();
        TblProdItemDescTO SelectTblProdItemDescTO(Int32 idProdItemDesc);

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