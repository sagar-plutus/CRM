using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class TblUnloadingSLATO
    {
        #region Declarations
        Int32 idSLA;
        String displayNo;
        Int32 loadingId;
        Int32 vehicleId;
        Int32 materialTypeId;
        Int32 slaUnloadingId;
        Int32 mixMaterialId;
        Decimal overSizePer;
        String waste;
        Int32 offChemistryId;
        String density;
        String vehicleNo;
        Int32 createdBy;
        DateTime createdOn;
        Int32 isActive;
        Int32 srNo;
        String materialType;
        String slaUnloadingDesc;
        String mixMaterialDesc;
        String offChemistryDesc;
        String createdByName;
        String  itemName;
        Double required;
        Double supply;
        Double sLAPer;
        Int32 idGenericMaster;
        Int32 unloadingPointId;
        String unloadingPointName;
        String loadingSlipNo;
        String requestDisplayNo;

        #endregion

        #region Constructor
        public TblUnloadingSLATO()
        {
        }
        #endregion

        #region GetSet
        public Int32 IdSLA
        {
            get { return idSLA; }
            set { idSLA = value; }
        } 
        public string DisplayNo { get => displayNo; set => displayNo = value; }
        public int LoadingId { get => loadingId; set => loadingId = value; }
        public int VehicleId { get => vehicleId; set => vehicleId = value; }
        public int MaterialTypeId { get => materialTypeId; set => materialTypeId = value; }
        public int SlaUnloadingId { get => slaUnloadingId; set => slaUnloadingId = value; }
        public int MixMaterialId { get => mixMaterialId; set => mixMaterialId = value; }
        public decimal OverSizePer { get => overSizePer; set => overSizePer = value; }
        public string Waste { get => waste; set => waste = value; }
        public int OffChemistryId { get => offChemistryId; set => offChemistryId = value; }
        public string Density { get => density; set => density = value; }
        public string VehicleNo { get => vehicleNo; set => vehicleNo = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public int SrNo { get => srNo; set => srNo = value; }
        public string MaterialType { get => materialType; set => materialType = value; }
        public string SlaUnloadingDesc { get => slaUnloadingDesc; set => slaUnloadingDesc = value; }
        public string MixMaterialDesc { get => mixMaterialDesc; set => mixMaterialDesc = value; }
        public string OffChemistryDesc { get => offChemistryDesc; set => offChemistryDesc = value; }
        public string CreatedByName { get => createdByName; set => createdByName = value; }
        public string  ItemName { get => itemName ; set => itemName  = value; }
        public Double Required { get => required ; set => required = value; }
        public Double Supply { get => supply ; set => supply = value; }
        public Double SLAPer { get => sLAPer; set => sLAPer = value; }

        public Int32 IdGenericMaster { get => idGenericMaster ; set => idGenericMaster = value; }
        public Int32  UnloadingPointId { get => unloadingPointId ; set => unloadingPointId = value; }
        public String UnloadingPointName { get => unloadingPointName; set => unloadingPointName = value; }
        public string RequestDisplayNo { get => requestDisplayNo; set => requestDisplayNo = value; }
        public string LoadingSlipNo { get => loadingSlipNo; set => loadingSlipNo = value; }

        #endregion
    }

    public class UnloadingSLAFilterTO
    {
        #region Declarations
        DateTime fromDate;
        DateTime toDate;
        Boolean skipDateFilter;
        #endregion

        #region Constructor
        public UnloadingSLAFilterTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime FromDate { get => fromDate; set => fromDate = value; }
        public DateTime ToDate { get => toDate; set => toDate = value; }
        public bool SkipDateFilter { get => skipDateFilter; set => skipDateFilter = value; }
        #endregion
    }
}
