using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblOrgPersonDtlsTO
    {
        #region Declarations
        Int32 idOrgPersonDtl;
        Int32 personId;
        Int32 organizationId;
        Int32 personTypeId;
        Int32 createdBy;
        Int32 isActive;
        DateTime createdOn;
        #endregion

        #region Constructor
        public TblOrgPersonDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOrgPersonDtl
        {
            get { return idOrgPersonDtl; }
            set { idOrgPersonDtl = value; }
        }
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public Int32 PersonTypeId
        {
            get { return personTypeId; }
            set { personTypeId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
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
        #endregion
    }
}
