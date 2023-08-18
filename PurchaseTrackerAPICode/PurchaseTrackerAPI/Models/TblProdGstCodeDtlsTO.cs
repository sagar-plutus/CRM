using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblProdGstCodeDtlsTO
    {
        #region Declarations
        Int32 idProdGstCode;
        Int32 prodCatId;
        Int32 prodSpecId;
        Int32 prodItemId;
        Int32 materialId;
        Int32 gstCodeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime effectiveFromDt;
        DateTime effectiveTodt;
        DateTime createdOn;
        DateTime updatedOn;
        String remark;
        Int32 isActive;
        String prodItemDesc;
        #endregion

        #region Constructor
        public TblProdGstCodeDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProdGstCode
        {
            get { return idProdGstCode; }
            set { idProdGstCode = value; }
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
        public Int32 GstCodeId
        {
            get { return gstCodeId; }
            set { gstCodeId = value; }
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
        public DateTime EffectiveFromDt
        {
            get { return effectiveFromDt; }
            set { effectiveFromDt = value; }
        }
        public DateTime EffectiveTodt
        {
            get { return effectiveTodt; }
            set { effectiveTodt = value; }
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
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public int ProdItemId { get => prodItemId; set => prodItemId = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public string ProdItemDesc { get => prodItemDesc; set => prodItemDesc = value; }
        public int MaterialId { get => materialId; set => materialId = value; }

        public string EffectiveFromDtStr
        {
            get
            {
                if (this.effectiveFromDt != DateTime.MinValue)
                    return this.effectiveFromDt.ToString(Constants.DefaultDateFormat);
                else return string.Empty;
            }
        }

        public string EffectiveToDtStr
        {
            get
            {
                if (this.effectiveTodt != DateTime.MinValue)
                    return this.effectiveTodt.ToString(Constants.DefaultDateFormat);
                else return string.Empty;
            }
        }

        public string CreatedOnStr
        {
            get
            {
                if (this.createdOn != DateTime.MinValue)
                    return this.createdOn.ToString(Constants.DefaultDateFormat);
                else return string.Empty;
            }
        }

        #endregion
    }
}
