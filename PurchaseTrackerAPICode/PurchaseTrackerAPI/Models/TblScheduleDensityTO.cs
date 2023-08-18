using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblScheduleDensityTO
    {
        #region Declarations
        Int32 idScheduleDensity;
        Int32 purchaseScheduleSummaryId;
        Int32 phaseId;
        Int32 vehicleTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double height;
        Double width;
        Double length;
        #endregion

        #region Constructor
        public TblScheduleDensityTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdScheduleDensity
        {
            get { return idScheduleDensity; }
            set { idScheduleDensity = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public Int32 PhaseId
        {
            get { return phaseId; }
            set { phaseId = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
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
        public Double Height
        {
            get { return height; }
            set { height = value; }
        }
        public Double Width
        {
            get { return width; }
            set { width = value; }
        }
        public Double Length
        {
            get { return length; }
            set { length = value; }
        }
        #endregion
    }
}
 