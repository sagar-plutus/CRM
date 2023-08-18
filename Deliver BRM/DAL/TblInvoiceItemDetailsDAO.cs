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
    public class TblInvoiceItemDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tempInvoiceItemDetails]" +

                                  // Vaibhav [20-Nov-2017] Added to select from finalInvoiceItemDetails

                                 " UNION ALL "+
                                 " SELECT * FROM [finalInvoiceItemDetails]";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetails()
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
                List<TblInvoiceItemDetailsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
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

        public static TblInvoiceItemDetailsTO SelectTblInvoiceItemDetails(Int32 idInvoiceItem)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery()+ ")sq1 WHERE idInvoiceItem = " + idInvoiceItem +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceItemDetailsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
            }
            catch(Exception ex)
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
        /// Ramdas.w @14-12-2017 : get other Tax by Invoice 
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        public static TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTOByInvoice(Int32 idInvoice)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE invoiceId = " + idInvoice + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceItemDetailsTO> list = ConvertDTToList(reader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetails(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 " +
                " LEFT JOIN tblOtherTaxes otherTaxes on otherTaxes.idOtherTax = sq1.otherTaxId" +
                " WHERE invoiceId=" + invoiceId + " order by otherTaxes.sequanceNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceItemDetailsTO> list = ConvertDTToList(reader);
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


        public static List<TblInvoiceItemDetailsTO> ConvertDTToList(SqlDataReader tblInvoiceItemDetailsTODT)
        {
            List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();
            if (tblInvoiceItemDetailsTODT != null)
            {
                while (tblInvoiceItemDetailsTODT.Read())
                {
                    TblInvoiceItemDetailsTO tblInvoiceItemDetailsTONew = new TblInvoiceItemDetailsTO();
                    if (tblInvoiceItemDetailsTODT["idInvoiceItem"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.IdInvoiceItem = Convert.ToInt32(tblInvoiceItemDetailsTODT["idInvoiceItem"].ToString());
                    if (tblInvoiceItemDetailsTODT["invoiceId"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.InvoiceId = Convert.ToInt32(tblInvoiceItemDetailsTODT["invoiceId"].ToString());
                    if (tblInvoiceItemDetailsTODT["loadingSlipExtId"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.LoadingSlipExtId = Convert.ToInt32(tblInvoiceItemDetailsTODT["loadingSlipExtId"].ToString());
                    if (tblInvoiceItemDetailsTODT["prodGstCodeId"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.ProdGstCodeId = Convert.ToInt32(tblInvoiceItemDetailsTODT["prodGstCodeId"].ToString());
                    if (tblInvoiceItemDetailsTODT["bundles"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.Bundles = Convert.ToDouble(tblInvoiceItemDetailsTODT["bundles"].ToString());
                    if (tblInvoiceItemDetailsTODT["invoiceQty"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.InvoiceQty = Convert.ToDouble(tblInvoiceItemDetailsTODT["invoiceQty"].ToString());
                    if (tblInvoiceItemDetailsTODT["rate"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.Rate = Convert.ToDouble(tblInvoiceItemDetailsTODT["rate"].ToString());
                    if (tblInvoiceItemDetailsTODT["basicTotal"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.BasicTotal = Convert.ToDouble(tblInvoiceItemDetailsTODT["basicTotal"].ToString());
                    if (tblInvoiceItemDetailsTODT["taxableAmt"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.TaxableAmt = Convert.ToDouble(tblInvoiceItemDetailsTODT["taxableAmt"].ToString());
                    if (tblInvoiceItemDetailsTODT["grandTotal"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.GrandTotal = Convert.ToDouble(tblInvoiceItemDetailsTODT["grandTotal"].ToString());
                    if (tblInvoiceItemDetailsTODT["prodItemDesc"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.ProdItemDesc = Convert.ToString(tblInvoiceItemDetailsTODT["prodItemDesc"].ToString());
                    if (tblInvoiceItemDetailsTODT["cdStructure"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.CdStructure = Convert.ToDouble(tblInvoiceItemDetailsTODT["cdStructure"]);
                    if (tblInvoiceItemDetailsTODT["cdAmt"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.CdAmt = Convert.ToDouble(tblInvoiceItemDetailsTODT["cdAmt"]);
                    if (tblInvoiceItemDetailsTODT["gstinCodeNo"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.GstinCodeNo = Convert.ToString(tblInvoiceItemDetailsTODT["gstinCodeNo"].ToString());

                    if (tblInvoiceItemDetailsTODT["otherTaxId"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.OtherTaxId = Convert.ToInt32(tblInvoiceItemDetailsTODT["otherTaxId"].ToString());
                    if (tblInvoiceItemDetailsTODT["taxPct"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.TaxPct = Convert.ToDouble(tblInvoiceItemDetailsTODT["taxPct"].ToString());
                    if (tblInvoiceItemDetailsTODT["taxAmt"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.TaxAmt = Convert.ToDouble(tblInvoiceItemDetailsTODT["taxAmt"].ToString());

                    if (tblInvoiceItemDetailsTODT["cdStructureId"] != DBNull.Value)
                        tblInvoiceItemDetailsTONew.CdStructureId = Convert.ToInt32(tblInvoiceItemDetailsTODT["cdStructureId"]);

                    tblInvoiceItemDetailsTOList.Add(tblInvoiceItemDetailsTONew);
                }
            }
            return tblInvoiceItemDetailsTOList;
        }

        public static List<TblInvoiceItemDetailsTO> SelectAllTempInvoiceItemDetails(Int32 invoiceId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                //String sqlQuery = " SELECT * FROM tempInvoiceItemDetails where invoiceId IN ( "+
                //                  " (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId IN( "+
                //                  " SELECT idLoadingSlip from tempLoadingSlip WHERE loadingId = "+loadingId+" AND isConfirmed = "+isConfirmed+")))";

                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE invoiceId ="+invoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceItemDetailsTO> list = ConvertDTToList(reader);
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

        #endregion

        #region Insertion
        public static int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceItemDetailsTO, cmdInsert);
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

        public static int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceItemDetailsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempInvoiceItemDetails]( " + 
                                "  [invoiceId]" +
                                " ,[loadingSlipExtId]" +
                                " ,[prodGstCodeId]" +
                                " ,[bundles]" +
                                " ,[invoiceQty]" +
                                " ,[rate]" +
                                " ,[basicTotal]" +
                                " ,[taxableAmt]" +
                                " ,[grandTotal]" +
                                " ,[prodItemDesc]" +
                                " ,[cdStructure]" +
                                " ,[cdAmt]" +
                                " ,[gstinCodeNo]" +
                                " ,[otherTaxId]" +
                                " ,[taxPct]" +
                                " ,[taxAmt]" +
                                " ,[cdStructureId]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceId " +
                                " ,@LoadingSlipExtId " +
                                " ,@ProdGstCodeId " +
                                " ,@Bundles " +
                                " ,@InvoiceQty " +
                                " ,@Rate " +
                                " ,@BasicTotal " +
                                " ,@TaxableAmt " +
                                " ,@GrandTotal " +
                                " ,@ProdItemDesc " +
                                " ,@cdStructure " +
                                " ,@cdAmt " +
                                " ,@gstinCodeNo " +
                                " ,@otherTaxId " +
                                " ,@taxPct " +
                                " ,@taxAmt " +
                                " ,@cdStructureId " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoiceItem", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.IdInvoiceItem;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.InvoiceId;
            cmdInsert.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.LoadingSlipExtId);
            cmdInsert.Parameters.Add("@ProdGstCodeId", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.ProdGstCodeId;
            cmdInsert.Parameters.Add("@Bundles", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.Bundles);
            cmdInsert.Parameters.Add("@InvoiceQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.InvoiceQty);
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.Rate);
            cmdInsert.Parameters.Add("@BasicTotal", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.BasicTotal);
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.TaxableAmt;
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.GrandTotal;
            cmdInsert.Parameters.Add("@ProdItemDesc", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.ProdItemDesc;
            cmdInsert.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdStructure);
            cmdInsert.Parameters.Add("@cdAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdAmt);
            cmdInsert.Parameters.Add("@gstinCodeNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.GstinCodeNo);
            cmdInsert.Parameters.Add("@otherTaxId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.OtherTaxId);
            cmdInsert.Parameters.Add("@taxPct", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.TaxPct);
            cmdInsert.Parameters.Add("@taxAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.TaxAmt);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdStructureId);

            if (cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceItemDetailsTO.IdInvoiceItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblInvoiceItemDetailsTO, cmdUpdate);
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

        public static int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblInvoiceItemDetailsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempInvoiceItemDetails] SET " +
                                " [invoiceId]= @InvoiceId" +
                                " ,[loadingSlipExtId]= @LoadingSlipExtId" +
                                " ,[prodGstCodeId]= @ProdGstCodeId" +
                                " ,[bundles]= @Bundles" +
                                " ,[invoiceQty]= @InvoiceQty" +
                                " ,[rate]= @Rate" +
                                " ,[basicTotal]= @BasicTotal" +
                                " ,[taxableAmt]= @TaxableAmt" +
                                " ,[grandTotal]= @GrandTotal" +
                                " ,[prodItemDesc] = @ProdItemDesc" +
                                " ,[cdStructure] = @cdStructure " +
                                " ,[cdAmt] = @cdAmt " +
                                " ,[gstinCodeNo] = @gstinCodeNo " +
                                " ,[otherTaxId] = @otherTaxId " +
                                " ,[taxPct] = @taxPct " +
                                " ,[taxAmt] = @taxAmt " +
                                " ,[cdStructureId] = @cdStructureId " +

                                " WHERE [idInvoiceItem] = @IdInvoiceItem"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdInvoiceItem", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.IdInvoiceItem;
            cmdUpdate.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.InvoiceId;
            cmdUpdate.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.LoadingSlipExtId);
            cmdUpdate.Parameters.Add("@ProdGstCodeId", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.ProdGstCodeId;
            cmdUpdate.Parameters.Add("@Bundles", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.Bundles);
            cmdUpdate.Parameters.Add("@InvoiceQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.InvoiceQty);
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.Rate);
            cmdUpdate.Parameters.Add("@BasicTotal", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.BasicTotal);
            cmdUpdate.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.TaxableAmt;
            cmdUpdate.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.GrandTotal;
            cmdUpdate.Parameters.Add("@ProdItemDesc", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.ProdItemDesc;
            cmdUpdate.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdStructure);
            cmdUpdate.Parameters.Add("@cdAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdAmt);
            cmdUpdate.Parameters.Add("@gstinCodeNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.GstinCodeNo);
            cmdUpdate.Parameters.Add("@otherTaxId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.OtherTaxId);
            cmdUpdate.Parameters.Add("@taxPct", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.TaxPct);
            cmdUpdate.Parameters.Add("@taxAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.TaxAmt);
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdStructureId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvoiceItem, cmdDelete);
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

        public static int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvoiceItem, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idInvoiceItem, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoiceItemDetails] " +
            " WHERE idInvoiceItem = " + idInvoiceItem +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvoiceItem", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.IdInvoiceItem;
            return cmdDelete.ExecuteNonQuery();
        }


        /// <summary>
        ///  Vijaymala [17-04-2018]:added to delete invoice item by invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int DeleteTblInvoiceItemDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommandByInvoiceId(invoiceId, cmdDelete);
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

        /// <summary>
        /// Vijaymala [17-04-2018]:added to delete invoice item by invoice id 
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="cmdDelete"></param>
        /// <returns></returns>
        public static int ExecuteDeletionCommandByInvoiceId(Int32 invoiceId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoiceItemDetails] " +
            " WHERE invoiceId = " + invoiceId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvoiceItem", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.IdInvoiceItem;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion

    }
}
