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
    public class TblInvoiceDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT invoice.* ,loading.isDBup, dealer.firmName as dealerName,distributor.firmName AS distributorName " +
                                  " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc,loadingSlip.statusId as loadingStatusId " +
                                  " FROM tempInvoice invoice " +
                                  " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                  " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                  " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                  " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                                  " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                  " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                                  " LEFT JOIN tempLoadingSlip loadingSlip ON idLoadingSlip = invoice.loadingSlipId " +
                                  " LEFT JOIN tempLoading loading ON idLoading = loadingSlip.loadingid " +

                                  // Vaibhav [20-Nov-2017] Added to select from finalInvoice
                                  " UNION ALL " +
                                  " SELECT invoice.* ,loading.isDBup, dealer.firmName as dealerName,distributor.firmName AS distributorName " +
                                  " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc, loadingSlip.statusId as loadingStatusId" +
                                  " FROM finalInvoice invoice " +
                                  " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                  " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                  " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                  " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                                  " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                  " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                                  " LEFT JOIN finalLoadingSlip loadingSlip  ON idLoadingSlip = invoice.loadingSlipId " +
                                  " LEFT JOIN finalLoading loading ON idLoading = loadingSlip.loadingid ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblInvoiceTO> SelectAllTblInvoice()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Ramdas.w @24102017 this method is  Get Generated Invoice List
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isConfirm"></param>
        /// <param name="cnfId"></param>
        /// <param name="dealerID"></param>
        /// <param name="userRoleTO"></param>
        /// <returns></returns>
        public static List<TblInvoiceTO> SelectAllTblInvoice(DateTime frmDt, DateTime toDt, int isConfirmed, Int32 cnfId , Int32 dealerId, TblUserRoleTO tblUserRoleTO, string selectedOrgId,Int32 defualtOrgId)
     {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string strQuery = string.Empty;
            string strCAndFQuery = string.Empty;
            String areConfJoin = String.Empty;
            string strStatusIds =(int) Constants.InvoiceStatusE.AUTHORIZED + "," + (int)Constants.InvoiceStatusE.CANCELLED;
            SqlDataReader reader = null;
            int isConfEn = 0;
            int userId = 0;
            int modeId = Constants.getModeIdConfigTO();
            string isConfirm =Convert.ToString(isConfirmed);//"1,0";
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                conn.Open();

                //// Vaibhav [24-Nov-2017] Added to select invoices from only temp data

                String sqlQueryTemp = " SELECT invoice.* ,loading.isDBup,loading.modeId,dealer.firmName as dealerName,distributor.firmName AS distributorName " +
                                  " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc,loadingSlip.statusId as loadingStatusId " +
                                  " FROM tempInvoice invoice " +
                                  " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                  " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                  " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                  " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                                  " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                  " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                                  " LEFT JOIN tempLoadingSlip loadingSlip ON idLoadingSlip = invoice.loadingSlipId " +
                                   " LEFT JOIN temploading loading ON idLoading = loadingSlip.loadingId ";

                String sqlQueryFinal = " SELECT invoice.* ,loading.isDBup,loading.modeId,dealer.firmName as dealerName,distributor.firmName AS distributorName " +
                               " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc, loadingSlip.statusId as loadingStatusId " +
                               " FROM finalInvoice invoice " +
                               " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                               " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                               " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                               " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                               " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                               " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId "+
                               " LEFT JOIN finalLoadingSlip loadingSlip ON idLoadingSlip = invoice.loadingSlipId " +
                               " LEFT JOIN finalloading loading ON idLoading = loadingSlip.loadingId ";
                if (isConfEn == 1)
                {
                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = invoice.distributorOrgId AND invoice.dealerOrgId = userAreaDealer.idOrganization ";
                }
                //if (cnfId == 0 && dealerId == 0)
                //{
                //    strQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(invoice.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND isConfirmed IN(" + isConfirm + ")";
                //}

                //else if (cnfId > 0 && dealerId == 0)
                //{
                //    strQuery = SqlSelectQuery() + areConfJoin + "WHERE CAST(invoice.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND isConfirmed IN(" + isConfirm + ") AND invoice.distributorOrgId='" + cnfId + "'";
                //}

                //else if (cnfId > 0 && dealerId > 0)
                //{
                //    strQuery = SqlSelectQuery() + areConfJoin + "WHERE CAST(invoice.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND isConfirmed IN(" + isConfirm + ")  AND invoice.distributorOrgId='" + cnfId + "' AND invoice.dealerOrgId='" + dealerId + "' ";
                //}


                // Vaibhav comment and modify
                if (cnfId == 0 && dealerId == 0)
                {

                    strQuery = " SELECT * FROM ( " + sqlQueryTemp + areConfJoin + " WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND invoice.isConfirmed IN(" + isConfirm + ")" +

                              " UNION ALL " + sqlQueryFinal + areConfJoin + " WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND invoice.isConfirmed IN(" + isConfirm + "))sq1";

                }

                else if (cnfId > 0 && dealerId == 0)
                {

                    strQuery = " SELECT * FROM ( " + sqlQueryTemp + areConfJoin + "WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND invoice.isConfirmed IN(" + isConfirm + ") AND invoice.distributorOrgId='" + cnfId + "'" +
                                " UNION ALL " + sqlQueryFinal + areConfJoin + "WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND invoice.isConfirmed IN(" + isConfirm + ") AND invoice.distributorOrgId='" + cnfId + "')sq1";
                }

                else if (cnfId > 0 && dealerId > 0)
                {

                    strQuery = " SELECT * FROM ( " + sqlQueryTemp + areConfJoin + "WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND invoice.isConfirmed IN(" + isConfirm + ")  AND invoice.distributorOrgId='" + cnfId + "' AND invoice.dealerOrgId='" + dealerId + "' " +
                                " UNION ALL " + sqlQueryFinal + areConfJoin + "WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND invoice.isConfirmed IN(" + isConfirm + ")  AND invoice.distributorOrgId='" + cnfId + "' AND invoice.dealerOrgId='" + dealerId + "' )sq1";

                }
                String strWhere = String.Empty;
                //if (selectedOrgId > 0)
                if (!String.IsNullOrEmpty(selectedOrgId))
                {
                    //if (isBrm == (int)Constants.InvoiceGenerateModeE.REGULAR)
                    //{
                    //    strWhere = " WHERE sq1.invFromOrgId  NOT IN (" + internalOrgId + ")";
                    //}
                    if (Convert.ToInt32(selectedOrgId.Split(',')[0]) ==defualtOrgId)
                        strWhere = " WHERE sq1.invFromOrgId  IN (" + selectedOrgId + ",0 )";
                    else
                    strWhere = " WHERE sq1.invFromOrgId  IN (" + selectedOrgId + ")";
                }

                cmdSelect.CommandText = strQuery + strWhere;
                if (modeId > 0)
                {
                    cmdSelect.CommandText += " AND ISNULL(sq1.modeId,1) = " + modeId;
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblInvoiceTO SelectTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idInvoice = " + idInvoice + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Ramdas.W:@22092017:API This method is used to Get List of Invoice By Status
        /// </summary>
        /// <param name="StatusId"></param>
        /// <param name="distributorOrgId"></param>
        /// <returns></returns>
        public static List<TblInvoiceTO> SelectTblInvoiceByStatus(Int32 statusId, int distributorOrgId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (distributorOrgId == 0)
                {
                    //cmdSelect.CommandText = SqlSelectQuery() + " WHERE invoice.statusId = " + statusId;

                    cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.statusId = " + statusId;

                }
                else
                {
                    //cmdSelect.CommandText = SqlSelectQuery() + " WHERE invoice.statusId = " + statusId + " AND ISNULL(invoice.distributorOrgId,0)=" + distributorOrgId;
                    cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.statusId = " + statusId + " AND ISNULL(sq1.distributorOrgId,0)=" + distributorOrgId;
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null && list.Count >= 1) return list;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public static List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.loadingSlipId = " + loadingSlipId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public static double GetTareWeightFromInvoice(String lodingSlipIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = "SELECT MIN(tareWeight) FROM tempinvoice  WHERE loadingSlipId IN (" + lodingSlipIds + " )";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return Convert.ToDouble(cmdSelect.ExecuteScalar());
             
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public static List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ( " +SqlSelectQuery() + ")sq1  WHERE sq1.loadingSlipId = " + loadingSlipId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Saket [2018-11-05] Added.
        /// </summary>
        /// <param name="loadingSlipIds"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipIds(String loadingSlipIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.loadingSlipId IN (" + loadingSlipIds + ") ";
                //cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE  sq1.idInvoice IN (select invoiceId from tempLoadingSlipInvoice where loadingSlipId IN ( " + loadingSlipIds + " ))";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public static List<TblInvoiceTO> ConvertDTToList(SqlDataReader tblInvoiceTODT)
        {
            List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();
            if (tblInvoiceTODT != null)
            {
                while (tblInvoiceTODT.Read())
                {
                    TblInvoiceTO tblInvoiceTONew = new TblInvoiceTO();
                    if (tblInvoiceTODT["idInvoice"] != DBNull.Value)
                        tblInvoiceTONew.IdInvoice = Convert.ToInt32(tblInvoiceTODT["idInvoice"].ToString());
                    if (tblInvoiceTODT["invoiceTypeId"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceTypeId = Convert.ToInt32(tblInvoiceTODT["invoiceTypeId"].ToString());
                    if (tblInvoiceTODT["transportOrgId"] != DBNull.Value)
                        tblInvoiceTONew.TransportOrgId = Convert.ToInt32(tblInvoiceTODT["transportOrgId"].ToString());
                    if (tblInvoiceTODT["transportModeId"] != DBNull.Value)
                        tblInvoiceTONew.TransportModeId = Convert.ToInt32(tblInvoiceTODT["transportModeId"].ToString());
                    if (tblInvoiceTODT["currencyId"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyId = Convert.ToInt32(tblInvoiceTODT["currencyId"].ToString());
                    if (tblInvoiceTODT["loadingSlipId"] != DBNull.Value)
                        tblInvoiceTONew.LoadingSlipId = Convert.ToInt32(tblInvoiceTODT["loadingSlipId"].ToString());
                    if (tblInvoiceTODT["distributorOrgId"] != DBNull.Value)
                        tblInvoiceTONew.DistributorOrgId = Convert.ToInt32(tblInvoiceTODT["distributorOrgId"].ToString());
                    if (tblInvoiceTODT["dealerOrgId"] != DBNull.Value)
                        tblInvoiceTONew.DealerOrgId = Convert.ToInt32(tblInvoiceTODT["dealerOrgId"].ToString());
                    if (tblInvoiceTODT["finYearId"] != DBNull.Value)
                        tblInvoiceTONew.FinYearId = Convert.ToInt32(tblInvoiceTODT["finYearId"].ToString());
                    if (tblInvoiceTODT["statusId"] != DBNull.Value)
                        tblInvoiceTONew.StatusId = Convert.ToInt32(tblInvoiceTODT["statusId"].ToString());
                    if (tblInvoiceTODT["createdBy"] != DBNull.Value)
                        tblInvoiceTONew.CreatedBy = Convert.ToInt32(tblInvoiceTODT["createdBy"].ToString());
                    if (tblInvoiceTODT["updatedBy"] != DBNull.Value)
                        tblInvoiceTONew.UpdatedBy = Convert.ToInt32(tblInvoiceTODT["updatedBy"].ToString());
                    if (tblInvoiceTODT["invoiceDate"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceDate = Convert.ToDateTime(tblInvoiceTODT["invoiceDate"].ToString());
                    if (tblInvoiceTODT["lrDate"] != DBNull.Value)
                        tblInvoiceTONew.LrDate = Convert.ToDateTime(tblInvoiceTODT["lrDate"].ToString());
                    if (tblInvoiceTODT["statusDate"] != DBNull.Value)
                        tblInvoiceTONew.StatusDate = Convert.ToDateTime(tblInvoiceTODT["statusDate"].ToString());
                    if (tblInvoiceTODT["createdOn"] != DBNull.Value)
                        tblInvoiceTONew.CreatedOn = Convert.ToDateTime(tblInvoiceTODT["createdOn"].ToString());
                    if (tblInvoiceTODT["updatedOn"] != DBNull.Value)
                        tblInvoiceTONew.UpdatedOn = Convert.ToDateTime(tblInvoiceTODT["updatedOn"].ToString());
                    if (tblInvoiceTODT["currencyRate"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyRate = Convert.ToDouble(tblInvoiceTODT["currencyRate"].ToString());
                    if (tblInvoiceTODT["basicAmt"] != DBNull.Value)
                        tblInvoiceTONew.BasicAmt = Convert.ToDouble(tblInvoiceTODT["basicAmt"].ToString());
                    if (tblInvoiceTODT["discountAmt"] != DBNull.Value)
                        tblInvoiceTONew.DiscountAmt = Convert.ToDouble(tblInvoiceTODT["discountAmt"].ToString());
                    if (tblInvoiceTODT["taxableAmt"] != DBNull.Value)
                        tblInvoiceTONew.TaxableAmt = Convert.ToDouble(tblInvoiceTODT["taxableAmt"].ToString());
                    if (tblInvoiceTODT["cgstAmt"] != DBNull.Value)
                        tblInvoiceTONew.CgstAmt = Convert.ToDouble(tblInvoiceTODT["cgstAmt"].ToString());
                    if (tblInvoiceTODT["sgstAmt"] != DBNull.Value)
                        tblInvoiceTONew.SgstAmt = Convert.ToDouble(tblInvoiceTODT["sgstAmt"].ToString());
                    if (tblInvoiceTODT["igstAmt"] != DBNull.Value)
                        tblInvoiceTONew.IgstAmt = Convert.ToDouble(tblInvoiceTODT["igstAmt"].ToString());
                    if (tblInvoiceTODT["freightPct"] != DBNull.Value)
                        tblInvoiceTONew.FreightPct = Convert.ToDouble(tblInvoiceTODT["freightPct"].ToString());
                    if (tblInvoiceTODT["freightAmt"] != DBNull.Value)
                        tblInvoiceTONew.FreightAmt = Convert.ToDouble(tblInvoiceTODT["freightAmt"].ToString());
                    if (tblInvoiceTODT["roundOffAmt"] != DBNull.Value)
                        tblInvoiceTONew.RoundOffAmt = Convert.ToDouble(tblInvoiceTODT["roundOffAmt"].ToString());
                    if (tblInvoiceTODT["grandTotal"] != DBNull.Value)
                        tblInvoiceTONew.GrandTotal = Convert.ToDouble(tblInvoiceTODT["grandTotal"].ToString());
                    if (tblInvoiceTODT["invoiceNo"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceNo = Convert.ToString(tblInvoiceTODT["invoiceNo"].ToString());
                    if (tblInvoiceTODT["electronicRefNo"] != DBNull.Value)
                        tblInvoiceTONew.ElectronicRefNo = Convert.ToString(tblInvoiceTODT["electronicRefNo"].ToString());
                    if (tblInvoiceTODT["vehicleNo"] != DBNull.Value)
                        tblInvoiceTONew.VehicleNo = Convert.ToString(tblInvoiceTODT["vehicleNo"].ToString());
                    if (tblInvoiceTODT["lrNumber"] != DBNull.Value)
                        tblInvoiceTONew.LrNumber = Convert.ToString(tblInvoiceTODT["lrNumber"].ToString());
                    if (tblInvoiceTODT["roadPermitNo"] != DBNull.Value)
                        tblInvoiceTONew.RoadPermitNo = Convert.ToString(tblInvoiceTODT["roadPermitNo"].ToString());
                    if (tblInvoiceTODT["transportorForm"] != DBNull.Value)
                        tblInvoiceTONew.TransportorForm = Convert.ToString(tblInvoiceTODT["transportorForm"].ToString());
                    if (tblInvoiceTODT["airwayBillNo"] != DBNull.Value)
                        tblInvoiceTONew.AirwayBillNo = Convert.ToString(tblInvoiceTODT["airwayBillNo"].ToString());
                    if (tblInvoiceTODT["narration"] != DBNull.Value)
                        tblInvoiceTONew.Narration = Convert.ToString(tblInvoiceTODT["narration"].ToString());
                    if (tblInvoiceTODT["bankDetails"] != DBNull.Value)
                        tblInvoiceTONew.BankDetails = Convert.ToString(tblInvoiceTODT["bankDetails"].ToString());
                    if (tblInvoiceTODT["invoiceModeId"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceModeId = Convert.ToInt32(tblInvoiceTODT["invoiceModeId"]);

                    if (tblInvoiceTODT["dealerName"] != DBNull.Value)
                        tblInvoiceTONew.DealerName = Convert.ToString(tblInvoiceTODT["dealerName"].ToString());
                    if (tblInvoiceTODT["distributorName"] != DBNull.Value)
                        tblInvoiceTONew.DistributorName = Convert.ToString(tblInvoiceTODT["distributorName"].ToString());
                    if (tblInvoiceTODT["transporterName"] != DBNull.Value)
                        tblInvoiceTONew.TransporterName = Convert.ToString(tblInvoiceTODT["transporterName"].ToString());
                    if (tblInvoiceTODT["currencyName"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyName = Convert.ToString(tblInvoiceTODT["currencyName"].ToString());
                    if (tblInvoiceTODT["statusName"] != DBNull.Value)
                        tblInvoiceTONew.StatusName = Convert.ToString(tblInvoiceTODT["statusName"].ToString());
                    if (tblInvoiceTODT["invoiceTypeDesc"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceTypeDesc = Convert.ToString(tblInvoiceTODT["invoiceTypeDesc"].ToString());
                    if (tblInvoiceTODT["deliveryLocation"] != DBNull.Value)
                        tblInvoiceTONew.DeliveryLocation = Convert.ToString(tblInvoiceTODT["deliveryLocation"]);

                    if (tblInvoiceTODT["changeIn"] != DBNull.Value)
                        tblInvoiceTONew.ChangeIn = Convert.ToString(tblInvoiceTODT["changeIn"]);
                    if (tblInvoiceTODT["expenseAmt"] != DBNull.Value)
                        tblInvoiceTONew.ExpenseAmt = Convert.ToDouble(tblInvoiceTODT["expenseAmt"].ToString());
                    if (tblInvoiceTODT["otherAmt"] != DBNull.Value)
                        tblInvoiceTONew.OtherAmt = Convert.ToDouble(tblInvoiceTODT["otherAmt"].ToString());
                    if (tblInvoiceTODT["tareWeight"] != DBNull.Value)
                        tblInvoiceTONew.TareWeight = Convert.ToDouble(tblInvoiceTODT["tareWeight"]);
                    if (tblInvoiceTODT["netWeight"] != DBNull.Value)
                        tblInvoiceTONew.NetWeight = Convert.ToDouble(tblInvoiceTODT["netWeight"]);
                    if (tblInvoiceTODT["grossWeight"] != DBNull.Value)
                        tblInvoiceTONew.GrossWeight = Convert.ToDouble(tblInvoiceTODT["grossWeight"]);
                    if (tblInvoiceTODT["isConfirmed"] != DBNull.Value)
                        tblInvoiceTONew.IsConfirmed = Convert.ToInt32(tblInvoiceTODT["isConfirmed"]);
                    if (tblInvoiceTODT["rcmFlag"] != DBNull.Value)
                        tblInvoiceTONew.RcmFlag = Convert.ToInt32(tblInvoiceTODT["rcmFlag"]);
                    if (tblInvoiceTODT["remark"] != DBNull.Value)
                        tblInvoiceTONew.Remark = Convert.ToString(tblInvoiceTODT["remark"]);

                    if (tblInvoiceTODT["invFromOrgId"] != DBNull.Value)
                        tblInvoiceTONew.InvFromOrgId = Convert.ToInt32(tblInvoiceTODT["invFromOrgId"]);

                    if (tblInvoiceTODT["deliveredOn"] != DBNull.Value)
                        tblInvoiceTONew.DeliveredOn = Convert.ToDateTime(tblInvoiceTODT["deliveredOn"].ToString());

                    if (tblInvoiceTODT["loadingStatusId"] != DBNull.Value)
                        tblInvoiceTONew.LoadingStatusId = Convert.ToInt32(tblInvoiceTODT["loadingStatusId"]);
                    if (tblInvoiceTODT["isDBup"] != DBNull.Value)
                        tblInvoiceTONew.IsDBup = Convert.ToInt32(tblInvoiceTODT["isDBup"]);
                    if (tblInvoiceTODT["invFromOrgFreeze"] != DBNull.Value)
                        tblInvoiceTONew.InvFromOrgFreeze = Convert.ToInt32(tblInvoiceTODT["invFromOrgFreeze"].ToString());

                    if (tblInvoiceTODT["IrnNo"] != DBNull.Value)
                        tblInvoiceTONew.IrnNo = tblInvoiceTODT["IrnNo"].ToString();
                    if (tblInvoiceTODT["isEInvGenerated"] != DBNull.Value)
                        tblInvoiceTONew.IsEInvGenerated = Convert.ToInt32(tblInvoiceTODT["isEInvGenerated"].ToString());
                    if (tblInvoiceTODT["distanceInKM"] != DBNull.Value)
                        tblInvoiceTONew.DistanceInKM = Convert.ToInt32(tblInvoiceTODT["distanceInKM"].ToString());
                    if (tblInvoiceTODT["isEwayBillGenerated"] != DBNull.Value)
                        tblInvoiceTONew.IsEWayBillGenerated = Convert.ToInt32(tblInvoiceTODT["isEwayBillGenerated"].ToString());
                    if (tblInvoiceTODT["tdsAmt"] != DBNull.Value)
                        tblInvoiceTONew.TdsAmt = Convert.ToDouble(tblInvoiceTODT["tdsAmt"].ToString());

                    tblInvoiceTOList.Add(tblInvoiceTONew);
                }
            }
            return tblInvoiceTOList;
        }

        //public static List<TblInvoiceRptTO> SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, string strOrgTempId)
        //{
        //    String sqlConnStr = Startup.ConnectionString;
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    SqlDataReader reader = null;
        //    string selectQuery = String.Empty;
        //    DateTime sysDate = Constants.ServerDateTime;
        //    try
        //    {
        //        conn.Open();
        //        selectQuery =
        //            " Select invoice.idInvoice,invoice.invoiceNo,invoice.statusDate ,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn," +
        //                   " invoiceAddress.billingName as partyName, org.firmName cnfName,  " +
        //                   "  booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
        //                   "  itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, " +
        //                  "   itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId,itemTaxDetails.taxRatePct ,taxRate.taxTypeId , " +
        //                    " invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , itemTaxDetails.taxAmt,itemDetails.grandTotal," +
        //                    " invoice.isConfirmed,invoiceAddress.txnAddrTypeId,invoice.statusId,invoiceAddress.taluka as talukaName " +
        //                    " ,invoice.deliveredOn,invoice.invFromOrgId FROM tempInvoice invoice " +
        //                    " INNER JOIN tempInvoiceAddress invoiceAddress " +
        //                    " ON invoiceAddress.invoiceId = invoice.idInvoice " +
        //                    " INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId " +
        //                    " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
        //                    " LEFT JOIN tempLoadingSlipExt lExt " +
        //                    " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
        //                    " LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking " +
        //                    " LEFT JOIN tempInvoiceItemTaxDtls itemTaxDetails " +
        //                    " ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem " +
        //                    " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId " +

        //                   // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipExt and finalInvoice and finalInvoiceAddress and finalInvoiceItemDetails and finalInvoiceItemTaxDtls

        //                   " UNION ALL " +
        //                   " Select invoice.idInvoice,invoice.invoiceNo,invoice.statusDate ,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn,   " +
        //                   " invoiceAddress.billingName as partyName, org.firmName cnfName,  " +
        //                   "  booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
        //                   "  itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, " +
        //                   "   itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId,itemTaxDetails.taxRatePct ,taxRate.taxTypeId , " +
        //                    " invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , itemTaxDetails.taxAmt,itemDetails.grandTotal," +
        //                    " invoice.isConfirmed,invoiceAddress.txnAddrTypeId,invoice.statusId,invoiceAddress.taluka as talukaName " +
        //                    " ,invoice.deliveredOn,invoice.invFromOrgId FROM finalInvoice invoice " +
        //                    " INNER JOIN finalInvoiceAddress invoiceAddress " +
        //                    " ON invoiceAddress.invoiceId = invoice.idInvoice " +
        //                    " INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId " +
        //                    " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
        //                    " LEFT JOIN finalLoadingSlipExt lExt " +
        //                    " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
        //                    " LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking " +
        //                    " LEFT JOIN finalInvoiceItemTaxDtls itemTaxDetails " +
        //                    " ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem " +
        //                    " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId ";


        //        //cmdSelect.CommandText = selectQuery + " WHERE invoice.isConfirmed =" + isConfirm +
        //        //     " AND CAST(invoice.invoiceDate AS DATE) BETWEEN @fromDate AND @toDate" +
        //        //     " AND invoiceAddress.txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS +
        //        //     " AND invoice.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED;

        //        //Vaibhav comment and modify
        //        cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
        //             " AND CAST(sq1.deliveredOn AS DATE) BETWEEN @fromDate AND @toDate" +
        //             " AND sq1.txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS +
        //             " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED +
        //             " AND sq1.invFromOrgId in(" + strOrgTempId + ")" +
        //             " order by sq1.deliveredOn asc";


        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;
        //        cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
        //        cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
        //        reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (reader != null) reader.Dispose();
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        /// <summary>
        /// Vijaymala[15-09-2017] Added To Get Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, string strOrgTempId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                    " Select invoice.idInvoice,invoice.invoiceNo,invoice.statusDate ,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn," +
                           " invoiceAddress.billingName as partyName, org.firmName cnfName,  " +
                           "  booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
                           "  mat.materialSubType as materialName , tblProductItem.itemName ,  " + 
                           "  itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, invoice.tdsAmt, " +
                           "   itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId,itemTaxDetails.taxRatePct , " +
                            " invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , itemTaxDetails.taxAmt,itemDetails.grandTotal," +
                            " invoice.isConfirmed,invoiceAddress.txnAddrTypeId,invoice.statusId,invoiceAddress.taluka as talukaName " +
                            " ,invoice.deliveredOn,invoice.invFromOrgId,weighingMeasures.machineName AS 'godown' ,taxRate.taxTypeId FROM tempInvoice invoice " +
                            " INNER JOIN tempInvoiceAddress invoiceAddress " +
                            " ON invoiceAddress.invoiceId = invoice.idInvoice " +
                            " INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId " +
                            " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                            " LEFT JOIN tempLoadingSlipExt lExt " +
                            " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                            " LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking " +
                            " LEFT JOIN tempInvoiceItemTaxDtls itemTaxDetails " +
                            " ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem " +
                            " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId " +

                            " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                            " LEFT JOIN  tblProductItem tblProductItem on prodGstCodeDtl.prodItemId = tblProductItem.idProdItem" +
                            " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +

                            //Added by minal 31 March 2021 for Deliver Report
                            " LEFT JOIN tempLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = invoice.loadingSlipId" +
                            " LEFT JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId" +
                            " LEFT JOIN (" +
                            " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                            " tempWeighingMeasures.idWeightMeasure,tblWeighingMachine.machineName,tempWeighingMeasures.loadingId" +
                            " FROM tempWeighingMeasures tempWeighingMeasures" +
                            " LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tempWeighingMeasures.weighingMachineId" +
                            " WHERE tempWeighingMeasures.weightMeasurTypeId = 1 ) AS weighingMeasures " +
                            " ON weighingMeasures.loadingId = loading.idLoading and weighingMeasures.row_number = 1" +
                           //Added by minal
                           // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipExt and finalInvoice and finalInvoiceAddress and finalInvoiceItemDetails and finalInvoiceItemTaxDtls

                           " UNION ALL " +
                           " Select invoice.idInvoice,invoice.invoiceNo,invoice.statusDate ,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn,   " +
                           " invoiceAddress.billingName as partyName, org.firmName cnfName,  " +
                           "  booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
                           "  mat.materialSubType as materialName , tblProductItem.itemName ,  " +
                           "  itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, " +
                           "   itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId,itemTaxDetails.taxRatePct ,invoice.tdsAmt, " +
                            " invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , itemTaxDetails.taxAmt,itemDetails.grandTotal," +
                            " invoice.isConfirmed,invoiceAddress.txnAddrTypeId,invoice.statusId,invoiceAddress.taluka as talukaName " +
                            " ,invoice.deliveredOn,invoice.invFromOrgId,weighingMeasures.machineName AS 'godown',taxRate.taxTypeId FROM finalInvoice invoice " +
                            " INNER JOIN finalInvoiceAddress invoiceAddress " +
                            " ON invoiceAddress.invoiceId = invoice.idInvoice " +
                            " INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId " +
                            " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                            " LEFT JOIN finalLoadingSlipExt lExt " +
                            " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                            " LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking " +
                            " LEFT JOIN finalInvoiceItemTaxDtls itemTaxDetails " +
                            " ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem " +
                            " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId " +

                            " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                            " LEFT JOIN  tblProductItem tblProductItem on prodGstCodeDtl.prodItemId = tblProductItem.idProdItem" +
                            " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +

                            //Added by minal 31 March 2021 For Deliver
                            " LEFT JOIN finalLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = invoice.loadingSlipId" +
                            " LEFT JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId" +
                            " LEFT JOIN (" +
                            " SELECT ROW_NUMBER() OVER(PARTITION BY finalWeighingMeasures.loadingId ORDER BY finalWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                            " finalWeighingMeasures.idWeightMeasure,tblWeighingMachine.machineName,finalWeighingMeasures.loadingId" +
                            " FROM finalWeighingMeasures finalWeighingMeasures" +
                            " LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = finalWeighingMeasures.weighingMachineId" +
                            " WHERE finalWeighingMeasures.weightMeasurTypeId = 1 ) AS weighingMeasures" +
                            " ON weighingMeasures.loadingId = loading.idLoading and weighingMeasures.row_number = 1";

                //cmdSelect.CommandText = selectQuery + " WHERE invoice.isConfirmed =" + isConfirm +
                //     " AND CAST(invoice.invoiceDate AS DATE) BETWEEN @fromDate AND @toDate" +
                //     " AND invoiceAddress.txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS +
                //     " AND invoice.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED;

                //Vaibhav comment and modify
                cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     " AND CAST(sq1.deliveredOn AS DATE) BETWEEN @fromDate AND @toDate" +
                     " AND sq1.txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED +
                     " AND sq1.invFromOrgId in(" + strOrgTempId + ")" +
                     " order by sq1.deliveredOn asc";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Vijaymala[15-09-2017] Added This method to convert dt to rpt invoice List
        /// </summary>
        /// <param name="tblInvoiceRptTODT"></param>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> ConvertDTToListForRPTInvoice(SqlDataReader tblInvoiceRptTODT)
        {
            List<TblInvoiceRptTO> tblInvoiceRPtTOList = new List<TblInvoiceRptTO>();
            try
            {
                if (tblInvoiceRptTODT != null)
                {

                    while (tblInvoiceRptTODT.Read())
                    {
                        TblInvoiceRptTO tblInvoiceRptTONew = new TblInvoiceRptTO();
                        for (int i = 0; i < tblInvoiceRptTODT.FieldCount; i++)
                        {
                            if (tblInvoiceRptTODT.GetName(i).Equals("mode"))
                            {
                                if (tblInvoiceRptTODT["mode"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceMode = Convert.ToString(tblInvoiceRptTODT["mode"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("idInvoice"))
                            {
                                if (tblInvoiceRptTODT["idInvoice"] != DBNull.Value)
                                    tblInvoiceRptTONew.IdInvoice = Convert.ToInt32(tblInvoiceRptTODT["idInvoice"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceNo"))
                            {
                                if (tblInvoiceRptTODT["invoiceNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceNo = Convert.ToString(tblInvoiceRptTODT["invoiceNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("vehicleNo"))
                            {
                                if (tblInvoiceRptTODT["vehicleNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.VehicleNo = Convert.ToString(tblInvoiceRptTODT["vehicleNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceDate"))
                            {
                                if (tblInvoiceRptTODT["invoiceDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceDate = Convert.ToDateTime(tblInvoiceRptTODT["invoiceDate"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("electronicRefNo"))
                            {
                                if (tblInvoiceRptTODT["electronicRefNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.ElectronicRefNo = Convert.ToString(tblInvoiceRptTODT["electronicRefNo"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceTypeDesc"))
                            {
                                if (tblInvoiceRptTODT["invoiceTypeDesc"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceTypeDesc = Convert.ToString(tblInvoiceRptTODT["invoiceTypeDesc"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("IrnNo"))
                            {
                                if (tblInvoiceRptTODT["IrnNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.IrnNo = Convert.ToString(tblInvoiceRptTODT["IrnNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("codeNumber"))
                            {
                                if (tblInvoiceRptTODT["codeNumber"] != DBNull.Value)
                                    tblInvoiceRptTONew.CodeNumber = Convert.ToString(tblInvoiceRptTODT["codeNumber"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("partyName"))
                            {
                                if (tblInvoiceRptTODT["partyName"] != DBNull.Value)
                                    tblInvoiceRptTONew.PartyName = Convert.ToString(tblInvoiceRptTODT["partyName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cnfName"))
                            {
                                if (tblInvoiceRptTODT["cnfName"] != DBNull.Value)
                                    tblInvoiceRptTONew.CnfName = Convert.ToString(tblInvoiceRptTODT["cnfName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("bookingRate"))
                            {
                                if (tblInvoiceRptTODT["bookingRate"] != DBNull.Value)
                                    tblInvoiceRptTONew.BookingRate = Convert.ToDouble(tblInvoiceRptTODT["bookingRate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceItemId"))
                            {
                                if (tblInvoiceRptTODT["invoiceItemId"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceItemId = Convert.ToInt32(tblInvoiceRptTODT["invoiceItemId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("prodItemDesc"))
                            {
                                if (tblInvoiceRptTODT["prodItemDesc"] != DBNull.Value)
                                    tblInvoiceRptTONew.ProdItemDesc = Convert.ToString(tblInvoiceRptTODT["prodItemDesc"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("bundles"))
                            {
                                if (tblInvoiceRptTODT["bundles"] != DBNull.Value)
                                    tblInvoiceRptTONew.Bundles = Convert.ToDouble(tblInvoiceRptTODT["bundles"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("tareWeight"))
                            {
                                if (tblInvoiceRptTODT["tareWeight"] != DBNull.Value)
                                    tblInvoiceRptTONew.TareWeight = Convert.ToDouble(tblInvoiceRptTODT["tareWeight"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("igstPct"))
                            {
                                if (tblInvoiceRptTODT["igstPct"] != DBNull.Value)
                                    tblInvoiceRptTONew.IgstPct = Convert.ToDouble(tblInvoiceRptTODT["igstPct"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("sgstPct"))
                            {
                                if (tblInvoiceRptTODT["sgstPct"] != DBNull.Value)
                                    tblInvoiceRptTONew.SgstPct = Convert.ToDouble(tblInvoiceRptTODT["sgstPct"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cgstPct"))
                            {
                                if (tblInvoiceRptTODT["cgstPct"] != DBNull.Value)
                                    tblInvoiceRptTONew.CgstPct = Convert.ToDouble(tblInvoiceRptTODT["cgstPct"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("grossWeight"))
                            {
                                if (tblInvoiceRptTODT["grossWeight"] != DBNull.Value)
                                    tblInvoiceRptTONew.GrossWeight = Convert.ToDouble(tblInvoiceRptTODT["grossWeight"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("netWeight"))
                            {
                                if (tblInvoiceRptTODT["netWeight"] != DBNull.Value)
                                    tblInvoiceRptTONew.NetWeight = Convert.ToDouble(tblInvoiceRptTODT["netWeight"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("roundOffAmt"))
                            {
                                if (tblInvoiceRptTODT["roundOffAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.RoundOffAmt = Convert.ToDouble(tblInvoiceRptTODT["roundOffAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("rate"))
                            {
                                if (tblInvoiceRptTODT["rate"] != DBNull.Value)
                                    tblInvoiceRptTONew.Rate = Convert.ToDouble(tblInvoiceRptTODT["rate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cdStructure"))
                            {
                                if (tblInvoiceRptTODT["cdStructure"] != DBNull.Value)
                                    tblInvoiceRptTONew.CdStructure = Convert.ToDouble(tblInvoiceRptTODT["cdStructure"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cdAmt"))
                            {
                                if (tblInvoiceRptTODT["cdAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.CdAmt = Convert.ToDouble(tblInvoiceRptTODT["cdAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("taxRatePct"))
                            {
                                if (tblInvoiceRptTODT["taxRatePct"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxRatePct = Convert.ToDouble(tblInvoiceRptTODT["taxRatePct"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("taxTypeId"))
                            {
                                if (tblInvoiceRptTODT["taxTypeId"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxTypeId = Convert.ToInt32(tblInvoiceRptTODT["taxTypeId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("freightAmt"))
                            {
                                if (tblInvoiceRptTODT["freightAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.FreightAmt = Convert.ToDouble(tblInvoiceRptTODT["freightAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("tcsAmt"))
                            {
                                if (tblInvoiceRptTODT["tcsAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.TcsAmt = Convert.ToDouble(tblInvoiceRptTODT["tcsAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceQty"))
                            {
                                if (tblInvoiceRptTODT["invoiceQty"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceQty = Convert.ToDouble(tblInvoiceRptTODT["invoiceQty"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("taxableAmt"))
                            {
                                if (tblInvoiceRptTODT["taxableAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxableAmt = Convert.ToDouble(tblInvoiceRptTODT["taxableAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("taxAmt"))
                            {
                                if (tblInvoiceRptTODT["taxAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxAmt = Convert.ToDouble(tblInvoiceRptTODT["taxAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("billingTypeId"))
                            {
                                if (tblInvoiceRptTODT["billingTypeId"] != DBNull.Value)
                                    tblInvoiceRptTONew.BillingTypeId = Convert.ToInt32(tblInvoiceRptTODT["billingTypeId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyer"))
                            {
                                if (tblInvoiceRptTODT["buyer"] != DBNull.Value)
                                    tblInvoiceRptTONew.Buyer = Convert.ToString(tblInvoiceRptTODT["buyer"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgstateName"))
                            {
                                if (tblInvoiceRptTODT["OrgstateName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgstateName = Convert.ToString(tblInvoiceRptTODT["OrgstateName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgvillageName"))
                            {
                                if (tblInvoiceRptTODT["OrgcountryName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgcountryName = Convert.ToString(tblInvoiceRptTODT["OrgcountryName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgvillageName"))
                            {
                                if (tblInvoiceRptTODT["OrgvillageName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgvillageName = Convert.ToString(tblInvoiceRptTODT["OrgvillageName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgdistrictName"))
                            {
                                if (tblInvoiceRptTODT["OrgdistrictName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgdistrictName = Convert.ToString(tblInvoiceRptTODT["OrgdistrictName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invFromOrgName"))
                            {
                                if (tblInvoiceRptTODT["invFromOrgName"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvFromOrgName = Convert.ToString(tblInvoiceRptTODT["invFromOrgName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgareaName"))
                            {
                                if (tblInvoiceRptTODT["OrgareaName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgareaName = Convert.ToString(tblInvoiceRptTODT["OrgareaName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("Orgpincode"))
                            {
                                if (tblInvoiceRptTODT["Orgpincode"] != DBNull.Value)
                                    tblInvoiceRptTONew.Orgpincode = Convert.ToString(tblInvoiceRptTODT["Orgpincode"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgplotNo"))
                            {
                                if (tblInvoiceRptTODT["OrgplotNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgplotNo = Convert.ToString(tblInvoiceRptTODT["OrgplotNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerGstNo"))
                            {
                                if (tblInvoiceRptTODT["buyerGstNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerGstNo = Convert.ToString(tblInvoiceRptTODT["buyerGstNo"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerAddress"))
                            {
                                if (tblInvoiceRptTODT["buyerAddress"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerAddress = Convert.ToString(tblInvoiceRptTODT["buyerAddress"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerDistrict"))
                            {
                                if (tblInvoiceRptTODT["buyerDistrict"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerDistrict = Convert.ToString(tblInvoiceRptTODT["buyerDistrict"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerPinCode"))
                            {
                                if (tblInvoiceRptTODT["buyerPinCode"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerPinCode = Convert.ToString(tblInvoiceRptTODT["buyerPinCode"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerTaluka"))
                            {
                                if (tblInvoiceRptTODT["buyerTaluka"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerTaluka = Convert.ToString(tblInvoiceRptTODT["buyerTaluka"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeTaluka"))
                            {
                                if (tblInvoiceRptTODT["consigneeTaluka"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeTaluka = Convert.ToString(tblInvoiceRptTODT["consigneeTaluka"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeTypeId"))
                            {
                                if (tblInvoiceRptTODT["consigneeTypeId"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeTypeId = Convert.ToInt32(tblInvoiceRptTODT["consigneeTypeId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consignee"))
                            {
                                if (tblInvoiceRptTODT["consignee"] != DBNull.Value)
                                    tblInvoiceRptTONew.Consignee = Convert.ToString(tblInvoiceRptTODT["consignee"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeGstNo"))
                            {
                                if (tblInvoiceRptTODT["consigneeGstNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeGstNo = Convert.ToString(tblInvoiceRptTODT["consigneeGstNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("deliveryLocation"))
                            {
                                if (tblInvoiceRptTODT["deliveryLocation"] != DBNull.Value)
                                    tblInvoiceRptTONew.DeliveryLocation = Convert.ToString(tblInvoiceRptTODT["deliveryLocation"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i) == "basicAmt")
                            {
                                if (tblInvoiceRptTODT["basicAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.BasicAmt = Convert.ToDouble(tblInvoiceRptTODT["basicAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cgstAmt"))
                            {
                                if (tblInvoiceRptTODT["cgstAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.CgstTaxAmt = Convert.ToDouble(tblInvoiceRptTODT["cgstAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("sgstAmt"))
                            {
                                if (tblInvoiceRptTODT["sgstAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.SgstTaxAmt = Convert.ToDouble(tblInvoiceRptTODT["sgstAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("igstAmt"))
                            {
                                if (tblInvoiceRptTODT["igstAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.IgstTaxAmt = Convert.ToDouble(tblInvoiceRptTODT["igstAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("grandTotal"))
                            {
                                if (tblInvoiceRptTODT["grandTotal"] != DBNull.Value)
                                    tblInvoiceRptTONew.GrandTotal = Convert.ToDouble(tblInvoiceRptTODT["grandTotal"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("createdOn"))
                            {
                                if (tblInvoiceRptTODT["createdOn"] != DBNull.Value)
                                    tblInvoiceRptTONew.CreatedOn = Convert.ToDateTime(tblInvoiceRptTODT["createdOn"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("gstinCodeNo"))
                            {
                                if (tblInvoiceRptTODT["gstinCodeNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.GstinCodeNo = Convert.ToInt32(tblInvoiceRptTODT["gstinCodeNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("stateId"))
                            {
                                if (tblInvoiceRptTODT["stateId"] != DBNull.Value)
                                    tblInvoiceRptTONew.StateId = Convert.ToInt32(tblInvoiceRptTODT["stateId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("isConfirmed"))
                            {
                                if (tblInvoiceRptTODT["isConfirmed"] != DBNull.Value)
                                    tblInvoiceRptTONew.IsConfirmed = Convert.ToInt32(tblInvoiceRptTODT["isConfirmed"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("otherTaxId"))
                            {
                                if (tblInvoiceRptTODT["otherTaxId"] != DBNull.Value)
                                    tblInvoiceRptTONew.OtherTaxId = Convert.ToInt32(tblInvoiceRptTODT["otherTaxId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("statusId"))
                            {
                                if (tblInvoiceRptTODT["statusId"] != DBNull.Value)
                                    tblInvoiceRptTONew.StatusId = Convert.ToInt32(tblInvoiceRptTODT["statusId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("stateOrUTCode"))
                            {
                                if (tblInvoiceRptTODT["stateOrUTCode"] != DBNull.Value)
                                    tblInvoiceRptTONew.StateOrUTCode = Convert.ToInt32(tblInvoiceRptTODT["stateOrUTCode"].ToString());
                            }


                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerState"))
                            {
                                if (tblInvoiceRptTODT["buyerState"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerState = Convert.ToString(tblInvoiceRptTODT["buyerState"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeAddress"))
                            {
                                if (tblInvoiceRptTODT["consigneeAddress"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeAddress = Convert.ToString(tblInvoiceRptTODT["consigneeAddress"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeDistict"))
                            {
                                if (tblInvoiceRptTODT["consigneeDistict"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeDistict = Convert.ToString(tblInvoiceRptTODT["consigneeDistict"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneePinCode"))
                            {
                                if (tblInvoiceRptTODT["consigneePinCode"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneePinCode = Convert.ToString(tblInvoiceRptTODT["consigneePinCode"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeState"))
                            {
                                if (tblInvoiceRptTODT["consigneeState"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeState = Convert.ToString(tblInvoiceRptTODT["consigneeState"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("materialName"))
                            {
                                if (tblInvoiceRptTODT["materialName"] != DBNull.Value)
                                    tblInvoiceRptTONew.MaterialName = Convert.ToString(tblInvoiceRptTODT["materialName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("transporterName"))
                            {
                                if (tblInvoiceRptTODT["transporterName"] != DBNull.Value)
                                    tblInvoiceRptTONew.TransporterName = Convert.ToString(tblInvoiceRptTODT["transporterName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("contactNo"))
                            {
                                if (tblInvoiceRptTODT["contactNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.ContactNo = Convert.ToString(tblInvoiceRptTODT["contactNo"].ToString());
                            }                        
                            if (tblInvoiceRptTODT.GetName(i).Equals("statusDate"))
                            {
                                if (tblInvoiceRptTODT["statusDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.StatusDate = Convert.ToDateTime(tblInvoiceRptTODT["statusDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cnfMobNo"))
                            {
                                if (tblInvoiceRptTODT["cnfMobNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.CnfMobNo = Convert.ToString(tblInvoiceRptTODT["cnfMobNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("dealerMobNo"))
                            {
                                if (tblInvoiceRptTODT["dealerMobNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.DealerMobNo = Convert.ToString(tblInvoiceRptTODT["dealerMobNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("lrNumber"))
                            {
                                if (tblInvoiceRptTODT["lrNumber"] != DBNull.Value)
                                    tblInvoiceRptTONew.LrNumber = Convert.ToString(tblInvoiceRptTODT["lrNumber"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("lrDate"))
                            {
                                if (tblInvoiceRptTODT["lrDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.LrDate = Convert.ToDateTime(tblInvoiceRptTODT["lrDate"].ToString());
                            }

                            // Vaibhav [18-Jan-2018]  Select loadingId
                            if (tblInvoiceRptTODT.GetName(i).Equals("loadingId"))
                            {
                                if (tblInvoiceRptTODT["loadingId"] != DBNull.Value)
                                    tblInvoiceRptTONew.LoadingId = Convert.ToInt32(tblInvoiceRptTODT["loadingId"].ToString());
                            }

                            //Vijaymala [16-03-2018] Added
                            if (tblInvoiceRptTODT.GetName(i).Equals("talukaName"))
                            {
                                if (tblInvoiceRptTODT["talukaName"] != DBNull.Value)
                                    tblInvoiceRptTONew.TalukaName = Convert.ToString(tblInvoiceRptTODT["talukaName"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("deliveredOn"))
                            {
                                if (tblInvoiceRptTODT["deliveredOn"] != DBNull.Value)
                                    tblInvoiceRptTONew.DeliveredOn = Convert.ToDateTime(tblInvoiceRptTODT["deliveredOn"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("godown"))
                            {
                                if (tblInvoiceRptTODT["godown"] != DBNull.Value)
                                    tblInvoiceRptTONew.Godown = Convert.ToString(tblInvoiceRptTODT["godown"].ToString());
                            }
                            
                            if (tblInvoiceRptTODT.GetName(i).Equals("tdsAmt"))
                            {
                                if (tblInvoiceRptTODT["tdsAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.TdsAmt = Convert.ToDouble(tblInvoiceRptTODT["tdsAmt"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("isInternalCnf"))
                            {
                                if (tblInvoiceRptTODT["isInternalCnf"] != DBNull.Value)
                                    tblInvoiceRptTONew.InternalCnf = Convert.ToInt32(tblInvoiceRptTODT["isInternalCnf"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("isInternalCnf"))
                            {
                                if (tblInvoiceRptTODT["isInternalCnf"] != DBNull.Value)
                                    tblInvoiceRptTONew.InternalCnf = Convert.ToInt32(tblInvoiceRptTODT["isInternalCnf"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("itemName"))
                            {
                                if (tblInvoiceRptTODT["itemName"] != DBNull.Value)
                                    tblInvoiceRptTONew.ItemName = Convert.ToString(tblInvoiceRptTODT["itemName"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("overdueTallyRefId"))
                            {
                                if (tblInvoiceRptTODT["overdueTallyRefId"] != DBNull.Value)
                                    tblInvoiceRptTONew.OverdueTallyRefId = Convert.ToString(tblInvoiceRptTODT["overdueTallyRefId"].ToString());
                            }


                        }

                        if(! String.IsNullOrEmpty(tblInvoiceRptTONew.OverdueTallyRefId))
                        {
                            tblInvoiceRptTONew.ProdItemDesc = tblInvoiceRptTONew.OverdueTallyRefId;
                        }
                                                       
                        tblInvoiceRPtTOList.Add(tblInvoiceRptTONew);

                    }
                }
                // return tblInvoiceTOList;
                return tblInvoiceRPtTOList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        /// <summary>
        /// Vijaymala[06-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> SelectInvoiceExportList(DateTime frmDt, DateTime toDt, int isConfirm, string strOrgTempId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                    " select distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.statusDate,invoice.invoiceDate,invoice.invoiceNo,invAddrBill.txnAddrTypeId as billingTypeId," +
                    " invAddrBill.billingName as buyer,invAddrBill.gstinNo as buyerGstNo,invAddrBill.stateId,invAddrCons.txnAddrTypeId as consigneeTypeId," +
                    " invAddrCons.billingName as consignee,invAddrCons.gstinNo as consigneeGstNo,invoice.deliveryLocation ," +
                    " invoiceItem.invoiceQty ,basicAmt,discountAmt as cdAmt,isConfirmed,invoice.statusId,invoice.invFromOrgId, invoice.tdsAmt, " +
                    " taxableAmt,cgstAmt,sgstAmt,igstAmt,grandTotal,vehicleNo,createdOn,freightItem.freightAmt,dimState.stateOrUTCode from  " +

                    " (select  invoiceModeId,invoiceDate, idInvoice, statusDate, invoiceNo, deliveryLocation,discountAmt ,   " +
                    " cgstAmt, sgstAmt, igstAmt, grandTotal, vehicleNo,createdOn ,isConfirmed,statusId,invFromOrgId from tempInvoice invoice  " +
                    " INNER JOIN tempInvoiceAddress invoiceAdd on invoice.idInvoice = invoiceAdd.invoiceId)invoice  " +
                    " INNER JOIN(select invoiceId, billingName, txnAddrTypeId, gstinNo,stateId from tempInvoiceAddress " +

                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill  " +
                    "inner join (select idState ,stateOrUTCode from dimState)dimState on invAddrBill.stateId = dimState.idState" +
                    " on invAddrBill.invoiceId = invoice.idInvoice  " +
                    " INNER JOIN(select invoiceId, billingName, txnAddrTypeId, gstinNo from tempInvoiceAddress " +
                    " where txnAddrTypeId=" + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons  " +
                    " on invAddrCons.invoiceId = invoice.idInvoice  " +
                    " INNER JOIN(select invoiceId, sum(invoiceQty)as invoiceQty,sum(basicTotal)as basicAmt, sum(taxableAmt)as taxableAmt from tempInvoiceItemDetails  where otherTaxId is null group by invoiceId)invoiceItem   " +
                    " on invoiceItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN (select invoiceId, taxableAmt as freightAmt from tempInvoiceItemDetails where otherTaxId =  2  )freightItem" +
                    " On freightItem.invoiceId = invoice.idInvoice " +

                // Vaibhav [20-Nov-2017] Added to select from  finalInvoice and finalInvoiceAddress and finalInvoiceItemDetails
                " UNION ALL " +
                " select distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice, invoice.statusDate,invoice.invoiceDate,invoice.invoiceNo,invAddrBill.txnAddrTypeId as billingTypeId," +
                " invAddrBill.billingName as buyer,invAddrBill.gstinNo as buyerGstNo,invAddrBill.stateId,invAddrCons.txnAddrTypeId as consigneeTypeId," +
                " invAddrCons.billingName as consignee,invAddrCons.gstinNo as consigneeGstNo,invoice.deliveryLocation ," +
                " invoiceItem.invoiceQty ,basicAmt,discountAmt as cdAmt,isConfirmed,invoice.statusId,invoice.invFromOrgId, invoice.tdsAmt, " +
                " taxableAmt,cgstAmt,sgstAmt,igstAmt,grandTotal,vehicleNo,createdOn,freightItem.freightAmt,dimState.stateOrUTCode from  " +

                " (select  invoiceModeId,invoiceDate, idInvoice, statusDate, invoiceNo, deliveryLocation,discountAmt ,   " +
                " cgstAmt, sgstAmt, igstAmt, grandTotal, vehicleNo,createdOn ,isConfirmed,statusId ,invFromOrgId from finalInvoice invoice  " +
                " INNER JOIN finalInvoiceAddress invoiceAdd on invoice.idInvoice = invoiceAdd.invoiceId)invoice  " +
                " INNER JOIN(select invoiceId, billingName, txnAddrTypeId, gstinNo,stateId from finalInvoiceAddress " +
                " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill  " +
                "inner join (select idState ,stateOrUTCode from dimState)dimState on invAddrBill.stateId = dimState.idState" +
                " on invAddrBill.invoiceId = invoice.idInvoice  " +
                " INNER JOIN(select invoiceId, billingName, txnAddrTypeId, gstinNo from finalInvoiceAddress " +
                " where txnAddrTypeId=" + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons  " +
                " on invAddrCons.invoiceId = invoice.idInvoice  " +
                " INNER JOIN(select invoiceId, sum(invoiceQty)as invoiceQty,sum(basicTotal)as basicAmt, sum(taxableAmt)as taxableAmt from finalInvoiceItemDetails  where otherTaxId is null group by invoiceId)invoiceItem   " +
                " on invoiceItem.invoiceId = invoice.idInvoice " +
                " LEFT JOIN (select invoiceId, taxableAmt as freightAmt from finalInvoiceItemDetails where otherTaxId =  2  )freightItem" +
                " On freightItem.invoiceId = invoice.idInvoice ";

                cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                 " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED +
                 " AND CAST(sq1.statusDate AS DATE) BETWEEN @fromDate AND @toDate" +
                 " AND isnull(sq1.invFromOrgId,0) in(" + strOrgTempId + ")"+
                 " order by sq1.invoiceNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);


                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Vijaymala[07-10-2017] Added To Get Invoice List To Generate HSN Excel
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> SelectHsnExportList(DateTime frmDt, DateTime toDt, int isConfirm,string strOrgTempId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                 " select   distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.statusDate, invoice.tdsAmt ,invoice.invoiceDate,invoice.invoiceNo,invAddrBill.txnAddrTypeId as billTypeId,invAddrBill.billingName as buyer, " +
                 " invAddrBill.gstinNo as buyerGstNo,invAddrCons.txnAddrTypeId as consigneeTypeId,invAddrCons.billingName as consignee, " +
                 " invAddrCons.gstinNo as consigneeGstNo,invoiceItem.gstinCodeNo,invoiceItem.prodItemDesc,invoiceItem.invoiceQty,invoiceItem.rate,invoiceItem.basicTotal as basicAmt," +
                 " invoiceItem.taxableAmt,invoiceItem.idInvoiceItem as invoiceItemId,invoiceTax.taxTypeId,invoiceTax.taxAmt,invoiceItem.grandTotal,invoice.createdOn,invoice.vehicleNo,invoice.invFromOrgId," +
                 " freightItem.freightAmt,invoice.statusId,dimState.stateOrUTCode,invoiceItem.otherTaxId,isConfirmed from " +
                 " (select  invoiceModeId,invoiceDate, idInvoice, invoiceNo,vehicleNo , invoice.statusDate  , createdOn,isConfirmed,statusId,invFromOrgId from tempInvoice invoice " +
                 " INNER JOIN tempInvoiceAddress invoiceAdd On invoice.idInvoice = invoiceAdd.invoiceId)invoice " +
                 " INNER JOIN(select invoiceId, billingName, txnAddrTypeId, gstinNo,stateId from tempInvoiceAddress where txnAddrTypeId  = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + " )invAddrBill " +
                 " inner join (select idState ,stateOrUTCode from dimState)dimState on invAddrBill.stateId = dimState.idState"+
                 " On invAddrBill.invoiceId = invoice.idInvoice " +
                 " INNER JOIN(select invoiceId, billingName, txnAddrTypeId, gstinNo from tempInvoiceAddress where txnAddrTypeId= " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                 " On invAddrCons.invoiceId = invoice.idInvoice " +
                 " INNER JOIN(select idInvoiceItem, invoiceId, gstinCodeNo, prodItemDesc, invoiceQty, rate, basicTotal, taxableAmt, grandTotal,otherTaxId " +
                 " from tempInvoiceItemDetails )invoiceItem  On invoiceItem.invoiceId = invoice.idInvoice " +
                 " INNER JOIN(select itemTaxDetails.idInvItemTaxDtl, itemTaxDetails.invoiceItemId, itemTaxDetails.taxAmt, taxRate.taxTypeId from tempInvoiceItemTaxDtls itemTaxDetails " +
                 " INNER JOIN tblTaxRates taxRate ON taxRate.idTaxRate = itemTaxDetails.taxRateId)as invoiceTax " +
                 " On invoiceTax.invoiceItemId = invoiceItem.idInvoiceItem " +
                 "LEFT JOIN (select invoiceId,taxAmt as freightAmt from tempInvoiceItemDetails where otherTaxId =  2 )freightItem" +
                 " On freightItem.invoiceId = invoice.idInvoice " +
                //" where DAY(invoice.invoiceDate) = " + sysDate.Day + " AND MONTH(invoice.invoiceDate) = " + sysDate.Month + " AND YEAR(invoice.invoiceDate) = " + sysDate.Year;

                // Vaibhav [20-Nov-2017] Added to select from finalInvoice and finalInvoiceAddress and finalInvoiceItemDetails and finalInvoiceItemTaxDtls

                " UNION ALL " +
                " select   distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.statusDate , invoice.tdsAmt,invoice.invoiceDate,invoice.invoiceNo,invoice.invFromOrgId,invAddrBill.txnAddrTypeId as billTypeId,invAddrBill.billingName as buyer, " +
               " invAddrBill.gstinNo as buyerGstNo,invAddrCons.txnAddrTypeId as consigneeTypeId,invAddrCons.billingName as consignee, " +
               " invAddrCons.gstinNo as consigneeGstNo,invoiceItem.gstinCodeNo,invoiceItem.prodItemDesc,invoiceItem.invoiceQty,invoiceItem.rate,invoiceItem.basicTotal as basicAmt," +
               " invoiceItem.taxableAmt,invoiceItem.idInvoiceItem as invoiceItemId,invoiceTax.taxTypeId,invoiceTax.taxAmt,invoiceItem.grandTotal,invoice.createdOn,invoice.vehicleNo," +
               " freightItem.freightAmt,invoice.statusId,dimState.stateOrUTCode,invoiceItem.otherTaxId,isConfirmed from " +

               " (select  invoiceModeId,invoiceDate, idInvoice, statusDate,invoiceNo,vehicleNo , createdOn,isConfirmed,statusId,invFromOrgId from finalInvoice invoice " +
               " INNER JOIN finalInvoiceAddress invoiceAdd On invoice.idInvoice = invoiceAdd.invoiceId)invoice " +
               " INNER JOIN(select invoiceId, billingName, txnAddrTypeId, gstinNo,stateId from finalInvoiceAddress where txnAddrTypeId  = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + " )invAddrBill " +
               " inner join (select idState ,stateOrUTCode from dimState)dimState on invAddrBill.stateId = dimState.idState" +
               " On invAddrBill.invoiceId = invoice.idInvoice " +
               " INNER JOIN(select invoiceId, billingName, txnAddrTypeId, gstinNo from finalInvoiceAddress where txnAddrTypeId= " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
               " On invAddrCons.invoiceId = invoice.idInvoice " +
               " INNER JOIN(select idInvoiceItem, invoiceId, gstinCodeNo, prodItemDesc, invoiceQty, rate, basicTotal, taxableAmt, grandTotal,otherTaxId " +
               " from finalInvoiceItemDetails )invoiceItem  On invoiceItem.invoiceId = invoice.idInvoice " +
               " INNER JOIN(select itemTaxDetails.idInvItemTaxDtl, itemTaxDetails.invoiceItemId, itemTaxDetails.taxAmt, taxRate.taxTypeId from finalInvoiceItemTaxDtls itemTaxDetails " +
               " INNER JOIN tblTaxRates taxRate ON taxRate.idTaxRate = itemTaxDetails.taxRateId)as invoiceTax " +
               " On invoiceTax.invoiceItemId = invoiceItem.idInvoiceItem " +
               "LEFT JOIN (select invoiceId,taxAmt as freightAmt from finalInvoiceItemDetails where otherTaxId =  2 )freightItem" +
               " On freightItem.invoiceId = invoice.idInvoice ";

                cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED +
                     " AND isnull(sq1.invFromOrgId,0) in("+ strOrgTempId + ")"+
                     " AND CAST(sq1.statusDate AS DATE) BETWEEN @fromDate AND @toDate" +
                     " order by sq1.invoiceNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
                {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        /// <summary>
        /// Vijaymala[11-01-2018] Added To Get Sales Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> SelectSalesInvoiceListForReport(DateTime frmDt, DateTime toDt, int isConfirm,string selectedOrg,int defualtOrg, int isManual, int isFromPurchase=0)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                    " Select distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.invoiceNo,invoice.electronicRefNo,invoice.IrnNo,invoice.dealerOrgId, " +
                    " tblGstCodeDtls.codeNumber, invoice.tareWeight,invoice.grossWeight,invoice.netWeight, invoice.roundOffAmt,invoice.narration, invoice.tdsAmt, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,mat.materialSubType " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.taxableAmt " +
                    " as taxableAmt  ,freightItem.freightAmt,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,   " +
                    " invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId, " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo , " +
                    " invoice.grandTotal, invoice.isConfirmed , invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , " +
                    " invoice.lrDate , invoice.lrNumber,invoice.deliveredOn, dimInvoiceTypes.invoiceTypeDesc, LEFT(weighingMeasures.machineName,3) AS 'godown', ISNULL(itemTallyRefDtls.overdueTallyRefId, '') overdueTallyRefId FROM tempInvoice invoice " +

                    " LEFT JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                    " invAddrB.gstinNo, invAddrB.state as stateName from tempInvoiceAddress invAddrB " +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo, invAddrC.state as stateName " +
                    " from tempInvoiceAddress invAddrC   " +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN tempLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                    " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +
                    " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN  tblGstCodeDtls tblGstCodeDtls on prodGstCodeDtl.gstCodeId = tblGstCodeDtls.idGstCode" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                    " LEFT JOIN tblItemTallyRefDtls itemTallyRefDtls on ISNULL(itemTallyRefDtls.prodCatId, 0) = ISNULL(prodGstCodeDtl.prodCatId, 0) AND ISNULL(itemTallyRefDtls.prodSpecId, 0) = ISNULL(prodGstCodeDtl.prodSpecId, 0) AND ISNULL(itemTallyRefDtls.materialId, 0) = ISNULL(prodGstCodeDtl.materialId, 0) AND ISNULL(itemTallyRefDtls.prodItemId, 0) = ISNULL(prodGstCodeDtl.prodItemId, 0) " +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                     " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 5  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN dimInvoiceTypes dimInvoiceTypes on dimInvoiceTypes.idInvoiceType = invoice.invoiceTypeId  " +
                    " LEFT JOIN tempLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN tempLoading loading on loading.idLoading = loadingSlip.loadingId  " +
                    " LEFT JOIN (" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tblWeighingMachine.machineName,tempWeighingMeasures.loadingId" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tempWeighingMeasures.weighingMachineId" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 1 ) AS weighingMeasures " +
                    " ON weighingMeasures.loadingId = loading.idLoading and weighingMeasures.row_number = 1" +


                    // Vaibhav [17-Jan-2018] To select from final tables.
                    " UNION ALL " +

                    " Select distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.invoiceNo,invoice.electronicRefNo,invoice.IrnNo,invoice.dealerOrgId,tblGstCodeDtls.codeNumber," +
                    " invoice.tareWeight,invoice.grossWeight,invoice.netWeight,invoice.roundOffAmt, invoice.narration, invoice.tdsAmt, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,mat.materialSubType " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.taxableAmt " +
                    " as taxableAmt  ,freightItem.freightAmt,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,  " +
                    " invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId,  " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo, " +
                    " invoice.grandTotal, invoice.isConfirmed ,invoice.statusId, invoice.invFromOrgId , " +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , invoice.lrDate , invoice.lrNumber" +
                    " ,invoice.deliveredOn,dimInvoiceTypes.invoiceTypeDesc, LEFT(weighingMeasures.machineName,3) AS 'godown', ISNULL(itemTallyRefDtls.overdueTallyRefId, '') overdueTallyRefId FROM finalInvoice invoice " +
                    
                    " LEFT JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                    " invAddrB.gstinNo, invAddrB.state as stateName from finalInvoiceAddress invAddrB " +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo,invAddrC.state as stateName " +
                    " from finalInvoiceAddress invAddrC   " +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN finalLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                    " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +
                    " LEFT JOIN tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN  tblGstCodeDtls tblGstCodeDtls on prodGstCodeDtl.gstCodeId = tblGstCodeDtls.idGstCode" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                    " LEFT JOIN tblItemTallyRefDtls itemTallyRefDtls on ISNULL(itemTallyRefDtls.prodCatId, 0) = ISNULL(prodGstCodeDtl.prodCatId, 0) AND ISNULL(itemTallyRefDtls.prodSpecId, 0) = ISNULL(prodGstCodeDtl.prodSpecId, 0) AND ISNULL(itemTallyRefDtls.materialId, 0) = ISNULL(prodGstCodeDtl.materialId, 0) AND ISNULL(itemTallyRefDtls.prodItemId, 0) = ISNULL(prodGstCodeDtl.prodItemId, 0) " +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                     " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 5  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN dimInvoiceTypes dimInvoiceTypes on dimInvoiceTypes.idInvoiceType = invoice.invoiceTypeId " +
                    " LEFT JOIN finalLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN finalLoading loading on loading.idLoading = loadingSlip.loadingId  " +
                    " LEFT JOIN (" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY finalWeighingMeasures.loadingId ORDER BY finalWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                    " finalWeighingMeasures.idWeightMeasure,tblWeighingMachine.machineName,finalWeighingMeasures.loadingId" +
                    " FROM finalWeighingMeasures finalWeighingMeasures" +
                    " LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = finalWeighingMeasures.weighingMachineId" +
                    " WHERE finalWeighingMeasures.weightMeasurTypeId = 1 ) AS weighingMeasures" +
                    " ON weighingMeasures.loadingId = loading.idLoading and weighingMeasures.row_number = 1";




                String strWhere = String.Empty;
                //if (selectedOrg > 0)
                //{
                //    if(defualtOrg == selectedOrg)
                //    {

                //        strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ",0)";
                //    }
                //    else
                //        strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ")";
                //}
                if(isFromPurchase == 1)
                {
                    strWhere = " AND sq1.dealerOrgId  IN (" + selectedOrg + ")";
                }
                else
                {
                    strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ")";
                }
                if(isManual == 1)
                {
                    strWhere += " AND sq1.mode = 'Yes'";
                }
                else {
                    strWhere += " AND sq1.mode = 'No'";
                }
                cmdSelect.CommandText = " SELECT * FROM ("+ selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     " AND CAST(sq1.deliveredOn AS DATE) BETWEEN @fromDate AND @toDate" +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED + strWhere + " order by sq1.invoiceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //Deepali Added

        public static List<TblInvoiceRptTO> SelectSalesPurchaseListForReport(DateTime frmDt, DateTime toDt, int isConfirm, string selectedOrg, int defualtOrg, int isFromPurchase = 0)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                    " Select distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.invoiceNo,invoice.electronicRefNo,invoice.IrnNo,invoice.dealerOrgId, " +
                    " tblGstCodeDtls.codeNumber, invoice.tareWeight,invoice.grossWeight,invoice.netWeight, invoice.roundOffAmt,invoice.narration, invoice.tdsAmt, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.address as buyerAddress," +
                    " invAddrBill.district as buyerDistrict,invAddrBill.pinCode as buyerPinCode," +
                    " invAddrBill.taluka as buyerTaluka,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeTaluka,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,mat.materialSubType " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.taxableAmt " +
                    " as taxableAmt  ,freightItem.freightAmt,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,   " +
                    " (SELECT itemTax.taxAmt FROM [tempInvoiceItemTaxDtls] itemTax  LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate" +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 2) as cgstAmt," +
                    " (SELECT itemTax.taxAmt FROM[tempInvoiceItemTaxDtls] itemTax  LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 3) as sgstAmt, " +
                    " (SELECT itemTax.taxAmt FROM[tempInvoiceItemTaxDtls] itemTax  LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 1) as igstAmt ," +
                    " (SELECT itemTax.taxRatePct FROM[tempInvoiceItemTaxDtls] itemTax LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 1) as igstPct , " +
                    " (SELECT itemTax.taxRatePct FROM[tempInvoiceItemTaxDtls] itemTax LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 3) as sgstPct , " +
                    " (SELECT itemTax.taxRatePct FROM[tempInvoiceItemTaxDtls] itemTax LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 2) as cgstPct ," +
                    " itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId, " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo , " +
                    " invoice.grandTotal, invoice.isConfirmed , invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , " +
                    " invoice.lrDate , invoice.lrNumber,invoice.deliveredOn, dimInvoiceTypes.invoiceTypeDesc,invFromOrg.firmName As invFromOrgName,tblAddress.areaName as OrgareaName,tblAddress.pincode as Orgpincode " +
                    " ,tblAddress.plotNo as OrgplotNo,tblAddress.villageName as OrgvillageName ,dimDistrict.districtName as OrgdistrictName," +
                    " dimState.stateName as OrgstateName,dimCountry.countryName As OrgcountryName FROM tempInvoice invoice " +

                    " LEFT JOIN(select invAddrB.invoiceId,invAddrB.address,invAddrB.district,invAddrB.pinCode,invAddrB.taluka, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                    " invAddrB.gstinNo, invAddrB.state as stateName from tempInvoiceAddress invAddrB " +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.taluka as consigneeTaluka,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo, invAddrC.state as stateName " +
                    " from tempInvoiceAddress invAddrC   " +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization invFromOrg  ON invFromOrg.idOrganization = invoice.invFromOrgId " +
                    " LEFT JOIN tblOrgAddress tblOrgAddress  ON tblOrgAddress.organizationId = invoice.invFromOrgId " +
                    " and tblOrgAddress.addrTypeId = 5 " +
                    " LEFT JOIN tblAddress tblAddress  ON tblAddress.idAddr = tblOrgAddress.addressId " +
                    " LEFT JOIN dimDistrict dimDistrict  ON dimDistrict.idDistrict = tblAddress.districtId " +
                    " LEFT JOIN dimState dimState  ON dimState.idState = tblAddress.stateId " +
                    " LEFT JOIN dimCountry dimCountry  ON dimCountry.idCountry = tblAddress.countryId  " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN tempLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                    " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +
                    " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN  tblGstCodeDtls tblGstCodeDtls on prodGstCodeDtl.gstCodeId = tblGstCodeDtls.idGstCode" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                     " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 5  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tempLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN tempLoading loading on loading.idLoading = loadingSlip.loadingId  " +
                    " LEFT JOIN dimInvoiceTypes dimInvoiceTypes on dimInvoiceTypes.idInvoiceType = invoice.invoiceTypeId  " +

                    // Vaibhav [17-Jan-2018] To select from final tables.
                    " UNION ALL " +

                    " Select distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.invoiceNo,invoice.electronicRefNo,invoice.IrnNo,invoice.dealerOrgId,tblGstCodeDtls.codeNumber," +
                    " invoice.tareWeight,invoice.grossWeight,invoice.netWeight,invoice.roundOffAmt, invoice.narration, invoice.tdsAmt, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.address as buyerAddress," +
                    " invAddrBill.district as buyerDistrict,invAddrBill.pinCode as buyerPinCode," +
                    " invAddrBill.taluka as buyerTaluka,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeTaluka,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,mat.materialSubType " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.taxableAmt " +
                    " as taxableAmt  ,freightItem.freightAmt,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,  " +
                    " (SELECT itemTax.taxAmt FROM [finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 2) as cgstAmt," +
                    " (SELECT itemTax.taxAmt FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 3) as sgstAmt," +
                    " (SELECT itemTax.taxAmt FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 1) as igstAmt " +
                    "  ,(SELECT itemTax.taxRatePct FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate  where itemTax.invoiceItemId = itemDetails.idInvoiceItem " +
                    " AND taxRate.taxTypeId = 1) as igstPct , " +
                    " (SELECT itemTax.taxRatePct FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate  where itemTax.invoiceItemId = itemDetails.idInvoiceItem " +
                    " AND taxRate.taxTypeId = 3) as sgstPct ," +
                    " (SELECT itemTax.taxRatePct FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate  where itemTax.invoiceItemId = itemDetails.idInvoiceItem " +
                    " AND taxRate.taxTypeId = 2) as cgstPct ," +
                    " itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId,  " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo, " +
                    " invoice.grandTotal, invoice.isConfirmed ,invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , invoice.lrDate , invoice.lrNumber" +
                    " ,invoice.deliveredOn,dimInvoiceTypes.invoiceTypeDesc,invFromOrg.firmName As invFromOrgName,tblAddress.areaName as OrgareaName, " +
                    " tblAddress.pincode as Orgpincode ,tblAddress.plotNo as OrgplotNo,tblAddress.villageName as OrgvillageName," +
                    " dimDistrict.districtName as OrgdistrictName,dimState.stateName as OrgstateName,dimCountry.countryName As OrgcountryName FROM finalInvoice invoice " +

                    " LEFT JOIN(select invAddrB.invoiceId,invAddrB.address,invAddrB.district,invAddrB.pinCode,invAddrB.taluka, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                    " invAddrB.gstinNo, invAddrB.state as stateName from finalInvoiceAddress invAddrB " +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.taluka as consigneeTaluka,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo,invAddrC.state as stateName " +
                    " from finalInvoiceAddress invAddrC   " +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization invFromOrg  ON invFromOrg.idOrganization = invoice.invFromOrgId " +
                    " LEFT JOIN tblOrgAddress tblOrgAddress  ON tblOrgAddress.organizationId = invoice.invFromOrgId " +
                    " and tblOrgAddress.addrTypeId = 5 " +
                    " LEFT JOIN tblAddress tblAddress  ON tblAddress.idAddr = tblOrgAddress.addressId " +
                    " LEFT JOIN dimDistrict dimDistrict  ON dimDistrict.idDistrict = tblAddress.districtId " +
                    " LEFT JOIN dimState dimState  ON dimState.idState = tblAddress.stateId " +
                    " LEFT JOIN dimCountry dimCountry  ON dimCountry.idCountry = tblAddress.countryId  " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN finalLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                    " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +
                    " LEFT JOIN tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN  tblGstCodeDtls tblGstCodeDtls on prodGstCodeDtl.gstCodeId = tblGstCodeDtls.idGstCode" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +

                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                     " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 5  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN finalLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN finalLoading loading on loading.idLoading = loadingSlip.loadingId  " +
                    "LEFT JOIN dimInvoiceTypes dimInvoiceTypes on dimInvoiceTypes.idInvoiceType = invoice.invoiceTypeId ";


                String strWhere = String.Empty;
                //if (selectedOrg > 0)
                //{
                //    if(defualtOrg == selectedOrg)
                //    {

                //        strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ",0)";
                //    }
                //    else
                //        strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ")";
                //}
                if (isFromPurchase == 1)
                {
                    strWhere = " AND sq1.dealerOrgId  IN (" + selectedOrg + ")";
                }
                else
                {
                    strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ")";
                }
                cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     " AND CAST(sq1.invoiceDate AS DATE) BETWEEN @fromDate AND @toDate" +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED + strWhere + " order by sq1.invoiceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<TblInvoiceTO> SelectAllTempInvoice(Int32 loadingSlipId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId = " + loadingSlipId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblInvoiceTO> SelectAllTempInvoice(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId = " + loadingSlipId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Vijaymala added [09-05-2018]:To get notified invoices list
        /// </summary>
        /// <param name="tblInvoiceTO"></param>
        /// <returns></returns>
        public static List<TblInvoiceTO> SelectAllTNotifiedblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm,string strOrgTempId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                String whereCondition = " Where sq1.invoiceModeId = " + (int)Constants.InvoiceModeE.AUTO_INVOICE_EDIT +
                    " AND CAST(sq1.statusDate AS Date) BETWEEN @fromDate AND @toDate";

                whereCondition += " AND ISNULL(sq1.isConfirmed,0) = " + isConfirm;
                if((!string.IsNullOrEmpty(strOrgTempId)) && isConfirm==1)
                {
                    whereCondition += " AND ISNULL(sq1.invFromOrgId,0) in( " + strOrgTempId+")";
                }
                conn.Open();
                cmdSelect.CommandText = "Select * From (" + SqlSelectQuery() + ")sq1 " + whereCondition;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public static int InsertTblInvoice(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceTO, cmdInsert);
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

        public static int InsertTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblInvoiceTO tblInvoiceTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempInvoice]( " +
                                "  [invoiceTypeId]" +
                                " ,[transportOrgId]" +
                                " ,[transportModeId]" +
                                " ,[currencyId]" +
                                " ,[loadingSlipId]" +
                                " ,[distributorOrgId]" +
                                " ,[dealerOrgId]" +
                                " ,[finYearId]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[invoiceDate]" +
                                " ,[lrDate]" +
                                " ,[statusDate]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[currencyRate]" +
                                " ,[basicAmt]" +
                                " ,[discountAmt]" +
                                " ,[taxableAmt]" +
                                " ,[cgstAmt]" +
                                " ,[sgstAmt]" +
                                " ,[igstAmt]" +
                                " ,[freightPct]" +
                                " ,[freightAmt]" +
                                " ,[roundOffAmt]" +
                                " ,[grandTotal]" +
                                " ,[invoiceNo]" +
                                " ,[electronicRefNo]" +
                                " ,[vehicleNo]" +
                                " ,[lrNumber]" +
                                " ,[roadPermitNo]" +
                                " ,[transportorForm]" +
                                " ,[airwayBillNo]" +
                                " ,[narration]" +
                                " ,[bankDetails]" +
                                " ,[invoiceModeId]" +
                                " ,[deliveryLocation]" +
                                  " ,[tareWeight]" +
                                " ,[netWeight]" +
                                " ,[grossWeight]" +
                                 " ,[changeIn]" +
                                " ,[expenseAmt]" +
                                " ,[otherAmt]" +
                                " ,[isConfirmed]" +
                                " ,[rcmFlag]" +
                                " ,[remark]" +
                                " ,[invFromOrgId]" +
                                " ,[deliveredOn]"+
                                " ,[invFromOrgFreeze]" +
                                " ,[tdsAmt]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceTypeId " +
                                " ,@TransportOrgId " +
                                " ,@TransportModeId " +
                                " ,@CurrencyId " +
                                " ,@LoadingSlipId " +
                                " ,@DistributorOrgId " +
                                " ,@DealerOrgId " +
                                " ,@FinYearId " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@InvoiceDate " +
                                " ,@LrDate " +
                                " ,@StatusDate " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@CurrencyRate " +
                                " ,@BasicAmt " +
                                " ,@DiscountAmt " +
                                " ,@TaxableAmt " +
                                " ,@CgstAmt " +
                                " ,@SgstAmt " +
                                " ,@IgstAmt " +
                                " ,@FreightPct " +
                                " ,@FreightAmt " +
                                " ,@RoundOffAmt " +
                                " ,@GrandTotal " +
                                " ,@InvoiceNo " +
                                " ,@ElectronicRefNo " +
                                " ,@VehicleNo " +
                                " ,@LrNumber " +
                                " ,@RoadPermitNo " +
                                " ,@TransportorForm " +
                                " ,@AirwayBillNo " +
                                " ,@Narration " +
                                " ,@BankDetails " +
                                " ,@invoiceModeId " +
                                " ,@deliveryLocation " +
                                 " ,@tareWeight " +
                                " ,@netWeight " +
                                " ,@grossWeight " +
                                 " ,@changeIn " +
                                " ,@expenseAmt " +
                                " ,@otherAmt " +
                                " ,@isConfirmed " +
                                " ,@rcmFlag " +
                                " ,@remark " +
                                " ,@invFromOrgId " +
                                " ,@deliveredOn " +
                                " ,@InvFromOrgFreeze" +
                                " ,@tdsAmt" +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
            cmdInsert.Parameters.Add("@InvoiceTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvoiceTypeId;
            cmdInsert.Parameters.Add("@TransportOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportOrgId);
            cmdInsert.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportModeId);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.CurrencyId;
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LoadingSlipId);
            cmdInsert.Parameters.Add("@DistributorOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DistributorOrgId);
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DealerOrgId);
            cmdInsert.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.FinYearId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.UpdatedBy);
            cmdInsert.Parameters.Add("@InvoiceDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.InvoiceDate;
            cmdInsert.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrDate);
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.UpdatedOn);
            cmdInsert.Parameters.Add("@CurrencyRate", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CurrencyRate;
            cmdInsert.Parameters.Add("@BasicAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.BasicAmt;
            cmdInsert.Parameters.Add("@DiscountAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.DiscountAmt;
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.TaxableAmt;
            cmdInsert.Parameters.Add("@CgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CgstAmt;
            cmdInsert.Parameters.Add("@SgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SgstAmt;
            cmdInsert.Parameters.Add("@IgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IgstAmt;
            cmdInsert.Parameters.Add("@FreightPct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightPct;
            cmdInsert.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightAmt;
            cmdInsert.Parameters.Add("@RoundOffAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.RoundOffAmt;
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.GrandTotal;
            cmdInsert.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceNo);
            cmdInsert.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.VehicleNo);
            cmdInsert.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
            cmdInsert.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
            cmdInsert.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
            cmdInsert.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
            cmdInsert.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
            cmdInsert.Parameters.Add("@invoiceModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceModeId);
            cmdInsert.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);
            cmdInsert.Parameters.Add("@tareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TareWeight);
            cmdInsert.Parameters.Add("@netWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.NetWeight);
            cmdInsert.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.GrossWeight);
            cmdInsert.Parameters.Add("@changeIn", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ChangeIn);
            cmdInsert.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.ExpenseAmt;
            cmdInsert.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.OtherAmt;
            cmdInsert.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsConfirmed;
            cmdInsert.Parameters.Add("@rcmFlag", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RcmFlag);
            cmdInsert.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Remark);
            cmdInsert.Parameters.Add("@invFromOrgId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgId;
            cmdInsert.Parameters.Add("@deliveredOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveredOn);
            cmdInsert.Parameters.Add("@InvFromOrgFreeze", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgFreeze;
            cmdInsert.Parameters.Add("@tdsAmt", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.TdsAmt;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceTO.IdInvoice = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblInvoiceTO, cmdUpdate);
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

        public static int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblInvoiceTO, cmdUpdate);
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

        public static int UpdateTblInvoiceFinalTareWt(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [finalInvoice] SET " +
                                " [tareWeight] = @tareWeight " +
                                " ,[grossWeight] = @grossWeight " +
                                " WHERE [loadingSlipId] = @loadingSlipId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@tareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TareWeight);
                cmdUpdate.Parameters.Add("@loadingSlipId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LoadingSlipId);
                cmdUpdate.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.GrossWeight);


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


        public static int UpdateInvoiceNonCommercDtls(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +
                            "  [transportModeId]= @TransportModeId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[lrDate]= @LrDate" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[electronicRefNo]= @ElectronicRefNo" +
                            " ,[lrNumber]= @LrNumber" +
                            " ,[roadPermitNo]= @RoadPermitNo" +
                            " ,[transportorForm]= @TransportorForm" +
                            " ,[airwayBillNo]= @AirwayBillNo" +
                            " ,[narration]= @Narration" +
                            " ,[bankDetails] = @BankDetails" +
                            " ,[deliveryLocation] = @deliveryLocation " +
                            " ,[invFromOrgFreeze]=@InvFromOrgFreeze " +
                            " WHERE [idInvoice] = @IdInvoice";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportModeId);
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrDate);
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
                cmdUpdate.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
                cmdUpdate.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
                cmdUpdate.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
                cmdUpdate.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
                cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
                cmdUpdate.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
                cmdUpdate.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);
                cmdUpdate.Parameters.Add("@InvFromOrgFreeze", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgFreeze;

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

        public static int ExecuteUpdationCommand(TblInvoiceTO tblInvoiceTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempInvoice] SET " +
                            "  [invoiceTypeId]= @InvoiceTypeId" +
                            " ,[transportOrgId]= @TransportOrgId" +
                            " ,[transportModeId]= @TransportModeId" +
                            " ,[currencyId]= @CurrencyId" +
                            " ,[loadingSlipId]= @LoadingSlipId" +
                            " ,[distributorOrgId]= @DistributorOrgId" +
                            " ,[dealerOrgId]= @DealerOrgId" +
                            " ,[finYearId]= @FinYearId" +
                            " ,[statusId]= @StatusId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[invoiceDate]= @InvoiceDate" +
                            " ,[lrDate]= @LrDate" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[currencyRate]= @CurrencyRate" +
                            " ,[basicAmt]= @BasicAmt" +
                            " ,[discountAmt]= @DiscountAmt" +
                            " ,[taxableAmt]= @TaxableAmt" +
                            " ,[cgstAmt]= @CgstAmt" +
                            " ,[sgstAmt]= @SgstAmt" +
                            " ,[igstAmt]= @IgstAmt" +
                            " ,[freightPct]= @FreightPct" +
                            " ,[freightAmt]= @FreightAmt" +
                            " ,[roundOffAmt]= @RoundOffAmt" +
                            " ,[grandTotal]= @GrandTotal" +
                            " ,[invoiceNo]= @InvoiceNo" +
                            " ,[electronicRefNo]= @ElectronicRefNo" +
                            " ,[vehicleNo]= @VehicleNo" +
                            " ,[lrNumber]= @LrNumber" +
                            " ,[roadPermitNo]= @RoadPermitNo" +
                            " ,[transportorForm]= @TransportorForm" +
                            " ,[airwayBillNo]= @AirwayBillNo" +
                            " ,[narration]= @Narration" +
                            " ,[bankDetails] = @BankDetails" +
                            " ,[invoiceModeId] = @invoiceModeId " +
                            " ,[deliveryLocation] = @deliveryLocation " +
                             " ,[tareWeight] = @tareWeight " +
                            " ,[netWeight] = @netWeight " +
                            " ,[grossWeight] = @grossWeight " +
                             " ,[changeIn] = @changeIn " +
                            " ,[expenseAmt] = @expenseAmt " +
                            " ,[otherAmt] = @otherAmt " +
                            " ,[isConfirmed] = @isConfirmed " +
                            " ,[rcmFlag] = @rcmFlag " +
                            " ,[remark] = @remark " +
                            " ,[invFromOrgId] = @invFromOrgId " +
                            " ,[deliveredOn]=@deliveredOn " +
                            " ,[invFromOrgFreeze]=@InvFromOrgFreeze " +
                            " ,[tdsAmt]=@TdsAmt " +
                            " WHERE [idInvoice] = @IdInvoice";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
            cmdUpdate.Parameters.Add("@InvoiceTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvoiceTypeId;
            cmdUpdate.Parameters.Add("@TransportOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportOrgId);
            cmdUpdate.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportModeId);
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.CurrencyId;
            cmdUpdate.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LoadingSlipId);
            cmdUpdate.Parameters.Add("@DistributorOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DistributorOrgId);
            cmdUpdate.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DealerOrgId);
            cmdUpdate.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.FinYearId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.StatusId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@InvoiceDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.InvoiceDate;
            cmdUpdate.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrDate);
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.StatusDate;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@CurrencyRate", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CurrencyRate;
            cmdUpdate.Parameters.Add("@BasicAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.BasicAmt;
            cmdUpdate.Parameters.Add("@DiscountAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.DiscountAmt;
            cmdUpdate.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.TaxableAmt;
            cmdUpdate.Parameters.Add("@CgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CgstAmt;
            cmdUpdate.Parameters.Add("@SgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SgstAmt;
            cmdUpdate.Parameters.Add("@IgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IgstAmt;
            cmdUpdate.Parameters.Add("@FreightPct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightPct;
            cmdUpdate.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightAmt;
            cmdUpdate.Parameters.Add("@RoundOffAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.RoundOffAmt;
            cmdUpdate.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.GrandTotal;
            cmdUpdate.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceNo);
            cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.VehicleNo);
            cmdUpdate.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
            cmdUpdate.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
            cmdUpdate.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
            cmdUpdate.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
            cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
            cmdUpdate.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
            cmdUpdate.Parameters.Add("@invoiceModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceModeId);
            cmdUpdate.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);
            cmdUpdate.Parameters.Add("@tareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TareWeight);
            cmdUpdate.Parameters.Add("@netWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.NetWeight);
            cmdUpdate.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.GrossWeight);
            cmdUpdate.Parameters.Add("@changeIn", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ChangeIn);
            cmdUpdate.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.ExpenseAmt;
            cmdUpdate.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.OtherAmt;
            cmdUpdate.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@rcmFlag", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RcmFlag);
            cmdUpdate.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Remark);
            cmdUpdate.Parameters.Add("@invFromOrgId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgId;
            cmdUpdate.Parameters.Add("@deliveredOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveredOn);
            cmdUpdate.Parameters.Add("@InvFromOrgFreeze", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgFreeze;
            cmdUpdate.Parameters.Add("@TdsAmt", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.TdsAmt;

            return cmdUpdate.ExecuteNonQuery();
        }
        public static int UpdateMappedSAPInvoiceNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = "UPDATE tempInvoice SET sapMappedSalesOrderNo = @SapMappedSalesOrderNo, sapMappedSalesInvoiceNo = @SapMappedSalesInvoiceNo WHERE idInvoice = @IdInvoice";
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@SapMappedSalesInvoiceNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SapMappedSalesInvoiceNo;
                cmdUpdate.Parameters.Add("@SapMappedSalesOrderNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SapMappedSalesOrderNo;
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

        public int UpdateEInvoicNo(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 "  [IrnNo] = @IrnNo " +
                 " ,[isEInvGenerated]= @IsEInvGenerated" +
                 " ,[distanceInKM]= @DistanceInKM" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@IrnNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IrnNo;
                cmdUpdate.Parameters.Add("@IsEInvGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEInvGenerated;
                cmdUpdate.Parameters.Add("@DistanceInKM", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.DistanceInKM;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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
        public static int PostUpdateInvoiceStatus(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                String sqlQuery = @" UPDATE [tempInvoice] SET " +
                 "  [invoiceModeId] = @InvoiceModeId " +
                 " ,[statusId]= @StatusId" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.StatusId;
                cmdUpdate.Parameters.Add("@InvoiceModeId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvoiceModeId;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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
        public static int UpdateEInvoicNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 "  [IrnNo] = @IrnNo " +
                 " ,[isEInvGenerated]= @IsEInvGenerated" +
                 " ,[distanceInKM]= @DistanceInKM" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@IrnNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IrnNo;
                cmdUpdate.Parameters.Add("@IsEInvGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEInvGenerated;
                cmdUpdate.Parameters.Add("@DistanceInKM", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.DistanceInKM;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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

        public int UpdateEWayBill(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 " [isEwayBillGenerated]= @IsEwayBillGenerated" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@IsEwayBillGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEWayBillGenerated;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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

        public static int UpdateEWayBill(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 " [isEwayBillGenerated]= @IsEwayBillGenerated" +
                 " ,[electronicRefNo]= @ElectronicRefNo" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@IsEwayBillGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEWayBillGenerated;
                cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.ElectronicRefNo;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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

        public static int UpdateTempInvoiceDistanceInKM(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 " [distanceInKM]= @DistanceInKM" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@DistanceInKM", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.DistanceInKM;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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
        #endregion

        #region Deletion
        public static int DeleteTblInvoice(Int32 idInvoice)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvoice, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvoice, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idInvoice, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoice] " +
            " WHERE idInvoice = " + idInvoice + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        #region Reports
        /// <summary>
        /// Priyanka [26-03-2018] : Added to get the other tax report in (TAX INVOICE).
        /// </summary>
        /// <param name="frmDt"></param>
        /// <param name="toDt"></param>
        /// <param name="isConfirm"></param>
        /// <param name="otherTaxId"></param>
        /// <returns></returns>

        public static List<TblOtherTaxRpt> SelectOtherTaxDetailsReport(DateTime frmDt, DateTime toDt, int isConfirm, Int32 otherTaxId,string strOrgTempId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();

                String whereCondition = "WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate ";

                whereCondition += " AND ISNULL(isConfirmed,0) = " + isConfirm;

                if (otherTaxId > 0)
                {
                    whereCondition += "AND otherTaxId = " + otherTaxId;
                }
                else
                {
                    whereCondition += "AND otherTaxId > 0 ";
                }
                if(!string.IsNullOrEmpty(strOrgTempId))
                {
                    whereCondition += "AND invFromOrgId in("+ strOrgTempId + ")";
                }

                selectQuery = " SELECT " +
                    " invoice.idInvoice " +
                    " ,invoice.invoiceNo " +
                    " ,invoice.vehicleNo " +
                    " ,invoice.netWeight " +
                    " ,invoice.vehicleNo " +
                    " ,invoice.invoiceDate " +
                    " ,Invoice.deliveryLocation " +
                    " ,invoice.isConfirmed " +
                    " ,invoice.statusId " +
                    " ,Invoice.statusDate " +
                    " ,InvoiceItemDetails.basicTotal " +
                    " ,InvoiceItemDetails.grandTotal " +
                    " ,dealer.firmName as partyName " +
                    " ,distributor.firmName AS cnfName " +
                    " ,transport.firmName AS transporterName " +
                    " ,InvoiceItemDetails.idInvoiceItem " +
                    " ,InvoiceItemDetails.prodItemDesc " +
                    " ,InvoiceItemDetails.taxableAmt " +
                    " ,InvoiceItemDetails.taxAmt " +
                    " ,InvoiceItemDetails.otherTaxId " +
                    " ,currencyName " +
                    " ,statusName " +
                    " ,invoiceTypeDesc " +
                    " FROM tempInvoiceItemDetails InvoiceItemDetails " +
                    " LEFT JOIN tempInvoice Invoice ON InvoiceItemDetails.invoiceId = Invoice.idInvoice " +
                    " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                    " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                    " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                    " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                    " " + whereCondition +
                    " UNION ALL " +
                    " SELECT " +
                     " invoice.idInvoice " +
                    " ,invoice.invoiceNo " +
                    " ,invoice.vehicleNo " +
                    " ,invoice.netWeight " +
                    " ,invoice.vehicleNo " +
                    " ,invoice.invoiceDate " +
                    " ,Invoice.deliveryLocation " +
                    " ,invoice.isConfirmed " +
                    " ,invoice.statusId " +
                    " ,Invoice.statusDate " +
                    " ,InvoiceItemDetails.basicTotal " +
                    " ,InvoiceItemDetails.grandTotal " +
                    " ,dealer.firmName as partyName " +
                    " ,distributor.firmName AS cnfName " +
                    " ,transport.firmName AS transporterName " +
                    " ,InvoiceItemDetails.idInvoiceItem " +
                    " ,InvoiceItemDetails.prodItemDesc " +
                    " ,InvoiceItemDetails.taxableAmt " +
                    " ,InvoiceItemDetails.taxAmt " +
                    " ,InvoiceItemDetails.otherTaxId " +
                    " ,currencyName " +
                    " ,statusName " +
                    " ,invoiceTypeDesc " +
                    " FROM finalInvoiceItemDetails InvoiceItemDetails " +
                    " LEFT JOIN finalInvoice Invoice ON InvoiceItemDetails.invoiceId = Invoice.idInvoice " +
                    " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                    " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                    " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                    " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                    " " + whereCondition;


                cmdSelect.Connection = conn;
                cmdSelect.CommandText = selectQuery;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOtherTaxRpt> list = ConvertDTToOtherTaxRptTOList(reader);
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<TblOtherTaxRpt> ConvertDTToOtherTaxRptTOList(SqlDataReader tblInvoiceRptTODT)
        {
            List<TblOtherTaxRpt> tblOtherTaxRptList = new List<TblOtherTaxRpt>();
            try
            {
                if (tblInvoiceRptTODT != null)
                {

                    while (tblInvoiceRptTODT.Read())
                    {
                        TblOtherTaxRpt tblOtherTaxRptNew = new TblOtherTaxRpt();
                        for (int i = 0; i < tblInvoiceRptTODT.FieldCount; i++)
                        {
                            if (tblInvoiceRptTODT.GetName(i).Equals("idInvoice"))
                            {
                                if (tblInvoiceRptTODT["idInvoice"] != DBNull.Value)
                                    tblOtherTaxRptNew.IdInvoice = Convert.ToInt32(tblInvoiceRptTODT["idInvoice"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceNo"))
                            {
                                if (tblInvoiceRptTODT["invoiceNo"] != DBNull.Value)
                                    tblOtherTaxRptNew.InvoiceNo = Convert.ToString(tblInvoiceRptTODT["invoiceNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("vehicleNo"))
                            {
                                if (tblInvoiceRptTODT["vehicleNo"] != DBNull.Value)
                                    tblOtherTaxRptNew.VehicleNo = Convert.ToString(tblInvoiceRptTODT["vehicleNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceDate"))
                            {
                                if (tblInvoiceRptTODT["invoiceDate"] != DBNull.Value)
                                    tblOtherTaxRptNew.InvoiceDate = Convert.ToDateTime(tblInvoiceRptTODT["invoiceDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("netWeight"))
                            {
                                if (tblInvoiceRptTODT["netWeight"] != DBNull.Value)
                                    tblOtherTaxRptNew.NetWeight = Convert.ToDouble(tblInvoiceRptTODT["netWeight"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i) == "basicTotal")
                            {
                                if (tblInvoiceRptTODT["basicTotal"] != DBNull.Value)
                                    tblOtherTaxRptNew.BasicAmt = Convert.ToDouble(tblInvoiceRptTODT["basicTotal"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("partyName"))
                            {
                                if (tblInvoiceRptTODT["partyName"] != DBNull.Value)
                                    tblOtherTaxRptNew.PartyName = Convert.ToString(tblInvoiceRptTODT["partyName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cnfName"))
                            {
                                if (tblInvoiceRptTODT["cnfName"] != DBNull.Value)
                                    tblOtherTaxRptNew.CnfName = Convert.ToString(tblInvoiceRptTODT["cnfName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("transporterName"))
                            {
                                if (tblInvoiceRptTODT["transporterName"] != DBNull.Value)
                                    tblOtherTaxRptNew.TransporterName = Convert.ToString(tblInvoiceRptTODT["transporterName"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceItemId"))
                            {
                                if (tblInvoiceRptTODT["invoiceItemId"] != DBNull.Value)
                                    tblOtherTaxRptNew.InvoiceItemId = Convert.ToInt32(tblInvoiceRptTODT["invoiceItemId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("prodItemDesc"))
                            {
                                if (tblInvoiceRptTODT["prodItemDesc"] != DBNull.Value)
                                    tblOtherTaxRptNew.ProdItemDesc = Convert.ToString(tblInvoiceRptTODT["prodItemDesc"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("grandTotal"))
                            {
                                if (tblInvoiceRptTODT["grandTotal"] != DBNull.Value)
                                    tblOtherTaxRptNew.GrandTotal = Convert.ToDouble(tblInvoiceRptTODT["grandTotal"].ToString());
                            }


                            if (tblInvoiceRptTODT.GetName(i).Equals("taxableAmt"))
                            {
                                if (tblInvoiceRptTODT["taxableAmt"] != DBNull.Value)
                                    tblOtherTaxRptNew.TaxableAmt = Convert.ToDouble(tblInvoiceRptTODT["taxableAmt"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("deliveryLocation"))
                            {
                                if (tblInvoiceRptTODT["deliveryLocation"] != DBNull.Value)
                                    tblOtherTaxRptNew.DeliveryLocation = Convert.ToString(tblInvoiceRptTODT["deliveryLocation"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("taxAmt"))
                            {
                                if (tblInvoiceRptTODT["taxAmt"] != DBNull.Value)
                                    tblOtherTaxRptNew.TaxAmt = Convert.ToDouble(tblInvoiceRptTODT["taxAmt"].ToString());
                            }


                            if (tblInvoiceRptTODT.GetName(i).Equals("isConfirmed"))
                            {
                                if (tblInvoiceRptTODT["isConfirmed"] != DBNull.Value)
                                    tblOtherTaxRptNew.IsConfirmed = Convert.ToInt32(tblInvoiceRptTODT["isConfirmed"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("otherTaxId"))
                            {
                                if (tblInvoiceRptTODT["otherTaxId"] != DBNull.Value)
                                    tblOtherTaxRptNew.OtherTaxId = Convert.ToInt32(tblInvoiceRptTODT["otherTaxId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("statusId"))
                            {
                                if (tblInvoiceRptTODT["statusId"] != DBNull.Value)
                                    tblOtherTaxRptNew.StatusId = Convert.ToInt32(tblInvoiceRptTODT["statusId"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("statusDate"))
                            {
                                if (tblInvoiceRptTODT["statusDate"] != DBNull.Value)
                                    tblOtherTaxRptNew.StatusDate = Convert.ToDateTime(tblInvoiceRptTODT["statusDate"].ToString());
                            }



                            if (tblInvoiceRptTODT.GetName(i).Equals("statusName"))
                            {
                                if (tblInvoiceRptTODT["statusName"] != DBNull.Value)
                                    tblOtherTaxRptNew.StatusName = Convert.ToString(tblInvoiceRptTODT["statusName"].ToString());
                            }


                        }

                        tblOtherTaxRptList.Add(tblOtherTaxRptNew);

                    }
                }
                // return tblInvoiceTOList;
                return tblOtherTaxRptList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion

        #region For Dropbox

        //Added by minal 02 April 2021

        public static List<TblTallyStockTransferRptTO> SelectTallyStockTransferDetails(DateTime fromDate,DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = Constants.ServerDateTime;
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string sql1 = " WITH cte_TallyStockTransfer AS  " +
                                        "  ( " +
                                        "    SELECT loading.createdOn AS date,'Stock Journal' AS voucherType," +
                                        "    CASE WHEN productItem.itemName IS NOT NULL THEN productItem.itemName ELSE " +
                                        "  (material.materialSubType + '/' + dimProdCat.prodCateDesc + '/' + dimProdSpec.prodSpecDesc) END AS SourceConsumptionSTOCKITEM , " +
                                        "           LEFT(org.firmName,3) AS SourceConsumptionGODOWNCAndF,CAST(ROUND(ISNULL(itemDetails.invoiceQty,0),3) AS NUMERIC(36,3)) AS SourceConsumptionQTY, " +
                                        "           CAST(ROUND(ISNULL(itemDetails.rate,0),3) AS NUMERIC(36,3)) AS SourceConsumptionRate," +
                                        "           CAST(ROUND(ISNULL(((itemDetails.invoiceQty)*(itemDetails.rate)),0),2) AS NUMERIC(36,2)) AS SourceConsumptionAMOUNT," +
                                        "           CASE WHEN productItem.itemName IS NOT NULL THEN productItem.itemName ELSE " +
                                        "  (material.materialSubType + '/' + dimProdCat.prodCateDesc + '/' + dimProdSpec.prodSpecDesc) END AS DestinationProductionSTOCKITEM ," +
                                        "          LEFT(orgDealer.firmName,3) AS DestinationProductionGODOWNDelerMappedtoCAndF," +
                                        "           CAST(ROUND(ISNULL(itemDetails.invoiceQty,0),3) AS NUMERIC(36,3)) AS DestinationProductionQTY," +
                                        "           CAST(ROUND(ISNULL(itemDetails.rate,0),3) AS NUMERIC(36,3)) AS DestinationProductionRate," +
                                        "           CAST(ROUND(ISNULL(((itemDetails.invoiceQty)*(itemDetails.rate)),0),2) AS NUMERIC(36,2)) AS DestinationProductionAMOUNT," +
                                        "           UPPER(loadingSlip.vehicleNo) + ' + ' + CAST(itemDetails.invoiceQty AS NVARCHAR(10)) AS Narration  " +
                                        "    FROM tempLoading loading " +
                                        "    LEFT JOIN tempLoadingSlip loadingSlip ON loadingSlip.loadingId = loading.idLoading  " +
                                        "    LEFT JOIN tempLoadingSlipExt lExt ON lExt.loadingSlipId = loadingSlip.idLoadingSlip   " +
                                        "    LEFT JOIN tempInvoiceItemDetails itemDetails ON itemDetails.loadingSlipExtId = lExt.idLoadingSlipExt" +
                                        "    LEFT JOIN tblProdGstCodeDtls prodGstCodeDtls ON prodGstCodeDtls.idProdGstCode = itemDetails.prodGstCodeId " +
                                        "    LEFT JOIN tblProductItem productItem ON productItem.idProdItem = prodGstCodeDtls.prodItemId " +
                                        "    LEFT JOIN tblMaterial material ON material.idMaterial = prodGstCodeDtls.materialId " +
                                        "    LEFT JOIN dimProdCat dimProdCat ON dimProdCat.idProdCat = prodGstCodeDtls.prodCatId " +
                                        "    LEFT JOIN dimProdSpec dimProdSpec ON dimProdSpec.idProdSpec = prodGstCodeDtls.prodSpecId " +
                                        "    LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId" +
                                        "    LEFT JOIN tblOrganization orgDealer ON orgDealer.idOrganization = loadingSlip.dealerOrgId   " +
                                        "    WHERE   org.isInternalCnf = 1 AND loadingSlip.statusId = 17 AND " +
                                        "    loading.createdOn BETWEEN  @fromDate AND @toDate " +
                                        " )" +
                                        " " +
                                        " SELECT FORMAT(date,'dd/MM/yyyy') AS date,voucherType ,SourceConsumptionSTOCKITEM," +
                                        " SourceConsumptionGODOWNCAndF ,CAST(SourceConsumptionQTY AS NVARCHAR) AS SourceConsumptionQTY," +
                                        " CAST(SourceConsumptionRate AS NVARCHAR) AS SourceConsumptionRate,CAST(SourceConsumptionAMOUNT AS NVARCHAR) AS SourceConsumptionAMOUNT," +
                                        " DestinationProductionSTOCKITEM ,DestinationProductionGODOWNDelerMappedtoCAndF," +
                                        " CAST(DestinationProductionQTY AS NVARCHAR) AS DestinationProductionQTY,CAST(DestinationProductionRate AS NVARCHAR) AS DestinationProductionRate," +
                                        " CAST(DestinationProductionAMOUNT AS NVARCHAR) AS DestinationProductionAMOUNT,Narration " +
                                        " FROM cte_TallyStockTransfer" +
                                        " ORDER BY CONVERT(DateTime,date,101)  ASC";

                cmdSelect.CommandText = sql1;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTallyStockTransferRptTO> list = ConvertDTToListForRPTTallyStockTransferReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblCCStockTransferRptTO> SelectCCStockTransferDetails(DateTime fromDate,DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string Sql1 = " SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NEWID())) AS srNo, " +
                                        " FORMAT(loadingSlip.createdOn,'dd/MM/yyyy') AS date, " +
                                        " CONVERT(CHAR(5), loadingSlip.createdOn, 108) AS time," +
                                        " loading.vehicleNo AS vehicleNo," +
                                        " org.firmName AS fromCAndF," +
                                        " orgDealer.firmName AS toFromDelear," +
                                        " lExt.LoadingNetWht AS loadingNetWeight," +
                                        " itemName = CASE WHEN (STUFF((SELECT DISTINCT ',' + tblProductItem1.itemName" +
                                        " FROM tempLoadingSlipExt tempLoadingSlipExt" +
                                        " LEFT JOIN tempLoadingSlip loadingSlipItemName1 ON loadingSlipItemName1.idLoadingSlip = tempLoadingSlipExt.loadingSlipId " +
                                        " LEFT JOIN tblProductItem tblProductItem1 ON tblProductItem1.idProdItem = tempLoadingSlipExt.prodItemId" +
                                        " WHERE loadingSlipItemName1.idLoadingSlip  = loadingSlip.idLoadingSlip" +
                                        " FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) IS NOT NULL THEN" +
                                        " (STUFF((SELECT DISTINCT ',' + tblProductItem2.itemName" +
                                        " FROM tempLoadingSlipExt tempLoadingSlipExt" +
                                        " LEFT JOIN tempLoadingSlip loadingSlipItemName2 ON loadingSlipItemName2.idLoadingSlip = tempLoadingSlipExt.loadingSlipId " +
                                        " LEFT JOIN tblProductItem tblProductItem2 ON tblProductItem2.idProdItem = tempLoadingSlipExt.prodItemId" +
                                        " WHERE loadingSlipItemName2.idLoadingSlip  = loadingSlip.idLoadingSlip" +
                                        " FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) ELSE" +
                                        " (STUFF((SELECT DISTINCT ',' + (material.materialSubType + '/' + dimProdCat.prodCateDesc + '/' + dimProdSpec.prodSpecDesc)" +
                                        " FROM tempLoadingSlipExt tempLoadingSlipExt" +
                                        " LEFT JOIN tblMaterial material ON material.idMaterial = tempLoadingSlipExt.materialId" +
                                        " LEFT JOIN dimProdCat dimProdCat ON dimProdCat.idProdCat = tempLoadingSlipExt.prodCatId" +
                                        " LEFT JOIN dimProdSpec dimProdSpec ON dimProdSpec.idProdSpec = tempLoadingSlipExt.prodSpecId" +
                                        " LEFT JOIN tempLoadingSlip loadingSlipStuff ON loadingSlipStuff.idLoadingSlip = tempLoadingSlipExt.loadingSlipId" +
                                        " WHERE loadingSlipStuff.idLoadingSlip  = loadingSlip.idLoadingSlip" +
                                        " FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) END" +
                                        " FROM tempLoading loading" +
                                        " LEFT JOIN tempLoadingSlip loadingSlip ON loadingSlip.loadingId = loading.idLoading " +
                                        " LEFT JOIN" +
                                        "           (" +
                                        "             SELECT tempLoadingSlipExt.loadingSlipId,SUM(tempLoadingSlipExt.loadedWeight) AS LoadingNetWht" +
                                        "              FROM tempLoadingSlipExt tempLoadingSlipExt" +
                                        "             GROUP BY tempLoadingSlipExt.loadingSlipId" +
                                        "            ) AS lExt" +
                                        " ON lExt.loadingSlipId = loadingSlip.idLoadingSlip" +
                                        " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId" +
                                        " LEFT JOIN tblOrganization orgDealer ON orgDealer.idOrganization = loadingSlip.dealerOrgId" +
                                        " WHERE org.isInternalCnf = 1 AND loadingSlip.statusId = 17 AND " +
                                        " loading.createdOn BETWEEN  @fromDate AND @toDate ";
                                        //" ORDER BY loading.vehicleNo ASC";
                
                cmdSelect.CommandText = Sql1;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCCStockTransferRptTO> list = ConvertDTToListForRPTCCStockTransferReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }        

        public static List<TblInvoiceRptTO> SelectAllRptInvoiceDetailsForDropbox(DateTime fromDate,DateTime toDate,int isConfirmed,String strOrgTempId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                selectQuery =
                    " Select invoice.idInvoice,invoice.invoiceNo,invoice.statusDate ,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn,invoice.tdsAmt," +
                           " invoiceAddress.billingName as partyName, org.firmName cnfName , org.isInternalCnf ,  " +
                           "  booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
                           "  itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, " +
                           "  mat.materialSubType as materialName , tblProductItem.itemName ,  " +
                          "   itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId,itemTaxDetails.taxRatePct ,taxRate.taxTypeId , " +
                            " invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , itemTaxDetails.taxAmt,itemDetails.grandTotal," +
                            " invoice.isConfirmed,invoiceAddress.txnAddrTypeId,invoice.statusId,invoiceAddress.taluka as talukaName " +
                            " ,invoice.deliveredOn,invoice.invFromOrgId, LEFT(weighingMeasures.machineName,3) AS 'godown',loading.idLoading AS loadingId, ISNULL(itemTallyRefDtls.overdueTallyRefId, '') overdueTallyRefId FROM tempInvoice invoice " +
                            " INNER JOIN tempInvoiceAddress invoiceAddress " +
                            " ON invoiceAddress.invoiceId = invoice.idInvoice " +
                            " INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId " +
                            " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                            " LEFT JOIN tempLoadingSlipExt lExt " +
                            " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                            " LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking " +
                            " LEFT JOIN tempInvoiceItemTaxDtls itemTaxDetails " +
                            " ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem " +
                            " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId " +

                            " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                            " LEFT JOIN  tblProductItem tblProductItem on prodGstCodeDtl.prodItemId = tblProductItem.idProdItem" +
                            " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                            " LEFT JOIN tblItemTallyRefDtls itemTallyRefDtls on ISNULL(itemTallyRefDtls.prodCatId, 0) = ISNULL(prodGstCodeDtl.prodCatId, 0) AND ISNULL(itemTallyRefDtls.prodSpecId, 0) = ISNULL(prodGstCodeDtl.prodSpecId, 0) AND ISNULL(itemTallyRefDtls.materialId, 0) = ISNULL(prodGstCodeDtl.materialId, 0) AND ISNULL(itemTallyRefDtls.prodItemId, 0) = ISNULL(prodGstCodeDtl.prodItemId, 0) " +

                            //Added by minal 31 March 2021 for Deliver Report
                            " LEFT JOIN tempLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = invoice.loadingSlipId" +
                            " LEFT JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId" +
                            " LEFT JOIN (" +
                            " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                            " tempWeighingMeasures.idWeightMeasure,tblWeighingMachine.machineName,tempWeighingMeasures.loadingId" +
                            " FROM tempWeighingMeasures tempWeighingMeasures" +
                            " LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tempWeighingMeasures.weighingMachineId" +
                            " WHERE tempWeighingMeasures.weightMeasurTypeId = 1 ) AS weighingMeasures " +
                            " ON weighingMeasures.loadingId = loading.idLoading and weighingMeasures.row_number = 1" +
                           //Added by minal
                           // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipExt and finalInvoice and finalInvoiceAddress and finalInvoiceItemDetails and finalInvoiceItemTaxDtls

                           " UNION ALL " +
                           " Select invoice.idInvoice,invoice.invoiceNo,invoice.statusDate ,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn, invoice.tdsAmt,  " +
                           " invoiceAddress.billingName as partyName, org.firmName cnfName , org.isInternalCnf,  " +
                           "  booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
                           "  itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, " +
                           "  mat.materialSubType as materialName , tblProductItem.itemName ,  " +
                           "   itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId,itemTaxDetails.taxRatePct ,taxRate.taxTypeId , " +
                            " invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , itemTaxDetails.taxAmt,itemDetails.grandTotal," +
                            " invoice.isConfirmed,invoiceAddress.txnAddrTypeId,invoice.statusId,invoiceAddress.taluka as talukaName " +
                            " ,invoice.deliveredOn,invoice.invFromOrgId, LEFT(weighingMeasures.machineName,3) AS 'godown',loading.idLoading AS loadingId, ISNULL(itemTallyRefDtls.overdueTallyRefId, '') overdueTallyRefId FROM finalInvoice invoice " +
                            " INNER JOIN finalInvoiceAddress invoiceAddress " +
                            " ON invoiceAddress.invoiceId = invoice.idInvoice " +
                            " INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId " +
                            " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                            " LEFT JOIN finalLoadingSlipExt lExt " +
                            " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                            " LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking " +
                            " LEFT JOIN finalInvoiceItemTaxDtls itemTaxDetails " +
                            " ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem " +
                            " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId " +

                            " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                            " LEFT JOIN  tblProductItem tblProductItem on prodGstCodeDtl.prodItemId = tblProductItem.idProdItem" +
                            " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                            " LEFT JOIN tblItemTallyRefDtls itemTallyRefDtls on ISNULL(itemTallyRefDtls.prodCatId, 0) = ISNULL(prodGstCodeDtl.prodCatId, 0) AND ISNULL(itemTallyRefDtls.prodSpecId, 0) = ISNULL(prodGstCodeDtl.prodSpecId, 0) AND ISNULL(itemTallyRefDtls.materialId, 0) = ISNULL(prodGstCodeDtl.materialId, 0) AND ISNULL(itemTallyRefDtls.prodItemId, 0) = ISNULL(prodGstCodeDtl.prodItemId, 0) " +

                            //Added by minal 31 March 2021 For Deliver
                            " LEFT JOIN finalLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = invoice.loadingSlipId" +
                            " LEFT JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId" +
                            " LEFT JOIN (" +
                            " SELECT ROW_NUMBER() OVER(PARTITION BY finalWeighingMeasures.loadingId ORDER BY finalWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                            " finalWeighingMeasures.idWeightMeasure,tblWeighingMachine.machineName,finalWeighingMeasures.loadingId" +
                            " FROM finalWeighingMeasures finalWeighingMeasures" +
                            " LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = finalWeighingMeasures.weighingMachineId" +
                            " WHERE finalWeighingMeasures.weightMeasurTypeId = 1 ) AS weighingMeasures" +
                            " ON weighingMeasures.loadingId = loading.idLoading and weighingMeasures.row_number = 1";

                //cmdSelect.CommandText = selectQuery + " WHERE invoice.isConfirmed =" + isConfirm +
                //     " AND CAST(invoice.invoiceDate AS DATE) BETWEEN @fromDate AND @toDate" +
                //     " AND invoiceAddress.txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS +
                //     " AND invoice.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED;

                //Vaibhav comment and modify
                cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE " +
                     " sq1.invoiceDate BETWEEN @fromDate AND @toDate" +
                     " AND sq1.txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED;
                if (!String.IsNullOrEmpty(strOrgTempId))
                {
                    cmdSelect.CommandText += " AND sq1.invFromOrgId in(" + strOrgTempId + ")";
                }
                                          

                if (isConfirmed == 0)
                {
                    cmdSelect.CommandText += " AND sq1.isConfirmed=" + isConfirmed;
                }

                cmdSelect.CommandText += " order by sq1.deliveredOn asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = Constants.ServerDateTime;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<TblWBRptTO> SelectWBForPurchaseReportList(string loadingIds)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            //DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery = " DECLARE @Temp TABLE(idR INT IDENTITY NOT NULL,wBID NVARCHAR(100),userID NVARCHAR(100),orignalRSTNo NVARCHAR(200),additionalRSTNo NVARCHAR(200),date NVARCHAR(20),time NVARCHAR(20),materialType NVARCHAR(500)," +
                    " materialSubType NVARCHAR(1000),grossWeight DECIMAL(18,2),firstWeight DECIMAL(18,2),secondWeight DECIMAL(18,2),thirdWeight DECIMAL(18,2),forthWeight DECIMAL(18,2),fifthWeight DECIMAL(18,2),sixthWeight DECIMAL(18,2)," +
                    " seventhWeight DECIMAL(18,2),tareWeight DECIMAL(18,2),netWeight DECIMAL(18,2),loadOrUnload NVARCHAR(50),fromLocation NVARCHAR(100),toLocation NVARCHAR(100),transactionType NVARCHAR(100),vehicleNumber NVARCHAR(100),vehicleStatus NVARCHAR(100),billType NVARCHAR(100),vehicleID NVARCHAR(100)," +
                    " statusId INT,isActive INT,rootScheduleId INT,idPurchaseScheduleSummary INT)" +
                    " DECLARE @Temp1 TABLE (  idR1 INT IDENTITY NOT NULL,rootScheduleId INT)" +
                    " INSERT INTO @Temp " +
                    " SELECT purchaseWeighingStageSummary.machineName AS wBID,purchaseWeighingStageSummary.userDisplayName AS userID,'-' AS orignalRSTNo, " +
                    " '-' AS additionalRSTNo,FORMAT(tareWt.createdOn,'dd/MM/yyyy') AS date,CONVERT(CHAR(5),tareWt.createdOn, 108) AS time," +
                    " prodClassification.prodClassDesc AS materialType,materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName FROM tblProductItem productItem " +
                    " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem " +
                    " WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = purchaseScheduleSummary.idPurchaseScheduleSummary" +
                    " FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')),grossWt.grossWeightMT AS grossWeight," +
                    " wtStage1.actualWeightMT AS firstWeight,wtStage2.actualWeightMT AS secondWeight,wtStage3.actualWeightMT AS thirdWeight," +
                    " wtStage4.actualWeightMT AS forthWeight,wtStage5.actualWeightMT AS fifthWeight,wtStage6.actualWeightMT AS sixthWeight," +
                    " wtStage7.actualWeightMT AS seventhWeight,tareWt.actualWeightMT AS tareWeight," +
                    " CASE WHEN ((ISNULL(grossWt.grossWeightMT,0)) - (ISNULL(tareWt.actualWeightMT,0))) < 0 THEN 0" +
                    " ELSE ((ISNULL(grossWt.grossWeightMT,0)) - (ISNULL(tareWt.actualWeightMT,0))) END AS netWeight ," +
                    " 'Unload' AS loadOrUnload, " +
                    " CASE WHEN purchaseVehicleSpotEntry.location IS NOT NULL THEN purchaseVehicleSpotEntry.location ELSE purchaseScheduleSummary.location END AS fromLocation, " +
                    " 'Jalna' AS toLocation,'Purchase' AS transactionType, " +
                    " purchaseScheduleSummary.vehicleNo AS vehicleNumber,dimStatus.statusDesc AS vehicleStatus," +
                    " CASE WHEN purchaseScheduleSummary.cOrNCId = 1 THEN 'Order' WHEN purchaseScheduleSummary.cOrNCId = 0 THEN 'Enquiry' ELSE '' END AS billType," +
                    " purchaseScheduleSummary.rootScheduleId AS vehicleID,purchaseScheduleSummary.statusId,purchaseScheduleSummary.isActive,purchaseScheduleSummary.rootScheduleId,purchaseScheduleSummary.idPurchaseScheduleSummary  " +
                    " FROM tblPurchaseScheduleSummary purchaseScheduleSummary" +
                    " LEFT JOIN " +
                    "           (" +
                    "               SELECT tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId,weighingMachine.machineName,tblUser.userDisplayName " +
                    "               FROM tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
                    "               LEFT JOIN tblWeighingMachine weighingMachine ON weighingMachine.idWeighingMachine = tblPurchaseWeighingStageSummary.weighingMachineId " +
                    "               LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseWeighingStageSummary.createdBy " +
                    "               WHERE tblPurchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                    "           ) AS purchaseWeighingStageSummary  " +
                    " ON ISNULL(purchaseScheduleSummary.rootScheduleId,purchaseScheduleSummary.idPurchaseScheduleSummary) = purchaseWeighingStageSummary.purchaseScheduleSummaryId " +
                    " LEFT JOIN tblPurchaseEnquiry purchaseEnquiry ON purchaseEnquiry.idPurchaseEnquiry = purchaseScheduleSummary.purchaseEnquiryId " +
                    " LEFT JOIN tblProdClassification prodClassification ON prodClassification.idProdClass = purchaseEnquiry.prodClassId " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary grossWt ON grossWt.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND grossWt.weightStageId = 0 AND grossWt.weightMeasurTypeId = 3 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage1 ON wtStage1.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage1.weightStageId = 1 AND wtStage1.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage2 ON wtStage2.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage2.weightStageId = 2 AND wtStage2.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage3 ON wtStage3.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage3.weightStageId = 3 AND wtStage3.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage4 ON wtStage4.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage4.weightStageId = 4 AND wtStage4.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage5 ON wtStage5.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage5.weightStageId = 5 AND wtStage5.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage6 ON wtStage6.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage6.weightStageId = 6 AND wtStage6.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage7 ON wtStage7.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage7.weightStageId = 7 AND wtStage7.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary tareWt ON tareWt.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND tareWt.weightMeasurTypeId = 1 " +
                    " LEFT JOIN tblPurchaseVehicleSpotEntry purchaseVehicleSpotEntry ON purchaseVehicleSpotEntry.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary)" +
                    " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus = purchaseScheduleSummary.statusId" +
                    " WHERE CAST(purchaseScheduleSummary.createdOn AS DATE) = @toDate" +
                    " AND purchaseScheduleSummary.isActive = 1 " +
                    "" +
                    " INSERT INTO @Temp1 (rootScheduleId)" +
                    " SELECT rootScheduleId FROM @Temp" +
                    "" +
                    " DECLARE @VarID INT" +
                    " SET     @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp1)" +
                    " DECLARE @VarRid INT" +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp1" +
                    " WHILE @VarID !=0" +
                    " BEGIN" +
                    "" +
                    "  DECLARE @statusId INT" +
                    "  DECLARE @vehiclePhaseId INT" +
                    "  DECLARE @rootScheduleId  INT" +
                    "  SELECT  @rootScheduleId = rootScheduleId FROM @Temp1 WHERE   IdR1 = @VarRid" +
                    "" +
                    " SELECT @statusId = tblPurchaseScheduleSummary.statusId,@vehiclePhaseId = tblPurchaseScheduleSummary.vehiclePhaseId " +
                    " FROM  tblPurchaseScheduleSummary tblPurchaseScheduleSummary" +
                    " WHERE tblPurchaseScheduleSummary.idPurchaseScheduleSummary = (SELECT MAX(purchaseScheduleSummarytbl.idPurchaseScheduleSummary) FROM tblPurchaseScheduleSummary purchaseScheduleSummarytbl WHERE purchaseScheduleSummarytbl.rootScheduleId = @rootScheduleId)" +
                    "" +
                    "  IF @statusId = 509 AND @vehiclePhaseId = 2 " +
                    "  BEGIN" +
                    "  UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    "  FROM tblProductItem productItem " +
                    "  LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    "  WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 2) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    "  WHERE rootScheduleId = @rootScheduleId " +
                    "" +
                    " UPDATE @Temp SET vehicleStatus = 'Grading Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    "" +
                    " IF @statusId = 510 AND @vehiclePhaseId = 2" +
                    " BEGIN" +
                    "  UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END" +
                    "" +
                    "  IF @statusId = 509 AND @vehiclePhaseId = 3 " +
                    "  BEGIN" +
                    "  UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    "  FROM tblProductItem productItem " +
                    "  LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    "  WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 3) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    "  WHERE rootScheduleId = @rootScheduleId " +
                    "" +
                    " UPDATE @Temp SET vehicleStatus = 'Recovery Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    "" +
                    " IF @statusId = 510 AND @vehiclePhaseId = 3" +
                    " BEGIN" +
                    "   UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END" +
                    "" +
                    " IF @statusId = 509 AND @vehiclePhaseId = 4 " +
                    " BEGIN" +
                    " UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    " FROM tblProductItem productItem " +
                    " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    " WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 4) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    " WHERE rootScheduleId = @rootScheduleId " +
                    " " +
                    " UPDATE @Temp SET vehicleStatus = 'Correction Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    "" +
                    " IF @statusId = 510 AND @vehiclePhaseId = 4" +
                    " BEGIN" +
                    "  UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END" +
                    "" +
                    " " +
                    " DELETE @Temp1 WHERE IdR1 = @VarRid    " +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp1) " +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp1" +
                    " END" +
                    "" +
                    " SELECT wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,firstWeight,secondWeight,thirdWeight,forthWeight," +
                    " fifthWeight,sixthWeight,seventhWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,UPPER(vehicleNumber) AS vehicleNumber,vehicleStatus,billType,vehicleID" +
                    " FROM @Temp";
                cmdSelect.CommandText = selectQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                //cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt.Date;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblWBRptTO> SelectWBForSaleReportList(DateTime fromDate,DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string sql1 = String.Empty;
            
            try
            {
                conn.Open();
                sql1 = " DECLARE @Temp TABLE ( idR1 INT IDENTITY NOT NULL,loadingId INT)" +
                    " DECLARE @Temp1 TABLE( idR2 INT IDENTITY NOT NULL,rowNumber INT,idWeightMeasure INT,loadingId INT,intermediateWt DECIMAL(18,2))" +
                    " DECLARE @Temp2 TABLE(idR3 INT IDENTITY NOT NULL,loadingId INT,wBID NVARCHAR(100),userID NVARCHAR(100),orignalRSTNo NVARCHAR(200),additionalRSTNo NVARCHAR(200),date NVARCHAR(20),time NVARCHAR(20),materialType NVARCHAR(500)," +
                    " materialSubType NVARCHAR(1000),grossWeight DECIMAL(18,2),firstWeight DECIMAL(18,2),secondWeight DECIMAL(18,2),thirdWeight DECIMAL(18,2),forthWeight DECIMAL(18,2),fifthWeight DECIMAL(18,2),sixthWeight DECIMAL(18,2)," +
                    " seventhWeight DECIMAL(18,2),tareWeight DECIMAL(18,2),netWeight DECIMAL(18,2),loadOrUnload NVARCHAR(50),fromLocation NVARCHAR(100),toLocation NVARCHAR(100),transactionType NVARCHAR(100),vehicleNumber NVARCHAR(100),vehicleStatus NVARCHAR(100),billType NVARCHAR(100),vehicleID NVARCHAR(100))" +
                    " INSERT INTO @Temp (loadingId) SELECT loading.idLoading FROM tempLoading loading "+                              
                    " WHERE CAST(loading.createdOn AS DATE) BETWEEN @fromDate AND @toDate" + 
                     " "+
                     " "+
                     " INSERT INTO @Temp2 " +
                     " (loadingId,wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,tareWeight,netWeight," +
                     " loadOrUnload,fromLocation,toLocation,transactionType,vehicleNumber,vehicleStatus,billType,vehicleID)" +
                     " SELECT loading.idLoading, weighingMeasures.machineName AS wBID, weighingMeasures.userDisplayName AS userID," +
                     " '-' AS orignalRSTNo,'-' AS additionalRSTNo,FORMAT(weighingMeasuresTareWt.createdOn,'dd/MM/yyyy') AS date,CONVERT(CHAR(5),weighingMeasuresTareWt.createdOn, 108) AS time," +
                     " CASE WHEN loading.loadingType = 1 THEN 'TMT' WHEN loading.loadingType = 2 THEN 'Other' ELSE '' END AS materialType,'-' AS materialSubType, " +
                     " weighingMeasuresGrossWt.weightMT AS grossWeight,weighingMeasuresTareWt.weightMT AS tareWeight, " +
                     " CASE WHEN ((ISNULL(weighingMeasuresGrossWt.weightMT,0)) - (ISNULL(weighingMeasuresTareWt.weightMT,0))) < 0 THEN 0" +
                     " ELSE ((ISNULL(weighingMeasuresGrossWt.weightMT,0)) - (ISNULL(weighingMeasuresTareWt.weightMT,0))) END AS netWeight," +
                     " 'Load' AS loadOrUnload,'-' AS fromLocation ,'Jalna' AS toLocation,'Sale' AS transactionType," +
                     " weighingMeasuresTareWt.vehicleNo AS vehicleNumber, dimStatus.statusName AS vehicleStatus,'-' AS billType,loading.loadingSlipNo AS vehicleId " +
                     " FROM tempLoading loading " +
                     " LEFT JOIN (" +
                     "              SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tblWeighingMachine.machineName ASC) AS row_number," +
                     "              tempWeighingMeasures.idWeightMeasure,tblWeighingMachine.machineName,tempWeighingMeasures.loadingId,tblUser.userDisplayName" +
                     "              FROM tempWeighingMeasures tempWeighingMeasures" +
                     "              LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tempWeighingMeasures.weighingMachineId" +
                     "              LEFT JOIN tblUser tblUser ON tblUser.idUser = tempWeighingMeasures.createdBy " +
                     "              WHERE tempWeighingMeasures.weightMeasurTypeId = 1 " +
                     "           ) AS weighingMeasures" +
                     " ON weighingMeasures.loadingId = loading.idLoading AND weighingMeasures.row_number = 1" +
                     " LEFT JOIN (" +
                     " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure ASC) AS row_number," +
                     " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.loadingId,tempWeighingMeasures.weightMT" +
                     " FROM tempWeighingMeasures tempWeighingMeasures" +
                     " WHERE tempWeighingMeasures.weightMeasurTypeId = 3" +
                     "  ) AS weighingMeasuresGrossWt" +
                     " ON weighingMeasuresGrossWt.loadingId = loading.idLoading AND weighingMeasuresGrossWt.row_number = 1" +
                     " LEFT JOIN (" +
                     " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                     " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.loadingId,tempWeighingMeasures.weightMT,tempWeighingMeasures.vehicleNo,tempWeighingMeasures.createdOn" +
                     " FROM tempWeighingMeasures tempWeighingMeasures" +
                     " WHERE tempWeighingMeasures.weightMeasurTypeId = 1" +
                     " ) AS weighingMeasuresTareWt" +
                     " ON weighingMeasuresTareWt.loadingId = loading.idLoading AND weighingMeasuresTareWt.row_number = 1" +
                     " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus = loading.statusId"+
                     " WHERE CAST(loading.createdOn AS DATE) BETWEEN @fromDate AND @toDate " +
                     " "+
                     " DECLARE @VarID INT" +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp)" +
                    " DECLARE @VarRid INT" +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp" +
                    " WHILE @VarID !=0" +
                    "    BEGIN" +
                    "       DECLARE @loadingId  INT" +
                    "            SELECT  @loadingId = loadingId FROM @Temp WHERE IdR1 = @VarRid" +
                    " INSERT INTO @Temp1 (rowNumber,idWeightMeasure,loadingId,intermediateWt)" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure ASC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.loadingId,tempWeighingMeasures.weightMT" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 2 AND tempWeighingMeasures.loadingId =@loadingId " +
                    " " +
                    " UPDATE @Temp2 SET firstWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 1 AND loadingId = @loadingId) WHERE loadingId = @loadingId " +
                    " UPDATE @Temp2 SET secondWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 2 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET thirdWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 3 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET forthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 4 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET fifthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 5 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET sixthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 6 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET seventhWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 7 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " " +
                    " DELETE @Temp WHERE IdR1 = @VarRid" +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp) " +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp " +
                    " END" +
                    " SELECT LEFT(wBID,3) AS wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,firstWeight,secondWeight,thirdWeight,forthWeight," +
                    " fifthWeight,sixthWeight,seventhWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,UPPER(vehicleNumber) AS vehicleNumber,vehicleStatus,billType,vehicleID" +
                    " FROM @Temp2";

                cmdSelect.CommandText = sql1;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblWBRptTO> SelectWBForUnloadReportList(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string sql1 = String.Empty;           
            //DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                sql1 = " DECLARE @Temp TABLE (idR1 INT IDENTITY NOT NULL,unLoadingId INT)" +
                    " DECLARE @Temp1 TABLE( idR2 INT IDENTITY NOT NULL,rowNumber INT,idWeightMeasure INT,unLoadingId INT,intermediateWt DECIMAL(18,3))" +
                    " DECLARE @Temp2 TABLE(idR3 INT IDENTITY NOT NULL,unLoadingId INT,wBID NVARCHAR(100),userID NVARCHAR(100),orignalRSTNo NVARCHAR(200),additionalRSTNo NVARCHAR(200),date NVARCHAR(20),time NVARCHAR(20),materialType NVARCHAR(500)," +
                    " materialSubType NVARCHAR(1000),grossWeight DECIMAL(18,2),firstWeight DECIMAL(18,2),secondWeight DECIMAL(18,2),thirdWeight DECIMAL(18,2),forthWeight DECIMAL(18,2),fifthWeight DECIMAL(18,2),sixthWeight DECIMAL(18,2)," +
                    " seventhWeight DECIMAL(18,2),tareWeight DECIMAL(18,2),netWeight DECIMAL(18,2),loadOrUnload NVARCHAR(50),fromLocation NVARCHAR(100),toLocation NVARCHAR(100),transactionType NVARCHAR(100),vehicleNumber NVARCHAR(100),vehicleStatus NVARCHAR(100),billType NVARCHAR(100),vehicleID NVARCHAR(100))" +
                    " INSERT INTO @Temp (unLoadingId) SELECT tblUnLoading.idUnLoading FROM tblUnLoading tblUnLoading "+
                    " WHERE CAST(tblUnLoading.createdOn AS DATE) BETWEEN @fromDate AND @toDate " +
                    "" +
                    "" +
                    " INSERT INTO @Temp2 " +
                    " (unLoadingId,wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,vehicleNumber,vehicleStatus,billType,vehicleID)" +
                    " SELECT tblUnLoading.idUnLoading,weighingMeasures.machineName,weighingMeasures.userDisplayName," +
                    " '-' AS orignalRSTNo,'-' AS additionalRSTNo,FORMAT(weighingMeasuresGrossWt.createdOn,'dd/MM/yyyy') AS date," +
                    " CONVERT(CHAR(5),weighingMeasuresGrossWt.createdOn, 108) AS time,'Other Unloading' AS materialType,tblUnLoading.remark AS materialSubType, " +
                    " weighingMeasuresGrossWt.weightMT AS grossWeight,weighingMeasuresTareWt.weightMT AS tareWeight," +
                    " CASE WHEN ((ISNULL(weighingMeasuresGrossWt.weightMT,0)) - (ISNULL(weighingMeasuresTareWt.weightMT,0))) < 0 THEN 0 " +
                    " ELSE ((ISNULL(weighingMeasuresGrossWt.weightMT,0)) - (ISNULL(weighingMeasuresTareWt.weightMT,0))) END AS netWeight," +
                    " 'Unload' AS loadOrUnload,'-' AS fromLocation ,'Jalna' AS toLocation,'-' AS transactionType," +
                    " tblUnLoading.vehicleNo AS vehicleNumber, '-' AS vehicleStatus,'-' AS billType,tblUnLoading.idUnLoading AS vehicleId " +
                    " FROM tblUnLoading tblUnLoading " +
                    " LEFT JOIN (" +
                    "              SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.unLoadingId ORDER BY tblWeighingMachine.machineName DESC) AS row_number," +
                    "              tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.unLoadingId,tblWeighingMachine.machineName,tblUser.userDisplayName" +
                    "              FROM tempWeighingMeasures tempWeighingMeasures" +
                    "              LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tempWeighingMeasures.weighingMachineId" +
                    "              LEFT JOIN tblUser tblUser ON tblUser.idUser = tempWeighingMeasures.createdBy " +
                    "              WHERE tempWeighingMeasures.weightMeasurTypeId = 1 " +
                    "           ) AS weighingMeasures" +
                    " ON weighingMeasures.unLoadingId = tblUnLoading.idUnLoading AND weighingMeasures.row_number = 1" +
                    " LEFT JOIN (" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.unLoadingId ORDER BY tempWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.unLoadingId,tempWeighingMeasures.weightMT,tempWeighingMeasures.createdOn" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 3" +
                    "  ) AS weighingMeasuresGrossWt" +
                    " ON weighingMeasuresGrossWt.unLoadingId = tblUnLoading.idUnLoading AND weighingMeasuresGrossWt.row_number = 1" +
                    " LEFT JOIN (" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.unLoadingId ORDER BY tempWeighingMeasures.idWeightMeasure ASC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.unLoadingId,tempWeighingMeasures.weightMT" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 1" +
                    " ) AS weighingMeasuresTareWt" +
                    " ON weighingMeasuresTareWt.unLoadingId = tblUnLoading.idUnLoading AND weighingMeasuresTareWt.row_number = 1"+
                    " WHERE CAST(tblUnLoading.createdOn AS Date) BETWEEN @fromDate AND @toDate " +
                    "  " +
                    "  " +
                    " DECLARE @VarID INT" +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp)" +
                    " DECLARE @VarRid INT" +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp" +
                    " WHILE @VarID !=0" +
                    "    BEGIN" +
                    "       DECLARE @UnLoadingId  INT" +
                    "            SELECT  @UnLoadingId = unLoadingId FROM @Temp WHERE IdR1 = @VarRid" +
                    " INSERT INTO @Temp1 (rowNumber,idWeightMeasure,unLoadingId,intermediateWt)" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.unLoadingId ORDER BY tempWeighingMeasures.idWeightMeasure ASC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.unLoadingId,tempWeighingMeasures.weightMT" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 2 AND tempWeighingMeasures.unLoadingId =@UnLoadingId " +
                    " " +
                    " UPDATE @Temp2 SET firstWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 1 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId " +
                    " UPDATE @Temp2 SET secondWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 2 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET thirdWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 3 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET forthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 4 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET fifthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 5 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET sixthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 6 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET seventhWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 7 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " " +
                    " DELETE @Temp WHERE IdR1 = @VarRid" +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp) " +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp " +
                    " END" +
                    " SELECT LEFT(wBID,3) AS wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,firstWeight,secondWeight,thirdWeight,forthWeight," +
                    " fifthWeight,sixthWeight,seventhWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,UPPER(vehicleNumber) AS vehicleNumber,vehicleStatus,billType,vehicleID" +
                    " FROM @Temp2";

                cmdSelect.CommandText = sql1;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Minal[02-April-2021] Added This method to convert dt to rpt WB Report List
        /// </summary>
        /// <param name="tblInvoiceRptTODT"></param>
        /// <returns></returns>
        public static List<TblWBRptTO> ConvertDTToListForRPTWBReport(SqlDataReader tblWBRptTOTODT)
        {
            List<TblWBRptTO> TblWBRptTOList = new List<TblWBRptTO>();
            try
            {
                if (tblWBRptTOTODT != null)
                {

                    while (tblWBRptTOTODT.Read())
                    {
                        TblWBRptTO tblWBRptTONew = new TblWBRptTO();
                        //for (int i = 0; i < tblWBRptTOTODT.FieldCount; i++)
                        //{
                        //    if (tblWBRptTOTODT.GetName(i).Equals("wBID"))
                        //    {
                        if (tblWBRptTOTODT["wBID"] != DBNull.Value)
                            tblWBRptTONew.WBID = Convert.ToString(tblWBRptTOTODT["wBID"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("userID"))
                        //{
                        if (tblWBRptTOTODT["userID"] != DBNull.Value)
                            tblWBRptTONew.UserID = Convert.ToString(tblWBRptTOTODT["userID"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("orignalRSTNo"))
                        //{
                        if (tblWBRptTOTODT["orignalRSTNo"] != DBNull.Value)
                            tblWBRptTONew.OrignalRSTNo = Convert.ToString(tblWBRptTOTODT["orignalRSTNo"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("additionalRSTNo"))
                        //{
                        if (tblWBRptTOTODT["orignalRSTNo"] != DBNull.Value)
                            tblWBRptTONew.AdditionalRSTNo = Convert.ToString(tblWBRptTOTODT["additionalRSTNo"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("date"))
                        //{
                        if (tblWBRptTOTODT["date"] != DBNull.Value)
                            tblWBRptTONew.Date = Convert.ToString(tblWBRptTOTODT["date"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("time"))
                        //{
                        if (tblWBRptTOTODT["time"] != DBNull.Value)
                            tblWBRptTONew.Time = Convert.ToString(tblWBRptTOTODT["time"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("materialType"))
                        //{
                        if (tblWBRptTOTODT["materialType"] != DBNull.Value)
                            tblWBRptTONew.MaterialType = Convert.ToString(tblWBRptTOTODT["materialType"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("materialSubType"))
                        //{
                        if (tblWBRptTOTODT["materialSubType"] != DBNull.Value)
                            tblWBRptTONew.MaterialSubType = Convert.ToString(tblWBRptTOTODT["materialSubType"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("grossWeight"))
                        //{
                        if (tblWBRptTOTODT["grossWeight"] != DBNull.Value)
                            tblWBRptTONew.GrossWeight = Convert.ToDecimal(tblWBRptTOTODT["grossWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("firstWeight"))
                        //{
                        if (tblWBRptTOTODT["firstWeight"] != DBNull.Value)
                            tblWBRptTONew.FirstWeight = Convert.ToDecimal(tblWBRptTOTODT["firstWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("secondWeight"))
                        //{
                        if (tblWBRptTOTODT["secondWeight"] != DBNull.Value)
                            tblWBRptTONew.SecondWeight = Convert.ToDecimal(tblWBRptTOTODT["secondWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("thirdWeight"))
                        //{
                        if (tblWBRptTOTODT["thirdWeight"] != DBNull.Value)
                            tblWBRptTONew.ThirdWeight = Convert.ToDecimal(tblWBRptTOTODT["thirdWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("forthWeight"))
                        //{
                        if (tblWBRptTOTODT["forthWeight"] != DBNull.Value)
                            tblWBRptTONew.ForthWeight = Convert.ToDecimal(tblWBRptTOTODT["forthWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("fifthWeight"))
                        //{
                        if (tblWBRptTOTODT["fifthWeight"] != DBNull.Value)
                            tblWBRptTONew.FifthWeight = Convert.ToDecimal(tblWBRptTOTODT["fifthWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("sixthWeight"))
                        //{
                        if (tblWBRptTOTODT["sixthWeight"] != DBNull.Value)
                            tblWBRptTONew.SixthWeight = Convert.ToDecimal(tblWBRptTOTODT["sixthWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("seventhWeight"))
                        //{
                        if (tblWBRptTOTODT["seventhWeight"] != DBNull.Value)
                            tblWBRptTONew.SeventhWeight = Convert.ToDecimal(tblWBRptTOTODT["seventhWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("tareWeight"))
                        //{
                        if (tblWBRptTOTODT["tareWeight"] != DBNull.Value)
                            tblWBRptTONew.TareWeight = Convert.ToDecimal(tblWBRptTOTODT["tareWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("netWeight"))
                        //{
                        if (tblWBRptTOTODT["netWeight"] != DBNull.Value)
                            tblWBRptTONew.NetWeight = Convert.ToDecimal(tblWBRptTOTODT["netWeight"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("loadOrUnload"))
                        //{
                        if (tblWBRptTOTODT["loadOrUnload"] != DBNull.Value)
                            tblWBRptTONew.LoadOrUnload = Convert.ToString(tblWBRptTOTODT["loadOrUnload"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("fromLocation"))
                        //{
                        if (tblWBRptTOTODT["fromLocation"] != DBNull.Value)
                            tblWBRptTONew.FromLocation = Convert.ToString(tblWBRptTOTODT["fromLocation"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("toLocation"))
                        //{
                        if (tblWBRptTOTODT["toLocation"] != DBNull.Value)
                            tblWBRptTONew.ToLocation = Convert.ToString(tblWBRptTOTODT["toLocation"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("transactionType"))
                        //{
                        if (tblWBRptTOTODT["transactionType"] != DBNull.Value)
                            tblWBRptTONew.TransactionType = Convert.ToString(tblWBRptTOTODT["transactionType"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("vehicleNumber"))
                        //{
                        if (tblWBRptTOTODT["vehicleNumber"] != DBNull.Value)
                            tblWBRptTONew.VehicleNumber = Convert.ToString(tblWBRptTOTODT["vehicleNumber"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("vehicleStatus"))
                        //{
                        if (tblWBRptTOTODT["vehicleStatus"] != DBNull.Value)
                            tblWBRptTONew.VehicleStatus = Convert.ToString(tblWBRptTOTODT["vehicleStatus"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("billType"))
                        //{
                        if (tblWBRptTOTODT["billType"] != DBNull.Value)
                            tblWBRptTONew.BillType = Convert.ToString(tblWBRptTOTODT["billType"].ToString());
                        //}
                        //if (tblWBRptTOTODT.GetName(i).Equals("vehicleID"))
                        //{
                        if (tblWBRptTOTODT["vehicleID"] != DBNull.Value)
                            tblWBRptTONew.VehicleID = Convert.ToString(tblWBRptTOTODT["vehicleID"].ToString());
                        //}
                        //  }

                        TblWBRptTOList.Add(tblWBRptTONew);

                    }
                }
                // return TblWBRptTOList;
                return TblWBRptTOList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<TblTallyStockTransferRptTO> ConvertDTToListForRPTTallyStockTransferReport(SqlDataReader tblTallyStockTransferRptTODT)
        {
            List<TblTallyStockTransferRptTO> tblTallyStockTransferRptTOList = new List<TblTallyStockTransferRptTO>();
            try
            {
                if (tblTallyStockTransferRptTODT != null)
                {

                    while (tblTallyStockTransferRptTODT.Read())
                    {
                        TblTallyStockTransferRptTO tblTallyStockTransferRptTONew = new TblTallyStockTransferRptTO();
                        //for (int i = 0; i < tblTallyStockTransferRptTODT.FieldCount; i++)
                        //{
                            if (tblTallyStockTransferRptTODT["date"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.Date = Convert.ToString(tblTallyStockTransferRptTODT["date"].ToString());
                            if (tblTallyStockTransferRptTODT["voucherType"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.VoucherType = Convert.ToString(tblTallyStockTransferRptTODT["voucherType"].ToString());
                            if (tblTallyStockTransferRptTODT["SourceConsumptionSTOCKITEM"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.StockItem = Convert.ToString(tblTallyStockTransferRptTODT["SourceConsumptionSTOCKITEM"].ToString());
                            if (tblTallyStockTransferRptTODT["SourceConsumptionGODOWNCAndF"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.Godown = Convert.ToString(tblTallyStockTransferRptTODT["SourceConsumptionGODOWNCAndF"].ToString());
                            if (tblTallyStockTransferRptTODT["SourceConsumptionQTY"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.Qty = Convert.ToDouble(tblTallyStockTransferRptTODT["SourceConsumptionQTY"].ToString());
                            if (tblTallyStockTransferRptTODT["SourceConsumptionRate"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.Rate = Convert.ToDouble(tblTallyStockTransferRptTODT["SourceConsumptionRate"].ToString());
                            if (tblTallyStockTransferRptTODT["SourceConsumptionAMOUNT"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.Amount = Convert.ToDouble(tblTallyStockTransferRptTODT["SourceConsumptionAMOUNT"].ToString());
                            if (tblTallyStockTransferRptTODT["DestinationProductionSTOCKITEM"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.StockItemD = Convert.ToString(tblTallyStockTransferRptTODT["DestinationProductionSTOCKITEM"].ToString());
                            if (tblTallyStockTransferRptTODT["DestinationProductionGODOWNDelerMappedtoCAndF"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.GodownD = Convert.ToString(tblTallyStockTransferRptTODT["DestinationProductionGODOWNDelerMappedtoCAndF"].ToString());
                            if (tblTallyStockTransferRptTODT["DestinationProductionQTY"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.QtyD = Convert.ToDouble(tblTallyStockTransferRptTODT["DestinationProductionQTY"].ToString());
                            if (tblTallyStockTransferRptTODT["DestinationProductionRate"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.RateD = Convert.ToDouble(tblTallyStockTransferRptTODT["DestinationProductionRate"].ToString());
                            if (tblTallyStockTransferRptTODT["DestinationProductionAMOUNT"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.AmountD = Convert.ToDouble(tblTallyStockTransferRptTODT["DestinationProductionAMOUNT"].ToString());
                            if (tblTallyStockTransferRptTODT["Narration"] != DBNull.Value)
                                tblTallyStockTransferRptTONew.Narration = Convert.ToString(tblTallyStockTransferRptTODT["Narration"].ToString());
                                tblTallyStockTransferRptTOList.Add(tblTallyStockTransferRptTONew);
                       // }                      

                    }
                }
                return tblTallyStockTransferRptTOList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static List<TblCCStockTransferRptTO> ConvertDTToListForRPTCCStockTransferReport(SqlDataReader tblCCStockTransferRptTODT)
        {
            List<TblCCStockTransferRptTO> tblCCStockTransferRptTOList = new List<TblCCStockTransferRptTO>();
            try
            {
                if (tblCCStockTransferRptTODT != null)
                {

                    while (tblCCStockTransferRptTODT.Read())
                    {
                        TblCCStockTransferRptTO tblCCStockTransferRptTONew = new TblCCStockTransferRptTO();
                        // for (int i = 0; i < tblTallyStockTransferRptTODT.FieldCount; i++)
                        //{
                        if (tblCCStockTransferRptTODT["srNo"] != DBNull.Value)
                            tblCCStockTransferRptTONew.SrNo = Convert.ToString(tblCCStockTransferRptTODT["srNo"].ToString());
                        if (tblCCStockTransferRptTODT["date"] != DBNull.Value)
                            tblCCStockTransferRptTONew.Date = Convert.ToString(tblCCStockTransferRptTODT["date"].ToString());
                        if (tblCCStockTransferRptTODT["time"] != DBNull.Value)
                            tblCCStockTransferRptTONew.Time = Convert.ToString(tblCCStockTransferRptTODT["time"].ToString());
                        if (tblCCStockTransferRptTODT["vehicleNo"] != DBNull.Value)
                            tblCCStockTransferRptTONew.VehicleNo = Convert.ToString(tblCCStockTransferRptTODT["vehicleNo"].ToString());
                        if (tblCCStockTransferRptTODT["fromCAndF"] != DBNull.Value)
                            tblCCStockTransferRptTONew.FromCAndF = Convert.ToString(tblCCStockTransferRptTODT["fromCAndF"].ToString());
                        if (tblCCStockTransferRptTODT["toFromDelear"] != DBNull.Value)
                            tblCCStockTransferRptTONew.ToFromDealer = Convert.ToString(tblCCStockTransferRptTODT["toFromDelear"].ToString());
                        if (tblCCStockTransferRptTODT["loadingNetWeight"] != DBNull.Value)
                            tblCCStockTransferRptTONew.LoadingNetWeight = Convert.ToDouble(tblCCStockTransferRptTODT["loadingNetWeight"].ToString());
                        if (tblCCStockTransferRptTODT["itemName"] != DBNull.Value)
                            tblCCStockTransferRptTONew.ItemName = Convert.ToString(tblCCStockTransferRptTODT["itemName"].ToString());

                        // }

                        tblCCStockTransferRptTOList.Add(tblCCStockTransferRptTONew);

                    }
                }
                return tblCCStockTransferRptTOList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        //

        #endregion
    }
}
