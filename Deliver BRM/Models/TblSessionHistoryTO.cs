using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblSessionHistoryTO
    {
        #region Declarations
        Int32 idSessionHistory;
        Int32 sessionId;
        Int32 converionMediaType;
        String conversionBody;
        String otherDesc;
        int isEdit;
        int conversionUserId;
        int senderUserId;
        String sendOn;
        int isSendNotification;
        #endregion

        #region Constructor
        public TblSessionHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSessionHistory
        {
            get { return idSessionHistory; }
            set { idSessionHistory = value; }
        }
        public Int32 SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
        public Int32 ConverionMediaType
        {
            get { return converionMediaType; }
            set { converionMediaType = value; }
        }
        public String ConversionBody
        {
            get { return conversionBody; }
            set { conversionBody = value; }
        }
        public String OtherDesc
        {
            get { return otherDesc; }
            set { otherDesc = value; }
        }

        public Int32 IsEdit { get => isEdit; set => isEdit = value; }
        public int ConversionUserId { get => conversionUserId; set => conversionUserId = value; }
        public int SenderUserId { get => senderUserId; set => senderUserId = value; }
        public String SendOn { get => sendOn; set => sendOn = value; }
        public int IsSendNotification { get => isSendNotification; set => isSendNotification = value; }
        #endregion
    }
}
