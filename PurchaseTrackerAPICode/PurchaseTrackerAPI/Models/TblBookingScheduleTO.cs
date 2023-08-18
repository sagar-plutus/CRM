using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblBookingScheduleTO
    {
        #region Declarations
        Int32 idSchedule;
        Int32 bookingId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime scheduleDate;
        DateTime createdOn;
        DateTime updatedOn;
        Double qty;
        String remark;
        List<TblBookingDelAddrTO> deliveryAddressLst;
        List<TblBookingExtTO> orderDetailsLst;

        List<TblLoadingSlipExtTO> loadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
        String scheduleDateStr;
        Double balanceQty;
        #endregion

        #region Constructor
        public TblBookingScheduleTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSchedule
        {
            get { return idSchedule; }
            set { idSchedule = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// [2017-14-12] Vijaymala :Added To Get data In Post action
        /// </summary>
        public List<TblBookingDelAddrTO> DeliveryAddressLst
        {
            get
            {
                return deliveryAddressLst;
            }

            set
            {
                deliveryAddressLst = value;
            }
        }




        /// <summary>
        /// [2017-14-12] Vijaymala :Added To Get data In Post action
        /// </summary>
        public List<TblBookingExtTO> OrderDetailsLst
        {
            get
            {
                return orderDetailsLst;
            }

            set
            {
                orderDetailsLst = value;
            }
        }

        public List<TblLoadingSlipExtTO> LoadingSlipExtTOList { get => loadingSlipExtTOList; set => loadingSlipExtTOList = value; }
        public string ScheduleDateStr { get => scheduleDateStr; set => scheduleDateStr = value; }
        public double BalanceQty { get => balanceQty; set => balanceQty = value; }
        #endregion
    }
}
