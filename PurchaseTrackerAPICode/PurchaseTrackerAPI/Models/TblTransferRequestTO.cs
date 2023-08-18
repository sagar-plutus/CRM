using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblTransferRequestTO
    {
        #region Declarations
        Int32 idTransferRequest;
        Int32 materialTypeId;
        Int32 materialSubTypeId;
        Int32 fromLocationId;
        Int32 toLocationId;
        Int32 unloadingPointId;
        Int32 statusId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime toDate;

        DateTime updatedOn;
        Int32 isAutoCreated;
        Int32 isActive;
        Double qty;
        Double scheduleqty;
        String requestDisplayNo;
        String narration;
        Int32 statusChangedBy;
        DateTime statusChangedOn;
        string statusIdStr;
        string materialType;
        string materialsSubType;
        string fromLocation;
        string toLocation;
        string unloadingPoint;
        string status;
        string createdByName;
        string updatedByName;
        #endregion

        #region Constructor
        public TblTransferRequestTO()
        {
        }

        #endregion

        #region GetSet
        public string StatusIdStr { get => statusIdStr; set => statusIdStr = value; }
        public string MaterialType { get => materialType; set => materialType = value; }
        public string MaterialsSubType { get => materialsSubType; set => materialsSubType = value; }
        public string FromLocation { get => fromLocation; set => fromLocation = value; }
        public string ToLocation { get => toLocation; set => toLocation = value; }
        public string UnloadingPoint { get => unloadingPoint; set => unloadingPoint = value; }
        public string Status { get => status; set => status = value; }
        public string CreatedByName { get => createdByName; set => createdByName = value; }
        public string UpdatedByName { get => updatedByName; set => updatedByName = value; }
        public DateTime ToDate
        {
            get { return toDate ; }
            set { toDate  = value; }
        }
        public Int32 StatusChangedBy
        {
            get { return statusChangedBy; }
            set { statusChangedBy = value; }
        }
        public DateTime StatusChangedOn
        {
            get { return statusChangedOn; }
            set { statusChangedOn = value; }
        }
        public Int32 IdTransferRequest
        {
            get { return idTransferRequest; }
            set { idTransferRequest = value; }
        }
        public Int32 MaterialTypeId
        {
            get { return materialTypeId; }
            set { materialTypeId = value; }
        }
        public Int32 MaterialSubTypeId
        {
            get { return materialSubTypeId; }
            set { materialSubTypeId = value; }
        }
        public Int32 FromLocationId
        {
            get { return fromLocationId; }
            set { fromLocationId = value; }
        }
        public Int32 ToLocationId
        {
            get { return toLocationId; }
            set { toLocationId = value; }
        }
        public Int32 UnloadingPointId
        {
            get { return unloadingPointId; }
            set { unloadingPointId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
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
        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        public Double Scheduleqty
        {
            get { return scheduleqty; }
            set { scheduleqty = value; }
        }
        public String RequestDisplayNo
        {
            get { return requestDisplayNo; }
            set { requestDisplayNo = value; }
        }
        public String Narration
        {
            get { return narration; }
            set { narration = value; }
        }

        public int IsAutoCreated { get => isAutoCreated; set => isAutoCreated = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        #endregion
    }
}
