using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseEnquiryHistoryTO
    {
        #region Declarations
        Int32 idPurchaseEnquiryHistory;
        Int32 idPurchaseEnquiry;
        Int32 createdBy;
        Int32 statusId;
        DateTime createdOn;
        Double globalRatePurchaseId;
        Double bookingQty;
        Double bookingRate;
        String comments;
        String statusName;

        double wtActualRate;
        #endregion

        #region Constructor
        public TblPurchaseEnquiryHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseEnquiryHistory
        {
            get { return idPurchaseEnquiryHistory; }
            set { idPurchaseEnquiryHistory = value; }
        }
        public Int32 IdPurchaseEnquiry
        {
            get { return idPurchaseEnquiry; }
            set { idPurchaseEnquiry = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double GlobalRatePurchaseId
        {
            get { return globalRatePurchaseId; }
            set { globalRatePurchaseId = value; }
        }
        public Double BookingQty
        {
            get { return bookingQty; }
            set { bookingQty = value; }
        }
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


        public double WtActualRate
        {
            get { return wtActualRate; }
            set { wtActualRate = value; }
        }


        public string StatusName { get => statusName; set => statusName = value; }


        #endregion
    }
}
