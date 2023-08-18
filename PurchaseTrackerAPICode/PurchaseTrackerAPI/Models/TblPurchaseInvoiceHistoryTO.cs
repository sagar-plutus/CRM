using PurchaseTrackerAPI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseInvoiceHistoryTO
    {
        #region Declarations
        Int32 statusId;
        Int32 createdBy;
        DateTime statusDate;
        DateTime createdOn;
        Int64 idPurchaseInvHistory;
        Int64 purchaseInvoiceId;
        String statusRemark;
        #endregion

        #region Constructor
        public TblPurchaseInvoiceHistoryTO()
        {
        }

        public TblPurchaseInvoiceHistoryTO(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, Icommondao _iCommonDAO)
        {
            if (tblPurchaseInvoiceTO.UpdatedBy > 0)
            {
                this.CreatedBy = tblPurchaseInvoiceTO.UpdatedBy;
                this.CreatedOn = tblPurchaseInvoiceTO.UpdatedOn;
            }
            else
            {
                this.CreatedBy = tblPurchaseInvoiceTO.CreatedBy;
                this.CreatedOn = tblPurchaseInvoiceTO.CreatedOn;
            }

            if (this.CreatedOn == new DateTime())
            {
                this.CreatedOn = _iCommonDAO.ServerDateTime;
            }

            this.StatusDate = this.CreatedOn;
            this.PurchaseInvoiceId = tblPurchaseInvoiceTO.IdInvoicePurchase;

            this.StatusId = tblPurchaseInvoiceTO.StatusId;
            this.StatusRemark = tblPurchaseInvoiceTO.Remark;

        }

        #endregion

        #region GetSet
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
        public Int64 IdPurchaseInvHistory
        {
            get { return idPurchaseInvHistory; }
            set { idPurchaseInvHistory = value; }
        }
        public Int64 PurchaseInvoiceId
        {
            get { return purchaseInvoiceId; }
            set { purchaseInvoiceId = value; }
        }
        public String StatusRemark
        {
            get { return statusRemark; }
            set { statusRemark = value; }
        }
        #endregion
    }
}
