using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseVehLinkSaudaTO
    {
        #region Declarations
        Int32 idVehLinkSauda;
        Int32 rootScheduleId;
        Int32 purchaseEnquiryId;
        Int32 createdBy;
        DateTime createdOn;
        Double linkedQty;
        Int32 isActive;
        Int32 isPrimarySauda;
        #endregion

        #region Constructor
        public TblPurchaseVehLinkSaudaTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehLinkSauda
        {
            get { return idVehLinkSauda; }
            set { idVehLinkSauda = value; }
        }
        public Int32 RootScheduleId
        {
            get { return rootScheduleId; }
            set { rootScheduleId = value; }
        }
        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
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
        public Double LinkedQty
        {
            get { return linkedQty; }
            set { linkedQty = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 IsPrimarySauda { get => isPrimarySauda; set => isPrimarySauda = value; }

        #endregion
    }
}
