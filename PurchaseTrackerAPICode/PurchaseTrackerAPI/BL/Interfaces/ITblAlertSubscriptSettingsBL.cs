using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertSubscriptSettingsBL
    {
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList();
        TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsTO(Int32 idSubscriSettings);
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId, SqlConnection conn, SqlTransaction tran);
        int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO);
        int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO);
        int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings);
        int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran);

    }
}