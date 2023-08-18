using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblQuotaDeclarationTO
    {
        #region Declarations
        Int32 idQuotaDeclaration;
        Int32 orgId;
        Int32 globalRateId;
        Int32 createdBy;
        DateTime quotaAllocDate;
        DateTime createdOn;
        Double rateBand;
        Double allocQty;
        Double balanceQty;
        Double calculatedRate;
        Int32 validUpto;
        Int32 isActive;
        Int32 updatedBy;
        DateTime updatedOn;
        Double declaredRate;

        String quotaAllocDateStr;
        Object tag;
        string brandName;
        Int32 brandId;
        #endregion

        #region Constructor
        public TblQuotaDeclarationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdQuotaDeclaration
        {
            get { return idQuotaDeclaration; }
            set { idQuotaDeclaration = value; }
        }
        public Int32 OrgId
        {
            get { return orgId; }
            set { orgId = value; }
        }
        public Int32 GlobalRateId
        {
            get { return globalRateId; }
            set { globalRateId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime QuotaAllocDate
        {
            get { return quotaAllocDate; }
            set { quotaAllocDate = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double RateBand
        {
            get { return rateBand; }
            set { rateBand = value; }
        }
        public Double AllocQty
        {
            get { return allocQty; }
            set { allocQty = value; }
        }
        public Double BalanceQty
        {
            get { return balanceQty; }
            set { balanceQty = value; }
        }
        public Double CalculatedRate
        {
            get { return calculatedRate; }
            set { calculatedRate = value; }
        }
        public Int32 ValidUpto
        {
            get { return validUpto; }
            set { validUpto = value; }
        }

        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }

        public Double DeclaredRate
        {
            get { return declaredRate; }
            set { declaredRate = value; }
        }

        public String QuotaAllocDateStr
        {
            get { return quotaAllocDate.ToString(Constants.DefaultDateFormat); }
        }
        public String ValidUptoDateStr
        {
            get { return quotaAllocDate.AddMinutes(validUpto).ToString(Constants.DefaultDateFormat); }
        }

        /// <summary>
        /// [23-11-2017] Vijaymala:Added to get brand name and id 
        /// </summary>
        public string BrandName
        {
            get { return brandName; }
            set { brandName = value; }

        }
        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }

        public object Tag { get => tag; set => tag = value; }
        #endregion
    }
}
