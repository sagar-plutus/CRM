using PurchaseTrackerAPI.StaticStuff;

using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblGroupTO
    {
        #region Declarations
        Int32 idGroup;
        Int32 createdBy;
        Int32 isActive;
        DateTime createdOn;
        String groupName;
        Double rate;
        Int32 updatedBy;
        DateTime updatedOn;
        #endregion

        #region Constructor
        public TblGroupTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdGroup
        {
            get { return idGroup; }
            set { idGroup = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        public double Rate { get => rate; set => rate = value; }
        public int UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        #endregion
    }
}
