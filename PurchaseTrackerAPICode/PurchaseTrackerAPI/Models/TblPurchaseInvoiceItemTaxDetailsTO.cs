using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseInvoiceItemTaxDetailsTO
    {
        #region Declarations
        Int32 taxRateId;
        Double taxPct;
        Double taxRatePct;
        Double taxableAmt;
        Double taxAmt;
        Int64 idPurchaseInvItemTaxDtl;
        Int64 purchaseInvoiceItemId;
        int taxTypeId;
        #endregion

        #region Constructor
        public TblPurchaseInvoiceItemTaxDetailsTO()
        {
        }

        #endregion

        #region GetSet  

        public Int32 TaxTypeId
        {
            get { return taxTypeId; }
            set { taxTypeId = value; }
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
        public Int64 IdPurchaseInvItemTaxDtl
        {
            get { return idPurchaseInvItemTaxDtl; }
            set { idPurchaseInvItemTaxDtl = value; }
        }
        public Int64 PurchaseInvoiceItemId
        {
             get { return purchaseInvoiceItemId; }
            set { purchaseInvoiceItemId = value; }
        }
        #endregion
    }
}
