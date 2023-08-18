using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblVisitRoleTO
    {
        #region Declarations
        Int16 isActive;
        Int32 idVisitRole;
        Int32 createdBy;
        DateTime createdOn;
        String visitRoleName;
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
        public String VisitRoleName
        {
            get { return visitRoleName; }
            set { visitRoleName = value; }
        }
        #endregion
    }
}
