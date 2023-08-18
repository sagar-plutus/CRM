using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblTaskModuleExtTO
    {
        #region Declarations
        Int32 idTaskModuleExt;
        Int32 moduleId;
        Int32 taskId;
        Int32 taskTypeId;
        Int32 entityId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 eventTypeId;
        #endregion

        #region Constructor
        public TblTaskModuleExtTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTaskModuleExt
        {
            get { return idTaskModuleExt; }
            set { idTaskModuleExt = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public Int32 TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }
        public Int32 TaskTypeId
        {
            get { return taskTypeId; }
            set { taskTypeId = value; }
        }
        public Int32 EntityId
        {
            get { return entityId; }
            set { entityId = value; }
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

        public int EventTypeId { get => eventTypeId; set => eventTypeId = value; }
        #endregion

    }
}
