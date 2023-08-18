using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblWeighingMeasuresTO 
    {
        #region Declarations
        Int32 idWeightMeasure;
        Int32 weighingMachineId;
        Int32 loadingId;
        Int32 weightMeasurTypeId;
        Int32 isConfirmed;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double weightMT;
        String vehicleNo;
        Int32 machineCalibrationId;
        Int32 isLoadingCompleted;
        Int32 unLoadingId;
        Int32 isCheckInvoiceGenerated;

        #endregion

        #region Constructor
        public TblWeighingMeasuresTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdWeightMeasure
        {
            get { return idWeightMeasure; }
            set { idWeightMeasure = value; }
        }
        public Int32 WeighingMachineId
        {
            get { return weighingMachineId; }
            set { weighingMachineId = value; }
        }
        public Int32 LoadingId
        {
            get { return loadingId; }
            set { loadingId = value; }
        }
        public Int32 WeightMeasurTypeId
        {
            get { return weightMeasurTypeId; }
            set { weightMeasurTypeId = value; }
        }
        public Int32 IsConfirmed
        {
            get { return isConfirmed; }
            set { isConfirmed = value; }
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
        public Double WeightMT
        {
            get { return weightMT; }
            set { weightMT = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public Int32 MachineCalibrationId
        {
            get { return machineCalibrationId; }
            set { machineCalibrationId = value; }
        }
        public Int32 UnLoadingId
        {
            get { return unLoadingId; }
            set { unLoadingId = value; }
        }



        public int IsLoadingCompleted { get => isLoadingCompleted; set => isLoadingCompleted = value; }
        public Int32 IsCheckInvoiceGenerated { get => isCheckInvoiceGenerated; set => isCheckInvoiceGenerated = value; }    
        #endregion
    }

    public class TblWeghingMessureDtlsTO
    {
        #region Declarations
        Int32 idWeightMeasure;
        Int32 weighingMachineId;
        Int32 loadingId;
        Int32 weightMeasurTypeId;
        Int32 layerId;
        #endregion

        #region Constructor
        public TblWeghingMessureDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdWeightMeasure
        {
            get { return idWeightMeasure; }
            set { idWeightMeasure = value; }
        }
        public Int32 WeighingMachineId
        {
            get { return weighingMachineId; }
            set { weighingMachineId = value; }
        }
        public Int32 LoadingId
        {
            get { return loadingId; }
            set { loadingId = value; }
        }
        public Int32 WeightMeasurTypeId
        {
            get { return weightMeasurTypeId; }
            set { weightMeasurTypeId = value; }
        }
        public Int32 LayerId
        {
            get { return layerId; }
            set { layerId = value; }
        }
        #endregion
    }
}
