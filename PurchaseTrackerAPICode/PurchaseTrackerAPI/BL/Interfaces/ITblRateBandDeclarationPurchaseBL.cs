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
    public interface ITblRateBandDeclarationPurchaseBL
    {
        Dictionary<Int32, List<TblRateBandDeclarationPurchaseTO>> SelectAllTblRateBandDeclarationPurchaseList(Int32 globalRatePurchaseId);
        List<TblRateBandDeclarationPurchaseTO> GetPurchaseManagerWithBand(Int32 globalRatePurchaseId);
        List<TblRateBandDeclarationPurchaseTO> SelectAllTblUserOfPurchase();
        PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo SelectQuotaAndRateDashboardInfo(Int32 roleId, Int32 orgId, DateTime sysDate);
        TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration, SqlConnection conn, SqlTransaction tran);
        TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration);
        List<TblRateBandDeclarationPurchaseTO> SelectLatestRateBandDeclarationPurchaseTOList(Int32 cnfId, DateTime date);
        TblRateBandDeclarationPurchaseTO SelectOldRateTOList(Int32 cnfId, DateTime date);
        TblRateBandDeclarationPurchaseTO SelectLatestRateTOList(Int32 cnfId, DateTime date);
        TblRateBandDeclarationPurchaseTO SelectPreviousTblRateDeclarationTO(Int32 idRateBandDeclarationPurchase, Int32 userId);
        Boolean CheckForValidityAndReset(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO);
        int InsertTblRateBandDeclarationPurchase(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveDeclaredRate(List<TblGlobalRatePurchaseTO> tblGlobalRatePurchaseTOList, Int32 loginUserId, DateTime serverDate);
        int UpdateTblRateDeclaration(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO);
        List<TblRateBandDeclarationPurchaseTO> SelectAllTblRateBandDeclarationPurchase(Int32 globalRatePurchaseId);
       
    }
}