using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertEscalationSettingsDAO
    {
        String SqlSelectQuery();
          List<TblAlertEscalationSettingsTO> SelectAllTblAlertEscalationSettings();
        TblAlertEscalationSettingsTO SelectTblAlertEscalationSettings(Int32 idEscalationSetting);
        List<TblAlertEscalationSettingsTO> ConvertDTToList(SqlDataReader tblAlertEscalationSettingsTODT);
        int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO);
          int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteInsertionCommand(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlCommand cmdInsert);
        int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO);
        int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlCommand cmdUpdate);
        int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting);
        int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idEscalationSetting, SqlCommand cmdDelete);

    }
}