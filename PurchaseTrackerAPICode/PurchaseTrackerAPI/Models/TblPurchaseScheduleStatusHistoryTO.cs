using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseScheduleStatusHistoryTO
    {
        #region Declarations
        Int32 idScheduleAuthHistory;
        Int32 purchaseScheduleSummaryId;
        Int32 statusId;
        Int32 phaseId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 acceptStatusId;
        Int32 acceptPhaseId;
        Int32 rejectStatusId;
        Int32 rejectPhaseId;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        String statusRemark;
        String navigationUrl;
        Int32 isIgnoreApproval;

        Int32 isApproved;
        Int32 isLatest;

        Int32 approvalType;
        #endregion

        #region Constructor
        public TblPurchaseScheduleStatusHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdScheduleAuthHistory
        {
            get { return idScheduleAuthHistory; }
            set { idScheduleAuthHistory = value; }
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
        public Int32 PhaseId
        {
            get { return phaseId; }
            set { phaseId = value; }
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
        public Int32 AcceptStatusId
        {
            get { return acceptStatusId; }
            set { acceptStatusId = value; }
        }
        public Int32 AcceptPhaseId
        {
            get { return acceptPhaseId; }
            set { acceptPhaseId = value; }
        }
        public Int32 RejectStatusId
        {
            get { return rejectStatusId; }
            set { rejectStatusId = value; }
        }
        public Int32 RejectPhaseId
        {
            get { return rejectPhaseId; }
            set { rejectPhaseId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 IsIgnoreApproval
        {
            get { return isIgnoreApproval; }
            set { isIgnoreApproval = value; }
        }
        public Int32 IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }
        public Int32 IsLatest
        {
            get { return isLatest; }
            set { isLatest = value; }
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
        public String StatusRemark
        {
            get { return statusRemark; }
            set { statusRemark = value; }
        }
        public String NavigationUrl
        {
            get { return navigationUrl; }
            set { navigationUrl = value; }
        }

        public Int32 ApprovalType
        {
            get { return approvalType; }
            set { approvalType = value; }
        }
        #endregion
    }
}
