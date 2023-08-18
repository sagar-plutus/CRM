using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.BL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrganizationStructureController : Controller
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

        /// <summary>
        /// Vaibhav [25-Sep-2017] Get All organization structure hierarchy
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetAllOrgStructureList")]
        [HttpGet]
        public List<TblOrgStructureTO> GetAllOrgStructureList()
        {
            List<TblOrgStructureTO> orgStructureTOList = BL.TblOrgStructureBL.SelectAllOrgStructureList();
            return orgStructureTOList;
        }

        /// <summary>
        /// Vaibhav [28-Sep-2017] added to get all specific organization structure users.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetOrgStructureUserDetails")]
        [HttpGet]
        public List<TblUserReportingDetailsTO> GetOrgStructureUserDetails(Int16 orgStructureId)
        {
            return BL.TblOrgStructureBL.GetOrgStructureUserDetails(orgStructureId);
        }

        /// <summary>
        /// Hrushikesh added to get all reporting to employee list on Type
        /// </summary>
        /// <param name="value"></param>
        [Route("GetUserReportingToList")]
        [HttpGet]
        public List<DropDownTO> GetUserReportingToList(int orgStructureId, int type = 1)
        {
            return BL.TblOrgStructureBL.SelectReportingToUserList(orgStructureId, type);
        }


        /// <summary>
        /// Vaibhav [11-Oct-2017] New get organization structure list
        /// </summary>
        /// <param name="value"></param>
        [Route("GetOrgStuctureListForHierarchy")]
        [HttpGet]
        public List<TblOrgStructureTO> GetOrgStuctureListForHierarchy(int reportingTypeId)
        {
            List<TblOrgStructureTO> orgStuctureList = BL.TblOrgStructureBL.SelectOrgStuctureListForHierarchy(reportingTypeId);
            if (orgStuctureList != null)
                return orgStuctureList;
            else
                return null;
        }

        /// <summary>
        /// Sudhir[01JAN2018] Get list of OrgnizationStructure for Tree Table Structure.
        /// </summary>
        /// <param name="reportingTypeId"></param>
        /// <returns></returns>
        [Route("GetOrgStructureListForDisplay")]
        [HttpGet]
        public List<TblOrgStructureTO> GetOrgStructureListForDisplay(int reportingTypeId)
        {
            List<TblOrgStructureTO> orgStuctureList = BL.TblOrgStructureBL.GetOrgStructureListForDisplay(reportingTypeId);
            if (orgStuctureList != null)
                return orgStuctureList;
            else
                return null;
        }

        /// <summary>
        /// Sudhir[01JAN2018] Get list of OrgnizationStructure for Heirarchy Structure.
        /// </summary>
        /// <returns></returns>
        [Route("GetOrgStructureListForDisplayTree")]
        [HttpGet]
        public List<TblOrgStructureTO> GetOrgStructureListForDisplayTree()
        {
            List<TblOrgStructureTO> orgStuctureList = BL.TblOrgStructureBL.GetOrgStructureListForDisplayTree();
            if (orgStuctureList != null)
                return orgStuctureList;
            else
                return null;
        }

        /// <summary>
        /// Get All Level List.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllLevelList")]
        [HttpGet]
        public List<DimLevelsTO> GetAllLevelList()
        {
            List<DimLevelsTO> allLevelesList = BL.TblOrgStructureBL.GetAllLevelsToList();
            if (allLevelesList != null)
                return allLevelesList;
            else
                return null;
        }

        /// <summary>
        /// Get All ChildPosition List Based on orgStructure Id and Reporting Type.
        /// </summary>
        /// <param name="idOrgStructure"></param>
        /// <param name="reportingTypeId"></param>
        /// <returns></returns>
        [Route("GetAllchildPositionList")]
        [HttpGet]
        public List<TblOrgStructureTO> GetAllchildPositionList(int idOrgStructure, int reportingTypeId)
        {
            List<TblOrgStructureTO> childPositionList = TblOrgStructureBL.GetAllchildPositionList(idOrgStructure, reportingTypeId);
            if (childPositionList != null)
                return childPositionList;
            else
                return null;
        }

        /// <summary>
        /// Get OrgStructureList Whose Position Not Linked OR (Not Attached).
        /// </summary>
        /// <returns></returns>
        [Route("GetNotLinkedPositionsList")]
        [HttpGet]
        public List<TblOrgStructureTO> GetNotLinkedPositionsList()
        {
            List<TblOrgStructureTO> childPositionList = TblOrgStructureBL.GetNotLinkedPositionsList();
            if (childPositionList != null)
                return childPositionList;
            else
                return null;
        }

        /// <summary>
        /// Get All Organization Hierarchy List
        /// </summary>
        /// <returns></returns>
        [Route("GetAllOrganizationHierarchyList")]
        [HttpGet]
        public List<TblOrgStructureHierarchyTO> GetAllOrganizationHierarchyList()
        {
            List<TblOrgStructureHierarchyTO> hierarchyList = TblOrgStructureBL.SelectAllTblOrgStructureHierarchy();
            if (hierarchyList != null)
                return hierarchyList;
            else
                return null;
        }

        /// <summary>
        /// //Sudhir[08-MAR-2018] Added for Get Users for Organizations Structre Based on User ID's
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>DropDownTO</returns>
        [Route("GetChildUserListOnUserId")]
        [HttpGet]
        public object GetChildUserListOnUserId(Int32 userId, int? isObjectType, int reportingType = 1)
        {
            if (isObjectType == null)
                isObjectType = 0;
            return BL.TblOrgStructureBL.ChildUserListOnUserId(userId, isObjectType, reportingType);
        }


        /// <summary>
        /// //Sudhir[04-July-2018] Added for Get Users for Organizations Structre Based on User ID's
        /// </summary>
        /// <param name="idOrgStructure"></param>
        /// <returns>DropDownTO</returns>
        [Route("GetPositionLinkDetails")]
        [HttpGet]
        public List<TblOrgStructureTO> GetPositionLinkDetails(int idOrgStructure)
        {
            return TblOrgStructureBL.SelectPositionLinkDetails(idOrgStructure);
        }

        /// <summary>
        /// //Sudhir[22-AUG-2018] Added for Parent User On UserId
        /// </summary>
        /// <param name="idOrgStructure"></param>
        /// <returns>DropDownTO</returns>
        [Route("GetParentUserOnUserId")]
        [HttpGet]
        public DropDownTO GetParentUserOnUserId(int idUser, int repType = 1)
        {
            return TblOrgStructureBL.SelectParentUserOnUserId(idUser, repType);
        }


        /// <summary>
        /// //Sudhir[17-OCT-2018] Get UpperLevel List from UserReportingDetails
        /// </summary>
        /// <param name="idOrgStructure"></param>
        /// <returns>DropDownTO</returns>
        [Route("GetDeactivationReportingToList")]
        [HttpPost]
        public List<TblUserReportingDetailsTO> GetDeactivationReportingToList([FromBody] JObject data)
        {
            List<TblUserReportingDetailsTO> list = new List<TblUserReportingDetailsTO>();
            try
            {

                TblUserReportingDetailsTO userReportingDtlTO = JsonConvert.DeserializeObject<TblUserReportingDetailsTO>(data["userReportingDetailsTO"].ToString());

                if (userReportingDtlTO != null)
                {
                    list = TblOrgStructureBL.SelectDeactivateUserReportingList(userReportingDtlTO);
                }
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// //Sudhir[17-OCT-2018] Get UpperLevel List from UserReportingDetails
        /// </summary>
        /// <param name="idOrgStructure"></param>
        /// <returns>DropDownTO</returns>
        [Route("GetDeactivationEmployeeList")]
        [HttpPost]
        public List<TblUserReportingDetailsTO> GetDeactivationEmployeeList([FromBody] JObject data)
        {
            List<TblUserReportingDetailsTO> list = new List<TblUserReportingDetailsTO>();
            try
            {

                TblUserReportingDetailsTO userReportingDtlTO = JsonConvert.DeserializeObject<TblUserReportingDetailsTO>(data["userReportingDetailsTO"].ToString());

                if (userReportingDtlTO != null)
                {
                    list = TblOrgStructureBL.SelectDeactivateChildEmployeeList(userReportingDtlTO);
                }
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// Sudhir[22-OCT-2018] Added for Get Child Positon List Based On OrgStructure Id.
        /// </summary>
        /// <param name="idOrgStructure"></param>
        /// <returns></returns>
        [Route("GetChildPositions")]
        [HttpGet]
        public List<TblOrgStructureTO> GetChildPositions(int idOrgStructure)
        {
            return TblOrgStructureBL.SelectChildPositions(idOrgStructure);
        }


        /// <summary>
        /// Sudhir[22-OCT-2018] Added for Update Organization Structure Hierarchy
        /// </summary>
        /// <param name="selfOrgStructureId"></param>
        [Route("postDelinkPosition")]
        [HttpGet]
        public ResultMessage postDelinkPosition(int selfOrgStructureId, int parentOrgStrtucureId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (selfOrgStructureId > 0 && parentOrgStrtucureId > 0)
                {
                    resultMessage = TblOrgStructureBL.PostDelinkPosition(selfOrgStructureId, parentOrgStrtucureId);
                    //resultMessage.DefaultBehaviour("Error While Position Delinking");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour("Error While Position Delinking");
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #endregion

        #region POST

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Vaibhav [26-Sep-2017] Added to save organization hierarchy structure
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostOrgStructure")]
        [HttpPost]
        public ResultMessage PostOrgStructure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblOrgStructureTO orgStructureTO = JsonConvert.DeserializeObject<TblOrgStructureTO>(data["orgStructureTO"].ToString());

                if (orgStructureTO == null)
                {
                    resultMessage.DefaultBehaviour("orgStructureTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                orgStructureTO.CreatedBy = Convert.ToInt32(loginUserId);
                orgStructureTO.CreatedOn = Constants.ServerDateTime;
                orgStructureTO.IsActive = 1;

                // To Handle OrgStructure description undefine.
                if (orgStructureTO.OrgStructureDesc.Contains("undefined"))
                {
                    int strIndex = orgStructureTO.OrgStructureDesc.IndexOf("undefined");
                    orgStructureTO.OrgStructureDesc = orgStructureTO.OrgStructureDesc.Remove(strIndex - 1);
                }
                if (orgStructureTO.OrgStructureDesc.Contains("--"))
                {
                    int strIndex = orgStructureTO.OrgStructureDesc.IndexOf("--");
                    orgStructureTO.OrgStructureDesc = orgStructureTO.OrgStructureDesc.Remove(strIndex);
                }

                if (orgStructureTO.OrgStructureDesc.Last() == '-')
                {
                    int strIndex = orgStructureTO.OrgStructureDesc.LastIndexOf("-");
                    orgStructureTO.OrgStructureDesc = orgStructureTO.OrgStructureDesc.Remove(strIndex);
                }

                return BL.TblOrgStructureBL.SaveOrganizationStructureHierarchy(orgStructureTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostOrgStructure");
                return resultMessage;
            }
        }


        /// <summary>
        /// Vaibhav [27-Sep-2017] added to update organization structure
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostUpdateOrgStructure")]
        [HttpPost]
        public ResultMessage PostUpdateOrgStructure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblOrgStructureTO orgStructureTO = JsonConvert.DeserializeObject<TblOrgStructureTO>(data["orgStructureTO"].ToString());

                if (orgStructureTO == null)
                {
                    resultMessage.DefaultBehaviour("orgStructureTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                orgStructureTO.UpdatedBy = Convert.ToInt32(loginUserId);
                orgStructureTO.UpdatedOn = Constants.ServerDateTime;
                orgStructureTO.IsActive = 1;

                // To Handle OrgStructure description undefine.
                if (orgStructureTO.OrgStructureDesc.Contains("undefined"))
                {
                    int strIndex = orgStructureTO.OrgStructureDesc.IndexOf("undefined");
                    orgStructureTO.OrgStructureDesc = orgStructureTO.OrgStructureDesc.Remove(strIndex - 1);
                }
                if (orgStructureTO.OrgStructureDesc.Contains("--"))
                {
                    int strIndex = orgStructureTO.OrgStructureDesc.IndexOf("--");
                    orgStructureTO.OrgStructureDesc = orgStructureTO.OrgStructureDesc.Remove(strIndex);
                }

                if (orgStructureTO.OrgStructureDesc.Last() == '-')
                {
                    int strIndex = orgStructureTO.OrgStructureDesc.LastIndexOf("-");
                    orgStructureTO.OrgStructureDesc = orgStructureTO.OrgStructureDesc.Remove(strIndex);
                }

                return BL.TblOrgStructureBL.UpdateTblOrgStructure(orgStructureTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateOrgStructure");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [27-Sep-2017] added to deactivate specific organization structure hierarchy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostDeactivateOrgStructure")]
        [HttpPost]
        public ResultMessage PostDeactivateOrgStructure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblOrgStructureTO orgStructureTO = JsonConvert.DeserializeObject<TblOrgStructureTO>(data["orgStructureTO"].ToString());

                if (orgStructureTO == null)
                {
                    resultMessage.DefaultBehaviour("orgStructureTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                var reportingId = data["reportingTypeId"].ToString();
                Int32 ReportingTypeId = Convert.ToInt32(reportingId);
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                orgStructureTO.IsActive = 0;
                orgStructureTO.UpdatedBy = Convert.ToInt32(loginUserId);
                orgStructureTO.UpdatedOn = Constants.ServerDateTime;
                return BL.TblOrgStructureBL.DeactivateOrgStructure(orgStructureTO, ReportingTypeId);
                //return new ResultMessage();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateOrgStructure");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [29-Sep-2017] added to update user reporting details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostUpdateUserReportingDetails")]
        [HttpPost]
        public ResultMessage PostDeactivateUserForOrgStructure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblUserReportingDetailsTO userReportingDtlTO = JsonConvert.DeserializeObject<TblUserReportingDetailsTO>(data["userReportingDetailsTO"].ToString());

                if (userReportingDtlTO == null)
                {
                    resultMessage.DefaultBehaviour("userReportingDtlTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                // to deactivate user
                if (userReportingDtlTO.IsActive == 0)
                {
                    userReportingDtlTO.IsActive = 0;
                    userReportingDtlTO.DeActivatedBy = Convert.ToInt32(loginUserId);
                    userReportingDtlTO.DeActivatedOn = Constants.ServerDateTime;
                }
                else
                {
                    userReportingDtlTO.IsActive = 1;
                    userReportingDtlTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    userReportingDtlTO.UpdatedOn = Constants.ServerDateTime;
                }

                return BL.TblOrgStructureBL.UpdateUserReportingDetails(userReportingDtlTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateUserReportingDetails");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [26-Sep-2017] aded to attach employee for specifc organization hierachy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostAttachNewUserToOrgStructure")]
        [HttpPost]
        public ResultMessage PostAttachNewUserToOrgStructure([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblUserReportingDetailsTO userReportingDetailsTO = JsonConvert.DeserializeObject<TblUserReportingDetailsTO>(data["userReportingDetailsTO"].ToString());
                //  List<TblUserReportingDetailsTO> deactivateCurrentPosList = JsonConvert.DeserializeObject<List<TblUserReportingDetailsTO>>(data["UserReportingDetailsTOList"].ToString());
                List<TblUserRoleTO> deactivateCurrentRolesList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(data["UserReportingDetailsTOList"].ToString());

                if (userReportingDetailsTO == null)
                {
                    resultMessage.DefaultBehaviour("userReportingDetailsTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                userReportingDetailsTO.CreatedBy = Convert.ToInt32(loginUserId);
                userReportingDetailsTO.CreatedOn = Constants.ServerDateTime;
                userReportingDetailsTO.IsActive = 1;


                return BL.TblOrgStructureBL.AttachNewUserToOrgStructure(userReportingDetailsTO, deactivateCurrentRolesList);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostAttachNewUserToOrgStructure");
                return resultMessage;
            }
        }

        /// <summary>
        /// This Method Deactivate USer Reporting.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostDeactivateUserReporting")]
        [HttpPost]
        public ResultMessage PostDeactivateUserReporting([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblUserReportingDetailsTO userReportingDetailsTO = JsonConvert.DeserializeObject<TblUserReportingDetailsTO>(data["userReportingDetailsTO"].ToString());

                if (userReportingDetailsTO == null)
                {
                    resultMessage.DefaultBehaviour("userReportingDetailsTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                userReportingDetailsTO.DeActivatedBy = Convert.ToInt32(loginUserId);
                userReportingDetailsTO.DeActivatedOn = Constants.ServerDateTime;
                userReportingDetailsTO.IsActive = 0;


                return BL.TblOrgStructureBL.DeactivateUserReporting(userReportingDetailsTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostAttachNewUserToOrgStructure");
                return resultMessage;
            }
        }

        /// <summary>
        /// This Method is for Linking Organization Structure.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostChildPositionOrgStructure")]
        [HttpPost]
        public ResultMessage PostChildPositionOrgStructure([FromBody] JObject data)
        {

            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblOrgStructureTO orgStructureTO = JsonConvert.DeserializeObject<TblOrgStructureTO>(data["orgStructureTO"].ToString());

                if (orgStructureTO == null)
                {
                    resultMessage.DefaultBehaviour("orgStructureTO found null");
                    return resultMessage;
                }
                var loginUserId = data["loginUserId"].ToString();
                var reportingTypeId = Convert.ToInt32(data["reportingTypeId"].ToString());
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }

                orgStructureTO.UpdatedBy = Convert.ToInt32(loginUserId);
                orgStructureTO.UpdatedOn = Constants.ServerDateTime;
                orgStructureTO.IsActive = 1;

                return BL.TblOrgStructureBL.UpdateChildOrgStructure(orgStructureTO, reportingTypeId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateOrgStructure");
                return resultMessage;
            }
        }



        /// <summary>
        /// This Method is for Linking Change Child User Reporting Details.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostChangeReportingToDetails")]
        [HttpPost]
        public ResultMessage PostChangeReportingToDetails([FromBody] JObject data)
        {

            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblUserReportingDetailsTO> userReportingList = JsonConvert.DeserializeObject<List<TblUserReportingDetailsTO>>(data["userReportingList"].ToString());


                var IduserReporting = data["IduserReporting"].ToString();
                Int32 userReportingId = 0;
                if (Convert.ToInt32(IduserReporting) <= 0)
                {
                    //userReportingId = Convert.ToInt32(IduserReporting);
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                else
                {
                    userReportingId = Convert.ToInt32(IduserReporting);
                }
                if ((userReportingList != null && userReportingList.Count > 0) || userReportingId > 0)
                {
                    //Proceed 
                    resultMessage = TblOrgStructureBL.UpdateUserReportingDetails(userReportingList, userReportingId);
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Error While Changing Reporting To");
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateOrgStructure");
                return resultMessage;
            }
        }

        /// <summary>
        /// Hrushikesh added to get all specific organization structure users for BOM.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetOrgStructureUserDetailsForBom")]
        [HttpGet]
        public List<TblUserReportingDetailsTO> GetOrgStructureUserDetailsBom(Int16 orgStructureId)
        {
            return BL.TblOrgStructureBL.GetOrgStructureUserDetailsForBom(orgStructureId);
        }


        /// <summary>
        /// This Method is for Linking Change Child User Reporting Details.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostChangeReportingToDetailsBom")]
        [HttpPost]
        public ResultMessage PostChangeReportingToDetailsBOM([FromBody] JObject data)
        {

            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblUserReportingDetailsTO> userReportingList = JsonConvert.DeserializeObject<List<TblUserReportingDetailsTO>>(data["userReportingList"].ToString());


                var IduserReporting = data["IduserReporting"].ToString();
                Int32 userReportingId = 0;
                if (Convert.ToInt32(IduserReporting) <= 0)
                {
                    //userReportingId = Convert.ToInt32(IduserReporting);
                    resultMessage.DefaultBehaviour("loginUserId found null");
                    return resultMessage;
                }
                else
                {
                    userReportingId = Convert.ToInt32(IduserReporting);
                }
                if ((userReportingList != null && userReportingList.Count > 0) || userReportingId > 0)
                {
                    //Proceed   
                    resultMessage = TblOrgStructureBL.UpdateUserReportingDetailsBom(userReportingList, userReportingId);
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Error While Changing Reporting To");
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateOrgStructure");
                return resultMessage;
            }
        }


        // code added by aniket
        // to update RoleTypeId in tblRole
        [Route("PostRoleTypeChange")]
        [HttpPost]
        public ResultMessage PostUpdateRoleType([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblOrgStructureTO orgStructureTO = JsonConvert.DeserializeObject<TblOrgStructureTO>(data["orgstructureTO"].ToString());
            if (orgStructureTO == null)
            {
                resultMessage.DefaultBehaviour("orgStructureTO found null");
                //return resultMessage;
            }
            var loginUserId = data["loginUserId"].ToString();
            if (Convert.ToInt32(loginUserId) <= 0)
            {
                resultMessage.DefaultBehaviour("loginUserId found null");
                //return resultMessage;
            }
            return BL.TblRoleBL.UpdateRoleType(orgStructureTO);
        }
        //end added by aniket
        #endregion

        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion 

        #region DELETE

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion
    }
}
