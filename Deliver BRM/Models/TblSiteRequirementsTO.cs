using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblSiteRequirementsTO
    {
        #region Declarations
        Int32 idSiteRequirement;
        Int32 visitId;
        Int32 boughtFrom;
        Int32 brandId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double steelReqForTotalProject;
        Double boughtSoFor;
        Double immediateReq;
        #endregion

        #region Constructor
        public TblSiteRequirementsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSiteRequirement
        {
            get { return idSiteRequirement; }
            set { idSiteRequirement = value; }
        }
        public Int32 VisitId
        {
            get { return visitId; }
            set { visitId = value; }
        }
        public Int32 BoughtFrom
        {
            get { return boughtFrom; }
            set { boughtFrom = value; }
        }
        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
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
        public Double SteelReqForTotalProject
        {
            get { return steelReqForTotalProject; }
            set { steelReqForTotalProject = value; }
        }
        public Double BoughtSoFor
        {
            get { return boughtSoFor; }
            set { boughtSoFor = value; }
        }
        public Double ImmediateReq
        {
            get { return immediateReq; }
            set { immediateReq = value; }
        }
        #endregion
    }
}
