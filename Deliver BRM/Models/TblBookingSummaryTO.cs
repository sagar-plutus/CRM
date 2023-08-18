using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblBookingSummaryTO
    {
        #region Declarations

        string displayName;
        Double bookingQty;
        DateTime timeView;
        Int32 idBooking;
        Double bookingDelQty;
        #endregion

        #region Constructor
        public TblBookingSummaryTO()
        {

        }

        #endregion

        #region GetSet

        public string DisplayName { get => displayName; set => displayName = value; }
        public double BookingQty { get => bookingQty; set => bookingQty = value; }
        public DateTime TimeView { get => timeView; set => timeView = value; }
        public int IdBooking { get => idBooking; set => idBooking = value; }
        public double BookingDelQty { get => bookingDelQty; set => bookingDelQty = value; }

        #endregion
    }
}
