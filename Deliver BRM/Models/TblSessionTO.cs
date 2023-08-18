using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblSessionTO
    {
        #region Declarations
        DateTime startTime;
        DateTime endTime;
        int idsession;
        int duration;
        int createUserId;
        int isActive;
        int isEndSession;
        String transactionType;
        int moduleId;
        Int32 refId;
        #endregion

        #region Constructor
        public TblSessionTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        public int Idsession
        {
            get { return idsession; }
            set { idsession = value; }
        }
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public int CreateUserId
        {
            get { return createUserId; }
            set { createUserId = value; }
        }
        public int IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public int IsEndSession { get => isEndSession; set => isEndSession = value; }
        public string TransactionType { get => transactionType; set => transactionType = value; }
        public int ModuleId { get => moduleId; set => moduleId = value; }
        public int RefId { get => refId; set => refId = value; }
        #endregion
    }
}
