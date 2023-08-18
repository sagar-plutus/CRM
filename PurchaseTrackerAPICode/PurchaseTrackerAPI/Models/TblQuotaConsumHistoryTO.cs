using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblQuotaConsumHistoryTO
    {
        #region Declarations
        Int32 idQuotaConsmHIstory;
        Int32 quotaDeclarationId;
        Int32 bookingId;
        Int32 txnOpTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Double availableQuota;
        Double balanceQuota;
        Double quotaQty;
        String remark;
        #endregion

        #region Constructor
        public TblQuotaConsumHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdQuotaConsmHIstory
        {
            get { return idQuotaConsmHIstory; }
            set { idQuotaConsmHIstory = value; }
        }
        public Int32 QuotaDeclarationId
        {
            get { return quotaDeclarationId; }
            set { quotaDeclarationId = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
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

        public Constants.TxnOperationTypeE TxnOperationTypeE
        {
            get
            {
                TxnOperationTypeE txnOperationTypeE = TxnOperationTypeE.NONE;
                if (Enum.IsDefined(typeof(TxnOperationTypeE), txnOpTypeId))
                {
                    txnOperationTypeE = (TxnOperationTypeE)txnOpTypeId;
                }
                return txnOperationTypeE;

            }
            set
            {
                txnOpTypeId = (int)value;
            }
        }

        #endregion
    }
}
