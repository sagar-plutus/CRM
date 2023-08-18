using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseUnloadingDtlTO
    {
        #region Declarations
        Int32 idPurchaseUnloadingDtl;
        Int32 purchaseWeighingStageId;
        Int32 weighingStageId;
        Int32 prodItemId;
        Int32 createdBy;
        DateTime createdOn;
        Double qtyMT;
        string itemName;
        Int32 purchaseScheduleSummaryId;

        Int32 isGradingBeforeUnld;
        Int32 isNextUnldGrade;

        Int32 isConfirmUnloading;

        Int32 isNonCommercialItem;
        Int32 isGradeSelected;
        Int32 isGradingUnldCompleted;
        Int64 intervalTime;
        double recovery;
        DateTime gradingEndTime;
        Int32 processVarId;
        string processVarDesc;
        #endregion

        #region Constructor
        public TblPurchaseUnloadingDtlTO()
        {
        }

        #endregion

        #region GetSet

        public DateTime GradingEndTime
        {
            get { return gradingEndTime; }
            set { gradingEndTime = value; }
        }
        public Int64 IntervalTime
        {
            get { return intervalTime; }
            set { intervalTime = value; }
        }
        public Int32 IdPurchaseUnloadingDtl
        {
            get { return idPurchaseUnloadingDtl; }
            set { idPurchaseUnloadingDtl = value; }
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

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public Int32 IsConfirmUnloading
        {
            get { return isConfirmUnloading; }
            set { isConfirmUnloading = value; }
        }

        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }

        public Int32 IsGradingBeforeUnld
        {
            get { return isGradingBeforeUnld; }
            set { isGradingBeforeUnld = value; }
        }

        public Int32 IsNextUnldGrade
        {
            get { return isNextUnldGrade; }
            set { isNextUnldGrade = value; }
        }

        public Int32 IsNonCommercialItem
        {
            get { return isNonCommercialItem; }
            set { isNonCommercialItem = value; }
        }



        public double Recovery
        {
            get { return recovery; }
            set { recovery = value; }
        }


        public Int32 IsGradeSelected
        {
            get { return isGradeSelected; }
            set { isGradeSelected = value; }
        }

        public Int32 IsGradingUnldCompleted
        {
            get { return isGradingUnldCompleted; }
            set { isGradingUnldCompleted = value; }
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
