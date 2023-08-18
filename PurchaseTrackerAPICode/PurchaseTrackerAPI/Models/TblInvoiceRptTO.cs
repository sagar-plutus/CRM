using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblInvoiceRptTO
    {
        #region Declarations
        Int32 idInvoice;
        String invoiceNo;
        String vehicleNo;
        DateTime invoiceDate;
        String partyName;
        String cnfName;
        Double bookingRate;
        Int32 invoiceItemId;
        String prodItemDesc;
        Double bundles;
        Double rate;
        Double cdStructure;
        Double cdAmt;
        Double taxRatePct;
        Int32 taxTypeId;
        Double freightAmt;
        Double invoiceQty;
        Double taxableAmt;
        Double taxAmt;
        DateTime createdOn;

        Int32 billingTypeId;
        String buyer;
        String buyerGstNo;
        Int32 consigneeTypeId;
        String consignee;
        String consigneeGstNo;
        String deliveryLocation;
        Double basicAmt;
        Double cgstAmt;
        Double sgstAmt;
        Double igstAmt;
        double grandTotal;
        Int32 gstinCodeNo;
        Int32 stateId;
        Int32 otherTaxId;
        Int32 isConfirmed;
        Int32 statusId;
        Int32 stateOrUTCode;
        DateTime statusDate;
        String narration;


        String buyerState;
        String consigneeAddress;
        String consigneeDistict;
        String consigneePinCode;
        String consigneeState;
        String materialName;
        String transporterName;
        String contactNo;

        Double cgstPct;
        Double sgstPct;
        Double igstPct;
        String cnfMobNo;
        String dealerMobNo;
        DateTime lrDate;
        String lrNumber;

        Int32 loadingId;
        #endregion

        #region Constructor


        #endregion

        #region GetSet
        public Int32 IdInvoice
        {
            get { return idInvoice; }
            set { idInvoice = value; }
        }
        public String InvoiceNo
        {
            get { return invoiceNo; }
            set { invoiceNo = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { invoiceDate = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }
        public String CnfName
        {
            get { return cnfName; }
            set { cnfName = value; }
        }
        public Double BookingRate
        {
            get { return bookingRate; }
            set { bookingRate = value; }
        }
        public Int32 InvoiceItemId
        {
            get { return invoiceItemId; }
            set { invoiceItemId = value; }
        }
        public String ProdItemDesc
        {
            get { return prodItemDesc; }
            set { prodItemDesc = value; }
        }
        public Double Bundles
        {
            get { return bundles; }
            set { bundles = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }
        public Double CdAmt
        {
            get { return cdAmt; }
            set { cdAmt = value; }
        }
        public Double TaxRatePct
        {
            get { return taxRatePct; }
            set { taxRatePct = value; }
        }
        public Int32 TaxTypeId
        {
            get { return taxTypeId; }
            set { taxTypeId = value; }
        }
        public Double FreightAmt
        {
            get { return freightAmt; }
            set { freightAmt = value; }
        }
        public Double InvoiceQty
        {
            get { return invoiceQty; }
            set { invoiceQty = value; }
        }
        public Double TaxableAmt
        {
            get { return taxableAmt; }
            set { taxableAmt = value; }
        }
        public Double TaxAmt
        {
            get { return taxAmt; }
            set { taxAmt = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String InvoiceDateStr
        {
            get { return invoiceDate.ToString("dd-MM-yyyy"); }
        }
        public String statusDateStr
        {
            get { return statusDate.ToString("dd-MM-yyyy"); }
        }

        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }


        public String InvoiceNoWrtDate
        {
            get
            {

                return createdOn.ToString("ddMMyyyyHHmmss") + "N" + idInvoice + "  ";
            }
        }
        public Int32 BillingTypeId
        {
            get { return billingTypeId; }
            set { billingTypeId = value; }
        }
        public String Buyer
        {
            get { return buyer; }
            set { buyer = value; }
        }
        public String BuyerGstNo
        {
            get { return buyerGstNo; }
            set { buyerGstNo = value; }
        }
        public Int32 ConsigneeTypeId
        {
            get { return consigneeTypeId; }
            set { consigneeTypeId = value; }
        }
        public String Consignee
        {
            get { return consignee; }
            set { consignee = value; }
        }
        public  String ConsigneeGstNo
         {
            get { return consigneeGstNo; }
            set { consigneeGstNo = value; }
        }
        public String DeliveryLocation
        {
            get { return deliveryLocation; }
            set { deliveryLocation = value; }
        }
        public Double BasicAmt
        {
            get { return basicAmt; }
            set { basicAmt = value; }
        }
        public Double CgstTaxAmt
         {
            get { return cgstAmt; }
            set { cgstAmt = value; }
        }
        public Double SgstTaxAmt
        {
            get { return sgstAmt; }
            set { sgstAmt = value; }
        }
        public Double IgstTaxAmt
        {
            get { return igstAmt; }
            set { igstAmt = value; }
        }
        public Double GrandTotal
        {
            get { return grandTotal; }
            set { grandTotal = value; }
        }
        public Int32 GstinCodeNo
        {
            get { return gstinCodeNo; }
            set { gstinCodeNo = value; }
        }
        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }

        public Int32 IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
        }
        public Int32 OtherTaxId
        {
            get { return otherTaxId; }
            set { otherTaxId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 StateOrUTCode
        {
            get { return stateOrUTCode; }
            set { stateOrUTCode = value; }
        }

        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }

        public String BuyerState
        {
            get { return buyerState; }
            set { buyerState = value; }
        }

        public String ConsigneeAddress
        {
            get { return consigneeAddress; }
            set { consigneeAddress = value; }
        }
        public String ConsigneeDistict
        {
            get { return consigneeDistict; }
            set { consigneeDistict = value; }
        }

        public String ConsigneePinCode
        {
            get { return consigneePinCode; }
            set { consigneePinCode = value; }
        }

        public String ConsigneeState
        {
            get { return consigneeState; }
            set { consigneeState = value; }
        }

        public String MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }


        public String TransporterName
        {
            get { return transporterName; }
            set { transporterName = value; }
        }

        public String ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; }
        }

        public double CgstPct
        {
            get { return cgstPct; }
            set { cgstPct = value; }
        }

        public double SgstPct
        {
            get { return sgstPct; }
            set { sgstPct = value; }
        }

        public double IgstPct
        {
            get { return igstPct; }
            set { igstPct = value; }
        }
        public String StatusDateStrNew
        {
            get { return statusDate.ToString("dd-MMM-yy"); }
        }

        public String CnfMobNo
        {
            get { return cnfMobNo; }
            set { cnfMobNo = value; }
        }

        public String DealerMobNo
        {
            get { return dealerMobNo; }
            set { dealerMobNo = value; }
        }
        public DateTime LrDate
        {
            get { return lrDate; }
            set { lrDate = value; }
        }
        public String LrNumber
        {
            get { return lrNumber; }
            set { lrNumber = value; }
        }
        public String LrDateStr
        {

            get
            {
                if (lrDate != new DateTime())
                {
                    return lrDate.ToString("dd-MMM-yy");
                }
                else
                {
                    return null;
                }
            }
        }
        public Int32 LoadingId
        {
            get { return loadingId; }
            set { loadingId = value; }
        }
        #endregion
    }
}
