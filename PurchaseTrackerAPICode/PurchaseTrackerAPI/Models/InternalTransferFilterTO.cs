using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class InternalTransferFilterTO
    {
        String statusIdStr;
        Int32 approvalUserId;
        DateTime fromDate;
        DateTime toDate;
        Int32 createdBy;
        Boolean skipDateFilter;
        public InternalTransferFilterTO()
        {
        }
        public string StatusIdStr { get => statusIdStr; set => statusIdStr = value; }
        public int ApprovalUserId { get => approvalUserId; set => approvalUserId = value; }
        public DateTime FromDate { get => fromDate; set => fromDate = value; }
        public DateTime ToDate { get => toDate; set => toDate = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public bool SkipDateFilter { get => skipDateFilter; set => skipDateFilter = value; }
    }
}
