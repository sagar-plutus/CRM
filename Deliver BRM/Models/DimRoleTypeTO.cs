using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class DimRoleTypeTO
    {
        #region Declarations
        Int32 idRoleType;
        String roleTypeDesc;
       // String roleId;
        Int32 isActive;
        #endregion

        #region Constructor
        public DimRoleTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdRoleType
        {
            get { return idRoleType; }
            set { idRoleType = value; }
        }
        public String RoleTypeDesc
        {
            get { return roleTypeDesc; }
            set { roleTypeDesc = value; }
        }
        //public String RoleId
        //{
        //    get { return roleId; }
        //    set { roleId = value; }
        //}
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        #endregion
    }
}
