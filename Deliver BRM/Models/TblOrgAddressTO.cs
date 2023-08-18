using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblOrgAddressTO
    {
        #region Declarations
        Int32 idOrgAddr;
        Int32 organizationId;
        Int32 addrTypeId;
        Int32 addressId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        #endregion

        #region Constructor
        public TblOrgAddressTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOrgAddr
        {
            get { return idOrgAddr; }
            set { idOrgAddr = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 AddrTypeId
        {
            get { return addrTypeId; }
            set { addrTypeId = value; }
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
