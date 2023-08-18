using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblUserVerTO
    {
        #region Declarations
        Int32 idUserVer;
        Int32 versionId;
        Int32 userId;
        Int32 createdBy;
        DateTime createdOn;
        string imeiNumber;
        #endregion

        #region Constructor
        public TblUserVerTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUserVer
        {
            get { return idUserVer; }
            set { idUserVer = value; }
        }
        public Int32 VersionId
        {
            get { return versionId; }
            set { versionId = value; }
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
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        public string ImeiNumber { get => imeiNumber; set => imeiNumber = value; }
        #endregion
    }
}
