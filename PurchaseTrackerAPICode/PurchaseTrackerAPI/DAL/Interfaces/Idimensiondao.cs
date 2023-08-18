using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface Idimensiondao
    {
        Boolean CheckAleradyAvailableData(DimGenericMasterTO DimGenericMasterTO);

        Boolean CheckExistingData(DimGenericMasterTO DimGenericMasterTO);
        int SaveDimGenericMasterData(DimGenericMasterTO dropDownTO);
        int UpdateDimGenericMasterData(DimGenericMasterTO dropDownTO);
        List<DimGenericMasterTO> GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0);  
        List<DropDownTO> GetApprovalUserList(String idUserStr);
        List<DropDownTO> SelectVehicalType();
        List<DropDownTO> SelectDeliPeriodForDropDown();
        List<DropDownTO> SelectAllSystemUsersFromRoleType(int roleTypeId);
        List<DropDownTO> SelectCDStructureForDropDown();
        List<DropDownTO> SelectDimMasterValues(Int32 masterId);

        List<DropDownTO> SelectAddOnFunDtls(Int32 transId);

        
        List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId);
        List<DropDownTO> SelectCountriesForDropDown();
        List<DropDownTO> SelectOrgLicensesForDropDown();
        List<DropDownTO> SelectLocationForDropDown();
        List<DropDownTO> SelectSalutationsForDropDown();
        List<DropDownTO> SelectDistrictForDropDown(int stateId);
        List<DropDownTO> SelectStatesForDropDown(int countryId);
        List<DropDownTO> SelectTalukaForDropDown(int districtId);
        List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown();
        List<DropDownTO> SelectAllSystemRoleListForDropDown();
        List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId);
        List<DropDownTO> SelectAllTransportModeForDropDown();
        List<DropDownTO> SelectInvoiceTypeForDropDown();
        List<DropDownTO> SelectInvoiceModeForDropDown();
        List<DropDownTO> SelectCurrencyForDropDown();
        List<DropDownTO> GetInvoiceStatusForDropDown();
        List<DimFinYearTO> SelectAllMstFinYearList(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectReportingType();
        List<DimVisitIssueReasonsTO> SelectVisitIssueReasonsList();
        List<DropDownTO> SelectBrandList();
        List<DropDownTO> SelectLoadingLayerList();
        DropDownTO SelectStateCode(Int32 stateId);
        int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran);
        int InsertMstFinYear(DimFinYearTO newMstFinYearTO, SqlConnection conn, SqlTransaction tran);

        List<DropDownTO> SelectAllSystemUsersListFromRoleType(int roleTypeId);
       List<int> GeModRefMaxDataNonMulti();
        List<int> GeModRefMaxData();
    }
}