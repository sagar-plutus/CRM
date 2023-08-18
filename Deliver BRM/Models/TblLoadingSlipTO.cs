using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
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
        Int32 cnfOrgId;
        Int32 bookingId;
        String bookingDisplayNo;
        String dealerOrgName;

        Double loadingQty;

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
        Int32 fromOrgId;
        List<TblLoadingStatusHistoryTO> loadingStatusHistoryTOList;
        LoadingStatusDateTO loadingStatusDateTO;

        Double orcAmt;          //Priyanka [07-05-2018]
        String orcMeasure;      //Priyanka [07-05-2018]
        String cnfOrgName;     //Vijaymala [21-05-2018]

        Int32 modbusRefId;
        Int32 gateId;
        String portNumber;
        String iotUrl;
        String machineIP;
        Int32 isDBup;
        Int32 transporterOrgId;
        String statusDesc;

        #endregion

        #region Constructor
        public TblLoadingSlipTO()
        {
        }

        #endregion

        #region GetSet

        public Int32 FromOrgId
        {
            get { return fromOrgId; }
            set { fromOrgId = value; }
        }
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
        public Int32 CnfOrgId
        {
            get { return cnfOrgId; }
            set { cnfOrgId = value; }
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
                TranStatusE tranStatusE = TranStatusE.LOADING_NEW;
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

        //Sudhir[27-02-2018] Added for the StatusHistoryList
        public List<TblLoadingStatusHistoryTO> LoadingStatusHistoryTOList
        {
            get
            {
                return loadingStatusHistoryTOList;
            }

            set
            {
                loadingStatusHistoryTOList = value;
            }
        }


        public LoadingStatusDateTO LoadingStatusDateTO
        {
            get
            {
                return loadingStatusDateTO;
            }

            set
            {
                loadingStatusDateTO = value;
            }
        }
        /// <summary>
        /// Priyanka [05-03-2018]
        /// </summary>
        public Double OrcAmt
        {
            get
            {
                return orcAmt;
            }
            set
            {
                orcAmt = value;
            }
        }
        public String OrcMeasure
        {
            get
            {
                return orcMeasure;
            }
            set
            {
                orcMeasure = value;
            }
        }

        public string CnfOrgName { get => cnfOrgName; set => cnfOrgName = value; }
        public int BookingId { get => bookingId; set => bookingId = value; }
        public string BookingDisplayNo { get => bookingDisplayNo; set => bookingDisplayNo = value; }

        public int ModbusRefId { get => modbusRefId; set => modbusRefId = value; }
        public int GateId { get => gateId; set => gateId = value; }
        public string PortNumber { get => portNumber; set => portNumber = value; }
        public string IotUrl { get => iotUrl; set => iotUrl = value; }
        public string MachineIP { get => machineIP; set => machineIP = value; }
        public int IsDBup { get => isDBup; set => isDBup = value; }

        public int TransporterOrgId { get => transporterOrgId; set => transporterOrgId = value; }
        public string StatusDesc { get => statusDesc; set => statusDesc = value; }
        public double LoadingQty { get => loadingQty; set => loadingQty = value; }
        public object ModeId { get; set; }
        #endregion
    }

    //public class TblGate
    //{
    //    public string ModbusRefId { get; set; }
    //    public string IotURl { get; set; }
    //    public string PortNumber { get; set; }
    //    public string MachineIP { get; set; }
    //}
}
