using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblProdItemDescTO
    {
        #region Declarations
        Int32 idProdItemDesc;
        Int32 seqNo;
        Int32 itemId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String name;
        String description;
        #endregion

        #region Constructor
        public TblProdItemDescTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProdItemDesc
        {
            get { return idProdItemDesc; }
            set { idProdItemDesc = value; }
        }
        public Int32 SeqNo
        {
            get { return seqNo; }
            set { seqNo = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 ItemId
        {
            get { return itemId; }
            set { itemId = value; }
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
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Description
        {
            get { return description; }
            set { description = value; }
        }
        #endregion
    }
}
