using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class DimProdCatTO
    {
        #region Declarations
        Int32 idProdCat;
        Int32 isActive;
        String prodCateDesc;
        #endregion

        #region Constructor
        public DimProdCatTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProdCat
        {
            get { return idProdCat; }
            set { idProdCat = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String ProdCateDesc
        {
            get { return prodCateDesc; }
            set { prodCateDesc = value; }
        }
        #endregion
    }
}
