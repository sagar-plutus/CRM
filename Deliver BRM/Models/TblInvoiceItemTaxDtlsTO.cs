using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblInvoiceItemTaxDtlsTO
    {
        #region Declarations
        Int32 idInvItemTaxDtl;
        Int32 invoiceItemId;
        Int32 taxRateId;
        Double taxPct;
        Double taxRatePct;
        Double taxableAmt;
        Double taxAmt;
        Int32 taxTypeId;
        #endregion

        #region Constructor
        public TblInvoiceItemTaxDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdInvItemTaxDtl
        {
            get { return idInvItemTaxDtl; }
            set { idInvItemTaxDtl = value; }
        }
        public Int32 InvoiceItemId
        {
            get { return invoiceItemId; }
            set { invoiceItemId = value; }
        }
        public Int32 TaxRateId
        {
            get { return taxRateId; }
            set { taxRateId = value; }
        }
        public Double TaxPct
        {
            get { return taxPct; }
            set { taxPct = value; }
        }
        public Double TaxRatePct
        {
            get { return taxRatePct; }
            set { taxRatePct = value; }
        }
        public Double TaxableAmt
        {
            get { return taxableAmt; }
            set { taxableAmt = value; }
        }
        public Double TaxAmt
        {
            get { return taxAmt; }
            set { taxAmt = value; }
        }

        public int TaxTypeId { get => taxTypeId; set => taxTypeId = value; }
        #endregion

        #region MyRegion

        public TblInvoiceItemTaxDtlsTO DeepCopy()
        {
            TblInvoiceItemTaxDtlsTO other = (TblInvoiceItemTaxDtlsTO)this.MemberwiseClone();
            return other;
        }

        #endregion
    }
}
