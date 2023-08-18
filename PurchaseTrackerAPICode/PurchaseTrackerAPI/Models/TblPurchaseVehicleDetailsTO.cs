using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PurchaseTrackerAPI.StaticStuff.Constants;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.Models
{
    public class TblPurchaseVehicleDetailsTO
    {
        #region Declarations

        Int32 idVehiclePurchase;
        Int32 schedulePurchaseId;
        Int32 prodItemId;
        Double qty;
        Double assignedQty;
        Double scheduleQty;
        String remark;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        String vehicleNo;
        Int32 supplierId;
        Int32 locationId;
        String suppliername;
        String createdname;
        Int32 tempScheduleId;
        String statusName;
        Int32 statusId;
        String firmName;
        String username;
        Int32 roleId;

        Int32 purchaseEnquiryId;
        String itemName;
        String prodClassDesc;
        Int32 prodClassId;
        Double declaredRate;

        Double rate;
        Double productAomunt;
        Double productRecovery;
        Double recovery;
        Double calculatedMetalCost;
        Double baseMetalCost;
        Double padta;
        Int32 isTransfered;

        Int32 corNcId;
        Int32 transferedFrmScheduleId;
        Int32 isNonCommercialItem;

        Double metalCost;
        Double totalCost;
        Double totalProduct;
        Double gradePadta;

        Int32 oldVehiclePurchaseId;

        Int32 isRemoveItem;

        Int32 groupBySeqNo;
        Int32 itemSrNo;

        Int32 displaySequanceNo;

        double pendingQty;
        double transportAmtPerMT;
        double transportAmt;

        Int32 purchaseEnqDtlsId;

        Double previousQty;

        Double itemBookingRate;

        double recImpurities;
        Int32 itemSeqNo;
        Int32 processVarId;
        Double processVarValue;
        string processVarDisplayName;

        List<TblGradeExpressionDtlsTO> gradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();
        List<TblPurchaseVehicleDetailsTO> bookingVehicleDetailsTOList;

        Int32 purEnqId;
        String purEnqDisplayNo;
        #endregion

        #region Constructor

        public Double Padta
        {
            get { return padta; }
            set { padta = value; }
        }
        public Double CalculatedMetalCost
        {
            get { return calculatedMetalCost; }
            set { calculatedMetalCost = value; }
        }
        public Double BaseMetalCost
        {
            get { return baseMetalCost; }
            set { baseMetalCost = value; }
        }
        public TblPurchaseVehicleDetailsTO()
        {
        }

        #endregion

        #region GetSet

        
        public Int32 ItemSeqNo
        {
            get { return itemSeqNo; }
            set { itemSeqNo = value; }
        }
        public Int32 RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        public Constants.SystemRolesE SystemRolesE
        {
            get
            {
                SystemRolesE SystemRolesE = SystemRolesE.PURCHASE_MANAGER;
                if (Enum.IsDefined(typeof(TranStatusE), statusId))
                {
                    SystemRolesE = (SystemRolesE)statusId;
                }
                return SystemRolesE;

            }
            set
            {
                statusId = (int)value;
            }

        }

        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }

        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 PurchaseEnquiryId
        {
            get { return purchaseEnquiryId; }
            set { purchaseEnquiryId = value; }
        }
        public String Suppliername
        {
            get { return suppliername; }
            set { suppliername = value; }
        }

        public String StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }

        public String Createdname
        {
            get { return createdname; }
            set { createdname = value; }
        }

        public Int32 LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }

        public Int32 IdVehiclePurchase
        {
            get { return idVehiclePurchase; }
            set { idVehiclePurchase = value; }
        }

        public Int32 SchedulePurchaseId
        {
            get { return schedulePurchaseId; }
            set { schedulePurchaseId = value; }
        }

        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }

        public Double ScheduleQty
        {
            get { return scheduleQty; }
            set { scheduleQty = value; }
        }

        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        public Double AssignedQty
        {
            get { return assignedQty; }
            set { assignedQty = value; }
        }

        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }

        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }

        public String FirmName
        {
            get { return firmName; }
            set { firmName = value; }
        }

        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public Int32 TempScheduleId
        {
            get { return tempScheduleId; }
            set { tempScheduleId = value; }
        }
        public String ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public String ProdClassDesc
        {
            get { return prodClassDesc; }
            set { prodClassDesc = value; }
        }

        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }

        public Double DeclaredRate
        {
            get { return declaredRate; }
            set { declaredRate = value; }
        }
        public String CreatedOnStr
        {
            get { return createdOn.ToString(Constants.DefaultDateFormat); }
        }

        public Double Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        // public Double ProductAomunt
        // {
        //     get { return Rate * Qty; }
        //     set { productAomunt = value; }
        // }
        public Double ProductAomunt
        {
            get { return productAomunt; }
            set { productAomunt = value; }
        }

        public Double ProductRecovery
        {
            get { return productRecovery; }
            set { productRecovery = value; }
        }
        public Double Recovery
        {
            get { return recovery; }
            set { recovery = value; }
        }
        public Int32 IsTransfered
        {
            get { return isTransfered; }
            set { isTransfered = value; }
        }

        public Int32 CorNcId
        {
            get { return corNcId; }
            set { corNcId = value; }
        }

        public Int32 TransferedFrmScheduleId
        {
            get { return transferedFrmScheduleId; }
            set { transferedFrmScheduleId = value; }
        }

        public Int32 IsNonCommercialItem
        {
            get { return isNonCommercialItem; }
            set { isNonCommercialItem = value; }
        }
        public List<TblPurchaseVehicleDetailsTO> BookingVehicleDetailsTOList { get => bookingVehicleDetailsTOList; set => bookingVehicleDetailsTOList = value; }

        public Double MetalCost
        {
            get { return metalCost; }
            set { metalCost = value; }
        }

        public Double TotalCost
        {
            get { return totalCost; }
            set { totalCost = value; }
        }
        public Double TotalProduct
        {
            get { return totalProduct; }
            set { totalProduct = value; }
        }
        public Double GradePadta
        {
            get { return gradePadta; }
            set { gradePadta = value; }
        }

        public Int32 OldVehiclePurchaseId
        {
            get { return oldVehiclePurchaseId; }
            set { oldVehiclePurchaseId = value; }
        }

        public List<TblGradeExpressionDtlsTO> GradeExpressionDtlsTOList
        {
            get { return gradeExpressionDtlsTOList; }
            set { gradeExpressionDtlsTOList = value; }
        }

        public TblPurchaseVehicleDetailsTO DeepCopy()
        {
            TblPurchaseVehicleDetailsTO other = (TblPurchaseVehicleDetailsTO)this.MemberwiseClone();
            return other;
        }


        public Int32 IsRemoveItem
        {
            get { return isRemoveItem; }
            set { isRemoveItem = value; }
        }


        public Int32 GroupBySeqNo
        {
            get { return groupBySeqNo; }
            set { groupBySeqNo = value; }
        }

        public Int32 ItemSrNo
        {
            get { return itemSrNo; }
            set { itemSrNo = value; }
        }

        public Int32 DisplaySequanceNo
        {
            get { return displaySequanceNo; }
            set { displaySequanceNo = value; }
        }

        public double PendingQty
        {
            get { return pendingQty; }
            set { pendingQty = value; }
        }

        public double TransportAmtPerMT
        {
            get { return transportAmtPerMT; }
            set { transportAmtPerMT = value; }
        }

        public double TransportAmt
        {
            get { return transportAmt; }
            set { transportAmt = value; }
        }


        public Int32 PurchaseEnqDtlsId
        {
            get { return purchaseEnqDtlsId; }
            set { purchaseEnqDtlsId = value; }
        }
        public Double PreviousQty
        {
            get { return previousQty; }
            set { previousQty = value; }
        }

        public Double ItemBookingRate
        {
            get { return itemBookingRate; }
            set { itemBookingRate = value; }
        }
        public double RecImpurities
        {
            get { return recImpurities; }
            set { recImpurities = value; }
        }
        
         public Int32 ProcessVarId
        {
            get { return processVarId; }
            set { processVarId = value; }
        }
        public Double ProcessVarValue
        {
            get { return processVarValue; }
            set { processVarValue = value; }
        }
        public string ProcessVarDisplayName
        {
            get { return processVarDisplayName; }
            set { processVarDisplayName = value; }
        }

        public Int32 PurEnqId
        {
            get { return purEnqId; }
            set { purEnqId = value; }
        }

        public String PurEnqDisplayNo
        {
            get { return purEnqDisplayNo; }
            set { purEnqDisplayNo = value; }
        }
        #endregion
    }
}
