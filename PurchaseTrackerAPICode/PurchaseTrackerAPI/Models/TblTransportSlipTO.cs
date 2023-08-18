using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblTransportSlipTO
    {
        #region Declarations
        Int32 idTransportSlip;
        Int32 partyOrgId;
        Int32 transporterOrgId;
        Int32 vehicleTypeId;
        Int32 isFromDealer;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String partyName;
        String transporterName;
        String destination;
        String vehicleNo;
        String driverName;
        String contactNo;
        String comments;
        String refNo;
        Int32 loadingId;
        #endregion

        #region Constructor
        public TblTransportSlipTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTransportSlip
        {
            get { return idTransportSlip; }
            set { idTransportSlip = value; }
        }
        public Int32 PartyOrgId
        {
            get { return partyOrgId; }
            set { partyOrgId = value; }
        }
        public Int32 TransporterOrgId
        {
            get { return transporterOrgId; }
            set { transporterOrgId = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
        }
        public Int32 IsFromDealer
        {
            get { return isFromDealer; }
            set { isFromDealer = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }
        public String TransporterName
        {
            get { return transporterName; }
            set { transporterName = value; }
        }
        public String Destination
        {
            get { return destination; }
            set { destination = value; }
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
        public String ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public String RefNo
        {
            get { return refNo; }
            set { refNo = value; }
        }

        public int LoadingId { get => loadingId; set => loadingId = value; }
        #endregion
    }
}
