using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class UnloadingRptTO : Models.TblUnLoadingTO
    {
        #region Declaration

        private String supplierName;
        private double grossWeight;
        private double tareWeight;
        private double netWeight;


        #endregion

        #region GetSet

        public string SupplierName { get => supplierName; set => supplierName = value; }
        public double GrossWeight { get => grossWeight; set => grossWeight = value; }
        public double TareWeight { get => tareWeight; set => tareWeight = value; }
        public double NetWeight { get => netWeight; set => netWeight = value; }
        public string CreatedOnstr { get { return this.CreatedOn.ToString(StaticStuff.Constants.DefaultDateFormat); }}

        #endregion

    }
}
