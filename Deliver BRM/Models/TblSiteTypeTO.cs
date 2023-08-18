using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblSiteTypeTO
    {
        #region Declarations
        Int32 idSiteType;
        Int32 parentSiteTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int16 isActive;
        Int32 dimSiteTypeId;
        String siteTypeDisplayName;
        String siteTypeDesc;
        #endregion

        #region Constructor
        public TblSiteTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSiteType
        {
            get { return idSiteType; }
            set { idSiteType = value; }
        }
        public Int32 ParentSiteTypeId
        {
            get { return parentSiteTypeId; }
            set { parentSiteTypeId = value; }
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
        public Int16 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 DimSiteTypeId
        {
            get { return dimSiteTypeId; }
            set { dimSiteTypeId = value; }
        }
        public String SiteTypeDisplayName
        {
            get { return siteTypeDisplayName; }
            set { siteTypeDisplayName = value; }
        }
        public String SiteTypeDesc
        {
            get { return siteTypeDesc; }
            set { siteTypeDesc = value; }
        }
        #endregion
    }
}
