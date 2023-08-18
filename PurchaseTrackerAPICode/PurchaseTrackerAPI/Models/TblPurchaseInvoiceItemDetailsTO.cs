using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseInvoiceItemDetailsTO
    {
        #region Declarations
        Int32 purchaseScheduleDetailsId;
        Int32 prodClassId;
        Int32 productItemId;
        Int32 prodGstCodeId;
        Int32 cdStructureId;
        Int32 otherTaxId;
        Double invoiceQty;
        Double rate;
        Double basicTotal;
        Double taxableAmt;
        Double grandTotal;
        Double cdStructure;
        Double cdAmt;
        Double taxPct;
        Double taxAmt;
        Int64 idPurchaseInvoiceItem;
        Int64 purchaseInvoiceId;
        String productItemDesc;
        String gstinCodeNo;
        TblGstCodeDtlsTO gstCodeDtlsTO;
        List<TblPurchaseInvoiceItemTaxDetailsTO> tblPurchaseInvoiceItemTaxDetailsTOList = new List<TblPurchaseInvoiceItemTaxDetailsTO>();

        Int32 sapProdItemId;
        Double cdPerc;
        #endregion

        #region Constructor
        public TblPurchaseInvoiceItemDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 PurchaseScheduleDetailsId
        {
            get { return purchaseScheduleDetailsId; }
            set { purchaseScheduleDetailsId = value; }
        }
        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public Int32 ProductItemId
        {
            get { return productItemId; }
            set { productItemId = value; }
        }
        public Int32 ProdGstCodeId
        {
            get { return prodGstCodeId; }
            set { prodGstCodeId = value; }
        }
        public Int32 CdStructureId
        {
            get { return cdStructureId; }
            set { cdStructureId = value; }
        }
        public Int32 OtherTaxId
        {
            get { return otherTaxId; }
            set { otherTaxId = value; }
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
        public Double TaxPct
        {
            get { return taxPct; }
            set { taxPct = value; }
        }
        public Double TaxAmt
        {
            get { return taxAmt; }
            set { taxAmt = value; }
        }
        public Int64 IdPurchaseInvoiceItem
        {
            get { return idPurchaseInvoiceItem; }
            set { idPurchaseInvoiceItem = value; }
        }
        public Int64 PurchaseInvoiceId
        {
            get { return purchaseInvoiceId; }
            set { purchaseInvoiceId = value; }
        }
        public String ProductItemDesc
        {
            get { return productItemDesc; }
            set { productItemDesc = value; }
        }
        public String GstinCodeNo
        {
            get { return gstinCodeNo; }
            set { gstinCodeNo = value; }
        }
        public TblGstCodeDtlsTO GstCodeDtlsTO
        {
            get { return gstCodeDtlsTO; }
            set { gstCodeDtlsTO = value; }
        }
        public List<TblPurchaseInvoiceItemTaxDetailsTO> TblPurchaseInvoiceItemTaxDetailsTOList { get => tblPurchaseInvoiceItemTaxDetailsTOList; set => tblPurchaseInvoiceItemTaxDetailsTOList = value; }

        
        public Int32 SapProdItemId
        {
            get { return sapProdItemId; }
            set { sapProdItemId = value; }
        }
        public Double CdPerc
        {
            get { return cdPerc; }
            set { cdPerc = value; }
        }

        

        #endregion
    }
}
