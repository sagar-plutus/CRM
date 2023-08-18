using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class VisitFirmDetailsTO
    {
        #region Declarations

        Int32 firmId;
        String firmAdd;
        List<TblPersonTO> firmOwnerList;

        #endregion

        #region GetSet

        public Int32 FirmId
        {
            get { return firmId; }
            set { firmId = value; }
        }

        public String FirmAdd
        {
            get { return firmAdd; }
            set { firmAdd = value; }
        }

        public List<TblPersonTO> FirmOwnerList
        {
            get { return FirmOwnerList; }
            set { FirmOwnerList = value; }
        }

        #endregion
    }
}
