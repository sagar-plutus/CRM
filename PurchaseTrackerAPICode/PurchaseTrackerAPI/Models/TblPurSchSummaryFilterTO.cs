using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurSchSummaryFilterTO
    {
        string fromDateStr;
        string toDateStr;
        DateTime fromDate;
        DateTime toDate;
        String userId;
        String notInStatusIds;
        string inStatusIds;
        bool isInStatusIds;

        String loginUserId;
        Int32 roleTypeId;
        Int32 statusId;
        Int32 supplierId;
        Int32 phaseId;
        Int32 forScheduleActualOrUnloading;
        Int32 prodClassId;

        Int32 vehicleTypeId;

        string cOrNcId;

        bool skipDateTime;

        Int32 rootScheduleId;
        Int32 isConsiderTm;

        public string FromDateStr { get => fromDateStr; set => fromDateStr = value; }
        public DateTime FromDate { get => fromDate; set => fromDate = value; }
        public DateTime ToDate { get => toDate; set => toDate = value; }
        public string ToDateStr { get => toDateStr; set => toDateStr = value; }
        
        public int ForScheduleActualOrUnloading { get => forScheduleActualOrUnloading; set => forScheduleActualOrUnloading = value; }
        public String LoginUserId { get => loginUserId; set => loginUserId = value; }
        public int RoleTypeId { get => roleTypeId; set => roleTypeId = value; }
        public String UserId { get => userId; set => userId = value; }
        public String NotInStatusIds { get => notInStatusIds; set => notInStatusIds = value; }
        public bool SkipDateTime { get => skipDateTime; set => skipDateTime = value; }

        public bool IsInStatusIds { get => isInStatusIds; set => isInStatusIds = value; }
        public string InStatusIds { get => inStatusIds; set => inStatusIds = value; }

        public int SupplierId { get => supplierId; set => supplierId = value; }
        public int PhaseId { get => phaseId; set => phaseId = value; }
        public int StatusId { get => statusId; set => statusId = value; }
        public int ProdClassId { get => prodClassId; set => prodClassId = value; }

        public int VehicleTypeId { get => vehicleTypeId; set => vehicleTypeId = value; }

        
        public string COrNcId { get => cOrNcId; set => cOrNcId = value; }

        public Int32 RootScheduleId { get => rootScheduleId; set => rootScheduleId = value; }

        public Int32 IsConsiderTm { get => isConsiderTm; set => isConsiderTm = value; }

        

    }
}
