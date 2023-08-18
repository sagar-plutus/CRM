using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblSmsTO
    {
        #region Declarations
        Int32 idSms;
        Int32 alertInstanceId;
        DateTime sentOn;
        String mobileNo;
        String smsTxt;
        String replyTxt;
        String sourceTxnDesc;
        #endregion

        #region Constructor
        public TblSmsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSms
        {
            get { return idSms; }
            set { idSms = value; }
        }
        public Int32 AlertInstanceId
        {
            get { return alertInstanceId; }
            set { alertInstanceId = value; }
        }
        public DateTime SentOn
        {
            get { return sentOn; }
            set { sentOn = value; }
        }
        public String MobileNo
        {
            get { return mobileNo; }
            set { mobileNo = value; }
        }
        public String SmsTxt
        {
            get { return smsTxt; }
            set { smsTxt = value; }
        }
        public String ReplyTxt
        {
            get { return replyTxt; }
            set { replyTxt = value; }
        }
        public String SourceTxnDesc
        {
            get { return sourceTxnDesc; }
            set { sourceTxnDesc = value; }
        }
        #endregion
    }
}
