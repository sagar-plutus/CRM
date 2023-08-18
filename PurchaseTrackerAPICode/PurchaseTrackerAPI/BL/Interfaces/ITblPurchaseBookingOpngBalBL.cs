
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblPurchaseBookingOpngBalBL
    {

        #region Selection
        List<TblPurchaseBookingOpngBalTO> SelectAllTblPurchaseBookingOpngBal();

        List<TblPurchaseBookingOpngBalTO> SelectAllTblPurchaseBookingOpngBalList();

        TblPurchaseBookingOpngBalTO SelectTblPurchaseBookingOpngBalTO(Int32 idPurchaseBookingOpngBal);

        #endregion

        #region Insertion
        int calculateOpeningClosingBal();
        int InsertTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO);

        int InsertTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
        int UpdateTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO);
        int UpdateTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
        int DeleteTblPurchaseBookingOpngBal(Int32 idPurchaseBookingOpngBal);

        int DeleteTblPurchaseBookingOpngBal(Int32 idPurchaseBookingOpngBal, SqlConnection conn, SqlTransaction tran);
        List<SaudaReportTo> GetAllPendingSaudaForReport(int cnfOrgId, int dealerOrgId, int materialTypeId, int statusId);

        #endregion


    }
}