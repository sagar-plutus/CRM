using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertEscalationSettingsBL
    {
        List<TblAlertEscalationSettingsTO> SelectAllTblAlertEscalationSettingsList();
        TblAlertEscalationSettingsTO SelectTblAlertEscalationSettingsTO(Int32 idEscalationSetting);
        int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO);
        int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO);
        int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting);
        int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting, SqlConnection conn, SqlTransaction tran);

    }
}