using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblORCReportTO
    {
        #region Declarations

        String dealer;
        String cnfName;
        String statusName;
        Int32 idBooking;
        DateTime createdOn;
        Double bookingQty;
        Double bookingRate;
        String comment;
        Double orcAmt;
        String orcMeasure;
        String rateCalcDesc;

        Int32 invoiceNo;
        DateTime invoiceDate;
        String deliveryLocation;
        Double invoiceQty;
        Double basicAmt;
        Double grandTotal;

        String bookingDisplayNo;
        #endregion

        #region Constructor
        public TblORCReportTO()
        {
        }

        public string RateCalcDesc { get => rateCalcDesc; set => rateCalcDesc = value; }

        #endregion

        #region GetSet

        public String BillingName { get; set; }
        public Double DiscountAmount { get; set; }
        public Double TaxableAmt { get; set; }


        public String Dealer
        {
            get { return dealer; }
            set { dealer = value; }
        }
        public String CnfName
        {
            get { return cnfName; }
            set { cnfName = value; }
        }
        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
        public Int32 IdBooking
        {
            get { return idBooking; }
            set { idBooking = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        public Double BookingQty
        {
            get { return bookingQty; }
            set { bookingQty = value; }
        }
        public Double BookingRate
        {
            get { return bookingRate; }
            set { bookingRate = value; }
        }
        public string Comment { get => comment; set => comment = value; }

        public Double OrcAmt
        {
            get
            {
                return orcAmt;
            }
            set
            {
                orcAmt = value;
            }
        }
        public String OrcMeasure
        {
            get
            {
                return orcMeasure;
            }
            set
            {
                orcMeasure = value;
            }
        }
        public String RateCalDesc
        {
            get
            {
                return rateCalcDesc;
            }
            set
            {
                rateCalcDesc = value;
            }
        }
        public string BookingDisplayNo { get => bookingDisplayNo; set => bookingDisplayNo = value; }

        public DateTime InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        public string DeliveryLocation { get => deliveryLocation; set => deliveryLocation = value; }
        public double InvoiceQty { get => invoiceQty; set => invoiceQty = value; }
        public double BasicAmt { get => basicAmt; set => basicAmt = value; }
        public double GrandTotal { get => grandTotal; set => grandTotal = value; }
        public int InvoiceNo { get => invoiceNo; set => invoiceNo = value; }

        #endregion
    }
}

