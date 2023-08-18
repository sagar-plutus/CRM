using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseEnquiryDetailsTO
    {
        #region Declarations

        Int32 idPurchaseEnquiryDetails;
        Int32 purchaseEnquiryId;
        Int32 prodItemId;
        Double qty;
        Double rate;
        Double productAomunt;
        Double productRecovery;
        Int32 schedulePurchaseId;
        Double pendingQty;
        Int32 loadingLayerId;

        String itemName;
        Double parityAmt;
        Double nonConfParityAmt;
        Double recovery;

        Double demandedRate;
        Int32 isNonCommercialItem;
        Double metalCost;
        Double totalCost;
        Double totalProduct;
        Double gradePadta;
        double actualRate;

        List<TblGradeExpressionDtlsTO> gradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();

        Double itemBookingRate;         //Priyanka [11-01-2019]
        #endregion

        #region Constructor
        public TblPurchaseEnquiryDetailsTO()
        {
        }

        #endregion

        #region 
        public Double ParityAmt
        {
            get { return parityAmt; }
            set { parityAmt = value; }
        }
        public Double NonConfParityAmt
        {
            get { return nonConfParityAmt; }
            set { nonConfParityAmt = value; }
        }
        public Double ActualRate
        {
            get { return actualRate; }
            set { actualRate = value; }
        }
        public Double Recovery
        {
            get { return recovery; }
            set { recovery = value; }
        }
        public Int32 IdPurchaseEnquiryDetails
        {
            get { return idPurchaseEnquiryDetails; }
            set { idPurchaseEnquiryDetails = value; }
        }
        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public Double ProductAomunt
        {
            get { return productAomunt; }
            set { productAomunt = value; }
        }

        public Double ProductRecovery
        {
            get { return productRecovery; }
            set { productRecovery = value; }
        }

        public Int32 SchedulePurchaseId
        {
            get { return schedulePurchaseId; }
            set { schedulePurchaseId = value; }
        }

        public Double PendingQty
        {
            get { return pendingQty; }
            set { pendingQty = value; }
        }

        public Int32 LoadingLayerId
        {
            get { return loadingLayerId; }
            set { loadingLayerId = value; }
        }

        public String ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        //Prajakta[2018-Nov-21] Added
        public Int32 IsNonCommercialItem
        {
            get { return isNonCommercialItem; }
            set { isNonCommercialItem = value; }
        }

        /// <summary>
        /// Sanjay [03 Oct 2018] Demanded Price added. suggested From Rajuri Still-MetaRoll-YKP Email dated 25 Sept
        /// </summary>
        public double DemandedRate { get => demandedRate; set => demandedRate = value; }

        public Double MetalCost
        {
            get { return metalCost; }
            set { metalCost = value; }
        }

        public Double TotalCost
        {
            get { return totalCost; }
            set { totalCost = value; }
        }
        public Double TotalProduct
        {
            get { return totalProduct; }
            set { totalProduct = value; }
        }
        public Double GradePadta
        {
            get { return gradePadta; }
            set { gradePadta = value; }
        }

        public List<TblGradeExpressionDtlsTO> GradeExpressionDtlsTOList
        {
            get { return gradeExpressionDtlsTOList; }
            set { gradeExpressionDtlsTOList = value; }
        }

        public double ItemBookingRate { get => itemBookingRate; set => itemBookingRate = value; }
        #endregion
    }
}
