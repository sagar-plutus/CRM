using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.StaticStuff
{
    public class NotificationConstants
    {

        #region Notifications
        public enum NotificationsE
        {
            NEW_RATE_AND_QUOTA_DECLARED = 1,
            BOOKING_APPROVAL_REQUIRED = 2,
            BOOKING_APPROVED_BY_DIRECTORS = 3,
            BOOKING_CONFIRMED = 4,
            BOOKING_REJECTED_BY_DIRECTORS = 5,
            BOOKINGS_CLOSED = 6,
            TODAYS_STOCK_CONFIRMED = 7,
            TODAYS_STOCK_AS_PER_ACCOUNTANT = 8,
            BOOKING_ACCEPTED_BY_CNF = 9,
            BOOKING_REJECTED_BY_CNF = 10,
            LOADING_SLIP_CONFIRMATION_REQUIRED = 11,
            LOADING_SLIP_CONFIRMED = 12,
            LOADING_SLIP_CANCELLED = 13,
            VEHICLE_OUT_FOR_DELIVERY = 14,
            LOADING_QUOTA_DECLARED = 15,
            LOADING_STOPPED = 16,
            STRAIGHT_TO_BEND_TRANSFER_REQUEST = 17,
            INVOICE_APPROVAL_REQUIRED = 18,
            INVOICE_APPROVED_BY_DIRECTOR = 19,
            INVOICE_REJECTED_BY_DIRECTOR = 20,
            INVOICE_ACCEPTED_BY_DISTRIBUTOR = 21,
            INVOICE_REJECTED_BY_DISTRIBUTOR = 22,
            INVOICE_ACCEPTANCE_REQUIRED = 23,
            SITE_VISTED = 24,

            //Priyanka [12-10-2018] : Added for vehicle statuses.
            VEHICLE_REPORTED_FOR_LOADING = 25,
            LOADING_VEHICLE_CLEARANCE_TO_SEND_IN = 26,
            LOADING_GATE_IN = 27,

            //Priyanka [18-04-2019]
            BOOKING_PENDING_FOR_CNF_APPROVAL = 28

        }

        #endregion

        public enum NotificationTypeE
        {
            ALERT = 1,
            EMAIL = 2,
            SMS = 3
        }
    }
}
