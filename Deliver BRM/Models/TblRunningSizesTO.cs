using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblRunningSizesTO
    {
        #region Declarations
        Int32 idRunningSize;
        Int32 locationId;
        Int32 prodCatId;
        Int32 materialId;
        Int32 prodSpecId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime stockDate;
        DateTime createdOn;
        DateTime updatedOn;
        Double noOfBundles;
        Double totalStock;
        String locationName;
        String materialDesc;

        #endregion

        #region Constructor
        public TblRunningSizesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdRunningSize
        {
            get { return idRunningSize; }
            set { idRunningSize = value; }
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
        public DateTime StockDate
        {
            get { return stockDate; }
            set { stockDate = value; }
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

        public string LocationName
        {
            get
            {
                return locationName;
            }

            set
            {
                locationName = value;
            }
        }

        public string MaterialDesc
        {
            get
            {
                return materialDesc;
            }

            set
            {
                materialDesc = value;
            }
        }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        #endregion
    }
}
