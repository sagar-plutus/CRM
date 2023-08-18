using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblStockConsumptionTO
    {
        #region Declarations
        Int32 idStockConsumption;
        Int32 stockDtlId;
        Int32 loadingSlipExtId;
        Int32 transferNoteId;
        Int32 txnOpTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Double beforeStockQty;
        Double afterStockQty;
        Double txnQty;
        String remark;
        #endregion

        #region Constructor
        public TblStockConsumptionTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStockConsumption
        {
            get { return idStockConsumption; }
            set { idStockConsumption = value; }
        }
        public Int32 StockDtlId
        {
            get { return stockDtlId; }
            set { stockDtlId = value; }
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
        public Double BeforeStockQty
        {
            get { return beforeStockQty; }
            set { beforeStockQty = value; }
        }
        public Double AfterStockQty
        {
            get { return afterStockQty; }
            set { afterStockQty = value; }
        }
        public Double TxnQty
        {
            get { return txnQty; }
            set { txnQty = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        #endregion
    }
}
