using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace   PurchaseTrackerAPI.Models
{
    public class TblTRSLATO
    {
        #region Declarations
        Int32 idSLA;
        Int32 transferRequestId;
        Int32 unloadingId;
        Int32 mixMaterialId;
        Int32 waste;
        Int32 offChemistryId;
        Int32 descity;
        Int32 statusId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Boolean isActive;
        Double overSizePer;
        String displayNo;
        #endregion

        #region Constructor
        public TblTRSLATO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSLA
        {
            get { return idSLA; }
            set { idSLA = value; }
        }
        public Int32 TransferRequestId
        {
            get { return transferRequestId; }
            set { transferRequestId = value; }
        }
        public Int32 UnloadingId
        {
            get { return unloadingId; }
            set { unloadingId = value; }
        }
        public Int32 MixMaterialId
        {
            get { return mixMaterialId; }
            set { mixMaterialId = value; }
        }
        public Int32 Waste
        {
            get { return waste; }
            set { waste = value; }
        }
        public Int32 OffChemistryId
        {
            get { return offChemistryId; }
            set { offChemistryId = value; }
        }
        public Int32 Descity
        {
            get { return descity; }
            set { descity = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
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
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Double OverSizePer
        {
            get { return overSizePer; }
            set { overSizePer = value; }
        }
        public String DisplayNo
        {
            get { return displayNo; }
            set { displayNo = value; }
        }
        #endregion
    }
}
