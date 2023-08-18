using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblInspectionTO
    {
        #region Declarations

        Int32 idOutSideInspector;
        Int32 purchaseEnquiryId;
        Int32 vehiclePurchaseId;
        Int32 prodItemId;
        Double actualQty;
        String vehicleNo;
        Int32 statusId;
        DateTime statusDate;
        String statusReason;
        Int32 flag;
        Int32 supervisorId;
        Int32 engineerId;
        Int32 photographerId;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;

        #endregion

        #region Constructor
        public TblInspectionTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOutSideInspector
        {
            get { return idOutSideInspector; }
            set { idOutSideInspector = value; }
        }
        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
        }
        public Int32 VehiclePurchaseId
        {
            get { return vehiclePurchaseId; }
            set { vehiclePurchaseId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Double ActualQty
        {
            get { return actualQty; }
            set { actualQty = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public String StatusReason
        {
            get { return statusReason; }
            set { statusReason = value; }
        }
        public Int32 Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        public Int32 SupervisorId
        {
            get { return supervisorId; }
            set { supervisorId = value; }
        }
        public Int32 EngineerId
        {
            get { return engineerId; }
            set { engineerId = value; }
        }
        public Int32 PhotographerId
        {
            get { return photographerId; }
            set { photographerId = value; }
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
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }

       
        #endregion
    }
}
