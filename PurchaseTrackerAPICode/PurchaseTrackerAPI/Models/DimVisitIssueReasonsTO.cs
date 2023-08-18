using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class DimVisitIssueReasonsTO
    {
        #region Declarations
        Int32 idVisitIssueReasons;
        Int32 isActive;
        String visitIssueReasonName;
        String visitIssueReasonDesc;
        Int32 issueTypeId;
        #endregion

        #region Constructor
        public DimVisitIssueReasonsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVisitIssueReasons
        {
            get { return idVisitIssueReasons; }
            set { idVisitIssueReasons = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String VisitIssueReasonName
        {
            get { return visitIssueReasonName; }
            set { visitIssueReasonName = value; }
        }
        public String VisitIssueReasonDesc
        {
            get { return visitIssueReasonDesc; }
            set { visitIssueReasonDesc = value; }
        }

        public Int32 IssueTypeId
        {
            get { return issueTypeId; }
            set { issueTypeId = value; }
        }
        #endregion
    }
}
