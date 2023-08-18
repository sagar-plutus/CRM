using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseEnqVehDescTO
    {
        #region Declarations
        Int32 idVehTypeDesc;
        Int32 purchaseEnqId;
        Int32 vehicleTypeId;
        String vehicleTypeDesc;
        #endregion

        #region Constructor
        public TblPurchaseEnqVehDescTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehTypeDesc
        {
            get { return idVehTypeDesc; }
            set { idVehTypeDesc = value; }
        }
        public Int32 PurchaseEnqId
        {
            get { return purchaseEnqId; }
            set { purchaseEnqId = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
        }
        public String VehicleTypeDesc
        {
            get { return vehicleTypeDesc; }
            set { vehicleTypeDesc = value; }
        }
        #endregion
    }
}
