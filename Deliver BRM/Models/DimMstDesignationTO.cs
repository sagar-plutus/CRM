using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class DimMstDesignationTO
    {
        #region Declarations
        Int32 idDesignation;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 noticePeriodInMonth;
        Int32 isVisible;
        DateTime createdOn;
        DateTime updatedOn;
        String designationDesc;
        String remark;
        Int32 deactivatedBy;
        DateTime deactivatedOn;
        #endregion

        #region Constructor
        public DimMstDesignationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdDesignation
        {
            get { return idDesignation; }
            set { idDesignation = value; }
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
        public Int32 NoticePeriodInMonth
        {
            get { return noticePeriodInMonth; }
            set { noticePeriodInMonth = value; }
        }
        public Int32 IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
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
        public String DesignationDesc
        {
            get { return designationDesc; }
            set { designationDesc = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public Int32 DeactivatedBy
        {
            get { return deactivatedBy; }
            set { deactivatedBy = value; }
        }
        public DateTime DeactivatedOn
        {
            get { return deactivatedOn; }
            set { deactivatedOn = value; }
        }
        #endregion
    }
}
