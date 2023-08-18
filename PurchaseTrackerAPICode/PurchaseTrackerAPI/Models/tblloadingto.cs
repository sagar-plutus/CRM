using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingTO
    {
        #region Declarations
        Int32 idLoading;
        Int32 isJointDelivery;
        Int32 noOfDeliveries;
        Int32 statusId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime statusDate;
        DateTime loadingDatetime;
        DateTime createdOn;
        DateTime updatedOn;
        String loadingSlipNo;
        String vehicleNo;
        String statusReason;
        List<TblLoadingSlipTO> loadingSlipList;
        Int32 cnfOrgId;
        String cnfOrgName;
        Double totalLoadingQty;
        String statusDesc;
        Double qty;
        Int32 statusReasonId;
        Int32 transporterOrgId;
        String transporterOrgName;
        Double freightAmt;
        Int32 superwisorId;
        String superwisorName;
        Int32 isRestorePrevStatus;
        Int32 isFreightIncluded;
        String contactNo;
        String driverName;
        String digitalSign;
        String createdByUserName;
        Int32 parentLoadingId;
        Int32 callFlag;
        DateTime flagUpdatedOn;
        Int32 isAllowNxtLoading;
        Int32 loadingType;
        Int32 currencyId; 
        Double currencyRate;
        #endregion

        #region Constructor
        public TblLoadingTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLoading
        {
            get { return idLoading; }
            set { idLoading = value; }
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
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
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
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String LoadingSlipNo
        {
            get { return loadingSlipNo; }
            set { loadingSlipNo = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String StatusReason
        {
            get { return statusReason; }
            set { statusReason = value; }
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

        public List<TblLoadingSlipTO> LoadingSlipList
        {
            get
            {
                return loadingSlipList;
            }

            set
            {
                loadingSlipList = value;
            }
        }

        public Int32 CnfOrgId
        {
            get { return cnfOrgId; }
            set { cnfOrgId = value; }
        }
        public String CnfOrgName
        {
            get { return cnfOrgName; }
            set { cnfOrgName = value; }
        }
        public Double TotalLoadingQty
        {
            get { return totalLoadingQty; }
            set { totalLoadingQty = value; }
        }

        public Double Qty
        {
            get { return totalLoadingQty; }
            set { totalLoadingQty = value; }
        }
        public String StatusDesc
        {
            get { return statusDesc; }
            set { statusDesc = value; }
        }

        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public String StatusDateStr
        {
            get { return statusDate.ToString(Constants.DefaultDateFormat); }
        }
        public String UpdatedOnStr
        {
            get { return updatedOn.ToString(Constants.DefaultDateFormat); }
        }

        public String LoadingDatetimeStr
        {
            get
            {
                if (loadingDatetime != DateTime.MinValue)
                    return loadingDatetime.ToString(Constants.DefaultDateFormat);
                else return "";
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

        public int TransporterOrgId
        {
            get
            {
                return transporterOrgId;
            }

            set
            {
                transporterOrgId = value;
            }
        }

        /// <summary>
        /// Sanjay [2017-04-04] Added while creating loading slip.
        /// This is Freight Charge Amt with Rs/MT(Metric Ton)
        /// </summary>
        public double FreightAmt
        {
            get
            {
                return freightAmt;
            }

            set
            {
                freightAmt = value;
            }
        }


        /// <summary>
        /// Sanjay [2017-04-04] Added while creating loading slip.
        /// This is Transporter Name Of the vehicle selected while loading
        /// </summary>
        public string TransporterOrgName
        {
            get
            {
                return transporterOrgName;
            }

            set
            {
                transporterOrgName = value;
            }
        }

        public int SuperwisorId
        {
            get
            {
                return superwisorId;
            }

            set
            {
                superwisorId = value;
            }
        }

        public string SuperwisorName
        {
            get
            {
                return superwisorName;
            }

            set
            {
                superwisorName = value;
            }
        }
        /*GJ@20170913 : Added Below properties to know Loading slip is Regular Type of Other*/
        public Int32 LoadingType
        {
            get => loadingType; set => loadingType = value;
        }
        public Constants.LoadingTypeE LoadingTypeE
        {
            get
            {
                LoadingTypeE loadingTypeE = LoadingTypeE.REGULAR;
                if (Enum.IsDefined(typeof(LoadingTypeE), loadingType))
                {
                    loadingTypeE = (LoadingTypeE)loadingType;
                }
                return loadingTypeE;

            }
            set
            {
                loadingType = (int)value;
            }
        }
        public Int32 CurrencyId
        {
            get { return currencyId;  }
            set { currencyId  = value; }
        }

        public Double CurrencyRate
        {
            get { return currencyRate; }
            set { currencyRate = value; }
        }
        public int IsRestorePrevStatus { get => isRestorePrevStatus; set => isRestorePrevStatus = value; }
        public int IsFreightIncluded { get => isFreightIncluded; set => isFreightIncluded = value; }
        public string ContactNo { get => contactNo; set => contactNo = value; }
        public string DriverName { get => driverName; set => driverName = value; }
        public string DigitalSign { get => digitalSign; set => digitalSign = value; }
        public string CreatedByUserName { get => createdByUserName; set => createdByUserName = value; }
        public int ParentLoadingId { get => parentLoadingId; set => parentLoadingId = value; }
        public int CallFlag { get => callFlag; set => callFlag = value; }
        public DateTime FlagUpdatedOn { get => flagUpdatedOn; set => flagUpdatedOn = value; }
        public int IsAllowNxtLoading { get => isAllowNxtLoading; set => isAllowNxtLoading = value; }

        #endregion

        #region Methods

        public TblLoadingStatusHistoryTO GetLoadingStatusHistoryTO()
        {
            TblLoadingStatusHistoryTO loadingStatusHistoryTO = new Models.TblLoadingStatusHistoryTO();
            loadingStatusHistoryTO.CreatedBy = this.createdBy;
            loadingStatusHistoryTO.CreatedOn = this.createdOn;
            loadingStatusHistoryTO.LoadingId = this.idLoading;
            loadingStatusHistoryTO.StatusDate = this.statusDate;
            loadingStatusHistoryTO.StatusId = this.statusId;
            loadingStatusHistoryTO.StatusRemark = this.statusReason;
            return loadingStatusHistoryTO;
        }

        #endregion
    }
}
