using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{

    public class PartWiseReportTO
    {
        String supplier;
        String grade;
        double totalQty;
        double totalAmount;
        double averageRate;
        Int64 supplierId;
        int isTotalRow;
        String displaySupplier;

        public String DisplaySupplier { get => displaySupplier; set => displaySupplier = value; }
        public string Supplier { get => supplier; set => supplier = value; }
        public string Grade { get => grade; set => grade = value; }
        public double TotalQty { get => totalQty; set => totalQty = value; }
        public double TotalAmount { get => totalAmount; set => totalAmount = value; }
        public double AverageRate { get => averageRate; set => averageRate = value; }

        public Int64 SupplierId { get => supplierId; set => supplierId = value; }

        public int IsTotalRow { get => isTotalRow; set => isTotalRow = value; }

    }

    public class VehicleWiseReportTO
    {
        int idPurchaseScheduleSummary;
        String vehicleNumber;
        String date;
        String supplier;
        String remark;
        String grade;
        double processChargePerVeh;
        double totalQty;
        double rate;
        double totalAmount;
        int isTotalRow;
        int cOrNCId;
        int isBoth;
        String displayVehicleNo;
        String displayDate;
        String displaySupplier;
        String displayRemark;


        public String DisplayVehicleNo { get => displayVehicleNo; set => displayVehicleNo = value; }
        public String DisplayDate { get => displayDate; set => displayDate = value; }
        public String DisplaySupplier { get => displaySupplier; set => displaySupplier = value; }
        public String DisplayRemark { get => displayRemark; set => displayRemark = value; }

        public int COrNCId { get => cOrNCId; set => cOrNCId = value; }
        public int IsBoth { get => isBoth; set => isBoth = value; }

        public int IdPurchaseScheduleSummary { get => idPurchaseScheduleSummary; set => idPurchaseScheduleSummary = value; }

        [JsonProperty("Vehicle Number")]
        public string VehicleNumber { get => vehicleNumber; set => vehicleNumber = value; }

        [JsonProperty("Date")]
        public string Date { get => date; set => date = value; }

        [JsonProperty("Supplier")]
        public string Supplier { get => supplier; set => supplier = value; }

        [JsonProperty("Remark")]
        public string Remark { get => remark; set => remark = value; }

        [JsonProperty("Grade")]
        public string Grade { get => grade; set => grade = value; }

        [JsonProperty("Process Charge Per Vehicle")]
        public double ProcessChargePerVeh { get => processChargePerVeh; set => processChargePerVeh = value; }

        [JsonProperty("Total Qty")]
        public double TotalQty { get => totalQty; set => totalQty = value; }

        [JsonProperty("Rate")]
        public double Rate { get => rate; set => rate = value; }

        [JsonProperty("Total Amount")]
        public double TotalAmount { get => totalAmount; set => totalAmount = value; }

        public int IsTotalRow { get => isTotalRow; set => isTotalRow = value; }
    }

    public class TallyReportTO
    {
        int idPurchaseScheduleSummary;
        String date;
        String truckNo;
        String supplierName;
        String pm;
        String location;
        String grade;
        double gradeQty;
        double gradeRate;
        double total;
        double totalGradeQty;
        String billType;
        String materialType;
        String containerNo;
        double dustQty;
        String displayGradeQty;
        String displayGradeRate;
        String displayTotal;
        String godown;
        double processChargePerVeh;
        int cOrNCId;
        int isBoth;
        Int32 rootScheduleId;
        String displayTotalGradeQty;
        String narration;
        Double grandTotal;
        String displayGrandTotal;
        String voucherNo;
        String purchaseLedger;
        String displayProcessChargePerVeh;
        Int32 displayRecordInFirstRow;

        public String DisplayProcessChargePerVeh { get => displayProcessChargePerVeh; set => displayProcessChargePerVeh = value; }
        public String VoucherNo { get => voucherNo; set => voucherNo = value; }
        public String PurchaseLedger { get => purchaseLedger; set => purchaseLedger = value; }
        public String DisplayGrandTotal { get => displayGrandTotal; set => displayGrandTotal = value; }
        public Double GrandTotal { get => grandTotal; set => grandTotal = value; }
        public String Narration { get => narration; set => narration = value; }
        public double TotalGradeQty { get => totalGradeQty; set => totalGradeQty = value; }
        public String DisplayTotalGradeQty { get => displayTotalGradeQty; set => displayTotalGradeQty = value; }
        public int COrNCId { get => cOrNCId; set => cOrNCId = value; }
        public int IsBoth { get => isBoth; set => isBoth = value; }
        public double ProcessChargePerVeh { get => processChargePerVeh; set => processChargePerVeh = value; }
        public string Godown { get => godown; set => godown = value; }
        public string Date { get => date; set => date = value; }
        [JsonProperty("Truck No")]
        public string TruckNo { get => truckNo; set => truckNo = value; }

        [JsonProperty("Supplier Name")]
        public string SupplierName { get => supplierName; set => supplierName = value; }
        public string PM { get => pm; set => pm = value; }
        public string Location { get => location; set => location = value; }
        public string Grade { get => grade; set => grade = value; }
        [JsonProperty("Grade Qty")]
        public double GradeQty { get => gradeQty; set => gradeQty = value; }
        [JsonProperty("Display Grade Qty")]
        public string DisplayGradeQty { get => displayGradeQty; set => displayGradeQty = value; }
        [JsonProperty("Grade Rate")]
        public double GradeRate { get => gradeRate; set => gradeRate = value; }
        public double Total { get => total; set => total = value; }
        [JsonProperty("Bill Type")]
        public string BillType { get => billType; set => billType = value; }
        [JsonProperty("Material Type")]
        public string MaterialType { get => materialType; set => materialType = value; }
        [JsonProperty("Container No")]
        public string ContainerNo { get => containerNo; set => containerNo = value; }
        public int IdPurchaseScheduleSummary { get => idPurchaseScheduleSummary; set => idPurchaseScheduleSummary = value; }
        public double DustQty { get => dustQty; set => dustQty = value; }

        public string DisplayGradeRate { get => displayGradeRate; set => displayGradeRate = value; }
        public string DisplayTotal { get => displayTotal; set => displayTotal = value; }
        public Int32 RootScheduleId
        {
            get { return rootScheduleId; }
            set { rootScheduleId = value; }
        }
        public Int32 DisplayRecordInFirstRow
        {
            get { return displayRecordInFirstRow; }
            set { displayRecordInFirstRow = value; }
        }
        public Double GrossWeight { get; set; }
        public Double TareWeight { get; set; }
        public Double NetWeight { get; set; }
    }

    public class TallyDrNoteReportTO
    {
        String date;
        String voucherType;
        String originalInvoiceNo;
        String invoiceDate;
        String supplierName;
        String debitAmountRs;
        String againstRef;
        String purchaseLedger;
        String purchaseLedgerAmountRs;
        String cGSTINPUT;
        String cGSTINPUTAmountRs;
        String sGSTINPUT;
        String sGSTINPUTAmountRs;
        String iGSTINPUT;
        String iGSTINPUTAmountRs;
        String narration;

        public string Date { get => date; set => date = value; }
        public string VoucherType { get => voucherType; set => voucherType = value; }
        public string OriginalInvoiceNo { get => originalInvoiceNo; set => originalInvoiceNo = value; }
        public string InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        public string SupplierName { get => supplierName; set => supplierName = value; }
        public string DebitAmountRs { get => debitAmountRs; set => debitAmountRs = value; }
        public string AgainstRef { get => againstRef; set => againstRef = value; }
        public string PurchaseLedger { get => purchaseLedger; set => purchaseLedger = value; }
        public string PurchaseLedgerAmountRs { get => purchaseLedgerAmountRs; set => purchaseLedgerAmountRs = value; }
        public string CGSTINPUT { get => cGSTINPUT; set => cGSTINPUT = value; }
        public string CGSTINPUTAmountRs { get => cGSTINPUTAmountRs; set => cGSTINPUTAmountRs = value; }
        public string SGSTINPUT { get => sGSTINPUT; set => sGSTINPUT = value; }
        public string SGSTINPUTAmountRs { get => sGSTINPUTAmountRs; set => sGSTINPUTAmountRs = value; }
        public string IGSTINPUT { get => iGSTINPUT; set => iGSTINPUT = value; }
        public string IGSTINPUTAmountRs { get => iGSTINPUTAmountRs; set => iGSTINPUTAmountRs = value; }
        public string Narration { get => narration; set => narration = value; }

    }


    public class PadtaReportTO
    {
        int idPurchaseScheduleSummary;
        string vehicleNo;
        string supplierName;
        string pm;
        string truckType;
        double qty;
        double gradeRate;
        double kg;
        double todaysRate;
        double wrt;
        double wy;
        double padta_MT;
        double sRate;
        string type;
        string billType;
        string grade;
        double productAomunt;
        double productRecovery;
        double baseItemRecovery;
        double dustQty;

        public int IdPurchaseScheduleSummary { get => idPurchaseScheduleSummary; set => idPurchaseScheduleSummary = value; }
        [JsonProperty("Vehical No")]
        public string VehicalNo { get => vehicleNo; set => vehicleNo = value; }
        [JsonProperty("Supplier Name")]
        public string SupplierName { get => supplierName; set => supplierName = value; }
        public string Pm { get => pm; set => pm = value; }
        [JsonProperty("Truck Type")]
        public string TruckType { get => truckType; set => truckType = value; }
        public double Qty { get => qty; set => qty = value; }
        [JsonProperty("Grade Rate")]
        public double GradeRate { get => gradeRate; set => gradeRate = value; }
        public double Kg { get => kg; set => kg = value; }
        [JsonProperty("Todays Rate")]
        public double TodaysRate { get => todaysRate; set => todaysRate = value; }
        public double Wrt { get => wrt; set => wrt = value; }
        public double Wy { get => wy; set => wy = value; }
        [JsonProperty("Padta(MT)")]
        public double Padta_MT { get => padta_MT; set => padta_MT = value; }
        public double SRate { get => sRate; set => sRate = value; }
        public string Type { get => type; set => type = value; }
        [JsonProperty("Bill Type")]
        public string BillType { get => billType; set => billType = value; }
        public string Grade { get => grade; set => grade = value; }
        public double ProductAomunt { get => productAomunt; set => productAomunt = value; }
        public double ProductRecovery { get => productRecovery; set => productRecovery = value; }
        public double BaseItemRecovery { get => baseItemRecovery; set => baseItemRecovery = value; }

        public double DustQty { get => dustQty; set => dustQty = value; }

        
    }

    public class purchaseSummuryReportTo
    {
        string createdOn;
        Int32 idInvoicePurchase;
        // string createdOnStr;
        String invoiceNo;
        string invoiceDate;
        String transportorName;
        String electronicRefNo;
        string ewayBillDate;
        string ewayBillExpiryDate;
        string vehicleNo;
        String supplierName;
        String supplierDist;
        String voucherType;
        string voucherClass;
        String purAcc;
        string purchaseVatTaxClass;
        String cgst;

        string niUAddDuty;
        string niUForSH;
        string niUGoodsCenvat1st;
        string niUGoodsCenvat2nd;
        string niUAddDuty1st;
        string niUAddDuty2nd;
        string niUForEdu;
        string niUForSHEdu;
        String igst;

        string niUFormToIssue;
        String sgst;
        string inputVatTax;
        string vatCSTToPutManually;

        String otherExpAcc;
        String ipTransportAdvAcc;
        String productItemDesc;
        string invoiceQty;
        string rate;
        double basicTotal;
        double unloadedQty;
        double cgstAmt;

        double niUAddDutyRs;
        double niUForSHRs;
        double niUGoodsCenvat1stRs;
        double niUGoodsCenvat2ndRs;
        double niUAddDuty1stRs;
        double niUAddDuty2ndRs;
        double niUForEduRs;
        double niUForSHEduRs;

        double igstAmt;
        double sgstAmt;
        double otherExpAmt;
        double transportorAdvAmt;
        double grandTotal;
        string narration;
        double vatAssembleValueInRs;
        string grade;
        string costCategory;
        string costCenter;
        string stateName;
        string buyerGstin;

        string salerState;
        string salerGstin;
        string purchaseManager;
        double bookingRate;
        double freightAmt;
        Double tCSAmt;
        double cdAmt;
        String godown;//added by minal
        string transporterMobNo;
        string supplierMobNo;
        string lrDate;
        string lrNo;
        string supplierAddress;
        string materialType;
        string brokerMobNo;
        string tallyRefId;
        string orgSupplierName;
        int idPurchaseInvoiceItem;
        String otherExpensesInsuranceInput;
        String tdsInput;
        Double otherExpensesInsuranceamt;
        Double tdsAmt;
        Double amountToSupplier;


        //// public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        //public Int32 IdPurchaseInvoiceItem { get => idPurchaseInvoiceItem; set => idPurchaseInvoiceItem = value; }

        //public string OrgSupplierName { get => orgSupplierName; set => orgSupplierName = value; }

        //public string CreatedOn { get => createdOn; set => createdOn = value; }
        //public string InvoiceNo { get => invoiceNo; set => invoiceNo = value; }
        //public string InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        ////public string CreatedOn { get => createdOn; set => createdOn = value; }
        //public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        //public string TransportorName { get => transportorName; set => transportorName = value; }
        //public string ElectronicRefNo { get => electronicRefNo; set => electronicRefNo = value; }
        //public string EwayBillDate { get => ewayBillDate; set => ewayBillDate = value; }
        //public string EwayBillExpiryDate { get => ewayBillExpiryDate; set => ewayBillExpiryDate = value; }
        //public string SupplierName { get => supplierName; set => supplierName = value; }

        //public string SupplierDist { get => supplierDist; set => supplierDist = value; }
        //public string VoucherType { get => voucherType; set => voucherType = value; }
        //public string VoucherClass { get => voucherClass; set => voucherClass = value; }

        //public string PurAcc { get => purAcc; set => purAcc = value; }
        //public string PurchaseVatTaxClass { get => purchaseVatTaxClass; set => purchaseVatTaxClass = value; }

        //public string Cgst { get => cgst; set => cgst = value; }


        //public String NiUAddDuty { get => niUAddDuty; set => niUAddDuty = value; }
        //public String NiUForSH { get => niUForSH; set => niUForSH = value; }
        //public String NiUGoodsCenvat1st { get => niUGoodsCenvat1st; set => niUGoodsCenvat1st = value; }
        //public String NiUGoodsCenvat2nd { get => niUGoodsCenvat2nd; set => niUGoodsCenvat2nd = value; }
        //public String NiUAddDuty1st { get => niUAddDuty1st; set => niUAddDuty1st = value; }
        //public String NiUAddDuty2nd { get => niUAddDuty2nd; set => niUAddDuty2nd = value; }
        //public String NiUForEdu { get => niUForEdu; set => niUForEdu = value; }
        //public String NiUForSHEdu { get => niUForSHEdu; set => niUForSHEdu = value; }
        //public string Igst { get => igst; set => igst = value; }

        //public string NiUFormToIssue { get => niUFormToIssue; set => niUFormToIssue = value; }

        //public string Sgst { get => sgst; set => sgst = value; }

        //public string InputVatTax { get => inputVatTax; set => inputVatTax = value; }

        //public string VatCSTToPutManually { get => vatCSTToPutManually; set => vatCSTToPutManually = value; }

        //public string OtherExpAcc { get => otherExpAcc; set => otherExpAcc = value; }
        //public string IpTransportAdvAcc { get => ipTransportAdvAcc; set => ipTransportAdvAcc = value; }
        //public String TdsInput { get => tdsInput; set => tdsInput = value; }
        //public string ProductItemDesc { get => productItemDesc; set => productItemDesc = value; }
        //public string InvoiceQty { get => invoiceQty; set => invoiceQty = value; }
        //public string Rate { get => rate; set => rate = value; }
        //public double BasicTotal { get => basicTotal; set => basicTotal = value; }
        //public double CgstAmt { get => cgstAmt; set => cgstAmt = value; }
        //public double NiUAddDutyRs { get => niUAddDutyRs; set => niUAddDutyRs = value; }
        //public double NiUForSHRs { get => niUForSHRs; set => niUForSHRs = value; }
        //public double NiUGoodsCenvat1stRs { get => niUGoodsCenvat1stRs; set => niUGoodsCenvat1stRs = value; }
        //public double NiUGoodsCenvat2ndRs { get => niUGoodsCenvat2ndRs; set => niUGoodsCenvat2ndRs = value; }
        //public double NiUAddDuty1stRs { get => niUAddDuty1stRs; set => niUAddDuty1stRs = value; }
        //public double NiUAddDuty2ndRs { get => niUAddDuty2ndRs; set => niUAddDuty2ndRs = value; }
        //public double NiUForEduRs { get => niUForEduRs; set => niUForEduRs = value; }
        //public double NiUForSHEduRs { get => niUForSHEduRs; set => niUForSHEduRs = value; }


        //public double IgstAmt { get => igstAmt; set => igstAmt = value; }
        //public double SgstAmt { get => sgstAmt; set => sgstAmt = value; }

        //public double OtherExpAmt { get => otherExpAmt; set => otherExpAmt = value; }
        //public double TransportorAdvAmt { get => transportorAdvAmt; set => transportorAdvAmt = value; }
        //public Double TdsAmt { get => tdsAmt; set => tdsAmt = value; }
        //public double GrandTotal { get => grandTotal; set => grandTotal = value; }
        //public double AmountToSupplier { get => amountToSupplier; set => amountToSupplier = value; }


        //public double VatAssembleValueInRs { get => vatAssembleValueInRs; set => vatAssembleValueInRs = value; }

        //public string Narration { get => narration; set => narration = value; }

        //public string Grade { get => grade; set => grade = value; }
        //public string CostCategory { get => costCategory; set => costCategory = value; }

        //public string CostCenter { get => costCenter; set => costCenter = value; }
        //public string StateName { get => stateName; set => stateName = value; }
        //public string BuyerGstin { get => buyerGstin; set => buyerGstin = value; }

        //public Int32 IdInvoicePurchase { get => idInvoicePurchase; set => idInvoicePurchase = value; }


        //public string SalerState { get => salerState; set => salerState = value; }

        //public string SalerGstin { get => salerGstin; set => salerGstin = value; }

        //public string PurchaseManager { get => purchaseManager; set => purchaseManager = value; }

        //public double BookingRate { get => bookingRate; set => bookingRate = value; }

        //public double FreightAmt { get => freightAmt; set => freightAmt = value; }

        //public double TCSAmt { get => tCSAmt; set => tCSAmt = value; }



        //public double CdAmt { get => cdAmt; set => cdAmt = value; }

        //public string TransporterMobNo { get => transporterMobNo; set => transporterMobNo = value; }

        //public string SupplierMobNo { get => supplierMobNo; set => supplierMobNo = value; }

        //public string LrDate { get => lrDate; set => lrDate = value; }
        //public string LrNo { get => lrNo; set => lrNo = value; }

        //public string SupplierAddress { get => supplierAddress; set => supplierAddress = value; }

        //public string MaterialType { get => materialType; set => materialType = value; }

        //public string BrokerMobNo { get => brokerMobNo; set => brokerMobNo = value; }

        //public string TallyRefId { get => tallyRefId; set => tallyRefId = value; }

        //public String OtherExpensesInsuranceInput { get => otherExpensesInsuranceInput; set => otherExpensesInsuranceInput = value; }

        //public Double OtherExpensesInsuranceamt { get => otherExpensesInsuranceamt; set => otherExpensesInsuranceamt = value; }

        //public string Godown { get => godown; set => godown = value; }


        public Int32 IdPurchaseInvoiceItem { get => idPurchaseInvoiceItem; set => idPurchaseInvoiceItem = value; }

        public string OrgSupplierName { get => orgSupplierName; set => orgSupplierName = value; }

        public string CreatedOn { get => createdOn; set => createdOn = value; }
        public string InvoiceNo { get => invoiceNo; set => invoiceNo = value; }
        public string InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        //public string CreatedOn { get => createdOn; set => createdOn = value; }
        public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        public string TransportorName { get => transportorName; set => transportorName = value; }
        public string ElectronicRefNo { get => electronicRefNo; set => electronicRefNo = value; }
        public string EwayBillDate { get => ewayBillDate; set => ewayBillDate = value; }
        public string EwayBillExpiryDate { get => ewayBillExpiryDate; set => ewayBillExpiryDate = value; }
        public string VoucherType { get => voucherType; set => voucherType = value; }

        public string PurAcc { get => purAcc; set => purAcc = value; }
        public string Cgst { get => cgst; set => cgst = value; }
        public string Sgst { get => sgst; set => sgst = value; }
        public string Igst { get => igst; set => igst = value; }
        public string OtherExpAcc { get => otherExpAcc; set => otherExpAcc = value; }

        public string IpTransportAdvAcc { get => ipTransportAdvAcc; set => ipTransportAdvAcc = value; }

        public string SupplierName { get => supplierName; set => supplierName = value; }
        public String TdsInput { get => tdsInput; set => tdsInput = value; }

        public string ProductItemDesc { get => productItemDesc; set => productItemDesc = value; }

        public string InvoiceQty { get => invoiceQty; set => invoiceQty = value; }
        public string Rate { get => rate; set => rate = value; }

        public double BasicTotal { get => basicTotal; set => basicTotal = value; }
        public double CgstAmt { get => cgstAmt; set => cgstAmt = value; }

        public double SgstAmt { get => sgstAmt; set => sgstAmt = value; }
        public double IgstAmt { get => igstAmt; set => igstAmt = value; }
        public double OtherExpAmt { get => otherExpAmt; set => otherExpAmt = value; }
        public double TransportorAdvAmt { get => transportorAdvAmt; set => transportorAdvAmt = value; }
        public Double TdsAmt { get => tdsAmt; set => tdsAmt = value; }
        public double AmountToSupplier { get => amountToSupplier; set => amountToSupplier = value; }
        public double GrandTotal { get => grandTotal; set => grandTotal = value; }

        public string Narration { get => narration; set => narration = value; }
        public string Grade { get => grade; set => grade = value; }
        public string CostCategory { get => costCategory; set => costCategory = value; }
        public string CostCenter { get => costCenter; set => costCenter = value; }

        public String OtherExpensesInsuranceInput { get => otherExpensesInsuranceInput; set => otherExpensesInsuranceInput = value; }

        public Double OtherExpensesInsuranceamt { get => otherExpensesInsuranceamt; set => otherExpensesInsuranceamt = value; }

        public string Godown { get => godown; set => godown = value; }



        public string SupplierDist { get => supplierDist; set => supplierDist = value; }

        public string VoucherClass { get => voucherClass; set => voucherClass = value; }


        public string PurchaseVatTaxClass { get => purchaseVatTaxClass; set => purchaseVatTaxClass = value; }




        public String NiUAddDuty { get => niUAddDuty; set => niUAddDuty = value; }
        public String NiUForSH { get => niUForSH; set => niUForSH = value; }
        public String NiUGoodsCenvat1st { get => niUGoodsCenvat1st; set => niUGoodsCenvat1st = value; }
        public String NiUGoodsCenvat2nd { get => niUGoodsCenvat2nd; set => niUGoodsCenvat2nd = value; }
        public String NiUAddDuty1st { get => niUAddDuty1st; set => niUAddDuty1st = value; }
        public String NiUAddDuty2nd { get => niUAddDuty2nd; set => niUAddDuty2nd = value; }
        public String NiUForEdu { get => niUForEdu; set => niUForEdu = value; }
        public String NiUForSHEdu { get => niUForSHEdu; set => niUForSHEdu = value; }

        public string NiUFormToIssue { get => niUFormToIssue; set => niUFormToIssue = value; }

        public string InputVatTax { get => inputVatTax; set => inputVatTax = value; }

        public string VatCSTToPutManually { get => vatCSTToPutManually; set => vatCSTToPutManually = value; }
        

        public double NiUAddDutyRs { get => niUAddDutyRs; set => niUAddDutyRs = value; }
        public double NiUForSHRs { get => niUForSHRs; set => niUForSHRs = value; }
        public double NiUGoodsCenvat1stRs { get => niUGoodsCenvat1stRs; set => niUGoodsCenvat1stRs = value; }
        public double NiUGoodsCenvat2ndRs { get => niUGoodsCenvat2ndRs; set => niUGoodsCenvat2ndRs = value; }
        public double NiUAddDuty1stRs { get => niUAddDuty1stRs; set => niUAddDuty1stRs = value; }
        public double NiUAddDuty2ndRs { get => niUAddDuty2ndRs; set => niUAddDuty2ndRs = value; }
        public double NiUForEduRs { get => niUForEduRs; set => niUForEduRs = value; }
        public double NiUForSHEduRs { get => niUForSHEduRs; set => niUForSHEduRs = value; }

        
        public double VatAssembleValueInRs { get => vatAssembleValueInRs; set => vatAssembleValueInRs = value; }

       

        public string StateName { get => stateName; set => stateName = value; }
        public string BuyerGstin { get => buyerGstin; set => buyerGstin = value; }

        public Int32 IdInvoicePurchase { get => idInvoicePurchase; set => idInvoicePurchase = value; }


        public string SalerState { get => salerState; set => salerState = value; }

        public string SalerGstin { get => salerGstin; set => salerGstin = value; }

        public string PurchaseManager { get => purchaseManager; set => purchaseManager = value; }

        public double BookingRate { get => bookingRate; set => bookingRate = value; }

        public double FreightAmt { get => freightAmt; set => freightAmt = value; }

        public double TCSAmt { get => tCSAmt; set => tCSAmt = value; }



        public double CdAmt { get => cdAmt; set => cdAmt = value; }

        public string TransporterMobNo { get => transporterMobNo; set => transporterMobNo = value; }

        public string SupplierMobNo { get => supplierMobNo; set => supplierMobNo = value; }

        public string LrDate { get => lrDate; set => lrDate = value; }
        public string LrNo { get => lrNo; set => lrNo = value; }

        public string SupplierAddress { get => supplierAddress; set => supplierAddress = value; }

        public string MaterialType { get => materialType; set => materialType = value; }

        public string BrokerMobNo { get => brokerMobNo; set => brokerMobNo = value; }

        public string TallyRefId { get => tallyRefId; set => tallyRefId = value; }
        public double UnloadedQty { get => unloadedQty; set => unloadedQty = value; }
    }

    public class PurchaseSummaryReportHTO
    {
        Int32 idInvoicePurchase;
        Int32 purSchSummaryId;
        string invoiceDate;
        string invoiceNo;
        Int32 billingTypeId;
        string buyerName;
        string buyerGstNo;
        double invoiceQty;
        double basicAmt;
        double cdAmt;
        double taxableAmt;
        double cgstAmt;
        double sgstAmt;
        double igstAmt;

        string cgst;
        string sgst;
        string igst;

        double grandTotal;
        string vehicleNo;
        double freightAmt;

        public Int32 IdInvoicePurchase { get => idInvoicePurchase; set => idInvoicePurchase = value; }
        public Int32 PurSchSummaryId { get => purSchSummaryId; set => purSchSummaryId = value; }
        public string InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        public string InvoiceNo { get => invoiceNo; set => invoiceNo = value; }
        public Int32 BillingTypeId { get => billingTypeId; set => billingTypeId = value; }
        public string BuyerName { get => buyerName; set => buyerName = value; }
        public string BuyerGstNo { get => buyerGstNo; set => buyerGstNo = value; }
        public double InvoiceQty { get => invoiceQty; set => invoiceQty = value; }
        public double BasicAmt { get => basicAmt; set => basicAmt = value; }
        public double CdAmt { get => cdAmt; set => cdAmt = value; }
        public double TaxableAmt { get => taxableAmt; set => taxableAmt = value; }
        public double CgstAmt { get => cgstAmt; set => cgstAmt = value; }
        public double SgstAmt { get => sgstAmt; set => sgstAmt = value; }
        public double IgstAmt { get => igstAmt; set => igstAmt = value; }
        public double GrandTotal { get => grandTotal; set => grandTotal = value; }
        public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        public double FreightAmt { get => freightAmt; set => freightAmt = value; }

        public string Cgst { get => cgst; set => cgst = value; }
        public string Sgst { get => sgst; set => sgst = value; }
        public string Igst { get => igst; set => igst = value; }
    }

    public class SaudaReportTo
    {
        string materialType;
        DateTime saudaEndDate;
        string purchaseManager;
        string partyName;
        DateTime saudaDate;
        double orgSaudaQty;
        double rate;
        double openingSaudaQty;
        double closingSaudaQty;
        double todaysUnloadingQty;

        double todayConfirmedUnloadQty;
        string statusName;
        string enquiryNo;
        int enquiryId;

        int daysElapsed;
        int cnfOrgId;

        int deliveryDays;

        int dealerOrgId;
        public int DaysElapsed { get => daysElapsed; set => daysElapsed = value; }
        public int EnquiryId { get => enquiryId; set => enquiryId = value; }
        public DateTime SaudaDate { get => saudaDate; set => saudaDate = value; }
        public string MaterialType { get => materialType; set => materialType = value; }
        public double OrgSaudaQty { get => orgSaudaQty; set => orgSaudaQty = value; }
        public string PurchaseManager { get => purchaseManager; set => purchaseManager = value; }
        public string PartyName { get => partyName; set => partyName = value; }
        public string EnquiryNo { get => enquiryNo; set => enquiryNo = value; }
        public DateTime SaudaEndDate { get => saudaEndDate; set => saudaEndDate = value; }
        public string StatusName { get => statusName; set => statusName = value; }
        public double OpeningSaudaQty { get => openingSaudaQty; set => openingSaudaQty = value; }
        public double TodayConfirmedUnloadQty { get => todayConfirmedUnloadQty; set => todayConfirmedUnloadQty = value; }

        public double Rate { get => rate; set => rate = value; }
        public double TodaysUnloadingQty { get => todaysUnloadingQty; set => todaysUnloadingQty = value; }
        public double ClosingSaudaQty { get => closingSaudaQty; set => closingSaudaQty = value; }
        public int CnfOrgId { get => cnfOrgId; set => cnfOrgId = value; }
        public int DeliveryDays { get => deliveryDays; set => deliveryDays = value; }
        public int DealerOrgId { get => dealerOrgId; set => dealerOrgId = value; }
        // public int DealerOrgId { get; internal set; }
    }


    public class SaudaReportEnquiryTo
    {
        string materialType;
        string purchaseManager;
        string partyName;
        DateTime saudaDate;
        double orgSaudaQty;
        double rate;
        string enquiryNo;
        int enquiryId;
        int cnfOrgId;
        int deliveryDays;
        int dealerOrgId;
        public int EnquiryId { get => enquiryId; set => enquiryId = value; }
        public DateTime SaudaDate { get => saudaDate; set => saudaDate = value; }
        public string MaterialType { get => materialType; set => materialType = value; }
        public double OrgSaudaQty { get => orgSaudaQty; set => orgSaudaQty = value; }
        public string PurchaseManager { get => purchaseManager; set => purchaseManager = value; }
        public string PartyName { get => partyName; set => partyName = value; }
        public string EnquiryNo { get => enquiryNo; set => enquiryNo = value; }
        public double Rate { get => rate; set => rate = value; }
        public int CnfOrgId { get => cnfOrgId; set => cnfOrgId = value; }
        public int DeliveryDays { get => deliveryDays; set => deliveryDays = value; }
        public int DealerOrgId { get => dealerOrgId; set => dealerOrgId = value; }
        // public int DealerOrgId { get; internal set; }
    }

    public class GradeWiseWnloadingReportTO
    {
        string itemName;
        //int prodItemId;
        double qtyMT;
        String displayQtyMTRpt;
        public string ItemName
        { get => itemName; set => itemName = value; }
        // public int ProdItemId
        // { get => prodItemId; set => prodItemId = value; }
        public double QtyMT
        { get => qtyMT; set => qtyMT = value; }
        public string DisplayQtyMTRpt { get => displayQtyMTRpt; set => displayQtyMTRpt = value; }
    }

    public class PartyWiseWeighingCompaReportTO
    {


        string truckNo;

        string partyName;

        string total;

        double partyTareWt;
        double totalPartyTareWt;        

        double partyNetWt;
        double totalPartyNetWt;        

        double partyGrossWt;
        double totalPartyGrossWt;       

        double srjGrossWt;
        double totalSrjGrossWt;        

        double srjTareWt;
        double totalSrjTareWt;
       
        double srjNetWt;
        double totalSrjNetWt;        

        double srjActualWt;
        double totalSrjActualWt;        

        double possitiveDiff;
        double totalpossitiveDiff;
        
        double negativeDiff;
        double totalNegativeDiff;        

        double dust;
        double totalDust;       

        string isValidInfo;
        string isStorageExcess;

        Double freight;
        Double totalFreight;        

        Double advance;
        Double totalAdvance;
        
        Double unloadingQty;
        Double totalUnloadingQty;        

        Double shortage;
        Double totalShortage;
        
        Double amount;
        Double totalAmount;        

        double impurities;
        double totalImpurities;

        Int16 isTotal;
       

        public string TruckNo { get => truckNo; set => truckNo = value; }
        public string PartyName { get => partyName; set => partyName = value; }

        public string Total { get => total; set => total = value; }

        public double PartyTareWt { get => partyTareWt; set => partyTareWt = value; }
        public double PartyNetWt { get => partyNetWt; set => partyNetWt = value; }
        public double Impurities { get => impurities; set => impurities = value; }

        public double PartyGrossWt { get => partyGrossWt; set => partyGrossWt = value; }

        public double SrjGrossWt { get => srjGrossWt; set => srjGrossWt = value; }

        public double SrjTareWt { get => srjTareWt; set => srjTareWt = value; }

        public double SrjNetWt { get => srjNetWt; set => srjNetWt = value; }

        public double SrjActualWt { get => srjActualWt; set => srjActualWt = value; }

        public double PossitiveDiff { get => possitiveDiff; set => possitiveDiff = value; }

        public double NegativeDiff { get => negativeDiff; set => negativeDiff = value; }

        public double Dust { get => dust; set => dust = value; }

        public string IsValidInfo { get => isValidInfo; set => isValidInfo = value; }

        public string IsStorageExcess { get => isStorageExcess; set => isStorageExcess = value; }

        public Double Freight { get => freight; set => freight = value; }

        public Double Advance { get => advance; set => advance = value; }

        public Double UnloadingQty { get => unloadingQty; set => unloadingQty = value; }

        public Double Shortage { get => shortage; set => shortage = value; }

        public Double Amount { get => amount; set => amount = value; }

        public double TotalPartyTareWt { get => totalPartyTareWt; set => totalPartyTareWt = value; }
        public double TotalPartyNetWt { get => totalPartyNetWt; set => totalPartyNetWt = value; }
        public double TotalPartyGrossWt { get => totalPartyGrossWt; set => totalPartyGrossWt = value; }
        public double TotalSrjGrossWt { get => totalSrjGrossWt; set => totalSrjGrossWt = value; }
        public double TotalSrjTareWt { get => totalSrjTareWt; set => totalSrjTareWt = value; }
        public double TotalSrjNetWt { get => totalSrjNetWt; set => totalSrjNetWt = value; }
        public double TotalSrjActualWt { get => totalSrjActualWt; set => totalSrjActualWt = value; }
        public double TotalpossitiveDiff { get => totalpossitiveDiff; set => totalpossitiveDiff = value; }
        public double TotalNegativeDiff { get => totalNegativeDiff; set => totalNegativeDiff = value; }
        public double TotalDust { get => totalDust; set => totalDust = value; }
        public double TotalFreight { get => totalFreight; set => totalFreight = value; }
        public double TotalAdvance { get => totalAdvance; set => totalAdvance = value; }
        public double TotalUnloadingQty { get => totalUnloadingQty; set => totalUnloadingQty = value; }
        public double TotalShortage { get => totalShortage; set => totalShortage = value; }
        public double TotalAmount { get => totalAmount; set => totalAmount = value; }
        public double TotalImpurities { get => totalImpurities; set => totalImpurities = value; }

        public Int16 IsTotal { get => isTotal; set => isTotal = value; }
    }

    
    public class CorrectionUnloadingReportTO
    {
        DateTime corretionCompletedOn;
        Double correctionQty;
        Double correctionRate;
        Double correctionAmt;
        Int32 supervisorId;
        String supervisorName;
        String itemName;
        String correctionQtyForReport;
        String correctionAmtForReport;
        public DateTime CorretionCompletedOn
        {
            get { return corretionCompletedOn; }
            set { corretionCompletedOn = value; }
        }

        public Double CorrectionQty
        {
            get { return correctionQty; }
            set { correctionQty = value; }
        }

        public Double CorrectionRate
        {
            get { return correctionRate; }
            set { correctionRate = value; }
        }

        public Double CorrectionAmt
        {
            get { return correctionAmt; }
            set { correctionAmt = value; }
        }

        public Int32 SupervisorId
        {
            get { return supervisorId; }
            set { supervisorId = value; }
        }

        public String SupervisorName
        {
            get { return supervisorName; }
            set { supervisorName = value; }
        }
        public String CorretionCompletedOnStr
        {
            get { return CorretionCompletedOn.ToString(StaticStuff.Constants.DefaultDateFormat); }
        }
        public String ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public DateTime CorretionCompletedDate
        {
            get { return CorretionCompletedOn.Date;}
        }
        public String CorrectionQtyForReport
        {
            get { return correctionQtyForReport; }
            set { correctionQtyForReport = value; }
        }
        public String CorrectionAmtForReport
        {
            get { return correctionAmtForReport; }
            set { correctionAmtForReport = value; }
        }
        
           
    }

    public class TallyTransportEnquiryTO
    {
        String date;
        String voucherType;
        String partyName;
        String cash;
        Double tranportAmountRS;
        Double manojSrpPettyCsh;
        String narration;

        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public String VoucherType
        {
            get { return voucherType; }
            set { voucherType = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }
        public String Cash
        {
            get { return cash; }
            set { cash = value; }
        }
        public Double TranportAmountRS
        {
            get { return tranportAmountRS; }
            set { tranportAmountRS = value; }
        }
        public Double ManojSrpPettyCsh
        {
            get { return manojSrpPettyCsh; }
            set { manojSrpPettyCsh = value; }
        }
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
    }

    public class CCTransportEnquiryTO
    {
        String srNo;
        String date;
        String partyName;
        String vehicleNumber;
        Double transportPayment;


        public String SrNo
        {
            get { return srNo; }
            set { srNo = value; }
        }
        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }
        public String VehicleNumber
        {
            get { return vehicleNumber; }
            set { vehicleNumber = value; }
        }
        public Double TransportPayment
        {
            get { return transportPayment; }
            set { transportPayment = value; }
        }
    }

    public class TallyCrOrderReportTO
    {
        String date;
        String voucherType;
        String drLedgerName;
        Double drLedgerAmount;
        String crLedgerName;
        Double crLedgerAmount;
        String narration;

        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public String VoucherType
        {
            get { return voucherType; }
            set { voucherType = value; }
        }
        public String DrLedgerName
        {
            get { return drLedgerName; }
            set { drLedgerName = value; }
        }
        public Double DrLedgerAmount
        {
            get { return drLedgerAmount; }
            set { drLedgerAmount = value; }
        }
        public String CrLedgerName
        {
            get { return crLedgerName; }
            set { crLedgerName = value; }
        }
        public Double CrLedgerAmount
        {
            get { return crLedgerAmount; }
            set { crLedgerAmount = value; }
        }
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
    }

    public class GradeNoteOrderPReportTO
    {
        String date;
        String truckNo;
        String supplierName;
        String pm;
        String location;
        String grade;
        Double qty;
        Double rate;
        Double basicGradeAmount;
        String cgst;
        String sgst;
        String igst;
        String totalTax;
        String tcs;
        String totalAmountToBePaidToParty;
        String invoiceAmount;
        String freight;
        String balanceAmountRs;
        String billNoAndDate;
        String eWayBillNo;
        String eWayBillDate;
        String narration;
        String partyName;
        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public String TruckNo
        {
            get { return truckNo; }
            set { truckNo = value; }
        }
        public String SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        public String Pm
        {
            get { return pm; }
            set { pm = value; }
        }
        public String Location
        {
            get { return location; }
            set { location = value; }
        }
        public String Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public Double BasicGradeAmount
        {
            get { return basicGradeAmount; }
            set { basicGradeAmount = value; }
        }
        public String CGST
        {
            get { return cgst; }
            set { cgst = value; }
        }
        public String SGST
        {
            get { return sgst; }
            set { sgst = value; }
        }
        public String IGST
        {
            get { return igst; }
            set { igst = value; }
        }
        public String TotalTax
        {
            get { return totalTax; }
            set { totalTax = value; }
        }
        public String Tcs
        {
            get { return tcs; }
            set { tcs = value; }
        }
        public String TotalAmountToBePaidToParty
        {
            get { return totalAmountToBePaidToParty; }
            set { totalAmountToBePaidToParty = value; }
        }
        public String InvoiceAmount
        {
            get { return invoiceAmount; }
            set { invoiceAmount = value; }
        }
        public String Freight
        {
            get { return freight; }
            set { freight = value; }
        }
        public String BalanceAmountRs
        {
            get { return balanceAmountRs; }
            set { balanceAmountRs = value; }
        }
        public String BillNoAndDate
        {
            get { return billNoAndDate; }
            set { billNoAndDate = value; }
        }
        public String EWayBillNo
        {
            get { return eWayBillNo; }
            set { eWayBillNo = value; }
        }
        public String EWayBillDate
        {
            get { return eWayBillDate; }
            set { eWayBillDate = value; }
        }
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        public String PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }

    }

}



