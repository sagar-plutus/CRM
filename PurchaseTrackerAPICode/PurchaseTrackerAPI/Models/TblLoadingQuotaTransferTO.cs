using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingQuotaTransferTO
    {
        #region Declarations
        Int32 idTransferNote;
        Int32 fromCnfOrgId;
        Int32 toCnfOrgId;
        Int32 againstLoadingQuotaId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double transferQty;
        String transferDesc;
        String fromCnfOrgName;
        String toCnfOrgName;
        #endregion

        #region Constructor
        public TblLoadingQuotaTransferTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdTransferNote
        {
            get { return idTransferNote; }
            set { idTransferNote = value; }
        }
        public Int32 FromCnfOrgId
        {
            get { return fromCnfOrgId; }
            set { fromCnfOrgId = value; }
        }
        public Int32 ToCnfOrgId
        {
            get { return toCnfOrgId; }
            set { toCnfOrgId = value; }
        }
        public Int32 AgainstLoadingQuotaId
        {
            get { return againstLoadingQuotaId; }
            set { againstLoadingQuotaId = value; }
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
        public Double TransferQty
        {
            get { return transferQty; }
            set { transferQty = value; }
        }
        public String TransferDesc
        {
            get { return transferDesc; }
            set { transferDesc = value; }
        }

        public string FromCnfOrgName
        {
            get
            {
                return fromCnfOrgName;
            }

            set
            {
                fromCnfOrgName = value;
            }
        }

        public string ToCnfOrgName
        {
            get
            {
                return toCnfOrgName;
            }

            set
            {
                toCnfOrgName = value;
            }
        }
        #endregion
    }
}
