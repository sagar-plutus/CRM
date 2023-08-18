using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGlobalRatePurchaseDAO
    {
        String SqlSelectQuery();
          List<TblGlobalRatePurchaseTO> SelectLatestRateOfPurchaseDCT(DateTime date,Boolean isGetLatest = true);
        List<TblGlobalRatePurchaseTO> GetGlobalPurchaseRateList(DateTime fromDate, DateTime toDate);
        List<TblGlobalRatePurchaseTO> GetGlobalRateList(DateTime fromDate, DateTime toDate);
        List<TblPurchaseScheduleSummaryTO> GetAvgSaleDateWise(DateTime fromDate, DateTime toDate);
        Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran);
        List<TblGlobalRatePurchaseTO> ConvertDTToList(SqlDataReader TblGlobalRatePurchaseTODT);
        TblGlobalRatePurchaseTO SelectTblGlobalRatePurchase1(Int32 idGlobalRatePurchase);
        TblGlobalRatePurchaseTO SelectTblGlobalRatePurchase(Int32 idGlobalRatePurchase, SqlConnection conn, SqlTransaction tran);
        int InsertTblGlobalRatePurchase(TblGlobalRatePurchaseTO TblGlobalRatePurchaseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblGlobalRatePurchaseTO TblGlobalRatePurchaseTO, SqlCommand cmdInsert);
        List<TblGlobalRatePurchaseTO> GetPurchaseRateWithAvgDtls(DateTime fromDate, DateTime toDate);
    }
}