using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnqVehDescDAO
    {
        #region Selection
        List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc();
        List<TblPurchaseEnqVehDescTO> SelectTblPurchaseEnqVehDesc(Int32 idVehTypeDesc);
        List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(SqlConnection conn, SqlTransaction tran);

        List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(Int32 purchaseEnqId, SqlConnection conn = null, SqlTransaction tran = null);

        List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(Int32 purchaseEnqId);
        #endregion

        #region Insertion
        int InsertTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO);
        int InsertTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlCommand cmdInsert);

        #endregion

        #region Updation
        int UpdateTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO);
        int UpdateTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlCommand cmdUpdate);

        #endregion

        #region Deletion
        int DeleteTblPurchaseEnqVehDesc(Int32 idVehTypeDesc);
        int DeleteTblPurchaseEnqVehDesc(Int32 idVehTypeDesc, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVehTypeDesc, SqlCommand cmdDelete);
        int DeletePurchVehDesc(Int32 purchaseEnqId, SqlConnection conn, SqlTransaction tran);

        

        
        #endregion
    }
}
