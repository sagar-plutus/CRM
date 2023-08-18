using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseInvoiceDocumentsTO
    {
        #region Declarations
        Int32 documentTypeId;
        Int32 documentId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        Int64 idPurchaseInvDocument;
        Int64 purchaseInvoiceId;
        String documentTypeValue;
        Int32 isDocAttach;
        Int32 masterValueId;
        Int32 masterId;
        Int32 isFromMaster;

        string masterValueDesc;
        #endregion

        #region Constructor
        public TblPurchaseInvoiceDocumentsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 DocumentTypeId
        {
            get { return documentTypeId; }
            set { documentTypeId = value; }
        }
        public Int32 DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
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
        public Int64 IdPurchaseInvDocument
        {
            get { return idPurchaseInvDocument; }
            set { idPurchaseInvDocument = value; }
        }
        public Int64 PurchaseInvoiceId
        {
            get { return purchaseInvoiceId; }
            set { purchaseInvoiceId = value; }
        }
        public String DocumentTypeValue
        {
            get { return documentTypeValue; }
            set { documentTypeValue = value; }
        }

        public int IsDocAttach { get => isDocAttach; set => isDocAttach = value; }
        public int MasterValueId { get => masterValueId; set => masterValueId = value; }
        public int MasterId { get => masterId; set => masterId = value; }
        public int IsFromMaster { get => isFromMaster; set => isFromMaster = value; }
        public string MasterValueDesc { get => masterValueDesc; set => masterValueDesc = value; }
        #endregion
    }
}
