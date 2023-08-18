using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;

namespace PurchaseTrackerAPI.DAL.Interfaces 
{
    public interface ITblTRLoadingHistoryDAO
    {
        String SqlSelectQuery();

        DataTable SelectAllTblTRLoadingHistory();

        DataTable SelectTblTRLoadingHistory(Int32 idLoadingHistory);

        DataTable SelectAllTblTRLoadingHistory(SqlConnection conn, SqlTransaction tran);

        int InsertTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO);

        int InsertTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteInsertionCommand(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlCommand cmdInsert);

        int UpdateTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO);

        int UpdateTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteUpdationCommand(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlCommand cmdUpdate);

        int DeleteTblTRLoadingHistory(Int32 idLoadingHistory);

        int DeleteTblTRLoadingHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran);

        int ExecuteDeletionCommand(Int32 idLoadingHistory, SqlCommand cmdDelete);


    }
}
