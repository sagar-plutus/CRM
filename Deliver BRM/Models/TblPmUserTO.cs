using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblPmUserTO
    {
        #region Declarations
        Int32 idPmUser;
        Int32 pmId;
        Int32 userId;
        Int32 isActive;
        Int32 createdBy;
        DateTime createdOn;

        Int32 updatedBy;
        DateTime updatedOn;

        string userDisplayName;
        #endregion

        #region Constructor
        public TblPmUserTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPmUser
        {
            get { return idPmUser; }
            set { idPmUser = value; }
        }
        public Int32 PmId
        {
            get { return pmId; }
            set { pmId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
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

          public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public string UserDisplayName
        {
            get { return userDisplayName; }
            set { userDisplayName = value; }
        }
        #endregion
    }
}
