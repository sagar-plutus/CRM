using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface IDimReportTemplateBL
    {
        List<DimReportTemplateTO> SelectAllDimReportTemplate();
        DimReportTemplateTO SelectDimReportTemplateTO(Int32 idReport);
        String SelectReportFullName(String reportName);
        DimReportTemplateTO SelectDimReportTemplateTO(String reportName);
        Boolean isVisibleAllowMultisheetReport(String reportFileName, SqlConnection conn, SqlTransaction tran);
        Boolean isVisibleAllowMultisheetReport(String reportFileName);
        int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO);
        int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO);
        int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimReportTemplate(Int32 idReport);
        int DeleteDimReportTemplate(Int32 idReport, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryTO> GetAllEnquiryList(DateTime from_Date, DateTime to_Date,String purchaseMangerIds);
    }
}
