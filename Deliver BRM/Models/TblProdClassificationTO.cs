using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblProdClassificationTO
    {
        #region Declarations
        Int32 idProdClass;
        Int32 parentProdClassId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String prodClassType;
        String prodClassDesc;
        String remark;
        Int32 isActive;
        String displayName;
        //Sanjay [2018-02-19] Following two new properties Added To distinguish between FG & Scrap ,etc
        Int32 itemProdCatId;
        String itemProdCategory;
        Int32 codeTypeId;       //Priyanka [16-05-2018] : Added for tax type (HSN & SAC)

        #endregion

        #region Constructor
        public TblProdClassificationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProdClass
        {
            get { return idProdClass; }
            set { idProdClass = value; }
        }
        public Int32 ParentProdClassId
        {
            get { return parentProdClassId; }
            set { parentProdClassId = value; }
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
        public String ProdClassType
        {
            get { return prodClassType; }
            set { prodClassType = value; }
        }
        public String ProdClassDesc
        {
            get { return prodClassDesc; }
            set { prodClassDesc = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public String DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        public int IsActive { get => isActive; set => isActive = value; }
        public int ItemProdCatId { get => itemProdCatId; set => itemProdCatId = value; }
        public string ItemProdCategory { get => itemProdCategory; set => itemProdCategory = value; }
        public int CodeTypeId { get => codeTypeId; set => codeTypeId = value; }
        #endregion
    }
}
