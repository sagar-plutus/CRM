using System;
using System.Collections.Generic;
using System.Text;

namespace PurchaseTrackerAPI.Models
{
    public class TblSpotVehMatDtlsTO
    {
        #region Declarations
        Int32 idSpotVehMatDtls;
        Int32 vehSpotEntryId;
        Int32 prodClassId;
        Int32 prodItemId;
        Double qty;

        
        #endregion

        #region Constructor
        public TblSpotVehMatDtlsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSpotVehMatDtls
        {
            get { return idSpotVehMatDtls; }
            set { idSpotVehMatDtls = value; }
        }
        public Int32 VehSpotEntryId
        {
            get { return vehSpotEntryId; }
            set { vehSpotEntryId = value; }
        }
        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Double Qty
        {
            get { return qty; }
            set { qty = value; }
        }
        #endregion
    }
}
