using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblAlertActionDtlTO
    {
        #region Declarations
        Int32 idAlertActionDtl;
        Int32 alertInstanceId;
        Int32 userId;
        Int32 snoozeTime;
        Int32 snoozeCount;
        DateTime acknowledgedOn;
        DateTime snoozeOn;
        DateTime resetDate;
        #endregion

        #region Constructor
        public TblAlertActionDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdAlertActionDtl
        {
            get { return idAlertActionDtl; }
            set { idAlertActionDtl = value; }
        }
        public Int32 AlertInstanceId
        {
            get { return alertInstanceId; }
            set { alertInstanceId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 SnoozeTime
        {
            get { return snoozeTime; }
            set { snoozeTime = value; }
        }
        public Int32 SnoozeCount
        {
            get { return snoozeCount; }
            set { snoozeCount = value; }
        }
        public DateTime AcknowledgedOn
        {
            get { return acknowledgedOn; }
            set { acknowledgedOn = value; }
        }
        public DateTime SnoozeOn
        {
            get { return snoozeOn; }
            set { snoozeOn = value; }
        }
        public DateTime ResetDate
        {
            get { return resetDate; }
            set { resetDate = value; }
        }
        #endregion
    }
}
