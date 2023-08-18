using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblQuotaDeclarationDAO
    {
        String SqlSelectQuery();
        List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclaration();
        List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclaration(Int32 globalRateId);
        List<TblQuotaDeclarationTO> ConvertDTToList(SqlDataReader tblQuotaDeclarationTODT);
        TblQuotaDeclarationTO SelectTblQuotaDeclaration(Int32 idQuotaDeclaration);
        TblQuotaDeclarationTO SelectPreviousTblQuotaDeclarationTO(Int32 idQuotaDeclaration, Int32 cnfOrgId);
        TblQuotaDeclarationTO SelectTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran);
        TblQuotaDeclarationTO SelectLatestQuotaDeclarationTO(SqlConnection conn, SqlTransaction tran);
        List<TblQuotaDeclarationTO> SelectLatestQuotaDeclaration(Int32 orgId, DateTime date);
        PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo SelectDashboardQuotaAndRateInfo(Int32 roleId, Int32 orgId, DateTime sysDate);
        int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlCommand cmdInsert);
        int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateAllDeclaredQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran);
        int UpdateQuotaDeclarationValidity(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlCommand cmdUpdate);
        int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration);
        int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idQuotaDeclaration, SqlCommand cmdDelete);

    }
}