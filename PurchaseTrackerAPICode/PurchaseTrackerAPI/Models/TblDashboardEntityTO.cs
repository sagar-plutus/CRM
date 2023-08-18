using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblDashboardEntityTO
    {
        #region Declarations
        Int32 idDashboardEntity;
        Int32 moduleId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updateOn;
        String entityName;
        String entityValue;
        #endregion

        #region Constructor
        public TblDashboardEntityTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdDashboardEntity
        {
            get { return idDashboardEntity; }
            set { idDashboardEntity = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
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
        public DateTime UpdateOn
        {
            get { return updateOn; }
            set { updateOn = value; }
        }
        public String EntityName
        {
            get { return entityName; }
            set { entityName = value; }
        }
        public String EntityValue
        {
            get { return entityValue; }
            set { entityValue = value; }
        }
        #endregion
    }
}
