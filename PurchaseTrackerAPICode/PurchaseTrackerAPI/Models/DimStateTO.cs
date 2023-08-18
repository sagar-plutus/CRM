using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class DimStateTO
    {
        #region Declarations
        Int32 idState;
        String stateCode;
        String stateName;
        String stateOrUTCode;
        #endregion

        #region Constructor
        public DimStateTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdState
        {
            get { return idState; }
            set { idState = value; }
        }
        public String StateCode
        {
            get { return stateCode; }
            set { stateCode = value; }
        }
        public String StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }
        public String StateOrUTCode
        {
            get { return stateOrUTCode; }
            set { stateOrUTCode = value; }
        }
        #endregion
    }
}
