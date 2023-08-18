using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblKYCDetailsTO
    {
        #region Declarations
        Int32 idKYCDetails;
        Int32 organizationId;
        Int32 aggrSign;
        Int32 chequeRcvd;
        Int32 kYCCompleted;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 isActive;
        #endregion

        #region Constructor
        public TblKYCDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdKYCDetails
        {
            get { return idKYCDetails; }
            set { idKYCDetails = value; }
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

        public Int32 AggrSign { get => aggrSign; set => aggrSign = value; }
        public Int32 ChequeRcvd { get => chequeRcvd; set => chequeRcvd = value; }
        public Int32 KYCCompleted { get => kYCCompleted; set => kYCCompleted = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        #endregion
    }
}
