using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblRateBandDeclarationPurchaseTO
    {
        #region Declarations
        Int32 idRateBandDeclarationPurchase;
        Int32 userId;
        Int32 globalRatePurchaseId;
        Double rateBandCosting;
        Double rateBandCorrection;
        Int32 validUpto;
        Int32 newvalidupto;
        Double calculatedRateCosting;
        Double calculatedRateCorrection;
        Int32 createdBy;
        DateTime createdOn;
        Int32 isActive;
        Int32 updatedBy;
        DateTime updatedOn;

        Object tag;
        String purchaseManager;

        Double declaredRate;

        List<TblGradeWiseTargetQtyTO> gradeWiseTargetQtyTOList = new List<TblGradeWiseTargetQtyTO>();
        #endregion

        #region Constructor
        public TblRateBandDeclarationPurchaseTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdRateBandDeclarationPurchase
        {
            get { return idRateBandDeclarationPurchase; }
            set { idRateBandDeclarationPurchase = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 GlobalRatePurchaseId
        {
            get { return globalRatePurchaseId; }
            set { globalRatePurchaseId = value; }
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
        public Double RateBandCosting
        {
            get { return rateBandCosting; }
            set { rateBandCosting = value; }
        }
        //public Double DeclaredRate
        //{
        //    get { return declaredRate; }
        //    set { declaredRate = value; }
        //}

        public Double RateBandCorrection
        {
            get { return rateBandCorrection; }
            set { rateBandCorrection = value; }
        }

        public Double CalculatedRateCosting
        {
            get { return calculatedRateCosting; }
            set { calculatedRateCosting = value; }
        }
        public Double CalculatedRateCorrection
        {
            get { return calculatedRateCorrection; }
            set { calculatedRateCorrection = value; }
        }
        public Int32 ValidUpto
        {
            get { return validUpto; }
            set { validUpto = value; }
        }
        public Int32 NewValidUpto
        {
            get { return newvalidupto; }
            set { newvalidupto = value; }
        }

        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }

        public String PurchaseManager
        {
            get { return purchaseManager; }
            set { purchaseManager = value; }
        }

        public object Tag { get => tag; set => tag = value; }
        public Double DeclaredRate
        {
            get { return declaredRate; }
            set { declaredRate = value; }
        }


        public List<TblGradeWiseTargetQtyTO> GradeWiseTargetQtyTOList
        {
            get { return gradeWiseTargetQtyTOList; }
            set { gradeWiseTargetQtyTOList = value; }
        }

        #endregion
    }
}
