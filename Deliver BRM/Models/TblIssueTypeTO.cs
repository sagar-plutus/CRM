using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblIssueTypeTO
    {
        #region Declarations
        Int32 idIssueType;
        Int32 isActive;
        String issueTypeName;
        #endregion

        #region Constructor
        public TblIssueTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdIssueType
        {
            get { return idIssueType; }
            set { idIssueType = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String IssueTypeName
        {
            get { return issueTypeName; }
            set { issueTypeName = value; }
        }
        #endregion
    }
}
