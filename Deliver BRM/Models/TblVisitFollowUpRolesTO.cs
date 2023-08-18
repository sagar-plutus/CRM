using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblVisitFollowUpRolesTO
    {
        #region Declarations
        Int32 idVisitFollowUpRole;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 followUpActionId;
        Int32 isActive;
        Int32 followUpRoleId; //Sudhir[20-NOV-2017] Added
        Int32 roleId; //Sudhir[20-NOV-2017] Added
        #endregion

        #region Constructor
        public TblVisitFollowUpRolesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVisitFollowUpRole
        {
            get { return idVisitFollowUpRole; }
            set { idVisitFollowUpRole = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }

        public Int32 FollowUpActionId
        {
            get { return followUpActionId; }
            set { followUpActionId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        
        public Int32 FollowUpRoleId
        {
            get { return followUpRoleId; }
            set { followUpRoleId = value; }
        }

        public Int32 RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        #endregion
    }
}
