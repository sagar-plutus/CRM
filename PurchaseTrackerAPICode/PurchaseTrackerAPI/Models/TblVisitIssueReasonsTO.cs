using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblVisitIssueReasonsTO
    {
        #region Declarations
        Int32 idVisitIssueReasons;
        Int32 issueTypeId;
        Int32 isActive;
        String visitIssueReasonName;
        String visitIssueReasonDesc;
        #endregion

        #region Constructor
        public TblVisitIssueReasonsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVisitIssueReasons
        {
            get { return idVisitIssueReasons; }
            set { idVisitIssueReasons = value; }
        }
        public Int32 IssueTypeId
        {
            get { return issueTypeId; }
            set { issueTypeId = value; }
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
        #endregion
    }
}
