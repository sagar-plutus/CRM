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
    public class TblAlertSubscriptSettingsBL: ITblAlertSubscriptSettingsBL
    {
        private readonly ITblAlertSubscriptSettingsDAO _iTblAlertSubscriptSettingsDAO;
        #region Selection

        public TblAlertSubscriptSettingsBL(ITblAlertSubscriptSettingsDAO iTblAlertSubscriptSettingsDAO) {
            _iTblAlertSubscriptSettingsDAO = iTblAlertSubscriptSettingsDAO;
        }

        public  List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList()
        {
            return  _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings();
        }

        public  TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsTO(Int32 idSubscriSettings)
        {
            return  _iTblAlertSubscriptSettingsDAO.SelectTblAlertSubscriptSettings(idSubscriSettings);
        }

        public  List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(subscriptionId,conn,tran);
        }

        #endregion

        #region Insertion
        public  int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            return _iTblAlertSubscriptSettingsDAO.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
        }

        public  int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            return _iTblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
        }

        public  int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings)
        {
            return _iTblAlertSubscriptSettingsDAO.DeleteTblAlertSubscriptSettings(idSubscriSettings);
        }

        public  int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.DeleteTblAlertSubscriptSettings(idSubscriSettings, conn, tran);
        }

        #endregion
        
    }
}
