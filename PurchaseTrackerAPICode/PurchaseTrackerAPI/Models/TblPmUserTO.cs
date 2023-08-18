using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
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
        #endregion
    }
}
