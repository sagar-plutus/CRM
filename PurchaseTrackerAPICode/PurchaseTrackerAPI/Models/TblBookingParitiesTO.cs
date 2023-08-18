using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblBookingParitiesTO
    {
        #region Declarations
        Int32 idBookingParity;
        Int32 bookingId;
        Int32 parityId;
        Double bookingRate;
        Int32 brandId;
        #endregion

        #region Constructor
        public TblBookingParitiesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBookingParity
        {
            get { return idBookingParity; }
            set { idBookingParity = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 ParityId
        {
            get { return parityId; }
            set { parityId = value; }
        }

        public Double BookingRate
        {
            get { return bookingRate; }
            set { bookingRate = value; }
        }

        public int BrandId { get => brandId; set => brandId = value; }
        #endregion
    }
}
