using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblModuleCommHisTO
    {
        #region Declarations
        Int32 idModuleCommHis;
        Int32 currentModuleId;
        Int32 recentModuleId;
        DateTime inTime;
        DateTime outTime;
        Int32 userId;
        string loginIP;
        Int32 loginCount;
        string userLogin;

        Int32 loginId;
        string deviceId;






        #endregion

        #region Constructor
        public TblModuleCommHisTO()
        {
        }

     
        #endregion

        #region GetSet

   public int IdModuleCommHis { get => idModuleCommHis; set => idModuleCommHis = value; }
        public int CurrentModuleId { get => currentModuleId; set => currentModuleId = value; }
        public int RecentModuleId { get => recentModuleId; set => recentModuleId = value; }
        public DateTime InTime { get => inTime; set => inTime = value; }
        public DateTime OutTime { get => outTime; set => outTime = value; }
        public int UserId { get => userId; set => userId = value; }
        public String ModuleName{get;set;}
        public int LoginId{get;set;}
        public string LoginIP { get => loginIP; set => loginIP = value; }
        public int LoginCount { get => loginCount; set => loginCount = value; }
        public string UserLogin { get => userLogin; set => userLogin = value; }
        public string DeviceId { get => deviceId; set => deviceId = value; }
        

        #endregion
    }
}
