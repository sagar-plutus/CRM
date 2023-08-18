using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblParityDetailsTO
    {
        #region Declarations
        Int32 idParityDtl;
        Int32 parityId;
        Int32 materialId;
        Int32 createdBy;
        DateTime createdOn;
        Double parityAmt;
        Double nonConfParityAmt;
        String remark;
        Int32 prodCatId;
        String prodCatDesc;
        String materialDesc;
        Int32 prodSpecId;
        String prodSpecDesc;
        Int32 brandId;
        #endregion

        #region Constructor
        public TblParityDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdParityDtl
        {
            get { return idParityDtl; }
            set { idParityDtl = value; }
        }
        public Int32 ParityId
        {
            get { return parityId; }
            set { parityId = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
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
        public Double ParityAmt
        {
            get { return parityAmt; }
            set { parityAmt = value; }
        }
        public Double NonConfParityAmt
        {
            get { return nonConfParityAmt; }
            set { nonConfParityAmt = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public int ProdCatId
        {
            get
            {
                return prodCatId;
            }

            set
            {
                prodCatId = value;
            }
        }

        public string ProdCatDesc
        {
            get
            {
                return prodCatDesc;
            }

            set
            {
                prodCatDesc = value;
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

        public String CreatedOnStr
        {
            get
            {
                if (createdOn == DateTime.MinValue)
                    return "-";
                else
                    return createdOn.ToString(Constants.DefaultDateFormat);
            }
        }

        public int ProdSpecId { get => prodSpecId; set => prodSpecId = value; }
        public string ProdSpecDesc { get => prodSpecDesc; set => prodSpecDesc = value; }
        public int BrandId { get => brandId; set => brandId = value; }
        #endregion
    }
}
