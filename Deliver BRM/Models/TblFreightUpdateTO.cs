using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblFreightUpdateTO
    {
        #region Declarations
        Int32 idFreightUpdate;
        Int32 districtId;
        Int32 talukaId;
        Int32 transporterId;
        Int32 vehicleTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Double freightAmt;
        String locationDesc;
        String districtDesc;
        String talukaDesc;
        String vehicleType;
        String transporterName;
        String createdUserName;
        #endregion

        #region Constructor
        public TblFreightUpdateTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdFreightUpdate
        {
            get { return idFreightUpdate; }
            set { idFreightUpdate = value; }
        }
        public Int32 DistrictId
        {
            get { return districtId; }
            set { districtId = value; }
        }
        public Int32 TalukaId
        {
            get { return talukaId; }
            set { talukaId = value; }
        }
        public Int32 TransporterId
        {
            get { return transporterId; }
            set { transporterId = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double FreightAmt
        {
            get { return freightAmt; }
            set { freightAmt = value; }
        }
        public String LocationDesc
        {
            get { return locationDesc; }
            set { locationDesc = value; }
        }

        public string DistrictDesc { get => districtDesc; set => districtDesc = value; }
        public string TalukaDesc { get => talukaDesc; set => talukaDesc = value; }
        public string VehicleType { get => vehicleType; set => vehicleType = value; }
        public string TransporterName { get => transporterName; set => transporterName = value; }
        public string CreatedUserName { get => createdUserName; set => createdUserName = value; }
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        #endregion
    }
}
