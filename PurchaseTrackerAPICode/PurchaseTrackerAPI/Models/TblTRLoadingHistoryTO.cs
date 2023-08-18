using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models 
{
    public class TblTRLoadingHistoryTO
    {
        #region Declarations
        Int32 idLoadingHistory;
        Int32 loadingId;
        Int32 statusId;
        Int32 statusBy;
        DateTime statusOn;
        #endregion

        #region Constructor
        public TblTRLoadingHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingHistory
        {
            get { return idLoadingHistory; }
            set { idLoadingHistory = value; }
        }
        public Int32 LoadingId
        {
            get { return loadingId; }
            set { loadingId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 StatusBy
        {
            get { return statusBy; }
            set { statusBy = value; }
        }
        public DateTime StatusOn
        {
            get { return statusOn; }
            set { statusOn = value; }
        }
        #endregion
    }
}
