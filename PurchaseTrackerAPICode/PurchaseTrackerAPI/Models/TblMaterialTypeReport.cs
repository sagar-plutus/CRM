using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblMaterialTypeReport
    {
        public string MaterialType;
        public string FromLocation;
        public string ToLocation;
        public int CompletedVehicleCount;
        public decimal CompletedQty;
        public int PendingVehicleCount;
        public decimal PendingQty;
    }
}
