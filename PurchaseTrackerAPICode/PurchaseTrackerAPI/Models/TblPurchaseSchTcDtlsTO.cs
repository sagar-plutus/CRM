using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseSchTcDtlsTO
    {
        #region Declarations
        Int32 idPurchasseSchTcDtls;
        Int32 tcTypeId;
        Int32 tcElementId;
        Int32 isActive;
        Int32 purchaseScheduleSummaryId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String tcEleValue;
        String tcTypeName;
        String tcElementName;

        #endregion

        #region Constructor
        public TblPurchaseSchTcDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchasseSchTcDtls
        {
            get { return idPurchasseSchTcDtls; }
            set { idPurchasseSchTcDtls = value; }
        }
        public Int32 TcTypeId
        {
            get { return tcTypeId; }
            set { tcTypeId = value; }
        }
        public Int32 TcElementId
        {
            get { return tcElementId; }
            set { tcElementId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
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
        public String TcEleValue
        {
            get { return tcEleValue; }
            set { tcEleValue = value; }
        }

        public String TcTypeName
        {
            get { return tcTypeName; }
            set { tcTypeName = value; }
        }

        public String TcElementName
        {
            get { return tcElementName; }
            set { tcElementName = value; }
        }

        #endregion
    }
}
