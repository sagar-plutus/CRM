using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblAlertEscalationSettingsTO
    {
        #region Declarations
        Int32 idEscalationSetting;
        Int32 alertDefId;
        Int32 userId;
        Int32 roleId;
        Int32 escalationPeriodMin;
        Int32 createdBy;
        DateTime createdOn;
        #endregion

        #region Constructor
        public TblAlertEscalationSettingsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdEscalationSetting
        {
            get { return idEscalationSetting; }
            set { idEscalationSetting = value; }
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
        public Int32 EscalationPeriodMin
        {
            get { return escalationPeriodMin; }
            set { escalationPeriodMin = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        #endregion
    }
}
