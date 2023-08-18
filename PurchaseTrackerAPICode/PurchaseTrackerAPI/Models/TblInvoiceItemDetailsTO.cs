using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblInvoiceItemDetailsTO
    {
        #region Declarations
        Int32 idInvoiceItem;
        Int32 invoiceId;
        Int32 loadingSlipExtId;
        Int32 prodGstCodeId;
        Double bundles;
        Double invoiceQty;
        Double rate;
        Double basicTotal;
        Double taxableAmt;
        Double grandTotal;
        String prodItemDesc;
        /*GJ@20170915 : Added new Field*/
        Double cdStructure;
        Double cdAmt;
        List<TblInvoiceItemTaxDtlsTO> invoiceItemTaxDtlsTOList;
        String gstinCodeNo;
        Int32 cdStructureId;

        Int32 otherTaxId;
        Double taxPct;
        Double taxAmt;
        String changeIn;
        TblGstCodeDtlsTO gstCodeDtlsTO;

        Int32 brandId;

        #endregion

        #region Constructor
        public TblInvoiceItemDetailsTO()
        {
        }

        #endregion

        #region GetSet

        public TblGstCodeDtlsTO GstCodeDtlsTO
        {
            get { return gstCodeDtlsTO; }
            set { gstCodeDtlsTO = value; }
        }

        public Int32 IdInvoiceItem
        {
            get { return idInvoiceItem; }
            set { idInvoiceItem = value; }
        }
        public Int32 InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
        }
        public Int32 LoadingSlipExtId
        {
            get { return loadingSlipExtId; }
            set { loadingSlipExtId = value; }
        }
        public Int32 ProdGstCodeId
        {
            get { return prodGstCodeId; }
            set { prodGstCodeId = value; }
        }
        public Double Bundles
        {
            get { return bundles; }
            set { bundles = value; }
        }
        public Double InvoiceQty
        {
            get { return invoiceQty; }
            set { invoiceQty = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public Double BasicTotal
        {
            get { return basicTotal; }
            set { basicTotal = value; }
        }
        public Double TaxableAmt
        {
            get { return taxableAmt; }
            set { taxableAmt = value; }
        }
        public Double GrandTotal
        {
            get { return grandTotal; }
            set { grandTotal = value; }
        }
        public String ProdItemDesc
        {
            get { return prodItemDesc; }
            set { prodItemDesc = value; }
        }
        public List<TblInvoiceItemTaxDtlsTO> InvoiceItemTaxDtlsTOList
        {
            get { return invoiceItemTaxDtlsTOList; }
            set { invoiceItemTaxDtlsTOList = value; }
        }
        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }
        public Double CdAmt
        {
            get { return cdAmt; }
            set { cdAmt = value; }
        }

        public string GstinCodeNo { get => gstinCodeNo; set => gstinCodeNo = value; }
        public int OtherTaxId { get => otherTaxId; set => otherTaxId = value; }
        public double TaxPct { get => taxPct; set => taxPct = value; }
        public double TaxAmt { get => taxAmt; set => taxAmt = value; }
        public Int32 CdStructureId { get => cdStructureId; set => cdStructureId = value; }
        public string ChangeIn { get => changeIn; set => changeIn = value; }
        public int BrandId { get => brandId; set => brandId = value; }
        #endregion

        public TblInvoiceItemDetailsTO DeepCopy()
        {
            TblInvoiceItemDetailsTO other = (TblInvoiceItemDetailsTO)this.MemberwiseClone();
            return other;
        }
    }
}
