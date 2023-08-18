using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblWeighingMachineTO
    {
        #region Declarations
        Int32 idWeighingMachine;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double weighingCapMT;
        String machineName;
        String codeNumber;
        String machineDesc;
        String location;
        String deviceId;
        String machineIP;
        string portNumber;
        string ioTUrl;
        int isActive;
        String userIds;
        #endregion

        #region Constructor
        public TblWeighingMachineTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdWeighingMachine
        {
            get { return idWeighingMachine; }
            set { idWeighingMachine = value; }
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
        public Double WeighingCapMT
        {
            get { return weighingCapMT; }
            set { weighingCapMT = value; }
        }
        public String MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }
        public String CodeNumber
        {
            get { return codeNumber; }
            set { codeNumber = value; }
        }
        public String MachineDesc
        {
            get { return machineDesc; }
            set { machineDesc = value; }
        }
        public String Location
        {
            get { return location; }
            set { location = value; }
        }
        public String DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }
        public String MachineIP
        {
            get { return machineIP; }
            set { machineIP = value; }
        }

        public string PortNumber { get => portNumber; set => portNumber = value; }
        public string IoTUrl { get => ioTUrl; set => ioTUrl = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public string UserIds { get => userIds; set => userIds = value; }
        #endregion
    }
}
