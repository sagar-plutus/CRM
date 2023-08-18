using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
   
    public class PermissionTO
    {
        #region Declarations
        Int32 idSysElement;
        Int32 pageElementId;
        Int32 menuId;
        String type;
        Int32 givePermissionToAllUser;
        Int32 roleId;
        Int32 userId;
        String effectivePermission;
        String elementName;
        String userName;
        String elementDesc;
        Int32 createdBy;
        DateTime createdOn;
        Boolean isboolImpPerson;
        Int32 isImpPerson;
        string configLicenseCnt;
        Boolean isPermission;
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
        public string UserName { get => userName; set => userName = value; }
        public bool IsboolImpPerson { get => isboolImpPerson; set => isboolImpPerson = value; }
        public int IsImpPerson { get => isImpPerson; set => isImpPerson = value; }
        public bool IsPermission { get => isPermission; set => isPermission = value; }
        public string ConfigLicenseCnt { get => configLicenseCnt; set => configLicenseCnt = value; }
        public int GivePermissionToAllUser { get => givePermissionToAllUser; set => givePermissionToAllUser = value; }
        #endregion
    }

}
