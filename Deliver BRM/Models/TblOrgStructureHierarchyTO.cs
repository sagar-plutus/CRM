using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{

    public class TblOrgStructureHierarchyTO
    {
        #region Declarations
        Int32 idOrgHierarchy;
        Int32 orgStructureId;
        Int32 parentOrgStructId;
        Int32 reportingTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 isActive;
        #endregion

        #region Constructor
        public TblOrgStructureHierarchyTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOrgHierarchy
        {
            get { return idOrgHierarchy; }
            set { idOrgHierarchy = value; }
        }
        public Int32 OrgStructureId
        {
            get { return orgStructureId; }
            set { orgStructureId = value; }
        }
        public Int32 ParentOrgStructId
        {
            get { return parentOrgStructId; }
            set { parentOrgStructId = value; }
        }
        public Int32 ReportingTypeId
        {
            get { return reportingTypeId; }
            set { reportingTypeId = value; }
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

        public int IsActive { get => isActive; set => isActive = value; }
        #endregion
    }
}
