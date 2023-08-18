using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class MarketingDetailsTO
    {
        #region Declarations

        TblVisitDetailsTO visitDetailsTO;
        TblSiteRequirementsTO requirementTO;
        TblVisitAdditionalDetailsTO additionalInfoTO;
        TblVisitFollowupInfoTO visitFollowUpInfoTo;
        List<TblVisitIssueDetailsTO> visitIssueDetailsTOList;
        List<TblVisitProjectDetailsTO> visitProjectDetailsTOList;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        #endregion

        #region Constructor
        public MarketingDetailsTO()
        {

        }

        #endregion

        #region GetSet

        public TblVisitDetailsTO VisitDetailsTO
        {
            get { return visitDetailsTO; }
            set { visitDetailsTO = value; }
        }

        public TblSiteRequirementsTO RequirementTO
        {
            get { return requirementTO; }
            set { requirementTO = value; }
        }

        public TblVisitAdditionalDetailsTO AdditionalInfoTO
        {
            get { return additionalInfoTO; }
            set { additionalInfoTO = value; }
        }

        public TblVisitFollowupInfoTO VisitFollowUpInfoTo
        {
            get { return visitFollowUpInfoTo; }
            set { visitFollowUpInfoTo = value; }
        }

        public List<TblVisitIssueDetailsTO> VisitIssueDetailsTOList
        {
            get { return visitIssueDetailsTOList; }
            set { visitIssueDetailsTOList = value; }
        }

        public List<TblVisitProjectDetailsTO> VisitProjectDetailsTOList
        {
            get { return visitProjectDetailsTOList; }
            set { visitProjectDetailsTOList = value; }
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

        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }

        #endregion
    }
}
