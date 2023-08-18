using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblInvoiceOtherDetailsTO
    {
        #region Declarations
        Int32 idInvoiceOtherDetails;
        Int32 orgId;
        Int32 createdBy;
        DateTime createdOn;
        String description;
        Int32 detailTypeId;
        #endregion

        #region Constructor
        public TblInvoiceOtherDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdInvoiceOtherDetails
        {
            get { return idInvoiceOtherDetails; }
            set { idInvoiceOtherDetails = value; }
        }
        public Int32 OrgId
        {
            get { return orgId; }
            set { orgId = value; }
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
        public String Description
        {
            get { return description; }
            set { description = value; }
        }
        public Int32 DetailTypeId
        {
            get { return detailTypeId; }
            set { detailTypeId = value; }
        }
        #endregion
    }
}
