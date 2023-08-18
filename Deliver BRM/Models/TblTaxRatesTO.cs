using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblTaxRatesTO
    {
        #region Declarations
        Int32 idTaxRate;
        Int32 gstCodeId;
        Int32 taxTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime effectiveFromDt;
        DateTime effectiveToDt;
        DateTime createdOn;
        DateTime updatedOn;
        Double taxPct;
        Int32 isActive;
        String sapTaxCode;
        #endregion

        #region Constructor
        public TblTaxRatesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTaxRate
        {
            get { return idTaxRate; }
            set { idTaxRate = value; }
        }
        public Int32 GstCodeId
        {
            get { return gstCodeId; }
            set { gstCodeId = value; }
        }
        public Int32 TaxTypeId
        {
            get { return taxTypeId; }
            set { taxTypeId = value; }
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
        public DateTime EffectiveFromDt
        {
            get { return effectiveFromDt; }
            set { effectiveFromDt = value; }
        }
        public DateTime EffectiveToDt
        {
            get { return effectiveToDt; }
            set { effectiveToDt = value; }
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
        public Double TaxPct
        {
            get { return taxPct; }
            set { taxPct = value; }
        }

        public int IsActive { get => isActive; set => isActive = value; }

        public String EffectiveFromDtStr
        {
            get { return effectiveFromDt.ToString(Constants.DefaultDateFormat); }
        }
        public String EffectiveToDtStr
        {
            get { return effectiveToDt.ToString(Constants.DefaultDateFormat); }

        }

        public string SapTaxCode { get => sapTaxCode; set => sapTaxCode = value; }
        #endregion
    }
}
