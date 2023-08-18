using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblInvoiceHistoryTO
    {
        #region Declarations
        Int32 idInvHistory;
        Int32 invoiceId;
        Int32 invoiceItemId;
        Int32 oldCdStructureId;
        Int32 newCdStructureId;
        Int32 statusId;
        Int32 createdBy;
        DateTime statusDate;
        DateTime createdOn;
        Double oldUnitRate;
        Double newUnitRate;
        Double oldQty;
        Double newQty;
        String oldBillingAddr;
        String newBillingAddr;
        String oldConsinAddr;
        String newConsinAddr;
        String oldEwayBillNo;
        String newEwayBillNo;
        String statusRemark;
        #endregion

        #region Constructor
        public TblInvoiceHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdInvHistory
        {
            get { return idInvHistory; }
            set { idInvHistory = value; }
        }
        public Int32 InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
        }
        public Int32 InvoiceItemId
        {
            get { return invoiceItemId; }
            set { invoiceItemId = value; }
        }
        public Int32 OldCdStructureId
        {
            get { return oldCdStructureId; }
            set { oldCdStructureId = value; }
        }
        public Int32 NewCdStructureId
        {
            get { return newCdStructureId; }
            set { newCdStructureId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double OldUnitRate
        {
            get { return oldUnitRate; }
            set { oldUnitRate = value; }
        }
        public Double NewUnitRate
        {
            get { return newUnitRate; }
            set { newUnitRate = value; }
        }
        public Double OldQty
        {
            get { return oldQty; }
            set { oldQty = value; }
        }
        public Double NewQty
        {
            get { return newQty; }
            set { newQty = value; }
        }
        public String OldBillingAddr
        {
            get { return oldBillingAddr; }
            set { oldBillingAddr = value; }
        }
        public String NewBillingAddr
        {
            get { return newBillingAddr; }
            set { newBillingAddr = value; }
        }
        public String OldConsinAddr
        {
            get { return oldConsinAddr; }
            set { oldConsinAddr = value; }
        }
        public String NewConsinAddr
        {
            get { return newConsinAddr; }
            set { newConsinAddr = value; }
        }
        public String OldEwayBillNo
        {
            get { return oldEwayBillNo; }
            set { oldEwayBillNo = value; }
        }
        public String NewEwayBillNo
        {
            get { return newEwayBillNo; }
            set { newEwayBillNo = value; }
        }
        public String StatusRemark
        {
            get { return statusRemark; }
            set { statusRemark = value; }
        }
        #endregion
    }
}
