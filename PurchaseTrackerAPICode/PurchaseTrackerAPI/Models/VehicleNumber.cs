using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class VehicleNumber
    {
        #region Declaration

        String stateCode;
        String districtCode;
        String uniqueLetters;
        Int32 vehicleNo;

        #endregion

        #region Get Set
        public string StateCode
        {
            get
            {
                return stateCode;
            }

            set
            {
                stateCode = value;
            }
        }

        public string DistrictCode
        {
            get
            {
                return districtCode;
            }

            set
            {
                districtCode = value;
            }
        }

        public string UniqueLetters
        {
            get
            {
                return uniqueLetters;
            }

            set
            {
                uniqueLetters = value;
            }
        }

        public int VehicleNo
        {
            get
            {
                return vehicleNo;
            }

            set
            {
                vehicleNo = value;
            }
        }

        #endregion
    }
}
