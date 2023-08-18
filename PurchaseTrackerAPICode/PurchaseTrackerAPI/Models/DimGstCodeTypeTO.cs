using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class DimGstCodeTypeTO
    {
        #region Declarations
        Int32 idCodeType;
        DateTime createdOn;
        String codeDesc;
        #endregion

        #region Constructor
        public DimGstCodeTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdCodeType
        {
            get { return idCodeType; }
            set { idCodeType = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String CodeDesc
        {
            get { return codeDesc; }
            set { codeDesc = value; }
        }
        #endregion
    }
}
