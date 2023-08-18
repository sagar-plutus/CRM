using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class NotificationTO
    {
        int alterInstanceId;
        int moduleId;
        String navigationUrl;
        String androidUrl;

        public int AlterInstanceId { get => alterInstanceId; set => alterInstanceId = value; }
        public int ModuleId { get => moduleId; set => moduleId = value; }
        public string NavigationUrl { get => navigationUrl; set => navigationUrl = value; }
        public string AndroidUrl { get => androidUrl; set => androidUrl = value; }
    }
}
