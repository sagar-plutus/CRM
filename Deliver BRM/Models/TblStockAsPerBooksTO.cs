using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblStockAsPerBooksTO
    {
        #region Declarations
        Int32 idStockAsPerBooks;
        Int32 isConfirmed;
        Int32 createdBy;
        DateTime createdOn;
        Double stockInMT;
        Double stockFactor;
        String remark;
        #endregion

        #region Constructor
        public TblStockAsPerBooksTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStockAsPerBooks
        {
            get { return idStockAsPerBooks; }
            set { idStockAsPerBooks = value; }
        }
        public Int32 IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
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
        public Double StockInMT
        {
            get { return stockInMT; }
            set { stockInMT = value; }
        }
        public Double StockFactor
        {
            get { return stockFactor; }
            set { stockFactor = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        #endregion
    }
}
