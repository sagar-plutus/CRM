using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using System.Data;
using PurchaseTrackerAPI.BL; 

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class MastersController : Controller
    {
        private readonly ITblRecycleDocumentBL _iTblRecycleDocumentBL;
        private readonly ITblExpressionDtlsBL _iTblExpressionDtlsBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly IDimStatusBL _iDimStatusBL;
        private readonly IDimStateBL _iDimStateBL;
        private readonly IDimVehicleTypeBL _iDimVehicleTypeBL;
        private readonly Idimensionbl _idimensionbl;
        private readonly ITblPagesBL _iTblPagesBL;
        private readonly ITblPageElementsBL _iTblPageElementsBL;
        private readonly ITblDocumentDetailsBL _iTblDocumentDetailsBL;
        private readonly ITblEmailConfigrationBL _iTblEmailConfigrationBL;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblVariablesBL _iTblVariablesBL;
        private readonly ITblModuleBL _iTblModuleBL;
        //private readonly ITblPurchaseSchTcDtlsBL _iTblPurchaseSchTcDtlsBL;

        private readonly ITblBaseItemMetalCostBL _iTblBaseItemMetalCostBL;
        private readonly ITblGradeQtyDtlsBL _iTblGradeQtyDtlsBL;
        public readonly ITblTRLoadingBL _iTblTRLoadingBL;
        public MastersController(
             Idimensionbl idimensionbl, IDimStateBL iDimStateBL, IDimStatusBL iDimStatusBL, IDimVehicleTypeBL iDimVehicleTypeBL, ITblPagesBL iTblPagesBL
            , ITblPageElementsBL iTblPageElementsBL, ITblConfigParamsBL iTblConfigParamsBL, ITblEmailConfigrationBL iTblEmailConfigrationBL,//,
             ITblDocumentDetailsBL iTblDocumentDetailsBL,
            ITblUserBL iTblUserBL,
            ITblVariablesBL iTblVariablesBL,
             ITblExpressionDtlsBL iTblExpressionDtlsBL,
             ITblRecycleDocumentBL iTblRecycleDocumentBL,
             ITblBaseItemMetalCostBL iTblBaseItemMetalCostBL,
             ITblModuleBL iTblModuleBL,
             ITblGradeQtyDtlsBL iTblGradeQtyDtlsBL, ITblTRLoadingBL iTblTRLoadingBL
             )
        {
            _iTblRecycleDocumentBL = iTblRecycleDocumentBL;
            _iTblExpressionDtlsBL = iTblExpressionDtlsBL;
            _iTblVariablesBL = iTblVariablesBL;
            _iTblUserBL = iTblUserBL;
            _iTblDocumentDetailsBL = iTblDocumentDetailsBL;
            _iTblEmailConfigrationBL = iTblEmailConfigrationBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblPageElementsBL = iTblPageElementsBL;
            _iTblPagesBL = iTblPagesBL;
            _iDimVehicleTypeBL = iDimVehicleTypeBL;
            _iDimStateBL = iDimStateBL;
            _idimensionbl = idimensionbl;
            _iDimStatusBL = iDimStatusBL;
            _iTblBaseItemMetalCostBL = iTblBaseItemMetalCostBL;
            _iTblModuleBL = iTblModuleBL;
            _iTblGradeQtyDtlsBL = iTblGradeQtyDtlsBL;
            _iTblTRLoadingBL = iTblTRLoadingBL;
        }


        #region GET

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("GetStatusListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetStatusListForDropDown(Int32 txnTypeId)
        {
            List<DimStatusTO> statusList = _iDimStatusBL.SelectAllDimStatusList(txnTypeId);
            List<DropDownTO> list = new List<DropDownTO>();
            if (statusList != null && statusList.Count > 0)
            {
                for (int i = 0; i < statusList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    //dropDownTO.Text = statusList[i].StatusName;
                    dropDownTO.Text = statusList[i].StatusDesc;
                    dropDownTO.Value = statusList[i].IdStatus;
                    list.Add(dropDownTO);
                }
            }

            return list;
        }


        //[Route("GetAllSchTcDtlsList")]
        //[HttpGet]
        //public List<TblPurchaseSchTcDtlsTO> GetAllSchTcDtlsList(String rootScheduleId)
        //{
        //    return _iTblPurchaseSchTcDtlsBL.SelectAllScheTcDtls(rootScheduleId);
        //}

        [Route("GetMaterialSampleCategoryListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetMaterialSampleCategoryListForDropDown()
        {
            List<TblPurchaseMaterialSampleCategoryTo> statusList = _iDimVehicleTypeBL.SelectDimSampleCategoryTO();
            List<DropDownTO> list = new List<DropDownTO>();
            if (statusList != null && statusList.Count > 0)
            {
                for (int i = 0; i < statusList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = statusList[i].CategoryName;
                    dropDownTO.Value = statusList[i].IdPurchaseMaterialSampleCategory;
                    list.Add(dropDownTO);
                }
            }
            return list;
        }


        //IOT       
        [Route("getModbusRefList")]
        [HttpGet]
        public List<int> getModbusRefList()
        {
            try
            {
                return _idimensionbl.GetModbusRefList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

      
        [Route("GetSMaterialSampleTypeListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetMaterialSampleTypeListForDropDown()
        {
            List<TblPurchaseMaterialSampleTypeTo> statusList = _iDimVehicleTypeBL.SelectDimSampleTypeTO();
            List<DropDownTO> list = new List<DropDownTO>();
            if (statusList != null && statusList.Count > 0)
            {
                for (int i = 0; i < statusList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = statusList[i].TypeName;
                    dropDownTO.Value = statusList[i].IdPurchaseMaterialSampleType;
                    list.Add(dropDownTO);
                }
            }
            return list;
        }

        [Route("GetPagesListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPagesListForDropDown(Int32 moduleId = 0)
        {
            List<TblPagesTO> pagesList = _iTblPagesBL.SelectAllTblPagesList(moduleId);
            List<DropDownTO> list = new List<DropDownTO>();
            if (pagesList != null && pagesList.Count > 0)
            {
                for (int i = 0; i < pagesList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = pagesList[i].PageName;
                    dropDownTO.Value = pagesList[i].IdPage;
                    list.Add(dropDownTO);
                }
            }
            return list;
        }

        [Route("GetPageElementListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPageElementListForDropDown(Int32 pageId = 0)
        {
            List<TblPageElementsTO> pageEleList = _iTblPageElementsBL.SelectAllTblPageElementsList(pageId);
            List<DropDownTO> list = new List<DropDownTO>();
            if (pageEleList != null && pageEleList.Count > 0)
            {
                for (int i = 0; i < pageEleList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = pageEleList[i].ElementDisplayName;
                    dropDownTO.Value = pageEleList[i].IdPageElement;
                    list.Add(dropDownTO);
                }
            }
            return list;
        }

        [Route("GetVehicleTypesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetVehicleTypesForDropDown()
        {
            List<DimVehicleTypeTO> vehTypeList = _iDimVehicleTypeBL.SelectAllDimVehicleTypeList();
            List<DropDownTO> list = new List<DropDownTO>();
            if (vehTypeList != null && vehTypeList.Count > 0)
            {
                for (int i = 0; i < vehTypeList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = vehTypeList[i].VehicleTypeDesc;
                    dropDownTO.Value = vehTypeList[i].IdVehicleType;
                    list.Add(dropDownTO);
                }
            }

            return list;
        }

        [Route("GetCDStructureForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCDStructureForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.SelectCDStructureForDropDown();
            return statusList;
        }

        [Route("GetDeliveryPeriodForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetDeliveryPeriodForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.SelectDeliPeriodForDropDown();
            return statusList;
        }


        [Route("SelectDimMasterValues")]
        [HttpGet]
        public List<DropDownTO> SelectDimMasterValues(Int32 masterId)
        {
            List<DropDownTO> statusList = _idimensionbl.SelectDimMasterValues(masterId);
            return statusList;
        }
        /// <summary>
        /// Priyanka [19-02-2019]
        /// </summary>
        /// <param name="parentMasterValueId"></param>
        /// <returns></returns>
        [Route("SelectDimMasterValuesByParentMasterValueId")]
        [HttpGet]
        public List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId)
        {
            List<DropDownTO> statusList = _idimensionbl.SelectDimMasterValuesByParentMasterValueId(parentMasterValueId);
            return statusList;
        }

        [Route("GetMaxAllowedDeliveryPeriod")]
        [HttpGet]
        public Int32 GetMaxAllowedDeliveryPeriod()
        {
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CONSOLIDATE_STOCK);
            Int32 maxAllowedDelPeriod = 7;

            if (tblConfigParamsTO != null)
                maxAllowedDelPeriod = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

            return maxAllowedDelPeriod;
        }

        [Route("GetStockConfig")]
        [HttpGet]
        public Int32 GetStockConfigIsConsolidate()
        {
            return _iTblConfigParamsBL.GetStockConfigIsConsolidate();
        }


        [Route("GetDistrictForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetDistrictForDropDown(Int32 stateId)
        {
            List<DropDownTO> statusList = _idimensionbl.SelectDistrictForDropDown(stateId);
            return statusList;
        }

        [Route("GetCountriesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCountriesForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.SelectCountriesForDropDown();
            return statusList;
        }

        [Route("GetStatesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetStatesForDropDown(Int32 countryId)
        {
            List<DropDownTO> statusList = _idimensionbl.SelectStatesForDropDown(countryId);
            return statusList;
        }

        [Route("GetTalukasForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetTalukasForDropDown(Int32 districtId)
        {
            List<DropDownTO> statusList = _idimensionbl.SelectTalukaForDropDown(districtId);
            return statusList;
        }

        [Route("GetSalutaionForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetSalutaionForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.SelectSalutationsForDropDown();
            return statusList;
        }

        [Route("GetCommerLicensesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCommerLicensesForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.SelectOrgLicensesForDropDown();
            return statusList;
        }

        [Route("GetLocationForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetLocationForDropDown()
        {
            List<DropDownTO> locationList = _idimensionbl.SelectLocationForDropDown();
            return locationList;
        }

        [Route("GetRoleListWrtAreaAllocation")]
        [HttpGet]
        public List<DropDownTO> GetRoleListWrtAreaAllocation()
        {
            List<DropDownTO> roleList = _idimensionbl.SelectRoleListWrtAreaAllocationForDropDown();
            return roleList;
        }

        [Route("GetRoleListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetRoleListForDropDown()
        {
            List<DropDownTO> roleList = _idimensionbl.SelectAllSystemRoleListForDropDown();
            return roleList;
        }

        [Route("GetCnfDistrictForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCnfDistrictForDropDown(Int32 cnfOrgId)
        {
            List<DropDownTO> statusList = _idimensionbl.SelectCnfDistrictForDropDown(cnfOrgId);
            return statusList;
        }

        // [Route("GetStockYardList")]
        // [HttpGet]
        // public List<TblStockYardTO> GetStockYardList()
        // {
        //     return BL.TblStockYardBL.SelectAllTblStockYardList();
        // }

        // [Route("GetSuperwisorDetailList")]
        // [HttpGet]
        // public List<TblSupervisorTO> GetSuperwisorDetailList()
        // {
        //     return BL.TblSupervisorBL.SelectAllTblSupervisorList();
        // }


        [Route("GetTransportModeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetTransportModeForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.SelectAllTransportModeForDropDown();
            return statusList;
        }

        [Route("GetInvoiceTypeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetInvoiceTypeForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.SelectInvoiceTypeForDropDown();
            return statusList;
        }

        [Route("GetCurrencyForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCurrencyForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.SelectCurrencyForDropDown();
            return statusList;
        }

        [Route("GetInvoiceStatusForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {
            List<DropDownTO> statusList = _idimensionbl.GetInvoiceStatusForDropDown();
            return statusList;
        }

        // //Vijaymala[08-09-2017] Added To Get Designation List
        // [Route("GetDesignationList")]
        // [HttpGet]
        // public List<DimMstDesignationTO> GetDesignationList()
        // {
        //     return BL.DimMstDesignationBL.SelectAllDimMstDesignationList();
        // }

        //Kiran [09-01-2018] Added To Get Emailconfigration List
        [Route("GetEmailConfigurationList")]
        [HttpGet]
        public List<TblEmailConfigrationTO> GetEmailConfigurationList()
        {
            return _iTblEmailConfigrationBL.SelectAllDimEmailConfigrationList();
        }
        [Route("GetUploadedFileBasedOnFileType")]
        [HttpGet]
        public List<TblDocumentDetailsTO> GetUploadedFileBasedOnFileType(Int32 FilteTypeId, Int32 createdBy)
        {
            return _iTblDocumentDetailsBL.GetUploadedFileBasedOnFileType(FilteTypeId, createdBy);
        }
        // /// <summary>
        // /// Vaibhav [13-Sep-2017] added to fill UnitMeasures Drop Down
        // /// </summary>
        // /// <returns></returns>
        // [Route("GetUnitMeasuresForDropDown")]
        // [HttpGet]
        // public List<DropDownTO> GetUnitMeasuresForDropDown(int unitMeasureTypeId)
        // {
        //     List<DropDownTO> unitMeasuresList = BL.DimUnitMeasuresBL.SelectAllUnitMeasuresListForDropDown(unitMeasureTypeId);
        //     return unitMeasuresList;
        // }

        // /// <summary>
        // /// Vaibhav [13-Sep-2017] added to fill UnloadingStandDesc Drop Down
        // /// </summary>
        // /// <returns></returns>
        // [Route("GetUnloadingStandDescForDropDown")]
        // [HttpGet]
        // public List<DropDownTO> GetUnloadingStandDescForDropDown()
        // {
        //     List<DropDownTO> unloadingStandDescList = BL.TblUnloadingStandDescBL.SelectAllUnloadingStandDescForDropDown();
        //     return unloadingStandDescList;
        // }

        // /// <summary>
        // /// Vaibhav [15-Sep-2017] Get all department
        // /// </summary>
        // /// <param name="data"></param>
        // /// <returns></returns>
        // [Route("GetAllDepartmentList")]
        // [HttpGet]
        // public List<DimMstDeptTO> GetAllDepartmentList()
        // {
        //     return BL.DimMstDeptBL.SelectAllDimMstDeptList();
        // }

        // /// <summary>
        // /// Vaibhav [15-Sep-2017] added to fill division drop down.
        // /// </summary>
        // /// <param name="data"></param>
        // /// <returns></returns>
        // [Route("GetDivisionDropDownList")]
        // [HttpGet]
        // public List<DropDownTO> GetDivisionDropDownList(Int32 DeptTypeId = 0)
        // {
        //     List<DropDownTO> departmentMasterList = BL.DimMstDeptBL.SelectDivisionDropDownList(DeptTypeId);
        //     return departmentMasterList;
        // }


        // /// <summary>
        // /// Vaibhav [18-Sep-2017] added to fill department drop down by specific division 
        // /// </summary>
        // /// <param name="data"></param>
        // /// <returns></returns>
        // /// 
        // [Route("GetDepartmentDropDownListByDivision")]
        // [HttpGet]
        // public List<DropDownTO> GetDepartmentDropDownListByDivision(Int32 DivisionId)
        // {
        //     List<DropDownTO> divisionList = BL.DimMstDeptBL.SelectDepartmentDropDownListByDivision(DivisionId);
        //     return divisionList;
        // }

        // /// <summary>
        // /// Vaibhav [19-Sep-2017] Added to select BOM department
        // /// </summary>
        // /// <param name="data"></param>
        // /// <returns></returns>
        // [Route("GetBOMDepartmentTO")]
        // [HttpGet]
        // public DropDownTO GetBOMDepartmentTO()
        // {
        //     DropDownTO BOMDepartmentTO = BL.DimMstDeptBL.SelectBOMDepartmentTO();
        //     return BOMDepartmentTO;
        // }

        // /// <summary>
        // /// Vaibhav [13-Sep-2017] Get all UnloadingStandDesc List
        // /// </summary>
        // /// <returns></returns>
        // [Route("GetUnloadingStandDescList")]
        // [HttpGet]
        // public List<TblUnloadingStandDescTO> GetUnloadingStandDescList()
        // {
        //     List<TblUnloadingStandDescTO> unloadingStandDescList = BL.TblUnloadingStandDescBL.SelectAllTblUnloadingStandDescList();
        //     return unloadingStandDescList;
        // }



        // /// <summary>
        // /// Vaibhav [25-Sep-2017] get all sub departments for drop down by department
        // /// </summary>
        // /// <param name="data"></param>
        // /// <returns></returns>
        // [Route("GetSubDepartmentDropDownListByDepartment")]
        // [HttpGet]
        // public List<DropDownTO> GetSubDepartmentDropDownListByDepartment(int DepartmentId)
        // {
        //     List<DropDownTO> subDepartmentTOList = BL.DimMstDeptBL.SelectSubDepartmentDropDownListByDepartment(DepartmentId);
        //     return subDepartmentTOList;
        // }


        // /// <summary>
        // /// Vaibhav [25-Sep-2017] get all designation for drop down
        // /// </summary>
        // /// <param name="data"></param>
        // /// <returns></returns>
        // [Route("GetAllDesignationForDropDownList")]
        // [HttpGet]
        // public List<DropDownTO> GetAllDesignationForDropDown()
        // {
        //     List<DropDownTO> designationList = BL.DimMstDesignationBL.SelectAllDesignationForDropDownList();
        //     return designationList;
        // }

        /// <summary>
        /// Vaibhav [27-Sep-2017] added to select all designation list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetReportingTypeList")]
        [HttpGet]
        public List<DropDownTO> GetReportingTypeList()
        {
            return _idimensionbl.GetReportingType();
        }

        /// <summary>
        /// Vaibhav [3-Oct-2017] added to select visit issue reason list
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetVisitIssueReasonsList")]
        [HttpGet]
        public List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList()
        {
            return _idimensionbl.GetVisitIssueReasonsList();
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetBrandList")]
        [HttpGet]
        public List<DropDownTO> GetBrandList()
        {
            List<DropDownTO> brandList = _idimensionbl.SelectBrandList();
            return brandList;
        }



        // //Vijaymala[08-09-2017] Added To Get Designation List
        // [Route("GetStockConfigurationList")]
        // [HttpGet]
        // public List<TblStockConfigTO> GetStockConfigurationList()
        // {
        //     return BL.TblStockConfigBL.SelectAllTblStockConfigTOList();
        // }


        // [Route("GetAllBrandList")]
        // [HttpGet]
        // public List<DimBrandTO> GetAllBrandList()
        // {
        //     return BL.DimBrandBL.SelectAllDimBrandList();
        // }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetLoadingLayerList")]
        [HttpGet]
        public List<DropDownTO> GetLoadingLayerList()
        {
            List<DropDownTO> loadingLayerList = _idimensionbl.SelectLoadingLayerList();
            return loadingLayerList;
        }

        /// <summary>
        ///Sudhir[09-12-2017] Added For GetAllStatesList 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllStatesList")]
        [HttpGet]
        public List<DimStateTO> GetAllStatesList()
        {
            return _iDimStateBL.SelectAllDimState();
        }
        /// <summary>
        /// Vijaymala[10/01/2018]:Added to get state code
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [Route("GetStateCode")]
        [HttpGet]
        public DropDownTO GetStateCode(Int32 stateId)
        {
            DropDownTO stateCodeTO = _idimensionbl.SelectStateCode(stateId);
            return stateCodeTO;
        }

        /// <summary>
        /// AmolG[2022-Jan-20] Get all TR Loading by Passing the status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [Route("GetTRLoadingByStatus")]
        [HttpGet]
        public List<TblTRLoadingTO> GetTRLoadingByStatus(Int32 statusId)
        {
            return _iTblTRLoadingBL.SelectAllTblTRLoadingList(statusId);
        }

        /// <summary>
        /// AmolG[2022-Feb-05] Get TRLoading By Multiple Status Ids
        /// </summary>
        /// <param name="statusIds"></param>
        /// <returns></returns>
        [Route("GetTRLoadingByMultipleStatus")]
        [HttpGet]
        public List<TblTRLoadingTO> GetTRLoadingByMultipleStatus(String statusIds)
        {
            return _iTblTRLoadingBL.SelectAllTblTRLoadingList(statusIds);
        }


        /// <summary>
        /// AmolG[2022-Feb-05] Get TRLoading By Status and Created Date
        /// </summary>
        /// <param name="statusIds"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("GetTRLoadingByMultipleStatusAndCreatedDate")]
        [HttpGet]
        public List<TblTRLoadingTO> GetTRLoadingByMultipleStatusAndCreatedDate(String statusIds, DateTime fromDate, DateTime toDate)
        {
            return _iTblTRLoadingBL.SelectAllTblTRLoadingList(statusIds, fromDate, toDate);
        }

        /// <summary>
        /// AmolG[2022-Feb-05] Get TRLoading by created Date 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("GetTRLoadingByCreatedDate")]
        [HttpGet]
        public List<TblTRLoadingTO> GetTRLoadingByCreatedDate( DateTime fromDate, DateTime toDate)
        {
            return _iTblTRLoadingBL.SelectAllTblTRLoadingList(fromDate, toDate);
        }


        /// <summary>
        /// AmolG[2022-Feb-05] Get TRLoading By passing the TrLoading Id
        /// </summary>
        /// <param name="trLoadingId"></param>
        /// <returns></returns>
        [Route("GetTRLoadingById")]
        [HttpGet]
        public TblTRLoadingTO GetTRLoadingById(int trLoadingId)
        {
            return _iTblTRLoadingBL.SelectTblTRLoadingTO(trLoadingId);
        }

        /// <summary>
        /// AmolG[2022-Jan-20] Get all TR Loading
        /// </summary>
        /// <returns></returns>
        [Route("GetAllTRLoading")]
        [HttpGet]
        public List<TblTRLoadingTO> GetAllTRLoading()
        {
            return _iTblTRLoadingBL.SelectAllTblTRLoadingList();
        }

        /// <summary>
        /// AmolG[2022-Feb-08] Get TRLoading By Status and Created Date
        /// </summary>
        /// <param name="statusIds"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterOnId">Values are :-  1 - Status, 2 - Date, 3 - StatusNDate</param>
        /// <returns></returns>
        [Route("GetTRLoading")]
        [HttpGet]
        public List<TblTRLoadingTO> GetTRLoading(String statusIds, DateTime fromDate, DateTime toDate, int filterOnId)
        {
            Constants.TRLoadingFilterE tRLoadingFilterE = (Constants.TRLoadingFilterE)filterOnId;

            return _iTblTRLoadingBL.SelectAllTblTRLoadingList(statusIds, fromDate, toDate, tRLoadingFilterE);
        }

        /// <summary>
        /// Vijaymala:Added to get group list
        /// </summary>
        /// <param name=""></param>
        // /// <returns></returns>

        // [Route("GetAllGroupList")]
        // [HttpGet]
        // public List<TblGroupTO> GetAllGroupList()
        // {
        //     return BL.TblGroupBL.SelectAllTblGroupList();
        // }




        #endregion

        #region POST
        // // POST api/values
        // [Route("AddNewStockYard")]
        // [HttpPost]
        // public Int32 AddNewStockYard([FromBody] JObject data)
        // {
        //     try
        //     {
        //         TblStockYardTO tblStockYardTO = JsonConvert.DeserializeObject<TblStockYardTO>(data["stockYardInfo"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();
        //         if (tblStockYardTO == null)
        //         {
        //             return 0;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             return 0;
        //         }

        //         tblStockYardTO.CreatedOn =  _iCommonDAO.ServerDateTime;
        //         tblStockYardTO.CreatedBy = Convert.ToInt32(loginUserId);
        //         return BL.TblStockYardBL.InsertTblStockYard(tblStockYardTO);
        //     }
        //     catch (Exception ex)
        //     {
        //         return -1;
        //     }
        // }



        // [Route("PostNewSuperwisorMaster")]
        // [HttpPost]
        // public ResultMessage PostNewSuperwisorMaster([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {

        //         TblSupervisorTO supervisorTO = JsonConvert.DeserializeObject<TblSupervisorTO>(data["supervisorTO"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();
        //         if (supervisorTO == null)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "supervisorTO Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "loginUserId Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }

        //         supervisorTO.CreatedBy = Convert.ToInt32(loginUserId);
        //         supervisorTO.CreatedOn =  _iCommonDAO.ServerDateTime;
        //         supervisorTO.IsActive = 1;
        //         return BL.TblSupervisorBL.SaveNewSuperwisor(supervisorTO);

        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.MessageType = ResultMessageE.Error;
        //         resultMessage.Text = "API : Exception In Method PostNewSuperwisorMaster";
        //         resultMessage.Result = -1;
        //         resultMessage.Exception = ex;
        //         return resultMessage;
        //     }
        // }

        // [Route("PostUpdateSuperwisorMaster")]
        // [HttpPost]
        // public ResultMessage PostUpdateSuperwisorMaster([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {

        //         TblSupervisorTO supervisorTO = JsonConvert.DeserializeObject<TblSupervisorTO>(data["supervisorTO"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();
        //         if (supervisorTO == null)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "supervisorTO Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "loginUserId Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }

        //         return BL.TblSupervisorBL.UpdateSuperwisor(supervisorTO);

        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.MessageType = ResultMessageE.Error;
        //         resultMessage.Text = "API : Exception In Method PostUpdateSuperwisorMaster";
        //         resultMessage.Result = -1;
        //         resultMessage.Exception = ex;
        //         return resultMessage;
        //     }
        // }

        // POST api/values

        [Route("UploadMultipleTypesFile")]
        [HttpPost]
        public async Task<IActionResult> UploadMultipleTypesFile(List<IFormFile> file, Int32 createdBy, Int32 FileTypeId)
        {

            Task task = _iTblDocumentDetailsBL.UploadMultipleTypesFile(file, createdBy, FileTypeId);
            return Ok(task);
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }


        // //Vijaymala[08-09-2017] Added To Insert Designation Master
        // [Route("PostNewDesignationMaster")]
        // [HttpPost]
        // public ResultMessage PostNewDesignationMaster([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {

        //         DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();
        //         if (mstDesignationTO == null)
        //         {
        //             resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("loginUserId Found NULL");
        //             return resultMessage;
        //         }

        //         mstDesignationTO.CreatedBy = Convert.ToInt32(loginUserId);
        //         mstDesignationTO.CreatedOn =  _iCommonDAO.ServerDateTime;
        //         mstDesignationTO.IsVisible = 1;
        //         int result = BL.DimMstDesignationBL.InsertDimMstDesignation(mstDesignationTO);
        //         if (result != 1)
        //         {
        //             resultMessage.DefaultBehaviour("Error... Record could not be saved");
        //             return resultMessage;
        //         }
        //         resultMessage.DefaultSuccessBehaviour();
        //         return resultMessage;

        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "PostNewDesignationMaster");
        //         return resultMessage;
        //     }
        // }



        // //Vijaymala[08-09-2017] Added To Update Designation Master
        // [Route("PostUpdateDesignationMaster")]
        // [HttpPost]
        // public ResultMessage PostUpdateDesignationMaster([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {
        //         DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();
        //         if (mstDesignationTO == null)
        //         {
        //             resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("loginUserId Found NULL");
        //             return resultMessage;
        //         }
        //         mstDesignationTO.UpdatedOn =  _iCommonDAO.ServerDateTime;
        //         mstDesignationTO.UpdatedBy = Convert.ToInt32(loginUserId);
        //         int result = BL.DimMstDesignationBL.UpdateDimMstDesignation(mstDesignationTO);
        //         if (result != 1)
        //         {
        //             resultMessage.DefaultBehaviour("Error... Record could not be saved");
        //             return resultMessage;
        //         }
        //         resultMessage.DefaultSuccessBehaviour();
        //         return resultMessage;

        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateDesignationMaster");
        //         return resultMessage;
        //     }
        // }

        // //Vijaymala[08-09-2017] Added To Deactivate Designation 
        // [Route("PostDeactivateDesignation")]
        // [HttpPost]
        // public ResultMessage PostDeactivateDesignation([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {

        //         DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();

        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("loginUserId Found NULL");
        //             return resultMessage;
        //         }

        //         if (mstDesignationTO != null)
        //         {
        //             DateTime serverDate =  _iCommonDAO.ServerDateTime;
        //             mstDesignationTO.DeactivatedBy = Convert.ToInt32(loginUserId);
        //             mstDesignationTO.DeactivatedOn =  _iCommonDAO.ServerDateTime;
        //             //  organizationTO.DeactivatedOn = serverDate;
        //             mstDesignationTO.IsVisible = 0;

        //             int result = BL.DimMstDesignationBL.UpdateDimMstDesignation(mstDesignationTO);
        //             if (result != 1)
        //             {
        //                 resultMessage.DefaultBehaviour("Error... Record could not be deleted");
        //                 return resultMessage;
        //             }
        //             else
        //             {
        //                 resultMessage.DefaultSuccessBehaviour();
        //                 return resultMessage;
        //             }
        //         }
        //         else
        //         {
        //             resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
        //             return resultMessage;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDesignation");
        //         return resultMessage;
        //     }
        // }

        // /// <summary>
        // /// Vaibhav [16-Sep-2017] Added to insert new department. 
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="value"></param>
        // [Route("PostNewDepartmentMaster")]
        // [HttpPost]
        // public ResultMessage PostNewDepartmentMaster([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new ResultMessage();

        //     try
        //     {
        //         DimMstDeptTO dimMstDeptTO = JsonConvert.DeserializeObject<DimMstDeptTO>(data["mstDepartmentTO"].ToString());

        //         var loginUserId = data["loginUserId"].ToString();
        //         if (dimMstDeptTO == null)
        //         {
        //             resultMessage.DefaultBehaviour("dimMstDeptTO found null");
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("loginUserId found null");
        //             return resultMessage;
        //         }
        //         // check parent department id not null
        //         if (dimMstDeptTO.ParentDeptId <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("ParentDeptId found null");
        //             return resultMessage;
        //         }

        //         return BL.DimMstDeptBL.SaveDepartment(dimMstDeptTO);
        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "PostNewDepartment");
        //         return resultMessage;
        //     }
        // }


        // /// <summary>
        // /// Vaibhav [16-Sep-2017] Added to update department master
        // /// </summary>
        // /// <param name="id"></param>
        // [Route("PostUpdateDepartmentMaster")]
        // [HttpPost]
        // public ResultMessage PostUpdateDepartmentMaster([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new ResultMessage();
        //     try
        //     {
        //         DimMstDeptTO dimMstDeptTO = JsonConvert.DeserializeObject<DimMstDeptTO>(data["mstDepartmentTO"].ToString());

        //         var loginUserId = data["loginUserId"].ToString();
        //         if (dimMstDeptTO == null)
        //         {
        //             resultMessage.DefaultBehaviour("dimMstDeptTO found null");
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("loginUserId found null");
        //             return resultMessage;
        //         }
        //         // check parent department id not null
        //         if (dimMstDeptTO.ParentDeptId <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("ParentDeptId found null");
        //             return resultMessage;
        //         }

        //         return BL.DimMstDeptBL.UpdateDepartment(dimMstDeptTO);
        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
        //         return resultMessage;
        //     }
        // }

        // /// <summary>
        // /// [05-12-2017] Vijaymala:Added to save stock configuration
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="value"></param>
        // /// 
        // [Route("PostStockConfiguration")]
        // [HttpPost]
        // public ResultMessage PostStockConfiguration([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new ResultMessage();

        //     try
        //     {
        //         TblStockConfigTO tblStockConfigTO = JsonConvert.DeserializeObject<TblStockConfigTO>(data["stockConfigurationTO"].ToString());

        //         var loginUserId = data["loginUserId"].ToString();
        //         if (tblStockConfigTO == null)
        //         {
        //             resultMessage.DefaultBehaviour("tblStockConfigTO found null");
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("loginUserId found null");
        //             return resultMessage;
        //         }
        //         tblStockConfigTO.IsItemizedStock = 1;

        //         int result = BL.TblStockConfigBL.InsertTblStockConfig(tblStockConfigTO);
        //         if (result != 1)
        //         {
        //             resultMessage.DefaultBehaviour("Error... Record could not be saved");
        //             return resultMessage;
        //         }
        //         resultMessage.DefaultSuccessBehaviour();
        //         return resultMessage;
        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "PostStockConfiguration");
        //         return resultMessage;
        //     }
        // }

        /// <summary>
        /// [09-01-2018] Kiran:Added to save email configration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// 
        [Route("PostEmailConfigurationDetails")]
        [HttpPost]
        public ResultMessage PostEmailConfigurationDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblEmailConfigrationTO dimEmailConfigrationTO = JsonConvert.DeserializeObject<TblEmailConfigrationTO>(data["EmailConfigrationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimEmailConfigrationTO == null)
                {
                    resultMessage.DefaultBehaviour("dimEmailConfigrationTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                dimEmailConfigrationTO.IsActive = 0;
                int result = _iTblEmailConfigrationBL.InsertDimEmailConfigration(dimEmailConfigrationTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEmailConfigurationDetails");
                return resultMessage;
            }
        }

        /// <summary>
        /// Kiran [09-01-2018] Added to update email configration
        /// </summary>
        /// <param name="id"></param>
        [Route("PostUpdateEmailConfigurationDetails")]
        [HttpPost]
        public ResultMessage PostUpdateEmailConfigurationDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblEmailConfigrationTO dimEmailConfigrationTO = JsonConvert.DeserializeObject<TblEmailConfigrationTO>(data["EmailConfigrationTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimEmailConfigrationTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                int result = _iTblEmailConfigrationBL.UpdateDimEmailConfigration(dimEmailConfigrationTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
                return resultMessage;
            }
        }
        // //Vijaymala[08-09-2017] Added To Update Designation Master
        // [Route("PostUpdateStockConfiguration")]
        // [HttpPost]
        // public ResultMessage PostUpdateStockConfiguration([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {
        //         TblStockConfigTO tblStockConfigTO = JsonConvert.DeserializeObject<TblStockConfigTO>(data["stockConfigurationTO"].ToString());

        //         var loginUserId = data["loginUserId"].ToString();
        //         if (tblStockConfigTO == null)
        //         {
        //             resultMessage.DefaultBehaviour("tblStockConfigTO Found NULL");
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("loginUserId Found NULL");
        //             return resultMessage;
        //         }

        //         int result = BL.TblStockConfigBL.UpdateTblStockConfig(tblStockConfigTO);
        //         if (result != 1)
        //         {
        //             resultMessage.DefaultBehaviour("Error... Record could not be saved");
        //             return resultMessage;
        //         }
        //         resultMessage.DefaultSuccessBehaviour();
        //         return resultMessage;

        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateStockConfiguration");
        //         return resultMessage;
        //     }
        // }

        // [Route("PostDeactivateStockConfiguration")]
        // [HttpPost]
        // public  ResultMessage PostDeactivateStockConfiguration([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {
        //         TblStockConfigTO tblStockConfigTO = JsonConvert.DeserializeObject<TblStockConfigTO>(data["stockConfigurationTO"].ToString());

        //         var loginUserId = data["loginUserId"].ToString();
        //         if (tblStockConfigTO == null)
        //         {
        //             resultMessage.DefaultBehaviour("tblStockConfigTO Found NULL");
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.DefaultBehaviour("loginUserId Found NULL");
        //             return resultMessage;
        //         }

        //         resultMessage= BL.TblStockConfigBL.DeactivateTblStockConfig(tblStockConfigTO);
        //         //if (result != 1)
        //         //{
        //         //    resultMessage.DefaultBehaviour("Error... Record could not be saved");
        //         //    return resultMessage;
        //         //}
        //        // resultMessage.DefaultSuccessBehaviour();
        //         return resultMessage;

        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateStockConfiguration");
        //         return resultMessage;
        //     }
        // }

        /// <summary>
        /// Sudhir[09-12-2017]Added for Adding New State.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostNewState")]
        [HttpPost]
        public ResultMessage PostNewStatetMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimStateTO dimStateTO = JsonConvert.DeserializeObject<DimStateTO>(data["stateTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimStateTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }


                return _iDimStateBL.SaveNewState(dimStateTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
                return resultMessage;
            }
        }

        /// <summary>
        /// Sudhir[09-12-2017]Added for Updating State.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostUpdateState")]
        [HttpPost]
        public ResultMessage PostUpdateState([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimStateTO dimStateTO = JsonConvert.DeserializeObject<DimStateTO>(data["stateTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimStateTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                return _iDimStateBL.UpdateState(dimStateTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
                return resultMessage;
            }
        }


        //Sudhir[11-12-2017] Added To Delete State.
        [Route("PostDeleteState")]
        [HttpPost]
        public ResultMessage PostDeleteState([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                DimStateTO dimStateTO = JsonConvert.DeserializeObject<DimStateTO>(data["stateTo"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimStateTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                result = _iDimStateBL.DeleteDimState(dimStateTO.IdState);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be deleted");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeleteState");
                return resultMessage;
            }
        }


        /// <summary>
        /// AmolG[2022-Jan-20] Save the TR Loading by passing the TRLoadingTO and loginUserId
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("SaveTRLoading")]
        [HttpPost]
        public ResultMessage SaveTRLoading([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                TblTRLoadingTO tblTRLoadingTO = JsonConvert.DeserializeObject<TblTRLoadingTO>(data["tblTRLoadingTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblTRLoadingTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                String errorMsg = "";
                result = _iTblTRLoadingBL.InsertTblTRLoading(tblTRLoadingTO, Convert.ToInt32(loginUserId), ref errorMsg);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be Save");
                    resultMessage.Text = errorMsg;
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveTRLoading");
                return resultMessage;
            }
        }

        /// <summary>
        /// AmolG[2022-Jan-20] Update the TR Loading by passing the TRLoadingTO and loginUserId
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("UpdateTRLoading")]
        [HttpPost]
        public ResultMessage UpdateTRLoading([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                TblTRLoadingTO tblTRLoadingTO = JsonConvert.DeserializeObject<TblTRLoadingTO>(data["tblTRLoadingTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblTRLoadingTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                String errorMsg = "";
                result = _iTblTRLoadingBL.UpdateTblTRLoading(tblTRLoadingTO, Convert.ToInt32(loginUserId), ref errorMsg);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be Update");
                    resultMessage.Text = errorMsg;
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTRLoading");
                return resultMessage;
            }
        }

        /// <summary>
        /// AmolG[2022-Jan-20] Delete the TR Loading by passing the TRLoadingTO and loginUserId
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("DeleteTRLoading")]
        [HttpPost]
        public ResultMessage DeleteTRLoading([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                TblTRLoadingTO tblTRLoadingTO = JsonConvert.DeserializeObject<TblTRLoadingTO>(data["tblTRLoadingTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblTRLoadingTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                result = _iTblTRLoadingBL.DeleteTblTRLoading(tblTRLoadingTO, Convert.ToInt32(loginUserId));

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be Delete");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveTRLoading");
                return resultMessage;
            }
        }

        /// <summary>
        /// AmolG[2022-Jan-20] Change the Status of TR Loading by passing the TRLoadingTO and loginUserId
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("ChangeTRLoadingStatus")]
        [HttpPost]
        public ResultMessage ChangeTRLoadingStatus([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                TblTRLoadingTO tblTRLoadingTO = JsonConvert.DeserializeObject<TblTRLoadingTO>(data["tblTRLoadingTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblTRLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("tblTRLoadingTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                String errorMsg = "";
                result = _iTblTRLoadingBL.UpdateTblTRLoadingStatus(tblTRLoadingTO, Convert.ToInt32(loginUserId), ref errorMsg);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error... Record could not be Status");
                    resultMessage.Text = errorMsg;
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ChangeTRLoadingStatus");
                return resultMessage;
            }
        }

        // [Route("PostDimBrand")]
        // [HttpPost]
        // public ResultMessage PostDimBrand([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {

        //         DimBrandTO dimBrandTO = JsonConvert.DeserializeObject<DimBrandTO>(data["brandTO"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();
        //         if (dimBrandTO == null)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "brandTO Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "loginUserId Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }


        //         List<DimBrandTO> dimBrandTOList = BL.DimBrandBL.SelectAllDimBrandList(dimBrandTO);
        //         if (dimBrandTOList != null && dimBrandTOList.Count > 0)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "Brand Name Already Exist";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }


        //         //dimBrandTO.CreatedBy = Convert.ToInt32(loginUserId);
        //         dimBrandTO.CreatedOn =  _iCommonDAO.ServerDateTime;

        //         Int32 result = 0;

        //         if (dimBrandTO.IdBrand > 0)
        //         {
        //             result = BL.DimBrandBL.UpdateDimBrand(dimBrandTO);
        //             if (result == 1)
        //             {
        //                 resultMessage.MessageType = ResultMessageE.Information;
        //                 resultMessage.Result = 1;
        //                 resultMessage.Text = "Success... Brand Updated";
        //                 resultMessage.DisplayMessage = "Success... Brand Updated";
        //                 return resultMessage;
        //             }
        //         }
        //         else
        //         {
        //             result = BL.DimBrandBL.InsertDimBrand(dimBrandTO);
        //             if (result == 1)
        //             {
        //                 resultMessage.MessageType = ResultMessageE.Information;
        //                 resultMessage.Result = 1;
        //                 resultMessage.Text = "Success... Brand Saved";
        //                 resultMessage.DisplayMessage = "Success... Brand Saved";
        //                 return resultMessage;
        //             }
        //         }

        //         return resultMessage;
        //     }

        //     catch (Exception ex)
        //     {
        //         resultMessage.MessageType = ResultMessageE.Error;
        //         resultMessage.Text = "API : Exception In Method PostDimBrand";
        //         resultMessage.Result = -1;
        //         resultMessage.Exception = ex;
        //         return resultMessage;
        //     }
        // }

        // /// <summary>
        // /// [18-01-2018] Vijaymala:Added to save,update,deactivate group master
        // /// </summary>
        // /// <param name="data"></param>
        // /// <returns></returns>
        // [Route("PostGroupMaster")]
        // [HttpPost]
        // public ResultMessage PostGroupMaster([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {

        //         TblGroupTO tblGroupTO = JsonConvert.DeserializeObject<TblGroupTO>(data["groupTO"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();
        //         if (tblGroupTO == null)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "groupTO Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }
        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "loginUserId Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }


        //         List<TblGroupTO> tblGroupTOList = BL.TblGroupBL.SelectAllGroupList(tblGroupTO);
        //         if (tblGroupTOList != null && tblGroupTOList.Count > 0)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "Group Name Already Exist";
        //             resultMessage.DisplayMessage = "Group Name Already Exist";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }



        //         Int32 result = 0;

        //         if (tblGroupTO.IdGroup > 0)
        //         {
        //             tblGroupTO.UpdatedOn =  _iCommonDAO.ServerDateTime;
        //             tblGroupTO.UpdatedBy = Convert.ToInt32(loginUserId);
        //             return BL.TblGroupBL.UpdateTblGroup(tblGroupTO);
        //         }
        //         else
        //         {
        //             tblGroupTO.CreatedOn =  _iCommonDAO.ServerDateTime;
        //             tblGroupTO.CreatedBy = Convert.ToInt32(loginUserId);
        //             result = BL.TblGroupBL.InsertTblGroup(tblGroupTO);
        //             if (result != 1)
        //             {

        //                 resultMessage.MessageType = ResultMessageE.Error;
        //                 resultMessage.Text = "Error While InsertTblGroup";
        //                 resultMessage.DisplayMessage = "Record Cound Not Saved";
        //                 return resultMessage;

        //             }
        //             resultMessage.MessageType = ResultMessageE.Information;
        //             resultMessage.Result = 1;
        //             resultMessage.Text = "Success... Group Saved";
        //             resultMessage.DisplayMessage = "Success... Group Saved";


        //         }

        //         return resultMessage;
        //     }

        //     catch (Exception ex)
        //     {
        //         resultMessage.MessageType = ResultMessageE.Error;
        //         resultMessage.Text = "API : Exception In Method PostGroupMaster";
        //         resultMessage.Result = -1;
        //         resultMessage.Exception = ex;
        //         return resultMessage;
        //     }
        // }

        #endregion

        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [Route("GetRoleWiseUserListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUnloadingPersonListForDropDown(string roleId)
        {
            List<DropDownTO> statusList = _iTblUserBL.GetUnloadingPersonListForDropDown(roleId);
            return statusList;
        }

        [Route("GetAllVaribleDtls")]
        [HttpGet]
        public List<TblVariablesTO> GetAllVaribleDtls()
        {
            Int32 isActive = 1;
            List<TblVariablesTO> tblVariablesTOList = _iTblVariablesBL.SelectAllTblVariables(isActive);
            return tblVariablesTOList;
        }
        [Route("GetAllProcessVaribleDtlsList")]
        [HttpGet]
        public List<DropDownTO> GetAllProcessVaribleDtlsList(Int32 isProcessVar)
        {
            List<DropDownTO> tblVariablesTOList = _iTblVariablesBL.SelectVariableList(isProcessVar);
            return tblVariablesTOList;
        }
        [Route("GetAllExpressionDtls")]
        [HttpGet]
        public List<TblExpressionDtlsTO> GetAllExpressionDtls(Int32 prodClassId)
        {
            Int32 isActive = 1;
            List<TblExpressionDtlsTO> tblExpressionDtlsTOList = _iTblExpressionDtlsBL.SelectAllTblExpressionDtls(isActive, prodClassId);
            return tblExpressionDtlsTOList;
        }
        [Route("GetAllExpressionDtlsList")]
        [HttpGet]
        public List<TblExpressionDtlsTO> GetAllExpressionDtlsList()
        {
            Int32 isActive = 1;
            List<TblExpressionDtlsTO> tblExpressionDtlsTOList = _iTblExpressionDtlsBL.SelectAllTblExpressionDtlsList();
            return tblExpressionDtlsTOList;
        }

        [Route("GetUploadedImageDtls")]
        [HttpGet]
        public List<TblDocumentDetailsTO> GetUploadedImageDtls(string txnId, Int32 txnTypeId)
        {
            Int32 isActive = 1;
            List<TblDocumentDetailsTO> tblDocumentDetailsTOList = _iTblRecycleDocumentBL.SelectAllRecycleDocumentList(txnId, txnTypeId, isActive);
            return tblDocumentDetailsTOList;
        }

        [Route("PostVariableDetails")]
        [HttpPost]
        public ResultMessage PostVariableDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {

                List<TblVariablesTO> tblVariablesTOList = JsonConvert.DeserializeObject<List<TblVariablesTO>>(data["VariablesTOList"].ToString());

                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                if (tblVariablesTOList == null)
                {
                    resultMessage.DefaultBehaviour("Variable List found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                return _iTblVariablesBL.PostVariableDetails(tblVariablesTOList, loginUserId);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostVariableDetails");
                return resultMessage;
            }
        }

        [Route("GetAllImportedVariables")]
        [HttpGet]
        public List<TblVariablesTO> GetAllImportedVariables()
        {
            Int32 isActive = 1;
            return _iTblVariablesBL.SelectAllTblVariables(isActive);
        }

        [Route("GetHistoryOfVariablesbyUniqueNo")]
        [HttpGet]
        public List<TblVariablesTO> GetHistoryOfVariablesbyUniqueNo(int UniqueTrackId)
        {
            return _iTblVariablesBL.GetHistoryOfVariablesbyUniqueNo(UniqueTrackId);
        }

        [Route("GetHistoryOfExpressionsbyUniqueNo")]
        [HttpGet]
        public List<TblExpressionDtlsTO> GetHistoryOfExpressionsbyUniqueNo(int UniqueTrackId)
        {
            return _iTblExpressionDtlsBL.GetHistoryOfExpressionsbyUniqueNo(UniqueTrackId);
        }




        [Route("EditVariableDetails")]
        [HttpPost]
        public ResultMessage EditVariableDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {

                TblVariablesTO tblVariablesTO = JsonConvert.DeserializeObject<TblVariablesTO>(data["VariablesTO"].ToString());

                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                if (tblVariablesTO == null)
                {
                    resultMessage.DefaultBehaviour("Variable TO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                return _iTblVariablesBL.EditVariableDetails(tblVariablesTO, loginUserId);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "EditVariableDetails");
                return resultMessage;
            }
        }



        [Route("EditExpressionDetails")]
        [HttpPost]
        public ResultMessage EditExpressionDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {

                TblExpressionDtlsTO expTO = JsonConvert.DeserializeObject<TblExpressionDtlsTO>(data["ExpTO"].ToString());

                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                if (expTO == null)
                {
                    resultMessage.DefaultBehaviour("Expression TO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                return _iTblExpressionDtlsBL.EditExpressionDetails(expTO, loginUserId);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "EditExpressionDetails");
                return resultMessage;
            }
        }


        #endregion


        [Route("GetInventoryReportDetails")]
        [HttpPost]
        public List<dynamic> GetInventoryReportDetails([FromBody] JObject data)
        {
           TblReportsTO tblReportsTO1 = JsonConvert.DeserializeObject<TblReportsTO>(data["tblReportsTO"].ToString());

            //DateTime frmDateStr = DateTime.MinValue;
            //if (Constants.IsDateTime(fromDate))
            //{
            //    frmDateStr = Convert.ToDateTime(fromDate);
            //}

            //DateTime toDateStr = DateTime.MinValue;
            //if (Constants.IsDateTime(toDate))
            //{
            //    toDateStr = Convert.ToDateTime(toDate);
            //}

            if (tblReportsTO1 == null)
            {
                return null;
            }

            return _iTblGradeQtyDtlsBL.SelectGradeQtyDetails(tblReportsTO1);

        }

        //reshmaP[19-01-22] For Internal Tranfer
        [Route("GetVehicalTypeList")]
        [HttpGet]
        public List<DropDownTO> GetVehicalTypeList()
        {
            List<DropDownTO> VehicalTypeTOList = _idimensionbl.SelectVehicalType();
            return VehicalTypeTOList;
        }



    }
}
