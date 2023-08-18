using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblPurchaseItemDescBL
    {
        #region Selection
        List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc();

        List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDescList();
        List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDescList(int rootScheduleId, int itemId, int stageId, int phaseId);

        TblPurchaseItemDescTO SelectTblPurchaseItemDescTO(Int32 idPurchaseItemDesc);

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
        ResultMessage InsertTblPurchaseItemDesc(List<TblPurchaseItemDescTO> purchaseProdItemDescTOList, int loginUserId);
        List<TblPurchaseItemDescTO> GetAllDescriptionListForCorrection(int rootScheduleId, int itemId, int phaseId);

        int DeleteAllPurchaseItemDescAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

        #endregion

    }
}