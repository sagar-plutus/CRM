using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblAlertSubscribersTO
    {
        #region Declarations
        Int32 idSubscription;
        Int32 alertDefId;
        Int32 userId;
        Int32 roleId;
        Int32 subscribedBy;
        DateTime subscribedOn;
        List<TblAlertSubscriptSettingsTO> alertSubscriptSettingsTOList;

        #endregion

        #region Constructor
        public TblAlertSubscribersTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSubscription
        {
            get { return idSubscription; }
            set { idSubscription = value; }
        }
        public Int32 AlertDefId
        {
            get { return alertDefId; }
            set { alertDefId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }
        public Int32 SubscribedBy
        {
            get { return subscribedBy; }
            set { subscribedBy = value; }
        }
        public DateTime SubscribedOn
        {
            get { return subscribedOn; }
            set { subscribedOn = value; }
        }

        public List<TblAlertSubscriptSettingsTO> AlertSubscriptSettingsTOList
        {
            get
            {
                return alertSubscriptSettingsTOList;
            }

            set
            {
                alertSubscriptSettingsTOList = value;
            }
        }
        #endregion
    }
}
