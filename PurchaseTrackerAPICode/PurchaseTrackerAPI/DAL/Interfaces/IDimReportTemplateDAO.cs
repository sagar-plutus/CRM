using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;


namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimReportTemplateDAO
    {
        String SqlSelectQuery();
        List<DimReportTemplateTO> SelectAllDimReportTemplate();
        DimReportTemplateTO SelectDimReportTemplate(Int32 idReport);
        List<DimReportTemplateTO> ConvertDTToList(SqlDataReader dimReportTemplateTODT);
        DimReportTemplateTO SelectDimReportTemplate(String reportName);
        List<DimReportTemplateTO> isVisibleAllowMultisheetReportList(string reportFileName, SqlConnection conn, SqlTransaction tran);
        List<DimReportTemplateTO> isVisibleAllowMultisheetReportList(string reportFileName);
        int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO);
        int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimReportTemplateTO dimReportTemplateTO, SqlCommand cmdInsert);
        int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO);
        int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryTO> GetAllEnquiryList(DateTime from_Date, DateTime to_Date,String purchaseMangerIds);
        int ExecuteUpdationCommand(DimReportTemplateTO dimReportTemplateTO, SqlCommand cmdUpdate);
        int DeleteDimReportTemplate(Int32 idReport);
        int DeleteDimReportTemplate(Int32 idReport, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idReport, SqlCommand cmdDelete);

    }
}