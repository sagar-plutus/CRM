using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PurchaseTrackerAPI.StaticStuff;
using System.Data;
using Microsoft.Extensions.Logging;
using PurchaseTrackerAPI.DashboardModels;
using System.Linq;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using System.Globalization;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseEnquiryDAO : ITblPurchaseEnquiryDAO
    {
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        public TblPurchaseEnquiryDAO(IConnectionString iConnectionString, Icommondao icommondao, ITblConfigParamsBL iTblConfigParamsBL)
        {
            _iCommonDAO = icommondao;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            //" left join tblOrgAddress orgAddr
            //                      " ON org.idOrganization = orgAddr.organizationId
            //                      " left join tblAddress
            //                      " ON tblAddress.idAddr = orgAddr.addressId"
            //add above line to get state id of selected supplier

            String sqlSelectQry = " SELECT tblRateBand.rate_band_costing,rate,dimCurrency.currnecyCode,dimMasterValueImpurity.masterValueName as impuritiesToleranceStr,dimMasterValueWeiment.masterValueName  as weighmentToleranceStr,dimMasterValue.masterValueName as contractType,enquiry.*, org.firmName as supplierName, orgPurchaseManager.userDisplayName as purchaseManagerName, dimStatus.statusName,  ProdClass.prodClassDesc, tblAddress.stateId " +
                                  " , createdUser.userDisplayName as createdByName, updatedUser.userDisplayName as updatedByName  " +
                                  " FROM tblPurchaseEnquiry enquiry INNER JOIN tblRateBandDeclarationPurchase as tblRateBand on tblRateBand.idRateBandDeclarationPurchase=enquiry.rateBandDeclarationPurchaseId " +
                                  " INNER JOIN tblglobalratepurchase as globalratepurchase ON globalratepurchase.idGlobalRatePurchase=enquiry.globalRatePurchaseId AND globalratepurchase.idGlobalRatePurchase=tblRateBand.globalRatePurchaseId LEFT JOIN tblProdClassification ProdClass" +
                                  " ON ProdClass.idProdClass = enquiry.prodClassId" +
                                  " LEFT JOIN tblOrganization org" +
                                  " ON enquiry.SupplierId = org.idOrganization" +
                                  //Prajakta[2021-05-10] Commented and added
                                  //" left join tblOrgAddress orgAddr " +
                                  //" ON org.idOrganization = orgAddr.organizationId " +
                                  //" left join tblAddress " +
                                  //" ON tblAddress.idAddr = orgAddr.addressId" +
                                  " left join tblAddress " +
                                  " ON tblAddress.idAddr = org.addrId" +
                                  " LEFT JOIN tblUser orgPurchaseManager" +
                                  " ON enquiry.userId = orgPurchaseManager.idUser" +
                                  " LEFT JOIN tblUser createdUser ON enquiry.createdBy = createdUser.idUser" +
                                  " LEFT JOIN tblUser updatedUser ON enquiry.updatedBy = updatedUser.idUser" +
                                  " LEFT JOIN dimMasterValue dimMasterValue ON dimMasterValue.idMasterValue = enquiry.contractTypeId " +
                                  " LEFT JOIN dimMasterValue dimMasterValueWeiment ON dimMasterValueWeiment.idMasterValue = enquiry.weighmentTolerance " +
                                  " LEFT JOIN dimMasterValue dimMasterValueImpurity ON dimMasterValueImpurity.idMasterValue = enquiry.impuritiesTolerance " +
                                  " LEFT JOIN dimCurrency dimCurrency ON dimCurrency.idCurrency = enquiry.currencyId " +
                                  " LEFT JOIN dimStatus ON dimStatus.idStatus = enquiry.statusId";

            return sqlSelectQry;
        }

        #endregion

        #region Selection

        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiry(Int32 idPurchaseEnquiry)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseEnquiry = " + idPurchaseEnquiry + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(reader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public double SelectParityForCAndNC(Int32 StateId, bool CorNC, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            double parityAmtForCorNC = 0;
            try
            {
                if (CorNC == true)
                {
                    cmdSelect.CommandText = " select top(1) parityAmt  as parityAmt  from tblParityDetailsPurchase tblParityDetailsPurchase "
                   + " left join tblParitySummaryPurchase on tblParitySummaryPurchase.idParityPurchase =tblParityDetailsPurchase.parityPurchaseId "
                   + " where prodItemId = (SELECT  idProdItem   FROM tblProductItem  where isBaseItemForRate =1) and tblParitySummaryPurchase.stateId = " + StateId
                   + " order by tblParityDetailsPurchase.createdOn desc";

                }
                else
                {
                    cmdSelect.CommandText = " select top(1) parityAmt + nonConfParityAmt  as parityAmt  from tblParityDetailsPurchase tblParityDetailsPurchase "
                   + " left join tblParitySummaryPurchase on tblParitySummaryPurchase.idParityPurchase =tblParityDetailsPurchase.parityPurchaseId "
                   + " where prodItemId = (SELECT  idProdItem   FROM tblProductItem  where isBaseItemForRate =1) and tblParitySummaryPurchase.stateId = " + StateId
                   + " order by tblParityDetailsPurchase.createdOn desc";

                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        if (reader["parityAmt"] != DBNull.Value)
                            parityAmtForCorNC = Convert.ToDouble(reader["parityAmt"].ToString());
                    }
                }

                return parityAmtForCorNC;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                reader.Dispose();
                reader.Close();
                cmdSelect.Dispose();
            }

        }

        public TblPurchaseEnquiryTO SelectTblBookingsForPurchase(Int32 idPurchaseEnquiry, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseEnquiry = " + idPurchaseEnquiry + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(reader);
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
                reader.Dispose();
                reader.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseEnquiryTO> SelectAllPurchaseEnquiryForPM(Int32 userId, Int32 rateBandPurchaseId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string whrStr = "";
            try
            {
                whrStr = " AND enquiry.isConvertToSauda=1";
                if (prodClassId > 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE enquiry.userId = " + userId + " AND enquiry.rateBandDeclarationPurchaseId=" + rateBandPurchaseId + " AND enquiry.prodClassId=" + prodClassId;
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE enquiry.userId = " + userId + " AND enquiry.rateBandDeclarationPurchaseId=" + rateBandPurchaseId;

                cmdSelect.CommandText += whrStr;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                reader.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseEnquiryTO> GetSupplierWithMaterialHistList(Int32 supplierId, Int32 lastNRecords)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT Top 4 tblRateBand.rate_band_costing,rate,enquiry.*, org.firmName as supplierName, orgPurchaseManager.userDisplayName as purchaseManagerName, dimStatus.statusName,  ProdClass.prodClassDesc, tblAddress.stateId " +
                                  " FROM tblPurchaseEnquiry enquiry INNER JOIN tblRateBandDeclarationPurchase as tblRateBand on tblRateBand.idRateBandDeclarationPurchase=enquiry.rateBandDeclarationPurchaseId " +
                                  " INNER JOIN tblglobalratepurchase as globalratepurchase ON globalratepurchase.idGlobalRatePurchase=enquiry.globalRatePurchaseId AND globalratepurchase.idGlobalRatePurchase=tblRateBand.globalRatePurchaseId LEFT JOIN tblProdClassification ProdClass" +
                                  " ON ProdClass.idProdClass = enquiry.prodClassId" +
                                  " LEFT JOIN tblOrganization org" +
                                  " ON enquiry.SupplierId = org.idOrganization" +
                                  " left join tblOrgAddress orgAddr " +
                                  " ON org.idOrganization = orgAddr.organizationId " +
                                  " left join tblAddress " +
                                  " ON tblAddress.idAddr = orgAddr.addressId" +
                                  " LEFT JOIN tblUser orgPurchaseManager" +
                                  " ON enquiry.userId = orgPurchaseManager.idUser" +
                                  " LEFT JOIN dimStatus ON dimStatus.idStatus = enquiry.statusId" +
                                  " WHERE enquiry.SupplierId =" + supplierId +
                                  " ORDER BY enquiry.createdOn DESC ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToListNew(sqlReader, false);
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

        public List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where enquiry.SupplierId=" + supplierId + " and enquiry.isConvertToSauda=1 and enquiry.statusId in (" + statusId + ") order by enquiry.saudaCreatedOn desc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader, true);
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

        public List<TblPurchaseEnquiryTO> GetSupplierWiseSaudaDetails(Int32 supplierId, string statusId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where enquiry.SupplierId=" + supplierId + " and enquiry.isConvertToSauda=1 and enquiry.statusId in (" + statusId + ") order by enquiry.saudaCreatedOn desc";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader, true);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                sqlReader.Dispose();
            }
        }


        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where enquiry.idPurchaseEnquiry=" + purchaseEnquiryId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader, true);
                sqlReader.Close();
                if (list != null && list.Count == 1)
                    return list[0];
                else
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

        public List<TblPurchaseEnquiryTO> SelectSaudaListBySaudaIds(string saudaIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where enquiry.idPurchaseEnquiry IN (" + saudaIds + ")";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader, true);
                sqlReader.Close();
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

        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.Transaction = tran;
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where enquiry.idPurchaseEnquiry=" + purchaseEnquiryId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader, true);
                sqlReader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;
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

        public List<TblRecycleDocumentTO> SelectAllDocumentIdFromSpotEntryId(int transId, int transTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblRecycleDocument " + " where isActive =1 and txnId =" + transId + " and txnTypeId=" + transTypeId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader TblRecycleDocumentsTO = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTRecycleDocumentToList(TblRecycleDocumentsTO);
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

        public List<TblRecycleDocumentTO> ConvertDTRecycleDocumentToList(SqlDataReader tblRecycleDocumentsTODT)
        {
            List<TblRecycleDocumentTO> recycleDocumentsToList = new List<TblRecycleDocumentTO>();
            if (tblRecycleDocumentsTODT != null)
            {
                while ((tblRecycleDocumentsTODT).Read())
                {
                    TblRecycleDocumentTO recycleDocumentsTo = new TblRecycleDocumentTO();
                    if ((tblRecycleDocumentsTODT)["idRecycleDocument"] != DBNull.Value)
                        recycleDocumentsTo.IdRecycleDocument = Convert.ToInt32((tblRecycleDocumentsTODT)["idRecycleDocument"].ToString());
                    if ((tblRecycleDocumentsTODT)["txnId"] != DBNull.Value)
                        recycleDocumentsTo.TxnId = Convert.ToInt32((tblRecycleDocumentsTODT)["txnId"].ToString());
                    if ((tblRecycleDocumentsTODT)["txnTypeId"] != DBNull.Value)
                        recycleDocumentsTo.TxnTypeId = Convert.ToInt32((tblRecycleDocumentsTODT)["txnTypeId"].ToString());
                    if ((tblRecycleDocumentsTODT)["isActive"] != DBNull.Value)
                        recycleDocumentsTo.IsActive = Convert.ToInt32((tblRecycleDocumentsTODT)["isActive"].ToString());

                    if ((tblRecycleDocumentsTODT)["documentId"] != DBNull.Value)
                        recycleDocumentsTo.DocumentId = Convert.ToInt32((tblRecycleDocumentsTODT)["documentId"].ToString());
                    if ((tblRecycleDocumentsTODT)["createdBy"] != DBNull.Value)
                        recycleDocumentsTo.CreatedBy = Convert.ToInt32((tblRecycleDocumentsTODT)["createdBy"].ToString());
                    if ((tblRecycleDocumentsTODT)["updatedBy"] != DBNull.Value)
                        recycleDocumentsTo.UpdatedBy = Convert.ToInt32((tblRecycleDocumentsTODT)["updatedBy"].ToString());
                    if ((tblRecycleDocumentsTODT)["createdOn"] != DBNull.Value)
                        recycleDocumentsTo.CreatedOn = Convert.ToDateTime((tblRecycleDocumentsTODT)["createdOn"].ToString());
                    if ((tblRecycleDocumentsTODT)["updatedOn"] != DBNull.Value)
                        recycleDocumentsTo.UpdatedOn = Convert.ToDateTime((tblRecycleDocumentsTODT)["updatedOn"].ToString());
                    recycleDocumentsToList.Add(recycleDocumentsTo);
                }
            }
            return recycleDocumentsToList;
        }

        public TblPurchaseEnquiryTO SelectTblPurchaseEnquiryTO(Int32 userId, Int32 supplierId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where enquiry.userId=" + userId + " and enquiry.SupplierId=" + supplierId + " and enquiry.prodClassId=" + prodClassId + " and enquiry.bookingQty=" + 0;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader, true);
                sqlReader.Close();
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;
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
        public List<TblPurchaseEnquiryTO> SelectTblPurchaseEnquirySpotEntryTO(Int32 spotEntryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where enquiry.vehicleSpotEntryId=" + spotEntryId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader, true);
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

        public List<TblPurchaseEnquiryTO> GetAllEnquiryList(String userId, Int32 organizationId, Int32 statusId, DateTime fromDate, DateTime toDate, Int32 isConvertToSauda, Int32 isPending, Int32 materialTypeId,string cOrNcId, Int32 isSkipDateFilter)//, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            String areConfJoin = String.Empty;
            String notDelStatus = (int)Constants.TranStatusE.BOOKING_DELETE + "";
            // String unwantedStatus = (int)Constants.TranStatusE.BOOKING_DELETE + "," + (int)Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL + "," + (int)Constants.TranStatusE.PENDING_FOR_PURCHASE_MANAGER_APPROVAL;
            String unwantedStatus = string.Empty;

            unwantedStatus = (int)Constants.TranStatusE.BOOKING_DELETE + " ";

            if (isPending > 0 && !Startup.IsForBRM)
            {
                //Prajakta[2021-07-26]added
                unwantedStatus += "," + (int)Constants.TranStatusE.COMPLETED + "," + (int)Constants.TranStatusE.BOOKING_CANCELED;

            }

            String dateString = String.Empty;

            //String whereCondtion = " WHERE 1 = 1 ";
            String whereCondtion = " WHERE ISNULL(isConvertToSauda, 0) = " + isConvertToSauda + " ";

            try
            {
                conn.Open();

                if (isSkipDateFilter != 1)
                {
                    //dateString = "(CAST(enquiry.createdOn AS DATE) BETWEEN @fromDate AND @toDate) AND ";
                    whereCondtion += " AND (CAST(ISNULL(enquiry.saudaCreatedOn,enquiry.createdOn) AS DATE) BETWEEN @fromDate AND @toDate) ";
                }


                if (!String.IsNullOrEmpty(userId) && userId != "0")
                {
                    whereCondtion += " AND  enquiry.userId IN (" + userId + ") ";
                }

                if (organizationId > 0)
                {
                    whereCondtion += " AND enquiry.SupplierId=" + organizationId + " ";
                }

                if (materialTypeId > 0)
                {
                    whereCondtion += " AND enquiry.prodClassId=" + materialTypeId + " ";
                }

                if (statusId > 0)
                {
                    whereCondtion += " AND enquiry.statusId IN (" + statusId + ") ";
                }
                else
                {
                    whereCondtion += " AND  enquiry.statusId NOT IN(" + unwantedStatus + ") ";
                }

                if (isPending > 0 && Startup.IsForBRM)
                {
                    whereCondtion += " AND ISNULL(enquiry.pendingBookingQty,0) > 0 ";
                }

                if(!String.IsNullOrEmpty(cOrNcId))
                {
                    whereCondtion += " AND enquiry.cOrNCId = " + cOrNcId;
                }

                //cmdSelect.CommandText = sqlQuery + " AND isConvertToSauda = " + isConvertToSauda + " ORDER BY enquiry.idPurchaseEnquiry DESC";

                cmdSelect.CommandText = SqlSelectQuery() + whereCondtion + " ORDER BY enquiry.idPurchaseEnquiry DESC ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader);
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

        public List<TblOrganizationTO> SelectAllTblOrganization(Constants.OrgTypeE orgTypeE)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();


                if (orgTypeE == Constants.OrgTypeE.PURCHASE_COMPETITOR)
                {
                    sqlQuery = " SELECT * FROM tblOrganization compeInfo " +
                               " LEFT JOIN ( " +
                               "  SELECT result.* FROM( " +
                               "  SELECT competitorOrgId, max(updateDatetime) updateDatetime " +
                               "  FROM( " +
                               "  SELECT competitorOrgId , compUpdate.updateDatetime, ISNULL(LAG(price) OVER(PARTITION BY IdPurcompetitorExt,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblPurchaseCompetitorUpdates compUpdate " +
                               "   ) as res group by competitorOrgId " +
                               "   ) AS main " +
                               "   inner join " +
                               "   ( " +
                               "   SELECT compUpdate.IdPurcompetitorExt,competitorOrgId ,'' as brandName,'' as prodCapacityMT, compUpdate.updateDatetime, compUpdate.informerName, alternateInformerName ,compUpdate.price, ISNULL(LAG(price) OVER(PARTITION BY compUpdate.IdPurcompetitorExt,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblPurchaseCompetitorUpdates compUpdate " +
                               "   INNER JOIN tblPurchaseCompetitorExt comptExt ON comptExt.idPurCompetitorExt=compUpdate.IdPurcompetitorExt" +
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
                List<TblOrganizationTO> list = ConvertDTToListNew(rdr);
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
        public List<TblPurchaseEnquiryTO> SelectAllBookingsListForAcceptance(String cnfId, TblUserRoleTO tblUserRoleTO, Int32 isGetPendSaudaToClose, Int32 IsOrderOREnq)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL + "";//24 for director approval
            String areConfJoin = String.Empty;
            int isConfEn = 0;
            int userId = 0;
            String orderOREnq = String.Empty;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                conn.Open();


                //if (cnfId == 0)
                //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE statusId IN(" + statusIds + ")";
                //else
                //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE statusId IN(" + statusIds + ")" + " AND bookings.userId=" + cnfId;


                if (!String.IsNullOrEmpty(cnfId) && cnfId != "0")
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE enquiry.statusId IN(" + statusIds + ")" + " AND enquiry.userId IN (" + cnfId + ") ";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE enquiry.statusId IN(" + statusIds + ")";

                if (IsOrderOREnq >= 0)
                {
                    cmdSelect.CommandText += " AND ISNULL(enquiry.isConfirmed,0) = " + IsOrderOREnq;
                }

                if (isGetPendSaudaToClose == 1)
                {
                    cmdSelect.CommandText += " AND ISNULL(enquiry.isConvertToSauda,0) = 1 ";
                }
                else
                {
                    cmdSelect.CommandText += " AND ISNULL(enquiry.isConvertToSauda,0) = 0 ";
                }


                cmdSelect.CommandText += "ORDER BY enquiry.idPurchaseEnquiry DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader);
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

        public Int32 SelectMaxEnquiryNo(Int32 finYear, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            Int32 maxEnquiryNo = 0;
            try
            {
                cmdSelect.CommandText = " Select MAX(enqNo) as maxEnqNo from tblPurchaseEnquiry where finYear=" + finYear;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                while (sqlReader.Read())
                {
                    if (sqlReader["maxEnqNo"] != DBNull.Value)
                        maxEnquiryNo = Convert.ToInt32(sqlReader["maxEnqNo"].ToString());
                }

                sqlReader.Dispose();
                return maxEnquiryNo + 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public List<TblOrganizationTO> ConvertDTToListNew(SqlDataReader tblOrganizationTODT)
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

                    if (tblOrganizationTONew.OrgTypeE == Constants.OrgTypeE.PURCHASE_COMPETITOR)
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

                        //if (tblOrganizationTODT["prodCapacityMT"] != DBNull.Value)
                        //    tblOrganizationTONew.CompetitorUpdatesTO.ProdCapacityMT = Convert.ToDouble(tblOrganizationTODT["prodCapacityMT"].ToString());
                        if (tblOrganizationTODT["IdPurcompetitorExt"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.CompetitorExtId = Convert.ToInt32(tblOrganizationTODT["IdPurcompetitorExt"].ToString());
                        if (tblOrganizationTODT["competitorOrgId"] != DBNull.Value)
                            tblOrganizationTONew.CompetitorUpdatesTO.CompetitorOrgId = Convert.ToInt32(tblOrganizationTODT["competitorOrgId"].ToString());


                    }

                    //if (tblOrganizationTODT["overdue_ref_id"] != DBNull.Value)
                    //    tblOrganizationTONew.OverdueRefId = Convert.ToString(tblOrganizationTODT["overdue_ref_id"].ToString());

                    //if (tblOrganizationTODT["enq_ref_id"] != DBNull.Value)
                    //    tblOrganizationTONew.EnqRefId = Convert.ToString(tblOrganizationTODT["enq_ref_id"].ToString());

                    tblOrganizationTOList.Add(tblOrganizationTONew);
                }
            }
            return tblOrganizationTOList;
        }
        public List<TblPurchaseEnquiryTO> ConvertDTToList(SqlDataReader tblPurchaseEnquiryTODT, Boolean isPMDisplay = true)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = new List<TblPurchaseEnquiryTO>();
            if (tblPurchaseEnquiryTODT != null)
            {
                while (tblPurchaseEnquiryTODT.Read())
                {
                    TblPurchaseEnquiryTO tblPurchaseEnquiryTONew = new TblPurchaseEnquiryTO();
                    if (tblPurchaseEnquiryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryTODT["idPurchaseEnquiry"].ToString());
                    if (tblPurchaseEnquiryTODT["userId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UserId = Convert.ToInt32(tblPurchaseEnquiryTODT["userId"].ToString());
                    if (tblPurchaseEnquiryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.COrNCId = Convert.ToInt32(tblPurchaseEnquiryTODT["cOrNCId"].ToString());
                    if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SupplierId = Convert.ToInt32(tblPurchaseEnquiryTODT["supplierId"].ToString());
                    if (tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandDeclarationPurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["isConfirmed"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsConfirmed = Convert.ToInt32(tblPurchaseEnquiryTODT["isConfirmed"].ToString());
                    if (tblPurchaseEnquiryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ProdClassId = Convert.ToInt32(tblPurchaseEnquiryTODT["prodClassId"].ToString());
                    if (tblPurchaseEnquiryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseEnquiryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseEnquiryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["padta"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Padta = Convert.ToDouble(tblPurchaseEnquiryTODT["padta"].ToString());
                    if (tblPurchaseEnquiryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["updatedOn"].ToString());
                    if (tblPurchaseEnquiryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Comments = Convert.ToString(tblPurchaseEnquiryTODT["comments"].ToString());
                    if (tblPurchaseEnquiryTODT["isConvertToSauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsConvertToSauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isConvertToSauda"].ToString());
                    if (tblPurchaseEnquiryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ProdClassDesc = Convert.ToString(tblPurchaseEnquiryTODT["prodClassDesc"].ToString());
                    if (tblPurchaseEnquiryTODT["stateId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StateId = Convert.ToInt32(tblPurchaseEnquiryTODT["stateId"].ToString());
                    //Not showing rate band cost on enquiry view so add this code
                    if (tblPurchaseEnquiryTODT["rate_band_costing"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandCosting = Convert.ToDouble(tblPurchaseEnquiryTODT["rate_band_costing"].ToString());

                    //if (tblPurchaseEnquiryTODT["vehicleSpotEntryId"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.VehicleSpotEntryId = Convert.ToInt32(tblPurchaseEnquiryTODT["vehicleSpotEntryId"].ToString());

                    if (isPMDisplay)
                    {
                        if (tblPurchaseEnquiryTODT["supplierName"] != DBNull.Value)
                            tblPurchaseEnquiryTONew.SupplierName = Convert.ToString(tblPurchaseEnquiryTODT["supplierName"].ToString());
                        if (tblPurchaseEnquiryTODT["purchaseManagerName"] != DBNull.Value)
                            tblPurchaseEnquiryTONew.PurchaseManagerName = Convert.ToString(tblPurchaseEnquiryTODT["purchaseManagerName"].ToString());
                        if (tblPurchaseEnquiryTODT["statusName"] != DBNull.Value)
                            tblPurchaseEnquiryTONew.StatusName = Convert.ToString(tblPurchaseEnquiryTODT["statusName"].ToString());
                    }

                    if (tblPurchaseEnquiryTODT["demandedRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DemandedRate = Convert.ToDouble(tblPurchaseEnquiryTODT["demandedRate"].ToString());

                    if (tblPurchaseEnquiryTODT["authReasons"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AuthReasons = Convert.ToString(tblPurchaseEnquiryTODT["authReasons"].ToString());

                    if (tblPurchaseEnquiryTODT["isOpenQtySauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsOpenQtySauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isOpenQtySauda"].ToString());

                    if (tblPurchaseEnquiryTODT["pendingBookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PendingBookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["pendingBookingQty"].ToString());

                    //Prajakta[2018-11-20] Added
                    if (tblPurchaseEnquiryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseEnquiryTODT["enqDisplayNo"].ToString());

                    if (tblPurchaseEnquiryTODT["finYear"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinYear = Convert.ToInt32(tblPurchaseEnquiryTODT["finYear"].ToString());

                    if (tblPurchaseEnquiryTODT["enqNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqNo = Convert.ToInt32(tblPurchaseEnquiryTODT["enqNo"].ToString());

                    if (tblPurchaseEnquiryTODT["isSpotedVehicle"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsSpotedVehicle = Convert.ToInt32(tblPurchaseEnquiryTODT["isSpotedVehicle"].ToString());

                    if (tblPurchaseEnquiryTODT["saudaCreatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SaudaCreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["saudaCreatedOn"].ToString());

                    //Priyanka [03-01-2019]
                    if (tblPurchaseEnquiryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Comments = Convert.ToString(tblPurchaseEnquiryTODT["comments"].ToString());
                    //if (tblPurchaseEnquiryTODT["cnfName"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.PurchaseManagerName = Convert.ToString(tblPurchaseEnquiryTODT["cnfName"].ToString());
                    //if (tblPurchaseEnquiryTODT["dealerName"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.SupplierName = Convert.ToString(tblPurchaseEnquiryTODT["dealerName"].ToString());
                    //if (tblPurchaseEnquiryTODT["statusName"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.Status = Convert.ToString(tblPurchaseEnquiryTODT["statusName"].ToString());
                    if (tblPurchaseEnquiryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryTODT["idPurchaseEnquiry"].ToString());
                    if (tblPurchaseEnquiryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.GlobalRateId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["SupplierId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DealerOrgId = Convert.ToInt32(tblPurchaseEnquiryTODT["SupplierId"].ToString());
                    //Need To check....
                    if (tblPurchaseEnquiryTODT["userId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CnFOrgId = Convert.ToInt32(tblPurchaseEnquiryTODT["userId"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingPmRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Bookingpmrate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingPmRate"].ToString());

                    if (tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandDeclarationPurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"].ToString());

                    if (tblPurchaseEnquiryTODT["demandedRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DemandedRate = Convert.ToDouble(tblPurchaseEnquiryTODT["demandedRate"].ToString());

                    if (tblPurchaseEnquiryTODT["authReasons"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AuthReasons = Convert.ToString(tblPurchaseEnquiryTODT["authReasons"].ToString());

                    //Prajakta[2018-11-20] Added
                    if (tblPurchaseEnquiryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseEnquiryTODT["enqDisplayNo"].ToString());

                    if (tblPurchaseEnquiryTODT["finYear"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinYear = Convert.ToInt32(tblPurchaseEnquiryTODT["finYear"].ToString());

                    if (tblPurchaseEnquiryTODT["enqNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqNo = Convert.ToInt32(tblPurchaseEnquiryTODT["enqNo"].ToString());

                    if (tblPurchaseEnquiryTODT["deliveryDays"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DeliveryDays = Convert.ToInt32(tblPurchaseEnquiryTODT["deliveryDays"].ToString());

                    if (tblPurchaseEnquiryTODT["noOfVehicleSched"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.NoOfVehicleSched = Convert.ToInt32(tblPurchaseEnquiryTODT["noOfVehicleSched"].ToString());

                    if (tblPurchaseEnquiryTODT["remark"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Remark = Convert.ToString(tblPurchaseEnquiryTODT["remark"].ToString());

                    if (tblPurchaseEnquiryTODT["freight"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Freight = Convert.ToDouble(tblPurchaseEnquiryTODT["freight"].ToString());

                    if (tblPurchaseEnquiryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsFixed = Convert.ToInt32(tblPurchaseEnquiryTODT["isFixed"].ToString());

                    if (tblPurchaseEnquiryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseEnquiryTODT["transportAmtPerMT"].ToString());

                    if (tblPurchaseEnquiryTODT["rateForC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateForC = Convert.ToDouble(tblPurchaseEnquiryTODT["rateForC"].ToString());

                    if (tblPurchaseEnquiryTODT["rateForNC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateForNC = Convert.ToDouble(tblPurchaseEnquiryTODT["rateForNC"].ToString());


                    

                    if ( tblPurchaseEnquiryTODT["createdByName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedByName = Convert.ToString(tblPurchaseEnquiryTODT["createdByName"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedByName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedByName = Convert.ToString(tblPurchaseEnquiryTODT["updatedByName"].ToString());

                    //Prajakta[2019-08-01] Added
                    if (tblPurchaseEnquiryTODT["consumedQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ConsumedQty = Convert.ToDouble(tblPurchaseEnquiryTODT["consumedQty"].ToString());

                    if (tblPurchaseEnquiryTODT["optionalPendingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.OptionalPendingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["optionalPendingQty"].ToString());


                    if (tblPurchaseEnquiryTODT["wtRateApprovalDiff"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.WtRateApprovalDiff = Convert.ToDouble(tblPurchaseEnquiryTODT["wtRateApprovalDiff"].ToString());

                    if (tblPurchaseEnquiryTODT["isAutoSpotVehSauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsAutoSpotVehSauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isAutoSpotVehSauda"].ToString());

                    if (tblPurchaseEnquiryTODT["refRateofV48Var"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RefRateofV48Var = Convert.ToDouble(tblPurchaseEnquiryTODT["refRateofV48Var"].ToString());
                    if (tblPurchaseEnquiryTODT["refRateC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RefRateC = Convert.ToDouble(tblPurchaseEnquiryTODT["refRateC"].ToString());


                    if (tblPurchaseEnquiryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.VehicleTypeDesc = Convert.ToString(tblPurchaseEnquiryTODT["vehicleTypeDesc"].ToString());

                    if (tblPurchaseEnquiryTODT["saudaTypeId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SaudaTypeId = Convert.ToInt32(tblPurchaseEnquiryTODT["saudaTypeId"].ToString());

                    if (tblPurchaseEnquiryTODT["pendNoOfVeh"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PendNoOfVeh = Convert.ToInt32(tblPurchaseEnquiryTODT["pendNoOfVeh"].ToString());

                    if (tblPurchaseEnquiryTODT["currencyId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CurrencyId = Convert.ToInt32(tblPurchaseEnquiryTODT["currencyId"].ToString());

                    if (tblPurchaseEnquiryTODT["currnecyCode"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Currency = tblPurchaseEnquiryTODT["currnecyCode"].ToString();

                    if (tblPurchaseEnquiryTODT["contractTypeId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ContractTypeId = Convert.ToInt32(tblPurchaseEnquiryTODT["contractTypeId"].ToString());
                    if (tblPurchaseEnquiryTODT["contractType"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ContractType = tblPurchaseEnquiryTODT["contractType"].ToString();

                    if (tblPurchaseEnquiryTODT["contractComment"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ContractComment = tblPurchaseEnquiryTODT["contractComment"].ToString();

                    if (tblPurchaseEnquiryTODT["contractDate"] != DBNull.Value)
                    {
                        tblPurchaseEnquiryTONew.ContractDate = Convert.ToDateTime(tblPurchaseEnquiryTODT["contractDate"].ToString());
                        tblPurchaseEnquiryTONew.ContractDateStr = tblPurchaseEnquiryTONew.ContractDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }

                    if (tblPurchaseEnquiryTODT["contractNumber"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ContractNumber = tblPurchaseEnquiryTODT["contractNumber"].ToString();
                    if (tblPurchaseEnquiryTODT["countryOfOrigin"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CountryOfOrigin = tblPurchaseEnquiryTODT["countryOfOrigin"].ToString();
                    if (tblPurchaseEnquiryTODT["portOfLoading"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PortOfLoading = tblPurchaseEnquiryTODT["portOfLoading"].ToString();
                    if (tblPurchaseEnquiryTODT["portOfDischarge"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PortOfDischarge = tblPurchaseEnquiryTODT["portOfDischarge"].ToString();
                    if (tblPurchaseEnquiryTODT["finalPlaceOfDelivery"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinalPlaceOfDelivery = tblPurchaseEnquiryTODT["finalPlaceOfDelivery"].ToString();
                    if (tblPurchaseEnquiryTODT["weighmentTolerance"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.WeighmentTolerance = Convert.ToInt32(tblPurchaseEnquiryTODT["weighmentTolerance"].ToString());
                    if (tblPurchaseEnquiryTODT["impuritiesTolerance"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ImpuritiesTolerance = Convert.ToInt32(tblPurchaseEnquiryTODT["impuritiesTolerance"].ToString());
                    if (tblPurchaseEnquiryTODT["weighmentToleranceStr"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.WeighmentToleranceStr = tblPurchaseEnquiryTODT["weighmentToleranceStr"].ToString();

                    if (tblPurchaseEnquiryTODT["indentureName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IndentureName = tblPurchaseEnquiryTODT["indentureName"].ToString();

                    if (tblPurchaseEnquiryTODT["impuritiesToleranceStr"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ImpuritiesToleranceStr = tblPurchaseEnquiryTODT["impuritiesToleranceStr"].ToString();

                    if (tblPurchaseEnquiryTODT["averageLoading"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AverageLoading = Convert.ToDouble(tblPurchaseEnquiryTODT["averageLoading"].ToString());
                   
                    if (tblPurchaseEnquiryTONew.SaudaTypeId > 0)
                    {
                        if(tblPurchaseEnquiryTONew.SaudaTypeId == Convert.ToInt32(Constants.SaudaTypeE.TONNAGE_QTY))
                        {
                            tblPurchaseEnquiryTONew.SaudaTypeDesc = Constants.SaudaTypeDescE.TONNAGE_QTY.ToString();
                        }
                        else if(tblPurchaseEnquiryTONew.SaudaTypeId == Convert.ToInt32(Constants.SaudaTypeE.NO_OF_VEHICLES))
                        {
                            tblPurchaseEnquiryTONew.SaudaTypeDesc = Constants.SaudaTypeDescE.NO_OF_VEHICLES.ToString();
                        }
                    }

                        tblPurchaseEnquiryTOList.Add(tblPurchaseEnquiryTONew);
                }
            }
            return tblPurchaseEnquiryTOList;
        }

        public List<TblPurchaseEnquiryTO> ConvertDTToListNew(SqlDataReader tblPurchaseEnquiryTODT, Boolean isPMDisplay = true)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = new List<TblPurchaseEnquiryTO>();
            if (tblPurchaseEnquiryTODT != null)
            {
                while (tblPurchaseEnquiryTODT.Read())
                {
                    TblPurchaseEnquiryTO tblPurchaseEnquiryTONew = new TblPurchaseEnquiryTO();
                    if (tblPurchaseEnquiryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryTODT["idPurchaseEnquiry"].ToString());
                    if (tblPurchaseEnquiryTODT["userId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UserId = Convert.ToInt32(tblPurchaseEnquiryTODT["userId"].ToString());
                    if (tblPurchaseEnquiryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.COrNCId = Convert.ToInt32(tblPurchaseEnquiryTODT["cOrNCId"].ToString());
                    if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SupplierId = Convert.ToInt32(tblPurchaseEnquiryTODT["supplierId"].ToString());
                    if (tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandDeclarationPurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["isConfirmed"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsConfirmed = Convert.ToInt32(tblPurchaseEnquiryTODT["isConfirmed"].ToString());
                    if (tblPurchaseEnquiryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ProdClassId = Convert.ToInt32(tblPurchaseEnquiryTODT["prodClassId"].ToString());
                    if (tblPurchaseEnquiryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseEnquiryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseEnquiryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["padta"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Padta = Convert.ToDouble(tblPurchaseEnquiryTODT["padta"].ToString());
                    if (tblPurchaseEnquiryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["updatedOn"].ToString());
                    if (tblPurchaseEnquiryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Comments = Convert.ToString(tblPurchaseEnquiryTODT["comments"].ToString());
                    if (tblPurchaseEnquiryTODT["isConvertToSauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsConvertToSauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isConvertToSauda"].ToString());
                    if (tblPurchaseEnquiryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ProdClassDesc = Convert.ToString(tblPurchaseEnquiryTODT["prodClassDesc"].ToString());
                    if (tblPurchaseEnquiryTODT["stateId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StateId = Convert.ToInt32(tblPurchaseEnquiryTODT["stateId"].ToString());
                    //Not showing rate band cost on enquiry view so add this code
                    if (tblPurchaseEnquiryTODT["rate_band_costing"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandCosting = Convert.ToDouble(tblPurchaseEnquiryTODT["rate_band_costing"].ToString());

                    //if (tblPurchaseEnquiryTODT["vehicleSpotEntryId"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.VehicleSpotEntryId = Convert.ToInt32(tblPurchaseEnquiryTODT["vehicleSpotEntryId"].ToString());

                    if (isPMDisplay)
                    {
                        if (tblPurchaseEnquiryTODT["supplierName"] != DBNull.Value)
                            tblPurchaseEnquiryTONew.SupplierName = Convert.ToString(tblPurchaseEnquiryTODT["supplierName"].ToString());
                        if (tblPurchaseEnquiryTODT["purchaseManagerName"] != DBNull.Value)
                            tblPurchaseEnquiryTONew.PurchaseManagerName = Convert.ToString(tblPurchaseEnquiryTODT["purchaseManagerName"].ToString());
                        if (tblPurchaseEnquiryTODT["statusName"] != DBNull.Value)
                            tblPurchaseEnquiryTONew.StatusName = Convert.ToString(tblPurchaseEnquiryTODT["statusName"].ToString());
                    }

                    if (tblPurchaseEnquiryTODT["demandedRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DemandedRate = Convert.ToDouble(tblPurchaseEnquiryTODT["demandedRate"].ToString());

                    if (tblPurchaseEnquiryTODT["authReasons"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AuthReasons = Convert.ToString(tblPurchaseEnquiryTODT["authReasons"].ToString());

                    if (tblPurchaseEnquiryTODT["isOpenQtySauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsOpenQtySauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isOpenQtySauda"].ToString());

                    if (tblPurchaseEnquiryTODT["pendingBookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PendingBookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["pendingBookingQty"].ToString());

                    //Prajakta[2018-11-20] Added
                    if (tblPurchaseEnquiryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseEnquiryTODT["enqDisplayNo"].ToString());

                    if (tblPurchaseEnquiryTODT["finYear"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinYear = Convert.ToInt32(tblPurchaseEnquiryTODT["finYear"].ToString());

                    if (tblPurchaseEnquiryTODT["enqNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqNo = Convert.ToInt32(tblPurchaseEnquiryTODT["enqNo"].ToString());

                    if (tblPurchaseEnquiryTODT["isSpotedVehicle"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsSpotedVehicle = Convert.ToInt32(tblPurchaseEnquiryTODT["isSpotedVehicle"].ToString());

                    if (tblPurchaseEnquiryTODT["saudaCreatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SaudaCreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["saudaCreatedOn"].ToString());

                    //Priyanka [03-01-2019]
                    if (tblPurchaseEnquiryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Comments = Convert.ToString(tblPurchaseEnquiryTODT["comments"].ToString());
                    //if (tblPurchaseEnquiryTODT["cnfName"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.PurchaseManagerName = Convert.ToString(tblPurchaseEnquiryTODT["cnfName"].ToString());
                    //if (tblPurchaseEnquiryTODT["dealerName"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.SupplierName = Convert.ToString(tblPurchaseEnquiryTODT["dealerName"].ToString());
                    //if (tblPurchaseEnquiryTODT["statusName"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.Status = Convert.ToString(tblPurchaseEnquiryTODT["statusName"].ToString());
                    if (tblPurchaseEnquiryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryTODT["idPurchaseEnquiry"].ToString());
                    if (tblPurchaseEnquiryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.GlobalRateId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["SupplierId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DealerOrgId = Convert.ToInt32(tblPurchaseEnquiryTODT["SupplierId"].ToString());
                    //Need To check....
                    if (tblPurchaseEnquiryTODT["userId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CnFOrgId = Convert.ToInt32(tblPurchaseEnquiryTODT["userId"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingPmRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Bookingpmrate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingPmRate"].ToString());

                    if (tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandDeclarationPurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"].ToString());

                    if (tblPurchaseEnquiryTODT["demandedRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DemandedRate = Convert.ToDouble(tblPurchaseEnquiryTODT["demandedRate"].ToString());

                    if (tblPurchaseEnquiryTODT["authReasons"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AuthReasons = Convert.ToString(tblPurchaseEnquiryTODT["authReasons"].ToString());

                    //Prajakta[2018-11-20] Added
                    if (tblPurchaseEnquiryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseEnquiryTODT["enqDisplayNo"].ToString());

                    if (tblPurchaseEnquiryTODT["finYear"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinYear = Convert.ToInt32(tblPurchaseEnquiryTODT["finYear"].ToString());

                    if (tblPurchaseEnquiryTODT["enqNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqNo = Convert.ToInt32(tblPurchaseEnquiryTODT["enqNo"].ToString());

                    if (tblPurchaseEnquiryTODT["deliveryDays"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DeliveryDays = Convert.ToInt32(tblPurchaseEnquiryTODT["deliveryDays"].ToString());

                    if (tblPurchaseEnquiryTODT["noOfVehicleSched"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.NoOfVehicleSched = Convert.ToInt32(tblPurchaseEnquiryTODT["noOfVehicleSched"].ToString());

                    if (tblPurchaseEnquiryTODT["remark"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Remark = Convert.ToString(tblPurchaseEnquiryTODT["remark"].ToString());

                    if (tblPurchaseEnquiryTODT["freight"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Freight = Convert.ToDouble(tblPurchaseEnquiryTODT["freight"].ToString());

                    if (tblPurchaseEnquiryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsFixed = Convert.ToInt32(tblPurchaseEnquiryTODT["isFixed"].ToString());

                    if (tblPurchaseEnquiryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseEnquiryTODT["transportAmtPerMT"].ToString());

                    if (tblPurchaseEnquiryTODT["rateForC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateForC = Convert.ToDouble(tblPurchaseEnquiryTODT["rateForC"].ToString());

                    if (tblPurchaseEnquiryTODT["rateForNC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateForNC = Convert.ToDouble(tblPurchaseEnquiryTODT["rateForNC"].ToString());




                    //if (tblPurchaseEnquiryTODT["createdByName"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.CreatedByName = Convert.ToString(tblPurchaseEnquiryTODT["createdByName"].ToString());
                    //if (tblPurchaseEnquiryTODT["updatedByName"] != DBNull.Value)
                    //    tblPurchaseEnquiryTONew.UpdatedByName = Convert.ToString(tblPurchaseEnquiryTODT["updatedByName"].ToString());

                    //Prajakta[2019-08-01] Added
                    if (tblPurchaseEnquiryTODT["consumedQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ConsumedQty = Convert.ToDouble(tblPurchaseEnquiryTODT["consumedQty"].ToString());

                    if (tblPurchaseEnquiryTODT["optionalPendingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.OptionalPendingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["optionalPendingQty"].ToString());


                    if (tblPurchaseEnquiryTODT["wtRateApprovalDiff"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.WtRateApprovalDiff = Convert.ToDouble(tblPurchaseEnquiryTODT["wtRateApprovalDiff"].ToString());

                    if (tblPurchaseEnquiryTODT["isAutoSpotVehSauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsAutoSpotVehSauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isAutoSpotVehSauda"].ToString());

                    if (tblPurchaseEnquiryTODT["refRateofV48Var"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RefRateofV48Var = Convert.ToDouble(tblPurchaseEnquiryTODT["refRateofV48Var"].ToString());
                    if (tblPurchaseEnquiryTODT["refRateC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RefRateC = Convert.ToDouble(tblPurchaseEnquiryTODT["refRateC"].ToString());


                    if (tblPurchaseEnquiryTODT["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.VehicleTypeDesc = Convert.ToString(tblPurchaseEnquiryTODT["vehicleTypeDesc"].ToString());

                    if (tblPurchaseEnquiryTODT["saudaTypeId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SaudaTypeId = Convert.ToInt32(tblPurchaseEnquiryTODT["saudaTypeId"].ToString());


                    tblPurchaseEnquiryTOList.Add(tblPurchaseEnquiryTONew);
                }
            }
            return tblPurchaseEnquiryTOList;
        }


        public List<TblPurchaseEnquiryTO> ConvertDTToListForSaudaReport(SqlDataReader tblPurchaseEnquiryTODT, Boolean isPMDisplay = true)
        {
            List<TblPurchaseEnquiryTO> tblPurchaseEnquiryTOList = new List<TblPurchaseEnquiryTO>();
            if (tblPurchaseEnquiryTODT != null)
            {
                while (tblPurchaseEnquiryTODT.Read())
                {
                    TblPurchaseEnquiryTO tblPurchaseEnquiryTONew = new TblPurchaseEnquiryTO();
                    if (tblPurchaseEnquiryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryTODT["idPurchaseEnquiry"].ToString());
                    if (tblPurchaseEnquiryTODT["userId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UserId = Convert.ToInt32(tblPurchaseEnquiryTODT["userId"].ToString());
                    if (tblPurchaseEnquiryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.COrNCId = Convert.ToInt32(tblPurchaseEnquiryTODT["cOrNCId"].ToString());
                    if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SupplierId = Convert.ToInt32(tblPurchaseEnquiryTODT["supplierId"].ToString());
                    if (tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandDeclarationPurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["isConfirmed"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsConfirmed = Convert.ToInt32(tblPurchaseEnquiryTODT["isConfirmed"].ToString());
                    if (tblPurchaseEnquiryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ProdClassId = Convert.ToInt32(tblPurchaseEnquiryTODT["prodClassId"].ToString());
                    if (tblPurchaseEnquiryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseEnquiryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseEnquiryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["padta"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Padta = Convert.ToDouble(tblPurchaseEnquiryTODT["padta"].ToString());
                    if (tblPurchaseEnquiryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["updatedOn"].ToString());
                    if (tblPurchaseEnquiryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Comments = Convert.ToString(tblPurchaseEnquiryTODT["comments"].ToString());
                    if (tblPurchaseEnquiryTODT["isConvertToSauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsConvertToSauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isConvertToSauda"].ToString());

                    if (tblPurchaseEnquiryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ProdClassDesc = Convert.ToString(tblPurchaseEnquiryTODT["prodClassDesc"].ToString());
                    if (tblPurchaseEnquiryTODT["supplierName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SupplierName = Convert.ToString(tblPurchaseEnquiryTODT["supplierName"].ToString());
                    if (tblPurchaseEnquiryTODT["purchaseManagerName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PurchaseManagerName = Convert.ToString(tblPurchaseEnquiryTODT["purchaseManagerName"].ToString());

                    if (tblPurchaseEnquiryTODT["demandedRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DemandedRate = Convert.ToDouble(tblPurchaseEnquiryTODT["demandedRate"].ToString());

                    if (tblPurchaseEnquiryTODT["authReasons"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AuthReasons = Convert.ToString(tblPurchaseEnquiryTODT["authReasons"].ToString());

                    if (tblPurchaseEnquiryTODT["isOpenQtySauda"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsOpenQtySauda = Convert.ToInt32(tblPurchaseEnquiryTODT["isOpenQtySauda"].ToString());

                    if (tblPurchaseEnquiryTODT["pendingBookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.PendingBookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["pendingBookingQty"].ToString());

                    //Prajakta[2018-11-20] Added
                    if (tblPurchaseEnquiryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseEnquiryTODT["enqDisplayNo"].ToString());

                    if (tblPurchaseEnquiryTODT["finYear"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinYear = Convert.ToInt32(tblPurchaseEnquiryTODT["finYear"].ToString());

                    if (tblPurchaseEnquiryTODT["enqNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqNo = Convert.ToInt32(tblPurchaseEnquiryTODT["enqNo"].ToString());

                    if (tblPurchaseEnquiryTODT["isSpotedVehicle"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsSpotedVehicle = Convert.ToInt32(tblPurchaseEnquiryTODT["isSpotedVehicle"].ToString());

                    if (tblPurchaseEnquiryTODT["saudaCreatedOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.SaudaCreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["saudaCreatedOn"].ToString());

                    //Priyanka [03-01-2019]
                    if (tblPurchaseEnquiryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseEnquiryTODT["updatedBy"].ToString());
                    if (tblPurchaseEnquiryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Comments = Convert.ToString(tblPurchaseEnquiryTODT["comments"].ToString());

                    if (tblPurchaseEnquiryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryTODT["idPurchaseEnquiry"].ToString());
                    if (tblPurchaseEnquiryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.GlobalRateId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryTODT["SupplierId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DealerOrgId = Convert.ToInt32(tblPurchaseEnquiryTODT["SupplierId"].ToString());
                    //Need To check....
                    if (tblPurchaseEnquiryTODT["userId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.CnFOrgId = Convert.ToInt32(tblPurchaseEnquiryTODT["userId"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryTODT["bookingPmRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Bookingpmrate = Convert.ToDouble(tblPurchaseEnquiryTODT["bookingPmRate"].ToString());

                    if (tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateBandDeclarationPurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["rateBandDeclarationPurchaseId"].ToString());

                    if (tblPurchaseEnquiryTODT["demandedRate"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DemandedRate = Convert.ToDouble(tblPurchaseEnquiryTODT["demandedRate"].ToString());

                    if (tblPurchaseEnquiryTODT["authReasons"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.AuthReasons = Convert.ToString(tblPurchaseEnquiryTODT["authReasons"].ToString());

                    //Prajakta[2018-11-20] Added
                    if (tblPurchaseEnquiryTODT["enqDisplayNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseEnquiryTODT["enqDisplayNo"].ToString());

                    if (tblPurchaseEnquiryTODT["finYear"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.FinYear = Convert.ToInt32(tblPurchaseEnquiryTODT["finYear"].ToString());

                    if (tblPurchaseEnquiryTODT["enqNo"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.EnqNo = Convert.ToInt32(tblPurchaseEnquiryTODT["enqNo"].ToString());

                    if (tblPurchaseEnquiryTODT["deliveryDays"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.DeliveryDays = Convert.ToInt32(tblPurchaseEnquiryTODT["deliveryDays"].ToString());

                    if (tblPurchaseEnquiryTODT["noOfVehicleSched"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.NoOfVehicleSched = Convert.ToInt32(tblPurchaseEnquiryTODT["noOfVehicleSched"].ToString());

                    if (tblPurchaseEnquiryTODT["remark"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Remark = Convert.ToString(tblPurchaseEnquiryTODT["remark"].ToString());

                    if (tblPurchaseEnquiryTODT["StatusName"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.StatusName = Convert.ToString(tblPurchaseEnquiryTODT["StatusName"].ToString());

                    if (tblPurchaseEnquiryTODT["freight"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.Freight = Convert.ToDouble(tblPurchaseEnquiryTODT["freight"].ToString());

                    if (tblPurchaseEnquiryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.IsFixed = Convert.ToInt32(tblPurchaseEnquiryTODT["isFixed"].ToString());

                    if (tblPurchaseEnquiryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseEnquiryTODT["transportAmtPerMT"].ToString());

                    if (tblPurchaseEnquiryTODT["rateForC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateForC = Convert.ToDouble(tblPurchaseEnquiryTODT["rateForC"].ToString());

                    if (tblPurchaseEnquiryTODT["rateForNC"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.RateForNC = Convert.ToDouble(tblPurchaseEnquiryTODT["rateForNC"].ToString());

                    if (tblPurchaseEnquiryTODT["consumedQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.ConsumedQty = Convert.ToDouble(tblPurchaseEnquiryTODT["consumedQty"].ToString());

                    if (tblPurchaseEnquiryTODT["optionalPendingQty"] != DBNull.Value)
                        tblPurchaseEnquiryTONew.OptionalPendingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["optionalPendingQty"].ToString());


                    tblPurchaseEnquiryTOList.Add(tblPurchaseEnquiryTONew);
                }
            }
            return tblPurchaseEnquiryTOList;
        }


        //Priyanka [03-01-2019]
        //public  List<TblPurchaseEnquiryTO> SelectAllBookingsListForAcceptance(Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    String statusIds = (Int32)Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL + "";//24 for director approval
        //    String areConfJoin = String.Empty;
        //    int isConfEn = 0;
        //    int userId = 0;
        //    if (tblUserRoleTO != null)
        //    {
        //        isConfEn = tblUserRoleTO.EnableAreaAlloc;
        //        userId = tblUserRoleTO.UserId;
        //    }
        //    try
        //    {
        //        conn.Open();

        //        if (cnfId == 0)
        //            cmdSelect.CommandText = SqlSelectQuery() + " WHERE statusId IN(" + statusIds + ")";
        //        else
        //            cmdSelect.CommandText = SqlSelectQuery() + " WHERE statusId IN(" + statusIds + ")" + " AND bookings.userId=" + cnfId;

        //        cmdSelect.CommandText += "ORDER BY bookings.idPurchaseEnquiry DESC";
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblPurchaseEnquiryTO> list = ConvertDTToList(sqlReader);
        //        sqlReader.Dispose();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        public TblPurchaseEnquiryTO SelectTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseEnquiry = " + idBooking + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(reader);
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
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblPurchaseEnquiryTO SelectTblBookings(Int32 idBooking)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBooking = " + idBooking + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToList(reader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }



        public BookingInfo SelectBookingDashboardInfo(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string whereCond = string.Empty;
            string areConfJoin = string.Empty;
            SqlDataReader tblBookingsTODT = null;
            int isConfEn = 0;
            int userId = 0;
            string statusIds = string.Empty;
            string ids = string.Empty;
            string defaultPMRoleIds = string.Empty;
            List<Int32> pmRoleIds = new List<Int32>();
            // Int32[] pmRoleIds=[];

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_DASHBOARD_ENQ_QTY_STATUS);
            if (tblConfigParamsTO != null)
            {
                ids = tblConfigParamsTO.ConfigParamVal;
            }
            if (!String.IsNullOrEmpty(ids))
            {
                statusIds = " AND statusId IN (" + ids + ") ";
            }

            //Prajakta [2018 dec 12] Added
            // pmRoleIds = BL.TblConfigParamsBL.SelectDefaultPmRoleIds();
            // if (pmRoleIds.Count > 0)
            // {
            //     List<Int32> result = pmRoleIds.Where(a => a.ToString() == tblUserRoleTO.RoleId.ToString()).ToList();
            //     if (result.Count > 0)
            //     {
            //         whereCond = " AND userId IN (" + orgId + ")";
            //     }

            // }


            if (!String.IsNullOrEmpty(orgId))
            {
                whereCond = " AND userId IN (" + orgId + ")";
            }

            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                conn.Open();

                // if (tblUserRoleTO.RoleId == (int)Constants.SystemRolesE.PURCHASE_MANAGER)
                // {
                //     whereCond = " AND userId=" + orgId;
                // }

                // if (isConfEn == 1)
                // {
                //     areConfJoin = " INNER JOIN " +
                //                  " ( " +
                //                  "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                //                  "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                //                  "   INNER JOIN " +
                //                  "   ( " +
                //                  "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                //                  "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                //                  "  ) addrDtl  ON idOrganization = organizationId " +
                //                  "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                //                  "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                //                  " ) AS userAreaDealer On userAreaDealer.cnfOrgId = tblBookings.cnFOrgid AND tblBookings.dealerOrgId = userAreaDealer.idOrganization ";
                // }


                cmdSelect.CommandText = " SELECT SUM(bookingQty) bookingQty, count(idPurchaseEnquiry) totalCost," +
                                         //"sum(COST)/SUM(bookingQty) avgPrice " +
                                         "sum(COST)/ NULLIF(SUM(bookingQty),0) avgPrice " +
                                        " FROM " +
                                        " ( " +
                                        " SELECT idPurchaseEnquiry,bookingQty, bookingRate, (bookingQty * bookingRate) AS cost FROM tblPurchaseEnquiry " +// + areConfJoin +
                                        " WHERE  DAY(createdOn) = " + date.Day + " AND MONTH(createdOn) = " + date.Month + " AND YEAR(createdOn) = " + date.Year + statusIds +// + whereCond +
                                        " AND isConvertToSauda != '1'" + whereCond +                                                                                                                        //" AND statusId IN(2,3,9,11) AND globalRatePurchaseId = (SELECT TOP 1 idGlobalRatePurchase FROM tblGlobalRatePurchase ORDER BY createdOn DESC )" +
                                        " ) AS qryRes";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblBookingsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (tblBookingsTODT != null)
                {
                    while (tblBookingsTODT.Read())
                    {
                        BookingInfo tblBookingsTONew = new BookingInfo();
                        if (tblBookingsTODT["bookingQty"] != DBNull.Value)
                            tblBookingsTONew.BookedQty = Convert.ToDouble(tblBookingsTODT["bookingQty"].ToString());
                        if (tblBookingsTODT["avgPrice"] != DBNull.Value)
                            tblBookingsTONew.AvgPrice = Convert.ToDouble(tblBookingsTODT["avgPrice"].ToString());
                        if (tblBookingsTODT["totalCost"] != DBNull.Value)
                            tblBookingsTONew.Bookedcount = Convert.ToDouble(tblBookingsTODT["totalCost"].ToString());

                        return tblBookingsTONew;
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
                if (tblBookingsTODT != null)
                    tblBookingsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<BookingInfo> SelectMaterialWiseEnqOrSaudaInfoForDashboard(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date,Int32 isConvertToSauda)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string whereCond = string.Empty;
            SqlDataReader tblBookingsTODT = null;
            string statusIds = string.Empty;
            string ids = string.Empty;
            string isConvertToSaudaCond = string.Empty;
            List<BookingInfo> bookingInfoList = new List<BookingInfo>();

            TblConfigParamsTO tblConfigParamsTO = new TblConfigParamsTO();

            isConvertToSaudaCond = " ISNULL(isConvertToSauda,0) = 0 ";

            tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_DASHBOARD_ENQ_QTY_STATUS);
            if (tblConfigParamsTO != null)
            {
                ids = tblConfigParamsTO.ConfigParamVal;
            }

            if (isConvertToSauda == 1)
            {
                tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_DASHBOARD_SAUDA_QTY_STATUS);
                if (tblConfigParamsTO != null)
                {
                    ids = tblConfigParamsTO.ConfigParamVal;
                }

                isConvertToSaudaCond = " ISNULL(isConvertToSauda,0) = 1 ";
            }
         
            if (!String.IsNullOrEmpty(ids))
            {
                statusIds = " AND statusId IN (" + ids + ") ";
            }

            if (!String.IsNullOrEmpty(orgId))
            {
                whereCond = " AND userId IN (" + orgId + ")";
            }

            try
            {
                conn.Open();

                //cmdSelect.CommandText = " SELECT SUM(bookingQty) bookingQty, count(idPurchaseEnquiry) totalCost," +
                //                         //"sum(COST)/SUM(bookingQty) avgPrice " +
                //                         "sum(COST)/ NULLIF(SUM(bookingQty),0) avgPrice " +
                //                        " FROM " +
                //                        " ( " +
                //                        " SELECT idPurchaseEnquiry,bookingQty, bookingRate, (bookingQty * bookingRate) AS cost FROM tblPurchaseEnquiry " +// + areConfJoin +
                //                        " WHERE  DAY(createdOn) = " + date.Day + " AND MONTH(createdOn) = " + date.Month + " AND YEAR(createdOn) = " + date.Year + statusIds +// + whereCond +
                //                        " AND isConvertToSauda != '1'" + whereCond +                                                                                                                        //" AND statusId IN(2,3,9,11) AND globalRatePurchaseId = (SELECT TOP 1 idGlobalRatePurchase FROM tblGlobalRatePurchase ORDER BY createdOn DESC )" +
                //                        " ) AS qryRes";


                //Deepali added currencyId to show on Dashboard
                cmdSelect.CommandText = " SELECT tblProdClassification.prodClassDesc,currencyId, dimCurrency.currnecyCode, " +
                                        " CASE WHEN tblPurchaseEnquiry.cOrNCId = 1 THEN 'Order' WHEN tblPurchaseEnquiry.cOrNCId = 0 THEN 'Enquiry' ELSE '' END AS bookingType, " +
                                        " COUNT(*) as count,SUM(bookingQty) AS totalQty ," +
                                        " SUM(bookingQty * bookingRate)/ NULLIF(SUM(bookingQty),0) AS avgPrice FROM tblPurchaseEnquiry  tblPurchaseEnquiry " +
                                        " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass = tblPurchaseEnquiry.prodClassId   " +
                                        " LEFT JOIN dimCurrency dimCurrency ON dimCurrency.idCurrency =  tblPurchaseEnquiry.currencyId " +
                                        " WHERE " + isConvertToSaudaCond +
                                        " AND DAY(tblPurchaseEnquiry.createdOn) = " + date.Day + " AND MONTH(tblPurchaseEnquiry.createdOn) = " + date.Month + " AND YEAR(tblPurchaseEnquiry.createdOn) = " + date.Year + statusIds + whereCond +
                                        " GROUP BY prodClassId,currencyId,currnecyCode,corncid,tblProdClassification.prodClassDesc ";


                if(isConvertToSauda == 1)
                {
                    cmdSelect.CommandText = " SELECT tblProdClassification.prodClassDesc,currencyId,dimCurrency.currnecyCode, " +
                                       " CASE WHEN tblPurchaseEnquiry.cOrNCId = 1 THEN 'Order' WHEN tblPurchaseEnquiry.cOrNCId = 0 THEN 'Enquiry' ELSE '' END AS bookingType, " +
                                       " COUNT(*) as count,SUM(bookingQty) AS totalQty ," +
                                       " SUM(bookingQty * bookingRate)/ NULLIF(SUM(bookingQty),0) AS avgPrice FROM tblPurchaseEnquiry  tblPurchaseEnquiry " +
                                       " LEFT JOIN tblProdClassification tblProdClassification ON tblProdClassification.idProdClass = tblPurchaseEnquiry.prodClassId " +
                                       " LEFT JOIN dimCurrency dimCurrency ON dimCurrency.idCurrency =  tblPurchaseEnquiry.currencyId " +
                                       " WHERE " + isConvertToSaudaCond +
                                       " AND DAY(tblPurchaseEnquiry.saudaCreatedOn) = " + date.Day + " AND MONTH(tblPurchaseEnquiry.saudaCreatedOn) = " + date.Month + " AND YEAR(tblPurchaseEnquiry.saudaCreatedOn) = " + date.Year + statusIds + whereCond +
                                       " GROUP BY prodClassId,currencyId,currnecyCode,corncid,tblProdClassification.prodClassDesc ";

                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblBookingsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (tblBookingsTODT != null)
                {
                    while (tblBookingsTODT.Read())
                    {
                        BookingInfo tblBookingsTONew = new BookingInfo();

                        if (tblBookingsTODT["prodClassDesc"] != DBNull.Value)
                            tblBookingsTONew.MaterialType = Convert.ToString(tblBookingsTODT["prodClassDesc"].ToString());

                        if (tblBookingsTODT["bookingType"] != DBNull.Value)
                            tblBookingsTONew.BookingType = Convert.ToString(tblBookingsTODT["bookingType"].ToString());

                        if (tblBookingsTODT["count"] != DBNull.Value)
                            tblBookingsTONew.Bookedcount = Convert.ToDouble(tblBookingsTODT["count"].ToString());

                        if (tblBookingsTODT["totalQty"] != DBNull.Value)
                            tblBookingsTONew.BookedQty = Convert.ToDouble(tblBookingsTODT["totalQty"].ToString());

                        if (tblBookingsTODT["avgPrice"] != DBNull.Value)
                            tblBookingsTONew.AvgPrice = Convert.ToDouble(tblBookingsTODT["avgPrice"].ToString());

                        if (tblBookingsTODT["currencyId"] != DBNull.Value)
                            tblBookingsTONew.CurrencyId = Convert.ToInt32(tblBookingsTODT["currencyId"].ToString());

                        if (tblBookingsTODT["currnecyCode"] != DBNull.Value)
                            tblBookingsTONew.Currency = tblBookingsTODT["currnecyCode"].ToString();

                        bookingInfoList.Add(tblBookingsTONew);
                        
                    }
                }

                return bookingInfoList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblBookingsTODT != null)
                    tblBookingsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public BookingInfo SelectBookingSaudaDashboardInfo(TblUserRoleTO tblUserRoleTO, string orgId, DateTime date)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string whereCond = string.Empty;
            string areConfJoin = string.Empty;
            SqlDataReader tblBookingsTODT = null;
            int isConfEn = 0;
            int userId = 0;
            string statusIds = string.Empty;
            string ids = string.Empty;
            List<Int32> pmRoleIds = new List<Int32>();

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_DASHBOARD_SAUDA_QTY_STATUS);
            if (tblConfigParamsTO != null)
            {
                ids = tblConfigParamsTO.ConfigParamVal;
            }
            if (!String.IsNullOrEmpty(ids))
            {
                statusIds = " AND statusId IN (" + ids + ") ";
            }
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            // //Prajakta [2018 dec 12] Added
            // pmRoleIds = BL.TblConfigParamsBL.SelectDefaultPmRoleIds();
            // if (pmRoleIds.Count > 0)
            // {
            //     List<Int32> result = pmRoleIds.Where(a => a.ToString() == tblUserRoleTO.RoleId.ToString()).ToList();
            //     if (result.Count > 0)
            //     {
            //         whereCond = " AND userId IN (" + orgId + ")";
            //     }

            // }

            if (!String.IsNullOrEmpty(orgId))
            {
                whereCond = " AND userId IN (" + orgId + ")";
            }

            try
            {
                conn.Open();
                //Prajakta [2018 sept 23] Added
                // if (tblUserRoleTO.RoleId == (int)Constants.SystemRolesE.PURCHASE_MANAGER)
                // {
                //     whereCond = " AND userId=" + orgId;
                // }

                //if (isConfEn == 1)
                //{
                //    areConfJoin = " INNER JOIN " +
                //                 " ( " +
                //                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                //                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                //                 "   INNER JOIN " +
                //                 "   ( " +
                //                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                //                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                //                 "  ) addrDtl  ON idOrganization = organizationId " +
                //                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                //                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                //                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = tblBookings.cnFOrgid AND tblBookings.dealerOrgId = userAreaDealer.idOrganization ";
                //}


                cmdSelect.CommandText = " SELECT SUM(bookingQty) bookingQty, count(idPurchaseEnquiry) totalCost," +
                                        "  sum(COST)/ NULLIF(SUM(bookingQty),0) avgPrice " +
                                        //"sum(COST)/SUM(bookingQty) avgPrice " +
                                        " FROM " +
                                        " ( " +
                                        " SELECT idPurchaseEnquiry,bookingQty, bookingRate, (bookingQty * bookingRate) AS cost FROM tblPurchaseEnquiry " +// + areConfJoin +
                                        " WHERE  DAY(createdOn) = " + date.Day + " AND MONTH(createdOn) = " + date.Month + " AND YEAR(createdOn) = " + date.Year + statusIds +
                                        " AND isConvertToSauda = '1' " 
                                        //" and bookingQty > 0 "  //Prajakta[2019-12-24] Commented to get count of sauda having zero qty.
                                         + whereCond +
                                        " ) AS qryRes";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblBookingsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (tblBookingsTODT != null)
                {
                    while (tblBookingsTODT.Read())
                    {
                        BookingInfo tblBookingsTONew = new BookingInfo();
                        if (tblBookingsTODT["bookingQty"] != DBNull.Value)
                            tblBookingsTONew.BookedsaudaQty = Convert.ToDouble(tblBookingsTODT["bookingQty"].ToString());
                        if (tblBookingsTODT["avgPrice"] != DBNull.Value)
                            tblBookingsTONew.AvgsaudaPrice = Convert.ToDouble(tblBookingsTODT["avgPrice"].ToString());
                        if (tblBookingsTODT["totalCost"] != DBNull.Value)
                            tblBookingsTONew.Bookedsaudacount = Convert.ToDouble(tblBookingsTODT["totalCost"].ToString());

                        return tblBookingsTONew;
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
                if (tblBookingsTODT != null)
                    tblBookingsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public BookingInfo SelectTodayRtaeDashboardInfo(TblUserRoleTO tblUserRoleTO, int orgId, DateTime date)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblBookingsTODT = null;
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

                cmdSelect.CommandText = "SELECT rate FROM tblGlobalRatePurchase" +
                                        "  INNER JOIN[dbo].[tblRateBandDeclarationPurchase] Ratedec" +
                                        " ON Ratedec.globalRatePurchaseId=idGlobalRatePurchase" +
                                        " WHERE idGlobalRatePurchase = (select max(idGlobalRatePurchase) from tblGlobalRatePurchase" +
                                        " WHERE DAY(Ratedec.createdOn)= " + date.Day + " AND MONTH(Ratedec.createdOn)= " + date.Month + " AND YEAR(Ratedec.createdOn)= " + date.Year +
                                        " ) GROUP BY rate";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblBookingsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (tblBookingsTODT != null)
                {
                    BookingInfo tblBookingsTONew = new BookingInfo();
                    if (tblBookingsTODT.Read())
                    {
                        if (tblBookingsTODT["rate"] != DBNull.Value)
                            tblBookingsTONew.Rate = Convert.ToDouble(tblBookingsTODT["rate"].ToString());
                        return tblBookingsTONew;
                    }
                    else
                    {
                        tblBookingsTONew.Rate = 0;
                        return tblBookingsTONew;
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
                if (tblBookingsTODT != null)
                    tblBookingsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //[27/08/2020] Harshala added to get Purchase Enquiry total Booking Qty and avg booking rate Rate Wise
        public List<TblPurchaseEnquiryTO> SelectPurchaseEnquiryRateWise(String globalRatePurchaseIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblPurchaseEnquiryTODT = null;
            try
            {
                conn.Open();

                //Prajakta[2020-09-24] Added 
                String materialType = String.Empty;
                String materialTypeCondi = String.Empty;
                //Get local scrap enquiry avg price
                TblConfigParamsTO materialConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_SAUDA_AVG_PRICE_OF_MATERIAL);
                if (materialConfigTO != null)
                {
                    materialType = materialConfigTO.ConfigParamVal;
                }
                if (!String.IsNullOrEmpty(materialType))
                {
                    materialTypeCondi = " AND tblPurchaseEnquiry.prodClassId IN (" + materialType + ") ";
                }

                cmdSelect.CommandText = " SELECT globalRatePurchaseId, SUM(bookingQty) AS totalBookingQty,ROUND((SUM(bookingQty*bookingRate)/SUM(bookingQty)),2) AS avgBookingRate FROM tblPurchaseEnquiry tblPurchaseEnquiry " +
                    "WHERE tblPurchaseEnquiry.globalRatePurchaseId IN (" + globalRatePurchaseIds + ") " + materialTypeCondi +
                    "AND ISNULL(tblPurchaseEnquiry.isConvertToSauda,0)=1  GROUP BY tblPurchaseEnquiry.globalRatePurchaseId";

                //cmdSelect.CommandText = " SELECT globalRatePurchaseId, SUM(bookingQty) AS totalBookingQty,ROUND((SUM(bookingQty*bookingRate)/SUM(bookingQty)),2) AS avgBookingRate FROM tblpurchaseenquiry WHERE globalRatePurchaseId IN (" + globalRatePurchaseIds + ") and ISNULL(isConvertToSauda,0)=1  group by globalRatePurchaseId";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblPurchaseEnquiryTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (tblPurchaseEnquiryTODT != null)
                {
                    List<TblPurchaseEnquiryTO > purchaseEnquiryTOList = new List<TblPurchaseEnquiryTO>();
                    if (tblPurchaseEnquiryTODT!=null)
                    {
                        while (tblPurchaseEnquiryTODT.Read())
                        {
                            TblPurchaseEnquiryTO purchaseEnquiryTONew = new TblPurchaseEnquiryTO();

                            if (tblPurchaseEnquiryTODT["totalBookingQty"] != DBNull.Value)
                                purchaseEnquiryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryTODT["totalBookingQty"].ToString());
                            if (tblPurchaseEnquiryTODT["avgBookingRate"] != DBNull.Value)
                                purchaseEnquiryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryTODT["avgBookingRate"].ToString());
                            if (tblPurchaseEnquiryTODT["globalRatePurchaseId"] != DBNull.Value)
                                purchaseEnquiryTONew.GlobalRatePurchaseId = Convert.ToInt32(tblPurchaseEnquiryTODT["globalRatePurchaseId"].ToString());
                            purchaseEnquiryTOList.Add(purchaseEnquiryTONew);
                        }

                        return purchaseEnquiryTOList;
                    }

                    else
                    {
                        return null;
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
                if (tblPurchaseEnquiryTODT != null)
                    tblPurchaseEnquiryTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseEnquiry(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                int idEnquiry = 0;
                idEnquiry = ExecuteInsertionCommand(tblPurchaseEnquiryTO, cmdInsert);
                if (idEnquiry == 1)
                {
                    cmdInsert = new SqlCommand();
                    cmdInsert.Connection = conn;
                    cmdInsert.Transaction = tran;
                    return ExecuteInsertionCommandForHistory(tblPurchaseEnquiryTO, cmdInsert);

                }
                else
                    return idEnquiry;
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

        public int ExecuteInsertionCommand(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [tblPurchaseEnquiry]( " +
                  " [userId]" +
                  " ,[cOrNCId]" +
                  " ,[globalRatePurchaseId]" +
                  " ,[bookingQty]" +
                  " ,[bookingRate]" +
                  " ,[SupplierId]" +
                  " ,[rateBandDeclarationPurchaseId]" +
                  " ,[isConfirmed]" +
                  " ,[prodClassId]" +
                  " ,[statusId]" +
                  " ,[calculatedMetalCost]" +
                  " ,[baseMetalCost]" +
                  " ,[padta]" +
                  " ,[createdBy]" +
                  " ,[createdOn]" +
                  " ,[updatedBy]" +
                  " ,[updatedOn]" +
                  " ,[comments]" +
                  " ,[isConvertToSauda]" +
                  " ,[pendingBookingQty]" +
                  " ,[demandedRate]" +
                  " ,[authReasons]" +
                  " ,[isOpenQtySauda]" +
                  " ,[enqDisplayNo]" +
                  " ,[finYear]" +
                   " ,[isSpotedVehicle]" +
                  " ,[enqNo]" +
                  " ,[saudaCreatedOn]" +

                  //Priyanka [07-01-2018]
                  " ,[deliveryDays]" +
                  " ,[noOfVehicleSched]" +
                  " ,[remark]" +
                  " ,[freight]" +
                  " ,[isFixed]" +
                  " ,[transportAmtPerMT]" +
                  " ,[rateForC]" +
                  " ,[rateForNC]" +
                  " ,[consumedQty]" +
                  " ,[optionalPendingQty]" +
                  " ,[wtRateApprovalDiff]" +
                  " ,[isAutoSpotVehSauda]" +
                  " ,[refRateofV48Var]" +
                  " ,[refRateC]" +
                  " ,[vehicleTypeDesc]" +
                  " ,[isEnqTransfered]" +
                  " ,[saudaTypeId]" +
                    " ,[pendNoOfVeh]" +
                    //Deepali added for task no 920 and 921
                    " ,[currencyId]" +
                    " ,[contractTypeId]" +
                    " ,[contractComment]" +
                    " ,[contractNumber]" +
                    " ,[contractDate]" +
                    " ,[countryOfOrigin]" +
                    " ,[portOfLoading]" +
                    " ,[portOfDischarge]" +
                    " ,[finalPlaceOfDelivery]" +
                    " ,[weighmentTolerance]" +
                    " ,[impuritiesTolerance]" +
                    " ,[averageLoading]" +
                    " ,[indentureName]" +
                    " )" +
                " VALUES (" +
                            "  @UserId " +
                            " ,@COrNCId " +
                            " ,@GlobalRatePurchaseId " +
                            " ,@BookingQty " +
                            " ,@BookingRate " +
                            " ,@SupplierId " +
                            " ,@RateBandDeclarationPurchaseId " +
                            " ,@IsConfirmed " +
                            " ,@ProdClassId " +
                            " ,@StatusId " +
                            " ,@CalculatedMetalCost " +
                            " ,@BaseMetalCost " +
                            " ,@Padta " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedBy " +
                            " ,@UpdatedOn " +
                            " ,@Comments " +
                            " ,@IsConvertToSauda " +
                            " ,@PendingBookingQty " +
                            " ,@DemandedRate " +
                            " ,@AuthReasons " +
                            " ,@IsOpenQtySauda " +
                            " ,@EnqDisplayNo " +
                            " ,@FinYear " +
                            " ,@IsSpotedVehicle " +
                            " ,@EnqNo " +
                            " ,@SaudaCreatedOn " +
                            " ,@DeliveryDays" +
                            " ,@NoOfVehicleSched" +
                            " ,@Remark" +
                            " ,@Freight" +
                            " ,@IsFixed" +
                            " ,@TransportAmtPerMT" +
                            ",@RateForC" +
                            ",@RateForNC" +
                            ",@ConsumedQty" +
                            ",@OptionalPendingQty" +
                            ",@WtRateApprovalDiff" +
                            ",@IsAutoSpotVehSauda" +
                            ",@refRateofV48Var" +
                            ",@RefRateC" + 
                            ",@VehicleTypeDesc" +
                            ",@IsEnqTransfered" +
                            ",@SaudaTypeId" +
                            " ,@NoOfVehicleSched" +
                            //Deepali added for task no 920 and 921
                            " ,@CurrencyId" +
                            " ,@ContractTypeId" +
                            " ,@ContractComment" +
                            ", @ContractNumber"+
                            ", @ContractDate"+
                            ", @CountryOfOrigin"+
                            ", @PortOfLoading" +
                            ", @PortOfDischarge"+
                            ", @FinalPlaceOfDelivery"+
                            ", @WeighmentTolerance"+
                            ", @ImpuritiesTolerance"+
                            ", @AverageLoading"+
                            ", @IndentureName" +                            
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdBooking;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.UserId;
            cmdInsert.Parameters.Add("@COrNCId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.COrNCId;
            cmdInsert.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.GlobalRatePurchaseId;
            cmdInsert.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BookingQty;
            cmdInsert.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BookingRate;
            cmdInsert.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.SupplierId;
            cmdInsert.Parameters.Add("@RateBandDeclarationPurchaseId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.RateBandDeclarationPurchaseId;
            cmdInsert.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Decimal).Value = tblPurchaseEnquiryTO.IsConfirmed;
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ProdClassId);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.StatusId);
            cmdInsert.Parameters.Add("@CalculatedMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.CalculatedMetalCost;
            cmdInsert.Parameters.Add("@BaseMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BaseMetalCost;
            cmdInsert.Parameters.Add("@Padta", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.Padta;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.CreatedBy;
            // if (tblPurchaseEnquiryTO.IsConvertToSauda == 1)
            // {
            //     tblPurchaseEnquiryTO.CreatedOn =  _iCommonDAO.ServerDateTime;
            // }
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryTO.CreatedOn;
            cmdInsert.Parameters.Add("@refRateofV48Var", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.RefRateofV48Var);
            cmdInsert.Parameters.Add("@RefRateC", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.RefRateC);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.UpdatedOn);
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.Comments);
            cmdInsert.Parameters.Add("@IsConvertToSauda", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IsConvertToSauda;
            cmdInsert.Parameters.Add("@PendingBookingQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.PendingBookingQty);
            cmdInsert.Parameters.Add("@DemandedRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.DemandedRate;
            cmdInsert.Parameters.Add("@AuthReasons", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.AuthReasons);
            cmdInsert.Parameters.Add("@IsOpenQtySauda", System.Data.SqlDbType.Int).Value = (tblPurchaseEnquiryTO.IsOpenQtySauda);
            cmdInsert.Parameters.Add("@EnqDisplayNo", System.Data.SqlDbType.NVarChar).Value = (tblPurchaseEnquiryTO.EnqDisplayNo);
            cmdInsert.Parameters.Add("@FinYear", System.Data.SqlDbType.Int).Value = (tblPurchaseEnquiryTO.FinYear);
            cmdInsert.Parameters.Add("@IsSpotedVehicle", System.Data.SqlDbType.Int).Value = (tblPurchaseEnquiryTO.IsSpotedVehicle);
            cmdInsert.Parameters.Add("@EnqNo", System.Data.SqlDbType.Int).Value = (tblPurchaseEnquiryTO.EnqNo);
            cmdInsert.Parameters.Add("@SaudaCreatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.SaudaCreatedOn);

            cmdInsert.Parameters.Add("@DeliveryDays", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.DeliveryDays);
            cmdInsert.Parameters.Add("@NoOfVehicleSched", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.NoOfVehicleSched);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.Remark);
            cmdInsert.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.Freight);
            cmdInsert.Parameters.Add("@IsFixed", System.Data.SqlDbType.Int).Value = (tblPurchaseEnquiryTO.IsFixed);
            cmdInsert.Parameters.Add("@TransportAmtPerMT", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.TransportAmtPerMT);
            cmdInsert.Parameters.Add("@RateForC", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.RateForC);
            cmdInsert.Parameters.Add("@RateForNC", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.RateForNC);
            cmdInsert.Parameters.Add("@ConsumedQty", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.ConsumedQty);
            cmdInsert.Parameters.Add("@OptionalPendingQty", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.OptionalPendingQty);
            cmdInsert.Parameters.Add("@WtRateApprovalDiff", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.WtRateApprovalDiff);
            cmdInsert.Parameters.Add("@IsAutoSpotVehSauda", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.IsAutoSpotVehSauda);
            cmdInsert.Parameters.Add("@VehicleTypeDesc", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.VehicleTypeDesc);
            cmdInsert.Parameters.Add("@IsEnqTransfered", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.IsEnqTransfered);
            cmdInsert.Parameters.Add("@SaudaTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.SaudaTypeId);
            //Deepali added for task no 920 and 921
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.CurrencyId);
            cmdInsert.Parameters.Add("@ContractTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ContractTypeId);
            cmdInsert.Parameters.Add("@ContractComment", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ContractComment);

            cmdInsert.Parameters.Add("@ContractNumber", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ContractNumber);
            cmdInsert.Parameters.Add("@ContractDate", System.Data.SqlDbType.Date).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ContractDate);
            cmdInsert.Parameters.Add("@CountryOfOrigin", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.CountryOfOrigin);
            cmdInsert.Parameters.Add("@PortOfLoading", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.PortOfLoading);
            cmdInsert.Parameters.Add("@PortOfDischarge", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.PortOfDischarge);
            cmdInsert.Parameters.Add("@FinalPlaceOfDelivery", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.FinalPlaceOfDelivery);
            cmdInsert.Parameters.Add("@WeighmentTolerance", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.WeighmentTolerance);
            cmdInsert.Parameters.Add("@ImpuritiesTolerance", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ImpuritiesTolerance);
            cmdInsert.Parameters.Add("@AverageLoading", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.AverageLoading);
            cmdInsert.Parameters.Add("@IndentureName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.IndentureName);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblPurchaseEnquiryTO.IdPurchaseEnquiry = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        public int ExecuteInsertionCommandForHistory(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [tblPurchaseEnquiryHistory]( " +
                  " [idPurchaseEnquiry]" +
                  " ,[globalRatePurchaseId]" +
                  " ,[bookingQty]" +
                  " ,[bookingRate]" +
                  " ,[createdBy]" +
                  " ,[createdOn]" +
                  " ,[comments]" +
                  " ,[statusId]" +
                  " ,[wtActualRate]" +
                    " )" +
                " VALUES (" +
                            "  @IdPurchaseEnquiry " +
                            " ,@GlobalRatePurchaseId " +
                            " ,@BookingQty " +
                            " ,@BookingRate " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@Comments " +
                            " ,@StatusId " +
                            " ,@WtActualRate " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdBooking;
            cmdInsert.Parameters.Add("@IdPurchaseEnquiry", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
            cmdInsert.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.GlobalRatePurchaseId;
            cmdInsert.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BookingQty;
            cmdInsert.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BookingRate;

            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryTO.CreatedOn;
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.Comments);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.StatusId);
            cmdInsert.Parameters.Add("@WtActualRate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.WtActualRate);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                // tblPurchaseEnquiryTO.IdPurchaseEnquiryHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        //Priyanka [03-01-2019]
        public int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingActionsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblBookingActionsTO tblBookingActionsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseBookingActions]( " +
                                "  [isAuto]" +
                                " ,[statusBy]" +
                                " ,[statusDate]" +
                                " ,[bookingStatus]" +
                                " )" +
                    " VALUES (" +
                                "  @IsAuto " +
                                " ,@StatusBy " +
                                " ,@StatusDate " +
                                " ,@BookingStatus " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@IsAuto", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.IsAuto;
            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.StatusBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingActionsTO.StatusDate;
            cmdInsert.Parameters.Add("@BookingStatus", System.Data.SqlDbType.VarChar).Value = tblBookingActionsTO.BookingStatus;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingActionsTO.IdBookingAction = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblBookingsForPurchase(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseEnquiryTO, cmdUpdate);
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

        public int UpdateTblBookingsForConverToSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommandForSauda(tblPurchaseEnquiryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                conn.Close();
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
                conn.Close();
            }
        }

        public int ExecuteUpdationCommand(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdUpdate)
        {

            String sqlQuery = @" UPDATE [tblPurchaseEnquiry] SET " +
                             "  [cOrNCId]= @COrNCId" +
                             " ,[globalRatePurchaseId]= @GlobalRatePurchaseId" +
                             " ,[bookingQty]= @BookingQty" +
                             " ,[bookingRate]= @BookingRate" +
                             " ,[rateBandDeclarationPurchaseId]= @RateBandDeclarationPurchaseId" +
                             " ,[isConfirmed]= @IsConfirmed" +
                             " ,[prodClassId]= @ProdClassId" +
                             " ,[statusId]= @StatusId" +
                             " ,[calculatedMetalCost]= @CalculatedMetalCost" +
                             " ,[baseMetalCost]= @BaseMetalCost" +
                             " ,[padta]= @Padta" +
                             " ,[updatedBy]= @UpdatedBy" +
                             " ,[updatedOn]= @UpdatedOn" +
                             " ,[createdOn]= @CreatedOn" +
                             " ,[comments] = @Comments" +
                             " ,[isConvertToSauda]=@IsConvertToSauda " +
                             " ,[pendingBookingQty]=@PendingBookingQty " +
                             " ,[demandedRate]=@DemandedRate " +
                             " ,[authReasons]=@AuthReasons " +
                             " ,[isOpenQtySauda]=@IsOpenQtySauda " +
                             " ,[enqDisplayNo]=@EnqDisplayNo " +
                             " ,[finYear]=@FinYear " +
                             " ,[isSpotedVehicle]=@IsSpotedVehicle " +
                             " ,[enqNo]=@EnqNo " +
                             " ,[saudaCreatedOn]=@SaudaCreatedOn " +
                             " ,[SupplierId]=@SupplierId " +

                             //Priyanka [07-01-2019] Added
                             " ,[deliveryDays]=@DeliveryDays " +
                             " ,[noOfVehicleSched]=@NoOfVehicleSched " +
                             " ,[remark]= @Remark " +
                             " ,[freight]= @Freight " +
                             " ,[isFixed]= @IsFixed " +
                             " ,[transportAmtPerMT]= @TransportAmtPerMT " +
                             " ,[rateForC]= @RateForC " +
                             " ,[rateForNC]= @RateForNC " +
                             " ,[consumedQty]= @ConsumedQty " +
                             " ,[optionalPendingQty]= @OptionalPendingQty " +
                             " ,[wtRateApprovalDiff]= @WtRateApprovalDiff " +
                             " ,[refRateofV48Var]= @RefRateofV48Var " +
                             " ,[refRateC]= @RefRateC " +
                             " ,[isAutoSpotVehSauda]= @IsAutoSpotVehSauda " +
                             " ,[vehicleTypeDesc]= @VehicleTypeDesc " +
                             " ,[isEnqTransfered]= @IsEnqTransfered " +
                             " ,[saudaTypeId]= @SaudaTypeId " +
                              //Deepali added for task no 921
                             " ,[currencyId]= @CurrencyId " +
                             //Deepali added for task no 920
                             " ,[contractTypeId]= @ContractTypeId " +
                             " ,[contractComment]= @ContractComment " +
                             " ,[contractNumber]= @ContractNumber" +
                             " ,[contractDate]= @ContractDate" +
                             " ,[countryOfOrigin]= @CountryOfOrigin" +
                             " ,[portOfLoading]= @PortOfLoading" +
                             " ,[portOfDischarge]= @PortOfDischarge" +
                             " ,[finalPlaceOfDelivery]= @FinalPlaceOfDelivery" +
                             " ,[weighmentTolerance]= @WeighmentTolerance" +
                             " ,[impuritiesTolerance]= @ImpuritiesTolerance" +
                             " ,[averageLoading]= @AverageLoading" +
                             " ,[indentureName]= @IndentureName" +
                             " WHERE  [idPurchaseEnquiry] = @IdPurchaseEnquiry";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseEnquiry", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
            cmdUpdate.Parameters.Add("@COrNCId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.COrNCId;
            cmdUpdate.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.GlobalRatePurchaseId;
            cmdUpdate.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BookingQty;
            cmdUpdate.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BookingRate;
            cmdUpdate.Parameters.Add("@RateBandDeclarationPurchaseId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.RateBandDeclarationPurchaseId;
            cmdUpdate.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ProdClassId);
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.StatusId;
            cmdUpdate.Parameters.Add("@CalculatedMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.CalculatedMetalCost;
            cmdUpdate.Parameters.Add("@BaseMetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BaseMetalCost;
            cmdUpdate.Parameters.Add("@Padta", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.Padta;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.Comments);
            cmdUpdate.Parameters.Add("@IsConvertToSauda", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.IsConvertToSauda;
            cmdUpdate.Parameters.Add("@DemandedRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.DemandedRate;
            cmdUpdate.Parameters.Add("@PendingBookingQty", System.Data.SqlDbType.Decimal).Value = tblPurchaseEnquiryTO.PendingBookingQty;
            cmdUpdate.Parameters.Add("@AuthReasons", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.AuthReasons);
            cmdUpdate.Parameters.Add("@IsOpenQtySauda", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IsOpenQtySauda;
            cmdUpdate.Parameters.Add("@EnqDisplayNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.EnqDisplayNo;
            cmdUpdate.Parameters.Add("@FinYear", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.FinYear;
            cmdUpdate.Parameters.Add("@IsSpotedVehicle", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IsSpotedVehicle;
            cmdUpdate.Parameters.Add("@EnqNo", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.EnqNo;
            cmdUpdate.Parameters.Add("@SaudaCreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.SaudaCreatedOn);

            cmdUpdate.Parameters.Add("@DeliveryDays", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.DeliveryDays);
            cmdUpdate.Parameters.Add("@NoOfVehicleSched", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.NoOfVehicleSched);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.Remark);
            cmdUpdate.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = tblPurchaseEnquiryTO.Freight;
            cmdUpdate.Parameters.Add("@IsFixed", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IsFixed;
            cmdUpdate.Parameters.Add("@TransportAmtPerMT", System.Data.SqlDbType.Decimal).Value = tblPurchaseEnquiryTO.TransportAmtPerMT;
            cmdUpdate.Parameters.Add("@RateForC", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.RateForC);
            cmdUpdate.Parameters.Add("@RateForNC", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.RateForNC);
            cmdUpdate.Parameters.Add("@ConsumedQty", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.ConsumedQty);
            cmdUpdate.Parameters.Add("@OptionalPendingQty", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryTO.OptionalPendingQty);
            cmdUpdate.Parameters.Add("@WtRateApprovalDiff", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.WtRateApprovalDiff);
            cmdUpdate.Parameters.Add("@IsAutoSpotVehSauda", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.IsAutoSpotVehSauda);
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.SupplierId);
            cmdUpdate.Parameters.Add("@RefRateofV48Var", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.RefRateofV48Var);
            cmdUpdate.Parameters.Add("@RefRateC", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.RefRateC);
            cmdUpdate.Parameters.Add("@VehicleTypeDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.VehicleTypeDesc);
            cmdUpdate.Parameters.Add("@IsEnqTransfered", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.IsEnqTransfered);
            cmdUpdate.Parameters.Add("@SaudaTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.SaudaTypeId);

            // if (tblPurchaseEnquiryTO.IsConvertToSauda == 1)
            // {
            //     tblPurchaseEnquiryTO.CreatedOn =  _iCommonDAO.ServerDateTime;

            // }
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryTO.CreatedOn;
            //Deepali added for task no 921 and 920
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.CurrencyId);
            cmdUpdate.Parameters.Add("@ContractTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ContractTypeId);
            cmdUpdate.Parameters.Add("@ContractComment", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ContractComment);

            cmdUpdate.Parameters.Add("@ContractNumber", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ContractNumber);
            cmdUpdate.Parameters.Add("@ContractDate", System.Data.SqlDbType.Date).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ContractDate);
            cmdUpdate.Parameters.Add("@CountryOfOrigin", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.CountryOfOrigin);
            cmdUpdate.Parameters.Add("@PortOfLoading", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.PortOfLoading);
            cmdUpdate.Parameters.Add("@PortOfDischarge", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.PortOfDischarge);
            cmdUpdate.Parameters.Add("@FinalPlaceOfDelivery", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.FinalPlaceOfDelivery);
            cmdUpdate.Parameters.Add("@WeighmentTolerance", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.WeighmentTolerance);
            cmdUpdate.Parameters.Add("@ImpuritiesTolerance", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.ImpuritiesTolerance);
            cmdUpdate.Parameters.Add("@AverageLoading", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.AverageLoading);
            cmdUpdate.Parameters.Add("@IndentureName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.IndentureName);

            return cmdUpdate.ExecuteNonQuery();
        }

        public int ExecuteUpdationCommandForSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdUpdate)
        {

            String sqlQuery = @" UPDATE [tblPurchaseEnquiry] SET " +
                             " [statusId]= @StatusId" +
                             " ,[CreatedOn] = @CreatedOn" +
                             " ,[updatedBy]= @UpdatedBy" +
                             " ,[updatedOn]= @UpdatedOn" +
                             " ,[isConvertToSauda]=@IsConvertToSauda " +
                             //" ,[vehicleSpotEntryId]=@vehicleSpotEntryId " +

                             " WHERE  [idPurchaseEnquiry] = @IdPurchaseEnquiry";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseEnquiry", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsConvertToSauda", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IsConvertToSauda;
            cmdUpdate.Parameters.Add("@IdPurchaseEnquiry", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.VehicleSpotEntryId;

            return cmdUpdate.ExecuteNonQuery();
        }

        //Priyanka [03-01-2019]
        public int DeactivateAllDeclaredQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblRateBandDeclarationPurchase] SET " +
                                  "  [isActive] = @isActive " +
                                  " ,[updatedBy] = @updatedBy " +
                                  " ,[validUpto] = @validUpto " +
                                  " ,[updatedOn] = @updatedOn ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@validUpto", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = updatedBy;
                cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = _iCommonDAO.ServerDateTime;

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
        public int UpdateEnquiryPendingBookingQty(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseEnquiry] SET " +
                                  "  [pendingBookingQty] = @PendingBookingQty " + 
                                  " WHERE  [idPurchaseEnquiry] = @IdBooking";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                cmdUpdate.Parameters.Add("@PendingBookingQty", System.Data.SqlDbType.Decimal).Value = tblPurchaseEnquiryTO.PendingBookingQty;

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

        public int UpdateTblBookings(TblPurchaseEnquiryTO tblBookingsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingsTO, cmdUpdate);
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

        public int UpdateTblBookings(TblPurchaseEnquiryTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingsTO, cmdUpdate);
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

        public int ExecuteInsertionCommandForHistory(TblPurchaseEnquiryTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteInsertionCommandForHistory(tblBookingsTO, cmdUpdate);
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

        //public  int ExecuteUpdationCommand(TblPurchaseEnquiryTO tblBookingsTO, SqlCommand cmdUpdate)
        //{
        //    String sqlQuery = @" UPDATE [tblPurchaseEnquiry] SET " +
        //                    " [SupplierId]= @DealerOrgId" +
        //                    // " ,[isConfirmed]= @IsConfirmed" +
        //                    " ,[statusId]= @StatusId" +
        //                    " ,[globalRatePurchaseId]= @GlobalRateId" +
        //                    ", [CreatedOn]= @CreatedOn" +
        //                    " ,[updatedBy]= @UpdatedBy" +
        //                    " ,[updatedOn]= @UpdatedOn" +
        //                    " ,[bookingQty]= @BookingQty" +
        //                    " ,[bookingRate]= @BookingRate" +
        //                    " ,[comments] = @Comments" +
        //                    " ,[isConvertToSauda]= @isConvertToSauda " +
        //                    " ,[bookingPmRate]= @bookingPmRate" +
        //                    " ,[saudaCreatedOn]= @SaudaCreatedOn " +
        //                    " WHERE  [idPurchaseEnquiry] = @IdBooking";

        //    cmdUpdate.CommandText = sqlQuery;
        //    cmdUpdate.CommandType = System.Data.CommandType.Text;
        //    cmdUpdate.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdPurchaseEnquiry;
        //    cmdUpdate.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblBookingsTO.DealerOrgId;
        //    // cmdUpdate.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsConfirmed;
        //    cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingsTO.StatusId;
        //    cmdUpdate.Parameters.Add("@GlobalRateId", System.Data.SqlDbType.Int).Value = tblBookingsTO.GlobalRateId;
        //    cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBookingsTO.UpdatedBy;
        //    cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.UpdatedOn;
        //    cmdUpdate.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingQty;
        //    cmdUpdate.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingRate;
        //    cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.CreatedOn;

        //    // if (tblBookingsTO.StatusId != Convert.ToInt32(Constants.TranStatusE.BOOKING_ACCEPTED_BY_PM) && tblBookingsTO.StatusId != Convert.ToInt32(Constants.TranStatusE.BOOKING_ACCEPTED_BY_DIRECTOR))
        //    // {
        //    //     cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.CreatedOn;
        //    // }
        //    if (!string.IsNullOrEmpty(tblBookingsTO.StatusRemark))
        //        cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.StatusRemark);
        //    else
        //        cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.Comments);
        //    if (tblBookingsTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_ACCEPTED_BY_PM) || tblBookingsTO.StatusId == Convert.ToInt32(Constants.TranStatusE.BOOKING_ACCEPTED_BY_DIRECTOR))
        //    {
        //        cmdUpdate.Parameters.Add("@isConvertToSauda", System.Data.SqlDbType.Int).Value = 1;
        //        tblBookingsTO.SaudaCreatedOn =  _iCommonDAO.ServerDateTime;
        //        cmdUpdate.Parameters.Add("@SaudaCreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.SaudaCreatedOn;

        //    }
        //    else if (tblBookingsTO.StatusId == Convert.ToInt32(Constants.TranStatusE.PENDING_FOR_PURCHASE_MANAGER_APPROVAL))
        //    {
        //        cmdUpdate.Parameters.Add("@isConvertToSauda", System.Data.SqlDbType.Int).Value = 0;
        //        cmdUpdate.Parameters.Add("@SaudaCreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.SaudaCreatedOn);
        //    }
        //    else
        //    {
        //        cmdUpdate.Parameters.Add("@isConvertToSauda", System.Data.SqlDbType.Int).Value = 0;
        //        cmdUpdate.Parameters.Add("@SaudaCreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.SaudaCreatedOn);
        //    }

        //    cmdUpdate.Parameters.Add("@bookingPmRate", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.Bookingpmrate;

        //    return cmdUpdate.ExecuteNonQuery();
        //}

        //public  int ExecuteInsertionCommandForHistory(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlCommand cmdInsert)
        //{

        //    String sqlQuery = @" INSERT INTO [tblPurchaseEnquiryHistory]( " +
        //          " [idPurchaseEnquiry]" +
        //          " ,[globalRatePurchaseId]" +
        //          " ,[bookingQty]" +
        //          " ,[bookingRate]" +
        //          " ,[createdBy]" +
        //          " ,[createdOn]" +
        //          " ,[comments]" +
        //          " ,[statusId]" +
        //            " )" +
        //        " VALUES (" +
        //                    "  @IdPurchaseEnquiry " +
        //                    " ,@GlobalRatePurchaseId " +
        //                    " ,@BookingQty " +
        //                    " ,@BookingRate " +
        //                    " ,@CreatedBy " +
        //                    " ,@CreatedOn " +
        //                    " ,@Comments " +
        //                    " ,@StatusId " +
        //                    " )";

        //    cmdInsert.CommandText = sqlQuery;
        //    cmdInsert.CommandType = System.Data.CommandType.Text;
        //    String sqlSelectIdentityQry = "Select @@Identity";

        //    //cmdInsert.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdBooking;
        //    cmdInsert.Parameters.Add("@IdPurchaseEnquiry", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
        //    cmdInsert.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.GlobalRateId;
        //    cmdInsert.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BookingQty;
        //    cmdInsert.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.BookingRate;

        //    cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.CreatedBy;
        //    cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = DateTime.Now;//tblPurchaseEnquiryTO.CreatedOn;
        //    cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryTO.Comments;
        //    cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.StatusId);
        //    if (cmdInsert.ExecuteNonQuery() == 1)
        //    {
        //        cmdInsert.CommandText = sqlSelectIdentityQry;
        //        tblPurchaseEnquiryTO.IdPurchaseEnquiry = Convert.ToInt32(cmdInsert.ExecuteScalar());
        //        return 1;
        //    }
        //    else return 0;
        //}

        #endregion

        public List<TblPurchaseEnquiryTO> SelectAllTodaysBookingsWithOpeningBalance(int cnfOrgId, int dealerOrgId, DateTime serverDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                string whereCond = "";
                if (cnfOrgId > 0)
                {
                    whereCond = " AND tblPurchaseEnquiry.userId= " + cnfOrgId;
                }
                cmdSelect.CommandText = SqlQueryForSaudaReport() + " Inner join tblPurchaseBookingOpngBal tblPurchaseBookingOpngBal on tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseBookingOpngBal.enquiryId"
                                         + " where tblPurchaseBookingOpngBal.isActive =1" + whereCond;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToListForSaudaReport(reader);
                if (list != null && list.Count > 0)
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

        public List<TblPurchaseEnquiryTO> SelectAllPendingBookingsList(int cnfOrgId, int dealerOrgId, DateTime date, string v1, bool v2)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                String statusIds = (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_DIRECTOR + "," + (Int32)Constants.TranStatusE.BOOKING_APPROVED
                    + "," + (Int32)Constants.TranStatusE.COMPLETED + "," + (Int32)Constants.TranStatusE.BOOKING_CANCELED;
                string whereCond = "";
                if (cnfOrgId > 0)
                {
                    whereCond = " AND tblPurchaseEnquiry.userId= " + cnfOrgId;
                }
                if (dealerOrgId > 0)
                {
                    whereCond += " AND supplierId=" + dealerOrgId;
                }
                cmdSelect.CommandText = SqlQueryForSaudaReport() + " WHERE CAST(tblPurchaseEnquiry.saudacreatedon AS DATE) = "
                 + "@asOnDate AND tblPurchaseEnquiry.statusId IN(" + statusIds + ")" + " " + whereCond;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@asOnDate", System.Data.SqlDbType.Date).Value = date.ToString(Constants.AzureDateFormat);

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryTO> list = ConvertDTToListForSaudaReport(reader);
                if (list != null && list.Count > 0)
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


        public string SqlQueryForSaudaReport()
        {
            return @"select tblUser.userDisplayName as purchaseManagerName,tblProdClassification.prodClassDesc,tblOrganization.firmName as supplierName,dimStatus.StatusName,tblPurchaseEnquiry.* from tblPurchaseEnquiry tblPurchaseEnquiry "
                                         + " LEFT JOIN tblUser tblUser on tblUser.iduser = tblPurchaseEnquiry.userid"
                                         + " left join tblProdClassification tblProdClassification on tblProdClassification.idProdClass = tblPurchaseEnquiry.prodClassId "
                                         + " left join tblOrganization tblOrganization on tblOrganization.idOrganization = tblPurchaseEnquiry.SupplierId"
                                         + " LEFT JOIN dimStatus ON dimStatus.idStatus = tblPurchaseEnquiry.statusId ";
        }

        public int UpdatePendingNoOfVehicles(TblPurchaseEnquiryTO enquiryTO, int pendNoOfVeh, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblPurchaseEnquiry] SET " +
                " [pendNoOfVeh]= @pendNoOfVeh" +
                " WHERE [idPurchaseEnquiry] = "+ enquiryTO.IdPurchaseEnquiry;

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@pendNoOfVeh", System.Data.SqlDbType.Int).Value = pendNoOfVeh;
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


        public int UpdateMaterialTypeOfSauda(TblPurchaseEnquiryTO tblPurchaseEnquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseEnquiry] SET " +
                                 "  [prodClassId] = @ProdClassId" +
                                 "  [updatedBy] = @UpdatedBy" +
                                 "  [updatedOn] = @UpdatedOn" +
                                 " WHERE idPurchaseEnquiry = @IdPurchaseEnquiry";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseEnquiry", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdPurchaseEnquiry;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.UpdatedBy);
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryTO.UpdatedOn);
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

    }
}
