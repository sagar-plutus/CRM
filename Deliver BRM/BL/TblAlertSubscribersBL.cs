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
    public class TblAlertSubscribersBL
    {
        #region Selection
        public static List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList()
        {
            return  TblAlertSubscribersDAO.SelectAllTblAlertSubscribers();
        }

        public static TblAlertSubscribersTO SelectTblAlertSubscribersTO(Int32 idSubscription)
        {
            return  TblAlertSubscribersDAO.SelectTblAlertSubscribers(idSubscription);
        }

        public static List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList(Int32 alertDefId,SqlConnection conn,SqlTransaction tran)
        {
            List<TblAlertSubscribersTO> list= TblAlertSubscribersDAO.SelectAllTblAlertSubscribers(alertDefId,conn,tran);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].AlertSubscriptSettingsTOList = BL.TblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsList(list[i].IdSubscription, conn, tran);
                }
            }

            return list;
        }
        /// <summary>
        /// Priyanka [27-09-2018] 
        /// </summary>
        /// <param name="alertDefId"></param>
        /// <returns></returns>
        public static List<TblAlertSubscribersTO> SelectTblAlertSubscribersByAlertDefId(Int32 alertDefId)
        {
            List<TblAlertSubscribersTO> list = TblAlertSubscribersDAO.SelectTblAlertSubscribersByAlertDefId(alertDefId);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    List<TblAlertSubscriptSettingsTO> AlertSubscriptSettingsTOListWithNotify = BL.TblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsList(list[i].IdSubscription);

                    AlertSubscriptSettingsTOListWithNotify.ForEach(f => f.SubscriptionId = list[i].IdSubscription);

                    list[i].AlertSubscriptSettingsTOList = AlertSubscriptSettingsTOListWithNotify;
                }
            }

            TblAlertSubscribersTO defaultTblAlertSubscribersTO = new TblAlertSubscribersTO();
            List<TblAlertSubscriptSettingsTO> temp = BL.TblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsListByAlertDefId(alertDefId);
            temp.ForEach(f => f.AlertDefId = alertDefId);
            defaultTblAlertSubscribersTO.AlertSubscriptSettingsTOList = temp;

            //list.Add(defaultTblAlertSubscribersTO);

            List<TblAlertSubscribersTO> mainReturnlist = new List<TblAlertSubscribersTO>();
            mainReturnlist.Add(defaultTblAlertSubscribersTO);

            if (list != null)
            {
                mainReturnlist.AddRange(list);
            }

            return mainReturnlist;
        }

        #endregion

        #region Insertion
        public static int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            return TblAlertSubscribersDAO.InsertTblAlertSubscribers(tblAlertSubscribersTO);
        }

        public static int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscribersDAO.InsertTblAlertSubscribers(tblAlertSubscribersTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            return TblAlertSubscribersDAO.UpdateTblAlertSubscribers(tblAlertSubscribersTO);
        }

        public static int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscribersDAO.UpdateTblAlertSubscribers(tblAlertSubscribersTO, conn, tran);
        }
        /// <summary>
        /// Priyanka [27-09-2018]
        /// </summary>
        /// <param name="tblAlertSubscribersTO"></param>
        /// <returns></returns>
        public static ResultMessage UpdateAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblAlertSubscriptSettingsTO> tblAlertSubscriptSettingsTOList = BL.TblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsList(tblAlertSubscribersTO.IdSubscription, conn, tran);
                if (tblAlertSubscriptSettingsTOList != null && tblAlertSubscriptSettingsTOList.Count > 0)
                {
                    for (int i = 0; i < tblAlertSubscriptSettingsTOList.Count; i++)
                    {
                        TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO = tblAlertSubscriptSettingsTOList[i];
                        tblAlertSubscriptSettingsTO.IsActive = 0;
                        tblAlertSubscriptSettingsTO.UpdatedOn = Constants.ServerDateTime;
                        tblAlertSubscriptSettingsTO.UpdatedBy = tblAlertSubscribersTO.UpdatedBy;

                        int result1 = BL.TblAlertSubscriptSettingsBL.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
                        if (result1 != 1)
                        {
                            resultMessage.DefaultBehaviour("Error... Record could not be saved");
                            return resultMessage;
                        }
                    }
                }

                int result = BL.TblAlertSubscribersBL.UpdateTblAlertSubscribers(tblAlertSubscribersTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateVehicleDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region Deletion
        public static int DeleteTblAlertSubscribers(Int32 idSubscription)
        {
            return TblAlertSubscribersDAO.DeleteTblAlertSubscribers(idSubscription);
        }

        public static int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscribersDAO.DeleteTblAlertSubscribers(idSubscription, conn, tran);
        }

        #endregion
        
    }
}
