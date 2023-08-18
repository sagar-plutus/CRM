using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblVisitPurposeTO
    {
        #region Declarations
        Int32 idVisitPurpose;
        Int32 visitTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime createdOn;
        DateTime updatedOn;
        String visitPurposeDesc;
        #endregion

        #region Constructor
        public TblVisitPurposeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVisitPurpose
        {
            get { return idVisitPurpose; }
            set { idVisitPurpose = value; }
        }
        public Int32 VisitTypeId
        {
            get { return visitTypeId; }
            set { visitTypeId = value; }
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
        public String VisitPurposeDesc
        {
            get { return visitPurposeDesc; }
            set { visitPurposeDesc = value; }
        }
        #endregion
    }
}
