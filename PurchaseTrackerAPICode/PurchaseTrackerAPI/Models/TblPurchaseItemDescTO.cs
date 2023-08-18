using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseItemDescTO
    {
        #region Declarations
        Int32 idPurchaseItemDesc;
        Int32 rootScheduleId;

        string name;
        string description;
        Int32 phaseId;
        Int32 wtStageId;
        Int32 prodItemDescId;
        Int32 prodItemId;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;

        bool isSelected;

        int stageId;
        #endregion

        #region Constructor
        public TblPurchaseItemDescTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseItemDesc
        {
            get { return idPurchaseItemDesc; }
            set { idPurchaseItemDesc = value; }
        }
        public Int32 RootScheduleId
        {
            get { return rootScheduleId; }
            set { rootScheduleId = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public Int32 PhaseId
        {
            get { return phaseId; }
            set { phaseId = value; }
        }
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
        public Int32 ProdItemDescId
        {
            get { return prodItemDescId; }
            set { prodItemDescId = value; }
        }
        public Int32 StageId
        {
            get { return stageId; }
            set { stageId = value; }
        }

        public Int32 WtStageId
        {
            get { return wtStageId; }
            set { wtStageId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        #endregion
    }
}
