using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblInvoiceAddressTO
    {
        #region Declarations
        Int32 idInvoiceAddr;
        Int32 invoiceId;
        Int32 txnAddrTypeId;
        Int32 billingOrgId;
        Int32 talukaId;
        Int32 districtId;
        Int32 stateId;
        Int32 countryId;
        String billingName;
        String gstinNo;
        String panNo;
        String aadharNo;
        String contactNo;
        String address;
        String taluka;
        String district;
        String state;
        String pinCode;
        Int32 addrSourceTypeId;

        #endregion

        #region Constructor
        public TblInvoiceAddressTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdInvoiceAddr
        {
            get { return idInvoiceAddr; }
            set { idInvoiceAddr = value; }
        }
        public Int32 InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
        }
        public Int32 TxnAddrTypeId
        {
            get { return txnAddrTypeId; }
            set { txnAddrTypeId = value; }
        }
        public Int32 BillingOrgId
        {
            get { return billingOrgId; }
            set { billingOrgId = value; }
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
        public String BillingName
        {
            get { return billingName; }
            set { billingName = value; }
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
        public Int32 AddrSourceTypeId
        {
            get { return addrSourceTypeId; }
            set { addrSourceTypeId = value; }
        }


        #endregion

        public TblInvoiceAddressTO DeepCopy()
        {
            TblInvoiceAddressTO other = (TblInvoiceAddressTO)this.MemberwiseClone();
            return other;
        }
    }
}
