using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
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
        Int32 loadingLayerId;
        String loadingLayerDesc;
        Int32 noOfLayers;
        Int32 scheduleGroupId;
        Int32 isItemized;
        Boolean isUpdated;
        string cnfName;
        string dealerName;
        string bookingDisplayNo;
        Boolean isDisplay;
        Int32 dealerOrgId;

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

        
        public Int32 DealerOrgId
        {
            get { return dealerOrgId; }
            set { dealerOrgId = value; }
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
        public int LoadingLayerId { get => loadingLayerId; set => loadingLayerId = value; }
        public string LoadingLayerDesc { get => loadingLayerDesc; set => loadingLayerDesc = value; }
        public int NoOfLayers { get => noOfLayers; set => noOfLayers = value; }
        public int ScheduleGroupId { get => scheduleGroupId; set => scheduleGroupId = value; }
        public int IsItemized { get => isItemized; set => isItemized = value; }

        //AmolG[2020-Feb-25] For checking update
        public bool IsUpdated { get => isUpdated; set => isUpdated = value; }

        public string CnfName { get => cnfName; set => cnfName = value; }
        public string DealerName { get => dealerName; set => dealerName = value; }

        public string BookingDisplayNo { get => bookingDisplayNo; set => bookingDisplayNo = value; }

        public Boolean IsDisplay { get => isDisplay; set => isDisplay = value; }

        

        #endregion
    }
}
