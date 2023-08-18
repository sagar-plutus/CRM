using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblVisitAdditionalDetailsTO
    {
        #region Declarations
        Int16 orgAwareness;
        Int16 qualityAwareness;
        Int16 visitedFactory;
        Int16 materialUsedBefore;
        Int16 previousVisitedByRepresentative;
        Int16 satesfactoryLevel;
        Int16 isActive;
        Int32 idVisitDetails;
        Int32 visitId;
        Int32 siteComplaintReferredBy;
        Int32 communicationPersonId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime remindMeOn;
        DateTime createdOn;
        DateTime updatedOn;
        String otherSiteNotes;
        Int16 benifits;
        Int16 additionalFeatures;
        String comments;
        String suggestionsForOrg;
        Int32 giftId;
        Int32 designationId;

        List<TblVisitPersonDetailsTO> visitPersonDetailsTOList;
        DropDownTO designationTO;

        #endregion

        #region Constructor
        public TblVisitAdditionalDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int16 OrgAwareness
        {
            get { return orgAwareness; }
            set { orgAwareness = value; }
        }
        public Int16 QualityAwareness
        {
            get { return qualityAwareness; }
            set { qualityAwareness = value; }
        }
        public Int16 VisitedFactory
        {
            get { return visitedFactory; }
            set { visitedFactory = value; }
        }
        public Int16 MaterialUsedBefore
        {
            get { return materialUsedBefore; }
            set { materialUsedBefore = value; }
        }
        public Int16 PreviousVisitedByRepresentative
        {
            get { return previousVisitedByRepresentative; }
            set { previousVisitedByRepresentative = value; }
        }
        public Int16 SatesfactoryLevel
        {
            get { return satesfactoryLevel; }
            set { satesfactoryLevel = value; }
        }
        public Int16 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 IdVisitDetails
        {
            get { return idVisitDetails; }
            set { idVisitDetails = value; }
        }
        public Int32 VisitId
        {
            get { return visitId; }
            set { visitId = value; }
        }
        public Int32 SiteComplaintReferredBy
        {
            get { return siteComplaintReferredBy; }
            set { siteComplaintReferredBy = value; }
        }
        public Int32 CommunicationPersonId
        {
            get { return communicationPersonId; }
            set { communicationPersonId = value; }
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
        public DateTime RemindMeOn
        {
            get { return remindMeOn; }
            set { remindMeOn = value; }
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
        public String OtherSiteNotes
        {
            get { return otherSiteNotes; }
            set { otherSiteNotes = value; }
        }
        public Int16 Benifits
        {
            get { return benifits; }
            set { benifits = value; }
        }
        public Int16 AdditionalFeatures
        {
            get { return additionalFeatures; }
            set { additionalFeatures = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public string SuggestionsForOrg
        {
            get { return suggestionsForOrg; }
            set { suggestionsForOrg = value; }
        }

        public Int32 GiftId
        {
            get { return giftId; }
            set { giftId = value; }
        }

        public List<TblVisitPersonDetailsTO> VisitPersonDetailsTOList
        {
            get { return visitPersonDetailsTOList; }
            set { visitPersonDetailsTOList = value; }
        }

        public DropDownTO DesignationTO
        {
            get { return designationTO; }
            set { designationTO = value; }
        }

        public Int32 DesignationId
        {
            get { return designationId; }
            set { designationId = value; }
        }

        #endregion
    }
}
