using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblQualityPhaseDtlsTO
    {
        #region Declarations
        Int32 idTblQualityPhaseDtls;
        Int32 tblQualityPhaseId;
        Int32 qualitySampleTypeId;

        Int32 qualitySampleTypeParentId;
        string qualitySampleTypeName;
        Int32 flagStatusId;

        Boolean flagStatusIdBool;
        Int32 createdBy;
        Int32 updatedBy;

        Int32 isSelected;
        Boolean isChecked;

        DateTime createdOn;
        DateTime updatedOn;

        DateTime statusOn;
        Int32 statusBy;

        string remark;
        string sampleTypeName;

        string vehiclePhaseName;

        int flagTypeId;

        #endregion

        #region Constructor
        public TblQualityPhaseDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTblQualityPhaseDtls
        {
            get { return idTblQualityPhaseDtls; }
            set { idTblQualityPhaseDtls = value; }
        }
        public Int32 TblQualityPhaseId
        {
            get { return tblQualityPhaseId; }
            set { tblQualityPhaseId = value; }
        }
        public Int32 QualitySampleTypeId
        {
            get { return qualitySampleTypeId; }
            set { qualitySampleTypeId = value; }
        }

        public Int32 FlagTypeId
        {
            get { return flagTypeId; }
            set { flagTypeId = value; }
        }
        public Int32 QualitySampleTypeParentId
        {
            get { return qualitySampleTypeParentId; }
            set { qualitySampleTypeParentId = value; }
        }
        public string QualitySampleTypeName
        {
            get { return qualitySampleTypeName; }
            set { qualitySampleTypeName = value; }
        }

        public string VehiclePhaseName
        {
            get { return vehiclePhaseName; }
            set { vehiclePhaseName = value; }
        }

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public string SampleTypeName
        {
            get { return sampleTypeName; }
            set { sampleTypeName = value; }
        }
        public Int32 FlagStatusId
        {
            get { return flagStatusId; }
            set { flagStatusId = value; }
        }
        public Int32 IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
        public Boolean IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public Boolean FlagStatusIdBool
        {
            get { return flagStatusIdBool; }
            set { flagStatusIdBool = value; }
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

        public DateTime StatusOn { get => statusOn; set => statusOn = value; }
        public int StatusBy { get => statusBy; set => statusBy = value; }
        #endregion
    }
}
