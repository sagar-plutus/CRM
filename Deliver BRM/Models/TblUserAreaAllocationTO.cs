using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblUserAreaAllocationTO
    {
        #region Declarations
        Int32 idAreaAllocDtl;
        Int32 userId;
        Int32 cnfOrgId;
        Int32 districtId;
        Int32 createdBy;
        Int32 isActive;
        DateTime createdOn;
        String remark;
        String userName;
        String districtName;
        String cnfOrgName;
        String createdByUserName;

        #endregion

        #region Constructor
        public TblUserAreaAllocationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdAreaAllocDtl
        {
            get { return idAreaAllocDtl; }
            set { idAreaAllocDtl = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 CnfOrgId
        {
            get { return cnfOrgId; }
            set { cnfOrgId = value; }
        }
        public Int32 DistrictId
        {
            get { return districtId; }
            set { districtId = value; }
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
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public string UserName { get => userName; set => userName = value; }
        public string DistrictName { get => districtName; set => districtName = value; }
        public string CnfOrgName { get => cnfOrgName; set => cnfOrgName = value; }
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public String CreatedByUserName { get => createdByUserName; set => createdByUserName = value; }
        #endregion
    }
}
