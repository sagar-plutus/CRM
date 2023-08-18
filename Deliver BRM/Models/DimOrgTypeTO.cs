using System;

namespace SalesTrackerAPI.Models
{
    public class DimOrgTypeTO
    {
        #region Declarations
        Int32 idOrgType;
        Int32 isSystem;
        Int32 isActive;
        Int32 createUserYn;
        Int32 defaultRoleId;
        String orgType;
        #endregion

        #region Constructor
        public DimOrgTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOrgType
        {
            get { return idOrgType; }
            set { idOrgType = value; }
        }
        public Int32 IsSystem
        {
            get { return isSystem; }
            set { isSystem = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 CreateUserYn
        {
            get { return createUserYn; }
            set { createUserYn = value; }
        }
        public Int32 DefaultRoleId
        {
            get { return defaultRoleId; }
            set { defaultRoleId = value; }
        }
        public String OrgType
        {
            get { return orgType; }
            set { orgType = value; }
        }
        #endregion
    }
}
