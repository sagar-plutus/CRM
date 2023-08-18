using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblRateBandDeclarationPurchaseDAO
    {
        String SqlSelectQuery();
        String SqlSelectQueryForPurchase();
        List<TblRateBandDeclarationPurchaseTO> SelectAllTblUserOfPurchase();
        List<TblRateBandDeclarationPurchaseTO> SelectAllTblRateBandDeclarationPurchase(Int32 globalRatePurchaseId);
        List<TblRateBandDeclarationPurchaseTO> ConvertDTToPurchaseList(SqlDataReader TblRateBandDeclarationPurchaseTODT);
        List<TblRateBandDeclarationPurchaseTO> ConvertDTToList(SqlDataReader TblRateBandDeclarationPurchaseTODT);
        PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo SelectDashboardQuotaAndRateInfo(Int32 roleId, Int32 orgId, DateTime sysDate);
        TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration);
        TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration, SqlConnection conn, SqlTransaction tran);
        List<TblRateBandDeclarationPurchaseTO> SelectLatestRateBandDeclarationPurchaseTOList(Int32 userId, DateTime date);
        TblRateBandDeclarationPurchaseTO SelectOldRateTOList(Int32 userId, DateTime date);
        TblRateBandDeclarationPurchaseTO SelectLatestRateTOList(Int32 userId, DateTime date);
        TblRateBandDeclarationPurchaseTO SelectPreviousTblRateDeclarationTO(Int32 idRateBandDeclarationPurchase, Int32 userId);
        int DeactivateAllDeclaredQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran);
        int InsertTblRateBandDeclarationPurchase(TblRateBandDeclarationPurchaseTO TblRateBandDeclarationPurchaseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblRateBandDeclarationPurchaseTO TblRateBandDeclarationPurchaseTO, SqlCommand cmdInsert);
        int UpdateTblRateDeclaration(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO);
        int ExecuteUpdationCommand(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO,  SqlCommand cmdUpdate);

    }
}