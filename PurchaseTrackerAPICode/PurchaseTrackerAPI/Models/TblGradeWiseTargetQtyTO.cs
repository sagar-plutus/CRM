using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.StaticStuff;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblGradeWiseTargetQtyTO
    {
        #region Declarations
        Int32 idGradeWiseTargetQty;
        Int32 purchaseManagerId;
        Int32 prodItemId;
        Int32 isRestrict;
        Int32 prodClassId;
        Int32 globalRatePurchaseId;
        Double bookingTargetQty;
        Double unloadingTargetQty;
        Int32 rateBandPurchaseId;
        string prodClassName;
        Double pendingBookingQty;
        Double pendingUnloadingQty;
        string itemName;
        #endregion

        #region Constructor
        public TblGradeWiseTargetQtyTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGradeWiseTargetQty
        {
            get { return idGradeWiseTargetQty; }
            set { idGradeWiseTargetQty = value; }
        }
        public Int32 PurchaseManagerId
        {
            get { return purchaseManagerId; }
            set { purchaseManagerId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Int32 IsRestrict
        {
            get { return isRestrict; }
            set { isRestrict = value; }
        }
        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public Int32 GlobalRatePurchaseId
        {
            get { return globalRatePurchaseId; }
            set { globalRatePurchaseId = value; }
        }
        public Double BookingTargetQty
        {
            get { return bookingTargetQty; }
            set { bookingTargetQty = value; }
        }
        public Double UnloadingTargetQty
        {
            get { return unloadingTargetQty; }
            set { unloadingTargetQty = value; }
        }

        public Int32 RateBandPurchaseId
        {
            get { return rateBandPurchaseId; }
            set { rateBandPurchaseId = value; }
        }
        public string ProdClassName
        {
            get { return prodClassName; }
            set { prodClassName = value; }
        }
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public Double PendingBookingQty
        {
            get { return pendingBookingQty; }
            set { pendingBookingQty = value; }
        }
        public Double PendingUnloadingQty
        {
            get { return pendingUnloadingQty; }
            set { pendingUnloadingQty = value; }
        }

        #endregion
    }
}
