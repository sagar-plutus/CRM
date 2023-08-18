using System;
using System.Collections.Generic;
using System.Text;
namespace PurchaseTrackerAPI.Models
{
    public class TblVariablesTO
    {
        #region Declarations
        Int32 idVariable;
        Int32 isPerc;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double variableValue;
        string variableDisplayName;
        string createdByName;
        string variableCode;
        int sequanceNo;
        int uniqueTrackId;
        Int32 isProcessVar;

        #endregion

        #region Constructor
        public TblVariablesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVariable
        {
            get { return idVariable; }
            set { idVariable = value; }
        }

        public Int32 SequanceNo
        {
            get { return sequanceNo; }
            set { sequanceNo = value; }
        }
        public Int32 UniqueTrackId
        {
            get { return uniqueTrackId; }
            set { uniqueTrackId = value; }
        }
        public Int32 IsPerc
        {
            get { return isPerc; }
            set { isPerc = value; }
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
        public Double VariableValue
        {
            get { return variableValue; }
            set { variableValue = value; }
        }
        public String VariableDisplayName
        {
            get { return variableDisplayName; }
            set { variableDisplayName = value; }
        }

        public String CreatedByName
        {
            get { return createdByName; }
            set { createdByName = value; }
        }
        public String VariableCode
        {
            get { return variableCode; }
            set { variableCode = value; }
        }
        public Int32 IsProcessVar
        {
            get { return isProcessVar; }
            set { isProcessVar = value; }
        }

        
        #endregion
    }
}
