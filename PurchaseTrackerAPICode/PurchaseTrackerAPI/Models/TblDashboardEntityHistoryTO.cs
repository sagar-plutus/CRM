using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblDashboardEntityHistoryTO
    {
        #region Declarations
        Int32 idDashboardEntityHistoryId;
        Int32 dashboardEntityId;
        Int32 moduleId;
        Int32 createdBy;
        DateTime createdOn;
        String entityName;
        String entityValue;
        #endregion

        #region Constructor
        public TblDashboardEntityHistoryTO()
        {
        }

        #endregion

        #region GetSet

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        public Int32 IdDashboardEntityHistoryId
        {
            get { return idDashboardEntityHistoryId; }
            set { idDashboardEntityHistoryId = value; }
        }
        public Int32 DashboardEntityId
        {
            get { return dashboardEntityId; }
            set { dashboardEntityId = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
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
