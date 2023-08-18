using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblOrgLicenseDtlTO
    {
        #region Declarations
        Int32 idOrgLicense;
        Int32 organizationId;
        Int32 licenseId;
        Int32 createdBy;
        DateTime createdOn;
        String licenseValue;
        String licenseName;

        #endregion

        #region Constructor
        public TblOrgLicenseDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOrgLicense
        {
            get { return idOrgLicense; }
            set { idOrgLicense = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 LicenseId
        {
            get { return licenseId; }
            set { licenseId = value; }
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
        public String LicenseValue
        {
            get { return licenseValue; }
            set { licenseValue = value; }
        }

        public String LicenseName { get => licenseName; set => licenseName = value; }
        #endregion
    }
}
