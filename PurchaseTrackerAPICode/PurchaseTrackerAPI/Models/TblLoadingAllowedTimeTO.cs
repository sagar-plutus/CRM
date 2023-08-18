using PurchaseTrackerAPI.StaticStuff;
using System;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingAllowedTimeTO
    {
        #region Declarations
        Int32 idLoadingTime;
        Int32 createdBy;
        DateTime allowedLoadingTime;
        DateTime createdOn;
        String extendedHrs;
        #endregion

        #region Constructor
        public TblLoadingAllowedTimeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingTime
        {
            get { return idLoadingTime; }
            set { idLoadingTime = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime AllowedLoadingTime
        {
            get { return allowedLoadingTime; }
            set { allowedLoadingTime = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String AllowedLoadingTimeStr
        {
            get { return allowedLoadingTime.ToString("HH: mm"); }
        }

        public String AllowedLoadingDateTimeStr
        {
            get { return allowedLoadingTime.ToString(Constants.DefaultDateFormat); }
        }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public String ExtendedHrs { get => extendedHrs; set => extendedHrs = value; }
        #endregion
    }
}
