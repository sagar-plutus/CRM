using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblUnLoadingItemDetTO
    {
        #region Declarations
        Int32 idUnloadingItemDet;
        Int32 unLoadingId;
        Int32 productCatId;
        Int32 weightMeasurUnitId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double unLoadingQty;
        Double weightedQty;
        String productCatName;
        #endregion

        #region Constructor
        public TblUnLoadingItemDetTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUnloadingItemDet
        {
            get { return idUnloadingItemDet; }
            set { idUnloadingItemDet = value; }
        }
        public Int32 UnLoadingId
        {
            get { return unLoadingId; }
            set { unLoadingId = value; }
        }
        public Int32 ProductCatId
        {
            get { return productCatId; }
            set { productCatId = value; }
        }
        public Int32 WeightMeasurUnitId
        {
            get { return weightMeasurUnitId; }
            set { weightMeasurUnitId = value; }
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
        public Double UnLoadingQty
        {
            get { return unLoadingQty; }
            set { unLoadingQty = value; }
        }
        public Double WeightedQty
        {
            get { return weightedQty; }
            set { weightedQty = value; }
        }

        public String ProductCatName
        {
            get { return productCatName; }
            set { productCatName = value; }
        }
        #endregion
    }
}
