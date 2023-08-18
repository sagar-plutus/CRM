using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblAlertSubscriptSettingsTO
    {
        #region Declarations
        Int32 idSubscriSettings;
        Int32 subscriptionId;
        Int32 escalationSettingId;
        Int32 notificationTypeId;
        Int32 isActive;
        DateTime createdOn;
        #endregion

        #region Constructor
        public TblAlertSubscriptSettingsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSubscriSettings
        {
            get { return idSubscriSettings; }
            set { idSubscriSettings = value; }
        }
        public Int32 SubscriptionId
        {
            get { return subscriptionId; }
            set { subscriptionId = value; }
        }
        public Int32 EscalationSettingId
        {
            get { return escalationSettingId; }
            set { escalationSettingId = value; }
        }
        public Int32 NotificationTypeId
        {
            get { return notificationTypeId; }
            set { notificationTypeId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        #endregion
    }
}
