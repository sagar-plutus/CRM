using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
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
        // Int32 brandId;

        //Vijaymala Added [13-12-2017]
        Int32 brandId;
        String brandDesc;
        Int32 scheduleId;
        Double balanceQty;
        DateTime scheduleDate;

        //Vijaymala Added [02-01-2017]
        Int32 loadingLayerId;
        String layerDesc;

        #endregion

        #region Constructor
        public TblBookingExtTO()
        {
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

        public int BrandId { get => brandId; set => brandId = value; }
        public int ScheduleId { get => scheduleId; set => scheduleId = value; }

        public Double BalanceQty
        {
            get { return balanceQty; }
            set { balanceQty = value; }
        }

        public DateTime ScheduleDate { get => scheduleDate; set => scheduleDate = value; }
        public string BrandDesc { get => brandDesc; set => brandDesc = value; }

        //Vijaymala Added[02-01-2018]
        public int LoadingLayerId { get => loadingLayerId; set => loadingLayerId = value; }
        public string LayerDesc { get => layerDesc; set => layerDesc = value; }



        #endregion
    }
}
