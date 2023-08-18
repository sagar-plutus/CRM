using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class DimPurchaseGradeQtyTypeTO
    {
        #region Declarations
        Int32 idPurchaseGradeQtyType;
        Int32 isActive;
        String purchaseGradeQtyType;
        #endregion

        #region Constructor
        public DimPurchaseGradeQtyTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseGradeQtyType
        {
            get { return idPurchaseGradeQtyType; }
            set { idPurchaseGradeQtyType = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String PurchaseGradeQtyType
        {
            get { return purchaseGradeQtyType; }
            set { purchaseGradeQtyType = value; }
        }
        #endregion
    }
}
