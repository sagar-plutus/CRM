using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class LoadingStatusDateTO
    {
        DateTime vehicleReported;
        DateTime instructedForIn;
        DateTime gateIn;
        DateTime loadingCompleted;
        DateTime gateOut;

        string vehicleReportedStr;
        string instructedForInStr;
        string gateInStr;
        string loadingCompletedStr;
        string gateOutStr;

        string vehicleReportedMin;
        string instructedForInMin;
        string gateInMin;
        string loadingCompletedMin;
        string gateOutMin;

        public LoadingStatusDateTO()
        {

        }

        public DateTime VehicleReported { get => vehicleReported; set => vehicleReported = value; }
        public DateTime InstructedForIn { get => instructedForIn; set => instructedForIn = value; }
        public DateTime GateIn { get => gateIn; set => gateIn = value; }
        public DateTime LoadingCompleted { get => loadingCompleted; set => loadingCompleted = value; }
        public DateTime GateOut { get => gateOut; set => gateOut = value; }
        public string VehicleReportedStr { get => vehicleReportedStr; set => vehicleReportedStr = value; }
        public string InstructedForInStr { get => instructedForInStr; set => instructedForInStr = value; }
        public string GateInStr { get => gateInStr; set => gateInStr = value; }
        public string LoadingCompletedStr { get => loadingCompletedStr; set => loadingCompletedStr = value; }
        public string GateOutStr { get => gateOutStr; set => gateOutStr = value; }
        public string VehicleReportedMin { get => vehicleReportedMin; set => vehicleReportedMin = value; }
        public string InstructedForInMin { get => instructedForInMin; set => instructedForInMin = value; }
        public string GateInMin { get => gateInMin; set => gateInMin = value; }
        public string LoadingCompletedMin { get => loadingCompletedMin; set => loadingCompletedMin = value; }
        public string GateOutMin { get => gateOutMin; set => gateOutMin = value; }
    }
}
