using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class DimVehicleTypeTO
    {
        #region Declarations
        Int32 idVehicleType;
        String vehicleTypeDesc;
        #endregion

        #region Constructor
        public DimVehicleTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehicleType
        {
            get { return idVehicleType; }
            set { idVehicleType = value; }
        }
        public String VehicleTypeDesc
        {
            get { return vehicleTypeDesc; }
            set { vehicleTypeDesc = value; }
        }
        #endregion
    }
}
