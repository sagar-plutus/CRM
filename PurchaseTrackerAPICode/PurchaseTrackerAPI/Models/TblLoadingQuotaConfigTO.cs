using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingQuotaConfigTO
    {
        #region Declarations
        Int32 idLoadQuotaConfig;
        Int32 cnfOrgId;
        Int32 materialId;
        Int32 isActive;
        Int32 createdBy;
        DateTime createdOn;
        Double allocPct;
        String remark;
        String cnfOrgName;
        String materialDesc;
        DateTime deactivatedOn;
        Int32 deactivatedBy;
        Int32 prodCatId;
        Int32 prodSpecId;
        String prodCatDesc;
        String prodSpecDesc;
        #endregion

        #region Constructor
        public TblLoadingQuotaConfigTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadQuotaConfig
        {
            get { return idLoadQuotaConfig; }
            set { idLoadQuotaConfig = value; }
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
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double AllocPct
        {
            get { return allocPct; }
            set { allocPct = value; }
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

        public DateTime DeactivatedOn
        {
            get
            {
                return deactivatedOn;
            }

            set
            {
                deactivatedOn = value;
            }
        }

        public int DeactivatedBy
        {
            get
            {
                return deactivatedBy;
            }

            set
            {
                deactivatedBy = value;
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
        #endregion
    }
}
