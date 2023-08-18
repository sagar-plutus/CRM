using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblRoleTO
    {
        #region Declarations
        Int32 idRole;
        Int32 isActive;
        Int32 isSystem;
        Int32 createdBy;
        Int32 enableAreaAlloc;
        Int32 orgStructureId;
        DateTime createdOn;
        String roleDesc;
        #endregion

        #region Constructor
        public TblRoleTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdRole
        {
            get { return idRole; }
            set { idRole = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 IsSystem
        {
            get { return isSystem; }
            set { isSystem = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 EnableAreaAlloc
        {
            get { return enableAreaAlloc; }
            set { enableAreaAlloc = value; }
        }
        public Int32 OrgStructureId
        {
            get { return orgStructureId; }
            set { orgStructureId = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String RoleDesc
        {
            get { return roleDesc; }
            set { roleDesc = value; }
        }
        #endregion
    }
}
