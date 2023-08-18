using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class MastersController : Controller
    {
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
            List<DimStatusTO> statusList = BL.DimStatusBL.SelectAllDimStatusList(txnTypeId);
            List<DropDownTO> list = new List<DropDownTO>();
            if (statusList != null && statusList.Count > 0)
            {
                for (int i = 0; i < statusList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = statusList[i].StatusName;
                    dropDownTO.Value = statusList[i].IdStatus;
                    list.Add(dropDownTO);
                }
            }

            return list;
        }

        [Route("GetStatusListByTxnTypeId")]
        [HttpGet]
        public List<DimStatusTO> GetStatusListByTxnTypeId(Int32 txnTypeId)
        {
            List<DimStatusTO> statusList = BL.DimStatusBL.SelectAllDimStatusList(txnTypeId);
            return statusList;
        }

        [Route("GetPagesListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPagesListForDropDown(Int32 moduleId = 0)
        {
            List<TblPagesTO> pagesList = BL.TblPagesBL.SelectAllTblPagesList(moduleId);
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
            List<TblPageElementsTO> pageEleList = BL.TblPageElementsBL.SelectAllTblPageElementsList(pageId);
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
            List<DimVehicleTypeTO> vehTypeList = BL.DimVehicleTypeBL.SelectAllDimVehicleTypeList();
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
            List<DropDownTO> statusList = BL.DimensionBL.SelectCDStructureForDropDown();
            return statusList;
        }

        [Route("GetDeliveryPeriodForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetDeliveryPeriodForDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectDeliPeriodForDropDown();
            return statusList;
        }

        [Route("GetBookingTypeDropDown")]
        [HttpGet]
        public List<DropDownTO> GetBookingTypeDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectBookingTypeDropDown();
            return statusList;
        }
        [Route("GetMaxAllowedDeliveryPeriod")]
        [HttpGet]
        public Int32 GetMaxAllowedDeliveryPeriod()
        {
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_MAX_ALLOWED_DEL_PERIOD);
            Int32 maxAllowedDelPeriod = 7;

            if (tblConfigParamsTO != null)
                maxAllowedDelPeriod = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

            return maxAllowedDelPeriod;
        }

        [Route("GetDistrictForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetDistrictForDropDown(Int32 stateId)
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectDistrictForDropDown(stateId);
            return statusList;
        }

        [Route("getServerTime")]
        [HttpGet]
        public IActionResult  getServerTime()
        {
            DateTime currentDate = StaticStuff.Constants.ServerDateTime;
            return Ok(currentDate);
        }

        [Route("GetCountriesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCountriesForDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectCountriesForDropDown();
            return statusList;
        }

        [Route("GetStatesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetStatesForDropDown(Int32 countryId)
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectStatesForDropDown(countryId);
            return statusList;
        }

        [Route("GetTalukasForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetTalukasForDropDown(Int32 districtId)
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectTalukaForDropDown(districtId);
            return statusList;
        }
        //Added By kiran for CRM entity range 
        [Route("SelectEntityRangeTOFromVisitType")]
        [HttpGet]
        public IActionResult SelectEntityRangeTOFromVisitType(string entityName, DateTime createdOn)
        {
            try
            {
                TblEntityRangeTO EntityRangeTO =  BL.DimensionBL.SelectEntityRangeTOFromVisitType(entityName, createdOn);
                if (EntityRangeTO != null)
                {
                     return Ok(EntityRangeTO);
                }
                else
                {
                    return NotFound(EntityRangeTO);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("GetSalutaionForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetSalutaionForDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectSalutationsForDropDown();
            return statusList;
        }

        [Route("GetCommerLicensesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCommerLicensesForDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectOrgLicensesForDropDown();
            return statusList;
        }

        [Route("GetRoleListWrtAreaAllocation")]
        [HttpGet]
        public List<DropDownTO> GetRoleListWrtAreaAllocation()
        {
            List<DropDownTO> roleList = BL.DimensionBL.SelectRoleListWrtAreaAllocationForDropDown();
            return roleList;
        }

        [Route("GetRoleListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetRoleListForDropDown()
        {
            List<DropDownTO> roleList = BL.DimensionBL.SelectAllSystemRoleListForDropDown();
            return roleList;
        }


        [Route("GetDefaultRoleListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetDefaultRoleListForDropDown()
        {
            List<DropDownTO> roleList = BL.DimensionBL.SelectDefaultRoleListForDropDown();
            return roleList;
        }

        [Route("GetCnfDistrictForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCnfDistrictForDropDown(Int32 cnfOrgId)
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectCnfDistrictForDropDown(cnfOrgId);
            return statusList;
        }

        [Route("GetStockYardList")]
        [HttpGet]
        public List<TblStockYardTO> GetStockYardList()
        {
            return BL.TblStockYardBL.SelectAllTblStockYardList();
        }

        [Route("GetSuperwisorDetailList")]
        [HttpGet]
        public List<TblSupervisorTO> GetSuperwisorDetailList()
        {
            return BL.TblSupervisorBL.SelectAllTblSupervisorList();
        }


        [Route("GetTransportModeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetTransportModeForDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectAllTransportModeForDropDown();
            return statusList;
        }

        [Route("GetInvoiceTypeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetInvoiceTypeForDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectInvoiceTypeForDropDown();
            return statusList;
        }

        [Route("GetCurrencyForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCurrencyForDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectCurrencyForDropDown();
            return statusList;
        }

        [Route("GetInvoiceStatusForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {
            List<DropDownTO> statusList = BL.DimensionBL.GetInvoiceStatusForDropDown();
            return statusList;
        }

        //Vijaymala[08-09-2017] Added To Get Designation List
        [Route("GetDesignationList")]
        [HttpGet]
        public List<DimMstDesignationTO> GetDesignationList()
        {
            return BL.DimMstDesignationBL.SelectAllDimMstDesignationList();
        }



        [Route("GetAddOnsDetails")]
        [HttpGet]
        public List<TblAddonsFunDtlsTO> GetAddOnsDetails(int transId, int ModuleId, String TransactionType, String PageElementId = null, String transIds = null)
        {
            if (transId > 0)
            {
                return BL.TblAddonsFunDtlsBL.SelectAddonDetails(transId, ModuleId, TransactionType, PageElementId, transIds);
            }
            return null;
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] added to fill UnitMeasures Drop Down
        /// </summary>
        /// <returns></returns>
        [Route("GetUnitMeasuresForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUnitMeasuresForDropDown(int unitMeasureTypeId)
        {
            List<DropDownTO> unitMeasuresList = BL.DimUnitMeasuresBL.SelectAllUnitMeasuresListForDropDown(unitMeasureTypeId);
            return unitMeasuresList;
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] added to fill UnloadingStandDesc Drop Down
        /// </summary>
        /// <returns></returns>
        [Route("GetUnloadingStandDescForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetUnloadingStandDescForDropDown()
        {
            List<DropDownTO> unloadingStandDescList = BL.TblUnloadingStandDescBL.SelectAllUnloadingStandDescForDropDown();
            return unloadingStandDescList;
        }

        /// <summary>
        /// Vaibhav [15-Sep-2017] Get all department
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllDepartmentList")]
        [HttpGet]
        public List<DimMstDeptTO> GetAllDepartmentList()
        {
            return BL.DimMstDeptBL.SelectAllDimMstDeptList();
        }

        /// <summary>
        /// Deepali [19-Oct-2018] Get all department
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetUserListDepartmentWise")]
        [HttpGet]
        public List<DropDownTO> GetUserListDepartmentWise(string deptId)
        {
            return BL.DimensionBL.GetUserListDepartmentWise(deptId);
        }

        /// <summary>
        /// vinod on Date:12/12/2017
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("getProdSpecDescList")]
        [HttpGet]
        public List<DimProdSpecDescTO> getProdSpecDescList()
        {     
            return BL.DimProdSpecDescBL.SelectAllDimProdSpecDescList();
        }

        [Route("GetLoadingLayerList")]
        [HttpGet]
        public List<DropDownTO> GetLoadingLayerList()
        {
            List<DropDownTO> loadingLayerList = BL.DimensionBL.SelectLoadingLayerList();
            return loadingLayerList;
        }


        /// <summary>
        /// Vaibhav [15-Sep-2017] added to fill division drop down.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetDivisionDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetDivisionDropDownList(Int32 DeptTypeId = 0)
        {
            List<DropDownTO> departmentMasterList = BL.DimMstDeptBL.SelectDivisionDropDownList(DeptTypeId);
            return departmentMasterList;
        }


        /// <summary>
        /// Vaibhav [18-Sep-2017] added to fill department drop down by specific division 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [Route("GetDepartmentDropDownListByDivision")]
        [HttpGet]
        public List<DropDownTO> GetDepartmentDropDownListByDivision(Int32 DivisionId)
        {
            List<DropDownTO> divisionList = BL.DimMstDeptBL.SelectDepartmentDropDownListByDivision(DivisionId);
            return divisionList;
        }

        /// <summary>
        /// Vaibhav [19-Sep-2017] Added to select BOM department
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetBOMDepartmentTO")]
        [HttpGet]
        public DropDownTO GetBOMDepartmentTO()
        {
            DropDownTO BOMDepartmentTO = BL.DimMstDeptBL.SelectBOMDepartmentTO();
            return BOMDepartmentTO;
        }

        /// <summary>
        /// Vaibhav [13-Sep-2017] Get all UnloadingStandDesc List
        /// </summary>
        /// <returns></returns>
        [Route("GetUnloadingStandDescList")]
        [HttpGet]
        public List<TblUnloadingStandDescTO> GetUnloadingStandDescList()
        {
            List<TblUnloadingStandDescTO> unloadingStandDescList = BL.TblUnloadingStandDescBL.SelectAllTblUnloadingStandDescList();
            return unloadingStandDescList;
        }



        /// <summary>
        /// Vaibhav [25-Sep-2017] get all sub departments for drop down by department
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetSubDepartmentDropDownListByDepartment")]
        [HttpGet]
        public List<DropDownTO> GetSubDepartmentDropDownListByDepartment(int DepartmentId)
        {
            List<DropDownTO> subDepartmentTOList = BL.DimMstDeptBL.SelectSubDepartmentDropDownListByDepartment(DepartmentId);
            return subDepartmentTOList;
        }


        /// <summary>
        /// Vaibhav [25-Sep-2017] get all designation for drop down
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllDesignationForDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetAllDesignationForDropDown()
        {
            List<DropDownTO> designationList = BL.DimMstDesignationBL.SelectAllDesignationForDropDownList();
            return designationList;
        }

        /// <summary>
        /// Vaibhav [27-Sep-2017] added to select all designation list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetReportingTypeList")]
        [HttpGet]
        public List<DropDownTO> GetReportingTypeList()
        {
            return BL.DimensionBL.GetReportingType();
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
            return BL.DimensionBL.GetVisitIssueReasonsList();
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
            return BL.DimStateBL.SelectAllDimState();
        }

        [Route("GetStateCode")]
        [HttpGet]
        public DropDownTO GetStateCode(Int32 stateId)
        {
            DropDownTO stateCodeTO = BL.DimensionBL.SelectStateCode(stateId);
            return stateCodeTO;
        }


        [Route("GetArrangeForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetArrangeForDropDown()
        {
            return BL.DimensionBL.GetArrangeForDropDown();
        }

        [Route("GetArrangeVisitToDropDown")]
        [HttpGet]
        public List<DropDownTO> GetArrangeVisitToDropDown()
        {
            return BL.DimensionBL.GetArrangeVisitToDropDown();
        }

        [Route("GetCallBySelfForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCallBySelfForDropDown()
        {
            return BL.DimensionBL.GetCallBySelfForDropDown();
        }

        /// <summary>
        /// Sudhir[08-MARCH-2018]-This Methods Shows AllFirmTypes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllFirmTypesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetAllFirmTypesForDropDown()
        {
            List<DropDownTO> firmTypesList = BL.DimensionBL.GetAllFirmTypesForDropDown();
            return firmTypesList;
        }

        /// <summary>
        /// Sudhir[08-MARCH-2018]-This Methods Shows AllInfluencerType
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllInfluencerTypesForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetAllInfluencerTypesForDropDown()
        {
            List<DropDownTO> firmTypesList = BL.DimensionBL.GetAllInfluencerTypesForDropDown();
            return firmTypesList;
        }



        /// <summary>
        /// Sudhir[23-APR-2018] Added for Select All Products .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllItemBroadCategories")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblItemBroadCategoriesTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllItemBroadCategories()
        {
            try
            {
                List<TblItemBroadCategoriesTO> list = BL.TblItemBroadCategoriesBL.SelectAllTblItemBroadCategories();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sudhir[23-APR-2018] Added for Select All OtherDesignations .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllOtherDesignations")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblOtherDesignationsTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllOtherDesignations()
        {
            try
            {
                List<TblOtherDesignationsTO> list = BL.TblOtherDesignationsBL.SelectAllTblOtherDesignations();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sudhir[24-APR-2018] Added for Get All Master Org Types.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// 
        [Route("GetAllOrganizationTypes")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public  IActionResult GetAllOrganizationTypes()
        {
            try
            {
                List<DropDownTO> list = BL.DimensionBL.GetAllOrganizationType();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        //Sudhir [07-05-2018] Added To Get Emailconfigration List
        [Route("GetEmailConfigurationList")]
        [HttpGet]
        public List<TblEmailConfigrationTO> GetEmailConfigurationList()
        {
            return BL.TblEmailConfigrationBL.SelectAllDimEmailConfigrationList();
        }


        //Sudhir [10-05-2018] Added To Get Emailconfigration List
        [Route("GetUploadedFileBasedOnFileType")] 
        [HttpGet]
        public List<TblDocumentDetailsTO> GetUploadedFileBasedOnFileType(Int32 FilteTypeId,Int32 createdBy)
        {
            return BL.TblDocumentDetailsBL.GetUploadedFileBasedOnFileType(FilteTypeId, createdBy);
        }

        [Route("SelectAllTblDocumentDetailsList")]
        [HttpGet]
        public List<TblDocumentDetailsTO> SelectAllTblDocumentDetailsList(Int32 createdBy)
        {
            return BL.TblDocumentDetailsBL.SelectAllTblDocumentDetailsList(createdBy);
        }
        
        /// <summary>
        /// Sudhir[22-MAY-2018] Get All UnitMeasurement Types
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 
        [Route("GetAllUnitMeasurement")]
        [HttpGet]
        public List<DimUnitMeasuresTO> GetAllUnitMeasurement()
        {
            return BL.DimUnitMeasuresBL.SelectAllDimUnitMeasuresList();
        }

        //Kiran[15-Mar-2018] Added for get All  Dimesion Tables From tblMasterDimension
        [Route("GetMasterDimensionList")]
        [HttpGet]
        public List<DimensionTO> GetMasterDimensionList()
        {
            List<DimensionTO> masterList = BL.DimensionBL.SelectAllMasterDimensionList();
            return masterList;
        }
        /// <summary>
        /// Priyanka [23-04-2019] : Added to get the SAP Master Value according to their type.
        /// </summary>
        /// <returns></returns>
        [Route("GetSAPMasterDropDown")]
        [HttpGet]
        public List<DropDownTO> GetSAPMasterDropDown(Int32 dimensionId)
        {
            List<DropDownTO> SAPMasterList = BL.DimensionBL.GetSAPMasterDropDown(dimensionId);
            return SAPMasterList;
        }

        //Kiran[31-10-2018] 
        [Route("removeAllMsgHistory")]
        [HttpGet]
        public Int32 removeAllMsgHistory()
        {
            return BL.TblSessionBL.deleteAllMsgData();
        }
        

        /// <summary>
        /// Kiran [08-Sep-2018] added to fill UnitMeasures Drop Down Using Cat Id
        /// </summary>
        /// <returns></returns>
        [Route("SelectAllUnitMeasuresForDropDownByCatId")]
        [HttpGet]
        public List<DropDownTO> SelectAllUnitMeasuresForDropDownByCatId(Int32 unitCatId)
        {
            List<DropDownTO> unitMeasuresList = BL.DimUnitMeasuresBL.SelectAllUnitMeasuresForDropDownByCatId(unitCatId);
            return unitMeasuresList;
        }

        [Route("GetPersonOnPersonId")]
        [HttpGet]
        public TblPersonTO GetPersonOnPersonId(Int32 personId)
        {
            return BL.TblPersonBL.SelectTblPersonTO(personId);
        }

        /// <summary>
        /// Sudhir[21-JUNE-2018]
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [Route("GetDropDownListOnPersonId")]
        [HttpGet]
        public List<DropDownTO> GetDropDownListOnPersonId(Int32 personId)
        {
            return BL.TblPersonBL.SelectDropDownListOnPersonId(personId);
        }

        [Route("GetTaskModuleDetailsByEntityId")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblTaskModuleExtTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetTaskModuleDetailsByEntityId(Int32 EntityId)
        {
            try
            {
                List<TblTaskModuleExtTO> list = BL.TblTaskModuleExtBL.SelectTaskModuleDetailsByEntityId(EntityId);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Kiran[16-08-2018] To get module Communication List using moduleId and entityId
        /// </summary>
        /// <param name="srcModuleId"></param>
        /// <param name="srcTxnId"></param>
        /// <returns></returns>
        [Route("GetModuleCommunicationDetailsById")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblModuleCommunicationTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetModuleCommunicationDetailsById(Int32 srcModuleId, Int32 srcTxnId)
        {
            try
            {
                List<TblModuleCommunicationTO> list = BL.TblModuleCommunicationBL.SelectAllTblModuleCommunicationListById(srcModuleId, srcTxnId);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Dipali[26-7-2018] Added for get List of role and organization type Configuration based on VisitTypeId and personTypeId.
        /// </summary>
        /// <param name="visitTypeId"></param>
        /// <param name="personTypeId"></param>
        /// <returns></returns>
        [Route("GetRoleOrgListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetRoleOrgListForDropDown(int visitTypeId, int personTypeId)
        {
            List<DropDownTO> roleOrgList = BL.TblRoleOrgSettingBL.SelectAllSystemRoleOrgListForDropDown(visitTypeId, personTypeId);
            return roleOrgList;
        }

        //Dipali[26-07-2018] For RoleOrgTo List Mapping With Role
        [Route("GetAllRoleListForDropDown")]
        [HttpGet]
        public List<RoleOrgTO> GetAllRoleListForDropDown(int visitTypeId, int personTypeId)
        {
            return BL.DimensionBL.SelectAllSystemRoleListForTbl(visitTypeId,personTypeId);

        }

        //Dipali[26-07-2018] For RoleOrgTo List Mapping With Role
        [Route("GetAllOrgListForDropDown")]
        [HttpGet]
        public List<RoleOrgTO> GetAllOrgListForDropDown(int visitTypeId, int personTypeId)
        {
            return BL.DimensionBL.SelectAllSystemOrgListForTbl(visitTypeId,personTypeId);

        }
        [Route("GetAllVisitListForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetAllVisitListForDropDown()
        {
            List<DropDownTO> visitList = BL.DimensionBL.SelectAllVisitTypeListForDropDown();
            return visitList;
        }
        [Route("GetServerDatetime")]
        [HttpGet]
        public JsonResult GetServerDatetime()
        {
            string datetimeStr= Constants.ServerDateTime.ToString(Constants.DefaultDateOnlyFormat);
            //string datetimeStr = "ABC";
            return Json(datetimeStr);
        }

        /// <summary>
        /// Sudhir[30-08-2018] To get module Communication List using moduleId and entityId
        /// </summary>
        /// <param name="srcModuleId"></param>
        /// <param name="srcTxnId"></param>
        /// <returns></returns>
        [Route("GetTasksWithousSubscList")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TbltaskWithoutSubscTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetTasksWithousSubscList(Int32 srcModuleId, Int32 entityId)
        {
            try
            {
                List<TbltaskWithoutSubscTO> list = BL.TbltaskWithoutSubscBL.SelectTbltaskWithoutSubscList(srcModuleId, entityId);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        //
        /// <summary>
        /// Sudhir[06-09-2018] To get Fixed DropDown List 
        /// </summary>
        /// <param name="srcModuleId"></param>
        /// <param name="srcTxnId"></param>
        /// <returns></returns>
        [Route("GetFixedDropDownValues")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetFixedDropDownValues()
        {
            try
            {
                List<DropDownTO> list = BL.DimensionBL.GetFixedDropDownValues();
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        //Deepali [22-10-2018] Added To Get Emailconfigration List
        [Route("GetUploadedFileBasedOnDocumentId")]
        [HttpGet]
        public List<TblDocumentDetailsTO> GetUploadedFileBasedOnDocumentId(string DocumentIds)
        {
            return BL.TblDocumentDetailsBL.GetUploadedFileBasedOnDocumentId(DocumentIds);
        }

        //
        /// <summary>
        /// Sudhir[06-09-2018] To get Fixed DropDown List 
        /// </summary>
        /// <returns></returns>
        [Route("GetMasterSiteTypes")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetMasterSiteTypes(int parentSiteTypeId = 0)
        {
            try
            {
                List<DropDownTO> list = BL.DimensionBL.SelectMasterSiteTypes(parentSiteTypeId);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        //[Route("GetDynamicData")]
        //[HttpGet]
        //public JsonResult GetDynamicData()
        //{

        //    List<DynamicReportTO> data = BL.TblReportsBL.GetDynamicData("ABC", null);

        //    List<DynamicReportTO> listDict = data;//new List<DynamicReportTO>();
        //    int key = 0;
        //    var toDict = listDict.Select(p => new { id = key++, dropdownTo = p })
        //        .ToDictionary(x => x.id, x => x.dropdownTo);

        //    if (data != null)
        //    {
        //        List<List<DropDownTO>> List = data.Select(element => element.DropDownList.ToList()).ToList();
        //        var jsonData = data.Select(id => id.DropDownList.Select(element => element.Text + ":" + element.Tag));
        //        return Json(List);
        //        //return Json(data.Select(id=>id.DropDownList.Select(element=>element.Text+ ":"+element.Tag  )));
        //       // return new List<DynamicReportTO>()
        //       //{
        //       //    new DynamicReportTO()
        //       //    {
        //       //       DropDownList=new List<DropDownTO>()
        //       //       {
        //       //           new DropDownTO()
        //       //           {
        //       //               Text="ABC",
        //       //               Tag=123
        //       //           }
        //       //       }
        //       //    }
        //       //};
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        #endregion

        #region POST
        // POST api/values
        [Route("AddNewStockYard")]
        [HttpPost]
        public Int32 AddNewStockYard([FromBody] JObject data)
        {
            try
            {
                TblStockYardTO tblStockYardTO = JsonConvert.DeserializeObject<TblStockYardTO>(data["stockYardInfo"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblStockYardTO == null)
                {
                    return 0;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    return 0;
                }

                tblStockYardTO.CreatedOn = Constants.ServerDateTime;
                tblStockYardTO.CreatedBy = Convert.ToInt32(loginUserId);
                return BL.TblStockYardBL.InsertTblStockYard(tblStockYardTO);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        //Vinod [08-09-2017] Added To Deactivate Product Specification Description Master 
        [Route("AddProdSpecDesc")]
        [HttpPost]
        public ResultMessage AddProdSpecDesc([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                DimProdSpecDescTO tblProdSpecDescTO = JsonConvert.DeserializeObject<DimProdSpecDescTO>(data["ProductSpecTO"].ToString());
                // DimProdSpecDescTO mstProdSpecDescTO = JsonConvert.DeserializeObject<DimProdSpecDescTO>(data["ProductSpecTO"].ToString()); //ProductSpecTO
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblProdSpecDescTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    tblProdSpecDescTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblProdSpecDescTO.UpdatedOn = Constants.ServerDateTime;
                    //  organizationTO.DeactivatedOn = serverDate;
                    tblProdSpecDescTO.IsVisible = 0;

                    int ProdSpecDescId = BL.DimProdSpecDescBL.SelectAllDimProdSpecDescriptionList();
                    tblProdSpecDescTO.IdProductSpecDesc = ProdSpecDescId;
                    int result = BL.DimProdSpecDescBL.InsertDimProdSpecDesc(tblProdSpecDescTO);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be Inserted");
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.DefaultBehaviour("tblProdSpecDescTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AddProdSpecDesc");
                return resultMessage;
            }
        }

        //Vinod [08-09-2017] Update To Product Specification Description Master 
        [Route("UpdateProdSpecDesc")]
        [HttpPost]
        public ResultMessage UpdateProdSpecDesc([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimProdSpecDescTO tblProdSpecDescTO = JsonConvert.DeserializeObject<DimProdSpecDescTO>(data["ProductSpecTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (tblProdSpecDescTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    tblProdSpecDescTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblProdSpecDescTO.UpdatedOn = Constants.ServerDateTime;
                    tblProdSpecDescTO.IsVisible = 0;
                    int result = BL.DimProdSpecDescBL.UpdateDimProSpecDesc(tblProdSpecDescTO);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error... Record could not be Updated");
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.DefaultBehaviour("tblProdSpecDescTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProdSpecDesc");
                return resultMessage;
            }
        }

        
        //Vinod [08-09-2017] Added To Deactivate Product Specification Description Master 
        [Route("PostDeactivateProdSpecDesc")]
        [HttpPost]
        public ResultMessage PostDeactivateProdSpecDesc([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimProdSpecDescTO mstProdSpecDescTO = JsonConvert.DeserializeObject<DimProdSpecDescTO>(data["ProductSpecTO"].ToString()); //ProductSpecTO
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (mstProdSpecDescTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    mstProdSpecDescTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    mstProdSpecDescTO.UpdatedOn = Constants.ServerDateTime;
                    //  organizationTO.DeactivatedOn = serverDate;
                    mstProdSpecDescTO.IsVisible = 0;

                    int result = BL.DimProdSpecDescBL.DeleteDimProSpecDesc(mstProdSpecDescTO);

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
                else
                {
                    resultMessage.DefaultBehaviour("mstProdSpecDescTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateProdSpecDesc");
                return resultMessage;
            }
        }

        [Route("PostNewSuperwisorMaster")]
        [HttpPost]
        public ResultMessage PostNewSuperwisorMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblSupervisorTO supervisorTO = JsonConvert.DeserializeObject<TblSupervisorTO>(data["supervisorTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (supervisorTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "supervisorTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                supervisorTO.CreatedBy = Convert.ToInt32(loginUserId);
                supervisorTO.CreatedOn = Constants.ServerDateTime;
                supervisorTO.IsActive = 1;
                return BL.TblSupervisorBL.SaveNewSuperwisor(supervisorTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostNewSuperwisorMaster";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        //Kiran[25-oct-2018] start session of new conversion
        [Route("PostNewSessionData")]
        [HttpPost]
        public ResultMessage PostNewSessionData([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            try
            {
                TblSessionTO sessionTo = JsonConvert.DeserializeObject<TblSessionTO>(data.ToString());
                sessionTo.IsActive = 1;
                sessionTo.IsEndSession = 0;
                sessionTo.StartTime = StaticStuff.Constants.ServerDateTime;
                sessionTo.EndTime = StaticStuff.Constants.ServerDateTime;
                result = BL.TblSessionBL.InsertTblSession(sessionTo);
                if (result != 0)
                {
                    resultMessage.Tag = sessionTo.Idsession;
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Tag = result;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostNewSessionData";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        //Kiran[15-01-2019] Store all list of AddOn Transaction
        [Route("PostAddOnsDetails")]
        [HttpPost]
        public ResultMessage PostAddOnsDetails([FromBody] JObject data)
        {

            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList = JsonConvert.DeserializeObject<List<TblAddonsFunDtlsTO>>(data["tblAddonsFunDtlsTO"].ToString());
                if (tblAddonsFunDtlsTOList == null || tblAddonsFunDtlsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("tblAddonsFunDtlsTOList found null");
                    return resultMessage;
                }
                #region store all list against transaction id
                foreach (var item in tblAddonsFunDtlsTOList)
                {
                    if (item.IdAddonsfunDtls == 0)
                        resultMessage = BL.TblAddonsFunDtlsBL.InsertTblAddonsFunDtls(item);
                    else
                        resultMessage = BL.TblAddonsFunDtlsBL.UpdateTblAddonsFunDtls(item);

                    if (resultMessage.Result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in PostAddOnsDetails... Record could not be saved");
                        return resultMessage;
                    }
                }

                #endregion
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostAddOnsDetails");
                return resultMessage;
            }
        }

        //Kiran[25-oct-2018] Save All Conversion History  
        [Route("SaveAllConversionHistory")]
        [HttpPost]
        public ResultMessage SaveAllConversionHistory([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            try
            {
                List<TblSessionHistoryTO> SessionHistoryTOList = JsonConvert.DeserializeObject<List<TblSessionHistoryTO>>(data["SessionHistoryList"].ToString());
                if(SessionHistoryTOList != null && SessionHistoryTOList.Count > 0)
                {
                    foreach (var SessionHistoryTO in SessionHistoryTOList)
                    {
                        if (SessionHistoryTO.IsEdit == 1)
                        {
                            result = BL.TblSessionHistoryBL.InsertTblSessionHistory(SessionHistoryTO);
                            if(SessionHistoryTO.IsSendNotification == 1)
                            {
                                TblUserTO userTo = BL.TblUserBL.SelectTblUserTO(SessionHistoryTO.ConversionUserId);
                                string[] devices = new string[1];
                                if (userTo.RegisteredDeviceId != null)
                                {
                                    devices[0] = userTo.RegisteredDeviceId;
                                    String notifyBody = userTo.UserDisplayName + Environment.NewLine + SessionHistoryTO.ConversionBody;
                                    String notifyTitle = "SimpliChat";
                                    BL.VitplNotify.NotifyToRegisteredDevices(devices, notifyBody, notifyTitle,0);
                                    resultMessage.MessageType = ResultMessageE.Information;
                                    resultMessage.Text = "Acknowledged Sucessfully";
                                    resultMessage.Result = 1;
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            result = BL.TblSessionHistoryBL.UpdateTblSessionHistory(SessionHistoryTO);
                        }
                    }
                    if (result != 0)
                    {
                        //SessionHistoryTO.SessionId = result;
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method SaveAllConversionHistory";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }
        [Route("endChatSession")]
        [HttpGet]
        public ResultMessage endChatSession(Int32 idSession)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            try
            {
                result = BL.TblSessionBL.UpdateTblSession(idSession);
                if (result != 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method endChatSession";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        [Route("PostUpdateSuperwisorMaster")]
        [HttpPost]
        public ResultMessage PostUpdateSuperwisorMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblSupervisorTO supervisorTO = JsonConvert.DeserializeObject<TblSupervisorTO>(data["supervisorTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (supervisorTO == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "supervisorTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                return BL.TblSupervisorBL.UpdateSuperwisor(supervisorTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostUpdateSuperwisorMaster";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }
         
        // POST api/values 
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        //Kiran[15-Mar-2018] Added for Get Coloumn Names of Selected Table
        [Route("GetColumnName")]
        [HttpGet]
        public List<Dictionary<string, string>> GetColumnName(string tableName, Int32 tableValue)
        {

            if (tableName != null)
            {
                return BL.DimensionBL.GetColumnName(tableName, tableValue);
            }
            return null;
        }

        //Kiran[15-Mar-2018] Added for Add New Dimesion in Selected Table
        [Route("PostNewMasterData")]
        [HttpPost]
        public ResultMessage PostNewMasterData([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Int32 result = 0;
                var tableData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data["data"].ToString());
                var tableName = data["tableName"].ToString();
                var isAdded = data["isAdded"].ToString();
                if (isAdded == "True")
                {
                    result = BL.DimensionBL.saveNewDimensional(tableData, tableName);
                }
                else
                {
                    result = BL.DimensionBL.UpdateDimensionalData(tableData, tableName);
                }
                if (result != 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostNewMasterData";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        //Vijaymala[08-09-2017] Added To Insert Designation Master
        [Route("PostNewDesignationMaster")]
        [HttpPost]
        public ResultMessage PostNewDesignationMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (mstDesignationTO == null)
                {
                    resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                mstDesignationTO.CreatedBy = Convert.ToInt32(loginUserId);
                mstDesignationTO.CreatedOn = Constants.ServerDateTime;
                mstDesignationTO.IsVisible = 1;
                int result = BL.DimMstDesignationBL.InsertDimMstDesignation(mstDesignationTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewDesignationMaster");
                return resultMessage;
            }
        }

        //Vijaymala[08-09-2017] Added To Update Designation Master
        [Route("PostUpdateDesignationMaster")]
        [HttpPost]
        public ResultMessage PostUpdateDesignationMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (mstDesignationTO == null)
                {
                    resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }
                mstDesignationTO.UpdatedOn = Constants.ServerDateTime;
                mstDesignationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                int result = BL.DimMstDesignationBL.UpdateDimMstDesignation(mstDesignationTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateDesignationMaster");
                return resultMessage;
            }
        }

        //Vijaymala[08-09-2017] Added To Deactivate Designation 
        [Route("PostDeactivateDesignation")]
        [HttpPost]
        public ResultMessage PostDeactivateDesignation([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                DimMstDesignationTO mstDesignationTO = JsonConvert.DeserializeObject<DimMstDesignationTO>(data["mstDesignationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Found NULL");
                    return resultMessage;
                }

                if (mstDesignationTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    mstDesignationTO.DeactivatedBy = Convert.ToInt32(loginUserId);
                    mstDesignationTO.DeactivatedOn = Constants.ServerDateTime;
                    //  organizationTO.DeactivatedOn = serverDate;
                    mstDesignationTO.IsVisible = 0;

                    int result = BL.DimMstDesignationBL.UpdateDimMstDesignation(mstDesignationTO);
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
                else
                {
                    resultMessage.DefaultBehaviour("mstDesignationTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDesignation");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [16-Sep-2017] Added to insert new department. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostNewDepartmentMaster")]
        [HttpPost]
        public ResultMessage PostNewDepartmentMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                DimMstDeptTO dimMstDeptTO = JsonConvert.DeserializeObject<DimMstDeptTO>(data["mstDepartmentTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimMstDeptTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                // check parent department id not null
                if (dimMstDeptTO.ParentDeptId <= 0)
                {
                    resultMessage.DefaultBehaviour("ParentDeptId found null");
                    return resultMessage;
                }

                return BL.DimMstDeptBL.SaveDepartment(dimMstDeptTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewDepartment");
                return resultMessage;
            }
        }


        /// <summary>
        /// Vaibhav [16-Sep-2017] Added to update department master
        /// </summary>
        /// <param name="id"></param>
        [Route("PostUpdateDepartmentMaster")]
        [HttpPost]
        public ResultMessage PostUpdateDepartmentMaster([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimMstDeptTO dimMstDeptTO = JsonConvert.DeserializeObject<DimMstDeptTO>(data["mstDepartmentTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (dimMstDeptTO == null)
                {
                    resultMessage.DefaultBehaviour("dimMstDeptTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                // check parent department id not null
                if (dimMstDeptTO.ParentDeptId <= 0)
                {
                    resultMessage.DefaultBehaviour("ParentDeptId found null");
                    return resultMessage;
                }

                return BL.DimMstDeptBL.UpdateDepartment(dimMstDeptTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateDepartment");
                return resultMessage;
            }
        }

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


                return BL.DimStateBL.SaveNewState(dimStateTO);
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
                return BL.DimStateBL.UpdateState(dimStateTO);
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
                result = BL.DimStateBL.DeleteDimState(dimStateTO.IdState);
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
        ///Sudhir[24-APR-2018] Added for Uploading Image  
        /// </summary>
        /// <param name="tblDocumentDetailsTOTblDocumentDetailsTO"></param>
        /// <returns></returns>
        [Route("UploadDocument")]
        [HttpPost]
        public ResultMessage UploadDocument([FromBody] List<TblDocumentDetailsTO> data)
        {
            List<TblDocumentDetailsTO> tblDocumentDetailsTOList = data;
            return BL.TblDocumentDetailsBL.UploadDocument(tblDocumentDetailsTOList);
        }

        /// <summary>
        /// @Kiran 15-01-2019 to change old method and add FromBody  
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("UploadDocumentset")]
        [HttpPost]
        public ResultMessage UploadDocumentset([FromBody] JObject data)
        {
            List<TblDocumentDetailsTO> tblDocumentDetailsTOList = JsonConvert.DeserializeObject<List<TblDocumentDetailsTO>>(data["tblDocumentDetailsTOList"].ToString());
            return BL.TblDocumentDetailsBL.UploadDocument(tblDocumentDetailsTOList);
        } 

        /// <summary>
        /// [07-05-2018] Sudhir:Added to save email configration
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
                int result = BL.TblEmailConfigrationBL.InsertDimEmailConfigration(dimEmailConfigrationTO);
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
        /// Sudhir [07-05-2018] Added to update email configration
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
                int result = BL.TblEmailConfigrationBL.UpdateDimEmailConfigration(dimEmailConfigrationTO);
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
        
        /// <summary>
        /// Sudhir [07-05-2018] Added to update email configration
        /// </summary>
        /// <param name="id"></param>
        [Route("SendShareDocumentToMail")]
        [HttpPost]
        public async Task<ResultMessage> SendShareDocumentToMail([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            //ShareDetials
            try
            {
                //"TblCRMShareDocsDetailsTO": 

                int result = 0;
                TblCRMShareDocsDetailsTO tblCRMShareDocsDetailsTO = JsonConvert.DeserializeObject<TblCRMShareDocsDetailsTO>(data["tblCRMShareDocsDetailsTO"].ToString());
                if(tblCRMShareDocsDetailsTO != null)
                {
                    Task<ResultMessage> resultMessag = BL.TblCRMShareDocsDetailsBL.ShareDetialsAsync(tblCRMShareDocsDetailsTO);
                    resultMessage =await resultMessag;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendShareDocumentToMail");
               // resultMessage.DefaultBehaviour("Failed to Send Email");
                resultMessage.DisplayMessage = "Email Wasn't Sent.";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
        }

        [Route("UploadMultipleTypesFile")]
        [HttpPost]
        public async Task<IActionResult> UploadMultipleTypesFile(List<IFormFile> file,Int32 createdBy,Int32 FileTypeId, Int32 moduleId = 1)
        {
            BL.TblDocumentDetailsBL tblDocumentDetailsBL = new BL.TblDocumentDetailsBL();
            Task task = tblDocumentDetailsBL.UploadMultipleTypesFile(file,createdBy,FileTypeId, moduleId);
            return Ok(task);
        }

        //Sudhir[27-07-2018] Added for Asynchronous Call Example and Shows how Await work.
        [Route("AsyncTest")]
        [HttpPost]
        // Three things to note in the signature:  
        //  - The method has an async modifier.   
        //  - The return type is Task or Task<T>. (See "Return Types" section.)  
        //    Here, it is Task<int> because the return statement returns an integer.  
        //  - The method name ends in "Async."  
        public async Task<int> AsyncTest()
        {
            // You need to add a reference to System.Net.Http to declare client.  
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the  
            // task you'll get a string (urlContents).  
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.  
            int id = 15;
            List<DropDownTO> statusList = BL.DimensionBL.SelectDistrictForDropDown(id);
            // The await operator suspends AccessTheWebAsync.  
            //  - AccessTheWebAsync can't continue until getStringTask is complete.  
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.  
            //  - Control resumes here when getStringTask is complete.   
            //  - The await operator then retrieves the string result from getStringTask.  
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.  
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.  
            return urlContents.Length;
        }

        //SaveTaskModuleDetails

        /// <summary>
        /// Sudhir [07-05-2018] Added to Save TaskModule Details
        /// </summary>
        /// <param name="id"></param>
        [Route("SaveTaskModuleDetails")]
        [HttpPost]
        public ResultMessage SaveTaskModuleDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblTaskModuleExtTO tblTaskModuleExtTO = JsonConvert.DeserializeObject<TblTaskModuleExtTO>(data["tblTaskModuleExtTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblTaskModuleExtTO == null)
                {
                    resultMessage.DefaultBehaviour("tblTaskModuleExtTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                tblTaskModuleExtTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblTaskModuleExtTO.CreatedOn = Constants.ServerDateTime;
                int result = BL.TblTaskModuleExtBL.InsertTblTaskModuleExt(tblTaskModuleExtTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "SaveTaskModuleDetails");
                return resultMessage;
            }
        }



        //SaveModuleCommunicationDetails
        /// <summary>
        /// Kiran [16-08-2018] Added to Save Module Communication Details
        /// </summary>
        /// <param name="id"></param>
        [Route("SaveModuleCommunicationDetails")]
        [HttpPost]
        public ResultMessage SaveModuleCommunicationDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblModuleCommunicationTO> tblModuleCommunicationList = JsonConvert.DeserializeObject<List<TblModuleCommunicationTO>>(data["ModuleCommunicationList"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tblModuleCommunicationList == null)
                {
                    resultMessage.DefaultBehaviour("tblModuleCommunicationList found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                int result = BL.TblModuleCommunicationBL.InsertTblModuleCommunication(tblModuleCommunicationList, loginUserId);
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
                resultMessage.DefaultExceptionBehaviour(ex, "SaveTaskModuleDetails");
                return resultMessage;
            }
        }

        // POST api/values
        [Route("PostRolesAndOrgDetails")]
        [HttpPost]
        public ResultMessage PostRolesAndOrgDetails([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<RoleOrgTO> roleOrgTOList = JsonConvert.DeserializeObject<List<RoleOrgTO>>(data["roleOrgTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (roleOrgTOList == null || roleOrgTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : roleOrgTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = Constants.ServerDateTime;
                for (int i = 0; i < roleOrgTOList.Count; i++)
                {
                    roleOrgTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    roleOrgTOList[i].CreatedOn = confirmedDate;
                }

                ResultMessage resMsg = BL.TblRoleOrgSettingBL.SaveRolesAndOrg(roleOrgTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostRolesAndOrgDetails";
                return returnMsg;
            }
        }


        /// <summary>
        /// Sudhir [30-08-2018] Added to Save Tasks Without Subscription.
        /// </summary>
        /// <param name="id"></param>
        [Route("SaveTasksWithoutSubscription")]
        [HttpPost]
        public ResultMessage SaveTasksWithoutSubscription([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TbltaskWithoutSubscTO tbltaskWithoutSubscTO = JsonConvert.DeserializeObject<TbltaskWithoutSubscTO>(data["tbltaskWithoutSubscTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (tbltaskWithoutSubscTO == null)
                {
                    resultMessage.DefaultBehaviour("tbltaskWithoutSubscTO found null");
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                tbltaskWithoutSubscTO.CreatedOn = Constants.ServerDateTime;
                tbltaskWithoutSubscTO.CreatedBy = Convert.ToInt32(loginUserId);
                int result = BL.TbltaskWithoutSubscBL.InsertTbltaskWithoutSubsc(tbltaskWithoutSubscTO);
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
                resultMessage.DefaultExceptionBehaviour(ex, "SaveTaskModuleDetails");
                return resultMessage;
            }
        }
        /// <summary>
        /// Yogesh [24-10-2018] Added to get Department id From UserId Id
        /// </summary>
        /// <param name="id"></param>
        [Route("getDepartmentIdFromUserId")]
        [HttpGet]
        public TblRoleTO getDepartmentIdFromUserId(Int32 userId)
        {
            TblRoleTO statusList = BL.TblRoleBL.getDepartmentIdFromUserId(userId);
            return statusList;
        }

        ///// <summary>
        ///// Deepali [19-Oct-2018] Get all department
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //[Route("GetUserListDepartmentWise")]
        //[HttpGet]
        //public List<DropDownTO> GetUserListDepartmentWise(int deptId)
        //{
        //    return BL.DimensionBL.GetUserListDepartmentWise(deptId);
        //}

        #endregion

        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion



    }
}
