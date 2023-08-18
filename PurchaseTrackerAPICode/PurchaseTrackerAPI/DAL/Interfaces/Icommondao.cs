using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface Icommondao
    {
        DateTime ServerDateTime { get; }
        
        System.DateTime SelectServerDateTime();
        void SetDateStandards(Object classTO, Double timeOffsetMins);

        //Aniket [30-7-2019] added for IOT

        int GetNextAvailableModRefIdNew();
        int GetAvailNumber(List<int> list, int maxNumber);
        DataTable ToDataTable<T>(List<T> items);
    }
}