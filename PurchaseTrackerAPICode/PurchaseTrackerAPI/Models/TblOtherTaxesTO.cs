using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblOtherTaxesTO
    {
        #region Declarations
        Int32 idOtherTax;
        Int32 isBefore;
        Int32 isAfter;
        Int32 both;
        Int32 isActive;
        Int32 createdBy;
        DateTime createdOn;
        Double defaultPct;
        Double defaultAmt;
        String taxName;
        #endregion

        #region Constructor
        public TblOtherTaxesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOtherTax
        {
            get { return idOtherTax; }
            set { idOtherTax = value; }
        }
        public Int32 IsBefore
        {
            get { return isBefore; }
            set { isBefore = value; }
        }
        public Int32 IsAfter
        {
            get { return isAfter; }
            set { isAfter = value; }
        }
        public Int32 Both
        {
            get { return both; }
            set { both = value; }
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
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double DefaultPct
        {
            get { return defaultPct; }
            set { defaultPct = value; }
        }
        public Double DefaultAmt
        {
            get { return defaultAmt; }
            set { defaultAmt = value; }
        }
        public String TaxName
        {
            get { return taxName; }
            set { taxName = value; }
        }
        #endregion
    }
}
