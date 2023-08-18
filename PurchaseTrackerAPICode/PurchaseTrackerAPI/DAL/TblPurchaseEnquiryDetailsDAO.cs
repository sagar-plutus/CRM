using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseEnquiryDetailsDAO : ITblPurchaseEnquiryDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseEnquiryDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods

        public String SqlSelectQuery()
        {
            String sqlSelectQry = "Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName, parityAmt, nonConfParityAmt, recovery" +
                                  " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem" +
                                  " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId" +
                                  " LEFT JOIN tblParityDetailsPurchase" +
                                  " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem" +
                                  " LEFT JOIN tblParitySummaryPurchase " +
                                  " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId "; // +
                                                                                                                                //  " AND  tblParitySummaryPurchase.isActive=1";

            return sqlSelectQry;
        }
        public String SqlSelectQueryNew()
        {
            //Prajakta[2019-02-15] Commented and added recovery
            // String sqlSelectQry = "Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,tblProductItem.isNonCommercialItem, parityAmt, nonConfParityAmt, ((tblPurchaseEnquiryDetails.productRecovery/NULLIF(tblPurchaseEnquiryDetails.qty,0))*100) as 'recovery'" +
            //                       " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem" +
            //                       " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId" +
            //                       " LEFT JOIN tblParityDetailsPurchase" +
            //                       " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem" +
            //                       " LEFT JOIN tblParitySummaryPurchase " +
            //                       " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId "; 


            String sqlSelectQry = "Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,tblProductItem.isNonCommercialItem, parityAmt, nonConfParityAmt, tblPurchaseEnquiryDetails.recovery as 'recovery'" +
                                   " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem" +
                                   " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId" +
                                   " LEFT JOIN tblParityDetailsPurchase" +
                                   " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem" +
                                   " LEFT JOIN tblParitySummaryPurchase " +
                                   " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId ";


            return sqlSelectQry;
        }

        #endregion

        #region selection

        public List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsListByEnquiryId(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            try
            {

                cmdSelect.CommandText = "SELECT * FROM tblPurchaseEnquiryDetails WHERE purchaseEnquiryId =" + purchaseEnquiryId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblEnquiryDetailsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryDetailsTO> tblEnquiryDetailsTOList = new List<TblPurchaseEnquiryDetailsTO>();
                if (tblEnquiryDetailsTODT != null)
                {
                    while (tblEnquiryDetailsTODT.Read())
                    {
                        TblPurchaseEnquiryDetailsTO tblEnquiryDetailsTONew = new TblPurchaseEnquiryDetailsTO();
                        if (tblEnquiryDetailsTODT["idPurchaseEnquiryDetails"] != DBNull.Value)
                            tblEnquiryDetailsTONew.IdPurchaseEnquiryDetails = Convert.ToInt32(tblEnquiryDetailsTODT["idPurchaseEnquiryDetails"].ToString());
                        if (tblEnquiryDetailsTODT["Qty"] != DBNull.Value)
                            tblEnquiryDetailsTONew.Qty = Convert.ToDouble(tblEnquiryDetailsTODT["Qty"].ToString());
                        if (tblEnquiryDetailsTODT["prodItemId"] != DBNull.Value)
                            tblEnquiryDetailsTONew.ProdItemId = Convert.ToInt32(tblEnquiryDetailsTODT["prodItemId"].ToString());
                        if (tblEnquiryDetailsTODT["pendingQty"] != DBNull.Value)
                            tblEnquiryDetailsTONew.PendingQty = Convert.ToDouble(tblEnquiryDetailsTODT["pendingQty"].ToString());
                        tblEnquiryDetailsTOList.Add(tblEnquiryDetailsTONew);
                    }
                }
                //List<TblPurchaseEnquiryDetailsTO> list = ConvertDTToList(reader);
                tblEnquiryDetailsTODT.Dispose();
                tblEnquiryDetailsTODT.Close();
                return tblEnquiryDetailsTOList;
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

        public List<TblPurchaseEnquiryDetailsTO> SelectTblEnquiryDetailsList(Int32 purchaseEnquiryId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);

            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                // String sqlSelectQry = "Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,(productRecovery/tblPurchaseEnquiryDetails.Qty)*100  as recovery" +
                //                   " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem" +
                //                   " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId";

                String sqlSelectQry = "Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,tblPurchaseEnquiryDetails.recovery  as recovery" +
                                               " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem" +
                                               " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId";

                cmdSelect.CommandText = sqlSelectQry + " WHERE purchaseEnquiryId =" + purchaseEnquiryId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryDetailsTO> list = ConvertDTToListNew(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

                cmdSelect.Dispose();
                conn.Close();
            }
        }

        public List<TblPurchaseEnquiryDetailsTO> SelectEnquiryDetailsListBySaudaIds(string saudaIds)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                String sqlSelectQry = "Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,tblPurchaseEnquiryDetails.recovery  as recovery" +
                                               " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem" +
                                               " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId";

                cmdSelect.CommandText = sqlSelectQry + " WHERE purchaseEnquiryId IN (" + saudaIds + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryDetailsTO> list = ConvertDTToListNew(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        public List<TblPurchaseEnquiryDetailsTO> SelectTblEnquiryDetailsList(Int32 purchaseEnquiryId,SqlConnection conn,SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                // String sqlSelectQry = "Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,(productRecovery/tblPurchaseEnquiryDetails.Qty)*100  as recovery" +
                //                   " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem" +
                //                   " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId";

                String sqlSelectQry = "Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,tblPurchaseEnquiryDetails.recovery  as recovery" +
                                               " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem" +
                                               " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId";

                cmdSelect.CommandText = sqlSelectQry + " WHERE purchaseEnquiryId =" + purchaseEnquiryId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryDetailsTO> list = ConvertDTToListNew(reader);
                reader.Dispose();
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

        public List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            try
            {

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblParitySummaryPurchase.isActive=1 AND purchaseEnquiryId =" + purchaseEnquiryId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryDetailsTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseEnquiryDetailsTO> ConvertDTToList(SqlDataReader tblEnquiryDetailsTODT)
        {
            List<TblPurchaseEnquiryDetailsTO> tblEnquiryDetailsTOList = new List<TblPurchaseEnquiryDetailsTO>();
            if (tblEnquiryDetailsTODT != null)
            {
                while (tblEnquiryDetailsTODT.Read())
                {
                    TblPurchaseEnquiryDetailsTO tblEnquiryDetailsTONew = new TblPurchaseEnquiryDetailsTO();
                    if (tblEnquiryDetailsTODT["idPurchaseEnquiryDetails"] != DBNull.Value)
                        tblEnquiryDetailsTONew.IdPurchaseEnquiryDetails = Convert.ToInt32(tblEnquiryDetailsTODT["idPurchaseEnquiryDetails"].ToString());
                    if (tblEnquiryDetailsTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblEnquiryDetailsTONew.PurchaseEnquiryId = Convert.ToInt32(tblEnquiryDetailsTODT["purchaseEnquiryId"].ToString());
                    if (tblEnquiryDetailsTODT["prodItemId"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ProdItemId = Convert.ToInt32(tblEnquiryDetailsTODT["prodItemId"].ToString());
                    if (tblEnquiryDetailsTODT["Qty"] != DBNull.Value)
                        tblEnquiryDetailsTONew.Qty = Convert.ToDouble(tblEnquiryDetailsTODT["Qty"].ToString());
                    if (tblEnquiryDetailsTODT["rate"] != DBNull.Value)
                        tblEnquiryDetailsTONew.Rate = Convert.ToDouble(tblEnquiryDetailsTODT["rate"].ToString());
                    if (tblEnquiryDetailsTODT["productAomunt"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ProductAomunt = Convert.ToDouble(tblEnquiryDetailsTODT["productAomunt"].ToString());
                    if (tblEnquiryDetailsTODT["productRecovery"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ProductRecovery = Convert.ToDouble(tblEnquiryDetailsTODT["productRecovery"].ToString());
                    if (tblEnquiryDetailsTODT["pendingQty"] != DBNull.Value)
                        tblEnquiryDetailsTONew.PendingQty = Convert.ToDouble(tblEnquiryDetailsTODT["pendingQty"].ToString());
                    if (tblEnquiryDetailsTODT["loadingLayerId"] != DBNull.Value)
                        tblEnquiryDetailsTONew.LoadingLayerId = Convert.ToInt32(tblEnquiryDetailsTODT["loadingLayerId"].ToString());

                    if (tblEnquiryDetailsTODT["ItemName"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ItemName = tblEnquiryDetailsTODT["ItemName"].ToString();

                    if (tblEnquiryDetailsTODT["parityAmt"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ParityAmt = Convert.ToDouble(tblEnquiryDetailsTODT["parityAmt"].ToString());
                    if (tblEnquiryDetailsTODT["nonConfParityAmt"] != DBNull.Value)
                        tblEnquiryDetailsTONew.NonConfParityAmt = Convert.ToDouble(tblEnquiryDetailsTODT["nonConfParityAmt"].ToString());
                    if (tblEnquiryDetailsTODT["recovery"] != DBNull.Value)
                        tblEnquiryDetailsTONew.Recovery = Convert.ToDouble(tblEnquiryDetailsTODT["recovery"].ToString());

                    if (tblEnquiryDetailsTODT["actualRate"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ActualRate = Convert.ToDouble(tblEnquiryDetailsTODT["actualRate"].ToString());
                    // tblEnquiryDetailsTONew.ScheduleDateStr = tblEnquiryDetailsTONew.ScheduleDate.ToString("dd/MMM/yyyy");

                    if (tblEnquiryDetailsTODT["demandedRate"] != DBNull.Value)
                        tblEnquiryDetailsTONew.DemandedRate = Convert.ToDouble(tblEnquiryDetailsTODT["demandedRate"].ToString());

                    if (tblEnquiryDetailsTODT["isNonCommercialItem"] != DBNull.Value)
                        tblEnquiryDetailsTONew.IsNonCommercialItem = Convert.ToInt32(tblEnquiryDetailsTODT["isNonCommercialItem"].ToString());

                    if (tblEnquiryDetailsTODT["metalCost"] != DBNull.Value)
                        tblEnquiryDetailsTONew.MetalCost = Convert.ToDouble(tblEnquiryDetailsTODT["metalCost"].ToString());

                    if (tblEnquiryDetailsTODT["totalCost"] != DBNull.Value)
                        tblEnquiryDetailsTONew.TotalCost = Convert.ToDouble(tblEnquiryDetailsTODT["totalCost"].ToString());

                    if (tblEnquiryDetailsTODT["totalProduct"] != DBNull.Value)
                        tblEnquiryDetailsTONew.TotalProduct = Convert.ToDouble(tblEnquiryDetailsTODT["totalProduct"].ToString());

                    if (tblEnquiryDetailsTODT["gradePadta"] != DBNull.Value)
                        tblEnquiryDetailsTONew.GradePadta = Convert.ToDouble(tblEnquiryDetailsTODT["gradePadta"].ToString());

                    if (tblEnquiryDetailsTODT["itemBookingRate"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ItemBookingRate = Convert.ToDouble(tblEnquiryDetailsTODT["itemBookingRate"].ToString());
                    tblEnquiryDetailsTOList.Add(tblEnquiryDetailsTONew);
                }
            }
            return tblEnquiryDetailsTOList;
        }

        public List<TblPurchaseEnquiryDetailsTO> ConvertDTToListNew(SqlDataReader tblEnquiryDetailsTODT)
        {
            List<TblPurchaseEnquiryDetailsTO> tblEnquiryDetailsTOList = new List<TblPurchaseEnquiryDetailsTO>();
            if (tblEnquiryDetailsTODT != null)
            {
                while (tblEnquiryDetailsTODT.Read())
                {
                    TblPurchaseEnquiryDetailsTO tblEnquiryDetailsTONew = new TblPurchaseEnquiryDetailsTO();
                    if (tblEnquiryDetailsTODT["idPurchaseEnquiryDetails"] != DBNull.Value)
                        tblEnquiryDetailsTONew.IdPurchaseEnquiryDetails = Convert.ToInt32(tblEnquiryDetailsTODT["idPurchaseEnquiryDetails"].ToString());
                    if (tblEnquiryDetailsTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblEnquiryDetailsTONew.PurchaseEnquiryId = Convert.ToInt32(tblEnquiryDetailsTODT["purchaseEnquiryId"].ToString());
                    if (tblEnquiryDetailsTODT["prodItemId"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ProdItemId = Convert.ToInt32(tblEnquiryDetailsTODT["prodItemId"].ToString());
                    if (tblEnquiryDetailsTODT["Qty"] != DBNull.Value)
                        tblEnquiryDetailsTONew.Qty = Convert.ToDouble(tblEnquiryDetailsTODT["Qty"].ToString());
                    if (tblEnquiryDetailsTODT["rate"] != DBNull.Value)
                        tblEnquiryDetailsTONew.Rate = Convert.ToDouble(tblEnquiryDetailsTODT["rate"].ToString());
                    if (tblEnquiryDetailsTODT["productAomunt"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ProductAomunt = Convert.ToDouble(tblEnquiryDetailsTODT["productAomunt"].ToString());
                    if (tblEnquiryDetailsTODT["productRecovery"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ProductRecovery = Convert.ToDouble(tblEnquiryDetailsTODT["productRecovery"].ToString());
                    if (tblEnquiryDetailsTODT["pendingQty"] != DBNull.Value)
                        tblEnquiryDetailsTONew.PendingQty = Convert.ToDouble(tblEnquiryDetailsTODT["pendingQty"].ToString());
                    if (tblEnquiryDetailsTODT["loadingLayerId"] != DBNull.Value)
                        tblEnquiryDetailsTONew.LoadingLayerId = Convert.ToInt32(tblEnquiryDetailsTODT["loadingLayerId"].ToString());

                    if (tblEnquiryDetailsTODT["ItemName"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ItemName = tblEnquiryDetailsTODT["ItemName"].ToString();

                    if (tblEnquiryDetailsTODT["recovery"] != DBNull.Value)
                        tblEnquiryDetailsTONew.Recovery = Convert.ToDouble(tblEnquiryDetailsTODT["recovery"].ToString());

                    if (tblEnquiryDetailsTODT["demandedRate"] != DBNull.Value)
                        tblEnquiryDetailsTONew.DemandedRate = Convert.ToDouble(tblEnquiryDetailsTODT["demandedRate"].ToString());

                    //Priyanka [11-01-2019]
                    if (tblEnquiryDetailsTODT["itemBookingRate"] != DBNull.Value)
                        tblEnquiryDetailsTONew.ItemBookingRate = Convert.ToDouble(tblEnquiryDetailsTODT["itemBookingRate"].ToString());
                    tblEnquiryDetailsTOList.Add(tblEnquiryDetailsTONew);
                }
            }
            return tblEnquiryDetailsTOList;
        }
        public List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsList(Int32 purchaseEnquiryId, Int32 stateId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQueryNew() + "AND tblParitySummaryPurchase.stateId=" + stateId + " WHERE purchaseEnquiryId =" + purchaseEnquiryId +
                //                        " AND [tblParityDetailsPurchase].idParityDtlPurchase IN( SELECT [tblParityDetailsPurchase].idParityDtlPurchase " +
                //                        " FROM tblParityDetailsPurchase where createdOn = (select max(createdOn) from tblParityDetailsPurchase ParityDetailsPurchase" +
                //                        " where ParityDetailsPurchase.prodItemId = tblParityDetailsPurchase.prodItemId))";


                // cmdSelect.CommandText = " Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,tblProductItem.isNonCommercialItem, parity.parityAmt, parity.nonConfParityAmt, ((tblPurchaseEnquiryDetails.productRecovery / NULLIF(tblPurchaseEnquiryDetails.qty, 0)) * 100) as 'recovery' " +
                //                         " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem " +
                //                         " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId " +
                //                         " LEFT JOIN(SELECT parityAmt, nonConfParityAmt, prodItemId " +
                //                         " FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +
                //                         " WHERE stateId = " + stateId + " and isActive = 1) parity " +
                //                         " ON parity.prodItemId = tblPurchaseEnquiryDetails.prodItemId" +
                //                         " WHERE purchaseEnquiryId = " + purchaseEnquiryId;


                cmdSelect.CommandText = " Select tblPurchaseEnquiryDetails.*, tblProductItem.ItemName,tblProductItem.isNonCommercialItem, parity.parityAmt, parity.nonConfParityAmt, tblPurchaseEnquiryDetails.recovery as 'recovery' " +
                                                       " FROM tblPurchaseEnquiryDetails INNER JOIN tblProductItem " +
                                                       " ON tblProductItem.idProdItem = tblPurchaseEnquiryDetails.prodItemId " +
                                                       " LEFT JOIN(SELECT parityAmt, nonConfParityAmt, prodItemId " +
                                                       " FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +
                                                       " WHERE stateId = " + stateId + " and isActive = 1) parity " +
                                                       " ON parity.prodItemId = tblPurchaseEnquiryDetails.prodItemId" +
                                                       " WHERE purchaseEnquiryId = " + purchaseEnquiryId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryDetailsTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        #endregion

        #region Insertion

        public int InsertTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseEnquiryDetailsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseEnquiryDetails]( " +
                " [purchaseEnquiryId]" +
                " ,[prodItemId]" +
                " ,[Qty]" +
                " ,[rate]" +
                " ,[productAomunt]" +
                " ,[productRecovery]" +
                // " ,[schedulePurchaseId]" +
                " ,[pendingQty]" +
                " ,[loadingLayerId]" +
                " ,[demandedRate]" +
                " ,[metalCost]" +
                " ,[totalCost]" +
                " ,[actualRate]" +
                " ,[totalProduct]" +
                " ,[gradePadta]" +
                " ,[itemBookingRate]" +                 //Priyanka [11-01-2019]
                " ,[recovery]" +
                " )" +
                " VALUES (" +
                            "  @PurchaseEnquiryId " +
                            " ,@ProdItemId " +
                            " ,@Qty " +
                            " ,@Rate " +
                            " ,@ProductAomunt " +
                            " ,@ProductRecovery " +
                            //  " ,@SchedulePurchaseId " +
                            " ,@PendingQty " +
                            " ,@LoadingLayerId " +
                            " ,@demandedRate " +
                            " ,@MetalCost " +
                            " ,@TotalCost " +
                            " ,@ActualRate " +
                            " ,@TotalProduct " +
                            " ,@GradePadta " +
                            " ,@ItemBookingRate " +
                            " ,@Recovery " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryTO.IdBooking;
            cmdInsert.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryDetailsTO.PurchaseEnquiryId;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryDetailsTO.ProdItemId;
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = (tblPurchaseEnquiryDetailsTO.Qty);
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = (tblPurchaseEnquiryDetailsTO.Rate);
            cmdInsert.Parameters.Add("@ProductAomunt", System.Data.SqlDbType.NVarChar).Value = (tblPurchaseEnquiryDetailsTO.ProductAomunt);
            cmdInsert.Parameters.Add("@ProductRecovery", System.Data.SqlDbType.NVarChar).Value = (tblPurchaseEnquiryDetailsTO.ProductRecovery);
            //  cmdInsert.Parameters.Add("@SchedulePurchaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryDetailsTO.SchedulePurchaseId);
            cmdInsert.Parameters.Add("@PendingQty", System.Data.SqlDbType.Decimal).Value = tblPurchaseEnquiryDetailsTO.PendingQty;
            cmdInsert.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryDetailsTO.LoadingLayerId);
            cmdInsert.Parameters.Add("@demandedRate", System.Data.SqlDbType.NVarChar).Value = (tblPurchaseEnquiryDetailsTO.DemandedRate);
            cmdInsert.Parameters.Add("@MetalCost", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryDetailsTO.MetalCost);
            cmdInsert.Parameters.Add("@TotalCost", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryDetailsTO.TotalCost);
            cmdInsert.Parameters.Add("@ActualRate", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryDetailsTO.ActualRate);
            cmdInsert.Parameters.Add("@TotalProduct", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryDetailsTO.TotalProduct);
            cmdInsert.Parameters.Add("@GradePadta", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryDetailsTO.GradePadta);
            cmdInsert.Parameters.Add("@ItemBookingRate", System.Data.SqlDbType.NVarChar).Value = (tblPurchaseEnquiryDetailsTO.ItemBookingRate);         //Priyanka [11-01-2019]
            cmdInsert.Parameters.Add("@Recovery", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryDetailsTO.Recovery);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblPurchaseEnquiryDetailsTO.IdPurchaseEnquiryDetails = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            else return 0;
        }

        #endregion


        #region Updation
        public int UpdateTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseEnquiryDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseEnquiryDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseEnquiryDetails] SET " +
            //"  [idPurchaseEnquiryDetails] = @IdPurchaseEnquiryDetails" +
            "  [purchaseEnquiryId]= @PurchaseEnquiryId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[loadingLayerId]= @LoadingLayerId" +
            " ,[Qty]= @Qty" +
            " ,[rate]= @Rate" +
            " ,[productAomunt]= @ProductAomunt" +
            " ,[productRecovery]= @ProductRecovery" +
            " ,[pendingQty]= @PendingQty" +
            " ,[demandedRate]= @DemandedRate" +
            " ,[metalCost]= @MetalCost" +
            " ,[totalCost]= @TotalCost" +
            " ,[actualRate]= @ActualRate" +
            " ,[totalProduct]= @TotalProduct" +
            " ,[gradePadta] = @GradePadta" +
            " ,[itemBookingRate] = @ItemBookingRate" +
            " ,[recovery] = @Recovery" +
            " WHERE idPurchaseEnquiryDetails = @IdPurchaseEnquiryDetails ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseEnquiryDetails", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryDetailsTO.IdPurchaseEnquiryDetails;
            cmdUpdate.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryDetailsTO.PurchaseEnquiryId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryDetailsTO.ProdItemId;
            cmdUpdate.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryDetailsTO.LoadingLayerId);
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.Qty;
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.Rate;
            cmdUpdate.Parameters.Add("@ProductAomunt", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.ProductAomunt;
            cmdUpdate.Parameters.Add("@ProductRecovery", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.ProductRecovery;
            cmdUpdate.Parameters.Add("@PendingQty", System.Data.SqlDbType.Decimal).Value = tblPurchaseEnquiryDetailsTO.PendingQty;
            cmdUpdate.Parameters.Add("@DemandedRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.DemandedRate;
            cmdUpdate.Parameters.Add("@MetalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.MetalCost;
            cmdUpdate.Parameters.Add("@ActualRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.ActualRate;
            cmdUpdate.Parameters.Add("@TotalCost", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.TotalCost;
            cmdUpdate.Parameters.Add("@TotalProduct", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.TotalProduct;
            cmdUpdate.Parameters.Add("@GradePadta", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.GradePadta;
            cmdUpdate.Parameters.Add("@ItemBookingRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryDetailsTO.ItemBookingRate;
            cmdUpdate.Parameters.Add("@Recovery", System.Data.SqlDbType.Decimal).Value = tblPurchaseEnquiryDetailsTO.Recovery;
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateEnquiryItemPendingQty(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseEnquiryDetails] SET " +
             " [pendingQty]= @PendingQty" +
             " WHERE idPurchaseEnquiryDetails = @IdPurchaseEnquiryDetails ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseEnquiryDetails", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryDetailsTO.IdPurchaseEnquiryDetails;
                cmdUpdate.Parameters.Add("@PendingQty", System.Data.SqlDbType.Decimal).Value = (tblPurchaseEnquiryDetailsTO.PendingQty);

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

        //public  int DeleteTblBookingSchedule(Int32 idSchedule)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdDelete = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdDelete.Connection = conn;
        //        return ExecuteDeletionCommand(idSchedule, cmdDelete);
        //    }
        //    catch (Exception ex)
        //    {

        //        return 0;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdDelete.Dispose();
        //    }
        //}

        public int DeleteTblPurchaseEnquiryDetails(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(purchaseEnquiryId, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchaseEnquiryDetails, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseEnquiryDetails] " +
            " WHERE idPurchaseEnquiryDetails = " + idPurchaseEnquiryDetails + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllGradeDetailsForEnquiry(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseEnquiryDetails WHERE purchaseEnquiryId=" + purchaseEnquiryId + "";
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                return cmdDelete.ExecuteNonQuery();
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

        #endregion


    }
}
