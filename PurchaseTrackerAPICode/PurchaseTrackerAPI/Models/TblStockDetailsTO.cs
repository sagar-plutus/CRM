using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblStockDetailsTO
    {
        #region Declarations
        Int32 idStockDtl;
        Int32 stockSummaryId;
        Int32 locationId;
        Int32 prodCatId;
        Int32 materialId;
        Int32 prodSpecId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double noOfBundles;
        Double totalStock;
        Double loadedStock;
        Double balanceStock;

        String locationName;
        String prodCatDesc;
        String prodSpecDesc;
        String materialDesc;
        Int32 productId;
        Double removedStock;
        Double todaysStock;

        //Saket [2017-11-23] Added.
        Int32 brandId;
        Int32 isConsolidatedStock;
        Int32 isInMT;
        Int32 prodItemId;

        Int32 otherItem;

        #endregion

        #region Constructor
        public TblStockDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStockDtl
        {
            get { return idStockDtl; }
            set { idStockDtl = value; }
        }
        public Int32 StockSummaryId
        {
            get { return stockSummaryId; }
            set { stockSummaryId = value; }
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
        public String LocationName
        {
            get { return locationName; }
            set { locationName = value; }
        }

        public String ProdCatDesc
        {
            get { return prodCatDesc; }
            set { prodCatDesc = value; }
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

        public int ProductId
        {
            get
            {
                return productId;
            }

            set
            {
                productId = value;
            }
        }

        public double RemovedStock { get => removedStock; set => removedStock = value; }
        public double TodaysStock { get => todaysStock; set => todaysStock = value; }

        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }

        public Int32 IsConsolidatedStock
        {
            get { return isConsolidatedStock; }
            set { isConsolidatedStock = value; }
        }

        public Int32 IsInMT
        {
            get { return isInMT; }
            set { isInMT = value; }
        }

        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }

        public int OtherItem { get => otherItem; set => otherItem = value; }

        #endregion
    }
}
