using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblOrgStructureTO
    {
        #region Declarations
        Int32 idOrgStructure;
        Int32 parentOrgStructureId;
        Int32 deptId;

        Int32 deptTypeId;
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
        String parentOrgDisplayId;
        String tempOrgStructId;
        Int16 isDept;
        String designationName;
        String departmentName;
        String positionName;
        Int32 levelId;
        Int32 reportingTypeId;
        String reportingName;
        String levelName;
        Int16 isEmptyPosition;
        Int16 isPosition;
        Int16 isNewAdded;
        //Int32 technicalParentId;
        Int16 isAddDept;
        Int32 roleId;
        Int32 roleTypeId;
        #endregion

        #region Constructor
        public TblOrgStructureTO()
        {

        }


        public TblOrgStructureTO Clone()
        {
            TblOrgStructureTO TblOrgStructureTO = new TblOrgStructureTO();

            TblOrgStructureTO.IdOrgStructure = this.IdOrgStructure;
            TblOrgStructureTO.ParentOrgStructureId = this.ParentOrgStructureId;
            TblOrgStructureTO.DeptId = this.DeptId;
            TblOrgStructureTO.deptTypeId = this.DeptTypeId;
            TblOrgStructureTO.DesignationId = this.DesignationId;
            TblOrgStructureTO.CreatedBy = this.CreatedBy;
            TblOrgStructureTO.UpdatedBy = this.UpdatedBy;
            TblOrgStructureTO.CreatedOn = this.CreatedOn;
            TblOrgStructureTO.UpdatedOn = this.UpdatedOn;
            TblOrgStructureTO.IsActive = this.IsActive;
            TblOrgStructureTO.OrgStructureDesc = this.OrgStructureDesc;
            TblOrgStructureTO.EmployeeName = this.EmployeeName;
            TblOrgStructureTO.EmployeeId = this.EmployeeId;
            TblOrgStructureTO.ActualOrgStructureId = this.ActualOrgStructureId;
            TblOrgStructureTO.ParentOrgDisplayId = this.ParentOrgDisplayId;
            TblOrgStructureTO.TempOrgStructId = this.TempOrgStructId;
            TblOrgStructureTO.IsDept = this.IsDept;
            TblOrgStructureTO.PositionName = this.PositionName;
            TblOrgStructureTO.LevelId = this.levelId;
            //TblOrgStructureTO.ReportingTypeId = this.reportingTypeId;
            TblOrgStructureTO.ReportingName = this.reportingName;
            TblOrgStructureTO.LevelName = this.levelName;
            // TblOrgStructureTO.TechnicalParentId = this.technicalParentId;
            return TblOrgStructureTO;
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
        public Int32 DeptTypeId
        {
            get { return deptTypeId; }
            set { deptTypeId = value; }
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
        public Int16 IsDept
        {
            get { return isDept; }
            set { isDept = value; }
        }

        public String OrgStructureDesc
        {
            get { return orgStructureDesc; }
            set { orgStructureDesc = value; }
        }

        public String ParentOrgDisplayId
        {
            get { return parentOrgDisplayId; }
            set { parentOrgDisplayId = value; }
        }

        public String TempOrgStructId
        {
            get { return tempOrgStructId; }
            set { tempOrgStructId = value; }
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

        public String DesignationName
        {
            get { return designationName; }
            set { designationName = value; }
        }
        public String DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; }
        }
        public String PositionName
        {
            get { return positionName; }
            set { positionName = value; }
        }
        public Int32 LevelId
        {
            get { return levelId; }
            set { levelId = value; }
        }
        public Int32 ReportingTypeId
        {
            get { return reportingTypeId; }
            set { reportingTypeId = value; }
        }
        public String ReportingName
        {
            get { return reportingName; }
            set { reportingName = value; }
        }
        public String LevelName
        {
            get { return levelName; }
            set { levelName = value; }
        }
        public Int32 RoleTypeId
        {
            get { return roleTypeId; }
            set { roleTypeId = value; }
        }

        public short IsEmptyPosition { get => isEmptyPosition; set => isEmptyPosition = value; }
        public short IsPosition { get => isPosition; set => isPosition = value; }
        public short IsNewAdded { get => isNewAdded; set => isNewAdded = value; }
        // public Int32 TechnicalParentId { get => technicalParentId; set => technicalParentId = value; }
        public short IsAddDept { get => isAddDept; set => isAddDept = value; }
        public int RoleId { get => roleId; set => roleId = value; }
        #endregion
    }
}
