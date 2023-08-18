using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblStockYardTO
    {
        #region Declarations
        Int32 idStockYard;
        Int32 mateHandlSystemId;
        Int32 createdBy;
        DateTime createdOn;
        String location;
        String compartmentNo;
        String compartmentSize;
        String mateHandlSystem;
        #endregion

        #region Constructor
        public TblStockYardTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStockYard
        {
            get { return idStockYard; }
            set { idStockYard = value; }
        }
        public Int32 MateHandlSystemId
        {
            get { return mateHandlSystemId; }
            set { mateHandlSystemId = value; }
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
        public String Location
        {
            get { return location; }
            set { location = value; }
        }
        public String CompartmentNo
        {
            get { return compartmentNo; }
            set { compartmentNo = value; }
        }
        public String CompartmentSize
        {
            get { return compartmentSize; }
            set { compartmentSize = value; }
        }
        public String MateHandlSystem
        {
            get { return mateHandlSystem; }
            set { mateHandlSystem = value; }
        }
        #endregion
    }
}
