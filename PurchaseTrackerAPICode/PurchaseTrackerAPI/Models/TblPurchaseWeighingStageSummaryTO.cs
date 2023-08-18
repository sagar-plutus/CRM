using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseWeighingStageSummaryTO
    {
        #region Declarations
        Int32 correctionRecId;
        Int32 idPurchaseWeighingStage;
        Int32 weighingMachineId;
        Int32 supplierId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double grossWeightMT;
        Double actualWeightMT;
        Double netWeightMT;
        String rstNumber;
        String vehicleNo;
        Int32 purchaseScheduleSummaryId;
        Int32 weightMeasurTypeId;
        string weightMeasurTypeName;
        Int32 weightStageId;
        Int32 machineCalibrationId;

        Boolean isUpdateIsWeigingFlag;
        double partyTareWt;
        double partyNetWt;
        double partyGrossWt;

        string supplierName;

        List<TblPurchaseUnloadingDtlTO> purchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();

        List<TblPurchaseUnloadingDtlTO> afterGradingUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
        List<TblPurchaseGradingDtlsTO> purchaseGradingDtlsTOList = new List<TblPurchaseGradingDtlsTO>();

        int isValid;
        Double recoveryPer;
        Int32 recoveryBy;
        DateTime recoveryOn;
        Int32 isRecConfirm;

        Boolean isSaveWtStage;

        TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = null;

        string isValidInfo;
        string isStorageExcess;
        Double freight;
        Double advance;
        Double unloadingQty;
        Double shortage;
        Double amount;
        int supervisorId;
        Double totalNetWtOfVeh;

        Int32 vehiclePhaseId;
        Int64 intervalTime;
        DateTime gradingStartTime;
        DateTime gradingEndTime;
        String createdOnStr;
        String supervisor;
        #endregion

        #region Constructor
        public TblPurchaseWeighingStageSummaryTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime GradingStartTime
        {
            get { return gradingStartTime; }
            set { gradingStartTime = value; }
        }

        public DateTime GradingEndTime
        {
            get { return gradingEndTime; }
            set { gradingEndTime = value; }
        }
        public Int32 SupervisorId
        {
            get { return supervisorId; }
            set { supervisorId = value; }
        }
        public Int64 IntervalTime
        {
            get { return intervalTime;}
            set { intervalTime = value; }
        }
        public double PartyTareWt { get => partyTareWt; set => partyTareWt = value; }
        public Int32 IdPurchaseWeighingStage
        {
            get { return idPurchaseWeighingStage; }
            set { idPurchaseWeighingStage = value; }
        }
        public Int32 WeighingMachineId
        {
            get { return weighingMachineId; }
            set { weighingMachineId = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        public Int32 IsValid
        {
            get { return isValid; }
            set { isValid = value; }
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
        public Double GrossWeightMT
        {
            get { return grossWeightMT; }
            set { grossWeightMT = value; }
        }
        public Double ActualWeightMT
        {
            get { return actualWeightMT; }
            set { actualWeightMT = value; }
        }
        public Double NetWeightMT
        {
            get { return netWeightMT; }
            set { netWeightMT = value; }
        }
        public String RstNumber
        {
            get { return rstNumber; }
            set { rstNumber = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public Int32 WeightMeasurTypeId
        {
            get { return weightMeasurTypeId; }
            set { weightMeasurTypeId = value; }
        }

        public string WeightMeasurTypeName
        {
            get { return weightMeasurTypeName; }
            set { weightMeasurTypeName = value; }
        }
        public Int32 WeightStageId
        {
            get { return weightStageId; }
            set { weightStageId = value; }
        }
        
        public Int32 CorrectionRecId
        {
            get { return correctionRecId; }
            set { correctionRecId = value; }
        }
        public Int32 MachineCalibrationId
        {
            get { return machineCalibrationId; }
            set { machineCalibrationId = value; }
        }
        public List<TblPurchaseUnloadingDtlTO> PurchaseUnloadingDtlTOList
        {
            get { return purchaseUnloadingDtlTOList; }
            set { purchaseUnloadingDtlTOList = value; }
        }

        public List<TblPurchaseUnloadingDtlTO> AfterGradingUnloadingDtlTOList
        {
            get { return afterGradingUnloadingDtlTOList; }
            set { afterGradingUnloadingDtlTOList = value; }
        }
        public List<TblPurchaseGradingDtlsTO> PurchaseGradingDtlsTOList
        {
            get { return purchaseGradingDtlsTOList; }
            set { purchaseGradingDtlsTOList = value; }
        }

        public double RecoveryPer { get => recoveryPer; set => recoveryPer = value; }
        public int RecoveryBy { get => recoveryBy; set => recoveryBy = value; }
        public DateTime RecoveryOn { get => recoveryOn; set => recoveryOn = value; }
        public int IsRecConfirm { get => isRecConfirm; set => isRecConfirm = value; }
        public Boolean IsUpdateIsWeigingFlag { get => isUpdateIsWeigingFlag; set => isUpdateIsWeigingFlag = value; }

        public Boolean IsSaveWtStage { get => isSaveWtStage; set => isSaveWtStage = value; }

        public double PartyNetWt { get => partyNetWt; set => partyNetWt = value; }
        public double PartyGrossWt { get => partyGrossWt; set => partyGrossWt = value; }

        public string SupplierName { get => supplierName; set => supplierName = value; }

        public string IsValidInfo { get => isValidInfo; set => isValidInfo = value; }

        public string IsStorageExcess { get => isStorageExcess; set => isStorageExcess = value; }

        public Double Freight { get => freight; set => freight = value; }

        public Double Advance { get => advance; set => advance = value; }

        public Double UnloadingQty { get => unloadingQty; set => unloadingQty = value; }

        public Double Shortage { get => shortage; set => shortage = value; }

        public Double Amount { get => amount; set => amount = value; }

        public Double TotalNetWtOfVeh { get => totalNetWtOfVeh; set => totalNetWtOfVeh = value; }

        public Int32 VehiclePhaseId { get => vehiclePhaseId; set => vehiclePhaseId = value; }
        public string CreatedOnStr { get => createdOnStr; set => createdOnStr = value; }

        public string Supervisor { get => supervisor; set => supervisor = value; }

        public Int32 UnloadingConfirmedBy { get; set; } //[2021-10-04] Dhananjay Added
        public string UnloadingConfirmedByUser { get; set; } //[2021-10-04] Dhananjay Added

        public TblPurchaseWeighingStageSummaryTO DeepCopy()
        {
            TblPurchaseWeighingStageSummaryTO other = (TblPurchaseWeighingStageSummaryTO)this.MemberwiseClone();
            return other;
        }

        #endregion
    }

    public class GradeInfoTO
    {
        String gradeName;
        Double gradeQtyUnloaded;
        int prodItemId;
        Boolean isDustRecord;
        Boolean isTotalRecord;
        Boolean isGrandTotalRecord;
        public Boolean IsDustRecord
        {
            get { return isDustRecord; }
            set { isDustRecord = value; }
        }
        public Boolean IsTotalRecord
        {
            get { return isTotalRecord; }
            set { isTotalRecord = value; }
        }
        public Boolean IsGrandTotalRecord
        {
            get { return isGrandTotalRecord; }
            set { isGrandTotalRecord = value; }
        }
        public int ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Double GradeQtyUnloaded
        {
            get { return gradeQtyUnloaded; }
            set { gradeQtyUnloaded = value; }
        }
        public string GradeName
        {
            get { return gradeName; }
            set { gradeName = value; }
        }

    }
    public class UnloadedTimeReportTO
    {
        string vehicleNo;
        string unloadingPointName;
        string graderName;
        double avgUnloadingTime;
        string partyName;
        string gradeUnloaded;
        string quantityUnloaded;
        Boolean isMax;
        public Boolean IsMax
        {
            get { return isMax; }
            set { isMax = value; }
        }
        public string VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public string UnloadingPointName
        {
            get { return unloadingPointName; }
            set { unloadingPointName = value; }
        }
        public double AvgUnloadingTime
        {
            get { return avgUnloadingTime; }
            set { avgUnloadingTime = value; }
        }
        public string GraderName
        {
            get { return graderName; }
            set { graderName = value; }
        }
        public string PartyName
        {
            get { return partyName; }
            set { partyName = value; }
        }
        public string GradeUnloaded
        {
            get { return gradeUnloaded; }
            set { gradeUnloaded = value; }
        }
        public string QuantityUnloaded
        {
            get { return quantityUnloaded; }
            set { quantityUnloaded = value; }
        }
    }
}
