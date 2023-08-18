using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblUnloadingStandDescTO
    {
        #region Declarations
        Int32 idUnloadingStandDesc;
        Int32 isActive;
        String standardDesc;
        String remark;
        #endregion

        #region Constructor
        public TblUnloadingStandDescTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUnloadingStandDesc
        {
            get { return idUnloadingStandDesc; }
            set { idUnloadingStandDesc = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String StandardDesc
        {
            get { return standardDesc; }
            set { standardDesc = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        #endregion
    }
}
