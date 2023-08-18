using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertInstanceDAO
    {
        String SqlSelectQuery();
        List<TblAlertInstanceTO> SelectAllTblAlertInstance();
        List<TblAlertInstanceTO> SelectAllTblAlertInstance(Int32 userId, Int32 roleId);
        TblAlertInstanceTO SelectTblAlertInstance(Int32 idAlertInstance);
        List<TblAlertInstanceTO> ConvertDTToList(SqlDataReader tblAlertInstanceTODT);
        int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO);
        int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAlertInstanceTO tblAlertInstanceTO, SqlCommand cmdInsert);
        int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO);
        int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran);
        int ResetAlertInstanceByDef(String alertDefIds, SqlConnection conn, SqlTransaction tran);
        int ResetAutoResetAlertInstances(SqlConnection conn, SqlTransaction tran);
        int ResetAlertInstance(int alertDefId, int sourceEntityId, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAlertInstanceTO tblAlertInstanceTO, SqlCommand cmdUpdate);
        int DeleteTblAlertInstance(Int32 idAlertInstance);
        int DeleteTblAlertInstance(Int32 idAlertInstance, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idAlertInstance, SqlCommand cmdDelete);

    }
}