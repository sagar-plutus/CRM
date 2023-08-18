using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblInvoiceTO
    {
        #region Declarations
        Int32 idInvoice;
        Int32 invoiceTypeId;
        Int32 transportOrgId;
        Int32 transportModeId;
        Int32 currencyId;
        Int32 loadingSlipId;
        Int32 distributorOrgId;
        Int32 dealerOrgId;
        Int32 finYearId;
        Int32 statusId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime invoiceDate;
        DateTime lrDate;
        DateTime statusDate;
        DateTime createdOn;
        DateTime updatedOn;
        Double currencyRate;
        Double basicAmt;
        Double discountAmt;
        Double taxableAmt;
        Double cgstAmt;
        Double sgstAmt;
        Double igstAmt;
        Double freightPct;
        Double freightAmt;
        Double roundOffAmt;
        Double grandTotal;
        String invoiceNo;
        String electronicRefNo;
        String vehicleNo;
        String lrNumber;
        String roadPermitNo;
        String transportorForm;
        String airwayBillNo;
        String narration;
        String bankDetails;
        List<TblInvoiceAddressTO> invoiceAddressTOList;
        List<TblInvoiceItemDetailsTO> invoiceItemDetailsTOList;
        Int32 invoiceModeId;

        String dealerName;
        String distributorName;
        String transporterName;
        String currencyName;
        String statusName;
        String invoiceTypeDesc;
        String deliveryLocation;

        String changeIn;
        Double expenseAmt;
        Double otherAmt;
        Double tareWeight;
        Double netWeight;
        Double grossWeight;
        Int32 isConfirmed;
        Int32 rcmFlag;
        String remark;
        String historyDetail;
        Int32 invFromOrgId;
        #endregion

        #region Constructor
        public TblInvoiceTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdInvoice
        {
            get { return idInvoice; }
            set { idInvoice = value; }
        }
        public Int32 InvoiceTypeId
        {
            get { return invoiceTypeId; }
            set { invoiceTypeId = value; }
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
        public Int32 LoadingSlipId
        {
            get { return loadingSlipId; }
            set { loadingSlipId = value; }
        }
        public Int32 DistributorOrgId
        {
            get { return distributorOrgId; }
            set { distributorOrgId = value; }
        }
        public Int32 DealerOrgId
        {
            get { return dealerOrgId; }
            set { dealerOrgId = value; }
        }
        public Int32 FinYearId
        {
            get { return finYearId; }
            set { finYearId = value; }
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
        public Double BasicAmt
        {
            get { return basicAmt; }
            set { basicAmt = value; }
        }
        public Double DiscountAmt
        {
            get { return discountAmt; }
            set { discountAmt = value; }
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
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        public String BankDetails
        {
            get { return bankDetails; }
            set { bankDetails = value; }
        }
        public List<TblInvoiceAddressTO> InvoiceAddressTOList
        {
            get { return invoiceAddressTOList; }
            set { invoiceAddressTOList = value; }
        }
        public List<TblInvoiceItemDetailsTO> InvoiceItemDetailsTOList
        {
            get { return invoiceItemDetailsTOList; }
            set { invoiceItemDetailsTOList = value; }
        }
        public Int32 InvoiceModeId
        {
            get { return invoiceModeId; }
            set { invoiceModeId = value; }
        }

        public Constants.InvoiceTypeE InvoiceTypeE
        {
            get
            {
                Constants.InvoiceTypeE invoiceTypeE = Constants.InvoiceTypeE.REGULAR_TAX_INVOICE;
                if (Enum.IsDefined(typeof(Constants.InvoiceTypeE), invoiceTypeId))
                {
                    invoiceTypeE = (Constants.InvoiceTypeE)invoiceTypeId;
                }
                return invoiceTypeE;

            }
            set
            {
                invoiceTypeId = (int)value;
            }
        }

        public Constants.InvoiceModeE InvoiceModeE
        {
            get
            {
                Constants.InvoiceModeE invoiceModeE = Constants.InvoiceModeE.AUTO_INVOICE;
                if (Enum.IsDefined(typeof(Constants.InvoiceModeE), invoiceModeId))
                {
                    invoiceModeE = (Constants.InvoiceModeE)invoiceModeId;
                }
                return invoiceModeE;

            }
            set
            {
                invoiceModeId = (int)value;
            }
        }

        public Constants.InvoiceStatusE InvoiceStatusE
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
                statusId = (int)value;
            }
        }

        public string TransporterName { get => transporterName; set => transporterName = value; }
        public string CurrencyName { get => currencyName; set => currencyName = value; }
        public string StatusName { get => statusName; set => statusName = value; }
        public string InvoiceTypeDesc { get => invoiceTypeDesc; set => invoiceTypeDesc = value; }
        public string DistributorName { get => distributorName; set => distributorName = value; }
        public string DealerName { get => dealerName; set => dealerName = value; }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        public String StatusDateStr
        {
            get { return statusDate.ToString("dd-MM-yyyy"); }
        }
        public String InvoiceDateStr
        {
            get { return invoiceDate.ToString("dd-MM-yyyy"); }
        }
        public String DeliveryLocation 
        {
            get { return deliveryLocation; }
            set { deliveryLocation = value; }
        }
       

        public string ChangeIn { get => changeIn; set => changeIn = value; }
        public double ExpenseAmt { get => expenseAmt; set => expenseAmt = value; }
        public double OtherAmt { get => otherAmt; set => otherAmt = value; }
        public Double TareWeight
        {
            get { return tareWeight; }
            set { tareWeight = value; }
        }

        public Double NetWeight
        {
            get { return netWeight; }
            set { netWeight = value; }
        }

        public Double GrossWeight
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }

        public int IsConfirmed { get => isConfirmed; set => isConfirmed = value; }

        public String InvoiceNoWrtDate
        {
            get {

                return createdOn.ToString("ddMMyyyyHHmmss") + idInvoice;
            }
        }
        public Int32 RcmFlag
        {
            get { return rcmFlag; }
            set { rcmFlag = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public String HistoryDetails
        {
            get { return historyDetail; }
            set { historyDetail = value; }
        }

        public int InvFromOrgId { get => invFromOrgId; set => invFromOrgId = value; }

        #endregion

        public TblInvoiceTO DeepCopy()
        {
            TblInvoiceTO other = (TblInvoiceTO)this.MemberwiseClone();
            return other;
        }
    }
}
