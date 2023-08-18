using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblBookingsTO
    {
        #region Declarations
        Int32 idBooking;
        Int32 cnFOrgId;
        Int32 dealerOrgId;
        Int32 deliveryDays;
        Int32 noOfDeliveries;
        Int32 isConfirmed;
        Int32 isJointDelivery;
        Int32 isSpecialRequirement;
        Double cdStructure;
        Int32 statusId;
        Int32 isWithinQuotaLimit;
        Int32 globalRateId;
        Int32 quotaDeclarationId;
        Int32 quotaQtyBforBooking;
        Int32 quotaQtyAftBooking;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime bookingDatetime;
        DateTime statusDate;
        DateTime updatedOn;
        Double bookingQty;
        Double bookingRate;
        String comments;

        String cnfName;
        String dealerName;

        List<TblBookingDelAddrTO> deliveryAddressLst;
        List<TblBookingExtTO> orderDetailsLst;
        String status;
        Double pendingQty;
        Double loadingQty;

        Int32 isDeleted;
        String authReasons;
        Int32 cdStructureId;
        Int32 parityId;
        Double orcAmt;
        String orcMeasure;
        String billingName;
        String poNo;
        String statusRemark;
        Int32 bookingRefId;
        String bookingDisplayNo;
        List<TblBookingScheduleTO> bookingScheduleTOLst;

        Double sizesQty;
        Boolean isResetScheduledQty;
        String dealerVillageName;
        String dealerTalukaName;
        String dealerDistrictName;
        String dealerStateName;

        #endregion

        #region Constructor
        public TblBookingsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdBooking
        {
            get { return idBooking; }
            set { idBooking = value; }
        }
        public Int32 CnFOrgId
        {
            get { return cnFOrgId; }
            set { cnFOrgId = value; }
        }
        public Int32 DealerOrgId
        {
            get { return dealerOrgId; }
            set { dealerOrgId = value; }
        }
        public Int32 DeliveryDays
        {
            get { return deliveryDays; }
            set { deliveryDays = value; }
        }
        public Int32 NoOfDeliveries
        {
            get { return noOfDeliveries; }
            set { noOfDeliveries = value; }
        }
        public Int32 IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
        }
        public Int32 IsJointDelivery
        {
            get { return isJointDelivery; }
            set { isJointDelivery = value; }
        }
        public Int32 IsSpecialRequirement
        {
            get { return isSpecialRequirement; }
            set { isSpecialRequirement = value; }
        }
        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 IsWithinQuotaLimit
        {
            get { return isWithinQuotaLimit; }
            set { isWithinQuotaLimit = value; }
        }
        public Int32 GlobalRateId
        {
            get { return globalRateId; }
            set { globalRateId = value; }
        }
        public Int32 QuotaDeclarationId
        {
            get { return quotaDeclarationId; }
            set { quotaDeclarationId = value; }
        }
        public Int32 QuotaQtyBforBooking
        {
            get { return quotaQtyBforBooking; }
            set { quotaQtyBforBooking = value; }
        }
        public Int32 QuotaQtyAftBooking
        {
            get { return quotaQtyAftBooking; }
            set { quotaQtyAftBooking = value; }
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
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime BookingDatetime
        {
            get { return bookingDatetime; }
            set { bookingDatetime = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double BookingQty
        {
            get { return bookingQty; }
            set { bookingQty = value; }
        }
        public Double BulkBookingQty { get; set; }
        public Double BookingRate
        {
            get { return bookingRate; }
            set { bookingRate = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public String CnfName
        {
            get { return cnfName; }
            set { cnfName = value; }
        }

        public String DealerName
        {
            get { return dealerName; }
            set { dealerName = value; }
        }

        public String Status
        {
            get { return status; }
            set { status = value; }
        }


        /// <summary>
        /// Sanjay [2017-02-17] This is pending qty for loading
        /// </summary>
        public Double PendingQty
        {
            get { return pendingQty; }
            set { pendingQty = value; }
        }

        public Double LoadingQty
        {
            get { return loadingQty; }
            set { loadingQty = value; }
        }

        public Int32 IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }
        /// <summary>
        /// Sanjay [2017-02-11] To Get data In Post action
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
        /// Sanjay [2017-02-11] To Get data In Post action
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

        public Constants.TranStatusE TranStatusE
        {
            get
            {
                TranStatusE tranStatusE = TranStatusE.BOOKING_NEW;
                if (Enum.IsDefined(typeof(TranStatusE), statusId))
                {
                    tranStatusE = (TranStatusE)statusId;
                }
                return tranStatusE;

            }
            set
            {
                statusId = (int)value;
            }
        }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public String StatusDateStr
        {
            get { return statusDate.ToString(Constants.DefaultDateFormat); }
        }
        public String UpdatedOnStr
        {
            get { return updatedOn.ToString(Constants.DefaultDateFormat); }
        }

        public string AuthReasons
        {
            get
            {
                return authReasons;
            }

            set
            {
                authReasons = value;
            }
        }

        public int CdStructureId
        {
            get
            {
                return cdStructureId;
            }

            set
            {
                cdStructureId = value;
            }
        }
    
        public Boolean IsResetScheduledQty
        {
            get { return isResetScheduledQty; }
            set { isResetScheduledQty = value; }
        }

        /// <summary>
        /// Sanjay [2017-04-21] added after alpha release
        /// after discussion with Customer (Nitin Kabra)
        /// New Requirement
        /// </summary>
        public int ParityId
        {
            get
            {
                return parityId;
            }

            set
            {
                parityId = value;
            }
        }
        public List<TblBookingScheduleTO> BookingScheduleTOLst
        {
            get
            {
                return bookingScheduleTOLst;
            }

            set
            {
                bookingScheduleTOLst = value;
            }
        }

        public double OrcAmt { get => orcAmt; set => orcAmt = value; }
        public string OrcMeasure { get => orcMeasure; set => orcMeasure = value; }
        public string BillingName { get => billingName; set => billingName = value; }
        public string PoNo { get => poNo; set => poNo = value; }
        public string StatusRemark { get => statusRemark; set => statusRemark = value; }
        public int BookingRefId { get => bookingRefId; set => bookingRefId = value; }
        public string BookingDisplayNo { get => bookingDisplayNo; set => bookingDisplayNo = value; }
        public Double SizesQty
        {
            get { return sizesQty; }
            set { sizesQty = value; }
        }

        public Int32 LoadingSlipId { get; set; }
        public Int32 LoadingId { get; set; }
        public double LoadingSlipQty { get; set; }
        public string DealerVillageName { get => dealerVillageName; set => dealerVillageName = value; }
        public string DealerTalukaName { get => dealerTalukaName; set => dealerTalukaName = value; }
        public string DealerDistrictName { get => dealerDistrictName; set => dealerDistrictName = value; }
        public string DealerStateName { get => dealerStateName; set => dealerStateName = value; }
        public int IsCreateNewBooking { get; set; }
        public int BookingTypeId { get; set; }
        public string BookingTypeName { get; set; }
        #endregion

        #region Methods

        public TblBookingBeyondQuotaTO GetBookingBeyondQuotaTO()
        {
            TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO = new Models.TblBookingBeyondQuotaTO();
            tblBookingBeyondQuotaTO.BookingId = this.idBooking;
            tblBookingBeyondQuotaTO.DeliveryPeriod = this.deliveryDays;
            tblBookingBeyondQuotaTO.Quantity = this.bookingQty;
            tblBookingBeyondQuotaTO.CdStructure = this.cdStructure;
            tblBookingBeyondQuotaTO.CdStructureId = this.cdStructureId;
            tblBookingBeyondQuotaTO.OrcAmt = this.orcAmt;
            tblBookingBeyondQuotaTO.Quantity = this.bookingQty;
            tblBookingBeyondQuotaTO.Rate= this.bookingRate;
            tblBookingBeyondQuotaTO.StatusId= this.statusId;
            tblBookingBeyondQuotaTO.StatusDate = this.statusDate;
            return tblBookingBeyondQuotaTO;
        }
        #endregion
    }
}
