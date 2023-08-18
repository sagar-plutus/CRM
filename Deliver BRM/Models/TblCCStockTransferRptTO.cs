using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblCCStockTransferRptTO
    {
        #region Declarations
        String srNo;
        String date;
        String time;
        String vehicleNo;
        String fromCAndF;
        String toFromDealer;
        Double loadingNetWeight;
        String itemName;
        #endregion

        #region Constructor
        #endregion

        #region Dataset

       public String SrNo
       {
            get { return srNo; }
            set { srNo = value; }
       }
        public String Date
        {
            get { return date; }
            set { date = value; }
        }
        public String Time
        {
            get { return time; }
            set { time = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String FromCAndF
        {
            get { return fromCAndF; }
            set { fromCAndF = value; }
        }
        public String ToFromDealer
        {
            get { return toFromDealer; }
            set { toFromDealer = value; }
        }
        public Double LoadingNetWeight
        {
            get { return loadingNetWeight; }
            set { loadingNetWeight = value; }
        }
        public String ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }        

        #endregion
    }
}
