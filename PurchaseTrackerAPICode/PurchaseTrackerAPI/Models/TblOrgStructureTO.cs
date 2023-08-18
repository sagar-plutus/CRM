using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblOrgStructureTO
    {
        #region Declarations
        Int32 idOrgStructure;
        Int32 parentOrgStructureId;
        Int32 deptId;
        Int32 designationId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int16 isActive;
        String orgStructureDesc;
        String employeeName;
        Int32 employeeId;
        Int32 actualOrgStructureId;
        #endregion

        #region Constructor
        public TblOrgStructureTO()
        {

        }

        #endregion

        #region GetSet
        public Int32 IdOrgStructure
        {
            get { return idOrgStructure; }
            set { idOrgStructure = value; }
        }
        public Int32 ParentOrgStructureId
        {
            get { return parentOrgStructureId; }
            set { parentOrgStructureId = value; }
        }
        public Int32 DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }
        public Int32 DesignationId
        {
            get { return designationId; }
            set { designationId = value; }
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
        public Int16 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String OrgStructureDesc
        {
            get { return orgStructureDesc; }
            set { orgStructureDesc = value; }
        }

        public String EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        public Int32 EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

        public Int32 ActualOrgStructureId
        {
            get { return actualOrgStructureId; }
            set { actualOrgStructureId = value; }
        }
        #endregion
    }
}
