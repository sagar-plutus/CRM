using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblGstCodeDtlsTO
    {
        #region Declarations
        Int32 idGstCode;
        Int32 codeTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime effectiveFromDt;
        DateTime effectiveToDt;
        DateTime createdOn;
        DateTime updatedOn;
        Double taxPct;
        String codeDesc;
        String codeNumber;
        Int32 isActive;

        List<TblTaxRatesTO> taxRatesTOList;
        #endregion

        #region Constructor
        public TblGstCodeDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGstCode
        {
            get { return idGstCode; }
            set { idGstCode = value; }
        }
        public Int32 CodeTypeId
        {
            get { return codeTypeId; }
            set { codeTypeId = value; }
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
        public String CodeDesc
        {
            get { return codeDesc; }
            set { codeDesc = value; }
        }
        public String CodeNumber
        {
            get { return codeNumber; }
            set { codeNumber = value; }
        }

        public String EffectiveFromDtStr
        {
            get { return effectiveFromDt.ToString(Constants.DefaultDateFormat); }
        }
        public String EffectiveToDtStr
        {
            get
            {
                if (effectiveToDt != DateTime.MinValue)
                    return effectiveToDt.ToString(Constants.DefaultDateFormat);
                else
                    return string.Empty;
            }

        }

        public List<TblTaxRatesTO> TaxRatesTOList { get => taxRatesTOList; set => taxRatesTOList = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        #endregion
    }
}
