using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseBookingActionsDAO
    {
        String SqlSelectQuery();
//        List<TblBookingActionsTO> SelectAllTblBookingActions();
  //      TblBookingActionsTO SelectTblBookingActions(Int32 idBookingAction);
        TblPurchaseBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseBookingActionsTO> ConvertDTToList(SqlDataReader tblBookingActionsTODT);
          int InsertTblBookingActions(TblPurchaseBookingActionsTO tblBookingActionsTO);
        int InsertTblBookingActions(TblPurchaseBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseBookingActionsTO tblBookingActionsTO, SqlCommand cmdInsert);
       // int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO);
      //  int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
     //   int ExecuteUpdationCommand(TblBookingActionsTO tblBookingActionsTO, SqlCommand cmdUpdate);
       // int DeleteTblBookingActions(Int32 idBookingAction);
        //int DeleteTblBookingActions(Int32 idBookingAction, SqlConnection conn, SqlTransaction tran);
        //int ExecuteDeletionCommand(Int32 idBookingAction, SqlCommand cmdDelete);

    }
}