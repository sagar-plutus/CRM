using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblCompetitorExtTO
    {
        #region Declarations
        Int32 idCompetitorExt;
        Int32 orgId;
        Double prodCapacityMT;
        String brandName;
        String mfgCompanyName;
        #endregion

        #region Constructor
        public TblCompetitorExtTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdCompetitorExt
        {
            get { return idCompetitorExt; }
            set { idCompetitorExt = value; }
        }
        public Int32 OrgId
        {
            get { return orgId; }
            set { orgId = value; }
        }
        public Double ProdCapacityMT
        {
            get { return prodCapacityMT; }
            set { prodCapacityMT = value; }
        }
        public String BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }
        public String MfgCompanyName
        {
            get { return mfgCompanyName; }
            set { mfgCompanyName = value; }
        }
        #endregion
    }
}
