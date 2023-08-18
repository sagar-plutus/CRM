using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblUserExtTO
    {
        #region Declarations
        Int32 userId;
        Int32 personId;
        Int32 addressId;
        Int32 createdBy;
        DateTime createdOn;
        String comments;
        Int32 organizationId;
        #endregion

        #region Constructor
        public TblUserExtTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        public Int32 AddressId
        {
            get { return addressId; }
            set { addressId = value; }
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
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        
        #endregion
    }
}
