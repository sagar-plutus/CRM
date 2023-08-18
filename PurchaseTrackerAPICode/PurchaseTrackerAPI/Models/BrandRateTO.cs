using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMSWebAPI.Models
{
    public class BrandRateTO
    {
        #region Declaration

        int brandId;
        string brandName;
        double rate;



        #endregion

        #region GetSet

        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandName { get => brandName; set => brandName = value; }
        public double Rate { get => rate; set => rate = value; }

        #endregion

    }

    public class BrandRateDtlTO
    {
        #region Declaration

        int brandId;
        string brandName;
        double rate;
        double rateBand;



        #endregion

        #region GetSet

        public int BrandId { get => brandId; set => brandId = value; }
        public string BrandName { get => brandName; set => brandName = value; }
        public double Rate { get => rate; set => rate = value; }
        public double RateBand { get => rateBand; set => rateBand = value; }

        #endregion

    }
}
