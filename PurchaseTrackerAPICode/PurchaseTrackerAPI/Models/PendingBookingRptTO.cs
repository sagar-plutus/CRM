using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class PendingBookingRptTO
    {
        #region Declaration

        String cnfName;
        String dealerName;
        Int32 bookingId;
        Int32 cnfOrgId;
        Int32 dealerOrgId;
        DateTime bookingDate;
        String bookingDateStr;
        Double bookingRate;
        Double openingBalanceMT;
        Double todaysBookingMT;
        Double todaysLoadingQtyMT;
        Double loadAndDelBookingQty;
        Double todaysDelBookingQty;
        Double avgPrice;
        Double pendingQty;
        Double closingBalance;
        Int32 noOfDayElapsed;

        #endregion

        #region Get Set

        public string CnfName { get => cnfName; set => cnfName = value; }
        public string DealerName { get => dealerName; set => dealerName = value; }
        public double OpeningBalanceMT { get => openingBalanceMT; set => openingBalanceMT = value; }
        public double TodaysBookingMT { get => todaysBookingMT; set => todaysBookingMT = value; }
        public double TodaysLoadingQtyMT { get => todaysLoadingQtyMT; set => todaysLoadingQtyMT = value; }
        public double AvgPrice { get => avgPrice; set => avgPrice = value; }
        public double PendingQty { get => pendingQty; set => pendingQty = value; }
        public double LoadAndDelBookingQty { get => loadAndDelBookingQty; set => loadAndDelBookingQty = value; }
        public int BookingId { get => bookingId; set => bookingId = value; }
        public DateTime BookingDate { get => bookingDate; set => bookingDate = value; }
        public String BookingDateStr { get { return bookingDate.ToString(Constants.DefaultDateFormat); } }
        public Double BookingRate { get => bookingRate; set => bookingRate = value; }
        public double ClosingBalance { get => closingBalance; set => closingBalance = value; }
        public int CnfOrgId { get => cnfOrgId; set => cnfOrgId = value; }
        public int DealerOrgId { get => dealerOrgId; set => dealerOrgId = value; }
        public double TodaysDelBookingQty { get => todaysDelBookingQty; set => todaysDelBookingQty = value; }
        public int NoOfDayElapsed { get => noOfDayElapsed; set => noOfDayElapsed = value; }

        #endregion
    }
}
