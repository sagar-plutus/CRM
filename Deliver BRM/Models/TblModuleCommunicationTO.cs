using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.Models
{
    public class TblModuleCommunicationTO
    {
        #region Declarations
        Int32 idModuleCommunication;
        Int32 srcModuleId;
        Int32 srcTxnId;
        Int32 srcTxnTypeId;
        Int32 destModuleId;
        Int32 destTxnId;
        Int32 destTxnTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String srcDesc;
        #endregion

        #region Constructor
        public TblModuleCommunicationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdModuleCommunication
        {
            get { return idModuleCommunication; }
            set { idModuleCommunication = value; }
        }
        public Int32 SrcModuleId
        {
            get { return srcModuleId; }
            set { srcModuleId = value; }
        }
        public Int32 SrcTxnId
        {
            get { return srcTxnId; }
            set { srcTxnId = value; }
        }
        public Int32 SrcTxnTypeId
        {
            get { return srcTxnTypeId; }
            set { srcTxnTypeId = value; }
        }
        public Int32 DestModuleId
        {
            get { return destModuleId; }
            set { destModuleId = value; }
        }
        public Int32 DestTxnId
        {
            get { return destTxnId; }
            set { destTxnId = value; }
        }
        public Int32 DestTxnTypeId
        {
            get { return destTxnTypeId; }
            set { destTxnTypeId = value; }
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
        public String SrcDesc
        {
            get { return srcDesc; }
            set { srcDesc = value; }
        }
        #endregion
    }
}
