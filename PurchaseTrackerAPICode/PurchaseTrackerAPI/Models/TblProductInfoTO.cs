using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblProductInfoTO
    {
        #region Declarations
        Int32 idProduct;
        Int32 materialId;
        Int32 prodCatId;
        Int32 prodSpecId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double secWt;
        Double avgSecWt;
        Double stdLength;
        Double noOfPcs;
        Double avgBundleWt;
        String prodCatDesc;
        String prodSpecDesc;
        String materialDesc;
        Int32 brandId;  //Saket [2017-11-23] Added.

        #endregion

        #region Constructor
        public TblProductInfoTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProduct
        {
            get { return idProduct; }
            set { idProduct = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
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
        public Double SecWt
        {
            get { return secWt; }
            set { secWt = value; }
        }
        public Double AvgSecWt
        {
            get { return avgSecWt; }
            set { avgSecWt = value; }
        }
        public Double StdLength
        {
            get { return stdLength; }
            set { stdLength = value; }
        }
        public Double NoOfPcs
        {
            get { return noOfPcs; }
            set { noOfPcs = value; }
        }
        public Double AvgBundleWt
        {
            get { return avgBundleWt; }
            set { avgBundleWt = value; }
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

        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }

        #endregion
    }
}
