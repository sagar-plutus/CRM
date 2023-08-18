using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingSlipAddressTO
    {
        #region Declarations
        Int32 idLoadSlipAddr;
        Int32 bookDelAddrId;
        Int32 loadingSlipId;
        Int32 loadingLayerId;
        String address;
        String villageName;
        String talukaName;
        String districtName;
        String state;
        String country;
        String pincode;
        String comment;
        String billingName;
        String gstNo;
        String contactNo;
        Int32 txnAddrTypeId;
        String txnAddrType;
        String layerDesc;
        Int32 stateId;
        String panNo;
        String aadharNo;
        Int32 addrSourceTypeId;
        String addrSourceTypeDesc;

        #endregion

        #region Constructor
        public TblLoadingSlipAddressTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadSlipAddr
        {
            get { return idLoadSlipAddr; }
            set { idLoadSlipAddr = value; }
        }
        public Int32 BookDelAddrId
        {
            get { return bookDelAddrId; }
            set { bookDelAddrId = value; }
        }
        public Int32 LoadingSlipId
        {
            get { return loadingSlipId; }
            set { loadingSlipId = value; }
        }
        public Int32 LoadingLayerId
        {
            get { return loadingLayerId; }
            set { loadingLayerId = value; }
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
        public String Pincode
        {
            get { return pincode; }
            set { pincode = value; }
        }
        public String Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public string BillingName { get => billingName; set => billingName = value; }
        public string GstNo { get => gstNo; set => gstNo = value; }
        public string ContactNo { get => contactNo; set => contactNo = value; }
        public int TxnAddrTypeId { get => txnAddrTypeId; set => txnAddrTypeId = value; }
        public string TxnAddrType { get => txnAddrType; set => txnAddrType = value; }
        public string LayerDesc { get => layerDesc; set => layerDesc = value; }
        public int StateId { get => stateId; set => stateId = value; }
        public string PanNo { get => panNo; set => panNo = value; }
        public string AadharNo { get => aadharNo; set => aadharNo = value; }
        public int AddrSourceTypeId { get => addrSourceTypeId; set => addrSourceTypeId = value; }
        public string AddrSourceTypeDesc { get => addrSourceTypeDesc; set => addrSourceTypeDesc = value; }
        #endregion
    }
}
