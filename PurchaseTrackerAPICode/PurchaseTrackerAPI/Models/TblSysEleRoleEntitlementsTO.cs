using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblSysEleRoleEntitlementsTO
    {
        #region Declarations
        Int32 idRoleEntitlement;
        Int32 roleId;
        Int32 sysEleId;
        Int32 createdBy;
        DateTime createdOn;
        String permission;
        #endregion

        #region Constructor
        public TblSysEleRoleEntitlementsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdRoleEntitlement
        {
            get { return idRoleEntitlement; }
            set { idRoleEntitlement = value; }
        }
        public Int32 RoleId
        {
            get { return roleId; }
            set { roleId = value; }
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
