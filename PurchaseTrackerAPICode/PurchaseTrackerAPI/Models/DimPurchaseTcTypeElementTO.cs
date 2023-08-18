using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class DimPurchaseTcTypeElementTO
    {
        #region Declarations
        Int32 idPurchasseTcTypeElement;
        Int32 tcTypeId;
        Int32 tcElementId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String tcTypeName;
        String tcElementName;
        #endregion

        #region Constructor
        public DimPurchaseTcTypeElementTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchasseTcTypeElement
        {
            get { return idPurchasseTcTypeElement; }
            set { idPurchasseTcTypeElement = value; }
        }
        public Int32 TcTypeId
        {
            get { return tcTypeId; }
            set { tcTypeId = value; }
        }
        public Int32 TcElementId
        {
            get { return tcElementId; }
            set { tcElementId = value; }
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
        public String TcTypeName
        {
            get { return tcTypeName; }
            set { tcTypeName = value; }
        }

        public String TcElementName
        {
            get { return tcElementName; }
            set { tcElementName = value; }
        }

      
        #endregion
    }
}
