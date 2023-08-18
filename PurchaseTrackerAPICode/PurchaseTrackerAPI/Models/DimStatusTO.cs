using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class DimStatusTO
    {
        #region Declarations
        Int32 idStatus;
        Int32 transactionTypeId;
        Int32 isActive;
        String statusName;
        Int32 prevStatusId;
        String statusDesc;
        Int32 iotStatusId;
        string colorCode;
        Int32 isBlocked;

        #endregion

        #region Constructor
        public DimStatusTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdStatus
        {
            get { return idStatus; }
            set { idStatus = value; }
        }
        public Int32 TransactionTypeId
        {
            get { return transactionTypeId; }
            set { transactionTypeId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }

        public int PrevStatusId { get => prevStatusId; set => prevStatusId = value; }
        public String StatusDesc
        {
            get { return statusDesc; }
            set { statusDesc = value; }
        }

        public int IotStatusId { get => iotStatusId; set => iotStatusId = value; }
        public string ColorCode { get => colorCode; set => colorCode = value; }

        public int IsBlocked { get => isBlocked; set => isBlocked = value; }

        #endregion
    }
}
