using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblConfigParamsTO
    {
        #region Declarations
        Int32 idConfigParam;
        Int32 configParamValType;
        DateTime createdOn;
        String configParamName;
        String configParamVal;
        Int32 isActive;
        string configParamDisplayVal;
        Int32 moduleId;
        string configDevDesc;
        #endregion

        #region Constructor
        public TblConfigParamsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdConfigParam
        {
            get { return idConfigParam; }
            set { idConfigParam = value; }
        }
        public Int32 ConfigParamValType
        {
            get { return configParamValType; }
            set { configParamValType = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String ConfigParamName
        {
            get { return configParamName; }
            set { configParamName = value; }
        }
        public String ConfigParamVal
        {
            get { return configParamVal; }
            set { configParamVal = value; }
        }
         public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public string ConfigParamDisplayVal
        {
            get { return configParamDisplayVal; }
            set { configParamDisplayVal = value; }
        }
        public string ConfigDevDesc
        {
            get { return configDevDesc; }
            set { configDevDesc = value; }
        }
        #endregion
    }
}
