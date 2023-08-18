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
    public interface ITblOrganizationBL
    {
        List<TblOrganizationTO> SelectAllTblOrganizationList();
        List<TblOrganizationTO> SelectSalesAgentListWithBrandAndRate();
        TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization);
        List<TblOrganizationTO> SelectExistingAllTblOrganizationByRefIds(Int32 orgId, String overdueRefId, String enqRefId);
        TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization, SqlConnection conn, SqlTransaction tran);
        List<TblOrganizationTO> SelectAllChildOrganizationList(int orgTypeId, int parentId);
        List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE);
        List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectAllOrganizationListForDropDown(Constants.OrgTypeE orgTypeE, TblUserRoleTO userRoleTO);
        List<DropDownTO> SelectAllSpecialCnfListForDropDown(TblUserRoleTO userRoleTO);
        List<DropDownTO> SelectDealerListForDropDown(Int32 cnfId, TblUserRoleTO tblUserRoleTO);
        List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId);
        Dictionary<int, string> SelectRegisteredMobileNoDCT(String orgIds, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectRegisteredMobileNoDCTByOrgType(String orgTypeIds, SqlConnection conn, SqlTransaction tran);
        List<OrgExportRptTO> SelectAllOrgListToExport(Int32 orgTypeId, Int32 parentId);
        List<TblOrganizationTO> SelectOrganizationListByRegion(Int32 orgTypeId, Int32 districtId);
        TblOrganizationTO SelectTblOrganizationTOByEnqRefId(String enq_ref_id);
        int InsertTblOrganization(TblOrganizationTO tblOrganizationTO);
        int InsertTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewOrganization(TblOrganizationTO tblOrganizationTO);
        int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO);
        int UpdateTblOrganizationRefIds(TblOrganizationTO tblOrganizationTO);
        int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateOrganization(TblOrganizationTO tblOrganizationTO);
        int DeleteTblOrganization(Int32 idOrganization);
        int DeleteTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran);
        ResultMessage CheckIsSupplierIsBlocked(Int32 supplierID, SqlConnection conn, SqlTransaction tran);
        ResultMessage CheckIsSupplierIsBlocked(Int32 supplierID);

        //ResultMessage CheckIsSupplierIsBlocked(Int32 supplierID);

    }
}