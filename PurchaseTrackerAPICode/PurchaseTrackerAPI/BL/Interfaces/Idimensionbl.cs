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
    public interface Idimensionbl
    {
        List<DropDownTO> SelectVehicalType();
        List<DropDownTO> SelectAllSystemUsersListFromRoleType(Int32 RoleTypeId);
        List<DropDownTO> SelectDeliPeriodForDropDown();
        List<DropDownTO> SelectAllSystemUsersFromRoleType(Int32 RoleTypeId);
        List<DropDownTO> SelectDimMasterValues(Int32 masterId);
        List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId);
        List<DropDownTO> SelectCDStructureForDropDown();
        List<DropDownTO> SelectCountriesForDropDown();
        List<DropDownTO> SelectStatesForDropDown(int countryId);
        List<DropDownTO> SelectDistrictForDropDown(int stateId);
        List<DropDownTO> SelectTalukaForDropDown(int districtId);
        List<DropDownTO> SelectOrgLicensesForDropDown();
        List<DropDownTO> SelectLocationForDropDown();
        List<DropDownTO> SelectSalutationsForDropDown();
        List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown();
        List<DropDownTO> SelectAllSystemRoleListForDropDown();
        List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId);
        List<DropDownTO> SelectAllTransportModeForDropDown();
        List<DropDownTO> SelectInvoiceTypeForDropDown();
        List<DropDownTO> SelectInvoiceModeForDropDown();
        List<DropDownTO> SelectCurrencyForDropDown();
        List<DropDownTO> GetInvoiceStatusForDropDown();
        DimFinYearTO GetCurrentFinancialYear(DateTime curDate, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetReportingType();
        List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList();
        List<DropDownTO> SelectBrandList();
        List<DropDownTO> SelectLoadingLayerList();
        DropDownTO SelectStateCode(Int32 stateId);
         
        String GetRoleIdsStrFromRoleTypeId(Int32 roleTypeId);
        int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran);

        List<int> GetModbusRefList();
        ResultMessage UpdateGenericMasterData(DimGenericMasterTO DimGenericMasterTO);
        List<DimGenericMasterTO> GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0);
        ResultMessage PostGenericMasterData(DimGenericMasterTO DimGenericMasterTO);
        ResultMessage GetApprovalUserList();

    }
}