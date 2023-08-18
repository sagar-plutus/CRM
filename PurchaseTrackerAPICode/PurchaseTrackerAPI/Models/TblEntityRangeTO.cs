using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblEntityRangeTO
    {
        #region Declarations
        Int32 idEntityRange;
        Int32 finYearId;
        Int32 entityStartValue;
        Int32 entityRange;
        Int32 entityEndingValue;
        Int32 entityPrevValue;
        Int32 incrementBy;
        DateTime createdOn;
        String entityName;
        String entityDesc;
        String prefix;
        String suffix;


        #endregion

        #region Constructor
        public TblEntityRangeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdEntityRange
        {
            get { return idEntityRange; }
            set { idEntityRange = value; }
        }
        public Int32 FinYearId
        {
            get { return finYearId; }
            set { finYearId = value; }
        }
        public Int32 EntityStartValue
        {
            get { return entityStartValue; }
            set { entityStartValue = value; }
        }
        public Int32 EntityRange
        {
            get { return entityRange; }
            set { entityRange = value; }
        }
        public Int32 EntityEndingValue
        {
            get { return entityEndingValue; }
            set { entityEndingValue = value; }
        }
        public Int32 EntityPrevValue
        {
            get { return entityPrevValue; }
            set { entityPrevValue = value; }
        }
        public Int32 IncrementBy
        {
            get { return incrementBy; }
            set { incrementBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String EntityName
        {
            get { return entityName; }
            set { entityName = value; }
        }
        public String EntityDesc
        {
            get { return entityDesc; }
            set { entityDesc = value; }
        }

        public String Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }
        public String Suffix
        {
            get { return suffix; }
            set { suffix = value; }
        }

        #endregion
    }
}
