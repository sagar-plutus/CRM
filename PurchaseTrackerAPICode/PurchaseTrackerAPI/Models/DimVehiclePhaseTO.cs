using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class DimVehiclePhaseTO
    {
        #region Declarations
        Int32 idVehiclePhase;
        String phaseName;
        Int32 sequanceNo;
        #endregion

        #region Constructor
        public DimVehiclePhaseTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVehiclePhase
        {
            get { return idVehiclePhase; }
            set { idVehiclePhase = value; }
        }

        public Int32 SequanceNo
        {
            get { return sequanceNo; }
            set { sequanceNo = value; }
        }
        public String PhaseName
        {
            get { return phaseName; }
            set { phaseName = value; }
        }
        #endregion
    }
}
