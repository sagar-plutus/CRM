using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblUserReportingDetailsTO
    {

        #region Declarations
        Int16 isActive;
        Int32 idUserReportingDetails;
        Int32 reportingTypeId;
        Int32 userId;
        Int32 reportingTo;
        // Int32 reportingTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        Int32 deActivatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        DateTime deActivatedOn;
        String remark;
        Int32 roleId;
        Int32 orgStructureId;
        String userName;
        String reportingToName;
        String reportingType;
        Int32 levelId;
        String positionName;
        #endregion

        #region Constructor
        public TblUserReportingDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int16 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 IdUserReportingDetails
        {
            get { return idUserReportingDetails; }
            set { idUserReportingDetails = value; }
        }

        public Int32 ReportingTypeId
        {
            get { return reportingTypeId; }
            set { reportingTypeId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 ReportingTo
        {
            get { return reportingTo; }
            set { reportingTo = value; }
        }
        //public Int32 ReportingTypeId
        //{
        //    get { return reportingTypeId; }
        //    set { reportingTypeId = value; }
        //}
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public Int32 DeActivatedBy
        {
            get { return deActivatedBy; }
            set { deActivatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public DateTime DeActivatedOn
        {
            get { return deActivatedOn; }
            set { deActivatedOn = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public Int32 OrgStructureId
        {
            get { return orgStructureId; }
            set { orgStructureId = value; }
        }

        public String UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public String ReportingToName
        {
            get { return reportingToName; }
            set { reportingToName = value; }
        }
        public String ReportingType
        {
            get { return reportingType; }
            set { reportingType = value; }
        }

        //public String ReportingType
        //{
        //    get { return reportingType; }
        //    set { reportingType = value; }
        //}
        public Int32 LevelId
        {
            get { return levelId; }
            set { levelId = value; }
        }
        public Int32 RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        public string PositionName { get => positionName; set => positionName = value; }
        #endregion
    }
}
