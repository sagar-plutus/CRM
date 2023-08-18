using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblVisitIssueDetailsTO
    {
        #region Declarations
        Int32 idIssue;
        Int32 visitId;
        Int32 issueTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        String issueImage;
        Int32 issueReasonId;
        String issueComment;
        string issueReason;
        #endregion

        #region Constructor
        public TblVisitIssueDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdIssue
        {
            get { return idIssue; }
            set { idIssue = value; }
        }
        public Int32 VisitId
        {
            get { return visitId; }
            set { visitId = value; }
        }
        public Int32 IssueTypeId
        {
            get { return issueTypeId; }
            set { issueTypeId = value; }
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
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public String IssueImage
        {
            get { return issueImage; }
            set { issueImage = value; }
        }
        public Int32 IssueReasonId
        {
            get { return issueReasonId; }
            set { issueReasonId = value; }
        }

        public String IssueComment
        {
            get { return issueComment; }
            set { issueComment = value; }
        }

        public String IssueReason
        {
            get { return issueReason; }
            set { issueReason = value; }
        }
        #endregion
    }
}
