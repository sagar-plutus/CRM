using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class DimFinYearTO
    {
        #region Declarations
        Int32 idFinYear;
        DateTime finYearStartDate;
        DateTime finYearEndDate;
        String finYearDisplayName;
        #endregion

        #region Constructor
        public DimFinYearTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdFinYear
        {
            get { return idFinYear; }
            set { idFinYear = value; }
        }
        public DateTime FinYearStartDate
        {
            get { return finYearStartDate; }
            set { finYearStartDate = value; }
        }
        public DateTime FinYearEndDate
        {
            get { return finYearEndDate; }
            set { finYearEndDate = value; }
        }
        public String FinYearDisplayName
        {
            get { return finYearDisplayName; }
            set { finYearDisplayName = value; }
        }
        #endregion
    }
}
