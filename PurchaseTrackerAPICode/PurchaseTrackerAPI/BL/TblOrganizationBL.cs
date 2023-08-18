using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblOrganizationBL : ITblOrganizationBL
    {
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblCnfDealersBL _iTblCnfDealersBL;
        private readonly ITblAddressBL _iTblAddressBL;
        private readonly ITblOrganizationDAO _iTblOrganizationDAO;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblUserExtBL _iTblUserExtBL;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly Itblquotadeclarationbl _iTblQuotaDeclarationBL;
        private readonly IConnectionString _iConnectionString;
        private readonly IDimStatusDAO _iDimStatusDAO;
        public TblOrganizationBL(ITblOrganizationDAO iTblOrganizationDAO, ITblAddressBL itblAddressBL, ITblCnfDealersBL iTblCnfDealersBL, ITblPersonBL iTblPersonBL, ITblUserBL iTblUserBL, ITblUserExtBL iTblUserExtBL, ITblUserRoleBL iTblUserRoleBL, Itblquotadeclarationbl iTblQuotaDeclarationBL, IConnectionString iConnectionString,
            IDimStatusDAO iDimStatusDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblQuotaDeclarationBL = iTblQuotaDeclarationBL;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblUserExtBL = iTblUserExtBL;
            _iTblUserBL = iTblUserBL;
            _iTblPersonBL = iTblPersonBL;
            _iTblCnfDealersBL = iTblCnfDealersBL;
            _iTblAddressBL = itblAddressBL;
            _iTblOrganizationDAO = iTblOrganizationDAO;
            _iDimStatusDAO = iDimStatusDAO;
        }
        #region Selection

        //public ResultMessage CheckIsSupplierIsBlocked(Int32 supplierID)
        //{
        //    SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction tran = null;
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    Boolean isBlocked = false;

        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();

        //        isBlocked = CheckIsSupplierIsBlocked(supplierID, conn, tran);
        //        if (isBlocked)
        //        {
        //            resultMessage.DefaultBehaviour();
        //            resultMessage.DisplayMessage = "Supplier Is Blocked.";
        //            return resultMessage;
        //        }

        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;

        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "Error in CheckIsSupplierIsBlocked(Int32 supplierID)");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //}

        //public Boolean CheckIsSupplierIsBlocked(Int32 supplierID, SqlConnection conn, SqlTransaction tran)
        //{
        //    Boolean isBlocked = false;

        //    TblOrganizationTO organizationTO = new TblOrganizationTO();
        //    DimStatusTO dimStatusTO = new DimStatusTO();

        //    organizationTO = _iTblOrganizationDAO.SelectTblOrganization(supplierID, conn, tran);

        //    if (organizationTO != null)
        //    {
        //        dimStatusTO = _iDimStatusDAO.SelectDimStatus(organizationTO.OrgStatusId, conn, tran);
        //        if (dimStatusTO != null)
        //        {
        //            if (dimStatusTO.IsBlocked == 1)
        //                isBlocked = true;
        //        }
        //    }

        //    return isBlocked;
        //}

        public  List<TblOrganizationTO> SelectAllTblOrganizationList()
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization();
        }

        public  List<TblOrganizationTO> SelectSalesAgentListWithBrandAndRate()
        {
            // try
            // {
            //     List<TblOrganizationTO> orgList = DAL._iTblOrganizationDAO.SelectSaleAgentOrganizationList();
            //     if (orgList != null)
            //     {
            //         List<DropDownTO> brandList = BL.DimensionBL.SelectBrandList();
            //         Dictionary<Int32, Int32> brandRateDCT = BL.TblGlobalRateBL.SelectLatestBrandAndRateDCT();
            //         Dictionary<Int32, List<TblQuotaDeclarationTO>> rateAndBandDCT = new Dictionary<int, List<TblQuotaDeclarationTO>>();
            //         List<TblGlobalRateTO> tblGlobalRateTOList = new List<TblGlobalRateTO>();
            //         if (brandList == null || brandList.Count == 0)
            //             return null;

            //         foreach (var item in brandRateDCT.Keys)
            //         {
            //             Int32 rateID = brandRateDCT[item];
            //             TblGlobalRateTO rateTO = BL.TblGlobalRateBL.SelectTblGlobalRateTO(rateID);
            //             if (rateTO != null)
            //                 tblGlobalRateTOList.Add(rateTO);
            //             List<TblQuotaDeclarationTO> rateBandList = BL.TblQuotaDeclarationBL.SelectAllTblQuotaDeclarationList(rateID);

            //             rateAndBandDCT.Add(rateID, rateBandList);
            //         }

            //         for (int i = 0; i < orgList.Count; i++)
            //         {
            //             TblOrganizationTO tblOrganizationTO = orgList[i];
            //             tblOrganizationTO.BrandRateDtlTOList = new List<ODLMSWebAPI.Models.BrandRateDtlTO>();
            //             for (int b = 0; b < brandList.Count; b++)
            //             {
            //                 ODLMSWebAPI.Models.BrandRateDtlTO brandRateDtlTO = new ODLMSWebAPI.Models.BrandRateDtlTO();
            //                 brandRateDtlTO.BrandId = brandList[b].Value;
            //                 brandRateDtlTO.BrandName = brandList[b].Text;

            //                 if (brandRateDCT != null && brandRateDCT.ContainsKey(brandRateDtlTO.BrandId))
            //                 {
            //                     int rateId = brandRateDCT[brandRateDtlTO.BrandId];

            //                     if (tblGlobalRateTOList != null)
            //                     {
            //                         TblGlobalRateTO rateTO = tblGlobalRateTOList.Where(ri => ri.IdGlobalRate == rateId).FirstOrDefault();
            //                         if (rateTO != null)
            //                             brandRateDtlTO.Rate = rateTO.Rate;
            //                     }

            //                     if (rateAndBandDCT != null && rateAndBandDCT.ContainsKey(rateId))
            //                     {
            //                         List<TblQuotaDeclarationTO> rateBandList = rateAndBandDCT[rateId];
            //                         if (rateBandList != null)
            //                         {
            //                             var rateBandObj = rateBandList.Where(o => o.OrgId == tblOrganizationTO.IdOrganization).FirstOrDefault();
            //                             if (rateBandObj != null)
            //                                 brandRateDtlTO.RateBand = rateBandObj.RateBand;
            //                         }
            //                     }
            //                 }

            //                 tblOrganizationTO.BrandRateDtlTOList.Add(brandRateDtlTO);
            //             }

            //         }
            //     }

            //     return orgList;
            // }
            // catch (Exception ex)
            // {
            //     return null;
            // }
            // finally
            // {

            // }
            return null;
        }

        public  TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                return _iTblOrganizationDAO.SelectTblOrganization(idOrganization, conn, tran);
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

        public ResultMessage CheckIsSupplierIsBlocked(Int32 supplierID)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            Boolean isBlocked = false;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage =  CheckIsSupplierIsBlocked(supplierID, conn, tran);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in CheckIsSupplierIsBlocked(Int32 supplierID)");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        public ResultMessage CheckIsSupplierIsBlocked(Int32 supplierID, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Boolean isBlocked = false;

            TblOrganizationTO organizationTO = new TblOrganizationTO();
            DimStatusTO dimStatusTO = new DimStatusTO();

            organizationTO = _iTblOrganizationDAO.SelectTblOrganization(supplierID, conn, tran);

            if (organizationTO != null)
            {
                dimStatusTO = _iDimStatusDAO.SelectDimStatus(organizationTO.OrgStatusId, conn, tran);
                if (dimStatusTO != null)
                {
                    if (dimStatusTO.IsBlocked == 1)
                        isBlocked = true;
                }
            }

            if (isBlocked)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.DisplayMessage = "Supplier Is Blacklisted.";
                return resultMessage;
            }

            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public  List<TblOrganizationTO> SelectExistingAllTblOrganizationByRefIds(Int32 orgId, String overdueRefId, String enqRefId)
        {
            return _iTblOrganizationDAO.SelectExistingAllTblOrganizationByRefIds(orgId, overdueRefId, enqRefId);
        }

        public  TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.SelectTblOrganization(idOrganization, conn, tran);

        }
        public  List<TblOrganizationTO> SelectAllChildOrganizationList(int orgTypeId, int parentId)
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization(orgTypeId, parentId);
        }

        public  List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE)
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization(orgTypeE);
        }

        public  List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization(orgTypeE, conn, tran);
        }

        public  List<DropDownTO> SelectAllOrganizationListForDropDown(Constants.OrgTypeE orgTypeE, TblUserRoleTO userRoleTO)
        {
            return _iTblOrganizationDAO.SelectAllOrganizationListForDropDown(orgTypeE, userRoleTO);
        }

        public  List<DropDownTO> SelectAllSpecialCnfListForDropDown(TblUserRoleTO userRoleTO)
        {
            return _iTblOrganizationDAO.SelectAllSpecialCnfListForDropDown(userRoleTO);
        }

        public  List<DropDownTO> SelectDealerListForDropDown(Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        {
            return _iTblOrganizationDAO.SelectDealerListForDropDown(cnfId, tblUserRoleTO);
        }

        public  List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId)
        {
            return _iTblOrganizationDAO.GetDealerForLoadingDropDownList(cnfId);
        }

        public  Dictionary<int, string> SelectRegisteredMobileNoDCT(String orgIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(orgIds, conn, tran);
        }

        public  Dictionary<int, string> SelectRegisteredMobileNoDCTByOrgType(String orgTypeIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.SelectRegisteredMobileNoDCTByOrgType(orgTypeIds, conn, tran);
        }

        public  List<OrgExportRptTO> SelectAllOrgListToExport(Int32 orgTypeId, Int32 parentId)
        {
            List<OrgExportRptTO> list = _iTblOrganizationDAO.SelectAllOrgListToExport(orgTypeId, parentId);
            if (list != null && orgTypeId == (int)Constants.OrgTypeE.DEALER)
                list = list.OrderBy(a => a.CnfName).ThenBy(d => d.FirmName).ToList();
            else if (list != null)
                list = list.OrderBy(a => a.FirmName).ToList();

            return list;
        }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization list of particular region;
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public  List<TblOrganizationTO> SelectOrganizationListByRegion(Int32 orgTypeId, Int32 districtId)
        {
            return _iTblOrganizationDAO.SelectOrganizationListByRegion(orgTypeId, districtId);
        }

        public  TblOrganizationTO SelectTblOrganizationTOByEnqRefId(String enq_ref_id)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                return _iTblOrganizationDAO.SelectTblOrganizationTOByEnqRefId(enq_ref_id, conn, tran);
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

        #endregion

        #region Insertion
        public  int InsertTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            return _iTblOrganizationDAO.InsertTblOrganization(tblOrganizationTO);
        }

        public  int InsertTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.InsertTblOrganization(tblOrganizationTO, conn, tran);
        }

        public  ResultMessage SaveNewOrganization(TblOrganizationTO tblOrganizationTO)
        {
            ResultMessage rMessage = new StaticStuff.ResultMessage();
            rMessage.MessageType = ResultMessageE.None;
            rMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            Boolean updateOrgYn = false;
            TblPersonTO firstOwnerPersonTO = null;
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
                        // result = BL.TblCompetitorExtBL.InsertTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                        // if (result != 1)
                        // {
                        //     tran.Rollback();
                        //     rMessage.MessageType = ResultMessageE.Error;
                        //     rMessage.Text = "Error While InsertTblCompetitorExt Competitor Brand List in Method SaveNewOrganization";
                        //     return rMessage;
                        // }
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
                            updateOrgYn = true;
                        }

                        result = _iTblPersonBL.InsertTblPerson(personTO, conn, tran);
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

                            // result = DimensionBL.InsertDistrict(districtDimensionTO, conn, tran);
                            // if (result != 1)
                            // {
                            //     tran.Rollback();
                            //     rMessage.MessageType = ResultMessageE.Error;
                            //     rMessage.Text = "Error While InsertDistrict in Method SaveNewOrganization";
                            //     return rMessage;
                            // }
                            addressTO.DistrictId = districtDimensionTO.IdDimension;
                        }

                        if (addressTO.TalukaId == 0 && !string.IsNullOrEmpty(addressTO.TalukaName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO talukaDimensionTO = new CommonDimensionsTO();
                            talukaDimensionTO.ParentId = addressTO.DistrictId;
                            talukaDimensionTO.DimensionName = addressTO.TalukaName;

                            // result = DimensionBL.InsertTaluka(talukaDimensionTO, conn, tran);
                            // if (result != 1)
                            // {
                            //     tran.Rollback();
                            //     rMessage.MessageType = ResultMessageE.Error;
                            //     rMessage.Text = "Error While InsertTaluka in Method SaveNewOrganization";
                            //     return rMessage;
                            // }
                            addressTO.TalukaId = talukaDimensionTO.IdDimension;
                        }

                        result = _iTblAddressBL.InsertTblAddress(addressTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblAddress in Method SaveNewOrganization";
                            return rMessage;
                        }

                        TblOrgAddressTO tblOrgAddressTO = addressTO.GetTblOrgAddressTO();
                        tblOrgAddressTO.OrganizationId = tblOrganizationTO.IdOrganization;

                        // result = TblOrgAddressBL.InsertTblOrgAddress(tblOrgAddressTO, conn, tran);
                        // if (result != 1)
                        // {
                        //     tran.Rollback();
                        //     rMessage.MessageType = ResultMessageE.Error;
                        //     rMessage.Text = "Error While InsertTblOrgAddress in Method SaveNewOrganization";
                        //     return rMessage;
                        // }

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
                        tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                        tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.CreatedBy;
                        tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.CreatedOn;
                        // result = BL.TblOrgLicenseDtlBL.InsertTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                        // if (result != 1)
                        // {
                        //     tran.Rollback();
                        //     rMessage.MessageType = ResultMessageE.Error;
                        //     rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method SaveNewOrganization ";
                        //     return rMessage;
                        // }
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
                    if (tblOrganizationTO.FirstOwnerPersonId > 0)
                    {
                        String userId = _iTblUserBL.CreateUserName(firstOwnerPersonTO.FirstName, firstOwnerPersonTO.LastName, conn, tran);
                        String pwd = Constants.DefaultPassword;

                        TblUserTO userTO = new Models.TblUserTO();
                        userTO.UserLogin = userId;
                        userTO.UserPasswd = pwd;
                        userTO.UserDisplayName = firstOwnerPersonTO.FirstName + " " + firstOwnerPersonTO.LastName;
                        userTO.IsActive = 1;

                        result = _iTblUserBL.InsertTblUser(userTO, conn, tran);

                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblUser for Users in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        TblUserExtTO tblUserExtTO = new TblUserExtTO();
                        tblUserExtTO.AddressId = tblOrganizationTO.AddrId;
                        tblUserExtTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        tblUserExtTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        tblUserExtTO.PersonId = tblOrganizationTO.FirstOwnerPersonId;
                        tblUserExtTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblUserExtTO.UserId = userTO.IdUser;
                        tblUserExtTO.Comments = "New C&F User Created";

                        result = _iTblUserExtBL.InsertTblUserExt(tblUserExtTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblUserExt for Users in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                        tblUserRoleTO.UserId = userTO.IdUser;
                        tblUserRoleTO.RoleId = (int)Constants.SystemRolesE.C_AND_F_AGENT;
                        tblUserRoleTO.IsActive = 1;
                        tblUserRoleTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        tblUserRoleTO.CreatedOn = tblOrganizationTO.CreatedOn;

                        result = _iTblUserRoleBL.InsertTblUserRole(tblUserRoleTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewOrganization ";
                            return rMessage;
                        }
                    }

                    // //Create Default Loading Quota Configurations and its declaration with Zero Values 
                    // List<TblLoadingQuotaConfigTO> loadQCList = BL.TblLoadingQuotaConfigBL.SelectEmptyLoadingQuotaConfig(conn, tran);
                    // if (loadQCList != null)
                    // {
                    //     for (int lc = 0; lc < loadQCList.Count; lc++)
                    //     {
                    //         loadQCList[lc].CnfOrgId = tblOrganizationTO.IdOrganization;
                    //         loadQCList[lc].Remark = "Default Configuration";
                    //         loadQCList[lc].IsActive = 1;
                    //         loadQCList[lc].CreatedBy = tblOrganizationTO.CreatedBy;
                    //         loadQCList[lc].CreatedOn = tblOrganizationTO.CreatedOn;
                    //         result = BL.TblLoadingQuotaConfigBL.InsertTblLoadingQuotaConfig(loadQCList[lc], conn, tran);
                    //         if (result != 1)
                    //         {
                    //             tran.Rollback();
                    //             rMessage.MessageType = ResultMessageE.Error;
                    //             rMessage.Text = "Error While InsertTblLoadingQuotaConfig";
                    //             rMessage.Result = 0;
                    //             return rMessage;
                    //         }
                    //     }


                    //     if (BL.TblLoadingQuotaDeclarationBL.IsLoadingQuotaDeclaredForTheDate(tblOrganizationTO.CreatedOn, conn, tran))
                    //     {
                    //         List<TblLoadingQuotaDeclarationTO> loadQuotaList = BL.TblLoadingQuotaDeclarationBL.SelectLatestCalculatedLoadingQuotaDeclarationList(tblOrganizationTO.CreatedOn, tblOrganizationTO.IdOrganization, conn, tran);

                    //         for (int lq = 0; lq < loadQuotaList.Count; lq++)
                    //         {
                    //             loadQuotaList[lq].CreatedOn = tblOrganizationTO.CreatedOn;
                    //             loadQuotaList[lq].CreatedBy = tblOrganizationTO.CreatedBy;
                    //             loadQuotaList[lq].IsActive = 1;
                    //             loadQuotaList[lq].Remark = "New Default Quota Declaration On Cnf Creation";

                    //             result = BL.TblLoadingQuotaDeclarationBL.InsertTblLoadingQuotaDeclaration(loadQuotaList[lq], conn, tran);
                    //             if (result != 1)
                    //             {
                    //                 tran.Rollback();
                    //                 rMessage.Text = "Error While InsertTblLoadingQuotaDeclaration : SaveNewOrganization";
                    //                 rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    //                 rMessage.MessageType = ResultMessageE.Error;
                    //                 rMessage.Result = 0;
                    //                 return rMessage;
                    //             }
                    //         }
                    //     }
                    // }

                    //Assign Latest Rate Declaration To Him
                    TblQuotaDeclarationTO quotaDeclarationTO = _iTblQuotaDeclarationBL.SelectLatestQuotaDeclarationTO(conn, tran);
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

                        result = _iTblQuotaDeclarationBL.InsertTblQuotaDeclaration(quotaDeclarationTO, conn, tran);
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
                    TblCnfDealersTO tblCnfDealersTO = new TblCnfDealersTO();
                    tblCnfDealersTO.CnfOrgId = tblOrganizationTO.ParentId;
                    tblCnfDealersTO.DealerOrgId = tblOrganizationTO.IdOrganization;
                    tblCnfDealersTO.CreatedBy = tblOrganizationTO.CreatedBy;
                    tblCnfDealersTO.CreatedOn = tblOrganizationTO.CreatedOn;
                    tblCnfDealersTO.IsActive = 1;
                    tblCnfDealersTO.Remark = "Primary C&F";
                    result = _iTblCnfDealersBL.InsertTblCnfDealers(tblCnfDealersTO, conn, tran);
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
                            result = _iTblCnfDealersBL.InsertTblCnfDealers(tblOrganizationTO.CnfDealersTOList[c], conn, tran);
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


        #endregion

        #region Updation
        public  int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            return _iTblOrganizationDAO.UpdateTblOrganization(tblOrganizationTO);
        }

        public  int UpdateTblOrganizationRefIds(TblOrganizationTO tblOrganizationTO)
        {
            return _iTblOrganizationDAO.UpdateTblOrganizationRefIds(tblOrganizationTO);
        }

        public  int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.UpdateTblOrganization(tblOrganizationTO, conn, tran);
        }

        public  ResultMessage UpdateOrganization(TblOrganizationTO tblOrganizationTO)
        {
            ResultMessage rMessage = new StaticStuff.ResultMessage();
            rMessage.MessageType = ResultMessageE.None;
            rMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            Boolean updateOrgYn = false;
            TblPersonTO firstOwnerPersonTO = null;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();

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
                // List<TblCompetitorExtTO> list = BL.TblCompetitorExtBL.SelectAllTblCompetitorExtList(tblOrganizationTO.IdOrganization, conn, tran);
                // if (tblOrganizationTO.CompetitorExtTOList != null)
                // {
                //     for (int b = 0; b < tblOrganizationTO.CompetitorExtTOList.Count; b++)
                //     {
                //         TblCompetitorExtTO existTO = null;

                //         if (list != null)
                //             existTO = list.Where(l => l.IdCompetitorExt == tblOrganizationTO.CompetitorExtTOList[b].IdCompetitorExt).FirstOrDefault();

                //         if (existTO == null)
                //         {
                //             //Insert New Brand
                //             tblOrganizationTO.CompetitorExtTOList[b].OrgId = tblOrganizationTO.IdOrganization;
                //             result = BL.TblCompetitorExtBL.InsertTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                //             if (result != 1)
                //             {
                //                 tran.Rollback();
                //                 rMessage.MessageType = ResultMessageE.Error;
                //                 rMessage.Text = "Error While InsertTblCompetitorExt in Method UpdateTblOrganization";
                //                 return rMessage;
                //             }
                //         }
                //         else
                //         {
                //             //Update existing brand
                //             result = BL.TblCompetitorExtBL.UpdateTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                //             if (result != 1)
                //             {
                //                 tran.Rollback();
                //                 rMessage.MessageType = ResultMessageE.Error;
                //                 rMessage.Text = "Error While UpdateTblCompetitorExt in Method UpdateTblOrganization";
                //                 return rMessage;
                //             }
                //         }
                //     }
                // }

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
                            result = _iTblPersonBL.InsertTblPerson(personTO, conn, tran);
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
                            result = _iTblPersonBL.UpdateTblPerson(personTO, conn, tran);
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

                            // result = DimensionBL.InsertDistrict(districtDimensionTO, conn, tran);
                            // if (result != 1)
                            // {
                            //     tran.Rollback();
                            //     rMessage.MessageType = ResultMessageE.Error;
                            //     rMessage.Text = "Error While InsertDistrict in Method UpdateOrganization";
                            //     return rMessage;
                            // }

                            addressTO.DistrictId = districtDimensionTO.IdDimension;
                        }

                        if (addressTO.TalukaId == 0 && !string.IsNullOrEmpty(addressTO.TalukaName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO talukaDimensionTO = new CommonDimensionsTO();
                            talukaDimensionTO.ParentId = addressTO.DistrictId;
                            talukaDimensionTO.DimensionName = addressTO.TalukaName;

                            // result = DimensionBL.InsertTaluka(talukaDimensionTO, conn, tran);
                            // if (result != 1)
                            // {
                            //     tran.Rollback();
                            //     rMessage.MessageType = ResultMessageE.Error;
                            //     rMessage.Text = "Error While InsertTaluka in Method UpdateOrganization";
                            //     return rMessage;
                            // }

                            addressTO.TalukaId = talukaDimensionTO.IdDimension;

                        }

                        if (addressTO.IdAddr == 0)
                        {
                            result = _iTblAddressBL.InsertTblAddress(addressTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblAddress in Method UpdateOrganization";
                                return rMessage;
                            }

                            TblOrgAddressTO tblOrgAddressTO = addressTO.GetTblOrgAddressTO();
                            tblOrgAddressTO.OrganizationId = tblOrganizationTO.IdOrganization;

                            // result = TblOrgAddressBL.InsertTblOrgAddress(tblOrgAddressTO, conn, tran);
                            // if (result != 1)
                            // {
                            //     tran.Rollback();
                            //     rMessage.MessageType = ResultMessageE.Error;
                            //     rMessage.Text = "Error While InsertTblOrgAddress in Method UpdateOrganization";
                            //     return rMessage;
                            // }

                            if (addressTO.AddressTypeE == Constants.AddressTypeE.OFFICE_ADDRESS)
                            {
                                updateOrgYn = true;
                                tblOrganizationTO.AddrId = addressTO.IdAddr;
                            }
                        }
                        else
                        {
                            result = _iTblAddressBL.UpdateTblAddress(addressTO, conn, tran);
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
                            tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                            tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.UpdatedBy;
                            tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.UpdatedOn;
                            // result = BL.TblOrgLicenseDtlBL.InsertTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                            // if (result != 1)
                            // {
                            //     tran.Rollback();
                            //     rMessage.MessageType = ResultMessageE.Error;
                            //     rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method UpdateOrganization ";
                            //     return rMessage;
                            // }
                        }
                        else
                        {
                            tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                            // result = BL.TblOrgLicenseDtlBL.UpdateTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                            // if (result != 1)
                            // {
                            //     tran.Rollback();
                            //     rMessage.MessageType = ResultMessageE.Error;
                            //     rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method UpdateOrganization ";
                            //     return rMessage;
                            // }
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

                    List<TblCnfDealersTO> exList = _iTblCnfDealersBL.SelectAllActiveCnfDealersList(tblOrganizationTO.IdOrganization, false, conn, tran);
                    if (exList == null)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "C&F and dealer relation not found in Method UpdateOrganization ";
                        return rMessage;
                    }

                    var primaryCnfDelTO = exList.Where(a => a.IsActive == 1 && a.IsSpecialCnf == 0).FirstOrDefault();

                    if (primaryCnfDelTO.CnfOrgId != tblOrganizationTO.ParentId)
                    {
                        //update existing record and set to deactive
                        primaryCnfDelTO.IsActive = 0;
                        result = _iTblCnfDealersBL.UpdateTblCnfDealers(primaryCnfDelTO, conn, tran);
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
                        result = _iTblCnfDealersBL.InsertTblCnfDealers(newTblCnfDealersTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblCnfDealers for Primary C&F in Method UpdateOrganization ";
                            return rMessage;
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
                                result = _iTblCnfDealersBL.InsertTblCnfDealers(tblOrganizationTO.CnfDealersTOList[c], conn, tran);
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
        public  int DeleteTblOrganization(Int32 idOrganization)
        {
            return _iTblOrganizationDAO.DeleteTblOrganization(idOrganization);
        }

        public  int DeleteTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.DeleteTblOrganization(idOrganization, conn, tran);
        }

        #endregion

    }
}
