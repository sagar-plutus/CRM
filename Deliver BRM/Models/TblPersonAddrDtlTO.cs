using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblPersonAddrDtlTO
    {
        #region Declarations
        Int32 idPersonAddrDtl;
        Int32 personId;
        Int32 addressId;
        Int32 addressTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        #endregion

        #region Constructor
        public TblPersonAddrDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPersonAddrDtl
        {
            get { return idPersonAddrDtl; }
            set { idPersonAddrDtl = value; }
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
        public Int32 AddressTypeId
        {
            get { return addressTypeId; }
            set { addressTypeId = value; }
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
        #endregion
    }
}
