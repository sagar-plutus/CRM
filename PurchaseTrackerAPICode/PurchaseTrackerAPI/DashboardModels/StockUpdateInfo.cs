using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DashboardModels
{
    public class StockUpdateInfo
    {
        #region Declaration

        private double totalSysStock;
        private double totalBooksStock;
        private double stockFactor;

        #endregion

        #region Get Set
        public double TotalSysStock { get => totalSysStock; set => totalSysStock = value; }
        public double TotalBooksStock { get => totalBooksStock; set => totalBooksStock = value; }
        public double StockFactor { get => stockFactor; set => stockFactor = value; }

        #endregion
    }
}
