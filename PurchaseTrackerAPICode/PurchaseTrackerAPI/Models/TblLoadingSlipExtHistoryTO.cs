using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblLoadingSlipExtHistoryTO
    {
        #region Declarations
        Int32 idConfirmHistory;
        Int32 loadingSlipExtId;
        Int32 lastConfirmationStatus;
        Int32 currentConfirmationStatus;
        Int32 parityDtlId;
        Int32 createdBy;
        DateTime createdOn;
        Double lastRatePerMT;
        Double currentRatePerMT;
        String lastRateCalcDesc;
        String currentRateCalcDesc;
        Double lastCdAplAmt;
        Double currentCdAplAmt;

        #endregion

        #region Constructor
        public TblLoadingSlipExtHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdConfirmHistory
        {
            get { return idConfirmHistory; }
            set { idConfirmHistory = value; }
        }
        public Int32 LoadingSlipExtId
        {
            get { return loadingSlipExtId; }
            set { loadingSlipExtId = value; }
        }
        public Int32 LastConfirmationStatus
        {
            get { return lastConfirmationStatus; }
            set { lastConfirmationStatus = value; }
        }
        public Int32 CurrentConfirmationStatus
        {
            get { return currentConfirmationStatus; }
            set { currentConfirmationStatus = value; }
        }
        public Int32 ParityDtlId
        {
            get { return parityDtlId; }
            set { parityDtlId = value; }
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
        public Double LastRatePerMT
        {
            get { return lastRatePerMT; }
            set { lastRatePerMT = value; }
        }
        public Double CurrentRatePerMT
        {
            get { return currentRatePerMT; }
            set { currentRatePerMT = value; }
        }
        public String LastRateCalcDesc
        {
            get { return lastRateCalcDesc; }
            set { lastRateCalcDesc = value; }
        }
        public String CurrentRateCalcDesc
        {
            get { return currentRateCalcDesc; }
            set { currentRateCalcDesc = value; }
        }

        public double LastCdAplAmt { get => lastCdAplAmt; set => lastCdAplAmt = value; }
        public double CurrentCdAplAmt { get => currentCdAplAmt; set => currentCdAplAmt = value; }
        #endregion
    }
}
