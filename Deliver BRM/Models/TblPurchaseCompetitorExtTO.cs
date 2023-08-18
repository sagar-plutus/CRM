using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblPurchaseCompetitorExtTO
    {
        #region Declarations
        Int32 idPurCompetitorExt;
        Int32 organizationId;
        String materialType;
        String materialGrade;
        #endregion

        #region Constructor
        public TblPurchaseCompetitorExtTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurCompetitorExt
        {
            get { return idPurCompetitorExt; }
            set { idPurCompetitorExt = value; }
        }
        public Int32 OrganizationId
        {
            get { return organizationId; }
            set { organizationId = value; }
        }
        public String MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }
        public String MaterialGrade
        {
            get { return materialGrade; }
            set { materialGrade = value; }
        }
        #endregion
    }
}
