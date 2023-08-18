using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
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
        int brandId;
        string brandName;
        int groupId;
        List<TblQuotaDeclarationTO> quotaDeclarationTOList;
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

        public global::System.Int32 BrandId { get => brandId; set => brandId = value; }
        public global::System.String BrandName { get => brandName; set => brandName = value; }
        public List<TblQuotaDeclarationTO> QuotaDeclarationTOList { get => quotaDeclarationTOList; set => quotaDeclarationTOList = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        #endregion
    }
}
