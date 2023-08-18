using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SalesTrackerAPI.BL;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrganizationController : Controller
    {

        private readonly ILogger loggerObj;

        public OrganizationController(ILogger<OrganizationController> logger)
        {
            loggerObj = logger;
            Constants.LoggerObj = logger;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        /// <summary>
        /// Sanjay [2017-02-03]
        /// </summary>
        /// <remarks>This API will Retrive All Organization List based on given Type Id.</remarks>
        /// <param name="orgTypeId"></param>
        /// <returns></returns>
        [Route("GetOrganizationList")]
        [HttpGet]
        public List<TblOrganizationTO> GetOrganizationList(Int32 orgTypeId)
        {
            Constants.OrgTypeE orgTypeE = (Constants.OrgTypeE)Enum.Parse(typeof(Constants.OrgTypeE), orgTypeId.ToString());
            List<TblOrganizationTO> list = BL.TblOrganizationBL.SelectAllTblOrganizationList(orgTypeE).OrderBy(o => o.FirmName).ToList(); 
            return list;
        }

        [Route("GetOrganizationInfo")]
        [HttpGet]
        public TblOrganizationTO GetOrganizationInfo(Int32 orgId)
        {
            return BL.TblOrganizationBL.SelectTblOrganizationTO(orgId);
        }

        [Route("GetDealerOrganizationList")]
        [HttpGet]
        public List<TblOrganizationTO> GetDealerOrganizationList(Int32 cnfId)
        {
            int orgTypeId = (int)Constants.OrgTypeE.DEALER;
            List<TblOrganizationTO> list = BL.TblOrganizationBL.SelectAllChildOrganizationList(orgTypeId, cnfId);
            return list;
        }

        [Route("GetOrgOwnerDetails")]
        [HttpGet]
        public List<TblPersonTO> GetOrgOwnerDetails(Int32 organizationId)
        {
            List<TblPersonTO> list = BL.TblPersonBL.SelectAllPersonListByOrganization(organizationId);
            return list;
        }

        [Route("GetOrgAddressDetails")]
        [HttpGet]
        public List<TblAddressTO> GetOrgAddressDetails(Int32 organizationId)
        {
            return BL.TblAddressBL.SelectOrgAddressList(organizationId);
        }

        [Route("GetOrgCommercialLicenseDetails")]
        [HttpGet]
        public List<TblOrgLicenseDtlTO> GetOrgCommercialLicenseDetails(Int32 organizationId)
        {
            return BL.TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(organizationId);
        }
        
        //Priyanka [22-10-2018] : Added to get the KYC details of the particular organization.
        [Route("GetKYCDetails")]
        [HttpGet]
        public List<TblKYCDetailsTO> GetKYCDetails(Int32 organizationId)
        {
            return BL.TblKYCDetailsBL.SelectTblKYCDetailsTOByOrgId(organizationId);
        }

        /// <summary>
        ///  Priyanka [08-04-2019] : Added to get the overdue details of dealer.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>

        [Route("GetDealersOverdueDetailsInfo")]
        [HttpGet]
        public List<SalesTrackerAPI.DashboardModels.DealerOverdueDtl> GetDealersOverdueDetailsInfo(Int32 dealerId)
        {
            return BL.TblOrganizationBL.GetDealersOverdueDetailsInfo(dealerId);
        }

        [Route("GetCompetitorBrandList")]
        [HttpGet]
        public List<TblCompetitorExtTO> GetCompetitorBrandList(Int32 organizationId)
        {
            return BL.TblCompetitorExtBL.SelectAllTblCompetitorExtList(organizationId);
        }

        /// <summary>
        /// Priyanka [16-02-18] : Added to get Purchase Competitor Material and Grade Details
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns>List of Material and Grade for given Purchase Competitor</returns>
        [Route("GetPurchaseCompetitorMaterialList")]
        [HttpGet]
        public List<TblPurchaseCompetitorExtTO> GetPurchaseCompetitorMaterialList(Int32 organizationId)
        {
            return BL.TblPurchaseCompetitorExtBL.SelectAllTblPurchaseCompetitorExtList(organizationId);
        }

        [Route("GetCompetitorListWithHistory")]
        [HttpGet]
        public List<TblOrganizationTO> GetCompetitorListWithHistory()
         {
            Constants.OrgTypeE orgTypeE = Constants.OrgTypeE.COMPETITOR;
            List<TblOrganizationTO> list = BL.TblOrganizationBL.SelectAllTblOrganizationList(orgTypeE);

            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_COMPETITOR_TO_SHOW_IN_HISTORY);
            if (tblConfigParamsTO == null)
                return list;
            else
            {
                String csParamVal = tblConfigParamsTO.ConfigParamVal;
                if (csParamVal == "0")
                    return list;
                else
                {
                    List<TblOrganizationTO> finalList = new List<TblOrganizationTO>();
                    if(list != null && list.Count > 0)
                    {
                        string[] idsToShow = csParamVal.Split(',');

                        for (int i = 0; i < idsToShow.Length; i++)
                        {
                            var orgTO = list.Where(a => a.IdOrganization == Convert.ToInt32(idsToShow[i])).LastOrDefault();
                            if (orgTO != null)
                                finalList.Add(orgTO);
                        }

                        finalList = finalList.OrderByDescending(o => o.CompetitorUpdatesTO.UpdateDatetime).ToList();
                        
                    }
                    return finalList;

                }
            }
        }


        [Route("GetOrganizationDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetOrganizationDropDownList(Int32 orgTypeId, String userRoleTOList)
        {
            Constants.OrgTypeE orgTypeE = (Constants.OrgTypeE)Enum.Parse(typeof(Constants.OrgTypeE), orgTypeId.ToString());
            if (orgTypeE == Constants.OrgTypeE.OTHER)
            {
                List<DropDownTO> list = BL.TblOtherSourceBL.SelectOtherSourceOfMarketTrendForDropDown();
                return list;
            }
            else
            {
                List<TblUserRoleTO> tblUserRoleTOList = new List<TblUserRoleTO>();
                if (!string.IsNullOrEmpty(userRoleTOList))
                    tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
                List<DropDownTO> list = BL.TblOrganizationBL.SelectAllOrganizationListForDropDown(orgTypeE, tblUserRoleTOList).OrderBy(o => o.Text).ToList();
                return list;
            }
        }
        [Route("GetStatesForDDL")]
        [HttpGet]
        public List<DropDownTO> GetStatesForDDL(Int32 countryId)
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectStatesForDropDown(countryId);
            return statusList;
        }


        [Route("GetDistrictForDDL")]
        [HttpGet]
        public List<DropDownTO> GetDistrictForDDL(Int32 stateId)
        {
            List<DropDownTO> statusList = BL.DimensionBL.SelectDistrictForDropDown(stateId);
            return statusList;
        }


        [Route("GetDealerDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetDealerDropDownList(Int32 cnfId, String userRoleTOList, Int32 consumerType = 0)
        {
            List<TblUserRoleTO> tblUserRoleTOList = new List<TblUserRoleTO>();
            if (!string.IsNullOrEmpty(userRoleTOList))
                tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            List<DropDownTO> list = BL.TblOrganizationBL.SelectDealerListForDropDown(cnfId, tblUserRoleTOList, consumerType).OrderBy(o => o.Text).ToList();
            return list;
        }

        [Route("GetDealerDropDownListAsPerLastTxn")]
        [HttpGet]
        public List<DropDownTO> GetDealerDropDownListAsPerLastTxn(Int32 cnfId, String userRoleTOList)
        {
            List<TblUserRoleTO> tblUserRoleTOList = new List<TblUserRoleTO>();
            if (!string.IsNullOrEmpty(userRoleTOList))
                tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            //List<DropDownTO> list = BL.TblOrganizationBL.GetDealerDropDownListAsPerLastTxn(cnfId, tblUserRoleTOList).OrderBy(o => o.Text).ToList();
            List<DropDownTO> list = BL.TblOrganizationBL.GetDealerDropDownListAsPerLastTxn(cnfId, tblUserRoleTOList);
            return list;
        }


        /// <summary>
        /// Sudhir[26-July-2018] --Add this Method for District & field officer link to be establish. 
        ///                        Regional manger can see his field office visit list. 
        ///                        Also field office can see their own visits
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        [Route("GetDealerDropDownListForCRM")]
        [HttpGet]
        public List<DropDownTO> GetDealerDropDownListForCRM(Int32 cnfId, String userRoleTO)
        {
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
            List<DropDownTO> list = BL.TblOrganizationBL.SelectDealerListForDropDownForCRM(cnfId, tblUserRoleTO).OrderBy(o => o.Text).ToList();
            return list;
        }

        [Route("GetSpecialCnfDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetSpecialCnfDropDownList(String userRoleTOList)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            List<DropDownTO> list = BL.TblOrganizationBL.SelectAllSpecialCnfListForDropDown(tblUserRoleTOList).OrderBy(o => o.Text).ToList();
            return list;
        }

        [Route("GetDealersSpecialCnfList")]
        [HttpGet]
        public List<TblCnfDealersTO> GetDealersSpecialCnfList(Int32 dealerId)
        {
            return  BL.TblCnfDealersBL.SelectAllActiveCnfDealersList(dealerId,true);
        }

        [Route("GetDealerForLoadingDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId, String userRoleTOList)
        {
            List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
            List<DropDownTO> list = BL.TblOrganizationBL.GetDealerForLoadingDropDownList(cnfId, tblUserRoleTOList).OrderBy(o => o.Text).ToList();
            return list;
        }

        [Route("IsThisValidCommercialLicenses")]
        [HttpGet]
        public ResultMessage IsThisValidCommercialLicenses(Int32 orgId, Int32 licenseId, String licenseVal)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<TblOrgLicenseDtlTO> list = BL.TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(orgId, licenseId, licenseVal);
            if (list != null && list.Count > 0)
            {
                TblOrganizationTO orgTO = BL.TblOrganizationBL.SelectTblOrganizationTO(list[0].OrganizationId);
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Not Allowed, This License is already attached to " + orgTO.OrgTypeE.ToString() + "-" + orgTO.FirmName;
                resultMessage.Result = 0;
            }
            else
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Valid";
                resultMessage.Result = 1;
            }

            return resultMessage;
        }

        [Route("GetAllOrgListToExport")]
        [HttpGet]
        public List<OrgExportRptTO> GetAllOrgListToExport(Int32 orgTypeId, Int32 parentId)
        {
            List<OrgExportRptTO> list = BL.TblOrganizationBL.SelectAllOrgListToExport(orgTypeId, parentId);
            return list;
        }
        /// <summary>
        /// Vijaymala[31-10-2017] Added to get invoice other details like desription wich display
        /// on footer to display terms an dconditions
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>

        [Route("GetInvoiceOtherDetails")]
        [HttpGet]
        public List<TblInvoiceOtherDetailsTO> GetInvoiceOtherDetails(Int32 organizationId)
        {
            List<TblInvoiceOtherDetailsTO> list = BL.TblInvoiceOtherDetailsBL.SelectInvoiceOtherDetails(organizationId);
            return list;
        }

        [Route("GetInvoiceBankDetails")]
        [HttpGet]
        public List<TblInvoiceBankDetailsTO> GetInvoiceBankDetails(Int32 organizationId)
        {
            List<TblInvoiceBankDetailsTO> list = BL.TblInvoiceBankDetailsBL.SelectInvoiceBankDetails(organizationId);
            return list;
        }

        /// <summary>
        /// Sudhir[20-March-2018] Added for Get Person List On OrganizationId also in tblOrgPersonDtls.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [Route("GetOrganizationPersonList")]
        [HttpGet]
        public List<TblPersonTO> GetOrganizationPersonList(Int32 organizationId, Int32 personTypeId = 0)
        {
            List<TblPersonTO> list = BL.TblPersonBL.SelectAllPersonListByOrganizationV2(organizationId, personTypeId);
            return list;
        }

        /// <summary>
        /// Sudhir[23-APR-2018] Added for Checking Organization Name OR Phone No. is Already Present Or Not.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [Route("CheckOrgNameOrPhoneNoAlready")]
        [HttpGet]
        public ResultMessage CheckOrgNameOrPhoneNoAlready(String OrgName, String PhoneNo,Int32 orgType)
        {
            return TblOrganizationBL.CheckOrgNameOrPhoneNoIsExist(OrgName, PhoneNo, orgType);
        }

        /// <summary>
        /// Sudhir[23-APR-2018] Added for Select All OtherDesignations .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetPersonsOnOrgType")]
        [HttpGet]
        [ProducesResponseType(typeof(List<DropDownTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetPersonsOnOrgType(Int32 OrgType)
        {
            try
            {
                List<DropDownTO> list = BL.TblPersonBL.SelectPersonBasedOnOrgType(OrgType);//BL.TblOtherDesignationsBL.SelectAllTblOtherDesignations();
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

        [Route("GetAllPersonsOffline")]
        [HttpGet]
        public List<TblPersonTO> GetPersonsForOffline()
        {
            try
            {
                return BL.TblPersonBL.selectPersonsForOffline();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("GetAllPersonsOfflineDropDown")]
        [HttpGet]
        public List<DropDownTO> GetPersonsForOfflineDropDown()
        {
            try
            {
                return BL.TblPersonBL.selectPersonsDropdownForOffline();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("GetSalesEngineerDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetSalesEngineerDropDownList(Int32 orgId)
        {
            List<DropDownTO> list = BL.TblOrganizationBL.SelectSalesEngineerListForDropDown(orgId);
            return list;
        }

        /// <summary>
        /// Priyanka [14-02-2019] :Added to add enquiry detail of organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostOrganizationOverDueDtl")]
        [HttpPost]
        public ResultMessage PostOrganizationOverDueDtl([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblOverdueDtlTO> tblOverdueDtlTOList = JsonConvert.DeserializeObject<List<TblOverdueDtlTO>>(data["overDueDtlTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (tblOverdueDtlTOList != null && tblOverdueDtlTOList.Count > 0)
                {
                    return BL.TblOverdueDtlBL.SaveOrgOverDueDtl(tblOverdueDtlTOList, Convert.ToInt32(loginUserId));

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblOverdueDtlTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }



            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostOrganizationOverDueDtl";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }


        /// <summary>
        /// Priyanka [14-02-2019] : Added to save enquiry detail of organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostOrganizationEnquiryDtl")]
        [HttpPost]
        public ResultMessage PostOrganizationEnquiryDtl([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                List<TblEnquiryDtlTO> tblEnquiryDtlTOList = JsonConvert.DeserializeObject<List<TblEnquiryDtlTO>>(data["enquiryDtlTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "loginUserId Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                if (tblEnquiryDtlTOList != null && tblEnquiryDtlTOList.Count > 0)
                {
                    return BL.TblEnquiryDtlBL.SaveOrgEnquiryDtl(tblEnquiryDtlTOList, Convert.ToInt32(loginUserId));
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblEnquiryDtlTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }



            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostOrganizationEnquiryDtl";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }
        [Route("PostNewOrganization")]
        [HttpPost]
        public ResultMessage PostNewOrganization([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblOrganizationTO organizationTO = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (organizationTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    organizationTO.CreatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.CreatedOn = serverDate;
                    organizationTO.IsActive = 1;
                    return BL.TblOrganizationBL.SaveNewOrganization(organizationTO);

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "organizationTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostNewOrganization";
                return resultMessage;
            }
        }

        /// <summary>
        /// Priyanka A. [16-04-2019] Added to save the organizations overdue ref Ids and enquiry ref Ids.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostOrganizationRefIds")]
        [HttpPost]
        public ResultMessage PostOrganizationRefIds([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblOrganizationTO organizationTO = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (organizationTO != null)
                {
                    return BL.TblOrganizationBL.SaveOrganizationRefIds(organizationTO, loginUserId);
                    //Check Already exist
                  

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "organizationTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostOrganizationRefIds";
                return resultMessage;
            }
        }

        [Route("PostUpdateOrganization")]
        [HttpPost]
        public ResultMessage PostUpdateOrganization([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblOrganizationTO organizationTO = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (organizationTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.UpdatedOn = serverDate;

                    return BL.TblOrganizationBL.UpdateOrganization(organizationTO);

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "organizationTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostNewOrganization";
                return resultMessage;
            }
        }

        [Route("PostRemoveCnfDealerRelationShip")]
        [HttpPost]
        public ResultMessage PostRemoveCnfDealerRelationShip([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblCnfDealersTO cnfDealersTO = JsonConvert.DeserializeObject<TblCnfDealersTO>(data["cnfDealersTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (cnfDealersTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    cnfDealersTO.IsActive = 0;

                    int result= BL.TblCnfDealersBL.UpdateTblCnfDealers(cnfDealersTO);
                    if(result!=1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error. Record Could Not Be Updated ";
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Record Updated Successfully";
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "cnfDealersTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostRemoveCnfDealerRelationShip";
                return resultMessage;
            }
        }

        [Route("PostDeactivateOrganization")]
        [HttpPost]
        public ResultMessage PostDeactivateOrganization([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                TblOrganizationTO organizationTO = JsonConvert.DeserializeObject<TblOrganizationTO>(data["organizationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (organizationTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.UpdatedOn = serverDate;
                    organizationTO.DeactivatedOn = serverDate;
                    organizationTO.IsActive = 0;

                    int result = BL.TblOrganizationBL.UpdateTblOrganization(organizationTO);
                    if (result != 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error. Record Could Not Be Updated ";
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Record Updated Successfully";
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "organizationTO Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostDeactivateOrganization";
                return resultMessage;
            }
        }

        /// <summary>
        /// This Method is For Add New Person Along with Organization.
        /// </summary>
        /// <remarks>
        /// Sample Data {'personTO':{'SalutationId':1, 'MobileNo':"123456789", 'AlternateMobNo':"", 'PhoneNo':"", 'CreatedBy':1, 'FirstName':"xyz", 'MidName':"", 'LastName':"xyz", 'PrimaryEmail':"", 'AlternateEmail':"", 'Comments':"" }, 'loginUserId':1 }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostSaveNewPersonForOrganization")]
        [HttpPost]
        public ResultMessage PostSaveNewPersonForOrganization([FromBody]TblPersonTO personTO, Int32 organizationId, Int32 personTypeId)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblPersonTO tblPersonTO = personTO; //JsonConvert.DeserializeObject<TblPersonTO>(data["personTO"].ToString());

                if (tblPersonTO == null)
                {
                    resultMessage.DefaultBehaviour("marketingDetialsTO found null");
                    return resultMessage;
                }

                var loginUserId = tblPersonTO.CreatedBy;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                //tblPersonTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblPersonTO.CreatedOn = Constants.ServerDateTime;

                return TblPersonBL.SaveNewPersonAgainstOrganization(tblPersonTO, organizationId, personTypeId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostSaveNewPersonForhOrganization");
                return resultMessage;
            }
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }       


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
