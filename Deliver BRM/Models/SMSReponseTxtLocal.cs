using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class SMSReponseTxtLocal
    {
        String balance;
        String batch_id;
        String cost;
        String num_messages;
        string receipt_url;
        String status;

        public string Balance { get => balance; set => balance = value; }
        public string Batch_id { get => batch_id; set => batch_id = value; }
        public string Cost { get => cost; set => cost = value; }
        public string Num_messages { get => num_messages; set => num_messages = value; }
        public string Receipt_url { get => receipt_url; set => receipt_url = value; }
        public string Status { get => status; set => status = value; }
    }
}
