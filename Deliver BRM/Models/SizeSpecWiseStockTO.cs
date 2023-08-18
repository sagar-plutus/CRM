using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class SizeSpecWiseStockTO
    {
        #region Declarations

        Int32 stockSummaryId;
        Int32 materialId;
        Int32 prodSpecId;
        Double noOfBundles;
        Double totalStock;
        Double loadedStock;
        Double balanceStock;
        String prodSpecDesc;
        String materialDesc;
        DateTime stockDate;
        Int32 confirmedBy;
        DateTime confirmedOn;
        Double todaysStock;

        Int32 productItemId;
        String itemDisplayName;
        #endregion

        #region Constructor
        public SizeSpecWiseStockTO()
        {

        }

        #endregion

        #region GetSet
       
        public Int32 StockSummaryId
        {
            get { return stockSummaryId; }
            set { stockSummaryId = value; }
        }        
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public Int32 ProdSpecId
        {
            get { return prodSpecId; }
            set { prodSpecId = value; }
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
        public Double LoadedStock
        {
            get { return loadedStock; }
            set { loadedStock = value; }
        }
        public Double BalanceStock
        {
            get { return balanceStock; }
            set { balanceStock = value; }
        }
        public String ProdSpecDesc
        {
            get { return prodSpecDesc; }
            set { prodSpecDesc = value; }
        }
        public String MaterialDesc
        {
            get { return materialDesc; }
            set { materialDesc = value; }
        }

        public DateTime StockDate
        {
            get
            {
                return stockDate;
            }

            set
            {
                stockDate = value;
            }
        }

        public int ConfirmedBy
        {
            get
            {
                return confirmedBy;
            }

            set
            {
                confirmedBy = value;
            }
        }

        public DateTime ConfirmedOn
        {
            get
            {
                return confirmedOn;
            }

            set
            {
                confirmedOn = value;
            }
        }

        public double TodaysStock { get => todaysStock; set => todaysStock = value; }
        public int ProductItemId { get => productItemId; set => productItemId = value; }
        public string ItemDisplayName { get => itemDisplayName; set => itemDisplayName = value; }
        #endregion
    }
}
