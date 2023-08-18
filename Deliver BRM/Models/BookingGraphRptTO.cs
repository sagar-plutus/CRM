using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class BookingGraphRptTO
    {
        #region Declarations
        Int32 bookingId;
        Int32 cnFOrgId;
        Int32 dealerOrgId;
        String cnfName;
        String dealerName;
        Double bookingQty;
        Double bookingRate;
        Double avgPrice;
        #endregion

        #region Get Set
        public int BookingId { get => bookingId; set => bookingId = value; }
        public int CnFOrgId { get => cnFOrgId; set => cnFOrgId = value; }
        public int DealerOrgId { get => dealerOrgId; set => dealerOrgId = value; }
        public string CnfName { get => cnfName; set => cnfName = value; }
        public string DealerName { get => dealerName; set => dealerName = value; }
        public double BookingQty { get => bookingQty; set => bookingQty = value; }
        public Double BookingRate { get => bookingRate; set => bookingRate = value; }
        public double AvgPrice { get => avgPrice; set => avgPrice = value; }
        #endregion

    }
}
