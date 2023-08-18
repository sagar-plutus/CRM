using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblTallyStockTransferRptTO
    {

        #region Declarations
        String date;
        String voucherType;
        String stockItem;
        String godown;
        Double qty;
        Double rate;
        Double amount;
        String stockItemD;
        String godownD;
        Double qtyD;
        Double rateD;
        Double amountD;
        String narration;
        #endregion

        #region Constructor

        #endregion

        #region GetSet
        
        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public String VoucherType
        {
            get { return voucherType; }
            set { voucherType = value; }
        }
        public String StockItem
        {
            get { return stockItem; }
            set { stockItem = value; }
        }
        public String Godown
        {
            get { return godown; }
            set { godown = value; }
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
        public Double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public String StockItemD
        {
            get { return stockItemD; }
            set { stockItemD = value; }
        }
        public String GodownD
        {
            get { return godownD; }
            set { godownD = value; }
        }
        public Double QtyD
        {
            get { return qtyD; }
            set { qtyD = value; }
        }
        public Double RateD
        {
            get { return rateD; }
            set { rateD = value; }
        }
        public Double AmountD
        {
            get { return amountD; }
            set { amountD = value; }
        }
        
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        #endregion
    }
}
