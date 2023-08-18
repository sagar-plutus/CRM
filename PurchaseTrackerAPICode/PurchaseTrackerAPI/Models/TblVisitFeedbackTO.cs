using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblVisitFeedbackTO
    {
        #region Declarations
        Int32 idVisitFeedback;
        Int32 visitId;
        Int32 issueId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        #endregion

        #region Constructor
        public TblVisitFeedbackTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVisitFeedback
        {
            get { return idVisitFeedback; }
            set { idVisitFeedback = value; }
        }
        public Int32 VisitId
        {
            get { return visitId; }
            set { visitId = value; }
        }
        public Int32 IssueId
        {
            get { return issueId; }
            set { issueId = value; }
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
        #endregion
    }
}
