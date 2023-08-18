using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI
{
    public class TblpurchaseEnqShipmemtDtlsExtTO
    {
        #region Declarations
        Int32 idShipmentDtlsExt;
        Int32 shipmentDtlsId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        DateTime etaPortDate;
        DateTime etaIcdDate;
        Double netWt;
        Double grossWt;
        String containerNo;
        String sealNo;
        string doNumber;
        DateTime validTill;

        #endregion

        #region Constructor
        public TblpurchaseEnqShipmemtDtlsExtTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdShipmentDtlsExt
        {
            get { return idShipmentDtlsExt; }
            set { idShipmentDtlsExt = value; }
        }
        public Int32 ShipmentDtlsId
        {
            get { return shipmentDtlsId; }
            set { shipmentDtlsId = value; }
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
        public DateTime EtaPortDate
        {
            get { return etaPortDate; }
            set { etaPortDate = value; }
        }
        public DateTime EtaIcdDate
        {
            get { return etaIcdDate; }
            set { etaIcdDate = value; }
        }
        public Double NetWt
        {
            get { return netWt; }
            set { netWt = value; }
        }

        public Double GrossWt
        {
            get { return grossWt; }
            set { grossWt = value; }
        }

        public String ContainerNo
        {
            get { return containerNo; }
            set { containerNo = value; }
        }
        public String SealNo
        {
            get { return sealNo; }
            set { sealNo = value; }
        }
        public string EtaPortDateStr { get; set; }

        public string EtaIcdDateStr { get; set; }
        public string ValidTillStr { get; set; }

        public string DoNumber
        {
            get { return doNumber; }
            set { doNumber = value; }
        }
        public DateTime ValidTill
        {
            get { return validTill; }
            set { validTill = value; }
        }

        public string ShippingLine { get;  set; }
        public string BillNo { get;  set; }
        public string BeNo { get;  set; }
        public string BeDateStr { get; set; }
        public string BillDateStr { get;  set; }
        public string SupplierName { get;  set; }
        public string IndentureName { get; set; }
        public Int32 SrNo { get; set; }
        public Int32 PurchaseEnqId { get; set; }

        #endregion
    }
}
