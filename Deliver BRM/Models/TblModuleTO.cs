using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
     public class TblModuleTO
    {
        #region Declarations
        Int32 idModule;
        Int32 isLogin;
        DateTime createdOn;
        Int32 isImpPerson;
        String moduleName;
        String moduleDesc;
Int32 impPersonCount;
        string navigateUrl;
        int isActive;
        string logoUrl;
        Int32 sysElementId;
        string androidUrl;
        int isSubscribe;
        string containerName;
        int isExternal;

        int noOfAllowedLicenseCnt;
        int noOfConfiguredLicenseCnt;
        int noOfActiveLicenseCnt;
        Boolean isSubscribeBit;
        int iotconfigSetting;

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
        public string ContainerName { get => containerName; set => containerName = value; } //Sudhir[11-OCT-2018] Added for Azure Container Name.
        public int IsExternal { get => isExternal; set => isExternal = value; }

        /// <summary>
        /// Sanjay [25-Feb-2019] User Subscription Management
        /// </summary>
        public int NoOfAllowedLicenseCnt { get => noOfAllowedLicenseCnt; set => noOfAllowedLicenseCnt = value; }

        /// <summary>
        /// Sanjay [25-Feb-2019] User Subscription Management
        /// </summary>
        public int NoOfConfiguredLicenseCnt { get => noOfConfiguredLicenseCnt; set => noOfConfiguredLicenseCnt = value; }

        /// <summary>
        /// Sanjay [25-Feb-2019] User Subscription Management
        /// </summary>
        public int NoOfActiveLicenseCnt { get => noOfActiveLicenseCnt; set => noOfActiveLicenseCnt = value; }
        public bool IsSubscribeBit { get => isSubscribeBit; set => isSubscribeBit = value; }
        public int IsImpPerson { get => isImpPerson; set => isImpPerson = value; }
        public int ImpPersonCount { get => impPersonCount; set => impPersonCount = value; }
        public int IsLogin { get => isLogin; set => isLogin = value; }
        public int IotconfigSetting { get => iotconfigSetting; set => iotconfigSetting = value; }
        public int ModeId { get; set; }
        #endregion
    }
}
