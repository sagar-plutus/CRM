using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PurchaseTrackerAPI.StaticStuff.Constants;
namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseVehFreightDtlsTO
    {
        #region Declarations
        Int32 idPurchaseVehFreightDtls;
        Int32 purchaseScheduleSummaryId;
        Int32 isValidInfo;
        Int32 isStorageExcess;
        Int32 createdBy;
        DateTime createdOn;
        Double freight;
        Double advance;
        Double unloadingQty;
        Double shortage;
        Double amount;
        Double unloadingCharge;
        Double unloadingQtyAmt;
        Double bookingRate;
        Double actulWt;
        Int32 isFreightAdded;
        Int32 isdeleteGradingDts;
        #endregion

        #region Constructor
        public TblPurchaseVehFreightDtlsTO()
        {
        }

        #endregion

        #region GetSet

        public Double UnloadingQtyAmt
        {
            get { return unloadingQtyAmt; }
            set { unloadingQtyAmt = value; }
        }

        public Double BookingRate
        {
            get { return bookingRate; }
            set { bookingRate = value; }
        }

        public Double ActulWt
        {
            get { return actulWt; }
            set { actulWt = value; }
        }
        public Int32 IdPurchaseVehFreightDtls
        {
            get { return idPurchaseVehFreightDtls; }
            set { idPurchaseVehFreightDtls = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public Int32 IsValidInfo
        {
            get { return isValidInfo; }
            set { isValidInfo = value; }
        }
        public Int32 IsStorageExcess
        {
            get { return isStorageExcess; }
            set { isStorageExcess = value; }
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
        public Double Freight
        {
            get { return freight; }
            set { freight = value; }
        }
        public Double Advance
        {
            get { return advance; }
            set { advance = value; }
        }
        public Double UnloadingQty
        {
            get { return unloadingQty; }
            set { unloadingQty = value; }
        }
        public Double Shortage
        {
            get { return shortage; }
            set { shortage = value; }
        }
        public Double Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Double UnloadingCharge
        {
            get { return unloadingCharge; }
            set { unloadingCharge = value; }
        }

        public int IsFreightAdded
        {
            get { return isFreightAdded; }
            set { isFreightAdded = value; }
        }
        public int IsdeleteGradingDts
        {
            get { return isdeleteGradingDts; }
            set { isdeleteGradingDts = value; }
        }
        

        #endregion
    }
}
