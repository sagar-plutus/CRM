using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblVisitPersonTypeTO
    {
        #region Declarations
        Int32 idPersonType;
        Int32 isActive;
        String personTypeName;
        String personRoleDesc;
        #endregion

        #region Constructor
        public TblVisitPersonTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPersonType
        {
            get { return idPersonType; }
            set { idPersonType = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String PersonTypeName
        {
            get { return personTypeName; }
            set { personTypeName = value; }
        }
        public String PersonRoleDesc
        {
            get { return personRoleDesc; }
            set { personRoleDesc = value; }
        }
        #endregion
    }
}
