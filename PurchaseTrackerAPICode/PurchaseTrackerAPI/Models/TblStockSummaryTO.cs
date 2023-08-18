using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblStockSummaryTO
    {
        #region Declarations
        Int32 idStockSummary;
        Int32 confirmedBy;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime stockDate;
        DateTime confirmedOn;
        DateTime createdOn;
        DateTime updatedOn;
        Double noOfBundles;
        Double totalStock;
        List<TblStockDetailsTO> stockDetailsTOList;
        #endregion

        #region Constructor
        public TblStockSummaryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStockSummary
        {
            get { return idStockSummary; }
            set { idStockSummary = value; }
        }
        public Int32 ConfirmedBy
        {
            get { return confirmedBy; }
            set { confirmedBy = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime StockDate
        {
            get { return stockDate; }
            set { stockDate = value; }
        }
        public DateTime ConfirmedOn
        {
            get { return confirmedOn; }
            set { confirmedOn = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double NoOfBundles
        {
            get { return noOfBundles; }
            set { noOfBundles = value; }
        }
        public Double TotalStock
        {
            get { return totalStock; }
            set { totalStock = value; }
        }

        public List<TblStockDetailsTO> StockDetailsTOList
        {
            get
            {
                return stockDetailsTOList;
            }

            set
            {
                stockDetailsTOList = value;
            }
        }
        #endregion
    }
}
