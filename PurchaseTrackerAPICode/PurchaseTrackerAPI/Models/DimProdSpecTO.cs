using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class DimProdSpecTO
    {
        #region Declarations
        Int32 idProdSpec;
        Int32 isActive;
        String prodSpecDesc;
        Int32 isWeighingAllow;
        #endregion

        #region Constructor
        public DimProdSpecTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProdSpec
        {
            get { return idProdSpec; }
            set { idProdSpec = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String ProdSpecDesc
        {
            get { return prodSpecDesc; }
            set { prodSpecDesc = value; }
        }
        public Int32 IsWeighingAllow
        {
            get { return isWeighingAllow; }
            set { isWeighingAllow = value; }
        }
        #endregion
    }
}
