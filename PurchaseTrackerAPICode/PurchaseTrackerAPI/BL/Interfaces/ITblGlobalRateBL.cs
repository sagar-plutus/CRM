using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGlobalRateBL
    {
        TblGlobalRateTO SelectTblGlobalRateTO(Int32 idGlobalRate);
        TblGlobalRateTO SelectTblGlobalRateTO(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran);
        TblGlobalRateTO SelectLatestTblGlobalRateTO();
        TblGlobalRateTO SelectLatestTblGlobalRateTO(SqlConnection conn, SqlTransaction tran);
        List<TblGlobalRateTO> SelectTblGlobalRateTOList(DateTime fromDate, DateTime toDate);
        Dictionary<Int32, Int32> SelectLatestBrandAndRateDCT();
        Dictionary<Int32, Int32> SelectLatestGroupAndRateDCT();
        Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran);
        TblGlobalRateTO SelectProductGroupItemGlobalRate(Int32 prodItemId);
        int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO);
        int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran);
          int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO);
        int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGlobalRate(Int32 idGlobalRate);
        int DeleteTblGlobalRate(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran);

    }
}