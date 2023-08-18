using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseScheduleSummaryTO
    {
        #region Declarations
        Int32 idPurchaseScheduleSummary;
        Int32 parentPurchaseScheduleSummaryId;
        Int32 purchaseEnquiryId;
        Int32 supplierId;
        Int32 statusId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime scheduleDate;
        DateTime oldScheduleDate;
        String scheduleDateStr;
        DateTime createdOn;
        DateTime updatedOn;
        DateTime updatedOnStr;
        Double qty;
        Double orgUnloadedQty;
        Double scheduleQty;

        Double calculatedMetalCost;
        Double baseMetalCost;
        Double padta;
        Int32 saudaTypeId;

        Double calculatedMetalCostForNC;
        Double baseMetalCostForNC;
        Double padtaForNC;

        String vehicleNo;
        String remark;
        String statusName;
        String statusDesc;
        String colorCode;

        String supplierName;
        Int32 cOrNc;
        Double rate;
        String materailType;

        Double rateBand;
        Int32 prodClassId;
        string corretionCompletedOnStr;

        Int32 engineerId;
        Int32 supervisorId;
        Int32 photographerId;
        Boolean qualityFlag;
        Int32 stateId;
        String stateName;
        double total;

        double refRateofV48Var;
        double refRateC;

        //Nikhil[2018-05-25] Added
        String driverName;
        String driverContactNo;

        String lotSize;
        String transporterName;
        Int32 vehicleTypeId;
        String vehicleTypeName;
        Double targetPadta;
        Double freight;
        Int32 isFreightAdded;
        String containerNo;
        Int32 vehicleStateId;
        String vehicleStateName;

        Int32 isVehicleOut;

        String location;
        Int32 spotEntryVehicleId;

        Int32 locationId;
        Int32 cOrNcId;
        string supervisorName;
        Int32 narrationId;
        string narration;

        Int32 vehicleCatId;
        Int32 previousStatusId;
        string previousStatusName;

        DateTime latestWtTakenOn;


        String prodClassDesc;
        Int32 currentStatusId;
        Int32 vehiclePhaseId;

        Int32 vehiclePhaseSequanceNo;
        string vehiclePhaseName;
        Int32 isActive;

        double rateForC;
        double rateForNC;
        string photographer;
        Int32 previousParentId;

        Int32 rootScheduleId;
        string enqDisplayNo;

        Int32 isVehicleVerified;

        Int32 scheduleHistoryId;
        Int32 acceptStatusId;

        Int32 forSaveOrSubmit;

        Int32 isApproved;
        Int32 isLatest;
        string correNarration;
        string saudaNarration;
        String saudaSupplierName;


        Double totalSpotVehQty;
        Double totalSpotVehCnt;
        Double otherSpotVehQty;
        Double otherSpotVehCnt;

        Int32 isIgnoreApproval;

        Int32 historyPhaseId;
        Int32 rejectStatusId;
        Int32 rejectPhaseId;
        Int32 acceptPhaseId;
        Int32 historyIsActive;
        string statusRemark;

        string navigationUrl;

        Int32 graderId;

        Int32 isCorrectionCompleted;

        Int32 isGradingCompleted;

        string greaderName;
        string engineerName;
        double unldDateBaseMetalCost;
        double unldDatePadtaPerTon;

        double rejectedQty;

        Int32 rejectedBy;

        Int32 approvalType;

        DateTime rejectedOn;

        string vehRejectReasonId;

        string rejectedByUserName;

        Int32 isAutoSpotVehSauda;
        Int32 phaseIdForDensity;

        List<TblPurchaseVehicleDetailsTO> purchaseScheduleSummaryDetailsTOList;

        List<TblQualityPhaseTO> qualityPhaseTOList = new List<TblQualityPhaseTO>();

        TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = new TblPurchaseWeighingStageSummaryTO();
        TblPurchaseVehicleSpotEntryTO purchaseVehicleSpotEntryTO = new TblPurchaseVehicleSpotEntryTO();
        List<TblRecycleDocumentTO> recycleDocumentTOList = new List<TblRecycleDocumentTO>();

        TblPurchaseInvoiceTO tblPurchaseInvoiceTO = new TblPurchaseInvoiceTO();

        List<DashBoardInfoTO> dashBoardInfoTOList = new List<DashBoardInfoTO>();

        List<TblPurchaseVehLinkSaudaTO> purchaseVehLinkSaudaTOList= new List<TblPurchaseVehLinkSaudaTO>();


        DateTime unloadingDate;
        Int32 isRecovery;
        Int32 recoveryBy;

        Int32 unloadingCompCnt;
        Int32 gradingCompCnt;
        Int32 recoveryCompCnt;
        Int32 wtStageCompCnt;

        DateTime recoveryOn;

        Int32 isWeighing;

        //Priyanka [28-01-2019]
        Int32 commercialApproval;
        string purchaseManager;
        Int32 userId;

        Int32 commercialVerified;

        //Prajakta[2019-02-27] Added
        Int32 isBoth;
        Int32 isFixed;
        double transportAmtPerMT;

        DateTime corretionCompletedOn;

        Int32 isUnloadingCompleted;

        Int32 globalRatePurchaseId;

        Int32 isForCompare;

        Int32 isGetEndDateTime;

        Int32 groupByVehPhaseId;

        double totalImpurities;


        double wtRateApprovalDiff;

        Int32 isGradingUnldCompleted;

        double invoiceQty;
        double height;
        double width;
        double length;
        String createdByName;
        String updatedByName;
        Int32 isStatusUpdate;

        //For Dashboard display
        double totalVehQtyForC;
        double totalVehCntForC;
        double totalVehQtyForNC;
        double totalVehCntForNC;

        double insidePremisesVehQtyForC;
        double insidePremisesCntForC;
        double insidePremisesQtyForNC;
        double insidePremisesVehCntForNC;

        double outsidePremisesVehQtyForC;
        double outsidePremisesCntForC;
        double outsidePremisesQtyForNC;
        double outsidePremisesVehCntForNC;

        double completeVehQtyForC;
        double completeVehCntForC;
        double completeVehQtyForNC;
        double completeVehCntForNC;

        
        double unldDatePadtaPerTonForNC;

        Double processChargePerVeh;
        Double processChargePerMT;
        DateTime gradingComplOn;
        List<string> materialDescList = new List<string>();

        DateTime reportedDate;
        String reportedDateStr;

        string vehRejectReasonDesc;
        string linkSaudaNo;
        //Added By Gokul [14-03-21]
        Double correctionPadtaAmt;
        Double enqQty;

        //Added by minal 25 march 2021  
        Double freightAmount;
        //String partyName;

        #region Add For IoT
        int modbusRefId;
        List<int[]> dynamicItemList = new List<int[]>();
        Dictionary<int, List<int[]>> dynamicItemListDCT = new Dictionary<int, List<int[]>>();
        Int32 gateId;
        string portNumber;
        string ioTUrl;
        string machineIP;
        Int32 isDBup;
        #endregion

        string impuritiesStr;
        #endregion

        #region Constructor
        public TblPurchaseScheduleSummaryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseScheduleSummary
        {
            get { return idPurchaseScheduleSummary; }
            set { idPurchaseScheduleSummary = value; }
        }
        public Int32 IsUnloadingCompleted
        {
            get { return isUnloadingCompleted; }
            set { isUnloadingCompleted = value; }
        }
        public Int32 IsVehicleVerified
        {
            get { return isVehicleVerified; }
            set { isVehicleVerified = value; }
        }
        public Int32 PhotographerId
        {
            get { return photographerId; }
            set { photographerId = value; }
        }

        public string Photographer
        {
            get { return photographer; }
            set { photographer = value; }
        }
        public Int32 VehiclePhaseSequanceNo
        {
            get { return vehiclePhaseSequanceNo; }
            set { vehiclePhaseSequanceNo = value; }
        }


        public Int32 ScheduleHistoryId
        {
            get { return scheduleHistoryId; }
            set { scheduleHistoryId = value; }
        }

        public Int32 AcceptStatusId
        {
            get { return acceptStatusId; }
            set { acceptStatusId = value; }
        }

        public Int32 ForSaveOrSubmit
        {
            get { return forSaveOrSubmit; }
            set { forSaveOrSubmit = value; }
        }
        public Int32 NarrationId
        {
            get { return narrationId; }
            set { narrationId = value; }
        }
        public string Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        public string LotSize
        {
            get { return lotSize; }
            set { lotSize = value; }
        }

        public Int32 IsIgnoreApproval
        {
            get { return isIgnoreApproval; }
            set { isIgnoreApproval = value; }
        }
        public Int32 IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }
        public Int32 IsLatest
        {
            get { return isLatest; }
            set { isLatest = value; }
        }


        public Int32 HistoryPhaseId
        {
            get { return historyPhaseId; }
            set { historyPhaseId = value; }
        }

        public double RateForC
        {
            get { return rateForC; }
            set { rateForC = value; }
        }
        public double RateForNC
        {
            get { return rateForNC; }
            set { rateForNC = value; }
        }
        public double OrgUnloadedQty
        {
            get { return orgUnloadedQty; }
            set { orgUnloadedQty = value; }
        }

        public Int32 RejectStatusId
        {
            get { return rejectStatusId; }
            set { rejectStatusId = value; }
        }

        public Int32 RejectPhaseId
        {
            get { return rejectPhaseId; }
            set { rejectPhaseId = value; }
        }

        public Int32 AcceptPhaseId
        {
            get { return acceptPhaseId; }
            set { acceptPhaseId = value; }
        }

        public Int32 HistoryIsActive
        {
            get { return historyIsActive; }
            set { historyIsActive = value; }
        }

        public string StatusRemark
        {
            get { return statusRemark; }
            set { statusRemark = value; }
        }

        public string NavigationUrl
        {
            get { return navigationUrl; }
            set { navigationUrl = value; }
        }



        public Int32 ParentPurchaseScheduleSummaryId
        {
            get { return parentPurchaseScheduleSummaryId; }
            set { parentPurchaseScheduleSummaryId = value; }
        }
        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
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
        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
        }

        public DateTime OldScheduleDate
        {
            get { return oldScheduleDate; }
            set { oldScheduleDate = value; }
        }
        public String ScheduleDateStr
        {
            get { return scheduleDate.ToString(AzureDateFormat); }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        public String ProdClassDesc
        {
            get { return prodClassDesc; }
            set { prodClassDesc = value; }
        }

        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        public Double OrgScheduleQty
        {
            get { return scheduleQty; }
            set { scheduleQty = value; }
        }
        public Double CalculatedMetalCost
        {
            get { return calculatedMetalCost; }
            set { calculatedMetalCost = value; }
        }
        public Double BaseMetalCost
        {
            get { return baseMetalCost; }
            set { baseMetalCost = value; }
        }
        public Double Padta
        {
            get { return padta; }
            set { padta = value; }
        }

        public Double CalculatedMetalCostForNC
        {
            get { return calculatedMetalCostForNC; }
            set { calculatedMetalCostForNC = value; }
        }
        public Double BaseMetalCostForNC
        {
            get { return baseMetalCostForNC; }
            set { baseMetalCostForNC = value; }
        }
        public Double PadtaForNC
        {
            get { return padtaForNC; }
            set { padtaForNC = value; }
        }

        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }

        public String StatusDesc
        {
            get { return statusDesc; }
            set { statusDesc = value; }
        }



        public String ColorCode
        {
            get { return colorCode; }
            set { colorCode = value; }
        }



        public String SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        public Int32 COrNc
        {
            get { return cOrNc; }
            set { cOrNc = value; }
        }
        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public String MaterailType
        {
            get { return materailType; }
            set { materailType = value; }
        }
        public Double RateBand
        {
            get { return rateBand; }
            set { rateBand = value; }
        }

        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }

        public Int32 EngineerId
        {
            get { return engineerId; }
            set { engineerId = value; }
        }

        public Int32 SupervisorId
        {
            get { return supervisorId; }
            set { supervisorId = value; }
        }

        public Boolean QualityFlag
        {
            get { return qualityFlag; }
            set { qualityFlag = value; }
        }

        public Int32 StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }
        public String StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }

        public String DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        public String DriverContactNo
        {
            get { return driverContactNo; }
            set { driverContactNo = value; }
        }
        public String TransporterName
        {
            get { return transporterName; }
            set { transporterName = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
        }
        public String VehicleTypeName
        {
            get { return vehicleTypeName; }
            set { vehicleTypeName = value; }
        }

        public Double TargetPadta
        {
            get { return targetPadta; }
            set { targetPadta = value; }
        }
        public Double Freight
        {
            get { return freight; }
            set { freight = value; }
        }
        public int IsFreightAdded
        {
            get { return isFreightAdded; }
            set { isFreightAdded = value; }
        }
        public String ContainerNo
        {
            get { return containerNo; }
            set { containerNo = value; }
        }
        public Int32 VehicleStateId
        {
            get { return vehicleStateId; }
            set { vehicleStateId = value; }
        }
        public String VehicleStateName
        {
            get { return vehicleStateName; }
            set { vehicleStateName = value; }
        }
        public String Location
        {
            get { return location; }
            set { location = value; }
        }

        public Int32 SpotEntryVehicleId
        {
            get { return spotEntryVehicleId; }
            set { spotEntryVehicleId = value; }
        }
        public List<TblPurchaseVehicleDetailsTO> PurchaseScheduleSummaryDetailsTOList
        {
            get { return purchaseScheduleSummaryDetailsTOList; }
            set { purchaseScheduleSummaryDetailsTOList = value; }
        }
        public List<TblPurchaseVehLinkSaudaTO> PurchaseVehLinkSaudaTOList
        {
            get { return purchaseVehLinkSaudaTOList; }
            set { purchaseVehLinkSaudaTOList = value; }
        }

        

        public Int32 LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }
        public Int32 COrNcId
        {
            get { return cOrNcId; }
            set { cOrNcId = value; }
        }

        public string SupervisorName
        {
            get { return supervisorName; }
            set { supervisorName = value; }
        }

        public Int32 VehicleCatId
        {
            get { return vehicleCatId; }
            set { vehicleCatId = value; }
        }

        public Int32 PreviousStatusId
        {
            get { return previousStatusId; }
            set { previousStatusId = value; }
        }
        public string PreviousStatusName
        {
            get { return previousStatusName; }
            set { previousStatusName = value; }
        }

        public Int32 CurrentStatusId
        {
            get { return currentStatusId; }
            set { currentStatusId = value; }
        }

        public Int32 VehiclePhaseId
        {
            get { return vehiclePhaseId; }
            set { vehiclePhaseId = value; }
        }
        public string VehiclePhaseName
        {
            get { return vehiclePhaseName; }
            set { vehiclePhaseName = value; }
        }

        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }


        public Int32 PreviousParentId
        {
            get { return previousParentId; }
            set { previousParentId = value; }
        }
        public TblPurchaseWeighingStageSummaryTO TblPurchaseWeighingStageSummaryTO
        {
            get { return tblPurchaseWeighingStageSummaryTO; }
            set { tblPurchaseWeighingStageSummaryTO = value; }
        }

        public Int32 RootScheduleId
        {
            get { return rootScheduleId; }
            set { rootScheduleId = value; }
        }

        public Int32 GraderId
        {
            get { return graderId; }
            set { graderId = value; }
        }
        public Int32 IsGradingCompleted
        {
            get { return isGradingCompleted; }
            set { isGradingCompleted = value; }
        }
        public Int32 IsCorrectionCompleted
        {
            get { return isCorrectionCompleted; }
            set { isCorrectionCompleted = value; }
        }
        public TblPurchaseVehicleSpotEntryTO PurchaseVehicleSpotEntryTO
        {
            get { return purchaseVehicleSpotEntryTO; }
            set { purchaseVehicleSpotEntryTO = value; }
        }

        public TblPurchaseInvoiceTO TblPurchaseInvoiceTO
        {
            get { return tblPurchaseInvoiceTO; }
            set { tblPurchaseInvoiceTO = value; }
        }

        public string EnqDisplayNo
        {
            get { return enqDisplayNo; }
            set { enqDisplayNo = value; }
        }

        public List<TblRecycleDocumentTO> RecycleDocumentTOList
        {
            get { return recycleDocumentTOList; }
            set { recycleDocumentTOList = value; }
        }

          public List<DashBoardInfoTO> DashBoardInfoTOList
        {
            get { return dashBoardInfoTOList; }
            set { dashBoardInfoTOList = value; }
        }

        public List<TblQualityPhaseTO> QualityPhaseTOList
        {
            get { return qualityPhaseTOList; }
            set { qualityPhaseTOList = value; }
        }

        public int IsRecovery { get => isRecovery; set => isRecovery = value; }
        public int RecoveryBy { get => recoveryBy; set => recoveryBy = value; }
        public int UnloadingCompCnt { get => unloadingCompCnt; set => unloadingCompCnt = value; }
        public int GradingCompCnt { get => gradingCompCnt; set => gradingCompCnt = value; }
        public int RecoveryCompCnt { get => recoveryCompCnt; set => recoveryCompCnt = value; }

        public int WtStageCompCnt { get => wtStageCompCnt; set => wtStageCompCnt = value; }
        public DateTime RecoveryOn { get => recoveryOn; set => recoveryOn = value; }
        public int IsWeighing { get => isWeighing; set => isWeighing = value; }

        public TblPurchaseScheduleSummaryTO DeepCopy()
        {
            TblPurchaseScheduleSummaryTO other = (TblPurchaseScheduleSummaryTO)this.MemberwiseClone();
            return other;
        }

        public int CommercialApproval { get => commercialApproval; set => commercialApproval = value; }
        public string PurchaseManager { get => purchaseManager; set => purchaseManager = value; }
        public int UserId { get => userId; set => userId = value; }

        public string GreaderName
        {
            get { return greaderName; }
            set { greaderName = value; }
        }

        public string EngineerName
        {
            get { return engineerName; }
            set { engineerName = value; }
        }

        public Int32 IsBoth
        {
            get { return isBoth; }
            set { isBoth = value; }
        }

        public Int32 IsFixed
        {
            get { return isFixed; }
            set { isFixed = value; }
        }

        public Double EnqQty
        {
            get { return enqQty; }
            set { enqQty = value; }
        }
        public double TransportAmtPerMT
        {
            get { return transportAmtPerMT; }
            set { transportAmtPerMT = value; }
        }

        public double RejectedQty
        {
            get { return rejectedQty; }
            set { rejectedQty = value; }
        }

        public Int32 RejectedBy
        {
            get { return rejectedBy; }
            set { rejectedBy = value; }
        }

        public DateTime RejectedOn
        {
            get { return rejectedOn; }
            set { rejectedOn = value; }
        }

        public string VehRejectReasonId
        {
            get { return vehRejectReasonId; }
            set { vehRejectReasonId = value; }
        }

        public string RejectedByUserName
        {
            get { return rejectedByUserName; }
            set { rejectedByUserName = value; }
        }


        public DateTime CorretionCompletedOn
        {
            get { return corretionCompletedOn; }
            set { corretionCompletedOn = value; }
        }



        public Int32 GlobalRatePurchaseId
        {
            get { return globalRatePurchaseId; }
            set { globalRatePurchaseId = value; }
        }

        public Int32 IsForCompare
        {
            get { return isForCompare; }
            set { isForCompare = value; }
        }


        public Int32 GroupByVehPhaseId
        {
            get { return groupByVehPhaseId; }
            set { groupByVehPhaseId = value; }
        }

        public Int32 IsGetEndDateTime
        {
            get { return isGetEndDateTime; }
            set { isGetEndDateTime = value; }
        }


        public double WtRateApprovalDiff
        {
            get { return wtRateApprovalDiff; }
            set { wtRateApprovalDiff = value; }
        }

        public Int32 ApprovalType
        {
            get { return approvalType; }
            set { approvalType = value; }
        }

        public Int32 IsAutoSpotVehSauda
        {
            get { return isAutoSpotVehSauda; }
            set { isAutoSpotVehSauda = value; }
        }
      
        
        public string CorreNarration
        {
            get { return correNarration; }
            set { correNarration = value; }
        }

        public string SaudaNarration
        {
            get { return saudaNarration; }
            set { saudaNarration = value; }
        }

        public string SaudaSupplierName
        {
            get { return saudaSupplierName; }
            set { saudaSupplierName = value; }
        }

        public string ImpuritiesStr
        {
            get { return impuritiesStr; }
            set { impuritiesStr = value; }
        }

        public double TotalImpurities
        {
            get { return totalImpurities; }
            set { totalImpurities = value; }
        }


        public DateTime LatestWtTakenOn
        {
            get { return latestWtTakenOn; }
            set { latestWtTakenOn = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }
        public double Width
        {
            get { return width; }
            set { width = value; }
        }
        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        public Int32 ActualRootScheduleId
        {
            get
            {
                if (this.rootScheduleId > 0)
                    return this.rootScheduleId;
                else
                    return this.idPurchaseScheduleSummary;
            }

        }

        public int CommercialVerified { get => commercialVerified; set => commercialVerified = value; }

        public String TypeName
        {
            get
            {
                if (this.IsBoth == 1)
                    return "Both";
                else if (this.COrNcId == (Int32)StaticStuff.Constants.ConfirmTypeE.CONFIRM)
                    return "Order";
                else
                    return "Enquiry";

            }

        }

        public class DashBoardInfoTO
        { 
            public string materialTypeStr;
            public string MaterialTypeStr { get => materialTypeStr; set => materialTypeStr = value; }

            public string materialTypeQty;
            public string MaterialTypeQty
            {
                get { return materialTypeQty; }
                set
                {
                    materialTypeQty = value;
                }
            }


    }

        public int ModbusRefId { get => modbusRefId; set => modbusRefId = value; }
        public List<int[]> DynamicItemList { get => dynamicItemList; set => dynamicItemList = value; }
        public Dictionary<int, List<int[]>> DynamicItemListDCT { get => dynamicItemListDCT; set => dynamicItemListDCT = value; }
        public int GateId { get => gateId; set => gateId = value; }
        public string PortNumber { get => portNumber; set => portNumber = value; }
        public string IoTUrl { get => ioTUrl; set => ioTUrl = value; }
        public string MachineIP { get => machineIP; set => machineIP = value; }
        public int IsDBup { get => isDBup; set => isDBup = value; }
        public int IsVehicleOut { get => isVehicleOut; set => isVehicleOut = value; }

        public int IsGradingUnldCompleted { get => isGradingUnldCompleted; set => isGradingUnldCompleted = value; }

        public double InvoiceQty { get => invoiceQty; set => invoiceQty = value; }

        public double RefRateofV48Var { get => refRateofV48Var; set => refRateofV48Var = value; }

        public double RefRateC { get => refRateC; set => refRateC = value; }


        public string CreatedByName { get => createdByName; set => createdByName = value; }
        public string UpdatedByName { get => updatedByName; set => updatedByName = value; }

        public Int32 IsStatusUpdate { get => isStatusUpdate; set => isStatusUpdate = value; }
        

        public int PhaseIdForDensity { get => phaseIdForDensity; set => phaseIdForDensity = value; }

        public string CorretionCompletedOnStr { get => corretionCompletedOnStr; set => corretionCompletedOnStr = value; }
        //public string CorretionCompletedOnStr { get; internal set; }
        //public String CorretionCompletedOnStr
        //{
        //    get { return CorretionCompletedOn.ToString(StaticStuff.Constants.DefaultDateFormat); }
        //}

        public String UpdatedOnStr
        {
            get {
                if (UpdatedOn == new DateTime())
                    return "";
                else
                    return UpdatedOn.ToString(StaticStuff.Constants.DefaultDateFormat);
        }
        }


        public double Density { get; internal set; }
        public double RefRateForSauda { get; internal set; }

        public double RefRateForSaudaNC { get; internal set; }

        public double CorrectionAmount { get; internal set; }
        public double PadtaBeforeCorrection { get; internal set; }
        public double MarketFluctuations { get; internal set; }
        public double BookingLoss { get; internal set; }
        public double QualityEffects { get; internal set; }



        public double TotalVehQtyForC { get => totalVehQtyForC; set => totalVehQtyForC = value; }
        public double TotalVehCntForC { get => totalVehCntForC; set => totalVehCntForC = value; }

        public double TotalVehQtyForNC { get => totalVehQtyForNC; set => totalVehQtyForNC = value; }
        public double TotalVehCntForNC { get => totalVehCntForNC; set => totalVehCntForNC = value; }

        public double InsidePremisesVehQtyForC { get => insidePremisesVehQtyForC; set => insidePremisesVehQtyForC = value; }

        public double InsidePremisesCntForC { get => insidePremisesCntForC; set => insidePremisesCntForC = value; }

        public double InsidePremisesQtyForNC { get => insidePremisesQtyForNC; set => insidePremisesQtyForNC = value; }

        public double InsidePremisesVehCntForNC { get => insidePremisesVehCntForNC; set => insidePremisesVehCntForNC = value; }

        public double OutsidePremisesVehQtyForC { get => outsidePremisesVehQtyForC; set => outsidePremisesVehQtyForC = value; }

        public double OutsidePremisesCntForC { get => outsidePremisesCntForC; set => outsidePremisesCntForC = value; }

        public double OutsidePremisesQtyForNC { get => outsidePremisesQtyForNC; set => outsidePremisesQtyForNC = value; }

        public double OutsidePremisesVehCntForNC { get => outsidePremisesVehCntForNC; set => outsidePremisesVehCntForNC = value; }

        public double CompleteVehQtyForC { get => completeVehQtyForC; set => completeVehQtyForC = value; }

        public double CompleteVehCntForC { get => completeVehCntForC; set => completeVehCntForC = value; }

        public double CompleteVehQtyForNC { get => completeVehQtyForNC; set => completeVehQtyForNC = value; }

        public double CompleteVehCntForNC { get => completeVehCntForNC; set => completeVehCntForNC = value; }

        public double UnldDateBaseMetalCost { get => unldDateBaseMetalCost; set => unldDateBaseMetalCost = value; }
        public double UnldDatePadtaPerTon { get => unldDatePadtaPerTon; set => unldDatePadtaPerTon = value; }

        //public double UnldDateBaseMetalCost { get; internal set; }
        //public double UnldDatePadtaPerTon { get; internal set; }

        public double TotalSpotVehQty { get => totalSpotVehQty; set => totalSpotVehQty = value; }

        public double TotalSpotVehCnt { get => totalSpotVehCnt; set => totalSpotVehCnt = value; }

        public double OtherSpotVehQty { get => otherSpotVehQty; set => otherSpotVehQty = value; }
        public double OtherSpotVehCnt { get => otherSpotVehCnt; set => otherSpotVehCnt = value; }

        public List<string> MaterialDescList { get => materialDescList; set => materialDescList = value; }


         public DateTime UnloadingDate { get => unloadingDate; set => unloadingDate = value; }

        public Double UnldDatePadtaPerTonForNC { get => unldDatePadtaPerTonForNC; set => unldDatePadtaPerTonForNC = value; }

        public Double Total { get => total; set => total = value; }
        public string COrNcTypeName { get; internal set; }
        public DateTime ReportedDate { get => reportedDate; set => reportedDate = value; }
        public string ReportedDateStr
        {
            get
            {
                if (reportedDate == new DateTime())
                { return ""; }
                else
                {
                    return reportedDate.ToString(StaticStuff.Constants.DefaultDateFormat);
                }
            }
        }

        public string VehRejectReasonDesc { get => vehRejectReasonDesc; set => vehRejectReasonDesc = value; }

        public string LinkSaudaNo { get => linkSaudaNo; set => linkSaudaNo = value; }

        public Double ProcessChargePerVeh { get => processChargePerVeh; set => processChargePerVeh = value; }

        public Double ProcessChargePerMT { get => processChargePerMT; set => processChargePerMT = value; }

        public Int32 SaudaTypeId { get => saudaTypeId; set => saudaTypeId = value; }

        public DateTime GradingComplOn { get => gradingComplOn; set => gradingComplOn = value; }


        public Double CorrectionPadtaAmt
        {
            get { return correctionPadtaAmt; }
            set { correctionPadtaAmt = value; }
        }
        public string GradingComplOnStr
        {
            get
            {
                if (gradingComplOn == new DateTime())
                { return ""; }
                else
                {
                    return gradingComplOn.ToString(StaticStuff.Constants.DefaultDateFormat);
                }
            }
        }

        public string LatestWtTakenOnStr
        {
            get
            {
                if (latestWtTakenOn == new DateTime())
                { return ""; }
                else
                {
                    return latestWtTakenOn.ToString(StaticStuff.Constants.DefaultDateFormat);
                }
            }
        }


        public Double FreightAmount
        {
            get { return freightAmount; }
            set { freightAmount = value; }

        }

        //public String PartyName
        //{
        //    get { return partyName; }
        //    set { partyName = value; }
        //}
        #endregion
    }
}
