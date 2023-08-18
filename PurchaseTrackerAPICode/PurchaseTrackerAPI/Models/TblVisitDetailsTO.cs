using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblVisitDetailsTO
    {
        #region Declarations
        TimeSpan timeFrom;
        TimeSpan timeTo;
        Int16 roadLanes;
        Int32 idVisit;
        Int32 visitTypeId;
        Int32 visitPurposeId;
        //String visitPurposeName;
        Int32 siteSizeTypeId;
        //Int32 siteStatusId;
        String siteStatusName;
        Int32 paymentTermId;
        //String paymentTermName;
        // Int32 influencerId;
        DateTime visitDate;
        Double sizeOfSite;
        Double siteCost;
        String siteName;
        String notes;
        Int32 firmId;
        Int32 firmOwnerId;
        String visitePlace;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;

        Int32 siteOwnerTypeId;
        Int32 siteOwnerId;
        Int32 siteArchitectId;
        Int32 siteStructuralEnggId;
        Int32 contractorId;
        Int32 purchaseAuthorityPersonId;
        Int32 dealerId;
        Int32 dealerMeetingWithId;
        Int32 dealerVisitAlongWithDesignationId;
        Int32 dealerVisitAlongWithId;
        Int32 influencerVisitedBy;
        Int32 influencerRecommandedBy;
        Int32 siteStatusId;
        Int32 siteTypeId;
        Int32 siteSizeUnitId;
        Int32 siteCostUnitId;
         

        List<TblVisitPersonDetailsTO> visitPersonDetailsTOList;
        List<TblSiteTypeTO> siteTypeTOList;
        DropDownTO visitPurposeTO;
        DropDownTO paymentTermTO;
        DropDownTO siteStatusTO;
        #endregion

        #region Constructor
        public TblVisitDetailsTO()
        {

        }

        #endregion

        #region GetSet
        public TimeSpan TimeFrom
        {
            get { return timeFrom; }
            set { timeFrom = value; }
        }
        public TimeSpan TimeTo
        {
            get { return timeTo; }
            set { timeTo = value; }
        }
        public Int16 RoadLanes
        {
            get { return roadLanes; }
            set { roadLanes = value; }
        }
        public Int32 IdVisit
        {
            get { return idVisit; }
            set { idVisit = value; }
        }
        public Int32 VisitTypeId
        {
            get { return visitTypeId; }
            set { visitTypeId = value; }
        }
        public Int32 SiteSizeTypeId
        {
            get { return siteSizeTypeId; }
            set { siteSizeTypeId = value; }
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
        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
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
        public Double SizeOfSite
        {
            get { return sizeOfSite; }
            set { sizeOfSite = value; }
        }
        public Double SiteCost
        {
            get { return siteCost; }
            set { siteCost = value; }
        }
        public String SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }
        public String Notes
        {
            get { return notes; }
            set { notes = value; }
        }
        public Int32 FirmId
        {
            get { return firmId; }
            set { firmId = value; }
        }
        
            public Int32 FirmOwnerId
        {
            get { return firmOwnerId; }
            set { firmOwnerId = value; }
        }
        public String VisitePlace
        {
            get { return visitePlace; }
            set { visitePlace = value; }
        }

        public List<TblVisitPersonDetailsTO> VisitPersonDetailsTOList
        {
            get { return visitPersonDetailsTOList; }
            set { visitPersonDetailsTOList = value; }
        }

        public List<TblSiteTypeTO> SiteTypeTOList
        {
            get { return siteTypeTOList; }
            set { siteTypeTOList = value; }
        }
        public String SiteStatusName
        {
            get { return siteStatusName; }
            set { siteStatusName = value; }
        }

        public DropDownTO VisitPurposeTO
        {
            get { return visitPurposeTO; }
            set { visitPurposeTO = value; }
        }

        public DropDownTO PaymentTermTO
        {
            get { return paymentTermTO; }
            set { paymentTermTO = value; }
        }

        public DropDownTO SiteStatusTO
        {
            get { return siteStatusTO; }
            set { siteStatusTO = value; }
        }

        public Int32 SiteOwnerId
        {
            get { return siteOwnerId; }
            set { siteOwnerId = value; }
        }
        public Int32 SiteArchitectId
        {
            get { return siteArchitectId; }
            set { siteArchitectId = value; }
        }

        public Int32 SiteStructuralEnggId
        {
            get { return siteStructuralEnggId; }
            set { siteStructuralEnggId = value; }
        }

        public Int32 ContractorId
        {
            get { return contractorId; }
            set { contractorId = value; }
        }

        public Int32 PurchaseAuthorityPersonId
        {
            get { return purchaseAuthorityPersonId; }
            set { purchaseAuthorityPersonId = value; }
        }

        public Int32 DealerId
        {
            get { return dealerId; }
            set { dealerId = value; }
        }

        public Int32 DealerMeetingWithId
        {
            get { return dealerMeetingWithId; }
            set { dealerMeetingWithId = value; }
        }
        
        public Int32 DealerVisitAlongWithDesignationId
        {
            get { return dealerVisitAlongWithDesignationId; }
            set { dealerVisitAlongWithDesignationId = value; }
        }

        public Int32 DealerVisitAlongWithId
        {
            get { return dealerVisitAlongWithId; }
            set { dealerVisitAlongWithId = value; }
        }
        public Int32 InfluencerVisitedBy
        {
            get { return influencerVisitedBy; }
            set { influencerVisitedBy = value; }
        }
        public Int32 InfluencerRecommandedBy
        {
            get { return influencerRecommandedBy; }
            set { influencerRecommandedBy = value; }
        }

        public Int32 VisitPurposeId
        {
            get { return visitPurposeId; }
            set { visitPurposeId = value; }
        }
        public Int32 PaymentTermId
        {
            get { return paymentTermId; }
            set { paymentTermId = value; }
        }

        public Int32 SiteStatusId
        {
            get { return siteStatusId; }
            set { siteStatusId = value; }
        }
        public Int32 SiteTypeId
        {
            get { return siteTypeId; }
            set { siteTypeId = value; }
        }
        public Int32 SiteSizeUnitId
        {
            get { return siteSizeUnitId; }
            set { siteSizeUnitId = value; }
        }
        public Int32 SiteCostUnitId
        {
            get { return siteCostUnitId; }
            set { siteCostUnitId = value; }
        }

        public Int32 SiteOwnerTypeId
        {
            get { return siteOwnerTypeId; }
            set { siteOwnerTypeId = value; }
        }
        
        #endregion
    }
}
