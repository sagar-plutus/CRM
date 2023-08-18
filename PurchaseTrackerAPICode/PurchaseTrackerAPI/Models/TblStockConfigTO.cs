using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblStockConfigTO
    {
        #region Declarations
        Int32 idStockConfig;
        Int32 brandId;
        Int32 prodCatId;
        Int32 prodSpecId;
        Int32 materialId;
        Int32 isItemizedStock;
        String brandName;
        String prodCatName;
        String prodSpecName;
        String materialName;
        #endregion

        #region Constructor
        public TblStockConfigTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStockConfig
        {
            get { return idStockConfig; }
            set { idStockConfig = value; }
        }
        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
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
        public Int32 IsItemizedStock
        {
            get { return isItemizedStock; }
            set { isItemizedStock = value; }
        }

        public String BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }

        public String ProdCatName
        {
            get { return prodCatName; }
            set { prodCatName = value; }
        }

        public String ProdSpecName
        {
            get { return prodSpecName; }
            set { prodSpecName = value; }
        }

        public String MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }



        #endregion
    }
}
