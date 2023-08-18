using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TO;

namespace PurchaseTrackerAPI.Models
{
    public class TblVersionTO
    {
        #region Declarations
        Int32 idVersion;
        Int32 createdBy;
        DateTime createdOn;
        String versionNo;
        String verDesc;
        String urlPath;
        List<TblVerReleaseNotesTO> tblVerReleaseNotesTOList;
        #endregion

        #region Constructor
        public TblVersionTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdVersion
        {
            get { return idVersion; }
            set { idVersion = value; }
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
        public String VersionNo
        {
            get { return versionNo; }
            set { versionNo = value; }
        }
        public String VerDesc
        {
            get { return verDesc; }
            set { verDesc = value; }
        }
        public String UrlPath
        {
            get { return urlPath; }
            set { urlPath = value; }
        }
        public List<TblVerReleaseNotesTO> TblVerReleaseNotesTOList{
            get { return tblVerReleaseNotesTOList; }
            set { tblVerReleaseNotesTOList = value; }
        }
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        #endregion
    }
}
