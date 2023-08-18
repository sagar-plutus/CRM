using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblBaseItemMetalCostTO
    {
        #region Declarations
        Int32 idBaseItemMetalCost;
        Int32 globalRatePurchaseId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double baseMetalCostForC;
        Double baseMetalCostForNC;
        Double baseRecovery;

        Int32 cOrNcId;

        Double baseRate;

        Double declaredRate;
        DateTime baseMetalCostDate;



        List<TblGradeExpressionDtlsTO> gradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();
        #endregion

        #region Constructor
        public TblBaseItemMetalCostTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBaseItemMetalCost
        {
            get { return idBaseItemMetalCost; }
            set { idBaseItemMetalCost = value; }
        }
        public Int32 GlobalRatePurchaseId
        {
            get { return globalRatePurchaseId; }
            set { globalRatePurchaseId = value; }
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
        public Double BaseMetalCostForC
        {
            get { return baseMetalCostForC; }
            set { baseMetalCostForC = value; }
        }
        public Double BaseMetalCostForNC
        {
            get { return baseMetalCostForNC; }
            set { baseMetalCostForNC = value; }
        }
        public Double BaseRecovery
        {
            get { return baseRecovery; }
            set { baseRecovery = value; }
        }


        public List<TblGradeExpressionDtlsTO> GradeExpressionDtlsTOList
        {
            get { return gradeExpressionDtlsTOList; }
            set { gradeExpressionDtlsTOList = value; }
        }
        public Int32 COrNcId
        {
            get { return cOrNcId; }
            set { cOrNcId = value; }
        }

        public Double BaseRate
        {
            get { return baseRate; }
            set { baseRate = value; }
        }

        public Double DeclaredRate
        {
            get { return declaredRate; }
            set { declaredRate = value; }
        }


        public DateTime BaseMetalCostDate
        {
            get { return baseMetalCostDate; }
            set { baseMetalCostDate = value; }
        }

      public TblBaseItemMetalCostTO DeepCopy()
        {
            TblBaseItemMetalCostTO other = (TblBaseItemMetalCostTO)this.MemberwiseClone();
            return other;
        }




        #endregion
    }
}
