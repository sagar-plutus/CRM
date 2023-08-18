using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class DimMstDeptTO
    {
        #region Declarations
        Int32 idDept;
        Int32 parentDeptId;
        Int32 deptTypeId;
        Int32 orgUnitId;
        Int32 isVisible;
        String deptCode;
        String deptDisplayName;
        String deptDesc;
        #endregion

        #region Constructor
        public DimMstDeptTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdDept
        {
            get { return idDept; }
            set { idDept = value; }
        }
        public Int32 ParentDeptId
        {
            get { return parentDeptId; }
            set { parentDeptId = value; }
        }
        public Int32 DeptTypeId
        {
            get { return deptTypeId; }
            set { deptTypeId = value; }
        }
        public Int32 OrgUnitId
        {
            get { return orgUnitId; }
            set { orgUnitId = value; }
        }
        public Int32 IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        public String DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }
        public String DeptDisplayName
        {
            get { return deptDisplayName; }
            set { deptDisplayName = value; }
        }
        public String DeptDesc
        {
            get { return deptDesc; }
            set { deptDesc = value; }
        }
        #endregion
    }
}
