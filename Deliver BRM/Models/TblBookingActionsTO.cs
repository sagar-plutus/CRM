using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblBookingActionsTO
    {
        #region Declarations
        Int32 idBookingAction;
        Int32 isAuto;
        Int32 statusBy;
        DateTime statusDate;
        String bookingStatus;
        #endregion

        #region Constructor
        public TblBookingActionsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBookingAction
        {
            get { return idBookingAction; }
            set { idBookingAction = value; }
        }
        public Int32 IsAuto
        {
            get { return isAuto; }
            set { isAuto = value; }
        }
        public Int32 StatusBy
        {
            get { return statusBy; }
            set { statusBy = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public String BookingStatus
        {
            get { return bookingStatus; }
            set { bookingStatus = value; }
        }
        #endregion
    }
}
