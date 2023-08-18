using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblEnquiryDtlTO
    {
        #region Declarations
        Int32 idEnquiryDtl;
        Int32 organizationId;
        Int32 createdBy;
        DateTime asOnDate;
        DateTime createdOn;
        Double enquiryAmt;
        String partyName;
        Int32 isActive;

 
        String enqRefId;
        Int32 isMatch;

        #endregion

        #region Constructor
        public TblEnquiryDtlTO()
        {
        }
        #endregion

        #region GetSet

        public Int32 IdEnquiryDtl
        {
            get { return idEnquiryDtl; }
            set { idEnquiryDtl = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime AsOnDate
        {
            get { return asOnDate; }
            set { asOnDate = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double EnquiryAmt
        {
            get { return enquiryAmt; }
            set { enquiryAmt = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }

        public Int32 IsActive
        {
            get { return isActive;  }
            set { isActive = value; }
        }

     
        public String EnqRefId
        {
            get { return enqRefId; }
            set { enqRefId = value; }
        }

        public Int32 IsMatch
        {
            get { return isMatch; }
            set { isMatch = value; }
        }
        #endregion


    }
}
