using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblCnfDealersTO
    {
        #region Declarations
        Int32 idCnfDealerId;
        Int32 cnfOrgId;
        Int32 dealerOrgId;
        Int32 createdBy;
        Int32 isActive;
        DateTime createdOn;
        String remark;
        Int32 isSpecialCnf;
        String cnfOrgName;
        String dealerOrgName;

        #endregion

        #region Constructor
        public TblCnfDealersTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdCnfDealerId
        {
            get { return idCnfDealerId; }
            set { idCnfDealerId = value; }
        }
        public Int32 CnfOrgId
        {
            get { return cnfOrgId; }
            set { cnfOrgId = value; }
        }
        public Int32 DealerOrgId
        {
            get { return dealerOrgId; }
            set { dealerOrgId = value; }
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
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public int IsSpecialCnf { get => isSpecialCnf; set => isSpecialCnf = value; }
        public string CnfOrgName { get => cnfOrgName; set => cnfOrgName = value; }
        public string DealerOrgName { get => dealerOrgName; set => dealerOrgName = value; }
        #endregion
    }
}
