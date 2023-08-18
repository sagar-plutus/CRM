using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblFeedbackTO
    {
        #region Declarations
        Int32 idFeedback;
        Int32 pageId;
        Int32 pageEleId;
        Int32 isAttended;
        Int32 createdBy;
        DateTime createdOn;
        Double rating;
        String description;
        String replyDesc;
        String createdByUserName;
        String pageName;
        String pageEleDesc;
        #endregion

        #region Constructor
        public TblFeedbackTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdFeedback
        {
            get { return idFeedback; }
            set { idFeedback = value; }
        }
        public Int32 PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }
        public Int32 PageEleId
        {
            get { return pageEleId; }
            set { pageEleId = value; }
        }
        public Int32 IsAttended
        {
            get { return isAttended; }
            set { isAttended = value; }
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
        public Double Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        public String Description
        {
            get { return description; }
            set { description = value; }
        }
        public String ReplyDesc
        {
            get { return replyDesc; }
            set { replyDesc = value; }
        }

        public string CreatedByUserName { get => createdByUserName; set => createdByUserName = value; }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public string PageName { get => pageName; set => pageName = value; }
        public string PageEleDesc { get => pageEleDesc; set => pageEleDesc = value; }
        #endregion
    }
}
