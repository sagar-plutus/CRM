using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingStatusHistoryTO
    {
        #region Declarations
        Int32 idLoadingHistory;
        Int32 loadingId;
        Int32 statusId;
        Int32 createdBy;
        DateTime statusDate;
        DateTime createdOn;
        String statusRemark;
        Int32 statusReasonId;

        #endregion

        #region Constructor
        public TblLoadingStatusHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingHistory
        {
            get { return idLoadingHistory; }
            set { idLoadingHistory = value; }
        }
        public Int32 LoadingId
        {
            get { return loadingId; }
            set { loadingId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String StatusRemark
        {
            get { return statusRemark; }
            set { statusRemark = value; }
        }

        public Constants.TranStatusE TranStatusE
        {
            get
            {
                TranStatusE tranStatusE = TranStatusE.New;
                //TranStatusE tranStatusE = TranStatusE.LOADING_NEW;
                if (Enum.IsDefined(typeof(TranStatusE), statusId))
                {
                    tranStatusE = (TranStatusE)statusId;
                }
                return tranStatusE;

            }
            set
            {
                statusId = (int)value;
            }
        }

        public int StatusReasonId
        {
            get
            {
                return statusReasonId;
            }

            set
            {
                statusReasonId = value;
            }
        }
        #endregion
    }
}
