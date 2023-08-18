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
    public interface Itblquotadeclarationbl
    {
        List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclarationList();
        List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclarationList(Int32 globalRateId);
        TblQuotaDeclarationTO SelectTblQuotaDeclarationTO(Int32 idQuotaDeclaration);
        TblQuotaDeclarationTO SelectPreviousTblQuotaDeclarationTO(Int32 idQuotaDeclaration, Int32 cnfOrgId);
          TblQuotaDeclarationTO SelectTblQuotaDeclarationTO(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran);
        TblQuotaDeclarationTO SelectLatestQuotaDeclarationTO(SqlConnection conn, SqlTransaction tran);
          List<TblQuotaDeclarationTO> SelectLatestQuotaDeclarationTOList(Int32 cnfId, DateTime date);
        PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo SelectQuotaAndRateDashboardInfo(Int32 roleId, Int32 orgId, DateTime sysDate);
        Boolean CheckForValidityAndReset(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int SaveDeclaredRateAndAllocatedQuota(List<TblQuotaDeclarationTO> quotaExtList, List<TblQuotaDeclarationTO> quotaList, TblGlobalRateTO tblGlobalRateTO);
        ResultMessage SaveDeclaredRateAndAllocatedBand(List<TblGlobalRateTO> tblGlobalRateTOList);
        int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration);
        int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran);

    }
}