using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PurchaseTrackerAPI.DAL.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrganizationController : Controller
    {
        private readonly ITblOtherSourceBL _iTblOtherSourceBL;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        private readonly ILogger loggerObj;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblCompetitorExtBL _iTblCompetitorExtBL;
        private readonly ITblAddressBL _iTblAddressBL;
        private readonly ITblEnquiryDtlBL _iTblEnquiryDtlBL;
        private readonly ITblCnfDealersBL _iTblCnfDealersBL;
        private readonly Icommondao _iCommonDAO;

        public OrganizationController(
            Icommondao icommondao,
            ILogger<OrganizationController> logger, 
            ITblConfigParamsBL iTblConfigParamsBL,
           ITblPersonBL iTblPersonBL,  
           ITblOrganizationBL iTblOrganizationBL,
           ITblCompetitorExtBL iTblCompetitorExtBL,
             ITblAddressBL iTblAddressBL,
            ITblOtherSourceBL iTblOtherSourceBL
            , ITblCnfDealersBL iTblCnfDealersBL,
            ITblEnquiryDtlBL iTblEnquiryDtlBL
            )
        {
            _iCommonDAO = icommondao;
            _iTblEnquiryDtlBL = iTblEnquiryDtlBL;
            _iTblCnfDealersBL = iTblCnfDealersBL;
            _iTblOtherSourceBL = iTblOtherSourceBL;
            _iTblAddressBL = iTblAddressBL;
            _iTblCompetitorExtBL = iTblCompetitorExtBL;
            _iTblPersonBL = iTblPersonBL;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            loggerObj = logger;
            Constants.LoggerObj = logger;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetOrganizationList")]
        [HttpGet]
        public List<TblOrganizationTO> GetOrganizationList(Int32 orgTypeId)
        {
            Constants.OrgTypeE orgTypeE = (Constants.OrgTypeE)Enum.Parse(typeof(Constants.OrgTypeE), orgTypeId.ToString());
            List<TblOrganizationTO> list = _iTblOrganizationBL.SelectAllTblOrganizationList(orgTypeE).OrderBy(o => o.FirmName).ToList(); 
            return list;
        }

        [Route("GetOrganizationInfo")]
        [HttpGet]
        public TblOrganizationTO GetOrganizationInfo(Int32 orgId)
        {
            return _iTblOrganizationBL.SelectTblOrganizationTO(orgId);
        }

        [Route("GetDealerOrganizationList")]
        [HttpGet]
        public List<TblOrganizationTO> GetDealerOrganizationList(Int32 cnfId)
        {
            int orgTypeId = (int)Constants.OrgTypeE.DEALER;
            List<TblOrganizationTO> list = _iTblOrganizationBL.SelectAllChildOrganizationList(orgTypeId, cnfId);
            return list;
        }

        [Route("GetOrgOwnerDetails")]
        [HttpGet]
        public List<TblPersonTO> GetOrgOwnerDetails(Int32 organizationId)
        {
            List<TblPersonTO> list = _iTblPersonBL.SelectAllPersonListByOrganization(organizationId);
            return list;
        }

        //[Route("CheckIsSupplierIsBlocked")]
        //[HttpGet]
        //public ResultMessage CheckIsSupplierIsBlocked(Int32 supplierId)
        //{
        //    return _iTblOrganizationBL.CheckIsSupplierIsBlocked(supplierId); 
        //}

        [Route("GetOrgAddressDetails")]
        [HttpGet]
        public List<TblAddressTO> GetOrgAddressDetails(Int32 organizationId)
        {
            return _iTblAddressBL.SelectOrgAddressList(organizationId);
        }

        // [Route("GetOrgCommercialLicenseDetails")]
        // [HttpGet]
        // public List<TblOrgLicenseDtlTO> GetOrgCommercialLicenseDetails(Int32 organizationId)
        // {
        //     return BL.TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(organizationId);
        // }

        [Route("GetCompetitorBrandList")]
        [HttpGet]
        public List<TblCompetitorExtTO> GetCompetitorBrandList(Int32 organizationId)
        {
            return _iTblCompetitorExtBL.SelectAllTblCompetitorExtList(organizationId);
        }

        [Route("GetCompetitorListWithHistory")]
        [HttpGet]
        public List<TblOrganizationTO> GetCompetitorListWithHistory()
        {
            Constants.OrgTypeE orgTypeE = Constants.OrgTypeE.COMPETITOR;
            List<TblOrganizationTO> list = _iTblOrganizationBL.SelectAllTblOrganizationList(orgTypeE);

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_COMPETITOR_TO_SHOW_IN_HISTORY);
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
                    string[] idsToShow = csParamVal.Split(',');

                    for (int i = 0; i < idsToShow.Length; i++)
                    {
                        var orgTO = list.Where(a => a.IdOrganization == Convert.ToInt32(idsToShow[i])).LastOrDefault();
                        if (orgTO != null)
                            finalList.Add(orgTO);
                    }

                    finalList = finalList.OrderByDescending(o => o.CompetitorUpdatesTO.UpdateDatetime).ToList();
                    return finalList;
                }
            }
        }

        [Route("GetOrganizationDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetOrganizationDropDownList(Int32 orgTypeId,String userRoleTO)
        {
            Constants.OrgTypeE orgTypeE = (Constants.OrgTypeE)Enum.Parse(typeof(Constants.OrgTypeE), orgTypeId.ToString());
            if (orgTypeE == Constants.OrgTypeE.OTHER)
            {
                List<DropDownTO> list = _iTblOtherSourceBL.SelectOtherSourceOfMarketTrendForDropDown();
                return list;
            }
            else
            {
                TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
                List<DropDownTO> list = _iTblOrganizationBL.SelectAllOrganizationListForDropDown(orgTypeE, tblUserRoleTO).OrderBy(o => o.Text).ToList();
                return list;
            }
        }

        [Route("CheckIsSupplierIsBlocked")]
        [HttpGet]
        public ResultMessage CheckIsSupplierIsBlocked(Int32 supplierId)
        {
          return _iTblOrganizationBL.CheckIsSupplierIsBlocked(supplierId);
        }

        [Route("GetDealerDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetDealerDropDownList(Int32 cnfId, String userRoleTO)
        {
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
            List<DropDownTO> list = _iTblOrganizationBL.SelectDealerListForDropDown(cnfId, tblUserRoleTO).OrderBy(o => o.Text).ToList(); 
            return list;
        }

        [Route("GetSpecialCnfDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetSpecialCnfDropDownList(String userRoleTO)
        {
            TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
            List<DropDownTO> list = _iTblOrganizationBL.SelectAllSpecialCnfListForDropDown(tblUserRoleTO).OrderBy(o => o.Text).ToList();
            return list;
        }

        [Route("GetDealersSpecialCnfList")]
        [HttpGet]
        public List<TblCnfDealersTO> GetDealersSpecialCnfList(Int32 dealerId)
        {
            return  _iTblCnfDealersBL.SelectAllActiveCnfDealersList(dealerId,true);
        }

        [Route("GetDealerForLoadingDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId)
        {
            List<DropDownTO> list = _iTblOrganizationBL.GetDealerForLoadingDropDownList(cnfId).OrderBy(o => o.Text).ToList();
            return list;
        }

        // [Route("IsThisValidCommercialLicenses")]
        // [HttpGet]
        // public ResultMessage IsThisValidCommercialLicenses(Int32 orgId, Int32 licenseId, String licenseVal)
        // {
        //     ResultMessage resultMessage = new ResultMessage();
        //     List<TblOrgLicenseDtlTO> list = BL.TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(orgId, licenseId, licenseVal);
        //     if (list != null && list.Count > 0)
        //     {
        //         TblOrganizationTO orgTO = BL.TblOrganizationBL.SelectTblOrganizationTO(list[0].OrganizationId);
        //         resultMessage.MessageType = ResultMessageE.Error;
        //         resultMessage.Text = "Not Allowed, This License is already attached to " + orgTO.OrgTypeE.ToString() + "-" + orgTO.FirmName;
        //         resultMessage.Result = 0;
        //     }
        //     else
        //     {
        //         resultMessage.MessageType = ResultMessageE.Information;
        //         resultMessage.Text = "Valid";
        //         resultMessage.Result = 1;
        //     }

        //     return resultMessage;
        // }

        [Route("GetAllOrgListToExport")]
        [HttpGet]
        public List<OrgExportRptTO> GetAllOrgListToExport(Int32 orgTypeId, Int32 parentId)
        {
            List<OrgExportRptTO> list = _iTblOrganizationBL.SelectAllOrgListToExport(orgTypeId, parentId);
            return list;
        }
        /// <summary>
        /// Vijaymala[31-10-2017] Added to get invoice other details like desription wich display
        /// on footer to display terms an dconditions
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>

        // [Route("GetInvoiceOtherDetails")]
        // [HttpGet]
        // public List<TblInvoiceOtherDetailsTO> GetInvoiceOtherDetails(Int32 organizationId)
        // {
        //     List<TblInvoiceOtherDetailsTO> list = BL.TblInvoiceOtherDetailsBL.SelectInvoiceOtherDetails(organizationId);
        //     return list;
        // }

        // [Route("GetInvoiceBankDetails")]
        // [HttpGet]
        // public List<TblInvoiceBankDetailsTO> GetInvoiceBankDetails(Int32 organizationId)
        // {
        //     List<TblInvoiceBankDetailsTO> list = BL.TblInvoiceBankDetailsBL.SelectInvoiceBankDetails(organizationId);
        //     return list;
        // }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization list of particular region;
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>       
        [Route("GetOrganizationListByRegion")]
        [HttpGet]
        public List<TblOrganizationTO> GetOrganizationListByRegion(Int32 orgTypeId, Int32 districtId)
        {
            List<TblOrganizationTO> list = _iTblOrganizationBL.SelectOrganizationListByRegion(orgTypeId, districtId);
            return list;
        }


        /// <summary>
        /// [2017-11-29]Vijaymala:Added to get enquiry detail of particular organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetOrgEnquiryList")]
        [HttpGet]
        public List<TblEnquiryDtlTO> GetOrgEnquiryList(Int32 organizationId)
        {
            return _iTblEnquiryDtlBL.SelectEnquiryDtlList(organizationId);
        }

        /// <summary>
        /// [2017-11-29]Vijaymala:Added to get overdue detail of particular organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // [Route("GetOrgOverdueList")]
        // [HttpGet]
        // public List<TblOverdueDtlTO> GetOrgOverdueList(Int32 organizationId)
        // {
        //     return BL.TblOverdueDtlBL.SelectTblOverdueDtlList(organizationId);
        // }

        /// <summary>
        /// [2017-12-01]Vijaymala:Added to get enquiry detail List of organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllOrgEnquiryList")]
        [HttpGet]
        public List<TblEnquiryDtlTO> GetAllOrgEnquiryList(string organizationIds)
        {
            return _iTblEnquiryDtlBL.SelectAllTblEnquiryDtl(organizationIds);
        }

        // /// <summary>
        // /// [2017-12-01]Vijaymala:Added to get overdue detail List of particular organization
        // /// </summary>
        // /// <param name="data"></param>
        // /// <returns></returns>
        // [Route("GetAllOrgOverdueList")]
        // [HttpGet]
        // public List<TblOverdueDtlTO> GetAllOrgOverdueList(string organizationIds)
        // {
        //     return BL.TblOverdueDtlBL.SelectAllTblOverdueDtlList(organizationIds);
        // }


        /// <summary>
        /// [2017-11-29]Vijaymala:Added to get enquiry detail of particular organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetEnquiryListFromExcel")]
        [HttpGet]
        public List<TblEnquiryDtlTO> GetEnquiryListFromExcel(Int32 organizationId)
        {
            return _iTblEnquiryDtlBL.SelectEnquiryDtlList(organizationId);
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
                    DateTime serverDate =  _iCommonDAO.ServerDateTime;
                    organizationTO.CreatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.CreatedOn = serverDate;
                    organizationTO.IsActive = 1;
                    return _iTblOrganizationBL.SaveNewOrganization(organizationTO);

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

                    //Check Already exist
                    if (!String.IsNullOrEmpty(organizationTO.OverdueRefId))
                    {
                        List<TblOrganizationTO> tblOrganizationTOList = _iTblOrganizationBL.SelectExistingAllTblOrganizationByRefIds(organizationTO.IdOrganization, organizationTO.OverdueRefId, null);
                        if (tblOrganizationTOList != null)
                        {
                            Int32 overdueisExist = tblOrganizationTOList.Count;
                            if (overdueisExist > 0)
                            {
                                String orgName = String.Join(",", tblOrganizationTOList.Select(s => s.FirmName.ToString()).ToList());

                                resultMessage.MessageType = ResultMessageE.Information;
                                resultMessage.Result = 2;
                                resultMessage.Text = "Overdue Reference Id is already assign to " + orgName;
                                return resultMessage;
                            }
                        }
                    }
                    if (!String.IsNullOrEmpty(organizationTO.EnqRefId))
                    {
                        List<TblOrganizationTO> tblOrganizationTOList = _iTblOrganizationBL.SelectExistingAllTblOrganizationByRefIds(organizationTO.IdOrganization, null, organizationTO.EnqRefId);
                        if (tblOrganizationTOList != null)
                        {
                            Int32 enqyisExist = tblOrganizationTOList.Count;
                            if (enqyisExist > 0)
                            {
                                String orgName = String.Join(",", tblOrganizationTOList.Select(s => s.FirmName.ToString()).ToList());

                                resultMessage.MessageType = ResultMessageE.Information;
                                resultMessage.Result = 2;
                                resultMessage.Text = "Enquiry Reference Id is already assign to " + orgName;
                                return resultMessage;
                            }
                        }
                    }
                    DateTime serverDate =  _iCommonDAO.ServerDateTime;
                    organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.UpdatedOn = serverDate;
                    //organizationTO.IsActive = 1;
                    int result = _iTblOrganizationBL.UpdateTblOrganizationRefIds(organizationTO);
                    if (result != 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error while updating Ref Ids";
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Update Successfully";
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
                resultMessage.Text = "Exception Error IN API Call PostNewOrganization";
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
                    DateTime serverDate =  _iCommonDAO.ServerDateTime;
                    organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.UpdatedOn = serverDate;

                    return _iTblOrganizationBL.UpdateOrganization(organizationTO);

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
                    DateTime serverDate =  _iCommonDAO.ServerDateTime;
                    cnfDealersTO.IsActive = 0;

                    int result= _iTblCnfDealersBL.UpdateTblCnfDealers(cnfDealersTO);
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
                    DateTime serverDate =  _iCommonDAO.ServerDateTime;
                    organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    organizationTO.UpdatedOn = serverDate;
                    organizationTO.DeactivatedOn = serverDate;
                    organizationTO.IsActive = 0;

                    int result = _iTblOrganizationBL.UpdateTblOrganization(organizationTO);
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
        /// [08/12/2017] Vijaymala :Added to save enquiry detail of organization
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
                    return _iTblEnquiryDtlBL.SaveOrgEnquiryDtl(tblEnquiryDtlTOList, Convert.ToInt32(loginUserId));
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

        /// <summary>
        /// [11/12/2017] Vijaymala :Added to add enquiry detail of organization
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        // [Route("PostOrganizationOverDueDtl")]
        // [HttpPost]
        // public ResultMessage PostOrganizationOverDueDtl([FromBody] JObject data)
        // {
        //     ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //     try
        //     {

        //         List<TblOverdueDtlTO> tblOverdueDtlTOList = JsonConvert.DeserializeObject<List<TblOverdueDtlTO>>(data["overDueDtlTOList"].ToString());
        //         var loginUserId = data["loginUserId"].ToString();

        //         if (Convert.ToInt32(loginUserId) <= 0)
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "loginUserId Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }
        //         if (tblOverdueDtlTOList != null && tblOverdueDtlTOList.Count > 0)
        //         {
        //             return BL.TblOverdueDtlBL.SaveOrgOverDueDtl(tblOverdueDtlTOList, Convert.ToInt32(loginUserId));

        //         }
        //         else
        //         {
        //             resultMessage.MessageType = ResultMessageE.Error;
        //             resultMessage.Text = "tblOverdueDtlTOList Found NULL";
        //             resultMessage.Result = 0;
        //             return resultMessage;
        //         }



        //     }
        //     catch (Exception ex)
        //     {
        //         resultMessage.MessageType = ResultMessageE.Error;
        //         resultMessage.Text = "API : Exception In Method PostOrganizationOverDueDtl";
        //         resultMessage.Result = -1;
        //         resultMessage.Exception = ex;
        //         return resultMessage;
        //     }
        // }


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
