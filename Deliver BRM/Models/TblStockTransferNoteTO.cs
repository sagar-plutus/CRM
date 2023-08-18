using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblStockTransferNoteTO
    {
        #region Declarations
        Int32 idStkTransferNote;
        Int32 locationId;
        Int32 prodCatId;
        Int32 prodSpecId;
        Int32 materialId;
        Int32 stockQtyBundles;
        Int32 txnOpTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Double stockQtyMT;
        String remark;
        #endregion

        #region Constructor
        public TblStockTransferNoteTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStkTransferNote
        {
            get { return idStkTransferNote; }
            set { idStkTransferNote = value; }
        }
        public Int32 LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }
        public Int32 ProdCatId
        {
            get { return prodCatId; }
            set { prodCatId = value; }
        }
        public Int32 ProdSpecId
        {
            get { return prodSpecId; }
            set { prodSpecId = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public Int32 StockQtyBundles
        {
            get { return stockQtyBundles; }
            set { stockQtyBundles = value; }
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
        public Double StockQtyMT
        {
            get { return stockQtyMT; }
            set { stockQtyMT = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        #endregion
    }
}
