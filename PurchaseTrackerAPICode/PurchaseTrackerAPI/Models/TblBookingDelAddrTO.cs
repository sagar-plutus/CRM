using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblBookingDelAddrTO
    {
        #region Declarations
        Int32 idBookingDelAddr;
        Int32 bookingId;
        Int32 pincode;
        String address;
        String villageName;
        String talukaName;
        String districtName;
        String state;
        String country;
        String comment;
        String billingName;
        String gstNo;
        String contactNo;
        Int32 stateId;
        String panNo;
        String aadharNo;
        Int32 txnAddrTypeId;
        Int32 addrSourceTypeId;
        Int32 scheduleId;

        #endregion

        #region Constructor
        public TblBookingDelAddrTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBookingDelAddr
        {
            get { return idBookingDelAddr; }
            set { idBookingDelAddr = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 Pincode
        {
            get { return pincode; }
            set { pincode = value; }
        }
        public String Address
        {
            get { return address; }
            set { address = value; }
        }
        public String VillageName
        {
            get { return villageName; }
            set { villageName = value; }
        }
        public String TalukaName
        {
            get { return talukaName; }
            set { talukaName = value; }
        }
        public String DistrictName
        {
            get { return districtName; }
            set { districtName = value; }
        }
        public String Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        public String State
        {
            get { return state; }
            set { state = value; }
        }
        public String Country
        {
            get { return country; }
            set { country = value; }
        }

        public string BillingName { get => billingName; set => billingName = value; }
        public string GstNo { get => gstNo; set => gstNo = value; }
        public string ContactNo { get => contactNo; set => contactNo = value; }
        public int StateId { get => stateId; set => stateId = value; }
        public string PanNo { get => panNo; set => panNo = value; }
        public string AadharNo { get => aadharNo; set => aadharNo = value; }

        public int TxnAddrTypeId { get => txnAddrTypeId; set => txnAddrTypeId = value; }
        public int AddrSourceTypeId { get => addrSourceTypeId; set => addrSourceTypeId = value; }
        //Vijaymala added []13-12-2017]
        public Int32 ScheduleId { get => scheduleId; set => scheduleId = value; }

        #endregion
    }
}
