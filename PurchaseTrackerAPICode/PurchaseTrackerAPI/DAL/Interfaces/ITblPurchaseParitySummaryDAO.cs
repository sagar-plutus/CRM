using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseParitySummaryDAO
    {
        String SqlSelectQuery();
          List<TblPurchaseParitySummaryTO> SelectAllActivePurchaseCompetitorMaterialList(Int32 organizationId, DateTime fromDate, DateTime toDate, SqlConnection conn, SqlTransaction tran);
        TblPurchaseParitySummaryTO SelectStatesActiveParitySummary(Int32 stateId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseParitySummaryTO> SelectAllMaterialReasonsList(Int32 stateId);
        List<TblPurchaseParitySummaryTO> ConvertDTToMaterialList(SqlDataReader tblPurchaseParitySummaryTODT);
        List<TblPurchaseParitySummaryTO> ConvertDTToListCompetitor(SqlDataReader tblPurchaseParitySummaryTODT);
        int InsertTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO);
        int InsertTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlCommand cmdInsert);
        int UpdateTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO);
        int UpdateTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateAllParitySummary(Int32 stateId, Int32 materialId, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlCommand cmdUpdate);

    }
}