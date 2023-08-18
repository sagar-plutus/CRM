using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblBookingExtTO
    {
        #region Declarations
        Int32 idBookingExt;
        Int32 bookingId;
        Int32 materialId;
        Double bookedQty;
        Double rate;
        String materialSubType;
        DateTime bookingDatetime;
        Int32 isConfirmed;
        Int32 isJointDelivery;
        Double cdStructure;
        Int32 noOfDeliveries;
        Int32 prodCatId;
        Int32 prodSpecId;
        String prodCatDesc;
        String prodSpecDesc;
        Int32 prodItemId;
        String categoryName;
        String subCategoryName;
        String specificationName;
        String itemName;
        String displayName;
        Int32 scheduleId;
        double balanceQty;
        DateTime scheduleDate;
        Int32 loadingLayerId;
        #endregion

        #region Constructor
        public TblBookingExtTO()
        {
        }

        public TblBookingExtTO(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            this.ProdCatId = tblLoadingSlipExtTO.ProdCatId;
            this.ProdSpecId = tblLoadingSlipExtTO.ProdSpecId;
            this.ProdItemId= tblLoadingSlipExtTO.ProdItemId;
            this.MaterialId= tblLoadingSlipExtTO.MaterialId;
            this.BookedQty = tblLoadingSlipExtTO.LoadingQty;
        }

        #endregion

        #region GetSet
        public Int32 IdBookingExt
        {
            get { return idBookingExt; }
            set { idBookingExt = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 MaterialId
        {
            get { return materialId; }
            set { materialId = value; }
        }
        public Double BookedQty
        {
            get { return bookedQty; }
            set { bookedQty = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public String MaterialSubType
        {
            get { return materialSubType; }
            set { materialSubType = value; }
        }

        public DateTime BookingDatetime
        {
            get { return bookingDatetime; }
            set { bookingDatetime = value; }
        }

        public Int32 IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
        }

        public Int32 IsJointDelivery
        {
            get { return isJointDelivery; }
            set { isJointDelivery = value; }
        }

        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }

        public Int32 NoOfDeliveries
        {
            get { return noOfDeliveries; }
            set { noOfDeliveries = value; }
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

        public int ProdSpecId
        {
            get
            {
                return prodSpecId;
            }

            set
            {
                prodSpecId = value;
            }
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

        public int ProdItemId { get => prodItemId; set => prodItemId = value; }
        public string CategoryName { get => categoryName; set => categoryName = value; }
        public string SubCategoryName { get => subCategoryName; set => subCategoryName = value; }
        public string SpecificationName { get => specificationName; set => specificationName = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public Int32 ScheduleId { get => scheduleId; set => scheduleId = value; }

        public double BalanceQty { get => balanceQty; set => balanceQty = value; }

        public DateTime ScheduleDate { get => scheduleDate; set => scheduleDate = value; }
        public Int32 LoadingLayerId { get => loadingLayerId; set => loadingLayerId = value; }

        

        #endregion
    }
}
