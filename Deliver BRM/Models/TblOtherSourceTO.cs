using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblOtherSourceTO
    {
        #region Declarations
        Int32 idOtherSource;
        Int32 createdBy;
        DateTime createdOn;
        String otherDesc;
        #endregion

        #region Constructor
        public TblOtherSourceTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdOtherSource
        {
            get { return idOtherSource; }
            set { idOtherSource = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String OtherDesc
        {
            get { return otherDesc; }
            set { otherDesc = value; }
        }
        #endregion
    }
}
