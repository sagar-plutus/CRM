using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblMachineCalibrationTO
    {
        #region Declarations
        Int32 idMachineCalibration;
        Int32 weighingMachineId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double calibrationValue;
        #endregion

        #region Constructor
        public TblMachineCalibrationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdMachineCalibration
        {
            get { return idMachineCalibration; }
            set { idMachineCalibration = value; }
        }
        public Int32 WeighingMachineId
        {
            get { return weighingMachineId; }
            set { weighingMachineId = value; }
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
        public Double CalibrationValue
        {
            get { return calibrationValue; }
            set { calibrationValue = value; }
        }
        #endregion
    }
}
