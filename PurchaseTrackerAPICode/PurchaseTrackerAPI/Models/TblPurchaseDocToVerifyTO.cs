using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseDocToVerifyTO
    {
        #region Declarations
        Int32 idPurchaseDocType;
        Int32 isActive;
        String purchaseDocType;
        Int32 masterId;
        Int32 isFromMaster;
        #endregion

        #region Constructor
        public TblPurchaseDocToVerifyTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseDocType
        {
            get { return idPurchaseDocType; }
            set { idPurchaseDocType = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String PurchaseDocType
        {
            get { return purchaseDocType; }
            set { purchaseDocType = value; }
        }

        public int MasterId { get => masterId; set => masterId = value; }
        public int IsFromMaster { get => isFromMaster; set => isFromMaster = value; }
        #endregion
    }
}
