using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseBookingOpngBalDAO
    {
        TblPurchaseBookingOpngBalTO SelectTblPurchaseBookingOpngBal(int idPurchaseBookingOpngBal);
        int InsertTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO);
        int InsertTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO);
        int UpdateTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseBookingOpngBal(int idPurchaseBookingOpngBal);
        int DeleteTblPurchaseBookingOpngBal(int idPurchaseBookingOpngBal, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseBookingOpngBalTO> SelectAllTblPurchaseBookingOpngBal();
        Dictionary<int, double> SelectBookingsPendingQtyDCT(SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseBookingOpngBal(SqlConnection conn, SqlTransaction tran);
        Dictionary<int, double> SelectBookingWiseUnloadingQtyDCT(DateTime serverDate, bool v);
       TblPurchaseBookingOpngBalTO SelectTblPurchaseBookingOpngBalLatest();

    }
}