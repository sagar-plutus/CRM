using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class CommonDimensionsTO
    {
        #region Declaration
        Int32 idDimension;
        String dimensionCode;
        String dimensionName;
        Int32 parentId;



        #endregion

        #region Get Set

        public int IdDimension { get => idDimension; set => idDimension = value; }
        public string DimensionCode { get => dimensionCode; set => dimensionCode = value; }
        public Int32 ParentId { get => parentId; set => parentId = value; }
        public string DimensionName { get => dimensionName; set => dimensionName = value; }

        #endregion
    }
}
