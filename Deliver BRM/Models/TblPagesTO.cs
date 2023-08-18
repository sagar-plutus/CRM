using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblPagesTO
    {
        #region Declarations
        Int32 idPage;
        Int32 moduleId;
        DateTime createdOn;
        String pageName;
        String pageDesc;
        #endregion

        #region Constructor
        public TblPagesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPage
        {
            get { return idPage; }
            set { idPage = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }
        public String PageDesc
        {
            get { return pageDesc; }
            set { pageDesc = value; }
        }
        #endregion
    }
}
