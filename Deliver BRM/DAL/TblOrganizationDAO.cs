using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;

namespace SalesTrackerAPI.DAL
{
    public class TblOrganizationDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblOrganization.*,cdStructure.cdValue,dimDelPeriod.deliveryPeriod, villageName,districtId,consumerType FROM [tblOrganization] tblOrganization" +
                                  " LEFT JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=tblOrganization.cdStructureId" +
                                   " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=tblOrganization.delPeriodId"+
                                   " LEFT JOIN dimConsumerType dimConsumerType on tblOrganization.consumerTypeid = dimConsumerType.idConsumer";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblOrganizationTO> SelectAllTblOrganization()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static String GetFirmNameByOrgId(Int32 orgId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT firmName FROM tblOrganization where idOrganization = " + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);

                if (rdr != null)
                {
                    while (rdr.Read())
                    {
                        if (rdr["firmName"] != DBNull.Value)
                            return Convert.ToString(rdr["firmName"].ToString());
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrganizationTO> SelectAllTblOrganization(Int32 orgTypeId, Int32 parentId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (parentId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblOrganization.isActive=1 and orgTypeId=" + orgTypeId + " ORDER BY tblOrganization.firmName";
                else
                    cmdSelect.CommandText = SqlSelectQuery()
                                            + " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization " +
                                             " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + orgTypeId + " AND tblCnfDealers.cnfOrgId=" + parentId + " ORDER BY tblOrganization.firmName";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrganizationTO> SelectAllTblOrganizationWithDefaultAddress(Constants.OrgTypeE orgTypeE)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                sqlQuery = " SELECT tblOrganization.*,dimConsumerType.consumerType,addr.* ,tal.talukaName,dist.districtName,stat.stateName  FROM tblOrganization " +
                    " LEFT JOIN dimConsumerType ON tblOrganization.consumerTypeId = dimConsumerType.idConsumer " +
                    " LEFT JOIN tblAddress addr ON tblOrganization.addrId = addr.idAddr " +
                    " LEFT JOIN dimTaluka tal ON tal.idTaluka = addr.talukaId " +
                    " LEFT JOIN dimDistrict dist ON dist.idDistrict = addr.districtId " +
                    " LEFT JOIN dimState stat ON stat.idState = addr.stateId " +
                    " WHERE tblOrganization.orgTypeId=" + (int)orgTypeE;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrganizationTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblOrganizationTO> tblOrganizationTOList = new List<TblOrganizationTO>();
                if (tblOrganizationTODT != null)
                {
                    while (tblOrganizationTODT.Read())
                    {
                        TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                        if (tblOrganizationTODT["idOrganization"] != DBNull.Value)
                            tblOrganizationTONew.IdOrganization = Convert.ToInt32(tblOrganizationTODT["idOrganization"].ToString());
                        if (tblOrganizationTODT["orgTypeId"] != DBNull.Value)
                            tblOrganizationTONew.OrgTypeId = Convert.ToInt32(tblOrganizationTODT["orgTypeId"].ToString());
                        if (tblOrganizationTODT["addrId"] != DBNull.Value)
                            tblOrganizationTONew.AddrId = Convert.ToInt32(tblOrganizationTODT["addrId"].ToString());
                        if (tblOrganizationTODT["firstOwnerPersonId"] != DBNull.Value)
                            tblOrganizationTONew.FirstOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["firstOwnerPersonId"].ToString());
                        if (tblOrganizationTODT["secondOwnerPersonId"] != DBNull.Value)
                            tblOrganizationTONew.SecondOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["secondOwnerPersonId"].ToString());
                        if (tblOrganizationTODT["parentId"] != DBNull.Value)
                            tblOrganizationTONew.ParentId = Convert.ToInt32(tblOrganizationTODT["parentId"].ToString());
                        if (tblOrganizationTODT["createdBy"] != DBNull.Value)
                            tblOrganizationTONew.CreatedBy = Convert.ToInt32(tblOrganizationTODT["createdBy"].ToString());
                        if (tblOrganizationTODT["createdOn"] != DBNull.Value)
                            tblOrganizationTONew.CreatedOn = Convert.ToDateTime(tblOrganizationTODT["createdOn"].ToString());
                        if (tblOrganizationTODT["firmName"] != DBNull.Value)
                            tblOrganizationTONew.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString());
                        if (tblOrganizationTODT["consumerType"] != DBNull.Value)
                            tblOrganizationTONew.ConsumerType = Convert.ToString(tblOrganizationTODT["consumerType"].ToString());

                        if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                            tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                        if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                            tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                        if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                            tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                        if (tblOrganizationTODT["website"] != DBNull.Value)
                            tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());

                        if (tblOrganizationTODT["registeredMobileNos"] != DBNull.Value)
                            tblOrganizationTONew.RegisteredMobileNos = Convert.ToString(tblOrganizationTODT["registeredMobileNos"].ToString());

                        if (tblOrganizationTODT["cdStructureId"] != DBNull.Value)
                            tblOrganizationTONew.CdStructureId = Convert.ToInt32(tblOrganizationTODT["cdStructureId"].ToString());
                        if (tblOrganizationTODT["delPeriodId"] != DBNull.Value)
                            tblOrganizationTONew.DelPeriodId = Convert.ToInt32(tblOrganizationTODT["delPeriodId"].ToString());

                        if (tblOrganizationTODT["isActive"] != DBNull.Value)
                            tblOrganizationTONew.IsActive = Convert.ToInt32(tblOrganizationTODT["isActive"].ToString());
                        if (tblOrganizationTODT["remark"] != DBNull.Value)
                            tblOrganizationTONew.Remark = Convert.ToString(tblOrganizationTODT["remark"].ToString());

                        if (tblOrganizationTODT["isSpecialCnf"] != DBNull.Value)
                            tblOrganizationTONew.IsSpecialCnf = Convert.ToInt32(tblOrganizationTODT["isSpecialCnf"].ToString());

                        if (tblOrganizationTODT["digitalSign"] != DBNull.Value)
                            tblOrganizationTONew.DigitalSign = Convert.ToString(tblOrganizationTODT["digitalSign"].ToString());
                        if (tblOrganizationTODT["deactivatedOn"] != DBNull.Value)
                            tblOrganizationTONew.DeactivatedOn = Convert.ToDateTime(tblOrganizationTODT["deactivatedOn"].ToString());


                        if (tblOrganizationTODT["orgLogo"] != DBNull.Value)
                            tblOrganizationTONew.OrgLogo = Convert.ToString(tblOrganizationTODT["orgLogo"].ToString());

                        if (tblOrganizationTODT["firmTypeId"] != DBNull.Value)
                            tblOrganizationTONew.FirmTypeId = Convert.ToInt32(tblOrganizationTODT["firmTypeId"].ToString());

                        if (tblOrganizationTODT["influencerTypeId"] != DBNull.Value)
                            tblOrganizationTONew.InfluencerTypeId = Convert.ToInt32(tblOrganizationTODT["influencerTypeId"].ToString());

                        //Sudhir[22-APR-2018]
                        if (tblOrganizationTODT["dateOfEstablishment"] != DBNull.Value)
                            tblOrganizationTONew.DateOfEstablishment = Convert.ToDateTime(tblOrganizationTODT["dateOfEstablishment"].ToString());

                        if (tblOrganizationTODT["firmTypeId"] != DBNull.Value)
                            tblOrganizationTONew.FirmTypeId = Convert.ToInt32(tblOrganizationTODT["firmTypeId"].ToString());

                        if (tblOrganizationTODT["influencerTypeId"] != DBNull.Value)
                            tblOrganizationTONew.InfluencerTypeId = Convert.ToInt32(tblOrganizationTODT["influencerTypeId"].ToString());

                        //Priyanka [14-02-2019]
                        if (tblOrganizationTODT["overdueRefId"] != DBNull.Value)
                            tblOrganizationTONew.OverdueRefId = Convert.ToString(tblOrganizationTODT["overdueRefId"].ToString());

                        if (tblOrganizationTODT["enqRefId"] != DBNull.Value)
                            tblOrganizationTONew.EnqRefId = Convert.ToString(tblOrganizationTODT["enqRefId"].ToString());
                        if (tblOrganizationTODT["isInternalCnf"] != DBNull.Value)
                            tblOrganizationTONew.IsInternalCnf = Convert.ToInt32(tblOrganizationTODT["isInternalCnf"].ToString());

                        if (tblOrganizationTODT["isTcsApplicable"] != DBNull.Value)
                            tblOrganizationTONew.IsTcsApplicable = Convert.ToInt32(tblOrganizationTODT["isTcsApplicable"].ToString());
                        #region Address TO
                        TblAddressTO tblAddressTONew = new TblAddressTO();
                        if (tblOrganizationTODT["idAddr"] != DBNull.Value)
                            tblAddressTONew.IdAddr = Convert.ToInt32(tblOrganizationTODT["idAddr"].ToString());
                        if (tblOrganizationTODT["talukaId"] != DBNull.Value)
                            tblAddressTONew.TalukaId = Convert.ToInt32(tblOrganizationTODT["talukaId"].ToString());
                        if (tblOrganizationTODT["districtId"] != DBNull.Value)
                            tblAddressTONew.DistrictId = Convert.ToInt32(tblOrganizationTODT["districtId"].ToString());
                        if (tblOrganizationTODT["stateId"] != DBNull.Value)
                            tblAddressTONew.StateId = Convert.ToInt32(tblOrganizationTODT["stateId"].ToString());
                        if (tblOrganizationTODT["countryId"] != DBNull.Value)
                            tblAddressTONew.CountryId = Convert.ToInt32(tblOrganizationTODT["countryId"].ToString());
                        if (tblOrganizationTODT["pincode"] != DBNull.Value)
                            tblAddressTONew.Pincode = Convert.ToInt32(tblOrganizationTODT["pincode"].ToString());
                       
                        if (tblOrganizationTODT["plotNo"] != DBNull.Value)
                            tblAddressTONew.PlotNo = Convert.ToString(tblOrganizationTODT["plotNo"].ToString());
                        if (tblOrganizationTODT["streetName"] != DBNull.Value)
                            tblAddressTONew.StreetName = Convert.ToString(tblOrganizationTODT["streetName"].ToString());
                        if (tblOrganizationTODT["areaName"] != DBNull.Value)
                            tblAddressTONew.AreaName = Convert.ToString(tblOrganizationTODT["areaName"].ToString());
                        if (tblOrganizationTODT["villageName"] != DBNull.Value)
                            tblAddressTONew.VillageName = Convert.ToString(tblOrganizationTODT["villageName"].ToString());
                        if (tblOrganizationTODT["comments"] != DBNull.Value)
                            tblAddressTONew.Comments = Convert.ToString(tblOrganizationTODT["comments"].ToString());
                        if (tblOrganizationTODT["talukaName"] != DBNull.Value)
                            tblAddressTONew.TalukaName = Convert.ToString(tblOrganizationTODT["talukaName"].ToString());
                        if (tblOrganizationTODT["districtName"] != DBNull.Value)
                            tblAddressTONew.DistrictName = Convert.ToString(tblOrganizationTODT["districtName"].ToString());
                        if (tblOrganizationTODT["stateName"] != DBNull.Value)
                            tblAddressTONew.StateName = Convert.ToString(tblOrganizationTODT["stateName"].ToString());
                        #endregion
                        tblOrganizationTONew.AddressList = new List<TblAddressTO>();
                        tblOrganizationTONew.AddressList.Add(tblAddressTONew);

                        tblOrganizationTOList.Add(tblOrganizationTONew);
                    }

                }
                tblOrganizationTODT.Dispose();
                return tblOrganizationTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllTblOrganization at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<TblOrganizationTO> SelectAllTblOrganization(Constants.OrgTypeE orgTypeE)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                if (orgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                    sqlQuery = " SELECT cnFInfo.* ,cdStructure.cdValue,dimDelPeriod.deliveryPeriod,villageName,addrDtl.districtId, existingRate.idQuotaDeclaration,rate , existingRate.alloc_qty,existingRate.rate_band ,balance_qty,validUpto" +
                               " FROM tblOrganization cnFInfo " +
                               " LEFT JOIN " +
                               " ( " +
                               "     SELECT main.idQuotaDeclaration, rate, main.orgId, main.quotaAllocDate, alloc_qty, rate_band,balance_qty,validUpto " +
                               "     FROM tblQuotaDeclaration main " +
                               "    INNER JOIN " +
                               "       (SELECT orgId, max(quotaAllocDate) quotaAllocDate " +
                               "        FROM tblQuotaDeclaration " +
                               "         group by orgId) RESULT " +
                               "         ON main.orgId = RESULT.orgId and main.quotaAllocDate = RESULT.quotaAllocDate " +
                               "    INNER JOIN tblGlobalRate ON tblGlobalRate.idGlobalRate=main.globalRateId  " +
                               " ) AS existingRate " +
                               " ON cnFInfo.idOrganization = existingRate.orgId " +
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = addrDtl.organizationId " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=cnFInfo.cdStructureId" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=cnFInfo.delPeriodId" +
                               " WHERE  cnFInfo.isActive=1 AND cnFInfo.orgTypeId=" + (int)orgTypeE;

                else if (orgTypeE == Constants.OrgTypeE.COMPETITOR)
                {
                    sqlQuery = " SELECT * FROM tblOrganization compeInfo " +
                               " LEFT JOIN ( " +
                               "  SELECT result.* FROM( " +
                               "  SELECT competitorOrgId, max(updateDatetime) updateDatetime " +
                               "  FROM( " +
                               "  SELECT competitorOrgId , compUpdate.updateDatetime, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   ) as res group by competitorOrgId " +
                               "   ) AS main " +
                               "   inner join " +
                               "   ( " +
                               "   SELECT competitorExtId,competitorOrgId ,brandName, prodCapacityMT, compUpdate.updateDatetime, compUpdate.informerName, alternateInformerName ,compUpdate.price, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   INNER JOIN tblCompetitorExt comptExt ON comptExt.idCompetitorExt=compUpdate.competitorExtId" +
                               "   ) result " +
                               "   on main.competitorOrgId = result.competitorOrgId " +
                               "   AND main.updateDatetime = result.updateDatetime " +
                               "   ) AS compUpdate " +
                               "   ON compeInfo.idOrganization = compUpdate.competitorOrgId" +
                                " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON compeInfo.idOrganization = addrDtl.organizationId " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=compeInfo.cdStructureId" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=compeInfo.delPeriodId" +
                               " WHERE  compeInfo.isActive=1 AND compeInfo.orgTypeId=" + (int)orgTypeE +
                               " ORDER BY updateDatetime DESC";
                }

                else sqlQuery = SqlSelectQuery() + " WHERE  tblOrganization.isActive=1 AND orgTypeId=" + (int)orgTypeE;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrganizationTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllTblOrganization at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrganizationTO> SelectAllTblOrganization(Constants.OrgTypeE orgTypeE, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            SqlDataReader rdr = null;
            try
            {

                if (orgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                    sqlQuery = " SELECT cnFInfo.* ,cdStructure.cdValue,dimDelPeriod.deliveryPeriod,villageName,addrDtl.districtId,existingRate.idQuotaDeclaration,rate , existingRate.alloc_qty,existingRate.rate_band ,balance_qty,validUpto" +
                               " FROM tblOrganization cnFInfo " +
                               " LEFT JOIN " +
                               " ( " +
                               "     SELECT main.idQuotaDeclaration, rate, main.orgId, main.quotaAllocDate, alloc_qty, rate_band,balance_qty,validUpto " +
                               "     FROM tblQuotaDeclaration main " +
                               "    INNER JOIN " +
                               "       (SELECT orgId, max(quotaAllocDate) quotaAllocDate " +
                               "        FROM tblQuotaDeclaration " +
                               "         group by orgId) RESULT " +
                               "         ON main.orgId = RESULT.orgId and main.quotaAllocDate = RESULT.quotaAllocDate " +
                               "    INNER JOIN tblGlobalRate ON tblGlobalRate.idGlobalRate=main.globalRateId  " +
                               " ) AS existingRate " +
                               " ON cnFInfo.idOrganization = existingRate.orgId " +
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = addrDtl.organizationId " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=cnFInfo.cdStructureId" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=cnFInfo.delPeriodId" +
                               " WHERE  cnFInfo.isActive=1 AND cnFInfo.orgTypeId=" + (int)orgTypeE;

                else if (orgTypeE == Constants.OrgTypeE.COMPETITOR)
                {
                    sqlQuery = " SELECT * FROM tblOrganization compeInfo " +
                               " LEFT JOIN ( " +
                               "  SELECT result.* FROM( " +
                               "  SELECT competitorOrgId, max(updateDatetime) updateDatetime " +
                               "  FROM( " +
                               "  SELECT competitorOrgId , compUpdate.updateDatetime, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   ) as res group by competitorOrgId " +
                               "   ) AS main " +
                               "   inner join " +
                               "   ( " +
                               "   SELECT competitorExtId,competitorOrgId , brandName, prodCapacityMT,compUpdate.updateDatetime, compUpdate.informerName, alternateInformerName ,compUpdate.price, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   INNER JOIN tblCompetitorExt comptExt ON comptExt.idCompetitorExt=compUpdate.competitorExtId" +
                               "   ) result " +
                               "   on main.competitorOrgId = result.competitorOrgId " +
                               "   AND main.updateDatetime = result.updateDatetime " +
                               "   ) AS compUpdate " +
                               "   ON compeInfo.idOrganization = compUpdate.competitorOrgId" +
                                " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON compeInfo.idOrganization = addrDtl.organizationId " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=compeInfo.cdStructureId" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=compeInfo.delPeriodId" +
                               " WHERE   compeInfo.isActive=1 AND compeInfo.orgTypeId=" + (int)orgTypeE;
                }

                else sqlQuery = SqlSelectQuery() + " WHERE  tblOrganization.isActive=1 AND orgTypeId=" + (int)orgTypeE;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrganizationTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> SelectAllOrganizationListForDropDown(Constants.OrgTypeE orgTypeE, TblUserRoleTO userRoleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                if (orgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                {
                    int isConfEn = 0;
                    if (userRoleTO != null)
                        isConfEn = userRoleTO.EnableAreaAlloc;

                    if (isConfEn == 1)
                        sqlQuery = " SELECT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS firmName, isSpecialCnf FROM tblOrganization" +
                                    " LEFT JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " WHERE  orgTypeId=" + (int)orgTypeE +
                                   " AND idOrganization IN(SELECT cnfOrgId FROM tblUserAreaAllocation WHERE userId=" + userRoleTO.UserId + " AND isActive=1 ) " +
                                   " AND isActive=1";
                    else
                        sqlQuery = " SELECT idOrganization,CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS firmName ,isSpecialCnf FROM tblOrganization " +
                                    " LEFT JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                    " WHERE isActive=1 AND  orgTypeId=" + (int)orgTypeE;

                }
                else
                    //sqlQuery = " SELECT idOrganization,firmName , isSpecialCnf FROM tblOrganization WHERE  isActive=1 AND orgTypeId=" + (int)orgTypeE;
                    sqlQuery = " SELECT idOrganization,CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS firmName, isSpecialCnf FROM tblOrganization " +
                                    " LEFT JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                    " WHERE isActive=1 AND  orgTypeId=" + (int)orgTypeE;


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["firmName"].ToString());
                        if (tblOrgReader["isSpecialCnf"] != DBNull.Value)
                            dropDownTO.Tag = "isSpecialCnf : " + Convert.ToString(tblOrgReader["isSpecialCnf"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> SelectAllSpecialCnfListForDropDown(TblUserRoleTO userRoleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            Constants.OrgTypeE orgTypeE = Constants.OrgTypeE.C_AND_F_AGENT;
            try
            {
                conn.Open();


                sqlQuery = " SELECT idOrganization,firmName,isSpecialCnf FROM tblOrganization" +
                           " WHERE  orgTypeId=" + (int)orgTypeE +
                           " AND isActive=1 AND isSpecialCnf=1";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["firmName"].ToString());
                        if (tblOrgReader["isSpecialCnf"] != DBNull.Value)
                            dropDownTO.Tag = "isSpecialCnf : " + Convert.ToString(tblOrgReader["isSpecialCnf"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllSpecialCnfListForDropDown at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Prajakta[2020-03-16] Added to get dealer list as per last txn date
        public static List<DropDownTO> SelectDealerDropDownListAsPerLastTxn(Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            //TblUserRoleTO userRoleTO=BL.TblUserRoleBL.SelectTblUserRoleTO()
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();

                if (cnfId > 0)
                {
                    if (isConfEn == 0)
                    {
                        //sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                        //           " ELSE firmName +',' + villageName END AS dealerName " +
                        //           " FROM tblOrganization " +
                        //           " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                        //           " INNER JOIN " +
                        //           " ( " +
                        //           " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                        //           " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                        //           " ) addrDtl " +
                        //           " ON idOrganization = organizationId WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId;

                        sqlQuery = " SELECT DISTINCT idOrganization,tblBooking.latestBookingTime, CASE WHEN villageName IS NULL then  firmName " +
                                       " ELSE firmName +',' + villageName END AS dealerName " +
                                       " FROM tblOrganization " +
                                       " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                       " INNER JOIN " +
                                       " ( " +
                                       " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                       " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                       " ) addrDtl " +
                                       " ON idOrganization = organizationId " +
                                       " left join  (select dealerOrgId,MAX(bookingDatetime) as latestBookingTime from tblBookings where cnFOrgId = " + cnfId +
                                       " group by dealerOrgId) tblBooking on tblBooking.dealerOrgId = tblOrganization.idOrganization " +
                                       " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId +
                                       " ORDER BY latestBookingTime desc ";

                    }
                    else
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization,tblBooking.latestBookingTime, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   " left join  (select dealerOrgId,MAX(bookingDatetime) as latestBookingTime from tblBookings where cnFOrgId = " + cnfId +
                                   " group by dealerOrgId) tblBooking on tblBooking.dealerOrgId = tblOrganization.idOrganization " +
                                   " WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.cnfOrgId=" + cnfId + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 " +
                                   " ORDER BY latestBookingTime desc "; 

                    }
                }
                else
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization,tblBooking.latestBookingTime, CASE WHEN villageName IS NULL then  firmName " +
                               " ELSE firmName +',' + villageName END AS dealerName " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                               " INNER JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId " +
                               " left join  (select dealerOrgId,MAX(bookingDatetime) as latestBookingTime from tblBookings where cnFOrgId = " + cnfId +
                               " group by dealerOrgId) tblBooking on tblBooking.dealerOrgId = tblOrganization.idOrganization " +
                               " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER +
                               " ORDER BY latestBookingTime desc "; 
                    }
                    else
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization,tblBooking.latestBookingTime, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   " left join  (select dealerOrgId,MAX(bookingDatetime) as latestBookingTime from tblBookings where cnFOrgId = " + cnfId +
                                   " group by dealerOrgId) tblBooking on tblBooking.dealerOrgId = tblOrganization.idOrganization " +
                                   " WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 " +
                                   " ORDER BY latestBookingTime desc "; ;

                    }
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["dealerName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["dealerName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", cnfId);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> SelectDealerListForDropDown(Int32 cnfId, TblUserRoleTO tblUserRoleTO, Int32 consumerType = 0)
        {
            
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            //TblUserRoleTO userRoleTO=BL.TblUserRoleBL.SelectTblUserRoleTO()
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();

                if (cnfId > 0)
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId;
                    }
                    else
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.cnfOrgId=" + cnfId + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }
                else
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                               " ELSE firmName +',' + villageName END AS dealerName " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                               " INNER JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId " +
                               " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER;
                    }
                    else
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }
                if(consumerType != 0)
                {
                    sqlQuery += " AND consumerTypeId = " + consumerType;
                }
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["dealerName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["dealerName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", cnfId);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
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
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            //TblUserRoleTO userRoleTO=BL.TblUserRoleBL.SelectTblUserRoleTO()
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();

                if (cnfId > 0)
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId;
                    }
                    else
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.cnfOrgId=" + cnfId + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }
                else
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT idOrganization,CASE WHEN villageName IS NULL then  firmName ELSE firmName + ',' + villageName END " +
                                  " AS dealerName, isSpecialCnf FROM tblOrganization LEFT JOIN(SELECT tblAddress.*, organizationId FROM tblOrgAddress  " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1) addrDtl " +
                                  " ON idOrganization = organizationId  WHERE isActive = 1 AND orgTypeId = 2";
                    }
                    else
                    {
                        //sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                        //           " ELSE firmName +',' + villageName END AS dealerName " +
                        //           " FROM tblOrganization " +
                        //           " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                        //           " INNER JOIN " +
                        //           " ( " +
                        //           " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                        //           " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                        //           " ) addrDtl " +
                        //           " ON idOrganization = organizationId " +
                        //           " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                        //           " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                        //           "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                        sqlQuery = " SELECT DISTINCT idOrganization, " +
                                  " CASE WHEN villageName IS NULL then firmName  ELSE firmName +',' + villageName END AS dealerName " +
                                  " FROM tblOrganization " +
                                  " INNER JOIN   (    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1) addrDtl ON idOrganization = organizationId " +
                                  " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId " +
                                  " WHERE tblOrganization.isActive = 1  AND orgTypeId =" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + " AND areaConf.isActive = 1 ";

                    }
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["dealerName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["dealerName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", cnfId);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            String innerQuery = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            string areaConf = string.Empty;
            string whereCond = string.Empty;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                conn.Open();

                if (isConfEn == 1)
                {
                    areaConf = " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                     " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId ";
                    whereCond = " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";
                }
                if (cnfId > 0)
                {
                    sqlQuery = " SELECT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                               " ELSE firmName +',' + villageName END AS dealerName " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId "
                               + areaConf +
                               "WHERE  orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND tblCnfDealers.cnfOrgId=" + cnfId + whereCond +
                               " AND tblOrganization.idOrganization IN(SELECT distinct dealerOrgId FROM tblBookings WHERE cnFOrgId=" + cnfId + " AND pendingQty > 0 AND statusId IN(2,3,9,11))";
                }
                else
                    sqlQuery = " SELECT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                               " ELSE firmName +',' + villageName END AS dealerName " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId "
                               + areaConf +
                               "WHERE  orgTypeId=" + (int)Constants.OrgTypeE.DEALER + whereCond +
                               " AND tblOrganization.idOrganization IN(SELECT distinct dealerOrgId FROM tblBookings WHERE pendingQty > 0 AND statusId IN(2,3,9,11))";


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["dealerName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["dealerName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblOrganizationTO SelectTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrganization = " + idOrganization + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //Prajakta[2021-03-05] As ConvertDTToList is not work for common sql query
                //List<TblOrganizationTO> list = ConvertDTToList(rdr);
                List<TblOrganizationTO> list = SimpleConvertDTtoList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblOrganization");
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<int, string> SelectRegisteredMobileNoDCT(String orgIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            try
            {
                cmdSelect.CommandText = "SELECT idOrganization , registeredMobileNos FROM tblOrganization  WHERE idOrganization IN(" + orgIds + " ) ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 orgId = 0;
                        string regMobNos = string.Empty;
                        if (rdr["idOrganization"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["idOrganization"].ToString());
                        if (rdr["registeredMobileNos"] != DBNull.Value)
                            regMobNos = Convert.ToString(rdr["registeredMobileNos"].ToString());

                        if (orgId > 0 && !string.IsNullOrEmpty(regMobNos))
                        {
                            DCT.Add(orgId, regMobNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<int, string> SelectRegisteredMobileNoDCTByOrgType(String orgTypeId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            try
            {
                cmdSelect.CommandText = "SELECT idOrganization , registeredMobileNos FROM tblOrganization  WHERE isActive=1 AND orgTypeId IN(" + orgTypeId + " ) ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 orgId = 0;
                        string regMobNos = string.Empty;
                        if (rdr["idOrganization"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["idOrganization"].ToString());
                        if (rdr["registeredMobileNos"] != DBNull.Value)
                            regMobNos = Convert.ToString(rdr["registeredMobileNos"].ToString());

                        if (orgId > 0 && !string.IsNullOrEmpty(regMobNos))
                        {
                            DCT.Add(orgId, regMobNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static String SelectFirmNameOfOrganiationById(Int32 organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT firmName FROM tblOrganization WHERE idOrganization=" + organizationId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (rdr.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    if (rdr["firmName"] != DBNull.Value)
                        return Convert.ToString(rdr["firmName"].ToString());
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static Int32 SelectDefaultAddrOfOrganiationById(Int32 organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT addrId FROM tblOrganization WHERE idOrganization=" + organizationId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (rdr.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    if (rdr["addrId"] != DBNull.Value)
                        return Convert.ToInt32(rdr["addrId"].ToString());
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<TblOrganizationTO> SelectAllOrganizationListV2(Int32 orgType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT* FROM[tblOrganization] WHERE orgTypeId=" + orgType + " AND isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return SimpleConvertDTtoList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrganizationTO> ConvertDTToList(SqlDataReader tblOrganizationTODT)
        {
            List<TblOrganizationTO> tblOrganizationTOList = new List<TblOrganizationTO>();
            if (tblOrganizationTODT != null)
            {
                while (tblOrganizationTODT.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    if (tblOrganizationTODT["idOrganization"] != DBNull.Value)
                        tblOrganizationTONew.IdOrganization = Convert.ToInt32(tblOrganizationTODT["idOrganization"].ToString());
                    if (tblOrganizationTODT["orgTypeId"] != DBNull.Value)
                        tblOrganizationTONew.OrgTypeId = Convert.ToInt32(tblOrganizationTODT["orgTypeId"].ToString());
                    if (tblOrganizationTODT["addrId"] != DBNull.Value)
                        tblOrganizationTONew.AddrId = Convert.ToInt32(tblOrganizationTODT["addrId"].ToString());
                    if (tblOrganizationTODT["firstOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.FirstOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["firstOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["secondOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.SecondOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["secondOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["parentId"] != DBNull.Value)
                        tblOrganizationTONew.ParentId = Convert.ToInt32(tblOrganizationTODT["parentId"].ToString());
                    if (tblOrganizationTODT["createdBy"] != DBNull.Value)
                        tblOrganizationTONew.CreatedBy = Convert.ToInt32(tblOrganizationTODT["createdBy"].ToString());
                    if (tblOrganizationTODT["createdOn"] != DBNull.Value)
                        tblOrganizationTONew.CreatedOn = Convert.ToDateTime(tblOrganizationTODT["createdOn"].ToString());
                    if (tblOrganizationTODT["firmName"] != DBNull.Value)
                        tblOrganizationTONew.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString());

                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());

                    if (tblOrganizationTODT["registeredMobileNos"] != DBNull.Value)
                        tblOrganizationTONew.RegisteredMobileNos = Convert.ToString(tblOrganizationTODT["registeredMobileNos"].ToString());

                    if (tblOrganizationTODT["cdStructureId"] != DBNull.Value)
                        tblOrganizationTONew.CdStructureId = Convert.ToInt32(tblOrganizationTODT["cdStructureId"].ToString());
                    if (tblOrganizationTODT["cdValue"] != DBNull.Value)
                        tblOrganizationTONew.CdStructure = Convert.ToDouble(tblOrganizationTODT["cdValue"].ToString());
                    if (tblOrganizationTODT["delPeriodId"] != DBNull.Value)
                        tblOrganizationTONew.DelPeriodId = Convert.ToInt32(tblOrganizationTODT["delPeriodId"].ToString());
                    if (tblOrganizationTODT["deliveryPeriod"] != DBNull.Value)
                        tblOrganizationTONew.DeliveryPeriod = Convert.ToInt32(tblOrganizationTODT["deliveryPeriod"].ToString());

                    if (tblOrganizationTODT["isActive"] != DBNull.Value)
                        tblOrganizationTONew.IsActive = Convert.ToInt32(tblOrganizationTODT["isActive"].ToString());
                    if (tblOrganizationTODT["remark"] != DBNull.Value)
                        tblOrganizationTONew.Remark = Convert.ToString(tblOrganizationTODT["remark"].ToString());
                    if (tblOrganizationTODT["villageName"] != DBNull.Value)
                        tblOrganizationTONew.VillageName = Convert.ToString(tblOrganizationTODT["villageName"].ToString());
                    if (tblOrganizationTODT["isSpecialCnf"] != DBNull.Value)
                        tblOrganizationTONew.IsSpecialCnf = Convert.ToInt32(tblOrganizationTODT["isSpecialCnf"].ToString());

                    if (tblOrganizationTODT["digitalSign"] != DBNull.Value)
                        tblOrganizationTONew.DigitalSign = Convert.ToString(tblOrganizationTODT["digitalSign"].ToString());
                    if (tblOrganizationTODT["deactivatedOn"] != DBNull.Value)
                        tblOrganizationTONew.DeactivatedOn = Convert.ToDateTime(tblOrganizationTODT["deactivatedOn"].ToString());

                    if (tblOrganizationTODT["districtId"] != DBNull.Value)
                        tblOrganizationTONew.DistrictId = Convert.ToInt32(tblOrganizationTODT["districtId"].ToString());

                    if (tblOrganizationTODT["orgLogo"] != DBNull.Value)
                        tblOrganizationTONew.OrgLogo = Convert.ToString(tblOrganizationTODT["orgLogo"].ToString());



                    if (tblOrganizationTONew.OrgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                    {
                        if (tblOrganizationTODT["alloc_qty"] != DBNull.Value)
                            tblOrganizationTONew.LastAllocQty = Convert.ToDouble(tblOrganizationTODT["alloc_qty"].ToString());
                        if (tblOrganizationTODT["rate_band"] != DBNull.Value)
                            tblOrganizationTONew.LastRateBand = Convert.ToDouble(tblOrganizationTODT["rate_band"].ToString());
                        if (tblOrganizationTODT["balance_qty"] != DBNull.Value)
                            tblOrganizationTONew.BalanceQuota = Convert.ToDouble(tblOrganizationTODT["balance_qty"].ToString());
                        if (tblOrganizationTODT["validUpto"] != DBNull.Value)
                            tblOrganizationTONew.ValidUpto = Convert.ToInt32(tblOrganizationTODT["validUpto"].ToString());
                        if (tblOrganizationTODT["idQuotaDeclaration"] != DBNull.Value)
                            tblOrganizationTONew.QuotaDeclarationId = Convert.ToInt32(tblOrganizationTODT["idQuotaDeclaration"].ToString());
                        if (tblOrganizationTODT["rate"] != DBNull.Value)
                            tblOrganizationTONew.DeclaredRate = Convert.ToDouble(tblOrganizationTODT["rate"].ToString());

                    }

                    if (tblOrganizationTONew.OrgTypeE == Constants.OrgTypeE.COMPETITOR)
                    {
                        tblOrganizationTONew.CompetitorUpdatesTO = new Models.TblCompetitorUpdatesTO();
                        if (tblOrganizationTODT["updateDatetime"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.UpdateDatetime = Convert.ToDateTime(tblOrganizationTODT["updateDatetime"].ToString());
                        if (tblOrganizationTODT["price"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.Price = Convert.ToDouble(tblOrganizationTODT["price"].ToString());
                        if (tblOrganizationTODT["lastPrice"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.LastPrice = Convert.ToDouble(tblOrganizationTODT["lastPrice"].ToString());
                        if (tblOrganizationTODT["firmName"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString());
                        if (tblOrganizationTODT["informerName"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.InformerName = Convert.ToString(tblOrganizationTODT["informerName"].ToString());
                        if (tblOrganizationTODT["alternateInformerName"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.AlternateInformerName = Convert.ToString(tblOrganizationTODT["alternateInformerName"].ToString());

                        if (tblOrganizationTODT["brandName"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.BrandName = Convert.ToString(tblOrganizationTODT["brandName"].ToString());
                        if (tblOrganizationTODT["prodCapacityMT"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.ProdCapacityMT = Convert.ToDouble(tblOrganizationTODT["prodCapacityMT"].ToString());
                        if (tblOrganizationTODT["competitorExtId"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.CompetitorExtId = Convert.ToInt32(tblOrganizationTODT["competitorExtId"].ToString());
                        if (tblOrganizationTODT["competitorOrgId"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.CompetitorOrgId = Convert.ToInt32(tblOrganizationTODT["competitorOrgId"].ToString());


                    }
                    if (tblOrganizationTODT["firmTypeId"] != DBNull.Value)
                        tblOrganizationTONew.FirmTypeId = Convert.ToInt32(tblOrganizationTODT["firmTypeId"].ToString());

                    if (tblOrganizationTODT["influencerTypeId"] != DBNull.Value)
                        tblOrganizationTONew.InfluencerTypeId = Convert.ToInt32(tblOrganizationTODT["influencerTypeId"].ToString());

                    //Sudhir[22-APR-2018]
                    if (tblOrganizationTODT["dateOfEstablishment"] != DBNull.Value)
                        tblOrganizationTONew.DateOfEstablishment = Convert.ToDateTime(tblOrganizationTODT["dateOfEstablishment"].ToString());

                    //Priyanka [14-02-2019]
                    if (tblOrganizationTODT["overdueRefId"] != DBNull.Value)
                        tblOrganizationTONew.OverdueRefId = Convert.ToString(tblOrganizationTODT["overdueRefId"].ToString());

                    if (tblOrganizationTODT["enqRefId"] != DBNull.Value)
                        tblOrganizationTONew.EnqRefId = Convert.ToString(tblOrganizationTODT["enqRefId"].ToString());
                    if(tblOrganizationTODT["isInternalCnf"] !=  DBNull.Value)
                        tblOrganizationTONew.IsInternalCnf = Convert.ToInt32(tblOrganizationTODT["isInternalCnf"].ToString());

                    tblOrganizationTOList.Add(tblOrganizationTONew);
                }
            }
            return tblOrganizationTOList;
        }

        public static List<TblOrganizationTO> SimpleConvertDTtoList(SqlDataReader tblOrganizationTODT)
        {
            List<TblOrganizationTO> tblOrganizationTOList = new List<TblOrganizationTO>();
            if (tblOrganizationTODT != null)
            {
                while (tblOrganizationTODT.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    if (tblOrganizationTODT["idOrganization"] != DBNull.Value)
                        tblOrganizationTONew.IdOrganization = Convert.ToInt32(tblOrganizationTODT["idOrganization"].ToString());
                    if (tblOrganizationTODT["orgTypeId"] != DBNull.Value)
                        tblOrganizationTONew.OrgTypeId = Convert.ToInt32(tblOrganizationTODT["orgTypeId"].ToString());
                    if (tblOrganizationTODT["addrId"] != DBNull.Value)
                        tblOrganizationTONew.AddrId = Convert.ToInt32(tblOrganizationTODT["addrId"].ToString());
                    if (tblOrganizationTODT["firstOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.FirstOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["firstOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["secondOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.SecondOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["secondOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["parentId"] != DBNull.Value)
                        tblOrganizationTONew.ParentId = Convert.ToInt32(tblOrganizationTODT["parentId"].ToString());
                    if (tblOrganizationTODT["createdBy"] != DBNull.Value)
                        tblOrganizationTONew.CreatedBy = Convert.ToInt32(tblOrganizationTODT["createdBy"].ToString());
                    if (tblOrganizationTODT["createdOn"] != DBNull.Value)
                        tblOrganizationTONew.CreatedOn = Convert.ToDateTime(tblOrganizationTODT["createdOn"].ToString());
                    if (tblOrganizationTODT["firmName"] != DBNull.Value)
                        tblOrganizationTONew.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString());

                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());

                    if (tblOrganizationTODT["registeredMobileNos"] != DBNull.Value)
                        tblOrganizationTONew.RegisteredMobileNos = Convert.ToString(tblOrganizationTODT["registeredMobileNos"].ToString());

                    if (tblOrganizationTODT["cdStructureId"] != DBNull.Value)
                        tblOrganizationTONew.CdStructureId = Convert.ToInt32(tblOrganizationTODT["cdStructureId"].ToString());
                    if (tblOrganizationTODT["delPeriodId"] != DBNull.Value)
                        tblOrganizationTONew.DelPeriodId = Convert.ToInt32(tblOrganizationTODT["delPeriodId"].ToString());

                    if (tblOrganizationTODT["isActive"] != DBNull.Value)
                        tblOrganizationTONew.IsActive = Convert.ToInt32(tblOrganizationTODT["isActive"].ToString());
                    if (tblOrganizationTODT["remark"] != DBNull.Value)
                        tblOrganizationTONew.Remark = Convert.ToString(tblOrganizationTODT["remark"].ToString());

                    if (tblOrganizationTODT["isSpecialCnf"] != DBNull.Value)
                        tblOrganizationTONew.IsSpecialCnf = Convert.ToInt32(tblOrganizationTODT["isSpecialCnf"].ToString());

                    if (tblOrganizationTODT["digitalSign"] != DBNull.Value)
                        tblOrganizationTONew.DigitalSign = Convert.ToString(tblOrganizationTODT["digitalSign"].ToString());
                    if (tblOrganizationTODT["deactivatedOn"] != DBNull.Value)
                        tblOrganizationTONew.DeactivatedOn = Convert.ToDateTime(tblOrganizationTODT["deactivatedOn"].ToString());


                    if (tblOrganizationTODT["orgLogo"] != DBNull.Value)
                        tblOrganizationTONew.OrgLogo = Convert.ToString(tblOrganizationTODT["orgLogo"].ToString());

                    if (tblOrganizationTODT["firmTypeId"] != DBNull.Value)
                        tblOrganizationTONew.FirmTypeId = Convert.ToInt32(tblOrganizationTODT["firmTypeId"].ToString());

                    if (tblOrganizationTODT["influencerTypeId"] != DBNull.Value)
                        tblOrganizationTONew.InfluencerTypeId = Convert.ToInt32(tblOrganizationTODT["influencerTypeId"].ToString());

                    //Sudhir[22-APR-2018]
                    if (tblOrganizationTODT["dateOfEstablishment"] != DBNull.Value)
                        tblOrganizationTONew.DateOfEstablishment = Convert.ToDateTime(tblOrganizationTODT["dateOfEstablishment"].ToString());

                    if (tblOrganizationTODT["firmTypeId"] != DBNull.Value)
                        tblOrganizationTONew.FirmTypeId = Convert.ToInt32(tblOrganizationTODT["firmTypeId"].ToString());

                    if (tblOrganizationTODT["influencerTypeId"] != DBNull.Value)
                        tblOrganizationTONew.InfluencerTypeId = Convert.ToInt32(tblOrganizationTODT["influencerTypeId"].ToString());

                    //Priyanka [14-02-2019]
                    if (tblOrganizationTODT["overdueRefId"] != DBNull.Value)
                        tblOrganizationTONew.OverdueRefId = Convert.ToString(tblOrganizationTODT["overdueRefId"].ToString());

                    if (tblOrganizationTODT["enqRefId"] != DBNull.Value)
                        tblOrganizationTONew.EnqRefId = Convert.ToString(tblOrganizationTODT["enqRefId"].ToString());
                    if (tblOrganizationTODT["isInternalCnf"] != DBNull.Value)
                        tblOrganizationTONew.IsInternalCnf = Convert.ToInt32(tblOrganizationTODT["isInternalCnf"].ToString());

                    if (tblOrganizationTODT["isTcsApplicable"] != DBNull.Value)
                        tblOrganizationTONew.IsTcsApplicable = Convert.ToInt32(tblOrganizationTODT["isTcsApplicable"].ToString());
                    if (tblOrganizationTODT["consumerType"] != DBNull.Value)
                        tblOrganizationTONew.ConsumerType = Convert.ToString(tblOrganizationTODT["consumerType"].ToString());

                    tblOrganizationTOList.Add(tblOrganizationTONew);
                }
            }
            return tblOrganizationTOList;
        }

        public static List<OrgExportRptTO> SelectAllOrgListToExport(Int32 orgTypeId, Int32 parentId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string sqlQuery = string.Empty;
            try
            {
                conn.Open();

                sqlQuery = " SELECT tblOrganization.* , dimCdStructure.cdValue , dimDelPeriod.deliveryPeriod, addrDtl.*, " +
                           " provGstDtl.licenseValue as provGSTNo, permGstDtl.licenseValue as permGSTNo,  " +
                           " panDtl.licenseValue AS PanNO ,cnfDtl.firmName as CnfName " +
                           " ,ISNULL(tblPerson.firstName,'') AS foFirstName,ISNULL(tblPerson.midName,'') AS foMidName,ISNULL(tblPerson.lastName,'') AS foLastName " +
                           " ,ISNULL(tblPerson.mobileNo,'') AS foMobileNo,ISNULL(tblPerson.alternateMobNo,'') AS foAlternateMobNo,ISNULL(tblPerson.phoneNo,'') AS foPhoneNo " +
                           " ,tblPerson.dateOfBirth AS foDateOfBirth,ISNULL(tblPerson.primaryEmail,'') AS foPrimaryEmail,ISNULL(tblPerson.alternateEmail,'') AS foAlternateEmail " +
                           " ,ISNULL(soPerson.firstName,'') AS soFirstName,ISNULL(soPerson.midName,'') AS soMidName,ISNULL(soPerson.lastName,'') AS soLastName " +
                           " ,ISNULL(soPerson.mobileNo,'') AS soMobileNo,ISNULL(soPerson.alternateMobNo,'') AS soAlternateMobNo,ISNULL(soPerson.phoneNo,'') AS soPhoneNo " +
                           " ,soPerson.dateOfBirth AS soDateOfBirth,ISNULL(soPerson.primaryEmail,'') AS soPrimaryEmail,ISNULL(soPerson.alternateEmail,'') AS soAlternateEmail " +
                           " ,CASE WHEN LEN(tblOrganization.registeredMobileNos) > 10 OR LEN(tblOrganization.registeredMobileNos) <= 2 THEN SUBSTRING(tblOrganization.registeredMobileNos, 3, 8000) ELSE tblOrganization.registeredMobileNos END AS smsNo " +
                           " , CASE WHEN LEN(tblPerson.mobileNo) > 10 OR LEN(tblPerson.mobileNo) <= 2 THEN SUBSTRING(tblPerson.mobileNo, 3, 8000) ELSE tblPerson.mobileNo END AS OwnerNo " +
                           " FROM tblOrganization " +
                           " LEFT JOIN " +
                           " ( " +
                           "     SELECT tblOrgAddress.organizationId, tblAddress.* , ISNULL(districtName,'') districtName, ISNULL(talukaName,'') talukaName, ISNULL(stateName,'') stateName " +
                           "     FROM tblOrgAddress " +
                           "     LEFT JOIN tblAddress ON idAddr = addressId " +
                           "     LEFT JOIN dimDistrict On idDistrict= districtId " +
                           "     LEFT JOIN dimTaluka On idTaluka= talukaId " +
                           "     LEFT JOIN dimState On idState= tblAddress.stateId " +
                           "     WHERE addrTypeId = " + (int)Constants.AddressTypeE.OFFICE_ADDRESS + " " +
                           " ) AS addrDtl ON addrDtl.organizationId = idOrganization " +
                           " LEFT JOIN " +
                           " ( " +
                           "     SELECT organizationId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (" + (int)Constants.CommercialLicenseE.SGST_NO + ") " +
                           " ) as provGstDtl On provGstDtl.organizationId = idOrganization " +
                           " LEFT JOIN " +
                           " ( " +
                           "     SELECT organizationId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (" + (int)Constants.CommercialLicenseE.IGST_NO + ") " +
                           " ) as permGstDtl On permGstDtl.organizationId = idOrganization " +
                           " LEFT JOIN " +
                           " ( " +
                           "     SELECT organizationId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (" + (int)Constants.CommercialLicenseE.PAN_NO + ") " +
                           " ) AS panDtl " +
                           " ON panDtl.organizationId = idOrganization " +
                           " LEFT JOIN tblOrganization cnfDtl " +
                           " ON cnfDtl.idOrganization = tblOrganization.parentId " +
                           " LEFT JOIN tblPerson ON idPerson = tblOrganization.firstOwnerPersonId " +
                           " LEFT JOIN tblPerson soPerson ON soPerson.idPerson = tblOrganization.secondOwnerPersonId " +
                           " LEFT JOIN dimCdStructure ON idCdStructure = tblOrganization.cdStructureId " +
                           " LEFT JOIN dimDelPeriod ON idDelPeriod = tblOrganization.delPeriodId " +
                           " WHERE tblOrganization.orgTypeId = " + orgTypeId + " AND tblOrganization.isActive = 1";


                if (parentId > 0)
                    cmdSelect.CommandText = sqlQuery + " AND tblOrganization.parentId=" + parentId + " ORDER BY tblOrganization.firmName";
                else
                    cmdSelect.CommandText = sqlQuery;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertReaderToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<OrgExportRptTO> ConvertReaderToList(SqlDataReader tblOrganizationTODT)
        {
            List<OrgExportRptTO> orgExportRptTOList = new List<OrgExportRptTO>();
            if (tblOrganizationTODT != null)
            {
                while (tblOrganizationTODT.Read())
                {
                    OrgExportRptTO tblOrganizationTONew = new OrgExportRptTO();
                    if (tblOrganizationTODT["idOrganization"] != DBNull.Value)
                        tblOrganizationTONew.IdOrganization = Convert.ToInt32(tblOrganizationTODT["idOrganization"].ToString());
                    if (tblOrganizationTODT["firmName"] != DBNull.Value)
                        tblOrganizationTONew.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString());
                    if (tblOrganizationTODT["CnfName"] != DBNull.Value)
                        tblOrganizationTONew.CnfName = Convert.ToString(tblOrganizationTODT["CnfName"].ToString());
                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());

                    if (tblOrganizationTODT["registeredMobileNos"] != DBNull.Value)
                        tblOrganizationTONew.RegMobileNo = Convert.ToString(tblOrganizationTODT["registeredMobileNos"].ToString());

                    if (tblOrganizationTODT["cdValue"] != DBNull.Value)
                        tblOrganizationTONew.CdStructure = Convert.ToDouble(tblOrganizationTODT["cdValue"].ToString());
                    if (tblOrganizationTODT["deliveryPeriod"] != DBNull.Value)
                        tblOrganizationTONew.DeliveryPeriod = Convert.ToInt32(tblOrganizationTODT["deliveryPeriod"].ToString());

                    if (tblOrganizationTODT["villageName"] != DBNull.Value)
                        tblOrganizationTONew.Village = Convert.ToString(tblOrganizationTODT["villageName"].ToString());
                    if (tblOrganizationTODT["isSpecialCnf"] != DBNull.Value)
                        tblOrganizationTONew.IsSpecialCnf = Convert.ToInt32(tblOrganizationTODT["isSpecialCnf"].ToString());
                    if (tblOrganizationTODT["talukaName"] != DBNull.Value)
                        tblOrganizationTONew.Taluka = Convert.ToString(tblOrganizationTODT["talukaName"].ToString());
                    if (tblOrganizationTODT["districtName"] != DBNull.Value)
                        tblOrganizationTONew.District = Convert.ToString(tblOrganizationTODT["districtName"].ToString());
                    if (tblOrganizationTODT["stateName"] != DBNull.Value)
                        tblOrganizationTONew.State = Convert.ToString(tblOrganizationTODT["stateName"].ToString());

                    if (tblOrganizationTODT["foFirstName"] != DBNull.Value)
                        tblOrganizationTONew.FoFirstName = Convert.ToString(tblOrganizationTODT["foFirstName"].ToString());
                    if (tblOrganizationTODT["foMidName"] != DBNull.Value)
                        tblOrganizationTONew.FoMiddleName = Convert.ToString(tblOrganizationTODT["foMidName"].ToString());
                    if (tblOrganizationTODT["foLastName"] != DBNull.Value)
                        tblOrganizationTONew.FoLastName = Convert.ToString(tblOrganizationTODT["foLastName"].ToString());
                    if (tblOrganizationTODT["foMobileNo"] != DBNull.Value)
                        tblOrganizationTONew.FoMobileNo = Convert.ToString(tblOrganizationTODT["foMobileNo"].ToString());
                    if (tblOrganizationTODT["foAlternateMobNo"] != DBNull.Value)
                        tblOrganizationTONew.FoAlternateMobileNo = Convert.ToString(tblOrganizationTODT["foAlternateMobNo"].ToString());
                    if (tblOrganizationTODT["foPhoneNo"] != DBNull.Value)
                        tblOrganizationTONew.FoPhoneNo = Convert.ToString(tblOrganizationTODT["foPhoneNo"].ToString());
                    if (tblOrganizationTODT["foDateOfBirth"] != DBNull.Value)
                        tblOrganizationTONew.FoDob = Convert.ToDateTime(tblOrganizationTODT["foDateOfBirth"].ToString()).ToString("dd-MM-yyyy");
                    if (tblOrganizationTODT["foPrimaryEmail"] != DBNull.Value)
                        tblOrganizationTONew.FoEmailAddr = Convert.ToString(tblOrganizationTODT["foPrimaryEmail"].ToString());
                    if (tblOrganizationTODT["foAlternateEmail"] != DBNull.Value)
                        tblOrganizationTONew.FoAlterEmailAddr = Convert.ToString(tblOrganizationTODT["foAlternateEmail"].ToString());

                    if (tblOrganizationTODT["soFirstName"] != DBNull.Value)
                        tblOrganizationTONew.SoFirstName = Convert.ToString(tblOrganizationTODT["soFirstName"].ToString());
                    if (tblOrganizationTODT["soMidName"] != DBNull.Value)
                        tblOrganizationTONew.SoMiddleName = Convert.ToString(tblOrganizationTODT["soMidName"].ToString());
                    if (tblOrganizationTODT["soLastName"] != DBNull.Value)
                        tblOrganizationTONew.SoLastName = Convert.ToString(tblOrganizationTODT["soLastName"].ToString());
                    if (tblOrganizationTODT["soMobileNo"] != DBNull.Value)
                        tblOrganizationTONew.SoMobileNo = Convert.ToString(tblOrganizationTODT["soMobileNo"].ToString());
                    if (tblOrganizationTODT["soAlternateMobNo"] != DBNull.Value)
                        tblOrganizationTONew.SoAlternateMobileNo = Convert.ToString(tblOrganizationTODT["soAlternateMobNo"].ToString());
                    if (tblOrganizationTODT["soPhoneNo"] != DBNull.Value)
                        tblOrganizationTONew.SoPhoneNo = Convert.ToString(tblOrganizationTODT["soPhoneNo"].ToString());
                    if (tblOrganizationTODT["soDateOfBirth"] != DBNull.Value)
                        tblOrganizationTONew.SoDob = Convert.ToDateTime(tblOrganizationTODT["soDateOfBirth"].ToString()).ToString("dd-MM-yyyy");
                    if (tblOrganizationTODT["soPrimaryEmail"] != DBNull.Value)
                        tblOrganizationTONew.SoEmailAddr = Convert.ToString(tblOrganizationTODT["soPrimaryEmail"].ToString());
                    if (tblOrganizationTODT["soAlternateEmail"] != DBNull.Value)
                        tblOrganizationTONew.SoAlterEmailAddr = Convert.ToString(tblOrganizationTODT["soAlternateEmail"].ToString());

                    if (tblOrganizationTODT["plotNo"] != DBNull.Value)
                        tblOrganizationTONew.PlotNo = Convert.ToString(tblOrganizationTODT["plotNo"].ToString());
                    if (tblOrganizationTODT["streetName"] != DBNull.Value)
                        tblOrganizationTONew.StreetName = Convert.ToString(tblOrganizationTODT["streetName"].ToString());
                    if (tblOrganizationTODT["areaName"] != DBNull.Value)
                        tblOrganizationTONew.AreaName = Convert.ToString(tblOrganizationTODT["areaName"].ToString());
                    if (tblOrganizationTODT["pincode"] != DBNull.Value)
                        tblOrganizationTONew.PinCode = Convert.ToString(tblOrganizationTODT["pincode"].ToString());
                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());
                    if (tblOrganizationTODT["PanNO"] != DBNull.Value)
                        tblOrganizationTONew.PanNo = Convert.ToString(tblOrganizationTODT["PanNO"].ToString());
                    if (tblOrganizationTODT["PanNO"] != DBNull.Value)
                        tblOrganizationTONew.PanNo = Convert.ToString(tblOrganizationTODT["PanNO"].ToString());
                    if (tblOrganizationTODT["provGSTNo"] != DBNull.Value)
                        tblOrganizationTONew.ProvGstNo = Convert.ToString(tblOrganizationTODT["provGSTNo"].ToString());
                    if (tblOrganizationTODT["permGSTNo"] != DBNull.Value)
                        tblOrganizationTONew.PermGstNo = Convert.ToString(tblOrganizationTODT["permGSTNo"].ToString());

                    Constants.SetNullValuesToEmpty(tblOrganizationTONew);
                    orgExportRptTOList.Add(tblOrganizationTONew);
                }
            }
            return orgExportRptTOList;
        }

        public static List<DropDownTO> SelectSalesEngineerListForDropDown(Int32 orgId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                sqlQuery = " SELECT cnfOrg.idOrganization,(CASE WHEN addrDtl.villageName " +
                              " IS NULL then  cnfOrg.firmName  ELSE cnfOrg.firmName +',' + addrDtl.villageName END)" +
                              " as firmName ," +
                              " cnfOrg.isSpecialCnf FROM tblOrganization cnfOrg " +
                              " LEFT JOIN " +
                              " ( " +
                              " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                              " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                              " ) addrDtl " +
                              " ON cnfOrg.idOrganization = addrDtl.organizationId " +
                              " Left join tblOrganization dealerOrg on cnfOrg.idOrganization = dealerOrg.parentId " +
                              " WHERE  cnfOrg.isActive=1 AND dealerOrg.idOrganization =" + orgId;


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["firmName"].ToString());
                        if (tblOrgReader["isSpecialCnf"] != DBNull.Value)
                            dropDownTO.Tag = "isSpecialCnf : " + Convert.ToString(tblOrgReader["isSpecialCnf"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectSalesEngineerListForDropDown at DAO");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrganizationTO> SelectExistingAllTblOrganizationByRefIds(Int32 orgId, String overdueRefId, String enqRefId)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;

            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery();

                if (orgId != 0)
                    cmdSelect.CommandText += " WHERE tblOrganization.idOrganization != " + orgId;

                if (!String.IsNullOrEmpty(overdueRefId))
                {

                    if (cmdSelect.CommandText.Contains("WHERE"))
                        cmdSelect.CommandText += " AND ";
                    else
                        cmdSelect.CommandText += " WHERE ";

                    cmdSelect.CommandText += "tblOrganization.overdueRefId = '" + overdueRefId + "'";
                }

                if (!String.IsNullOrEmpty(enqRefId))
                {

                    if (cmdSelect.CommandText.Contains("WHERE"))
                        cmdSelect.CommandText += " AND ";
                    else
                        cmdSelect.CommandText += " WHERE ";

                    cmdSelect.CommandText += "tblOrganization.enqRefId = '" + enqRefId + "'";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Priyanka [08-04-2019]
        public static List<SalesTrackerAPI.DashboardModels.DealerOverdueDtl> GetDealersOverdueDetailsInfo(Int32 dealerId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblDealerOverdueDtlTODT = null;

            try
            {

                conn.Open();
                if (dealerId > 0)
                {
                    cmdSelect.CommandText = " SELECT overDue.overdueAmt, enqDtl.enquiryAmt, tblOrg.idOrganization FROM tblOrganization tblOrg " +
                                            " LEFT JOIN tblOverdueDtl overDue ON tblOrg.idOrganization = overDue.organizationId " +
                                            " LEFT JOIN tblEnquiryDtl enqDtl ON tblOrg.idOrganization = enqDtl.organizationId " +
                                            " WHERE tblOrg.idOrganization =" + dealerId;
                }
                else
                {
                    cmdSelect.CommandText = " SELECT SUM(overDue.overdueAmt) AS overdueAmt, SUM(enqDtl.enquiryAmt) AS enquiryAmt FROM tblOrganization tblOrg " +
                                            " LEFT JOIN tblOverdueDtl overDue ON  tblOrg.idOrganization = overDue.organizationId " +
                                            " LEFT JOIN tblEnquiryDtl enqDtl ON tblOrg.idOrganization = enqDtl.organizationId ";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblDealerOverdueDtlTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<SalesTrackerAPI.DashboardModels.DealerOverdueDtl> DealerOverdueDtlList = new List<DashboardModels.DealerOverdueDtl>();
                while (tblDealerOverdueDtlTODT.Read())
                {
                    SalesTrackerAPI.DashboardModels.DealerOverdueDtl DealerOverdueDtlNew = new SalesTrackerAPI.DashboardModels.DealerOverdueDtl();
                    if (tblDealerOverdueDtlTODT["overdueAmt"] != DBNull.Value)
                        DealerOverdueDtlNew.OverdueAmt = Convert.ToDouble(tblDealerOverdueDtlTODT["overdueAmt"].ToString());
                    if (tblDealerOverdueDtlTODT["enquiryAmt"] != DBNull.Value)
                        DealerOverdueDtlNew.EnquiryAmt = Convert.ToDouble(tblDealerOverdueDtlTODT["enquiryAmt"].ToString());
                    if (dealerId > 0)
                    {
                        if (tblDealerOverdueDtlTODT["idOrganization"] != DBNull.Value)
                            DealerOverdueDtlNew.IdOrganization = Convert.ToInt32(tblDealerOverdueDtlTODT["idOrganization"].ToString());
                    }

                    DealerOverdueDtlList.Add(DealerOverdueDtlNew);
                }

                return DealerOverdueDtlList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblDealerOverdueDtlTODT != null)
                    tblDealerOverdueDtlTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        #endregion

        #region Insertion
        public static int InsertTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrganizationTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrganizationTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblOrganizationTO tblOrganizationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrganization]( " +
                            "  [orgTypeId]" +
                            " ,[addrId]" +
                            " ,[firstOwnerPersonId]" +
                            " ,[secondOwnerPersonId]" +
                            " ,[parentId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[firmName]" +
                            " ,[phoneNo]" +
                            " ,[faxNo]" +
                            " ,[emailAddr]" +
                            " ,[website]" +
                            " ,[registeredMobileNos]" +
                            " ,[cdStructureId]" +
                            " ,[isActive]" +
                            " ,[remark]" +
                            " ,[delPeriodId]" +
                            " ,[isSpecialCnf]" +
                            " ,[digitalSign]" +
                            " ,[firmTypeId] " +
                            " ,[influencerTypeId]" +
                            " ,[dateOfEstablishment]" +
                            " )" +
                " VALUES (" +
                            "  @OrgTypeId " +
                            " ,@AddrId " +
                            " ,@FirstOwnerPersonId " +
                            " ,@SecondOwnerPersonId " +
                            " ,@ParentId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@FirmName " +
                            " ,@phoneNo " +
                            " ,@faxNo " +
                            " ,@emailAddr " +
                            " ,@website " +
                            " ,@registeredMobileNos " +
                            " ,@cdStructureId " +
                            " ,@isActive " +
                            " ,@remark " +
                            " ,@delPeriodId " +
                            " ,@isSpecialCnf " +
                            " ,@digitalSign " +
                            " ,@firmTypeId " +
                            " ,@InfluencerTypeId" +
                            " ,@DateOfEstablishment" +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
            cmdInsert.Parameters.Add("@OrgTypeId", System.Data.SqlDbType.Int).Value = tblOrganizationTO.OrgTypeId;
            cmdInsert.Parameters.Add("@AddrId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.AddrId);
            cmdInsert.Parameters.Add("@FirstOwnerPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirstOwnerPersonId);
            cmdInsert.Parameters.Add("@SecondOwnerPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.SecondOwnerPersonId);
            cmdInsert.Parameters.Add("@ParentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.ParentId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrganizationTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrganizationTO.CreatedOn;
            cmdInsert.Parameters.Add("@FirmName", System.Data.SqlDbType.NVarChar).Value = tblOrganizationTO.FirmName;
            cmdInsert.Parameters.Add("@phoneNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.PhoneNo);
            cmdInsert.Parameters.Add("@faxNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FaxNo);
            cmdInsert.Parameters.Add("@emailAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.EmailAddr);
            cmdInsert.Parameters.Add("@website", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.Website);
            cmdInsert.Parameters.Add("@registeredMobileNos", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.RegisteredMobileNos);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.CdStructureId);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IsActive;
            cmdInsert.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.Remark);
            cmdInsert.Parameters.Add("@delPeriodId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DelPeriodId);
            cmdInsert.Parameters.Add("@isSpecialCnf", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsSpecialCnf);
            cmdInsert.Parameters.Add("@digitalSign", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DigitalSign);
            cmdInsert.Parameters.Add("@firmTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirmTypeId);
            cmdInsert.Parameters.Add("@InfluencerTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.InfluencerTypeId);
            cmdInsert.Parameters.Add("@DateOfEstablishment", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DateOfEstablishment);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblOrganizationTO.IdOrganization = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrganizationTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrganizationTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblOrganizationTO tblOrganizationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrganization] SET " +
                            "  [orgTypeId]= @OrgTypeId" +
                            " ,[addrId]= @AddrId" +
                            " ,[firstOwnerPersonId]= @FirstOwnerPersonId" +
                            " ,[secondOwnerPersonId]= @SecondOwnerPersonId" +
                            " ,[parentId]= @ParentId" +
                            " ,[firmName] = @FirmName" +
                            " ,[updatedBy] = @updatedBy" +
                            " ,[updatedOn] = @updatedOn" +
                            " ,[phoneNo] = @phoneNo" +
                            " ,[faxNo] = @faxNo" +
                            " ,[emailAddr] = @emailAddr" +
                            " ,[website] = @website" +
                            " ,[registeredMobileNos] = @registeredMobileNos" +
                            " ,[cdStructureId] = @cdStructureId" +
                            " ,[isActive] = @isActive" +
                            " ,[remark] = @remark" +
                            " ,[delPeriodId] = @delPeriodId" +
                            " ,[isSpecialCnf] = @isSpecialCnf" +
                            " ,[digitalSign] = @digitalSign" +
                            " ,[deactivatedOn] = @deactivatedOn" +
                            " ,[firmTypeId] = @FirmTypeId " +
                            " ,[influencerTypeId] =@InfluencerTypeId" +
                            " ,[dateOfEstablishment]=@DateOfEstablishment" +
                            " WHERE [idOrganization] = @IdOrganization";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
            cmdUpdate.Parameters.Add("@OrgTypeId", System.Data.SqlDbType.Int).Value = tblOrganizationTO.OrgTypeId;
            cmdUpdate.Parameters.Add("@AddrId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.AddrId);
            cmdUpdate.Parameters.Add("@FirstOwnerPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirstOwnerPersonId);
            cmdUpdate.Parameters.Add("@SecondOwnerPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.SecondOwnerPersonId);
            cmdUpdate.Parameters.Add("@ParentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.ParentId);
            cmdUpdate.Parameters.Add("@FirmName", System.Data.SqlDbType.NVarChar).Value = tblOrganizationTO.FirmName;
            cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblOrganizationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = tblOrganizationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@phoneNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.PhoneNo);
            cmdUpdate.Parameters.Add("@faxNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FaxNo);
            cmdUpdate.Parameters.Add("@emailAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.EmailAddr);
            cmdUpdate.Parameters.Add("@website", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.Website);
            cmdUpdate.Parameters.Add("@registeredMobileNos", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.RegisteredMobileNos);
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.CdStructureId);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IsActive;
            cmdUpdate.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.Remark);
            cmdUpdate.Parameters.Add("@delPeriodId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DelPeriodId);
            cmdUpdate.Parameters.Add("@isSpecialCnf", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsSpecialCnf);
            cmdUpdate.Parameters.Add("@digitalSign", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DigitalSign);
            cmdUpdate.Parameters.Add("@deactivatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DeactivatedOn);
            cmdUpdate.Parameters.Add("@FirmTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirmTypeId);
            cmdUpdate.Parameters.Add("@InfluencerTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.InfluencerTypeId);
            cmdUpdate.Parameters.Add("@DateOfEstablishment", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DateOfEstablishment);

            return cmdUpdate.ExecuteNonQuery();
        }

        //Priyanka [08-04-2019]
        public static int UpdateTblOrganizationRefIds(TblOrganizationTO tblOrganizationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tblOrganization] SET " +
                                "  [overdueRefId]= @OverdueRefId" +
                                " ,[enqRefId]= @EnqRefId" +
                                " ,[updatedBy] = @updatedBy" +
                                " ,[updatedOn] = @updatedOn" +
                                " WHERE [idOrganization] = @IdOrganization";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
                cmdUpdate.Parameters.Add("@OverdueRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.OverdueRefId);
                cmdUpdate.Parameters.Add("@EnqRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.EnqRefId);
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblOrganizationTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = tblOrganizationTO.UpdatedOn;

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        #endregion

        #region Deletion
        public static int DeleteTblOrganization(Int32 idOrganization)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrganization, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrganization, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idOrganization, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrganization] " +
            " WHERE idOrganization = " + idOrganization + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
