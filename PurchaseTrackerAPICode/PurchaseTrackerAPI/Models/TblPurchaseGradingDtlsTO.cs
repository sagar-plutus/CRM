using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseGradingDtlsTO
    {
        #region Declarations
        Int32 idGradingDtls;
        Int32 purchaseScheduleSummaryId;
        Int32 purchaseWeighingStageId;
        Int32 weighingStageId;
        Int32 prodItemId;
        Int32 createdBy;
        DateTime createdOn;
        Double qtyMT;
        Double rate;
        Double productAmount;
        Int32 isConfirmGrading;
        Double recoveryPer;
        Int32 isNonCommercialItem;

        Double itemBookingRate;
        Int32 isGradeSelected;
        Int32 processVarId;
        string processVarDesc;
        #endregion

        #region Constructor
        public TblPurchaseGradingDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGradingDtls
        {
            get { return idGradingDtls; }
            set { idGradingDtls = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public Int32 PurchaseWeighingStageId
        {
            get { return purchaseWeighingStageId; }
            set { purchaseWeighingStageId = value; }
        }
        public Int32 WeighingStageId
        {
            get { return weighingStageId; }
            set { weighingStageId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
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
        public Double QtyMT
        {
            get { return qtyMT; }
            set { qtyMT = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public Double ProductAmount
        {
            get { return productAmount; }
            set { productAmount = value; }
        }

        public Int32 IsConfirmGrading
        {
            get { return isConfirmGrading; }
            set { isConfirmGrading = value; }
        }

        public Int32 IsNonCommercialItem
        {
            get { return isNonCommercialItem; }
            set { isNonCommercialItem = value; }
        }

        public double RecoveryPer { get => recoveryPer; set => recoveryPer = value; }
        public Double ItemBookingRate { get => itemBookingRate; set => itemBookingRate = value; }

        

        public Int32 IsGradeSelected
        {
            get { return isGradeSelected; }
            set { isGradeSelected = value; }
        }
        public Int32 ProcessVarId
        {
            get { return processVarId; }
            set { processVarId = value; }
        }
        public string ProcessVarDesc
        {
            get { return processVarDesc; }
            set { processVarDesc = value; }
        }
        


        #endregion
    }
}
