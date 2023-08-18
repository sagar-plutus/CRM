using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblUserRoleTO
    {
        #region Declarations
        Int32 idUserRole;
        Int32 userId;
        Int32 roleId;
        Int32 isActive;
        Int32 createdBy;
        DateTime createdOn;
        String roleDesc;
        Int32 enableAreaAlloc;
        Int32 roleTypeId;

        #endregion

        #region Constructor
        public TblUserRoleTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUserRole
        {
            get { return idUserRole; }
            set { idUserRole = value; }
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
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public String RoleDesc
        {
            get { return roleDesc; }
            set { roleDesc = value; }
        }

        public int EnableAreaAlloc { get => enableAreaAlloc; set => enableAreaAlloc = value; }
        public int RoleTypeId { get => roleTypeId; set => roleTypeId = value; }
        #endregion
    }
}
