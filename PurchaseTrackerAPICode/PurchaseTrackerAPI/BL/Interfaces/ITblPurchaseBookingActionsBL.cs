using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseBookingActionsBL
    {
       // List<TblPurchaseBookingActionsTO> SelectAllTblBookingActionsList();
       // TblBookingActionsTO SelectTblBookingActionsTO(Int32 idBookingAction);
        TblPurchaseBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran);
        TblPurchaseBookingActionsTO SelectLatestBookingActionTO();
        int InsertTblBookingActions(TblPurchaseBookingActionsTO tblBookingActionsTO);
        int InsertTblBookingActions(TblPurchaseBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveBookingActions(TblPurchaseBookingActionsTO tblBookingActionsTO);
      //  int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO);
       // int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
       // int DeleteTblBookingActions(Int32 idBookingAction);
        //int DeleteTblBookingActions(Int32 idBookingAction, SqlConnection conn, SqlTransaction tran);

    }
}