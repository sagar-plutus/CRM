using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface IRunReport
    {
        ResultMessage GenrateMktgInvoiceReport(DataSet invoiceDS, String templatePath, String localFilePath, Constants.ReportE reportE, Boolean IsProduction);
        ResultMessage GenrateMktgInvoiceReport(DataSet mktgInvoiceDS, String templatePath, String localFilePath, Constants.ReportE reportE, SqlConnection conn, SqlTransaction tran);
        void OpenExcelFileReport(String fileName);
        void OpenPDFFileReport(String fileName);
        ResultMessage GenrateReportWithPassword(DataSet mktgInvoiceDS, String templatePath, String localFilePath, String password);
        String GetDriveNameNotOSDrive();
    }
}
