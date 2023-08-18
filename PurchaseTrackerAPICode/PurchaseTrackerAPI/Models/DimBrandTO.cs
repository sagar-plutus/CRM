using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class DimBrandTO
    {
        #region Declarations
        Int32 idBrand;
        Int32 isActive;
        DateTime createdOn;
        String brandName;
        #endregion

        #region Constructor
        public DimBrandTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBrand
        {
            get { return idBrand; }
            set { idBrand = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }
        #endregion
    }
}
