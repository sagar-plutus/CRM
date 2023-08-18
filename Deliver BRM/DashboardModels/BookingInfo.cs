using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DashboardModels
{
    public class BookingInfo
    {
        #region Declaration

        Double bookedQty;
        Double avgPrice;



        #endregion

        #region GetSet
        public double BookedQty { get => bookedQty; set => bookedQty = value; }
        public double AvgPrice { get => avgPrice; set => avgPrice = value; }
        #endregion
    }
}
