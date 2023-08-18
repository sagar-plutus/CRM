using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblVisitProjectDetailsTO
    {
        #region Declarations
        Int32 idProject;
        Int32 visitId;
        String projectName;
        String projectAddress;
        String contactNo;
        String completionYear;
        Int32 projectTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        Int32 contactPersonId;
        String contactPersonName;
        String emailId;
        #endregion

        #region Constructor
        public TblVisitProjectDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProject
        {
            get { return idProject; }
            set { idProject = value; }
        }
        public Int32 VisitId
        {
            get { return visitId; }
            set { visitId = value; }
        }

        public Int32 ProjectTypeId
        {
            get { return projectTypeId; }
            set { projectTypeId = value; }
        }

        
        public String ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        public String ProjectAddress
        {
            get { return projectAddress; }
            set { projectAddress = value; }
        }
        public String ContactNo
        {
            get { return contactNo; }
            set { contactNo = value; }
        }
        public String CompletionYear
        {
            get { return completionYear; }
            set { completionYear = value; }
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
        public Int32 ContactPersonId
        {
            get { return contactPersonId; }
            set { contactPersonId = value; }
        }

        public String ContactPersonName
        {
            get { return contactPersonName; }
            set { contactPersonName = value; }
        }

        public String EmailId
        {
            get { return emailId; }
            set { emailId = value; }
        }

        #endregion
    }
}
