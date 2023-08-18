using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.StaticStuff;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblGlobalRatePurchaseTO
    {

        #region Declarations
        Int32 idGlobalRatePurchase;
        Int32 createdBy;
        DateTime createdOn;
        Double rate;
        String comments;
        Int32 rateReasonId;
        Int32 ratebandcosting;
        String rateReasonDesc;
        Double totalBookingQty;
        Double avgBookingRate;
        List<TblRateBandDeclarationPurchaseTO> rateBandDeclarationPurchaseTOList;
        #endregion

        #region Constructor
        public TblGlobalRatePurchaseTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGlobalRatePurchase
        {
            get { return idGlobalRatePurchase; }
            set { idGlobalRatePurchase = value; }
        }

        public Int32 Ratebandcosting
        {
            get { return ratebandcosting; }
            set { ratebandcosting = value; }
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
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public int RateReasonId
        {
            get { return rateReasonId; }
            set { rateReasonId = value; }
        }

        public String RateReasonDesc
        {
            get { return rateReasonDesc; }
            set { rateReasonDesc = value; }
        }
        //public String CreatedOnStr
        //{
        //    get { return createdOn.ToString(Constants.DefaultDateFormat); }
        //}
        
        public List<TblRateBandDeclarationPurchaseTO> RateBandDeclarationPurchaseTOList { get => rateBandDeclarationPurchaseTOList; set => rateBandDeclarationPurchaseTOList = value; }
        public double TotalBookingQty { get => totalBookingQty; set => totalBookingQty = value; }
        public double AvgBookingRate { get => avgBookingRate; set => avgBookingRate = value; }
        #endregion
    }
}
