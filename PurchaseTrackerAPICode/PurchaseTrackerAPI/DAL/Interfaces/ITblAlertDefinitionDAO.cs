using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertDefinitionDAO
    {
        String SqlSelectQuery();
        List<TblAlertDefinitionTO> SelectAllTblAlertDefinition();
        TblAlertDefinitionTO SelectTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran);
        List<TblAlertDefinitionTO> ConvertDTToList(SqlDataReader tblAlertDefinitionTODT);
        int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO);
        int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAlertDefinitionTO tblAlertDefinitionTO, SqlCommand cmdInsert);
        int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO);
        int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAlertDefinitionTO tblAlertDefinitionTO, SqlCommand cmdUpdate);
        int DeleteTblAlertDefinition(Int32 idAlertDef);
        int DeleteTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idAlertDef, SqlCommand cmdDelete);

    }
}