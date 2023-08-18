using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
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
        #endregion
    }
}
