using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI
{
    public class TblSpotEntryContainerDtlsTO
    {
        #region Declarations
        Int32 idSpotEntryContainerDtls;
        Int32 vehicleSpotEntryId;
        Int32 isActive;
        String containerNo;
        #endregion

        #region Constructor
        public TblSpotEntryContainerDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSpotEntryContainerDtls
        {
            get { return idSpotEntryContainerDtls; }
            set { idSpotEntryContainerDtls = value; }
        }
        public Int32 VehicleSpotEntryId
        {
            get { return vehicleSpotEntryId; }
            set { vehicleSpotEntryId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String ContainerNo
        {
            get { return containerNo; }
            set { containerNo = value; }
        }
        #endregion
    }
}
