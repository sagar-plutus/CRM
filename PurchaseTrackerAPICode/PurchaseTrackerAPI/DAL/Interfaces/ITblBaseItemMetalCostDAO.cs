using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblBaseItemMetalCostDAO
    {
        String SqlSelectQuery();
        List<TblBaseItemMetalCostTO> SelectAllTblBaseItemMetalCost();
        List<TblBaseItemMetalCostTO> SelectTblBaseItemMetalCost(Int32 idBaseItemMetalCost);
        List<TblBaseItemMetalCostTO> SelectLatestBaseItemMetalCost(Int32 globalRatePurchaseId);
        List<TblBaseItemMetalCostTO> SelectLatestBaseItemMetalCost(Int32 globalRatePurchaseId, SqlConnection conn, SqlTransaction tran);

        List<TblBaseItemMetalCostTO> SelectBaseItemMetalCostByGlobalRateId(Int32 globalRatePurchaseId,Int32 cOrNcId);
        List<TblBaseItemMetalCostTO> SelectAllTblBaseItemMetalCost(SqlConnection conn, SqlTransaction tran);
        List<TblBaseItemMetalCostTO> ConvertDTToList(SqlDataReader tblBaseItemMetalCostTODT);
        int InsertTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO);
        int InsertTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlCommand cmdInsert);
        int UpdateTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO);
        int UpdateTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlCommand cmdUpdate);
        int DeleteTblBaseItemMetalCost(Int32 idBaseItemMetalCost);
        int DeleteTblBaseItemMetalCost(Int32 idBaseItemMetalCost, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBaseItemMetalCost, SqlCommand cmdDelete);

    }
}