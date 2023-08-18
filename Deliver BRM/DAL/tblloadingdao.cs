using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.DAL
{
    public class TblLoadingDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loading.* , fromOrgNameTbl.firmName as fromOrgName ,org.digitalSign, org.firmName as cnfOrgName, org.isInternalCnf ,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +

                                  " ,createdUser.userDisplayName ,tblUserCallFlag.userDisplayName AS notifyByName" +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM tempLoading loading " +

                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser tblUserCallFlag ON tblUserCallFlag.idUser=loading.callFlagBy " +

                                  " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy" +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate=loading.gateId " +
                                  //Prajakta [2021-06-29] Added to show orgName on loading slip
                                  " LEFT JOIN tblOrganization fromOrgNameTbl on fromOrgNameTbl.idOrganization = loading.fromOrgId " +
                                // Vaibhav [20-Nov-2017] added to select from finalLoading
                                " UNION ALL " +

                                 " SELECT loading.*, fromOrgNameTbl.firmName as fromOrgName ,org.digitalSign, org.firmName as cnfOrgName,org.isInternalCnf,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                 " ,createdUser.userDisplayName ,tblUserCallFlag.userDisplayName AS notifyByName" +
                                 " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                 " FROM finalLoading loading " +
                                 " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                 " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                 " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                 " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                 " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                 " LEFT JOIN tblUser tblUserCallFlag ON tblUserCallFlag.idUser=loading.callFlagBy " +
                                 " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy" +
                                 " LEFT JOIN tblGate tblGate ON tblGate.idGate=loading.gateId " +
                                   //Prajakta [2021-06-29] Added to show orgName on loading slip
                                  " LEFT JOIN tblOrganization fromOrgNameTbl on fromOrgNameTbl.idOrganization = loading.fromOrgId " ;

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingTO> SelectAllTblLoading()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingTO> SelectAllLoadingsFromParentLoadingId(Int32 parentLoadingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                //Vaibhav [21-Nov-2017] Changed for data separation ativity
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.parentLoadingId=" + parentLoadingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<TblLoadingTO> SelectAllLoadingListByStatus(string statusId, SqlConnection conn, SqlTransaction tran,int gateId=0)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                int modeId = Constants.getModeIdConfigTO();
                //Vaibhav [21-Nov-2017] Changed for data separation ativity
                if(gateId == 0)
                    cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.statusId IN(" + statusId + ")" ;
                else
                    cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.statusId IN(" + statusId + ") AND sq1.gateId = " + gateId;

                if(modeId > 0)
                {
                    cmdSelect.CommandText += " AND ISNULL(sq1.modeId,1) ="+ modeId;
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static tblUserMachineMappingTo SelectUserMachineTo(int userId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM tblusermachinemapping where userId = " + userId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                tblUserMachineMappingTo MachineMappingTo = new tblUserMachineMappingTo();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        if (sqlReader["idUserMachineMapping"] != DBNull.Value)
                            MachineMappingTo.IdUserMachineMapping = Convert.ToInt32(sqlReader["idUserMachineMapping"].ToString());
                        if (sqlReader["userId"] != DBNull.Value)
                            MachineMappingTo.UserId = Convert.ToInt32(sqlReader["userId"].ToString());
                        if (sqlReader["gateId"] != DBNull.Value)
                            MachineMappingTo.GateId = Convert.ToInt32(sqlReader["gateId"].ToString());
                    }
                }
                return MachineMappingTo;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        #region tblgatelist
        public static List<TblGateTO> SelectTblGateList(TblLoadingTO tblLoading,SqlConnection conn)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
              
                //Vaibhav [21-Nov-2017] Changed for data separation ativity
                cmdSelect.CommandText = " select idGate,gateName,gateDesc,portNumber," +
                    "(select gateId from tempLoading where idLoading=" + tblLoading.IdLoading + ") " +
                    " as PreviousGateId,(select gateName from tempLoading left join tblGate g on " +
                    " g.idGate=tempLoading.gateId where idLoading=" + tblLoading.IdLoading + ") as PreviousGateName from tblgate " +
                    " where  isactive = 1 and idGate!=(select gateId from tempLoading where idLoading=" + tblLoading.IdLoading+") ";
               // cmdSelect.CommandText = " select idGate,gateName,gateDesc,portNumber from tblgate ";
                cmdSelect.Connection = conn;
              //  cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGateTO> tblgateList = new List<TblGateTO>();
                while(sqlReader.Read())
                {
                    TblGateTO tblGateTo = new TblGateTO();
                    if (sqlReader["idGate"] != DBNull.Value)
                      tblGateTo.IdGate = Convert.ToInt32(sqlReader["idGate"].ToString());
                    if (sqlReader["gateName"] != DBNull.Value)
                        tblGateTo.GateName =(sqlReader["gateName"].ToString());
                    if (sqlReader["gateDesc"] != DBNull.Value)
                        tblGateTo.GateDesc = (sqlReader["gateDesc"].ToString());
                    if (sqlReader["portNumber"] != DBNull.Value)
                        tblGateTo.PortNumber = (sqlReader["portNumber"].ToString());
                    if (sqlReader["PreviousGateId"] != DBNull.Value)
                        tblGateTo.PreviousGateId =Convert.ToInt32(sqlReader["PreviousGateId"].ToString());
                    if (sqlReader["PreviousGateName"] != DBNull.Value)
                        tblGateTo.PreviousGateName = (sqlReader["PreviousGateName"].ToString());
                    tblgateList.Add(tblGateTo);
                }
              
                return tblgateList;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
         public static List<TblGateTO> SelectAllTblGateList()
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                  String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
              conn.Open();
      
              
                cmdSelect.CommandText = " select idGate,gateName,gateDesc,portNumber from tblgate ";
                cmdSelect.Connection = conn;
           
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGateTO> tblgateList = new List<TblGateTO>();
                while(sqlReader.Read())
                {
                    TblGateTO tblGateTo = new TblGateTO();
                    if (sqlReader["idGate"] != DBNull.Value)
                      tblGateTo.IdGate = Convert.ToInt32(sqlReader["idGate"].ToString());
                    if (sqlReader["gateName"] != DBNull.Value)
                        tblGateTo.GateName =(sqlReader["gateName"].ToString());
                    if (sqlReader["gateDesc"] != DBNull.Value)
                        tblGateTo.GateDesc = (sqlReader["gateDesc"].ToString());
                    if (sqlReader["portNumber"] != DBNull.Value)
                        tblGateTo.PortNumber = (sqlReader["portNumber"].ToString());
                  
                    tblgateList.Add(tblGateTo);
                }
              
                return tblgateList;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        #endregion
        public static List<int> GeModRefMaxData()
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP 255 modbusRefId FROM tempLoading WHERE modbusRefId IS NOT NULL ORDER BY modbusRefId DESC";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<int> list = new List<int>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        int modRefId = 0;
                        if (sqlReader["modbusRefId"] != DBNull.Value)
                            modRefId = Convert.ToInt32(sqlReader["modbusRefId"].ToString());
                        if (modRefId > 0)
                            list.Add(modRefId);
                    }
                }
                        
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static  DimReportTemplateTO SelectDimReportTemplate(String reportName)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate WHERE reportName = '" + reportName + "' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@report_name", System.Data.SqlDbType.NVarChar).Value = mstReportTemplateTO.ReportName;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToListV2(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                //String userName = System.Windows.Forms.SystemInformation.UserName;
                //MessageBox.Show("Computer Name:" + computerName + Environment.NewLine + "User Name:" + userName + Environment.NewLine + "Class Name: MstReportTemplateDAO" + Environment.NewLine + "Method Name:SelectMstReportTemplate(MstReportTemplateTO mstReportTemplateTO)" + Environment.NewLine + "Exception Message:" + ex.Message.ToString() + "");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<DimReportTemplateTO> ConvertDTToListV2(SqlDataReader dimReportTemplateTODT)
        {
            List<DimReportTemplateTO> dimReportTemplateTOList = new List<DimReportTemplateTO>();
            if (dimReportTemplateTODT != null)
            {
                while (dimReportTemplateTODT.Read())
                {
                    DimReportTemplateTO dimReportTemplateTONew = new DimReportTemplateTO();
                    if (dimReportTemplateTODT["idReport"] != DBNull.Value)
                        dimReportTemplateTONew.IdReport = Convert.ToInt32(dimReportTemplateTODT["idReport"].ToString());
                    if (dimReportTemplateTODT["isDisplayMultisheetAllow"] != DBNull.Value)
                        dimReportTemplateTONew.IsDisplayMultisheetAllow = Convert.ToInt32(dimReportTemplateTODT["isDisplayMultisheetAllow"].ToString());
                    if (dimReportTemplateTODT["createdBy"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedBy = Convert.ToInt32(dimReportTemplateTODT["createdBy"].ToString());
                    if (dimReportTemplateTODT["createdOn"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedOn = Convert.ToDateTime(dimReportTemplateTODT["createdOn"].ToString());
                    if (dimReportTemplateTODT["reportName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportName = Convert.ToString(dimReportTemplateTODT["reportName"].ToString());
                    if (dimReportTemplateTODT["reportFileName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileName = Convert.ToString(dimReportTemplateTODT["reportFileName"].ToString());
                    if (dimReportTemplateTODT["reportFileExtension"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileExtension = Convert.ToString(dimReportTemplateTODT["reportFileExtension"].ToString());
                    if (dimReportTemplateTODT["reportPassword"] != DBNull.Value)
                        dimReportTemplateTONew.ReportPassword = Convert.ToString(dimReportTemplateTODT["reportPassword"].ToString());
                    dimReportTemplateTOList.Add(dimReportTemplateTONew);
                }
            }
            return dimReportTemplateTOList;
        }
        public static List<TblLoadingTO> SelectAllTblLoading(TblUserRoleTO tblUserRoleTO, int cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, string selectedOrgIdStr, Int32 isConfirm, Int32 brandId, Int32 loadingNavigateId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string whereCond = " WHERE CONVERT (DATE,loading.createdOn,103) BETWEEN @fromDate AND @toDate";
            string areConfJoin = string.Empty;
            string finalConfJoin = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            int modeId = Constants.getModeIdConfigTO();
            if (modeId > 0)
            {
                whereCond += " AND ISNULL(loading.modeId,1)="+ modeId;
            }

            String wherecnfIdTemp = String.Empty;
            String wherecnfIdFinal = String.Empty;

            String whereisConTemp = String.Empty;
            String whereisConFinal = String.Empty;

            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                if (isConfEn == 1)
                {
                    areConfJoin = " INNER JOIN ( SELECT DISTINCT cnfOrgId FROM tblUserAreaAllocation WHERE isActive=1 AND userId=" + userId + ") areaConf ON  areaConf.cnfOrgId = loading.cnfOrgId ";

                    //areConfJoin = " INNER JOIN " +
                    //                        " ( " +
                    //                        " SELECT areaConf.cnfOrgId, idOrganization " +
                    //                        " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                    //                        " INNER JOIN " +
                    //                        " ( " +
                    //                        "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                    //                        "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                    //                        "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                    //                        "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                    //                        " ) AS userAreaDealer On userAreaDealer.cnfOrgId = loading.cnFOrgid ";
                    ////"AND loadingslip.dealerOrgId = userAreaDealer.idOrganization ";

                    finalConfJoin = " INNER JOIN " +
                                           " ( " +
                                           " SELECT areaConf.cnfOrgId, idOrganization " +
                                           " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                           " INNER JOIN " +
                                           " ( " +
                                           "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                           "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                                           "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                           "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                           " ) AS userAreaDealer On userAreaDealer.cnfOrgId = loading.cnFOrgid";
                                           //" AND loadingslip.dealerOrgId = userAreaDealer.idOrganization ";

                }
                else
                {
                    if (!string.IsNullOrEmpty(selectedOrgIdStr))
                    {
                        whereCond += " AND loading.fromOrgId in(" + selectedOrgIdStr + ")";
                    }
                }
                conn.Open();
                if (cnfId == 0 && loadingStatusId > 0)
                    whereCond += " AND loading.statusId=" + loadingStatusId;
                else if (cnfId > 0 && loadingStatusId > 0)
                    whereCond += " AND loading.cnfOrgId=" + cnfId + " AND loading.statusId=" + loadingStatusId;
                else if (cnfId > 0 && loadingStatusId == 0)
                    whereCond += " AND loading.cnfOrgId=" + cnfId;
                if (loadingTypeId > 0)
                    whereCond += " AND loading.loadingType=" + loadingTypeId;


                if (dealerId > 0)
                {
                    wherecnfIdTemp += " AND loading.idLoading IN ( SELECT loadingId from tempLoadingSlip where dealerOrgId = " + dealerId + " ) ";
                    wherecnfIdFinal += " AND loading.idLoading IN ( SELECT loadingId from finalLoadingSlip where dealerOrgId = " + dealerId + " ) ";
                }

                if (brandId > 0)
                {
                    wherecnfIdTemp += " AND loading.idLoading IN ( SELECT loadingId from tempLoadingSlip where idLoadingSlip IN (select loadingSlipId from tempLoadingSlipExt where brandId = " + brandId + " ) ) ";
                    wherecnfIdFinal += " AND loading.idLoading IN ( SELECT loadingId from finalLoadingSlip where idLoadingSlip IN (select loadingSlipId from finalLoadingSlipExt where brandId = " + brandId + ") ) ";
                }
                //Priyanka [31-05-2018] : Added to show the confirm and non-confirm loading slip.
                if (isConfirm == 0 || isConfirm == 1)
                {
                    whereisConTemp += " AND loading.idLoading IN ( select loadingId from tempLoadingSlip where ISNULL(isConfirmed,0) = " + isConfirm + ")";
                    whereisConFinal += " AND loading.idLoading IN ( select loadingId from finalLoadingSlip where ISNULL(isConfirmed,0) = " + isConfirm + ")";
                }


                //Priyanka [18-08-2018] : Added for navigate through alert on specific loading slip 
                if (loadingNavigateId > 0)
                {
                    whereCond = " WHERE loading.idLoading =" + loadingNavigateId;
                }


                //chetan[27-April-2020] added

                //Hrushikesh added  for all loading slips exclude the records of delivery Information
                else 
                        whereCond += " AND loading.loadingType !=" + (int)Constants.LoadingTypeE.DELIVERY_INFO;


                // Vaibhav [22-Nov-2017] Added to select data from final tables

                String sqlQuery= " SELECT loading.* , fromOrgNameTbl.firmName as fromOrgName,org.digitalSign, org.firmName as cnfOrgName, org.isInternalCnf ,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,createdUser.userDisplayName , tblUserCallFlag.userDisplayName AS notifyByName" +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM tempLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser tblUserCallFlag ON tblUserCallFlag.idUser=loading.callFlagBy " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  " LEFT JOIN tblOrganization fromOrgNameTbl on fromOrgNameTbl.idOrganization = loading.fromOrgId " +
                                  " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy" + areConfJoin + whereCond+ wherecnfIdTemp + whereisConTemp +
                                  
                                 // Vaibhav [20-Nov-2017] added to select from finalLoading
                " UNION ALL" +

                                 " SELECT loading.*, fromOrgNameTbl.firmName as fromOrgName ,org.digitalSign, org.firmName as cnfOrgName, org.isInternalCnf ,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                 " ,createdUser.userDisplayName, tblUserCallFlag.userDisplayName AS notifyByName" +
                                 " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                 " FROM finalLoading loading " +
                                 " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                 " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                 " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                 " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                 " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                 " LEFT JOIN tblUser tblUserCallFlag ON tblUserCallFlag.idUser=loading.callFlagBy " +
                                 " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                 " LEFT JOIN tblOrganization fromOrgNameTbl on fromOrgNameTbl.idOrganization = loading.fromOrgId " +
                                 " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy" + areConfJoin + whereCond + wherecnfIdFinal + whereisConFinal;

                //cmdSelect.CommandText = SqlSelectQuery() + areConfJoin + whereCond;

                // Vaibhav [22-Nov-2017] Comment and added
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingTO> SelectAllTblLoading(int cnfId, String loadingStatusIdIn, DateTime loadingDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string whereCond = " WHERE DAY (loading.createdOn)=" + loadingDate.Day + " AND MONTH(loading.createdOn)=" + loadingDate.Month + " AND YEAR(loading.createdOn)=" + loadingDate.Year;
            try
            {
                conn.Open();
                if (cnfId == 0)
                    whereCond += " AND statusId IN(" + loadingStatusIdIn + ")";
                else if (cnfId > 0)
                    whereCond += " AND cnfOrgId=" + cnfId + " AND statusId IN(" + loadingStatusIdIn + ")";

                cmdSelect.CommandText = SqlSelectQuery() + whereCond;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingTO SelectTblLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //Vaibhav [21-Nov-2017] Changed for data separation ativity
                cmdSelect.CommandText = " SELECT * FROM (" +SqlSelectQuery() + ")sq1 WHERE idLoading = " + idLoading + " ";
                 cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingTO SelectTblLoadingByLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idLoading in ( select loadingId from tempLoadingSlip where idLoadingSlip =" + loadingSlipId + ") " +

                                      // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlip

                                      " UNION ALL " + " SELECT * FROM (" +  SqlSelectQuery() + ")sq1 WHERE idLoading in ( select loadingId from finalLoadingSlip where idLoadingSlip =" + loadingSlipId + ") ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingTO SelectTblLoadingTOByModBusRefId(Int32 modBusRefId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idLoading in ( select loadingId from tempLoadingSlip where modbusRefId =" + modBusRefId + ") " +

                                      // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlip

                                      " UNION ALL " + " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idLoading in ( select loadingId from finalLoadingSlip where modbusRefId =" + modBusRefId + ") ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static Int64 SelectCountOfLoadingSlips(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = " SELECT COUNT(*) as slipCount FROM tempLoading WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year;

                // Vaibhav [20-Nov-2017] Comment and add Union All for finalLoading
                String sqlQuery = "Select  (SELECT COUNT(*)  FROM tempLoading WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + " ) " +
                                 "  + " +
                                 " (SELECT COUNT(*)  FROM finalLoading WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + " ) AS slipCount";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                Int64 slipCount = 0;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (reader.Read())
                {
                    TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                    if (reader["slipCount"] != DBNull.Value)
                        slipCount = Convert.ToInt64(reader["slipCount"].ToString());
                }

                return slipCount;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingTO> SelectTblLoadingDatewise(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = "select modbusRefId from tempLoading where CAST(createdOn as date) = " +"\'" + date + "\'"  + " ORDER BY modbusRefId DESC  ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToListModbus(reader);

                if (list != null)
                {
                    return list;
                }

                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public static SalesTrackerAPI.DashboardModels.LoadingInfo SelectDashboardLoadingInfo(TblUserRoleTO tblUserRoleTO, Int32 orgId, DateTime sysDate, Int32 loadingType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblLoadingTODT = null;
            String whereCond = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                //if (tblUserRoleTO.RoleId == (int)Constants.SystemRolesE.C_AND_F_AGENT)
                if (orgId > 0)
                {
                    whereCond = " AND cnfOrgId=" + orgId;
                }
                conn.Open();

                if (isConfEn == 1)
                {
                    cmdSelect.CommandText = " SELECT SUM(totalLoadingQty) totalLoadingQty,SUM(confimedLoadingQty) confimedLoadingQty,SUM(notconfimedLoadingQty) notconfimedLoadingQty,SUM(deliveredQty) deliveredQty " +
                                            " FROM (" +
                                            " SELECT ISNULL(SUM(loadingQty),0) totalLoadingQty , " +
                                            " ISNULL(SUM(CASE WHEN tempLoading.statusId IN(7, 14, 15, 16, 17, 20,24,25) THEN loadingQty ELSE 0 END),0) AS confimedLoadingQty " +
                                            " , ISNULL(SUM(CASE WHEN tempLoading.statusId IN(4, 5, 6) THEN loadingQty ELSE 0 END),0) AS notconfimedLoadingQty " +
                                            " , ISNULL(SUM(CASE WHEN tempLoading.statusId IN(17) THEN loadingQty ELSE 0 END),0) AS deliveredQty " +
                                            " FROM tempLoading " +
                                            " INNER JOIN tempLoadingSlip ON idLoading = loadingId " +
                                            " INNER JOIN tempLoadingSlipDtl ON idLoadingSlip = loadingSlipId " +
                                            " INNER JOIN " +
                                            " ( " +
                                            " SELECT areaConf.cnfOrgId, idOrganization " +
                                            " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                            " INNER JOIN " +
                                            " ( " +
                                            "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                            "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                                            "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                            "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                            " ) AS userAreaDealer On userAreaDealer.cnfOrgId = tempLoading.cnFOrgid AND tempLoadingSlip.dealerOrgId = userAreaDealer.idOrganization " +
                                            " WHERE DAY(tempLoading.createdOn) = " + sysDate.Day + " AND MONTH(tempLoading.createdOn) = " + sysDate.Month + " AND YEAR(tempLoading.createdOn) = " + sysDate.Year + " " + whereCond +

                                            // Vaibhav [20-Nov-2017] Added to select from finalLoading and finalLoadingSlip and finalLoadingSlipDtl
                                            " UNION ALL " +

                                            " SELECT ISNULL(SUM(loadingQty),0) totalLoadingQty , " +
                                            " ISNULL(SUM(CASE WHEN finalLoading.statusId IN(7, 14, 15, 16, 17, 20,24,25) THEN loadingQty ELSE 0 END),0) AS confimedLoadingQty " +
                                            " , ISNULL(SUM(CASE WHEN finalLoading.statusId IN(4, 5, 6) THEN loadingQty ELSE 0 END),0) AS notconfimedLoadingQty " +
                                            " , ISNULL(SUM(CASE WHEN finalLoading.statusId IN(17) THEN loadingQty ELSE 0 END),0) AS deliveredQty " +
                                            " FROM finalLoading " +
                                            " INNER JOIN finalLoadingSlip ON idLoading = loadingId " +
                                            " INNER JOIN finalLoadingSlipDtl ON idLoadingSlip = loadingSlipId " +
                                            " INNER JOIN " +
                                            " ( " +
                                            " SELECT areaConf.cnfOrgId, idOrganization " +
                                            " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                            " INNER JOIN " +
                                            " ( " +
                                            "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                            "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                                            "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                            "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                            " ) AS userAreaDealer On userAreaDealer.cnfOrgId = finalLoading.cnFOrgid AND finalLoadingSlip.dealerOrgId = userAreaDealer.idOrganization " +
                                            " WHERE DAY(finalLoading.createdOn) = " + sysDate.Day + " AND MONTH(finalLoading.createdOn) = " + sysDate.Month + " AND YEAR(finalLoading.createdOn) = " + sysDate.Year + " " + whereCond +
                                            " ) AS TOTALRES";

                }
                else
                {

                    if (loadingType == (int)Constants.LoadingTypeE.REGULAR)
                    {
                        cmdSelect.CommandText = " SELECT SUM(totalLoadingQty) totalLoadingQty,SUM(confimedLoadingQty) confimedLoadingQty,SUM(notconfimedLoadingQty) notconfimedLoadingQty,SUM(deliveredQty) deliveredQty " +
                                            " FROM (" +
                                            " SELECT ISNULL(SUM(totalLoadingQty),0) totalLoadingQty , " +
                                           " ISNULL(SUM(CASE WHEN statusId IN(7, 14, 15, 16, 17,20,24,25) THEN totalLoadingQty ELSE 0 END),0) AS confimedLoadingQty, " +
                                           " ISNULL(SUM(CASE WHEN statusId IN(4, 5, 6) THEN totalLoadingQty ELSE 0 END),0) AS notconfimedLoadingQty, " +
                                           " ISNULL(SUM(CASE WHEN statusId IN(17) THEN totalLoadingQty ELSE 0 END),0) AS deliveredQty " +
                                           " FROM tempLoading " +
                                           " WHERE DAY(createdOn)= " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + whereCond + "AND loadingType=1" +

                                          // Vaibhav [20-Nov-2017] Added to select from finalLoading
                                          " UNION ALL " +

                                          " SELECT ISNULL(SUM(totalLoadingQty),0) totalLoadingQty , " +
                                          " ISNULL(SUM(CASE WHEN statusId IN(7, 14, 15, 16, 17,20,24,25) THEN totalLoadingQty ELSE 0 END),0) AS confimedLoadingQty, " +
                                          " ISNULL(SUM(CASE WHEN statusId IN(4, 5, 6) THEN totalLoadingQty ELSE 0 END),0) AS notconfimedLoadingQty, " +
                                          " ISNULL(SUM(CASE WHEN statusId IN(17) THEN totalLoadingQty ELSE 0 END),0) AS deliveredQty " +
                                          " FROM finalLoading " +
                                          " WHERE DAY(createdOn)= " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + whereCond + "AND loadingType=1" +
                                          ")AS TOTALRES";


                    }
                    else if (loadingType == (int)Constants.LoadingTypeE.OTHER)
                    {
                        cmdSelect.CommandText = " SELECT SUM(totalLoadingQty) totalLoadingQty,SUM(confimedLoadingQty) confimedLoadingQty,SUM(notconfimedLoadingQty) notconfimedLoadingQty,SUM(deliveredQty) deliveredQty " +
                                            " FROM (" +
                                            " SELECT ISNULL(SUM(totalLoadingQty),0) totalLoadingQty , " +
                                         " ISNULL(SUM(CASE WHEN statusId IN(7, 14, 15, 16, 17,20,24,25) THEN totalLoadingQty ELSE 0 END),0) AS confimedLoadingQty, " +
                                         " ISNULL(SUM(CASE WHEN statusId IN(4, 5, 6) THEN totalLoadingQty ELSE 0 END),0) AS notconfimedLoadingQty, " +
                                         " ISNULL(SUM(CASE WHEN statusId IN(17) THEN totalLoadingQty ELSE 0 END),0) AS deliveredQty " +
                                         " FROM tempLoading " +
                                         " WHERE DAY(createdOn)= " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + whereCond + "AND loadingType=2" +

                                         // Vaibhav [20-Nov-2017] Added to select from finalLoading
                                         " UNION ALL " +

                                         " SELECT ISNULL(SUM(totalLoadingQty),0) totalLoadingQty , " +
                                         " ISNULL(SUM(CASE WHEN statusId IN(7, 14, 15, 16, 17,20,24,25) THEN totalLoadingQty ELSE 0 END),0) AS confimedLoadingQty, " +
                                         " ISNULL(SUM(CASE WHEN statusId IN(4, 5, 6) THEN totalLoadingQty ELSE 0 END),0) AS notconfimedLoadingQty, " +
                                         " ISNULL(SUM(CASE WHEN statusId IN(17) THEN totalLoadingQty ELSE 0 END),0) AS deliveredQty " +
                                         " FROM finalLoading " +
                                         " WHERE DAY(createdOn)= " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + whereCond + "AND loadingType=2" +
                                         ") AS TOTALRES";

                    }

                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblLoadingTODT.Read())
                {
                    SalesTrackerAPI.DashboardModels.LoadingInfo tblLoadingTONew = new SalesTrackerAPI.DashboardModels.LoadingInfo();
                    if (tblLoadingTODT["totalLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalLoadingQty = Convert.ToDouble(tblLoadingTODT["totalLoadingQty"].ToString());
                    if (tblLoadingTODT["confimedLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalConfirmedLoadingQty = Convert.ToDouble(tblLoadingTODT["confimedLoadingQty"].ToString());
                    if (tblLoadingTODT["notconfimedLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.NotconfimedLoadingQty = Convert.ToDouble(tblLoadingTODT["notconfimedLoadingQty"].ToString());
                    if (tblLoadingTODT["deliveredQty"] != DBNull.Value)
                        tblLoadingTONew.TotalDeliveredQty = Convert.ToDouble(tblLoadingTODT["deliveredQty"].ToString());

                    tblLoadingTONew.TotalPendingQty = tblLoadingTONew.TotalConfirmedLoadingQty - tblLoadingTONew.TotalDeliveredQty;

                    return tblLoadingTONew;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingTODT != null)
                    tblLoadingTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, DateTime loadingDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();

                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" + " AND DAY(loading.createdOn)=" + loadingDate.Day +
                //                        " AND MONTH(loading.createdOn) = " + loadingDate.Month + " AND YEAR(loading.createdOn)=" + loadingDate.Year;

                // Vaibhav commented and modifed for data separation activity
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'" + " AND DAY(sq1.createdOn)=" + loadingDate.Day +
                        " AND MONTH(sq1.createdOn) = " + loadingDate.Month + " AND YEAR(sq1.createdOn)=" + loadingDate.Year;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


       

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading,int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string sqlQuery = null;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : START*/
            //String statusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            //Saket [2018-01-29] These is used for Vehicle Validation. So no Need to add new status.
            //String strStatusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED +","+ (int)Constants.TranStatusE.LOADING_IN_PROGRESS + "," + (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH;
            String strStatusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            string statusIdsForIn = (int)Constants.TranStatusE.LOADING_GATE_IN + "," + (int)Constants.TranStatusE.LOADING_CONFIRM + "," + (int)Constants.TranStatusE.LOADING_IN_PROGRESS + "," + (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : END*/
            //Ramdas.w @20122017: loading status chenged when first item weight taken 


            try
            {
                ////sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" + " AND loading.statusId NOT IN(" + statusIds + ")";
                //sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'";
                ////           " AND loading.statusId = " + statusIds;

                // Vaibhav comment and modify for data separation activity
                //@Kiran 13/12/2018 Commented for change vehical number condition
                //sqlQuery = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'"; //sq1.idLoading = 35834--

                //@Kiran 13/12/2018 Added for change vehical number condition to lodingId
                if(loadingId == 0)
                {
                   sqlQuery = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'"; //sq1.idLoading = 35834--
                }else
                {
                    sqlQuery = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.idLoading = " + loadingId;//--
                }

                if (isAllowNxtLoading)
                {
                    sqlQuery += " AND sq1.statusId NOT IN(" + strStatusIds + ")";
                    //" AND loading.isAllowNxtLoading = 0";
                }
                else
                {
                    sqlQuery += " AND sq1.statusId IN (" + statusIdsForIn + ")";
                    /*GJ@20170830 : Below Conditions for to check Supervisor is assigned or not*/
                    //sqlQuery += " AND ISNULL(loading.superwisorId ,0)> 0";
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut(string vehicleNo, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string sqlQuery = null;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : START*/
            //String statusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            string statusIdsForIn = (int)Constants.TranStatusE.LOADING_COMPLETED + "";
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : END*/
            try
            {

                ////sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" + " AND loading.statusId NOT IN(" + statusIds + ")";
                //sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" +
                //           " AND loading.statusId = " + (int)Constants.TranStatusE.LOADING_COMPLETED + "";

                //Vaibhav comment and modify
                sqlQuery = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'" +
                           " AND sq1.statusId = " + (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH + "";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut(int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string sqlQuery = null;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : START*/
            //String statusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            string statusIdsForIn = (int)Constants.TranStatusE.LOADING_COMPLETED + "";
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : END*/
            try
            {

                ////sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" + " AND loading.statusId NOT IN(" + statusIds + ")";
                //sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" +
                //           " AND loading.statusId = " + (int)Constants.TranStatusE.LOADING_COMPLETED + "";

                //Vaibhav comment and modify
                sqlQuery = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.idLoading ='" + loadingId + "'" +
                           " AND sq1.statusId = " + (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH + "";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingTO> SelectAllInLoadingListByVehicleNo(string vehicleNo)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : START*/
            //String statusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            int statusIds = (int)Constants.TranStatusE.LOADING_GATE_IN;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : END*/
            try
            {
                conn.Open();

                // Vaibhav 
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'" + " AND sq1.statusId = " + statusIds;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<Int32, Int32> SelectCountOfLoadingsOfSuperwisor(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                Dictionary<Int32, Int32> DCT = new Dictionary<int, Int32>();
                cmdSelect.CommandText = " SELECT superwisorId ,COUNT(*) as slipCount FROM tempLoading WHERE statusId =" + (int)Constants.TranStatusE.LOADING_GATE_IN + " AND DAY(updatedOn)=" + date.Day + " AND MONTH(updatedOn)=" + date.Month + " AND YEAR(updatedOn)=" + date.Year +
                                        " GROUP BY superwisorId";


                //// Vaibhav [20-Nov-2017] comment Added to select count from finalLoading

                //String sqlQuery = "SELECT ( SELECT superwisorId ,COUNT(*) FROM tempLoading WHERE statusId =" + (int)Constants.TranStatusE.LOADING_GATE_IN + " AND DAY(updatedOn)=" + date.Day + " AND MONTH(updatedOn)=" + date.Month + " AND YEAR(updatedOn)=" + date.Year +
                //                    " GROUP BY superwisorId ) + " +
                //                    "( SELECT superwisorId ,COUNT(*)  FROM tempLoading WHERE statusId =" + (int)Constants.TranStatusE.LOADING_GATE_IN + " AND DAY(updatedOn)=" + date.Day + " AND MONTH(updatedOn)=" + date.Month + " AND YEAR(updatedOn)=" + date.Year +
                //                    " GROUP BY superwisorId ) AS slipCount";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                Int32 slipCount = 0;
                Int32 superwisorId = 0;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (reader.Read())
                {
                    TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                    if (reader["superwisorId"] != DBNull.Value)
                        superwisorId = Convert.ToInt32(reader["superwisorId"].ToString());
                    if (reader["slipCount"] != DBNull.Value)
                        slipCount = Convert.ToInt32(reader["slipCount"].ToString());

                    DCT.Add(superwisorId, slipCount);
                }

                return DCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public static List<TblLoadingTO> ConvertDTToListModbus(SqlDataReader tblLoadingTODT)
        {
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
            if (tblLoadingTODT != null)
            {
                TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                while (tblLoadingTODT.Read())
                {
                    if (tblLoadingTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingTONew.ModbusRefId = Convert.ToInt32(tblLoadingTODT["modbusRefId"]);
                    tblLoadingTOList.Add(tblLoadingTONew);
                }
            }
            return tblLoadingTOList;
        }

        public static List<TblLoadingTO> ConvertDTToList(SqlDataReader tblLoadingTODT)
        {
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
            if (tblLoadingTODT != null)
            {
                while (tblLoadingTODT.Read())
                {
                    TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                    if (tblLoadingTODT["idLoading"] != DBNull.Value)
                        tblLoadingTONew.IdLoading = Convert.ToInt32(tblLoadingTODT["idLoading"].ToString());
                    if (tblLoadingTODT["isJointDelivery"] != DBNull.Value)
                        tblLoadingTONew.IsJointDelivery = Convert.ToInt32(tblLoadingTODT["isJointDelivery"].ToString());
                    if (tblLoadingTODT["noOfDeliveries"] != DBNull.Value)
                        tblLoadingTONew.NoOfDeliveries = Convert.ToInt32(tblLoadingTODT["noOfDeliveries"].ToString());
                    if (tblLoadingTODT["statusId"] != DBNull.Value)
                        tblLoadingTONew.StatusId = Convert.ToInt32(tblLoadingTODT["statusId"].ToString());
                    if (tblLoadingTODT["createdBy"] != DBNull.Value)
                        tblLoadingTONew.CreatedBy = Convert.ToInt32(tblLoadingTODT["createdBy"].ToString());
                    if (tblLoadingTODT["updatedBy"] != DBNull.Value)
                        tblLoadingTONew.UpdatedBy = Convert.ToInt32(tblLoadingTODT["updatedBy"].ToString());
                    if (tblLoadingTODT["statusDate"] != DBNull.Value)
                        tblLoadingTONew.StatusDate = Convert.ToDateTime(tblLoadingTODT["statusDate"].ToString());
                    if (tblLoadingTODT["loadingDatetime"] != DBNull.Value)
                        tblLoadingTONew.LoadingDatetime = Convert.ToDateTime(tblLoadingTODT["loadingDatetime"].ToString());
                    if (tblLoadingTODT["createdOn"] != DBNull.Value)
                        tblLoadingTONew.CreatedOn = Convert.ToDateTime(tblLoadingTODT["createdOn"].ToString());
                    if (tblLoadingTODT["updatedOn"] != DBNull.Value)
                        tblLoadingTONew.UpdatedOn = Convert.ToDateTime(tblLoadingTODT["updatedOn"].ToString());
                    if (tblLoadingTODT["loadingSlipNo"] != DBNull.Value)
                        tblLoadingTONew.LoadingSlipNo = Convert.ToString(tblLoadingTODT["loadingSlipNo"].ToString());
                    if (tblLoadingTODT["vehicleNo"] != DBNull.Value)
                        tblLoadingTONew.VehicleNo = Convert.ToString(tblLoadingTODT["vehicleNo"].ToString().ToUpper());
                    if (tblLoadingTODT["statusReason"] != DBNull.Value)
                        tblLoadingTONew.StatusReason = Convert.ToString(tblLoadingTODT["statusReason"].ToString());

                    if (tblLoadingTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingTONew.CnfOrgId = Convert.ToInt32(tblLoadingTODT["cnfOrgId"].ToString());
                    if (tblLoadingTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingTONew.CnfOrgName = Convert.ToString(tblLoadingTODT["cnfOrgName"].ToString());
                    if (tblLoadingTODT["totalLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalLoadingQty = Convert.ToDouble(tblLoadingTODT["totalLoadingQty"].ToString());

                    if (tblLoadingTODT["statusName"] != DBNull.Value)
                        tblLoadingTONew.StatusDesc = Convert.ToString(tblLoadingTODT["statusName"].ToString());
                    if (tblLoadingTODT["statusReasonId"] != DBNull.Value)
                        tblLoadingTONew.StatusReasonId = Convert.ToInt32(tblLoadingTODT["statusReasonId"].ToString());
                    if (tblLoadingTODT["transporterOrgId"] != DBNull.Value)
                        tblLoadingTONew.TransporterOrgId = Convert.ToInt32(tblLoadingTODT["transporterOrgId"].ToString());
                    if (tblLoadingTODT["freightAmt"] != DBNull.Value)
                        tblLoadingTONew.FreightAmt = Convert.ToDouble(tblLoadingTODT["freightAmt"].ToString());

                    if (tblLoadingTODT["transporterOrgName"] != DBNull.Value)
                        tblLoadingTONew.TransporterOrgName = Convert.ToString(tblLoadingTODT["transporterOrgName"].ToString());

                    if (tblLoadingTODT["superwisorId"] != DBNull.Value)
                        tblLoadingTONew.SuperwisorId = Convert.ToInt32(tblLoadingTODT["superwisorId"].ToString());
                    if (tblLoadingTODT["superwisorName"] != DBNull.Value)
                        tblLoadingTONew.SuperwisorName = Convert.ToString(tblLoadingTODT["superwisorName"].ToString());
                    if (tblLoadingTODT["isFreightIncluded"] != DBNull.Value)
                        tblLoadingTONew.IsFreightIncluded = Convert.ToInt32(tblLoadingTODT["isFreightIncluded"].ToString());

                    if (tblLoadingTODT["contactNo"] != DBNull.Value)
                        tblLoadingTONew.ContactNo = Convert.ToString(tblLoadingTODT["contactNo"].ToString());
                    if (tblLoadingTODT["driverName"] != DBNull.Value)
                        tblLoadingTONew.DriverName = Convert.ToString(tblLoadingTODT["driverName"].ToString());

                    if (tblLoadingTODT["digitalSign"] != DBNull.Value)
                        tblLoadingTONew.DigitalSign = Convert.ToString(tblLoadingTODT["digitalSign"].ToString());
                    if (tblLoadingTODT["userDisplayName"] != DBNull.Value)
                        tblLoadingTONew.CreatedByUserName = Convert.ToString(tblLoadingTODT["userDisplayName"].ToString());
                    if (tblLoadingTODT["parentLoadingId"] != DBNull.Value)
                        tblLoadingTONew.ParentLoadingId = Convert.ToInt32(tblLoadingTODT["parentLoadingId"].ToString());
                    if (tblLoadingTODT["callFlag"] != DBNull.Value)
                        tblLoadingTONew.CallFlag = Convert.ToInt32(tblLoadingTODT["callFlag"].ToString());
                    if (tblLoadingTODT["flagUpdatedOn"] != DBNull.Value)
                        tblLoadingTONew.FlagUpdatedOn = Convert.ToDateTime(tblLoadingTODT["flagUpdatedOn"].ToString());
                    if (tblLoadingTODT["isAllowNxtLoading"] != DBNull.Value)
                        tblLoadingTONew.IsAllowNxtLoading = Convert.ToInt32(tblLoadingTODT["isAllowNxtLoading"].ToString());
                    if (tblLoadingTODT["loadingType"] != DBNull.Value)
                        tblLoadingTONew.LoadingType = Convert.ToInt32(tblLoadingTODT["loadingType"]);
                    if (tblLoadingTODT["currencyId"] != DBNull.Value)
                        tblLoadingTONew.CurrencyId = Convert.ToInt32(tblLoadingTODT["currencyId"]);
                    if (tblLoadingTODT["currencyRate"] != DBNull.Value)
                        tblLoadingTONew.CurrencyRate = Convert.ToDouble(tblLoadingTODT["currencyRate"]);
                    if (tblLoadingTODT["callFlagBy"] != DBNull.Value)
                        tblLoadingTONew.CallFlagBy = Convert.ToInt32(tblLoadingTODT["callFlagBy"]);
                    if (tblLoadingTODT["notifyByName"] != DBNull.Value)
                       tblLoadingTONew.NotifyfiedUserName = Convert.ToString(tblLoadingTODT["notifyByName"].ToString());
                    if (tblLoadingTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingTONew.ModbusRefId = Convert.ToInt32(tblLoadingTODT["modbusRefId"]);

                    if (tblLoadingTODT["gateId"] != DBNull.Value)
                        tblLoadingTONew.GateId = Convert.ToInt32(tblLoadingTODT["gateId"]);
                    if (tblLoadingTODT["portNumber"] != DBNull.Value)
                        tblLoadingTONew.PortNumber = Convert.ToString(tblLoadingTODT["portNumber"]);
                    if (tblLoadingTODT["ioTUrl"] != DBNull.Value)
                        tblLoadingTONew.IoTUrl = Convert.ToString(tblLoadingTODT["ioTUrl"]);
                    if (tblLoadingTODT["machineIP"] != DBNull.Value)
                        tblLoadingTONew.MachineIP = Convert.ToString(tblLoadingTODT["machineIP"]);
                    if (tblLoadingTODT["isDBup"] != DBNull.Value)
                        tblLoadingTONew.IsDBup = Convert.ToInt32(tblLoadingTODT["isDBup"]);
                    if (tblLoadingTODT["fromOrgId"] != DBNull.Value)
                        tblLoadingTONew.FromOrgId = Convert.ToInt32(tblLoadingTODT["fromOrgId"]);
                    if (tblLoadingTODT["isInternalCnf"] != DBNull.Value)
                        tblLoadingTONew.IsInternalCnf = Convert.ToInt32(tblLoadingTODT["isInternalCnf"]);
                    if (tblLoadingTODT["fromOrgName"] != DBNull.Value)
                        tblLoadingTONew.FromOrgName = Convert.ToString(tblLoadingTODT["fromOrgName"]);

                    tblLoadingTOList.Add(tblLoadingTONew);
                }
            }
            return tblLoadingTOList;
        }

        public static List<VehicleNumber> SelectAllVehicles()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT distinct vehicleNo FROM tempLoading";

                                       //// Vaibhav [20-Nov-2017] Added to select from finalLoading
                                       //" UNION ALL " +
                                       //"SELECT distinct vehicleNo FROM finalLoading";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<VehicleNumber> list = new List<VehicleNumber>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        String vehicleNo = string.Empty;
                        if (sqlReader["vehicleNo"] != DBNull.Value)
                            vehicleNo = Convert.ToString(sqlReader["vehicleNo"].ToString());

                        if (!string.IsNullOrEmpty(vehicleNo))
                        {
                            String[] vehNoPart = vehicleNo.Split(' ');
                            if (vehNoPart.Length == 4)
                            {
                                VehicleNumber vehicleNumber = new VehicleNumber();
                                for (int i = 0; i < vehNoPart.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        vehicleNumber.StateCode = vehNoPart[i].ToUpper();
                                    }
                                    if (i == 1)
                                    {
                                        vehicleNumber.DistrictCode = vehNoPart[i].ToUpper();
                                    }
                                    if (i == 2)
                                    {
                                        vehicleNumber.UniqueLetters = vehNoPart[i];
                                        if (vehicleNumber.UniqueLetters == "undefined")
                                            vehicleNumber.UniqueLetters = "";
                                        else
                                            vehicleNumber.UniqueLetters = vehicleNumber.UniqueLetters.ToUpper();
                                    }
                                    if (i == 3)
                                    {
                                        if (Constants.IsInteger(vehNoPart[i]))
                                        {
                                            vehicleNumber.VehicleNo = Convert.ToInt32(vehNoPart[i]);
                                        }
                                        else break;
                                    }
                                }

                                if (vehicleNumber.VehicleNo > 0)
                                    list.Add(vehicleNumber);
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> SelectAllVehiclesListByStatus(int statusId , int status)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav [15-Sep-2017] Added condition for displaying new unloading records.
                if (statusId != (int)Constants.TranStatusE.UNLOADING_NEW)
                {
                    //cmdSelect.CommandText = "SELECT idLoading, vehicleNo FROM tblLoading WHERE statusId =" + statusId;
                    cmdSelect.CommandText = "SELECT idLoading, vehicleNo FROM tempLoading WHERE statusId = " + (int)Constants.TranStatusE.LOADING_GATE_IN +
                                            " AND ISNULL(isAllowNxtLoading,0) = 0" +
                                            " UNION ALL " +
                                            " SELECT distinct tbl.idLoading, tbl.vehicleNo from tempLoading tbl" +
                                            " LEFT OUTER JOIN tempLoading tbl1 ON tbl.vehicleNo = tbl1.vehicleNo" +
                                            //" AND tbl1.statusId =" + (int)Constants.TranStatusE.LOADING_COMPLETED + 
                                            " AND tbl1.isAllowNxtLoading = 1 " +
                                            " AND tbl.statusId =" + (int)Constants.TranStatusE.LOADING_CONFIRM + " OR tbl.statusId=" + (int)Constants.TranStatusE.LOADING_IN_PROGRESS + 
                                            " OR tbl.statusId=" + (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH + // Saket [2018-01-25] Added For Final Weighing.
                                            " WHERE tbl1.vehicleNo IS NOT NULL" +
                                            " AND ISNULL(tbl.isAllowNxtLoading,0) = 0";  //Saket [2018-01-29] Added as same vehicale no showing twice in weighing if Allow to new laoding against same vehilce
                                               //cmdSelect.CommandText = "SELECT * FROM (SELECT idLoading, vehicleNo , superwisorId FROM tblLoading " +
                                               //                         "WHERE statusId = 15 AND ISNULL(isAllowNxtLoading,0) = 0 " +
                                               //                         "UNION ALL SELECT tbl.idLoading, tbl.vehicleNo, tbl.superwisorId " +
                                               //                         "FROM tblLoading tbl LEFT OUTER JOIN tblLoading tbl1 ON tbl.vehicleNo = tbl1.vehicleNo " +
                                               //                         "AND tbl1.isAllowNxtLoading = 1  " +
                                               //                         "AND tbl.statusId ="+ (int)Constants.TranStatusE.LOADING_CONFIRM + "WHERE tbl1.vehicleNo IS NOT NULL) as b " +
                                               //                         "where 1 = 1 AND ISNULL(superwisorId ,0)> 0";

                                               //// Vaibhav [20-Nov-2017] Added to select from finalLoading
                                               //" UNION ALL " +

                                               //"SELECT idLoading, vehicleNo FROM finalLoading WHERE statusId = " + (int)Constants.TranStatusE.LOADING_GATE_IN +
                                               //" AND ISNULL(isAllowNxtLoading,0) = 0" +
                                               //" UNION ALL " +
                                               //" SELECT distinct tbl.idLoading, tbl.vehicleNo from finalLoading tbl" +
                                               //" LEFT OUTER JOIN finalLoading tbl1 ON tbl.vehicleNo = tbl1.vehicleNo" +
                                               ////" AND tbl1.statusId =" + (int)Constants.TranStatusE.LOADING_COMPLETED + 
                                               //" AND tbl1.isAllowNxtLoading = 1 " +
                                               //" AND tbl.statusId =" + (int)Constants.TranStatusE.LOADING_CONFIRM +
                                               //" WHERE tbl1.vehicleNo IS NOT NULL";

                }
                else
                {
                    cmdSelect.CommandText = " SELECT vehicleNo,idUnLoading AS 'idLoading' ,org.firmName AS orgName FROM tblUnLoading tblunload " +
                                            " INNER JOIN tblOrganization org ON org.idOrganization=tblunload.SupplierOrgId " +
                                            " WHERE statusId = " + (int)Constants.TranStatusE.UNLOADING_NEW + " order by idUnLoading desc ";
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        DropDownTO dropDownTo = new DropDownTO();
                        String vehicleNo = string.Empty;
                        if (sqlReader["vehicleNo"] != DBNull.Value)
                            dropDownTo.Text = Convert.ToString(sqlReader["vehicleNo"].ToString());
                        if (sqlReader["idLoading"] != DBNull.Value)
                            dropDownTo.Value = Convert.ToInt32(sqlReader["idLoading"].ToString());
                        if ((sqlReader.FieldCount > 2))
                        {
                            if (sqlReader["orgName"] != DBNull.Value)
                                dropDownTo.Tag = sqlReader["orgName"].ToString();
                        }
                        list.Add(dropDownTo);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        // Vaibhav [29-Nov-2017] Added to select all temp loading details.
        public static List<TblLoadingTO> SelectAllTempLoading(SqlConnection conn, SqlTransaction tran,DateTime migrateBeforeDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                String sqlQuery = " SELECT loading.* , fromOrgNameTbl.firmName as fromOrgName  ,org.digitalSign, org.firmName as cnfOrgName, org.isInternalCnf,transOrg.firmName as transporterOrgName ," +
                                  " dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,createdUser.userDisplayName, tblUserCallFlag.userDisplayName AS notifyByName " +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM tempLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser tblUserCallFlag ON tblUserCallFlag.idUser = loading.callFlagBy "+
                                  " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate=loading.gateId " +
                                  //Prajakta [2021-06-29] Added to show orgName on loading slip
                                  " LEFT JOIN tblOrganization fromOrgNameTbl on fromOrgNameTbl.idOrganization = loading.fromOrgId " +
                                  " WHERE loading.statusId IN " + 
                                  " ( "+(int)Constants.TranStatusE.LOADING_DELIVERED +","+ (int)Constants.TranStatusE.LOADING_CANCEL + ")"+
                                  " AND  CONVERT (DATE,statusDate,103) <= @StatusDate " +
                                  " ORDER BY idloading ASC ";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@StatusDate", SqlDbType.DateTime).Value = migrateBeforeDate.Date.ToString(Constants.AzureDateFormat);
            
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        //Sudhir[18-APR-2018] Added for Get Graphwise Loading Status Data .
        public static List<SalesTrackerAPI.DashboardModels.LoadingInfo> SelectLoadingStatusGraph(TblUserRoleTO tblUserRoleTO, Int32 cnforgId, DateTime fromDate, DateTime toDate, Int32 loadingType, Int32 dealerOrgId = 0)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblLoadingTODT = null;
            String whereCond = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                if (cnforgId > 0 && isConfEn != 1)
                {
                    whereCond = " AND cnfOrgId=" + cnforgId;
                }
                conn.Open();

                if (isConfEn == 1)
                {
                    cmdSelect.CommandText = " SELECT organnization.firmName,cnfOrgId, SUM(totalLoadingQty) totalLoadingQty,SUM(confimedLoadingQty) confimedLoadingQty,SUM(notconfimedLoadingQty) notconfimedLoadingQty,SUM(deliveredQty) deliveredQty " +
                                            " FROM (" +
                                            " SELECT userAreaDealer.cnfOrgId,ISNULL(SUM(loadingQty),0) totalLoadingQty , " +
                                            " ISNULL(SUM(CASE WHEN tempLoading.statusId IN(7, 14, 15, 16, 17, 20,24,25) THEN loadingQty ELSE 0 END),0) AS confimedLoadingQty " +
                                            " , ISNULL(SUM(CASE WHEN tempLoading.statusId IN(4, 5, 6) THEN loadingQty ELSE 0 END),0) AS notconfimedLoadingQty " +
                                            " , ISNULL(SUM(CASE WHEN tempLoading.statusId IN(17) THEN loadingQty ELSE 0 END),0) AS deliveredQty " +
                                            " FROM tempLoading " +
                                            " INNER JOIN tempLoadingSlip ON idLoading = loadingId " +
                                            " INNER JOIN tempLoadingSlipDtl ON idLoadingSlip = loadingSlipId " +
                                            " INNER JOIN " +
                                            " ( " +
                                            " SELECT areaConf.cnfOrgId, idOrganization " +
                                            " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                            " INNER JOIN " +
                                            " ( " +
                                            "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                            "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                                            "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                            "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                            " ) AS userAreaDealer On userAreaDealer.cnfOrgId = tempLoading.cnFOrgid AND tempLoadingSlip.dealerOrgId = userAreaDealer.idOrganization " +
                                            " WHERE CONVERT (DATE,tempLoading.createdOn,103) BETWEEN @fromDate AND @toDate " + whereCond + " GROUP BY userAreaDealer.cnfOrgId " +

                                            // Vaibhav [20-Nov-2017] Added to select from finalLoading and finalLoadingSlip and finalLoadingSlipDtl
                    " UNION ALL " +

                                            " SELECT userAreaDealer.cnfOrgId,ISNULL(SUM(loadingQty),0) totalLoadingQty , " +
                                            " ISNULL(SUM(CASE WHEN finalLoading.statusId IN(7, 14, 15, 16, 17, 20,24,25) THEN loadingQty ELSE 0 END),0) AS confimedLoadingQty " +
                                            " , ISNULL(SUM(CASE WHEN finalLoading.statusId IN(4, 5, 6) THEN loadingQty ELSE 0 END),0) AS notconfimedLoadingQty " +
                                            " , ISNULL(SUM(CASE WHEN finalLoading.statusId IN(17) THEN loadingQty ELSE 0 END),0) AS deliveredQty " +
                                            " FROM finalLoading " +
                                            " INNER JOIN finalLoadingSlip ON idLoading = loadingId " +
                                            " INNER JOIN finalLoadingSlipDtl ON idLoadingSlip = loadingSlipId " +
                                            " INNER JOIN " +
                                            " ( " +
                                            " SELECT areaConf.cnfOrgId, idOrganization " +
                                            " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                            " INNER JOIN " +
                                            " ( " +
                                            "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                            "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                                            "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                            "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                            " ) AS userAreaDealer On userAreaDealer.cnfOrgId = finalLoading.cnFOrgid AND finalLoadingSlip.dealerOrgId = userAreaDealer.idOrganization " +
                                            " WHERE CONVERT (DATE,finalLoading.createdOn,103) BETWEEN @fromDate AND @toDate " + whereCond + " GROUP BY userAreaDealer.cnfOrgId " +
                                            " ) AS TOTALRES" +
                                            " LEFT JOIN tblOrganization organnization ON TOTALRES.cnfOrgId = organnization.idOrganization " +
                                            " GROUP BY cnfOrgId,organnization.firmName ";
                }
                else
                {

                    if (loadingType == (int)Constants.LoadingTypeE.REGULAR)
                    {
                        cmdSelect.CommandText = " SELECT organnization.firmName,cnfOrgId, SUM(totalLoadingQty) totalLoadingQty, " +
                                            " SUM(confimedLoadingQty) confimedLoadingQty, SUM(notconfimedLoadingQty) notconfimedLoadingQty," +
                                            " SUM(deliveredQty) deliveredQty FROM " +
                                            " (" +
                                            " SELECT cnfOrgId, ISNULL(SUM(tempLoadingSlipDtl.loadingQty), 0) totalLoadingQty, " +
                                            "ISNULL(SUM(CASE WHEN tempLoadingSlip.isConfirmed=1 THEN tempLoadingSlipDtl.loadingQty ELSE 0 END), 0)" +
                                            " AS confimedLoadingQty, " +
                                            " ISNULL(SUM(CASE WHEN tempLoadingSlip.isConfirmed=0 THEN tempLoadingSlipDtl.loadingQty ELSE 0 END), 0) " +
                                            " AS notconfimedLoadingQty, " +
                                            " ISNULL(SUM(CASE WHEN tempLoading.statusId IN(17) THEN tempLoadingSlipDtl.loadingQty ELSE 0 END), 0) AS deliveredQty  " +
                                            " FROM tempLoadingSlip  " +
                                            " LEFT JOIN tempLoadingSlipDtl tempLoadingSlipDtl ON tempLoadingSlip.idLoadingSlip = tempLoadingSlipDtl.loadingSlipId  " +
                                            " LEFT JOIN tempLoading tempLoading ON tempLoadingSlip.loadingId = tempLoading.idLoading " +
                                            " LEFT JOIN tblOrganization organization ON organization.idOrganization = tempLoading.cnfOrgId " +
                                            " WHERE CONVERT (DATE,tempLoading.createdOn,103) BETWEEN @fromDate AND @toDate " + whereCond +
                                            " AND loadingType = 1  AND tempLoading.statusId NOT IN (1, 5, 6, 18) " +
                                            " GROUP BY cnfOrgId  " +
                                            " UNION ALL   " +
                                            " SELECT cnfOrgId, ISNULL(SUM(finalLoadingSlipDtl.loadingQty), 0) totalLoadingQty, " +
                                            " ISNULL(SUM(CASE WHEN finalLoadingSlip.isConfirmed=1 THEN finalLoadingSlipDtl.loadingQty ELSE 0 END), 0) " +
                                            " AS confimedLoadingQty,  " +
                                            " ISNULL(SUM(CASE WHEN finalLoadingSlip.isConfirmed=0 THEN finalLoadingSlipDtl.loadingQty ELSE 0 END), 0) AS notconfimedLoadingQty, " +
                                            " ISNULL(SUM(CASE WHEN finalLoading.statusId IN(17) THEN finalLoadingSlipDtl.loadingQty ELSE 0 END), 0) AS deliveredQty FROM finalLoadingSlip  " +
                                            " LEFT JOIN finalLoadingSlipDtl finalLoadingSlipDtl  ON  finalLoadingSlip.idLoadingSlip = finalLoadingSlipDtl.loadingSlipId  " +
                                            " LEFT JOIN finalLoading finalLoading ON finalLoadingSlip.loadingId = finalLoading.idLoading  " +
                                            " LEFT JOIN tblOrganization organization ON organization.idOrganization = finalLoading.cnfOrgId  " +
                                            " WHERE CONVERT (DATE,finalLoading.createdOn,103) BETWEEN @fromDate AND @toDate  " + whereCond +
                                            " AND loadingType = 1 AND finalLoading.statusId NOT IN (1, 5, 6, 18)  " +
                                            " GROUP BY cnfOrgId )" +
                                            " AS TOTALRES " +
                                            " LEFT JOIN tblOrganization organnization ON TOTALRES.cnfOrgId = organnization.idOrganization " +
                                            " GROUP BY cnfOrgId,organnization.firmName ";


                    }
                    else if (loadingType == (int)Constants.LoadingTypeE.OTHER)
                    {
                        cmdSelect.CommandText = " SELECT organnization.firmName,cnfOrgId, SUM(totalLoadingQty) totalLoadingQty, " +
                                            " SUM(confimedLoadingQty) confimedLoadingQty, SUM(notconfimedLoadingQty) notconfimedLoadingQty," +
                                            " SUM(deliveredQty) deliveredQty FROM " +
                                            " (" +
                                            " SELECT cnfOrgId, ISNULL(SUM(tempLoadingSlipDtl.loadingQty), 0) totalLoadingQty, " +
                                            "ISNULL(SUM(CASE WHEN tempLoadingSlip.isConfirmed=1 THEN tempLoadingSlipDtl.loadingQty ELSE 0 END), 0)" +
                                            " AS confimedLoadingQty, " +
                                            " ISNULL(SUM(CASE WHEN tempLoadingSlip.isConfirmed=0 THEN tempLoadingSlipDtl.loadingQty ELSE 0 END), 0) " +
                                            " AS notconfimedLoadingQty, " +
                                            " ISNULL(SUM(CASE WHEN tempLoading.statusId IN(17) THEN tempLoadingSlipDtl.loadingQty ELSE 0 END), 0) AS deliveredQty  " +
                                            " FROM tempLoadingSlip  " +
                                            " LEFT JOIN tempLoadingSlipDtl tempLoadingSlipDtl ON tempLoadingSlip.idLoadingSlip = tempLoadingSlipDtl.loadingSlipId  " +
                                            " LEFT JOIN tempLoading tempLoading ON tempLoadingSlip.loadingId = tempLoading.idLoading " +
                                            " LEFT JOIN tblOrganization organization ON organization.idOrganization = tempLoading.cnfOrgId " +
                                            " WHERE CONVERT (DATE,tempLoading.createdOn,103) BETWEEN @fromDate AND @toDate " + whereCond +
                                            " AND loadingType = 2  " +
                                            " GROUP BY cnfOrgId  " +
                                            " UNION ALL   " +
                                            " SELECT cnfOrgId, ISNULL(SUM(finalLoadingSlipDtl.loadingQty), 0) totalLoadingQty, " +
                                            " ISNULL(SUM(CASE WHEN finalLoadingSlip.isConfirmed=1 THEN finalLoadingSlipDtl.loadingQty ELSE 0 END), 0) " +
                                            " AS confimedLoadingQty,  " +
                                            " ISNULL(SUM(CASE WHEN finalLoadingSlip.isConfirmed=0 THEN finalLoadingSlipDtl.loadingQty ELSE 0 END), 0) AS notconfimedLoadingQty, " +
                                            " ISNULL(SUM(CASE WHEN finalLoading.statusId IN(17) THEN finalLoadingSlipDtl.loadingQty ELSE 0 END), 0) AS deliveredQty FROM finalLoadingSlip  " +
                                            " LEFT JOIN finalLoadingSlipDtl finalLoadingSlipDtl  ON  finalLoadingSlip.idLoadingSlip = finalLoadingSlipDtl.loadingSlipId  " +
                                            " LEFT JOIN finalLoading finalLoading ON finalLoadingSlip.loadingId = finalLoading.idLoading  " +
                                            " LEFT JOIN tblOrganization organization ON organization.idOrganization = finalLoading.cnfOrgId  " +
                                            " WHERE CONVERT (DATE,finalLoading.createdOn,103) BETWEEN @fromDate AND @toDate  " + whereCond +
                                            " AND loadingType = 2   " +
                                            " GROUP BY cnfOrgId )" +
                                            " AS TOTALRES " +
                                            " LEFT JOIN tblOrganization organnization ON TOTALRES.cnfOrgId = organnization.idOrganization " +
                                            " GROUP BY cnfOrgId,organnization.firmName ";

                    }

                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                tblLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<SalesTrackerAPI.DashboardModels.LoadingInfo> loadingInfoList = new List<DashboardModels.LoadingInfo>();
                while (tblLoadingTODT.Read())
                {
                    SalesTrackerAPI.DashboardModels.LoadingInfo tblLoadingTONew = new SalesTrackerAPI.DashboardModels.LoadingInfo();
                    if (tblLoadingTODT["firmName"] != DBNull.Value)
                        tblLoadingTONew.FirmName = tblLoadingTODT["firmName"].ToString();
                    if (tblLoadingTODT["totalLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalLoadingQty = Convert.ToDouble(tblLoadingTODT["totalLoadingQty"].ToString());
                    if (tblLoadingTODT["confimedLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalConfirmedLoadingQty = Convert.ToDouble(tblLoadingTODT["confimedLoadingQty"].ToString());
                    if (tblLoadingTODT["notconfimedLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.NotconfimedLoadingQty = Convert.ToDouble(tblLoadingTODT["notconfimedLoadingQty"].ToString());
                    if (tblLoadingTODT["deliveredQty"] != DBNull.Value)
                        tblLoadingTONew.TotalDeliveredQty = Convert.ToDouble(tblLoadingTODT["deliveredQty"].ToString());

                    tblLoadingTONew.TotalPendingQty = tblLoadingTONew.TotalConfirmedLoadingQty - tblLoadingTONew.TotalDeliveredQty;
                    loadingInfoList.Add(tblLoadingTONew);
                }

                return loadingInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingTODT != null)
                    tblLoadingTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }




        #endregion

        #region Insertion
        public static int InsertTblLoading(TblLoadingTO tblLoadingTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                tblLoadingTO.ModeId = Constants.getModeIdConfigTO();
                return ExecuteInsertionCommand(tblLoadingTO, cmdInsert);
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

        public static int InsertTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                tblLoadingTO.ModeId = Constants.getModeIdConfigTO();

                return ExecuteInsertionCommand(tblLoadingTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoadingTO tblLoadingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoading]( " +
                                "  [isJointDelivery]" +
                                " ,[noOfDeliveries]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[statusDate]" +
                                " ,[loadingDatetime]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[loadingSlipNo]" +
                                " ,[vehicleNo]" +
                                " ,[statusReason]" +
                                " ,[cnfOrgId]" +
                                " ,[totalLoadingQty]" +
                                " ,[statusReasonId]" +
                                " ,[transporterOrgId]" +
                                " ,[freightAmt]" +
                                " ,[superwisorId]" +
                                " ,[isFreightIncluded]" +
                                " ,[contactNo]" +
                                " ,[driverName]" +
                                " ,[parentLoadingId]" +
                                " ,[callFlag]" +
                                " ,[flagUpdatedOn]" +
                                " ,[isAllowNxtLoading]" +
                                " ,[loadingType]" +
                                " ,[currencyId]" +
                                " ,[currencyRate]" +
                                ",[callFlagBy]" +
                                ",[modbusRefId]"+
                                ",[gateId]" +
                                " ,[fromOrgId]" +
                                " ,[modeId]" +
                                " )" +
                    " VALUES (" +
                                "  @IsJointDelivery " +
                                " ,@NoOfDeliveries " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@StatusDate " +
                                " ,@LoadingDatetime " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@LoadingSlipNo " +
                                " ,@VehicleNo " +
                                " ,@StatusReason " +
                                " ,@cnfOrgId " +
                                " ,@totalLoadingQty " +
                                " ,@statusReasonId " +
                                " ,@transporterOrgId " +
                                " ,@freightAmt " +
                                " ,@superwisorId " +
                                " ,@isFreightIncluded " +
                                " ,@contactNo " +
                                " ,@driverName " +
                                " ,@parentLoadingId " +
                                " ,@callFlag " +
                                " ,@flagUpdatedOn " +
                                " ,@isAllowNxtLoading " +
                                " ,@loadingType " +
                                " ,@currencyId " +
                                " ,@currencyRate " +
                                ",@callFlagBy "+
                                " ,@ModbusRefId"+
                                " ,@GateId" +
                                " ,@fromOrgId " +
                                " ,@ModeId " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
            cmdInsert.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsJointDelivery;
            cmdInsert.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingTO.NoOfDeliveries;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.UpdatedBy);
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.StatusDate;
            cmdInsert.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingDatetime);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.UpdatedOn);
            cmdInsert.Parameters.Add("@LoadingSlipNo", System.Data.SqlDbType.VarChar).Value = tblLoadingTO.LoadingSlipNo;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = tblLoadingTO.VehicleNo;
            cmdInsert.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
            cmdInsert.Parameters.Add("@cnfOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CnfOrgId);
            cmdInsert.Parameters.Add("@totalLoadingQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TotalLoadingQty);
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReasonId);
            cmdInsert.Parameters.Add("@transporterOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TransporterOrgId);
            cmdInsert.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FreightAmt);
            cmdInsert.Parameters.Add("@superwisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.SuperwisorId);
            cmdInsert.Parameters.Add("@isFreightIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IsFreightIncluded);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ContactNo);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.DriverName);
            cmdInsert.Parameters.Add("@parentLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ParentLoadingId);
            cmdInsert.Parameters.Add("@callFlag", System.Data.SqlDbType.Int).Value = tblLoadingTO.CallFlag;
            cmdInsert.Parameters.Add("@flagUpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FlagUpdatedOn);
            cmdInsert.Parameters.Add("@isAllowNxtLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsAllowNxtLoading;
            cmdInsert.Parameters.Add("@loadingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingType);
            cmdInsert.Parameters.Add("@currencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyId);
            cmdInsert.Parameters.Add("@currencyRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyRate);
            cmdInsert.Parameters.Add("@callFlagBy", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CallFlagBy);
            cmdInsert.Parameters.Add("@ModbusRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ModbusRefId);
            cmdInsert.Parameters.Add("@GateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.GateId);
            cmdInsert.Parameters.Add("@fromOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FromOrgId);
            cmdInsert.Parameters.Add("@ModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ModeId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingTO.IdLoading = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblLoading(TblLoadingTO tblLoadingTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingTO, cmdUpdate);
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

        public static int UpdateTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingTO, cmdUpdate);
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


        public static int UpdateModeToRegular(TblLoadingSlipTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return UpdateModeToRegularCmd(tblLoadingTO, cmdUpdate);
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

        public static int UpdateModeToRegularCmd(TblLoadingSlipTO tblLoadingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoading] SET " +
                            "  [modeId]= @ModeId" +                        
                            " WHERE [idLoading] = @IdLoading ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.LoadingId;
            cmdUpdate.Parameters.Add("@ModeId", System.Data.SqlDbType.Int).Value = tblLoadingTO.ModeId;
          
            return cmdUpdate.ExecuteNonQuery();
        }


        public static int updateLaodingToCallFlag(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblLoadingTO tblLoadingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoading] SET " +
                            "  [isJointDelivery]= @IsJointDelivery" +
                            " ,[noOfDeliveries]= @NoOfDeliveries" +
                            " ,[statusId]= @StatusId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[loadingDatetime]= @LoadingDatetime" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[loadingSlipNo]= @LoadingSlipNo" +
                            " ,[vehicleNo]= @VehicleNo" +
                            " ,[statusReason] = @StatusReason" +
                            " ,[statusReasonId] = @statusReasonId" +
                            " ,[totalLoadingQty] = @totalLoadingQty" +
                            " ,[transporterOrgId] = @transporterOrgId" +
                            " ,[freightAmt] = @freightAmt" +
                            " ,[superwisorId] = @superwisorId" +
                            " ,[isFreightIncluded] = @isFreightIncluded" +
                            " ,[contactNo] = @contactNo" +
                            " ,[driverName] = @driverName" +
                            " ,[parentLoadingId] = @parentLoadingId" +
                            " ,[callFlag] = @callFlag" +
                            " ,[flagUpdatedOn] = @flagUpdatedOn" +
                            " ,[isAllowNxtLoading] = @isAllowNxtLoading" +
                            " ,[loadingType] = @loadingType " +
                            " ,[currencyId] = @currencyId " +
                            " ,[currencyRate] = @currencyRate " +
                            ",[callFlagBy]=@callFlagBy" +
                            ",[modbusRefId]=@ModbusRefId" +
                            ",[gateId]=@GateId" +
                            ",[isDBup]=@IsDBup" +
                            " ,[FromOrgId] = @FromOrgId " +
                            " WHERE [idLoading] = @IdLoading ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
            cmdUpdate.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsJointDelivery;
            cmdUpdate.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingTO.NoOfDeliveries;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLoadingTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.StatusDate;
            cmdUpdate.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingDatetime);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@LoadingSlipNo", System.Data.SqlDbType.VarChar).Value = tblLoadingTO.LoadingSlipNo;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.VehicleNo);
            cmdUpdate.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
            cmdUpdate.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReasonId);
            cmdUpdate.Parameters.Add("@totalLoadingQty", System.Data.SqlDbType.Decimal).Value = tblLoadingTO.TotalLoadingQty;
            cmdUpdate.Parameters.Add("@transporterOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TransporterOrgId);
            cmdUpdate.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FreightAmt);
            cmdUpdate.Parameters.Add("@superwisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.SuperwisorId);
            cmdUpdate.Parameters.Add("@isFreightIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IsFreightIncluded);
            cmdUpdate.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ContactNo);
            cmdUpdate.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.DriverName);
            cmdUpdate.Parameters.Add("@parentLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ParentLoadingId);
            cmdUpdate.Parameters.Add("@callFlag", System.Data.SqlDbType.Int).Value = tblLoadingTO.CallFlag;
            cmdUpdate.Parameters.Add("@flagUpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FlagUpdatedOn);
            cmdUpdate.Parameters.Add("@isAllowNxtLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsAllowNxtLoading;
            cmdUpdate.Parameters.Add("@loadingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingType);
            cmdUpdate.Parameters.Add("@currencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyId);
            cmdUpdate.Parameters.Add("@currencyRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyRate);
            cmdUpdate.Parameters.Add("@callFlagBy", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CallFlagBy);
            cmdUpdate.Parameters.Add("@ModbusRefId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ModbusRefId);
            cmdUpdate.Parameters.Add("@GateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.GateId);
            cmdUpdate.Parameters.Add("@IsDBup", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IsDBup);
            cmdUpdate.Parameters.Add("@FromOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FromOrgId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblLoading(Int32 idLoading)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoading, cmdDelete);
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

        public static int DeleteTblLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand(); 
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoading, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idLoading, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM [tempLoading] " +
                                    " WHERE idLoading = " + idLoading + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
