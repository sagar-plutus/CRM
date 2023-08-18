using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblVehicleTO
    {
        #region Declarations
        Int32 idVehicle;
        Int32 vehicleTypeId;
        Int32 approvalUserId;
        Int32 approvalStatusId;
        Int32 approvedBy;
        Int32 vehicleStatusId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime approvedOn;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 isActive;
        String vehicleNo;
        String narration;
        String vehicalType;
        String approvalUser;
        String approvalStatus;
        String approvalByName;
        String vehicalStatusName;
        String createdByName; 

        #endregion

        #region Constructor
        public TblVehicleTO()
        {
        }

        #endregion

        #region GetSet
        public String VehicalType
        {
            get { return vehicalType; }
            set { vehicalType = value; }
        }
        public String ApprovalUser
        {
            get { return approvalUser; }
            set { approvalUser = value; }
        }
        public String ApprovalStatus
        {
            get { return approvalStatus; }
            set { approvalStatus = value; }
        }
        public String ApprovalByName
        {
            get { return approvalByName; }
            set { approvalByName = value; }
        }
        public String VehicalStatusName
        {
            get { return vehicalStatusName; }
            set { vehicalStatusName = value; }
        }
        
            public String CreatedByName
        {
            get { return createdByName; }
            set { createdByName = value; }
        }
        public Int32 IdVehicle
        {
            get { return idVehicle; }
            set { idVehicle = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
        }
        public Int32 ApprovalUserId
        {
            get { return approvalUserId; }
            set { approvalUserId = value; }
        }
        public Int32 ApprovalStatusId
        {
            get { return approvalStatusId; }
            set { approvalStatusId = value; }
        }
        public Int32 ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }
        public Int32 VehicleStatusId
        {
            get { return vehicleStatusId; }
            set { vehicleStatusId = value; }
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
        public DateTime ApprovedOn
        {
            get { return approvedOn; }
            set { approvedOn = value; }
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
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        #endregion
    }
}
