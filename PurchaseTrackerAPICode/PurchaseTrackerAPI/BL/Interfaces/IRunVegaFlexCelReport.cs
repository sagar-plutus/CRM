using FlexCel.Render;
using FlexCel.Report;
using PurchaseTrackerAPI.StaticStuff;
//using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface IRunVegaFlexCelReport
    {
        Boolean Run(DataSet dataSet, String templateFileName);
        ResultMessage Run(DataSet dataSet, String templateFileName, String fileName, Boolean IsProduction);
        ResultMessage Run(DataSet dataSet, String templateFileName, String fileName, SqlConnection conn, SqlTransaction tran);
        ResultMessage Run(DataSet dataSet, String templateFileName, String fileName, String password);
    }
}
