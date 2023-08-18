using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblVisitPersonDetailsTO :TblPersonTO
    {
        #region Declarations
        Int32 personId;
        Int32 visitRoleId;
        Int32 visitId;
        Int32 personTypeId;
        Int32 organizationId;
        String displayName;
        Int16 isSiteOwner;
        #endregion

        #region Constructor
        public TblVisitPersonDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        public Int32 PersonTypeId
        {
            get { return personTypeId; }
            set { personTypeId = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 VisitRoleId
        {
            get { return visitRoleId; }
            set { visitRoleId = value; }
        }
        public Int32 VisitId
        {
            get { return visitId; }
            set { visitId = value; }
        }
        public String DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public Int16 IsSiteOwner
        {
            get { return isSiteOwner; }
            set { isSiteOwner = value; }
        }

        #endregion
    }
}
