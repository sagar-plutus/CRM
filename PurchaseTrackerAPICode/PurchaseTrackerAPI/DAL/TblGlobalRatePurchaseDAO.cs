using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblGlobalRatePurchaseDAO : ITblGlobalRatePurchaseDAO
    {


        private readonly IConnectionString _iConnectionString;
        public TblGlobalRatePurchaseDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = "SELECT * from tblGlobalRatePurchase";
            return sqlSelectQry;
        }

        public String SelectLatestRateWithAvg()
        {
            String sqlSelectQry = " SELECT CAST(tblGlobalRatePurchase.createdOn AS DATE),COUNT(*), " +
                                  " SUM(tblGlobalRatePurchase.rate) / COUNT(*) as avgRate FROM tblGlobalRatePurchase tblGlobalRatePurchase ";
                                  


            return sqlSelectQry;
        }

        #endregion

        #region Selection
        /// <summary>
        /// swati pisal
        /// To get latest Global purchase rate
        /// </summary>
        /// <returns></returns>
        public List<TblGlobalRatePurchaseTO> SelectLatestRateOfPurchaseDCT(DateTime date, Boolean isGetLatest)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlDataReader = null;
            try
            {
                conn.Open();

                //Saket [2018-01-18] Added new  query.
                //cmdSelect.CommandText = "SELECT max(idGlobalRatePurchase) as idGlobalRatePurchase, rate, Ratedec.createdBy, Ratedec.createdOn, comments, rateReasonId  " + //,Rate_Band_Costing 
                //    " FROM tblGlobalRatePurchase" +
                //    " INNER JOIN[dbo].[tblRateBandDeclarationPurchase] Ratedec" +
                //    " ON Ratedec.globalRatePurchaseId=idGlobalRatePurchase" +
                //    " WHERE idGlobalRatePurchase = (select max(idGlobalRatePurchase) from tblGlobalRatePurchase" +
                //    " WHERE DAY(Ratedec.createdOn)= " + date.Day + " AND MONTH(Ratedec.createdOn)= " + date.Month + " AND YEAR(Ratedec.createdOn)= " + date.Year +
                //    " ) GROUP BY rate, Ratedec.createdBy, Ratedec.createdOn, comments, rateReasonId"; //,Rate_Band_Costing";

                if (isGetLatest)
                {
                    cmdSelect.CommandText = "SELECT max(idGlobalRatePurchase) as idGlobalRatePurchase, rate, Ratedec.createdBy, Ratedec.createdOn, comments, rateReasonId  " + //,Rate_Band_Costing 
                                                       " FROM tblGlobalRatePurchase" +
                                                       " INNER JOIN[dbo].[tblRateBandDeclarationPurchase] Ratedec" +
                                                       " ON Ratedec.globalRatePurchaseId=idGlobalRatePurchase" +
                                                       " WHERE idGlobalRatePurchase = (select max(idGlobalRatePurchase) from tblGlobalRatePurchase" +
                                                       //" WHERE DAY(Ratedec.createdOn)= " + date.Day + " AND MONTH(Ratedec.createdOn)= " + date.Month + " AND YEAR(Ratedec.createdOn)= " + date.Year +
                                                       " ) GROUP BY rate, Ratedec.createdBy, Ratedec.createdOn, comments, rateReasonId"; //,Rate_Band_Costing";


                }
                else
                {
                      cmdSelect.CommandText = "SELECT max(idGlobalRatePurchase) as idGlobalRatePurchase, rate, Ratedec.createdBy, Ratedec.createdOn, comments, rateReasonId  " + //,Rate_Band_Costing 
                                                       " FROM tblGlobalRatePurchase" +
                                                       " INNER JOIN[dbo].[tblRateBandDeclarationPurchase] Ratedec" +
                                                       " ON Ratedec.globalRatePurchaseId=idGlobalRatePurchase" +
                                                       " WHERE idGlobalRatePurchase = (select max(idGlobalRatePurchase) from tblGlobalRatePurchase" +
                                                       " WHERE tblGlobalRatePurchase.createdOn <= @date" +
                                                       " ) GROUP BY rate, Ratedec.createdBy, Ratedec.createdOn, comments, rateReasonId"; //,Rate_Band_Costing";

                                                       cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = date;

                }


                //string g = "SELECT max(idGlobalRatePurchase) as idGlobalRatePurchase, rate, Ratedec.createdBy, Ratedec.createdOn, comments, rateReasonId,Rate_Band_Costing   FROM tblGlobalRatePurchase" +
                //    "  INNER JOIN[dbo].[tblRateBandDeclarationPurchase] Ratedec" +
                //    " ON Ratedec.globalRatePurchaseId=idGlobalRatePurchase" +
                //    "WHERE idGlobalRatePurchase = (select max(idGlobalRatePurchase) from tblGlobalRatePurchase" +
                //    " WHERE DAY(Ratedec.createdOn)= " + date.Day + " AND MONTH(Ratedec.createdOn)= " + date.Month + " AND YEAR(Ratedec.createdOn)= " + date.Year +
                //    " ) GROUP BY rate, Ratedec.createdBy, Ratedec.createdOn, comments, rateReasonId,Rate_Band_Costing";

                //cmdSelect.CommandText = " SELECT MAX(idGlobalRatePurchase) as idGlobalRatePurchase, rate  FROM tblGlobalRatePurchase" +
                //                        " GROUP BY rate";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader TblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGlobalRatePurchaseTO> list = ConvertDTToList(TblGlobalRatePurchaseTODT);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlDataReader != null) sqlDataReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblGlobalRatePurchaseTO> GetGlobalPurchaseRateList(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE CONVERT (DATE,createdOn,103)   BETWEEN @fromDate AND @toDate ORDER BY createdOn DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                SqlDataReader sqlDataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGlobalRatePurchaseTO> list = ConvertDTToList(sqlDataReader);
                sqlDataReader.Dispose();
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

        public List<TblPurchaseScheduleSummaryTO> GetAvgSaleDateWise(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT CAST(sql1.invoiceDate AS DATE) AS invDate, sum(itemwiseTaxableAmt)/sum(itemQty) AS AvgRate ,isConfirmed FROM( " +
                        " SELECT tempInvoice.*,tblProdGstCodeDtls.MaterialId,tempInvoiceItemDetails.otherTaxId,tempInvoiceItemDetails.invoiceQty as itemQty,(tempInvoiceItemDetails.basicTotal - ISNULL(tempInvoiceItemDetails.cdAmt, 0)) AS itemwiseTaxableAmt from tempInvoice " +
                        " LEFT JOIN tempInvoiceItemDetails ON tempInvoice.idInvoice = tempInvoiceItemDetails.invoiceId "+
                        " LEFT JOIN tblProdGstCodeDtls ON tempInvoiceItemDetails.prodGstCodeId = tblProdGstCodeDtls.idProdGstCode "+
                        " UNION ALL "+
                        " SELECT finalInvoice.*,tblProdGstCodeDtls.MaterialId, finalInvoiceItemDetails.otherTaxId,finalInvoiceItemDetails.invoiceQty as itemQty, (finalInvoiceItemDetails.basicTotal - ISNULL(finalInvoiceItemDetails.cdAmt, 0)) AS itemwiseTaxableAmt from finalInvoice " +
                        " LEFT JOIN finalInvoiceItemDetails ON finalInvoice.idInvoice = finalInvoiceItemDetails.invoiceId " +
                        " LEFT JOIN tblProdGstCodeDtls ON finalInvoiceItemDetails.prodGstCodeId = tblProdGstCodeDtls.idProdGstCode " +
                        " )sql1 " +
                        " WHERE sql1.MaterialId > 0 and ISNULL(sql1.otherTaxId,0) = 0 AND CONVERT (DATE,sql1.invoiceDate,103) BETWEEN @fromDate AND @toDate" +
                        " GROUP BY  CAST(sql1.invoiceDate AS DATE) ,sql1.isConfirmed";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                SqlDataReader TblPurchaseScheduleSummaryDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryList = new List<TblPurchaseScheduleSummaryTO>();
                if (TblPurchaseScheduleSummaryDT != null)
                {
                    while (TblPurchaseScheduleSummaryDT.Read())
                    {
                        TblPurchaseScheduleSummaryTO TblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                        if (TblPurchaseScheduleSummaryDT["isConfirmed"] != DBNull.Value)
                            TblPurchaseScheduleSummaryTONew.COrNcId = Convert.ToInt32(TblPurchaseScheduleSummaryDT["isConfirmed"]);
                        if (TblPurchaseScheduleSummaryDT["invDate"] != DBNull.Value)
                            TblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(TblPurchaseScheduleSummaryDT["invDate"]);
                        if (TblPurchaseScheduleSummaryDT["AvgRate"] != DBNull.Value)
                            TblPurchaseScheduleSummaryTONew.Rate = Convert.ToDouble(TblPurchaseScheduleSummaryDT["AvgRate"]);
                        tblPurchaseScheduleSummaryList.Add(TblPurchaseScheduleSummaryTONew);
                    }
                }
                TblPurchaseScheduleSummaryDT.Dispose();

                return tblPurchaseScheduleSummaryList;
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

        public List<TblGlobalRatePurchaseTO> GetGlobalRateList(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * from tblGlobalRate WHERE CONVERT (DATE,createdOn,103)   BETWEEN @fromDate AND @toDate ORDER BY createdOn DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                SqlDataReader TblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGlobalRatePurchaseTO> TblGlobalRatePurchaseTOList = new List<TblGlobalRatePurchaseTO>();
                if (TblGlobalRatePurchaseTODT != null)
                {
                    while (TblGlobalRatePurchaseTODT.Read())
                    {
                        TblGlobalRatePurchaseTO TblGlobalRatePurchaseTONew = new TblGlobalRatePurchaseTO();
                        if (TblGlobalRatePurchaseTODT["idGlobalRate"] != DBNull.Value)
                            TblGlobalRatePurchaseTONew.IdGlobalRatePurchase = Convert.ToInt32(TblGlobalRatePurchaseTODT["idGlobalRate"]);
                        if (TblGlobalRatePurchaseTODT["createdBy"] != DBNull.Value)
                            TblGlobalRatePurchaseTONew.CreatedBy = Convert.ToInt32(TblGlobalRatePurchaseTODT["createdBy"]);
                        if (TblGlobalRatePurchaseTODT["createdOn"] != DBNull.Value)
                            TblGlobalRatePurchaseTONew.CreatedOn = Convert.ToDateTime(TblGlobalRatePurchaseTODT["createdOn"]);
                        if (TblGlobalRatePurchaseTODT["rate"] != DBNull.Value)
                            TblGlobalRatePurchaseTONew.Rate = Convert.ToDouble(TblGlobalRatePurchaseTODT["rate"]);
                        if (TblGlobalRatePurchaseTODT["comments"] != DBNull.Value)
                            TblGlobalRatePurchaseTONew.Comments = Convert.ToString(TblGlobalRatePurchaseTODT["comments"]);
                        if (TblGlobalRatePurchaseTODT["rateReasonId"] != DBNull.Value)
                            TblGlobalRatePurchaseTONew.RateReasonId = Convert.ToInt32(TblGlobalRatePurchaseTODT["rateReasonId"]);
                        TblGlobalRatePurchaseTOList.Add(TblGlobalRatePurchaseTONew);
                    }
                }
                TblGlobalRatePurchaseTODT.Dispose();

                return TblGlobalRatePurchaseTOList;
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

        public List<TblGlobalRatePurchaseTO> GetPurchaseRateWithAvgDtls(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SelectLatestRateWithAvg() + " WHERE tblGlobalRatePurchase.createdOn " +
                                        " BETWEEN @fromDate AND @toDate GROUP BY CAST(tblGlobalRatePurchase.createdOn AS DATE)";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                SqlDataReader sqlDataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGlobalRatePurchaseTO> list = ConvertDTToList(sqlDataReader);
                sqlDataReader.Dispose();
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

        public Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader TblGlobalRatePurchaseTODT = null;
            try
            {
                cmdSelect.CommandText = "SELECT COUNT(*) AS todayCount FROM tblGlobalRate " + " WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                TblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                if (TblGlobalRatePurchaseTODT != null)
                {
                    while (TblGlobalRatePurchaseTODT.Read())
                    {
                        TblGlobalRatePurchaseTO TblGlobalRatePurchaseTONew = new TblGlobalRatePurchaseTO();
                        if (TblGlobalRatePurchaseTODT[0] != DBNull.Value)
                        {
                            if (Convert.ToInt32(TblGlobalRatePurchaseTODT[0]) > 0)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
            finally
            {
                if (TblGlobalRatePurchaseTODT != null)
                    TblGlobalRatePurchaseTODT.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblGlobalRatePurchaseTO> ConvertDTToList(SqlDataReader TblGlobalRatePurchaseTODT)
        {
            List<TblGlobalRatePurchaseTO> TblGlobalRatePurchaseTOList = new List<TblGlobalRatePurchaseTO>();
            if (TblGlobalRatePurchaseTODT != null)
            {
                while (TblGlobalRatePurchaseTODT.Read())
                {
                    TblGlobalRatePurchaseTO TblGlobalRatePurchaseTONew = new TblGlobalRatePurchaseTO();
                    if (TblGlobalRatePurchaseTODT["idGlobalRatePurchase"] != DBNull.Value)
                        TblGlobalRatePurchaseTONew.IdGlobalRatePurchase = Convert.ToInt32(TblGlobalRatePurchaseTODT["idGlobalRatePurchase"]);
                    if (TblGlobalRatePurchaseTODT["createdBy"] != DBNull.Value)
                        TblGlobalRatePurchaseTONew.CreatedBy = Convert.ToInt32(TblGlobalRatePurchaseTODT["createdBy"]);
                    if (TblGlobalRatePurchaseTODT["createdOn"] != DBNull.Value)
                        TblGlobalRatePurchaseTONew.CreatedOn = Convert.ToDateTime(TblGlobalRatePurchaseTODT["createdOn"]);
                    if (TblGlobalRatePurchaseTODT["rate"] != DBNull.Value)
                        TblGlobalRatePurchaseTONew.Rate = Convert.ToDouble(TblGlobalRatePurchaseTODT["rate"]);
                    if (TblGlobalRatePurchaseTODT["comments"] != DBNull.Value)
                        TblGlobalRatePurchaseTONew.Comments = Convert.ToString(TblGlobalRatePurchaseTODT["comments"]);
                    if (TblGlobalRatePurchaseTODT["rateReasonId"] != DBNull.Value)
                        TblGlobalRatePurchaseTONew.RateReasonId = Convert.ToInt32(TblGlobalRatePurchaseTODT["rateReasonId"]);
                    try
                    {
                        if (TblGlobalRatePurchaseTODT["Rate_Band_Costing"] != DBNull.Value)
                            TblGlobalRatePurchaseTONew.Ratebandcosting = Convert.ToInt32(TblGlobalRatePurchaseTODT["Rate_Band_Costing"]);
                    }
                    catch (Exception ex)
                    {

                    }
                    TblGlobalRatePurchaseTOList.Add(TblGlobalRatePurchaseTONew);
                }
            }
            return TblGlobalRatePurchaseTOList;
        }

        /// <summary>
        /// swati pisal
        /// </summary>
        /// <param name="idGlobalRatePurchase"></param>
        /// <returns></returns>
        public TblGlobalRatePurchaseTO SelectTblGlobalRatePurchase1(Int32 idGlobalRatePurchase)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGlobalRatePurchase = " + idGlobalRatePurchase + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader TblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblGlobalRatePurchaseTO> list = ConvertDTToList(TblGlobalRatePurchaseTODT);
                TblGlobalRatePurchaseTODT.Dispose();
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

        public TblGlobalRatePurchaseTO SelectTblGlobalRatePurchase(Int32 idGlobalRatePurchase, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGlobalRatePurchase = " + idGlobalRatePurchase + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;

                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblGlobalRatePurchaseTO> list = ConvertDTToList(sqlReader);
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
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion

        public int InsertTblGlobalRatePurchase(TblGlobalRatePurchaseTO TblGlobalRatePurchaseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(TblGlobalRatePurchaseTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblGlobalRatePurchaseTO TblGlobalRatePurchaseTO, SqlCommand cmdInsert)
        {
            //commented by swati for brand and purchase
            String sqlQuery = @" INSERT INTO [tblGlobalRatePurchase]( " +
                            "  [createdBy]" +
                            " ,[createdOn]" +
                            " ,[rate]" +
                            " ,[comments]" +
                            " ,[rateReasonId]" +
                            " )" +
                " VALUES (" +
                            "  @CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@Rate " +
                            " ,@Comments " +
                            " ,@rateReasonId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = TblGlobalRatePurchaseTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = TblGlobalRatePurchaseTO.CreatedOn;
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = TblGlobalRatePurchaseTO.Rate;
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(TblGlobalRatePurchaseTO.Comments);
            cmdInsert.Parameters.Add("@rateReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(TblGlobalRatePurchaseTO.RateReasonId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                TblGlobalRatePurchaseTO.IdGlobalRatePurchase = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

    }
}
