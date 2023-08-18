using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class DimReportTemplateBL : IDimReportTemplateBL
    {
        private readonly IDimReportTemplateDAO _iDimReportTemplateDAO;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        public DimReportTemplateBL(ITblConfigParamsBL iTblConfigParamsBL, IDimReportTemplateDAO iDimReportTemplateDAO)
        {
            _iDimReportTemplateDAO = iDimReportTemplateDAO;
            _iTblConfigParamsBL = iTblConfigParamsBL;
        }
        #region Selection
        public List<DimReportTemplateTO> SelectAllDimReportTemplate()
        {
            return _iDimReportTemplateDAO.SelectAllDimReportTemplate();
        }


        public DimReportTemplateTO SelectDimReportTemplateTO(Int32 idReport)
        {
            return _iDimReportTemplateDAO.SelectDimReportTemplate(idReport);
        }
        public List<TblPurchaseEnquiryTO> GetAllEnquiryList(DateTime fromDate, DateTime toDate,String purchaseMangerIds)
        {
            return _iDimReportTemplateDAO.GetAllEnquiryList(fromDate,toDate, purchaseMangerIds);
        }


        public String SelectReportFullName(String reportName)
        {
            String reportFullName = null;

            //MstReportTemplateTO mstReportTemplateTO = MstReportTemplateDAO.SelectMstReportTemplateTO(reportName);
            DimReportTemplateTO dimReportTemplateTO = SelectDimReportTemplateTO(reportName);
            if (dimReportTemplateTO != null)
            {

                TblConfigParamsTO templatePath = _iTblConfigParamsBL.SelectTblConfigParamsValByName("REPORT_TEMPLATE_FOLDER_PATH");
                //object templatePath = BL.MstCsParamBL.GetValue("TEMP_REPORT_PATH");//For Testing Pramod InputRemovalExciseReport

                if (templatePath != null)
                    return templatePath.ConfigParamVal+ dimReportTemplateTO.ReportFileName + "." + dimReportTemplateTO.ReportFileExtension;
            }
            return reportFullName;
        }

        public DimReportTemplateTO SelectDimReportTemplateTO(String reportName)
        {
            return _iDimReportTemplateDAO.SelectDimReportTemplate(reportName);


        } 

        /// <summary>
        /// KISHOR [2014-11-28] Add - with conn tran
        /// </summary>
        /// <param name="reportFileName"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public Boolean isVisibleAllowMultisheetReport(String reportFileName, SqlConnection conn, SqlTransaction tran)
        {
            
            List<DimReportTemplateTO> dimReportTemplateTOList = _iDimReportTemplateDAO.isVisibleAllowMultisheetReportList(reportFileName, conn, tran);
            if (dimReportTemplateTOList != null && dimReportTemplateTOList.Count == 1)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Amol[2011-09-16] Check is allow multisheet report for PDF file
        /// </summary>
        /// <param name="mstReportTemplateTO"></param>
        /// <returns></returns>
        /// 
        public Boolean isVisibleAllowMultisheetReport(String reportFileName)
        {
           
            List<DimReportTemplateTO>dimReportTemplateTOList = _iDimReportTemplateDAO.isVisibleAllowMultisheetReportList(reportFileName);
            if (dimReportTemplateTOList != null && dimReportTemplateTOList.Count == 1)
            {
                return true;
            }
            else
                return false;
        }
    
        #endregion

        #region Insertion
        public int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO)
        {
            return _iDimReportTemplateDAO.InsertDimReportTemplate(dimReportTemplateTO);
        }

        public int InsertDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimReportTemplateDAO.InsertDimReportTemplate(dimReportTemplateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO)
        {
            return _iDimReportTemplateDAO.UpdateDimReportTemplate(dimReportTemplateTO);
        }

        public int UpdateDimReportTemplate(DimReportTemplateTO dimReportTemplateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimReportTemplateDAO.UpdateDimReportTemplate(dimReportTemplateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimReportTemplate(Int32 idReport)
        {
            return _iDimReportTemplateDAO.DeleteDimReportTemplate(idReport);
        }

        public int DeleteDimReportTemplate(Int32 idReport, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimReportTemplateDAO.DeleteDimReportTemplate(idReport, conn, tran);
        }

        #endregion
        
    }
}
