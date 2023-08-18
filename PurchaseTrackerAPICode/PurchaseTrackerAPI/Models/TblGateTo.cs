using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblGateTO
    {
        #region Declarations
        Int32 idGate;
        Int32 isActive;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String gateName;
        String gateDesc;
        String portNumber;
        String ioTUrl;
        int isDefault;
        String machineIP;
        Int32 previousGateId;
        string previousGateName;
        Int32 moduleId;
        #endregion

        #region Constructor
        public TblGateTO()
        {
        }

        public TblGateTO(Int32 GateId,String IoTUrl, String MachineIP, String PortNumber)
        {
            this.IdGate = GateId;
            this.IoTUrl = IoTUrl;
            this.MachineIP = MachineIP;
            this.PortNumber = PortNumber;
        }

        #endregion

        #region GetSet
        public Int32 IdGate
        {
            get { return idGate; }
            set { idGate = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        public String GateName
        {
            get { return gateName; }
            set { gateName = value; }
        }
        public String GateDesc
        {
            get { return gateDesc; }
            set { gateDesc = value; }
        }
        public String PortNumber
        {
            get { return portNumber; }
            set { portNumber = value; }
        }
        public String IoTUrl
        {
            get { return ioTUrl; }
            set { ioTUrl = value; }
        }

        public int IsDefault { get => isDefault; set => isDefault = value; }
        public string MachineIP { get => machineIP; set => machineIP = value; }
        public int PreviousGateId { get => previousGateId; set => previousGateId = value; }
        public string PreviousGateName { get => previousGateName; set => previousGateName = value; }
        public int ModuleId { get => moduleId; set => moduleId = value; }
        #endregion
    }

    public class tblUserMachineMappingTo
    {
        #region Declarations
        Int32 idUserMachineMapping;
        Int32 userId;
        Int32 gateId;
        Int32 createdBy;
        DateTime createdOn;

        public int IdUserMachineMapping { get => idUserMachineMapping; set => idUserMachineMapping = value; }
        public int UserId { get => userId; set => userId = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public int GateId { get => gateId; set => gateId = value; }
        #endregion

        #region Constructor


        #endregion


    }
}
