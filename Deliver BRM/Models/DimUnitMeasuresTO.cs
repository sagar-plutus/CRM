using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class DimUnitMeasuresTO
    {
        #region Declarations
        Int32 idWeightMeasurUnit;
        Int32 isActive;
        String weightMeasurUnitDesc;
        Int32 unitCatId; //Sudhir[23-May-2018]
        #endregion

        #region Constructor
        public DimUnitMeasuresTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdWeightMeasurUnit
        {
            get { return idWeightMeasurUnit; }
            set { idWeightMeasurUnit = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String WeightMeasurUnitDesc
        {
            get { return weightMeasurUnitDesc; }
            set { weightMeasurUnitDesc = value; }
        }

        public int UnitCatId { get => unitCatId; set => unitCatId = value; }
        #endregion
    }
}
