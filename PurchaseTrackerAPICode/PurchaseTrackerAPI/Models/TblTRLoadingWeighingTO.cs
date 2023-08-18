using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblTRLoadingWeighingTO
    {
        #region Declarations
        Int32 idWeighing;
        String vehicleNo;
        Int32 loadingId;
        Int32 loadingTypeId;
        Decimal tareWeight;
        Decimal grossWeight;
        Decimal actualWeight;
        Decimal netWeight;
        Decimal totalNetWeight;
        String rstNumber;
        Int32 weighingStageId;
        Int32 weighingMachineId;
        Int32 weighingMeasureTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 isActive;
        Int32 srNo;
        String machineName;
        Int32 isWeighingCompleted;
        #endregion
        #region Constructor
        public TblTRLoadingWeighingTO()
        {
        }
        #endregion
        #region GetSet
        public int IdWeighing { get => idWeighing; set => idWeighing = value; }
        public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        public int LoadingId { get => loadingId; set => loadingId = value; }
        public int LoadingTypeId { get => loadingTypeId; set => loadingTypeId = value; }
        public decimal TareWeight { get => tareWeight; set => tareWeight = value; }
        public decimal GrossWeight { get => grossWeight; set => grossWeight = value; }
        public decimal ActualWeight { get => actualWeight; set => actualWeight = value; }
        public decimal NetWeight { get => netWeight; set => netWeight = value; }
        public string RstNumber { get => rstNumber; set => rstNumber = value; }
        public int WeighingStageId { get => weighingStageId; set => weighingStageId = value; }
        public int WeighingMachineId { get => weighingMachineId; set => weighingMachineId = value; }
        public int WeighingMeasureTypeId { get => weighingMeasureTypeId; set => weighingMeasureTypeId = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public int UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public int SrNo { get => srNo; set => srNo = value; }
        public string MachineName { get => machineName; set => machineName = value; }
        public int IsWeighingCompleted { get => isWeighingCompleted; set => isWeighingCompleted = value; }
        public decimal TotalNetWeight { get => totalNetWeight; set => totalNetWeight = value; }
        #endregion
    }
}
