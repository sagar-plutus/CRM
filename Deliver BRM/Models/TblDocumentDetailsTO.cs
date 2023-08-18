using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblDocumentDetailsTO
    {
        #region Declarations
        Int32 idDocument;
        Int32 moduleId;
        Int32 createdBy;
        Int32 isActive;
        DateTime createdOn;
        String documentDesc;
        String path;
        Byte[] fileData;
        String extension;
        Int32 fileTypeId;
        String userName;
        #endregion

        #region Constructor
        public TblDocumentDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdDocument
        {
            get { return idDocument; }
            set { idDocument = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
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
        public String DocumentDesc
        {
            get { return documentDesc; }
            set { documentDesc = value; }
        }
        public String Path
        {
            get { return path; }
            set { path = value; }
        }

        public Byte[] FileData { get => fileData; set => fileData = value; }
        public string Extension { get => extension; set => extension = value; }
        public int FileTypeId { get => fileTypeId; set => fileTypeId = value; }
        public string UserName { get => userName; set => userName = value; }
        #endregion
    }
}
