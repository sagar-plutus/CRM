using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblOrganizationBL
    {
        #region Selection

        public static List<TblOrganizationTO> SelectAllTblOrganizationList()
        {
            return TblOrganizationDAO.SelectAllTblOrganization();
        }
        //PRiyanka [08-04-2019]
        public static List<TblOrganizationTO> SelectExistingAllTblOrganizationByRefIds(Int32 orgId, String overdueRefId, String enqRefId)
        {
            return TblOrganizationDAO.SelectExistingAllTblOrganizationByRefIds(orgId, overdueRefId, enqRefId);
        }
        //Priyanka [08-04-19]
        public static int UpdateTblOrganizationRefIds(TblOrganizationTO tblOrganizationTO)
        {
            return TblOrganizationDAO.UpdateTblOrganizationRefIds(tblOrganizationTO);
        }

        public static String GetFirmNameByOrgId(Int32 orgId)
        {
            return TblOrganizationDAO.GetFirmNameByOrgId(orgId);
        }
        public static TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                TblOrganizationTO tblOrganizationTO = TblOrganizationDAO.SelectTblOrganization(idOrganization, conn, tran);
                if (tblOrganizationTO != null)
                {
                    List<TblOrgLicenseDtlTO> orgLicenseDtlTOList = TblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(tblOrganizationTO.IdOrganization, conn, tran);
                    tblOrganizationTO.OrgLicenseDtlTOList = orgLicenseDtlTOList;
                }
                return tblOrganizationTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblOrganizationTO");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrganizationDAO.SelectTblOrganization(idOrganization, conn, tran);

        }
        public static List<TblOrganizationTO> SelectAllChildOrganizationList(int orgTypeId, int parentId)
        {
            return TblOrganizationDAO.SelectAllTblOrganization(orgTypeId, parentId);
        }

        public static List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE)
        {
            return TblOrganizationDAO.SelectAllTblOrganization(orgTypeE);
        }

        public static List<TblOrganizationTO> SelectAllTblOrganizationWithDefaultAddress(Constants.OrgTypeE orgTypeE)
        {
            return TblOrganizationDAO.SelectAllTblOrganizationWithDefaultAddress(orgTypeE);
        }

        public static List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrganizationDAO.SelectAllTblOrganization(orgTypeE, conn, tran);
        }

        public static List<DropDownTO> SelectAllOrganizationListForDropDown(Constants.OrgTypeE orgTypeE, List<TblUserRoleTO> userRoleTOList)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (userRoleTOList != null && userRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(userRoleTOList);
            }
            return TblOrganizationDAO.SelectAllOrganizationListForDropDown(orgTypeE, tblUserRoleTO);
        }

        public static List<DropDownTO> SelectAllSpecialCnfListForDropDown(List<TblUserRoleTO> tblUserRoleTOList)
        {
            TblUserRoleTO userRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                userRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return TblOrganizationDAO.SelectAllSpecialCnfListForDropDown(userRoleTO);
        }

        public static List<DropDownTO> SelectDealerListForDropDown(Int32 cnfId, List<TblUserRoleTO> tblUserRoleTOList, Int32 consumerType = 0)
        {
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
            if (tblConfigParamsTO != null)
            {
                if (cnfId.ToString() == tblConfigParamsTO.ConfigParamVal)
                {
                    cnfId = 0;
                }
            }
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return TblOrganizationDAO.SelectDealerListForDropDown(cnfId, tblUserRoleTO, consumerType);
        }

        public static List<DropDownTO> GetDealerDropDownListAsPerLastTxn(Int32 cnfId, List<TblUserRoleTO> tblUserRoleTOList)
        {
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
            if (tblConfigParamsTO != null)
            {
                if (cnfId.ToString() == tblConfigParamsTO.ConfigParamVal)
                {
                    cnfId = 0;
                }
            }
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return TblOrganizationDAO.SelectDealerDropDownListAsPerLastTxn(cnfId, tblUserRoleTO);
        }


        /// <summary>
        /// Sudhir[26-July-2018] --Add this Method for District & field officer link to be establish. 
        ///                        Regional manger can see his field office visit list. 
        ///                        Also field office can see their own visits
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        public static List<DropDownTO> SelectDealerListForDropDownForCRM(Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        {
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
            if (tblConfigParamsTO != null)
            {
                if (cnfId.ToString() == tblConfigParamsTO.ConfigParamVal)
                {
                    cnfId = 0;
                }
            }

            return TblOrganizationDAO.SelectDealerListForDropDownForCRM(cnfId, tblUserRoleTO);
        }

        public static List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId, List<TblUserRoleTO> tblUserRoleTOList)
        {
            TblUserRoleTO userRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                userRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return TblOrganizationDAO.GetDealerForLoadingDropDownList(cnfId, userRoleTO);
        }

        public static Dictionary<int, string> SelectRegisteredMobileNoDCT(String orgIds, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrganizationDAO.SelectRegisteredMobileNoDCT(orgIds, conn, tran);
        }

        public static Dictionary<int, string> SelectRegisteredMobileNoDCTByOrgType(String orgTypeIds, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrganizationDAO.SelectRegisteredMobileNoDCTByOrgType(orgTypeIds, conn, tran);
        }

        public static List<OrgExportRptTO> SelectAllOrgListToExport(Int32 orgTypeId, Int32 parentId)
        {
            List<OrgExportRptTO> list = TblOrganizationDAO.SelectAllOrgListToExport(orgTypeId, parentId);
            if (list != null && orgTypeId == (int)Constants.OrgTypeE.DEALER)
                list = list.OrderBy(a => a.CnfName).ThenBy(d => d.FirmName).ToList();
            else if (list != null)
                list = list.OrderBy(a => a.FirmName).ToList();

            return list;
        }

        //Sudhir[23-APR-2018] Added for Check OrgName And Phone no is Already Exist or Not.
        public static ResultMessage CheckOrgNameOrPhoneNoIsExist(String OrgName, String PhoneNo, Int32 orgType)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.Result = 1;
            resultMessage.Text = "Valid Name And Phone Number";
            resultMessage.DisplayMessage = "Valid Name And Phone Number";
            resultMessage.MessageType = ResultMessageE.Information;
            Boolean isExistOrNot = false;
            try
            {
                List<TblOrganizationTO> allOrganizationList = TblOrganizationDAO.SelectAllOrganizationListV2(orgType);
                if (allOrganizationList != null && allOrganizationList.Count > 0)
                {
                    if (OrgName != String.Empty || PhoneNo != String.Empty)
                    {
                        if (OrgName != String.Empty && OrgName != null)
                        {
                            isExistOrNot = allOrganizationList.Any(str => str.FirmName.Trim() == OrgName.Trim());
                            if (isExistOrNot)
                            {
                                resultMessage.Text = "Organization Name is Already Exist";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                resultMessage.DisplayMessage = "Organization Name is Already Exist";
                                return resultMessage;
                            }
                        }

                        if (PhoneNo != String.Empty && PhoneNo != null)
                        {
                            isExistOrNot = allOrganizationList.Any(str => str.PhoneNo == PhoneNo.Trim());
                            if (isExistOrNot)
                            {
                                resultMessage.Text = "Phone Number is Already Exist";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                resultMessage.DisplayMessage = "Phone Number is Already Exist";
                                return resultMessage;
                            }
                        }

                        return resultMessage;
                    }
                    return resultMessage;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                return resultMessage;
            }
        }

        public static List<DropDownTO> SelectSalesEngineerListForDropDown(Int32 orgId)
        {
            return TblOrganizationDAO.SelectSalesEngineerListForDropDown(orgId);
        }

        //Priyanka [08-04-2019]
        public static List<SalesTrackerAPI.DashboardModels.DealerOverdueDtl> GetDealersOverdueDetailsInfo(Int32 dealerId)
        {
            return TblOrganizationDAO.GetDealersOverdueDetailsInfo(dealerId);
        }
       
        #endregion

        #region Insertion
        public static int InsertTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            return TblOrganizationDAO.InsertTblOrganization(tblOrganizationTO);
        }

        public static int InsertTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrganizationDAO.InsertTblOrganization(tblOrganizationTO, conn, tran);
        }

        public static ResultMessage SaveNewOrganization(TblOrganizationTO tblOrganizationTO)
        {
            ResultMessage rMessage = new StaticStuff.ResultMessage();
            rMessage.MessageType = ResultMessageE.None;
            rMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            Boolean updateOrgYn = false;
            TblPersonTO firstOwnerPersonTO = null;
            TblPersonTO secondOwnerPersonTO = null;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Create Organization First

                result = InsertTblOrganization(tblOrganizationTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblOrganization in Method SaveNewOrganization";
                    return rMessage;
                }

                #endregion

                #region 1.1 If OrgTypeE = Competitor Then Save its brand details

                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.COMPETITOR)
                {
                    if (tblOrganizationTO.CompetitorExtTOList == null || tblOrganizationTO.CompetitorExtTOList.Count == 0)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While Competitor Brand List Found Null in Method SaveNewOrganization";
                        return rMessage;
                    }

                    for (int b = 0; b < tblOrganizationTO.CompetitorExtTOList.Count; b++)
                    {
                        tblOrganizationTO.CompetitorExtTOList[b].OrgId = tblOrganizationTO.IdOrganization;
                        tblOrganizationTO.CompetitorExtTOList[b].MfgCompanyName = tblOrganizationTO.FirmName;
                        result = BL.TblCompetitorExtBL.InsertTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblCompetitorExt Competitor Brand List in Method SaveNewOrganization";
                            return rMessage;
                        }
                    }
                }

                #endregion


                //  Priyanka[16 - 02 - 18] : Added to Save the Purchase Competitor Material & Grade Details.
                #region 1.2 If OrgTypeE = Purchase Competitor Then Save its Material & Grade details

                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.PURCHASE_COMPETITOR)
                {
                    if (tblOrganizationTO.PurchaseCompetitorExtTOList == null || tblOrganizationTO.PurchaseCompetitorExtTOList.Count == 0)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While Purchase Competitor Material List Found Null in Method SaveNewOrganization";
                        return rMessage;
                    }

                    for (int b = 0; b < tblOrganizationTO.PurchaseCompetitorExtTOList.Count; b++)
                    {
                        tblOrganizationTO.PurchaseCompetitorExtTOList[b].OrganizationId = tblOrganizationTO.IdOrganization;
                        result = BL.TblPurchaseCompetitorExtBL.InsertTblPurchaseCompetitorExt(tblOrganizationTO.PurchaseCompetitorExtTOList[b], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblPurchaseCompetitorExt Competitor Material List in Method SaveNewOrganization";
                            return rMessage;
                        }
                    }
                }

                #endregion



                #region 2. Create New Persons and Update Owner Person in tblOrganization

                if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.PersonList.Count; i++)
                    {
                        TblPersonTO personTO = tblOrganizationTO.PersonList[i];
                        personTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        personTO.CreatedOn = tblOrganizationTO.CreatedOn;

                        if (personTO.DobDay > 0 && personTO.DobMonth > 0 && personTO.DobYear > 0)
                        {
                            personTO.DateOfBirth = new DateTime(personTO.DobYear, personTO.DobMonth, personTO.DobDay);
                        }
                        else
                        {
                            personTO.DateOfBirth = DateTime.MinValue;
                        }

                        if (personTO.SeqNo == 1)
                        {
                            personTO.Comments = "First Owner - " + tblOrganizationTO.FirmName;
                            tblOrganizationTO.FirstOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.FirstOwnerName = personTO.FirstName + " " + personTO.LastName;
                            firstOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }
                        else if (personTO.SeqNo == 2)
                        {
                            personTO.Comments = "Second Owner - " + tblOrganizationTO.FirmName;
                            tblOrganizationTO.SecondOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.SecondOwnerName = personTO.FirstName + " " + personTO.LastName;
                            secondOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }

                        result = BL.TblPersonBL.InsertTblPerson(personTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblPerson in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        if (personTO.SeqNo == 1)
                        {
                            tblOrganizationTO.FirstOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.FirstOwnerName = personTO.FirstName + " " + personTO.LastName;
                            firstOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }
                        else if (personTO.SeqNo == 2)
                        {
                            tblOrganizationTO.SecondOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.SecondOwnerName = personTO.FirstName + " " + personTO.LastName;
                            secondOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }

                    }

                }

                #endregion

                #region 3. Add Address Details

                List<TblOrgAddressTO> tblOrgAddressTOList = new List<Models.TblOrgAddressTO>();
                if (tblOrganizationTO.AddressList != null && tblOrganizationTO.AddressList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.AddressList.Count; i++)
                    {
                        TblAddressTO addressTO = tblOrganizationTO.AddressList[i];
                        addressTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        addressTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        if (addressTO.CountryId == 0)
                            addressTO.CountryId = Constants.DefaultCountryID;

                        if (addressTO.DistrictId == 0 && !string.IsNullOrEmpty(addressTO.DistrictName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO districtDimensionTO = new CommonDimensionsTO();
                            districtDimensionTO.ParentId = addressTO.StateId;
                            districtDimensionTO.DimensionName = addressTO.DistrictName;

                            result = DimensionBL.InsertDistrict(districtDimensionTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertDistrict in Method SaveNewOrganization";
                                return rMessage;
                            }
                            addressTO.DistrictId = districtDimensionTO.IdDimension;
                        }

                        if (addressTO.TalukaId == 0 && !string.IsNullOrEmpty(addressTO.TalukaName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO talukaDimensionTO = new CommonDimensionsTO();
                            talukaDimensionTO.ParentId = addressTO.DistrictId;
                            talukaDimensionTO.DimensionName = addressTO.TalukaName;

                            result = DimensionBL.InsertTaluka(talukaDimensionTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTaluka in Method SaveNewOrganization";
                                return rMessage;
                            }
                            addressTO.TalukaId = talukaDimensionTO.IdDimension;
                        }

                        result = TblAddressBL.InsertTblAddress(addressTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblAddress in Method SaveNewOrganization";
                            return rMessage;
                        }

                        TblOrgAddressTO tblOrgAddressTO = addressTO.GetTblOrgAddressTO();
                        tblOrgAddressTO.OrganizationId = tblOrganizationTO.IdOrganization;

                        result = TblOrgAddressBL.InsertTblOrgAddress(tblOrgAddressTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblOrgAddress in Method SaveNewOrganization";
                            return rMessage;
                        }

                        if (addressTO.AddressTypeE == Constants.AddressTypeE.OFFICE_ADDRESS)
                        {
                            updateOrgYn = true;
                            tblOrganizationTO.AddrId = addressTO.IdAddr;
                        }
                    }
                }
                #endregion

                #region 4. Save Organization Commercial Licences
                if (tblOrganizationTO.OrgLicenseDtlTOList != null && tblOrganizationTO.OrgLicenseDtlTOList.Count > 0)
                {
                    for (int ol = 0; ol < tblOrganizationTO.OrgLicenseDtlTOList.Count; ol++)
                    {
                        if (!String.IsNullOrEmpty(tblOrganizationTO.OrgLicenseDtlTOList[ol].LicenseValue))
                        {
                            tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                            tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.CreatedBy;
                            tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.CreatedOn;
                            result = BL.TblOrgLicenseDtlBL.InsertTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method SaveNewOrganization ";
                                return rMessage;
                            }
                        }
                    }
                }
                #endregion

                #region 4. Save KYC details

                if (tblOrganizationTO.KYCDetailsTO != null)
                {

                    tblOrganizationTO.KYCDetailsTO.OrganizationId = tblOrganizationTO.IdOrganization;
                    tblOrganizationTO.KYCDetailsTO.CreatedBy = tblOrganizationTO.CreatedBy;
                    tblOrganizationTO.KYCDetailsTO.CreatedOn = tblOrganizationTO.CreatedOn;
                    tblOrganizationTO.KYCDetailsTO.IsActive = 1;

                    result = BL.TblKYCDetailsBL.InsertTblKYCDetails(tblOrganizationTO.KYCDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While InsertTblKYCDetails for Users in Method SaveNewOrganization ";
                        return rMessage;
                    }

                }
                #endregion
                #region 5. Is Address Or Concern Person Found then Update in tblOrganization

                if (updateOrgYn)
                {
                    tblOrganizationTO.UpdatedBy = tblOrganizationTO.CreatedBy;
                    tblOrganizationTO.UpdatedOn = tblOrganizationTO.CreatedOn;
                    result = UpdateTblOrganization(tblOrganizationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While UpdateTblOrganization for Persons in Method SaveNewOrganization ";
                        return rMessage;
                    }
                }

                #endregion

                #region 6. If New Organization Type is Cnf Then Auto Create User for First Owner

                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                {
                    //Create Default Loading Quota Configurations and its declaration with Zero Values 
                    List<TblLoadingQuotaConfigTO> loadQCList = BL.TblLoadingQuotaConfigBL.SelectEmptyLoadingQuotaConfig(conn, tran);
                    if (loadQCList != null)
                    {
                        for (int lc = 0; lc < loadQCList.Count; lc++)
                        {
                            loadQCList[lc].CnfOrgId = tblOrganizationTO.IdOrganization;
                            loadQCList[lc].Remark = "Default Configuration";
                            loadQCList[lc].IsActive = 1;
                            loadQCList[lc].CreatedBy = tblOrganizationTO.CreatedBy;
                            loadQCList[lc].CreatedOn = tblOrganizationTO.CreatedOn;
                            result = BL.TblLoadingQuotaConfigBL.InsertTblLoadingQuotaConfig(loadQCList[lc], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblLoadingQuotaConfig";
                                rMessage.Result = 0;
                                return rMessage;
                            }
                        }


                        if (BL.TblLoadingQuotaDeclarationBL.IsLoadingQuotaDeclaredForTheDate(tblOrganizationTO.CreatedOn, conn, tran))
                        {
                            List<TblLoadingQuotaDeclarationTO> loadQuotaList = BL.TblLoadingQuotaDeclarationBL.SelectLatestCalculatedLoadingQuotaDeclarationList(tblOrganizationTO.CreatedOn, tblOrganizationTO.IdOrganization, conn, tran);

                            for (int lq = 0; lq < loadQuotaList.Count; lq++)
                            {
                                loadQuotaList[lq].CreatedOn = tblOrganizationTO.CreatedOn;
                                loadQuotaList[lq].CreatedBy = tblOrganizationTO.CreatedBy;
                                loadQuotaList[lq].IsActive = 1;
                                loadQuotaList[lq].Remark = "New Default Quota Declaration On Cnf Creation";

                                result = BL.TblLoadingQuotaDeclarationBL.InsertTblLoadingQuotaDeclaration(loadQuotaList[lq], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    rMessage.Text = "Error While InsertTblLoadingQuotaDeclaration : SaveNewOrganization";
                                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Result = 0;
                                    return rMessage;
                                }
                            }
                        }
                    }

                    //Assign Latest Rate Declaration To Him
                    TblQuotaDeclarationTO quotaDeclarationTO = BL.TblQuotaDeclarationBL.SelectLatestQuotaDeclarationTO(conn, tran);
                    if (quotaDeclarationTO != null && quotaDeclarationTO.CreatedOn.Date == tblOrganizationTO.CreatedOn.Date)
                    {
                        quotaDeclarationTO.AllocQty = 0;
                        quotaDeclarationTO.BalanceQty = 0;
                        quotaDeclarationTO.CalculatedRate = quotaDeclarationTO.CalculatedRate + quotaDeclarationTO.RateBand;
                        quotaDeclarationTO.RateBand = 0;
                        quotaDeclarationTO.ValidUpto = 0;
                        quotaDeclarationTO.OrgId = tblOrganizationTO.IdOrganization;
                        quotaDeclarationTO.QuotaAllocDate = tblOrganizationTO.CreatedOn;
                        quotaDeclarationTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        quotaDeclarationTO.CreatedBy = tblOrganizationTO.CreatedBy;

                        result = BL.TblQuotaDeclarationBL.InsertTblQuotaDeclaration(quotaDeclarationTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblQuotaDeclaration";
                            rMessage.Result = 0;
                            rMessage.Tag = tblOrganizationTO;
                            return rMessage;
                        }
                    }
                }
                #endregion

                #region If Dealer then Manage Cnf & Dealer Relationship

                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.DEALER)
                {
                    if (tblOrganizationTO.ParentId > 0)
                    {
                        TblCnfDealersTO tblCnfDealersTO = new TblCnfDealersTO();
                        tblCnfDealersTO.CnfOrgId = tblOrganizationTO.ParentId;
                        tblCnfDealersTO.DealerOrgId = tblOrganizationTO.IdOrganization;
                        tblCnfDealersTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        tblCnfDealersTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        tblCnfDealersTO.IsActive = 1;
                        tblCnfDealersTO.Remark = "Primary C&F";
                        result = BL.TblCnfDealersBL.InsertTblCnfDealers(tblCnfDealersTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblCnfDealers for CnfDealer in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        if (tblOrganizationTO.CnfDealersTOList != null)
                        {
                            for (int c = 0; c < tblOrganizationTO.CnfDealersTOList.Count; c++)
                            {
                                tblOrganizationTO.CnfDealersTOList[c].DealerOrgId = tblOrganizationTO.IdOrganization;
                                tblOrganizationTO.CnfDealersTOList[c].CreatedBy = tblOrganizationTO.CreatedBy;
                                tblOrganizationTO.CnfDealersTOList[c].CreatedOn = tblOrganizationTO.CreatedOn;
                                tblOrganizationTO.CnfDealersTOList[c].IsActive = 1;
                                tblOrganizationTO.CnfDealersTOList[c].IsSpecialCnf = 1;
                                tblOrganizationTO.CnfDealersTOList[c].Remark = "Special C&F";
                                result = BL.TblCnfDealersBL.InsertTblCnfDealers(tblOrganizationTO.CnfDealersTOList[c], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error While InsertTblCnfDealers for Special C&F in Method SaveNewOrganization ";
                                    return rMessage;
                                }
                            }
                        }
                    }
                }

                #endregion

                #region 7. User Creation Based On Orgtype settings defined in the masters

                DimOrgTypeTO orgTypeTO = BL.DimOrgTypeBL.SelectDimOrgTypeTO(tblOrganizationTO.OrgTypeId, conn, tran);

                if (orgTypeTO.CreateUserYn == 1 && orgTypeTO.DefaultRoleId > 0)
                {

                    if (tblOrganizationTO.FirstOwnerPersonId > 0)
                    {
                        rMessage = CreateNewOrganizationUser(tblOrganizationTO, firstOwnerPersonTO, orgTypeTO, tblOrganizationTO.CreatedBy, tblOrganizationTO.CreatedOn, conn, tran);
                        if (rMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            rMessage.DefaultBehaviour("Error While Org User Creation");
                            return rMessage;
                        }
                    }

                    if (tblOrganizationTO.SecondOwnerPersonId > 0)
                    {
                        rMessage = CreateNewOrganizationUser(tblOrganizationTO, secondOwnerPersonTO, orgTypeTO, tblOrganizationTO.CreatedBy, tblOrganizationTO.CreatedOn, conn, tran);
                        if (rMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            rMessage.DefaultBehaviour("Error While Org User Creation");
                            return rMessage;
                        }
                    }
                }
                else
                {
                    // Do Nothing. allow to save the record.
                }
                #endregion

                #region 8. If Organization Contains Persons then Add Persons Into tblOrgPersonDtls. //Sudhir[20-APR-2018] Added for Organization And Person Linking
                if (updateOrgYn)
                {
                    if (tblOrganizationTO.FirstOwnerPersonId > 0)
                    {
                        TblOrgPersonDtlsTO tblOrgPersonDtlsTO = new TblOrgPersonDtlsTO();
                        tblOrgPersonDtlsTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblOrgPersonDtlsTO.PersonId = tblOrganizationTO.FirstOwnerPersonId;
                        tblOrgPersonDtlsTO.PersonTypeId = (int)Constants.PersonType.FIRST_OWNER;
                        tblOrgPersonDtlsTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        tblOrgPersonDtlsTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        tblOrgPersonDtlsTO.IsActive = 1;
                        result = TblOrgPersonDtlsBL.InsertTblOrgPersonDtls(tblOrgPersonDtlsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While Inserting  First_Owner Into tblOrgPersonDtls for Persons & Organization in Method SaveNewOrganization ";
                            return rMessage;
                        }
                    }
                    if (tblOrganizationTO.SecondOwnerPersonId > 0)
                    {
                        TblOrgPersonDtlsTO tblOrgPersonDtlsTO = new TblOrgPersonDtlsTO();
                        tblOrgPersonDtlsTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblOrgPersonDtlsTO.PersonId = tblOrganizationTO.SecondOwnerPersonId;
                        tblOrgPersonDtlsTO.PersonTypeId = (int)Constants.PersonType.SECOND_OWNER;
                        tblOrgPersonDtlsTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        tblOrgPersonDtlsTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        tblOrgPersonDtlsTO.IsActive = 1;
                        result = TblOrgPersonDtlsBL.InsertTblOrgPersonDtls(tblOrgPersonDtlsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While Inserting  Second_Owner Into tblOrgPersonDtls for Persons & Organization in Method SaveNewOrganization ";
                            return rMessage;
                        }
                    }

                }
                #endregion

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Sucessfully";
                rMessage.Result = 1;
                rMessage.Tag = tblOrganizationTO;
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In Method SaveNewOrganization";
                rMessage.Tag = ex;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        //Priyanka [15-04-2019]
        public static ResultMessage SaveOrganizationRefIds(TblOrganizationTO organizationTO, string loginUserId)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {

                if (!String.IsNullOrEmpty(organizationTO.OverdueRefId))
                {
                    List<TblOrganizationTO> tblOrganizationTOList = TblOrganizationBL.SelectExistingAllTblOrganizationByRefIds(organizationTO.IdOrganization, organizationTO.OverdueRefId, null);
                    if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
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

                    if (organizationTO.EnqRefId == null)
                    {
                        TblOrganizationTO tblOrganizationTOOld = TblOrganizationBL.SelectTblOrganizationTO(organizationTO.IdOrganization);
                        if (tblOrganizationTOOld != null)
                        {
                            organizationTO.EnqRefId = tblOrganizationTOOld.EnqRefId;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(organizationTO.EnqRefId))
                {
                    List<TblOrganizationTO> tblOrganizationTOList = TblOrganizationBL.SelectExistingAllTblOrganizationByRefIds(organizationTO.IdOrganization, null, organizationTO.EnqRefId);
                    if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
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

                    if (organizationTO.OverdueRefId == null)
                    {
                        TblOrganizationTO tblOrganizationTOOld = TblOrganizationBL.SelectTblOrganizationTO(organizationTO.IdOrganization);
                        if (tblOrganizationTOOld != null)
                        {
                            organizationTO.OverdueRefId = tblOrganizationTOOld.OverdueRefId;
                        }
                    }
                }
                DateTime serverDate = Constants.ServerDateTime;
                organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                organizationTO.UpdatedOn = serverDate;
              
                result = BL.TblOrganizationBL.UpdateTblOrganizationRefIds(organizationTO);
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
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method SaveNewOrganization";
                resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        
        private static ResultMessage CreateNewOrganizationUser(TblOrganizationTO tblOrganizationTO, TblPersonTO personTO, DimOrgTypeTO orgTypeTO, Int32 createdBy, DateTime createdOn, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage rMessage = new ResultMessage();
            int result;
            String userId = TblUserBL.CreateUserName(personTO.FirstName, personTO.LastName, conn, tran);
            String pwd = Constants.DefaultPassword;

            TblUserTO userTO = new Models.TblUserTO();
            userTO.UserLogin = userId;
            userTO.UserPasswd = pwd;
            userTO.UserDisplayName = personTO.FirstName + " " + personTO.LastName;
            userTO.IsActive = 1;

            result = BL.TblUserBL.InsertTblUser(userTO, conn, tran);

            if (result != 1)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Error While InsertTblUser for Users in Method SaveNewOrganization ";
                return rMessage;
            }

            TblUserExtTO tblUserExtTO = new TblUserExtTO();
            tblUserExtTO.AddressId = tblOrganizationTO.AddrId;
            tblUserExtTO.CreatedBy = createdBy;
            tblUserExtTO.CreatedOn = createdOn;
            tblUserExtTO.PersonId = tblOrganizationTO.FirstOwnerPersonId;
            tblUserExtTO.OrganizationId = tblOrganizationTO.IdOrganization;
            tblUserExtTO.UserId = userTO.IdUser;
            tblUserExtTO.Comments = "New " + orgTypeTO.OrgType + " User Created";

            result = BL.TblUserExtBL.InsertTblUserExt(tblUserExtTO, conn, tran);
            if (result != 1)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Error While InsertTblUserExt for Users in Method SaveNewOrganization ";
                return rMessage;
            }

            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            tblUserRoleTO.UserId = userTO.IdUser;
            tblUserRoleTO.RoleId = orgTypeTO.DefaultRoleId;
            tblUserRoleTO.IsActive = 1;
            tblUserRoleTO.CreatedBy = createdBy;
            tblUserRoleTO.CreatedOn = createdOn;

            result = BL.TblUserRoleBL.InsertTblUserRole(tblUserRoleTO, conn, tran);
            if (result != 1)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Error While InsertTblUserRole for orgType=" + orgTypeTO.OrgType + " in Method SaveNewOrganization ";
                return rMessage;
            }

            rMessage.DefaultSuccessBehaviour();
            return rMessage;
        }



        #endregion

        #region Updation
        public static int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            return TblOrganizationDAO.UpdateTblOrganization(tblOrganizationTO);
        }

        public static int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrganizationDAO.UpdateTblOrganization(tblOrganizationTO, conn, tran);
        }

        public static ResultMessage UpdateOrganization(TblOrganizationTO tblOrganizationTO)
        {
            ResultMessage rMessage = new StaticStuff.ResultMessage();
            rMessage.MessageType = ResultMessageE.None;
            rMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            Boolean updateOrgYn = false;
            TblPersonTO firstOwnerPersonTO = null;
            TblPersonTO secondOwnerPersonTO = null;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();

                //Saket [2018-02-21] Added to create user while updating the C&F
                Boolean isNewFirstOwner = false;
                Boolean isNewSecondOwner = false;
                if (tblOrganizationTO.FirstOwnerPersonId == 0)
                {
                    isNewFirstOwner = true;
                }
                if (tblOrganizationTO.SecondOwnerPersonId == 0)
                {
                    isNewSecondOwner = true;
                }


                #region 1. Create Organization First

                result = UpdateTblOrganization(tblOrganizationTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While UpdateTblOrganization in Method UpdateOrganization";
                    return rMessage;
                }

                #endregion

                #region 1.1 If OrgType=Competitor then update the list
                List<TblCompetitorExtTO> list = BL.TblCompetitorExtBL.SelectAllTblCompetitorExtList(tblOrganizationTO.IdOrganization, conn, tran);
                if (tblOrganizationTO.CompetitorExtTOList != null)
                {
                    for (int b = 0; b < tblOrganizationTO.CompetitorExtTOList.Count; b++)
                    {
                        TblCompetitorExtTO existTO = null;

                        if (list != null)
                            existTO = list.Where(l => l.IdCompetitorExt == tblOrganizationTO.CompetitorExtTOList[b].IdCompetitorExt).FirstOrDefault();

                        if (existTO == null)
                        {
                            //Insert New Brand
                            tblOrganizationTO.CompetitorExtTOList[b].OrgId = tblOrganizationTO.IdOrganization;
                            result = BL.TblCompetitorExtBL.InsertTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblCompetitorExt in Method UpdateTblOrganization";
                                return rMessage;
                            }
                        }
                        else
                        {
                            //Update existing brand
                            result = BL.TblCompetitorExtBL.UpdateTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblCompetitorExt in Method UpdateTblOrganization";
                                return rMessage;
                            }
                        }
                    }
                }

                #endregion

                // Priyanka[16 - 02 - 18] : Added to save the Purchase Competitor Material & Grade Details.
                #region 1.2 If OrgType=Purchase Competitor then update the Material & Grade Details

                List<TblPurchaseCompetitorExtTO> tblPurchaseCompetitorExtTOList = BL.TblPurchaseCompetitorExtBL.SelectAllTblPurchaseCompetitorExtList(tblOrganizationTO.IdOrganization, conn, tran);
                if (tblOrganizationTO.PurchaseCompetitorExtTOList != null)
                {
                    for (int b = 0; b < tblOrganizationTO.PurchaseCompetitorExtTOList.Count; b++)
                    {
                        TblPurchaseCompetitorExtTO purchaseCompetitorexistTO = null;

                        if (tblPurchaseCompetitorExtTOList != null)
                            purchaseCompetitorexistTO = tblPurchaseCompetitorExtTOList.Where(l => l.IdPurCompetitorExt == tblOrganizationTO.PurchaseCompetitorExtTOList[b].IdPurCompetitorExt).FirstOrDefault();

                        if (purchaseCompetitorexistTO == null)
                        {
                            //Insert New Material & Grade
                            tblOrganizationTO.PurchaseCompetitorExtTOList[b].OrganizationId = tblOrganizationTO.IdOrganization;
                            result = BL.TblPurchaseCompetitorExtBL.InsertTblPurchaseCompetitorExt(tblOrganizationTO.PurchaseCompetitorExtTOList[b], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblPurchaseCompetitorExt in Method UpdateTblOrganization";
                                return rMessage;
                            }
                        }
                        else
                        {
                            //Update Material & Grade
                            result = BL.TblPurchaseCompetitorExtBL.UpdateTblPurchaseCompetitorExt(tblOrganizationTO.PurchaseCompetitorExtTOList[b], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblPurchaseCompetitorExt in Method UpdateTblOrganization";
                                return rMessage;
                            }
                        }
                    }
                }

                #endregion

                #region 2. Create New Persons and Update Owner Person in tblOrganization

                if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.PersonList.Count; i++)
                    {
                        TblPersonTO personTO = tblOrganizationTO.PersonList[i];

                        if (personTO.DobDay > 0 && personTO.DobMonth > 0 && personTO.DobYear > 0)
                        {
                            personTO.DateOfBirth = new DateTime(personTO.DobYear, personTO.DobMonth, personTO.DobDay);
                        }
                        else
                        {
                            personTO.DateOfBirth = DateTime.MinValue;
                        }

                        if (personTO.IdPerson == 0)
                        {
                            personTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            personTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                            result = BL.TblPersonBL.InsertTblPerson(personTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblPerson in Method UpdateOrganization ";
                                return rMessage;
                            }
                        }
                        else
                        {
                            result = BL.TblPersonBL.UpdateTblPerson(personTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblPerson in Method UpdateOrganization ";
                                return rMessage;
                            }
                        }

                        if (personTO.SeqNo == 1)
                        {
                            tblOrganizationTO.FirstOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.FirstOwnerName = personTO.FirstName + " " + personTO.LastName;
                            firstOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }
                        else if (personTO.SeqNo == 2)
                        {
                            tblOrganizationTO.SecondOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.SecondOwnerName = personTO.FirstName + " " + personTO.LastName;
                            secondOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }
                    }

                }

                #endregion

                #region 3. Add Address Details

                List<TblOrgAddressTO> tblOrgAddressTOList = new List<Models.TblOrgAddressTO>();
                if (tblOrganizationTO.AddressList != null && tblOrganizationTO.AddressList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.AddressList.Count; i++)
                    {
                        TblAddressTO addressTO = tblOrganizationTO.AddressList[i];
                        addressTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                        addressTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                        if (addressTO.CountryId == 0)
                            addressTO.CountryId = Constants.DefaultCountryID;

                        if (addressTO.DistrictId == 0 && !string.IsNullOrEmpty(addressTO.DistrictName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO districtDimensionTO = new CommonDimensionsTO();
                            districtDimensionTO.ParentId = addressTO.StateId;
                            districtDimensionTO.DimensionName = addressTO.DistrictName;

                            result = DimensionBL.InsertDistrict(districtDimensionTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertDistrict in Method UpdateOrganization";
                                return rMessage;
                            }

                            addressTO.DistrictId = districtDimensionTO.IdDimension;
                        }

                        if (addressTO.TalukaId == 0 && !string.IsNullOrEmpty(addressTO.TalukaName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO talukaDimensionTO = new CommonDimensionsTO();
                            talukaDimensionTO.ParentId = addressTO.DistrictId;
                            talukaDimensionTO.DimensionName = addressTO.TalukaName;

                            result = DimensionBL.InsertTaluka(talukaDimensionTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTaluka in Method UpdateOrganization";
                                return rMessage;
                            }

                            addressTO.TalukaId = talukaDimensionTO.IdDimension;

                        }

                        if (addressTO.IdAddr == 0)
                        {
                            result = TblAddressBL.InsertTblAddress(addressTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblAddress in Method UpdateOrganization";
                                return rMessage;
                            }

                            TblOrgAddressTO tblOrgAddressTO = addressTO.GetTblOrgAddressTO();
                            tblOrgAddressTO.OrganizationId = tblOrganizationTO.IdOrganization;

                            result = TblOrgAddressBL.InsertTblOrgAddress(tblOrgAddressTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblOrgAddress in Method UpdateOrganization";
                                return rMessage;
                            }

                            if (addressTO.AddressTypeE == Constants.AddressTypeE.OFFICE_ADDRESS)
                            {
                                updateOrgYn = true;
                                tblOrganizationTO.AddrId = addressTO.IdAddr;
                            }
                        }
                        else
                        {
                            result = TblAddressBL.UpdateTblAddress(addressTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblAddress in Method UpdateOrganization";
                                return rMessage;
                            }
                        }
                    }
                }
                #endregion

                #region 4. Save Organization Commercial Licences
                if (tblOrganizationTO.OrgLicenseDtlTOList != null && tblOrganizationTO.OrgLicenseDtlTOList.Count > 0)
                {
                    for (int ol = 0; ol < tblOrganizationTO.OrgLicenseDtlTOList.Count; ol++)
                    {

                        if (tblOrganizationTO.OrgLicenseDtlTOList[ol].IdOrgLicense == 0)
                        {
                            if (!String.IsNullOrEmpty(tblOrganizationTO.OrgLicenseDtlTOList[ol].LicenseValue))
                            {
                                tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                                tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.UpdatedBy;
                                tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.UpdatedOn;
                                result = BL.TblOrgLicenseDtlBL.InsertTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method UpdateOrganization ";
                                    return rMessage;
                                }
                            }
                        }
                        else
                        {
                            tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                            result = BL.TblOrgLicenseDtlBL.UpdateTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method UpdateOrganization ";
                                return rMessage;
                            }
                        }
                    }
                }
                #endregion

                #region 5. Save KYC Details
                //Priyanka [23-10-2018]  : Added to update KYC Details.
                if (tblOrganizationTO.KYCDetailsTO != null)
                {
                    TblKYCDetailsTO tblKYCDetailsTOExt = BL.TblKYCDetailsBL.SelectTblKYCDetailsTOByOrg(tblOrganizationTO.IdOrganization);
                    if (tblKYCDetailsTOExt != null)
                    {
                        tblKYCDetailsTOExt.IsActive = 0;
                        tblKYCDetailsTOExt.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblKYCDetailsTOExt.UpdatedOn = tblOrganizationTO.UpdatedOn;
                        tblKYCDetailsTOExt.UpdatedBy = tblOrganizationTO.UpdatedBy;
                        result = BL.TblKYCDetailsBL.UpdateTblKYCDetails(tblKYCDetailsTOExt, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While UpdateTblKYCDetails for Users in Method UpdateOrganization ";
                            return rMessage;
                        }
                        tblOrganizationTO.KYCDetailsTO.IsActive = 1;
                        tblOrganizationTO.KYCDetailsTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblOrganizationTO.KYCDetailsTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                        tblOrganizationTO.KYCDetailsTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                        result = BL.TblKYCDetailsBL.InsertTblKYCDetails(tblOrganizationTO.KYCDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblKYCDetails for Users in Method UpdateOrganization ";
                            return rMessage;
                        }
                    }
                    else
                    {

                        tblOrganizationTO.KYCDetailsTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblOrganizationTO.KYCDetailsTO.IsActive = 1;
                        tblOrganizationTO.KYCDetailsTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                        tblOrganizationTO.KYCDetailsTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                        result = BL.TblKYCDetailsBL.InsertTblKYCDetails(tblOrganizationTO.KYCDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblKYCDetails for Users in Method UpdateOrganization ";
                            return rMessage;
                        }
                    }
                }
                #endregion

                #region 5. Is Address Or Concern Person Found then Update in tblOrganization

                if (updateOrgYn)
                {
                    result = UpdateTblOrganization(tblOrganizationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While UpdateTblOrganization for Persons in Method UpdateOrganization ";
                        return rMessage;
                    }
                }

                #endregion

                #region If Dealer then Manage Cnf & Dealer Relationship


                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.DEALER)
                {

                    List<TblCnfDealersTO> exList = BL.TblCnfDealersBL.SelectAllActiveCnfDealersList(tblOrganizationTO.IdOrganization, false, conn, tran);
                    if (exList == null)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "C&F and dealer relation not found in Method UpdateOrganization ";
                        return rMessage;
                    }
                    //Sudhir[12-July-2018] Added Condition For CRM Organization Added Who dont have CNF Id 
                    if (exList.Count > 0)
                    {
                        var primaryCnfDelTO = exList.Where(a => a.IsActive == 1 && a.IsSpecialCnf == 0).FirstOrDefault();

                        if (primaryCnfDelTO.CnfOrgId != tblOrganizationTO.ParentId)
                        {
                            //update existing record and set to deactive
                            primaryCnfDelTO.IsActive = 0;
                            result = BL.TblCnfDealersBL.UpdateTblCnfDealers(primaryCnfDelTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblCnfDealers for Primary C&F in Method UpdateOrganization ";
                                return rMessage;
                            }

                            // Insert New Record of Relationship
                            TblCnfDealersTO newTblCnfDealersTO = new TblCnfDealersTO();
                            newTblCnfDealersTO.IsActive = 1;
                            newTblCnfDealersTO.CnfOrgId = tblOrganizationTO.ParentId;
                            newTblCnfDealersTO.DealerOrgId = tblOrganizationTO.IdOrganization;
                            newTblCnfDealersTO.Remark = "Updated Primary C&F";
                            newTblCnfDealersTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            newTblCnfDealersTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                            result = BL.TblCnfDealersBL.InsertTblCnfDealers(newTblCnfDealersTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblCnfDealers for Primary C&F in Method UpdateOrganization ";
                                return rMessage;
                            }
                        }
                    }

                    if (tblOrganizationTO.CnfDealersTOList != null)
                    {
                        for (int c = 0; c < tblOrganizationTO.CnfDealersTOList.Count; c++)
                        {
                            TblCnfDealersTO tblCnfDealersTO = new TblCnfDealersTO();

                            var SpecialTO = exList.Where(a => a.CnfOrgId == tblOrganizationTO.CnfDealersTOList[c].CnfOrgId && a.DealerOrgId == tblOrganizationTO.IdOrganization).FirstOrDefault();

                            if (SpecialTO == null)
                            {
                                tblOrganizationTO.CnfDealersTOList[c].DealerOrgId = tblOrganizationTO.IdOrganization;
                                tblOrganizationTO.CnfDealersTOList[c].CreatedBy = tblOrganizationTO.UpdatedBy;
                                tblOrganizationTO.CnfDealersTOList[c].CreatedOn = tblOrganizationTO.UpdatedOn;
                                tblOrganizationTO.CnfDealersTOList[c].IsActive = 1;
                                tblOrganizationTO.CnfDealersTOList[c].IsSpecialCnf = 1;
                                tblOrganizationTO.CnfDealersTOList[c].Remark = "Special C&F";
                                result = BL.TblCnfDealersBL.InsertTblCnfDealers(tblOrganizationTO.CnfDealersTOList[c], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error While InsertTblCnfDealers for Special C&F in Method UpdateOrganization ";
                                    return rMessage;
                                }
                            }
                        }
                    }
                }


                #endregion

                #region 9. User Creation Based On Orgtype settings defined in the masters

                DimOrgTypeTO orgTypeTO = BL.DimOrgTypeBL.SelectDimOrgTypeTO(tblOrganizationTO.OrgTypeId, conn, tran);

                if (orgTypeTO.CreateUserYn == 1 && orgTypeTO.DefaultRoleId > 0)
                {

                    if (tblOrganizationTO.FirstOwnerPersonId > 0)
                    {
                        if (isNewFirstOwner)
                        {
                            rMessage = CreateNewOrganizationUser(tblOrganizationTO, firstOwnerPersonTO, orgTypeTO, tblOrganizationTO.UpdatedBy, tblOrganizationTO.UpdatedOn, conn, tran);
                            if (rMessage.MessageType != ResultMessageE.Information)
                            {
                                tran.Rollback();
                                rMessage.DefaultBehaviour("Error While Org User Creation");
                                return rMessage;
                            }
                        }
                    }

                    if (tblOrganizationTO.SecondOwnerPersonId > 0)
                    {
                        if (isNewSecondOwner)
                        {
                            rMessage = CreateNewOrganizationUser(tblOrganizationTO, secondOwnerPersonTO, orgTypeTO, tblOrganizationTO.UpdatedBy, tblOrganizationTO.UpdatedOn, conn, tran);
                            if (rMessage.MessageType != ResultMessageE.Information)
                            {
                                tran.Rollback();
                                rMessage.DefaultBehaviour("Error While Org User Creation");
                                return rMessage;
                            }
                        }
                    }
                }
                else
                {
                    // Do Nothing. allow to save the record.
                }
                #endregion

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Sucessfully";
                rMessage.Tag = tblOrganizationTO;
                rMessage.Result = 1;
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In Method UpdateOrganization";
                rMessage.Tag = ex;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Deletion
        public static int DeleteTblOrganization(Int32 idOrganization)
        {
            return TblOrganizationDAO.DeleteTblOrganization(idOrganization);
        }

        public static int DeleteTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrganizationDAO.DeleteTblOrganization(idOrganization, conn, tran);
        }

        #endregion

    }
}
