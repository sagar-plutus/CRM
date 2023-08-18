using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblGlobalRateTO
    {
        #region Declarations
        Int32 idGlobalRate;
        Int32 createdBy;
        DateTime createdOn;
        Double rate;
        String comments;
        Double quantity;
        Double avgPrice;
        Int32 rateReasonId;
        String rateReasonDesc;
        #endregion

        #region Constructor
        public TblGlobalRateTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGlobalRate
        {
            get { return idGlobalRate; }
            set { idGlobalRate = value; }
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

        public Double Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public Double AvgPrice
        {
            get { return avgPrice; }
            set { avgPrice = value; }
        }
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat) ; }
        }

        public int RateReasonId
        {
            get
            {
                return rateReasonId;
            }

            set
            {
                rateReasonId = value;
            }
        }

        public String RateReasonDesc
        {
            get
            {
                return rateReasonDesc;
            }

            set
            {
                rateReasonDesc = value;
            }
        }
        #endregion
    }

    public class GlobalRateTOFroGraph
    {
        DateTime createdOn;
        Double rate;
        string firmName;
        Int32 orgId;
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        public string FirmName { get => firmName; set => firmName = value; }
        public int OrgId { get => orgId; set => orgId = value; }


        public String UpdateDatetimeStr
        {
            get { return createdOn.ToString("dd-MM-yy"); }
        }

    }

}

