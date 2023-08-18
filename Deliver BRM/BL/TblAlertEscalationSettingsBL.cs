using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblAlertEscalationSettingsBL
    {
        #region Selection
       
        public static List<TblAlertEscalationSettingsTO> SelectAllTblAlertEscalationSettingsList()
        {
           return  TblAlertEscalationSettingsDAO.SelectAllTblAlertEscalationSettings();
        }

        public static TblAlertEscalationSettingsTO SelectTblAlertEscalationSettingsTO(Int32 idEscalationSetting)
        {
            return  TblAlertEscalationSettingsDAO.SelectTblAlertEscalationSettings(idEscalationSetting);
        }

        #endregion
        
        #region Insertion
        public static int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            return TblAlertEscalationSettingsDAO.InsertTblAlertEscalationSettings(tblAlertEscalationSettingsTO);
        }

        public static int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertEscalationSettingsDAO.InsertTblAlertEscalationSettings(tblAlertEscalationSettingsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            return TblAlertEscalationSettingsDAO.UpdateTblAlertEscalationSettings(tblAlertEscalationSettingsTO);
        }

        public static int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertEscalationSettingsDAO.UpdateTblAlertEscalationSettings(tblAlertEscalationSettingsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting)
        {
            return TblAlertEscalationSettingsDAO.DeleteTblAlertEscalationSettings(idEscalationSetting);
        }

        public static int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertEscalationSettingsDAO.DeleteTblAlertEscalationSettings(idEscalationSetting, conn, tran);
        }

        #endregion
        
    }
}
