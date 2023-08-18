using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblCRMShareDocsDetailsTO
    {
        #region Declarations
        Int32 idShareDoc;
        Int32 visitId;
        Int32 documentId;
        Int32 userId;
        Int32 roleId;
        Int32 createdBy;
        DateTime createdOn;
        String fileName;
        int entityTypeId;
        List<PersonShareData> personShareData;

        #endregion

        #region Constructor
        public TblCRMShareDocsDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdShareDoc
        {
            get { return idShareDoc; }
            set { idShareDoc = value; }
        }
        public Int32 VisitId
        {
            get { return visitId; }
            set { visitId = value; }
        }
        public Int32 DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 RoleId
        {
            get { return roleId; }
            set { roleId = value; }
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

        public List<PersonShareData> PersonShareData { get => personShareData; set => personShareData = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public int EntityTypeId { get => entityTypeId; set => entityTypeId = value; }
        #endregion
    }
}
