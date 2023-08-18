using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseInvoiceTO
    {
        #region Declarations
        Int32 purchaseInvTypeId;
        Int32 finYearId;
        Int32 supplierId;
        Int32 brokerId;
        Int32 invoiceToOrgId;
        Int32 transportOrgId;
        Int32 transportModeId;
        Int32 currencyId;
        Int32 statusId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime invoiceDate;
        DateTime lrDate;
        DateTime statusDate;
        DateTime createdOn;
        DateTime updatedOn;
        Double currencyRate;
        Double billQty;
        Double discountPct;
        Double discountAmt;
        Double basicAmt;
        Double taxableAmt;
        Double cgstAmt;
        Double sgstAmt;
        Double igstAmt;
        Double freightPct;
        Double freightAmt;
        Double otherExpAmt;
        Double tcsAmt;
        Double transportorAdvAmt;
        Double roundOffAmt;
        Double grandTotal;
        Int64 idInvoicePurchase;
        String invoiceNo;
        String electronicRefNo;
        String vehicleNo;
        String lrNumber;
        String roadPermitNo;
        String supplierName;
        String brokerName;
        String transportorName;
        String transportorForm;
        String airwayBillNo;
        String location;
        String narration;
        String remark;
        Int32 purSchSummaryId;

        DateTime ewayBillDate;
        List<TblPurchaseInvoiceAddrTO> tblPurchaseInvoiceAddrTOList = new List<TblPurchaseInvoiceAddrTO>();
        List<TblPurchaseInvoiceItemDetailsTO> tblPurchaseInvoiceItemDetailsTOList = new List<TblPurchaseInvoiceItemDetailsTO>();
        TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceIntefacingDtls;

        List<TblPurchaseInvoiceDocumentsTO> tblPurchaseInvoiceDocumentsTOList = new List<TblPurchaseInvoiceDocumentsTO>();
        List<TblPurchaseDocToVerifyTO> tblPurchaseDocToVerifyTO = new List<TblPurchaseDocToVerifyTO>();

        DateTime ewayBillExpiryDate;
        string poId;
        string grrId;
        string createdByName;
        #endregion

        #region Constructor
        public TblPurchaseInvoiceTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 PurchaseInvTypeId
        {
            get { return purchaseInvTypeId; }
            set { purchaseInvTypeId = value; }
        }
        public Int32 FinYearId
        {
            get { return finYearId; }
            set { finYearId = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        public Int32 BrokerId
        {
            get { return brokerId; }
            set { brokerId = value; }
        }
        public Int32 InvoiceToOrgId
        {
            get { return invoiceToOrgId; }
            set { invoiceToOrgId = value; }
        }
        public Int32 TransportOrgId
        {
            get { return transportOrgId; }
            set { transportOrgId = value; }
        }
        public Int32 TransportModeId
        {
            get { return transportModeId; }
            set { transportModeId = value; }
        }
        public Int32 CurrencyId
        {
            get { return currencyId; }
            set { currencyId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
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
        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { invoiceDate = value; }
        }
        public DateTime LrDate
        {
            get { return lrDate; }
            set { lrDate = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
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
        public Double CurrencyRate
        {
            get { return currencyRate; }
            set { currencyRate = value; }
        }
        public Double BillQty
        {
            get { return billQty; }
            set { billQty = value; }
        }
        public Double DiscountPct
        {
            get { return discountPct; }
            set { discountPct = value; }
        }
        public Double DiscountAmt
        {
            get { return discountAmt; }
            set { discountAmt = value; }
        }
        public Double BasicAmt
        {
            get { return basicAmt; }
            set { basicAmt = value; }
        }
        public Double TaxableAmt
        {
            get { return taxableAmt; }
            set { taxableAmt = value; }
        }
        public Double CgstAmt
        {
            get { return cgstAmt; }
            set { cgstAmt = value; }
        }
        public Double SgstAmt
        {
            get { return sgstAmt; }
            set { sgstAmt = value; }
        }
        public Double IgstAmt
        {
            get { return igstAmt; }
            set { igstAmt = value; }
        }
        public Double FreightPct
        {
            get { return freightPct; }
            set { freightPct = value; }
        }
        public Double FreightAmt
        {
            get { return freightAmt; }
            set { freightAmt = value; }
        }
        public Double OtherExpAmt
        {
            get { return otherExpAmt; }
            set { otherExpAmt = value; }
        }
        public Double TcsAmt
        {
            get { return tcsAmt; }
            set { tcsAmt = value; }
        }
        public Double TransportorAdvAmt
        {
            get { return transportorAdvAmt; }
            set { transportorAdvAmt = value; }
        }
        public Double RoundOffAmt
        {
            get { return roundOffAmt; }
            set { roundOffAmt = value; }
        }
        public Double GrandTotal
        {
            get { return grandTotal; }
            set { grandTotal = value; }
        }
        public Int64 IdInvoicePurchase
        {
            get { return idInvoicePurchase; }
            set { idInvoicePurchase = value; }
        }
        public String InvoiceNo
        {
            get { return invoiceNo; }
            set { invoiceNo = value; }
        }
        public String ElectronicRefNo
        {
            get { return electronicRefNo; }
            set { electronicRefNo = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String LrNumber
        {
            get { return lrNumber; }
            set { lrNumber = value; }
        }
        public String RoadPermitNo
        {
            get { return roadPermitNo; }
            set { roadPermitNo = value; }
        }
        public String SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        public String BrokerName
        {
            get { return brokerName; }
            set { brokerName = value; }
        }
        public String TransportorName
        {
            get { return transportorName; }
            set { transportorName = value; }
        }
        public String TransportorForm
        {
            get { return transportorForm; }
            set { transportorForm = value; }
        }
        public String AirwayBillNo
        {
            get { return airwayBillNo; }
            set { airwayBillNo = value; }
        }
        public String Location
        {
            get { return location; }
            set { location = value; }
        }
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public string PoId
        {
            get { return poId; }
            set { poId = value; }
        }

        public string GrrId
        {
            get { return grrId; }
            set { grrId = value; }
        }
        
        public string CreatedByName
        {
            get { return createdByName; }
            set { createdByName = value; }
        }
        public Constants.InvoiceStatusE PurchaseInvoiceStatusE
        {
            get
            {
                Constants.InvoiceStatusE invoiceStatusE = Constants.InvoiceStatusE.NEW;
                if (Enum.IsDefined(typeof(Constants.InvoiceStatusE), statusId))
                {
                    invoiceStatusE = (Constants.InvoiceStatusE)statusId;
                }
                return invoiceStatusE;

            }
            set
            {
                //Saket [2019-02-05] Problem while accepting data from GUI For Save & submit
                //statusId = (int)value;
            }
        }
        public Constants.InvoiceTypeE PurhcaseInvoiceTypeE
        {
            get
            {
                Constants.InvoiceTypeE invoiceTypeE = Constants.InvoiceTypeE.REGULAR_TAX_INVOICE;
                if (Enum.IsDefined(typeof(Constants.InvoiceTypeE), purchaseInvTypeId))
                {
                    invoiceTypeE = (Constants.InvoiceTypeE)purchaseInvTypeId;
                }
                return invoiceTypeE;

            }
            set
            {
                purchaseInvTypeId = (int)value;
            }
        }

        public List<TblPurchaseInvoiceAddrTO> TblPurchaseInvoiceAddrTOList { get => tblPurchaseInvoiceAddrTOList; set => tblPurchaseInvoiceAddrTOList = value; }
        public List<TblPurchaseInvoiceItemDetailsTO> TblPurchaseInvoiceItemDetailsTOList { get => tblPurchaseInvoiceItemDetailsTOList; set => tblPurchaseInvoiceItemDetailsTOList = value; }
        public List<TblPurchaseInvoiceDocumentsTO> TblPurchaseInvoiceDocumentsTOList { get => tblPurchaseInvoiceDocumentsTOList; set => tblPurchaseInvoiceDocumentsTOList = value; }

        public List<TblPurchaseDocToVerifyTO> TblPurchaseDocToVerifyTOList { get => tblPurchaseDocToVerifyTO; set => tblPurchaseDocToVerifyTO = value; }
        public int PurSchSummaryId { get => purSchSummaryId; set => purSchSummaryId = value; }
        public TblPurchaseInvoiceInterfacingDtlTO TblPurchaseInvoiceIntefacingDtls { get => tblPurchaseInvoiceIntefacingDtls; set => tblPurchaseInvoiceIntefacingDtls = value; }
        public DateTime EwayBillDate { get => ewayBillDate; set => ewayBillDate = value; }
        public DateTime EwayBillExpiryDate { get => ewayBillExpiryDate; set => ewayBillExpiryDate = value; }
        // public List<TblPurchaseInvoiceDocumentsTO> TblPurchaseInvoiceDocumentsTOList { get => tblPurchaseInvoiceDocumentsTOList; set => tblPurchaseInvoiceDocumentsTOList = value; }
        #endregion
    }
}
