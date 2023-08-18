using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertSubscriptSettingsDAO
    {
        String SqlSelectQuery();
          List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings();
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings(int subscriptionId, SqlConnection conn, SqlTransaction tran);
        TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettings(Int32 idSubscriSettings);
        List<TblAlertSubscriptSettingsTO> ConvertDTToList(SqlDataReader tblAlertSubscriptSettingsTODT);
        int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO);
        int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlCommand cmdInsert);
        int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO);
        int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlCommand cmdUpdate);
        int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings);
        int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSubscriSettings, SqlCommand cmdDelete);

    }
}