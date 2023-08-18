using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseVehicleOthertEntryTO
    {
        #region Declarations
        Int32 idVehicleOtherEntry;
        Int32 categoryId;
        Int32 vehicleTypeId;
       // Int32 statusId;
        Int32 createdBy;
       // DateTime statusDate;
        DateTime createdOn;
       // Double vehicleQtyMT;
       // String location;
        String vehicleNo;
        String driverName;
        String remark;
        //Int32 locationId;

       // String supplierName;
        String vehicleTypeDesc;
                
       // Int32 purchaseEnquiryId;

        String driverContactNo;
        Boolean isSelfVehicle;
       // Int32 stateId;
        #endregion

        #region Constructor
        public TblPurchaseVehicleOthertEntryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehicleOtherEntry
        {
            get { return idVehicleOtherEntry; }
            set { idVehicleOtherEntry = value; }
        }
        public Int32 CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
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
         
        
        
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        

        public String VehicleTypeDesc
        {
            get { return vehicleTypeDesc; }
            set { vehicleTypeDesc = value; }
        }
        public Boolean IsSelfVehicle
        {
            get { return isSelfVehicle; }
            set { isSelfVehicle = value; }
        }


        public String DriverContactNo
        {
            get { return driverContactNo; }
            set { driverContactNo = value; }
        }

         
        #endregion
    }
}
