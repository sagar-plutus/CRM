using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblBookingOpngBalTO
    {
        #region Declarations
        Int32 idOpeningBal;
        Int32 bookingId;
        DateTime balAsOnDate;
        Double openingBalQty;
        #endregion

        #region Constructor
        public TblBookingOpngBalTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOpeningBal
        {
            get { return idOpeningBal; }
            set { idOpeningBal = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public DateTime BalAsOnDate
        {
            get { return balAsOnDate; }
            set { balAsOnDate = value; }
        }
        public Double OpeningBalQty
        {
            get { return openingBalQty; }
            set { openingBalQty = value; }
        }
        #endregion
    }
}
