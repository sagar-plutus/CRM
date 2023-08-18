using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.StaticStuff;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblExpressionDtlsTO
    {
        #region Declarations
        Int32 idExpDtls;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        string expFormula;
        string expCode;
        string expDesc;
        string expDisplayName;

        Int32 prodClassId;
        Int32 seqNo;
        double maxRecVal;
        Int32 includeInMetalCost;

        string isRecValFrmVariables;


        Int32 cOrNcId;
        Int32 uniqueTrackId;

        #endregion

        #region Constructor
        public TblExpressionDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdExpDtls
        {
            get { return idExpDtls; }
            set { idExpDtls = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
         public Int32 UniqueTrackId
        {
            get { return uniqueTrackId; }
            set { uniqueTrackId = value; }
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
        public string ExpFormula
        {
            get { return expFormula; }
            set { expFormula = value; }
        }
        public string ExpCode
        {
            get { return expCode; }
            set { expCode = value; }
        }
        public string ExpDesc
        {
            get { return expDesc; }
            set { expDesc = value; }
        }

        public Int32 SeqNo
        {
            get { return seqNo; }
            set { seqNo = value; }
        }


        public string ExpDisplayName
        {
            get { return expDisplayName; }
            set { expDisplayName = value; }
        }


        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }

        public double MaxRecVal
        {
            get { return maxRecVal; }
            set { maxRecVal = value; }
        }
        
         public Int32 COrNcId
        {
            get { return cOrNcId; }
            set { cOrNcId = value; }
        }


        public int IncludeInMetalCost { get => includeInMetalCost; set => includeInMetalCost = value; }

        //Prajakta[2019-06-24] Added
        public string IsRecValFrmVariables { get => isRecValFrmVariables; set => isRecValFrmVariables = value; }

        
        #endregion
    }
}
