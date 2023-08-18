using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblInvoiceChangeOrgHistoryTO
    {
        #region Declarations
        Int32 idInvoiceChangeOrgHistory;
        Int32 invoiceId;
        Int32 dupInvoiceId;
        Int32 createdBy;
        DateTime createdOn;
        String actionDesc;
        #endregion

        #region Constructor
        public TblInvoiceChangeOrgHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdInvoiceChangeOrgHistory
        {
            get { return idInvoiceChangeOrgHistory; }
            set { idInvoiceChangeOrgHistory = value; }
        }
        public Int32 InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
        }
        public Int32 DupInvoiceId
        {
            get { return dupInvoiceId; }
            set { dupInvoiceId = value; }
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
        public String ActionDesc
        {
            get { return actionDesc; }
            set { actionDesc = value; }
        }
        #endregion
    }
}
