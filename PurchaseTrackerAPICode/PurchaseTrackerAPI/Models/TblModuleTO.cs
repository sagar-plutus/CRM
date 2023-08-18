using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
     public class TblModuleTO
    {
        #region Declarations
        Int32 idModule;
        DateTime createdOn;
        String moduleName;
        String moduleDesc;

        string navigateUrl;
        int isActive;
        string logoUrl;
        Int32 sysElementId;
        string androidUrl;
        int isSubscribe; // Sudhir[30-08-2018] Added for Subscribe or Not.
        string containerName;
        int isExternal;
        int iotconfigSetting;
        int isSAPEnabled;

        #endregion

        #region Constructor
        public TblModuleTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdModule
        {
            get { return idModule; }
            set { idModule = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }
        public String ModuleDesc
        {
            get { return moduleDesc; }
            set { moduleDesc = value; }
        }

        public string NavigateUrl
        {
            get { return navigateUrl; }
            set { navigateUrl = value; }
        }

        public int IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public string LogoUrl
        {
            get { return logoUrl; }
            set { logoUrl = value; }
        }

        public int SysElementId { get => sysElementId; set => sysElementId = value; }
        public string AndroidUrl { get => androidUrl; set => androidUrl = value; }
        public int IsSubscribe { get => isSubscribe; set => isSubscribe = value; }
        public string ContainerName { get => containerName; set => containerName = value; }
        public int IsExternal { get => isExternal; set => isExternal = value; }
        public int IotconfigSetting { get => iotconfigSetting; set => iotconfigSetting = value; }

        public int IsSAPEnabled { get => isSAPEnabled; set => isSAPEnabled = value; }

        
        #endregion
    }
}
