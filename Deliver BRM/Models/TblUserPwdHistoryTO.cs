using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblUserPwdHistoryTO
    {
        #region Declarations
        Int32 idUserPwdHistory;
        Int32 userId;
        Int32 createdBy;
        DateTime createdOn;
        String newPwd;
        String oldPwd;
        #endregion

        #region Constructor
        public TblUserPwdHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUserPwdHistory
        {
            get { return idUserPwdHistory; }
            set { idUserPwdHistory = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
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
        public String NewPwd
        {
            get { return newPwd; }
            set { newPwd = value; }
        }
        public String OldPwd
        {
            get { return oldPwd; }
            set { oldPwd = value; }
        }
        #endregion
    }
}
