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
    public class TblAlertSubscriptSettingsBL
    {
        #region Selection
       
        public static List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList()
        {
            return  TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings();
        }

        public static TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsTO(Int32 idSubscriSettings)
        {
            return  TblAlertSubscriptSettingsDAO.SelectTblAlertSubscriptSettings(idSubscriSettings);
        }

        public static List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId,SqlConnection conn,SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(subscriptionId,conn,tran);
        }

        public static List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsListByAlertDefId(int alertDefId, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettingsByAlertDefId(alertDefId, conn, tran);
        }
        /// <summary>
        /// Priyanka [27-09-18] : Added to get the tblAlertSubscriptSetting TO from subscriptionId.
        /// </summary>
        /// <param name="NotificationTypeId"></param>
        /// <param name="SubscriptionId"></param>
        /// <param name="AlertDefId"></param>
        /// <returns></returns>
        public static TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsFromNotifyId(Int32 NotificationTypeId, Int32 SubscriptionId, Int32 AlertDefId)
        {
            return TblAlertSubscriptSettingsDAO.SelectTblAlertSubscriptSettingsFromNotifyId(NotificationTypeId, SubscriptionId, AlertDefId);
        }

        public static List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId)
        {
            return TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(subscriptionId);
        }

        //Priyanka [27-09-2018] : Added to get the alert subscription setting list by alert defination Id.
        public static List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsListByAlertDefId(int alertDefId)
        {
            return TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettingsByAlertDefId(alertDefId);
        }

        #endregion

        #region Insertion
        public static int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            return TblAlertSubscriptSettingsDAO.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
        }

        public static int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            return TblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
        }

        public static int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings)
        {
            return TblAlertSubscriptSettingsDAO.DeleteTblAlertSubscriptSettings(idSubscriSettings);
        }

        public static int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.DeleteTblAlertSubscriptSettings(idSubscriSettings, conn, tran);
        }

        #endregion
        
    }
}
