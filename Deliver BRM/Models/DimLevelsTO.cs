using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class DimLevelsTO
    {
        #region Declarations
        Int32 idLevel;
        String levelName;
        String levelDesc;
        String shortName;
        #endregion

        #region Constructor
        public DimLevelsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdLevel
        {
            get { return idLevel; }
            set { idLevel = value; }
        }
        public String LevelName
        {
            get { return levelName; }
            set { levelName = value; }
        }
        public String LevelDesc
        {
            get { return levelDesc; }
            set { levelDesc = value; }
        }
        public String ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }
        #endregion
    }
}
