using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseSchStatusHistoryTO
    {
        #region Declarations
        Int32 idPurchaseSchStatusHistory;
        Int32 statusId;
        Int32 vehiclePhaseId;
        Int32 createdBy;
        Int32 purchaseScheduleSummaryId;
        DateTime createdOn;
        String comment;
        #endregion

        #region Constructor
        public TblPurchaseSchStatusHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseSchStatusHistory
        {
            get { return idPurchaseSchStatusHistory; }
            set { idPurchaseSchStatusHistory = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 VehiclePhaseId
        {
            get { return vehiclePhaseId; }
            set { vehiclePhaseId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        #endregion
    }
}
