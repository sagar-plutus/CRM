using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblLoginTO
    {
        #region Declarations
        Int32 idLogin;
        Int32 userId;
        DateTime loginDate;
        DateTime logoutDate;
        String loginIP;
        String deviceId;
        String latitude;
        String longitude;
        String browserName;
        String platformDetails;
        String apkVersion;
        String userName;
        #endregion

        #region Constructor
        public TblLoginTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLogin
        {
            get { return idLogin; }
            set { idLogin = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public DateTime LoginDate
        {
            get { return loginDate; }
            set { loginDate = value; }
        }
        public DateTime LogoutDate
        {
            get { return logoutDate; }
            set { logoutDate = value; }
        }
        public String LoginIP
        {
            get { return loginIP; }
            set { loginIP = value; }
        }

        public string DeviceId { get => deviceId; set => deviceId = value; }
        public string Latitude { get => latitude; set => latitude = value; }
        public string Longitude { get => longitude; set => longitude = value; }
        public string BrowserName { get => browserName; set => browserName = value; }
        public string PlatformDetails { get => platformDetails; set => platformDetails = value; }
        public string ApkVersion { get => apkVersion; set => apkVersion = value; }
        public string UserName { get => userName; set => userName = value; }
        #endregion
    }
    public class dimUserConfigrationTO
    {
        Int32 idUserConfigration;
        String configDesc;
        String configValue;

        public int IdUserConfigration { get => idUserConfigration; set => idUserConfigration = value; }
        public string ConfigDesc { get => configDesc; set => configDesc = value; }
        public string ConfigValue { get => configValue; set => configValue = value; }
    }
}
