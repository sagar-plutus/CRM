using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DashboardModels
{
    public class LoadingInfo
    {
        #region Declaration

        Double totalLoadingQty;
        Double totalConfirmedLoadingQty;
        Double totalDeliveredQty;
        Double totalPendingQty;
        Double notconfimedLoadingQty;
        #endregion

        #region GetSet
        public double TotalLoadingQty { get => totalLoadingQty; set => totalLoadingQty = value; }
        public double TotalDeliveredQty { get => totalDeliveredQty; set => totalDeliveredQty = value; }
        public double TotalPendingQty { get => totalPendingQty; set => totalPendingQty = value; }
        public double TotalConfirmedLoadingQty { get => totalConfirmedLoadingQty; set => totalConfirmedLoadingQty = value; }
        public double NotconfimedLoadingQty { get => notconfimedLoadingQty; set => notconfimedLoadingQty = value; }

        #endregion
    }
}
