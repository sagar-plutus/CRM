using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PurchaseTrackerAPI.Models
{
    public class DimReportTemplateTO
    {
        #region Declarations
        Int32 idReport;
        Int32 isDisplayMultisheetAllow;
        Int32 createdBy;
        DateTime createdOn;
        String reportName;
        String reportFileName;
        String reportFileExtension;
        String reportPassword;
        #endregion

        #region Constructor
        public DimReportTemplateTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdReport
        {
            get { return idReport; }
            set { idReport = value; }
        }
        public Int32 IsDisplayMultisheetAllow
        {
            get { return isDisplayMultisheetAllow; }
            set { isDisplayMultisheetAllow = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String ReportName
        {
            get { return reportName; }
            set { reportName = value; }
        }
        public String ReportFileName
        {
            get { return reportFileName; }
            set { reportFileName = value; }
        }
        public String ReportFileExtension
        {
            get { return reportFileExtension; }
            set { reportFileExtension = value; }
        }
        public String ReportPassword
        {
            get { return reportPassword; }
            set { reportPassword = value; }
        }
        #endregion
    }
}
