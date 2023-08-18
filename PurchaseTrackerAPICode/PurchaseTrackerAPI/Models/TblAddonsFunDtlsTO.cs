using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblAddonsFunDtlsTO
    {
        #region Declarations
        int idAddonsfunDtls;
        int moduleId;
        String pageElementId;
        int transId;
        int funRefId;
        int createdBy;
        int updatedBy;
        int isActive;
        String addOnType;
        String transType;
        String funRefVal;
        DateTime createdOn;
        DateTime updatedOn;
        String userdisplayName;
        Int32 count;
        #endregion

        #region Constructor
        public TblAddonsFunDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Int32 IdAddonsfunDtls
        {
            get { return idAddonsfunDtls; }
            set { idAddonsfunDtls = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public String PageElementId
        {
            get { return pageElementId; }
            set { pageElementId = value; }
        }
        public Int32 TransId
        {
            get { return transId; }
            set { transId = value; }
        }
        public Int32 FunRefId
        {
            get { return funRefId; }
            set { funRefId = value; }
        }
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
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String TransType
        {
            get { return transType; }
            set { transType = value; }
        }

        public String AddOnType
        {
            get { return addOnType; }
            set { addOnType = value; }
        }
        public String FunRefVal
        {
            get { return funRefVal; }
            set { funRefVal = value; }
        }

        public string UserdisplayName { get => userdisplayName; set => userdisplayName = value; }
        public int Count { get => count; set => count = value; }
        #endregion
    }
}
