using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGlobalRateDAO
    {
        String SqlSelectQuery();
        TblGlobalRateTO SelectTblGlobalRate(Int32 idGlobalRate);
        TblGlobalRateTO SelectTblGlobalRate(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran);
        TblGlobalRateTO SelectLatestTblGlobalRateTO(SqlConnection conn, SqlTransaction tran);
        List<TblGlobalRateTO> SelectLatestTblGlobalRateTOList(DateTime fromDate, DateTime toDate);
        Dictionary<Int32, Int32> SelectLatestGroupAndRateDCT();
        Dictionary<Int32, Int32> SelectLatestBrandAndRateDCT();
        Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran);
        SqlDataReader SelectAllTblGlobalRate(SqlConnection conn, SqlTransaction tran);
        List<TblGlobalRateTO> ConvertDTToList(SqlDataReader tblGlobalRateTODT);
        int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO);
        int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblGlobalRateTO tblGlobalRateTO, SqlCommand cmdInsert);
        int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO);
        int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblGlobalRateTO tblGlobalRateTO, SqlCommand cmdUpdate);
        int DeleteTblGlobalRate(Int32 idGlobalRate);
        int DeleteTblGlobalRate(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idGlobalRate, SqlCommand cmdDelete);

    }
}