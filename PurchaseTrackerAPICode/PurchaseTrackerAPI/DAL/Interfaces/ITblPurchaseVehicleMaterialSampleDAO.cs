using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehicleMaterialSampleDAO
    {
        List<TblPurchaseMaterialSampleTypeTo> ConvertSampleTyoeList(SqlDataReader dimVehicleTypeTODT);
        List<TblPurchaseMaterialSampleTO> ConvertSampleMaterialList(SqlDataReader dimVehicleTypeTODT);
        List<TblPurchaseMaterialSampleCategoryTo> ConvertSampleCategoryList(SqlDataReader dimVehicleTypeTODT);
        List<TblPurchaseMaterialSampleTypeTo> SqlSelectSampleTypeQuery();
        TblPurchaseMaterialSampleTO getTblPurchaseMaterialSample(int purchaseScheduleSummaryId);
        List<TblPurchaseMaterialSampleCategoryTo> SqlSelectSampleCategoryQuery();
        String SqlSelectSampleQuery(int purchaseScheduleSummaryId);
        int InserttblPurchaseMaterialSample(TblPurchaseMaterialSampleTO tblPurchaseMaterialSampleTO);
        int ExecuteInsertionCommand(TblPurchaseMaterialSampleTO tblPurchaseMaterialSampleTO, SqlCommand cmdInsert);

    }
}