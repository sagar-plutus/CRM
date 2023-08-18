using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseBookingBeyondQuotaTO
    {

        #region Declarations

        Int32 idBookingAuth;
        Int32 bookingId;
        Int32 statusId;
        DateTime statusDate;
        Double rate;
        Double quantity;
        Double deliveryPeriod;
        Int32 createdBy;
        DateTime createdOn;
        String createdUserName;
        String statusDesc;
        Double orcAmt;
        Int32 cdStructureId;
        Double cdStructure;
        String remark;
        String statusRemark;

        Double calculatedMetalCost;
        Double baseMetalCost;
        Double padta;
        Double declaredrate;
        Int32 userId;
        Double bookingPmRate;

        string enqDisplayNo;

        string authReason;
        #endregion

        #region Constructor

        public TblPurchaseBookingBeyondQuotaTO()
        {
        }

        #endregion

        #region GetSet

        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public Int32 IdBookingAuth
        {
            get { return idBookingAuth; }
            set { idBookingAuth = value; }
        }

        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }

        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }

        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }

        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        public Double BookingPmRate
        {
            get { return bookingPmRate; }
            set { bookingPmRate = value; }
        }

        public Double CalculatedMetalCost
        {
            get { return calculatedMetalCost; }
            set { calculatedMetalCost = value; }
        }

        public Double Declaredrate
        {
            get { return declaredrate; }
            set { declaredrate = value; }
        }

        public Double BaseMetalCost
        {
            get { return baseMetalCost; }
            set { baseMetalCost = value; }
        }

        public Double Padta
        {
            get { return padta; }
            set { padta = value; }
        }

        public Double Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public Double DeliveryPeriod
        {
            get { return deliveryPeriod; }
            set { deliveryPeriod = value; }
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


        public string CreatedUserName
        {
            get
            {
                return createdUserName;
            }

            set
            {
                createdUserName = value;
            }
        }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public String StatusDateStr
        {
            get { return statusDate.ToString("dd-MM-yy HH:mm tt"); }
        }

        public string StatusDesc
        {
            get
            {
                return statusDesc;
            }

            set
            {
                statusDesc = value;
            }
        }

        public double OrcAmt { get => orcAmt; set => orcAmt = value; }
        public int CdStructureId { get => cdStructureId; set => cdStructureId = value; }
        public Double CdStructure { get => cdStructure; set => cdStructure = value; }
        public string Remark { get => remark; set => remark = value; }
        public string StatusRemark { get => statusRemark; set => statusRemark = value; }

        public string AuthReason { get => authReason; set => authReason = value; }

        public string EnqDisplayNo { get => enqDisplayNo; set => enqDisplayNo = value; }



        #endregion

    }
}

