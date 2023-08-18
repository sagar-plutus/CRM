using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblRateDeclareReasonsTO
    {
        #region Declarations
        Int32 idRateReason;
        Int32 createdBy;
        DateTime createdOn;
        String reasonDesc;
        #endregion

        #region Constructor
        public TblRateDeclareReasonsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdRateReason
        {
            get { return idRateReason; }
            set { idRateReason = value; }
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
        public String ReasonDesc
        {
            get { return reasonDesc; }
            set { reasonDesc = value; }
        }
        #endregion
    }
}
