using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblGradeExpressionDtlsTO
    {
        #region Declarations
        Int32 idGradeExpressionDtls;
        Int32 purchaseEnquiryDtlsId;
        Int32 purchaseScheduleDtlsId;
        Int32 expressionDtlsId;
        Double gradeValue;
        string expCode;
        string expDisplayName;
        Int32 seqNo;

        Int32 globleRatePurchaseId;
        Int32 includeInMetalCost;

        Int32 baseItemMetalCostId;

        #endregion

        #region Constructor
        public TblGradeExpressionDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGradeExpressionDtls
        {
            get { return idGradeExpressionDtls; }
            set { idGradeExpressionDtls = value; }
        }
        public Int32 PurchaseEnquiryDtlsId
        {
            get { return purchaseEnquiryDtlsId; }
            set { purchaseEnquiryDtlsId = value; }
        }
        public Int32 PurchaseScheduleDtlsId
        {
            get { return purchaseScheduleDtlsId; }
            set { purchaseScheduleDtlsId = value; }
        }
        public Int32 ExpressionDtlsId
        {
            get { return expressionDtlsId; }
            set { expressionDtlsId = value; }
        }
        public Double GradeValue
        {
            get { return gradeValue; }
            set { gradeValue = value; }
        }

        public string ExpCode
        {
            get { return expCode; }
            set { expCode = value; }
        }
        public string ExpDisplayName
        {
            get { return expDisplayName; }
            set { expDisplayName = value; }
        }

        public Int32 SeqNo
        {
            get { return seqNo; }
            set { seqNo = value; }
        }

        public Int32 GlobleRatePurchaseId
        {
            get { return globleRatePurchaseId; }
            set { globleRatePurchaseId = value; }
        }


        public Int32 BaseItemMetalCostId
        {
            get { return baseItemMetalCostId; }
            set { baseItemMetalCostId = value; }
        }


        public int IncludeInMetalCost { get => includeInMetalCost; set => includeInMetalCost = value; }


        #endregion
    }
}
