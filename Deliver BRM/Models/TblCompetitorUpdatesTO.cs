using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblCompetitorUpdatesTO
    {
        #region Declarations
        Int32 idCompeUpdate;
        Int32 competitorExtId;
        Int32 createdBy;
        DateTime updateDatetime;
        DateTime createdOn;
        Double price;
        String informerName;
        String alternateInformerName;
        String firmName;
        Double lastPrice;
        String createdByName;
        Int32 dealerId;
        String dealerName;
        Int32 otherSourceId;
        string otherSourceDesc;
        String otherSourceOtherDesc;
        Int32 competitorOrgId;
        Double prodCapacityMT;
        String brandName;
        #endregion

        #region Constructor
        public TblCompetitorUpdatesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdCompeUpdate
        {
            get { return idCompeUpdate; }
            set { idCompeUpdate = value; }
        }
        public Int32 CompetitorExtId
        {
            get { return competitorExtId; }
            set { competitorExtId = value; }
        }
        [JsonIgnore]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime UpdateDatetime
        {
            get { return updateDatetime; }
            set { updateDatetime = value; }
        }
        public String UpdateDatetimeStr
        {
            get { return UpdateDatetime.ToString(Constants.DefaultDateFormat); }
        }

        [JsonIgnore]
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double Price
        {
            get { return price; }
            set { price = value; }
        }
        public String InformerName
        {
            get { return informerName; }
            set { informerName = value; }
        }
        public String AlternateInformerName
        {
            get { return alternateInformerName; }
            set { alternateInformerName = value; }
        }

        public String FirmName
        {
            get { return firmName; }
            set { firmName = value; }
        }
        public Double LastPrice
        {
            get { return lastPrice; }
            set { lastPrice = value; }
        }

        public String CreatedByName
        {
            get
            {
                return createdByName;
            }

            set
            {
                createdByName = value;
            }
        }

        public int DealerId
        {
            get
            {
                return dealerId;
            }

            set
            {
                dealerId = value;
            }
        }

        public string DealerName
        {
            get
            {
                return dealerName;
            }

            set
            {
                dealerName = value;
            }
        }

        public int OtherSourceId { get => otherSourceId; set => otherSourceId = value; }
        public string OtherSourceDesc { get => otherSourceDesc; set => otherSourceDesc = value; }
        public string OtherSourceOtherDesc { get => otherSourceOtherDesc; set => otherSourceOtherDesc = value; }
        public int CompetitorOrgId { get => competitorOrgId; set => competitorOrgId = value; }
        public double ProdCapacityMT { get => prodCapacityMT; set => prodCapacityMT = value; }
        public string BrandName { get => brandName; set => brandName = value; }

        #endregion
    }
}
