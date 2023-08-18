using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblConfigParamHistoryTO
    {
        #region Declarations
        Int32 idParamHistory;
        Int32 configParamId;
        Int32 createdBy;
        DateTime createdOn;
        String configParamName;
        String configParamOldVal;
        String configParamNewVal;
        #endregion

        #region Constructor
        public TblConfigParamHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdParamHistory
        {
            get { return idParamHistory; }
            set { idParamHistory = value; }
        }
        public Int32 ConfigParamId
        {
            get { return configParamId; }
            set { configParamId = value; }
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
        public String ConfigParamName
        {
            get { return configParamName; }
            set { configParamName = value; }
        }
        public String ConfigParamOldVal
        {
            get { return configParamOldVal; }
            set { configParamOldVal = value; }
        }
        public String ConfigParamNewVal
        {
            get { return configParamNewVal; }
            set { configParamNewVal = value; }
        }
        #endregion
    }
}
