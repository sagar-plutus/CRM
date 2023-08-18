using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class DimWeightMeasrTypesTO
    {
        #region Declarations
        Int32 idWeightMeasurType;
        Int32 isActive;
        String weightMeasurTypeDesc;
        #endregion

        #region Constructor
        public DimWeightMeasrTypesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdWeightMeasurType
        {
            get { return idWeightMeasurType; }
            set { idWeightMeasurType = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String WeightMeasurTypeDesc
        {
            get { return weightMeasurTypeDesc; }
            set { weightMeasurTypeDesc = value; }
        }
        #endregion
    }
}
