using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class RoleOrgTO
    {
        #region Declarations
        String role;
        Int32 roleId;

        Int32 idRoleOrg;
        Int32 orgId;
        String org;
        Int32 visitTypeId;
        Int32 personTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Boolean status;
        #endregion

        #region GetSet
        public Int32 RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }
        public Int32 OrgId
        {
            get { return orgId; }
            set { orgId = value; }
        }
        public String Role
        {
            get { return role; }
            set { role = value; }
        }

        public String Org
        {
            get { return org; }
            set { org = value; }
        }
        public Int32 PersonTypeId
        {
            get { return personTypeId; }
            set { personTypeId = value; }
        }

        public Int32 IdRoleOrg
        {
            get { return idRoleOrg; }
            set { idRoleOrg = value; }
        }
        public Int32 VisitTypeId
        {
            get { return visitTypeId; }
            set { visitTypeId = value; }

        }

        public Boolean Status
        {
            get { return status; }
            set { status = value; }
        }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        #endregion
    }
}
