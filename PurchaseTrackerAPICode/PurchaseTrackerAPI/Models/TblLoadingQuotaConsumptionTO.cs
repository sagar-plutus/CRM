using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingQuotaConsumptionTO
    {
        #region Declarations
        Int32 idLoadQuotaConsum;
        Int32 loadingQuotaId;
        Int32 loadingSlipExtId;
        Int32 transferNoteId;
        Int32 txnOpTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Double availableQuota;
        Double balanceQuota;
        Double quotaQty;
        String remark;
        #endregion

        #region Constructor
        public TblLoadingQuotaConsumptionTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadQuotaConsum
        {
            get { return idLoadQuotaConsum; }
            set { idLoadQuotaConsum = value; }
        }
        public Int32 LoadingQuotaId
        {
            get { return loadingQuotaId; }
            set { loadingQuotaId = value; }
        }
        public Int32 LoadingSlipExtId
        {
            get { return loadingSlipExtId; }
            set { loadingSlipExtId = value; }
        }
        public Int32 TransferNoteId
        {
            get { return transferNoteId; }
            set { transferNoteId = value; }
        }
        public Int32 TxnOpTypeId
        {
            get { return txnOpTypeId; }
            set { txnOpTypeId = value; }
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
        public Double AvailableQuota
        {
            get { return availableQuota; }
            set { availableQuota = value; }
        }
        public Double BalanceQuota
        {
            get { return balanceQuota; }
            set { balanceQuota = value; }
        }
        public Double QuotaQty
        {
            get { return quotaQty; }
            set { quotaQty = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        #endregion
    }
}
