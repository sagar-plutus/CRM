using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblLocationTO
    {
        #region Declarations
        Int32 idLocation;
        Int32 parentLocId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String locationDesc;

        String compartmentNo;
        String compartmentSize;
        Int32 mateHandlSystemId;
        String mateHandlSystem;
        String parentLocationDesc;

        #endregion

        #region Constructor
        public TblLocationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLocation
        {
            get { return idLocation; }
            set { idLocation = value; }
        }
        public Int32 ParentLocId
        {
            get { return parentLocId; }
            set { parentLocId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String LocationDesc
        {
            get { return locationDesc; }
            set { locationDesc = value; }
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
        public Int32 MateHandlSystemId
        {
            get { return mateHandlSystemId; }
            set { mateHandlSystemId = value; }
        }

        public String MateHandlSystem
        {
            get { return mateHandlSystem; }
            set { mateHandlSystem = value; }
        }

        public string ParentLocationDesc { get => parentLocationDesc; set => parentLocationDesc = value; }
        #endregion
    }
}