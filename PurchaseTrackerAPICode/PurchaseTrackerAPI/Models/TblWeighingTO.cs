using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblWeighingTO
    {
        #region Declarations
        Int32 idWeighing;
        DateTime timeStamp;
        String measurement;
        String machineIp;
        #endregion

        #region Constructor
        public TblWeighingTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdWeighing
        {
            get { return idWeighing; }
            set { idWeighing = value; }
        }
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
        public String Measurement
        {
            get { return measurement; }
            set { measurement = value; }
        }
        public String MachineIp
        {
            get { return machineIp; }
            set { machineIp = value; }
        }
        #endregion
    }
    //chetan[20-jan-2020] added for get data from IOT to data base
    public class TblMachineBackupTO
    {
        #region Declarations
        DateTime backUpDate;
        int idMachineBackup;
        int transactionId;
        int machinePortNumber;
        String machinedata;
        int machineType;
        string globleIP;
        #endregion

        #region Constructor
        public TblMachineBackupTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime BackUpDate
        {
            get { return backUpDate; }
            set { backUpDate = value; }
        }
        public int IdMachineBackup
        {
            get { return idMachineBackup; }
            set { idMachineBackup = value; }
        }
        public int TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }
        public int MachinePortNumber
        {
            get { return machinePortNumber; }
            set { machinePortNumber = value; }
        }
        public String Machinedata
        {
            get { return machinedata; }
            set { machinedata = value; }
        }

        public int MachineType { get => machineType; set => machineType = value; }
        public string GlobleIP { get => globleIP; set => globleIP = value; }
        #endregion
    }
}
