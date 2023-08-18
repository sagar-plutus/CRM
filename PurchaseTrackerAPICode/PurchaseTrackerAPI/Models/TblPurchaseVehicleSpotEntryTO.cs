using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TO;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseVehicleSpotEntryTO
    {
        #region Declarations
        Int32 idVehicleSpotEntry;
        Int32 supplierId;
        Int32 vehicleTypeId;
        Int32 statusId;
        Int32 createdBy;
        DateTime statusDate;
        DateTime createdOn;
        Double vehicleQtyMT;
        String location;
        String vehicleNo;
        String driverName;
        String remark;
        Int32 locationId;
        Double spotVehicleQty;
        String supplierName;
        String vehicleTypeDesc;

        Int32 purchaseEnquiryId;

        Int32 prodClassId;

        String driverContactNo;
        Int32 stateId;

        Int32 isLinkToExistingSauda;
        Int32 purchaseScheduleSummaryId;

        string prodClassType;

        Int32 isAutoSpotVehSauda;

        List<TblSpotVehMatDtlsTO> spotVehMatDtlsTOList;
        TblPurchaseEnquiryTO bookingTO;

        string enqDisplayNo;

        List<TblRecycleDocumentTO> recycleDocumentsTOList = new List<TblRecycleDocumentTO>();
        List<TblSpotEntryContainerDtlsTO> tblSpotEntryContainerDtlsTOList = new List<TblSpotEntryContainerDtlsTO>();
        String createdOnStr;
        Int32 previousPurchaseEnqId;
        double vehicleQty;
        TblPartyWeighingMeasuresTO partyWeighingMeasureTO;
        #endregion

        #region Constructor
        public TblPurchaseVehicleSpotEntryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehicleSpotEntry
        {
            get { return idVehicleSpotEntry; }
            set { idVehicleSpotEntry = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
        }

        public Int32 LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double VehicleQtyMT
        {
            get { return vehicleQtyMT; }
            set { vehicleQtyMT = value; }
        }
        public String Location
        {
            get { return location; }
            set { location = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public String SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        public String VehicleTypeDesc
        {
            get { return vehicleTypeDesc; }
            set { vehicleTypeDesc = value; }
        }

        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
        }


        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public String DriverContactNo
        {
            get { return driverContactNo; }
            set { driverContactNo = value; }
        }

        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }

        public Double SpotVehicleQty
        {
            get { return spotVehicleQty; }
            set { spotVehicleQty = value; }
        }
        public List<TblSpotVehMatDtlsTO> SpotVehMatDtlsTOList
        {
            get { return spotVehMatDtlsTOList; }
            set { spotVehMatDtlsTOList = value; }

        }

        public string ProdClassType
        {
            get { return prodClassType; }
            set { prodClassType = value; }
        }

        public Int32 IsLinkToExistingSauda
        {
            get { return isLinkToExistingSauda; }
            set { isLinkToExistingSauda = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }

        public Int32 IsAutoSpotVehSauda
        {
            get { return isAutoSpotVehSauda; }
            set { isAutoSpotVehSauda = value; }
        }


        public string EnqDisplayNo
        {
            get { return enqDisplayNo; }
            set { enqDisplayNo = value; }
        }



        public TblPurchaseEnquiryTO BookingTO
        {
            get { return bookingTO; }
            set { bookingTO = value; }
        }

        public List<TblRecycleDocumentTO> RecycleDocumentsTOList
        {
            get { return recycleDocumentsTOList; }
            set { recycleDocumentsTOList = value; }
        }
        public List<TblSpotEntryContainerDtlsTO> TblSpotEntryContainerDtlsTOList
        {
            get { return tblSpotEntryContainerDtlsTOList; }
            set { tblSpotEntryContainerDtlsTOList = value; }
        }

        public string CreatedOnStr { get => createdOnStr; set => createdOnStr = value; }

        public Int32 PreviousPurchaseEnqId { get => previousPurchaseEnqId; set => previousPurchaseEnqId = value; }

        public Double VehicleQty { get => vehicleQty; set => vehicleQty = value; }

        public TblPartyWeighingMeasuresTO PartyWeighingMeasureTO
        {
            get { return partyWeighingMeasureTO; }
            set { partyWeighingMeasureTO = value; }
        }



        #endregion
    }
}
