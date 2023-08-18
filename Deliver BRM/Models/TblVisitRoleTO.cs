using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblVisitRoleTO
    {
        #region Declarations
        Int16 isActive;
        Int32 idVisitRole;
        String visitRoleName;
        String visitRoleDesc;
        #endregion

        #region GetSet
        public Int16 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 IdVisitRole
        {
            get { return idVisitRole; }
            set { idVisitRole = value; }
        }
     
        public String VisitRoleName
        {
            get { return visitRoleName; }
            set { visitRoleName = value; }
        }

        public String VisitRoleDesc
        {
            get { return visitRoleDesc; }
            set { visitRoleDesc = value; }
        }

        #endregion
    }
}
