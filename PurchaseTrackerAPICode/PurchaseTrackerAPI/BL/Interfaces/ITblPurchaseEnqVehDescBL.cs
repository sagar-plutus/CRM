using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;


namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblPurchaseEnqVehDescBL
    {
        #region Selection
        List<TblPurchaseEnqVehDescTO> SelectAllPurchaseEnqVehDesc();
        List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDescList();
        TblPurchaseEnqVehDescTO SelectTblPurchaseEnqVehDescTO(Int32 idVehTypeDesc);

        List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc( SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(Int32 purchaseEnqId, SqlConnection conn = null, SqlTransaction tran = null);

        #endregion

        #region Insertion
        int InsertTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO);
        int InsertTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
        int UpdateTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO);
        int UpdateTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
        int DeleteTblPurchaseEnqVehDesc(Int32 idVehTypeDesc);
        int DeleteTblPurchaseEnqVehDesc(Int32 idVehTypeDesc, SqlConnection conn, SqlTransaction tran);

        int DeletePurchVehDesc(Int32 purchaseEnqId, SqlConnection conn, SqlTransaction tran);
        #endregion
    }
}
