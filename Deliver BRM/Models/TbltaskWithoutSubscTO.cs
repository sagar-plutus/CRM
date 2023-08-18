using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TbltaskWithoutSubscTO
    {
        #region Declarations
        Int32 idTaskWithoutSubsc;
        Int32 moduleId;
        Int32 entityId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 isActive;
        DateTime entityWhen;
        DateTime createdOn;
        DateTime udatedOn;
        String description;
        Int16 entityTypeId;
        #endregion

        #region Constructor
        public TbltaskWithoutSubscTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTaskWithoutSubsc
        {
            get { return idTaskWithoutSubsc; }
            set { idTaskWithoutSubsc = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
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
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime EntityWhen
        {
            get { return entityWhen; }
            set { entityWhen = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UdatedOn
        {
            get { return udatedOn; }
            set { udatedOn = value; }
        }
        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public short EntityTypeId { get => entityTypeId; set => entityTypeId = value; }
        #endregion
    }
}
