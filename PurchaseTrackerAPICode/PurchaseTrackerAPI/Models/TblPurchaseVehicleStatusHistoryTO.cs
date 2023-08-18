using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseVehicleStatusHistoryTO
    {
        #region Declarations
        Int32 idPurVehStatusHistory;
        Int32 purchaseScheduleSummaryId;
        Int32 statusId;
        Int32 vehiclePhaseId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String comment;
        #endregion

        #region Constructor
        public TblPurchaseVehicleStatusHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurVehStatusHistory
        {
            get { return idPurVehStatusHistory; }
            set { idPurVehStatusHistory = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
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
        public String Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        #endregion
    }
}
