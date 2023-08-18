using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblMaterialTO
    {
        #region Declarations
        Int32 idMaterial;
        Int32 mateCompOrgId;
        Int32 mateSubCompOrgId;
        Int32 materialTypeId;
        Int32 createdBy;
        DateTime createdOn;
        String materialSubType;
        String userDisplayName;
        Int32 updatedBy;
        DateTime updatedOn;
        Int32 deactivatedBy;
        DateTime deactivatedOn;
        Int32 isActive;
        Int32 displaySequence;
        #endregion

        #region Constructor
        public TblMaterialTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdMaterial
        {
            get { return idMaterial; }
            set { idMaterial = value; }
        }
        public Int32 MateCompOrgId
        {
            get { return mateCompOrgId; }
            set { mateCompOrgId = value; }
        }
        public Int32 MateSubCompOrgId
        {
            get { return mateSubCompOrgId; }
            set { mateSubCompOrgId = value; }
        }
        public Int32 MaterialTypeId
        {
            get { return materialTypeId; }
            set { materialTypeId = value; }
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
        public String MaterialSubType
        {
            get { return materialSubType; }
            set { materialSubType = value; }
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
        public Int32 DeactivatedBy
        {
            get { return deactivatedBy; }
            set { deactivatedBy = value; }
        }
        public DateTime DeactivatedOn
        {
            get { return deactivatedOn; }
            set { deactivatedOn = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public string UserDisplayName { get => userDisplayName; set => userDisplayName = value; }

        /// <summary>
        /// Sanjay [03 Aug 2018] To Show material sequencewise. E.g Stock Summary view
        /// </summary>
        public int DisplaySequence { get => displaySequence; set => displaySequence = value; }
        #endregion
    }
}
