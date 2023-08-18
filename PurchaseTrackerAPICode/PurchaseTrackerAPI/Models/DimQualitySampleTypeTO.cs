using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class DimQualitySampleTypeTO
    {
        #region Declarations
        Int32 idQualitySampleType;
        Int32 parentSampleTypeId;
        Int32 isActive;
        Int32 seqNo;
        String sampleTypeName;
        String sampleTypeDesc;
        Int32 phaseId;
        Int32 flagTypeId;

        #endregion

        #region Constructor
        public DimQualitySampleTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdQualitySampleType
        {
            get { return idQualitySampleType; }
            set { idQualitySampleType = value; }
        }
        public Int32 ParentSampleTypeId
        {
            get { return parentSampleTypeId; }
            set { parentSampleTypeId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 SeqNo
        {
            get { return seqNo; }
            set { seqNo = value; }
        }
        public String SampleTypeName
        {
            get { return sampleTypeName; }
            set { sampleTypeName = value; }
        }
        public String SampleTypeDesc
        {
            get { return sampleTypeDesc; }
            set { sampleTypeDesc = value; }
        }

        public int PhaseId
        {
            get { return phaseId; }
            set { phaseId = value; }
        }
         public int FlagTypeId
        {
            get { return flagTypeId; }
            set { flagTypeId = value; }
        }
        #endregion
    }
}
