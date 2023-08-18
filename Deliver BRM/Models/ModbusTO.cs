using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{


    public class ModbusTO
    {

        public string LoadingSlipNo { get; set; }  //temploading
        public int LoadingId { get; set; }
        public int InvoiceId { get; set; }
        public string LoadingSlipsNo { get; set; } // temploadingslip
    }


}
