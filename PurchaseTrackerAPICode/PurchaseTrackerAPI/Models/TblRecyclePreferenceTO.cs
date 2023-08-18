using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblRecyclePreferenceTO
    {
        #region Declarations
        Int32 idPreference;
        String settingKey;
        String settingValue;
        #endregion

        #region Constructor
        public TblRecyclePreferenceTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPreference
        {
            get { return idPreference; }
            set { idPreference = value; }
        }
        public String SettingKey
        {
            get { return settingKey; }
            set { settingKey = value; }
        }
        public String SettingValue
        {
            get { return settingValue; }
            set { settingValue = value; }
        }
        #endregion
    }
}
