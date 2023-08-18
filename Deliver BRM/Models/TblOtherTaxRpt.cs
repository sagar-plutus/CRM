using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblOtherTaxRpt
    {
        Int32 idInvoice;
        Int32 invoiceItemId;
        String prodItemDesc;
        String deliveryLocation;
        Int32 otherTaxId;
        Int32 statusId;
        String statusName;
        DateTime statusDate;
        String transporterName;

        String invoiceNo;
        String vehicleNo;
        DateTime invoiceDate;
        String partyName;
        String cnfName;
        Double basicAmt;
        Double taxAmt;
        Double grandTotal;
        Int32 isConfirmed;
        Double taxableAmt;

        Double netWeight;

        public int IdInvoice { get => idInvoice; set => idInvoice = value; }
        public string InvoiceNo { get => invoiceNo; set => invoiceNo = value; }
        public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        public DateTime InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        public string PartyName { get => partyName; set => partyName = value; }
        public string CnfName { get => cnfName; set => cnfName = value; }
        public double BasicAmt { get => basicAmt; set => basicAmt = value; }
        public double TaxAmt { get => taxAmt; set => taxAmt = value; }
        public double GrandTotal { get => grandTotal; set => grandTotal = value; }
        public int IsConfirmed { get => isConfirmed; set => isConfirmed = value; }
        public double TaxableAmt { get => taxableAmt; set => taxableAmt = value; }
        public int InvoiceItemId { get => invoiceItemId; set => invoiceItemId = value; }
        public string ProdItemDesc { get => prodItemDesc; set => prodItemDesc = value; }
        public string DeliveryLocation { get => deliveryLocation; set => deliveryLocation = value; }
        public int OtherTaxId { get => otherTaxId; set => otherTaxId = value; }
        public int StatusId { get => statusId; set => statusId = value; }
        public string StatusName { get => statusName; set => statusName = value; }
        public DateTime StatusDate { get => statusDate; set => statusDate = value; }
        public string TransporterName { get => transporterName; set => transporterName = value; }
        public double NetWeight { get => netWeight; set => netWeight = value; }
    }
}
