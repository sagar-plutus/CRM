using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseBookingActionsTO
    {

        #region Declarations
        Int32 idBookingAction;
        Int32 isAuto;
        Int32 statusBy;
        DateTime statusDate;
        String bookingStatus;
        #endregion

        #region Constructor
        public TblPurchaseBookingActionsTO()
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
