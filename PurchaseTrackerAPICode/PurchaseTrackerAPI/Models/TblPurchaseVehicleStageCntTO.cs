using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TO;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseVehicleStageCntTO
    {
        #region Declarations
        Int32 idPurchaseVehicleStageCnt;
        Int32 rootScheduleId;
        Int32 wtStageCompCnt;
        Int32 unloadingCompCnt;
        Int32 gradingCompCnt;
        Int32 recoveryCompCnt;
        #endregion

        #region Constructor
        public TblPurchaseVehicleStageCntTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseVehicleStageCnt
        {
            get { return idPurchaseVehicleStageCnt; }
            set { idPurchaseVehicleStageCnt = value; }
        }
        public Int32 RootScheduleId
        {
            get { return rootScheduleId; }
            set { rootScheduleId = value; }
        }
        public Int32 WtStageCompCnt
        {
            get { return wtStageCompCnt; }
            set { wtStageCompCnt = value; }
        }
        public Int32 UnloadingCompCnt
        {
            get { return unloadingCompCnt; }
            set { unloadingCompCnt = value; }
        }
        public Int32 GradingCompCnt
        {
            get { return gradingCompCnt; }
            set { gradingCompCnt = value; }
        }
        public Int32 RecoveryCompCnt
        {
            get { return recoveryCompCnt; }
            set { recoveryCompCnt = value; }
        }
        #endregion
    }
}
