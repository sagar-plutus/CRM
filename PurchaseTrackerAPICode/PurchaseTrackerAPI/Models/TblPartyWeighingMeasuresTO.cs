using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPartyWeighingMeasuresTO
    {
        #region Declarations
        Int32 idPartyWeighingMeasures;
        Int32 weighingMachineId;
        Int32 purchaseScheduleSummaryId;
        Int32 weighingMeasureTypeId;
        Int32 prodClassId;
        Int32 vehicleTypeId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int64 tareWt;
        Int64 netWt;
        Int64 grossWt;
        String vehicleNo;
        String intervalTime;
        #endregion

        #region Constructor
        public TblPartyWeighingMeasuresTO()
        {
        }

        #endregion

        #region GetSet

        public String IntervalTime
        {
            get { return intervalTime; }
            set { intervalTime = value; }
        }
        public Int32 IdPartyWeighingMeasures
        {
            get { return idPartyWeighingMeasures; }
            set { idPartyWeighingMeasures = value; }
        }
        public Int32 WeighingMachineId
        {
            get { return weighingMachineId; }
            set { weighingMachineId = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public Int32 WeighingMeasureTypeId
        {
            get { return weighingMeasureTypeId; }
            set { weighingMeasureTypeId = value; }
        }
        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public Int32 VehicleTypeId
        {
            get { return vehicleTypeId; }
            set { vehicleTypeId = value; }
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
        public Int64 TareWt
        {
            get { return tareWt; }
            set { tareWt = value; }
        }
        public Int64 NetWt
        {
            get { return netWt; }
            set { netWt = value; }
        }
        public Int64 GrossWt
        {
            get { return grossWt; }
            set { grossWt = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        #endregion
    }
}
