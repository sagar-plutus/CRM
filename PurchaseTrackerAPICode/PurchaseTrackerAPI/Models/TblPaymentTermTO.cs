using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPaymentTermTO
    {
        #region Declarations
        Int32 idPaymentTerm;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 isActive;
        String paymentTermDisplayName;
        String paymentTermDesc;
        #endregion

        #region Constructor
        public TblPaymentTermTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPaymentTerm
        {
            get { return idPaymentTerm; }
            set { idPaymentTerm = value; }
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
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public String PaymentTermDisplayName
        {
            get { return paymentTermDisplayName; }
            set { paymentTermDisplayName = value; }
        }
        public String PaymentTermDesc
        {
            get { return paymentTermDesc; }
            set { paymentTermDesc = value; }
        }
        #endregion
    }
}
