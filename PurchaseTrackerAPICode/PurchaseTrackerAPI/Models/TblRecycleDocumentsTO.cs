
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace PurchaseTrackerAPI.Models
    {
        public class TblRecycleDocumentTO
        {
            #region Declarations
            Int32 idRecycleDocument;
            Int32 txnId;

            string documentIds;

            Int32 txnTypeId;
            Int32 documentId;
            Int32 createdBy;
            Int32 updatedBy;
            DateTime createdOn;
            DateTime updatedOn;

            int isActive;
            #endregion

            #region Constructor
            public TblRecycleDocumentTO()
            {
            }

            #endregion

            #region GetSet
            public Int32 IdRecycleDocument
            {
                get { return idRecycleDocument; }
                set { idRecycleDocument = value; }
            }
            public Int32 IsActive
            {
                get { return isActive; }
                set { isActive = value; }
            }
            public Int32 TxnId
            {
                get { return txnId; }
                set { txnId = value; }
            }
            public Int32 TxnTypeId
            {
                get { return txnTypeId; }
                set { txnTypeId = value; }
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
            public string DocumentIds
            {
                get { return documentIds; }
                set { documentIds = value; }
            }
            #endregion
        }
    }

