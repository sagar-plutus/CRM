using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models 
{
    public class TblTRLoadingTO
    {
        #region Declarations
        Int32 idLoading;
        String loadingSlipNo;
        Int32 loadingTypeId; 
        Int32 transferRequestId;
        Int32 fromLocationId;
        Int32 toLocationId;
        Int32 materialTypeId;
        Int32 materialSubTypeId;
        Int32 vehicleId;
        Int32 statusId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 statusBy;
        DateTime createdOn;
        DateTime updatedOn;
        DateTime statusOn;
        Int32 isActive;
        Double scheduleQty;
        String narration;
        String driverName;
        Int32 srNo;
        Int32 unloadingPointId;
        String vehicleNo;
        Int32 vehicleStatusId;
        String requestDisplayNo;
        String materialType;
        String materialsSubType;
        String fromLocation;
        String toLocation;
        String unloadingPoint;
        String statusDesc;

        String status;
        String createdByName;
        String updatedByName;
        String materialSubType;
        String transactionCloseRemark;
        String requestUserName;
        DateTime requestCreatedOn;
        Int32 idVehicle;
        String vehicleStatus;
        Int32 reviewUnloadingParam;
        Int32 isReviewUnloading;
        Int32 unMaterialTypeId;
        Int32 unMaterialSubTypeId;
        String unloadingNarration;
        String weighingRemark;
        String statusByUserName;
        Decimal loadingNetWeight;
        Decimal loadingTareWeight;
        Decimal loadingGrossWeight;
        Decimal unloadingNetWeight;
        Decimal unloadingGrossWeight;
        Decimal unloadingTareWeight;
        String createdOnDateStr;
        String statusOnDateStr;
        String statusOnTimeStr;
        String createdOnTimeStr;
        Int32 idSLA;
        Int32 addOnCnt;
        Int32 addOnUnCnt;
        Decimal totalScheduleQty;
        Int32 totalTransactionCnt;
        String loadingCompletedOnDateStr;
        String unLoadingCompletedOnDateStr;
        String loadingCompletedOnTimeStr;
        String unLoadingCompletedOnTimeStr;
        String unloadingTimeDiff;
        String statusByName;
        Int32 idLoadingHistory;
        String unloadingMaterialType;
        String unloadingMaterialsSubType;
        #endregion

        #region Constructor
        public TblTRLoadingTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoading
        {
            get { return idLoading; }
            set { idLoading = value; }
        }
        public String LoadingSlipNo
        {
            get { return loadingSlipNo; }
            set { loadingSlipNo = value; }
        }
        public Int32 LoadingTypeId
        {
            get { return loadingTypeId; }
            set { loadingTypeId = value; }
        }
        public Int32 TransferRequestId
        {
            get { return transferRequestId; }
            set { transferRequestId = value; }
        }
        public Int32 FromLocationId
        {
            get { return fromLocationId; }
            set { fromLocationId = value; }
        }
        public Int32 ToLocationId
        {
            get { return toLocationId; }
            set { toLocationId = value; }
        }
        public Int32 MaterialTypeId
        {
            get { return materialTypeId; }
            set { materialTypeId = value; }
        }
        public Int32 MaterialSubTypeId
        {
            get { return materialSubTypeId; }
            set { materialSubTypeId = value; }
        }
        public Int32 VehicleId
        {
            get { return vehicleId; }
            set { vehicleId = value; }
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
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public Int32 StatusBy
        {
            get { return statusBy; }
            set { statusBy = value; }
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
        public DateTime StatusOn
        {
            get { return statusOn; }
            set { statusOn = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Double ScheduleQty
        {
            get { return scheduleQty; }
            set { scheduleQty = value; }
        }
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        public String DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }

        public Int32 SrNo { get => srNo; set => srNo = value; }
        public int UnloadingPointId { get => unloadingPointId; set => unloadingPointId = value; }
        public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        public int VehicleStatusId { get => vehicleStatusId; set => vehicleStatusId = value; }
        public string RequestDisplayNo { get => requestDisplayNo; set => requestDisplayNo = value; }
        public string MaterialType { get => materialType; set => materialType = value; }
        public string FromLocation { get => fromLocation; set => fromLocation = value; }
        public string ToLocation { get => toLocation; set => toLocation = value; }
        public string UnloadingPoint { get => unloadingPoint; set => unloadingPoint = value; }
        public string StatusDesc { get => statusDesc; set => statusDesc = value; }
        public string MaterialsSubType { get => materialsSubType; set => materialsSubType = value; }

        //public int UnloadingPointId { get => unloadingPointId; set => unloadingPointId = value; }
        //public string MaterialType { get => materialType; set => materialType = value; }
        public string MaterialSubType { get => materialSubType; set => materialSubType = value; }
        //public string FromLocation { get => fromLocation; set => fromLocation = value; }
        //public string ToLocation { get => toLocation; set => toLocation = value; }
        //public string UnloadingPoint { get => unloadingPoint; set => unloadingPoint = value; }
        public string Status { get => status; set => status = value; }
        public string CreatedByName { get => createdByName; set => createdByName = value; }
        public string UpdatedByName { get => updatedByName; set => updatedByName = value; }
        public string TransactionCloseRemark { get => transactionCloseRemark; set => transactionCloseRemark = value; }
        public string RequestUserName { get => requestUserName; set => requestUserName = value; }
        public DateTime RequestCreatedOn { get => requestCreatedOn; set => requestCreatedOn = value; }
        public int IdVehicle { get => idVehicle; set => idVehicle = value; }
        public string VehicleStatus { get => vehicleStatus; set => vehicleStatus = value; }
        public int ReviewUnloadingParam { get => reviewUnloadingParam; set => reviewUnloadingParam = value; }
        public int IsReviewUnloading { get => isReviewUnloading; set => isReviewUnloading = value; }
        public int UnMaterialTypeId { get => unMaterialTypeId; set => unMaterialTypeId = value; }
        public int UnMaterialSubTypeId { get => unMaterialSubTypeId; set => unMaterialSubTypeId = value; }
        public string UnloadingNarration { get => unloadingNarration; set => unloadingNarration = value; }
        public string WeighingRemark { get => weighingRemark; set => weighingRemark = value; }
        public string StatusByUserName { get => statusByUserName; set => statusByUserName = value; }
        public decimal LoadingNetWeight { get => loadingNetWeight; set => loadingNetWeight = value; }
        public string CreatedOnDateStr { get => createdOnDateStr; set => createdOnDateStr = value; }
        public string StatusOnDateStr { get => statusOnDateStr; set => statusOnDateStr = value; }
        public string StatusOnTimeStr { get => statusOnTimeStr; set => statusOnTimeStr = value; }
        public decimal UnloadingNetWeight { get => unloadingNetWeight; set => unloadingNetWeight = value; }
        public int IdSLA { get => idSLA; set => idSLA = value; }
        public string CreatedOnTimeStr { get => createdOnTimeStr; set => createdOnTimeStr = value; }
        public int AddOnCnt { get => addOnCnt; set => addOnCnt = value; }
        public int AddOnUnCnt { get => addOnUnCnt; set => addOnUnCnt = value; }
        public decimal LoadingGrossWeight { get => loadingGrossWeight; set => loadingGrossWeight = value; }
        public decimal LoadingTareWeight { get => loadingTareWeight; set => loadingTareWeight = value; }
        public decimal TotalScheduleQty { get => totalScheduleQty; set => totalScheduleQty = value; }
        public int TotalTransactionCnt { get => totalTransactionCnt; set => totalTransactionCnt = value; }
        public decimal UnloadingTareWeight { get => unloadingTareWeight; set => unloadingTareWeight = value; }
        public decimal UnloadingGrossWeight { get => unloadingGrossWeight; set => unloadingGrossWeight = value; }
        public string LoadingCompletedOnDateStr { get => loadingCompletedOnDateStr; set => loadingCompletedOnDateStr = value; }
        public string UnLoadingCompletedOnDateStr { get => unLoadingCompletedOnDateStr; set => unLoadingCompletedOnDateStr = value; }
        public string LoadingCompletedOnTimeStr { get => loadingCompletedOnTimeStr; set => loadingCompletedOnTimeStr = value; }
        public string UnLoadingCompletedOnTimeStr { get => unLoadingCompletedOnTimeStr; set => unLoadingCompletedOnTimeStr = value; }
        public string UnloadingTimeDiff { get => unloadingTimeDiff; set => unloadingTimeDiff = value; }
        public string StatusByName { get => statusByName; set => statusByName = value; }
        public int IdLoadingHistory { get => idLoadingHistory; set => idLoadingHistory = value; }
        public string UnloadingMaterialType { get => unloadingMaterialType; set => unloadingMaterialType = value; }
        public string UnloadingMaterialsSubType { get => unloadingMaterialsSubType; set => unloadingMaterialsSubType = value; }
        #endregion
    }
    public class LoadingFilterTO
    {
        #region Declarations
        String statusIdStr;
        DateTime fromDate;
        DateTime toDate;
        Boolean skipDateFilter;
        String screenName;
        DateTime migrationDate;
        Int32 isReviewUnloading;
        #endregion

        #region Constructor
        public LoadingFilterTO()
        {
        }

        #endregion
        
        #region GetSet
        public string StatusIdStr { get => statusIdStr; set => statusIdStr = value; }
        public DateTime FromDate { get => fromDate; set => fromDate = value; }
        public DateTime ToDate { get => toDate; set => toDate = value; }
        public bool SkipDateFilter { get => skipDateFilter; set => skipDateFilter = value; }
        public string ScreenName { get => screenName; set => screenName = value; }
        public DateTime MigrationDate { get => migrationDate; set => migrationDate = value; }
        public int IsReviewUnloading { get => isReviewUnloading; set => isReviewUnloading = value; }
        #endregion
    }
}
