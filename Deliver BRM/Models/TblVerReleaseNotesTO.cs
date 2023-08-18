using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace TO
{
    public class TblVerReleaseNotesTO
    {
        #region Declarations
        Int32 idReleaseNote;
        Int32 versionId;
        Int32 createdBy;
        DateTime createdOn;
        String noteDesc;
        #endregion

        #region Constructor
        public TblVerReleaseNotesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdReleaseNote
        {
            get { return idReleaseNote; }
            set { idReleaseNote = value; }
        }
        public Int32 VersionId
        {
            get { return versionId; }
            set { versionId = value; }
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
        public String NoteDesc
        {
            get { return noteDesc; }
            set { noteDesc = value; }
        }
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }
        #endregion
    }
}
