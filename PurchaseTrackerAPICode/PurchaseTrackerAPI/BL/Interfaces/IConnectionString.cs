using System;
namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface IConnectionString{
        String GetConnectionString(String ConfigName);

        string GetSubDomain();
    }
}



