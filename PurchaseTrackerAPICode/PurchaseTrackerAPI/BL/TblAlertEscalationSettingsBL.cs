using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblAlertEscalationSettingsBL: ITblAlertEscalationSettingsBL
    {
        private readonly ITblAlertEscalationSettingsDAO _itblAlertEscalationSettingsDAO;
        public TblAlertEscalationSettingsBL(ITblAlertEscalationSettingsDAO itblAlertEscalationSettingsDAO) {
            _itblAlertEscalationSettingsDAO = itblAlertEscalationSettingsDAO;

        }

        #region Selection

        public  List<TblAlertEscalationSettingsTO> SelectAllTblAlertEscalationSettingsList()
        {
           return  _itblAlertEscalationSettingsDAO.SelectAllTblAlertEscalationSettings();
        }

        public  TblAlertEscalationSettingsTO SelectTblAlertEscalationSettingsTO(Int32 idEscalationSetting)
        {
            return  _itblAlertEscalationSettingsDAO.SelectTblAlertEscalationSettings(idEscalationSetting);
        }

        #endregion
        
        #region Insertion
        public  int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            return _itblAlertEscalationSettingsDAO.InsertTblAlertEscalationSettings(tblAlertEscalationSettingsTO);
        }

        public  int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertEscalationSettingsDAO.InsertTblAlertEscalationSettings(tblAlertEscalationSettingsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            return _itblAlertEscalationSettingsDAO.UpdateTblAlertEscalationSettings(tblAlertEscalationSettingsTO);
        }

        public  int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertEscalationSettingsDAO.UpdateTblAlertEscalationSettings(tblAlertEscalationSettingsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting)
        {
            return _itblAlertEscalationSettingsDAO.DeleteTblAlertEscalationSettings(idEscalationSetting);
        }

        public  int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertEscalationSettingsDAO.DeleteTblAlertEscalationSettings(idEscalationSetting, conn, tran);
        }

        #endregion
        
    }
}
