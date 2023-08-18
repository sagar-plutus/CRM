using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;


namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseItemDescDAO
    {

        #region Selection
        List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc();
        TblPurchaseItemDescTO SelectTblPurchaseItemDesc(Int32 idPurchaseItemDesc);

        List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc(int rootScheduleId, int itemId, int stageId);

        List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc(SqlConnection conn, SqlTransaction tran, int rootScheduleId, int itemId,int stageId);

        List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc(SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Insertion
        int InsertTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO);

        int InsertTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
        int UpdateTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO);

        int UpdateTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
        int DeleteTblPurchaseItemDesc(Int32 idPurchaseItemDesc);
        int DeleteTblPurchaseItemDesc(Int32 idPurchaseItemDesc, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchaseItemDesc, SqlCommand cmdDelete);
        int UpdateAllTblPurchaseItemDescPhaseAndRootWise(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseItemDescTO> GetAllDescriptionListForCorrection(int rootScheduleId, int itemId, int phaseId);

        int DeleteAllPurchaseItemDescAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        #endregion


    }
}