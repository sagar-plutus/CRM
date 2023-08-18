using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGlobalRatePurchaseBL
    {

        List<TblGlobalRatePurchaseTO> SelectLatestRateOfPurchaseDCT(DateTime sysDate,Boolean isGetLatest = true);
        List<TblGlobalRatePurchaseTO> SelectLatestRateOfPurchaseDCT(Int32 forQuota, DateTime sysDate, ref Int32 Count);
        List<TblGlobalRatePurchaseTO> GetGlobalPurchaseRateList(DateTime fromDate, DateTime toDate);
        Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran);
        TblGlobalRatePurchaseTO SelectTblGlobalRatePurchaseTO(Int32 idGlobalRatePurchase, SqlConnection conn, SqlTransaction tran);
        int InsertTblGlobalRatePurchase(TblGlobalRatePurchaseTO TblGlobalRatePurchaseTO, SqlConnection conn, SqlTransaction tran);
        List<TblGlobalRatePurchaseTO> GetPurchaseRateWithAvgDtls(DateTime fromDate, DateTime toDate);
    }
}