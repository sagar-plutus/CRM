using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblBaseItemMetalCostBL
    {
        List<TblBaseItemMetalCostTO> SelectAllTblBaseItemMetalCost();
        List<TblBaseItemMetalCostTO> SelectAllTblBaseItemMetalCostList();
        TblBaseItemMetalCostTO SelectTblBaseItemMetalCostTO(Int32 idBaseItemMetalCost);
        List<TblBaseItemMetalCostTO> SelectLatestBaseItemMetalCost(Int32 globalRatePurchaseId);
        List<TblBaseItemMetalCostTO> SelectLatestBaseItemMetalCost(Int32 globalRatePurchaseId, SqlConnection conn, SqlTransaction tran);

        List<TblBaseItemMetalCostTO> SelectBaseItemMetalCostByGlobalRateId(Int32 globalRatePurchaseId,Int32 cOrNcId);
        int InsertTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO);
        int InsertTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO);
        int UpdateTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn, SqlTransaction tran);
          int DeleteTblBaseItemMetalCost(Int32 idBaseItemMetalCost);
        int DeleteTblBaseItemMetalCost(Int32 idBaseItemMetalCost, SqlConnection conn, SqlTransaction tran);
        

    }
}