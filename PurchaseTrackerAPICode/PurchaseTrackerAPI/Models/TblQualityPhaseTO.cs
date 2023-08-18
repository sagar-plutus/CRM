using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
namespace PurchaseTrackerAPI.Models
{
    public class TblQualityPhaseTO
    {
        #region Declarations
        Int32 idTblQualityPhase;
        Int32 purchaseScheduleSummaryId;
        Int32 vehiclePhaseId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 flagStatusId;
        DateTime createdOn;
        DateTime updatedOn;
        string vehiclePhaseName;
        Int32 flagTypeId ;

        Int32 vehiclePhaseSequanceNo;
        List<TblQualityPhaseDtlsTO> qualityPhaseDtlsTOList = new List<TblQualityPhaseDtlsTO>();
        #endregion

        #region Constructor
        public TblQualityPhaseTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTblQualityPhase
        {
            get { return idTblQualityPhase; }
            set { idTblQualityPhase = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public Int32 VehiclePhaseId
        {
            get { return vehiclePhaseId; }
            set { vehiclePhaseId = value; }
        }
          public Int32 VehiclePhaseSequanceNo
        {
            get { return vehiclePhaseSequanceNo; }
            set { vehiclePhaseSequanceNo = value; }
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
         public Int32 FlagTypeId
        {
            get { return flagTypeId; }
            set { flagTypeId = value; }
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
        public Int32 FlagStatusId
        {
            get { return flagStatusId; }
            set { flagStatusId = value; }
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

        public List<TblQualityPhaseDtlsTO> QualityPhaseDtlsTOList
        {
            get { return qualityPhaseDtlsTOList; }
            set { qualityPhaseDtlsTOList = value; }
        }

        #endregion
    }
}
