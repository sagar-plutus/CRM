using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class PermissionTO
    {
        #region Declarations
        Int32 idSysElement;
        Int32 pageElementId;
        Int32 menuId;
        String type;
        Int32 roleId;
        Int32 userId;
        String effectivePermission;
        String elementName;
        String elementDesc;
        Int32 createdBy;
        DateTime createdOn;
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

        public int RoleId { get => roleId; set => roleId = value; }
        public int UserId { get => userId; set => userId = value; }
        public string EffectivePermission { get => effectivePermission; set => effectivePermission = value; }
        public string ElementName { get => elementName; set => elementName = value; }
        public string ElementDesc { get => elementDesc; set => elementDesc = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        #endregion
    }
}
