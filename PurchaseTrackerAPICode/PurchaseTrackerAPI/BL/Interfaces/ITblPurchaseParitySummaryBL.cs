using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseParitySummaryBL
    {
        TblPurchaseParitySummaryTO SelectStatesActiveParitySummaryTO(Int32 stateId, Int32 brandId);
        TblPurchaseParitySummaryTO SelectStatesActiveParitySummaryTO(Int32 stateId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseParitySummaryTO> SelectAllPurchaseCompetitorMaterialList(Int32 organizationId, DateTime fromDate, DateTime toDate);
        List<TblPurchaseParitySummaryTO> SelectAllMaterialReasonsList(Int32 stateId);
        ResultMessage SaveParitySettings(TblPurchaseParitySummaryTO tblParitySummaryTO);
        ResultMessage SaveProductImgSettings(SaveProductImgTO saveProductImgTO);
        int InsertTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO);
        int InsertTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran);

    }
}