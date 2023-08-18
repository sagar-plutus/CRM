using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblPageElementsTO
    {
        #region Declarations
        Int32 idPageElement;
        Int32 pageId;
        Int32 pageEleTypeId;
        DateTime createdOn;
        String elementName;
        String elementDesc;
        String elementDisplayName;
        #endregion

        #region Constructor
        public TblPageElementsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPageElement
        {
            get { return idPageElement; }
            set { idPageElement = value; }
        }
        public Int32 PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }
        public Int32 PageEleTypeId
        {
            get { return pageEleTypeId; }
            set { pageEleTypeId = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }
        public String ElementDesc
        {
            get { return elementDesc; }
            set { elementDesc = value; }
        }
        public String ElementDisplayName
        {
            get { return elementDisplayName; }
            set { elementDisplayName = value; }
        }
        #endregion
    }
}
