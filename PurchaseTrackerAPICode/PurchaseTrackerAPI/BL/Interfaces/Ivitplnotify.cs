using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface Ivitplnotify
    {
        string NotifyToRegisteredDevices();
        string NotifyToRegisteredDevices(String[] devices, String body, String title);
         void NotifyToRegisteredDevicesAsync();

    }
}