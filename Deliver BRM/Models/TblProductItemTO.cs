using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblProductItemTO
    {
        #region Declarations
        Int32 idProdItem;
        Int32 prodClassId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String itemName;
        String itemDesc;
        String remark;
        Int32 isActive;
        Int32 weightMeasureUnitId;
        Int32 conversionUnitOfMeasure;
        Double conversionFactor;
        Int32 isBaseItemForRate;
        Int32 isNonCommercialItem;
        Int32 codeTypeId;                   //Priyanka [16-05-2018] : Added for service tax type.

        //Sudhir
        Int32 isParity;
        Double basePrice;
        Int32 isStockRequire;
        String prodClassDisplayName;
        String weightMeasurUnitDesc;
        String mappedEInvoiceUOM;
        #endregion

        #region Constructor
        public TblProductItemTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProdItem
        {
            get { return idProdItem; }
            set { idProdItem = value; }
        }
        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
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
        public String ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public String ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public Int32 WeightMeasureUnitId
        {
            get { return weightMeasureUnitId; }
            set { weightMeasureUnitId = value; }
        }

        public Int32 ConversionUnitOfMeasure
        {
            get { return conversionUnitOfMeasure; }
            set { conversionUnitOfMeasure = value; }
        }

        public Double ConversionFactor
        {
            get { return conversionFactor; }
            set { conversionFactor = value; }
        }
        public Int32 IsBaseItemForRate
        {
            get { return isBaseItemForRate; }
            set { isBaseItemForRate = value; }
        }
        public Int32 IsNonCommercialItem
        {
            get { return isNonCommercialItem; }
            set { isNonCommercialItem = value; }
        }

        public int IsActive { get => isActive; set => isActive = value; }
        public int CodeTypeId { get => codeTypeId; set => codeTypeId = value; }

        public int IsParity { get => isParity; set => isParity = value; }
        public double BasePrice { get => basePrice; set => basePrice = value; }
        public int IsStockRequire { get => isStockRequire; set => isStockRequire = value; }
        public string ProdClassDisplayName { get => prodClassDisplayName; set => prodClassDisplayName = value; }
        public string WeightMeasurUnitDesc { get => weightMeasurUnitDesc; set => weightMeasurUnitDesc = value; }
        public string MappedEInvoiceUOM { get => mappedEInvoiceUOM; set => mappedEInvoiceUOM = value; }
        #endregion
    }
}
