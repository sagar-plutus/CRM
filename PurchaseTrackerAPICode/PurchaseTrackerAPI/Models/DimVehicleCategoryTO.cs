using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class DimVehicleCategoryTO
    {
        #region Declarations
        Int32 idVehicleCategory;
        String vehicleCatName;
        #endregion

        #region Constructor
        public DimVehicleCategoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehicleCategory
        {
            get { return idVehicleCategory; }
            set { idVehicleCategory = value; }
        }
        public String VehicleCatName
        {
            get { return vehicleCatName; }
            set { vehicleCatName = value; }
        }
        #endregion
    }
}
