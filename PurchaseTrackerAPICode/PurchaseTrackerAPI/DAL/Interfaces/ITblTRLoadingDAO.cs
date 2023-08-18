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
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.DAL.Interfaces 
{
    public interface ITblTRLoadingDAO
    {

        String SqlSelectQuery();

        DataTable SelectAllTblTRLoading();

        DataTable SelectTblTRLoading(Int32 idLoading);

        DataTable SelectTblTRLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran);

        DataTable SelectAllTblTRLoading(SqlConnection conn, SqlTransaction tran);

        DataTable SelectNextLoadingId();

        DataTable SelectAllTblTRLoading(Int32 statusId);

        DataTable SelectAllTblTRLoading(String statusId, DateTime fromDate, DateTime toDate);

        DataTable SelectAllTblTRLoading(String statusId, DateTime fromDate, DateTime toDate, TRLoadingFilterE tRLoadingFilterE);

        DataTable SelectAllTblTRLoading(DateTime fromDate, DateTime toDate);

        DataTable SelectAllTblTRLoading(String statusId);

        int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO);

        int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteInsertionCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdInsert);

        int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO);

        int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);

        int UpdateTblTRLoadingStatus(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteUpdationCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdUpdate);

        int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO);

        int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteDeletionCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdDelete);

    }
}
