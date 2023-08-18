using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TempInvoiceDocumentDetailsTO
    {
        #region Declarations
        Int32 idInvoiceDocument;
        Int32 invoiceId;
        Int32 documentId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String documentDesc;
        String path;
        Int32 isActive;
        #endregion

        #region Constructor
        public TempInvoiceDocumentDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdInvoiceDocument
        {
            get { return idInvoiceDocument; }
            set { idInvoiceDocument = value; }
        }
        public Int32 InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
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

        public string DocumentDesc { get => documentDesc; set => documentDesc = value; }
        public string Path { get => path; set => path = value; }
        public String CreatedOnStr
        {
            get { return CreatedOn.ToString("dd-MM-yyyy"); }
        }

        public int IsActive { get => isActive; set => isActive = value; }
        #endregion
    }
}
