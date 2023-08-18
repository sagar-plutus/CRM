using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.StaticStuff;
using static PurchaseTrackerAPI.StaticStuff.Constants;


namespace PurchaseTrackerAPI.Models//Need To Check...
{
    public class TblPurchaseParitySummaryTO
    {
        
        #region Declarations

        Int32 idParity;
        Int32 createdBy;
        String createdByName;//Prajakta[2018-04-18]Added
        Int32 isActive;
        DateTime createdOn;
        DateTime updateDatetime;//Prajakta[2018-04-18]Added

        String updateDatetimeStr; //Nikhil[2018-05-31] Added
        String remark;
        String informerName;//Prajakta[2018-04-18]Added

        Int32 dealerId;

         Int32 otherSourceId;
        List<TblPurchaseParityDetailsTO> parityDetailList;//Need To Check...
        Int32 stateId;
        String stateName;
        Double baseValCorAmt;
        Double freightAmt;
        Double expenseAmt;
        Double otherAmt;
        Int32 prodClassId;
        String idPurCompetitorExt;
        String organizationId;
        String materialType;
        String materialGrade;
        String firmName;
        String competitorOrgId;
        Double price;
        Int32 idProdClass;
        String prodClassDesc;

        #endregion

        #region Constructor

        public TblPurchaseParitySummaryTO()
        {
        }

        #endregion

        #region GetSet

        public Int32 IdParity
        {
            get { return idParity; }
            set { idParity = value; }
        }

         public Int32 DealerId 
        {
            get { return dealerId; }
            set { dealerId = value; }
        }

        public Int32 OtherSourceId 
        {
            get { return otherSourceId; }
            set { otherSourceId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        
        public DateTime UpdateDatetime
        {
            get { return updateDatetime; }
            set { updateDatetime = value; }
        }

        public String UpdateDatetimeStr
        {
            get { return updateDatetime.ToString(Constants.DefaultDateFormat); }
        } 
        
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public String CreatedByName
        {
            get { return createdByName; }
            set { createdByName = value; }
        }

        public String InformerName
        {
            get { return informerName; }
            set { informerName = value; }
        }
        public List<TblPurchaseParityDetailsTO> ParityDetailList
        {
            get
            {
                return parityDetailList;
            }

            set
            {
                parityDetailList = value;
            }
        }

        public int StateId { get => stateId; set => stateId = value; }
        public string StateName { get => stateName; set => stateName = value; }
        public double BaseValCorAmt { get => baseValCorAmt; set => baseValCorAmt = value; }
        public double FreightAmt { get => freightAmt; set => freightAmt = value; }
        public double ExpenseAmt { get => expenseAmt; set => expenseAmt = value; }
        public double OtherAmt { get => otherAmt; set => otherAmt = value; }

        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }

        public Double Price
        {
            get { return price; }
            set { price = value; }
        }
        public String IdPurCompetitorExt
        {
            get { return idPurCompetitorExt; }
            set { idPurCompetitorExt = value; }
        }
        public String OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public String MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }
        public String MaterialGrade
        {
            get { return materialGrade; }
            set { materialGrade = value; }
        }
        public String FirmName
        {
            get { return firmName; }
            set { firmName = value; }
        }
        public String CompetitorOrgId
        {
            get { return competitorOrgId; }
            set { competitorOrgId = value; }
        }

        public Int32 IdProdClass
        {
            get { return idProdClass; }
            set { idProdClass = value; }
        }

        public String ProdClassDesc
        {
            get { return prodClassDesc; }
            set { prodClassDesc = value; }
        }
        
        #endregion

    }
}
