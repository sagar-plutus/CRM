using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
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


        //Priyanka [27-09-18]
        Int32 updatedBy;
        DateTime updatedOn;
        Int32 idNotificationType;
        String notificationTypeDesc;
        Int32 alertDefId;
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

        public int UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public int IdNotificationType { get => idNotificationType; set => idNotificationType = value; }
        public string NotificationTypeDesc { get => notificationTypeDesc; set => notificationTypeDesc = value; }
        public int AlertDefId { get => alertDefId; set => alertDefId = value; }
        #endregion
    }
}
