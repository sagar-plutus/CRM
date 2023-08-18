using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseEnquiryScheduleTO
    {
        #region Declarations

        Int32 idSchedulePurchase;
        Int32 purchaseEnquiryId;
        Int32 prodItemId;
        DateTime scheduleDate;
        Double qty;
        Double scheduleQty;
        String remark;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        String vehicleNo;
        String itemName;
        Int32 supplierId;
        Int32 tempScheduleId;
        Int32 tempVehicleId;
        Double calculatedMetalCost;
        Double baseMetalCost;
        Double padta;

        //Nikhil[2018-05-25] Added
        String driverName;
        String driverContactNo;
        String transporterName;
        Int32 vehicleTypeId;
        String vehicleTypeName;
        Double freight;
        String containerNo;
        Int32 vehicleStateId;
        String vehicleStateName;
        String location;
        Int32 statusId;

        List<TblPurchaseVehicleDetailsTO> bookingVehicleDetailsTOList;
        #endregion

        #region Constructor
        public TblPurchaseEnquiryScheduleTO()
        {
        }

        #endregion

        #region GetSet

        public Double Padta
        {
            get { return padta; }
            set { padta = value; }
        }
        public Double CalculatedMetalCost
        {
            get { return calculatedMetalCost; }
            set { calculatedMetalCost = value; }
        }
        public Double BaseMetalCost
        {
            get { return baseMetalCost; }
            set { baseMetalCost = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        public Int32 IdSchedulePurchase
        {
            get { return idSchedulePurchase; }
            set { idSchedulePurchase = value; }
        }
        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }

        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
        }
        public String ScheduleDateStr
        {
            get { return scheduleDate.ToString("yyyy-MM-dd"); }
        }
        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        public Double ScheduleQty
        {
            get { return scheduleQty; }
            set { scheduleQty = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
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
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public Int32 TempScheduleId
        {
            get { return tempScheduleId; }
            set { tempScheduleId = value; }
        }
        public Int32 TempVehicleId
        {
            get { return tempVehicleId; }
            set { tempVehicleId = value; }
        }

        public String DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        public String DriverContactNo
        {
            get { return driverContactNo; }
            set { driverContactNo = value; }
        }
        public String TransporterName
        {
            get { return transporterName; }
            set { transporterName = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
        }
        public String VehicleTypeName
        {
            get { return vehicleTypeName; }
            set { vehicleTypeName = value; }
        }
        public Double Freight
        {
            get { return freight; }
            set { freight = value; }
        }
        public String ContainerNo
        {
            get { return containerNo; }
            set { containerNo = value; }
        }
        public Int32 VehicleStateId
        {
            get { return vehicleStateId; }
            set { vehicleStateId = value; }
        }
        public String VehicleStateName
        {
            get { return vehicleStateName; }
            set { vehicleStateName = value; }
        }
        public String Location
        {
            get { return location; }
            set { location = value; }
        }

        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }

        public List<TblPurchaseVehicleDetailsTO> BookingVehicleDetailsTOList { get => bookingVehicleDetailsTOList; set => bookingVehicleDetailsTOList = value; }

        #endregion
    }
}
