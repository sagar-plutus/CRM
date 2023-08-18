using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblOverdueDtlTO
    {
        #region Declarations
        Int32 idOverdueDtl;
        Int32 organizationId;
        Int32 createdBy;
        DateTime asOnDate;
        DateTime createdOn;
        Double overdueAmt;
        String partyName;
        String overDueRefId;
        Int32 isMatch;
        #endregion

        #region Constructor
        public TblOverdueDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOverdueDtl
        {
            get { return idOverdueDtl; }
            set { idOverdueDtl = value; }
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
        public Double OverdueAmt
        {
            get { return overdueAmt; }
            set { overdueAmt = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }

         public String OverDueRefId
        {
            get { return overDueRefId; }
            set { overDueRefId = value; }
        }

        public Int32 IsMatch
        {
            get { return isMatch; }
            set { isMatch = value; }
        }
        #endregion
    }
}
