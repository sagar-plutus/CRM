using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingQuotaDeclarationTO
    {
        #region Declarations
        Int32 idLoadingQuota;
        Int32 cnfOrgId;
        Int32 materialId;
        Int32 loadQuotaConfigId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double allocQuota;
        Double balanceQuota;
        String remark;
        String cnfOrgName;
        String materialDesc;
        Int32 prodCatId;
        Int32 prodSpecId;
        String prodCatDesc;
        String prodSpecDesc;
        Double transferedQuota;
        Double receivedQuota;
        Double removedQuota;
        #endregion

        #region Constructor
        public TblLoadingQuotaDeclarationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingQuota
        {
            get { return idLoadingQuota; }
            set { idLoadingQuota = value; }
        }
        public Int32 CnfOrgId
        {
            get { return cnfOrgId; }
            set { cnfOrgId = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public Int32 LoadQuotaConfigId
        {
            get { return loadQuotaConfigId; }
            set { loadQuotaConfigId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double AllocQuota
        {
            get { return allocQuota; }
            set { allocQuota = value; }
        }
        public Double BalanceQuota
        {
            get { return balanceQuota; }
            set { balanceQuota = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public string CnfOrgName
        {
            get
            {
                return cnfOrgName;
            }

            set
            {
                cnfOrgName = value;
            }
        }

        public string MaterialDesc
        {
            get
            {
                return materialDesc;
            }

            set
            {
                materialDesc = value;
            }
        }

        public Int32 ProdCatId
        {
            get { return prodCatId; }
            set { prodCatId = value; }
        }

        public Int32 ProdSpecId
        {
            get { return prodSpecId; }
            set { prodSpecId = value; }
        }

        public String ProdCatDesc
        {
            get { return prodCatDesc; }
            set { prodCatDesc = value; }
        }
        public String ProdSpecDesc
        {
            get { return prodSpecDesc; }
            set { prodSpecDesc = value; }
        }

        public double TransferedQuota
        {
            get
            {
                return transferedQuota;
            }

            set
            {
                transferedQuota = value;
            }
        }

        public double ReceivedQuota
        {
            get
            {
                return receivedQuota;
            }

            set
            {
                receivedQuota = value;
            }
        }

        public double RequiredQuota
        {
            get
            {
                if (balanceQuota < 0)
                    return Math.Abs(balanceQuota);
                else return 0;
            }
        }

        public double RemovedQuota { get => removedQuota; set => removedQuota = value; }
        #endregion
    }
}
