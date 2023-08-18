using System;
using System.Collections.Generic;
using System.Text;

namespace  PurchaseTrackerAPI.Models
{
    public class TblPurchaseEnquiryQtyConsumptionTO
    {
        #region Declarations
        Int32 idPurEnqQtyCons;
        Int32 purchaseEnqId;
        Int32 statusId;
        Int32 createdBy;
        DateTime createdOn;
        Double consumptionQty;
        String remark;
        Int32 isAuto;
        #endregion

        #region Constructor
        public TblPurchaseEnquiryQtyConsumptionTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurEnqQtyCons
        {
            get { return idPurEnqQtyCons; }
            set { idPurEnqQtyCons = value; }
        }
        public Int32 PurchaseEnqId
        {
            get { return purchaseEnqId; }
            set { purchaseEnqId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
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
        public Double ConsumptionQty
        {
            get { return consumptionQty; }
            set { consumptionQty = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public Int32 IsAuto
        {
            get { return isAuto; }
            set { isAuto = value; }
        }
        
        #endregion
    }
}
