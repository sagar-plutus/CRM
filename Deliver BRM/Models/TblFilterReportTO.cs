using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblFilterReportTO
    {

        #region Declarations
        Int32 idFilterReport;
        Int32 reportId;
        Int32 isRequired;
        String filterName;
        String inputType;
        String sourceApiName;
        String destinationApiName;
        String placeHolder;
        String idHtml;
        Int32 htmlInputTypeId;
        String htmlInputTypeName;
        Int32 sourceApiModuleId;
        Int32 orderArguments;
        Int32 parentControlId;
        String sqlParameterName;
        string outputValue;
        Int32 sqlDbTypeValue;
        string apiValue;
        string whereClause;
        int isOptional;
        #endregion

        #region Constructor
        public TblFilterReportTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdFilterReport
        {
            get { return idFilterReport; }
            set { idFilterReport = value; }
        }
        public Int32 ReportId
        {
            get { return reportId; }
            set { reportId = value; }
        }
        public Int32 IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }
        public String FilterName
        {
            get { return filterName; }
            set { filterName = value; }
        }
        public String InputType
        {
            get { return inputType; }
            set { inputType = value; }
        }
        public String SourceApiName
        {
            get { return sourceApiName; }
            set { sourceApiName = value; }
        }
        public String DestinationApiName
        {
            get { return destinationApiName; }
            set { destinationApiName = value; }
        }
        public String PlaceHolder
        {
            get { return placeHolder; }
            set { placeHolder = value; }
        }
        public String IdHtml
        {
            get { return idHtml; }
            set { idHtml = value; }
        }

        public int HtmlInputTypeId { get => htmlInputTypeId; set => htmlInputTypeId = value; }
        public string HtmlInputTypeName { get => htmlInputTypeName; set => htmlInputTypeName = value; }
        public int SourceApiModuleId { get => sourceApiModuleId; set => sourceApiModuleId = value; }
        public int OrderArguments { get => orderArguments; set => orderArguments = value; }
        public int ParentControlId { get => parentControlId; set => parentControlId = value; }
        public string OutputValue { get => outputValue; set => outputValue = value; }
        public string SqlParameterName { get => sqlParameterName; set => sqlParameterName = value; }
        public int SqlDbTypeValue { get => sqlDbTypeValue; set => sqlDbTypeValue = value; }
        public string ApiValue { get => apiValue; set => apiValue = value; }
        public string WhereClause { get => whereClause; set => whereClause = value; }
        public int IsOptional { get => isOptional; set => isOptional = value; }
        #endregion
    }
}
