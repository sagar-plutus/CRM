using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingSlipDtlTO
    {
        #region Declarations
        Int32 idLoadSlipDtl;
        Int32 loadingSlipId;
        Int32 bookingId;
        Int32 bookingExtId;
        Double loadingQty;

        Int32 idBooking;
        Double bookingRate;
        String specialNote;
        #endregion

        #region Constructor
        public TblLoadingSlipDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadSlipDtl
        {
            get { return idLoadSlipDtl; }
            set { idLoadSlipDtl = value; }
        }
        public Int32 LoadingSlipId
        {
            get { return loadingSlipId; }
            set { loadingSlipId = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 BookingExtId
        {
            get { return bookingExtId; }
            set { bookingExtId = value; }
        }


        /// <summary>
        /// Sanjay [2017-02-27] This is same property as bookingId. But while serializing when got from UI
        /// it naming is changed. As on UI bookingTO is shown.
        /// </summary>
        public Int32 IdBooking
        {
            get { return idBooking; }
            set { idBooking = value; }
        }
        public Double LoadingQty
        {
            get { return loadingQty; }
            set { loadingQty = value; }
        }

        public double BookingRate { get => bookingRate; set => bookingRate = value; }
        public String SpecialNote { get => specialNote; set => specialNote = value; }
        #endregion
    }
}
