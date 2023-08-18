using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblSiteStatusTO
    {
        #region Declarations
        Int32 idSiteStatus;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        String siteStatusCode;
        String siteStatusDisplayName;
        String siteStatusDesc;
        #endregion

        #region Constructor
        public TblSiteStatusTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSiteStatus
        {
            get { return idSiteStatus; }
            set { idSiteStatus = value; }
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
        public String SiteStatusCode
        {
            get { return siteStatusCode; }
            set { siteStatusCode = value; }
        }
        public String SiteStatusDisplayName
        {
            get { return siteStatusDisplayName; }
            set { siteStatusDisplayName = value; }
        }
        public String SiteStatusDesc
        {
            get { return siteStatusDesc; }
            set { siteStatusDesc = value; }
        }
        #endregion
    }
}
