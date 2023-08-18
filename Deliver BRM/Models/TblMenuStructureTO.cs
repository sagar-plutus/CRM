using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static SalesTrackerAPI.StaticStuff.Constants;

namespace SalesTrackerAPI.Models
{
    public class TblMenuStructureTO
    {
        #region Declarations
        Int32 idMenu;
        Int32 parentMenuId;
        Int32 moduleId;
        Int32 serNo;
        DateTime createdOn;
        String menuName;
        String menuDesc;
        String menuAction;
        String menuItemIcon;
        String menuShortCut;
        Int32 sysElementId;

        #endregion

        #region Constructor
        public TblMenuStructureTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdMenu
        {
            get { return idMenu; }
            set { idMenu = value; }
        }
        public Int32 ParentMenuId
        {
            get { return parentMenuId; }
            set { parentMenuId = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public Int32 SerNo
        {
            get { return serNo; }
            set { serNo = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }
        public String MenuDesc
        {
            get { return menuDesc; }
            set { menuDesc = value; }
        }
        public String MenuAction
        {
            get { return menuAction; }
            set { menuAction = value; }
        }
        public String MenuItemIcon
        {
            get { return menuItemIcon; }
            set { menuItemIcon = value; }
        }
        public String MenuShortCut
        {
            get { return menuShortCut; }
            set { menuShortCut = value; }
        }

        public int SysElementId { get => sysElementId; set => sysElementId = value; }
        #endregion
    }
}
