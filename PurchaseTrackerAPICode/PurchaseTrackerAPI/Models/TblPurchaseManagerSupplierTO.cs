using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseManagerSupplierTO
    {
        #region Declarations
        Int32 idPurchaseManagerSupplier;
        Int32 userId;
        Int32 organizationId;
        Int32 isActive;
        Int32 createdBy;
        DateTime createdOn;
        //String roleDesc;
        //Int32 enableAreaAlloc;
        String supplierName;

        Boolean isChecked;
        #endregion

        #region Constructor
        public TblPurchaseManagerSupplierTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseManagerSupplier
        {
            get { return idPurchaseManagerSupplier; }
            set { idPurchaseManagerSupplier = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
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
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        public Boolean IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
        //public String RoleDesc
        //{
        //    get { return roleDesc; }
        //    set { roleDesc = value; }
        //}

        //public int EnableAreaAlloc { get => enableAreaAlloc; set => enableAreaAlloc = value; }
        #endregion
    }
}
