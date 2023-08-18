using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DashboardModels
{
    public class BookingInfo
    {
        #region Declaration

        Double bookedQty;
        Double avgPrice;
        Double bookedcount;

        Double bookedsaudaQty;
        Double avgsaudaPrice;
        Double bookedsaudacount;

        Double rate;
        string materialType;
        string bookingType;

        #endregion

        #region GetSet

        public double BookedQty { get => bookedQty; set => bookedQty = value; }
        public double AvgPrice { get => avgPrice; set => avgPrice = value; }
        public double Bookedcount { get => bookedcount; set => bookedcount = value; }
        public double BookedsaudaQty { get => bookedsaudaQty; set => bookedsaudaQty = value; }
        public double AvgsaudaPrice { get => avgsaudaPrice; set => avgsaudaPrice = value; }
        public double Bookedsaudacount { get => bookedsaudacount; set => bookedsaudacount = value; }
        public double Rate { get => rate; set => rate = value; }

        public string MaterialType { get => materialType; set => materialType = value; }

        public string BookingType { get => bookingType; set => bookingType = value; }
        public int CurrencyId { get; set; }
        public string Currency { get;  set; }


        #endregion
    }
}
