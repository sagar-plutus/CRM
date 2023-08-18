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
    public class TblLoadingSlipDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tempLoadingSlip.* ,tblOrganization.firmName as dealerOrgName,dimStat.statusName,cnfOrg.firmName as cnfOrgName " +
                                  " ,loading.modbusRefId,loading.gateId,loading.isDBup,gate.portNumber,gate.IoTUrl,gate.machineIP ,tempLoadSlipdtl.loadingQty,tempLoadSlipdtl.bookingId,tblBookings.bookingDisplayNo " +
                                  " FROM tempLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = tempLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = tempLoadingSlip.statusId " +
                                  " LEFT JOIN tempLoading loading" +
                                  " ON loading.idLoading = tempLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg " +
                                  " ON cnfOrg.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN tblGate gate on gate.idGate=loading.gateId " +
                                  " LEFT JOIN tempLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = tempLoadingSlip.idLoadingSlip " +
                                  " LEFT JOIN tblBookings tblBookings ON tempLoadSlipdtl.bookingId = tblBookings.idbooking " +

                                  // Vaibhav [20-Nov-2017] Added to select from  finalLoadingSlip

                                  " UNION ALL " +
                                  " SELECT finalLoadingSlip.* ,tblOrganization.firmName as dealerOrgName,dimStat.statusName ,cnfOrg.firmName as cnfOrgName" +
                                  " ,loading.modbusRefId,loading.gateId,loading.isDBup,gate.portNumber,gate.IoTUrl,gate.machineIP ,tempLoadSlipdtl.loadingQty,tempLoadSlipdtl.bookingId,tblBookings.bookingDisplayNo " +
                                  " FROM finalLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = finalLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = finalLoadingSlip.statusId " +
                                  " LEFT JOIN finalLoading loading " +
                                  "  ON loading.idLoading = finalLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg" +
                                  " ON cnfOrg.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN tblGate gate on gate.idGate=loading.gateId " +
                                  " LEFT JOIN finalLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = finalLoadingSlip.idLoadingSlip " +
                                   " LEFT JOIN tblBookings tblBookings ON tempLoadSlipdtl.bookingId = tblBookings.idbooking ";

            return sqlSelectQry;
        }


        //Priyanka [08-05-2018] : For showing ORC Report from booking and loading.
        public static List<TblORCReportTO> SelectORCReportDetailsList(DateTime fromDate, DateTime toDate, Int32 flag, string selectOrgIdStr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            String sqlSelectQueryTemp = String.Empty;
            String sqlSelectQueryFinal = String.Empty;
            String FinalSqlSelectQuery = String.Empty;

            String whereCon = String.Empty;
            try
            {
                conn.Open();
                if (fromDate != null && toDate != null)
                {
                    if (flag == 0)
                    {
                        sqlSelectQueryTemp = " Select tblOrganization.firmName AS Dealer,dimStatus.statusName, cnfOrg.firmName As cnfName, tblBookings.idBooking,tblBookings.bookingDisplayNo, " +
                                          " tblLoadingSlip.createdOn, tblBookings.bookingQty, tblBookings.bookingRate, " +
                                          " tblLoadingSlip.comment,tblLoadingSlip.orcAmt, tblLoadingSlip.orcMeasure, " +
                                          " tblLoadingSlipExt.rateCalcDesc from tempLoadingSlip tblLoadingSlip " +
                                          " LEFT Join  tblOrganization tblOrganization ON tblLoadingSlip.dealerOrgId = tblOrganization.idOrganization" +
                                          " LEFT Join  tempLoading loading ON loading.idLoading = tblLoadingSlip.loadingId" +
                                          " LEFT Join  tblOrganization cnfOrg ON loading.cnfOrgId = cnfOrg.idOrganization" +
                                          " LEFT Join dimStatus dimStatus on tblLoadingSlip.statusId = dimStatus.idStatus " +
                                          " LEFT JOIN tempLoadingSlipExt tblLoadingSlipExt ON tblLoadingSlip.idLoadingSlip = tblLoadingSlipExt.loadingSlipId " +
                                          " LEFT Join tblBookings tblBookings ON tblBookings.idBooking = tblLoadingSlipExt.bookingId  ";

                        sqlSelectQueryFinal = " Select tblOrganization.firmName AS Dealer,dimStatus.statusName, cnfOrg.firmName AS cnfName, tblBookings.idBooking,tblBookings.bookingDisplayNo, " +
                                          " tblLoadingSlip.createdOn, tblBookings.bookingQty,tblBookings.bookingRate, " +
                                          " tblLoadingSlip.comment,tblLoadingSlip.orcAmt, tblLoadingSlip.orcMeasure, " +
                                          " tblLoadingSlipExt.rateCalcDesc FROM finalLoadingSlip tblLoadingSlip " +
                                          " LEFT JOIN tblOrganization tblOrganization ON tblLoadingSlip.dealerOrgId = tblOrganization.idOrganization" +
                                          " LEFT JOIN finalLoading loading ON loading.idLoading = tblLoadingSlip.loadingId" +
                                          " LEFT JOIN tblOrganization cnfOrg ON loading.cnfOrgId = cnfOrg.idOrganization" +
                                          " LEFT JOIN dimStatus dimStatus ON tblLoadingSlip.statusId = dimStatus.idStatus " +
                                          " LEFT JOIN finalLoadingSlipExt tblLoadingSlipExt ON tblLoadingSlip.idLoadingSlip = tblLoadingSlipExt.loadingSlipId" +
                                          " LEFT JOIN tblBookings tblBookings ON tblBookings.idBooking = tblLoadingSlipExt.bookingId ";

                        whereCon = " Where CAST(tblLoadingSlip.createdOn AS DATE) BETWEEN @fromDate AND @toDate " +
                                              "AND tblLoadingSlip.statusId NOT IN (" + (int)Constants.TranStatusE.LOADING_CANCEL + " ) AND ISNULL(tblLoadingSlip.orcAmt,0) != 0";
                        //chetan[27-April-2020] added for display data org wise
                        if(!string.IsNullOrEmpty(selectOrgIdStr))
                        {
                            whereCon+= " And isnull(tblLoadingSlip.fromOrgId,0) in(" + selectOrgIdStr + ")";
                        }


                        FinalSqlSelectQuery = "Select DISTINCT * from (" + sqlSelectQueryTemp + whereCon + "UNION ALL" + sqlSelectQueryFinal + whereCon + ") AS sq1";
                    }
                    else if (flag == 2)
                    {
                        sqlSelectQueryTemp = "  SELECT invoice.* ,tempinvoiceitemdetails.invoiceqty as invoiceQty" +
                                                    ",temploadingslip.orcAmt,temploadingslip.comment as loadingComment,dealer.firmName AS dealer, addres.billingName as billingName" +
                                                    ", distributor.firmName As cnfName  , transport.firmName AS transporterName " +
                                                    ",currencyName,statusName,invoiceTypeDesc FROM tempInvoice invoice " +
                                                    "LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                                    " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                                    " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                                    " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId" +
                                                    " LEFT JOIN tempInvoiceAddress addres ON addres.invoiceId = invoice.idInvoice and addres.txnAddrTypeId = 1 " +
                                                    " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                                    " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                                                    " LEFT JOIN tempLoadingSlip temploadingslip ON temploadingslip.idLoadingSlip = invoice.loadingSlipId " +
                                                    " LEFT JOIN(SELECT invoiceId, SUM(invoiceQty) AS invoiceqty FROM tempInvoiceItemDetails  where otherTaxId is null " +
                                                    " GROUP BY invoiceId)tempinvoiceitemdetails ON tempinvoiceitemdetails.invoiceId = invoice.idInvoice ";

                        sqlSelectQueryFinal = " SELECT invoice.* ,finalinvoiceitemdetails.invoiceqty as invoiceQty,temploadingslip.orcAmt" +
                                                " ,temploadingslip.comment as loadingComment,dealer.firmName AS dealer, addres.billingName as billingName, distributor.firmName As cnfName" +
                                                " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc" +
                                                " FROM finalInvoice invoice"+
                                                " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId" +
                                                " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId" +
                                                " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId" +
                                                " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId" +
                                                " LEFT JOIN finalInvoiceAddress addres ON addres.invoiceId = invoice.idInvoice and addres.txnAddrTypeId = 1" +
                                                " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId" +
                                                " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId" +
                                                " LEFT JOIN finalLoadingSlip temploadingslip ON temploadingslip.idLoadingSlip = invoice.loadingSlipId" +
                                                " LEFT JOIN(SELECT invoiceId, SUM(invoiceQty) AS invoiceqty FROM finalInvoiceItemDetails   where otherTaxId is null " +
                                                " GROUP BY invoiceId)finalinvoiceitemdetails ON finalinvoiceitemdetails.invoiceId = invoice.idInvoice";
                        whereCon = " WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate " +
                                   " AND temploadingslip.statusId NOT IN (" + (int)Constants.TranStatusE.LOADING_CANCEL + " ) and ISNULL(temploadingslip.orcAmt,0) != 0 ";

                        if (!string.IsNullOrEmpty(selectOrgIdStr))
                        {
                            whereCon += " And isnull(invoice.invFromOrgId,0) in(" + selectOrgIdStr + ")";
                        }

                        FinalSqlSelectQuery = "Select DISTINCT * FROM (" + sqlSelectQueryTemp + whereCon + "UNION ALL" + sqlSelectQueryFinal + whereCon + ") AS sq1";

                    }
                    else
                    {
                        FinalSqlSelectQuery = "Select tblOrganization.firmName AS Dealer, dimStatus.statusName ,cnfOrg.firmName AS cnfName," +
                                          " tblBookings.idBooking,tblBookings.bookingDisplayNo,tblBookings.createdOn, tblBookings.bookingQty, " +
                                          " tblBookings.bookingRate,tblBookings.comments AS comment,tblBookings.orcAmt , " +
                                          " tblBookings.orcMeasure, '' AS rateCalcDesc from tblBookings tblBookings " +
                                          " LEFT JOIN tblOrganization tblOrganization ON tblBookings.dealerOrgId = tblOrganization.idOrganization " +
                                          " LEFT JOIN  tblOrganization cnfOrg ON tblbookings.cnfOrgId = cnfOrg.idOrganization " +
                                          " LEFT JOIN dimStatus dimStatus ON tblBookings.statusId = dimStatus.idStatus " +
                                          " Where CAST(tblBookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate " +
                                          " AND tblBookings.statusId NOT IN (" + (int)Constants.TranStatusE.LOADING_CANCEL + " ) AND ISNULL(tblBookings.orcAmt,0) != 0";

                    }
                }
                else
                {
                    cmdSelect.CommandText =null;
                }
                cmdSelect.CommandText = FinalSqlSelectQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (flag == 2)
                {
                    List<TblORCReportTO> list = ConvertDTToListForInvoice(sqlReader);
                    if (sqlReader != null)
                        sqlReader.Dispose();
                    return list;
                }
                else
                {
                    List<TblORCReportTO> list = ConvertDTToListForORC(sqlReader);
                    if (sqlReader != null)
                        sqlReader.Dispose();
                    return list;
                }


            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectORCReportDetailsList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        #endregion

        #region Selection
        public static List<TblLoadingSlipTO> SelectAllTblLoadingSlip()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
       

        public static List<TblLoadingSlipTO> SelectAllTblLoadingSlip(int loadingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE loadingId=" + loadingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
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

       
        public static List<TblLoadingSlipTO> SelectAllTblLoadingSlip(int loadingId,SqlConnection conn,SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE loadingId=" + loadingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingSlipTO SelectTblLoadingSlip(Int32 idLoadingSlip)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery()+ ")sq1 WHERE idLoadingSlip = " + idLoadingSlip +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingSlipTO SelectTblLoadingSlip(int idLoadingSlip, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idLoadingSlip = " + idLoadingSlip + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingSlipTO> SelectAllTblLoadingSlipList(TblUserRoleTO tblUserRoleTO, int cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, string selectedOrgStr,int isFromBasicMode = 0)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            int modeId = Constants.getModeIdConfigTO();
            string whereCond = " WHERE CONVERT (DATE,sq1.createdOn,103) BETWEEN @fromDate AND @toDate";
            whereCond += " AND ISNULL(sq1.modeId,1) ="+ modeId;
            string whereCondMode = " WHERE ISNULL(sq1.modeId,1) ="+ (int)Constants.ApplicationModeTypeE.BASIC_MODE;

            string areConfJoin = string.Empty;
            string whereCondFin = string.Empty;
            string whereSupCond = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                if (isConfEn == 1)
                {
                    areConfJoin = " INNER JOIN ( SELECT DISTINCT cnfOrgId FROM tblUserAreaAllocation WHERE isActive=1 AND userId=" + userId + ") areaConf ON  areaConf.cnfOrgId = sq1.cnfOrgId ";
                }

                conn.Open();
                if (loadingStatusId > 0)
                    whereCond += " AND sq1.statusId=" + loadingStatusId;
                    whereCondMode += " AND sq1.statusId=" + loadingStatusId;
                // whereCondFin += " AND finloadingSlip.statusId=" + loadingStatusId;

                String wherecnfIdTemp = String.Empty;
                String wherecnfIdFinal = String.Empty;


                String whereisConTemp = String.Empty;
                String whereisConFinal = String.Empty;

                if (cnfId > 0)
                {
                    wherecnfIdTemp += " AND sq1.cnfOrgId = " + cnfId;
                    //    wherecnfIdFinal += " AND finloadingSlip.cnfOrgId = " + cnfId;
                }

                if (dealerId > 0)
                {
                    wherecnfIdTemp += " AND sq1.dealerOrgId = " + dealerId;
                    //   wherecnfIdFinal += " AND finloadingSlip.dealerOrgId = " + dealerId;
                }


                if (loadingTypeId > 0)
                {
                    whereCond += " AND sq1.loadingType=" + loadingTypeId;
                }
                if (!string.IsNullOrEmpty(selectedOrgStr))
                {
                    whereCond += " AND sq1.fromOrgId in(" + selectedOrgStr + ")";
                }

                String whereisConTemp1 = String.Empty;
                String whereisConFinal1 = String.Empty;

                String sqlQuery = " SELECT tblLoadingSlip.* ,org.digitalSign,tblBookings.bookingDisplayNo,loading.modeId, loading.loadingType, loading.superwisorId,org.firmName as cnfOrgName," +
                                   " CASE WHEN ISNULL(tblLoadSlipdtl.loadingQty,0) > 0 THEN  tblLoadSlipdtl.loadingQty else extLoadedQty END AS loadingQty, tblOrganization.firmName " +
                                  //" + ',' +  CASE WHEN tblOrganization.addrId IS NULL THEN '' Else case WHEN address.villageName IS NOT NULL " +
                                  //" THEN address.villageName ELSE CASE WHEN address.talukaName IS NOT NULL THEN address.talukaName " +
                                  //" ELSE CASE WHEN address.districtName IS NOT NULL THEN address.districtName ELSE address.stateName" +
                                  //" END END END END" +
                                  " AS  dealerOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName, tblLoadSlipdtl.bookingId, loading.cnfOrgId " +
                                   " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                   " , loading.modbusRefId, loading.gateId,loading.isDBup " +
                                  " FROM  tempLoadingSlip tblLoadingSlip " +
                                  " Left Join tempLoadingSlipDtl tblLoadSlipdtl ON tblLoadSlipdtl.loadingSlipId = tblLoadingSlip.idLoadingSlip " +
                                  //" Left Join tempLoadingSlipExt tempLoadingSlipExt ON tempLoadingSlipExt.loadingSlipId = tblLoadingSlip.idLoadingSlip " +
                                  " LEFT JOIN( " +
                                  " select loadingSlipId, sum(loadingQty) as extLoadedQty from tempLoadingSlipExt group by loadingSlipId " +
                                  " ) AS qtyLoadingSlipExtTbl on qtyLoadingSlipExtTbl.loadingSlipId = tblLoadingSlip.idLoadingSlip " +
                                  " Left Join tblBookings tblBookings ON tblLoadSlipdtl.bookingId = tblBookings.idbooking " +
                                  " LEFT JOIN tblOrganization ON tblOrganization.idOrganization = tblLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tempLoading loading ON loading.idLoading = tblLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = tblLoadingSlip.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  //" LEFT JOIN vAddressDetails address ON address.idAddr = tblOrganization.addrId  " 
                                  whereisConTemp1 +

                                  " UNION ALL " +

                                  " SELECT tblLoadingSlip.* ,org.digitalSign,tblBookings.bookingDisplayNo,loading.modeId, loading.loadingType,loading.superwisorId, org.firmName as cnfOrgName, " +
                                  " CASE WHEN ISNULL(tblLoadSlipdtl.loadingQty,0) > 0 THEN  tblLoadSlipdtl.loadingQty else extLoadedQty END AS loadingQty," +
                                  " tblOrganization.firmName " +
                                  // " + ',' +  CASE WHEN tblOrganization.addrId IS NULL THEN '' Else case WHEN address.villageName IS NOT NULL " +
                                  //" THEN address.villageName ELSE CASE WHEN address.talukaName IS NOT NULL THEN address.talukaName " +
                                  //" ELSE CASE WHEN address.districtName IS NOT NULL THEN address.districtName ELSE address.stateName" +
                                  //" END END END END" +
                                  " AS  dealerOrgName , " +
                                  " transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName, tblLoadSlipdtl.bookingId, loading.cnfOrgId  " +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                   " , loading.modbusRefId, loading.gateId,loading.isDBup " +
                                  " FROM finalLoadingSlip tblLoadingSlip  " +
                                  " Left Join finalLoadingSlipDtl tblLoadSlipdtl ON tblLoadSlipdtl.loadingSlipId = tblLoadingSlip.idLoadingSlip " +
                                  " Left Join tblBookings tblBookings ON tblLoadSlipdtl.bookingId = tblBookings.idbooking " +
                                  " LEFT JOIN tblOrganization ON tblOrganization.idOrganization = tblLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN finalLoading loading ON loading.idLoading = tblLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = tblLoadingSlip.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " +
                                  " LEFT JOIN ( " +
                                  "  select loadingSlipId, sum(loadingQty) as extLoadedQty from finalLoadingSlipExt group by loadingSlipId " +
                                  " ) AS qtyLoadingSlipExtTbl on qtyLoadingSlipExtTbl.loadingSlipId = tblLoadingSlip.idLoadingSlip " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  //" LEFT JOIN vAddressDetails address ON address.idAddr = tblOrganization.addrId " 
                                  whereisConFinal1;


                //cmdSelect.CommandText = "Select * from (" + sqlQuery + " )sq1" + areConfJoin + whereCond + wherecnfIdTemp + whereisConTemp + whereSupCond;

                if(isFromBasicMode == 1)
                {
                    cmdSelect.CommandText = "Select * from (" + sqlQuery + " )sq1" + areConfJoin + whereCondMode + wherecnfIdTemp + whereisConTemp + whereSupCond;
                }
                else
                {
                    cmdSelect.CommandText = "Select * from (" + sqlQuery + " )sq1" + areConfJoin + whereCond + wherecnfIdTemp + whereisConTemp + whereSupCond;
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
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


        public static Dictionary<int, string> SelectRegMobileNoDCTForLoadingDealers(String loadingIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            try
            {
                cmdSelect.CommandText = " SELECT distinct dealerOrgId,registeredMobileNos FROM tempLoadingSlip slips " +
                                        " INNER JOIN tblOrganization org ON slips.dealerOrgId = org.idOrganization " +
                                        " WHERE loadingId IN(" + loadingIds + ") " +

                                        // Vaibhav [20-Nov-2017] Added to select from  finalLoadingSlip

                                        " UNION ALL " +
                                        " SELECT distinct dealerOrgId,registeredMobileNos FROM finalLoadingSlip slips " +
                                        " INNER JOIN tblOrganization org ON slips.dealerOrgId = org.idOrganization " +
                                        " WHERE loadingId IN(" + loadingIds + ") ";

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
                        if (rdr["dealerOrgId"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["dealerOrgId"].ToString());
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


        public static Int64 SelectCountOfLoadingSlips(DateTime date,Int32 isConfirmed, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = " SELECT COUNT(*) as slipCount FROM tempLoadingSlip WHERE ISNULL(isConfirmed,0)=" + isConfirmed + " AND DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year;

                // Vaibhav [20-Nov-2017] comment and added to select from finalLoadingSlip
                cmdSelect.CommandText = " SELECT (SELECT COUNT(*) FROM tempLoadingSlip WHERE ISNULL(isConfirmed,0)=" + isConfirmed + " AND DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + ")" +
                                        " + (SELECT COUNT(*) FROM finalLoadingSlip WHERE ISNULL(isConfirmed,0)=" + isConfirmed + " AND DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + ") AS slipCount";

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

        public static List<TblLoadingSlipTO> ConvertDTToList(SqlDataReader tblLoadingSlipTODT)
        {
            List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();
            if (tblLoadingSlipTODT != null)
            {
                while (tblLoadingSlipTODT.Read())
                {
                    TblLoadingSlipTO tblLoadingSlipTONew = new TblLoadingSlipTO();
                    if (tblLoadingSlipTODT["idLoadingSlip"] != DBNull.Value)
                        tblLoadingSlipTONew.IdLoadingSlip = Convert.ToInt32(tblLoadingSlipTODT["idLoadingSlip"].ToString());
                    if (tblLoadingSlipTODT["dealerOrgId"] != DBNull.Value)
                        tblLoadingSlipTONew.DealerOrgId = Convert.ToInt32(tblLoadingSlipTODT["dealerOrgId"].ToString());
                    if (tblLoadingSlipTODT["isJointDelivery"] != DBNull.Value)
                        tblLoadingSlipTONew.IsJointDelivery = Convert.ToInt32(tblLoadingSlipTODT["isJointDelivery"].ToString());
                    if (tblLoadingSlipTODT["noOfDeliveries"] != DBNull.Value)
                        tblLoadingSlipTONew.NoOfDeliveries = Convert.ToInt32(tblLoadingSlipTODT["noOfDeliveries"].ToString());
                    if (tblLoadingSlipTODT["statusId"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusId = Convert.ToInt32(tblLoadingSlipTODT["statusId"].ToString());
                    if (tblLoadingSlipTODT["createdBy"] != DBNull.Value)
                        tblLoadingSlipTONew.CreatedBy = Convert.ToInt32(tblLoadingSlipTODT["createdBy"].ToString());
                    if (tblLoadingSlipTODT["statusDate"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusDate = Convert.ToDateTime(tblLoadingSlipTODT["statusDate"].ToString());
                    if (tblLoadingSlipTODT["loadingDatetime"] != DBNull.Value)
                        tblLoadingSlipTONew.LoadingDatetime = Convert.ToDateTime(tblLoadingSlipTODT["loadingDatetime"].ToString());
                    if (tblLoadingSlipTODT["createdOn"] != DBNull.Value)
                        tblLoadingSlipTONew.CreatedOn = Convert.ToDateTime(tblLoadingSlipTODT["createdOn"].ToString());
                    if (tblLoadingSlipTODT["cdStructure"] != DBNull.Value)
                        tblLoadingSlipTONew.CdStructure = Convert.ToDouble(tblLoadingSlipTODT["cdStructure"].ToString());
                    if (tblLoadingSlipTODT["statusReason"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusReason = Convert.ToString(tblLoadingSlipTODT["statusReason"].ToString());
                    if (tblLoadingSlipTODT["vehicleNo"] != DBNull.Value)
                        tblLoadingSlipTONew.VehicleNo = Convert.ToString(tblLoadingSlipTODT["vehicleNo"].ToString().ToUpper());
                    if (tblLoadingSlipTODT["dealerOrgName"] != DBNull.Value)
                        tblLoadingSlipTONew.DealerOrgName = Convert.ToString(tblLoadingSlipTODT["dealerOrgName"].ToString());
                    if (tblLoadingSlipTODT["statusName"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusName = Convert.ToString(tblLoadingSlipTODT["statusName"].ToString());

                    if (tblLoadingSlipTODT["loadingId"] != DBNull.Value)
                        tblLoadingSlipTONew.LoadingId = Convert.ToInt32(tblLoadingSlipTODT["loadingId"].ToString());
                    if (tblLoadingSlipTODT["statusReasonId"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusReasonId = Convert.ToInt32(tblLoadingSlipTODT["statusReasonId"].ToString());
                    if (tblLoadingSlipTODT["loadingSlipNo"] != DBNull.Value)
                        tblLoadingSlipTONew.LoadingSlipNo = Convert.ToString(tblLoadingSlipTODT["loadingSlipNo"].ToString());

                    if (tblLoadingSlipTODT["isConfirmed"] != DBNull.Value)
                        tblLoadingSlipTONew.IsConfirmed = Convert.ToInt32(tblLoadingSlipTODT["isConfirmed"].ToString());

                    if (tblLoadingSlipTODT["comment"] != DBNull.Value)
                        tblLoadingSlipTONew.Comment = Convert.ToString(tblLoadingSlipTODT["comment"].ToString());
                    if (tblLoadingSlipTODT["contactNo"] != DBNull.Value)
                        tblLoadingSlipTONew.ContactNo = Convert.ToString(tblLoadingSlipTODT["contactNo"].ToString());
                    if (tblLoadingSlipTODT["driverName"] != DBNull.Value)
                        tblLoadingSlipTONew.DriverName = Convert.ToString(tblLoadingSlipTODT["driverName"].ToString());
                    if (tblLoadingSlipTODT["cdStructureId"] != DBNull.Value)
                        tblLoadingSlipTONew.CdStructureId = Convert.ToInt32(tblLoadingSlipTODT["cdStructureId"].ToString());


                    //Priyanka [07-05-2018]
                    if (tblLoadingSlipTODT["orcAmt"] != DBNull.Value)
                        tblLoadingSlipTONew.OrcAmt = Convert.ToDouble(tblLoadingSlipTODT["orcAmt"].ToString());

                    //Priyanka [07-05-2018]
                    if (tblLoadingSlipTODT["orcMeasure"] != DBNull.Value)
                        tblLoadingSlipTONew.OrcMeasure = Convert.ToString(tblLoadingSlipTODT["orcMeasure"].ToString());

                    //Vijaymala[21-05-2018]added
                    if (tblLoadingSlipTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingSlipTONew.CnfOrgName = Convert.ToString(tblLoadingSlipTODT["cnfOrgName"].ToString());


                    if (tblLoadingSlipTODT["bookingId"] != DBNull.Value)
                        tblLoadingSlipTONew.BookingId = Convert.ToInt32(tblLoadingSlipTODT["bookingId"].ToString());
                    if (tblLoadingSlipTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingSlipTONew.ModbusRefId = Convert.ToInt32(tblLoadingSlipTODT["modbusRefId"]);
                    if (tblLoadingSlipTODT["gateId"] != DBNull.Value)
                        tblLoadingSlipTONew.GateId = Convert.ToInt32(tblLoadingSlipTODT["gateId"]);
                    if (tblLoadingSlipTODT["portNumber"] != DBNull.Value)
                        tblLoadingSlipTONew.PortNumber = Convert.ToString(tblLoadingSlipTODT["portNumber"]);
                    if (tblLoadingSlipTODT["ioTUrl"] != DBNull.Value)
                        tblLoadingSlipTONew.IotUrl = Convert.ToString(tblLoadingSlipTODT["ioTUrl"]);
                    if (tblLoadingSlipTODT["machineIP"] != DBNull.Value)
                        tblLoadingSlipTONew.MachineIP = Convert.ToString(tblLoadingSlipTODT["machineIP"]);
                    if (tblLoadingSlipTODT["isDBup"] != DBNull.Value)
                        tblLoadingSlipTONew.IsDBup = Convert.ToInt32(tblLoadingSlipTODT["isDBup"]);
                    if (tblLoadingSlipTODT["bookingDisplayNo"] != DBNull.Value)
                        tblLoadingSlipTONew.BookingDisplayNo = Convert.ToString(tblLoadingSlipTODT["bookingDisplayNo"]);
                    if (tblLoadingSlipTODT["loadingQty"] != DBNull.Value)
                        tblLoadingSlipTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipTODT["loadingQty"]);


                    tblLoadingSlipTOList.Add(tblLoadingSlipTONew);
                }
            }
            return tblLoadingSlipTOList;
        }

        //Priyanka [08-05-2018] : Added this ConvertDTToList for ORC report- from loading, from booking

        public static List<TblORCReportTO> ConvertDTToListForORC(SqlDataReader tblORCReportTODT)
        {
            List<TblORCReportTO> tblORCReportTOList = new List<TblORCReportTO>();
            if (tblORCReportTODT != null)
            {
                while (tblORCReportTODT.Read())
                {
                    TblORCReportTO tblORCReportTONew = new TblORCReportTO();
                    if (tblORCReportTODT["dealer"] != DBNull.Value)
                        tblORCReportTONew.Dealer = Convert.ToString(tblORCReportTODT["dealer"].ToString());
                    if (tblORCReportTODT["cnfName"] != DBNull.Value)
                        tblORCReportTONew.CnfName = Convert.ToString(tblORCReportTODT["cnfName"].ToString());
                    if (tblORCReportTODT["statusName"] != DBNull.Value)
                        tblORCReportTONew.StatusName = Convert.ToString(tblORCReportTODT["statusName"].ToString());
                    if (tblORCReportTODT["idBooking"] != DBNull.Value)
                        tblORCReportTONew.IdBooking = Convert.ToInt32(tblORCReportTODT["idBooking"].ToString());
                    if (tblORCReportTODT["createdOn"] != DBNull.Value)
                        tblORCReportTONew.CreatedOn = Convert.ToDateTime(tblORCReportTODT["createdOn"].ToString());
                    if (tblORCReportTODT["bookingQty"] != DBNull.Value)
                        tblORCReportTONew.BookingQty = Convert.ToDouble(tblORCReportTODT["bookingQty"].ToString());
                    if (tblORCReportTODT["bookingRate"] != DBNull.Value)
                        tblORCReportTONew.BookingRate = Convert.ToDouble(tblORCReportTODT["bookingRate"].ToString());
                    
                    if (tblORCReportTODT["comment"] != DBNull.Value)
                        tblORCReportTONew.Comment = Convert.ToString(tblORCReportTODT["comment"].ToString());

                    if (tblORCReportTODT["orcAmt"] != DBNull.Value)
                        tblORCReportTONew.OrcAmt = Convert.ToDouble(tblORCReportTODT["orcAmt"].ToString());

                    if (tblORCReportTODT["orcMeasure"] != DBNull.Value)
                        tblORCReportTONew.OrcMeasure = Convert.ToString(tblORCReportTODT["orcMeasure"].ToString());
                    if (tblORCReportTODT["rateCalcDesc"] != DBNull.Value)
                        tblORCReportTONew.RateCalcDesc = Convert.ToString(tblORCReportTODT["rateCalcDesc"].ToString());
                    //if (tblORCReportTODT["bookingDisplayNo"] != DBNull.Value)
                    //    tblORCReportTONew.BookingDisplayNo = Convert.ToString(tblORCReportTODT["bookingDisplayNo"].ToString());
                    tblORCReportTOList.Add(tblORCReportTONew);
                }
            }
            return tblORCReportTOList;
        }

        public static List<TblORCReportTO> ConvertDTToListForInvoice(SqlDataReader tblORCReportTODT)
        {
            List<TblORCReportTO> tblORCReportTOList = new List<TblORCReportTO>();
            if (tblORCReportTODT != null)
            {
                while (tblORCReportTODT.Read())
                {
                    TblORCReportTO tblORCReportTONew = new TblORCReportTO();
                    if (tblORCReportTODT["dealer"] != DBNull.Value)
                        tblORCReportTONew.Dealer = Convert.ToString(tblORCReportTODT["dealer"].ToString());
                    if (tblORCReportTODT["cnfName"] != DBNull.Value)
                        tblORCReportTONew.CnfName = Convert.ToString(tblORCReportTODT["cnfName"].ToString());
                    if (tblORCReportTODT["statusName"] != DBNull.Value)
                        tblORCReportTONew.StatusName = Convert.ToString(tblORCReportTODT["statusName"].ToString());
                    if (tblORCReportTODT["loadingComment"] != DBNull.Value)
                        tblORCReportTONew.Comment = Convert.ToString(tblORCReportTODT["loadingComment"].ToString());
 
                    if (tblORCReportTODT["orcAmt"] != DBNull.Value)
                        tblORCReportTONew.OrcAmt = Convert.ToDouble(tblORCReportTODT["orcAmt"].ToString());

                    if (tblORCReportTODT["invoiceNo"] != DBNull.Value)
                        tblORCReportTONew.InvoiceNo = Convert.ToInt32(tblORCReportTODT["invoiceNo"].ToString());
                    if (tblORCReportTODT["invoiceDate"] != DBNull.Value)
                        tblORCReportTONew.InvoiceDate = Convert.ToDateTime(tblORCReportTODT["invoiceDate"].ToString());
                    if (tblORCReportTODT["deliveryLocation"] != DBNull.Value)
                        tblORCReportTONew.DeliveryLocation = Convert.ToString(tblORCReportTODT["deliveryLocation"].ToString());
                    if (tblORCReportTODT["invoiceQty"] != DBNull.Value)
                        tblORCReportTONew.InvoiceQty = Convert.ToDouble(tblORCReportTODT["invoiceQty"].ToString());
                    if (tblORCReportTODT["basicAmt"] != DBNull.Value)
                        tblORCReportTONew.BasicAmt = Convert.ToDouble(tblORCReportTODT["basicAmt"].ToString());
                    if (tblORCReportTODT["grandTotal"] != DBNull.Value)
                        tblORCReportTONew.GrandTotal = Convert.ToDouble(tblORCReportTODT["grandTotal"].ToString());
                    if (tblORCReportTODT["discountAmt"] != DBNull.Value)
                        tblORCReportTONew.DiscountAmount = Convert.ToDouble(tblORCReportTODT["discountAmt"].ToString());
                    if (tblORCReportTODT["taxableAmt"] != DBNull.Value)
                        tblORCReportTONew.TaxableAmt = Convert.ToDouble(tblORCReportTODT["taxableAmt"].ToString());
                    if (tblORCReportTODT["billingName"] != DBNull.Value)
                        tblORCReportTONew.BillingName = Convert.ToString(tblORCReportTODT["billingName"].ToString());
                    

                    tblORCReportTOList.Add(tblORCReportTONew);
                }
            }
            return tblORCReportTOList;
        }

        //Sudhir
        public static List<TblLoadingSlipTO> SelectAllLoadingSlipListByVehicleNo(string vehicleNo)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE vehicleNo LIKE '%" + vehicleNo + "%' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        //Sudhir
        public static List<TblLoadingSlipTO> SelectTblLoadingTOByLoadingSlipIdForSupport(String loadingSlipNo)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipNo LIKE '%" + loadingSlipNo + "%' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        //Sudhir[09-03-2018]
        public static List<TblLoadingSlipTO> SelectAllTblLoadingSlipByDate(DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO,Int32 cnfId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            //vijaymala[04-04-2018]modify the code to display records acc to role
            int isConfEn = 0;
            int userId = 0;
            string areConfJoin = string.Empty;
            string cnfwhereCond = string.Empty;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                //vijaymala[04-04-2018]modify the code to display records acc to role

                String sqlSelectQry = " SELECT tempLoadingSlip.*,loading.cnfOrgId ,tblOrganization.firmName as dealerOrgName,dimStat.statusName,cnfOrg.firmName as cnfOrgName " +
                                  " FROM tempLoadingSlip" +
                                  " INNER JOIN tempLoading loading on  tempLoadingSlip.loadingId=loading.idLoading " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = tempLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = tempLoadingSlip.statusId " +
                                   " LEFT JOIN tempLoading " +
                                  " ON tempLoading.idLoading = tempLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg " +
                                  " ON cnfOrg.idOrganization = tempLoading.cnfOrgId " +

                                  // Vaibhav [20-Nov-2017] Added to select from  finalLoadingSlip

                                  " UNION ALL " +
                                  " SELECT finalLoadingSlip.*,loading.cnfOrgId  ,tblOrganization.firmName as dealerOrgName,dimStat.statusName,cnfOrg.firmName as cnfOrgName " +
                                  " FROM finalLoadingSlip" +
                                  " INNER JOIN finalLoading  loading on  finalLoadingSlip.loadingId=loading.idLoading " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = finalLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = finalLoadingSlip.statusId " +
                                   " LEFT JOIN finalLoading " +
                                  "  ON finalLoading.idLoading = finalLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg" +
                                  " ON cnfOrg.idOrganization = finalLoading.cnfOrgId "; ;

                if (isConfEn == 1)
                    {
                    //areConfJoin = " INNER JOIN " +
                    //           " ( " +
                    //           "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                    //           "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                    //           "   INNER JOIN " +
                    //           "   ( " +
                    //           "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                    //           "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                    //           "  ) addrDtl  ON idOrganization = organizationId " +
                    //           "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                    //           "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                    //           " ) AS userAreaDealer On userAreaDealer.cnfOrgId = sq1.cnfOrgId AND sq1.dealerOrgId = userAreaDealer.idOrganization ";
                    ////  areConfJoin = " INNER JOIN ( SELECT DISTINCT cnfOrgId FROM tblUserAreaAllocation WHERE isActive=1 AND userId=" + userId + ") areaConf ON  areaConf.cnfOrgId = sq1.cnfOrgId ";


                    areConfJoin = " INNER JOIN " +
                                            " ( " +
                                            " SELECT areaConf.cnfOrgId, idOrganization " +
                                            " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                            " INNER JOIN " +
                                            " ( " +
                                            "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                            "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                                            "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                            "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                            " ) AS userAreaDealer On userAreaDealer.cnfOrgId = sq1.cnFOrgid AND sq1.dealerOrgId = userAreaDealer.idOrganization ";

                }




                string whereCond = " WHERE CAST(sq1.statusDate AS DATE)  BETWEEN @fromDate AND @toDate";

                if (cnfId > 0)
                    cnfwhereCond = whereCond + " AND sq1.cnfOrgId=" + cnfId;
                else
                    cnfwhereCond = whereCond ;

                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + sqlSelectQry + ")sq1 " + areConfJoin + cnfwhereCond;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
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

        /// <summary>
        /// Vijaymala [08-05-2018] added to get notified loading list withiin period 
        /// </summary>
        /// <returns></returns>
        public static List<TblLoadingSlipTO> SelectAllNotifiedTblLoadingList(DateTime fromDate, DateTime toDate, Int32 callFlag,string selectOrgIdStr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                String whereCondition = "WHERE CAST(sq1.statusDate AS Date) BETWEEN @fromDate AND @toDate ";

                whereCondition += " AND ISNULL(sq1.callFlag,0) = " + callFlag;

                if(!string.IsNullOrEmpty(selectOrgIdStr))
                {
                    whereCondition += " AND isnull(sq1.fromOrgId,0) in ( " + selectOrgIdStr + ")";
                }

                String sqlSelectQry = " SELECT tempLoadingSlip.* ,tblOrganization.firmName as dealerOrgName, cnfOrg.firmName as cnfOrgName ,dimStat.statusName,tempLoading.callFlag " +
                                  " FROM tempLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = tempLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = tempLoadingSlip.statusId " +
                                  " LEFT JOIN tempLoading " +
                                  " ON tempLoading.idLoading = tempLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg " +
                                  " ON cnfOrg.idOrganization = tempLoading.cnfOrgId " +


                                  // Vaibhav [09-Jan-2018] Added to select from  finalLoadingSlip

                                  " UNION ALL " +

                                  " SELECT finalLoadingSlip.* ,tblOrganization.firmName as dealerOrgName, cnfOrg.firmName as cnfOrgName, dimStat.statusName,finalLoading.callFlag " +
                                  " FROM finalLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = finalLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = finalLoadingSlip.statusId " +
                                  " LEFT JOIN finalLoading " +
                                  "  ON finalLoading.idLoading = finalLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg" +
                                  " ON cnfOrg.idOrganization = finalLoading.cnfOrgId ";

                cmdSelect.CommandText = "SELECT * FROM(" + sqlSelectQry + ")sq1 " + whereCondition;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat); ;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat); ;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
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


        /// <summary>
        /// Vijaymala [08-05-2018] added to get notified loading list withiin period 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int,int> SelectModbusRefIdWrtLoadingSlipIdDCT(string loadingSlipNos)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader tblLoadingSlipTODT = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                String sqlSelectQry = " SELECT idLoadingSlip,modbusRefId FROM temploadingslip " +
                                      " INNER JOIN tempLoading ON idLoading = loadingId" +
                                      " WHERE idLoadingSlip IN(" + loadingSlipNos + ") AND modbusRefId IS NOT NULL";

                cmdSelect.CommandText = sqlSelectQry;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblLoadingSlipTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Dictionary<int, int> DCT = new Dictionary<int, int>();
                List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();
                if (tblLoadingSlipTODT != null)
                {
                    while (tblLoadingSlipTODT.Read())
                    {
                        int loadingslipId = 0, modRefId = 0;
                        if (tblLoadingSlipTODT["idLoadingSlip"] != DBNull.Value)
                            loadingslipId = Convert.ToInt32(tblLoadingSlipTODT["idLoadingSlip"].ToString());
                        if (tblLoadingSlipTODT["modbusRefId"] != DBNull.Value)
                            modRefId = Convert.ToInt32(tblLoadingSlipTODT["modbusRefId"].ToString());

                        if (loadingslipId > 0 && modRefId > 0)
                            DCT.Add(loadingslipId, modRefId);
                    }
                }
                //return list;
                return DCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingSlipTODT != null)
                    tblLoadingSlipTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //to get port No , machine IP
        public static Dictionary<Int32, TblLoadingTO> SelectModbusRefIdByLoadingSlipIdDCT(string loadingSlipNos)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader tblLoadingSlipTODT = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                //String sqlSelectQry = " SELECT idLoadingSlip,modbusRefId FROM temploadingslip " +
                //                      " INNER JOIN tempLoading ON idLoading = loadingId" +
                //                      " WHERE idLoadingSlip IN(" + loadingSlipNos + ") AND modbusRefId IS NOT NULL";

                String sqlSelectQry = " SELECT idLoadingSlip, modbusRefId,gateId, tblgate.iotUrl,tblgate.portnumber,tblgate.machineIP FROM temploadingslip " +
                              " INNER JOIN tempLoading tempLoading ON idLoading = loadingId " +
                              "  left join tblgate tblgate on tblgate.idGate = tempLoading.gateId " +
                              "   WHERE idLoadingSlip IN(" + loadingSlipNos + ") AND modbusRefId IS NOT NULL";

                cmdSelect.CommandText = sqlSelectQry;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblLoadingSlipTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //   Dictionary<int, int> DCT = new Dictionary<int, int>();
                //  List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();

                Dictionary<Int32, TblLoadingTO> tblLoadingTODct = new Dictionary<int, TblLoadingTO>(); 

               //TblLoadingTO tblLoadingTO = new TblLoadingTO();
                if (tblLoadingSlipTODT != null)
                {
                    while (tblLoadingSlipTODT.Read())
                    {
                        TblLoadingTO tblLoadingTO = new TblLoadingTO();
                        int loadingslipId = 0, modRefId = 0;
                        if (tblLoadingSlipTODT["idLoadingSlip"] != DBNull.Value)
                            loadingslipId = Convert.ToInt32(tblLoadingSlipTODT["idLoadingSlip"].ToString());
                        if (tblLoadingSlipTODT["modbusRefId"] != DBNull.Value)
                            tblLoadingTO.ModbusRefId = Convert.ToInt32(tblLoadingSlipTODT["modbusRefId"].ToString());
                        if (tblLoadingSlipTODT["iotUrl"] != DBNull.Value)
                            tblLoadingTO.IoTUrl = tblLoadingSlipTODT["iotUrl"].ToString();
                        if (tblLoadingSlipTODT["portnumber"] != DBNull.Value)
                            tblLoadingTO.PortNumber = tblLoadingSlipTODT["portnumber"].ToString();
                        if (tblLoadingSlipTODT["machineIP"] != DBNull.Value)
                            tblLoadingTO.MachineIP = tblLoadingSlipTODT["machineIP"].ToString();
                        if (tblLoadingSlipTODT["gateId"] != DBNull.Value)
                            tblLoadingTO.GateId = Convert.ToInt32(tblLoadingSlipTODT["gateId"].ToString());

                        if (!tblLoadingTODct.ContainsKey(loadingslipId))
                        {
                            tblLoadingTODct.Add(loadingslipId, tblLoadingTO);
                        }


                    }
                }
                //return list;
                return tblLoadingTODct;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingSlipTODT != null)
                    tblLoadingSlipTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        #endregion

        #region Insertion
        public static int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingSlipTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingSlipTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblLoadingSlipTO tblLoadingSlipTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingSlip]( " + 
                            "  [dealerOrgId]" +
                            " ,[isJointDelivery]" +
                            " ,[noOfDeliveries]" +
                            " ,[statusId]" +
                            " ,[createdBy]" +
                            " ,[statusDate]" +
                            " ,[loadingDatetime]" +
                            " ,[createdOn]" +
                            " ,[cdStructure]" +
                            " ,[statusReason]" +
                            " ,[vehicleNo]" +
                            " ,[loadingId]" +
                            " ,[statusReasonId]" +
                            " ,[loadingSlipNo]" +
                            " ,[isConfirmed]" +
                            " ,[comment]" +
                            " ,[contactNo]" +
                            " ,[driverName]" +
                            " ,[cdStructureId]" +
                            " ,[orcAmt]" +          //Priyanka [07-05-2018]
                            " ,[orcMeasure]" +      //Priyanka [07-05-2018]
                            " ,[fromOrgId]" +
                            " )" +
                " VALUES (" +
                            "  @DealerOrgId " +
                            " ,@IsJointDelivery " +
                            " ,@NoOfDeliveries " +
                            " ,@StatusId " +
                            " ,@CreatedBy " +
                            " ,@StatusDate " +
                            " ,@LoadingDatetime " +
                            " ,@CreatedOn " +
                            " ,@CdStructure " +
                            " ,@StatusReason " +
                            " ,@VehicleNo " +
                            " ,@LoadingId " +
                            " ,@statusReasonId " +
                            " ,@loadingSlipNo " +
                            " ,@isConfirmed " +
                            " ,@comment " +
                            " ,@contactNo " +
                            " ,@driverName " +
                            " ,@cdStructureId " +
                            " ,@orcAmt" +
                            " ,@orcMeasure" +
                            " ,@fromOrgId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdLoadingSlip", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IdLoadingSlip;
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.DealerOrgId;
            cmdInsert.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsJointDelivery;
            cmdInsert.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.NoOfDeliveries;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.StatusDate;
            cmdInsert.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingDatetime);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.CreatedOn;
            cmdInsert.Parameters.Add("@CdStructure", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipTO.CdStructure;
            cmdInsert.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.StatusReason);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipTO.VehicleNo;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.LoadingId;
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.StatusReasonId);
            cmdInsert.Parameters.Add("@loadingSlipNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingSlipNo);
            cmdInsert.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsConfirmed;
            cmdInsert.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.Comment);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ContactNo);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.DriverName);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.CdStructureId);
            cmdInsert.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcAmt); //Priyanka [07-05-2018] 
            cmdInsert.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcMeasure); //Priyanka [07-05-2018] 
            cmdInsert.Parameters.Add("@fromOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.FromOrgId);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblLoadingSlipTO.IdLoadingSlip = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingSlipTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingSlipTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblLoadingSlip(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tempLoadingSlip] SET " +
                            "  [statusId]= @StatusId" +
                            " ,[statusDate]= @StatusDate" +
                            //" ,[vehicleNo]= @VehicleNo" +  //Saket [2018-12-12] Added
                            " ,[loadingDatetime]= @LoadingDatetime" +
                            " ,[statusReason]= @StatusReason" +
                            " ,[statusReasonId]= @statusReasonId" +
                            " WHERE [loadingId]= @LoadingId ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusId;
                cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.StatusDate;
                cmdUpdate.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingDatetime);
                cmdUpdate.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
                cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
                cmdUpdate.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReasonId);
               // cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblLoadingTO.VehicleNo;
                return cmdUpdate.ExecuteNonQuery();
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

        public static int UpdateTblLoadingById(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tempLoading] SET " +
                            "  [statusId]= @StatusId" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[loadingDatetime]= @LoadingDatetime" +
                            " ,[statusReason]= @StatusReason" +
                            " ,[statusReasonId]= @statusReasonId" +
                            " WHERE [idLoading]= @idLoading ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusId;
                cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.StatusDate;
                cmdUpdate.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingDatetime);
                cmdUpdate.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
                cmdUpdate.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
                cmdUpdate.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReasonId);
                return cmdUpdate.ExecuteNonQuery();
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

        public static int UpdateTblLoadingsSlipById(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tempLoadingSlip] SET " +
                            "  [statusId]= @StatusId" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[loadingDatetime]= @LoadingDatetime" +
                            " ,[statusReason]= @StatusReason" +
                            " ,[statusReasonId]= @statusReasonId" +
                            " WHERE [idLoadingSlip]= @idLoadingSlip ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusId;
                cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.StatusDate;
                cmdUpdate.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingDatetime);
                cmdUpdate.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.StatusReason);
                cmdUpdate.Parameters.Add("@idLoadingSlip", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IdLoadingSlip;
                cmdUpdate.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusReasonId;
                return cmdUpdate.ExecuteNonQuery();
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
        public static int ExecuteUpdationCommand(TblLoadingSlipTO tblLoadingSlipTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingSlip] SET " + 
                            "  [dealerOrgId]= @DealerOrgId" +
                            " ,[isJointDelivery]= @IsJointDelivery" +
                            " ,[noOfDeliveries]= @NoOfDeliveries" +
                            " ,[statusId]= @StatusId" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[loadingDatetime]= @LoadingDatetime" +
                            " ,[cdStructure]= @CdStructure" +
                            " ,[statusReason]= @StatusReason" +
                            " ,[statusReasonId]= @statusReasonId" +
                            " ,[vehicleNo] = @VehicleNo" +
                            " ,[isConfirmed] = @isConfirmed" +
                            " ,[comment] = @comment" +
                            " ,[contactNo] = @contactNo" +
                            " ,[driverName] = @driverName" +
                            " ,[cdStructureId] = @cdStructureId" +
                            " ,[orcAmt]=@orcAmt" +                      //Priyanka [07-05-2018] Added
                            " ,[orcMeasure]=@orcMeasure" +              //Priyanka [07-05-2018] Added
                               " ,[FromOrgId] = @FromOrgId " +
                            " WHERE [idLoadingSlip] = @IdLoadingSlip "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingSlip", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IdLoadingSlip;
            cmdUpdate.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.DealerOrgId;
            cmdUpdate.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsJointDelivery;
            cmdUpdate.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.NoOfDeliveries;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusId;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.StatusDate;
            cmdUpdate.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingDatetime);
            cmdUpdate.Parameters.Add("@CdStructure", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipTO.CdStructure;
            cmdUpdate.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipTO.StatusReason;
            cmdUpdate.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusReasonId;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipTO.VehicleNo;
            cmdUpdate.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.Comment);
            cmdUpdate.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ContactNo);
            cmdUpdate.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.DriverName);
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.CdStructureId;

            cmdUpdate.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcAmt);          //Priyanka [07-05-2018] 
            cmdUpdate.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcMeasure); //Priyanka [07-05-2018] 
            cmdUpdate.Parameters.Add("@FromOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.FromOrgId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingSlip(Int32 idLoadingSlip)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingSlip, cmdDelete);
            }
            catch(Exception ex)
            {
               
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblLoadingSlip(Int32 idLoadingSlip, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingSlip, cmdDelete);
            }
            catch(Exception ex)
            {
               
                
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idLoadingSlip, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlip] " +
            " WHERE idLoadingSlip = " + idLoadingSlip +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingSlip", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IdLoadingSlip;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
