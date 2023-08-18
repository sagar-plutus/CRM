using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSysElementsBL
    {
        List<TblSysElementsTO> SelectAllTblSysElementsList(int menuPgId);
        TblSysElementsTO SelectTblSysElementsTO(Int32 idSysElement);
        Dictionary<int, String> SelectSysElementUserEntitlementDCT(int userId, int roleId);
        List<PermissionTO> SelectAllPermissionList(int menuPgId, int roleId, int userId);
        int InsertTblSysElements(TblSysElementsTO tblSysElementsTO);
        int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveRoleOrUserPermission(PermissionTO permissionTO);
        int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO);
        int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSysElements(Int32 idSysElement);
        int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran);

    }
}