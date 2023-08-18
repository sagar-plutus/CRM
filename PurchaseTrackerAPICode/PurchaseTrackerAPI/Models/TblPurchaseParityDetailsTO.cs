using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseParityDetailsTO //Need to check...
    {
        
        #region Declarations

        Int32 idParityDtl;
        Int32 parityId;
        Int32 prodItemId;
        double recovery;
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
        Double parityRecoveryAmt;

        Int32 isNonCommercialItem;
        String createdByName;
        #endregion

        #region Constructor

        public TblPurchaseParityDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Double ParityRecoveryAmt
        {
            get { return parityRecoveryAmt; }
            set { parityRecoveryAmt = value; }
        }
        

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
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }

        public double Recovery
        {
            get { return recovery; }
            set { recovery = value; }
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

        public Int32 IsNonCommercialItem { get => isNonCommercialItem; set => isNonCommercialItem = value; }
        public string CreatedByName { get => createdByName; set => createdByName = value; }

        #endregion
    }
}

