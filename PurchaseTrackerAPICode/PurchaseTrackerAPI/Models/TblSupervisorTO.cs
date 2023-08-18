using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblSupervisorTO
    {
        #region Declarations
        Int32 idSupervisor;
        Int32 isActive;
        Int32 createdBy;
        DateTime createdOn;
        Int32 personId;
        String supervisorName;
        TblPersonTO personTO;
        #endregion

        #region Constructor
        public TblSupervisorTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSupervisor
        {
            get { return idSupervisor; }
            set { idSupervisor = value; }
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
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String SupervisorName
        {
            get { return supervisorName; }
            set { supervisorName = value; }
        }
        public Int32 PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        public TblPersonTO PersonTO { get => personTO; set => personTO = value; }
        #endregion
    }
}
