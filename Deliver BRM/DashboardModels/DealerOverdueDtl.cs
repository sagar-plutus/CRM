using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DashboardModels
{
    public class DealerOverdueDtl
    {
        #region Declaration

        Double overdueAmt;
        Double enquiryAmt;
        Int32 idOrganization;

        #endregion

        #region GetSet
        public double OverdueAmt { get => overdueAmt; set => overdueAmt = value; }
        public double EnquiryAmt { get => enquiryAmt; set => enquiryAmt = value; }
        public Int32 IdOrganization { get => idOrganization; set => idOrganization = value; }

        #endregion
    }
}