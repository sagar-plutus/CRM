using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingSlipTO
    {
        #region Declarations
        Int32 idLoadingSlip;
        Int32 dealerOrgId;
        Int32 isJointDelivery;
        Int32 noOfDeliveries;
        Int32 statusId;
        Int32 createdBy;
        DateTime statusDate;
        DateTime loadingDatetime;
        DateTime createdOn;
        Double cdStructure;
        String statusReason;
        String vehicleNo;
        String dealerOrgName;

        TblLoadingSlipDtlTO tblLoadingSlipDtlTO;
        List<TblLoadingSlipExtTO> loadingSlipExtTOList;
        List<TblLoadingSlipAddressTO> deliveryAddressTOList;
        Int32 loadingId;
        String statusName;
        Int32 statusReasonId;
        String loadingSlipNo;
        Int32 isConfirmed;
        String contactNo;
        String driverName;
        String comment;
        Int32 cdStructureId;

        #endregion

        #region Constructor
        public TblLoadingSlipTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoadingSlip
        {
            get { return idLoadingSlip; }
            set { idLoadingSlip = value; }
        }
        public Int32 DealerOrgId
        {
            get { return dealerOrgId; }
            set { dealerOrgId = value; }
        }
        public Int32 IsJointDelivery
        {
            get { return isJointDelivery; }
            set { isJointDelivery = value; }
        }
        public Int32 NoOfDeliveries
        {
            get { return noOfDeliveries; }
            set { noOfDeliveries = value; }
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
        public DateTime LoadingDatetime
        {
            get { return loadingDatetime; }
            set { loadingDatetime = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double CdStructure
        {
            get { return cdStructure; }
            set { cdStructure = value; }
        }
        public String StatusReason
        {
            get { return statusReason; }
            set { statusReason = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }

        public String DealerOrgName
        {
            get { return dealerOrgName; }
            set { dealerOrgName = value; }
        }

        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }
        public Constants.TranStatusE TranStatusE
        {
            get
            {
                //TranStatusE tranStatusE = TranStatusE.LOADING_NEW;
                TranStatusE tranStatusE = TranStatusE.New;
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

        public TblLoadingSlipDtlTO TblLoadingSlipDtlTO
        {
            get
            {
                return tblLoadingSlipDtlTO;
            }

            set
            {
                tblLoadingSlipDtlTO = value;
            }
        }

        public List<TblLoadingSlipExtTO> LoadingSlipExtTOList
        {
            get
            {
                return loadingSlipExtTOList;
            }

            set
            {
                loadingSlipExtTOList = value;
            }
        }

        public Int32 LoadingId
        {
            get { return loadingId; }
            set { loadingId = value; }
        }

        /// <summary>
        /// Sanjay [2017-03-06] To Record Addresses for each loading slip.
        /// Addresses may vary accroding to loading layers
        /// </summary>
        public List<TblLoadingSlipAddressTO> DeliveryAddressTOList
        {
            get
            {
                return deliveryAddressTOList;
            }

            set
            {
                deliveryAddressTOList = value;
            }
        }


        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public String StatusDateStr
        {
            get { return statusDate.ToString(Constants.DefaultDateFormat); }
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

        public string LoadingSlipNo
        {
            get
            {
                return loadingSlipNo;
            }

            set
            {
                loadingSlipNo = value;
            }
        }

        public int IsConfirmed
        {
            get
            {
                return isConfirmed;
            }

            set
            {
                isConfirmed = value;
            }
        }

        public string ContactNo { get => contactNo; set => contactNo = value; }
        public string DriverName { get => driverName; set => driverName = value; }
        public string Comment { get => comment; set => comment = value; }
        public int CdStructureId { get => cdStructureId; set => cdStructureId = value; }
        #endregion
    }
}
