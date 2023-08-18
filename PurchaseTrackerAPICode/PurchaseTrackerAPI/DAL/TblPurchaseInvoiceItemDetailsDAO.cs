using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseInvoiceItemDetailsDAO : ITblPurchaseInvoiceItemDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseInvoiceItemDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblProdClassification.sapProdItemId,dimCdStructure.cdValue,* FROM [tblPurchaseInvoiceItemDetails] tblPurchaseInvoiceItemDetails " +
                " LEFT JOIN tblProdClassification tblProdClassification on tblProdClassification.idProdClass = tblPurchaseInvoiceItemDetails.prodClassId " +
            " LEFT JOIN dimCdStructure dimCdStructure on dimCdStructure.idCdStructure = tblPurchaseInvoiceItemDetails.cdStructureId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceItem", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemDetailsTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails(Int64 purchaseInvoiceId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceItemDetails.purchaseInvoiceId = " + purchaseInvoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceItem", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemDetailsTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceItemDetails.purchaseInvoiceId = " + purchaseInvoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemDetailsTO> list = ConvertDTToList(reader);
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

         public  List<TblPurchaseInvoiceItemDetailsTO> SelectPurchaseInvoiceItemDtlsForOtherTaxId(Int64 purchaseInvoiceId,Int32 otherTaxTypeId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceItemDetails.purchaseInvoiceId = " + purchaseInvoiceId + " and ISNULL(tblPurchaseInvoiceItemDetails.otherTaxId,0)=" + otherTaxTypeId ;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemDetailsTO> list = ConvertDTToList(reader);
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

        public  TblPurchaseInvoiceItemDetailsTO SelectTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchaseInvoiceItem = " + idPurchaseInvoiceItem +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceItem", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemDetailsTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceItem", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemDetailsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public  List<TblPurchaseInvoiceItemDetailsTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceItemDetailsTODT)
        {
            List<TblPurchaseInvoiceItemDetailsTO> tblPurchaseInvoiceItemDetailsTOList = new List<TblPurchaseInvoiceItemDetailsTO>();
            if (tblPurchaseInvoiceItemDetailsTODT != null)
            {
                while (tblPurchaseInvoiceItemDetailsTODT.Read())
                {
                    TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTONew = new TblPurchaseInvoiceItemDetailsTO();
                    if (tblPurchaseInvoiceItemDetailsTODT["purchaseScheduleDetailsId"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.PurchaseScheduleDetailsId = Convert.ToInt32(tblPurchaseInvoiceItemDetailsTODT["purchaseScheduleDetailsId"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.ProdClassId = Convert.ToInt32(tblPurchaseInvoiceItemDetailsTODT["prodClassId"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["productItemId"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.ProductItemId = Convert.ToInt32(tblPurchaseInvoiceItemDetailsTODT["productItemId"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["prodGstCodeId"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.ProdGstCodeId = Convert.ToInt32(tblPurchaseInvoiceItemDetailsTODT["prodGstCodeId"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["cdStructureId"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.CdStructureId = Convert.ToInt32(tblPurchaseInvoiceItemDetailsTODT["cdStructureId"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["otherTaxId"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.OtherTaxId = Convert.ToInt32(tblPurchaseInvoiceItemDetailsTODT["otherTaxId"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["invoiceQty"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.InvoiceQty = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["invoiceQty"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["rate"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.Rate = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["rate"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["basicTotal"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.BasicTotal = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["basicTotal"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["taxableAmt"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.TaxableAmt = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["taxableAmt"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["grandTotal"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.GrandTotal = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["grandTotal"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["cdStructure"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.CdStructure = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["cdStructure"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["cdAmt"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.CdAmt = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["cdAmt"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["taxPct"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.TaxPct = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["taxPct"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["taxAmt"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.TaxAmt = Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["taxAmt"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["idPurchaseInvoiceItem"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.IdPurchaseInvoiceItem = Convert.ToInt64(tblPurchaseInvoiceItemDetailsTODT["idPurchaseInvoiceItem"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["purchaseInvoiceId"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.PurchaseInvoiceId = Convert.ToInt64(tblPurchaseInvoiceItemDetailsTODT["purchaseInvoiceId"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["productItemDesc"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.ProductItemDesc = Convert.ToString(tblPurchaseInvoiceItemDetailsTODT["productItemDesc"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["gstinCodeNo"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.GstinCodeNo = Convert.ToString(tblPurchaseInvoiceItemDetailsTODT["gstinCodeNo"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["sapProdItemId"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.SapProdItemId = Convert.ToInt32(tblPurchaseInvoiceItemDetailsTODT["sapProdItemId"].ToString());
                    if (tblPurchaseInvoiceItemDetailsTODT["cdValue"] != DBNull.Value)
                        tblPurchaseInvoiceItemDetailsTONew.CdPerc= Convert.ToDouble(tblPurchaseInvoiceItemDetailsTODT["cdValue"].ToString());
                    tblPurchaseInvoiceItemDetailsTOList.Add(tblPurchaseInvoiceItemDetailsTONew);

                    
                }
            }
            return tblPurchaseInvoiceItemDetailsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseInvoiceItemDetailsTO, cmdInsert);
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

        public  int InsertTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseInvoiceItemDetailsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseInvoiceItemDetails]( " + 
            "  [purchaseScheduleDetailsId]" +
            " ,[prodClassId]" +
            " ,[productItemId]" +
            " ,[prodGstCodeId]" +
            " ,[cdStructureId]" +
            " ,[otherTaxId]" +
            " ,[invoiceQty]" +
            " ,[rate]" +
            " ,[basicTotal]" +
            " ,[taxableAmt]" +
            " ,[grandTotal]" +
            " ,[cdStructure]" +
            " ,[cdAmt]" +
            " ,[taxPct]" +
            " ,[taxAmt]" +
            //" ,[idPurchaseInvoiceItem]" +
            " ,[purchaseInvoiceId]" +
            " ,[productItemDesc]" +
            " ,[gstinCodeNo]" +
            " )" +
" VALUES (" +
            "  @PurchaseScheduleDetailsId " +
            " ,@ProdClassId " +
            " ,@ProductItemId " +
            " ,@ProdGstCodeId " +
            " ,@CdStructureId " +
            " ,@OtherTaxId " +
            " ,@InvoiceQty " +
            " ,@Rate " +
            " ,@BasicTotal " +
            " ,@TaxableAmt " +
            " ,@GrandTotal " +
            " ,@CdStructure " +
            " ,@CdAmt " +
            " ,@TaxPct " +
            " ,@TaxAmt " +
           // " ,@IdPurchaseInvoiceItem " +
            " ,@PurchaseInvoiceId " +
            " ,@ProductItemDesc " +
            " ,@GstinCodeNo " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@PurchaseScheduleDetailsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.PurchaseScheduleDetailsId);
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.ProdClassId);
            cmdInsert.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.ProductItemId);
            cmdInsert.Parameters.Add("@ProdGstCodeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.ProdGstCodeId);
            cmdInsert.Parameters.Add("@CdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.CdStructureId);
            cmdInsert.Parameters.Add("@OtherTaxId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.OtherTaxId);
            cmdInsert.Parameters.Add("@InvoiceQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.InvoiceQty);
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.Rate);
            cmdInsert.Parameters.Add("@BasicTotal", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.BasicTotal);
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.TaxableAmt);
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.GrandTotal);
            cmdInsert.Parameters.Add("@CdStructure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.CdStructure);
            cmdInsert.Parameters.Add("@CdAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.CdAmt);
            cmdInsert.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.TaxPct);
            cmdInsert.Parameters.Add("@TaxAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.TaxAmt);
           // cmdInsert.Parameters.Add("@IdPurchaseInvoiceItem", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem;
            cmdInsert.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.PurchaseInvoiceId;
            cmdInsert.Parameters.Add("@ProductItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.ProductItemDesc);
            cmdInsert.Parameters.Add("@GstinCodeNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.GstinCodeNo);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseInvoiceItemDetailsTO, cmdUpdate);
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

        public  int UpdateTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseInvoiceItemDetailsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseInvoiceItemDetails] SET " + 
            "  [purchaseScheduleDetailsId] = @PurchaseScheduleDetailsId" +
            " ,[prodClassId]= @ProdClassId" +
            " ,[productItemId]= @ProductItemId" +
            " ,[prodGstCodeId]= @ProdGstCodeId" +
            " ,[cdStructureId]= @CdStructureId" +
            " ,[otherTaxId]= @OtherTaxId" +
            " ,[invoiceQty]= @InvoiceQty" +
            " ,[rate]= @Rate" +
            " ,[basicTotal]= @BasicTotal" +
            " ,[taxableAmt]= @TaxableAmt" +
            " ,[grandTotal]= @GrandTotal" +
            " ,[cdStructure]= @CdStructure" +
            " ,[cdAmt]= @CdAmt" +
            " ,[taxPct]= @TaxPct" +
            " ,[taxAmt]= @TaxAmt" +
            //" ,[idPurchaseInvoiceItem]= @IdPurchaseInvoiceItem" +
            " ,[purchaseInvoiceId]= @PurchaseInvoiceId" +
            " ,[productItemDesc]= @ProductItemDesc" +
            " ,[gstinCodeNo] = @GstinCodeNo" +
            " WHERE [idPurchaseInvoiceItem]= @IdPurchaseInvoiceItem "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@PurchaseScheduleDetailsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.PurchaseScheduleDetailsId);
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.ProdClassId);
            cmdUpdate.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.ProductItemId);
            cmdUpdate.Parameters.Add("@ProdGstCodeId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceItemDetailsTO.ProdGstCodeId;
            cmdUpdate.Parameters.Add("@CdStructureId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceItemDetailsTO.CdStructureId;
            cmdUpdate.Parameters.Add("@OtherTaxId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.OtherTaxId);
            cmdUpdate.Parameters.Add("@InvoiceQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemDetailsTO.InvoiceQty;
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemDetailsTO.Rate;
            cmdUpdate.Parameters.Add("@BasicTotal", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemDetailsTO.BasicTotal;
            cmdUpdate.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemDetailsTO.TaxableAmt;
            cmdUpdate.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemDetailsTO.GrandTotal;
            cmdUpdate.Parameters.Add("@CdStructure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.CdStructure);
            cmdUpdate.Parameters.Add("@CdAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.CdAmt);
            cmdUpdate.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.TaxPct);
            cmdUpdate.Parameters.Add("@TaxAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.TaxAmt);
            cmdUpdate.Parameters.Add("@IdPurchaseInvoiceItem", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem;
            cmdUpdate.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.PurchaseInvoiceId;
            cmdUpdate.Parameters.Add("@ProductItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.ProductItemDesc);
            cmdUpdate.Parameters.Add("@GstinCodeNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceItemDetailsTO.GstinCodeNo);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseInvoiceItem, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseInvoiceItem, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public  int ExecuteDeletionCommand(Int64 idPurchaseInvoiceItem, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseInvoiceItemDetails] " +
            " WHERE idPurchaseInvoiceItem = " + idPurchaseInvoiceItem +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseInvoiceItem", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
