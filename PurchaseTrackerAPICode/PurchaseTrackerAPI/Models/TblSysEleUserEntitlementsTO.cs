using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblSysEleUserEntitlementsTO
    {
        #region Declarations
        Int32 idUserEntitlement;
        Int32 userId;
        Int32 sysEleId;
        Int32 createdBy;
        DateTime createdOn;
        String permission;
        #endregion

        #region Constructor
        public TblSysEleUserEntitlementsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUserEntitlement
        {
            get { return idUserEntitlement; }
            set { idUserEntitlement = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 SysEleId
        {
            get { return sysEleId; }
            set { sysEleId = value; }
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
        public String Permission
        {
            get { return permission; }
            set { permission = value; }
        }
        #endregion
    }
}
