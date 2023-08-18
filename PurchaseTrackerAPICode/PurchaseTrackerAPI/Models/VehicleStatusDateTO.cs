using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class VehicleStatusDateTO
    {
        String vehicleNo;
        String partyName;
        Int32 rootScheduleId;

        DateTime scheduleOn;
        DateTime sendForCommercialapproval;
        DateTime sendForOutsideInspection;
        DateTime vehicleReportedOn;
        DateTime requestedToSendIn;
        DateTime sentIn;
        DateTime weighingCompletedOn;
        DateTime unloadingCompletedOn;
        DateTime gradingCompletedOn;
        DateTime recoveryCompletedOn;
        DateTime correctionCompletedOn;
        DateTime vehicleOutOn;
        

        String scheduleOnStr;
        String sendForCommercialapprovalOnStr;
        String sendForOutsideInspectionOnStr;
        String vehicleReportedOnStr;
        String requestedToSendInOnStr;
        String sentInOnStr;
        String weighingCompletedOnStr;
        String unloadingCompletedOnStr;
        String gradingCompletedOnStr;
        String recoveryCompletedOnStr;
        String correctionCompletedOnStr;
        String vehicleOutOnStr;


        Int32 scheduleOnDiffMin;
        Int32 sendForCommercialapprovalOnDiffMin;
        Int32 sendForOutsideInspectionOnDiffMin;
        Int32 vehicleReportedOnDiffMin;
        Int32 requestedToSendInOnDiffMin;
        Int32 sentInOnDiffMin;
        Int32 weighingCompletedOnDiffMin;
        Int32 unloadingCompletedOnDiffMin;
        Int32 gradingCompletedOnDiffMin;
        Int32 recoveryCompletedOnDiffMin;
        Int32 correctionCompletedOnDiffMin;
        Int32 vehicleOutOnDiffMin;
        String graderName;
        String supervisorName;
        String engineerName;
        String managerName;
        Int32 weghingStageCnt;
        Int32 cOrNcId;
        String scheduleBy;
        String sendForCommercialapprovalBy;
        String sendForOutsideInspectionBy;
        String vehicleReportedBy;
        String requestedToSendInBy;
        String unloadingCompletedBy;
        String vehicleOutBy;
        String sentInBy;
        String gradingCompletedBy;
        String recoveryCompletedBy;
        String correctionCompletedBy;
        String bookingType;

        DateTime grossWtTakenOn;
        String grossWtTakenOnStr;
        String grossWtTakenBy;
        DateTime wtStage1TakenOn;
        String wtStage1TakenOnStr;
        String wtStage1TakenBy;

        DateTime wtStage2TakenOn;
        String wtStage2TakenOnStr;
        String wtStage2TakenBy;

        DateTime wtStage3TakenOn;
        String wtStage3TakenOnStr;
        String wtStage3TakenBy;

        DateTime wtStage4TakenOn;
        String wtStage4TakenOnStr;
        String wtStage4TakenBy;


        DateTime wtStage5TakenOn;
        String wtStage5TakenOnStr;
        String wtStage5TakenBy;

        DateTime wtStage6TakenOn;
        String wtStage6TakenOnStr;
        String wtStage6TakenBy;

        DateTime wtStage7TakenOn;
        String wtStage7TakenOnStr;
        String wtStage7TakenBy;

        DateTime tareWtTakenOn;
        String tareWtTakenOnStr;
        String tareWtTakenBy;

        String weighingCompletedBy;
        Int32 isCorrectionCompleted;




        public DateTime ScheduleOn { get => scheduleOn; set => scheduleOn = value; }
        public DateTime SendForCommercialapproval { get => sendForCommercialapproval; set => sendForCommercialapproval = value; }
        public DateTime SendForOutsideInspection { get => sendForOutsideInspection; set => sendForOutsideInspection = value; }
        public DateTime RequestedToSendIn { get => requestedToSendIn; set => requestedToSendIn = value; }
        public DateTime SentIn { get => sentIn; set => sentIn = value; }
        public DateTime WeighingCompletedOn { get => weighingCompletedOn; set => weighingCompletedOn = value; }
        public DateTime UnloadingCompletedOn { get => unloadingCompletedOn; set => unloadingCompletedOn = value; }
        public DateTime GradingCompletedOn { get => gradingCompletedOn; set => gradingCompletedOn = value; }
        public DateTime RecoveryCompletedOn { get => recoveryCompletedOn; set => recoveryCompletedOn = value; }
        public DateTime VehicleOutOn { get => vehicleOutOn; set => vehicleOutOn = value; }
        public string ScheduleOnStr { get => scheduleOnStr; set => scheduleOnStr = value; }
        public string SendForCommercialapprovalOnStr { get => sendForCommercialapprovalOnStr; set => sendForCommercialapprovalOnStr = value; }
        public string SendForOutsideInspectionOnStr { get => sendForOutsideInspectionOnStr; set => sendForOutsideInspectionOnStr = value; }
        public string RequestedToSendInOnStr { get => requestedToSendInOnStr; set => requestedToSendInOnStr = value; }
        public string SentInOnStr { get => sentInOnStr; set => sentInOnStr = value; }
        public string WeighingCompletedOnStr { get => weighingCompletedOnStr; set => weighingCompletedOnStr = value; }
        public string UnloadingCompletedOnStr { get => unloadingCompletedOnStr; set => unloadingCompletedOnStr = value; }
        public string GradingCompletedOnStr { get => gradingCompletedOnStr; set => gradingCompletedOnStr = value; }
        public string RecoveryCompletedOnStr { get => recoveryCompletedOnStr; set => recoveryCompletedOnStr = value; }
        public string VehicleOutOnStr { get => vehicleOutOnStr; set => vehicleOutOnStr = value; }
        public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        public string PartyName { get => partyName; set => partyName = value; }
        public int ScheduleOnDiffMin { get => scheduleOnDiffMin; set => scheduleOnDiffMin = value; }
        public int SendForCommercialapprovalOnDiffMin { get => sendForCommercialapprovalOnDiffMin; set => sendForCommercialapprovalOnDiffMin = value; }
        public int SendForOutsideInspectionOnDiffMin { get => sendForOutsideInspectionOnDiffMin; set => sendForOutsideInspectionOnDiffMin = value; }
        public int RequestedToSendInOnDiffMin { get => requestedToSendInOnDiffMin; set => requestedToSendInOnDiffMin = value; }
        public int SentInOnDiffMin { get => sentInOnDiffMin; set => sentInOnDiffMin = value; }
        public int WeighingCompletedOnDiffMin { get => weighingCompletedOnDiffMin; set => weighingCompletedOnDiffMin = value; }
        public int UnloadingCompletedOnDiffMin { get => unloadingCompletedOnDiffMin; set => unloadingCompletedOnDiffMin = value; }
        public int GradingCompletedOnDiffMin { get => gradingCompletedOnDiffMin; set => gradingCompletedOnDiffMin = value; }
        public int RecoveryCompletedOnDiffMin { get => recoveryCompletedOnDiffMin; set => recoveryCompletedOnDiffMin = value; }
        public int VehicleOutOnDiffMin { get => vehicleOutOnDiffMin; set => vehicleOutOnDiffMin = value; }
        public DateTime CorrectionCompletedOn { get => correctionCompletedOn; set => correctionCompletedOn = value; }
        public string CorrectionCompletedOnStr { get => correctionCompletedOnStr; set => correctionCompletedOnStr = value; }
        public int CorrectionCompletedOnDiffMin { get => correctionCompletedOnDiffMin; set => correctionCompletedOnDiffMin = value; }
        public int RootScheduleId { get => rootScheduleId; set => rootScheduleId = value; }
        public DateTime VehicleReportedOn { get => vehicleReportedOn; set => vehicleReportedOn = value; }
        public string VehicleReportedOnStr { get => vehicleReportedOnStr; set => vehicleReportedOnStr = value; }
        public int VehicleReportedOnDiffMin { get => vehicleReportedOnDiffMin; set => vehicleReportedOnDiffMin = value; }
        public string GraderName { get => graderName; set => graderName = value; }
        public string SupervisorName { get => supervisorName; set => supervisorName = value; }
        public string EngineerName { get => engineerName; set => engineerName = value; }
        public string ManagerName { get => managerName; set => managerName = value; }
        public int WeghingStageCnt { get => weghingStageCnt; set => weghingStageCnt = value; }
        public int COrNcId { get => cOrNcId; set => cOrNcId = value; }
        public string ScheduleBy { get => scheduleBy; set => scheduleBy = value; }
        public string SendForCommercialapprovalBy { get => sendForCommercialapprovalBy; set => sendForCommercialapprovalBy = value; }
        public string SendForOutsideInspectionBy { get => sendForOutsideInspectionBy; set => sendForOutsideInspectionBy = value; }
        public string VehicleReportedBy { get => vehicleReportedBy; set => vehicleReportedBy = value; }
        public string RequestedToSendInBy { get => requestedToSendInBy; set => requestedToSendInBy = value; }
        public string SentInBy { get => sentInBy; set => sentInBy = value; }
        public string UnloadingCompletedBy { get => unloadingCompletedBy; set => unloadingCompletedBy = value; }
        public string VehicleOutBy { get => vehicleOutBy; set => vehicleOutBy = value; }
        public string GradingCompletedBy { get => gradingCompletedBy; set => gradingCompletedBy = value; }
        public string RecoveryCompletedBy { get => recoveryCompletedBy; set => recoveryCompletedBy = value; }
        public string CorrectionCompletedBy { get => correctionCompletedBy; set => correctionCompletedBy = value; }
        public string BookingType { get => bookingType; set => bookingType = value; }

        public DateTime GrossWtTakenOn { get => grossWtTakenOn; set => grossWtTakenOn = value; }
        public String GrossWtTakenOnStr { get => grossWtTakenOnStr; set => grossWtTakenOnStr = value; }
        public String GrossWtTakenBy { get => grossWtTakenBy; set => grossWtTakenBy = value; }


        public DateTime WtStage1TakenOn { get => wtStage1TakenOn; set => wtStage1TakenOn = value; }
        public String WtStage1TakenOnStr { get => wtStage1TakenOnStr; set => wtStage1TakenOnStr = value; }
        public String WtStage1TakenBy { get => wtStage1TakenBy; set => wtStage1TakenBy = value; }

        public DateTime WtStage2TakenOn { get => wtStage2TakenOn; set => wtStage2TakenOn = value; }
        public String WtStage2TakenOnStr { get => wtStage2TakenOnStr; set => wtStage2TakenOnStr = value; }
        public String WtStage2TakenBy { get => wtStage2TakenBy; set => wtStage2TakenBy = value; }

        public DateTime WtStage3TakenOn { get => wtStage3TakenOn; set => wtStage3TakenOn = value; }
        public String WtStage3TakenOnStr { get => wtStage3TakenOnStr; set => wtStage3TakenOnStr = value; }
        public String WtStage3TakenBy { get => wtStage3TakenBy; set => wtStage3TakenBy = value; }

        public DateTime WtStage4TakenOn { get => wtStage4TakenOn; set => wtStage4TakenOn = value; }
        public String WtStage4TakenOnStr { get => wtStage4TakenOnStr; set => wtStage4TakenOnStr = value; }
        public String WtStage4TakenBy { get => wtStage4TakenBy; set => wtStage4TakenBy = value; }

        public DateTime WtStage5TakenOn { get => wtStage5TakenOn; set => wtStage5TakenOn = value; }
        public String WtStage5TakenOnStr { get => wtStage5TakenOnStr; set => wtStage5TakenOnStr = value; }
        public String WtStage5TakenBy { get => wtStage5TakenBy; set => wtStage5TakenBy = value; }

        public DateTime WtStage6TakenOn { get => wtStage6TakenOn; set => wtStage6TakenOn = value; }
        public String WtStage6TakenOnStr { get => wtStage6TakenOnStr; set => wtStage6TakenOnStr = value; }
        public String WtStage6TakenBy { get => wtStage6TakenBy; set => wtStage6TakenBy = value; }

        public DateTime WtStage7TakenOn { get => wtStage7TakenOn; set => wtStage7TakenOn = value; }
        public String WtStage7TakenOnStr { get => wtStage7TakenOnStr; set => wtStage7TakenOnStr = value; }
        public String WtStage7TakenBy { get => wtStage7TakenBy; set => wtStage7TakenBy = value; }

        public DateTime TareWtTakenOn { get => tareWtTakenOn; set => tareWtTakenOn = value; }
        public String TareWtTakenOnStr { get => tareWtTakenOnStr; set => tareWtTakenOnStr = value; }
        public String TareWtTakenBy { get => tareWtTakenBy; set => tareWtTakenBy = value; }

        public String WeighingCompletedBy { get => weighingCompletedBy; set => weighingCompletedBy = value; }

        public Int32 IsCorrectionCompleted { get => isCorrectionCompleted; set => isCorrectionCompleted = value; }


        




    }
}
