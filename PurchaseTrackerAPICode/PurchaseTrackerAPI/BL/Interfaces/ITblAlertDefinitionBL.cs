using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertDefinitionBL
    {
        List<TblAlertDefinitionTO> SelectAllTblAlertDefinitionList();
        TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef);
        TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran);
        int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO);
        int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO);
        int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertDefinition(Int32 idAlertDef);
        int DeleteTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran);

    }
}