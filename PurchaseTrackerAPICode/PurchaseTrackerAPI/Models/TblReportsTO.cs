using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblReportsTO
    {
        #region Declarations
        Int32 idReports;
        Int32 moduleId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String reportName;
        String apiName;
        String sqlQuery;
        List<TblFilterReportTO> TblFilterReportTOList;
        dynamic data;
        string whereClause;
        Int32 transactionTypeId;
        Int32 isCustomizedRpt;
        String customizeApiCall;
        String totalMetaData;
        String roleTypeCond;
        String roleTypeId;
        #endregion

        #region Constructor
        public TblReportsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdReports
        {
            get { return idReports; }
            set { idReports = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String ReportName
        {
            get { return reportName; }
            set { reportName = value; }
        }
        public String ApiName
        {
            get { return apiName; }
            set { apiName = value; }
        }
        public String SqlQuery
        {
            get { return sqlQuery; }
            set { sqlQuery = value; }
        }

        public List<TblFilterReportTO> TblFilterReportTOList1 { get => TblFilterReportTOList; set => TblFilterReportTOList = value; }
        public dynamic Data { get => data; set => data = value; }
        public string WhereClause { get => whereClause; set => whereClause = value; }

        public Int32 TransactionTypeId
        {
            get { return transactionTypeId; }
            set { transactionTypeId = value; }
        }

        public Int32 IsCustomizedRpt
        {
            get { return isCustomizedRpt; }
            set { isCustomizedRpt = value; }
        }

        public String CustomizeApiCall
        {
            get { return customizeApiCall; }
            set { customizeApiCall = value; }
        }

        public String TotalMetaData
        {
            get { return totalMetaData; }
            set { totalMetaData = value; }
        }

        public String RoleTypeCond
        {
            get { return roleTypeCond; }
            set { roleTypeCond = value; }
        }

        public String RoleTypeId
        {
            get { return roleTypeId; }
            set { roleTypeId = value; }
        }
        #endregion
    }
}
