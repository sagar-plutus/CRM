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
    public class TblAlertSubscribersBL : ITblAlertSubscribersBL
    {
        private readonly ITblAlertSubscribersDAO _itblAlertSubscribersDAO;
        private readonly ITblAlertSubscriptSettingsBL _iTblAlertSubscriptSettingsBL;

        public TblAlertSubscribersBL(ITblAlertSubscribersDAO itblAlertSubscribersDAO, 
            ITblAlertSubscriptSettingsBL iTblAlertSubscriptSettingsBL
            ) {
            _itblAlertSubscribersDAO = itblAlertSubscribersDAO;
            _iTblAlertSubscriptSettingsBL = iTblAlertSubscriptSettingsBL;
        }
        #region Selection
        public  List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList()
        {
            return  _itblAlertSubscribersDAO.SelectAllTblAlertSubscribers();
        }

        public  TblAlertSubscribersTO SelectTblAlertSubscribersTO(Int32 idSubscription)
        {
            return  _itblAlertSubscribersDAO.SelectTblAlertSubscribers(idSubscription);
        }

        public  List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList(Int32 alertDefId,SqlConnection conn,SqlTransaction tran)
        {
            List<TblAlertSubscribersTO> list= _itblAlertSubscribersDAO.SelectAllTblAlertSubscribers(alertDefId,conn,tran);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].AlertSubscriptSettingsTOList = _iTblAlertSubscriptSettingsBL.SelectAllTblAlertSubscriptSettingsList(list[i].IdSubscription, conn, tran);
                }
            }

            return list;
        }

        #endregion

        #region Insertion
        public  int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            return _itblAlertSubscribersDAO.InsertTblAlertSubscribers(tblAlertSubscribersTO);
        }

        public  int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertSubscribersDAO.InsertTblAlertSubscribers(tblAlertSubscribersTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            return _itblAlertSubscribersDAO.UpdateTblAlertSubscribers(tblAlertSubscribersTO);
        }

        public  int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertSubscribersDAO.UpdateTblAlertSubscribers(tblAlertSubscribersTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblAlertSubscribers(Int32 idSubscription)
        {
            return _itblAlertSubscribersDAO.DeleteTblAlertSubscribers(idSubscription);
        }

        public  int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertSubscribersDAO.DeleteTblAlertSubscribers(idSubscription, conn, tran);
        }

        #endregion
        
    }
}
