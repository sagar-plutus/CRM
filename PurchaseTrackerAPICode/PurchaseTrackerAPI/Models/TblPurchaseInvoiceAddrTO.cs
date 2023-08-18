using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseInvoiceAddrTO
    {
        #region Declarations
        Int32 txnAddrTypeId;
        Int32 billingPartyOrgId;
        Int32 talukaId;
        Int32 districtId;
        Int32 stateId;
        Int32 countryId;
        Int32 addrSourceTypeId;
        Int64 idPurchaseInvoiceAddr;
        Int64 purchaseInvoiceId;
        String billingPartyName;
        String gstinNo;
        String panNo;
        String aadharNo;
        String contactNo;
        String address;
        String taluka;
        String district;
        String state;
        String pinCode;
        String country;
        #endregion

        #region Constructor
        public TblPurchaseInvoiceAddrTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 TxnAddrTypeId
        {
            get { return txnAddrTypeId; }
            set { txnAddrTypeId = value; }
        }
        public Int32 BillingPartyOrgId
        {
            get { return billingPartyOrgId; }
            set { billingPartyOrgId = value; }
        }
        public Int32 TalukaId
        {
            get { return talukaId; }
            set { talukaId = value; }
        }
        public Int32 DistrictId
        {
            get { return districtId; }
            set { districtId = value; }
        }
        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public Int32 CountryId
        {
            get { return countryId; }
            set { countryId = value; }
        }
        public Int32 AddrSourceTypeId
        {
            get { return addrSourceTypeId; }
            set { addrSourceTypeId = value; }
        }
        public Int64 IdPurchaseInvoiceAddr
        {
            get { return idPurchaseInvoiceAddr; }
            set { idPurchaseInvoiceAddr = value; }
        }
        public Int64 PurchaseInvoiceId
        {
            get { return purchaseInvoiceId; }
            set { purchaseInvoiceId = value; }
        }
        public String BillingPartyName
        {
            get { return billingPartyName; }
            set { billingPartyName = value; }
        }
        public String GstinNo
        {
            get { return gstinNo; }
            set { gstinNo = value; }
        }
        public String PanNo
        {
            get { return panNo; }
            set { panNo = value; }
        }
        public String AadharNo
        {
            get { return aadharNo; }
            set { aadharNo = value; }
        }
        public String ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; }
        }
        public String Address
        {
            get { return address; }
            set { address = value; }
        }
        public String Taluka
        {
            get { return taluka; }
            set { taluka = value; }
        }
        public String District
        {
            get { return district; }
            set { district = value; }
        }
        public String State
        {
            get { return state; }
            set { state = value; }
        }
        public String PinCode
        {
            get { return pinCode; }
            set { pinCode = value; }
        }
        public String Country
        {
            get { return country; }
            set { country = value; }
        }
        #endregion
    }
}
