using Newtonsoft.Json;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.Models
{
    public class TblSysElementsTO
    {
        #region Declarations
        Int32 idSysElement;
        Int32 pageElementId;
        Int32 menuId;
        String type;
        String elementName;
        String elementDesc;
        #endregion

        #region Constructor
        public TblSysElementsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSysElement
        {
            get { return idSysElement; }
            set { idSysElement = value; }
        }
        public Int32 PageElementId
        {
            get { return pageElementId; }
            set { pageElementId = value; }
        }
        public Int32 MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }
        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public string ElementName { get => elementName; set => elementName = value; }
        public string ElementDesc { get => elementDesc; set => elementDesc = value; }
        #endregion
    }
}
