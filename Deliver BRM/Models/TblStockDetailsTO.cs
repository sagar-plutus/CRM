using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblStockDetailsTO
    {
        #region Declarations
        Int32 idStockDtl;

         Int32 cNFId1;
        Double bundles;
        Int32 sizeId;
        Int32 prodCatId1;
        Double stockInMT;
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

        //Sudhir[04-APR-2018] Added for Other Item.
        Int32 otherItem;
        Int32 prodItemId;

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

        public Int32 CNFId1
        {
            get { return cNFId1; }
            set { cNFId1 = value; }
        }

        public Double Bundles
        {
            get { return bundles; }
            set { bundles = value; }
        }
        public Int32 SizeId
        {
            get { return sizeId; }
            set { sizeId = value; }
        }

          public Double StockInMT
        {
            get { return stockInMT; }
            set { stockInMT = value; }
        }
          public Int32 ProdCatId1
        {
            get { return prodCatId1; }
            set { prodCatId1 = value; }
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
        public int OtherItem { get => otherItem; set => otherItem = value; }
        public int ProdItemId { get => prodItemId; set => prodItemId = value; }
        #endregion
    }
}
