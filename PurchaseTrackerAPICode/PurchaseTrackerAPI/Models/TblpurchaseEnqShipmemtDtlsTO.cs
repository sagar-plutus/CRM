using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI
{
    public class TblpurchaseEnqShipmemtDtlsTO
    {
        #region Declarations
        Int32 idShipmentDtls;
        Int32 purchaseEnquiryId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        DateTime billDate;
        DateTime beDate;
        String shippingLine;
        String billNo;
        String beNo;
        List<TblpurchaseEnqShipmemtDtlsExtTO> tblpurchaseEnqShipmemtDtlsExtTOList;
        #endregion

        #region Constructor
        public TblpurchaseEnqShipmemtDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdShipmentDtls
        {
            get { return idShipmentDtls; }
            set { idShipmentDtls = value; }
        }
        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
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
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public DateTime BillDate
        {
            get { return billDate; }
            set { billDate = value; }
        }
        public DateTime BeDate
        {
            get { return beDate; }
            set { beDate = value; }
        }
        public String ShippingLine
        {
            get { return shippingLine; }
            set { shippingLine = value; }
        }
        public String BillNo
        {
            get { return billNo; }
            set { billNo = value; }
        }
        public String BeNo
        {
            get { return beNo; }
            set { beNo = value; }
        }

        public List<TblpurchaseEnqShipmemtDtlsExtTO> TblpurchaseEnqShipmemtDtlsExtTOList
        {
            get { return tblpurchaseEnqShipmemtDtlsExtTOList; }
            set { tblpurchaseEnqShipmemtDtlsExtTOList = value; }
        }

        public string BillDateStr { get; set; }
        public string BeDateStr { get; set; }
        public string SupplierName { get; set; }
        public string IndentureName { get; set; }
        #endregion
    }
}
