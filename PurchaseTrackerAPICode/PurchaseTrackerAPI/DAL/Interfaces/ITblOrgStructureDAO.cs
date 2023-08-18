using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblOrgStructureDAO
    {
        String SqlSelectQuery();
        List<TblOrgStructureTO> SelectAllOrgStructureHierarchy();
        TblOrgStructureTO SelectTblOrgStructure(Int32 idOrgStructure);
        TblOrgStructureTO SelectAllTblOrgStructure(SqlConnection conn, SqlTransaction tran);
        List<TblOrgStructureTO> ConvertDTToList(SqlDataReader orgStructureDT);
        List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(int orgStructureId);
        String SelectAllOrgStructureIdList(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectReportingToUserList(Int32 orgStructureId);
        int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO);
        int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdInsert);
        int InsertTblUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlCommand cmdInsert);
        int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO);
        int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate);
        int DeactivateOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeactivateOrgSTructureCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate);
        int DeactivateOrgStructureEmployees(String orgStructureIdList, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeactivateOrgSTructureEmployeesCommand(String orgStructureIdList, SqlCommand cmdUpdate);
        int UpdateUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdateUserReportingDetailsCommand(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblOrgStructure(Int32 idOrgStructure);
        int DeleteTblOrgStructure(Int32 idOrgStructure, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOrgStructure, SqlCommand cmdDelete);

    }
}